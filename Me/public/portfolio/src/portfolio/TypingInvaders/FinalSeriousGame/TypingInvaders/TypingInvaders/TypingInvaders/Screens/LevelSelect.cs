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
    public class LevelSelect
    {
        #region State change variables
        public bool Complete = false;
        public ChangeScreen ChangeTheScreen;
        #endregion

        #region Sound change variables
        public PlayASound PlaySound;
        #endregion

        #region Fields
        Texture2D background;
        Vector2 backgroundPosition;

        Button[] levelButtons;
        SpriteFont font;

        MouseState mouse;

        public int ChosenLevel = 0;
        #endregion

        #region Properties
        public Vector2 BackgroundPosition
        {
            get { return backgroundPosition; }
            set { backgroundPosition = value; }
        }
        #endregion

        #region Start Up Methods
        public LevelSelect()
        {

        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            // TO-DO: change these values to match the width and height of the backround image
            int width = 1;
            int height = 1;
            background = new Texture2D(graphics.GraphicsDevice, width, height);

            // TO-DO: change these values to position the top left corner of the backround image
            float x = 0;
            float y = 0;
            backgroundPosition = new Vector2(x, y);

            // TO-DO: change this int to how ever many buttons you want on the menu screen
            int numOfButtons = 5;
            levelButtons = new Button[numOfButtons];

            // TO-DO: change the values of the variables to change positions and names of the buttons
            levelButtons[0] = new Button("Level 1", 300, 100, 200, 75, graphics.GraphicsDevice);
            levelButtons[1] = new Button("Level 2", 300, 200, 200, 75, graphics.GraphicsDevice);
            levelButtons[2] = new Button("Level 3", 300, 300, 200, 75, graphics.GraphicsDevice);
            levelButtons[3] = new Button("Level 4", 300, 400, 200, 75, graphics.GraphicsDevice);
            levelButtons[4] = new Button("Menu", 20, 515, 200, 75, graphics.GraphicsDevice);
        }

        public void LoadContent(ContentManager Content)
        {
            // TO-DO: edit these image files to change the look of the different pieces
            background = Content.Load<Texture2D>("Screens/Levelselect/background");

            // TO-DO: change the settings in the buttonFont file to change the look of the font
            font = Content.Load<SpriteFont>("General/generalFont");

            for (int i = 0; i < levelButtons.Length; i++)
            {
                levelButtons[i].LoadContent(font, "General/Buttons/genericBttnN", "General/Buttons/genericBttnH", "General/Buttons/genericBttnC", Content);
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
            for (int i = 0; i < levelButtons.Length; i++)
            {
                levelButtons[i].Update(mouse);
                if (levelButtons[i].CurrentState == BttnState.FullClick)
                {
                    PlaySound(SoundType.Effect, SoundName.HitElectronic);

                    for (int k = 0; k < levelButtons.Length; k++)
                    {
                        levelButtons[k].CurrentState = BttnState.Neutral;
                    }

                    string screenName = levelButtons[i].ButtonName;

                    switch (screenName)
                    {
                        case "Level 1":
                            ChangeTheScreen(ScreenName.Tutorial);
                            ChosenLevel = 1;
                            break;

                        case "Level 2":
                            ChangeTheScreen(ScreenName.Tutorial);
                            ChosenLevel = 2;
                            break;

                        case "Level 3":
                            ChangeTheScreen(ScreenName.Tutorial);
                            ChosenLevel = 3;
                            break;

                        case "Level 4":
                            ChangeTheScreen(ScreenName.Tutorial);
                            ChosenLevel = 4;
                            break;

                        case "Menu":
                            ChangeTheScreen(ScreenName.Menu);
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

            for (int i = 0; i < levelButtons.Length; i++)
            {
                levelButtons[i].Draw(spriteBatch);
            }
        }
        #endregion

        #region Private Methods
        public void OnScreenEnter()
        {
            ChosenLevel = 0;
        }

        public int OnScreenExit()
        {
            return ChosenLevel;
        }
        #endregion
    }
}



