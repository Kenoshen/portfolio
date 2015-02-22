using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Winger.UserInterface.TransitionImpl
{
    public class FadeTransition : Transition
    {
        public FadeTransition()
        {
            MillisecondsToLive = 200;
        }


        public override void Update()
        {
            switch (TransitionState)
            {
                case 1: // transition on
                    float complOn = GetPercentageComplete();
                    foreach (Element element in Page.GetAllElements())
                    {
                        Color col = element.Color;
                        Color colText = element.ColorText;
                        Color prevColor = Color.Black;
                        Color prevColorText = Color.Black;
                        Dictionary<string, object> record = null;
                        if (!Records.ContainsKey(element.GetHashCode()))
                        {
                            record = AddNewRecord(element);
                            record["color"] = new Color(element.Color.ToVector4());
                            record["color-text"] = new Color(element.ColorText.ToVector4());
                        }
                        else
                        {
                            record = Records[element.GetHashCode()];
                        }
                        prevColor = (Color)record["color"];
                        prevColorText = (Color)record["color-text"];

                        col.A = (byte)(complOn * (float)prevColor.A);
                        element.Color = col;
                        colText.A = (byte)(complOn * (float)prevColorText.A);
                        element.ColorText = colText;
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
                        Color col = element.Color;
                        Color colText = element.ColorText;
                        Color prevColor = (Color)record["color"];
                        Color prevColorText = (Color)record["color-text"];
                        col.A = (byte)((1f - complOff) * (float)prevColor.A);
                        element.Color = col;
                        colText.A = (byte)((1f - complOff) * (float)prevColorText.A);
                        element.ColorText = colText;
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
                record["color"] = new Color(element.Color.ToVector4());
                record["color-text"] = new Color(element.ColorText.ToVector4());
            }
            OffTransitionStarted();
        }


        public override void TransitionOn()
        {
            OnTransitionStarted();
        }


        public override Transition Clone(Page page)
        {
            FadeTransition clone = new FadeTransition();
            clone.MillisecondsToLive = this.MillisecondsToLive;
            clone.Page = page;
            return clone;
        }
    }
}
