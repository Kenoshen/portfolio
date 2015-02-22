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
    /// This is the pause screen class
    /// </summary>
    public class Pause
    {
        #region State change variables
        public bool Complete = false;
        public ChangeScreen ChangeTheScreen;
        #endregion

        #region Sound change variables
        public PlayASound PlaySound;
        #endregion

        #region Fields
        Button returnToGameBttn;
        SpriteFont font;
        MouseState mouse;
        #endregion

        #region Properties
        #endregion

        #region Start Up Methods
        public Pause()
        {

        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            // TO-DO: change variables to alter position and size of button
            returnToGameBttn = new Button("Resume", 300, 100, 100, 50, graphics.GraphicsDevice);
        }

        public void LoadContent(ContentManager Content)
        {
            // TO-DO: alter the stats in the font file to change the look of the font
            font = Content.Load<SpriteFont>("Screens/Pause/pauseFont");

            // TO-DO: change the image files to make the button look different
            returnToGameBttn.LoadContent(font, "General/Buttons/genericBttnN", "General/Buttons/genericBttnH", "General/Buttons/genericBttnC", Content);
        }

        public void UnloadContent()
        {

        }
        #endregion

        #region Running Methods
        public void Update(GameTime gameTime, ContentManager Content)
        {
            mouse = Mouse.GetState();
            returnToGameBttn.Update(mouse);
            if (returnToGameBttn.CurrentState == BttnState.FullClick)
            {
                PlaySound(SoundType.Effect, SoundName.HitElectronic);
                returnToGameBttn.CurrentState = BttnState.Neutral;
                ChangeTheScreen(ScreenName.Game);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            returnToGameBttn.Draw(spriteBatch);
        }
        #endregion

        #region Private Methods
        #endregion
    }
}


