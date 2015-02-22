using System.Collections.Generic;
using Winger.SimpleMath;

namespace Winger.UserInterface.TransitionImpl
{
    public class FallRightTransition : Transition
    {
        public FallRightTransition()
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
                        float x = element.X;
                        float prevX = 0;
                        Dictionary<string, object> record = null;
                        if (!Records.ContainsKey(element.GetHashCode()))
                        {
                            record = AddNewRecord(element);
                            record["x"] = element.X;
                        }
                        else
                        {
                            record = Records[element.GetHashCode()];
                        }
                        prevX = (float)record["x"];

                        x = Tween.SinusoidalInOut(complOn, prevX + 800, -800, 1);
                        element.X = x;
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
                        float x = element.X;
                        float prevX = (float)record["x"];

                        x = Tween.SinusoidalInOut(complOff, prevX, +800, 1);
                        element.X = x;
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
                record["x"] = element.X;
            }
            OffTransitionStarted();
        }

        public override void TransitionOn()
        {
            OnTransitionStarted();
        }

        public override Transition Clone(Page page)
        {
            FallRightTransition clone = new FallRightTransition();
            clone.MillisecondsToLive = this.MillisecondsToLive;
            clone.Page = page;
            return clone;
        }
    }
}
