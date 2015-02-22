using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Winger.Utils;

namespace Winger.Input.Raw
{
    /// <summary>
    /// A custom wrapper class for the GamePad class
    /// </summary>
    public class CGamePad
    {
        private static IEnumerable<CGamePadButton> BUTTONS = EnumUtils.GetValues<CGamePadButton>();

        public GamePadState State { get; private set; }
        public PlayerIndex PlayerNum { get; private set; }

        public Notifier<CGamePadEventHandler, CGamePadButton, ButtonState> ButtonNotifier { get; private set; }
        public Notifier<CGamePadEventHandler, CGamePadTrigger> TriggerNotifier { get; private set; }
        public Notifier<CGamePadEventHandler, CGamePadStick> ThumbStickNotifier { get; private set; }

        private Dictionary<CGamePadButton, ButtonState> lbs = null; // last button state
        private Dictionary<CGamePadButton, ButtonState> cbs = new Dictionary<CGamePadButton,ButtonState>(); // current button state
        private float llts = 0; // last left trigger state
        private float clts = 0; // current left trigger state
        private float lrts = 0; // last right trigger state
        private float crts = 0; // current right trigger state
        private Vector2 llss = Vector2.Zero; // last left stick state
        private Vector2 clss = Vector2.Zero; // current left stick state
        private Vector2 lrss = Vector2.Zero; // last right stick state
        private Vector2 crss = Vector2.Zero; // current right stick state


        /// <summary>
        /// A wrapper for the GamePad class, defaults to PlayerIndex.One
        /// </summary>
        public CGamePad()
        {
            Initialize(PlayerIndex.One);
        }


        /// <summary>
        /// A wrapper for the GamePad class
        /// </summary>
        /// <param name="playerIndex">the player index of the game pad</param>
        public CGamePad(PlayerIndex playerIndex)
        {
            Initialize(playerIndex);
        }


        /// <summary>
        /// Inizializes the CGamePad
        /// </summary>
        /// <param name="playerIndex">the player index of the game pad</param>
        private void Initialize(PlayerIndex playerIndex)
        {
            PlayerNum = playerIndex;
            State = GamePad.GetState(PlayerNum);
            ButtonNotifier = new Notifier<CGamePadEventHandler, CGamePadButton, ButtonState>();
            TriggerNotifier = new Notifier<CGamePadEventHandler, CGamePadTrigger>();
            ThumbStickNotifier = new Notifier<CGamePadEventHandler, CGamePadStick>();
            foreach (CGamePadButton button in BUTTONS)
            {
                cbs[button] = ButtonState.Released;
            }
            GetCurrentButtonStates();
            GetCurrentTriggerStates();
            GetCurrentStickStates();
        }


        /// <summary>
        /// Updates the GamePad state and sends events to event handlers if necessary
        /// </summary>
        public void Update()
        {
            State = GamePad.GetState(PlayerNum);
            if (!ButtonNotifier.IsEmpty())
            {
                GetCurrentButtonStates();
                DoButtonEvents();
            }
            if (!TriggerNotifier.IsEmpty())
            {
                GetCurrentTriggerStates();
                DoTriggerEvents();
            }
            if (!ThumbStickNotifier.IsEmpty())
            {
                GetCurrentStickStates();
                DoStickEvents();
            }
        }


        #region State checks
        /// <summary>
        /// Checks if the current state of the button is pressed
        /// </summary>
        /// <param name="button">the button to check</param>
        /// <returns>true if the button is preased</returns>
        public bool IsDown(CGamePadButton button)
        {
            return (cbs[button] == ButtonState.Pressed);
        }


        /// <summary>
        /// Checks if the current state of the button is released
        /// </summary>
        /// <param name="button">the button to check</param>
        /// <returns>true if the button is released</returns>
        public bool IsUp(CGamePadButton button)
        {
            return !IsDown(button);
        }


        /// <summary>
        /// Checks if the current state of the button is different then the last state
        /// </summary>
        /// <param name="button">the button to check</param>
        /// <returns>true if the current button state is different then the last state</returns>
        public bool IsChanged(CGamePadButton button)
        {
            return (cbs[button] != lbs[button]);
        }


