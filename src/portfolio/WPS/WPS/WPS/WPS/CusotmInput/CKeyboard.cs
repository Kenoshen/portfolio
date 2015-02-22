using System;
using Microsoft.Xna.Framework.Input;

namespace WPS.Input
{
    public class CKeyboard
    {
        /// <summary>
        /// The current keyboard state
        /// </summary>
        public KeyboardState CurKeyboard { get; private set; }

        /// <summary>
        /// The previous keyboard state
        /// </summary>
        public KeyboardState LastKeyboard { get; private set; }

        private bool fullUpdate = true;

        /// <summary>
        /// A custom keyboard class that simplifies the use of keyboard input
        /// </summary>
        public CKeyboard()
        {
            CurKeyboard = Keyboard.GetState();
            LastKeyboard = Keyboard.GetState();
        }

        /// <summary>
        /// This must be called before any of the methods of the custom keyboard are used.  (Think of it like spriteBatch.Begin()).
        /// </summary>
        public void BeginUpdate()
        {
            if (fullUpdate)
            {
                CurKeyboard = Keyboard.GetState();
                fullUpdate = false;
            }
            else
                throw new Exception("Must end the keyboard update before begining another one.");
        }

        /// <summary>
        /// This must be called before any of the methods of the custom keyboard are used.  (Think of it like spriteBatch.Begin()).
        /// </summary>
        /// <param name="kState">this is if you want to use your own keyboard state as the begin state</param>
        public void BeginUpdate(KeyboardState kState)
        {
            if (fullUpdate)
            {
                CurKeyboard = kState;
                fullUpdate = false;
            }
            else
                throw new Exception("Must end the keyboard update before begining another one.");
        }

        /// <summary>
        /// This must be called after you are done using the methods of the custom keyboard.  (Think of it like spriteBatch.End()).
        /// </summary>
        public void EndUpdate()
        {
            if (fullUpdate)
                throw new Exception("Must begin the keyboard update before ending it.");
            else
            {
                LastKeyboard = Keyboard.GetState();
                fullUpdate = true;
            }
        }

        /// <summary>
        /// This must be called after you are done using the methods of the custom keyboard.  (Think of it like spriteBatch.End()).
        /// </summary>
        /// <param name="mState">this is if you want to use your own keyboard state as the end state</param>
        public void EndUpdate(KeyboardState kState)
        {
            if (fullUpdate)
                throw new Exception("Must begin the keyboard update before ending it.");
            else
            {
                LastKeyboard = kState;
                fullUpdate = true;
            }
        }

        /// <summary>
        /// Checks if the given key is down in the current frame
        /// </summary>
        /// <param name="key">the key to check</param>
        /// <returns>true if the key is down in the current frame</returns>
        public bool IsKeyBeingPressed(Keys key)
        {
            return CurKeyboard.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if the given key is up in the previous frame and down in the current frame
        /// </summary>
        /// <param name="key">the key to check</param>
        /// <returns>true if the key is up in the previous frame and down in the current frame</returns>
        public bool IsKeyJustPressed(Keys key)
        {
            if (CurKeyboard.IsKeyDown(key) && LastKeyboard.IsKeyUp(key))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the given key is down in the previous frame and up in the current frame
        /// </summary>
        /// <param name="key">the key to check</param>
        /// <returns>true if the key is down in the previous frame and up in the current frame</returns>
        public bool IsKeyJustReleased(Keys key)
        {
            if (CurKeyboard.IsKeyUp(key) && LastKeyboard.IsKeyDown(key))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the given key is down in the previous frame and down in the current frame
        /// </summary>
        /// <param name="key">the key to check</param>
        /// <returns>true if the key is down in the previous frame and down in the current frame</returns>
        public bool IsKeyBeingHeld(Keys key)
        {
            if (CurKeyboard.IsKeyDown(key) && LastKeyboard.IsKeyDown(key))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets an array of keys that are down in the current frame
        /// </summary>
        /// <returns>array of keys that are down in the current frame</returns>
        public Keys[] CurrentKeysDown()
        {
            return CurKeyboard.GetPressedKeys();
        }

        /// <summary>
        /// Gets an array of keys that are down in the previous frame
        /// </summary>
        /// <returns>array of keys that are down in the previous frame</returns>
        public Keys[] LastKeysDown()
        {
            return LastKeyboard.GetPressedKeys();
        }
    }
}
