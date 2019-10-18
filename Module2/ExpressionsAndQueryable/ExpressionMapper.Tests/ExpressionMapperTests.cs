using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionMapper.Test
{
    [TestClass]
    public class ExpressionMapperTests
    {
        public class Source
        {
            public int A { get; set; }
            public string B { get; set; }
            public string C { get; set; }

            public override string ToString()
            {
                return $"Source  A: {A}, B: {B}, C: {C}";
            }
        }
        public class Target
        {
            public int A { get; set; }
            public string B { get; set; }
            public string D { get; set; }

            public override string ToString()
            {
                return $"Target  A: {A}, B: {B}, D: {D}";
            }
        }

        [TestMethod]
        public void ExpressionMapperMapFooToBar()
        {
            var mapGenerator = new ExpressionMapper<Target>();
            var source = new Source()
            {
                A = 5,
                B = "text1",
                C = "text2"
            };
            
            var result = mapGenerator.Map(source);

            System.Console.WriteLine(source);
            System.Console.WriteLine(result);

            Assert.AreEqual(source.A, result.A);
            Assert.AreEqual(source.B, result.B);
            Assert.IsNull(result.D);
        }
    }
}
