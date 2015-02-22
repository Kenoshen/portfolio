using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Winger.UserInterface.Utils;
using Winger.Utils;
using Microsoft.Xna.Framework.Input;
using Winger.Input.Raw;

namespace Winger.UserInterface
{
    public abstract class Element : IEnumerator<Element>, IEnumerable<Element>
    {
        // TODO: implement drag element (maybe combine with scroll?)

        private JSON json = null;
        private JSON pageSettings = null;
        private JSON pageGlobals = null;

        public string Id
        {
            get { return (string)Get("id"); }
            set { Put("id", value); }
        }
        public string Type
        {
            get { return (string)Get("type"); }
            set { Put("type", value); }
        }
        public float X
        {
            get { return Convert.ToSingle(Get("x")); }
            set { Put("x", value); }
        }
        public float Y
        {
            get { return Convert.ToSingle(Get("y")); }
            set { Put("y", value); }
        }
        public float Z
        {
            get { return Convert.ToSingle(Get("z")); }
            set { Put("z", value); }
        }
        public float Width
        {
            get { return Convert.ToSingle(Get("width")); }
            set { Put("width", value); }
        }
        public float Height
        {
            get { return Convert.ToSingle(Get("height")); }
            set { Put("height", value); }
        }
        public float Rotation
        {
            get { return Convert.ToSingle(Get("rotation")); }
            set { Put("rotation", value); }
        }
        public string Text
        {
            get { return (string)Get("text"); }
            set { Put("text", value); }
        }
        public Alignment Alignment
        {
            get { return (Alignment)Get("alignment"); }
            set { Put("alignment", value); }
        }
        public Alignment TextAlignment
        {
            get { return (Alignment)Get("text-alignment"); }
            set { Put("text-alignment", value); }
        }
        public Color Color
        {
            get { return (Color)Get("color"); }
            set { Put("color", value); }
        }
        public Color ColorText
        {
            get { return (Color)Get("color-text"); }
            set { Put("color-text", value); }
        }
        public Texture2D Icon
        {
            get { return (Texture2D)Get("texture-icon"); }
            set { Put("texture-icon", value); }
        }
        public Texture2D Texture
        {
            get { return (Texture2D)Get("texture"); }
            set { Put("texture", value); }
        }
        public Texture2D TextureHover
        {
            get { return (Texture2D)Get("texture-hover"); }
            set { Put("texture-hover", value); }
        }
        public Texture2D TextureSelect
        {
            get { return (Texture2D)Get("texture-select"); }
            set { Put("texture-select", value); }
        }
        public SpriteFont Font
        {
            get { return (SpriteFont)Get("font"); }
            set { Put("font", value); }
        }
        public bool IsEnabled
        {
            get { return Convert.ToBoolean(Get("is-enabled")); }
            set { Put("is-enabled", value); }
        }
        public bool IsVisible
        {
            get { return Convert.ToBoolean(Get("is-visible")); }
            set { Put("is-visible", value); }
        }
        public bool HasFocus
        {
            get { return Convert.ToBoolean(Get("has-focus")); }
            set { Put("has-focus", value); }
        }
        public string OnHoverStartEventName
        {
            get { return (string)Get("on-hover-start"); }
            set { Put("on-hover-start", value); }
        }
        public string OnHoverEndEventName
        {
            get { return (string)Get("on-hover-end"); }
            set { Put("on-hover-end", value); }
        }
        public string OnSelectStartEventName
        {
            get { return (string)Get("on-select-start"); }
            set { Put("on-select-start", value); }
        }
        public string OnSelectEndEventName
        {
            get { return (string)Get("on-select-end"); }
            set { Put("on-select-end", value); }
        }
        public string TransitionOnSelect
        {
            get { return (string)Get("transition-on-select"); }
            set { Put("transition-on-select", value); }
        }
        public Element Parent { get; set; }
        public List<Element> Children
        {
            get { return (List<Element>)Get("children"); }
            set { Put("children", value); }
        }


        protected int enumeratorPosition = 0;
        protected List<Element> enumeratorList = new List<Element>();


        protected Texture2D tex = null;
        protected bool isHover = false;
        protected bool isSelected = false;


        public Element(string json) : this(new JSON(json)) { }


        public Element(JSON json) : this(json, new JSON("{}"), new JSON("{}"), null) { }


