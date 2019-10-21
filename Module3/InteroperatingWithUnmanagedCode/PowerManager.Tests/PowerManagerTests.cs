using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerManager.Structures;
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

            var result = powermanager.GetLastSleepTime();
            Console.WriteLine(result);

            Assert.IsInstanceOfType(result, typeof(long));
            Assert.AreNotEqual(0, result);
        }

        [TestMethod]
        public void GetLastWakeTimeTest()
        {
            var powermanager = new PowerManager();

            var result = powermanager.GetLastWakeTime();
            Console.WriteLine(result);

            Assert.IsInstanceOfType(result, typeof(long));
            Assert.AreNotEqual(0, result);
        }

        [TestMethod]
        public void GetSystemBatteryStateTest()
        {
            var powermanager = new PowerManager();

            var result = powermanager.GetSystemBatteryState();
            Console.WriteLine(result.RemainingCapacity);

            Assert.IsInstanceOfType(result, typeof(SYSTEM_BATTERY_STATE));
            Assert.AreNotEqual(0, result.RemainingCapacity);
        }

        [TestMethod]
        public void GetSystemPowerInformationTest()
        {
            var powermanager = new PowerManager();

            var result = powermanager.GetSystemPowerInformation();
            Console.WriteLine(result.TimeRemaining);

            Assert.IsInstanceOfType(result, typeof(SYSTEM_POWER_INFORMATION));
            Assert.AreNotEqual(0, result.TimeRemaining);
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

            Assert.IsTrue(powermanager.Hibernate());
        }

        [TestMethod]
        public void SleepTest()
        {
            var powermanager = new PowerManager();

            Assert.IsTrue(powermanager.Sleep());
        }
    }
}
