using NUnit.Framework;

using System;

namespace KSharpParser.Tests
{
    class OperationTests
    {
        [TestFixture]
        public class LogicalTests : KSharpTestBase
        {
            [TestCase("1 < 2", true)]

            [TestCase("true || true", true)]
            [TestCase("false || false", false)]
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
            [TestCase(@"1==0 or ""a""==""a""", true)]
            [TestCase(@"1==0 or ""a""==""b""", false)]

            [TestCase(@"1==0 && 2==2", false)]
            [TestCase(@"1==0 and 2==2", false)]
            [TestCase(@"1==0 || 1==2", false)]
            [TestCase(@"null==null", true)]

            [TestCase("i = 4; ((i mod 2) == 0)", true)]
            public void Logical_IsSuccessful_HasResult(string input, bool expected)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
            }
        }


        [TestFixture]
        public class ArithmeticTests : KSharpTestBase
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
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
            }
        }


        [TestFixture]
        public class TernaryOperatorTests : KSharpTestBase
        {
            [TestCase("true ? \"yes\" : \"no\"", "yes")]
            [TestCase("false ? \"yes\" : \"no\"", "no")]
            public void TernaryOperator_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
            }


            [TestCase("0 ? 1 : 0")]
            [TestCase("1 ? 1 : 0")]
            public void TernaryOperator_NotSuccessful_ThrowsInvalidCast(string input)
            {
                Assert.Throws<InvalidCastException>(() => Visitor.GetFirstResult(GetParser(input).begin_expression()));
            }
        }
    }
}
