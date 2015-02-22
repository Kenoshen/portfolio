using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WPS.Input
{
    public class CMouse
    {
        /// <summary>
        /// The current mouse state
        /// </summary>
        public MouseState CurMouse { get; private set; }

        /// <summary>
        /// The previous mouse state
        /// </summary>
        public MouseState LastMouse { get; private set; }

        /// <summary>
        /// The 2D position of the mouse on the screen in pixels.
        /// </summary>
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

        /// <summary>
        /// The X coordinate of the mouse on the screen in pixels.
        /// </summary>
        public int X
        {
            get { return CurMouse.X; }
            set { Mouse.SetPosition(value, CurMouse.Y); }
        }

        /// <summary>
        /// The Y coordinate of the mouse on the screen in pixels.
        /// </summary>
        public int Y
        {
            get { return CurMouse.Y; }
            set { Mouse.SetPosition(CurMouse.X, value); }
        }

        private bool fullUpdate = true;

        /// <summary>
        /// A custom mouse class that helps simplify the use of the mouse input.
        /// </summary>
        public CMouse()
        {
            CurMouse = Mouse.GetState();
            LastMouse = Mouse.GetState();
        }

        /// <summary>
        /// This must be called before any of the methods of the custom mouse are used.  (Think of it like spriteBatch.Begin()).
        /// </summary>
        public void BeginUpdate()
        {
            if (fullUpdate)
            {
                CurMouse = Mouse.GetState();
                fullUpdate = false;
            }
            else
                throw new Exception("Must end the mouse update before begining another one.");
        }

        /// <summary>
        /// This must be called before any of the methods of the custom mouse are used.  (Think of it like spriteBatch.Begin()).
        /// </summary>
        /// <param name="mState">this is if you want to use your own mouse state as the begin state</param>
        public void BeginUpdate(MouseState mState)
        {
            if (fullUpdate)
            {
                CurMouse = mState;
                fullUpdate = false;
            }
            else
                throw new Exception("Must end the mouse update before begining another one.");
        }

        /// <summary>
        /// This must be called after you are done using the methods of the custom mouse.  (Think of it like spriteBatch.End()).
        /// </summary>
        public void EndUpdate()
        {
            if (fullUpdate)
                throw new Exception("Must begin the mouse update before ending it.");
            else
            {
                LastMouse = Mouse.GetState();
                fullUpdate = true;
            }
        }

        /// <summary>
        /// This must be called after you are done using the methods of the custom mouse.  (Think of it like spriteBatch.End()).
        /// </summary>
        /// <param name="mState">this is if you want to use your own mouse state as the end state</param>
        public void EndUpdate(MouseState mState)
        {
            if (fullUpdate)
                throw new Exception("Must begin the mouse update before ending it.");
            else
            {
                LastMouse = mState;
                fullUpdate = true;
            }
        }

        /// <summary>
        /// Checks if the left mouse button has just been pressed within the last frame and the current frame
        /// </summary>
        /// <returns>true if the button was just pressed within the last frame and the current frame</returns>
        public bool LeftJustPressed()
        {
            if (CurMouse.LeftButton == ButtonState.Pressed && LastMouse.LeftButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the left mouse button has just been released within the last frame and the current frame
        /// </summary>
        /// <returns>true if the button was just released within the last frame and the current frame</returns>
        public bool LeftJustReleased()
        {
            if (CurMouse.LeftButton == ButtonState.Released && LastMouse.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the left mouse button was down on the last frame as well as the current frame
        /// </summary>
        /// <returns>true if the button was down on the last frame as well as the current frame</returns>
        public bool LeftHeldDown()
        {
            if (CurMouse.LeftButton == ButtonState.Pressed && LastMouse.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the right mouse button has just been pressed within the last frame and the current frame
        /// </summary>
        /// <returns>true if the button was just pressed within the last frame and the current frame</returns>
        public bool RightJustPressed()
        {
            if (CurMouse.RightButton == ButtonState.Pressed && LastMouse.RightButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the right mouse button has just been released within the last frame and the current frame
        /// </summary>
        /// <returns>true if the button was just released within the last frame and the current frame</returns>
        public bool RightJustReleased()
        {
            if (CurMouse.RightButton == ButtonState.Released && LastMouse.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the right mouse button was down on the last frame as well as the current frame
        /// </summary>
        /// <returns>true if the button was down on the last frame as well as the current frame</returns>
        public bool RightHeldDown()
        {
            if (CurMouse.RightButton == ButtonState.Pressed && LastMouse.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the middle mouse button has just been pressed within the last frame and the current frame
        /// </summary>
        /// <returns>true if the button was just pressed within the last frame and the current frame</returns>
        public bool MiddleJustPressed()
        {
            if (CurMouse.MiddleButton == ButtonState.Pressed && LastMouse.MiddleButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the middle mouse button has just been released within the last frame and the current frame
        /// </summary>
        /// <returns>true if the button was just released within the last frame and the current frame</returns>
        public bool MiddleJustReleased()
        {
            if (CurMouse.MiddleButton == ButtonState.Released && LastMouse.MiddleButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the middle mouse button was down on the last frame as well as the current frame
        /// </summary>
        /// <returns>true if the button was down on the last frame as well as the current frame</returns>
        public bool MiddleHeldDown()
        {
            if (CurMouse.MiddleButton == ButtonState.Pressed && LastMouse.MiddleButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets the difference of the scroll wheel from the last frame to the current frame
        /// </summary>
        /// <returns>the difference as an int</returns>
        public int GetScrollDifference()
        {
            return CurMouse.ScrollWheelValue - LastMouse.ScrollWheelValue;
        }

        /// <summary>
        /// Gets the difference of the mouse position from the last frame to the current frame
        /// </summary>
        /// <returns>the 2D difference in pixels</returns>
        public Vector2 GetPositionDifference()
        {
            return new Vector2(CurMouse.X, CurMouse.Y) - new Vector2(LastMouse.X, LastMouse.Y);
        }
    }
}
