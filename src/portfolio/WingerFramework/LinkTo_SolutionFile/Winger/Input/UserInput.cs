using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Winger.Input.Event;
using Winger.Utils;

namespace Winger.Input
{
    /// <summary>
    /// A class to abstract the implementation of different forms of user input
    /// </summary>
    public abstract class UserInput : Notifier<InputEventHandler, InputEvent, InputEventType>
    {
        protected UserInput()
        {
        }


        /// <summary>
        /// Notifies all qualifying event handlers that a specific event has occured
        /// </summary>
        /// <param name="e">the event</param>
        /// <param name="type">the type of the event</param>
        public void NotifySubscribers(InputEvent e, InputEventType type)
        {
            List<SubscriptionRecord<InputEventHandler, InputEvent, InputEventType>> subsToNotify = this.GetSubscribersToNotify(e, type);
            foreach (SubscriptionRecord<InputEventHandler, InputEvent, InputEventType> subscriber in subsToNotify)
            {
                subscriber.Handler.HandleEvent(this, e, type);
            }
        }


        public abstract void Update();


        public abstract Vector2 GetMoveStats();


        public abstract bool IsDown(InputEvent e);


        public abstract bool IsUp(InputEvent e);


        public abstract bool IsChanged(InputEvent e);
    }
}
