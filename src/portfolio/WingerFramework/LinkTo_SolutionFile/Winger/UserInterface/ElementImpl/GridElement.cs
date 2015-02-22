using Winger.Utils;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Winger.UserInterface.ElementImpl
{
    public class GridElement : Element
    {
        public int Row
        {
            get { return Convert.ToInt32(Get("row")); }
            set { Put("row", value); }
        }
        public int Column
        {
            get { return Convert.ToInt32(Get("column")); }
            set { Put("column", value); }
        }
        public float Padding
        {
            get { return Convert.ToSingle(Get("padding")); }
            set { Put("padding", value); }
        }
        public float PaddingLeft
        {
            get { return Convert.ToSingle(Get("padding-left")); }
            set { Put("padding-left", value); }
        }
        public float PaddingRight
        {
            get { return Convert.ToSingle(Get("padding-right")); }
            set { Put("padding-right", value); }
        }
        public float PaddingTop
        {
            get { return Convert.ToSingle(Get("padding-top")); }
            set { Put("padding-top", value); }
        }
        public float PaddingBottom
        {
            get { return Convert.ToSingle(Get("padding-bottom")); }
            set { Put("padding-bottom", value); }
        }

        public GridElement(string json) : base(json) { }

        public GridElement(JSON json) : base(json) { }

        public GridElement(JSON json, JSON settings, JSON globals) : base(json, settings, globals) { }

        public GridElement(JSON json, JSON settings, JSON globals, Element parent) : base(json, settings, globals, parent) { }


        public override void Initialize()
        {
            base.Initialize();

            AddDefaultValues("row", -1, "column", -1, "padding", 0, 
                "padding-left", 0, "padding-right", 0, "padding-top", 0, 
                "padding-bottom", 0);

            ArrangeChildren();
        }


        public virtual void ArrangeChildren()
        {
            int rowCount = Row;
            int colCount = Column;
            if (rowCount < 0 && colCount < 0)
            {
                rowCount = 1;
                colCount = 100000;
            }
            else if (rowCount < 0)
            {
                rowCount = 100000;
            }
            else if (colCount < 0)
            {
                colCount = 100000;
            }
            float defaultPadding = Padding;
            float padL = defaultPadding;
            float padR = defaultPadding;
            float padT = defaultPadding;
            float padB = defaultPadding;
            if (PaddingLeft != 0)
            {
                padL = PaddingLeft;
            }
            if (PaddingRight != 0)
            {
                padR = PaddingRight;
            }
            if (PaddingTop != 0)
            {
                padT = PaddingTop;
            }
            if (PaddingBottom != 0)
            {
                padB = PaddingBottom;
            }

            float w = 0;
            float h = 0;
            float rowHeight = 0;
            float maxWidth = 0;
            int i = 0;
            List<Element> children = Children;
            foreach (Element child in children)
            {
                child.IsVisible = false;
                child.IsEnabled = false;
            }
            for (int r = 0; r < rowCount; r++)
            {
                h += padT;
                w = 0;
                for (int c = 0; c < colCount; c++)
                {
                    if (i < children.Count)
                    {
                        Element child = children[i];
                        child.IsEnabled = true;
                        child.IsVisible = true;
                        if (child.Height > rowHeight)
                        {
                            rowHeight = child.Height;
                        }
                        Vector2 origin = Element.GetAlignmentVector(child.Alignment) * new Vector2(child.Width, child.Height);
                        Vector2 widHei = new Vector2(child.Width, child.Height) - origin;

                        w += padL + origin.X;
                        child.X = w;
                        child.Y = h + origin.Y;
                        w += widHei.X + padR;

                        if (w > maxWidth)
                        {
                            maxWidth = w;
                        }
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                h += rowHeight;
                h += padB;
                if (i >= children.Count)
                {
                    break;
                }
            }

            if (Width == 0)
            {
                Width = maxWidth;
            }
            if (Height == 0)
            {
                Height = h;
            }
        }
    }
}
