
namespace Winger.Utils
{
    /// <summary>
    /// This class helps track subscribers when they are subscribed to Notify classes
    /// </summary>
    /// <typeparam name="H">the event handler</typeparam>
    /// <typeparam name="T">the event type</typeparam>
    /// <typeparam name="V">the value of the event</typeparam>
    public class SubscriptionRecord<H, T, V>
    {
        public H Handler { get; set; }
        public T Type { get; set; }
        public V Val { get; set; }

        public SubscriptionRecord(H handler, T type, V val)
        {
            this.Handler = handler;
            this.Type = type;
            this.Val = val;
        }

        public bool IsQualified(T type, V val)
        {
            return (this.Type.Equals(type) && this.Val.Equals(val));
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is SubscriptionRecord<H, T, V>)
            {
                SubscriptionRecord<H, T, V> o = obj as SubscriptionRecord<H, T, V>;
                return (Handler.Equals(o.Handler) && Type.Equals(o.Type) && Val.Equals(o.Val));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// This class helps track subscribers when they are subscribed to Notify classes
    /// </summary>
    /// <typeparam name="H">the event handler</typeparam>
    /// <typeparam name="T">the event type</typeparam>
    public class SubscriptionRecord<H, T>
    {
        public H Handler { get; set; }
        public T Type { get; set; }

        public SubscriptionRecord(H handler, T type)
        {
            this.Handler = handler;
            this.Type = type;
        }

        public bool IsQualified(T type)
        {
            return this.Type.Equals(type);
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is SubscriptionRecord<H, T>)
            {
                SubscriptionRecord<H, T> o = obj as SubscriptionRecord<H, T>;
                return (Handler.Equals(o.Handler) && Type.Equals(o.Type));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