        public Element(JSON json, JSON settings, JSON globals) : this(json, settings, globals, null) { }


        public Element(JSON json, JSON settings, JSON globals, Element parent)
        {
            this.json = json;
            this.pageSettings = settings;
            this.pageGlobals = globals;
            Parent = parent;
        }


        public virtual void Initialize()
        {
            ParseGlobals();
            ParseSettings();
            AddDefaultValues("x", 0, "y", 0, "z", 1, "width", 0, "height", 0, 
                "rotation", 0, "is-enabled", true, "is-visible", true, 
                "has-focus", false);
            ParsePercentages("x", "width", "y", "height", "width", "width", "height", "height", "rotation", "rotation");
            ParseAlignments(GetAllPropertiesThatContain("alignment"));
            ParseColors(GetAllPropertiesThatContain("color"));
            ParseTextures(GetAllPropertiesThatContain("texture"));
            ParseFonts(GetAllPropertiesThatContain("font"));
            AddDefaultValues("alignment", Alignment.CENTER, "text-alignment", Alignment.CENTER, "color", Color.White, "color-text", Color.Black,
                "texture", null, "texture-hover", null, "texture-select", null, "font", null);
            ParseChildren();
        }


        public object Get(string xpath)
        {
            return json.Get(xpath);
        }


        public void Put(string xpath, object val)
        {
            json.Put(xpath, val);
        }
        

        public override string ToString()
        {
            return json.ToString();
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                tex = null;
                if (IsNeutral())
                    tex = Texture;
                else if (IsHover())
                    tex = TextureHover;
                else if (IsSelected())
                    tex = TextureSelect;

                if (IsHover() && TextureHover == null)
                    tex = Texture;
                else if (IsSelected() && TextureSelect == null)
                    tex = Texture;

                CRectangle absoluteRect = GetAbsoluteBoundingBox();
                if (tex != null)
                {
                    spriteBatch.Draw(tex,
                            new Rectangle((int)absoluteRect.X, (int)absoluteRect.Y, (int)absoluteRect.Width, (int)absoluteRect.Height),
                            null,
                            Color,
                            absoluteRect.Rotation,
                            GetSpriteOrigin(tex.Width, tex.Height),
                            SpriteEffects.None,
                            Z);
                }

                if (Icon != null)
                {
                    spriteBatch.Draw(Icon,
                            new Rectangle((int)absoluteRect.X, (int)absoluteRect.Y, (int)absoluteRect.Width, (int)absoluteRect.Height),
                            null,
                            Color,
                            absoluteRect.Rotation,
                            GetSpriteOrigin(Icon.Width, Icon.Height),
                            SpriteEffects.None,
                            Z);
                }

                if (Font != null && Text != null && Text != "")
                {
                    Vector2 offset = GetTextOffset(TextAlignment, Font.MeasureString(Text));
                    spriteBatch.DrawString(Font, Text, new Vector2(absoluteRect.X, absoluteRect.Y), ColorText, absoluteRect.Rotation, offset, 1, SpriteEffects.None, Z - 0.0001f);
                }
            }
        }


        public virtual Vector2 GetSpriteOrigin(float width, float height)
        {
            Vector2 org = GetAlignmentVector(Alignment);
            org.X *= width;
            org.Y *= height;
            return org;
        }


        public static Vector2 GetTextOffset(Alignment alignment, Vector2 size)
        {
            Vector2 offset = GetAlignmentVector(alignment);
            offset *= size;
            return offset;
        }


        public List<Element> GetAncestorChain()
        {
            List<Element> chain = new List<Element>();
            GetAncestorChainRecurse(chain, this);
            return chain;
        }


        private static void GetAncestorChainRecurse(List<Element> chain, Element current)
        {
            if (current != null && current.Parent != null)
            {
                chain.Add(current.Parent);
                GetAncestorChainRecurse(chain, current.Parent);
            }
        }


        public CRectangle GetAbsoluteBoundingBox()
        {
            CRectangle rect = new CRectangle(X, Y, Width, Height, Rotation);
            rect.Origin = GetOrigin();

            List<Element> ancestors = GetAncestorChain();
            foreach (Element ancestor in ancestors)
            {
                Vector2 ancestorPoint = new Vector2(ancestor.X, ancestor.Y);
                Vector2 curPoint = new Vector2(rect.X, rect.Y);
                curPoint += ancestorPoint;
                if (ancestor.Rotation != 0)
                {
                    curPoint = SimpleMath.VectorMath.RotatePointAroundZero(curPoint - ancestorPoint, ancestor.Rotation) + ancestorPoint;
                    rect.Rotation += ancestor.Rotation;
                }
                rect.X = curPoint.X;
                rect.Y = curPoint.Y;
            }

            return rect;
        }


