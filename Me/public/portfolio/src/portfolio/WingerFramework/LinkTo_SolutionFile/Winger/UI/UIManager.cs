using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Winger.Utils;
using Winger.UI.CSS;
using Winger.UI.HTML;
using Winger.UI.IMG;

namespace Winger.UI
{
    public class UIManager
    {
        #region Singleton
        private static UIManager instance;

        private UIManager() { }

        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UIManager();
                }
                return instance;
            }
        }
        #endregion


        public string RootDirectory = "";
        private List<UIScreen> screens = new List<UIScreen>();
        private UIScreen curScreen = null;


        #region Build screens
        public void BuildUIScreens()
        {
            string dir = Directory.GetCurrentDirectory() + "\\" + RootDirectory;

            BuildUIScreen(dir, "//");
        }


        private void BuildUIScreen(string dir, string parentPath)
        {
            string[] files = Directory.GetFiles(dir, "index.html");
            if (files.Length == 0)
            {
                Console.WriteLine("There was no index.html file at '" + dir + "'");
                return;
            }
            UIScreen screen = new UIScreen();
            screen.ParentPath = parentPath;
            string[] split = dir.Split('\\');
            screen.Name = split[split.Length - 1];
            string indexFilePath = files[0];
            // get html elements here
            screen.RootHTMLElement = HTMLUtils.ParseHTMLFile(indexFilePath);
            List<string> linksForCSS = screen.DiscoverLinks();
            foreach (string link in linksForCSS)
            {
                string cssLinkPath = dir + "\\" + link;
                if (File.Exists(cssLinkPath))
                {
                    string cssStr = FileUtils.FileToString(cssLinkPath);
                    // parse css elements here
                    screen.CSSElements = CSSUtils.ParseCSSString(cssStr, FileUtils.FilePathToDir(cssLinkPath));
                }
                else
                {
                    Console.WriteLine("Could not find file at " + cssLinkPath);
                }
            }
            screen.ApplyCSSToHTML();
            screen.RootHTMLElement.SetAllRects(new CRectangle(GraphicsUtils.Instance.GraphicsDevice.Viewport.Bounds));
            screen.BuildRenderTree();

            // add the screen to the screen list
            screens.Add(screen);

            List<string> directories = new List<string>(Directory.EnumerateDirectories(dir));
            foreach (string directory in directories)
            {
                BuildUIScreen(directory, screen.FullPathName);
            }

            ImgElementManager.Instance.LoadContentForImgElements();
        }
        #endregion


        #region Access screens
        public UIScreen GetScreenFromName(string screenName)
        {
            foreach (UIScreen screen in screens)
            {
                if (screen.Name.Equals(screenName))
                {
                    return screen;
                }
            }
            return null;
        }


        public UIScreen GetScreenFromFullPathName(string fullPathName)
        {
            foreach (UIScreen screen in screens)
            {
                if (screen.FullPathName.Equals(fullPathName))
                {
                    return screen;
                }
            }
            return null;
        }


        public List<UIScreen> GetChildScreensFromParentPath(string parentPath)
        {
            List<UIScreen> childScreens = new List<UIScreen>();
            foreach (UIScreen screen in screens)
            {
                if (screen.ParentPath.Equals(parentPath))
                {
                    childScreens.Add(screen);
                }
            }
            return childScreens;
        }


        public UIScreen GetCurrentScreen()
        {
            return curScreen;
        }


        public void PrintScreens()
        {
            Console.WriteLine(ListUtils.ListToString<UIScreen>(screens));
        }
        #endregion
    }
}
