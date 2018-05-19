using System;
using System.Collections;
using System.Collections.Generic;

namespace KSharp
{
    /// <summary>
    /// Defines methods which need to be evaluated with the system context.
    /// </summary>
    public interface INodeEvaluator
    {
        /// <summary>
        /// A collection of the object comparers for custom type comparison.
        /// </summary>
        IDictionary<Type, IComparer> KnownComparers { get; }


        /// <summary>
        /// Saves the parameter value into current evaluation context.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterValue">Value of the parameter.</param>
        void SaveParameter(string parameterName, object parameterValue);


        /// <summary>
        /// Obtains value from the system (sytem variables, settings, database items)...
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <returns>Value of a system variable.</returns>
        object GetVariableValue(string variableName);


        /// <summary>
        /// Invokes system method.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="arguments">Arguments of the method.</param>
        /// <returns>Return value of invoked method.</returns>
        object InvokeMethod(string methodName, object[] arguments);
    }
}
