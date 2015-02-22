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
    class NameInputBox
    {
        #region Fields
        KeyboardState keyboard;
        string typedWord;
        Vector2 position;
        List<Keys> thisKeysPressed;
        List<Keys> lastKeysPressed;
        SpriteFont font;
        bool finished;
        #endregion

        #region Properties
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public string Word
        {
            get { return typedWord; }
        }

        public int WordLength
        {
            get { return typedWord.Length; }
        }

        public bool Finished
        {
            get { return finished; }
        }
        #endregion

        #region Start up methods
        public NameInputBox()
        {
            typedWord = "";
            position = new Vector2(50, 400);
            thisKeysPressed = new List<Keys>();
            lastKeysPressed = new List<Keys>();
            finished = false;
        }

        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Screens/Gameover/gameoverFont");
        }
        #endregion

        #region Running methods
        public void Update()
        {
            if (!finished)
            {
                keyboard = Keyboard.GetState();

                Keys[] keysPressed = keyboard.GetPressedKeys();
                thisKeysPressed = keysPressed.ToList<Keys>();

                #region Remove keys from previous presses
                for (int i = 0; i < thisKeysPressed.Count; i++)
                {
                    for (int k = 0; k < lastKeysPressed.Count; k++)
                    {
                        if (thisKeysPressed[i] == lastKeysPressed[k])
                        {
                            thisKeysPressed.RemoveAt(i);
                            i--;
                            k = lastKeysPressed.Count;
                        }
                    }
                }
                #endregion

                #region Add single press to string
                if (thisKeysPressed.Count == 1)
                {
                    if (typedWord.Length < 15 && AcceptableKeyPress(thisKeysPressed[0]))
                    {
                        typedWord += thisKeysPressed[0];
                    }
                }
                #endregion

                #region Delete entry
                if (keyboard.IsKeyDown(Keys.Back) || keyboard.IsKeyDown(Keys.Delete))
                {
                    typedWord = "";
                }
                #endregion

                #region Accept entry
                if (keyboard.IsKeyDown(Keys.Enter) && typedWord.Length > 0)
                {
                    finished = true;
                }
                #endregion

                lastKeysPressed = keysPressed.ToList<Keys>();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Type Your Name and Press ENTER:  " + typedWord, position, Color.White);
        }
        #endregion

        #region Public methods
        public void ResetNameBox()
        {
            typedWord = "";
            finished = false;
        }

        public bool AcceptableKeyPress(Keys key)
        {
            bool correctKey = false;

            if (key == Keys.A ||
                key == Keys.B ||
                key == Keys.C ||
                key == Keys.D ||
                key == Keys.E ||
                key == Keys.F ||
                key == Keys.G ||
                key == Keys.H ||
                key == Keys.I ||
                key == Keys.J ||
                key == Keys.K ||
                key == Keys.L ||
                key == Keys.M ||
                key == Keys.N ||
                key == Keys.O ||
                key == Keys.P ||
                key == Keys.Q ||
                key == Keys.R ||
                key == Keys.S ||
                key == Keys.T ||
                key == Keys.U ||
                key == Keys.V ||
                key == Keys.W ||
                key == Keys.X ||
                key == Keys.Y ||
                key == Keys.Z)
            {
                correctKey = true;
            }

            return correctKey;
        }
        #endregion
    }
}
