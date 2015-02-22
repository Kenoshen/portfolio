using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Winger.Utils
{
    public class FontManager
    {
        #region Singleton
        private static FontManager instance;

        private FontManager() { }

        public static FontManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FontManager();
                }
                return instance;
            }
        }
        #endregion

        private Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();


        public void AddDefaultFont(SpriteFont font)
        {
            AddFont("default", font);
        }


        public SpriteFont GetDefaultFont()
        {
            return GetFont("default");
        }


        public void AddFont(string name, SpriteFont font)
        {
            fonts[name] = font;
        }


        public SpriteFont GetFont(string name)
        {
            if (fonts.ContainsKey(name))
            {
                return fonts[name];
            }
            return null;
        }


        public bool RemoveFont(string name)
        {
            if (fonts.ContainsKey(name))
            {
                return fonts.Remove(name);
            }
            return false;
        }
    }
}
