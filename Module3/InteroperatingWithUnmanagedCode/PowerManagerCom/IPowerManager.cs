using PowerManager.Structures;
using System;
using System.Runtime.InteropServices;

namespace PowerManager
{
    [ComVisible(true)]
    [Guid("DB3653BE-CF41-407F-9FDC-58DA558325F8")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IPowerManager
    {
        uint DeleteHibernationFile();
        long GetLastSleepTime();
        long GetLastWakeTime();
        SYSTEM_BATTERY_STATE GetSystemBatteryState();
        SYSTEM_POWER_INFORMATION GetSystemPowerInformation();
        bool Hibernate();
        uint ReserveHibernationFile();
        bool Sleep();
    }
}