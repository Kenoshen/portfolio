using System;
using System.Collections.Generic;
using System.Linq;
using Winger.UI.IMG;

namespace Winger.UI.CSS
{
    public static class CSSUtils
    {
        

        public static List<CSSElement> ParseCSSString(string cssStr, string dir)
        {
            List<CSSElement> elements = new List<CSSElement>();
            string[] split = cssStr.Split(new char[] { '{', '}' }, StringSplitOptions.None);

            for (int i = 0; i + 1 < split.Length; i += 2)
            {
                string[] names = split[i].Split(',');
                for (int k = 0; k < names.Length; k++)
                {
                    names[k] = names[k].Trim();
                }
                CSSElement element = new CSSElement(names.ToList<string>());
                element.ParentDirectory = dir;
                if (split[i + 1] != null && !split[i + 1].Equals(""))
                {
                    string[] props = split[i + 1].Split(';');
                    for (int k = 0; k < props.Length; k++)
                    {
                        if (props[k] != null)
                        {
                            props[k] = props[k].Trim();
                            if (!props[k].Equals(""))
                            {
                                string[] propsSplit = props[k].Split(new char[] { ':' }, StringSplitOptions.None);
                                string propName = propsSplit[0];
                                string propVal = propsSplit[1].Trim();
                                element.AddProperty(CSSPropertyUtils.GetCSSPropertyForString(propName), propVal);

                                // get ImgElement if applicable
                                if (propName.Contains("image"))
                                {
                                    ImgElementManager.Instance.AddImgElement(dir + "\\" + propVal);
                                }
                            }
                        }
                    }
                }

                elements.Add(element);
            }
            return elements;
        }
    }
}
