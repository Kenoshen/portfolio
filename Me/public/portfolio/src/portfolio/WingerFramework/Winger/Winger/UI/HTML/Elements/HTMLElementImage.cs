using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Winger.UI.HTML.Elements
{
    public class HTMLElementImage : HTMLElement
    {
        public string Source
        {
            get { return GetAttribute("src"); }
        }

        public HTMLElementImage(HTMLTag tag)
        {
            Tag = tag;
        }
    }
}
