
using NUnit.Framework;

namespace KSharpParserTests
{

    [TestFixture]
    public class MethodTests : KSharpTestBase
    {
        [TestCase("5.ToString()", "5")]
        [TestCase("\"aaa\".ToUpper()", "AAA")]

        [TestCase("ToUpper(\"aaa\")", "AAA")]

        [TestCase("\"\" + ToDouble(\"2.45\", 0, \"en-us\")", "2.45")]
        [TestCase("\"\" + ToDouble(\"2,45\", 0, \"cs-cz\")", "2.45")]
        [TestCase("\"\" + ToDouble(\"2,45\")", "2.45")]

        public void Method_IsSuccessful_HasResult(string input, object expected)
        {
            var tree = GetParser(input).begin_expression();
            Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
        }
    }
}