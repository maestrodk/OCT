using System.Collections.Generic;

namespace CCD.Struct
{
    /// <summary>
    /// This class is used in order to wrap path and mode info.
    /// </summary>
    public struct DisplayConfigPathWrap
    {
        /// <summary>
        /// Holds display path.
        /// </summary>
        public DisplayConfigPathInfo Path { get; private set; }

        /// <summary>
        /// Holds possible modes for path.
        /// </summary>
        public IEnumerable<DisplayConfigModeInfo> Modes { get; private set; }

        /// <summary>
        /// Initializes new structure. Having constructor for struct makes it immutable.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="modeInfo"></param>
        public DisplayConfigPathWrap(DisplayConfigPathInfo path,
            IEnumerable<DisplayConfigModeInfo> modeInfo)
            : this()
        {
            Path = path;
            Modes = modeInfo;
        }
    }
}
