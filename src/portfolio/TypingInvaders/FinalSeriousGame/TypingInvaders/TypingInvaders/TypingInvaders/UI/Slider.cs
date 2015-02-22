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
    class Slider
    {
        #region Fields
        Texture2D slider;
        Vector2 sliderPos;

        Texture2D button;
        Rectangle buttonRect;

        Texture2D end;
        Vector2 endL;
        Vector2 endR;

        float maxLeft;
        float maxRight;

        int value;
        float length;

        bool mouseGrabbing;
        bool mouseHovering;
        #endregion

        #region Properties
        public int Value
        {
            get { return value; }
        }

        public Vector2 Position
        {
            get { return new Vector2(sliderPos.X - end.Width, sliderPos.Y); }
        }
        #endregion

        #region Start up methods
        /// <summary>
        /// Makes a slider user interface object
        /// </summary>
        /// <param name="position">the top left corner of the slider bar</param>
        /// <param name="pixelLength"> the length in pixels of the slider bar</param>
        /// <param name="startingPercentValue">the default value for the sliding bar</param>
        /// <param name="graphics">graphics.GraphicsDevice</param>
        public Slider(Vector2 position, int pixelLength, int startingPercentValue, GraphicsDevice graphics)
        {
            slider = new Texture2D(graphics, pixelLength, 20);

            button = new Texture2D(graphics, 20, 20);
            value = startingPercentValue;

            end = new Texture2D(graphics, 20, 20);

            mouseGrabbing = false;
            mouseHovering = false;

            endL = position;
            sliderPos = position;
            sliderPos.X += end.Width;
            endR = position;
            endR.X += end.Width + slider.Width;

            maxLeft = endL.X;
            maxRight = endR.X;

            length = maxRight - maxLeft;

            buttonRect = new Rectangle((int)position.X, (int)position.Y, button.Width, button.Height);
            
            int tempValue = (int)(((float)value / 100) * length);
            buttonRect.X = tempValue + (int)maxLeft;
        }

        public void LoadContent(ContentManager Content)
        {
            // TO-DO: change the look of the sprites but don't alter their pixel height and width
            slider = Content.Load<Texture2D>("General/UI/sliderMiddle");
            end = Content.Load<Texture2D>("General/UI/sliderEnd");
            button = Content.Load<Texture2D>("General/UI/sliderButton");
        }
        #endregion

        #region Running methods
        public void Update(MouseState mouse)
        {
            if (mouseGrabbing == false)
            {
                if (buttonRect.Contains(mouse.X, mouse.Y))
                {
                    if (mouse.LeftButton == ButtonState.Released && !mouseHovering)
                    {
                        mouseHovering = true;
                    }

                    if (mouse.LeftButton == ButtonState.Pressed && mouseHovering)
                    {
                        mouseGrabbing = true;
                        mouseHovering = false;
                    }

                }
            }
            else
            {
                if (mouse.LeftButton == ButtonState.Released)
                {
                    mouseGrabbing = false;
                }
                else
                {
                    buttonRect.X = mouse.X - button.Width / 2;

                    value = buttonRect.X - (int)maxLeft;
                    value = (int)((value / length) * 100);

                    if (buttonRect.X < maxLeft)
                    {
                        buttonRect.X = (int)maxLeft;
                        value = 0;
                    }
                    else if (buttonRect.X > maxRight)
                    {
                        buttonRect.X = (int)maxRight;
                        value = 100;
                    }
                }
            }

            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(end, endL, Color.White);
            spriteBatch.Draw(slider, sliderPos, null, Color.White, 0, Vector2.Zero, new Vector2((length - end.Width) / 100, 1), SpriteEffects.None, 0);
            spriteBatch.Draw(end, endR, null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.FlipHorizontally, 0);

            spriteBatch.Draw(button, buttonRect, Color.White);
        }
        #endregion
    }
}
