using System;

namespace KSharp
{
    internal class NumericComparer : ComparerBase<object>
    {
        public override int Compare(object leftOperand, object rightOperand)
        {
            return Convert.ToDecimal(leftOperand) == Convert.ToDecimal(rightOperand) ? 0 : 1;
        }
    }
}