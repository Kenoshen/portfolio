using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Winger.UI.Input;

namespace Winger.UI.UI.Tags
{
    public class Slider : UIObject
    {
        public Vector2 StartPosition
        {
            get
            {
                object o = Get("startposition");
                Vector2 v = default_StartPosition;
                if (o != null)
                {
                    if (o is Vector2)
                        return (Vector2)o;

                    v = Winger.UI.Helper.Utils.Vector2FromString((string)o);
                    Put("startPosition", v);
                }
                else
                    Put("startposition", v);

                return v;
            }
            set
            {
                Put("startposition", value);
            }
        }
        public Vector2 EndPosition
        {
            get
            {
                object o = Get("endposition");
                Vector2 v = default_EndPosition;
                if (o != null)
                {
                    if (o is Vector2)
                        return (Vector2)o;

                    v = Winger.UI.Helper.Utils.Vector2FromString((string)o);
                    Put("endposition", v);
                }
                else
                    Put("endposition", v);

                return v;
            }
            set
            {
                Put("endposition", value);
            }
        }
        public float SlideWidth
        {
            get
            {
                object o = Get("slidewidth");
                float f = default_SlideWidth;
                if (o != null)
                {
                    if (o is float)
                        return (float)o;
                    f = float.Parse((string)o);
                    Put("slidewidth", f);
                }
                else
                {
                    object w = Get("width");
                    if (w != null)
                    {
                        if (w is float)
                            f = (float)w;
                        else
                            f = float.Parse((string)w);
                    }
                    Put("slidewidth", f);
                }
                return f;
            }
            set
            {
                Put("slidewidth", value);
            }
        }
        public Texture2D SlideTexture
        {
            get
            {
                return t2d_slide;
            }
            set
            {
                t2d_slide = value;
                Put("textureslide", "undefined");
            }
        }
        public float Value
        {
            get
            {
                object o = Get("value");
                float f = default_Value;
                if (o != null)
                {
                    if (o is float)
                        f = (float)o;
                    else if (o is string)
                    {
                        try
                        {
                            f = float.Parse((string)o);
                            Put("value", f);
                        }
                        catch (Exception) { }
                    }
                }
                else
                    Put("value", f);

                return f;
            }
            set
            {
                Put("value", value);
            }
        }
        public bool DisplayValue
        {
            get
            {
                object o = Get("displayvalue");
                bool b = default_DisplayValue;
                if (o is bool)
                    b = (bool)o;
                else if (o is string)
                {
                    if (((string)o) == "true")
                    {
                        b = true;
                        Put("displayvalue", b);
                    }
                }
                else
                {
                    Put("displayvalue", b);
                }
                return b;
            }
            set
            {
                Put("displayvalue", value);
            }
        }

        #region Default Properties
        protected Vector2 default_StartPosition = Vector2.Zero;
        protected Vector2 default_EndPosition = Vector2.Zero;
        protected float default_SlideWidth = 0;
        protected float default_Value = 0;
        protected bool default_DisplayValue = false;
        #endregion

        protected Texture2D t2d_slide;

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            if (Get("textureslide") != null)
                t2d_slide = Content.Load<Texture2D>((string)Get("textureslide"));
        }

        public override void Update(int elapsedMilliseconds, UIObject root, CMouse mouse)
        {
            base.Update(elapsedMilliseconds, root, mouse);

            if (root.IsEnabled)
            {
                if (IsSelected())
                {
                    FindValueWithPos(mouse.X, mouse.Y);
                }

                Vector2 delta = ((EndPosition - StartPosition) * Value) + StartPosition;
                X = delta.X;
                Y = delta.Y;
            }
        }

        public override void Draw(UIObject root, SpriteBatch spriteBatch)
        {
            if (root.IsVisible)
            {
                if (t2d_slide != null)
                {
                    Vector2 mid = FindMiddle();
                    float f = FindRotation();
                    spriteBatch.Draw(t2d_slide,
                        new Rectangle((int)(mid.X + root.X), (int)(mid.Y + root.Y), (int)(FindDistance() + root.Width), (int)(SlideWidth + root.Height)),
                        null,
                        Color,
                        f + root.Rotation,
                        new Vector2(((float)t2d_slide.Width) / 2f, ((float)t2d_slide.Height) / 2f),
                        SpriteEffects.None,
                        Depth + 0.0001f);
                }
            }
            base.Draw(root, spriteBatch);
        }

        protected void FindValueWithPos(float x, float y)
        {
            if (Type == null || Type == "horizontal" || Type == "x")
            {
                if (StartPosition.X > EndPosition.X)
                {
                    float maxX = StartPosition.X;
                    float minX = EndPosition.X;

                    if (x >= maxX)
                        Value = 0;
                    else if (x <= minX)
                        Value = 1;
                    else
                        Value = 1 - ((x - minX) / (maxX - minX));
                }
                else
                {
                    float maxX = EndPosition.X;
                    float minX = StartPosition.X;

                    if (x >= maxX)
                        Value = 1;
                    else if (x <= minX)
                        Value = 0;
                    else
                        Value = (x - minX) / (maxX - minX);
                }
            }
            else
            {
                if (StartPosition.Y > EndPosition.Y)
                {
                    float maxY = StartPosition.Y;
                    float minY = EndPosition.Y;

                    if (y >= maxY)
                        Value = 0;
                    else if (y <= minY)
                        Value = 1;
                    else
                        Value = 1 - ((y - minY) / (maxY - minY));
                }
                else
                {
                    float maxY = EndPosition.Y;
                    float minY = StartPosition.Y;

                    if (y >= maxY)
                        Value = 1;
                    else if (y <= minY)
                        Value = 0;
                    else
                        Value = (y - minY) / (maxY - minY);
                }
            }

            if (DisplayValue)
                Text = "" + Value;
        }

        protected float FindRotation()
        {
            if (StartPosition.X == EndPosition.X)
            {
                return MathHelper.ToRadians(90);
            }
            else if (StartPosition.Y == EndPosition.Y)
            {
                return 0;
            }
            else
            {
                Vector2 dir = (EndPosition - StartPosition);
                Vector2 rot = new Vector2(1, 0);
                Vector2 side = dir - rot;
                return (float)Math.Atan2(side.Y,side.X);
            }
        }

        protected float FindDistance()
        {
            return Vector2.Distance(StartPosition, EndPosition);
        }

        protected Vector2 FindMiddle()
        {
            return ((EndPosition - StartPosition) * 0.5f) + StartPosition;
        }
    }
}
