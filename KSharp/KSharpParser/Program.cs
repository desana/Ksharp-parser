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
            AntlrInputStream inputStream = new AntlrInputStream("Insert expression here.");
            KSharpGrammarLexer lexer = new KSharpGrammarLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);

            KSharpGrammarParser parser = new KSharpGrammarParser(commonTokenStream);
            IParseTree tree = parser.begin_expression();

            Console.WriteLine(tree.ToStringTree(parser));
        }
    }
}

