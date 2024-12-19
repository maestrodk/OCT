using System;

namespace CCD.Enum
{
    [Flags]
    public enum DisplayConfigTargetStatus : uint
    {
        Zero = 0x0,

        InUse = 0x00000001,
        Forcible = 0x00000002,
        ForcedAvailabilityBoot = 0x00000004,
        ForcedAvailabilityPath = 0x00000008,
        ForcedAvailabilitySystem = 0x00000010,
    }
}