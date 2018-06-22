
namespace KSharpParser
{
    /// <summary>
    /// Enum representing unary and assignment operators.
    /// </summary>
    public enum OperatorTypeEnum
    {
        /// <summary>
        /// Represents '=' operator.
        /// </summary>
        ASSIGN,


        /// <summary>
        /// Represents '+=' operator.
        /// </summary>
        PLUS_ASSIGN,


        /// <summary>
        /// Represents '-=' operator.
        /// </summary>
        MINUS_ASSIGN,


        /// <summary>
        /// Represents '*=' operator.
        /// </summary>
        MULTIPLY_ASSIGN,


        /// <summary>
        /// Represents '/=' operator.
        /// </summary>
        DIVIDE_ASSIGN,


        /// <summary>
        /// Represents '%=' operator.
        /// </summary>
        MODULO_ASSIGN,


        /// <summary>
        /// Represents '&=' operator.
        /// </summary>
        AND_ASSIGN,


        /// <summary>
        /// Represents '|=' operator.
        /// </summary>
        OR_ASSIGN,


        /// <summary>
        /// Represents '^=' operator.
        /// </summary>
        XOR_ASSIGN,


        /// <summary>
        /// Represents '<<=' operator.
        /// </summary>
        LEFT_SHIFT_ASSIGN,


        /// <summary>
        /// Represents '>>=' operator.
        /// </summary>
        RIGHT_SHIFT_ASSIGN,


        /// <summary>
        /// Represents '+' operator.
        /// </summary>
        PLUS,


        /// <summary>
        /// Represents '-' operator.
        /// </summary>
        MINUS,


        /// <summary>
        /// Represents '!' and '~' operators.
        /// </summary>
        BANG,


        /// <summary>
        /// Represents default value, when none of the operators is matched.
        /// </summary>
        NONE,
    }
}
