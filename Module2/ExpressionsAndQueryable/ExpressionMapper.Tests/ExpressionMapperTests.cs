using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionMapper.Test
{
    [TestClass]
    public class ExpressionMapperTests
    {
        public class Foo
        {
            public int a { get; set; }
            public string b { get; set; }
            public string c { get; set; }

            public override string ToString()
            {
                return $"Class Foo  A: {a}, B: {b}, C: {c}";
            }
        }
        public class Bar
        {
            public int a { get; set; }
            public string b { get; set; }
            public string d { get; set; }

            public override string ToString()
            {
                return $"Class Bar  A: {a}, B: {b}, D: {d}";
            }
        }

        [TestMethod]
        public void ExpressionMapperMapFooToBar()
        {
            var mapGenerator = new ExpressionMapper<Bar>();
            var source = new Foo()
            {
                a = 5,
                b = "text",
                c = "test1"
            };
            
            var result = mapGenerator.Map(source);

            System.Console.WriteLine(source);
            System.Console.WriteLine(result);

            Assert.AreEqual(source.a, result.a);
            Assert.AreEqual(source.b, result.b);
            Assert.IsNull(result.d);
        }
    }
}
