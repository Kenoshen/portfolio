using Winger.Utils;

namespace Winger.Network.UDP
{
    public class SimulatedClients
    {
        public string Name { get; set; }
        public JSON Data { get; set; }
        public long TimeToLive { get; set; }
        public long CurrentTimestamp { get; set; }

        public SimulatedClients(string name, JSON data)
        {
            Name = name;
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
