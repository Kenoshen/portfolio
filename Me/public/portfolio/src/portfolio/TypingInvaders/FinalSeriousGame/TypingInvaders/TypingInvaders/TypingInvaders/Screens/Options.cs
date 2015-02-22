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
    /// This is the options screen class
    /// </summary>
    public class Options
    {
        #region State change variables
        public bool Complete = false;
        public ChangeScreen ChangeTheScreen;
        #endregion

        #region Sound change variables
        public PlayASound PlaySound;
        public ToggleLoopMusic PlayMusic;
        public ChangeSoundVolume ChangeVolume;
        float lastEffectLevel;
        float currentEffectLevel;
        #endregion

        #region Fields
        Slider volumeSlider;
        Slider soundFXSlider;
        Slider musicSlider;
        Vector2 sliderDiff;
        Texture2D sliderBackground;
        Vector2 sliderBackgroundPos;
        Vector2 sliderTextPos;

        Texture2D background;

        Button returnToMenuBttn;
        SpriteFont font;
        MouseState mouse;
        #endregion

        #region Properties
        #endregion

        #region Start Up Methods
        public Options()
        {
            
        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            background = new Texture2D(graphics.GraphicsDevice, 800, 600);

            // TO-DO: change variables to alter position and size of button
            returnToMenuBttn = new Button("Back", 10, 515, 200, 75, graphics.GraphicsDevice);

            // TO-DO: change variables to alter position and length of the slider
            sliderDiff = new Vector2(0, 150);
            volumeSlider = new Slider(new Vector2(300, 100), 300, 50, graphics.GraphicsDevice);
            musicSlider = new Slider(volumeSlider.Position + sliderDiff, 300, 75, graphics.GraphicsDevice);
            soundFXSlider = new Slider(volumeSlider.Position + sliderDiff * 2, 300, 75, graphics.GraphicsDevice);
            currentEffectLevel = ((float)soundFXSlider.Value / 100);
            lastEffectLevel = currentEffectLevel;

            sliderBackground = new Texture2D(graphics.GraphicsDevice, 200, 75);
            sliderBackgroundPos = volumeSlider.Position;
            sliderBackgroundPos.X -= 250;
            sliderBackgroundPos.Y -= 28;
            sliderTextPos = volumeSlider.Position;
            sliderTextPos.X -= 150;
            sliderTextPos.Y -= 4;
        }

        public void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>("Screens/Options/background");

            // TO-DO: alter the stats in the font file to change the look of the font
            font = Content.Load<SpriteFont>("Screens/Options/optionsFont");

            // TO-DO: change the image files to make the button look different
            //returnToMenuBttn.LoadContent(font, "Options/Buttons/backBttnN", "Options/Buttons/backBttnH", "Options/Buttons/backBttnC", Content);
            returnToMenuBttn.LoadContent(font, "General/Buttons/genericBttnN", "General/Buttons/genericBttnH", "General/Buttons/genericBttnC", Content);

            volumeSlider.LoadContent(Content);
            musicSlider.LoadContent(Content);
            soundFXSlider.LoadContent(Content);
            sliderBackground = Content.Load<Texture2D>("General/UI/sliderBackground");
        }

        public void UnloadContent()
        {

        }
        #endregion

        #region Running Methods
        public void Update(GameTime gameTime, ContentManager Content)
        {
            mouse = Mouse.GetState();
            returnToMenuBttn.Update(mouse);
            if (returnToMenuBttn.CurrentState == BttnState.FullClick)
            {
                PlaySound(SoundType.Effect, SoundName.HitElectronic);
                returnToMenuBttn.CurrentState = BttnState.Neutral;
                ChangeTheScreen(ScreenName.Menu);
            }

            volumeSlider.Update(mouse);
            musicSlider.Update(mouse);
            soundFXSlider.Update(mouse);

            UpdateVolumeLevels();

            if (lastEffectLevel != currentEffectLevel)
            {
                PlaySound(SoundType.Effect, SoundName.Missile);
                lastEffectLevel = currentEffectLevel;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);


            spriteBatch.Draw(sliderBackground, sliderBackgroundPos, null, 
                Color.White, 0, Vector2.Zero, new Vector2(3.5f, 1), SpriteEffects.None, 0);
            spriteBatch.Draw(sliderBackground, sliderBackgroundPos + sliderDiff, 
                null, Color.White, 0, Vector2.Zero, new Vector2(3.5f, 1), SpriteEffects.None, 0);
            spriteBatch.Draw(sliderBackground, sliderBackgroundPos + sliderDiff * 2,
                null, Color.White, 0, Vector2.Zero, new Vector2(3.5f, 1), SpriteEffects.None, 0);

            returnToMenuBttn.Draw(spriteBatch);
            volumeSlider.Draw(spriteBatch);
            musicSlider.Draw(spriteBatch);
            soundFXSlider.Draw(spriteBatch);

            spriteBatch.DrawString(font, "Volume:   " + volumeSlider.Value, sliderTextPos, Color.Black);
            spriteBatch.DrawString(font, "Music:    " + musicSlider.Value, sliderTextPos + sliderDiff, Color.Black);
            spriteBatch.DrawString(font, "Sound FX: " + soundFXSlider.Value, sliderTextPos + sliderDiff * 2, Color.Black);
        }
        #endregion

        #region Private Methods
        public void UpdateVolumeLevels()
        {
            ChangeVolume(SoundType.Music, ((float)musicSlider.Value / 100) * ((float)volumeSlider.Value / 100));
            ChangeVolume(SoundType.Effect, ((float)soundFXSlider.Value / 100) * ((float)volumeSlider.Value / 100));

            currentEffectLevel = ((float)soundFXSlider.Value / 100);
        }

        public void OnScreenEnter()
        {
            PlayMusic(SoundName.TranceHouseLoop, true);
        }

        public void OnScreenExit()
        {
            PlayMusic(SoundName.TranceHouseLoop, false);
        }
        #endregion
    }
}

