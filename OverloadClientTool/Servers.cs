using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Principal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OverloadClientTool
{
    public class Server
    {
        public string IP { get; set; }
        public string Name { get; set; }
        public string Mode { get; set; }
        public string Notes { get; set; }
        public int NumPlayers { get; set; }
        public int MaxNumPlayers { get; set; }
        public string Map { get; set; }
        public DateTime Started { get; set; }
        public bool Old { get; set; }

        public override string ToString()
        {
            return $"[{IP}] {Name} - {Mode} - {NumPlayers} / {MaxNumPlayers} - {Notes}";
        }
    }

    public class Servers
    {
        private const string serverListUrl = @"https://olproxy.otl.gg/api";
        
        public static List<Server> ServerList
        {
            get
            {
                List<Server> servers = new List<Server>();

                try
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => { return true; };
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    string json = "";
                    
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("User-Agent", "Overload Client Tool - user " + WindowsIdentity.GetCurrent().Name);
                        json = wc.DownloadString(serverListUrl);
                    }

                    dynamic serverInfo = JsonConvert.DeserializeObject(json);
                    JContainer serverInfoContainer = (JContainer)serverInfo;

                    Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(serverInfo.ToString());

                    int count = serverInfoContainer.Count;


                    int i = 0;
                    foreach (KeyValuePair<string, object> kvp in dict)
                    {
                        Server server = new Server();

                        Dictionary<string, object> srv = JsonConvert.DeserializeObject<Dictionary<string, object>>(kvp.Value.ToString());

                        server.IP = srv["ip"].ToString();
                        server.Name = srv["name"].ToString();
                        server.Notes = srv["notes"].ToString();
                        server.NumPlayers = Convert.ToInt32(srv["numPlayers"]);
                        server.MaxNumPlayers = Convert.ToInt32(srv["maxNumPlayers"]);
                        server.Map = srv["map"].ToString();
                        server.Mode = srv["mode"].ToString();
                        server.Started = Convert.ToDateTime(srv["gameStarted"]);
                        server.Old = Convert.ToBoolean(srv["old"]); 
                        servers.Add(server);
                    }

                    return servers;
                }
                catch (Exception ex)
                {
                    OverloadClientToolApplication.LogDebugMessage($"Cannot get serverlist from OTL: {ex.Message}");
                }

                return null;
            }
        }
    }
}
