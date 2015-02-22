using System.Collections.Generic;
using Winger.Utils;

namespace Winger.UserInterface
{
    public abstract class Transition
    {
        public Page Page { get; set; }
        private long _millisToLive = 0;
        public long MillisecondsToLive
        {
            get { return _millisToLive; }
            set { _millisToLive = value; }
        }
        public long GetMillisecondsToLive()
        {
            return _millisToLive;
        }
        public Transition SetMillisecondsToLive(long milliseconds)
        {
            _millisToLive = milliseconds;
            return this;
        }

        /// <summary>
        /// -1 means it is transitioning off
        /// 0 means it is not transitioning
        /// 1 means it is transitioning on
        /// </summary>
        protected int TransitionState { get; set; }
        protected long StartTime { get; private set; }
        protected long ExpectedEndTime { get; private set; }

        protected Dictionary<int, Dictionary<string, object>> Records = new Dictionary<int, Dictionary<string, object>>();

        protected virtual void InitializeDefaults()
        {
            MillisecondsToLive = 1000;
            SetStartTimeAsNow();
        }


        protected void SetStartTimeAsNow()
        {
            StartTime = GetCurrentTime();
            ExpectedEndTime = StartTime + MillisecondsToLive;
        }


        protected long GetCurrentTime()
        {
            return Timestamp.Now;
        }


        protected bool IsCurrentTimeExceededTimeToLive()
        {
            return (GetCurrentTime() > ExpectedEndTime);
        }


        protected float GetPercentageComplete()
        {
            if (MillisecondsToLive != 0)
            {
                float percentage = (float)(ExpectedEndTime - GetCurrentTime()) / (float)MillisecondsToLive;
                if (percentage > 1f)
                {
                    percentage = 1f;
                }
                else if (percentage < 0f)
                {
                    percentage = 0f;
                }
                return (1f - percentage);
            }
            return 0;
        }


        protected long GetMillisecondsLeft()
        {
            long left = ExpectedEndTime - GetCurrentTime();
            if (left < 0)
            {
                return 0;
            }
            return left;
        }


        protected Dictionary<string, object> AddNewRecord(Element element)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Records[element.GetHashCode()] = dic;
            return dic;
        }


        protected void OffTransitionStarted()
        {
            Page.IsEnabled = false;
            Page.IsVisible = true;
            Page.IsTransitioning = true;
            SetStartTimeAsNow();
            TransitionState = -1;
            if (Page.OnTransitionOffStart != null)
            {
                Notify(Page.OnTransitionOffStart);
            }
        }


        protected void OffTransitionCompleted()
        {
            Page.IsEnabled = false;
            Page.IsVisible = false;
            Page.IsTransitioning = false;
            TransitionState = 0;
            if (Page.OnTransitionOffEnd != null)
            {
                Notify(Page.OnTransitionOffEnd);
            }
        }


        protected void OnTransitionStarted()
        {
            Page.IsEnabled = false;
            Page.IsVisible = true;
            Page.IsTransitioning = true;
            SetStartTimeAsNow();
            TransitionState = 1;
            if (Page.OnTransitionOnStart != null)
            {
                Notify(Page.OnTransitionOnStart);
            }
        }


        protected void OnTransitionCompleted()
        {
            Page.IsEnabled = true;
            Page.IsVisible = true;
            Page.IsTransitioning = false;
            TransitionState = 0;
            if (Page.OnTransitionOnEnd != null)
            {
                Notify(Page.OnTransitionOnEnd);
            }
        }


        protected void Notify(string eventStr)
        {
            PageManager.Instance.NotifySubscribers(Page, Page.Name + "." + eventStr);
        }


        public abstract void Update();
        
        public abstract void TransitionOff();

        public abstract void TransitionOn();
        
        public abstract Transition Clone(Page page);
    }
}
