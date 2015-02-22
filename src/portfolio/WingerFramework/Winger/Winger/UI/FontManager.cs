using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Winger.UI
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


        public void AddFont(SpriteFont font, string fontName)
        {
            fonts[fontName] = font;
        }


        public void AddDefaultFont(SpriteFont font)
        {
            fonts["default"] = font;
        }


        public SpriteFont GetFont(string fontName)
        {
            if (fonts.ContainsKey(fontName))
            {
                return fonts[fontName];
            }
            if (fonts.ContainsKey("default"))
            {
                return fonts["default"];
            }
            return null;
        }
    }
}
