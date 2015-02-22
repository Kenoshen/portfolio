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
    class HealthBar
    {
        Texture2D green;
        Texture2D red;

        Rectangle barRect;

        float percentage;
        float value;
        float total;

        public float PercentageFull
        {
            get { return percentage; }
        }

        public float Total
        {
            get { return total; }
        }

        public float Value
        {
            get { return value; }
        }

        public HealthBar(Vector2 position, int width, int height, GraphicsDevice graphics)
        {
            green = new Texture2D(graphics, 100, 30);
            red = new Texture2D(graphics, 100, 30);

            barRect = new Rectangle((int)position.X, (int)position.Y, width, height);

            percentage = 100;
            total = width;
            value = total;
        }

        public void LoadContent(ContentManager Content)
        {
            green = Content.Load<Texture2D>("GamePlayContent/healthBarGreen");
            red = Content.Load<Texture2D>("GamePlayContent/healthBarRed");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(red, new Vector2(barRect.X, barRect.Y), null, Color.White, 0, Vector2.Zero, new Vector2(barRect.Width / 100, (float)barRect.Height / 30), SpriteEffects.None, 0);
            spriteBatch.Draw(green, new Vector2(barRect.X, barRect.Y), null, Color.White, 0, Vector2.Zero, new Vector2(value / 100, (float)barRect.Height / 30), SpriteEffects.None, 0); 
        }

        public void MinusPercentageTotalHealth(float health)
        {
            if (health > 100)
            {
                health = 100;
            }
            else if (health < 0)
            {
                health = 0;
            }

            value -= total * (health / 100);

            if (value > total)
            {
                value = total;
            }
            else if (value < 0)
            {
                value = 0;
            }

            CalculatePercentage();
        }

        public void AddPercentageTotalHealth(float health)
        {
            if (health > 100)
            {
                health = 100;
            }
            else if (health < 0)
            {
                health = 0;
            }

            value += total * (health / 100);

            if (value > total)
            {
                value = total;
            }
            else if (value < 0)
            {
                value = 0;
            }

            CalculatePercentage();
        }

        public void MinusPercentageCurrentHealth(float health)
        {
            if (health > 100)
            {
                health = 100;
            }
            else if (health < 0)
            {
                health = 0;
            }

            value -= value * (health / 100);

            if (value > total)
            {
                value = total;
            }
            else if (value < 0)
            {
                value = 0;
            }

            CalculatePercentage();
        }

        public void AddPercentageCurrentHealth(float health)
        {
            if (health > 100)
            {
                health = 100;
            }
            else if (health < 0)
            {
                health = 0;
            }

            value += value * (health / 100);

            if (value > total)
            {
                value = total;
            }
            else if (value < 0)
            {
                value = 0;
            }

            CalculatePercentage();
        }

        private void CalculatePercentage()
        {
            percentage = value / total;
            percentage *= 100;

            if (percentage > 100)
            {
                percentage = 100;
            }
            else if (percentage < 0)
            {
                percentage = 0;
            }
        }
    }
}
