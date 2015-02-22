using System.Collections.Generic;
using Winger.SimpleMath;

namespace Winger.UserInterface.TransitionImpl
{
    public class FallDownTransition : Transition
    {
        public FallDownTransition()
        {
            MillisecondsToLive = 1000;
        }

        public override void Update()
        {
            switch (TransitionState)
            {
                case 1: // transition on
                    float complOn = GetPercentageComplete();
                    foreach (Element element in Page.GetAllElements())
                    {
                        float y = element.Y;
                        float prevY = 0;
                        Dictionary<string, object> record = null;
                        if (!Records.ContainsKey(element.GetHashCode()))
                        {
                            record = AddNewRecord(element);
                            record["y"] = element.Y;
                        }
                        else
                        {
                            record = Records[element.GetHashCode()];
                        }
                        prevY = (float)record["y"];

                        y = Tween.SinusoidalInOut(complOn, prevY + 500, -500, 1);
                        element.Y = y;
                    }
                    if (complOn == 1)
                    {
                        OnTransitionCompleted();
                    }
                    break;

                case -1: // transition off
                    float complOff = GetPercentageComplete();
                    foreach (Element element in Page.GetAllElements())
                    {
                        Dictionary<string, object> record = Records[element.GetHashCode()];
                        float y = element.Y;
                        float prevY = (float)record["y"];

                        y = Tween.SinusoidalInOut(complOff, prevY, +500, 1);
                        element.Y = y;
                    }
                    if (complOff == 1)
                    {
                        OffTransitionCompleted();
                    }
                    break;
            }
        }

        public override void TransitionOff()
        {
            foreach (Element element in Page.GetAllElements())
            {
                Dictionary<string, object> record = AddNewRecord(element);
                record["y"] = element.Y;
            }
            OffTransitionStarted();
        }

        public override void TransitionOn()
        {
            OnTransitionStarted();
        }

        public override Transition Clone(Page page)
        {
            FallDownTransition clone = new FallDownTransition();
            clone.MillisecondsToLive = this.MillisecondsToLive;
            clone.Page = page;
            return clone;
        }
    }
}
