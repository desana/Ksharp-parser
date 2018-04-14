using System;

namespace KSharpParser
{
    /// <summary>
    /// Implementats <seealso cref="KSharpGrammarBaseVisitor{Result}"/>.
    /// </summary>
    [CLSCompliant(false)]
    public class KSharpVisitor : KSharpGrammarBaseVisitor<object>
    {
        private INodeEvaluator mEvaluator;

        #region "Constructors"
         
        /// <summary>
        /// Creates the visitor.
        /// </summary>
        /// <param name="resolver">Instance of a method resolver.</param>
        public KSharpVisitor(INodeEvaluator evaluator)
        {
            mEvaluator = evaluator;
        }

        #endregion        
    }
}
