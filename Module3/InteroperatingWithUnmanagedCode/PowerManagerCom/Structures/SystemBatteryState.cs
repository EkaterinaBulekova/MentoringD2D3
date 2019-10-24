using System;
using System.Runtime.InteropServices;

namespace PowerManager.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEM_BATTERY_STATE
    {
        public Byte AcOnLine;
        public Byte BatteryPresent;
        public Byte Charging;
        public Byte Discharging;
        public Byte spare1;
        public Byte spare2;
        public Byte spare3;
        public Byte spare4;
        public long MaxCapacity;
        public long RemainingCapacity;
        public Int32 Rate;
        public long EstimatedTime;
        public long DefaultAlert1;
        public long DefaultAlert2;
    }
}
