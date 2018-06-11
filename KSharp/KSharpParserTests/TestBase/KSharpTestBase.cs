using Antlr4.Runtime;

using KSharp;
using KSharpParser;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace KSharpParserTests
{
    public class KSharpTestBase
    {
        public KSharpVisitor Visitor { get; private set; }


        public Mock<INodeEvaluator> EvaluatorMock { get; private set; }
        

        private string consoleOutput;


        [SetUp]
        public void SetUp()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            EvaluatorMock = new Mock<INodeEvaluator>();

            EvaluatorMock.Setup(m => m.KnownComparers).Returns(new Dictionary<Type, IComparer>
            {
                { typeof(string), StringComparer.Create(CultureInfo.InvariantCulture, false) }
            });

            EvaluatorMock.Setup(m => m.FlushOutput()).Returns(() => {
                var temp = consoleOutput;
                consoleOutput = null;
                return temp;
            });

            EvaluatorMock.Setup(m => m.InvokeMember("\"aaa\"", "ToUpper", new object[] { })).Returns("AAA");
            EvaluatorMock.Setup(m => m.InvokeMethod("ToUpper", new object[] { "aaa" })).Returns("AAA");
            EvaluatorMock.Setup(m => m.InvokeMember(5, "ToString", new object[] { })).Returns("5");

            EvaluatorMock.Setup(m => m.InvokeMethod("ToDouble", new object[] { "2.45", 0, "en-us" })).Returns(2.45);
            EvaluatorMock.Setup(m => m.InvokeMethod("ToDouble", new object[] { "2,45", 0, "cs-cz" })).Returns(2.45);
            EvaluatorMock.Setup(m => m.InvokeMethod("ToDouble", new object[] { "2,45" })).Returns(2.45);

            EvaluatorMock.Setup(m => m.InvokeMethod("ToDateTime", new object[] { "10/5/2010" })).Returns(DateTime.Parse("10/5/2010"));
            EvaluatorMock.Setup(m => m.InvokeMethod("ToDateTime", new object[] { "12/31/2017 11:59 PM" })).Returns(DateTime.Parse("12/31/2017 11:59 PM"));
            EvaluatorMock.Setup(m => m.InvokeMethod("ToTimeSpan", new object[] { "1:00:00" })).Returns(TimeSpan.Parse("1:00:00"));
            EvaluatorMock.Setup(m => m.InvokeMethod("ToDateTime", new object[] { "10.5.2010" })).Returns(DateTime.Parse("10.5.2010", CultureInfo.GetCultureInfo("cs-cz")));
            EvaluatorMock.Setup(m => m.InvokeMethod("ToDateTime", new object[] { "31.12.2017 11:59 PM" })).Returns(DateTime.Parse("31.12.2017 11:59 PM", CultureInfo.GetCultureInfo("cs-cz")));

            EvaluatorMock.Setup(m => m.InvokeMethod("List", It.IsAny<object[]>())).Returns<string, object[]>((name, items) => items as IList);

            EvaluatorMock
                .Setup(m => m.InvokeMember(It.IsAny<object>(), "toupper", It.IsAny<object[]>()))
                .Returns<object, string, object[]>((word, methodName, args) => { return (word.ToString()).ToUpper(); });

            EvaluatorMock.Setup(m => m.InvokeMethod("GetDict", new object[] { })).Returns(new Dictionary<string, int>()
            {
                { "one", 1 }
            });

            EvaluatorMock.Setup(m => m.InvokeMethod("print", It.IsAny<object[]>())).Returns<string, object[]>((methodName, objectToPrint) => {
                consoleOutput += Convert.ToString(objectToPrint[0]);
                return null;
            });

            EvaluatorMock.Setup(m => m.InvokeMethod("println", It.IsAny<object[]>())).Returns<string, object[]>((methodName, objectToPrint) => {
                consoleOutput += Convert.ToString(objectToPrint[0]);
                string temp = consoleOutput;
                consoleOutput = null;
                return temp;
            });

            EvaluatorMock.Setup(m => m.GetVariableValue("string")).Returns(typeof(string));
            EvaluatorMock.Setup(m => m.InvokeMember(typeof(string), "Empty", null)).Returns(string.Empty);

            EvaluatorMock.Setup(m => m.InvokeMember(string.Empty, "Length", null)).Returns(0);
            
            EvaluatorMock.Setup(m => m.GetVariableValue("DocumentName")).Returns("This is sparta!");
            EvaluatorMock.Setup(m => m.InvokeMember("This is sparta!", "ToString", new object[] { })).Returns("This is sparta!");
            
            EvaluatorMock.Setup(m => m.GetVariableValue("DocumentName2")).Returns("This is sparta!");
            EvaluatorMock.Setup(m => m.InvokeMember("This is sparta!", "ToString", new object[] {"defaultValue" })).Returns("defaultValue");
            
            EvaluatorMock.Setup(m => m.InvokeMember("\"string\"", "ToUpper", new object[] { })).Returns("STRING");
            EvaluatorMock.Setup(m => m.InvokeMethod("Cache", new object[] { "STRING" })).Returns("STRING - Cached");

            EvaluatorMock.Setup(m => m.GetVariableValue("GlobalObjects")).Returns("GlobalObjects");
            EvaluatorMock.Setup(m => m.InvokeMember("GlobalObjects", "Users", null)).Returns("Users");

            EvaluatorMock.Setup(m => m.GetVariableValue("Users")).Returns("Users");
            EvaluatorMock.Setup(m => m.InvokeMember("Users", "RandomSelection", new object[] { })).Returns("RandomName");

            EvaluatorMock.Setup(m => m.GetVariableValue("RandomName")).Returns("RandomName");
            EvaluatorMock.Setup(m => m.InvokeMember("RandomName", "UserName", null)).Returns("John Doe");

            EvaluatorMock.Setup(m => m.InvokeMember("Users", "GetItem", new object[] { 0 })).Returns("FirstUser");
            EvaluatorMock.Setup(m => m.GetVariableValue("FirstUser")).Returns("FirstUser");

            EvaluatorMock.Setup(m => m.InvokeMember("FirstUser", "UserName", null)).Returns("Echo from Dollhouse");

            EvaluatorMock.Setup(m => m.GetVariableValue("UserEnabled")).Returns(true);
            EvaluatorMock.Setup(m => m.InvokeMember("Users", "Any", new object[] { false })).Returns("Jack Nenabled");
            
            EvaluatorMock.Setup(m => m.InvokeIndexer("Users", 1)).Returns('s');
            EvaluatorMock.Setup(m => m.InvokeMember('s', "UserName", null)).Returns("Alpha from Dollhouse");

            EvaluatorMock.Setup(m => m.InvokeIndexer("\"hello\"", 1)).Returns('e');
                        
            EvaluatorMock.Setup(m => m.InvokeIndexer(new Dictionary<string, int>()
            {
                { "one", 1 }
            }, "one")).Returns(1);

            EvaluatorMock.Setup(m => m.InvokeIndexer(It.IsAny<IList>(), It.IsAny<int>())).Returns<IList, int>((collection, index) =>
            {
                return collection[index];
            });

            var ct = new CancellationTokenSource();
            ct.CancelAfter(15000);
            EvaluatorMock.Setup(m => m.GetCancellationToken()).Returns(ct.Token);
            
            Visitor = new KSharpVisitor(EvaluatorMock.Object);
        }
        

        public KSharpGrammarParser GetParser(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            KSharpGrammarLexer lexer = new KSharpGrammarLexer(inputStream);

            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);

            KSharpGrammarParser parser = new KSharpGrammarParser(commonTokenStream);
            return parser;
        }
    }
}