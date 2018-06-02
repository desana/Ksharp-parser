using Antlr4.Runtime;

using KSharp;
using KSharpParser;

using Moq;

using NUnit.Framework;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

using static KSharpParser.KSharpGrammarParser;

namespace KSharpParserTests
{
    class KSharpVisitorTests
    {
        #region "Helper methods"

        public static KSharpGrammarParser GenerateParser(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            KSharpGrammarLexer lexer = new KSharpGrammarLexer(inputStream);

            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);

            KSharpGrammarParser parser = new KSharpGrammarParser(commonTokenStream);
            return parser;
        }


        public static KSharpVisitor GetVisitor()
        {
            Mock<INodeEvaluator> evaluatorMock = new Mock<INodeEvaluator>();

            // methods
            evaluatorMock.Setup(m => m.InvokeMethodForObject("\"aaa\"", "ToUpper", new object[] { })).Returns("AAA");
            evaluatorMock.Setup(m => m.InvokeMethod("ToUpper", new object[]{"aaa"})).Returns("AAA");
            evaluatorMock.Setup(m => m.InvokeMethodForObject(5, "ToString", new object[] {})).Returns("5");

            evaluatorMock.Setup(m => m.InvokeMethod("ToDouble", new object[] { "2.45", 0, "en-us" })).Returns(2.45);
            evaluatorMock.Setup(m => m.InvokeMethod("ToDouble", new object[] { "2.45", 0, "cs-cz" })).Returns(2.45);
            evaluatorMock.Setup(m => m.InvokeMethod("ToDouble", new object[] { "2.45"})).Returns(2.45);

            // comparers
            IDictionary<Type, IComparer> comparers = new Dictionary<Type, IComparer>();
            comparers.Add(typeof(string), StringComparer.Create(CultureInfo.InvariantCulture, false));
            evaluatorMock.Setup(m => m.KnownComparers).Returns(comparers);

            return new KSharpVisitor(evaluatorMock.Object);
        }


        public static object GetFirstItem(Begin_expressionContext tree)
        {
            var list = GetVisitor().Visit(tree) as IList;
            return list[0];
        }

        
        public static object GetList(Begin_expressionContext tree)
        {
            return GetVisitor().Visit(tree);             
        }

        #endregion


        #region "Data types tests"

        [TestFixture]
        public class BasicStructuresTests
        {
            [TestCase("true", true)]
            [TestCase("false", false)]

            [TestCase("1", 1)]
            [TestCase("98746311", 98746311)]

            [TestCase("-1", -1)]
            [TestCase("-98746311", -98746311)]

            [TestCase("3.465e-5", 3.465e-5)]
            [TestCase("3.465e+5", 346500000)]
            [TestCase("3.465", 3.456)]

            [TestCase("30%", 0.30d)]
            [TestCase("30.4%",0.304d)]

            [TestCase("\"String\"", "String")]
            public void BasicStructures_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GenerateParser(input).begin_expression();

                Assert.AreEqual(expected, GetFirstItem(tree));
            }


