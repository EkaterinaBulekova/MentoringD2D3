using System;
using System.Runtime.InteropServices;

namespace PowerManager.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEM_POWER_INFORMATION
    {
        public long MaxIdlenessAllowed;
        public long Idleness;
        public long TimeRemaining;
        public Byte CoolingMode;
    }
}
