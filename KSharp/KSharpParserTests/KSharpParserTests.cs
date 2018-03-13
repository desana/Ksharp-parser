using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using KSharpParser;

using NUnit.Framework;

namespace KSharprTests
{    
    public class KSharprTests
    {
        #region "Helper methods"

        public static KSharpGrammarParser CreaterParserFromInput(string input)
        {
            AntlrInputStream inputStream = new AntlrInputStream(input);
            KSharpGrammarLexer lexer = new KSharpGrammarLexer(inputStream);

            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            commonTokenStream.Fill();
            var tokens = commonTokenStream.GetTokens();

            KSharpGrammarParser parser = new KSharpGrammarParser(commonTokenStream);
            return parser;
        }
        

        public static int GetParsingErrors(string input)
        {
            KSharpGrammarParser parser = CreaterParserFromInput(input);

            ITree tree = parser.expression();

            return parser.NumberOfSyntaxErrors;
        }

        #endregion

        [TestFixture]
        public class BasicStructuresTests
        {
            [TestCase("Class.Member")]
            [TestCase("Class.Mehod()")]
            [TestCase("Variable")]

            [TestCase("1")]
            [TestCase("-1")]
            [TestCase("30%")]

            [TestCase("true")]

            [TestCase("\"String\"")]
            public void BasicStructures_IsSuccessful(string input)
            {
                Assert.AreEqual(GetParsingErrors(input), 0);
            }
        }


        [TestFixture]
        public class LogicalTests
        {
            [TestCase("1 < 2")]
            [TestCase("true && true")]
            public void Logical_IsSuccessful(string input)
            {
                Assert.AreEqual(GetParsingErrors(input), 0);
            }
        }


        [TestFixture]
        public class BracesTests
        {
            [TestCase("{1 < 2}")]
            [TestCase("if (!Condition) {{ return Value }}")]
            public void Braces_IsSuccessful(string input)
            {
                Assert.AreEqual(GetParsingErrors(input), 0);
            }
        }

               
        [TestFixture]
        public class AssignmentTests
        {
            [TestCase("Variable = 1")]
            [TestCase("Variable = \"String\"")]
            public void Assignment_IsSuccessful(string input)
            {
                Assert.AreEqual(GetParsingErrors(input), 0);
            }
        }

      
        [TestFixture]
        public class ArithmeticalTests
        {
            [TestCase("1 + 2")]
            [TestCase("5 mod 2")]
            [TestCase("SubVariable + AnotherSubVariable")]

            [TestCase("Variable = 1; Variable += 2")]
            [TestCase("Variable = SubVariable + AnotherSubVariable")]
            public void Arithmetical_IsSuccessful(string input)
            {
                Assert.AreEqual(GetParsingErrors(input), 0);
            }
        }

        
        [TestFixture]
        public class StringTests
        {
            [TestCase("\"String\" + \"AnotherString\"")]
            [TestCase("\"String\" - \"AnotherString\"")]

            [TestCase("Variable = \"String\" + \"AnotherString\"")]
            public void String_IsSuccessful(string input)
            {
                Assert.AreEqual(GetParsingErrors(input), 0);
            }


            [TestCase("\"FinalString\" = \"String\" + \"AnotherString\"")]
            public void String_IsNotSuccessful(string input)
            {
                Assert.AreNotEqual(GetParsingErrors(input), 0);
            }
        }

        
        [TestFixture]
        public class IfTests
        {
            [TestCase("if (Condition) { return Value; }")]

            [TestCase("if (Condition) { return Value }")]        
            [TestCase("if (Condition) { return Value } else { return AnotherValue }")]

            [TestCase("if (!Condition) { return Value }")]
            public void If_IsSuccessful(string input)
            {
                Assert.AreEqual(GetParsingErrors(input), 0);
            }


            [TestCase("if Condition")]
            [TestCase("if Condition return Value")]
            [TestCase("if Condition { return Value }")]

            [TestCase("if (Condition) Value")]
            [TestCase("if (Condition) return Value")]
            [TestCase("if (Condition) { Value } else AnotherValue")]
            [TestCase("if (Condition) else { return Value }")]
            [TestCase("if (Condition) then { return Value } else { return Value }")]
            public void If_IsNotSuccessful(string input)
            {
                Assert.AreNotEqual(GetParsingErrors(input), 0);
            }
        }


        [TestFixture]
        public class LambdaTests
        {
            [TestCase("lambdaSucc = (x => x + 1); lambdaSucc(3)")]
            [TestCase("lambdaMultiply = ((x, y) => x * y); lambdaMultiply(2,3)")]
            public void Lambda_IsSuccessful(string input)
            {
                Assert.AreEqual(GetParsingErrors(input), 0);
            }
        }


