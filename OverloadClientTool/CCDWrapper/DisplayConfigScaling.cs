using System;

namespace CCD.Enum
{
    [Flags]
    public enum DisplayConfigScaling : uint
    {
        Zero = 0x0,

        Identity = 1,
        Centered = 2,
        Stretched = 3,
        Aspectratiocenteredmax = 4,
        Custom = 5,
        Preferred = 128,
    }
}