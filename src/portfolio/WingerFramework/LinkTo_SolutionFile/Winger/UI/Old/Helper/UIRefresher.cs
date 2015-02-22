using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Winger.UI.UI;
using Winger.UI.Input;
using Winger.UI.Event;

namespace Winger.UI.Helper
{
    public class UIRefresher : Winger.UI.UI.UI
    {
        private UIObject refreshButton;
        private List<Winger.UI.UI.UI> refreshListeners = new List<Winger.UI.UI.UI>();
        private ContentManager Content;
        private string spriteFontName;

        public UIRefresher(ContentManager Content, string spriteFontName)
        {
            this.Content = Content;
            this.spriteFontName = spriteFontName;
            tree = new Tree<UIObject>(UIObjectFactory.CreateWithTag("UI"));
            refreshButton = UIObjectFactory.CreateWithTag("button");
            tree.AddLeaf(refreshButton);
            refreshButton.X = 38;
            refreshButton.Y = 14;
            refreshButton.Width = 77;
            refreshButton.Height = 28;
            refreshButton.Text = "REFRESH";
            refreshButton.Depth = 0;

            root = tree.GetSelf();
            objs = tree.GetTreeList();
            mouse = new CMouse();

            UIObjectDelegate objDel = new UIObjectDelegate(RefreshUIEvent);
            foreach (Tree<UIObject> obj in objs)
            {
                obj.GetSelf().Delegate = objDel;
            }

            LoadContent(Content);
        }

        private new void LoadContent(ContentManager Content)
        {
            refreshButton.Put("font", spriteFontName);
            foreach (Tree<UIObject> obj in objs)
            {
                obj.GetSelf().LoadContent(Content);
            }
        }

        public void RegisterRefreshListener(Winger.UI.UI.UI ui)
        {
            bool found = false;
            for (int i = 0; i < refreshListeners.Count; i++)
            {
                if (refreshListeners[i] == ui)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                refreshListeners.Add(ui);
        }

        protected void RefreshUIEvent(UIObject sender, UIEventType type, string name)
        {
            if (type == UIEventType.SELECT_END)
            {
                for (int i = 0; i < refreshListeners.Count; i++)
                {
                    string path = refreshListeners[i].FileLocation;
                    refreshListeners[i].BuildFromFile(path);
                    refreshListeners[i].LoadContent(Content);
                    refreshListeners[i].StartTweens();
                }
            }
        }
    }
}
