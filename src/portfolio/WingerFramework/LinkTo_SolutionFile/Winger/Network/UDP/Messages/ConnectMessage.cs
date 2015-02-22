using System;
using Winger.Utils;

namespace Winger.Network.UDP.Messages
{
    public class ConnectMessage : Message
    {
        public bool Accept
        {
            get { return Convert.ToBoolean(Data.Get("accept")); }
            set { Data.Put("accept", value); }
        }
        
        public string PublicKey
        {
            get { return (string)(Data.Get("pubkey")); }
            set { Data.Put("pubkey", value); }
        }

        public long TimeToLive
        {
            get { return Convert.ToInt64(Data.Get("ttl")); }
            set { Data.Put("ttl", value); }
        }

        public ConnectMessage(JSON jdata)
        {
            Initialize(jdata);
        }
    }
}
