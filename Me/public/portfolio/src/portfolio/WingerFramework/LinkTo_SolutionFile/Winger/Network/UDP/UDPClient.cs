using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using Winger.Network.UDP.Messages;
using Winger.Network.Utils;
using Winger.Utils;

namespace Winger.Network.UDP
{
    public delegate void OnSimClientDisconnect(JSON data);
    public delegate void OnTimeout();

    public class UDPClient
    {
        protected Log log = new Log("[CLIENT] ", LogLevel.DEBUG);

        protected UdpClient udp;

        public string Name { get; protected set; }
        public IPEndPoint DestinationIp { get; protected set; }
        public int DestinationPort { get; protected set; }
        public int ListeningPort { get; protected set; }
        public bool IsConnected { get; protected set; }
        public JSON Data { get; set; }
        public OnSimClientDisconnect OnSimClientDisconnect { get; set; }
        public OnTimeout OnTimeout { get; set; }
        public long TimeToLive { get; protected set; }
        public string ServerPublicKey { get; protected set; }
        public string PublicKey { get; protected set; }
        public string PrivateKey { get; protected set; }
        public bool Listening { get; protected set; }

        protected Dictionary<string, SimulatedClients> simulatedClients = new Dictionary<string, SimulatedClients>();

        protected Thread runningThread;
        protected System.Timers.Timer keepAliveTimer;
        protected bool needKeepAliveThisCycle = true;


        public UDPClient(string name, int listeningPort, int destinationPort)
        {
            Name = name;
            ListeningPort = listeningPort;
            DestinationPort = destinationPort;
            DestinationIp = null;
            TimeToLive = 10000; // defaults to 10 seconds
            udp = new UdpClient(ListeningPort);

            runningThread = ThreadUtils.StartUpInThread(Listen);
        }


        public UDPClient(string name, int listeningPort, string destinationIp, int destinationPort)
        {
            Name = name;
            ListeningPort = listeningPort;
            DestinationPort = destinationPort;
            DestinationIp = new IPEndPoint(IPAddress.Parse(destinationIp), DestinationPort);
            udp = new UdpClient(ListeningPort);

            runningThread = ThreadUtils.StartUpInThread(Listen);
        }


        public List<SimulatedClients> GetSimClients()
        {
            return simulatedClients.Values.ToList<SimulatedClients>();
        }


        #region Send messages
        public void SendDiscover()
        {
            DiscoverMessage mes = MessageFactory.ToDiscoverMessage(Name);

            string localIp = IPUtils.LocalIPAddress();
            string[] ipBytesStr = localIp.Split('.');
            int localIpByte = Int32.Parse(ipBytesStr[3]);
            string ipWithoutLast = ipBytesStr[0] + "." + ipBytesStr[1] + "." + ipBytesStr[2] + ".";
            // sending a "broadcast" message (substitute the last byte with 255 and it should send to all computers listening)
            string destIpStr = ipWithoutLast + 255;
            IPEndPoint destIp = new IPEndPoint(IPAddress.Parse(destIpStr), DestinationPort);
            log.Debug("Sending discover message to " + destIp.ToString());
            Send(mes.Data, destIp);
        }


        public void SendConnect()
        {
            if (DestinationIp == null)
            {
                throw new Exception("DestinationIp cannot be null, either set it explicitly or call SendDiscover()");
            }
            ConnectMessage mes = MessageFactory.ToConnectMessage(Name);
            mes.PublicKey = PublicKey;
            log.Debug("Send connect message");
            Send(mes.Data);
        }


        public void SendKeepAlive()
        {
            KeepAliveMessage mes = MessageFactory.ToKeepAliveMessage(Name);
            mes.CurrentTimestamp = Timestamp.Now;
            log.Debug("Send keep alive message");
            Send(mes.Data);
        }


        public void SendUpdate()
        {
            UpdateMessage mes = new UpdateMessage(Data);
            mes.Name = Name;
            mes.Type = "update";
            mes.CurrentTimestamp = Timestamp.Now;
            log.Debug("Send update message");
            Send(mes.Data);
            needKeepAliveThisCycle = false;
        }


        public void SendDisconnect()
        {
            Listening = false;
            DisconnectMessage mes = MessageFactory.ToDisconnectMessage(Name);
            log.Debug("Send disconnect message");
            Send(mes.Data);
            ThreadUtils.Pause(500);
            Stop();
        }


        protected void Send(JSON data, IPEndPoint ip)
        {
            byte[] bData = data.ToByteArray();
            udp.Send(bData, bData.Length, ip);
        }


        protected void Send(JSON data)
        {
            Send(data, DestinationIp);
        }


        public virtual void StartKeepAliveCycle()
        {
            keepAliveTimer = new System.Timers.Timer(TimeToLive / 4);
            keepAliveTimer.Elapsed += new ElapsedEventHandler(OnKeepAliveCycleEvent);
            keepAliveTimer.Enabled = true;
        }