        protected Vector2 GetOrigin()
        {
            Vector2 org = GetAlignmentVector(Alignment);
            org.X *= Width;
            org.Y *= Height;
            return org;
        }


        protected static Vector2 GetAlignmentVector(Alignment align)
        {
            Vector2 org = Vector2.Zero;
            switch (align)
            {
                case Alignment.CENTER:
                    org.Y += 0.5f;
                    org.X += 0.5f;
                    break;

                case Alignment.TOP_LEFT:
                    break;

                case Alignment.TOP:
                    org.X += 0.5f;
                    break;

                case Alignment.TOP_RIGHT:
                    org.X += 1;
                    break;

                case Alignment.RIGHT:
                    org.Y += 0.5f;
                    org.X += 1;
                    break;

                case Alignment.BOTTOM_RIGHT:
                    org.Y += 1;
                    org.X += 1;
                    break;

                case Alignment.BOTTOM:
                    org.Y += 1;
                    org.X += 0.5f;
                    break;

                case Alignment.BOTTOM_LEFT:
                    org.Y += 1;
                    break;

                case Alignment.LEFT:
                    org.Y += 0.5f;
                    break;
            }
            return org;
        }


        public virtual void SendKeyboardInfoToThisElement(CKeyboard keyboard)
        {

        }


        #region JSON Parsers
        /// <summary>
        /// Name(string) and Value(object) pairs to put into the element object
        /// </summary>
        /// <param name="values">"name1", obj1, "name2", obj2</param>
        protected void AddDefaultValues(params object[] values)
        {
            for (int i = 0; i + 1 < values.Length; i += 2)
            {
                string name = (string) values[i];
                object val = values[i + 1];
                if (Get(name) == null)
                {
                    Put(name, val);
                }
            }
        }


        protected List<string> GetAllPropertiesThatContain(string str)
        {
            List<string> props = new List<string>();
            foreach (string prop in json.Properties())
            {
                if (prop.Contains(str))
                {
                    props.Add(prop);
                }
            }
            return props;
        }


        protected void ParseAlignments(List<string> alignments)
        {
            IEnumerable alignmentEnum = null;
            if (alignments != null && alignments.Count > 0)
            {
                alignmentEnum = EnumUtils.GetValues<Alignment>();
            }
            foreach (string propName in alignments)
            {
                string alignmentStr = (string)Get(propName);
                foreach (Alignment alignment in alignmentEnum)
                {
                    if (alignment.EqualsStr(alignmentStr))
                    {
                        Put(propName, alignment);
                    }
                }
            }
        }


        protected void ParseColors(List<string> colors)
        {
            foreach (string propName in colors)
            {
                Color col = ParseUtils.DecodeColor((string)Get(propName));
                Put(propName, col);
            }
        }


        protected void ParseTextures(List<string> textures)
        {
            foreach (string propName in textures)
            {
                Put(propName, TextureManager.Instance.GetTexture((string) Get(propName)));
            }
        }


        protected void ParseFonts(List<string> fonts)
        {
            foreach (string propName in fonts)
            {
                Put(propName, FontManager.Instance.GetFont((string)Get(propName)));
            }
        }


        protected void ParsePercentages(params string[] names)
        {
            for(int i = 0; i + 1 < names.Length; i+=2)
            {
                object val = Get(names[i]);
                if (val != null && val is string)
                {
                    string valStr = val as string;
                    if (valStr.Trim().EndsWith("%"))
                    {
                        double valDub = 0;
                        if (Double.TryParse(valStr.Replace("%", "").Trim(), out valDub))
                        {
                            float parentVal = 0;
                            if (Parent == null)
                            {
                                if (names[i + 1] == "width")
                                {
                                    parentVal = WindowHelper.Instance.Bounds.Width;
                                }
                                else if (names[i + 1] == "height")
                                {
                                    parentVal = WindowHelper.Instance.Bounds.Height;
                                }
                            }
                            else
                            {
                                parentVal = Convert.ToSingle(Parent.Get(names[i + 1]));
                            }
                            valDub *= 0.01f;
                            valDub *= parentVal;
                        }
                        Put(names[i], valDub);
                    }
                }
            }
        }


