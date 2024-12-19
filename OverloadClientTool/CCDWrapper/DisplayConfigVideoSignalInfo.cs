using System.Runtime.InteropServices;
using CCD.Enum;

namespace CCD.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DisplayConfigVideoSignalInfo
    {
        public long pixelRate;
        public DisplayConfigRational hSyncFreq;
        public DisplayConfigRational vSyncFreq;
        public DisplayConfig2DRegion activeSize;
        public DisplayConfig2DRegion totalSize;

        public D3DmdtVideoSignalStandard videoStandard;
        public DisplayConfigScanLineOrdering scanLineOrdering;
    }
}