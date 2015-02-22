using Winger.Utils;
using System;
using Microsoft.Xna.Framework;

namespace Winger.UserInterface.ElementImpl
{
    public class ScrollElement : Element
    {
        public bool HorizontalScroll
        {
            get { return Convert.ToBoolean(Get("horizontal")); }
            set { Put("horizontal", value); }
        }
        public bool VerticalScroll
        {
            get { return Convert.ToBoolean(Get("vertical")); }
            set { Put("vertical", value); }
        }

        private CRectangle window = CRectangle.Empty;
        public float WindowWidth
        {
            get { return window.Width; }
        }
        public float WindowHeight
        {
            get { return window.Height; }
        }

        public ScrollElement(string json) : base(json) { }

        public ScrollElement(JSON json) : base(json) { }

        public ScrollElement(JSON json, JSON settings, JSON globals) : base(json, settings, globals) { }

        public ScrollElement(JSON json, JSON settings, JSON globals, Element parent) : base(json, settings, globals, parent) { }


        public override void Initialize()
        {
            base.Initialize();

            AddDefaultValues("horizontal", true, "vertical", true);

            CalculateWindow();
        }

        public virtual void CalculateWindow()
        {
            window = new CRectangle(10000, 10000, -100000, -100000);
            foreach (Element child in this)
            {
                if (!(this.Equals(child)))
                {
                    CRectangle childAbs = child.GetAbsoluteBoundingBox();
                    if (window.X > childAbs.X)
                    {
                        window.X = childAbs.X;
                    }
                    if (window.Y > childAbs.Y)
                    {
                        window.Y = childAbs.Y;
                    }
                    if (window.X + window.Width < childAbs.X + childAbs.Width)
                    {
                        float toOrig = childAbs.X - window.X;
                        window.Width = toOrig + childAbs.Width;
                    }
                    if (window.Y + window.Height < childAbs.Y + childAbs.Height)
                    {
                        float toOrig = childAbs.Y - window.Y;
                        window.Height = toOrig + childAbs.Height;
                    }
                }
            }

            window.Origin = new Vector2(X, Y) - new Vector2(window.X, window.Y);
        }

        public virtual void ScrollHorizontal(float amount)
        {
            window.Origin = new Vector2(window.Origin.X - amount, window.Origin.Y);
            foreach (Element immediateChild in Children)
            {
                immediateChild.X += amount;
            }
        }

        public virtual void ScrollVertical(float amount)
        {
            window.Origin = new Vector2(window.Origin.X, window.Origin.Y - amount);
            foreach (Element immediateChild in Children)
            {
                immediateChild.Y += amount;
            }
        }

        public virtual void Scroll(Vector2 amount)
        {
            window.Origin = window.Origin - amount;
            foreach (Element immediateChild in Children)
            {
                immediateChild.X += amount.X;
                immediateChild.Y += amount.Y;
            }
        }
    }
}