        /// <summary>
        /// Checks if the current trigger state is different then the last trigger state
        /// </summary>
        /// <param name="trigger">the trigger to check</param>
        /// <returns>true if the trigger changed</returns>
        public bool IsChanged(CGamePadTrigger trigger)
        {
            if (trigger == CGamePadTrigger.LEFT)
            {
                return (clts != llts);
            }
            else if (trigger == CGamePadTrigger.RIGHT)
            {
                return (crts != lrts);
            }
            return false;
        }


        /// <summary>
        /// Checks if the current stick state is different then the last stick state
        /// </summary>
        /// <param name="stick">the stick to check</param>
        /// <returns>true if the stick changed</returns>
        public bool IsChanged(CGamePadStick stick)
        {
            if (stick == CGamePadStick.LEFT)
            {
                return (clss != llss);
            }
            else if (stick == CGamePadStick.RIGHT)
            {
                return (crss != lrss);
            }
            return false;
        }
        #endregion


        #region Buttons
        private void GetCurrentButtonStates()
        {
            lbs = new Dictionary<CGamePadButton, ButtonState>(cbs);
            foreach (CGamePadButton button in BUTTONS)
            {
                cbs[button] = GetButtonStateForCGamePadButton(button);
            }
        }


        private ButtonState GetButtonStateForCGamePadButton(CGamePadButton button)
        {
            switch (button)
            {
                case CGamePadButton.X:
                    return State.Buttons.X;

                case CGamePadButton.Y:
                    return State.Buttons.Y;

                case CGamePadButton.A:
                    return State.Buttons.A;

                case CGamePadButton.B:
                    return State.Buttons.B;

                case CGamePadButton.BACK:
                    return State.Buttons.Back;

                case CGamePadButton.START:
                    return State.Buttons.Start;

                case CGamePadButton.XBOX:
                    return State.Buttons.BigButton;

                case CGamePadButton.LB:
                    return State.Buttons.LeftShoulder;

                case CGamePadButton.RB:
                    return State.Buttons.RightShoulder;

                case CGamePadButton.LSTICK:
                    return State.Buttons.LeftStick;

                case CGamePadButton.RSTICK:
                    return State.Buttons.RightStick;

                case CGamePadButton.DPADL:
                    return State.DPad.Left;

                case CGamePadButton.DPADR:
                    return State.DPad.Right;

                case CGamePadButton.DPADU:
                    return State.DPad.Up;

                case CGamePadButton.DPADD:
                    return State.DPad.Down;

                case CGamePadButton.LT:
                    return GetFloatingState(State.Triggers.Left);

                case CGamePadButton.RT:
                    return GetFloatingState(State.Triggers.Right);

                case CGamePadButton.LSTICKL:
                    return GetFloatingState(-State.ThumbSticks.Left.X);

                case CGamePadButton.LSTICKR:
                    return GetFloatingState(State.ThumbSticks.Left.X);

                case CGamePadButton.LSTICKU:
                    return GetFloatingState(-State.ThumbSticks.Left.Y);

                case CGamePadButton.LSTICKD:
                    return GetFloatingState(State.ThumbSticks.Left.Y);

                case CGamePadButton.RSTICKL:
                    return GetFloatingState(-State.ThumbSticks.Right.X);

                case CGamePadButton.RSTICKR:
                    return GetFloatingState(State.ThumbSticks.Right.X);

                case CGamePadButton.RSTICKU:
                    return GetFloatingState(-State.ThumbSticks.Right.Y);

                case CGamePadButton.RSTICKD:
                    return GetFloatingState(State.ThumbSticks.Right.Y);

                default:
                    return ButtonState.Released;
            }
        }


        private ButtonState GetFloatingState(float f)
        {
            if (f > 0.5f)
                return ButtonState.Pressed;
            return ButtonState.Released;
        }


        private void DoButtonEvents()
        {
            foreach (CGamePadButton button in cbs.Keys)
            {
                if (cbs[button] != lbs[button])
                {
                    NotifyButtonEventHandlers(button, cbs[button]);
                }
            }
        }


