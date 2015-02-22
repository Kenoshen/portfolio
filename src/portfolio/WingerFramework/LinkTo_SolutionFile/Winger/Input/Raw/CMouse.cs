using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Winger.Utils;

namespace Winger.Input.Raw
{
    public class CMouse
    {
        public MouseState State { get; private set; }
        private MouseState LastState { get; set; }

        public Notifier<CMouseEventHandler, CMouseButton, ButtonState> ClickNotifier { get; private set; }
        public Notifier<CMouseEventHandler, CMouseScroll> ScrollNotifier { get; private set; }
        public Notifier<CMouseEventHandler, CMousePosition> MoveNotifier { get; private set; }
        public Notifier<CMouseEventHandler, CMouseButton, CMouseDrag> DragNotifier { get; private set; }

        private static IEnumerable<CMouseButton> buttonsEnum = EnumUtils.GetValues<CMouseButton>();
        private Dictionary<CMouseButton, Vector2> downPos = new Dictionary<CMouseButton, Vector2>();
        private Dictionary<CMouseButton, Vector2> upPos = new Dictionary<CMouseButton, Vector2>();
        private Dictionary<CMouseButton, bool> heldNotDragged = new Dictionary<CMouseButton, bool>();
        private Dictionary<CMouseButton, bool> dragged = new Dictionary<CMouseButton, bool>();

        private float dragBreakDist = 5;

        #region Properties
        public Vector2 Position
        {
            get
            {
                return new Vector2(State.X, State.Y);
            }

            set
            {
                Mouse.SetPosition((int)value.X, (int)value.Y);
            }
        }

        public int X
        {
            get { return State.X; }
            set { Mouse.SetPosition(value, State.Y); }
        }

        public int Y
        {
            get { return State.Y; }
            set { Mouse.SetPosition(State.X, value); }
        }
        #endregion


        public CMouse()
        {
            State = Mouse.GetState();

            ClickNotifier = new Notifier<CMouseEventHandler, CMouseButton, ButtonState>();
            ScrollNotifier = new Notifier<CMouseEventHandler, CMouseScroll>();
            MoveNotifier = new Notifier<CMouseEventHandler, CMousePosition>();
            DragNotifier = new Notifier<CMouseEventHandler, CMouseButton, CMouseDrag>();

            downPos[CMouseButton.LEFT] = Vector2.Zero;
            downPos[CMouseButton.RIGHT] = Vector2.Zero;
            downPos[CMouseButton.MIDDLE] = Vector2.Zero;
            upPos[CMouseButton.LEFT] = Vector2.Zero;
            upPos[CMouseButton.RIGHT] = Vector2.Zero;
            upPos[CMouseButton.MIDDLE] = Vector2.Zero;
            heldNotDragged[CMouseButton.LEFT] = false;
            heldNotDragged[CMouseButton.RIGHT] = false;
            heldNotDragged[CMouseButton.MIDDLE] = false;
            dragged[CMouseButton.LEFT] = false;
            dragged[CMouseButton.RIGHT] = false;
            dragged[CMouseButton.MIDDLE] = false;
        }


        public void Update()
        {
            LastState = State;
            State = Mouse.GetState();

            if (!ClickNotifier.IsEmpty())
            {
                CheckClickStatesAndDoEvents();
            }
            if (!ScrollNotifier.IsEmpty())
            {
                CheckScrollStateAndDoEvent();
            }
            if (!MoveNotifier.IsEmpty())
            {
                CheckMoveStateAndDoEvent();
            }

            Vector2 pos = Position;
            foreach (CMouseButton button in buttonsEnum)
            {
                if (JustPressed(button))
                {
                    downPos[button] = pos;
                    heldNotDragged[button] = true;
                }
                if (Held(button))
                {
                    if (heldNotDragged[button])
                    {
                        if (Vector2.Distance(pos, downPos[button]) < dragBreakDist)
                        {
                            // still holding but not moved past the drag break dist
                        }
                        else
                        {
                            heldNotDragged[button] = false;
                            dragged[button] = true;
                            // drag begin event
                            NotifyDragEventHandlers(button, CMouseDrag.BEGIN);
                        }
                    }
                }
                if (JustReleased(button))
                {
                    upPos[button] = pos;
                    heldNotDragged[button] = false;
                    if (dragged[button])
                    {
                        dragged[button] = false;
                        // drag end event
                        NotifyDragEventHandlers(button, CMouseDrag.END);
                    }
                }
            }
        }


        #region State checks
        public bool JustPressed(CMouseButton button)
        {
            switch (button)
            {
                case CMouseButton.LEFT:
                    return (State.LeftButton == ButtonState.Pressed &&
                        LastState.LeftButton == ButtonState.Released);

                case CMouseButton.RIGHT:
                    return (State.RightButton == ButtonState.Pressed &&
                        LastState.RightButton == ButtonState.Released);

                case CMouseButton.MIDDLE:
                    return (State.MiddleButton == ButtonState.Pressed &&
                        LastState.MiddleButton == ButtonState.Released);
            }
            return false;
        }


