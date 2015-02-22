using Winger.Utils;

namespace Winger.Network.UDP.Messages
{
    public abstract class Message
    {
        public string Type
        {
            get { return (string) Data.Get("type"); }
            set { Data.Put("type", value); }
        }
        public string Name
        {
            get { return (string) Data.Get("name"); }
            set { Data.Put("name", value); }
        }
        public JSON Data { get; set; }


        protected void Initialize(JSON jdata)
        {
            Data = jdata;
        }
    }
}
