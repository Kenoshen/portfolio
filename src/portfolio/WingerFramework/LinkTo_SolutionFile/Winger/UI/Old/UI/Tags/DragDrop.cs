using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Winger.UI.Tween;
using Winger.UI.Event;
using Winger.UI.Input;

namespace Winger.UI.UI.Tags
{
    public class DragDrop : UIObject
    {
        public Texture2D TextureDrag
        {
            get
            {
                return t2d_drag;
            }
            set
            {
                t2d_drag = value;
                Put("texturedrag", "undefined");
            }
        }
        public bool IsDragging
        {
            get
            {
                object o = Get("isdragging");
                bool b = default_IsDragging;
                if (o is bool)
                    b = (bool)o;
                else if (o is string)
                {
                    if (((string)o) == "false")
                    {
                        b = false;
                        Put("isdragging", b);
                    }
                }
                else
                {
                    Put("isdragging", b);
                }
                return b;
            }
            set
            {
                Put("isdragging", value);
            }
        }
        public float StartDragRadius
        {
            get
            {
                object o = Get("startdragradius");
                float f = default_StartDragRadius;
                if (o != null)
                {
                    if (o is float)
                        f = (float)o;
                    else if (o is string)
                    {
                        try
                        {
                            f = float.Parse((string)o);
                            Put("startdragradius", f);
                        }
                        catch (Exception) { }
                    }
                }
                else
                    Put("startdragradius", f);

                return f;
            }
            set
            {
                Put("startdragradius", value);
            }
        }
        public float DragWidth
        {
            get
            {
                object o = Get("dragwidth");
                float f = Width;
                if (o != null)
                {
                    if (o is float)
                        f = (float)o;
                    else if (o is string)
                    {
                        try
                        {
                            f = float.Parse((string)o);
                            Put("dragwidth", f);
                        }
                        catch (Exception) { }
                    }
                }
                else
                    Put("dragwidth", f);

                return f;
            }
            set
            {
                Put("dragwidth", value);
            }
        }
        public float DragHeight
        {
            get
            {
                object o = Get("dragheight");
                float f = Height;
                if (o != null)
                {
                    if (o is float)
                        f = (float)o;
                    else if (o is string)
                    {
                        try
                        {
                            f = float.Parse((string)o);
                            Put("dragheight", f);
                        }
                        catch (Exception) { }
                    }
                }
                else
                    Put("dragheight", f);

                return f;
            }
            set
            {
                Put("dragheight", value);
            }
        }
        public float DragRotation
        {
            get
            {
                object o = Get("dragrotation");
                float f = Rotation;
                if (o != null)
                {
                    if (o is float)
                        f = (float)o;
                    else if (o is string)
                    {
                        try
                        {
                            f = float.Parse((string)o);
                            Put("dragrotation", f);
                        }
                        catch (Exception) { }
                    }
                }
                else
                    Put("dragrotation", f);

                return f;
            }
            set
            {
                Put("dragrotation", value);
            }
        }
        public float DragDepth
        {
            get
            {
                object o = Get("dragdepth");
                float f = default_DragDepth;
                if (o != null)
                {
                    if (o is float)
                        f = (float)o;
                    else if (o is string)
                    {
                        try
                        {
                            f = float.Parse((string)o);
                            Put("dragdepth", f);
                        }
                        catch (Exception) { }
                    }
                }
                else
                    Put("dragdepth", f);

                return f;
            }
            set
            {
                Put("dragdepth", value);
            }
        }
        public Color DragColor
        {
            get
            {
                object o = Get("dragcolor");
                if (o == null || o is string)
                {
                    Color c = Winger.UI.Helper.Utils.ColorDecoder((string)o, default_DragColor);
                    Put("dragcolor", c);
                    return c;
                }
                else
                    return (Color)o;
            }
            set
            {
                Put("dragcolor", value);
            }
        }

        #region Default Properties
        protected bool default_IsDragging = false;
        protected float default_StartDragRadius = 0;
        protected float default_DragDepth = 0;
        protected Color default_DragColor = new Color(1, 1, 1, 0.5f);
        #endregion

        protected float drag_X = 0;
        protected float drag_Y = 0;

        protected Texture2D t2d_drag;

        protected Winger.UI.Tween.Tween falseDrop = null;

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            if (Get("texturedrag") != null)
                t2d_drag = Content.Load<Texture2D>((string)Get("texturedrag"));
        }

        public override void Update(int elapsedMilliseconds, UIObject root, CMouse mouse)
        {
            base.Update(elapsedMilliseconds, root, mouse);

            if (root.IsEnabled && IsEnabled)
            {
                if (!IsDragging)
                {
                    if (IsSelected() && Vector2.Distance(mouseDownPosition, mouse.MousePos) >= StartDragRadius)
                    {
                        IsDragging = true;
                    }
                }

                if (IsDragging)
                {
                    drag_X = mouse.X;
                    drag_Y = mouse.Y;

                    if (!mouse.LeftHeldDown())
                    {
                        IsDragging = false;
                        Delegate(this, UIEventType.CUSTOM, null);
                    }
                }
                if (falseDrop != null)
                {
                    falseDrop.Update(elapsedMilliseconds);
                    if (falseDrop.IsTweening())
                    {
                        drag_X = falseDrop.X;
                        drag_Y = falseDrop.Y;
                    }
                }
            }
        }

        public override void Draw(UIObject root, SpriteBatch spriteBatch)
        {
            base.Draw(root, spriteBatch);

            if (root.IsVisible && IsVisible)
            {
                if (t2d_drag != null)
                {
                    if (IsDragging || (falseDrop != null && falseDrop.IsTweening()))
                    {
                        spriteBatch.Draw(t2d_drag,
                                new Rectangle((int)(drag_X), (int)(drag_Y), (int)(DragWidth), (int)(DragHeight)),
                                null,
                                DragColor,
                                DragRotation,
                                GetSpriteOrigin(t2d_drag.Width, t2d_drag.Height),
                                SpriteEffects.None,
                                DragDepth);
                    }
                }
            }
        }

        public Vector2 GetDropPoint()
        {
            return new Vector2(drag_X, drag_Y);
        }

        public void DropPointIsFalseStartTween()
        {
            if (falseDrop == null)
            {
                falseDrop = new Winger.UI.Tween.Tween();
                falseDrop.TweenTypeX = TweenType.SINUSOIDAL_INOUT;
                falseDrop.TweenTypeY = TweenType.SINUSOIDAL_INOUT;
                falseDrop.Duration = 500;
            }

            falseDrop.X = drag_X;
            falseDrop.Y = drag_Y;
            falseDrop.OriginX = drag_X;
            falseDrop.OriginY = drag_Y;
            falseDrop.GoalX = X;
            falseDrop.GoalY = Y;

            falseDrop.Start();
        }
    }
}
