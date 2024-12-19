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
using System.Xml;

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

        public Server()
        {
            IP = "";
            Name = "";
            Mode = "";
            Notes = "";
            NumPlayers = 0;
            MaxNumPlayers = 0;
            Started = DateTime.MinValue;
            Old = false;
        }

        public override string ToString()
        {
            return $"[{IP}] {Name} - {Mode} - {NumPlayers} / {MaxNumPlayers} - {Notes}";
        }
    }

    public class Servers
    {
        public static OCTMain Parent { get; set; } = null;

        private static Dictionary<string, int> ActiveServers = new Dictionary<string, int>();

        private const string realServerListUrl = @"https://tracker.otl.gg/api";

        private const string serverListUrl = @"https://octcache.playoverload.online/octServerList.dat";
        private const string serverListRequestTimeUrl = @"https://octcache.playoverload.online/octServerListRequestTime.dat";
        private const string serverListTestUrl = @"https://gbs.globaltraders.de:22255/octdata/getwebdata/getwebdata.dat";

        public static int ServerRefreshIntervalSeconds
        {
            get
            {
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers.Add("User-Agent", "Overload Client Tool v2.0.1 - user " + WindowsIdentity.GetCurrent().Name);
                        int seconds = Convert.ToInt32(wc.DownloadString(serverListRequestTimeUrl));
                        if ((seconds >= 15) && (seconds <= 3600)) return seconds;
                    }
                }
                catch
                {
                }
                return 60;
            }
        }

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
                        // json = wc.DownloadString(serverListTestUrl);
                    }

                    dynamic serverInfo = JsonConvert.DeserializeObject(json);
                    JContainer serverInfoContainer = (JContainer)serverInfo;

                    Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(serverInfo.ToString());

                    int count = serverInfoContainer.Count;

                    foreach (KeyValuePair<string, object> kvp in dict)
                    {
                        Server server = new Server();

                        Dictionary<string, object> srv = JsonConvert.DeserializeObject<Dictionary<string, object>>(kvp.Value.ToString());

                        try { server.IP = srv["ip"].ToString(); } catch { }
                        try { server.Name = srv["name"].ToString(); } catch { }
                        try { server.Notes = srv["notes"].ToString(); } catch { }
                        try { server.NumPlayers = Convert.ToInt32(srv["numPlayers"]); } catch { }
                        try { server.MaxNumPlayers = Convert.ToInt32(srv["maxNumPlayers"]); } catch { }
                        try { server.Map = srv["map"].ToString(); } catch { }
                        try { server.Mode = srv["mode"].ToString(); } catch { }
                        try { server.Started = Convert.ToDateTime(srv["gameStarted"]); } catch { }
                        try { server.Old = Convert.ToBoolean(srv["old"]); } catch { }

                        //if (server.IP == "85.27.246.6") 
                        servers.Add(server);
                    }

                    UpdateActiveServers(servers);

                    return servers;
                }
                catch (Exception ex)
                {
                    OverloadClientToolApplication.LogDebugMessage($"Cannot get server list from OTL: {ex.Message}");
                }

                return null;
            }
        }

        private static void UpdateActiveServers(List<Server> servers)
        {
            List<string> removeList = new List<string>();
            foreach (KeyValuePair<string, int> activeServer in ActiveServers)
            {
                bool remove = true;
                foreach (Server server in servers)
                {
                    if (server.IP == activeServer.Key) remove = false;
                }
                if (remove) removeList.Add(activeServer.Key);
            }

            // Remove servers not in active list.
            foreach (string ip in removeList) ActiveServers.Remove(ip);

            foreach (Server server in servers)
            {
                if (ActiveServers.ContainsKey(server.IP))
                {
                    int before = ActiveServers[server.IP];
                    int now = server.NumPlayers;
                    int change = now - before;

                    if (change != 0)
                    {
                        string message;
                        if (change > 0)
                        {
                            if (change == 1) message = $"A player joined server {server.Name}, map {server.Map}";
                            else message = $"Some players joined server {server.Name}, map {server.Map}";
                        }
                        else
                        {
                            if (change == -1) message = $"A player left server {server.Name}" + (String.IsNullOrEmpty(server.Map) ? "" : $", map {server.Map}");
                            else message = $"Some players left server {server.Name}" + (String.IsNullOrEmpty(server.Map) ? "" : $", map {server.Map}");
                        }
                        Parent.AddNewLogMessage(message);
                    }

                    // Remove from active list if no players.
                    if (now == 0)
                    {
                        ActiveServers.Remove(server.IP);
                    }
                }
                else
                {
                    // Add to active list if player joined.
                    if (server.NumPlayers > 0)
                    {
                        string message;
                        if (server.NumPlayers == 1) message = $"A player joined server {server.Name}, map {server.Map}";
                        else message = $"Some players joined server {server.Name}, map {server.Map}";
                        Parent.AddNewLogMessage(message);
                        ActiveServers.Add(server.IP, server.NumPlayers);
                    }
                }
            }
        }

    }
}
