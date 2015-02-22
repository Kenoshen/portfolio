using System;
using System.Net;
using System.Net.Sockets;

namespace Winger.Network.Utils
{
    /// <summary>
    /// A wrapper for the System.Net.Socket class that simplifies the Socket class for connectionless UDP sockets
    /// </summary>
    public class DatagramSocket
    {
        private Socket socket = null;

        /// <summary>
        /// Sends the given DatagramPacket using connectionless UDP
        /// </summary>
        /// <param name="packet">the DatagramPacket to be sent</param>
        public static void Send(DatagramPacket packet)
        {
            IPEndPoint ep = new IPEndPoint(packet.getDST_IPAddress(), packet.DST_Port);
            Socket socket = new Socket(ep.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            socket.SendTo(packet.Data, ep);
            socket.Close();
        }

        /// <summary>
        /// Listens to a given port for UDP messages that are within the given maximum byte size and returns 
        /// a DatagramPacket constructed from the sender's IP address, the current listening port, and the 
        /// byte array data from the UDP message.  Only supports blocking.
        /// </summary>
        /// <param name="maxSize">the maximum number of bytes in a message that might be received</param>
        /// <param name="port">the port to listen to</param>
        /// <returns>the DatagramPacket constructed with the sender's IP address, the current listening port, and the byte array data from the UDP message</returns>
        public DatagramPacket Listen(int maxSize, int port)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);
            if (socket == null)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.Bind(ep);
            }
            DatagramPacket dgp = null;
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint senderRemote = (EndPoint)sender;

            dgp = new DatagramPacket(null, null, null, null, new byte[maxSize]);

            socket.ReceiveFrom(dgp.Data, ref senderRemote);
            dgp.SRC_IP = senderRemote.ToString().Split(':')[0];
            dgp.SRC_Port = Int32.Parse(senderRemote.ToString().Split(':')[1]);
            dgp.DST_IP = socket.LocalEndPoint.ToString().Split(':')[0];
            dgp.DST_Port = Int32.Parse(socket.LocalEndPoint.ToString().Split(':')[1]);

            return dgp;
        }

        public void Close()
        {
            if (socket != null)
                socket.Close();
        }
    }
}
