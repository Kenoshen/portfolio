using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Winger.Input.Event;
using Winger.Input.Raw;
using Winger.Utils;

namespace Winger.Input.Impl
{
    public class MouseKeyboardInput : UserInput, CMouseEventHandler, CKeyboardEventHandler
    {
        private CMouse mouse;
        private CKeyboard keyboard;
        private Dictionary<InputEvent, List<Keys>> eventsToKeys;
        private Dictionary<Keys, List<InputEvent>> keysToEvents;
        private Dictionary<string, List<Keys>> moveToKeys;
        private Dictionary<InputEvent, CMouseButton> eventsToMouse;
        private Dictionary<CMouseButton, InputEvent> mouseToEvents;


        public MouseKeyboardInput()
        {
            mouse = new CMouse();
            keyboard = new CKeyboard();
            InitializeDefaultMapping();
            SubscribeToMouseAndKeyboardEvents();
        }


        public void InitializeDefaultMapping()
        {
            eventsToKeys = new Dictionary<InputEvent, List<Keys>>();
            ////////////////////////////////////////////////////////////////////////////////////////////
            // this can be made to accept a SettingsFile
            eventsToKeys.Add(InputEvent.JUMP, ListUtils.ToList<Keys>(Keys.Space, Keys.NumPad0));
            eventsToKeys.Add(InputEvent.ATTACKLIGHT, ListUtils.ToList<Keys>(Keys.J, Keys.V));
            eventsToKeys.Add(InputEvent.ATTACKHEAVY, ListUtils.ToList<Keys>(Keys.K, Keys.C));
            eventsToKeys.Add(InputEvent.BLOCK, ListUtils.ToList<Keys>(Keys.L, Keys.X));
            eventsToKeys.Add(InputEvent.MODMAGIC, ListUtils.ToList<Keys>(Keys.Q, Keys.D1, Keys.NumPad1));
            eventsToKeys.Add(InputEvent.MODRANGE, ListUtils.ToList<Keys>(Keys.E, Keys.D3, Keys.NumPad3));
            eventsToKeys.Add(InputEvent.PAUSE, ListUtils.ToList<Keys>(Keys.Escape, Keys.Home));
            eventsToKeys.Add(InputEvent.MAP, ListUtils.ToList<Keys>(Keys.M, Keys.End));
            //
            eventsToKeys.Add(InputEvent.MOVE, ListUtils.ToList<Keys>(Keys.Up, Keys.W, Keys.Left, Keys.A, Keys.Down, Keys.S, Keys.Right, Keys.D));
            //
            ////////////////////////////////////////////////////////////////////////////////////////////

            // parse the events to keys dictionary into a keys to events dictionary
            keysToEvents = new Dictionary<Keys, List<InputEvent>>();
            foreach (InputEvent e in eventsToKeys.Keys)
            {
                List<Keys> keys = eventsToKeys[e];
                foreach (Keys key in keys)
                {
                    if (keysToEvents.ContainsKey(key))
                    {
                        keysToEvents[key].Add(e);
                    }
                    else
                    {
                        keysToEvents[key] = ListUtils.ToList<InputEvent>(e);
                    }
                }
            }

            // for getting move stats
            moveToKeys = new Dictionary<string, List<Keys>>();
            moveToKeys.Add("up", ListUtils.ToList<Keys>(Keys.Up, Keys.W));
            moveToKeys.Add("left", ListUtils.ToList<Keys>(Keys.Left, Keys.A));
            moveToKeys.Add("down", ListUtils.ToList<Keys>(Keys.Down, Keys.S));
            moveToKeys.Add("right", ListUtils.ToList<Keys>(Keys.Right, Keys.D));


            eventsToMouse = new Dictionary<InputEvent,CMouseButton>();
            ////////////////////////////////////////////////////////////////////////////////////////////
            // this can be made to accept a SettingsFile
            eventsToMouse.Add(InputEvent.ATTACKLIGHT, CMouseButton.LEFT);
            eventsToMouse.Add(InputEvent.ATTACKHEAVY, CMouseButton.RIGHT);
            eventsToMouse.Add(InputEvent.BLOCK, CMouseButton.MIDDLE);
            //
            ////////////////////////////////////////////////////////////////////////////////////////////

            // parse the events to mouse dictionary into a mouse to events dictionary
            mouseToEvents = new Dictionary<CMouseButton, InputEvent>();
            foreach (InputEvent e in eventsToMouse.Keys)
            {
                mouseToEvents[eventsToMouse[e]] = e;
            }
        }


