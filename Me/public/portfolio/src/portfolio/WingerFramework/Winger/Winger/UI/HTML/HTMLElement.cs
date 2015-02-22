using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Winger.UI.CSS;
using Winger.Utils;
using System.Collections;
using System;
using Winger.UI.IMG;
using Winger.Draw;

namespace Winger.UI.HTML
{
    public class HTMLElement : IEnumerator<HTMLElement>, IEnumerable<HTMLElement>
    {
        // object stuff
        public float Left { get { return MarginRect.X; } }
        public float Top { get { return MarginRect.Y; } }
        public float Right { get { return Left + MarginRect.Width; } }
        public float Bottom { get { return Top + MarginRect.Height; } }
        public CRectangle MarginRect = new CRectangle(Rectangle.Empty);
        public CRectangle BorderRect = new CRectangle(Rectangle.Empty);
        public CRectangle PaddingRect = new CRectangle(Rectangle.Empty);
        public CRectangle ContentRect = new CRectangle(Rectangle.Empty);
        public Texture2D Texture
        {
            get { if (Img != null) return Img.Texture; return null; }
        }
        public ImgElement Img { get; set; }
        public bool WillDrawInRenderTree { get; set; }
        public SpriteFont Font { get; set; }
        // html stuff
        public HTMLTag Tag = HTMLTag.NONE;
        public string Text = "";
        public string ParentDirectory { get; set; }
        public bool IsDrawable { get; protected set; }
        protected Dictionary<string, string> attributes = new Dictionary<string, string>();
        protected CSSElement css = new CSSElement("this");
        protected ISet<string> cssParentDirectories = new HashSet<string>();
        // dom stuff
        public HTMLElement Parent = null;
        public List<HTMLElement> Children = new List<HTMLElement>();
        // enumerator stuff
        protected int enumeratorPosition = 0;
        protected List<HTMLElement> enumeratorList = new List<HTMLElement>();
        // debugger
        public bool Debug { get; set; }
        private DrawRectangle debugRectangle = null;


        public void MoveTo(float x, float y)
        {
            // move the margin rectangle to point and the other rectangles to their relative positions
            Vector2 diffA = new Vector2(BorderRect.X - MarginRect.X, BorderRect.Y - MarginRect.Y);
            Vector2 diffB = new Vector2(PaddingRect.X - BorderRect.X, PaddingRect.Y - BorderRect.Y);
            Vector2 diffC = new Vector2(ContentRect.X - PaddingRect.X, ContentRect.Y - PaddingRect.Y);

            MarginRect.X = x;
            MarginRect.Y = y;

            BorderRect.X = MarginRect.X + diffA.X;
            BorderRect.Y = MarginRect.Y + diffA.Y;

            PaddingRect.X = BorderRect.X + diffB.X;
            PaddingRect.Y = BorderRect.Y + diffB.Y;

            ContentRect.X = PaddingRect.X + diffC.X;
            ContentRect.Y = PaddingRect.Y + diffC.Y;
        }


        public void MoveTo(Vector2 point)
        {
            MoveTo(point.X, point.Y);
        }


        public void SetAllRects(CRectangle rect)
        {
            MarginRect = rect.Clone();
            BorderRect = rect.Clone();
            PaddingRect = rect.Clone();
            ContentRect = rect.Clone();
        }


        #region Generation
        public void GenerateProperties()
        {
            if (Tag == HTMLTag.NONE)
            {
                return;
            }
            if (Tag == HTMLTag.HEAD)
            {
                IsDrawable = false;
            }
            else
            {
                IsDrawable = true;
            }


        }


