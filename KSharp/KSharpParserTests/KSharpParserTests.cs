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

            ITree tree = parser.begin_expression();

            return parser.NumberOfSyntaxErrors;
        }

        #endregion

        [TestFixture]
        public class BasicStructuresTests
        {
            [TestCase("Class.Member")]
            [TestCase("Class.Mehod()")]
            [TestCase("Variable")]

            [TestCase("true")]
            [TestCase("false")]

            [TestCase("1")]
            [TestCase("98746311")]

            [TestCase("-1")]
            [TestCase("-98746311")]

            [TestCase("3.465e-5")]
            [TestCase("3.465e+5")]
            [TestCase("3.465")]
            [TestCase("3,465")]

            [TestCase("identifier")]
            [TestCase("指")]

            [TestCase("30%")]
                        
            [TestCase(";")]
            [TestCase(";;;")]

            [TestCase("\"String\"")]

            // invalid identifier, it is ignored when found
            [TestCase("2identifier")]
            public void BasicStructures_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }


            [TestCase(")", Description = "Unexpected token")]
            [TestCase("()", Description = "No content in the brackets")]
            [TestCase("1 |+} 2 ", Description = "Invalid operator")]
            public void BasicStructures_NotSuccessful(string input)
            {
                Assert.AreNotEqual(0, GetParsingErrors(input));
            }
        }


        [TestFixture]
        public class LogicalTests
        {
            [TestCase("1 < 2")]
            
            [TestCase("true || true")]
            [TestCase("false || false")]
            [TestCase("false || true")]
            [TestCase("true || false")]
            [TestCase("true && true")]
            [TestCase("false && false")]
            [TestCase("false && true")]
            [TestCase("true && false")]

            [TestCase("null == 0 || null == null")]
            [TestCase("0 == 0 || 0 == null")]
            [TestCase("1 == 0 || 1 == null")]

            [TestCase(@"1==1 && ""a""==""a""")]
            [TestCase(@"1==1 and ""a""==""a""")]
            [TestCase(@"1==0 && 2==2")]
            [TestCase(@"1==0 and 2==2")]
            [TestCase(@"1==1 || ""a""==""a""")]
            [TestCase(@"1==0 or ""a""==""a""")]
            [TestCase(@"1==0 || 1==2")]
            [TestCase(@"1==0 or ""a""==""b""")]

            [TestCase("0 ? 1 : 0")]
            [TestCase("1 ? 1 : 0")]
            [TestCase("true ? \"yes\" : \"no\"")]
            [TestCase("false ? \"yes\" : \"no\"")]
            public void Logical_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }
        }


        [TestFixture]
        public class BracesTests
        {
            [TestCase("{1 < 2}")]
            [TestCase("if (!Condition) {{ return Value }}")]
            public void Braces_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }
        }

               
        [TestFixture]
        public class AssignmentTests
        {
            [TestCase("Variable = 1")]
            [TestCase("Variable = \"String\"")]
            public void Assignment_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }


            [TestCase("5 = 7 + 2")]
            [TestCase("\"FinalString\" = \"String\" + \"AnotherString\"")]
            public void Assignment_NotSuccessful(string input)
            {
                Assert.AreNotEqual(0, GetParsingErrors(input));
            }
        }

      
        [TestFixture]
        public class ArithmeticTests
        {
            [TestCase("1 + 2")]
            [TestCase("5 mod 2")]
            [TestCase("SubVariable + AnotherSubVariable")]

            [TestCase("Variable = 1; Variable += 2")]
            [TestCase("Variable = SubVariable + AnotherSubVariable")]

            [TestCase("2.3e-2 + 5 * 2")]
            [TestCase("2.3e-2 + 5 * 2 == 2 * 5 + 2.3e-2")]
            [TestCase("9 + 2 - 2 - 2")]
            [TestCase("9 + 2 + 2 - 2")]
            [TestCase("9 + 2 + 2 + 2")]
            [TestCase("9 - 2 + 2 + 2")]
            [TestCase("9 - 2 - 2 + 2")]
            [TestCase("9 - 2 - 2 - 2")]
            [TestCase("9 + 2 * 2 - 2")]
            [TestCase("9 - 2 + 2 * 2")]
            [TestCase("9 + ((-2) + 2 * 2)")]
            [TestCase("9 + (-2 + 2 * 2)")]
            [TestCase("-5 * 5 / 10 + 1")]
            [TestCase("-5 * -5 / 10 + 1")]
            [TestCase("9 + (2 + (-2 * 2))")]
            [TestCase("9 - 2 + 2 * 3")]
            [TestCase("9 - 2 * 2 + 2")]
            [TestCase("1 << 8 >> 8 << 2")]
            [TestCase("1 << (8 >> (8 << 2))")]
            [TestCase("5 - 2 * 4 - 3 + 2")]
            public void Arithmetic_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }
        }


        [TestFixture]
        public class DoubleTests
        {
            [TestCase("\"\" + ToDouble(\"2.45\", 0, \"en-us\")")]
            [TestCase("\"\" + ToDouble(\"2,45\", 0, \"cs-cz\")")]
            [TestCase("\"\" + ToDouble(\"2,45\")")]
            public void Double_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }
        }


        [TestFixture]
        public class StringTests
        {
            [TestCase("")]
            [TestCase(" ")]

            [TestCase("\"String\" + \"AnotherString\"")]
            [TestCase("\"String\" - \"AnotherString\"")]
            
            [TestCase("\"String\" \"AnotherString\"")]

            [TestCase("Variable = \"String\" + \"AnotherString\"")]

            [TestCase("\"\" + \"ahoj\"")]
            [TestCase("\"\" + 23.05")]
            [TestCase("\"\" + False")]

            [TestCase("\"\"")]
            [TestCase("\"\\\"\"")]
            [TestCase("@\"\"")]

            [TestCase("@\"This string displays as is. No newlines\n, tabs\t or backslash-escapes\\.\"")]
            public void String_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }
                     
             
            [TestCase("@\"\"\"")]
            [TestCase("@\"\"\"\"\"")]
            public void String_NotSuccessful(string input)
            {
                Assert.AreNotEqual(0, GetParsingErrors(input));
            }
        }

        
        [TestFixture]
        public class IfTests
        {
            [TestCase("if (Condition) { Value; }")]

            [TestCase("if (Condition) { return Value }")]        
            [TestCase("if (Condition) { return Value } else { return AnotherValue }")]

            [TestCase("if (!Condition) { return Value }")]

            [TestCase("If (1==2) {\"A\"} Else {\"B\"}")]
            [TestCase("if (1==2) {\"A\"} Else {\"B\"}")]

            [TestCase("cond = \"should go through\"; if (cond) { 1 } else { 0 }")]
            [TestCase("cond = null; if (cond) { 1 } else { 0 }")]

            [TestCase("if (15) { 1 } else { 0 }")]
            [TestCase("if (-15) { 1 } else { 0 }")]
            [TestCase("if (0) { 1 } else { 0 }")]
            [TestCase("If (1==2) {\"A\"} else {\"B\"}")]
            public void If_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }


            [TestCase("if Condition")]
            [TestCase("if Condition return Value")]
            [TestCase("if Condition { return Value }")]

            [TestCase("if (Condition) Value")]
            [TestCase("if (Condition) return Value")]
            [TestCase("if (Condition) { Value } else AnotherValue")]
            [TestCase("if (Condition) else { return Value }")]
            [TestCase("if (Condition) then { return Value } else { return Value }")]

            [TestCase("if () {return Value}")]
            [TestCase("if (if (1 > 2){return 1}) {return Value}")]
            public void If_NotSuccessful(string input)
            {
                Assert.AreNotEqual(0, GetParsingErrors(input));
            }
        }
        

        [TestFixture]
        public class DateTimeTests
        {
            [TestCase("\"\" + ToDateTime(\"10.5.2010\", CurrentDateTime, \"cs-cz\")")]
            [TestCase("\"\" + ToDateTime(\"5/10/2010\", CurrentDateTime, \"en-us\")")]
            [TestCase("\"\" + ToDateTime(\"10.5.2010\")")]

            [TestCase("ToDateTime(\"12/31/2017 11:59 PM\")")]
            [TestCase("FromOADate(43100.999305556) ")]
            [TestCase("ToTimeSpan(\"1:00:00\")")]
            public void DateTime_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }
        }


        [TestFixture]
        public class ReturnTests
        {
            [TestCase("return \"output\"")]

            [TestCase("return 2 * 3 - (2 + 1)")]
            [TestCase("return 2 + 3 * (1 + 2)")]

            [TestCase("x = 0; for (i = 0; i < 10; i++) { x += i; if (i == 5) { return x; } }")]

            [TestCase("print(\"should not be output\"); return \"output\"")]
            [TestCase("return \"first\"; return \"second\"")]

            [TestCase("x = 0; for (i = 0; i < 10; i++) { print(i); if (i == 5) { return; } }")]
            [TestCase("x = 0; for (i = 0; i < 10; i++) { print(i); if (i == 5) { return unresolved; } }")]
            [TestCase("x = 0; for (i = 0; i < 10; i++) { if (i == 5) { return; } }")]            
            public void Return_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }
        }


        [TestFixture]
        public class LambdaTests
        {
            [TestCase("lambdaSucc = (x => x + 1); lambdaSucc(3)")]
            [TestCase("lambdaMultiply = ((x, y) => x * y); lambdaMultiply(2,3)")]

            [TestCase("(x,y) => x*func(y)")]

            [TestCase("lambda = x => x * 2; lambda(lambda(5))")]
            [TestCase("lambda = (x, y, z) => x * y + z; lambda(5, 4, 3)")]
            public void Lambda_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }
        }


        [TestFixture]
        public class ForTests
        {
            [TestCase("for (i=0; i<=5 ; i++) {i}")]

            [TestCase("for (i=0; i<=5 ; i++) {i;i;}")]

            [TestCase("i = 3; for (i; i<=5 ; i++) {i}")]

            [TestCase("z = 0; for (i = 0; i < 5; i++) { z += 1 }; z")]
            [TestCase("z = 0; for (i = 0; i < 5; i++) { z += 1 } ")]

            [TestCase("for (i = 1; i <= 3; i++) { print(i); }")]
            [TestCase("for (i = 1; i <= 3; i++) { return i; }")]
            [TestCase("for (i = 1; i <= 3; i++) { i; }")]
            [TestCase("4; for (i = 1; i <= 3; i++) { i; }")]
            [TestCase("4; for (i = 1; i <= 3; i++) { i; } 5;")]
            public void For_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }


            [TestCase("for (i = 0; if (z<10) {a++}; i++) { z += 1 };")]
            [TestCase("for ()")]
            [TestCase("for (x = 10) { x }")]
            [TestCase("for (x = 1; x < 10; x++)")]
            [TestCase("for (x = 1; x < 10) { x }")]

            [TestCase("for (x = 1; if (1>2){return Value;}; x++){return null;}")]
            
            [TestCase("y = for (i = 1; i <= 3; i++) { i; }")]
            public void For_NotSuccessful(string input)
            {
                Assert.AreNotEqual(0, GetParsingErrors(input));
            }
        }

        
        [TestFixture]
        public class ForeachTests
        {
            [TestCase("foreach (x in y) {x.toupper()}")]

            [TestCase("foreach (x in \"hello\") {z += x.toupper()}")]
            [TestCase("z = \"\"; foreach (x in \"hello\") {z += x.toupper()}")]
            [TestCase("z = \"\"; foreach (x in \"hello\") {z += x.toupper()}; z")]

            [TestCase("foreach (i in array) { print(i); }")]
            [TestCase("foreach (i in array) { return i; }")]
            [TestCase("foreach (i in array) { i; }")]
            [TestCase("foreach (i in array) { foreach (j in array) { j } }")]

            [TestCase("foreach (x in \"output\") { print(x) }")]
            [TestCase("foreach (x in \"output\") { x }")]
            public void Foreach_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }


            [TestCase("foreach (x.y in \"output\") { print(x.y) }")]
            [TestCase("foreach (x in \"output\") print(x)")]

            [TestCase("y = foreach (x in \"output\") { print(x) }")]
            
            [TestCase("foreach (1+1 in 2+2) { x }")]
            [TestCase("foreach (i in 2+2) { x }")]
            public void Foreach_NotSuccessful(string input)
            {
                Assert.AreNotEqual(0, GetParsingErrors(input));
            }
        }

        
        [TestFixture]
        public class WhileTests
        {
            [TestCase("z = 1; while (z<10) {++z}; z")]
            [TestCase("z = 1; while (z<10) {++z} ")]

            [TestCase("while (z<10) {++z} ")]

            [TestCase("z = 0; while (z < 10) {if (z > 4) {break}; ++z}")]

            [TestCase("i = 1; while (i < 4) {print(i++)}; \"ignored\"")]
            [TestCase("i = 1; while (i < 4) {print(i++)}; return \"result\"")]
            [TestCase("x = 0; while (x < 5) { x++; }; x")]
            [TestCase("x = 0; while (x < 5) { x++ }")]
            [TestCase("cond = \"wrong type\"; while (cond) { x++ }")]
            [TestCase("x = 5; cond = null; while (cond) { x = 10; }; x")]

            [TestCase("while (true) { x += 1; }")]
            public void While_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }


            [TestCase("while (true) print(x.y)")]
            [TestCase("while (if (z<10) {a++}) {++z} ")]
            
            [TestCase("y = while (z<10) {++z} ")]
            public void While_NotSuccessful(string input)
            {
                Assert.AreNotEqual(0, GetParsingErrors(input));
            }
        }


        [TestFixture]
        public class IndexerTests
        {
            [TestCase("GlobalObjects.Users[\"administrator\"].FullName")]
            [TestCase("CurrentDocument == Documents[\"/Services/WebDesign\"]")]
            [TestCase("CurrentUser.Children[\"cms_category\"][0].CategoryName")]
            public void Indexer_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }
        }


        [TestFixture]
        public class CommentTests
        {
            [TestCase("// This is a one-line comment. Initiated by two forward slashes, spans across one full line.")]
            [TestCase(@"/* This is a multi - line comment.
        Opened by a forward slash - asterisk sequence and closed with the asterisk - forward slash sequence.
        Can span across any number of lines.
        */")]
            [TestCase("x = 5; y = 3; /* This is an inline comment nested in the middle of an expression. */ x+= 2; x + y")]

            [TestCase("// c\nx=\"5\"")]
            [TestCase("// c\r\nx=\"5\"")]

            [TestCase("object.method(param1, /* comment */ ++param2, param3--, inner())")]
            public void Comment_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }
        }


        [TestFixture]
        public class MiscTests { 

            [TestCase("foreach (page in CurrentDocument.CultureVersions) {if (page.DocumentCulture != CurrentDocument.DocumentCulture) {\"<link rel=\"alternate\" href=\"\"+ page.AbsoluteUrl + \"\" hreflang=\"\"+ page.DocumentCulture +\"\"/>\";}}")]
            
            // Todo: categorize
            [TestCase("\"string\" + 5")]
            [TestCase("ResolveBBCode(\"[quote]Sample text[/ quote]\")")]
            [TestCase("FormatPrice(GetSKUTax(SKUID), false)")]

            [TestCase("CurrentPageInfo.DocumentPageTitle + \" | suffix\"")]
            [TestCase("CurrentDocument.Children.FirstItem ?? \"No child pages\"")]
            [TestCase("50 == 5*10")]
            [TestCase("CurrentUser.UserName != \"administrator\"")]
            [TestCase("CurrentPageInfo.DocumentPublishFrom <= DateTime.Now")]            
            

            [TestCase("Replace(\"The sky is blue on blue planets\", \"blue\", \"red\")")]
            [TestCase("\"The sky is blue on blue planets\".Replace(\"blue\", \"red\")")]

            [TestCase("\"word\".ToUpper()")]
            [TestCase("ToUpper(\"word\")")]

            [TestCase("for (i=0; i<=5 ; i++) {if (i == 3) {continue}; i}")]

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

            [TestCase("Cache(\"string\".ToUpper())")]
            [TestCase("Cache(CurrentUser.GetFormattedUserName(), 5, true, \"username|\" + CurrentUser.UserName, GetCacheDependency(\"cms.user|all\"))")]
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
            [TestCase("DocumentName.ToString( \"defaultValue\", DocumentCulture, \"Document name is: {0}\")")]
            [TestCase("DocumentID.ToInt()")]
            [TestCase("DocumentShowInSiteMap.ToBool()")]
            [TestCase("DocumentID.ToDecimal()")]
            [TestCase("DocumentID.ToDecimal(\"0.0\", DocumentCulture)")]
            [TestCase("DocumentID.ToDouble()")]
            [TestCase("DocumentID.ToDouble(\"0.0\", DocumentCulture)")]
            [TestCase("DocumentGUID.ToGuid()")]
            [TestCase("CurrentUser.ToBaseInfo()")]
            [TestCase("List(\"Apple\", \"Orange\", \"Banana\")")]
                        
            public void Misc_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }
        }


        [TestFixture]        
        public class PipeTests
        {
            [Ignore("Not going to do that here")]

            [TestCase("CurrentUser.UserDateOfBirth|(default)N|A")]
            [TestCase("ArticleSummary|(encode)true")]
            [TestCase("ArticleText|(encode)true|(recursive)true")]
            [TestCase("DocumentPublishFrom|(culture)en-us")]
            [TestCase("Contains(\"term\", ArticleText)|(casesensitive)true")]
            [TestCase("GetDocumentUrl()|(timeout)2000")]
            [TestCase("QueryString.Param|(handlesqlinjection)true")]
            [TestCase("Documents[\"/News\"].Children.WithAllData|(debug)true")]
            
            [TestCase("\"<br>\"|(encode)")]
            [TestCase("\"<br>\"|(encode)true")]

            [TestCase("\"resolved\"|(resolver)ExpectedResolverName")]
            [TestCase("\"resolved\"|(culture)en-gb")]
            [TestCase("\"resolved\"|(debug)true")]
            [TestCase("\"resolved\"|(debug)false")]
            [TestCase("\"resolved\"|(debug)")]
            [TestCase("\"resolved\"|(casesensitive)false")]
            [TestCase("\"resolved\"|(casesensitive)true")]
            [TestCase("\"resolved\"|(casesensitive)")]
            [TestCase("\"resolved\"|(encode)false")]
            [TestCase("\"resolved\"|(encode)true")]
            [TestCase("\"resolved\"|(encode)")]
            [TestCase("\"resolved\"|(timeout)123456")]
            public void Pipe_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }
        }


        [TestFixture]
        public class MathTests
        {
            [TestCase("Math.Abs(-1)")]
            [TestCase("Abs(-1)")]
            [TestCase("Math.PI")]
            [TestCase("PI")]
            public void Math_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }
        }

     
        [TestFixture]
        public class OpenExpressionsTests
        {
            [Ignore("Need to think about this")]
            [TestCase("for (i = 1; i <= 3; i++) { %}{% i %} {% }")]
            [TestCase("foreach (i in array) { %}{% i %} {% }")]
            [TestCase("foreach (i in array) { %}{% i %}-{% foreach (j in array) { %}{% j %}{% } %} {% }")]
            [TestCase("i = 4;return; %}{% x = ((i mod 2) == 0);return; %}{% x ? \"yes\" : \"no\"")]
            [TestCase("i = 3;return; %}{% x = ((i mod 2) == 0);return; %}{% x ? \"yes\" : \"no\"")]
            [TestCase("for ( i = 0; i < 2; i++) { %}{% x = (i mod 2) == 0; %}{% x ? \"yes\" : \"no\" %}{% }")]
            [TestCase("for ( i = 0; i < 2; i++) { %}{% x = false; x ? \"yes\" : \"no\" %}{% }")]
            [TestCase("for ( i = 0; i < 2; i++) { x = false; x ? \"yes\" : \"no\" }")]
            [TestCase("for ( i = 0; i < 2; i++) { %}{% x = (i mod 2) == 0; x ? \"yes\" : \"no\" %}testtext{% }")]
            [TestCase("for ( i = 0; i < 2; i++) {  %}{% i %}{% }")]
            
            [TestCase("if (true) { %}<br>{%}|(encode)false")]
            [TestCase("if (true) { %}{%\"<br>\"%}{%}|(encode)false")]
            [TestCase("if (true) { %}{%\"<br>\"|(encode)%}{%}|(encode)false")]
            [TestCase("if (true) { %}{%\"<br>\"|(encode)false%}{%}|(encode)true")]
            [TestCase("if (true) {%}<br>{%\"test\"%}test{%}|(encode)")]
            [TestCase("if (true) {%}<br>{%\"test\"%}test{%}|(encode)false%}{%\"<br>\"")]

            [TestCase("if (PrintThreadCulturesTest() == \"en-GB,en-GB\") { %}{% PrintThreadCUlturesTest()|(culture)ja-JP %}{% }")]
            public void OpenExpressions_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }
        }


        [TestFixture]
        public class UncategorizedTests
        {
            [TestCase(@"
Cache(a = true, 10, true, ""a"");
Cache(a = false, 10, true, ""a"");

return a
")]

            [TestCase(@"
System.Cache(b = true, 10, true, ""b"");
System.Cache(b = false, 10, true, ""b"");

return b
")]

            [TestCase(@"
(c = true).Cache(10, true, ""c"");
(c = false).Cache(10, true, ""c"");

return c
")]

            [TestCase("print(\"Console\");")]
            [TestCase("println(\"first\"); println(\"second\");")]
            [TestCase("print(\"Console priority\"); \"overriden\"")]
            [TestCase("print(\"Console\"); 2 + 3; return")]
            [TestCase("print(\"Console priority\") + \" works\"")]
            [TestCase("\"Simple string literal\"")]
            [TestCase("\"3.456\"")]
                        
            [TestCase("2.48 + 0.02-- + ++2.0e-1;")]
   
            [TestCase("x[y[z]]")]
            [TestCase("if (x++) { a } else { z }")]
            [TestCase("x mod y == 2 of 100")]
            [TestCase("23.ToString(true)")]
            [TestCase("ahoj23.ToString(false)")]
            [TestCase("x=-1")]
            [TestCase("x==-1")]

            [TestCase("1/5")]
            [TestCase("\"X\"")]
            [TestCase("\"X\"; \"Y\";")]
            [TestCase("\"X\"; { \"Y\"; }")]
            [TestCase("{ \"X\"; \"Y\"; }")]
            [TestCase("\"X\"; return \"Y\"; \"Z\"")]
            [TestCase("\"X\"; print(\"Y\"); \"Z\"; print(\"W\")")]
            [TestCase("i = 4; x = ((i mod 2) == 0); x ? \"yes\" : \"no\"")]
            [TestCase("\"<br>\"")]
            [TestCase(@"2++;3;-4 // comment
/* comment
 * multiline */
_id /* inline comment */ + _id2 + @""ahoj
jak """" \n se mas""")]

            [TestCase("x ? \"yes\" : \"no\"")]

            [TestCase("((i mod 2) == 0)")]
            public void Uncategorized_IsSuccessful(string input)
            {
                Assert.AreEqual(0, GetParsingErrors(input));
            }
        }
    }
}
