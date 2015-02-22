using Winger.Utils;

namespace Winger.Network.UDP.Messages
{
    public class DisconnectMessage : Message
    {
        public string Reason
        {
            get { return (string)(Data.Get("reason")); }
            set { Data.Put("reason", value); }
        }

        public DisconnectMessage(JSON jdata)
        {
            Initialize(jdata);
        }
    }
}