        public void GenerateMarginRect()
        {
            if (Tag == HTMLTag.HTML)
            {
                return;
            }
            if (Tag == HTMLTag.HEAD)
            {
                MarginRect = CRectangle.Empty;
                BorderRect = CRectangle.Empty;
                PaddingRect = CRectangle.Empty;
                ContentRect = CRectangle.Empty;
                return;
            }
            if (Tag == HTMLTag.BODY)
            {
                MarginRect = Parent.MarginRect.Clone();
                BorderRect = Parent.BorderRect.Clone();
                PaddingRect = Parent.PaddingRect.Clone();
                ContentRect = Parent.ContentRect.Clone();
                return;
            }

            CalculateContentRect();

            // calculate x and y if applicable
            if (css.HasProperty(CSSProperty.POSITION))
            {
                if (css.GetProperty(CSSProperty.POSITION).Equals("fixed"))
                {
                    if (css.HasProperty(CSSProperty.RIGHT))
                    {
                        ContentRect.X = GenValueWithProperty(CSSProperty.RIGHT, Parent.ContentRect.Width) - ContentRect.Width;
                    }
                    if (css.HasProperty(CSSProperty.BOTTOM))
                    {
                        ContentRect.Y = GenValueWithProperty(CSSProperty.BOTTOM, Parent.ContentRect.Height) - ContentRect.Height;
                    }
                    if (css.HasProperty(CSSProperty.LEFT))
                    {
                        ContentRect.X = GenValueWithProperty(CSSProperty.LEFT, Parent.ContentRect.Width);
                    }
                    if (css.HasProperty(CSSProperty.TOP))
                    {
                        ContentRect.Y = GenValueWithProperty(CSSProperty.TOP, Parent.ContentRect.Height);
                    }
                }
            }

            // figure out padding rect from content rect
            PaddingRect = ContentRect.Clone();
            GenRectFromProperties(PaddingRect, Parent.ContentRect, CSSProperty.PADDING, CSSProperty.PADDING_RIGHT, CSSProperty.PADDING_TOP, CSSProperty.PADDING_BOTTOM, CSSProperty.PADDING_LEFT);
            // figure out border rect from padding rect
            BorderRect = PaddingRect.Clone();
            GenRectFromProperties(BorderRect, Parent.ContentRect, CSSProperty.BORDER, CSSProperty.BORDER_RIGHT, CSSProperty.BORDER_TOP, CSSProperty.BORDER_BOTTOM, CSSProperty.BORDER_LEFT);
            // figure out margin rect from border rect
            MarginRect = BorderRect.Clone();
            GenRectFromProperties(MarginRect, Parent.ContentRect, CSSProperty.MARGIN, CSSProperty.MARGIN_RIGHT, CSSProperty.MARGIN_TOP, CSSProperty.MARGIN_BOTTOM, CSSProperty.MARGIN_LEFT);

            // move the margin rect to the (0,0) position
            MoveTo(Vector2.Zero);
        }


        protected void GenRectFromProperties(CRectangle rect, CRectangle parent, CSSProperty main, CSSProperty right, CSSProperty top, CSSProperty bottom, CSSProperty left)
        {
            float parentWidth = parent.Width;
            float parentHeight = parent.Height;

            // NOLONGERATODO: do calculation for main css property

            float leftVal = 0;
            if (css.HasProperty(left))
            {
                leftVal = GenValue(css.GetProperty(left), parentWidth);
            }
            float rightVal = 0;
            if (css.HasProperty(right))
            {
                rightVal = GenValue(css.GetProperty(right), parentWidth);
            }
            float topVal = 0;
            if (css.HasProperty(top))
            {
                topVal = GenValue(css.GetProperty(top), parentHeight);
            }
            float bottomVal = 0;
            if (css.HasProperty(bottom))
            {
                bottomVal = GenValue(css.GetProperty(bottom), parentHeight);
            }

            rect.X -= leftVal;
            rect.Width += leftVal;
            rect.Width += rightVal;

            rect.Y -= topVal;
            rect.Height += topVal;
            rect.Height += bottomVal;
        }


        protected float GenValueWithProperty(CSSProperty property, float parentValue)
        {
            return GenValueWithProperty(property, CSSProperty.NONE, CSSProperty.NONE, parentValue);
        }


        protected float GenValueWithProperty(CSSProperty property, CSSProperty min, CSSProperty max, float parentValue)
        {
            float value = 0;
            if (css.HasProperty(property))
            {
                value = GenValue(css.GetProperty(property), parentValue);
            }
            else
            {
                value = parentValue;
            }
            if (css.HasProperty(max))
            {
                float maxWidth = GenValue(css.GetProperty(max), parentValue);
                if (value > maxWidth)
                {
                    value = maxWidth;
                }
            }
            if (css.HasProperty(min))
            {
                float minWidth = GenValue(css.GetProperty(min), 0);
                if (value < minWidth)
                {
                    value = minWidth;
                }
            }
            return value;
        }


