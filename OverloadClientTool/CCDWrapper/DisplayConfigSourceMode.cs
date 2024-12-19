using System.Runtime.InteropServices;
using CCD.Enum;

namespace CCD.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigSourceMode
    {
        public int width;
        public int height;
        public DisplayConfigPixelFormat pixelFormat;
        public PointL position;
    }
}