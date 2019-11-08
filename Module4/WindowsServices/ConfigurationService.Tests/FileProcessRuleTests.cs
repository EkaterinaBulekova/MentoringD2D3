using CofigurationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConfigurationService.Tests
{
    [TestClass]
    public class FileProcessRuleTests
    {
        [TestMethod]
        public void TestGetIndexReturnTheCorrectIndex()
        {
            FileProcessRule rule = new FileProcessRule() { Prefix = "image", NumberLenght = 5, Delimeter = "_", Extentions = "jpg", Timeout = 1000};
            var result1 = rule.GetIndex("image_25.jpg");
            var result2 = rule.GetIndex("image_005.jpg");

            Assert.AreEqual(25, result1);
            Assert.AreEqual(5, result2);
        }

        [TestMethod]
        public void TestGetIndexReturnTheInitialIndex()
        {
            FileProcessRule rule = new FileProcessRule() { Prefix = "image", NumberLenght = 5, Delimeter = "_", Extentions = "jpg", Timeout = 1000 };
            var result = rule.GetIndex("image_as.jpg");

            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void TestShouldProcessReturnTrue()
        {
            FileProcessRule rule = new FileProcessRule() { Prefix = "image", NumberLenght = 5, Delimeter = "_", Extentions = "jpg", Timeout = 1000 };
            var result1 = rule.ShouldProcess("image_25.jpg");
            var result2 = rule.ShouldProcess("image_005.jpg");

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
        }

        [TestMethod]
        public void TestShouldProcessReturnFalse()
        {
            FileProcessRule rule = new FileProcessRule() { Prefix = "image", NumberLenght = 5, Delimeter = "_", Extentions = "jpg", Timeout = 1000 };
            var result1 = rule.ShouldProcess("image_25.png");
            var result2 = rule.ShouldProcess("image_.jpg");

            Assert.AreEqual(false, result1);
            Assert.AreEqual(false, result2);
        }
    }
}
