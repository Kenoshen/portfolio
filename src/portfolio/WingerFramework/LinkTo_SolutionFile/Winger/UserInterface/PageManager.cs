using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Winger.Utils;

namespace Winger.UserInterface
{
    public class PageManager : Notifier<PageEventHandler, string>
    {
        #region Singleton
        private static PageManager instance;

        private PageManager() { }

        public static PageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PageManager();
                }
                return instance;
            }
        }
        #endregion


        private Dictionary<string, Page> pages = new Dictionary<string, Page>();


        public void AddPagesInDirectory(string absoluteDirectoryPath)
        {
            List<string> pageFilesPaths = Directory.GetFiles(absoluteDirectoryPath, "*.ui").ToList<string>();

            foreach (string page in pageFilesPaths)
            {
                string pageName = Path.GetFileNameWithoutExtension(page);
                Page pageObj = new Page(page);
                PutPage(pageName, pageObj);
            }
            // add sub directories
            List<string> directories = new List<string>(Directory.EnumerateDirectories(absoluteDirectoryPath));
            foreach (string directory in directories)
            {
                AddPagesInDirectory(directory);
            }
        }


        public void PutPage(string name, Page page)
        {
            pages[name] = page;
            page.Name = name;
        }


        public Page GetPage(string name)
        {
            if (pages.ContainsKey(name))
            {
                return pages[name];
            }
            return null;
        }


        public bool RemovePage(string name)
        {
            if (pages.ContainsKey(name))
            {
                return pages.Remove(name);
            }
            return false;
        }


        public void TransitionToPage(Page page)
        {
            if (page == null)
            {
                return;
            }

            for (int i = 0; i < pages.Keys.Count; i++)
            {
                Page curPage = pages[pages.Keys.ElementAt(i)];
                if (curPage.IsEnabled || curPage.IsTransitioning)
                {
                    // set that page to transition off
                    if (curPage.Transition != null)
                    {
                        curPage.Transition.TransitionOff();
                    }
                }
            }

            // tell the page to transition on
            if (page.Transition != null)
            {
                page.Transition.TransitionOn();
            }
        }


        public void TransitionToPage(string page)
        {
            TransitionToPage(GetPage(page));
        }


        public Element GetElement(string pageName, string elementId)
        {
            Element e = null;
            Page p = GetPage(pageName);
            if (p != null)
            {
                e = p.GetElementById(elementId);
            }
            return e;
        }


        public void NotifySubscribers(object sender, string identifier)
        {
            List<SubscriptionRecord<PageEventHandler, string>> subsToNotify = GetSubscribersToNotify(identifier);
            foreach (SubscriptionRecord<PageEventHandler, string> sub in subsToNotify)
            {
                sub.Handler.HandleEvent(sender, identifier);
            }
        }


        public void Update()
        {
            for (int i = 0; i < pages.Keys.Count; i++)
            {
                pages[pages.Keys.ElementAt(i)].Update();
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < pages.Keys.Count; i++)
            {
                pages[pages.Keys.ElementAt(i)].Draw(spriteBatch);
            }
        }
    }
}
