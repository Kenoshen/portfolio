using System.Collections.Generic;
using System.IO;
using System.Xml;
using Winger.Utils;
using Winger.UI.CSS;
using Winger.UI.HTML.Elements;

namespace Winger.UI.HTML
{
    public static class HTMLUtils
    {
        public static HTMLElement ParseHTMLFile(string filePath)
        {
            string absolutePath = FileUtils.AbsoluteFilePath(filePath);
            string parentDir = FileUtils.FilePathToDir(absolutePath);
            string fileStr = FileUtils.FileToString(absolutePath);
            StreamReader file = new StreamReader(absolutePath);
            XmlReader reader = XmlTextReader.Create(file);
            Stack<HTMLElement> elementStack = new Stack<HTMLElement>();
            HTMLElement lastObj = null;

            while (!reader.EOF)
            {
                if (!reader.IsEmptyElement)
                {
                    if (reader.IsStartElement())
                    {
                        HTMLElement parent = null;
                        HTMLElement curElm = ElementFromTag(reader.Name);
                        curElm.ParentDirectory = parentDir;
                        if (elementStack.Count > 0)
                        {
                            parent = elementStack.Peek();
                            curElm.Parent = parent;
                            parent.Children.Add(curElm);
                        }
                        elementStack.Push(curElm);
                        //HTMLElement[] objArr = elementStack.ToArray();
                        //HTMLElement[] revArr = new HTMLElement[objArr.Length];
                        //for (int i = 0; i < objArr.Length; i++)
                        //    revArr[(objArr.Length - 1) - i] = objArr[i];
                        //tree.AddBranch(revArr);
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        lastObj = elementStack.Pop();
                    }
                    else if (reader.NodeType == XmlNodeType.Text)
                    {
                        string textVal = reader.Value;
                        HTMLElement parent = elementStack.Peek();
                        parent.Text += textVal;
                        HTMLElement curElm = new HTMLElementRawText(HTMLTag.T, textVal);
                        curElm.Parent = parent;
                        parent.Children.Add(curElm);
                    }

                    for (int i = 0; i < reader.AttributeCount; i++)
                    {
                        reader.MoveToAttribute(i);
                        elementStack.Peek().AddAttribute(reader.Name, reader.Value);
                    }
                }

                reader.Read();
            }

            reader.Close();
            file.Close();

            return lastObj;
        }

        public static HTMLElement ElementFromTag(string tagStr)
        {
            HTMLTag tag = HTMLTagUtils.GetCSSPropertyForString(tagStr);
            HTMLElement element = null;
            if (tag == HTMLTag.NONE)
            {
                return null;
            }
            else if (tag == HTMLTag.HTML ||
                tag == HTMLTag.HEAD ||
                tag == HTMLTag.BODY ||
                tag == HTMLTag.DIV)
            {
                element = new HTMLElementContainer(tag);
            }
            else if (tag == HTMLTag.TITLE ||
                tag == HTMLTag.H1 ||
                tag == HTMLTag.H2 ||
                tag == HTMLTag.H3 ||
                tag == HTMLTag.P ||
                tag == HTMLTag.B ||
                tag == HTMLTag.I)
            {
                element = new HTMLElementText(tag);
            }
            else if (tag == HTMLTag.INPUT)
            {
                element = new HTMLElementInput(tag);
            }
            else if (tag == HTMLTag.A ||
                tag == HTMLTag.LINK)
            {
                element = new HTMLElementLink(tag);
            }
            else if (tag == HTMLTag.IMG)
            {
                element = new HTMLElementImage(tag);
            }
            else if (tag == HTMLTag.T)
            {
                element = new HTMLElementRawText(tag);
            }
            return element;
        }
    }
}
