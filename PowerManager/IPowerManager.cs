using PowerManager.Structures;
using System.Runtime.InteropServices;

namespace PowerManager
{
    [ComVisible(true)]
    [Guid("EC3488A6-380A-49AF-94EC-05A51B9B6B10")]
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