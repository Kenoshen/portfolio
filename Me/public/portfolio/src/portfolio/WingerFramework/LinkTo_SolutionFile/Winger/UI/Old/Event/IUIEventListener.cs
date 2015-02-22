using Winger.UI.UI;

namespace Winger.UI.Event
{
    public interface IUIEventListener
    {
        /// <summary>
        /// This event is called when the listener has subscribed to a named event
        /// </summary>
        /// <param name="sender">the object that sent the event</param>
        /// <param name="name">the name of the event</param>
        void NamedUIEvent(UIObject sender, string name);
        /// <summary>
        /// This event is called when the listener has subscribed to HoverStartUIEvent of the sender object
        /// </summary>
        /// <param name="sender">the object that sent the event</param>
        void HoverStartUIEvent(UIObject sender);
        /// <summary>
        /// This event is called when the listener has subscribed to HoverEndUIEvent of the sender object
        /// </summary>
        /// <param name="sender">the object that sent the event</param>
        void HoverEndUIEvent(UIObject sender);
        /// <summary>
        /// This event is called when the listener has subscribed to SelectStartUIEvent of the sender object
        /// </summary>
        /// <param name="sender">the object that sent the event</param>
        void SelectStartUIEvent(UIObject sender);
        /// <summary>
        /// This event is called when the listener has subscribed to SelectEndUIEvent of the sender object
        /// </summary>
        /// <param name="sender">the object that sent the event</param>
        void SelectEndUIEvent(UIObject sender);
        /// <summary>
        /// This event is called when the listener has subscribed to CustomUIEvent of the sender object
        /// </summary>
        /// <param name="sender">the object that sent the event</param>
        void CustomUIEvent(UIObject sender);
    }
}
