using NUnit.Framework;

using System;

namespace KSharpParserTests.Visitor
{
    class LoopTests
    {
        [TestFixture]
        public class ForTests : KSharpTestBase
        {
            [TestCase("for (i=0; i<=5 ; i++) {i}", new object[] { 0, 1, 2, 3, 4, 5 })]
            [TestCase("for (i=0; i<=5 ; i++) {i;i;}", new object[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 })]
            [TestCase("i = 3; for (i; i<=5 ; i++) {i}", new object[] { 3, 4, 5 })]
            [TestCase("z = 0; for (i = 0; i < 5; i++) { z += 1 }; z", new object[] { 5 })]
            [TestCase("for (i = 1; i <= 3; i++) { print(i); }", new object[] { "123" })]
            [TestCase("for (i = 1; i <= 3; i++) { return i; }", new object[] { 1 })]
            [TestCase("for (i = 1; i <= 3; i++) { i; }", new object[] { 1, 2, 3 })]
            [TestCase("4; for (i = 1; i <= 3; i++) { i; }", new object[] { 4, 1, 2, 3 })]
            [TestCase("4; for (i = 1; i <= 3; i++) { i; } 5;", new object[] { 4, 1, 2, 3, 5 })]
            public void For_IsSuccessful_HasResult(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetResultList(tree));
            }


            [TestCase("z = 0; for (i = 0; i < 5; i++) { z += 1 } ")]
            public void For_IsSuccessful_NoResult(string input)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.IsNull(Visitor.GetResultList(tree));
            }
        }


        [TestFixture]
        public class WhileTests : KSharpTestBase
        {
            [TestCase("z = 1; while (z<10) {++z}; z", new object[] { 10 })]
            [TestCase("i = 1; while (i < 4) {print(i);i++}; \"string\"", new object[] { "123string" })]
            [TestCase("i = 1; while (i < 4) {print(i);i++;}; return \"result\"", new object[] { "123result" })]
            [TestCase("x = 0; while (x < 5) { x++; }; x", new object[] { 5 })]
            public void While_IsSuccessful(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetResultList(tree));
            }


            [TestCase("z = 0; while (z < 10) {if (z > 4) {break}; ++z}")]
            [TestCase("z = 1; while (z<10) {++z} ")]
            public void While_IsSuccessful_NoResult(string input)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.IsNull(Visitor.GetResultList(tree));
            }


            [TestCase("cond = \"wrong type\"; while (cond) { x++ }")]
            public void While_NotSuccessful_ThrowsInvalidCast(string input)
            {
                Assert.Throws<InvalidCastException>(() => Visitor.GetFirstResult(GetParser(input).begin_expression()));
            }


            [TestCase("x = 5; cond = null; while (cond) { x = 10; }; x")]
            [TestCase("while (x) { x += 1; }")]
            [TestCase("while (x.y) print(x.y)")]
            [TestCase("while (if (z<10) {a++}) {++z} ")]
            [TestCase("while (z<10) {++z} ")]
            [TestCase("y = while (z<10) {++z} ")]
            public void While_NotSuccessful_ThrowsNullReference(string input)
            {
                Assert.Throws<NullReferenceException>(() => Visitor.GetFirstResult(GetParser(input).begin_expression()));
            }
        }


        [TestFixture]
        public class ForeachTests : KSharpTestBase
        {
            [TestCase("y = List(1,2,3,4,4,5,4); foreach (x in y) {x}", new object[]{ 1, 2, 3, 4, 4, 5, 4 })]      
            [TestCase("foreach (x in \"hello\") {x.toupper()}", new object[] { "H","E","L","L", "O" })]
            [TestCase("array = List(1,2); foreach (i in array) { foreach (j in array) { j } }", new object[] { 1,2,1,2 })]
            public void Foreach_IsSuccessful(string input, object expected)
            {
                var tree = GetParser(input).begin_expression();

                Assert.IsNull(tree.exception);
                Assert.AreEqual(expected, Visitor.GetResultList(tree));
            }
        }
    }
}
