using NUnit.Framework;

namespace KSharpParserTests
{
    class MiscellaneousTests
    {                
        [TestFixture]
        public class BracesTests : KSharpTestBase
        {
            [TestCase("{1 < 2}", true)]
            [TestCase("if (1 < 2) {{ return 5 }}", 5)]
            public void Braces_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();
                Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
            }
        }


        [TestFixture]
        public class CommentTests : KSharpTestBase
        {
            [TestCase("x = 5; y = 3; /* This is an inline comment nested in the middle of an expression. */ x+= 2; x + y",10)]           
            public void Comment_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();
                Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
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
                var tree = GetParser(input).begin_expression();
                Assert.IsNull(Visitor.GetResultList(tree));
            }
        }        
    }
}