        protected void ParseSettings()
        {
            List<string> thisProps = json.Properties();
            for (int i = 0; i < thisProps.Count; i++)
            {
                object propObj = Get(thisProps[i]);
                if (propObj != null && propObj is string)
                {
                    string propStr = propObj as string;
                    if (propStr.Contains("$!<"))
                    {
                        int startIndex = propStr.IndexOf("$!<");
                        int endIndex = propStr.IndexOf(">", startIndex) + 1;
                        string subStr = propStr.Substring(startIndex, endIndex - startIndex);
                        propStr = propStr.Remove(startIndex, endIndex - startIndex);
                        subStr = subStr.Replace("$!<", "").Replace(">", "");

                        if (propStr.Length > 0)
                        {
                            propStr.Insert(startIndex, "" + pageSettings.Get(subStr));
                            Put(thisProps[i], propStr);
                        }
                        else
                        {
                            Put(thisProps[i], pageSettings.Get(subStr));
                        }
                    }
                }
            }
        }


        protected void ParseGlobals()
        {
            foreach (string globalPropName in pageGlobals.Properties())
            {
                if (Get(globalPropName) == null)
                {
                    Put(globalPropName, pageGlobals.Get(globalPropName));
                }
            }
        }


        protected void ParseChildren()
        {

            List<object> childrenObjs = (List<object>) json.Get("children.#");
            List<Element> childrenElems = new List<Element>();
            if (childrenObjs != null)
            {
                foreach (object childObj in childrenObjs)
                {
                    Element childElem = ElementFactory.CreateElement((JSON)childObj, pageSettings, pageGlobals, this);
                    childrenElems.Add(childElem);
                }
            }
            Children = childrenElems;
        }
        #endregion


        #region Enumerator and Enumerable
        public List<Element> GetElementTreeAsList()
        {
            HashSet<Element> elements = new HashSet<Element>();
            elements.Add(this);
            GetElementTreeAsListRecurse(0, elements);
            return elements.ToList<Element>();
        }

        protected static void GetElementTreeAsListRecurse(int elementPosition, HashSet<Element> elements)
        {
            if (elementPosition >= elements.Count)
            {
                return;
            }
            Element curElement = elements.ElementAt(elementPosition);
            foreach (Element child in curElement.Children)
            {
                elements.Add(child);
            }
            GetElementTreeAsListRecurse(elementPosition + 1, elements);
        }

        Element IEnumerator<Element>.Current
        {
            get { return enumeratorList[enumeratorPosition]; }
        }

        void IDisposable.Dispose()
        {
        }

        object IEnumerator.Current
        {
            get { return enumeratorList[enumeratorPosition]; }
        }

        bool IEnumerator.MoveNext()
        {
            enumeratorPosition++;
            return (enumeratorPosition < enumeratorList.Count);
        }

        void IEnumerator.Reset()
        {
            enumeratorPosition = 0;
        }


        IEnumerator<Element> IEnumerable<Element>.GetEnumerator()
        {
            enumeratorPosition = -1;
            enumeratorList = GetElementTreeAsList();
            return (IEnumerator<Element>)this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            enumeratorPosition = -1;
            enumeratorList = GetElementTreeAsList();
            return (IEnumerator)this;
        }
        #endregion


        #region Hover and Select Events
        public virtual bool IsHover()
        {
            return isHover;
        }

        public virtual bool IsSelected()
        {
            return isSelected;
        }

        public virtual bool IsNeutral()
        {
            if (isHover || isSelected)
                return false;
            else
                return true;
        }


        public virtual void OnHoverStartEvent()
        {
            isHover = true;
            isSelected = false;
        }

        public virtual void OnHoverEndEvent()
        {
            isHover = false;
        }

        public virtual void OnSelectStartEvent()
        {
            isSelected = true;
            isHover = false;
            HasFocus = true;
        }

        public virtual void OnSelectEndEvent()
        {
            isSelected = false;
        }

        public virtual void NeutralizeEvent()
        {
            isHover = false;
            isSelected = false;
            HasFocus = false;
        }
        #endregion
    }
}