            [TestCase(";")]
            [TestCase(";;;")]           
            [TestCase("2identifier")]  // invalid identifier, it is ignored when found
            public void BasicStructures_IsSuccessful_NoResult(string input)
            {
                var tree = GenerateParser(input).begin_expression();

                Assert.IsNull(GetList(tree));
            }
        }


        [TestFixture]
        public class StringTests
        {
            [TestCase("\"String\" + \"AnotherString\"", "StringAnotherString")]   
            [TestCase("Variable = \"String\" + \"AnotherString\"; Variable", "StringAnotherString")]

            [TestCase("\"\" + \"ahoj\"", "ahoj")]
            [TestCase("\"\" + 23.05", "23.05")]
            [TestCase("\"\" + False", "False")]

            [TestCase("@\"This string displays as is. No newlines\n, tabs\t or backslash-escapes\\.\"", "@\"This string displays as is. No newlines\n, tabs\t or backslash-escapes\\.")]
            public void String_IsSuccessful_HasResult(string input, string expected)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.AreEqual(expected, GetFirstItem(tree));
            }


            [TestCase("")]
            [TestCase(" ")]
            public void String_IsSuccessful_HasResult(string input)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.IsNull(GetList(tree));
            }


            [TestCase("\"String\" \"AnotherString\"")]
            [TestCase("\"\"")]
            [TestCase("\"\\\"\"")]
            [TestCase("@\"\"")]
            public void String_IsSuccessful_ThrowsInputMismatch(string input)
            {
                Assert.Throws<InputMismatchException>(() => GetFirstItem(GenerateParser(input).begin_expression()));
            }


            [TestCase("\"String\" - \"AnotherString\"")]
            public void String_IsSuccessful_ThrowsInvalidOperation(string input)
            {
                Assert.Throws<InvalidOperationException>(() => GetFirstItem(GenerateParser(input).begin_expression()));
            }
        }
        

        [TestFixture]
        public class GuidTests
        {
            [TestCase("0362d604-e293-496e-a73f-abdf522ce31d")]

            public void Guid_IsSuccessful_HasResult(string input)
            {
                var tree = GenerateParser(input).begin_expression();

                Assert.AreEqual(new Guid("0362d604-e293-496e-a73f-abdf522ce31d"), GetFirstItem(tree));
            }
        }
        

        [TestFixture]
        public class DateTimeTests
        {
            [TestCase("12.12.2012")]
            [TestCase("12/12/2012")]
            public void DateTime_IsSuccessful_HasResult(string input)
            {
                var tree = GenerateParser(input).begin_expression();

                Assert.AreEqual(new DateTime(2012,12,12), GetFirstItem(tree));
            }
        }
        #endregion


        #region "Assignment tests"

        [TestFixture]
        public class BasicAssignmentTests
        {
            [TestCase("Variable = 1; Variable", 1)]
            [TestCase("Variable = \"String\"; Variable", "String")]
            [TestCase("Variable = false; Variable", false)]
            [TestCase("指 = 4; 指", 4)] // UNIDCODE characters in identifiers
            [TestCase("trala指lal = 4; trala指lal", 4)] // UNIDCODE characters in identifiers
            public void BasicAssignment_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.AreEqual(expected, GetFirstItem(tree));
            }


            [TestCase("Variable = 1")]
            [TestCase("Variable = \"String\"")]
            [TestCase("Variable = false")]

            [TestCase("5 = 7 + 2")]
            [TestCase("\"FinalString\" = \"String\" + \"AnotherString\"")]
            public void BasicAssignment_IsSuccessful_NoResult(string input)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.IsNull(GetList(tree));
            }
        }


        [TestFixture]
        public class AdvancedAssignmentTests
        {
            [TestCase("Variable = 1; Variable++; Variable", 2)]
            [TestCase("Variable = 1; ++Variable; Variable", 2)]
            [TestCase("Variable = 1; Variable--; Variable", 0)]
            [TestCase("Variable = 1; --Variable; Variable", 0)]

            [TestCase("Variable = 1; Variable += 2; Variable", 3)]
            [TestCase("Variable = 1; Variable -= 2; Variable", -1)]
            [TestCase("Variable = 1; Variable *= 2; Variable", 2)]
            [TestCase("Variable = 1; Variable /= 2; Variable", 0.5)]
            public void BasicAssignment_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.AreEqual(expected, GetFirstItem(tree));
            }
        }

        #endregion


        #region "Oparations tests"

        [TestFixture]
        public class LogicalTests
        {
            [TestCase("1 < 2")]

            [TestCase("true || true", true)]
            [TestCase("false || false")]
            [TestCase("false || true", true)]
            [TestCase("true || false", true)]
            [TestCase("true && true", true)]
            [TestCase("false && false", false)]
            [TestCase("false && true", false)]
            [TestCase("true && false", false)]

            [TestCase("true or false", true)]
            [TestCase("true and false", false)]

            [TestCase("null == 0 || null == null", true)]
            [TestCase("0 == 0 || 0 == null", true)]
            [TestCase("1 == 0 || 1 == null", false)]

            [TestCase(@"1==1 && ""a""==""a""", true)]
            [TestCase(@"1==1 and ""a""==""a""", true)]
            [TestCase(@"1==1 || ""a""==""a""", true)]
            [TestCase(@"1==0 or ""a""==""a""", false)]
            [TestCase(@"1==0 or ""a""==""b""", false)]

            [TestCase(@"1==0 && 2==2", false)]
            [TestCase(@"1==0 and 2==2", false)]
            [TestCase(@"1==0 || 1==2", false)]
            public void Logical_IsSuccessful_HasResult(string input, bool expected)
            {
                var tree = GenerateParser(input).begin_expression();

                Assert.AreEqual(expected, GetFirstItem(tree));
            }
        }


        [TestFixture]
        public class ArithmeticTests
        {
            [TestCase("1 + 2", 3)]
            [TestCase("5 mod 2", 1)]

            [TestCase("2.3e-2 + 5 * 2", 10.023)]
            [TestCase("2.3e-2 + 5 * 2 == 2 * 5 + 2.3e-2", true)]
            [TestCase("9 + 2 - 2 - 2", 7)]
            [TestCase("9 + 2 + 2 - 2", 11)]
            [TestCase("9 + 2 + 2 + 2", 15)]
            [TestCase("9 - 2 + 2 + 2", 11)]
            [TestCase("9 - 2 - 2 + 2", 7)]
            [TestCase("9 - 2 - 2 - 2", 3)]
            [TestCase("9 + 2 * 2 - 2", 11)]
            [TestCase("9 - 2 + 2 * 2", 11)]
            [TestCase("9 + ((-2) + 2 * 2)", 11)]
            [TestCase("9 + (-2 + 2 * 2)", 11)]
            [TestCase("-5 * 5 / 10 + 1", -1.5)]
            [TestCase("-5 * -5 / 10 + 1", 3.5)]
            [TestCase("5 - 2 * 4 - 3 + 2", -4)]
            [TestCase("9 + (2 + (-2 * 2))", 7)]
            [TestCase("9 - 2 + 2 * 3", 13)]
            [TestCase("9 - 2 * 2 + 2", 7)]
            [TestCase("1 << 8 >> 8 << 2", 4)]
            [TestCase("1 << (8 >> (8 << 2))", 256)]
            [TestCase("SubVariable = 6; AnotherSubVariable = 3; SubVariable + AnotherSubVariable", 9)]
            [TestCase("SubVariable = 6; AnotherSubVariable = 3; Variable = SubVariable + AnotherSubVariable; Variable", 9)]
            [TestCase("Variable = 1; Variable += 2; Variable", 3)]
            public void Arithmetic_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.AreEqual(expected, GetFirstItem(tree));
            }
        }


        [TestFixture]
        public class TernaryOperatorTests
        {
            [TestCase("true ? \"yes\" : \"no\"", "yes")]
            [TestCase("false ? \"yes\" : \"no\"", "no")]
            public void Braces_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.AreEqual(expected, GetFirstItem(tree));
            }


            [TestCase("0 ? 1 : 0")]
            [TestCase("1 ? 1 : 0")]
            public void Braces_NotSuccessful_ThrowsInvalidCast(string input)
            {
                Assert.Throws<InvalidCastException>(() => GetFirstItem(GenerateParser(input).begin_expression()));
            }
        }

        #endregion


        #region "Methods tests"

        [TestFixture]
        public class MethodTests
        {
            [TestCase("5.ToString()", "5")]
            [TestCase("\"aaa\".ToUpper()", "AAA")]

            [TestCase("ToUpper(\"aaa\")", "AAA")]

            [TestCase("\"\" + ToDouble(\"2.45\", 0, \"en-us\")", "2.45")]
            [TestCase("\"\" + ToDouble(\"2,45\", 0, \"cs-cz\")", "2,45")]
            [TestCase("\"\" + ToDouble(\"2,45\")", "2.45")]

            public void Method_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.AreEqual(expected, GetFirstItem(tree));
            }

        }


        [TestFixture]
        public class LambdaTests
        {
            [TestCase("lambdaSucc = (x => x + 1); lambdaSucc(3)", 4)]
            [TestCase("lambdaMultiply = ((x, y) => x * y); lambdaMultiply(2,3)", 6)]
            [TestCase("lambda = x => x * 2; lambda(lambda(5))", 20)]
            [TestCase("lambda = (x, y, z) => x * y + z; lambda(5, 4, 3)", 23)]
            [TestCase("fun = (x => {if (x>1){\"Hello\"}}); fun(5);", "Hello")]
            public void Lambda_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.AreEqual(expected, GetFirstItem(tree));
            }
        }


        #endregion


        #region "If tests"

        [TestFixture]
        public class IfTests
        {
            [TestCase("if (1 < 2) { 1; }", 1)]
            [TestCase("if (1 < 2) { return 1 }", 1)]
            [TestCase("if (1 > 2) { return 1 } else { return 2 }",2)]

            [TestCase("if (!false) { return 1 }", 1)]

            [TestCase("If (1==2) {\"A\"} Else {\"B\"}", "B")]
            [TestCase("if (1==2) {\"A\"} Else {\"B\"}", "B")]           
            [TestCase("If (1==2) {\"A\"} else {\"B\"}", "B")]
            public void If_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.AreEqual(expected, GetFirstItem(tree));
            }


            [TestCase("cond = \"should go through\"; if (cond) { 1 } else { 0 }")]

            [TestCase("if (15) { 1 } else { 0 }")]
            [TestCase("if (-15) { 1 } else { 0 }")]
            [TestCase("if (0) { 1 } else { 0 }")]
            public void If_IsNotSuccessful_ThrowsInvalidCastException(string input)
            {
                Assert.Throws<InvalidCastException>(() => GetFirstItem(GenerateParser(input).begin_expression()));                     
            }


            [TestCase("cond = null; if (cond) { 1 } else { 0 }")]
            [TestCase("if (null) { 1 } else { 0 }")]
            public void If_IsNotSuccessful_ThrowsNullReferenceException(string input)
            {
                Assert.Throws<NullReferenceException>(() => GetFirstItem(GenerateParser(input).begin_expression()));
            }
        }
        
        #endregion


        #region "Output tests"

        [TestFixture]
        public class ReturnTests
        {
            [TestCase("return \"output\"", "output")]

            [TestCase("return 2 * 3 - (2 + 1)", 3)]
            [TestCase("return 2 + 3 * (1 + 2)", 11)]
            public void Return_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.AreEqual(expected, GetFirstItem(tree));
            }


            [TestCase("\"first\"; return \"second\"", new object[] { "first", "second" })]

            [TestCase("print(\"should be on output\"); return \"output\"", new object[]{"should be on output", "output"})]
            [TestCase("return \"first\"; return \"second\"", new object[] { "first", "second" })]
            

            [TestCase("x = 0; for (i = 0; i < 10; i++) { x += i; if (i == 5) { return x; } }", new object[] { 15 })]
            [TestCase("x = 0; for (i = 0; i < 10; i++) { print(i); if (i == 5) { return; } }", new object[] {"012345"})]
            [TestCase("x = 0; for (i = 0; i < 10; i++) { i }", new object[] { 0,1,2,3,4,5,6,7,8,9 })]
            public void Return_IsSuccessful_HasListOfResults(string input, object expected)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.AreEqual(expected, GetList(tree));
            }


            [TestCase("x = 0; for (i = 0; i < 10; i++) { if (i == 5) { return; } }")]
            public void Return_IsSuccessful_NoResult(string input)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.IsNull(GetList(tree));
            }
        }

        #endregion


        #region "Loops tests"

        [TestFixture]
        public class ForTests
        {
            [TestCase("for (i=0; i<=5 ; i++) {i}", new object[] { 0,1,2,3,4,5 })]
            [TestCase("for (i=0; i<=5 ; i++) {i;i;}", new object[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 })]
            [TestCase("i = 3; for (i; i<=5 ; i++) {i}", new object[] { 3,4,5 })]
            [TestCase("z = 0; for (i = 0; i < 5; i++) { z += 1 }; z", new object[] { 5 })]
            [TestCase("for (i = 1; i <= 3; i++) { print(i); }", new object[] { "123" })]
            [TestCase("for (i = 1; i <= 3; i++) { return i; }", new object[] { 1 })]
            [TestCase("for (i = 1; i <= 3; i++) { i; }", new object[] { 1,2,3 })]
            [TestCase("4; for (i = 1; i <= 3; i++) { i; }", new object[] { 4,1,2,3 })]
            [TestCase("4; for (i = 1; i <= 3; i++) { i; } 5;", new object[] { 4,1,2,3,5 })]
            public void For_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.AreEqual(expected, GetList(tree));
            }


            [TestCase("z = 0; for (i = 0; i < 5; i++) { z += 1 } ")]
            public void For_IsSuccessful_NoResult(string input)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.IsNull(GetList(tree));
            }
        }


        [TestFixture]
        public class WhileTests
        {
            [TestCase("z = 1; while (z<10) {++z}; z", new object[] { 10 })]
            [TestCase("i = 1; while (i < 4) {print(i++)}; \"string\"", new object[] { 1,2,3,4,"string" })]
            [TestCase("i = 1; while (i < 4) {print(i++)}; return \"result\"", new object[] { 1, 2, 3, 4, "result" })]
            [TestCase("x = 0; while (x < 5) { x++; }; x", new object[] { 5 })]
            public void While_IsSuccessful(string input, object expected)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.AreEqual(expected, GetList(tree));
            }


            [TestCase("z = 0; while (z < 10) {if (z > 4) {break}; ++z}")]           
            [TestCase("z = 1; while (z<10) {++z} ")]
            public void While_IsSuccessful_NoResult(string input)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.IsNull(GetList(tree));
            }


            [TestCase("cond = \"wrong type\"; while (cond) { x++ }")]
            public void While_NotSuccessful_ThrowsInvalidCast(string input)
            {
                Assert.Throws<InvalidCastException>(() => GetFirstItem(GenerateParser(input).begin_expression()));
            }


            [TestCase("x = 5; cond = null; while (cond) { x = 10; }; x")]
            [TestCase("while (true) { x += 1; }")]
            [TestCase("while (true) print(x.y)")]
            [TestCase("while (if (z<10) {a++}) {++z} ")]
            [TestCase("while (z<10) {++z} ")]
            [TestCase("y = while (z<10) {++z} ")]
            public void While_NotSuccessful_ThrowsNullReference(string input)
            {
                Assert.Throws<NullReferenceException>(() => GetFirstItem(GenerateParser(input).begin_expression()));
            }            
        }

        #endregion

                
        [TestFixture]
        public class BracesTests
        {
            [TestCase("{1 < 2}", true)]
            [TestCase("if (1 < 2) {{ return 5 }}", 5)]
            public void Braces_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.AreEqual(expected, GetFirstItem(tree));
            }
        }


        [TestFixture]
        public class CommentTests
        {
            [TestCase("x = 5; y = 3; /* This is an inline comment nested in the middle of an expression. */ x+= 2; x + y",10)]           
            public void Comment_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.AreEqual(expected, GetFirstItem(tree));
            }


            [TestCase("// This is a one-line comment. Initiated by two forward slashes, spans across one full line.")]
            [TestCase(@"/* This is a multi - line comment.
                Opened by a forward slash - asterisk sequence and closed with the asterisk - forward slash sequence.
                Can span across any number of lines.
                */")]
            [TestCase("// c\nx=\"5\"")]
            [TestCase("// c\r\nx=\"5\"")]
            public void Comment_IsSuccessful_NoResult(string input)
            {
                var tree = GenerateParser(input).begin_expression();
                Assert.IsNull(GetList(tree));
            }
        }        
    }
}
