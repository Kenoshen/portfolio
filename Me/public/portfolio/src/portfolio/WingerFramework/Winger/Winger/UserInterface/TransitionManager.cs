using System.Collections.Generic;
using Winger.UserInterface.TransitionImpl;

namespace Winger.UserInterface
{
    public class TransitionManager
    {
        #region Singleton
        private static TransitionManager instance;

        private TransitionManager()
        {
            SetDefaultTransition(new DefaultTransition());
        }

        public static TransitionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TransitionManager();
                }
                return instance;
            }
        }
        #endregion


        private Dictionary<string, Transition> transitions = new Dictionary<string, Transition>();


        public Transition GetDefaultTransition()
        {
            return GetTransition("default");
        }


        public void SetDefaultTransition(Transition transition)
        {
            PutTransition("default", transition);
        }


        public void PutTransition(string name, Transition transition)
        {
            transitions[name] = transition;
        }


        public Transition GetTransition(string name)
        {
            if (transitions.ContainsKey(name))
            {
                return transitions[name];
            }
            return null;
        }


        public bool RemoveTransition(string name)
        {
            if (transitions.ContainsKey(name))
            {
                return transitions.Remove(name);
            }
            return false;
        }
    }
}
