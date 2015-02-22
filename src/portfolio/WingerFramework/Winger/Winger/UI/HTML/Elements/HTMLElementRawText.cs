using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Winger.UI.HTML.Elements
{
    public class HTMLElementRawText : HTMLElement
    {
        public HTMLElementRawText(HTMLTag tag)
        {
            Tag = tag;
        }


        public HTMLElementRawText(HTMLTag tag, string rawText)
        {
            Tag = tag;
            Text = rawText;
        }
    }
}
