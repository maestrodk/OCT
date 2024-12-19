using System.Runtime.InteropServices;
using CCD.Enum;

namespace CCD.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigPathSourceInfo
    {
        public Luid adapterId;
        public uint id;
        public uint modeInfoIdx;

        public DisplayConfigSourceStatus statusFlags;
    }
}