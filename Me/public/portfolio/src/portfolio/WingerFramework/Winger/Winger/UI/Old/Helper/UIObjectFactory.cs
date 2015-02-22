using Winger.UI.UI;
using Winger.UI.UI.Tags;

namespace Winger.UI.Helper
{
    public static class UIObjectFactory
    {
        public static UIObject CreateGeneric()
        {
            return new UIObject();
        }

        public static UIObject CreateWithTag(string tag)
        {
            UIObject ui;
            switch (tag)
            {
                case "slider":
                    ui = new Slider();
                    break;

                case "label":
                    ui = new Label();
                    break;

                case "info":
                    ui = new Info();
                    break;

                case "dragdrop":
                    ui = new DragDrop();
                    break;

                case "textbox":
                    ui = new TextBox();
                    break;

                default:
                    ui = new UIObject();
                    break;
            }
            ui.Tag = tag;
            return ui;
        }
    }
}
