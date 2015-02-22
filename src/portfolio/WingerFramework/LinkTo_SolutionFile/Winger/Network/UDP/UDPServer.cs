using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Winger.Network.Utils;
using Winger.Utils;
using Winger.Network.UDP.Messages;

namespace Winger.Network.UDP
{
    public class UDPServer
    {
        protected Log log = new Log("[SERVER] ", LogLevel.DEBUG);

        protected UdpClient udp;
        protected bool running;
        protected Dictionary<string, ConnectedClient> connected = new Dictionary<string, ConnectedClient>();

        public string Ip { get; private set; }
        public int Port { get; set; }

        public long TimeToLive { get; set; }
        protected long currentTime = 0;

        protected List<string> deadClients = new List<string>();

        protected Thread runningThread;

        public UDPServer(int port)
        {
            Port = port;
            Ip = IPUtils.LocalIPAddress();
            TimeToLive = 10000; // default to 10 seconds
            currentTime = Timestamp.Now;
        }


        public void Start()
        {
            udp = new UdpClient(Port);

            runningThread = ThreadUtils.StartUpInThread(Run);
        }


        protected void Run()
        {
            log.Info("Server is up and running at " + Ip + ":" + Port);
            running = true;
            while (running)
            {
                try
                {
                    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, Port);
                    byte[] data = udp.Receive(ref ipEndPoint);
                    JSON jData = new JSON(ByteUtils.byteArrayToASCIIString(data, 4));
                    log.Debug("Got message from " + jData.Get("name"));
                    BeginHandle(jData, ipEndPoint);
                }
                catch (SocketException e)
                {
                    if (running == true)
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
            log.Debug("Kill thread");
            running = false;
            ThreadUtils.Pause(100);
            log.Debug("Calling udp.Client.Close()");
            udp.Client.Close();
        }


        public void Update()
        {
            if (connected.Count > 0)
            {
                currentTime = Timestamp.Now;
                lock (connected)
                {
                    foreach (KeyValuePair<string, ConnectedClient> clientA in connected)
                    {
                        if (clientA.Value.IsDirty)
                        {
                            byte[] data = clientA.Value.Data.ToByteArray();
                            foreach (KeyValuePair<string, ConnectedClient> clientB in connected)
                            {
                                if (clientA.Key != clientB.Key)
                                {
                                    // send the updated clientA data to clientB
                                    log.Debug("Sent update message");
                                    udp.Send(data, data.Length, clientB.Value.Ip);
                                }
                            }
                            clientA.Value.IsDirty = false;
                        }
                        if (clientA.Value.IsDead(currentTime))
                        {
                            deadClients.Add(clientA.Value.Name);
                        }
                    }
                    for (int i = 0; i < deadClients.Count; i++)
                    {
                        DisconnectMessage message = MessageFactory.ToDisconnectMessage(deadClients[i]);
                        message.Reason = "dead";
                        IPEndPoint ip = connected[message.Name].Ip;
                        SendDisconnectMessage(message, ip);
                        deadClients.RemoveAt(i);
                        i--;
                    }
                }
            }
        }


        #region Message Handlers
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
                else if (message is KeepAliveMessage)
                {
                    HandleKeepAlive((KeepAliveMessage)message, ip);
                }
                else if (message is UpdateMessage)
                {
                    HandleUpdate((UpdateMessage)message, ip);
                }
                else if (message is DisconnectMessage)
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
            message.Exists = true;
            byte[] resp = message.Data.ToByteArray();
            log.Debug("Sent discover response");
            udp.Send(resp, resp.Length, ip);
        }

        protected void HandleConnect(ConnectMessage message, IPEndPoint ip)
        {
            lock (connected)
            {
                if (!connected.ContainsKey(message.Name))
                {
                    log.Info("Connected: " + message.Name);
                    connected.Add(message.Name, new ConnectedClient(message.Name, ip, message.Data));

                    message.Accept = true;
                    message.TimeToLive = TimeToLive;
                    // do something with encryption here: message.PublicKey = pubkey;
                    byte[] resp = message.Data.ToByteArray();
                    log.Debug("Sent connect response");
                    udp.Send(resp, resp.Length, ip);

                    ThreadUtils.Pause(100);

                    // TODO: should send around a connect message to the other clients

                    foreach (string key in connected.Keys)
                    {
                        connected[key].IsDirty = true;
                    }
                }
                else
                {
                    log.Error("Already connected: " + message.Name);
                    message.Accept = false;
                    byte[] resp = message.Data.ToByteArray();
                    udp.Send(resp, resp.Length, ip);
                }
            }
        }

        protected void HandleKeepAlive(KeepAliveMessage message, IPEndPoint ip)
        {
            lock (connected)
            {
                if (connected.ContainsKey(message.Name))
                {
                    UpdateCurrentTimestamp(message.CurrentTimestamp, message.Name);
                }
                else
                {
                    log.Error("Not connected: " + message.Name);
                }
            }
        }

        protected void HandleUpdate(UpdateMessage message, IPEndPoint ip)
        {
            lock (connected)
            {
                if (connected.ContainsKey(message.Name))
                {
                    connected[message.Name].Data = message.Data;
                    connected[message.Name].IsDirty = true;

                    UpdateCurrentTimestamp(message.CurrentTimestamp, message.Name);
                }
                else
                {
                    log.Error("Not connected: " + message.Name);
                }
            }
        }

        protected void HandleDisconnect(DisconnectMessage message, IPEndPoint ip)
        {
            lock (connected)
            {
                if (connected.ContainsKey(message.Name))
                {
                    message.Reason = "user";
                    SendDisconnectMessage(message, ip);
                }
                else
                {
                    log.Error("Not connected: " + message.Name);
                }
            }
        }

        protected virtual void HandleGenericMessage(Message message, IPEndPoint ip)
        {
            log.Debug("Got generic message type: " + message.Data.ToString(3) + "\n\n");
        }

        protected void SendDisconnectMessage(DisconnectMessage message, IPEndPoint ip)
        {
            byte[] byteData = message.Data.ToByteArray();
            foreach (KeyValuePair<string, ConnectedClient> clientB in connected)
            {
                log.Debug("Sent disconnect message for " + message.Name + " to " + clientB.Value.Name);
                udp.Send(byteData, byteData.Length, clientB.Value.Ip);
            }
            connected.Remove(message.Name);
            log.Info("Disconnected " + message.Name);
        }

        protected void UpdateCurrentTimestamp(long ts, string name)
        {
            if (ts > currentTime)
            {
                ts = currentTime;
            }
            if (connected[name].CurrentTimestamp < ts)
            {
                log.Debug("Updated current timestamp for " + name + " to " + ts);
                connected[name].CurrentTimestamp = ts;
            }
        }
        #endregion
    }
}