        public bool JustReleased(CMouseButton button)
        {
            switch (button)
            {
                case CMouseButton.LEFT:
                    return (State.LeftButton == ButtonState.Released &&
                        LastState.LeftButton == ButtonState.Pressed);

                case CMouseButton.RIGHT:
                    return (State.RightButton == ButtonState.Released &&
                        LastState.RightButton == ButtonState.Pressed);

                case CMouseButton.MIDDLE:
                    return (State.MiddleButton == ButtonState.Released &&
                        LastState.MiddleButton == ButtonState.Pressed);
            }
            return false;
        }


        public bool Held(CMouseButton button)
        {
            switch (button)
            {
                case CMouseButton.LEFT:
                    return (State.LeftButton == ButtonState.Pressed &&
                        LastState.LeftButton == ButtonState.Pressed);

                case CMouseButton.RIGHT:
                    return (State.RightButton == ButtonState.Pressed &&
                        LastState.RightButton == ButtonState.Pressed);

                case CMouseButton.MIDDLE:
                    return (State.MiddleButton == ButtonState.Pressed &&
                        LastState.MiddleButton == ButtonState.Pressed);
            }
            return false;
        }


        public float GetScrollDifference()
        {
            return State.ScrollWheelValue - LastState.ScrollWheelValue;
        }


        public Vector2 GetPositionDifference()
        {
            return new Vector2(State.X - LastState.X, State.Y - LastState.Y);
        }


        private void CheckClickStatesAndDoEvents()
        {
            if (LastState.LeftButton != State.LeftButton)
            {
                NotifyClickEventHandlers(CMouseButton.LEFT, State.LeftButton);
            }
            if (LastState.RightButton != State.RightButton)
            {
                NotifyClickEventHandlers(CMouseButton.RIGHT, State.RightButton);
            }
            if (LastState.MiddleButton != State.MiddleButton)
            {
                NotifyClickEventHandlers(CMouseButton.MIDDLE, State.MiddleButton);
            }
        }


        private void CheckScrollStateAndDoEvent()
        {
            if (LastState.ScrollWheelValue != State.ScrollWheelValue)
            {
                NotifyScrollEventHandlers(CMouseScroll.CHANGED);
            }
        }


        private void CheckMoveStateAndDoEvent()
        {
            if (LastState.X != State.X || LastState.Y != State.Y)
            {
                NotifyMoveEventHandlers(CMousePosition.CHANGED);
            }
        }
        #endregion


        #region Notifiers
        /// <summary>
        /// Sends an event to the click event handlers
        /// </summary>
        /// <param name="button">the button</param>
        /// <param name="state">the state of the button</param>
        public void NotifyClickEventHandlers(CMouseButton button, ButtonState state)
        {
            List<SubscriptionRecord<CMouseEventHandler, CMouseButton, ButtonState>> subsToNotify = ClickNotifier.GetSubscribersToNotify(button, state);
            foreach (SubscriptionRecord<CMouseEventHandler, CMouseButton, ButtonState> record in subsToNotify)
            {
                record.Handler.HandleClickEvent(this, button, state);
            }
        }


        /// <summary>
        /// Sends an event to the scroll event handlers
        /// </summary>
        /// <param name="state">the state of the scroll wheel</param>
        public void NotifyScrollEventHandlers(CMouseScroll state)
        {
            List<SubscriptionRecord<CMouseEventHandler, CMouseScroll>> subsToNotify = ScrollNotifier.GetSubscribersToNotify(state);
            float diff = GetScrollDifference();
            foreach (SubscriptionRecord<CMouseEventHandler, CMouseScroll> record in subsToNotify)
            {
                record.Handler.HandleScrollEvent(this, diff);
            }
        }


        /// <summary>
        /// Sends an event to the move event handlers
        /// </summary>
        /// <param name="state">the state of the mouse position</param>
        public void NotifyMoveEventHandlers(CMousePosition state)
        {
            List<SubscriptionRecord<CMouseEventHandler, CMousePosition>> subsToNotify = MoveNotifier.GetSubscribersToNotify(state);
            Vector2 pos = Position;
            foreach (SubscriptionRecord<CMouseEventHandler, CMousePosition> record in subsToNotify)
            {
                record.Handler.HandleMoveEvent(this, pos);
            }
        }


        /// <summary>
        /// Sends an event to the drag event handlers
        /// </summary>
        /// <param name="button">the button</param>
        /// <param name="state">the state of the drag</param>
        public void NotifyDragEventHandlers(CMouseButton button, CMouseDrag state)
        {
            List<SubscriptionRecord<CMouseEventHandler, CMouseButton, CMouseDrag>> subsToNotify = DragNotifier.GetSubscribersToNotify(button, state);
            foreach (SubscriptionRecord<CMouseEventHandler, CMouseButton, CMouseDrag> record in subsToNotify)
            {
                record.Handler.HandleDragEvent(this, button, state);
            }
        }
        #endregion
    }

    public interface CMouseEventHandler
    {
        void HandleClickEvent(CMouse mouse, CMouseButton button, ButtonState state);
        void HandleScrollEvent(CMouse mouse, float difference);
        void HandleMoveEvent(CMouse mouse, Vector2 position);
        void HandleDragEvent(CMouse mouse, CMouseButton button, CMouseDrag state);
    }


    public enum CMouseButton
    {
        LEFT,
        RIGHT,
        MIDDLE,
    }


    public enum CMouseScroll
    {
        CHANGED,
    }


    public enum CMousePosition
    {
        CHANGED,
    }


    public enum CMouseDrag
    {
        BEGIN,
        END,
    }
}
