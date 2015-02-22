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
    /// This is the menu screen class
    /// </summary>
    public class Menu
    {
        #region State change variables
        public bool Complete = false;
        public ChangeScreen ChangeTheScreen;
        public QuitGame Quit;
        #endregion

        #region Sound change variables
        public PlayASound PlaySound;
        public ToggleLoopMusic PlayMusic;
        #endregion

        #region Fields
        Texture2D background;
        Vector2 backgroundPosition;

        SpriteStrip title;

        Button[] menuButtons;
        SpriteFont buttonFont;

        MouseState mouse;
        #endregion

        #region Properties
        public Vector2 BackgroundPosition
        {
            get { return backgroundPosition; }
            set { backgroundPosition = value; }
        }
        #endregion

        #region Start Up Methods
        public Menu()
        {

        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            title = new SpriteStrip(500, 250, 2000, 1000, 15, 4, graphics.GraphicsDevice);
            title.FrameSpeed = 3;
            title.Size = 1.5f;
            title.Position = new Vector2(30, 0);

            // TO-DO: change these values to match the width and height of the backround image
            int width = 1;
            int height = 1;
            background = new Texture2D(graphics.GraphicsDevice, width, height);

            // TO-DO: change these values to position the top left corner of the backround image
            float x = 0;
            float y = 0;
            backgroundPosition = new Vector2(x, y);

            // TO-DO: change this int to how ever many buttons you want on the menu screen
            int numOfButtons = 4;
            menuButtons = new Button[numOfButtons];

            // TO-DO: change the values of the variables to change positions and names of the buttons
            menuButtons[0] = new Button("Play",         300, 275,   200, 75, graphics.GraphicsDevice);
            menuButtons[1] = new Button("Options",      300, 355,   200, 75, graphics.GraphicsDevice);
            menuButtons[2] = new Button("Scores",   300, 435,   200, 75, graphics.GraphicsDevice);
            menuButtons[3] = new Button("Quit",         300, 515,   200, 75, graphics.GraphicsDevice);
        }

        public void LoadContent(ContentManager Content)
        {
            title.LoadContent("Screens/Menu/title2", Content);

            // TO-DO: edit these image files to change the look of the different pieces
            background = Content.Load<Texture2D>("Screens/Menu/background");

            // TO-DO: change the settings in the buttonFont file to change the look of the font
            buttonFont = Content.Load<SpriteFont>("Screens/Menu/menuFont");

            for (int i = 0; i < menuButtons.Length; i++)
            {
                menuButtons[i].LoadContent(buttonFont, "General/Buttons/genericBttnN", "General/Buttons/genericBttnH", "General/Buttons/genericBttnC", Content);
            }
        }

        public void UnloadContent()
        {

        }
        #endregion

        #region Running Methods
        public void Update(GameTime gameTime, ContentManager Content)
        {
            mouse = Mouse.GetState();
            for (int i = 0; i < menuButtons.Length; i++)
            {
                menuButtons[i].Update(mouse);
                if (menuButtons[i].CurrentState == BttnState.FullClick)
                {
                    PlaySound(SoundType.Effect, SoundName.HitElectronic);

                    for (int k = 0; k < menuButtons.Length; k++)
                    {
                        menuButtons[k].CurrentState = BttnState.Neutral;
                    }

                    string screenName = menuButtons[i].ButtonName;

                    switch (screenName)
                    {
                        case "Play":
                            ChangeTheScreen(ScreenName.Levelselect);
                            break;

                        case "Options":
                            ChangeTheScreen(ScreenName.Options);
                            break;

                        case "Scores":
                            ChangeTheScreen(ScreenName.Highscore);
                            break;

                        case "Quit":
                            Quit();
                            break;

                        default:
                            ChangeTheScreen(ScreenName.Logo);
                            break;
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, backgroundPosition, Color.White);
            title.Draw(spriteBatch);

            for (int i = 0; i < menuButtons.Length; i++)
            {
                menuButtons[i].Draw(spriteBatch);
            }
        }
        #endregion

        #region Private Methods
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


