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


        private string consoleOutput;


        public TestEvaluator()
        {
            Parameters = new Dictionary<string, object>();

            KnownComparers = new Dictionary<Type, IComparer>
            {
                { typeof(string), StringComparer.Create(CultureInfo.InvariantCulture, false) }
            };

            evaluatorMock = new Mock<INodeEvaluator>();

            evaluatorMock.Setup(m => m.InvokeMember("\"aaa\"", "ToUpper", new object[] { })).Returns("AAA");
            evaluatorMock.Setup(m => m.InvokeMethod("ToUpper", new object[] { "aaa" })).Returns("AAA");
            evaluatorMock.Setup(m => m.InvokeMember(5, "ToString", new object[] { })).Returns("5");

            evaluatorMock.Setup(m => m.InvokeMethod("ToDouble", new object[] { "2.45", 0, "en-us" })).Returns(2.45);
            evaluatorMock.Setup(m => m.InvokeMethod("ToDouble", new object[] { "2,45", 0, "cs-cz" })).Returns(2.45);
            evaluatorMock.Setup(m => m.InvokeMethod("ToDouble", new object[] { "2,45" })).Returns(2.45);

            evaluatorMock.Setup(m => m.InvokeMethod("ToDateTime", new object[] { "10/5/2010" })).Returns(DateTime.Parse("10/5/2010"));
            evaluatorMock.Setup(m => m.InvokeMethod("ToDateTime", new object[] { "12/31/2017 11:59 PM" })).Returns(DateTime.Parse("12/31/2017 11:59 PM"));
            evaluatorMock.Setup(m => m.InvokeMethod("ToTimeSpan", new object[] { "1:00:00" })).Returns(TimeSpan.Parse("1:00:00"));
            evaluatorMock.Setup(m => m.InvokeMethod("ToDateTime", new object[] { "10.5.2010" })).Returns(DateTime.Parse("10.5.2010", CultureInfo.GetCultureInfo("cs-cz")));
            evaluatorMock.Setup(m => m.InvokeMethod("ToDateTime", new object[] { "31.12.2017 11:59 PM" })).Returns(DateTime.Parse("31.12.2017 11:59 PM", CultureInfo.GetCultureInfo("cs-cz")));

            evaluatorMock.Setup(m => m.InvokeMethod("List", It.IsAny < object[]>()))
            .Returns<string, object[]>((name, items) => items as IList);

            evaluatorMock
                .Setup(m => m.InvokeMember(It.IsAny<object>(), "toupper", It.IsAny<object[]>()))
                .Returns<object, string, object[]>((word, methodName, args) => { return (word.ToString()).ToUpper();});

            evaluatorMock.Setup(m => m.InvokeMethod("GetDict", new object[] { })).Returns(new Dictionary<string, int>()
            {
                { "one", 1 }
            });

            evaluatorMock.Setup(m => m.InvokeMethod("print", It.IsAny<object[]>())).Returns<string, object[]>((methodName, objectToPrint) => {
                consoleOutput += Convert.ToString(objectToPrint[0]);
                return null;
                });
            evaluatorMock.Setup(m => m.InvokeMethod("println", It.IsAny<object[]>())).Returns<string, object[]>((methodName, objectToPrint) => {
                consoleOutput += Convert.ToString(objectToPrint[0]);
                string temp = consoleOutput;
                consoleOutput = null;
                return temp;
            });
        }


        public object GetVariableValue(string variableName)
        {
            return null;
        }


        public object InvokeMethod(string methodName, object[] arguments)
        {
            return evaluatorMock.Object.InvokeMethod(methodName, arguments);
        }


        public void SaveParameter(string parameterName, object parameterValue)
        {
            Parameters.Add(parameterName, parameterValue);
        }


        public object FlushOutput()
        {
            var temp = consoleOutput;
            consoleOutput = null;
            return temp;
        }


        public object InvokeMember(object accessedObject, string propertyOrMethodName, object[] arguments)
        {
            return evaluatorMock.Object.InvokeMember(accessedObject, propertyOrMethodName, arguments);
        }
    }
}
