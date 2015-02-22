
namespace Winger.UserInterface.TransitionImpl
{
    public class DefaultTransition : Transition
    {
        public DefaultTransition()
        {
            MillisecondsToLive = 1;
        }


        public override void Update()
        {
            
        }


        public override void TransitionOff()
        {
            OffTransitionCompleted();
        }


        public override void TransitionOn()
        {
            OnTransitionCompleted();
        }


        public override Transition Clone(Page page)
        {
            DefaultTransition clone = new DefaultTransition();
            clone.MillisecondsToLive = this.MillisecondsToLive;
            clone.Page = page;
            return clone;
        }
    }
}