        /// <summary>
        /// Sends an event to the button event handlers
        /// </summary>
        /// <param name="button">the button</param>
        /// <param name="state">the state of the button</param>
        public void NotifyButtonEventHandlers(CGamePadButton button, ButtonState state)
        {
            List<SubscriptionRecord<CGamePadEventHandler, CGamePadButton, ButtonState>> subsToNotify = ButtonNotifier.GetSubscribersToNotify(button, state);
            foreach (SubscriptionRecord<CGamePadEventHandler, CGamePadButton, ButtonState> record in subsToNotify)
            {
                record.Handler.HandleButtonEvent(this, button, state);
            }
        }
        #endregion


        #region Triggers
        private void GetCurrentTriggerStates()
        {
            llts = clts;
            clts = State.Triggers.Left;
            lrts = crts;
            crts = State.Triggers.Right;
        }


        private void DoTriggerEvents()
        {
            if (clts != llts)
            {
                NotifyTriggerEventHandlers(CGamePadTrigger.LEFT, clts);
            }

            if (crts != lrts)
            {
                NotifyTriggerEventHandlers(CGamePadTrigger.RIGHT, crts);
            }
        }


        /// <summary>
        /// Sends an event to the trigger event handlers
        /// </summary>
        /// <param name="trigger">the trigger</param>
        /// <param name="state">the state of the trigger</param>
        public void NotifyTriggerEventHandlers(CGamePadTrigger trigger, float state)
        {
            List<SubscriptionRecord<CGamePadEventHandler, CGamePadTrigger>> subsToNotify = TriggerNotifier.GetSubscribersToNotify(trigger);
            foreach (SubscriptionRecord<CGamePadEventHandler, CGamePadTrigger> record in subsToNotify)
            {
                record.Handler.HandleTriggerEvent(this, trigger, state);
            }
        }
        #endregion


        #region ThumbSticks
        private void GetCurrentStickStates()
        {
            llss = clss;
            clss = State.ThumbSticks.Left;
            lrss = crss;
            crss = State.ThumbSticks.Right;
        }


        private void DoStickEvents()
        {
            if (clss != llss)
            {
                NotifyStickEventHandlers(CGamePadStick.LEFT, clss);
            }

            if (crss != lrss)
            {
                NotifyStickEventHandlers(CGamePadStick.RIGHT, crss);
            }
        }


        /// <summary>
        /// Sends an event to the stick event handlers
        /// </summary>
        /// <param name="stick">the stick</param>
        /// <param name="state">the state of the stick</param>
        public void NotifyStickEventHandlers(CGamePadStick stick, Vector2 state)
        {
            List<SubscriptionRecord<CGamePadEventHandler, CGamePadStick>> subsToNotify = ThumbStickNotifier.GetSubscribersToNotify(stick);
            foreach (SubscriptionRecord<CGamePadEventHandler, CGamePadStick> record in subsToNotify)
            {
                record.Handler.HandleStickEvent(this, stick, state);
            }
        }
        #endregion
    }

    public interface CGamePadEventHandler
    {
        void HandleButtonEvent(CGamePad gamePad, CGamePadButton button, ButtonState state);
        void HandleTriggerEvent(CGamePad gamePad, CGamePadTrigger trigger, float state);
        void HandleStickEvent(CGamePad gamePad, CGamePadStick stick, Vector2 state);
    }

    public enum CGamePadTrigger
    {
        RIGHT,
        LEFT,
    }

    public enum CGamePadStick
    {
        RIGHT,
        LEFT,
    }

    public enum CGamePadButton
    {
        X,
        Y,
        A,
        B,
        BACK,
        START,
        XBOX,
        RB,
        LB,
        RT,
        LT,
        DPADL,
        DPADR,
        DPADU,
        DPADD,
        LSTICK,
        LSTICKL,
        LSTICKR,
        LSTICKU,
        LSTICKD,
        RSTICK,
        RSTICKL,
        RSTICKR,
        RSTICKU,
        RSTICKD,
    }
}
