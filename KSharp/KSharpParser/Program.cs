using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace KSharpParser
{
    class Program
    {
        private static void Main(string[] args)
        {
            (new Program()).Run();
        }
        public void Run()
        {
            try
            {
                Console.WriteLine("START");
                RunParser();
                Console.Write("DONE. Hit RETURN to exit: ");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex);
                Console.Write("Hit RETURN to exit: ");
            }
            Console.ReadLine();
        }
        private void RunParser()
        {
            AntlrInputStream inputStream = new AntlrInputStream("// This is a one-line comment. Initiated by two forward slashes, spans across one full line.");
            KSharpGrammarLexer lexer = new KSharpGrammarLexer(inputStream);
           
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            commonTokenStream.Fill();
            var tokens = commonTokenStream.GetTokens();
            KSharpGrammarParser parser = new KSharpGrammarParser(commonTokenStream);
            IParseTree tree = parser.begin_expression();
            IParseTreeVisitor<object> visitor = new KSharpGrammarBaseVisitor<object>();
            var result = visitor.Visit(tree);
            Console.WriteLine(tree.ToStringTree(parser));
        }
    }
}

