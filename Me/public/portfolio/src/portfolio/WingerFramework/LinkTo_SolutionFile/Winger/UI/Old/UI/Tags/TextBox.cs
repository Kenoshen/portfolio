using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Winger.UI.Input;

namespace Winger.UI.UI.Tags
{
    public class TextBox : UIObject
    {
        public int CursorIndex
        {
            get
            {
                object o = Get("cursorindex");
                int f = default_CursorIndex;
                if (o != null)
                {
                    if (o is int)
                        f = (int)o;
                    else if (o is string)
                    {
                        try
                        {
                            f = int.Parse((string)o);
                            Put("cursorindex", f);
                        }
                        catch (Exception) { }
                    }
                }
                else
                    Put("cursorindex", f);

                return f;
            }
            set
            {
                Put("cursorindex", value);
            }
        }

        public float Padding
        {
            get
            {
                object o = Get("padding");
                float f = default_Padding;
                if (o != null)
                {
                    if (o is float)
                        f = (float)o;
                    else if (o is string)
                    {
                        try
                        {
                            f = float.Parse((string)o);
                            Put("padding", f);
                        }
                        catch (Exception) { }
                    }
                }
                else
                    Put("padding", f);

                return f;
            }
            set
            {
                Put("padding", value);
            }
        }

        protected int default_CursorIndex = 0;
        protected float default_Padding = 0;

        protected bool capsLockOn = false;
        protected bool shiftDown = false;

        protected int keyHoldTimeBeforeSpam = 45;

        protected CKeyboard keyboard;

        public TextBox()
        {
            default_Alignment = Winger.UI.Helper.Alignment.LEFT;
            keyboard = new CKeyboard();
        }

        public override void Update(int elapsedMilliseconds, UIObject root, CMouse mouse)
        {
            keyboard.Update();
            if (root.IsEnabled && IsEnabled && HasFocus)
            {
                Keys[] keyspressed = keyboard.CurrentKeysDown();
                shiftDown = (keyboard.IsKeyBeingPressed(Keys.LeftShift) || keyboard.IsKeyBeingPressed(Keys.RightShift));
                if (keyboard.IsKeyJustPressed(Keys.CapsLock))
                    capsLockOn = !capsLockOn;
                foreach (Keys k in keyspressed)
                {
                    if (keyboard.IsKeyJustPressed(k) || keyboard.IsKeyBeingHeld(k, keyHoldTimeBeforeSpam))
                        Text = TypeKeyIntoString(k, shiftDown, capsLockOn);
                }
                if (keyboard.IsKeyJustPressed(Keys.Enter) || keyboard.IsKeyJustPressed(Keys.Escape))
                    NeutralizeEvent();
            }

            base.Update(elapsedMilliseconds, root, mouse);
        }

        public override Vector2 GetSpriteOrigin(float width, float height)
        {
            return new Vector2(Padding, height / 2);
        }

        public override void SelectStartEvent()
        {
            base.SelectStartEvent();
            CursorIndex = Text.Length;
        }

