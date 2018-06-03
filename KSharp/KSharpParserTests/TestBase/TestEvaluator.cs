using KSharp;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace KSharpParserTests
{
    public class TestEvaluator : INodeEvaluator
    {
        public IDictionary<string, object> Parameters;

        
        public IDictionary<Type, IComparer> KnownComparers { get; }

        
        private Mock<INodeEvaluator> evaluatorMock;


        public TestEvaluator()
        {
            Parameters = new Dictionary<string, object>();

            KnownComparers = new Dictionary<Type, IComparer>
            {
                { typeof(string), StringComparer.Create(CultureInfo.InvariantCulture, false) }
            };

            evaluatorMock = new Mock<INodeEvaluator>();

            evaluatorMock.Setup(m => m.InvokeMethodForObject("\"aaa\"", "ToUpper", new object[] { })).Returns("AAA");
            evaluatorMock.Setup(m => m.InvokeMethod("ToUpper", new object[] { "aaa" })).Returns("AAA");
            evaluatorMock.Setup(m => m.InvokeMethodForObject(5, "ToString", new object[] { })).Returns("5");

            evaluatorMock.Setup(m => m.InvokeMethod("ToDouble", new object[] { "2.45", 0, "en-us" })).Returns(2.45);
            evaluatorMock.Setup(m => m.InvokeMethod("ToDouble", new object[] { "2,45", 0, "cs-cz" })).Returns(2.45);
            evaluatorMock.Setup(m => m.InvokeMethod("ToDouble", new object[] { "2,45" })).Returns(2.45);
        }


        public object GetVariableValue(string variableName)
        {
            return null;
        }


        public object InvokeMethod(string methodName, object[] arguments)
        {
            return evaluatorMock.Object.InvokeMethod(methodName, arguments);
        }


        public object InvokeMethodForObject(object objectToCallMethodOn, string methodName, object[] arguments)
        {
            return evaluatorMock.Object.InvokeMethodForObject(objectToCallMethodOn, methodName, arguments);
        }


        public void SaveParameter(string parameterName, object parameterValue)
        {
            Parameters.Add(parameterName, parameterValue);
        }
    }

}