        [TestFixture]
        public class ForTests
        {
            [TestCase("z = 0; for (i = 0; i < 5; i++) { z += 1 }; z")]
            [TestCase("z = 0; for (i = 0; i < 5; i++) { z += 1 } ")]
            [TestCase("for (i=0; i<=5 ; i++) {if (i == 3) {continue}; i}")]

            [TestCase("for (i=0; i<=5 ; i++) {i}")]
            public void For_IsSuccessful(string input)
            {
                Assert.AreEqual(GetParsingErrors(input), 0);
            }
        }

        
        [TestFixture]
        public class ForeachTests
        {
            [TestCase("z = \"\"; foreach (x in \"hello\") {z += x.toupper()}; z")]
            [TestCase("z = \"\"; foreach (x in \"hello\") {z += x.toupper()}")]
            public void Foreach_IsSuccessful(string input)
            {
                Assert.AreEqual(GetParsingErrors(input), 0);
            }
        }

        
        [TestFixture]
        public class WhileTests
        {
            [TestCase("z = 1; while (z<10) {++z}; z")]
            [TestCase("z = 1; while (z<10) {++z} ")]
            
            [TestCase("z = 0; while (z < 10) {if (z > 4) {break}; ++z}")]

            [TestCase("i = 1; while (i < 4) {print(i++)}; \"ignored\"")]
            [TestCase("i = 1; while (i < 4) {print(i++)}; return \"result\"")]
            public void While_IsSuccessful(string input)
            {
                Assert.AreEqual(GetParsingErrors(input), 0);
            }
        }


