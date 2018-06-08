
using NUnit.Framework;

using System;
using System.Globalization;

namespace KSharpParserTests
{
    public class AcessorTests
    {
        [TestFixture]
        public class IndexerTests : KSharpTestBase
        {
            [TestCase("list = List(1,2,3,4,4,5,4); list[0]", 1)]
            [TestCase("list = List(1,2,3,4,4,5,4); list[0+1]", 2)]
            [TestCase("i = 0; list = List(1,2,3,4,4,5,4); list[i]", 1)]

            [TestCase("dict = GetDict(); dict[\"one\"]", 1)]
            [TestCase("\"hello\"[1]", 'e')]
            [TestCase("list = List(1,2,3,4,4,5,4); x = List(list, 42); x[list[0]]", 42)]
            [TestCase("list = List(1,2,3,4,4,5,4); x = List(list, 42); x[0][0]", 1)]

            [TestCase("\"hello\"[1]", 'e')]

            public void Indexer_IsSuccessful(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
            }
        }


        [TestFixture]
        public class MethodTests : KSharpTestBase
        {
            [TestCase("5.ToString()", "5")]
            [TestCase("\"aaa\".ToUpper()", "AAA")]
            [TestCase("ToUpper(\"aaa\")", "AAA")]

            [TestCase("\"\" + ToDouble(\"2.45\", 0, \"en-us\")", "2.45")]
            [TestCase("\"\" + ToDouble(\"2,45\", 0, \"cs-cz\")", "2.45")]
            [TestCase("\"\" + ToDouble(\"2,45\")", "2.45")]

            [TestCase("Cache(\"string\".ToUpper())", "STRING - Cached")]
            [TestCase("DocumentName.ToString()", "This is sparta!")]
            [TestCase("DocumentName2.ToString(\"defaultValue\")", "defaultValue")]

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


        [TestFixture]
        public class PropertyTests : KSharpTestBase
        {
            [TestCase("string.Empty.Length", 0)]
            [TestCase("string.Empty", "")]
            public void Property_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();
                
                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
            }
        }

        [TestFixture]
        public class MixedTests : KSharpTestBase
        {
            [TestCase("GlobalObjects.Users.RandomSelection().UserName", "John Doe")]
            [TestCase("GlobalObjects.Users.Any(UserEnabled == false)", "Jack Nenabled")]
            [TestCase("GlobalObjects.Users.GetItem(0).UserName", "Echo from Dollhouse")]
            [TestCase("GlobalObjects.Users[1].UserName", "Alpha from Dollhouse")]

            public void Mixed_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetFirstResult(tree));
            }

        }
    }
}