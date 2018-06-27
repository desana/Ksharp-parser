using System.Collections;
using System.Collections.Generic;

namespace KSharpParser
{
    /// <summary>
    /// Base class for custom implementation of <see cref="IComparer"/>.
    /// </summary>
    /// <typeparam name="T">Object type of the comparer.</typeparam>
    public abstract class ComparerBase<T> : IComparer, IComparer<T>
    {
        int IComparer.Compare(object leftOperand, object rightOperand) => Compare((T)leftOperand, (T)rightOperand);


        public abstract int Compare(T leftOperand, T rightOperand);
    }
}
