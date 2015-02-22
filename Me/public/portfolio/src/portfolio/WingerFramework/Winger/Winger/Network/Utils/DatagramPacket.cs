using System;
using System.Net;

namespace Winger.Network.Utils
{
    /// <summary>
    /// A UDP packet based on the Java DatagramPacket class.  Used to send data accross UDP connections.
    /// </summary>
    public class DatagramPacket
    {
        private IPAddress src_ip = null;
        private IPAddress dst_ip = null;
        private int src_port = 8080;
        private int dst_port = 8080;
        private byte[] data = new byte[0];

        /// <summary>
        /// The source IP address of the datagram packet. Defaults to "127.0.0.1"
        /// </summary>
        public String SRC_IP
        {
            set
            {
                try
                {
                    src_ip = IPAddress.Parse(value);
                }
                catch (Exception)
                {
                    src_ip = IPAddress.Parse("127.0.0.1");
                }
            }
            get
            {
                if (src_ip == null)
                    src_ip = IPAddress.Parse("127.0.0.1");
                return src_ip.ToString();
            }
        }

        /// <summary>
        /// The destination IP address of the datagram packet. Defaults to "127.0.0.1"
        /// </summary>
        public String DST_IP
        {
            set
            {
                try
                {
                    dst_ip = IPAddress.Parse(value);
                }
                catch (Exception)
                {
                    dst_ip = IPAddress.Parse("127.0.0.1");
                }
            }
            get
            {
                if (dst_ip == null)
                    dst_ip = IPAddress.Parse("127.0.0.1");
                return dst_ip.ToString();
            }
        }

        /// <summary>
        /// The source port of the datagram packet. Defaults to 8080
        /// </summary>
        public int SRC_Port
        {
            set { src_port = value; }
            get { return src_port; }
        }

        /// <summary>
        /// The destination port of the datagram packet. Defaults to 8080
        /// </summary>
        public int DST_Port
        {
            set { dst_port = value; }
            get { return dst_port; }
        }

        /// <summary>
        /// The byte data of the incoming message when received and of the outgoing message when sent. Defaults to new byte [0]
        /// </summary>
        public byte[] Data
        {
            set { data = value; }
            get { return data; }
        }

        /// <summary>
        /// Constructs a DatagramPacket with the default values for IP address, port, and message data.
        /// </summary>
        public DatagramPacket()
        {
            Initialize(null, null, null, null, null);
        }

        /// <summary>
        /// Constructs a DatagramPacket with the given IP addresses, ports, and message data.  Any of the  
        /// values are allowed to be null.  If null is given then the parameter uses the default value.
        /// </summary>
        /// <param name="src_ip">the source IP address. Defaults to "127.0.0.1"</param>
        /// <param name="dst_ip">the destination IP address. Defaults to "127.0.0.1"</param>
        /// <param name="src_port">the source port. Defaults to 8080</param>
        /// <param name="dst_port">the destination port. Defaults to 8080</param>
        /// <param name="data">The byte array data. Defaults to new byte[0]</param>
        public DatagramPacket(String src_ip, String dst_ip, Nullable<int> src_port, Nullable<int> dst_port, byte[] data)
        {
            Initialize(src_ip, dst_ip, src_port, dst_port, data);
        }

        private void Initialize(String src_ip, String dst_ip, Nullable<int> src_port, Nullable<int> dst_port, byte[] data)
        {
            if (src_ip != null)
                SRC_IP = src_ip;
            if (dst_ip != null)
                DST_IP = dst_ip;
            if (src_port != null)
                SRC_Port = src_port.Value;
            if (dst_port != null)
                DST_Port = dst_port.Value;
            if (data != null)
                Data = data;
        }

        /// <summary>
        /// Gets the source IPAddress object for the IP string that the DatagramPacket contains
        /// </summary>
        /// <returns>the IPAddress object for the IP string that the DatagramPacket contains</returns>
        public IPAddress getSRC_IPAddress()
        {
            return src_ip;
        }

        /// <summary>
        /// Gets the destination IPAddress object for the IP string that the DatagramPacket contains
        /// </summary>
        /// <returns>the IPAddress object for the IP string that the DatagramPacket contains</returns>
        public IPAddress getDST_IPAddress()
        {
            return dst_ip;
        }

        public override string ToString()
        {
            return "[src_" + SRC_IP + ":" + SRC_Port + " dst_" + DST_IP + ":" + DST_Port + " data:\n" + ByteUtils.byteArrayToHexTable(Data) + "]";
        }
    }
}
