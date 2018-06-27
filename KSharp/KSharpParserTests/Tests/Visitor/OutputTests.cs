using NUnit.Framework;

namespace KSharpParser.Tests
{
    class OutputTests
    {
        [TestFixture]
        public class ReturnTests : KSharpTestBase
        {
            [TestCase("return \"output\"", "output")]

            [TestCase("return 2 * 3 - (2 + 1)", 3)]
            [TestCase("return 2 + 3 * (1 + 2)", 11)]
            public void Return_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
            }


            [TestCase("return \"first\"; return \"second\"", new object[] { "first" })]
            [TestCase("x = 0; for (i = 0; i < 10; i++) { x += i; if (i == 5) { return x; } }", new object[] { 15 })]
            public void Return_IsSuccessful_HasListOfResults(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetResultList(tree));
            }


            [TestCase("x = 0; for (i = 0; i < 10; i++) { if (i == 5) { return; } }")]
            public void Return_IsSuccessful_NoResult(string input)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.IsNull(Visitor.GetResultList(tree));
            }
        }

        [TestFixture]
        public class PrintTests : KSharpTestBase
        {

            [TestCase("print(\"Console\");", new object[] { "Console" })]
            [TestCase("print(\"Console priority\"); \"overriden\"", new object[] { "Console priorityoverriden" })]
            [TestCase("print(\"Console\"); 2 + 3; return", new object[] { "Console5" })]
            [TestCase("print(\"Console priority\"); println(\" works\")", new object[] { "Console priority works" })]
            [TestCase("println(\"first\"); println(\"second\");", new object[] { "first", "second" })]
            [TestCase("print(\"Console priority\"); println(\" works\"); print(\" good enough!\")", new object[] { "Console priority works", " good enough!" })]
            public void Print_IsSuccessful_HasListOfResults(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetResultList(tree));
            }
        }
        [TestFixture]
        public class StatementTests : KSharpTestBase
        {
            [TestCase("\"X\"", new object[] { "X" })]
            [TestCase("\"X\"; \"Y\";", new object[] { "X", "Y" })]
            [TestCase("\"X\"; { \"Y\"; }", new object[] { "X", "Y" })]
            [TestCase("{ \"X\"; \"Y\"; }", new object[] { "X", "Y" })]
            [TestCase("x = 0; for (i = 0; i < 10; i++) { i }", new object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })]
            public void Statement_IsSuccessful_HasListOfResults(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetResultList(tree));
            }
        }


        [TestFixture]
        public class MixedOutputTests : KSharpTestBase
        {
            [TestCase("\"X\"; return \"Y\"; \"Z\"", new object[] { "X", "Y" })]
            [TestCase("\"X\"; print(\"Y\"); \"Z\"; print(\"W\")", new object[] { "X", "YZ", "W"})]
            [TestCase("\"first\"; return \"second\"", new object[] { "first", "second" })]

            [TestCase("x = 0; for (i = 0; i < 10; i++) { print(i); if (i == 5) { return; } }", new object[] { "012345" })]
            [TestCase("print(\"should be on output\"); return \"output\"", new object[] { "should be on outputoutput" })]
            [TestCase("\"red\"; \"yellow\"; return \"green\"; \"blue\"", new object[] { "red", "yellow", "green"})]
            public void MixedOutput_IsSuccessful_HasListOfResults(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetResultList(tree));
            }
        }
    }
}
