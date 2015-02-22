using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Winger.Draw
{
    public class DrawText
    {
        #region Constructors

        public DrawText(string text, Vector2 position, SpriteBatch spriteBatch, SpriteFont font, Color color)
        {
            Text = text;
            Position = position;
            SpriteBatch = spriteBatch;
            SpriteFont = font;
            Color = color;
        }

        public DrawText(string text, Vector2 position, SpriteBatch spriteBatch, SpriteFont font)
        {
            Text = text;
            Position = position;
            SpriteBatch = spriteBatch;
            SpriteFont = font;
            Color = Microsoft.Xna.Framework.Color.Black;
        }

        public DrawText(SpriteBatch spriteBatch, SpriteFont font)
        {
            Text = "";
            Position = Vector2.Zero;
            SpriteBatch = spriteBatch;
            SpriteFont = font;
            Color = Microsoft.Xna.Framework.Color.Black;
        }

        #endregion

        public Vector2 Position { get; set; }
        public string Text { get; set; }
        public Color Color { get; set; }
        public SpriteFont SpriteFont { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        public void Draw()
        {
            SpriteBatch.DrawString(SpriteFont, Text, Position, Color);
        }

        #region Static

        public static void DrawTextToScreen(Vector2 position, string text, SpriteBatch spriteBatch, SpriteFont fontToUse)
        {
            spriteBatch.DrawString(fontToUse, text, position, Color.Black);
        }

        public static void DrawTextToScreen(Vector2 position, string text, SpriteBatch spriteBatch, SpriteFont fontToUse, Color color)
        {
            spriteBatch.DrawString(fontToUse, text, position, color);
        }

        #endregion
    }
}
