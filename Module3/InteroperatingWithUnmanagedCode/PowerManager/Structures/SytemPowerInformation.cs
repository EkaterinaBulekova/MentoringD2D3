using System.Runtime.InteropServices;

namespace PowerManager.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEM_POWER_INFORMATION
    {
        public uint MaxIdlenessAllowed;
        public uint Idleness;
        public uint TimeRemaining;
        public byte CoolingMode;
    }
}