        public string TypeKeyIntoString(Keys key, bool shiftDown, bool capsLockOn)
        {
            if (key == Keys.Back && CursorIndex != 0)
            {
                CursorIndex = CursorIndex - 1;
                return Text.Remove(CursorIndex, 1);
            }
            else if (key == Keys.Delete && CursorIndex < Text.Length)
            {
                return Text.Remove(CursorIndex, 1);
            }
            else
            {
                string k = "";

                if (key == Keys.A)
                    k = "a";
                else if (key == Keys.B)
                    k = "b";
                else if (key == Keys.C)
                    k = "c";
                else if (key == Keys.D)
                    k = "d";
                else if (key == Keys.E)
                    k = "e";
                else if (key == Keys.F)
                    k = "f";
                else if (key == Keys.G)
                    k = "g";
                else if (key == Keys.H)
                    k = "h";
                else if (key == Keys.I)
                    k = "i";
                else if (key == Keys.J)
                    k = "j";
                else if (key == Keys.K)
                    k = "k";
                else if (key == Keys.L)
                    k = "l";
                else if (key == Keys.M)
                    k = "m";
                else if (key == Keys.N)
                    k = "n";
                else if (key == Keys.O)
                    k = "o";
                else if (key == Keys.P)
                    k = "p";
                else if (key == Keys.Q)
                    k = "q";
                else if (key == Keys.R)
                    k = "r";
                else if (key == Keys.S)
                    k = "s";
                else if (key == Keys.T)
                    k = "t";
                else if (key == Keys.U)
                    k = "u";
                else if (key == Keys.V)
                    k = "v";
                else if (key == Keys.W)
                    k = "w";
                else if (key == Keys.X)
                    k = "x";
                else if (key == Keys.Y)
                    k = "y";
                else if (key == Keys.Z)
                    k = "z";
                else if (key == Keys.Divide)
                    k = "/";
                else if (key == Keys.Multiply)
                    k = "*";
                else if (key == Keys.Add)
                    k = "+";
                else if (key == Keys.Subtract)
                    k = "-";
                else if (key == Keys.Space)
                    k = " ";
                else if (key == Keys.NumPad0)
                    k = "0";
                else if (key == Keys.NumPad1)
                    k = "1";
                else if (key == Keys.NumPad2)
                    k = "2";
                else if (key == Keys.NumPad3)
                    k = "3";
                else if (key == Keys.NumPad4)
                    k = "4";
                else if (key == Keys.NumPad5)
                    k = "5";
                else if (key == Keys.NumPad6)
                    k = "6";
                else if (key == Keys.NumPad7)
                    k = "7";
                else if (key == Keys.NumPad8)
                    k = "8";
                else if (key == Keys.NumPad9)
                    k = "9";

                if (shiftDown || capsLockOn)
                    k = k.ToUpper();

                if (!shiftDown && k == "")
                {
                    if (key == Keys.D0)
                        k = "0";
                    else if (key == Keys.D1)
                        k = "1";
                    else if (key == Keys.D2)
                        k = "2";
                    else if (key == Keys.D3)
                        k = "3";
                    else if (key == Keys.D4)
                        k = "4";
                    else if (key == Keys.D5)
                        k = "5";
                    else if (key == Keys.D6)
                        k = "6";
                    else if (key == Keys.D7)
                        k = "7";
                    else if (key == Keys.D8)
                        k = "8";
                    else if (key == Keys.D9)
                        k = "9";
                    else if (key == Keys.OemBackslash)
                        k = "\\";
                    else if (key == Keys.OemCloseBrackets)
                        k = "]";
                    else if (key == Keys.OemComma)
                        k = ",";
                    else if (key == Keys.OemMinus)
                        k = "-";
                    else if (key == Keys.OemOpenBrackets)
                        k = "[";
                    else if (key == Keys.OemPeriod)
                        k = ".";
                    else if (key == Keys.OemPipe)
                        k = "|";
                    else if (key == Keys.OemPlus)
                        k = "=";
                    else if (key == Keys.OemQuestion)
                        k = "/";
                    else if (key == Keys.OemQuotes)
                        k = "'";
                    else if (key == Keys.OemSemicolon)
                        k = ";";
                    else if (key == Keys.OemTilde)
                        k = "`";
                }
                else if (shiftDown && k == "")
                {
                    if (key == Keys.D0)
                        k = ")";
                    else if (key == Keys.D1)
                        k = "!";
                    else if (key == Keys.D2)
                        k = "@";
                    else if (key == Keys.D3)
                        k = "#";
                    else if (key == Keys.D4)
                        k = "$";
                    else if (key == Keys.D5)
                        k = "%";
                    else if (key == Keys.D6)
                        k = "^";
                    else if (key == Keys.D7)
                        k = "&";
                    else if (key == Keys.D8)
                        k = "*";
                    else if (key == Keys.D9)
                        k = "(";
                    else if (key == Keys.OemBackslash)
                        k = "|";
                    else if (key == Keys.OemCloseBrackets)
                        k = "}";
                    else if (key == Keys.OemComma)
                        k = "<";
                    else if (key == Keys.OemMinus)
                        k = "_";
                    else if (key == Keys.OemOpenBrackets)
                        k = "{";
                    else if (key == Keys.OemPeriod)
                        k = ">";
                    else if (key == Keys.OemPipe)
                        k = "|";
                    else if (key == Keys.OemPlus)
                        k = "+";
                    else if (key == Keys.OemQuestion)
                        k = "?";
                    else if (key == Keys.OemQuotes)
                        k = "\"";
                    else if (key == Keys.OemSemicolon)
                        k = ":";
                    else if (key == Keys.OemTilde)
                        k = "~";
                }

                if (key == Keys.Left && CursorIndex != 0)
                    CursorIndex = CursorIndex - 1;
                else if (key == Keys.Right && CursorIndex < Text.Length)
                    CursorIndex = CursorIndex + 1;

                //if (key == Keys.Tab)
                //    k = "\t";

                if (k != "")
                {
                    CursorIndex = CursorIndex + 1;
                    return Text.Insert(CursorIndex - 1, k);
                }
                else
                    return Text;
            }
        }
    }
}
