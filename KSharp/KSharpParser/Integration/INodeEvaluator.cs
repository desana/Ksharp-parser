using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace KSharp
{
    /// <summary>
    /// Defines methods which need to be evaluated with the system context.
    /// </summary>
    /// <remarks>
    /// Method which may cause infinite loops should use <seealso cref="CancellationToken"/>.
    /// </remarks>
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


        /// <summary>
        /// Invokes member of the object.
        /// </summary>
        /// <param name="accessedObject">Object to invoke member on.</param>
        /// <param name="propertyOrMethodName">Name of the member.</param>
        /// <param name="arguments">Arguments, if the member is a method, otherwise the argument is <c>null</c>.</param>
        /// <returns>Return value of invoked member.</returns>
        object InvokeMember(object accessedObject, string propertyOrMethodName, object[] arguments);


        /// <summary>
        /// Invokes indexer on object
        /// </summary>
        /// <param name="collection">Collection to invoke on.</param>
        /// <param name="index">Index value.</param>
        /// <returns></returns>
        object InvokeIndexer(object collection, object index);


        /// <summary>
        /// Flushes output stream.
        /// </summary>
        /// <returns>Output stream.</returns>
        object FlushOutput();


        /// <summary>
        /// Returns cancellation token with timer set to correct value.
        /// </summary>
        /// <returns>Cancellation token.</returns>
        CancellationToken GetCancellationToken();
    }
}