        protected float GenValue(string valueStr, float max)
        {
            if (valueStr.Contains("%"))
            {
                return GenPercentageValue(valueStr, max);
            }
            return GenPixelValue(valueStr);
        }


        protected float GenPercentageValue(string valueStr, float max)
        {
            float value = float.Parse(valueStr.Replace("%", "").Replace(" ", ""));
            value = value * 0.01f;
            if (value > 1)
            {
                value = 1;
            }
            return value * max;
        }


        protected float GenPixelValue(string valueStr)
        {
            return float.Parse(valueStr.Replace("px", "").Replace(" ", ""));
        }


        protected void CalculateContentRect()
        {
            if (Tag == HTMLTag.INPUT)
            {

            }
            ContentRect.Width = GenValueWithProperty(CSSProperty.WIDTH, CSSProperty.MIN_WIDTH, CSSProperty.MAX_WIDTH, Parent.ContentRect.Width);
            ContentRect.Height = GenValueWithProperty(CSSProperty.HEIGHT, CSSProperty.MIN_HEIGHT, CSSProperty.MAX_HEIGHT, Parent.ContentRect.Height); ;
        }
        #endregion


        #region RenderTree
        public void GenerateRenderTree()
        {
            float minX = ContentRect.X;
            float minY = ContentRect.Y;
            float maxX = ContentRect.X + ContentRect.Width;
            float maxY = ContentRect.Y + ContentRect.Height;

            float curMinX = minX;
            float curMinY = minY;

            float lastBottom = curMinY;

            for (int i = 0; i < Children.Count; i++)
            {
                HTMLElement element = Children[i];
                if (element.MarginRect.Width > ContentRect.Width ||
                    element.MarginRect.Height > ContentRect.Height)
                {
                    // the element will not fit no matter what, don't draw it
                    element.WillDrawInRenderTree = false;
                    continue;
                }
                element.MoveTo(curMinX, curMinY);
                if (element.Bottom > maxY)
                {
                    // the element is off the bottom, don't draw it
                    element.WillDrawInRenderTree = false;
                }
                else if (element.Right > maxX)
                {
                    // start new row and redo the last element
                    curMinX = minX;
                    curMinY = lastBottom;
                    i--;
                }
                else
                {
                    // the element fits, so draw it
                    element.WillDrawInRenderTree = true;
                    if (element.Bottom > lastBottom)
                    {
                        lastBottom = element.Bottom;
                    }
                    curMinX = element.Right;
                }
            }
        }
        #endregion


        #region Attributes
        public void AddAttribute(string name, string value)
        {
            if (name != null && !name.Equals(""))
            {
                attributes[name.ToLower()] = value;
            }
        }


        public void RemoveAttribute(string name)
        {
            if (name != null && !name.Equals(""))
            {
                if (attributes.ContainsKey(name.ToLower()))
                {
                    attributes.Remove(name.ToLower());
                }
            }
        }


        public string GetAttribute(string name)
        {
            if (name != null && !name.Equals(""))
            {
                if (attributes.ContainsKey(name.ToLower()))
                {
                    return attributes[name.ToLower()];
                }
            }
            return null;
        }


        public List<string> GetAttributeNames()
        {
            return attributes.Keys.ToList<string>();
        }
        #endregion


        #region CSS
        public void ApplyCSSElement(CSSElement element)
        {
            cssParentDirectories.Add(element.ParentDirectory);
            foreach (CSSProperty prop in element.GetProperties())
            {
                string propertyValue = element.GetProperty(prop);
                css.AddProperty(prop, propertyValue);
            }

            string imgPath = null;
            if (css.HasProperty(CSSProperty.BACKGROUND_IMAGE))
            {
                imgPath = css.GetProperty(CSSProperty.BACKGROUND_IMAGE);
            }
            else if (css.HasProperty(CSSProperty.LIST_STYLE_IMAGE))
            {
                imgPath = css.GetProperty(CSSProperty.LIST_STYLE_IMAGE);
            }

            if (imgPath != null)
            {
                foreach (string dir in cssParentDirectories)
                {
                    string source = dir + "\\" + imgPath;
                    ImgElement elem = ImgElementManager.Instance.GetImgElementWithSource(FileUtils.AbsoluteFilePath(source));
                    if (elem != null)
                    {
                        Img = elem;
                        break;
                    }
                }
                if (Texture == null)
                {
                    throw new Exception("Couldn't find imgPath " + imgPath);
                }
            }

            string fontName = null;
            if (css.HasProperty(CSSProperty.FONT_FAMILY))
            {
                // NOLONGERATODO: literally in the middle of this line I decided this was a waste of time
                //fontName = 
                if (fontName == null)
                {

                }
            }
        }


