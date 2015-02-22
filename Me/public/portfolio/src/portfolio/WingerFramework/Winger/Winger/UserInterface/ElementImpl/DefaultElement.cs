using Winger.Utils;

namespace Winger.UserInterface.ElementImpl
{
    public class DefaultElement : Element
    {
        public DefaultElement(string json) : base(json) { }

        public DefaultElement(JSON json) : base(json) { }

        public DefaultElement(JSON json, JSON settings, JSON globals) : base(json, settings, globals) { }

        public DefaultElement(JSON json, JSON settings, JSON globals, Element parent) : base(json, settings, globals, parent) { }
    }
}
