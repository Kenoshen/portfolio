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
    public enum BttnState
    {
        Neutral,
        Hover,
        Click,
        FullClick,
    }

    class Button
    {
        #region Fields
        Texture2D neutral;
        Texture2D hover;
        Texture2D click;
        SpriteFont font;
        string name;
        Vector2 textPos;
        Rectangle rectangle;
        BttnState currentState;
        #endregion

        #region Properties
        public BttnState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public string ButtonName
        {
            get { return name; }
            set { name = value; }
        }
        #endregion

        #region Start up methods
        /// <summary>
        /// Makes a new button
        /// </summary>
        /// <param name="buttonName">the text to be written on the button</param>
        /// <param name="x">x coordinate of the top left corner location of the button</param>
        /// <param name="y">x coordinate of the top left corner location of the button</param>
        /// <param name="width">width of the button</param>
        /// <param name="height">height of the button</param>        
        /// <param name="graphics">graphics.GraphicsDevice</param>
        public Button(string buttonName, int x, int y, int width, int height, GraphicsDevice graphics)
        {
            rectangle = new Rectangle(x, y, width, height);

            neutral = new Texture2D(graphics, width, height);
            hover = new Texture2D(graphics, width, height);
            click = new Texture2D(graphics, width, height);

            name = buttonName;
            textPos = new Vector2(rectangle.X + rectangle.Width / 2 - name.Length * 7, rectangle.Y + rectangle.Height / 4);

            currentState = BttnState.Neutral;
        }

        /// <summary>
        /// Loads the image files for the different button states
        /// </summary>
        /// /// <param name="buttonFont">the font for the text that is displayed on the button</param>
        /// <param name="neutralState">the file location for the button when it not being acted upon</param>
        /// <param name="hoverState">the file location for the button when it is being hovered over by the mouse</param>
        /// <param name="clickState">the file location for the button when it is being click on</param>
        /// <param name="Content">Content</param>
        public void LoadContent(SpriteFont buttonFont, string neutralState, string hoverState, string clickState, ContentManager Content)
        {
            font = buttonFont;

            neutral = Content.Load<Texture2D>(neutralState);
            hover = Content.Load<Texture2D>(hoverState);
            click = Content.Load<Texture2D>(clickState);
        }
        #endregion

        #region Running methods
        public void Update(MouseState mouse)
        {
            #region Mouse stats
            Vector2 mPos = new Vector2(mouse.X, mouse.Y);
            bool pressed = false;
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                pressed = true;
            }
            #endregion

            switch (currentState)
            {
                case BttnState.Neutral:
                    UpdateNeutral(mPos, pressed);
                    break;

                case BttnState.Hover:
                    UpdateHover(mPos, pressed);
                    break;

                case BttnState.Click:
                    UpdateClick(mPos, pressed);
                    break;

                case BttnState.FullClick:
                    // do nothing
                    break;

                default:
                    UpdateNeutral(mPos, pressed);
                    break;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (currentState)
            {
                case BttnState.Neutral:
                    spriteBatch.Draw(neutral, rectangle, Color.White);
                    break;

                case BttnState.Hover:
                    spriteBatch.Draw(hover, rectangle, Color.White);
                    break;

                case BttnState.Click:
                    spriteBatch.Draw(click, rectangle, Color.White);
                    break;

                case BttnState.FullClick:
                    spriteBatch.Draw(neutral, rectangle, Color.White);
                    break;

                default:
                    spriteBatch.Draw(neutral, rectangle, Color.White);
                    break;
            }

            spriteBatch.DrawString(font, name, textPos, Color.Black);
        }
        #endregion

        #region Private methods
        private void UpdateNeutral(Vector2 mousePos, bool leftPressed)
        {
            if (rectangle.Contains((int)mousePos.X, (int)mousePos.Y))
            {
                if (leftPressed)
                {
                    currentState = BttnState.Click;
                }
                else
                {
                    currentState = BttnState.Hover;
                }
            }
        }

        private void UpdateHover(Vector2 mousePos, bool leftPressed)
        {
            if (rectangle.Contains((int)mousePos.X, (int)mousePos.Y))
            {
                if (leftPressed)
                {
                    currentState = BttnState.Click;
                }
            }
            else
            {
                currentState = BttnState.Neutral;
            }
        }

        private void UpdateClick(Vector2 mousePos, bool leftPressed)
        {
            if (rectangle.Contains((int)mousePos.X, (int)mousePos.Y))
            {
                if (!leftPressed)
                {
                    currentState = BttnState.FullClick;
                }
            }
            else
            {
                currentState = BttnState.Neutral;
            }
        }
        #endregion
    }
}
