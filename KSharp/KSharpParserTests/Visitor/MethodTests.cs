
using NUnit.Framework;

using System;
using System.Globalization;

namespace KSharpParserTests
{

    [TestFixture]
    public class MethodTests : KSharpTestBase
    {
        [TestCase("5.ToString()", "5")]
        [TestCase("\"aaa\".ToUpper()", "AAA")]
        [TestCase("ToUpper(\"aaa\")", "AAA")]

        [TestCase("\"\" + ToDouble(\"2.45\", 0, \"en-us\")", "2.45")]
        [TestCase("\"\" + ToDouble(\"2,45\", 0, \"cs-cz\")", "2.45")]
        [TestCase("\"\" + ToDouble(\"2,45\")", "2.45")]

        [TestCase("Cache(\"string\".ToUpper())")]
        [TestCase("DocumentName.ToString()")]
        [TestCase("DocumentName.ToString(\"defaultValue\")")]
        [TestCase("GlobalObjects.Users.RandomSelection().UserName")]
        [TestCase("GlobalObjects.Users.Any(UserEnabled == false)")]
        [TestCase("GlobalObjects.Users.GetItem(0).UserName")]       

        public void Method_IsSuccessful_HasResult(string input, object expected)
        {
            var tree = GetParser(input).begin_expression();

            Assert.IsNull(tree.exception);
            Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
        }


        [TestCase("ToDateTime(\"12/31/2017 11:59 PM\")", "12/31/2017 11:59 PM")]
        [TestCase("ToDateTime(\"10/5/2010\")", "10/5/2010")]
        public void Method_InvariantCulture_IsSuccessful_HasDateTimeResult(string input, string expectedDate)
        {
            var tree = GetParser(input).begin_expression();

            Assert.IsNull(tree.exception);
            Assert.AreEqual(DateTime.Parse(expectedDate), Visitor.GetFirstResult(tree));
        }


        [TestCase("ToDateTime(\"31.12.2017 11:59 PM\")", "31.12.2017 11:59 PM")]
        [TestCase("ToDateTime(\"10.5.2010\")", "10.5.2010")]
        public void Method_CzechCulture_IsSuccessful_HasDateTimeResult(string input, string expectedDate)
        {
            var tree = GetParser(input).begin_expression();

            Assert.IsNull(tree.exception);
            Assert.AreEqual(DateTime.Parse(expectedDate, CultureInfo.GetCultureInfo("cs-cz")), Visitor.GetFirstResult(tree));
        }


        [TestCase("ToTimeSpan(\"1:00:00\")")]
        public void Method_IsSuccessful_HasTimeSpanResult(string input)
        {
            var tree = GetParser(input).begin_expression();

            Assert.IsNull(tree.exception);
            Assert.AreEqual(TimeSpan.Parse("1:00:00"), Visitor.GetFirstResult(tree));
        }
    }
}