        private void SubscribeToMouseAndKeyboardEvents()
        {
            foreach (Keys key in keysToEvents.Keys)
            {
                keyboard.KeyNotifier.SubscribeToEvent(this, key, ButtonState.Pressed);
                keyboard.KeyNotifier.SubscribeToEvent(this, key, ButtonState.Released);
            }

            mouse.ClickNotifier.SubscribeToEvent(this, CMouseButton.LEFT, ButtonState.Pressed);
            mouse.ClickNotifier.SubscribeToEvent(this, CMouseButton.LEFT, ButtonState.Released);
            mouse.ClickNotifier.SubscribeToEvent(this, CMouseButton.RIGHT, ButtonState.Pressed);
            mouse.ClickNotifier.SubscribeToEvent(this, CMouseButton.RIGHT, ButtonState.Released);
        }


        public override void Update()
        {
            mouse.Update();
            keyboard.Update();
        }


        public override Vector2 GetMoveStats()
        {
            Vector2 moveStats = Vector2.Zero;
            foreach (Keys key in moveToKeys["up"])
            {
                if (keyboard.IsKeyBeingPressed(key))
                {
                    moveStats.Y = -1;
                }
            }
            foreach (Keys key in moveToKeys["down"])
            {
                if (keyboard.IsKeyBeingPressed(key))
                {
                    moveStats.Y = 1;
                }
            }
            foreach (Keys key in moveToKeys["left"])
            {
                if (keyboard.IsKeyBeingPressed(key))
                {
                    moveStats.X = -1;
                }
            }
            foreach (Keys key in moveToKeys["right"])
            {
                if (keyboard.IsKeyBeingPressed(key))
                {
                    moveStats.X = 1;
                }
            }
            return moveStats;
        }


        public override bool IsDown(InputEvent e)
        {
            bool foundDownKey = false;
            if (eventsToKeys.ContainsKey(e))
            {
                List<Keys> keysToCheck = eventsToKeys[e];
                foreach (Keys key in keysToCheck)
                {
                    if (keyboard.IsKeyBeingPressed(key))
                    {
                        foundDownKey = true;
                        break;
                    }
                }
            }
            if (!foundDownKey && eventsToMouse.ContainsKey(e))
            {
                CMouseButton buttonToCheck = eventsToMouse[e];
                if (mouse.Held(buttonToCheck))
                {
                    foundDownKey = true;
                }
            }
            return foundDownKey;
        }


        public override bool IsUp(InputEvent e)
        {
            if (eventsToKeys.ContainsKey(e))
            {
                return !IsDown(e);
            }
            return false;
        }


        public override bool IsChanged(InputEvent e)
        {
            bool foundChange = false;
            if (eventsToKeys.ContainsKey(e))
            {
                List<Keys> keysToCheck = eventsToKeys[e];
                foreach (Keys key in keysToCheck)
                {
                    if (keyboard.IsKeyJustPressed(key) || keyboard.IsKeyJustReleased(key))
                    {
                        foundChange = true;
                        break;
                    }
                }
            }
            return foundChange;
        }


        #region CMouseEventHandler Members
        void CMouseEventHandler.HandleClickEvent(CMouse mouse, CMouseButton button, ButtonState state)
        {
            InputEventType type = InputEventType.UP;
            if (state == ButtonState.Pressed)
            {
                type = InputEventType.DOWN;
            }

            NotifySubscribers(mouseToEvents[button], type);
        }


        void CMouseEventHandler.HandleScrollEvent(CMouse mouse, float difference)
        {
            throw new System.NotImplementedException();
        }


        void CMouseEventHandler.HandleMoveEvent(CMouse mouse, Vector2 position)
        {
            throw new System.NotImplementedException();
        }


        void CMouseEventHandler.HandleDragEvent(CMouse mouse, CMouseButton button, CMouseDrag state)
        {
            throw new System.NotImplementedException();
        }
        #endregion


        #region CKeyboardEventHandler Members
        void CKeyboardEventHandler.HandleKeyEvent(CKeyboard keyboard, Keys key, ButtonState state)
        {
            InputEventType type = InputEventType.UP;
            if (state == ButtonState.Pressed)
            {
                type = InputEventType.DOWN;
            }

            List<InputEvent> eventsToFire = keysToEvents[key];
            foreach (InputEvent e in eventsToFire)
            {
                NotifySubscribers(e, type);
            }
        }
        #endregion
    }
}
