using System;
using Winger.Utils;

namespace Winger.Network.UDP.Messages
{
    public class UpdateMessage : Message
    {
        public long CurrentTimestamp
        {
            get { return Convert.ToInt64(Data.Get("ts")); }
            set { Data.Put("ts", value); }
        }

        public UpdateMessage(JSON jdata)
        {
            Initialize(jdata);
        }
    }
}
