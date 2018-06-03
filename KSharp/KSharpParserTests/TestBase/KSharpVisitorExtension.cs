using KSharp;

using System.Collections;

using static KSharpParser.KSharpGrammarParser;

namespace KSharpParserTests
{
    public static class KSharpVisitorExtension
    {

        public static object GetFirstResult(this KSharpVisitor visitor, Begin_expressionContext tree)
        {
            var list = visitor.Visit(tree) as IList;
            return list[0];
        }


        public static IList GetResultList(this KSharpVisitor visitor, Begin_expressionContext tree)
        {
            return visitor.Visit(tree) as IList;
        }
    }
}