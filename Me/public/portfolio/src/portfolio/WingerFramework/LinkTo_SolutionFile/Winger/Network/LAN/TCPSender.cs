using System;
using Winger.Network.Utils;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Winger.Network.LAN
{
    public class TCPSender
    {
        private TcpClient client;
        private Stream stream;

        /// <summary>
        /// The destination of the messages to send
        /// </summary>
        public String DestinationIp { get; set; }

        /// <summary>
        /// The port of the message destination
        /// </summary>
        public int DestinationPort { get; set; }


        /// <summary>
        /// Connect to the destination ip and port
        /// </summary>
        public void Connect()
        {
            client = new TcpClient();
            Console.WriteLine("Connecting...");
            client.Connect(DestinationIp, DestinationPort);
            Console.WriteLine("Connected");
            stream = client.GetStream();
        }


        /// <summary>
        /// Closes the TCP connection
        /// </summary>
        public void Close()
        {
            client.Close();
        }


        /// <summary>
        /// Sends a given byteable object in a byte[] form over a TCP connection
        /// </summary>
        /// <param name="data">the object to send in byte[] form</param>
        public void Send(Byteable data)
        {
            Send(data.ToByteArray());
        }

        /// <summary>
        /// Sends a byte[] over a TCP connection
        /// </summary>
        /// <param name="data">the data to send</param>
        public void Send(byte[] data)
        {
            stream.Write(data, 0, data.Length);
            //stream.Flush();
        }
    }
}
