
using NUnit.Framework;

namespace KSharpParserTests
{
    [TestFixture]
    public class LambdaTests : KSharpTestBase
    {
        [TestCase("lambdaSucc = (x => x + 1); lambdaSucc(3)", 4)]
        [TestCase("lambdaMultiply = ((x, y) => x * y); lambdaMultiply(2,3)", 6)]
        [TestCase("lambda = x => x * 2; lambda(lambda(5))", 20)]
        [TestCase("lambda = (x, y, z) => x * y + z; lambda(5, 4, 3)", 23)]
        [TestCase("fun = (x => {if (x>1){\"Hello\"}}); fun(5);", "Hello")]
        public void Lambda_IsSuccessful_HasResult(string input, object expected)
        {
            var tree = GetParser(input).begin_expression();

            Assert.IsNull(tree.exception);
            Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
        }
    }
}
