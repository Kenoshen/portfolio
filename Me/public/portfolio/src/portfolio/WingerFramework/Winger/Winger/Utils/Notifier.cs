using System.Collections.Generic;

namespace Winger.Utils
{
    /// <summary>
    /// This class is part of the Observer Pattern, it will notify subscribers of events
    /// </summary>
    /// <typeparam name="HANDLER">the handler object (usually an interface event handler)</typeparam>
    /// <typeparam name="TYPE">the type of the event (an example is a Key on a keyboard)</typeparam>
    /// <typeparam name="VALUE">the value of the event (an example is a Key is pressed or released)</typeparam>
    public class Notifier<HANDLER, TYPE, VALUE>
    {
        private List<SubscriptionRecord<HANDLER, TYPE, VALUE>> subscribers = new List<SubscriptionRecord<HANDLER, TYPE, VALUE>>();

        /// <summary>
        /// Subscribes an event handler to a given type and value
        /// </summary>
        /// <param name="handler">the subscriber to the event</param>
        /// <param name="type">the type of event</param>
        /// <param name="val">the value of the event</param>
        public void SubscribeToEvent(HANDLER handler, TYPE type, VALUE val)
        {
            subscribers.Add(new SubscriptionRecord<HANDLER, TYPE, VALUE>(handler, type, val));
        }


        /// <summary>
        /// Unsubscribes an event handler from a given type and value
        /// </summary>
        /// <param name="handler">the subscriber to the event</param>
        /// <param name="type">the type of event</param>
        /// <param name="val">the value of the event</param>
        public void UnsubscribeFromEvent(HANDLER handler, TYPE type, VALUE val)
        {
            subscribers.Remove(new SubscriptionRecord<HANDLER, TYPE, VALUE>(handler, type, val));
        }


        /// <summary>
        /// Gets the SubscriptionRecords of the subscribers who qualify for a given event of type and value
        /// </summary>
        /// <param name="type">the type of event</param>
        /// <param name="val">the value of the event</param>
        /// <returns>list of subscribers who qualify for the event</returns>
        public List<SubscriptionRecord<HANDLER, TYPE, VALUE>> GetSubscribersToNotify(TYPE type, VALUE val)
        {
            List<SubscriptionRecord<HANDLER, TYPE, VALUE>> subsToNotify = new List<SubscriptionRecord<HANDLER, TYPE, VALUE>>();
            foreach (SubscriptionRecord<HANDLER, TYPE, VALUE> subscriber in subscribers)
            {
                if (subscriber.IsQualified(type, val))
                {
                    subsToNotify.Add(subscriber);
                }
            }
            return subsToNotify;
        }


        /// <summary>
        /// Checks if there are any subscribers
        /// </summary>
        /// <returns>returns true if there are no subscribers</returns>
        public bool IsEmpty()
        {
            return (subscribers.Count == 0);
        }
    }


    /// <summary>
    /// This class is part of the Observer Pattern, it will notify subscribers of events regardless of the state of the event
    /// </summary>
    /// <typeparam name="HANDLER">the handler object (usually an interface event handler)</typeparam>
    /// <typeparam name="TYPE">the type of the event (an example is a Key on a keyboard)</typeparam>
    public class Notifier<HANDLER, TYPE>
    {
        private List<SubscriptionRecord<HANDLER, TYPE>> subscribers = new List<SubscriptionRecord<HANDLER, TYPE>>();

        /// <summary>
        /// Subscribes an event handler to a given type regardless of state
        /// </summary>
        /// <param name="handler">the subscriber to the event</param>
        /// <param name="type">the type of event</param>
        public void SubscribeToEvent(HANDLER handler, TYPE type)
        {
            subscribers.Add(new SubscriptionRecord<HANDLER, TYPE>(handler, type));
        }


        /// <summary>
        /// Unsubscribes an event handler from a given type regardless of state
        /// </summary>
        /// <param name="handler">the subscriber to the event</param>
        /// <param name="type">the type of event</param>
        public void UnsubscribeFromEvent(HANDLER handler, TYPE type)
        {
            subscribers.Remove(new SubscriptionRecord<HANDLER, TYPE>(handler, type));
        }


        /// <summary>
        /// Gets the SubscriptionRecords of the subscribers who qualify for a given event of type regardless of state
        /// </summary>
        /// <param name="type">the type of event</param>
        /// <param name="val">the value of the event</param>
        /// <returns>list of subscribers who qualify for the event</returns>
        public List<SubscriptionRecord<HANDLER, TYPE>> GetSubscribersToNotify(TYPE type)
        {
            List<SubscriptionRecord<HANDLER, TYPE>> subsToNotify = new List<SubscriptionRecord<HANDLER, TYPE>>();
            foreach (SubscriptionRecord<HANDLER, TYPE> subscriber in subscribers)
            {
                if (subscriber.IsQualified(type))
                {
                    subsToNotify.Add(subscriber);
                }
            }
            return subsToNotify;
        }


        /// <summary>
        /// Checks if there are any subscribers
        /// </summary>
        /// <returns>returns true if there are no subscribers</returns>
        public bool IsEmpty()
        {
            return (subscribers.Count == 0);
        }
    }
}
