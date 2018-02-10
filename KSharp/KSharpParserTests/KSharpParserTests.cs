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

        // basic structures tests

        [TestCase("Class.Member")]
        [TestCase("Class.Mehod()")]
        [TestCase("Variable")]

        [TestCase("1")]

        [TestCase("\"String\"")]

        // logical operation tests

        [TestCase("1 < 2")]

        [TestCase("true && true")]

        // bracket tests

        [TestCase("{1 < 2}")]

        // assignment test
        
        [TestCase("Variable = 1")]
        [TestCase("Variable = \"String\"")]

        // arithmetical tests

        [TestCase("1 + 2")]              
        [TestCase("SubVariable + AnotherSubVariable")]

        [TestCase("Variable = 1; Variable += 2")]
        [TestCase("Variable = SubVariable + AnotherSubVariable")]

        // string operation tests

        [TestCase("\"String\" + \"AnotherString\"")]
        [TestCase("\"String\" - \"AnotherString\"")]

        [TestCase("Variable = \"String\" + \"AnotherString\"")]        

        // if tests

        [TestCase("if (Condition) { return Value }")]        
        [TestCase("if (Condition) { return Value } else { return AnotherValue }")]

        [TestCase("if (!Condition) { return Value }")]

        public void ParseIsSuccessful(string input)
        {
            KSharpGrammarParser parser = CreateParserFromInput(input);

            IParseTree tree = parser.expression();

            Assert.AreEqual(parser.NumberOfSyntaxErrors, 0);
        }

        // if tests

        [TestCase("if Condition")]
        [TestCase("if Condition return Value")]
        [TestCase("if Condition { return Value }")]

        [TestCase("if (Condition) Value")]
        [TestCase("if (Condition) { Value } else AnotherValue")]
        [TestCase("if (Condition) else { return Value }")]
        [TestCase("if (Condition) then { return Value } else { return Value }")]

        // string operation tests

        [TestCase("\"FinalString\" = \"String\" + \"AnotherString\"")]        
        public void ParseIsNotSuccessful(string input)
        {
            KSharpGrammarParser parser = CreateParserFromInput(input);
            
            IParseTree tree = parser.expression();

            Assert.AreNotEqual(parser.NumberOfSyntaxErrors, 0);
        }
    }
}
