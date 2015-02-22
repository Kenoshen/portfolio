using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Winger.UI.HTML;
using Winger.UI.CSS;
using Winger.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace Winger.UI
{
    public class UIScreen
    {
        public string Name = "";
        public string ParentPath = "";
        public string FullPathName
        {
            get { return ParentPath + Name + "/"; }
        }


        public HTMLElement RootHTMLElement = null;
        public List<CSSElement> CSSElements = new List<CSSElement>();


        #region Building
        public List<string> DiscoverLinks()
        {
            List<string> links = new List<string>();
            foreach (HTMLElement elem in RootHTMLElement)
            {
                if (elem.Tag == HTMLTag.LINK)
                {
                    if (elem.GetAttribute("href") != null)
                    {
                        links.Add(elem.GetAttribute("href"));
                    }
                }
            }
            return links;
        }


        public void ApplyCSSToHTML()
        {
            Dictionary<string, CSSElement> cssForSearchName = new Dictionary<string, CSSElement>();
            foreach (CSSElement cssElm in CSSElements)
            {
                if (cssElm != null)
                {
                    foreach (string searchName in cssElm.GetSearchNames())
                    {
                        if (searchName != null)
                        {
                            cssForSearchName[searchName] = cssElm;
                        }
                    }
                }
            }
            List<string> cssById = new List<string>();
            List<string> cssByClass = new List<string>();
            List<string> cssByTagPath = new List<string>();
            List<string> cssByTag = new List<string>();
            List<string> cssByGlobal = new List<string>();

            foreach (string name in cssForSearchName.Keys)
            {
                if (name.StartsWith("#"))
                {
                    cssById.Add(name);
                }
                else if (name.StartsWith("."))
                {
                    cssByClass.Add(name);
                }
                else if (name.StartsWith("*"))
                {
                    cssByGlobal.Add(name);
                }
                else if (name.Contains(" "))
                {
                    cssByTagPath.Add(name);
                }
                else
                {
                    cssByTag.Add(name);
                }
            }
            List<string> combineCSSLists = new List<string>();
            combineCSSLists.AddRange(cssByGlobal);
            combineCSSLists.AddRange(cssByTag);
            combineCSSLists.AddRange(cssByTagPath);
            combineCSSLists.AddRange(cssByClass);
            combineCSSLists.AddRange(cssById);

            foreach (string name in combineCSSLists)
            {
                CSSElement cssElement = cssForSearchName[name];
                foreach (HTMLElement htmlElement in RootHTMLElement)
                {
                    if (DoesCSSApply(name, htmlElement))
                    {
                        htmlElement.ApplyCSSElement(cssElement);
                    }
                }
            }
        }


        public static bool DoesCSSApply(string name, HTMLElement element)
        {
            if (element != null)
            {
                name = name.Trim();
                if (name.Contains(" "))
                {
                    // have to figure out how to do path stuff
                }
                else if (name.StartsWith("*"))
                {
                    return true;
                }
                else if (name.StartsWith("."))
                {
                    return (name.Replace(".", "").Equals(element.GetAttribute("class")));
                }
                else if (name.StartsWith("#"))
                {
                    string elemClassAtt = element.GetAttribute("id");
                    if (elemClassAtt != null && !elemClassAtt.Equals(""))
                    {
                        return (elemClassAtt.Contains(name.Replace("#", "")));
                    }
                }
                else if (name.Equals(element.Tag))
                {
                    return true;
                }
            }
            return false;
        }


        public void BuildRenderTree()
        {
            foreach (HTMLElement element in RootHTMLElement)
            {
                element.GenerateMarginRect();
            }
            HTMLElement headElement = null;
            foreach (HTMLElement element in RootHTMLElement)
            {
                // NOLONGERATODO: figure out if this is working properly
                element.GenerateRenderTree();
                if (element.Tag == HTMLTag.HEAD)
                {
                    headElement = element;
                }
            }
            if (headElement != null)
            {
                foreach (HTMLElement element in headElement)
                {
                    element.WillDrawInRenderTree = false;
                }
            }
        }
        #endregion


        public void SetDebug(bool debug)
        {
            foreach (HTMLElement element in RootHTMLElement)
            {
                element.Debug = debug;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (HTMLElement element in RootHTMLElement)
            {
                element.Draw(spriteBatch);
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("##############################################\n");
            sb.Append("NAME: ").Append(Name).Append("\n");
            sb.Append(RootHTMLElement).Append("\n\n\n");
            sb.Append(ListUtils.ListToString<CSSElement>(CSSElements)).Append("\n");
            sb.Append("##############################################");
            return sb.ToString();
        }
    }
}
