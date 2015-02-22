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
    class SpriteStrip
    {
        #region Fields
        Texture2D spriteStrip;
        Rectangle spriteBox;
        Vector2 position;
        Vector2[] boxPlacments;
        bool animationActive;
        int animationFrame;
        int frameSpeed;
        int timer;
        float size;
        #endregion

        #region Properties
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Rectangle SpriteBox
        {
            get { return spriteBox; }
            set { spriteBox = value; }
        }

        public bool AnimationActive
        {
            get { return animationActive; }
            set { animationActive = value; }
        }

        public int AnimationFrame
        {
            get { return animationFrame; }
            set
            {
                 animationFrame = value;
                spriteBox.X = (int)boxPlacments[animationFrame].X;
                spriteBox.Y = (int)boxPlacments[animationFrame].Y;
            }
        }

        public int FrameSpeed
        {
            get { return frameSpeed; }
            set { frameSpeed = value; }
        }

        public float Size
        {
            get { return size; }
            set { size = value; }
        }
        #endregion

        #region Start up methods
        public SpriteStrip(int boxWidth, int boxHeight, int totalWidth, int totalHeight, int frames, int collumns, GraphicsDevice graphics)
        {
            animationActive = true;
            animationFrame = 0;
            frameSpeed = 1;
            timer = 0;
            size = 1;
            position = Vector2.Zero;
            spriteBox = new Rectangle(0, 0, boxWidth, boxHeight);
            spriteStrip = new Texture2D(graphics, totalWidth, totalHeight);
            boxPlacments = new Vector2[frames];

            int extra = frames % collumns;
            int rows = frames / collumns;
            if (extra != 0)
            {
                rows++;     
            }

            int currentRow;
            int currentCollumn;
            int index = 0;

            for (currentRow = 0; currentRow < rows; currentRow++)
            {
                if (currentRow + 1 == rows)
                {
                    if (rows != 1)
                    {
                        for (int i = 0; i < extra; i++)
                        {
                            boxPlacments[index] = new Vector2(boxWidth * i, boxHeight * currentRow);
                            index++;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < collumns; i++)
                        {
                            boxPlacments[index] = new Vector2(boxWidth * i, 0);
                            index++;
                        }
                    }
                }
                else
                {
                    for (currentCollumn = 0; currentCollumn < collumns; currentCollumn++)
                    {
                        boxPlacments[index] = new Vector2(boxWidth * currentCollumn, boxHeight * currentRow);
                        index++;
                    }
                }
            }
        }

        public void LoadContent(string filePath, ContentManager Content)
        {
            spriteStrip = Content.Load<Texture2D>(filePath);
        }
        #endregion

        #region Running methods
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteStrip, position, spriteBox, Color.White, 0, Vector2.Zero, size, SpriteEffects.None, 0);

            if (animationActive)
            {
                timer++;
                if (timer >= frameSpeed)
                {
                    animationFrame++;
                    if (animationFrame >= boxPlacments.Length || animationFrame < 0)
                    {
                        animationFrame = 0;
                    }

                    spriteBox.X = (int)boxPlacments[animationFrame].X;
                    spriteBox.Y = (int)boxPlacments[animationFrame].Y;

                    timer = 0;
                }
            }
        }
        #endregion
    }
}
