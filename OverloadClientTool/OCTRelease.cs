using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadClientTool
{
    public class OCTRelease
    {
        public string DownloadUrl { get; set; }
        public long Size { get; set; }
        public DateTime Created { get; set; }
        public string Version { get; set; }
    }
}
