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
    class KeyboardTutorial
    {
        Texture2D keyboard;
        Vector2 kBPos;
        Texture2D highlight;
        List<Keys> hLKeys;
        List<Vector2> hLPos;

        public KeyboardTutorial(Vector2 position, GraphicsDevice graphics)
        {
            keyboard = new Texture2D(graphics, 500, 150);
            kBPos = position;
            highlight = new Texture2D(graphics, 50, 50);
            hLKeys = new List<Keys>();
            hLPos = new List<Vector2>();
        }

        public void LoadContent(ContentManager Content)
        {
            keyboard = Content.Load<Texture2D>("Screens/Tutorial/keyboard");
            highlight = Content.Load<Texture2D>("Screens/Tutorial/keyboardHighlight");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Vector2 pos in hLPos)
            {
                spriteBatch.Draw(highlight, pos, Color.White);
            }

            spriteBatch.Draw(keyboard, kBPos, Color.White);
        }

        public bool AddKeyToHighlight(Keys key)
        {
            bool added = true;

            foreach (Keys k in hLKeys)
            {
                if (k == key)
                {
                    added = false;
                }
            }

            if (added)
            {
                hLKeys.Add(key);
                hLPos.Add(GetPosWithKey(key));
            }

            return added;
        }

        public bool RemoveKeyToHighlight(Keys key)
        {
            bool removed = false;
            int index = 0;

            for(int i = 0; i < hLKeys.Count - 1; i++)
            {
                if (hLKeys[i] == key)
                {
                    removed = true;
                    index = i;
                    break;
                }
            }

            if (removed)
            {
                hLKeys.RemoveAt(index);
                hLPos.RemoveAt(index);
            }

            return removed;
        }

        public void NoHighlightedKeys()
        {
            hLKeys = new List<Keys>();
            hLPos = new List<Vector2>();
        }

        public Vector2 GetPosWithKey(Keys key)
        {
            switch (key)
            {
                case Keys.A:
                    return new Vector2(kBPos.X + 25 * 1, kBPos.Y + 50);

                case Keys.B:
                    return new Vector2(kBPos.X + 25 * 10, kBPos.Y + 100);

                case Keys.C:
                    return new Vector2(kBPos.X + 25 * 6, kBPos.Y + 100);

                case Keys.D:
                    return new Vector2(kBPos.X + 25 * 5, kBPos.Y + 50);

                case Keys.E:
                    return new Vector2(kBPos.X + 25 * 4, kBPos.Y);

                case Keys.F:
                    return new Vector2(kBPos.X + 25 * 7, kBPos.Y + 50);

                case Keys.G:
                    return new Vector2(kBPos.X + 25 * 9, kBPos.Y + 50);

                case Keys.H:
                    return new Vector2(kBPos.X + 25 * 11, kBPos.Y + 50);

                case Keys.I:
                    return new Vector2(kBPos.X + 25 * 14, kBPos.Y);

                case Keys.J:
                    return new Vector2(kBPos.X + 25 * 13, kBPos.Y + 50);

                case Keys.K:
                    return new Vector2(kBPos.X + 25 * 15, kBPos.Y + 50);

                case Keys.L:
                    return new Vector2(kBPos.X + 25 * 17, kBPos.Y + 50);

                case Keys.M:
                    return new Vector2(kBPos.X + 25 * 14, kBPos.Y + 100);

                case Keys.N:
                    return new Vector2(kBPos.X + 25 * 12, kBPos.Y + 100);

                case Keys.O:
                    return new Vector2(kBPos.X + 25 * 16, kBPos.Y);

                case Keys.P:
                    return new Vector2(kBPos.X + 25 * 18, kBPos.Y);

                case Keys.Q:
                    return new Vector2(kBPos.X + 25 * 0, kBPos.Y);

                case Keys.R:
                    return new Vector2(kBPos.X + 25 * 6, kBPos.Y);

                case Keys.S:
                    return new Vector2(kBPos.X + 25 * 3, kBPos.Y + 50);

                case Keys.T:
                    return new Vector2(kBPos.X + 25 * 8, kBPos.Y);

                case Keys.U:
                    return new Vector2(kBPos.X + 25 * 12, kBPos.Y);

                case Keys.V:
                    return new Vector2(kBPos.X + 25 * 8, kBPos.Y + 100);

                case Keys.W:
                    return new Vector2(kBPos.X + 25 * 2, kBPos.Y);

                case Keys.X:
                    return new Vector2(kBPos.X + 25 * 4, kBPos.Y + 100);

                case Keys.Y:
                    return new Vector2(kBPos.X + 25 * 10, kBPos.Y);

                case Keys.Z:
                    return new Vector2(kBPos.X + 25 * 2, kBPos.Y + 100);

                case Keys.OemSemicolon:
                    return new Vector2(kBPos.X + 25 * 19, kBPos.Y + 50);

                default:
                    return new Vector2(-100, -100);
            }
        }
    }
}
