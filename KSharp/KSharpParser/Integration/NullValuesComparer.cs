using KSharp;
using System;

namespace KSharpParser.Integration
{
    public class NullValuesComparer : ComparerBase<object>
    {
        public override int Compare(object leftOperand, object rightOperand)
        {
            return IsNull(leftOperand) == IsNull(rightOperand) ? 0 : throw new NotSupportedException($"Comparison of {leftOperand.GetType().Name} and {rightOperand.GetType().Name} is not supported"); ;
        }

        private bool IsNull(object operand)
        {
            if (operand is null)
            {
                return true;
            }
            else if (operand is int)
            {
                return (int)operand == 0;
            }
            else if (operand is decimal)
            {
                return (decimal)operand == 0;
            }
            else if (operand is string)
            {
                return (string)operand == null;
            }
            return false;
        }
    }
}
