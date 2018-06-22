
using NUnit.Framework;

using System;

namespace KSharpParser.Tests
{
    [TestFixture]
    public class IfTests : KSharpTestBase
    {
        [TestCase("if (1 < 2) { 1; }", 1)]
        [TestCase("if (1 < 2) { return 1 }", 1)]
        [TestCase("if (1 > 2) { return 1 } else { return 2 }", 2)]

        [TestCase("if (!false) { return 1 }", 1)]

        [TestCase("If (1==2) {\"A\"} Else {\"B\"}", "B")]
        [TestCase("if (1==2) {\"A\"} Else {\"B\"}", "B")]
        [TestCase("If (1==2) {\"A\"} else {\"B\"}", "B")]
        public void If_IsSuccessful_HasResult(string input, object expected)
        {
            var tree = GetParser(input).begin_expression();

            Assert.IsNull(tree.exception);
            Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
        }


        [TestCase("cond = \"should go through\"; if (cond) { 1 } else { 0 }")]

        [TestCase("if (15) { 1 } else { 0 }")]
        [TestCase("if (-15) { 1 } else { 0 }")]
        [TestCase("if (0) { 1 } else { 0 }")]
        public void If_IsNotSuccessful_ThrowsInvalidCastException(string input)
        {
            Assert.Throws<InvalidCastException>(() => Visitor.GetFirstResult(GetParser(input).begin_expression()));
        }


        [TestCase("cond = null; if (cond) { 1 } else { 0 }")]
        [TestCase("if (null) { 1 } else { 0 }")]
        public void If_IsNotSuccessful_ThrowsNullReferenceException(string input)
        {
            Assert.Throws<NullReferenceException>(() => Visitor.GetFirstResult(GetParser(input).begin_expression()));
        }
    }

}
