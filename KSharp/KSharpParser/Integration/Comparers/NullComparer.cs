using KSharp;

namespace KSharpParser.Integration
{
    /// <summary>
    /// Returns 0 if both operands are <c>null</c>, 1 otherwise.
    /// </summary>
    public class NullComparer : ComparerBase<object>
    {
        public override int Compare(object leftOperand, object rightOperand)
        {
            return IsNull(leftOperand) == IsNull(rightOperand) ? 0 : 1;
        }

        private bool IsNull(object operand) => operand is null;
    }
}
