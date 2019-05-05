using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadClientTool
{
    public class OlmodManager
    {
        private string jsonUrl = @"https://api.github.com/repos/arbruijn/olmod/releases/latest";

        public static string GetInstalledVersion
        {
            get
            {
                return "";
            }
        }

        public static string GetOnlineVersion
        {
            get
            {
                return "";
            }
        }

        public static string DownloadOlmodZip(string url)
        {
            return "";
        }
    }
}
