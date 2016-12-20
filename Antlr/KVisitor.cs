
using Antlr.Grammars;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Antlr
{
    internal class KVisitor : KBaseVisitor<object>
    {
        public override object VisitMultiplyingExpression(KParser.MultiplyingExpressionContext context)
        {
            return (int)VisitLeft(context) * (int)VisitRight(context); //todo
        }
        
        #region visitor helper methods

        /// <summary>
        /// Visit left parser rule.
        /// </summary>
        /// <param name="context">Current rule context.</param>
        /// <returns>Left rule context.</returns>
        private object VisitLeft(ParserRuleContext context)
        {
            return Visit(context.GetRuleContext<ParserRuleContext>(0));
        }


        /// <summary>
        /// Visit right parser rule.
        /// </summary>
        /// <param name="context">Current rule context.</param>
        /// <returns>Right rule context.</returns>
        private object VisitRight(ParserRuleContext context)
        {
            return Visit(context.GetRuleContext<ParserRuleContext>(1));
        }

        #endregion
    }

}