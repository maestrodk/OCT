namespace OverloadClientTool
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Management;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Threading;

    using System.Windows.Forms;
    using CCD;
    using CCD.Enum;
    using CCD.Struct;



    #region enums

    public enum QUERY_DEVICE_CONFIG_FLAGS : uint
    {
        QDC_ALL_PATHS = 0x00000001,
        QDC_ONLY_ACTIVE_PATHS = 0x00000002,
        QDC_DATABASE_CURRENT = 0x00000004
    }

    public enum DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY : uint
    {
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_OTHER = 0xFFFFFFFF,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_HD15 = 0,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_SVIDEO = 1,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_COMPOSITE_VIDEO = 2,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_COMPONENT_VIDEO = 3,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DVI = 4,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_HDMI = 5,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_LVDS = 6,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_D_JPN = 8,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_SDI = 9,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DISPLAYPORT_EXTERNAL = 10,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DISPLAYPORT_EMBEDDED = 11,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_UDI_EXTERNAL = 12,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_UDI_EMBEDDED = 13,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_SDTVDONGLE = 14,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_MIRACAST = 15,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_INTERNAL = 0x80000000,
        DISPLAYCONFIG_OUTPUT_TECHNOLOGY_FORCE_UINT32 = 0xFFFFFFFF
    }

    public enum DISPLAYCONFIG_SCANLINE_ORDERING : uint
    {
        DISPLAYCONFIG_SCANLINE_ORDERING_UNSPECIFIED = 0,
        DISPLAYCONFIG_SCANLINE_ORDERING_PROGRESSIVE = 1,
        DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED = 2,
        DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED_UPPERFIELDFIRST = DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED,
        DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED_LOWERFIELDFIRST = 3,
        DISPLAYCONFIG_SCANLINE_ORDERING_FORCE_UINT32 = 0xFFFFFFFF
    }

    public enum DISPLAYCONFIG_ROTATION : uint
    {
        DISPLAYCONFIG_ROTATION_IDENTITY = 1,
        DISPLAYCONFIG_ROTATION_ROTATE90 = 2,
        DISPLAYCONFIG_ROTATION_ROTATE180 = 3,
        DISPLAYCONFIG_ROTATION_ROTATE270 = 4,
        DISPLAYCONFIG_ROTATION_FORCE_UINT32 = 0xFFFFFFFF
    }

    public enum DISPLAYCONFIG_SCALING : uint
    {
        DISPLAYCONFIG_SCALING_IDENTITY = 1,
        DISPLAYCONFIG_SCALING_CENTERED = 2,
        DISPLAYCONFIG_SCALING_STRETCHED = 3,
        DISPLAYCONFIG_SCALING_ASPECTRATIOCENTEREDMAX = 4,
        DISPLAYCONFIG_SCALING_CUSTOM = 5,
        DISPLAYCONFIG_SCALING_PREFERRED = 128,
        DISPLAYCONFIG_SCALING_FORCE_UINT32 = 0xFFFFFFFF
    }

    public enum DISPLAYCONFIG_PIXELFORMAT : uint
    {
        DISPLAYCONFIG_PIXELFORMAT_8BPP = 1,
        DISPLAYCONFIG_PIXELFORMAT_16BPP = 2,
        DISPLAYCONFIG_PIXELFORMAT_24BPP = 3,
        DISPLAYCONFIG_PIXELFORMAT_32BPP = 4,
        DISPLAYCONFIG_PIXELFORMAT_NONGDI = 5,
        DISPLAYCONFIG_PIXELFORMAT_FORCE_UINT32 = 0xffffffff
    }

    public enum DISPLAYCONFIG_MODE_INFO_TYPE : uint
    {
        DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE = 1,
        DISPLAYCONFIG_MODE_INFO_TYPE_TARGET = 2,
        DISPLAYCONFIG_MODE_INFO_TYPE_FORCE_UINT32 = 0xFFFFFFFF
    }

    public enum DISPLAYCONFIG_DEVICE_INFO_TYPE : uint
    {
        DISPLAYCONFIG_DEVICE_INFO_GET_SOURCE_NAME = 1,
        DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_NAME = 2,
        DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_PREFERRED_MODE = 3,
        DISPLAYCONFIG_DEVICE_INFO_GET_ADAPTER_NAME = 4,
        DISPLAYCONFIG_DEVICE_INFO_SET_TARGET_PERSISTENCE = 5,
        DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_BASE_TYPE = 6,
        DISPLAYCONFIG_DEVICE_INFO_FORCE_UINT32 = 0xFFFFFFFF
    }

    #endregion

    #region structs

    [StructLayout(LayoutKind.Sequential)]
    public struct LUID
    {
        public uint LowPart;
        public int HighPart;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_PATH_SOURCE_INFO
    {
        public LUID adapterId;
        public uint id;
        public uint modeInfoIdx;
        public uint statusFlags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_PATH_TARGET_INFO
    {
        public LUID adapterId;
        public uint id;
        public uint modeInfoIdx;
        private DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY outputTechnology;
        private DISPLAYCONFIG_ROTATION rotation;
        private DISPLAYCONFIG_SCALING scaling;
        private DISPLAYCONFIG_RATIONAL refreshRate;
        private DISPLAYCONFIG_SCANLINE_ORDERING scanLineOrdering;
        public bool targetAvailable;
        public uint statusFlags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_RATIONAL
    {
        public uint Numerator;
        public uint Denominator;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_PATH_INFO
    {
        public DISPLAYCONFIG_PATH_SOURCE_INFO sourceInfo;
        public DISPLAYCONFIG_PATH_TARGET_INFO targetInfo;
        public uint flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_2DREGION
    {
        public uint cx;
        public uint cy;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_VIDEO_SIGNAL_INFO
    {
        public ulong pixelRate;
        public DISPLAYCONFIG_RATIONAL hSyncFreq;
        public DISPLAYCONFIG_RATIONAL vSyncFreq;
        public DISPLAYCONFIG_2DREGION activeSize;
        public DISPLAYCONFIG_2DREGION totalSize;
        public uint videoStandard;
        public DISPLAYCONFIG_SCANLINE_ORDERING scanLineOrdering;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_TARGET_MODE
    {
        public DISPLAYCONFIG_VIDEO_SIGNAL_INFO targetVideoSignalInfo;
    }

    /*
    [StructLayout(LayoutKind.Sequential)]
    public struct POINTL
    {
        private int x;
        private int y;
    }
*/

    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_SOURCE_MODE
    {
        public uint width;
        public uint height;
        public DISPLAYCONFIG_PIXELFORMAT pixelFormat;
        public POINTL position;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct DISPLAYCONFIG_MODE_INFO_UNION
    {
        [FieldOffset(0)]
        public DISPLAYCONFIG_TARGET_MODE targetMode;

        [FieldOffset(0)]
        public DISPLAYCONFIG_SOURCE_MODE sourceMode;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_MODE_INFO
    {
        public DISPLAYCONFIG_MODE_INFO_TYPE infoType;
        public uint id;
        public LUID adapterId;
        public DISPLAYCONFIG_MODE_INFO_UNION modeInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_TARGET_DEVICE_NAME_FLAGS
    {
        public uint value;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DISPLAYCONFIG_DEVICE_INFO_HEADER
    {
        public DISPLAYCONFIG_DEVICE_INFO_TYPE type;
        public uint size;
        public LUID adapterId;
        public uint id;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DISPLAYCONFIG_TARGET_DEVICE_NAME
    {
        public DISPLAYCONFIG_DEVICE_INFO_HEADER header;
        public DISPLAYCONFIG_TARGET_DEVICE_NAME_FLAGS flags;
        public DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY outputTechnology;
        public ushort edidManufactureId;
        public ushort edidProductCodeId;
        public uint connectorInstance;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string monitorFriendlyDeviceName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string monitorDevicePath;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DISPLAYCONFIG_SOURCE_DEVICE_NAME
    {
        public DISPLAYCONFIG_DEVICE_INFO_HEADER header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string viewGdiDeviceName;
    }

    #endregion

    public class DisplayConfiguration
    {
        DisplayConfigPathInfo[] pathInfoArray;
        DisplayConfigModeInfo[] modeInfoArray;

        public DisplayConfiguration()
        {
        }

        public DisplayConfiguration(DisplayConfigPathInfo[] pathInfo, DisplayConfigModeInfo[] modeInfo)
        {
            this.pathInfoArray = pathInfo;
            this.modeInfoArray = modeInfo;
        }

        public static DisplayConfiguration ActiveConfiguration
        {
            get 
            {
                DisplayConfiguration configuration = new DisplayConfiguration();
                bool success = Monitor.GetDisplayConfiguration(out configuration.pathInfoArray, out configuration.modeInfoArray);
                if (success == false) configuration = null;
                return configuration;
            }
            set
            {
                Monitor.SetDisplayConfiguration(value.pathInfoArray, value.modeInfoArray);
            }
        }

        public static void Disable(int displayNumber)
        {
            Monitor.Disconnect(displayNumber);
        }
    }

    public class Monitor
    {
        #region Dll imports

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        private static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] MonitorInfoEx info);

        [DllImport("user32.dll", ExactSpelling = true)]
        [ResourceExposure(ResourceScope.None)]
        private static extern bool EnumDisplayMonitors(HandleRef hdc, IntPtr rcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

        private delegate bool MonitorEnumProc(IntPtr monitor, IntPtr hdc, IntPtr lprcMonitor, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public class Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public Rect()
            {
            }

            public Rect(int l, int t, int r, int b)
            {
                this.left = l;
                this.top = t;
                this.right = r;                    
                this.bottom = b;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        private class MonitorInfoEx
        {
            internal int cbSize = Marshal.SizeOf(typeof(MonitorInfoEx));
            internal Rect rcMonitor = new Rect();
            internal Rect rcWork = new Rect();
            internal int dwFlags = 0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            internal char[] szDevice = new char[32];
        }

        private const int MonitorinfofPrimary = 0x00000001;

        [DllImport("user32.dll")]
        public static extern int GetDisplayConfigBufferSizes(QUERY_DEVICE_CONFIG_FLAGS flags, out uint numPathArrayElements, out uint numModeInfoArrayElements);

        [DllImport("user32.dll")]
        public static extern int QueryDisplayConfig(
            QUERY_DEVICE_CONFIG_FLAGS flags,
            ref uint numPathArrayElements, [Out] DISPLAYCONFIG_PATH_INFO[] PathInfoArray,
            ref uint numModeInfoArrayElements, [Out] DISPLAYCONFIG_MODE_INFO[] ModeInfoArray,
            IntPtr currentTopologyId
            );

        [DllImport("user32.dll")]
        public static extern int DisplayConfigGetDeviceInfo(ref DISPLAYCONFIG_TARGET_DEVICE_NAME deviceName);

        [DllImport("user32.dll")]
        public static extern int DisplayConfigGetDeviceInfo(ref DISPLAYCONFIG_SOURCE_DEVICE_NAME deviceName);

        #endregion

        public static HandleRef nullHandleRef = new HandleRef(null, IntPtr.Zero);

        public Rect Bounds { get; private set; }

        public Rect WorkingArea { get; private set; }

        public string Name { get; private set; }

        public bool IsPrimary { get; private set; }

        public IntPtr Handle { get; private set; }

        public Monitor(IntPtr monitor)
        {
            Handle = monitor;

            var info = new MonitorInfoEx();

            GetMonitorInfo(new HandleRef(null, monitor), info);

            Bounds = new Rect(info.rcMonitor.left, info.rcMonitor.top, info.rcMonitor.right - info.rcMonitor.left, info.rcMonitor.bottom - info.rcMonitor.top);
            WorkingArea = new Rect(info.rcWork.left, info.rcWork.top, info.rcWork.right - info.rcWork.left, info.rcWork.bottom - info.rcWork.top);
            IsPrimary = ((info.dwFlags & MonitorinfofPrimary) != 0);
            Name = new string(info.szDevice).TrimEnd((char)0);
        }

        public static IEnumerable<Monitor> AllMonitors
        {
            get
            {
                var closure = new MonitorEnumCallback();
                var proc = new MonitorEnumProc(closure.Callback);

                EnumDisplayMonitors(nullHandleRef, IntPtr.Zero, proc, IntPtr.Zero);

                return closure.Monitors.Cast<Monitor>();
            }
        }
   
        /// <summary>
        /// Disconnect a display.
        /// </summary>
        public static bool Disconnect(int displayNumber)
        {
            // Get the necessary display information
            int numPathArrayElements = -1;
            int numModeInfoArrayElements = -1;

            StatusCode error = Wrapper.GetDisplayConfigBufferSizes(
                QueryDisplayFlags.OnlyActivePaths,
                out numPathArrayElements,
                out numModeInfoArrayElements);

            DisplayConfigPathInfo[] pathInfoArray = new DisplayConfigPathInfo[numPathArrayElements];
            DisplayConfigModeInfo[] modeInfoArray = new DisplayConfigModeInfo[numModeInfoArrayElements];

            error = Wrapper.QueryDisplayConfig(
                QueryDisplayFlags.OnlyActivePaths,
                ref numPathArrayElements,
                pathInfoArray,
                ref numModeInfoArrayElements,
                modeInfoArray,
                IntPtr.Zero);

            if (error != StatusCode.Success) return false;

            if (displayNumber > (pathInfoArray.Length - 1)) return false;

            // Check the index
            if (pathInfoArray[displayNumber].sourceInfo.modeInfoIdx < modeInfoArray.Length)
            {
                // Disable and reset the display configuration
                pathInfoArray[displayNumber].flags = DisplayConfigFlags.Zero;

                error = Wrapper.SetDisplayConfig(
                    pathInfoArray.Length,
                    pathInfoArray,
                    modeInfoArray.Length,
                    modeInfoArray,
                    (SdcFlags.Apply | SdcFlags.AllowChanges | SdcFlags.UseSuppliedDisplayConfig));

                if (error == StatusCode.Success)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool SetDisplayConfiguration(DisplayConfigPathInfo[] pathInfoArray, DisplayConfigModeInfo[] modeInfoArray)
        {
            StatusCode error = Wrapper.SetDisplayConfig(
                pathInfoArray.Length,
                pathInfoArray,
                modeInfoArray.Length,
                modeInfoArray,
                (SdcFlags.Apply | SdcFlags.AllowChanges | SdcFlags.UseSuppliedDisplayConfig));

            return (error == StatusCode.Success);
        }

        public static bool GetDisplayConfiguration(out DisplayConfigPathInfo[] pathInfoArray, out DisplayConfigModeInfo[] modeInfoArray)
        {
            // Get the necessary display information
            int numPathArrayElements = -1;
            int numModeInfoArrayElements = -1;
            StatusCode error = Wrapper.GetDisplayConfigBufferSizes(
                QueryDisplayFlags.OnlyActivePaths,
                out numPathArrayElements,
                out numModeInfoArrayElements);

            pathInfoArray = new DisplayConfigPathInfo[numPathArrayElements];
            modeInfoArray = new DisplayConfigModeInfo[numModeInfoArrayElements];

            error = Wrapper.QueryDisplayConfig(
                QueryDisplayFlags.OnlyActivePaths,
                ref numPathArrayElements,
                pathInfoArray,
                ref numModeInfoArrayElements,
                modeInfoArray,
                IntPtr.Zero);

            return (error == StatusCode.Success);
        }

        const int ERROR_SUCCESS = 0;

        public static string MonitorFriendlyName(LUID adapterId, uint targetId)
        {
            var deviceName = new DISPLAYCONFIG_TARGET_DEVICE_NAME
            {
                header =
                {
                    size = (uint)Marshal.SizeOf(typeof (DISPLAYCONFIG_TARGET_DEVICE_NAME)),
                    adapterId = adapterId,
                    id = targetId,
                    type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_NAME
                }
            };

            var error = DisplayConfigGetDeviceInfo(ref deviceName);

            if (error != ERROR_SUCCESS) return null;
            else return deviceName.monitorFriendlyDeviceName;
        }

        public static string MonitorDeviceName(LUID adapterId, uint targetId)
        {
            var deviceName = new DISPLAYCONFIG_SOURCE_DEVICE_NAME
            {
                header =
                {
                    size = (uint)Marshal.SizeOf(typeof (DISPLAYCONFIG_SOURCE_DEVICE_NAME)),
                    adapterId = adapterId,
                    id = targetId,
                    type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_SOURCE_NAME
                }
            };

            var error = DisplayConfigGetDeviceInfo(ref deviceName);

            if (error != ERROR_SUCCESS)
            {
                return null;
            }
            else
            {
                return deviceName.viewGdiDeviceName;
            }
        }

        public static IEnumerable<string> GetAllMonitorFriendlyNames()
        {
            uint pathCount, modeCount;
            var error = GetDisplayConfigBufferSizes(QUERY_DEVICE_CONFIG_FLAGS.QDC_ONLY_ACTIVE_PATHS, out pathCount, out modeCount);
            
            if (error != ERROR_SUCCESS) yield return null;

            var displayPaths = new DISPLAYCONFIG_PATH_INFO[pathCount];
            var displayModes = new DISPLAYCONFIG_MODE_INFO[modeCount];

            error = QueryDisplayConfig(QUERY_DEVICE_CONFIG_FLAGS.QDC_ONLY_ACTIVE_PATHS,
                ref pathCount, displayPaths, ref modeCount, displayModes, IntPtr.Zero);

            if (error != ERROR_SUCCESS) yield return null;

            for (var i = 0; i < modeCount; i++)
                if (displayModes[i].infoType == DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_TARGET)
                    yield return MonitorFriendlyName(displayModes[i].adapterId, displayModes[i].id);
        }

        public static IEnumerable<string> GetAllMonitorDeviceNames()
        {
            uint pathCount, modeCount;
            var error = GetDisplayConfigBufferSizes(QUERY_DEVICE_CONFIG_FLAGS.QDC_ONLY_ACTIVE_PATHS, out pathCount, out modeCount);

            if (error != ERROR_SUCCESS) yield return null;

            var displayPaths = new DISPLAYCONFIG_PATH_INFO[pathCount];
            var displayModes = new DISPLAYCONFIG_MODE_INFO[modeCount];

            error = QueryDisplayConfig(QUERY_DEVICE_CONFIG_FLAGS.QDC_ONLY_ACTIVE_PATHS,
                ref pathCount, displayPaths, ref modeCount, displayModes, IntPtr.Zero);

            if (error != ERROR_SUCCESS) yield return null;

            for (var i = 0; i < modeCount; i++)
                if (displayModes[i].infoType == DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_TARGET)
                    yield return MonitorDeviceName(displayModes[i].adapterId, displayModes[i].id);
        }

        private class MonitorEnumCallback
        {
            public ArrayList Monitors { get; set; }

            public MonitorEnumCallback()
            {
                Monitors = new ArrayList();
            }

            public bool Callback(IntPtr monitor, IntPtr hdc, IntPtr lprcMonitor, IntPtr lparam)
            {
                Monitors.Add(new Monitor(monitor));
                return true;
            }
        }

        public static List<string> GetMonitorNames()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM WmiMonitorConnectionParams");
            var info = new List<string>();

            foreach (ManagementObject queryObj in searcher.Get())
            {
                foreach (PropertyData pd in queryObj.Properties)
                {
                    string propertyName = pd.Name;
                    object propertyValue = pd.Value;

                    Console.WriteLine($"{propertyName} = {propertyValue}");
                }

                info.Add(queryObj["InstanceName"].ToString());
            }

            return info;
        }
    }
}