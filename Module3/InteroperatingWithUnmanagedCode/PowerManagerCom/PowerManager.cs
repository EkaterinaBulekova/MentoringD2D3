using PowerManager.Structures;
using System;
using System.Runtime.InteropServices;

namespace PowerManager
{
    [ComVisible(true)]
    [Guid("490DC193-1CF9-491A-A6D6-B3EEC2AD2573")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("PowerManagerCom.PowerManager")]
    public class PowerManager : IPowerManager
    {
        const long STATUS_SUCCESS = 0;

        [DllImport("PowrProf.dll", EntryPoint = "CallNtPowerInformation", ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint CallNtPowerInformation(
                        POWER_INFORMATION_LEVEL informationLevel,
                        [In]IntPtr lpInputBuffer,
                        uint nInputBufferSize,
                        [In, Out]IntPtr lpOutputBuffer,
                        uint nOutputBufferSize);

        [DllImport("Powrprof.dll", SetLastError = true)]
        static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        private T GetPowerInformation<T>(POWER_INFORMATION_LEVEL infoLevel)
        {
            var resultType = typeof(T);
            IntPtr result = Marshal.AllocCoTaskMem(Marshal.SizeOf(resultType));
            try
            {
                long retval = CallNtPowerInformation(
                    infoLevel,
                    IntPtr.Zero,
                    0,
                    result,
                    (uint)Marshal.SizeOf(resultType));
                if (retval == STATUS_SUCCESS)
                {
                    var value = Marshal.PtrToStructure<T>(result);
                    return value;
                }
                throw new Exception("some problem!");
            }
            finally
            {
                if (result != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(result);
            }
        }

        private uint ManageHibernationFile(bool reserve)
        {
            IntPtr _reserve = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(bool)));
            Marshal.StructureToPtr(reserve, _reserve, false);
            try
            {
                uint retval = CallNtPowerInformation(
                    POWER_INFORMATION_LEVEL.SystemReserveHiberFile,
                    _reserve,
                    (uint)Marshal.SizeOf(typeof(bool)),
                    IntPtr.Zero,
                    0);
                return retval;
            }
            finally
            {
                if (_reserve != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(_reserve);
            }
        }

        public long GetLastSleepTime()
        {
            long lastSleepTimeInSeconds = GetPowerInformation<long>(POWER_INFORMATION_LEVEL.LastSleepTime) / 10000000;

            return lastSleepTimeInSeconds;
        }

        public long GetLastWakeTime()
        {
            long lastWakeTimeInSeconds = GetPowerInformation<long>(POWER_INFORMATION_LEVEL.LastWakeTime) / 10000000;

            return lastWakeTimeInSeconds;
        }

        public SYSTEM_BATTERY_STATE GetSystemBatteryState()
        {
            return GetPowerInformation<SYSTEM_BATTERY_STATE>(POWER_INFORMATION_LEVEL.SystemBatteryState);
        }

        public SYSTEM_POWER_INFORMATION GetSystemPowerInformation()
        {
            return GetPowerInformation<SYSTEM_POWER_INFORMATION>(POWER_INFORMATION_LEVEL.SystemPowerInformation);
        }
        public uint ReserveHibernationFile()
        {
            return ManageHibernationFile(true);
        }

        public uint DeleteHibernationFile()
        {
            return ManageHibernationFile(false);
        }
        public bool Hibernate()
        {
            return SetSuspendState(true, false, false);
        }

        public bool Sleep()
        {
            return SetSuspendState(false, false, false);
        }
    }
}
