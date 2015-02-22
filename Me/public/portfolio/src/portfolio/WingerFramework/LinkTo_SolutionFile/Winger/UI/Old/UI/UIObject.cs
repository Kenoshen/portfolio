using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Winger.UI.Event;
using Winger.UI.Helper;
using Winger.UI.Input;

namespace Winger.UI.UI
{
    public delegate void UIObjectDelegate(UIObject sender, UIEventType type, string name);

    public class UIObject
    {
        public UIObjectDelegate Delegate;
        public string OriginalText
        {
            get
            {
                try
                {
                    return (string)Get("originaltext");
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                Put("originaltext", value);
            }
        }
        public string Tag
        {
            get
            {
                try
                {
                    return (string)Get("tag");
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                Put("tag", value);
            }
        }
        public string Name
        {
            get
            {
                try
                {
                    return (string)Get("name");
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                Put("name", value);
            }
        }
        public float X
        {
            get
            {
                object o = Get("x");
                float f = default_X;
                if (o != null)
                {
                    if (o is float)
                        f = (float)o;
                    else if (o is string)
                    {
                        try
                        {
                            f = float.Parse((string)o);
                            Put("x", f);
                        }
                        catch (Exception){}
                    }
                }
                else
                    Put("x", f);

                return f;
            }
            set
            {
                Put("x", value);
            }
        }
        public float Y
        {
            get
            {
                object o = Get("y");
                float f = default_Y;
                if (o != null)
                {
                    if (o is float)
                        f = (float)o;
                    else if (o is string)
                    {
                        try
                        {
                            f = float.Parse((string)o);
                            Put("y", f);
                        }
                        catch (Exception) { }
                    }
                }
                else
                    Put("y", f);

                return f;
            }
            set
            {
                Put("y", value);
            }
        }
        public float Width
        {
            get
            {
                object o = Get("width");
                float f = default_Width;
                if (o != null)
                {
                    if (o is float)
                        f = (float)o;
                    else if (o is string)
                    {
                        try
                        {
                            f = float.Parse((string)o);
                            Put("width", f);
                        }
                        catch (Exception) { }
                    }
                }
                else
                    Put("width", f);

                return f;
            }
            set
            {
                Put("width", value);
            }
        }
        public float Height
        {
            get
            {
                object o = Get("height");
                float f = default_Height;
                if (o != null)
                {
                    if (o is float)
                        f = (float)o;
                    else if (o is string)
                    {
                        try
                        {
                            f = float.Parse((string)o);
                            Put("height", f);
                        }
                        catch (Exception) { }
                    }
                }
                else
                    Put("height", f);

                return f;
            }
            set
            {
                Put("height", value);
            }
        }
        public float Rotation
        {
            get
            {
                object o = Get("rotation");
                float f = default_Rotation;
                if (o != null)
                {
                    if (o is float)
                        f = (float)o;
                    else if (o is string)
                    {
                        try
                        {
                            f = float.Parse((string)o);
                            Put("rotation", f);
                        }
                        catch (Exception) { }
                    }
                }
                else
                    Put("rotation", f);

                return f;
            }
            set
            {
                Put("rotation", value);
            }
        }
        public Color Color
        {
            get
            {
                object o = Get("color");
                if (o == null || o is string)
                {
                    Color c = Winger.UI.Helper.Utils.ColorDecoder((string)o, default_Color);
                    Put("color", c);
                    return c;
                }
                else
                    return (Color)o;
            }
            set
            {
                Put("color", value);
            }
        }
        public Color TextColor
        {
            get
            {
                object o = Get("textcolor");
                if (o == null || o is string)
                {
                    Color c = Winger.UI.Helper.Utils.ColorDecoder((string)o, default_TextColor);
                    Put("textcolor", c);
                    return c;
                }
                else
                    return (Color)o;
            }
            set
            {
                Put("textcolor", value);
            }
        }
        public string Text
        {
            get
            {
                try
                {
                    string s = (string)Get("text");
                    s = s.Replace("\n", "").Replace("\t", "");
                    return s;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                Put("text", value);
            }
        }
        public Alignment Alignment
        {
            get
            {
                object o = Get("alignment");
                Alignment a = default_Alignment;
                if (o != null)
                {
                    if (o is Alignment)
                        a = (Alignment)o;
                    else if (o is string)
                    {
                        a = Winger.UI.Helper.Utils.GetAlignmentFromString((string)o, a);
                        Put("alignment", a);
                    }
                }
                else
                    Put("alignment", a);

                return a;
            }
            set
            {
                Put("alignment", value);
            }
        }
        public string Type
        {
            get
            {
                try
                {
                    return (string)Get("type");
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                Put("type", value);
            }
        }
        public bool IsEnabled
        {
            get
            {
                object o = Get("isenabled");
                bool b = default_IsEnabled;
                if (o is bool)
                    b = (bool)o;
                else if (o is string)
                {
                    if (((string)o) == "false")
                    {
                        b = false;
                        Put("isenabled", b);
                    }
                }
                else
                {
                    Put("isenabled", b);
                }
                return b;
            }
            set
            {
                Put("isenabled", value);
            }
        }
        public bool IsVisible
        {
            get
            {
                object o = Get("isvisible");
                bool b = default_IsVisible;
                if (o is bool)
                    b = (bool)o;
                else if (o is string)
                {
                    if (((string)o) == "false")
                    {
                        b = false;
                        Put("isvisible", b);
                    }
                }
                else
                {
                    Put("isvisible", b);
                }
                return b;
            }
            set
            {
                Put("isvisible", value);
            }
        }
        public Texture2D Texture
        {
            get
            {
                return t2d_neutral;
            }
            set
            {
                t2d_neutral = value;
                Put("texture", "undefined");
            }
        }
        public Texture2D TextureHover
        {
            get
            {
                return t2d_hover;
            }
            set
            {
                t2d_hover = value;
                Put("texturehover", "undefined");
            }
        }
        public Texture2D TextureSelect
        {
            get
            {
                return t2d_select;
            }
            set
            {
                t2d_select = value;
                Put("textureselect", "undefined");
            }
        }
        public SpriteFont Font
        {
            get
            {
                return font;
            }
            set
            {
                font = value;
                Put("font", "undefined");
            }
        }
        public float Depth
        {
            get
            {
                object o = Get("depth");
                float f = default_Depth;
                if (o != null)
                {
                    if (o is float)
                        f = (float)o;
                    else if (o is string)
                    {
                        try
                        {
                            f = float.Parse((string)o);
                            Put("depth", f);
                        }
                        catch (Exception) { }
                    }
                }
                else
                    Put("depth", f);

                if (f == 0)
                {
                    f += 0.0001f;
                    Put("depth", f);
                }

                return f;
            }
            set
            {
                Put("depth", value);
            }
        }
        public string HoverStartEventName
        {
            get
            {
                try
                {
                    return (string)Get("hoverstartevent");
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                Put("hoverstartevent", value);
            }
        }
        public string HoverEndEventName
        {
            get
            {
                try
                {
                    return (string)Get("hoverendevent");
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                Put("hoverendevent", value);
            }
        }
        public string SelectStartEventName
        {
            get
            {
                try
                {
                    return (string)Get("selectstartevent");
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                Put("selectstartevent", value);
            }
        }
        public string SelectEndEventName
        {
            get
            {
                try
                {
                    return (string)Get("selectendevent");
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                Put("selectendevent", value);
            }
        }
        public Winger.UI.Tween.Tween Tween
        {
            get
            {
                return tween;
            }
            set
            {
                tween = value;
                if (tween.Pos == Vector2.Zero)
                    tween.Pos = new Vector2(X, Y);
            }
        }
        public bool HasFocus
        {
            get
            {
                object o = Get("hasfocus");
                bool b = default_HasFocus;
                if (o is bool)
                    b = (bool)o;
                else if (o is string)
                {
                    if (((string)o) == "true")
                    {
                        b = true;
                        Put("hasfocus", b);
                    }
                }
                else
                {
                    Put("hasfocus", b);
                }
                return b;
            }
            set
            {
                Put("hasfocus", value);
            }
        }

        #region Default Properties
        protected float default_X = 0;
        protected float default_Y = 0;
        protected float default_Width = 0;
        protected float default_Height = 0;
        protected float default_Rotation = 0;
        protected Color default_Color = Color.White;
        protected Color default_TextColor = Color.Black;
        protected Alignment default_Alignment = Alignment.CENTER;
        protected bool default_IsEnabled = true;
        protected bool default_IsVisible = true;
        protected float default_Depth = 1;
        protected bool default_HasFocus = false;
        #endregion

        protected Dictionary<string, object> properties = new Dictionary<string, object>();
        
        protected Texture2D t2d_neutral = null;
        protected Texture2D t2d_hover = null;
        protected Texture2D t2d_select = null;
        protected Texture2D tex = null;

        protected SpriteFont font = null;

        protected bool isHover = false;
        protected bool isSelected = false;

        protected Winger.UI.Tween.Tween tween = null;

        protected Vector2 mouseDownPosition = Vector2.Zero;

        public virtual void LoadContent(ContentManager Content)
        {
            if (Get("texture") != null)
                t2d_neutral = Content.Load<Texture2D>((string)Get("texture"));
            if (Get("texturehover") != null)
                t2d_hover = Content.Load<Texture2D>((string)Get("texturehover"));
            if (Get("textureselect") != null)
                t2d_select = Content.Load<Texture2D>((string)Get("textureselect"));
            if (Get("font") != null)
                font = Content.Load<SpriteFont>((string)Get("font"));
        }

        public virtual void Update(int elapsedMilliseconds, UIObject root, CMouse mouse)
        {
            if (root.IsEnabled && tween != null && IsEnabled)
            {
                tween.Update(elapsedMilliseconds);
                X = tween.X;
                Y = tween.Y;
            }

            if (root.IsEnabled && this != root && IsEnabled)
            {
                CRectangle hitBox = new CRectangle(X, Y, Width, Height, GetSpriteOrigin(Width, Height), Rotation);
                bool mouseInside = false;
                if (hitBox.Contains(mouse.X, mouse.Y))
                    mouseInside = true;
                bool mouseDown = false;
                if (mouse.LeftJustPressed() || mouse.LeftHeldDown())
                    mouseDown = true;
                bool mouseJustDown = mouse.LeftJustPressed();
                bool mouseJustUp = mouse.LeftJustReleased();

                if (mouseJustDown)
                    mouseDownPosition = mouse.MousePos;

                if (mouseJustDown && !mouseInside)
                    NeutralizeEvent();

                if (IsNeutral() && mouseInside)
                {
                    HoverStartEvent();
                }
                else if (IsHover() && !mouseInside)
                {
                    HoverEndEvent();
                }
                else if (IsHover() && mouseJustDown)
                {
                    SelectStartEvent();
                }
                else if (IsSelected() && mouseInside && !mouseDown)
                {
                    SelectEndEvent();
                    HoverStartEvent();
                }
                else if (IsSelected() && !mouseInside && !mouseDown)
                {
                    NeutralizeEvent();
                }
            }
        }

        public virtual void Draw(UIObject root, SpriteBatch spriteBatch)
        {
            if (root.IsVisible && this != root && IsVisible)
            {
                tex = null;
                if (IsNeutral())
                    tex = t2d_neutral;
                else if (IsHover())
                    tex = t2d_hover;
                else if (IsSelected())
                    tex = t2d_select;

                if (IsHover() && t2d_hover == null)
                    tex = t2d_neutral;
                else if (IsSelected() && t2d_select == null)
                    tex = t2d_neutral;

                if (tex != null)
                {
                    spriteBatch.Draw(tex,
                        new Rectangle((int)(X + root.X), (int)(Y + root.Y), (int)(Width + root.Width), (int)(Height + root.Height)),
                        null,
                        Color,
                        Rotation + root.Rotation,
                        GetSpriteOrigin(tex.Width, tex.Height),
                        SpriteEffects.None,
                        Depth);
                }

                if (font != null && Text != null && Text != "")
                {
                    Vector2 offset = Winger.UI.Helper.Utils.GetTextOffset(Alignment, font.MeasureString(Text));
                    spriteBatch.DrawString(font, Text, new Vector2(X + root.X, Y + root.Y), TextColor, Rotation, offset, 1, SpriteEffects.None, Depth - 0.0001f);
                }
            }
        }


        public virtual Vector2 GetSpriteOrigin(float width, float height)
        {
            return new Vector2(width / 2f, height / 2f);
        }


        public virtual object Get(string propertyName)
        {
            try
            {
                return properties[propertyName];
            }
            catch (Exception)
            {
                Put(propertyName, null);
                return null;
            }
        }

        public virtual void Put(string propertyName, object propertyValue)
        {
            properties[propertyName] = propertyValue;
        }


        public virtual void StartTweenIfExists()
        {
            if (Tween != null)
                Tween.Start();
        }

        public virtual void PauseTweenIfExists()
        {
            if (Tween != null)
                Tween.Pause();
        }

        public virtual void UnPauseTweenIfExists()
        {
            if (Tween != null)
                Tween.UnPause();
        }


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


        public virtual void HoverStartEvent()
        {
            isHover = true;
            isSelected = false;
            if (Delegate != null)
                Delegate(this, UIEventType.HOVER_START, HoverStartEventName);
        }

        public virtual void HoverEndEvent()
        {
            isHover = false;
            if (Delegate != null)
                Delegate(this, UIEventType.HOVER_END, HoverEndEventName);
        }

        public virtual void SelectStartEvent()
        {
            isSelected = true;
            isHover = false;
            HasFocus = true;
            if (Delegate != null)
                Delegate(this, UIEventType.SELECT_START, SelectStartEventName);
        }

        public virtual void SelectEndEvent()
        {
            isSelected = false;
            if (Delegate != null)
                Delegate(this, UIEventType.SELECT_END, SelectEndEventName);
        }

        public virtual void NeutralizeEvent()
        {
            isHover = false;
            isSelected = false;
            HasFocus = false;
        }


        public override string ToString()
        {
            string s = "";
            s += "<" + Tag;

            Dictionary<string, object>.KeyCollection.Enumerator it = properties.Keys.GetEnumerator();
            while (it.MoveNext())
                if (it.Current != "tag" && it.Current != "text")
                    s += " " + it.Current + "=\"" + properties[it.Current] + "\"";

            s += ">";

            if (Text != null)
                s += Text;

            s += "</" + Tag + ">";
            return s;
        }
    }
}
