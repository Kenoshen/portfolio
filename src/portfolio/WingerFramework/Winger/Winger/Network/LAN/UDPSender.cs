using System;
using Winger.Network.Utils;

namespace Winger.Network.LAN
{
    public static class UDPSender
    {
        /// <summary>
        /// Sends a given byteable object in a byte[] form over connectionless UDP 
        /// with the default IP address of 127.0.0.1 and default port 8080
        /// </summary>
        /// <param name="data">the object to send in byte[] form</param>
        public static void Send(Byteable data)
        {
            Send(data, null, null);
        }

        /// <summary>
        /// Sends a given byteable object in a byte[] form over connectionless UDP 
        /// with the given IP address and port
        /// </summary>
        /// <param name="data">the byteable object to send in byte[] form</param>
        /// <param name="ip">the IP address to send the data to, null uses 127.0.0.1</param>
        /// <param name="port">the port to use to send the data, null uses 8080</param>
        public static void Send(Byteable data, String ip, Nullable<int> port)
        {
            if (data != null)
                DatagramSocket.Send(new DatagramPacket(ip, ip, port, port, data.ToByteArray()));
        }

        /// <summary>
        /// Sends a byte[] over connectionless UDP with the default 
        /// IP address of 127.0.0.1 and default port 8080
        /// </summary>
        /// <param name="data">the byteable object to send in byte[] form</param>
        public static void Send(byte[] data)
        {
            Send(data, null, null);
        }

        /// <summary>
        /// Sends a byte[] over connectionless UDP with the given IP address and port
        /// </summary>
        /// <param name="data">the byteable object to send in byte[] form</param>
        /// <param name="ip">the IP address to send the data to, null uses 127.0.0.1</param>
        /// <param name="port">the port to use to send the data, null uses 8080</param>
        public static void Send(byte[] data, String ip, Nullable<int> port)
        {
            DatagramSocket.Send(new DatagramPacket(ip, ip, port, port, data));
        }
    }
}