        [TestFixture]
        public class CommentTests
        {
            [TestCase("// This is a one-line comment. Initiated by two forward slashes, spans across one full line.")]
            [TestCase(@"/* This is a multi - line comment.
        Opened by a forward slash - asterisk sequence and closed with the asterisk - forward slash sequence.
        Can span across any number of lines.
        */ ")]
            [TestCase("x = 5; y = 3; /* This is an inline comment nested in the middle of an expression. */ x+= 2; x + y")]
            public void Comment_IsSuccessful(string input)
            {
                Assert.AreEqual(GetParsingErrors(input), 0);
            }
        }


        [TestFixture]
        public class MiscTests { 

            [TestCase("foreach (page in CurrentDocument.CultureVersions) {if (page.DocumentCulture != CurrentDocument.DocumentCulture) {\"<link rel=\"alternate\" href=\"\"+ page.AbsoluteUrl + \"\" hreflang=\"\"+ page.DocumentCulture +\"\"/>\";}}")]
            [TestCase("GlobalObjects.Users[\"administrator\"].FullName")]

            // Documentation examples https://docs.kentico.com/k11/macro-expressions/macro-syntax

            // Todo: categorize

            [TestCase("CurrentPageInfo.DocumentPageTitle + \" | suffix\"")]
            [TestCase("CurrentDocument.Children.FirstItem ?? \"No child pages\"")]
            [TestCase("50 == 5*10")]
            [TestCase("CurrentUser.UserName != \"administrator\"")]
            [TestCase("CurrentDocument == Documents[\"/Services/WebDesign\"]")]
            [TestCase("CurrentPageInfo.DocumentPublishFrom <= DateTime.Now")]            
            

            [TestCase("Replace(\"The sky is blue on blue planets\", \"blue\", \"red\")")]
            [TestCase("\"The sky is blue on blue planets\".Replace(\"blue\", \"red\")")]

            [TestCase("\"word\".ToUpper()")]
            [TestCase("ToUpper(\"word\")")]

            [TestCase("x = 5; x + 7")]
            [TestCase("x = 5; y = 3; x += 2; x + y")]
            [TestCase("z = 1; if (z<3) {\"z is less than 3\"}")]
            [TestCase("z = 5; if (z < 3) {\"z is less than 3\"} else {\"z is greater than or equal to 3\"}")]
            [TestCase("x=1; y=2; x > y ? \"The first parameter is greater\" : \"The second parameter is greater\"")]

            [TestCase("orders = ECommerceContext.CurrentCustomer.AllOrders; if (orders.Count > 0) {print(\"<ul>\"); foreach (order in orders) { foreach (item in order.OrderItems) { print(\"<li>\" + item.OrderItemSKUName + \"</li>\") }}; print(\"</ul>\");}")]
            [TestCase("orders = ECommerceContext.CurrentCustomer.AllOrders; if (orders.Count > 0) { result = \"<ul>\"; foreach (order in orders) { foreach (item in order.OrderItems) { result += \"<li>\" + item.OrderItemSKUName + \"</li>\" } }; return result + \"</ul>\";}")]

            [TestCase("\"red\"; \"yellow\"; return \"green\"; \"blue\"")]
            [TestCase("z = \"\"; foreach (x in \"hello\") {return \"ignore the loop\"; z += x }")]

            [TestCase("\"hello\"[1]")]
            [TestCase("dataRow[\"FirstName\"]")]

            [TestCase("CurrentUser.UserDateOfBirth|(default)N|A")]
            [TestCase("ArticleSummary|(encode)true")]
            [TestCase("ArticleText|(encode)true|(recursive)true")]
            [TestCase("DocumentPublishFrom|(culture)en-us")]
            [TestCase("Contains(\"term\", ArticleText)|(casesensitive)true")]
            [TestCase("GetDocumentUrl()|(timeout)2000")]
            [TestCase("QueryString.Param|(handlesqlinjection)true")]
            [TestCase("Documents[\" / News\"].Children.WithAllData|(debug)true")]

            // Other documentation  // todo advanced text processing https://docs.kentico.com/k11/macro-expressions/reference-macro-methods 

            [TestCase("Cache(\"string\".ToUpper())")]
            [TestCase("Cache(CurrentUser.GetFormattedUserName(), 5, true, \"username|\" + CurrentUser.UserName, GetCacheDependency(\"cms.user|all\"))")]
            [TestCase("CurrentUser.Children[\"cms_category\"][0].CategoryName")]
            [TestCase("\"String1\".ConnectStrings(\"String2\")")]
            [TestCase("ConnectStrings(\"String1\", \"String2\")")]
            [TestCase("String.ConnectStrings(\"First part\", \"Second part\")")]
            [TestCase("CurrentDocument.GetValue(\"NewsTitle\")")]
            [TestCase("CurrentDocument.GetProperty(\"NewsTitle\")")]
            [TestCase("GlobalObjects.Users.GetItem(0).UserName")]
            [TestCase("GlobalObjects.Users.OrderBy(\"UserCreated DESC\").FirstItem.UserNameUser")]
            [TestCase("GlobalObjects.Users.Where(\"Email LIKE '%@localhost.local'\").FirstItem.UserName")]
            [TestCase("GlobalObjects.Users.TopN(1).FirstItem.UserName ")]
            [TestCase("GlobalObjects.Users.Columns(\"UserName, Email\").FirstItem.UserName")]
            [TestCase("GlobalObjects.Users.Filter(UserID == 53).FirstItem.UserName")]
            [TestCase("Documents.ClassNames(\"cms.menuitem;cms.news\").Count")]
            [TestCase("CurrentDocument.ClassName.InList(\"cms.menuitem, cms.root\".Split(\", \"))")]
            [TestCase("GlobalObjects.Users.All(UserEnabled == true)")]
            [TestCase("GlobalObjects.Users.Any(UserEnabled == false)")]
            [TestCase("GlobalObjects.Users.Exists(UserEnabled == false)")]
            [TestCase("GlobalObjects.Users.RandomSelection().UserName")]
            [TestCase("Documents.SelectInterval(0,9)")]

            [TestCase("DocumentName.ToString()")]
            [TestCase("DocumentName.ToString(\"defaultValue\")")]
            [TestCase("DocumentName.ToString(\"defaultValue\", \"en - US\")")]
            [TestCase("DocumentName.ToString(\"defaultValue\", DocumentCulture, \"Document name is: {0}\")")]
            [TestCase("DocumentID.ToInt()")]
            [TestCase("DocumentShowInSiteMap.ToBool()")]
            [TestCase("DocumentID.ToDecimal()")]
            [TestCase("DocumentID.ToDecimal(\"0.0\", DocumentCulture)")]
            [TestCase("DocumentID.ToDouble()")]
            [TestCase("DocumentID.ToDouble(\"0.0\", DocumentCulture)")]
            [TestCase("DocumentGUID.ToGuid()")]
            [TestCase("ToDateTime(\"12/31/2017 11:59 PM\")")]
            [TestCase(" FromOADate(43100.999305556) ")]
            [TestCase("ToTimeSpan(\"1:00:00\")")]
            [TestCase("CurrentUser.ToBaseInfo()")]
            [TestCase("List(\"Apple\", \"Orange\", \"Banana\")")]
                        
            public void Misc_IsSuccessful(string input)
            {
                Assert.AreEqual(GetParsingErrors(input), 0);
            }
        }
    }
}
