using System;

namespace CCD.Enum
{
    [Flags]
    public enum DisplayConfigPixelFormat : uint
    {
        Zero = 0x0,

        Pixelformat8Bpp = 1,
        Pixelformat16Bpp = 2,
        Pixelformat24Bpp = 3,
        Pixelformat32Bpp = 4,
        PixelformatNongdi = 5,
    }
}