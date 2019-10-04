using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace AsyncCalculator.Tests
{
    [TestClass]
    public class AsyncSumTests
    {       
        CancellationTokenSource tokenSource;
        AsyncSum asyncSum;


        [TestInitialize]
        public void Initialize()
        {
            tokenSource = new CancellationTokenSource();
            asyncSum = new AsyncSum();
        }

        [TestCleanup]
        public void Cleanup()
        {
            tokenSource.Dispose();
            asyncSum = null;
        }

        [TestMethod]
        public void TestCalculateSum()
        {
            var result10 = asyncSum.CalculateSum(10, tokenSource.Token);
            var result100 = asyncSum.CalculateSum(1000, tokenSource.Token); 

            Assert.AreEqual(55, result10);
            Assert.AreEqual(500500,result100);
        }

        [TestMethod]
        public void TestGetCalculationResult()
        {
            var result10 = asyncSum.GetCalculationResult(10, tokenSource.Token);
            var result1000 = asyncSum.GetCalculationResult(1000, tokenSource.Token);

            Assert.AreEqual("Number = 10 | Sum = 55", result10);
            Assert.AreEqual("Number = 1000 | Sum = 500500", result1000);
        }

        [TestMethod]
        [ExpectedException(typeof(System.OperationCanceledException))]
        public void TestGetCalculationResultWhenCancel()
        {
            tokenSource.Cancel();

            asyncSum.GetCalculationResult(1000000, tokenSource.Token);

            //assert - Expects exception
        }

    }
}
