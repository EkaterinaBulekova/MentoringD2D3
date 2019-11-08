using CofigurationService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ConfigurationService.Tests
{
    [TestClass]
    public class ConfiguratorTests
    {
        [TestMethod]
        public void TestIsValidTrueIfValidConfiguration()
        {
            var mockOpt = new Mock<IOptions<WorkerSettings>>();
            mockOpt.Setup(p => p.Value).Returns( new WorkerSettings()
            {
                Directories = new Directories() { TargetDirectory = "target", WatchedDirectories = new string[] { "dir1", "dir2" }, WatchTimeout = 10000 },
                FileProcessRule = new FileProcessRule() { Prefix = "image", NumberLenght = 5, Delimeter = "_", Extentions = "jpg", Timeout = 1000 }
            });
            var mockLog = new Mock<ILogger<Configurator>>();
            IConfigurator config = new Configurator(mockOpt.Object, mockLog.Object);
            var result = config.IsValid();
            
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(CannotCreateConfiguratorException))]
        public void TestCannotCreateNotValidConfiguration()
        {
            var mockOpt = new Mock<IOptions<WorkerSettings>>();
            mockOpt.Setup(p => p.Value).Returns(new WorkerSettings()
            {
                Directories = new Directories() { TargetDirectory = "", WatchedDirectories = new string[] { "dir1", "dir2" }, WatchTimeout = 10000 },
                FileProcessRule = new FileProcessRule() { Prefix = "image", NumberLenght = 5, Delimeter = "_", Extentions = "jpg", Timeout = 1000 }
            });
            var mockLog = new Mock<ILogger<Configurator>>();
            IConfigurator config = new Configurator(mockOpt.Object, mockLog.Object);
        }
    }
}