        public CSSElement GetCSS()
        {
            return css;
        }
        #endregion


        #region General
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ToStringRecurse(this, 0));
            return sb.ToString();
        }


        protected static string ToStringRecurse(HTMLElement curElement, int depth)
        {
            if (curElement.Tag == HTMLTag.NONE)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            string tab = "";
            for (int i = 0; i < depth; i++)
            {
                tab += "\t";
            }
            sb.Append(tab);
            if (curElement.Tag == HTMLTag.T)
            {
                sb.Append(curElement.Text);
                return sb.ToString();
            }
            sb.Append("<" + curElement.Tag.AsString());

            foreach (string name in curElement.attributes.Keys)
            {
                sb.Append(" " + name + "=\"" + curElement.attributes[name] + "\"");
            }

            sb.Append(">");
            StringBuilder childrenStr = new StringBuilder();
            int childDepth = depth + 1;
            foreach (HTMLElement child in curElement.Children)
            {
                childrenStr.Append(ToStringRecurse(child, childDepth));
            }
            if (childrenStr != null && !childrenStr.ToString().Equals(""))
            {
                sb.Append("\n").Append(childrenStr).Append("\n").Append(tab);
            }
            sb.Append("</" + curElement.Tag.AsString() + ">\n");
            return sb.ToString();
        }


        protected void UpdateDebugRectangle()
        {
            // debug code
            // NOLONGERATODO: probably need to get rid of this at some point
            if (debugRectangle != null)
            {
                bool dirtyDrawRectangle = false;
                if (debugRectangle.Width != MarginRect.Width)
                {
                    debugRectangle.Width = MarginRect.Width;
                    dirtyDrawRectangle = true;
                }
                if (debugRectangle.Height != MarginRect.Height)
                {
                    debugRectangle.Height = MarginRect.Height;
                    dirtyDrawRectangle = true;
                }
                debugRectangle.X = MarginRect.X;
                debugRectangle.Y = MarginRect.Y;
                if (dirtyDrawRectangle)
                {
                    debugRectangle.UpdateTexture();
                }
            }
            else
            {
                debugRectangle = new DrawRectangle(MarginRect.Width, MarginRect.Height, true, true, true, GraphicsUtils.Instance.GraphicsDevice);
                debugRectangle.X = MarginRect.X;
                debugRectangle.Y = MarginRect.Y;
                debugRectangle.FillColor = ColorUtils.GetRandomColor();
                debugRectangle.UpdateTexture();
            }
        }
        #endregion


        #region Enumerator and Enumerable
        public List<HTMLElement> GetHTMLTreeAsList()
        {
            List<HTMLElement> elements = new List<HTMLElement>();
            GetHTMLTreeAsListRecurse(this, elements);
            return elements;
        }

        protected static void GetHTMLTreeAsListRecurse(HTMLElement element, List<HTMLElement> elements)
        {
            elements.Add(element);
            foreach (HTMLElement child in element.Children)
            {
                GetHTMLTreeAsListRecurse(child, elements);
            }
        }

        HTMLElement IEnumerator<HTMLElement>.Current
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
        

        IEnumerator<HTMLElement> IEnumerable<HTMLElement>.GetEnumerator()
        {
            enumeratorPosition = -1;
            enumeratorList = GetHTMLTreeAsList();
            return (IEnumerator<HTMLElement>)this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            enumeratorPosition = -1;
            enumeratorList = GetHTMLTreeAsList();
            return (IEnumerator) this;
        }
        #endregion


        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            if (WillDrawInRenderTree)
            {
                if (Texture != null)
                {
                    spriteBatch.Draw(Texture, BorderRect.Rect, Color.White);
                }

                if (Debug)
                {
                    UpdateDebugRectangle();
                    debugRectangle.Draw(spriteBatch);
                }
            }
        }

        #endregion
    }
}
