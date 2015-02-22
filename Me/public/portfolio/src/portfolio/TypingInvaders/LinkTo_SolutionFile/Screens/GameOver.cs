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
    /// This is the highscore screen class
    /// </summary>
    public class GameOver
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
        Button returnToMenuBttn;
        SpriteFont font;
        MouseState mouse;

        GamePlayStats stats;
        Vector2 statPos = new Vector2(100, 100);
        Vector2 statDif = new Vector2(0, 50);
        string stat_wpm = "Words Per Min: ";
        string stat_mstk = "Number of Mistakes: ";
        string stat_prcntge = "Percentage Correct: ";
        string stat_rthm = "Typing Rhythm: ";
        string stat_wrds = "Total typed Words: ";

        NameInputBox name;
        bool showButton = false;

        int RhythmGood = 60;
        int RhythmFair = 100;
        string rG = "Good";
        string rF = "Fair";
        string rB = "Bad";
        string rGrade = "";
        #endregion

        #region Properties
        #endregion

        #region Start Up Methods
        public GameOver()
        {

        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            stats = new GamePlayStats();

            background = new Texture2D(graphics.GraphicsDevice, 800, 600);

            // TO-DO: change variables to alter position and size of button
            returnToMenuBttn = new Button("Menu", 10, 515, 200, 75, graphics.GraphicsDevice);

            name = new NameInputBox();
        }

        public void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>("Screens/Gameover/background");

            // TO-DO: alter the stats in the font file to change the look of the font
            font = Content.Load<SpriteFont>("Screens/Gameover/gameoverFont");

            // TO-DO: change the image files to make the button look different
            //returnToMenuBttn.LoadContent(font, "Highscore/Buttons/backBttnN", "Highscore/Buttons/backBttnH", "Highscore/Buttons/backBttnC", Content);
            returnToMenuBttn.LoadContent(font, "General/Buttons/genericBttnN", "General/Buttons/genericBttnH", "General/Buttons/genericBttnC", Content);

            name.LoadContent(Content);
        }

        public void UnloadContent()
        {

        }
        #endregion

        #region Running Methods
        public void Update(GameTime gameTime, ContentManager Content)
        {
            if (showButton)
            {
                mouse = Mouse.GetState();
                returnToMenuBttn.Update(mouse);
                if (returnToMenuBttn.CurrentState == BttnState.FullClick)
                {
                    PlaySound(SoundType.Effect, SoundName.HitElectronic);
                    returnToMenuBttn.CurrentState = BttnState.Neutral;
                    ChangeTheScreen(ScreenName.Menu);
                }
            }

            name.Update();

            if (name.Finished)
            {
                showButton = true;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            if (showButton)
            {
                returnToMenuBttn.Draw(spriteBatch);
            }

            spriteBatch.DrawString(font, stat_wpm + stats.WordsPerMinute, statPos + statDif * 0, Color.White);
            spriteBatch.DrawString(font, stat_wrds + stats.NumberOfWords, statPos + statDif * 1, Color.White);
            spriteBatch.DrawString(font, stat_mstk + stats.NumberOfMistakes, statPos + statDif * 2, Color.White);
            spriteBatch.DrawString(font, stat_prcntge + stats.TotalPercentageCorrect, statPos + statDif * 3, Color.White);
            spriteBatch.DrawString(font, stat_rthm + rGrade, statPos + statDif * 4, Color.White);

            name.Draw(spriteBatch);
        }
        #endregion

        #region Private Methods
        public void OnScreenEnter(GamePlayStats lastGameStats)
        {
            stats = lastGameStats;
            name.ResetNameBox();
            showButton = false;

            #region Convert Rhythm stat
            if (stats.AvgRhythm < RhythmGood)
            {
                rGrade = rG;
            }
            else if (stats.AvgRhythm < RhythmFair)
            {
                rGrade = rF;
            }
            else
            {
                rGrade = rB;
            }
            #endregion
        }

        public string OnScreenExit()
        {
            return name.Word;
        }
        #endregion
    }
}