        protected virtual void OnKeepAliveCycleEvent(object source, ElapsedEventArgs e)
        {
            if (needKeepAliveThisCycle)
            {
                SendKeepAlive();
            }
            else
            {
                needKeepAliveThisCycle = true;
            }
        }
        #endregion


        #region Thread messages
        protected void Listen()
        {
            Listening = true;
            while (Listening)
            {
                try
                {
                    IPEndPoint ip = null;
                    if (DestinationIp != null)
                    {
                        ip = DestinationIp;
                    }
                    else
                    {
                        ip = new IPEndPoint(IPAddress.Any, ListeningPort);
                    }
                    log.Debug("Listening for message on " + ip);
                    byte[] bData = udp.Receive(ref ip);
                    JSON jData = new JSON(ByteUtils.byteArrayToASCIIString(bData, 4));
                    log.Debug("Got message");
                    BeginHandle(jData, ip);
                }
                catch (SocketException e)
                {
                    if (Listening)
                    {
                        throw new Exception("ERROR", e);
                    }
                    else
                    {
                        log.Debug("Interrupted");
                    }
                }
            }
            log.Debug("Thread is now dead");
        }


        public void Stop()
        {
            Listening = false;
            ThreadUtils.Pause(100);
            log.Debug("Calling udp.Client.Close()");
            udp.Client.Close();
        }
        #endregion


        #region Handle messages
        protected void BeginHandle(JSON jData, IPEndPoint ip)
        {
            Message message = MessageFactory.ParseDataToMessage(jData);
            if (message == null)
            {
                log.Error("Could not parse message data into a message: " + jData.ToString(3) + "\n\n");
                return;
            }
            else
            {
                if (message is DiscoverMessage)
                {
                    HandleDiscover((DiscoverMessage)message, ip);
                }
                else if (message is ConnectMessage)
                {
                    HandleConnect((ConnectMessage)message, ip);
                }
                else if (message is KeepAliveMessage && IsConnected)
                {
                    HandleKeepAlive((KeepAliveMessage)message, ip);
                }
                else if (message is UpdateMessage && IsConnected)
                {
                    HandleUpdate((UpdateMessage)message, ip);
                }
                else if (message is DisconnectMessage && IsConnected)
                {
                    HandleDisconnect((DisconnectMessage)message, ip);
                }
                else
                {
                    HandleGenericMessage(message, ip);
                }
            }
        }


        protected void HandleDiscover(DiscoverMessage message, IPEndPoint ip)
        {
            if (message.Exists && message.Name == Name && DestinationIp == null)
            {
                DestinationIp = ip;
                DestinationIp.Port = DestinationPort;
            }
        }


        protected void HandleConnect(ConnectMessage message, IPEndPoint ip)
        {
            if (message.Accept)
            {
                string name = message.Name;
                if (name == Name)
                {
                    IsConnected = true;
                    TimeToLive = message.TimeToLive;
                    // something with Encryption ServerPublicKey = message.PublicKey;
                }
                else
                {
                    log.Error("Names did not match: " + Name + "!=" + name);
                }
            }
        }


        protected void HandleKeepAlive(KeepAliveMessage message, IPEndPoint ip)
        {
            // shouldn't ever get a keep alive message
            log.Error("Got a keep-alive message for some reason from " + message.Name);
        }


        protected void HandleUpdate(UpdateMessage message, IPEndPoint ip)
        {
            if (message.Name == null)
            {
                log.Error("Name was null");
            }
            else if (simulatedClients.ContainsKey(message.Name))
            {
                simulatedClients[message.Name].Data = message.Data;
            }
            else
            {
                lock (simulatedClients)
                {
                    simulatedClients.Add(message.Name, new SimulatedClients(message.Name, message.Data));
                }
            }
        }


        protected void HandleDisconnect(DisconnectMessage message, IPEndPoint ip)
        {
            if (message.Name == null)
            {
                log.Error("Name was null");
            }
            else if (message.Name == Name)
            {
                if (message.Reason == "dead")
                {
                    // handle when a client has died from a disconnect
                    if (OnTimeout != null)
                    {
                        OnTimeout();
                    }
                }
            }
            else if (simulatedClients.ContainsKey(message.Name))
            {
                lock (simulatedClients)
                {
                    log.Info("Removing simulated client: " + message.Name);
                    simulatedClients.Remove(message.Name);
                    if (OnSimClientDisconnect != null)
                    {
                        OnSimClientDisconnect(message.Data);
                    }
                }
            }
            else
            {
                log.Error("Name did not exist: " + message.Name);
            }
        }


        protected virtual void HandleGenericMessage(Message message, IPEndPoint ip)
        {
            log.Debug("Got generic message type: " + message.Data.ToString(3) + "\n\n");
        }
        #endregion
    }
}
