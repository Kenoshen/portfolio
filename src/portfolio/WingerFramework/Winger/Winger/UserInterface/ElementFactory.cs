using Winger.UserInterface.ElementImpl;
using Winger.Utils;

namespace Winger.UserInterface
{
    public static class ElementFactory
    {
        public static Element CreateElement(JSON json, JSON settings, JSON globals, Element parent)
        {
            string type = (string) json.Get("type");
            if (type == null)
            {
                type = "default";
            }
            Element elem = null;
            switch (type)
            {
                case "grid":
                    elem = new GridElement(json, settings, globals, parent);
                    break;

                case "textbox":
                    elem = new TextBoxElement(json, settings, globals, parent);
                    break;

                default:
                    elem = new DefaultElement(json, settings, globals, parent);
                    break;
            }
            elem.Initialize();
            return elem;
        }
    }
}
