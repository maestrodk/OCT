using System;

namespace CCD.Enum
{
    [Flags]
    public enum DisplayConfigScanLineOrdering : uint
    {
        Unspecified = 0,
        Progressive = 1,
        Interlaced = 2,
        InterlacedUpperfieldfirst = Interlaced,
        InterlacedLowerfieldfirst = 3,
    }
}