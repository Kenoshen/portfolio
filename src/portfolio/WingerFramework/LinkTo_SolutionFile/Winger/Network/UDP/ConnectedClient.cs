using System.Net;
using Winger.Utils;

namespace Winger.Network.UDP
{
    public class ConnectedClient
    {
        public string Name { get; set; }
        public IPEndPoint Ip { get; set; }
        public JSON Data { get; set; }
        public bool IsDirty { get; set; }
        public long TimeToLive { get; set; }
        public long CurrentTimestamp { get; set; }

        public ConnectedClient(string name, IPEndPoint ip, JSON data)
        {
            IsDirty = true;
            Name = name;
            Ip = ip;
            Data = data;
            TimeToLive = 10000; // default to 10 seconds
            CurrentTimestamp = Timestamp.Now;
        }

        public bool IsDead(long now)
        {
            return (now - CurrentTimestamp > TimeToLive);
        }

        public bool IsDead()
        {
            return IsDead(Timestamp.Now);
        }
    }
}
