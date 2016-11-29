using System;
using Antlr4.Runtime;
using Antlr.Grammars;

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
            AntlrInputStream inputStream = new AntlrInputStream("{% expr | (m) arg %}");
            BaseLexer baseLexer = new BaseLexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(baseLexer);
            BaseParser baseParser = new BaseParser(commonTokenStream);
            BaseParser.InputContext rContext = baseParser.input();
            IBaseVisitor<string> visitor = new BaseBaseVisitor<string>();
            var result = visitor.VisitInput(rContext);
            Console.WriteLine(result);
        }
    }
}
