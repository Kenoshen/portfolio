using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;

namespace Winger.Network.Utils
{
    public static class IPUtils
    {
        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }


        public static string PublicIPAddress()
        {
            // send http request to the url to get public ip address
            string url = "http://checkip.dyndns.org/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();

            // read stream into byte array
            int curByte = resStream.ReadByte();
            List<int> bytes = new List<int>();
            while (curByte != -1)
            {
                bytes.Add(curByte);
                curByte = resStream.ReadByte();
            }
            byte[] buffer = new byte[bytes.Count];
            for(int i = 0; i < bytes.Count; i++)
            {
                buffer[i] = (byte)bytes[i];
            }

            // format is 'Current IP Address: 207.93.212.56'
            string ipResponse = ByteUtils.byteArrayToASCIIString(buffer);
            if (ipResponse.Contains(":"))
            {
                return ipResponse.Split(':')[1].Split('<')[0].Trim();
            }
            else
            {
                return ipResponse;
            }
        }
    }
}
