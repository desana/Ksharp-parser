using Antlr4.Runtime;

using KSharp;
using KSharpParser;

using NUnit.Framework;

using System.Globalization;
using System.Threading;

namespace KSharpParserTests
{
    public class KSharpTestBase
    {
        public TestEvaluator NodeEvaluator { get; private set; }


        public KSharpVisitor Visitor { get; private set; }


        [SetUp]
        public void SetUp()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            NodeEvaluator = new TestEvaluator();
            Visitor = new KSharpVisitor(NodeEvaluator);
        }
        

        public KSharpGrammarParser GetParser(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            KSharpGrammarLexer lexer = new KSharpGrammarLexer(inputStream);

            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);

            KSharpGrammarParser parser = new KSharpGrammarParser(commonTokenStream);
            return parser;
        }
    }
}