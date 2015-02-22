using System;
using Winger.Utils;

namespace Winger.Network.UDP.Messages
{
    public class DiscoverMessage : Message
    {
        public bool Exists
        {
            get { return Convert.ToBoolean(Data.Get("exists")); }
            set { Data.Put("exists", value); }
        }

        public DiscoverMessage(JSON jdata)
        {
            Initialize(jdata);
        }
    }
}
