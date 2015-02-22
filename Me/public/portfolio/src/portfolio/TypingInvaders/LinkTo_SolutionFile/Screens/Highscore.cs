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
    public class Highscore
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

        List<GamePlayStats> stats;
        List<string> names;
        Vector2 statPos = new Vector2(10, 50);
        Vector2 colDif = new Vector2(135, 0);
        Vector2 rowDif = new Vector2(0, 30);
        string stat_name =    "Name";
        string stat_wpm =     "WPM";
        string stat_mstk =    "Mistakes";
        string stat_prcntge = "%Right";
        string stat_rthm =    "Rhythm";
        string stat_wrds =    "Words";

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
        public Highscore()
        {

        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            stats = new List<GamePlayStats>();
            names = new List<string>();

            background = new Texture2D(graphics.GraphicsDevice, 800, 600);

            // TO-DO: change variables to alter position and size of button
            returnToMenuBttn = new Button("Menu", 10, 515, 200, 75, graphics.GraphicsDevice);
        }

        public void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>("Screens/Highscore/background");

            // TO-DO: alter the stats in the font file to change the look of the font
            font = Content.Load<SpriteFont>("Screens/Highscore/highscoreFont");

            // TO-DO: change the image files to make the button look different
            //returnToMenuBttn.LoadContent(font, "Highscore/Buttons/backBttnN", "Highscore/Buttons/backBttnH", "Highscore/Buttons/backBttnC", Content);
            returnToMenuBttn.LoadContent(font, "General/Buttons/genericBttnN", "General/Buttons/genericBttnH", "General/Buttons/genericBttnC", Content);
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
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            returnToMenuBttn.Draw(spriteBatch);

            spriteBatch.DrawString(font, stat_name, statPos + colDif * 0, Color.White);
            spriteBatch.DrawString(font, stat_wpm, statPos + colDif * 1, Color.White);
            spriteBatch.DrawString(font, stat_wrds, statPos + colDif * 2, Color.White);
            spriteBatch.DrawString(font, stat_mstk, statPos + colDif * 3, Color.White);
            spriteBatch.DrawString(font, stat_prcntge, statPos + colDif * 4, Color.White);
            spriteBatch.DrawString(font, stat_rthm, statPos + colDif * 5, Color.White);
            
            if (stats.Count > 0)
            {
                int index = 0;
                foreach (string name in names)
                {
                    #region Convert Rhythm stat
                    if (stats[index].AvgRhythm < RhythmGood)
                    {
                        rGrade = rG;
                    }
                    else if (stats[index].AvgRhythm < RhythmFair)
                    {
                        rGrade = rF;
                    }
                    else
                    {
                        rGrade = rB;
                    }
                    #endregion

                    spriteBatch.DrawString(font, name, statPos + (rowDif * (index + 1)) + (colDif * 0), Color.White);
                    spriteBatch.DrawString(font, "" + stats[index].WordsPerMinute, statPos + (rowDif * (index + 1)) + (colDif * 1), Color.White);
                    spriteBatch.DrawString(font, "" + stats[index].NumberOfWords, statPos + (rowDif * (index + 1)) + (colDif * 2), Color.White);
                    spriteBatch.DrawString(font, "" + stats[index].NumberOfMistakes, statPos + (rowDif * (index + 1)) + (colDif * 3), Color.White);
                    spriteBatch.DrawString(font, "" + stats[index].TotalPercentageCorrect, statPos + (rowDif * (index + 1)) + (colDif * 4), Color.White);
                    spriteBatch.DrawString(font, rGrade, statPos + (rowDif * (index + 1)) + (colDif * 5), Color.White);
                    index++;
                }
            }
        }
        #endregion

        #region Private Methods
        public void OnScreenEnter(List<GamePlayStats> gameStats, List<string> gameNames)
        {
            stats = gameStats;
            names = gameNames;

            if (names.Count > 12)
            {
                stats.RemoveAt(0);
                names.RemoveAt(0);
            }
        }

        public void OnScreenExit()
        {
            
        }
        #endregion
    }
}

