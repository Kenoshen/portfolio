using System;
using Microsoft.Xna.Framework.Input;

namespace Winger.UI.Input
{
    public class CKeyboard
    {
        private string[] keysDown = new string[70];
        
        public CKeyboard()
        {
            for (int i = 0; i < keysDown.Length; i++)
                keysDown[i] = "";
        }

        public CKeyboard(int framesToKeepTrackOf)
        {
            keysDown = new string[framesToKeepTrackOf];
            for (int i = 0; i < keysDown.Length; i++)
                keysDown[i] = "";
        }

        public void Update()
        {
            for (int i = keysDown.Length - 1; i > 0; i--)
                keysDown[i] = keysDown[i - 1];
            keysDown[0] = "";
            Keys[] thisKeyDown = Keyboard.GetState().GetPressedKeys();
            for (int i = 0; i < thisKeyDown.Length; i++)
            {
                keysDown[0] += thisKeyDown[i].ToString() + ",";
            }
        }

        public bool IsKeyBeingPressed(Keys key)
        {
            return Contains(key, 0);
        }

        public bool IsKeyJustPressed(Keys key)
        {
            if (Contains(key, 0) && !Contains(key, 1))
                return true;
            else
                return false;
        }

        public bool IsKeyJustReleased(Keys key)
        {
            if (!Contains(key, 0) && Contains(key, 1))
                return true;
            else
                return false;
        }

        public bool IsKeyBeingHeld(Keys key, int minNumOfFrames)
        {
            if (minNumOfFrames < 1)
                minNumOfFrames = 1;
            else if (minNumOfFrames > keysDown.Length)
                minNumOfFrames = keysDown.Length;

            for (int i = 0; i < minNumOfFrames; i++)
            {
                if (!Contains(key, i))
                    return false;
            }
            return true;
        }

        public Keys[] CurrentKeysDown()
        {
            return Keyboard.GetState().GetPressedKeys();
        }

        public KeyboardState GetState()
        {
            return Keyboard.GetState();
        }

        private bool Contains(Keys key, int frame)
        {
            return Contains(key.ToString(), frame);
        }

        private bool Contains(string key, int frame)
        {
            return keysDown[frame].Contains(key);
        }
    }
}
