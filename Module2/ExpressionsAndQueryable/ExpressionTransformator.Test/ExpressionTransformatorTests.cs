using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTransformator.Test
{
    [TestClass]
    public class ExpressionTransformatorTests
    {
        [TestMethod]
        public void VisitWithoutAnyTransformation()
        {
            var transformator = new ExpressionTransformator();
            Expression<Func<int, int, int>> replace = (a, b) => (b + 1) * a + 5 - (3 * a + 1 - 1);

            var parametrs = new Dictionary<string, int>
            {
                { "a", 5 },
                { "b", 4 }
            };

            Expression<Func<int, int, int>> replaced =
                (Expression<Func<int, int, int>>)transformator.Visit(replace, parametrs);
            Console.WriteLine($"before transform: {replace}");
            Console.WriteLine($"result: {replace.Compile().Invoke(3, 3)}");
            Console.WriteLine($"after transform: {replaced}");
            Console.WriteLine($"result: {replaced.Compile().Invoke(3, 3)}");
        }
        [TestMethod]
        public void VisitWithIncrementDecrementTransformation()
        {
            var transformator = new ExpressionTransformator(TrancformationOptions.IncrementDecrement);
            Expression<Func<int, int, int>> replace = (a, b) => (b + 1) * a + 5 - (3 * a + 1 - 1);

            var parametrs = new Dictionary<string, int>
            {
                { "a", 5 },
                { "b", 4 }
            };

            Expression<Func<int, int, int>> replaced =
                (Expression<Func<int, int, int>>)transformator.Visit(replace, parametrs);
            Console.WriteLine($"before transform: {replace}");
            Console.WriteLine($"result: {replace.Compile().Invoke(3, 3)}");
            Console.WriteLine($"after transform: {replaced}");
            Console.WriteLine($"result: {replaced.Compile().Invoke(3, 3)}");
        }
        [TestMethod]
        public void VisitWithReplaceParametrsTransformation()
        {
            var transformator = new ExpressionTransformator(TrancformationOptions.ReplaceParametrs);
            Expression<Func<int, int, int>> replace = (a, b) => (b + 1) * a + 5 - (3 * a + 1 - 1);

            var parametrs = new Dictionary<string, int>
            {
                { "a", 5 },
                { "b", 4 }
            };

            Expression<Func<int, int, int>> replaced =
                (Expression<Func<int, int, int>>)transformator.Visit(replace, parametrs);
            Console.WriteLine($"before transform: {replace}");
            Console.WriteLine($"result: {replace.Compile().Invoke(3, 3)}");
            Console.WriteLine($"after transform: {replaced}");
            Console.WriteLine($"result: {replaced.Compile().Invoke(3, 3)}");
        }
        [TestMethod]
        public void VisitWithIncrementDecrementAndReplaceParametrsTransformation()
        {
            var transformator = new ExpressionTransformator(TrancformationOptions.IncrementDecrement | TrancformationOptions.ReplaceParametrs);
            Expression<Func<int, int, int>> replace = (a, b) => (b + 1) * a + 5 - (3 * a + 1 - 1);

            var parametrs = new Dictionary<string, int>
            {
                { "a", 5 },
                { "b", 4 }
            };

            Expression<Func<int, int, int>> replaced =
                (Expression<Func<int, int, int>>)transformator.Visit(replace, parametrs);
            Console.WriteLine($"before transform: {replace}");
            Console.WriteLine($"result: {replace.Compile().Invoke(3, 3)}");
            Console.WriteLine($"after transform: {replaced}");
            Console.WriteLine($"result: {replaced.Compile().Invoke(3, 3)}");
        }

    }
}
