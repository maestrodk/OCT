using System.Runtime.InteropServices;
using CCD.Enum;

namespace CCD.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigDeviceInfoHeader
    {
        public DisplayConfigDeviceInfoType type;
        public int size;
        public Luid adapterId;
        public uint id;
    }
}