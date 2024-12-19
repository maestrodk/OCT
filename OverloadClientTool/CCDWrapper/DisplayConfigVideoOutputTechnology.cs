using System;

namespace CCD.Enum
{
    [Flags]
    public enum DisplayConfigVideoOutputTechnology : uint
    {
        Other = 4294967295, // -1
        Hd15 = 0,
        Svideo = 1,
        CompositeVideo = 2,
        ComponentVideo = 3,
        Dvi = 4,
        Hdmi = 5,
        Lvds = 6,
        DJpn = 8,
        Sdi = 9,
        DisplayportExternal = 10,
        DisplayportEmbedded = 11,
        UdiExternal = 12,
        UdiEmbedded = 13,
        Sdtvdongle = 14,
        Internal = 0x80000000,
    }
}