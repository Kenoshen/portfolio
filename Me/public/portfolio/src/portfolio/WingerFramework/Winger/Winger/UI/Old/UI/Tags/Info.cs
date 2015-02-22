using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Winger.UI.Input;

namespace Winger.UI.UI.Tags
{
    class Info : UIObject
    {
        public float Delay
        {
            get
            {
                object o = Get("delay");
                float f = default_Delay;
                if (o != null)
                {
                    if (o is float)
                        f = (float)o;
                    else if (o is string)
                    {
                        try
                        {
                            f = float.Parse((string)o);
                            Put("delay", f);
                        }
                        catch (Exception) { }
                    }
                }
                else
                    Put("delay", f);

                return f;
            }
            set
            {
                Put("delay", value);
            }
        }

        protected float default_Delay = 0;

        protected float counter = 0;
        protected UIObject dummyRoot = new UIObject();

        public Info()
        {
            default_IsVisible = false;
            default_Alignment = Winger.UI.Helper.Alignment.BOTTOM_LEFT;
            default_Depth = 0;
        }

        public override void Update(int elapsedMilliseconds, UIObject root, CMouse mouse)
        {
            base.Update(elapsedMilliseconds, root, mouse);

            if (root.IsEnabled && IsEnabled)
            {
                X = mouse.X;
                Y = mouse.Y;
                IsVisible = false;
                if (root.IsHover())
                {
                    Vector2 mouseDiff = mouse.GetPositionDifference();
                    if (mouseDiff == Vector2.Zero)
                        counter += elapsedMilliseconds;
                    else
                        counter = 0;
                    if (Delay <= counter)
                    {
                        IsVisible = true;
                        counter = Delay;
                    }
                }
                else
                    counter = 0;
            }
        }

        public override void Draw(UIObject root, SpriteBatch spriteBatch)
        {
            base.Draw(dummyRoot, spriteBatch);
        }

        public override Vector2 GetSpriteOrigin(float width, float height)
        {
            return new Vector2(0, height);
        }
    }
}
