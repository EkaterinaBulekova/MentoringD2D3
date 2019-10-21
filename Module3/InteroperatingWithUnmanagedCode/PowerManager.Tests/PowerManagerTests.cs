using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PowerManager.Tests
{
    [TestClass]
    public class PowerManagerTests
    {
        const uint STATUS_SUCCESS = 0;

        [TestMethod]
        public void GetLastSleepTimeTest()
        {
            var powermanager = new PowerManager();
            Console.WriteLine(powermanager.GetLastSleepTime());
        }

        [TestMethod]
        public void GetLastWakeTimeTest()
        {
            var powermanager = new PowerManager();
            Console.WriteLine(powermanager.GetLastWakeTime());
        }

        [TestMethod]
        public void GetSystemBatteryStateTest()
        {
            var powermanager = new PowerManager();
            Console.WriteLine(powermanager.GetSystemBatteryState().RemainingCapacity);
        }

        [TestMethod]
        public void GetSystemPowerInformationTest()
        {
            var powermanager = new PowerManager();
            Console.WriteLine(powermanager.GetSystemPowerInformation().TimeRemaining);
        }

        [TestMethod]
        public void ReserveHibernationFileTest()
        {
            var powermanager = new PowerManager();
            Assert.AreEqual(STATUS_SUCCESS, powermanager.ReserveHibernationFile());
        }

        [TestMethod]
        public void DeleteHibernationFileTest()
        {
            var powermanager = new PowerManager();

            Assert.AreEqual(STATUS_SUCCESS, powermanager.DeleteHibernationFile());
        }

        [TestMethod]
        public void HibernateTest()
        {
            var powermanager = new PowerManager();
            Assert.AreEqual(STATUS_SUCCESS, powermanager.Hibernate());
        }

        [TestMethod]
        public void SleepTest()
        {
            var powermanager = new PowerManager();
            Assert.AreEqual(STATUS_SUCCESS, powermanager.Sleep());
        }
    }
}
