using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Winger.UI.Input
{
    public class CMouse
    {
        public MouseState CurMouse { get; private set; }
        public MouseState LastMouse { get; private set; }

        public Vector2 MousePos
        {
            get
            {
                return new Vector2(CurMouse.X, CurMouse.Y);
            }

            set
            {
                Mouse.SetPosition((int)value.X, (int)value.Y);
            }
        }

        public int X
        {
            get { return CurMouse.X; }
            set { Mouse.SetPosition(value, CurMouse.Y); }
        }

        public int Y
        {
            get { return CurMouse.Y; }
            set { Mouse.SetPosition(CurMouse.X, value); }
        }

        public CMouse()
        {
            CurMouse = Mouse.GetState();
            LastMouse = Mouse.GetState();
        }

        public void Update()
        {
            LastMouse = CurMouse;
            CurMouse = Mouse.GetState();
        }

        public bool LeftJustPressed()
        {
            if (CurMouse.LeftButton == ButtonState.Pressed && LastMouse.LeftButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        public bool LeftJustReleased()
        {
            if (CurMouse.LeftButton == ButtonState.Released && LastMouse.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public bool LeftHeldDown()
        {
            if (CurMouse.LeftButton == ButtonState.Pressed && LastMouse.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public bool RightJustPressed()
        {
            if (CurMouse.RightButton == ButtonState.Pressed && LastMouse.RightButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        public bool RightJustReleased()
        {
            if (CurMouse.RightButton == ButtonState.Released && LastMouse.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public bool RightHeldDown()
        {
            if (CurMouse.RightButton == ButtonState.Pressed && LastMouse.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public bool MiddleJustPressed()
        {
            if (CurMouse.MiddleButton == ButtonState.Pressed && LastMouse.MiddleButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        public bool MiddleJustReleased()
        {
            if (CurMouse.MiddleButton == ButtonState.Released && LastMouse.MiddleButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public bool MiddleHeldDown()
        {
            if (CurMouse.MiddleButton == ButtonState.Pressed && LastMouse.MiddleButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public int GetScrollDifference()
        {
            return CurMouse.ScrollWheelValue - LastMouse.ScrollWheelValue;
        }

        public Vector2 GetPositionDifference()
        {
            return new Vector2(CurMouse.X, CurMouse.Y) - new Vector2(LastMouse.X, LastMouse.Y);
        }
    }
}
