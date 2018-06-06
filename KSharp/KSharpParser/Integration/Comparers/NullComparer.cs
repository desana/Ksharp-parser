using KSharp;

namespace KSharpParser.Integration
{
    public class NullComparer : ComparerBase<object>
    {
        public override int Compare(object leftOperand, object rightOperand)
        {
            return IsNull(leftOperand) == IsNull(rightOperand) ? 0 : 1;
        }

        private bool IsNull(object operand) => operand is null;
    }
}
