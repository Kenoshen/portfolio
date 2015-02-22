using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Winger.UI.Helper;
using Winger.UI.Input;
using Winger.UI.Event;

namespace Winger.UI.UI
{
    public class UI
    {
        public string FileLocation
        {
            get
            {
                return fileLocation;
            }
        }

        public float RootX
        {
            get
            {
                return root.X;
            }
            set
            {
                root.X = value;
            }
        }

        public float RootY
        {
            get
            {
                return root.Y;
            }
            set
            {
                root.Y = value;
            }
        }

        public string RootName
        {
            get
            {
                return root.Name;
            }
            set
            {
                root.Name = value;
            }
        }

        protected Tree<UIObject> tree;
        protected UIObject root;
        protected string fileLocation;
        protected CMouse mouse;
        protected List<Tree<UIObject>> objs;
        protected List<UIEventListenerTracker> listeners = new List<UIEventListenerTracker>();

        protected UI()
        {

        }

        public UI(string fileLocation)
        {
            this.fileLocation = fileLocation;
            BuildFromFile(this.fileLocation);
            mouse = new CMouse();
        }

        public void BuildFromFile(string fileLocation)
        {
            tree = Winger.UI.Helper.Utils.GetTreeFromFile(fileLocation);
            root = tree.GetSelf();
            objs = tree.GetTreeList();

            UIObjectDelegate objDel = new UIObjectDelegate(UIEvent);
            foreach (Tree<UIObject> obj in objs)
            {
                obj.GetSelf().Delegate = objDel;
            }
        }

        public void LoadContent(ContentManager Content)
        {
            foreach (Tree<UIObject> obj in objs)
            {
                obj.GetSelf().LoadContent(Content);
            }
        }

        public void Update(int elapsedMilliseconds)
        {
            mouse.Update();
            foreach (Tree<UIObject> obj in objs)
            {
                if (obj.GetParent() != null)
                    obj.GetSelf().Update(elapsedMilliseconds, obj.GetParent().GetSelf(), mouse);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tree<UIObject> obj in objs)
            {
                if (obj.GetParent() != null)
                    obj.GetSelf().Draw(obj.GetParent().GetSelf(), spriteBatch);
            }
        }

        public UIObject GetUIObjectByName(string name)
        {
            foreach (Tree<UIObject> obj in objs)
            {
                if (obj.GetSelf().Name == name)
                {
                    return obj.GetSelf();
                }
            }
            return null;
        }

        public void StartTweens()
        {
            foreach (Tree<UIObject> obj in objs)
            {
                obj.GetSelf().StartTweenIfExists();
            }
        }

        public void RegisterListener(IUIEventListener listener, string name)
        {
            UIEventListenerTracker tracker = new UIEventListenerTracker();
            tracker.listener = listener;
            tracker.name = name;
            listeners.Add(tracker);
        }

        public void RegisterListener(IUIEventListener listener, string senderName, UIEventType type)
        {
            UIEventListenerTracker tracker = new UIEventListenerTracker();
            tracker.listener = listener;
            tracker.senderName = senderName;
            tracker.type = type;
            listeners.Add(tracker);
        }

        protected void UIEvent(UIObject sender, UIEventType type, string name)
        {
            if (name == null)
            {
                for (int i = 0; i < listeners.Count; i++)
                {
                    if (sender.Name != null && sender.Name == listeners[i].senderName && type == listeners[i].type)
                    {
                        switch (type)
                        {
                            case UIEventType.HOVER_START:
                                listeners[i].listener.HoverStartUIEvent(sender);
                                break;

                            case UIEventType.HOVER_END:
                                listeners[i].listener.HoverEndUIEvent(sender);
                                break;

                            case UIEventType.SELECT_START:
                                listeners[i].listener.SelectStartUIEvent(sender);
                                break;

                            case UIEventType.SELECT_END:
                                listeners[i].listener.SelectEndUIEvent(sender);
                                break;

                            case UIEventType.CUSTOM:
                                listeners[i].listener.CustomUIEvent(sender);
                                break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < listeners.Count; i++)
                {
                    if (name == listeners[i].name)
                    {
                        listeners[i].listener.NamedUIEvent(sender, name);
                    }
                }
            }
        }



        protected struct UIEventListenerTracker
        {
            public IUIEventListener listener;
            public string senderName;
            public UIEventType type;
            public string name;
        }
    }
}
