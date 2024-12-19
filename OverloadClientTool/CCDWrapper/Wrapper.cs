using System;
using System.Runtime.InteropServices;
using CCD.Enum;
using CCD.Struct;

namespace CCD
{
    // ReSharper disable once InconsistentNaming
    public class Wrapper
    {
        [DllImport("User32.dll")]
        public static extern StatusCode SetDisplayConfig(
            int numPathArrayElements,
            [In] DisplayConfigPathInfo[] pathArray,
            int numModeInfoArrayElements,
            [In] DisplayConfigModeInfo[] modeInfoArray,
            SdcFlags flags
        );

        [DllImport("User32.dll")]
        public static extern StatusCode SetDisplayConfig(
            int numPathArrayElements,
            [In] IntPtr pathArray,
            int numModeInfoArrayElements,
            [In] IntPtr modeInfoArray,
            SdcFlags flags
        );


        [DllImport("User32.dll")]
        public static extern StatusCode QueryDisplayConfig(
            QueryDisplayFlags flags,
            ref int numPathArrayElements,
            [Out] DisplayConfigPathInfo[] pathInfoArray,
            ref int modeInfoArrayElements,
            [Out] DisplayConfigModeInfo[] modeInfoArray,
            IntPtr topologyId = default(IntPtr)
        );

        [DllImport("User32.dll")]
        public static extern StatusCode QueryDisplayConfig(
            QueryDisplayFlags flags,
            ref int numPathArrayElements,
            [Out] DisplayConfigPathInfo[] pathInfoArray,
            ref int modeInfoArrayElements,
            [Out] DisplayConfigModeInfo[] modeInfoArray,
            out DisplayConfigTopologyId topologyId
        );

        [DllImport("User32.dll")]
        public static extern StatusCode GetDisplayConfigBufferSizes(
            QueryDisplayFlags flags,
            out int numPathArrayElements,
            out int numModeInfoArrayElements);

        [DllImport("User32.dll")]
        private static extern StatusCode DisplayConfigSetDeviceInfo(IntPtr requestPacket);
        public static StatusCode DisplayConfigSetDeviceInfo<T>(ref T displayConfig) where T : IDisplayConfigInfo
        {
            return MarshalStructureAndCall(ref displayConfig, DisplayConfigSetDeviceInfo);
        }


        [DllImport("User32.dll")]
        private static extern StatusCode DisplayConfigGetDeviceInfo(IntPtr requestPacket);
        public static StatusCode DisplayConfigGetDeviceInfo<T>(ref T displayConfig) where T : IDisplayConfigInfo
        {
            return MarshalStructureAndCall(ref displayConfig, DisplayConfigGetDeviceInfo);
        }

        /// <summary>
        /// The idea of this method is to make sure we have type-safety, without any stupid overloads.
        /// Without this, you would need to marshal yourself everything when using DisplayConfigGetDeviceInfo,
        /// or SetDeviceInfo, without any type-safety. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="displayConfig"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private static StatusCode MarshalStructureAndCall<T>(ref T displayConfig,
            Func<IntPtr, StatusCode> func) where T : IDisplayConfigInfo
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(displayConfig));
            Marshal.StructureToPtr(displayConfig, ptr, false);

            var returnValue = func(ptr);

            displayConfig = (T)Marshal.PtrToStructure(ptr, displayConfig.GetType());

            Marshal.FreeHGlobal(ptr);
            return returnValue;
        }
    }
}