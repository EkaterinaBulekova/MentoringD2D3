using PowerManager.Structures;

namespace PowerManager
{
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