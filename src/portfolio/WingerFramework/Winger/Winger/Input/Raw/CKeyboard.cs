using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Winger.Utils;

namespace Winger.Input.Raw
{
    public class CKeyboard
    {
        public KeyboardState State { get; private set; }

        public Notifier<CKeyboardEventHandler, Keys, ButtonState> KeyNotifier { get; private set; }

        private Keys[] lastKeysDown = new Keys[0];
        private Keys[] curKeysDown = new Keys[0];


        public CKeyboard()
        {
            KeyNotifier = new Notifier<CKeyboardEventHandler, Keys, ButtonState>();
            State = Keyboard.GetState();
        }


        public void Update()
        {
            State = Keyboard.GetState();
            lastKeysDown = curKeysDown;
            curKeysDown = State.GetPressedKeys();
            if (!KeyNotifier.IsEmpty())
            {
                DoKeyEvents();
            }
        }


        #region State checks
        public bool IsKeyBeingPressed(Keys key)
        {
            return Contains(curKeysDown, key);
        }


        public bool IsKeyJustPressed(Keys key)
        {
            if (Contains(curKeysDown, key) && !Contains(lastKeysDown, key))
                return true;
            else
                return false;
        }


        public bool IsKeyJustReleased(Keys key)
        {
            if (!Contains(curKeysDown, key) && Contains(lastKeysDown, key))
                return true;
            else
                return false;
        }


        public bool IsKeyBeingHeld(Keys key)
        {
            if (Contains(curKeysDown, key) && Contains(lastKeysDown, key))
                return true;
            else
                return false;
        }


        public Keys[] CurrentKeysDown()
        {
            return curKeysDown;
        }


        public Keys[] LastKeysDown()
        {
            return lastKeysDown;
        }


        public Keys[] KeysJustDown()
        {
            List<Keys> keysJustDown = new List<Keys>();
            for (int i = 0; i < curKeysDown.Length; i++)
            {
                if (!Contains(lastKeysDown, curKeysDown[i]))
                {
                    keysJustDown.Add(curKeysDown[i]);
                }
            }
            return keysJustDown.ToArray();
        }


        public Keys[] KeysJustUp()
        {
            List<Keys> keysJustUp = new List<Keys>();
            for (int i = 0; i < lastKeysDown.Length; i++)
            {
                if (!Contains(curKeysDown, lastKeysDown[i]))
                {
                    keysJustUp.Add(lastKeysDown[i]);
                }
            }
            return keysJustUp.ToArray();
        }


        private bool Contains(Keys[] keys, Keys key)
        {
            for (int i = 0; i < keys.Length; i++)
                if (keys[i] == key)
                    return true;
            return false;
        }


        private void DoKeyEvents()
        {
            foreach (Keys lastkey in lastKeysDown)
            {
                if (!Contains(curKeysDown, lastkey))
                {
                    NotifyButtonEventHandlers(lastkey, ButtonState.Released);
                }
            }

            foreach (Keys curkey in curKeysDown)
            {
                if(!Contains(lastKeysDown, curkey))
                {
                    NotifyButtonEventHandlers(curkey, ButtonState.Pressed);
                }
            }
        }
        #endregion


        /// <summary>
        /// Sends an event to the key event handlers
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="state">the state of the button</param>
        public void NotifyButtonEventHandlers(Keys key, ButtonState state)
        {
            List<SubscriptionRecord<CKeyboardEventHandler, Keys, ButtonState>> subsToNotify = KeyNotifier.GetSubscribersToNotify(key, state);
            foreach (SubscriptionRecord<CKeyboardEventHandler, Keys, ButtonState> record in subsToNotify)
            {
                record.Handler.HandleKeyEvent(this, key, state);
            }
        }


        /// <summary>
        /// Subscribes to all keyboard event types with a given state
        /// </summary>
        /// <param name="handler">the handler to register</param>
        /// <param name="state">the button state to register for</param>
        public void SubscribeToAllKeyboardEvents(CKeyboardEventHandler handler, ButtonState state)
        {
            foreach (Keys key in EnumUtils.GetValues<Keys>())
            {
                KeyNotifier.SubscribeToEvent(handler, key, state);
            }
        }
    }

    public interface CKeyboardEventHandler
    {
        void HandleKeyEvent(CKeyboard keyboard, Keys key, ButtonState state);
    }
}
