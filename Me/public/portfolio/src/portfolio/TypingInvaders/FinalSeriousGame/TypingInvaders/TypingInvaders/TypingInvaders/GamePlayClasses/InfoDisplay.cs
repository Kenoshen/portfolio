using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TypingInvaders
{
    class InfoDisplay
    {
        Texture2D display;
        Rectangle displayRect;

        string text;
        string title;
        SpriteFont font;

        public int X
        {
            get { return displayRect.X; }
            set { displayRect.X = value; }
        }

        public int Y
        {
            get { return displayRect.Y; }
            set { displayRect.Y = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public InfoDisplay(string text, string title, Vector2 position, int width, int height, GraphicsDevice graphics)
        {
            display = new Texture2D(graphics, 100, 100);

            displayRect = new Rectangle((int)position.X, (int)position.Y, width, height);

            this.text = text;
            this.title = title;
        }

        public void LoadContent(ContentManager Content)
        {
            display = Content.Load<Texture2D>("GamePlayContent/displayBox");
            font = Content.Load<SpriteFont>("GamePlayContent/gameFont");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(display, new Vector2(displayRect.X, displayRect.Y), null, Color.White, 0, 
                Vector2.Zero, new Vector2((float)displayRect.Width / 100, (float)displayRect.Height / 100),SpriteEffects.None, 0);

            spriteBatch.DrawString(font, text, new Vector2(displayRect.X + 10, displayRect.Y + (displayRect.Width / 2) - 10),Color.Black);
            spriteBatch.DrawString(font, title, new Vector2(displayRect.X + (displayRect.Width / 2) - (title.Length * 5), displayRect.Y - 20), Color.Black);
        }
    }
}
