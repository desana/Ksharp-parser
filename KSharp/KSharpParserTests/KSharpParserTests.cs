using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using KSharpParser;
using NUnit.Framework;

namespace KSharpParserTests
{
    [TestFixture]
    public class KSharpParserTests
    {
        #region "Helper methods"

        private static KSharpGrammarParser CreateParserFromInput(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            KSharpGrammarLexer lexer = new KSharpGrammarLexer(inputStream);

            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            commonTokenStream.Fill();
            var tokens = commonTokenStream.GetTokens();

            KSharpGrammarParser parser = new KSharpGrammarParser(commonTokenStream);
            return parser;
        }

        #endregion

        [TestCase("1 < 2")]
        [TestCase("1 + 2")]

        [TestCase("1")]

        [TestCase("a = 1")]
        [TestCase("a = 1; a += 2")]

        [TestCase("apple + pie")]
        [TestCase("a = apple + pie")]

        [TestCase("\"apple\"")]
        [TestCase("\"apple\" + \"pie\"")]
        [TestCase("a = \"apple\"")]
        [TestCase("a = \"apple\" + \"pie\"")]
        public void ParseIsSuccessful(string input)
        {
            KSharpGrammarParser parser = CreateParserFromInput(input);

            IParseTree tree = parser.expression();

            Assert.AreEqual(parser.NumberOfSyntaxErrors, 0);
        }


        [TestCase("{1 < 2}")]
        [TestCase("if 1 < 2")]
        [TestCase("if 1 < 2 then 3")]
        [TestCase("if (1 < 2) then 1")]
        [TestCase("if 1 < 2 then {1}")]
        [TestCase("if (1 < 2) then {1} else 2")]
        [TestCase("if (1 < 2) else {3}")]

        [TestCase("if (1 < 2) then {1}")]
        [TestCase("if (1 < 2) then {1} else {2}")]
                      
        // toto kentico nevie ale nepoklada to za nespravny stav[TestCase("\"apple\" - \"e\"")]
        public void ParseIsNotSuccessful(string input)
        {
            KSharpGrammarParser parser = CreateParserFromInput(input);
            
            IParseTree tree = parser.expression();

            Assert.AreNotEqual(parser.NumberOfSyntaxErrors, 0);
        }
    }
}
