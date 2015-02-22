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
    /// <summary>
    /// This is the logo screen class
    /// </summary>
    public class Logo
    {
        #region State change variables
        public bool Complete = false;
        public ChangeScreen ChangeTheScreen;
        #endregion

        #region Sound change variables
        public PlayASound PlaySound;
        #endregion

        #region Fields
        SpriteStrip logoStrip;

        Texture2D background;
        Vector2 backgroundPosition;

        const int MAX_TIME_ON_SCREEN = 5000;
        int timer;

        bool mousePressed;
        #endregion

        #region Properties
        public Vector2 LogoPosition
        {
            get { return logoStrip.Position; }
            set { logoStrip.Position = value; }
        }

        public Vector2 BackgroundPosition
        {
            get { return backgroundPosition; }
            set { backgroundPosition = value; }
        }

        public int Timer
        {
            get { return timer; }
            set { timer = value; }
        }

        public int MaxTimeOnScreen
        {
            get { return MAX_TIME_ON_SCREEN; }
        }
        #endregion

        #region Start Up Methods
        public Logo()
        {

        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            logoStrip = new SpriteStrip(100, 100, 500, 500, 25, 5, graphics.GraphicsDevice);
            logoStrip.FrameSpeed = 4;
            logoStrip.Position = new Vector2(10, 135);
            logoStrip.Size = 3f;

            int width = 1;
            int height = 1;
            background = new Texture2D(graphics.GraphicsDevice, width, height);
            float x = 0;
            float y = 0;
            backgroundPosition = new Vector2(x, y);

            timer = 0;

            mousePressed = false;
        }

        public void LoadContent(ContentManager Content)
        {
            logoStrip.LoadContent("Screens/Logo/logo", Content);

            background = Content.Load<Texture2D>("Screens/Logo/background");
        }

        public void UnloadContent()
        {

        }
        #endregion

        #region Running Methods
        public void Update(GameTime gameTime, ContentManager Content)
        {
            #region Updates the timer and changes the screen name when the timer hits the max mark
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer >= MAX_TIME_ON_SCREEN)
            {
                timer = 0;
                Complete = true;
                ChangeTheScreen(ScreenName.Menu);
            }
            #endregion

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                mousePressed = true;
            }
            else if (mousePressed == true)
            {
                PlaySound(SoundType.Effect, SoundName.HitElectronic);
                timer += MAX_TIME_ON_SCREEN;
                mousePressed = false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, backgroundPosition, Color.White);
            logoStrip.Draw(spriteBatch);
        }
        #endregion

        #region Private Methods
        #endregion
    }
}


