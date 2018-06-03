using NUnit.Framework;

namespace KSharpParserTests.Visitor
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
                Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
            }


            [TestCase("\"first\"; return \"second\"", new object[] { "first", "second" })]

            [TestCase("print(\"should be on output\"); return \"output\"", new object[] { "should be on outputoutput" })]
            [TestCase("return \"first\"; return \"second\"", new object[] { "first" })]


            [TestCase("x = 0; for (i = 0; i < 10; i++) { x += i; if (i == 5) { return x; } }", new object[] { 15 })]
            [TestCase("x = 0; for (i = 0; i < 10; i++) { print(i); if (i == 5) { return; } }", new object[] { "012345" })]
            [TestCase("x = 0; for (i = 0; i < 10; i++) { i }", new object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })]
            public void Return_IsSuccessful_HasListOfResults(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();
                Assert.AreEqual(expected, Visitor.GetResultList(tree));
            }


            [TestCase("x = 0; for (i = 0; i < 10; i++) { if (i == 5) { return; } }")]
            public void Return_IsSuccessful_NoResult(string input)
            {
                var tree = GetParser(input).begin_expression();
                Assert.IsNull(Visitor.GetResultList(tree));
            }
        }
    }
}
