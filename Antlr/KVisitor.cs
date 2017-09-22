
using Antlr4.Runtime;

namespace Antlr
{
    internal class KVisitor : KParserBaseVisitor<object>
    {        
        #region visitor helper methods

        /// <summary>
        /// Visit left parser rule.
        /// </summary>
        /// <param name="context">Current rule context.</param>
        /// <returns>Left rule context.</returns>
        private object VisitLeft(ParserRuleContext context)
        {
            var childRuleContext = context.children[0];
            return childRuleContext == null ? context : Visit(childRuleContext);
        }


        /// <summary>
        /// Visit right parser rule.
        /// </summary>
        /// <param name="context">Current rule context.</param>
        /// <returns>Right rule context.</returns>
        private object VisitRight(ParserRuleContext context)
        {
            var childRuleContext = context.GetRuleContext<ParserRuleContext>(1);
            return childRuleContext == null ? context : Visit(childRuleContext);
        }

        #endregion
    }

}