using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OverloadClientTool
{
    public class Pinger
    {
        private object lockObject = new object();

        public Dictionary<string, string> pingResults = new Dictionary<string, string>();
        public Dictionary<string, Stopwatch> pingTimers = new Dictionary<string, Stopwatch>();

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
                    if (!pingResults.ContainsKey(host)) pingResults.Add(host, "");
                }
                catch (Exception ex)
                {
                }
            }
        }

        public string Ping(string host)
        {
            lock (lockObject)
            {
                try
                {
                    if (pingResults.ContainsKey(host)) return pingResults[host];
                }
                catch (Exception ex)
                {
                }
                return "";
            }
        }

        /// <summary>
        /// Remove hosts from ping list if not in the specified list of hostst.
        /// </summary>
        /// <param name="hosts"></param>
        public void Cleanup(string[] hosts)
        {
            lock (lockObject)
            {
                try
                {
                    foreach (KeyValuePair<string, string> kvp in pingResults)
                    {
                        if (!hosts.Contains(kvp.Key)) pingResults.Remove(kvp.Key);

                        if (pingTimers.ContainsKey(kvp.Key)) pingTimers.Remove(kvp.Key);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void PingUpdate()
        {
            lock (lockObject)
            {
                try
                {
                    foreach (KeyValuePair<string, string> kvp in pingResults)
                    {
                        Ping ping = new Ping();
                        
                        if (pingTimers.ContainsKey(kvp.Key)) pingTimers[kvp.Key] = new Stopwatch();
                        else pingTimers.Add(kvp.Key, new Stopwatch());

                        pingTimers[kvp.Key].Reset();
                        pingTimers[kvp.Key].Start();

                        TcpClient tcpClient = new TcpClient();
                        AsyncCallback callBack = new AsyncCallback(EndConnect); //Set the callback to the doSomething void
                        tcpClient.BeginConnect(kvp.Key, 500, callBack, kvp.Key); //Try to connect to Google on port 80


                        //ping.PingCompleted += new PingCompletedEventHandler(PingCompleted);
                        //ping.SendAsync(kvp.Key, 1000, kvp.Key);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void EndConnect(IAsyncResult ar)
        {
            string host = (string)ar.AsyncState;
            lock (lockObject)
            {
                if (pingResults.ContainsKey(host) && pingTimers.ContainsKey(host))
                {
                    pingResults[host] = pingTimers[host].ElapsedMilliseconds.ToString();
                    pingTimers.Remove(host);
                }
            }
        }

        private void PingCompleted(object sender, PingCompletedEventArgs e)
        {
            try
            {
                string ip = (string)e.UserState;
                if (e.Reply != null && e.Reply.Status == IPStatus.Success)
                {
                    lock (lockObject)
                    {
                        if (pingResults.ContainsKey(ip) || pingTimers.ContainsKey(ip))
                        {
                            pingResults[ip] = pingTimers[ip].ElapsedMilliseconds.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
