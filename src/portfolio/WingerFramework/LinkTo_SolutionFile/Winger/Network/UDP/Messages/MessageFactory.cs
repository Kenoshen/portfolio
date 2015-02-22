using Winger.Utils;

namespace Winger.Network.UDP.Messages
{
    public static class MessageFactory
    {
        private static Log log = new Log("[MESSAGE FACTORY] ");

        public static Message ParseDataToMessage(JSON data)
        {
            Message message = null;
            if (data == null)
            {
                log.Error("Data was null, could not parse");
                return message;
            }
            string type = (string)data.Get("type");
            if (type == null)
            {
                log.Error("Type was null: " + data.ToString(3));
            }
            if (type == "")
            {
                log.Error("Type was empty: " + data.ToString(3));
            }
            string name = (string)data.Get("name");
            if (name == null)
            {
                log.Error("Name was null: " + data.ToString(3));
                type = "";
            }
            if (name == "")
            {
                log.Error("Name was empty: " + data.ToString(3));
                type = "";
            }
            switch (type)
            {
                case "discover":
                    log.Debug("Message type was Discover");
                    message = new DiscoverMessage(data);
                    break;

                case "connect":
                    log.Debug("Message type was Connect");
                    message = new ConnectMessage(data);
                    break;

                case "keepalive":
                    log.Debug("Message type was KeepAlive");
                    message = new KeepAliveMessage(data);
                    break;

                case "update":
                    log.Debug("Message type was Update");
                    message = new UpdateMessage(data);
                    break;

                case "disconnect":
                    log.Debug("Message type was Disconnect");
                    message = new DisconnectMessage(data);
                    break;
            }
            return message;
        }

        public static DiscoverMessage ToDiscoverMessage(string name)
        {
            JSON data = new JSON("{}");
            data.Put("type", "discover");
            data.Put("name", name);
            DiscoverMessage mes = (DiscoverMessage)ParseDataToMessage(data);
            mes.Exists = false;
            return mes;
        }

        public static ConnectMessage ToConnectMessage(string name)
        {
            JSON data = new JSON("{}");
            data.Put("type", "connect");
            data.Put("name", name);
            ConnectMessage mes = (ConnectMessage)ParseDataToMessage(data);
            mes.Accept = false;
            return mes;
        }

        public static KeepAliveMessage ToKeepAliveMessage(string name)
        {
            JSON data = new JSON("{}");
            data.Put("type", "keepalive");
            data.Put("name", name);
            return (KeepAliveMessage)ParseDataToMessage(data);
        }

        public static UpdateMessage ToUpdateMessage(string name)
        {
            JSON data = new JSON("{}");
            data.Put("type", "update");
            data.Put("name", name);
            return (UpdateMessage)ParseDataToMessage(data);
        }

        public static DisconnectMessage ToDisconnectMessage(string name)
        {
            JSON data = new JSON("{}");
            data.Put("type", "disconnect");
            data.Put("name", name);
            return (DisconnectMessage)ParseDataToMessage(data);
        }
    }
}
