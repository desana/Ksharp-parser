using System;
using Antlr4.Runtime;
using Antlr.Grammars;
using Antlr4.Runtime.Tree;


namespace Antlr
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
            AntlrInputStream inputStream = new AntlrInputStream("132 * 111");
            KLexer lexer = new KLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            KParser parser = new KParser(commonTokenStream);
            IParseTree tree = parser.compileUnit();
            IParseTreeVisitor<object> visitor = new KVisitor();
            var result = visitor.Visit(tree);
            Console.WriteLine(result);
            
        }
    }
}
