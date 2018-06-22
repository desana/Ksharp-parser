using static KSharpParser.KSharpGrammarParser;

namespace KSharp
{
    /// <summary>
    /// Extensions methods for matching operators to <see cref="OperatorTypeEnum"/>.
    /// </summary>
    internal static class KSharpVisitorExtensions
    {
        /// <summary>
        /// Matches assignment operators to <see cref="OperatorTypeEnum"/>.
        /// </summary>
        /// <param name="visitor">Visitor.</param>
        /// <param name="context">Current rule context.</param>
        /// <returns>Correct value of <see cref="OperatorTypeEnum"/>.</returns>
        public static OperatorTypeEnum MatchAssignmentOperator(this KSharpVisitor visitor, Assignment_operatorContext context)
        {
            if (context.PLUS_ASSIGN() != null)
            {
                return OperatorTypeEnum.PLUS_ASSIGN;
            }

            else if (context.MINUS_ASSIGN() != null)
            {
                return OperatorTypeEnum.MINUS_ASSIGN;
            }

            else if (context.MUL_ASSIGN() != null)
            {
                return OperatorTypeEnum.MULTIPLY_ASSIGN;
            }

            else if (context.DIV_ASSIGN() != null)
            {
                return OperatorTypeEnum.DIVIDE_ASSIGN;
            }

            else if (context.MOD_ASSIGN() != null)
            {
                return OperatorTypeEnum.MODULO_ASSIGN;
            }

            else if (context.AND_ASSIGN() != null)
            {
                return OperatorTypeEnum.AND_ASSIGN;
            }

            else if (context.OR_ASSIGN() != null)
            {
                return OperatorTypeEnum.OR_ASSIGN;
            }

            else if (context.XOR_ASSIGN() != null)
            {
                return OperatorTypeEnum.XOR_ASSIGN;
            }

            else if (context.LEFT_SHIFT_ASSIGN() != null)
            {
                return OperatorTypeEnum.LEFT_SHIFT_ASSIGN;
            }

            else if (context.right_shift_assignment() != null)
            {
                return OperatorTypeEnum.RIGHT_SHIFT_ASSIGN;
            }

            else if (context.ASSIGN() != null)
            {
                return OperatorTypeEnum.ASSIGN;
            }

            return OperatorTypeEnum.NONE;
        }


        /// <summary>
        /// Matches unary operators to <see cref="OperatorTypeEnum"/>.
        /// </summary>
        /// <param name="visitor">Visitor.</param>
        /// <param name="context">Current rule context.</param>
        /// <returns>Correct value of <see cref="OperatorTypeEnum"/>.</returns>
        public static OperatorTypeEnum MatchUnaryOperator(this KSharpVisitor visitor, Unary_expressionContext context)
        {
            if (context.PLUS() != null)
            {
                return OperatorTypeEnum.PLUS;
            }

            else if (context.MINUS() != null)
            {
                return OperatorTypeEnum.MINUS;
            }

            else if (context.BANG() != null || context.WAVE_DASH() != null)
            {
                return OperatorTypeEnum.BANG;
            }

            return OperatorTypeEnum.NONE;
        }
    }
}
