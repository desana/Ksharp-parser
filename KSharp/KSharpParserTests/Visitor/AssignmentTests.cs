using Antlr4.Runtime;
using NUnit.Framework;
using System;

namespace KSharpParserTests.Visitor
{
    class AssignmentTests 
    {
        [TestFixture]
        public class BasicAssignmentTests : KSharpTestBase
        {
            [TestCase("Variable = 1; Variable", 1)]
            [TestCase("Variable = \"String\"; Variable", "String")]
            [TestCase("Variable = false; Variable", false)]
            [TestCase("指 = 4; 指", 4)] // UNIDCODE characters in identifiers
            [TestCase("trala指lal = 4; trala指lal", 4)] // UNIDCODE characters in identifiers
            public void BasicAssignment_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
            }


            [TestCase("Variable = 1")]
            [TestCase("Variable = \"String\"")]
            [TestCase("Variable = false")]
            public void BasicAssignment_IsSuccessful_NoResult(string input)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.IsNull(Visitor.GetResultList(tree));
            }


            [TestCase("5 = 7")]
            [TestCase("\"FinalString\" = \"String\"")]
            public void BasicAssignment_NotSuccessful_ThrowsInputMismatch(string input)
            {
                var tree = GetParser(input).begin_expression();
                Assert.AreEqual(typeof(InputMismatchException), tree.exception.GetType());
            }
        }


        [TestFixture]
        public class AdvancedAssignmentTests : KSharpTestBase
        {
            [TestCase("Variable = 1; Variable++; Variable", 2)]
            [TestCase("Variable = 1; ++Variable; Variable", 2)]
            [TestCase("Variable = 1; Variable--; Variable", 0)]
            [TestCase("Variable = 1; --Variable; Variable", 0)]

            [TestCase("Variable = 1; Variable += 2; Variable", 3)]
            [TestCase("Variable = 1; Variable -= 2; Variable", -1)]
            [TestCase("Variable = 1; Variable *= 2; Variable", 2)]
            [TestCase("Variable = 1; Variable /= 2; Variable", 0.5)]
            public void AdvancedAssignment_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
            }


            [TestCase("Variable++;")]
            [TestCase("++Variable;")]
            [TestCase("Variable +=2;")]
            [TestCase("Variable -=2;")]
            [TestCase("Variable /=2;")]
            [TestCase("Variable *=2;")]
            [TestCase("Variable %=2;")]
            [TestCase("Variable &=2;")]
            [TestCase("Variable |=2;")]
            [TestCase("Variable ^=2;")]
            [TestCase("Variable <<= 2;")]
            [TestCase("Variable >>= 2;")]
            
            public void AdvancedAssignment_NotSuccessful_ThrowsInvalidOperationException(string input)
            {
                var tree = GetParser(input).begin_expression();

                Assert.Throws<InvalidOperationException>(() => Visitor.GetFirstResult(tree));
            }
        }
    }
}
