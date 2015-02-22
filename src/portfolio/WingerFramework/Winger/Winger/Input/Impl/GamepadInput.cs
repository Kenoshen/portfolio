using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Winger.Input.Event;
using Winger.Input.Raw;
using Winger.Utils;

namespace Winger.Input.Impl
{
    public class GamePadInput : UserInput, CGamePadEventHandler
    {
        private CGamePad gamePad;
        private Dictionary<InputEvent, List<CGamePadButton>> eventsToButtons;
        private Dictionary<CGamePadButton, List<InputEvent>> buttonsToEvents;
        

        public GamePadInput(PlayerIndex playerIndex)
        {
            gamePad = new CGamePad(playerIndex);
            InitializeDefaultMapping();
            SubscribeToGamePadEvents();
        }


        public void InitializeDefaultMapping()
        {
            eventsToButtons = new Dictionary<InputEvent, List<CGamePadButton>>();
            ////////////////////////////////////////////////////////////////////////////////////////////
            // this can be made to accept a SettingsFile
            eventsToButtons.Add(InputEvent.JUMP, ListUtils.ToList<CGamePadButton>(CGamePadButton.A));
            eventsToButtons.Add(InputEvent.ATTACKLIGHT, ListUtils.ToList<CGamePadButton>(CGamePadButton.X));
            eventsToButtons.Add(InputEvent.ATTACKHEAVY, ListUtils.ToList<CGamePadButton>(CGamePadButton.Y));
            eventsToButtons.Add(InputEvent.BLOCK, ListUtils.ToList<CGamePadButton>(CGamePadButton.B));
            eventsToButtons.Add(InputEvent.MODMAGIC, ListUtils.ToList<CGamePadButton>(CGamePadButton.LT));
            eventsToButtons.Add(InputEvent.MODRANGE, ListUtils.ToList<CGamePadButton>(CGamePadButton.RT));
            eventsToButtons.Add(InputEvent.PAUSE, ListUtils.ToList<CGamePadButton>(CGamePadButton.START));
            eventsToButtons.Add(InputEvent.MAP, ListUtils.ToList<CGamePadButton>(CGamePadButton.BACK));
            //
            ////////////////////////////////////////////////////////////////////////////////////////////

            // parse the events to buttons dictionary into a buttons to events dictionary
            buttonsToEvents = new Dictionary<CGamePadButton, List<InputEvent>>();
            foreach (InputEvent e in eventsToButtons.Keys)
            {
                List<CGamePadButton> buttons = eventsToButtons[e];
                foreach (CGamePadButton button in buttons)
                {
                    if (buttonsToEvents.ContainsKey(button))
                    {
                        buttonsToEvents[button].Add(e);
                    }
                    else
                    {
                        buttonsToEvents[button] = ListUtils.ToList<InputEvent>(e);
                    }
                }
            }
        }


        private void SubscribeToGamePadEvents()
        {
            foreach (CGamePadButton button in buttonsToEvents.Keys)
            {
                gamePad.ButtonNotifier.SubscribeToEvent(this, button, ButtonState.Pressed);
                gamePad.ButtonNotifier.SubscribeToEvent(this, button, ButtonState.Released);
            }

            //gamePad.ThumbStickNotifier.SubscribeToEvent(this, CGamePadStick.LEFT);
            //gamePad.ThumbStickNotifier.SubscribeToEvent(this, CGamePadStick.RIGHT);

            //gamePad.TriggerNotifier.SubscribeToEvent(this, CGamePadTrigger.LEFT);
            //gamePad.TriggerNotifier.SubscribeToEvent(this, CGamePadTrigger.RIGHT);
        }


        public override void Update()
        {
            gamePad.Update();
        }

        public override Vector2 GetMoveStats()
        {
            return gamePad.State.ThumbSticks.Left;
        }

        public override bool IsDown(InputEvent e)
        {
            bool foundDownButton = false;
            if (eventsToButtons.ContainsKey(e))
            {
                List<CGamePadButton> buttonsToCheck = eventsToButtons[e];
                foreach (CGamePadButton button in buttonsToCheck)
                {
                    if (gamePad.IsDown(button))
                    {
                        foundDownButton = true;
                        break;
                    }
                }
            }
            return foundDownButton;
        }

        public override bool IsUp(InputEvent e)
        {
            if (eventsToButtons.ContainsKey(e))
            {
                return !IsDown(e);
            }
            return false;
        }

        public override bool IsChanged(InputEvent e)
        {
            bool foundChange = false;
            if (eventsToButtons.ContainsKey(e))
            {
                List<CGamePadButton> buttonsToCheck = eventsToButtons[e];
                foreach (CGamePadButton button in buttonsToCheck)
                {
                    if (gamePad.IsChanged(button))
                    {
                        foundChange = true;
                        break;
                    }
                }
            }
            else
            {
                if (e == InputEvent.MOVE)
                {
                    foundChange = gamePad.IsChanged(CGamePadStick.LEFT);
                }
            }
            return foundChange;
        }

        void CGamePadEventHandler.HandleButtonEvent(CGamePad gamePad, CGamePadButton button, ButtonState state)
        {
            InputEventType type = InputEventType.UP;
            if (state == ButtonState.Pressed)
            {
                type = InputEventType.DOWN;
            }

            List<InputEvent> eventsToFire = buttonsToEvents[button];
            foreach (InputEvent e in eventsToFire)
            {
                NotifySubscribers(e, type);
            }
        }

        void CGamePadEventHandler.HandleTriggerEvent(CGamePad gamePad, CGamePadTrigger trigger, float state)
        {
            throw new System.NotImplementedException();
        }

        void CGamePadEventHandler.HandleStickEvent(CGamePad gamePad, CGamePadStick stick, Vector2 state)
        {
            throw new System.NotImplementedException();
        }
    }
}
