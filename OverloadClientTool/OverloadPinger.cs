using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OverloadClientTool
{
    public class OverloadPinger
    {
        private object lockObject = new object();
        public Dictionary<string, string> serverPingResults = new Dictionary<string, string>();
        public Dictionary<string, Stopwatch> pingTimers = new Dictionary<string, Stopwatch>();

        internal bool Active { get; set; } = true;

        /// <summary>
        /// Pings a host.
        /// </summary>
        /// <param name="host">DNS or IP address.</param>
        /// <returns>Ping time milliseconds, -1 if timeout/error.</returns>
        internal async Task<PingResult> OverloadServerPingAsync(string host)
        {
            if (!Active) return new PingResult(host); // Defaults to -1.

            if (!IPAddress.TryParse(host, out IPAddress addr))
            {
                var addrs = Dns.GetHostAddresses(host);
                addr = ((addrs == null) || (addrs.Length == 0)) ? null : addrs[0];
            }

            if (addr == null) return new PingResult(host); // Bad host/IP.

            using (UdpClient remoteSocket = new UdpClient())
            {
                int remotePort = 8001;

                try { remoteSocket.Connect(new IPEndPoint(addr, remotePort)); } catch { return new PingResult(host); } // No connection.

                var packet = new byte[19 + 4 + 4 + 8 + 4];
                int seqNum = 1;

                long transmitTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                Array.Copy(BitConverter.GetBytes(-1), 0, packet, 0, 4);
                Array.Copy(BitConverter.GetBytes(0), 0, packet, 8, 4);
                Array.Copy(BitConverter.GetBytes(0), 0, packet, 19, 4);
                Array.Copy(BitConverter.GetBytes(0), 0, packet, 19 + 4, 4);
                Array.Copy(BitConverter.GetBytes(transmitTime), 0, packet, 19 + 4 + 4, 8);
                Array.Copy(BitConverter.GetBytes(seqNum), 0, packet, 19 + 4 + 4 + 8, 4);
                uint hash = xxHash.CalculateHash(packet);
                Array.Copy(BitConverter.GetBytes(hash), 0, packet, 8, 4);
                remoteSocket.Send(packet, packet.Length);

                Task<UdpReceiveResult> result;
                await Task.WhenAny(result = remoteSocket.ReceiveAsync(), Task.Delay(5000));

                if (result.IsCompleted == false)
                {
                    // Timeout. Shut down further processing of the remote socket instance.
                    try { remoteSocket.Close(); } catch { Debug.WriteLine($"[{host}] Remote socket close exception"); }
                    return new PingResult(host);
                }

                var ret = result.Result.Buffer;

                long receiveTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                hash = BitConverter.ToUInt32(ret, 8);
                Array.Copy(BitConverter.GetBytes(0), 0, ret, 8, 4);
                uint srcHash = xxHash.CalculateHash(ret);
                if (srcHash != hash) return new PingResult(host);

                PingResult pingResult = new PingResult(host);
                pingResult.PingTime = (int)(receiveTime - transmitTime);

                return pingResult;
            }
        }

        /// <summary>
        /// Add or update host and ping result.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="value"></param>
        public void AddHost(string host)
        {
            lock (lockObject)
            {
                try
                {
                    if (!serverPingResults.ContainsKey(host)) serverPingResults.Add(host, "9999");
                }
                catch
                {
                }
            }
        }

        public string PingTime(string host)
        {
            lock (lockObject)
            {
                string result = "9999";
                try
                {
                    if (Active && serverPingResults.ContainsKey(host)) result = serverPingResults[host];
                }
                catch
                {
                }
                return result;
            }
        }

        internal class PingResult
        {
            public int PingTime { get; set; }
            public string Server { get; set; }

            public PingResult(string server)
            {
                Server = server;
                PingTime = -1;
            }
        }

        public void PingUpdateThread()
        {
            while (true)
            {
                DateTime nextTime = DateTime.Now + new TimeSpan(0, 0, 15);

                if (Active)
                {
                    // Get a copy of the current server list.
                    List<string> servers = new List<string>();
                    lock (lockObject) { foreach (KeyValuePair<string, string> kvp in serverPingResults) servers.Add(kvp.Key); }
                    
                    try
                    {
                        List<Task<PingResult>> taskList = new List<Task<PingResult>>();

                        foreach (string server in servers) taskList.Add(OverloadServerPingAsync(server));

                        Task.WaitAll(taskList.ToArray());

                        lock (lockObject)
                        {
                            foreach (Task<PingResult> task in taskList)
                            {
                                int pingTime = task.Result.PingTime;
                                string server = task.Result.Server;

                                Debug.WriteLine($"Ping update server {server} {pingTime}");

                                // Only update if the server is still in the list.
                                if (serverPingResults.ContainsKey(server))
                                {
                                    serverPingResults[server] = (pingTime < 1) ? "9999" : pingTime.ToString().PadLeft(4, '0');
                                }

                            }
                        }

                        taskList = null;
                    }
                    catch
                    {
                    }
                }

                // See if we should wait a little while before next ping round.
                while (DateTime.Now < nextTime) Thread.Sleep(250);
            }
        }
    }
}
