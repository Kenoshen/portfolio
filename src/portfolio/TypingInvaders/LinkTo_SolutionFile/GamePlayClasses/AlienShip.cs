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
    class AlienShip
    {
        #region Fields
        SpriteStrip alien;
        SpriteStrip highlight;
        SpriteStrip explosion;

        Vector2 position;
        Vector2 speed;

        bool selected;
        bool dead;
        bool attacking;

        bool active;
        #endregion

        #region Properties
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public bool Dead
        {
            get { return dead; }
            set { dead = value; }
        }

        public bool Attacking
        {
            get { return attacking; }
            set { attacking = value; }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public float VerticalSpeed
        {
            get { return speed.Y; }
            set { speed.Y = value; }
        }

        public float HorizontalSpeed
        {
            get { return speed.X; }
            set { speed.X = value; }
        }
        #endregion

        #region Start up methods
        public AlienShip(GraphicsDevice graphics, float horizontalSpeed, float verticalSpeed)
        {
            alien = new SpriteStrip(100, 100, 300, 400, 10, 3, graphics);
            alien.FrameSpeed = 3;
            highlight = new SpriteStrip(100, 100, 100, 100, 1, 1, graphics);
            explosion = new SpriteStrip(100, 100, 1000, 400, 39, 10, graphics);
            explosion.FrameSpeed = 3;

            position = new Vector2(350, 0);
            speed = new Vector2(horizontalSpeed, verticalSpeed);

            selected = false;
            dead = false;
            attacking = false;

            active = true;
        }

        public void LoadContent(ContentManager Content)
        {
            alien.LoadContent("GamePlayContent/alienShip", Content);
            explosion.LoadContent("GamePlayContent/alienExplosion", Content);
        }
        #endregion

        #region Running methods
        public void Update(float verticalSpeed)
        {
            if (active)
            {
                speed.Y = verticalSpeed;

                if (position.X < 50)
                {
                    speed.X += 0.1f;
                }

                if (position.X + alien.SpriteBox.Width > 750)
                {
                    speed.X -= 0.1f;
                }

                #region Set positions
                position += speed;

                alien.Position = position;
                highlight.Position = position;
                explosion.Position = position;
                #endregion
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                if (selected)
                {
                    highlight.Draw(spriteBatch);
                }

                if (!dead)
                {
                    alien.Draw(spriteBatch);
                }
                else
                {
                    explosion.Draw(spriteBatch);
                }
            }
        }
        #endregion

        #region Private methods
        public void Reset(float yPosition, float verticalSpeed)
        {
            position.Y = yPosition;
            speed.Y = verticalSpeed;
            dead = false;
            active = true;
            selected = false;
        }
        #endregion
    }
}
