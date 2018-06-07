using Moq;
using NUnit.Framework;

namespace KSharpParserTests
{
    [TestFixture]
    public class ParameterTests : KSharpTestBase
    {        
        [TestCase("\"resolved\"|(culture)en-us", "culture", "en-us")]
        [TestCase("\"resolved\"|(default)N\\|A", "default", "N\\|A")]
        [TestCase("\"resolved\"|(encode)false", "encode", false)]
        [TestCase("\"resolved\"|(encode)true", "encode", true)]
        [TestCase("\"resolved\"|(encode)", "encode", null)]
        [TestCase("\"resolved\"|(timeout)123456", "timeout",123456)]
        public void Parameter_AreSaved(string input, string expectedKey, object expectedValue)
        {
            var tree = GetParser(input).begin_expression();

            Assert.IsNull(tree.exception);

            Visitor.Visit(tree);

            EvaluatorMock.Verify(m => m.SaveParameter(expectedKey, expectedValue), Times.Once());
        }
    }
}
