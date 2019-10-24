using System;
using System.Runtime.InteropServices;

namespace PowerManager.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESSOR_POWER_INFORMATION
    {
        public long Number;
        public long MaxMhz;
        public long CurrentMhz;
        public long MhzLimit;
        public long MaxIdleState;
        public long CurrentIdleState;
    }
}
