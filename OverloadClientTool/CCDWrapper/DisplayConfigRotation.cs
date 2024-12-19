using System;

namespace CCD.Enum
{
    [Flags]
    public enum DisplayConfigRotation : uint
    {
        Zero = 0x0,

        Identity = 1,
        Rotate90 = 2,
        Rotate180 = 3,
        Rotate270 = 4,
    }
}
