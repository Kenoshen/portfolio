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
    public enum ScreenName
    {
        Logo,
        Menu,
        Options,
        Tutorial,
        Levelselect,
        Game,
        Pause,
        Gameover,
        Highscore,
    }

    public delegate void ChangeScreen(ScreenName name);
    public delegate void PlayASound(SoundType soundType, string soundName);
    public delegate void ChangeSoundVolume(SoundType soundType, float amountToChangeBy);
    public delegate void ToggleLoopMusic(string soundName, bool onOrOff);
    public delegate void StatisticTransferFromGame(GamePlayStats stats);

    /// <summary>
    /// This is the class that manages all the other screens
    /// </summary>
    public class ScreenManager
    {
        #region Screens
        Logo logo;
        Menu menu;
        Options options;
        Tutorial tutorial;
        LevelSelect levelselect;
        Game game;
        Pause pause;
        GameOver gameover;
        Highscore highscore;
        #endregion

        ScreenName currentScreen;
        ScreenName lastScreen;

        SoundManager soundManager;
        bool playingMusic;
        string currentMusic;
        Cue currentCue;

        List<GamePlayStats> stats;
        List<string> names;
        int ChosenLevel = 0;

        public QuitGame Quit;

        /// <summary>
        /// Calls the constructor for all the screens as well as 
        /// Links up all the delegates for the different screens
        /// </summary>
        public ScreenManager()
        {
            logo = new Logo();
            menu = new Menu();
            options = new Options();
            tutorial = new Tutorial();
            levelselect = new LevelSelect();
            game = new Game();
            pause = new Pause();
            gameover = new GameOver();
            highscore = new Highscore();

            logo.ChangeTheScreen = new TypingInvaders.ChangeScreen(ChangeScreen);
            menu.ChangeTheScreen = new TypingInvaders.ChangeScreen(ChangeScreen);
            options.ChangeTheScreen = new TypingInvaders.ChangeScreen(ChangeScreen);
            tutorial.ChangeTheScreen = new TypingInvaders.ChangeScreen(ChangeScreen);
            levelselect.ChangeTheScreen = new TypingInvaders.ChangeScreen(ChangeScreen);
            game.ChangeTheScreen = new TypingInvaders.ChangeScreen(ChangeScreen);
            pause.ChangeTheScreen = new TypingInvaders.ChangeScreen(ChangeScreen);
            gameover.ChangeTheScreen = new TypingInvaders.ChangeScreen(ChangeScreen);
            highscore.ChangeTheScreen = new TypingInvaders.ChangeScreen(ChangeScreen);

            logo.PlaySound = new TypingInvaders.PlayASound(PlaySound);
            menu.PlaySound = new TypingInvaders.PlayASound(PlaySound);
            options.PlaySound = new TypingInvaders.PlayASound(PlaySound);
            tutorial.PlaySound = new TypingInvaders.PlayASound(PlaySound);
            levelselect.PlaySound = new TypingInvaders.PlayASound(PlaySound);
            game.PlaySound = new TypingInvaders.PlayASound(PlaySound);
            pause.PlaySound = new TypingInvaders.PlayASound(PlaySound);
            gameover.PlaySound = new TypingInvaders.PlayASound(PlaySound);
            highscore.PlaySound = new TypingInvaders.PlayASound(PlaySound);

            menu.PlayMusic = new ToggleLoopMusic(PlayMusic);
            options.PlayMusic = new ToggleLoopMusic(PlayMusic);
            game.PlayMusic = new ToggleLoopMusic(PlayMusic);

            menu.Quit = new QuitGame(QuitOutOfTheGame);
            options.ChangeVolume = new ChangeSoundVolume(ChangeVolume);
            game.StatTransfer = new StatisticTransferFromGame(AddOntoStats);

            currentScreen = ScreenName.Logo;
            lastScreen = currentScreen;

            soundManager = new SoundManager();
            playingMusic = false;
            currentMusic = SoundName.TranceHouseLoop;

            stats = new List<GamePlayStats>();
            names = new List<string>();
        }

        /// <summary>
        /// Initializes all the screens in the game
        /// </summary>
        /// <param name="graphics">graphics</param>
        public void Initialize(GraphicsDeviceManager graphics)
        {
            logo.Initialize(graphics);
            menu.Initialize(graphics);
            options.Initialize(graphics);
            tutorial.Initialize(graphics);
            levelselect.Initialize(graphics);
            game.Initialize(graphics);
            gameover.Initialize(graphics);
            highscore.Initialize(graphics);

            options.UpdateVolumeLevels();
        }

        /// <summary>
        /// Loads all of the content for the screens
        /// </summary>
        /// <param name="Content">Content</param>
        public void LoadContent(ContentManager Content)
        {
            logo.LoadContent(Content);
            menu.LoadContent(Content);
            options.LoadContent(Content);
            tutorial.LoadContent(Content);
            levelselect.LoadContent(Content);
            game.LoadContent(Content);
            gameover.LoadContent(Content);
            highscore.LoadContent(Content);
        }

        /// <summary>
        /// Calls the UnloadContent methods for each screen
        /// </summary>
        public void UnloadContent()
        {
            logo.UnloadContent();
            menu.UnloadContent();
            options.UnloadContent();
            tutorial.UnloadContent();
            game.UnloadContent();
            gameover.UnloadContent();
            highscore.UnloadContent();
        }

        /// <summary>
        /// Updates the current screen and ignores all other screens
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="Content">Content</param>
        public void Update(GameTime gameTime, ContentManager Content)
        {
            #region Update current screen
            switch (currentScreen)
            {
                case ScreenName.Logo:
                    logo.Update(gameTime, Content);
                    break;

                case ScreenName.Menu:
                    menu.Update(gameTime, Content);
                    break;

                case ScreenName.Options:
                    options.Update(gameTime, Content);
                    break;

                case ScreenName.Tutorial:
                    tutorial.Update(gameTime, Content);
                    break;

                case ScreenName.Levelselect:
                    levelselect.Update(gameTime, Content);
                    break;

                case ScreenName.Game:
                    game.Update(gameTime, Content);
                    break;

                case ScreenName.Pause:
                    pause.Update(gameTime, Content);
                    break;

                case ScreenName.Gameover:
                    gameover.Update(gameTime, Content);
                    break;

                case ScreenName.Highscore:
                    highscore.Update(gameTime, Content);
                    break;

                default:
                    logo.Update(gameTime, Content);
                    break;
            }
            #endregion

            #region On enter and exits
            if (lastScreen != currentScreen)
            {
                switch (lastScreen)
                {
                    case ScreenName.Logo:
                        break;

                    case ScreenName.Menu:
                        menu.OnScreenExit();
                        break;

                    case ScreenName.Options:
                        options.OnScreenExit();
                        break;

                    case ScreenName.Tutorial:
                        tutorial.OnScreenExit();
                        break;

                    case ScreenName.Levelselect:
                        ChosenLevel = levelselect.OnScreenExit();
                        break;

                    case ScreenName.Game:
                        game.OnScreenExit();
                        break;

                    case ScreenName.Pause:
                        break;

                    case ScreenName.Gameover:
                        names.Add(gameover.OnScreenExit());
                        break;

                    case ScreenName.Highscore:
                        highscore.OnScreenExit();
                        break;

                    default:
                        break;
                }

                switch (currentScreen)
                {
                    case ScreenName.Logo:
                        break;

                    case ScreenName.Menu:
                        menu.OnScreenEnter();
                        break;

                    case ScreenName.Options:
                        options.OnScreenEnter();
                        break;

                    case ScreenName.Tutorial:
                        tutorial.OnScreenEnter(ChosenLevel);
                        break;

                    case ScreenName.Levelselect:
                        levelselect.OnScreenEnter();
                        break;

                    case ScreenName.Game:
                        game.OnScreenEnter(ChosenLevel);
                        break;

                    case ScreenName.Pause:
                        break;

                    case ScreenName.Gameover:
                        gameover.OnScreenEnter(stats[stats.Count - 1]);
                        break;

                    case ScreenName.Highscore:
                        highscore.OnScreenEnter(stats, names);
                        break;

                    default:
                        break;
                }
            }
            #endregion

            #region Sound updates
            soundManager.Update();

            if(!playingMusic)
            {
                soundManager.soundBank.GetCue(currentMusic).Stop(AudioStopOptions.Immediate);
            }
            #endregion

            lastScreen = currentScreen;
        }

        /// <summary>
        /// Draws the current screen and ignores all the others
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="spriteBatch">spriteBatch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            switch (currentScreen)
            {
                case ScreenName.Logo:
                    logo.Draw(gameTime, spriteBatch);
                    break;

                case ScreenName.Menu:
                    menu.Draw(gameTime, spriteBatch);
                    break;

                case ScreenName.Options:
                    options.Draw(gameTime, spriteBatch);
                    break;

                case ScreenName.Tutorial:
                    tutorial.Draw(gameTime, spriteBatch);
                    break;

                case ScreenName.Levelselect:
                    levelselect.Draw(gameTime, spriteBatch);
                    break;

                case ScreenName.Game:
                    game.Draw(gameTime, spriteBatch);
                    break;

                case ScreenName.Pause:
                    pause.Draw(gameTime, spriteBatch);
                    break;

                case ScreenName.Gameover:
                    gameover.Draw(gameTime, spriteBatch);
                    break;

                case ScreenName.Highscore:
                    highscore.Draw(gameTime, spriteBatch);
                    break;

                default:
                    logo.Draw(gameTime, spriteBatch);
                    break;
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Changes the screen state to a given screen
        /// </summary>
        /// <param name="name">the name of the screen to change to</param>
        public void ChangeScreen(ScreenName name)
        {
            currentScreen = name;
        }

        public void QuitOutOfTheGame()
        {
            Quit();
        }

        public void PlaySound(SoundType soundType, string soundName)
        {
            soundManager.PlayCue(soundType, soundName);
        }

        public void ChangeVolume(SoundType soundType, float amountToChangeBy)
        {
            switch (soundType)
            {
                case SoundType.Music:
                    soundManager.MusicVolume(amountToChangeBy);
                    break;

                case SoundType.Effect:
                    soundManager.EffectVolume(amountToChangeBy);
                    break;

                default:
                    break;
            }
        }

        public void PlayMusic(string soundName, bool onOrOff)
        {
            currentMusic = soundName;
            playingMusic = onOrOff;

            if (playingMusic)
            {
                currentCue = soundManager.soundBank.GetCue(soundName);
                currentCue.Play();
            }
            else
            {
                currentCue.Stop(AudioStopOptions.Immediate);
            }
        }

        public void AddOntoStats(GamePlayStats gameStats)
        {
            GamePlayStats addingStats = new GamePlayStats();
            addingStats.AvgRhythm = gameStats.AvgRhythm;
            addingStats.EachWordStats = gameStats.EachWordStats;
            addingStats.EndTime = gameStats.EndTime;
            addingStats.NumberOfMistakes = gameStats.NumberOfMistakes;
            addingStats.NumberOfWords = gameStats.NumberOfWords;
            addingStats.StartTime = gameStats.StartTime;
            addingStats.TotalPercentageCorrect = gameStats.TotalPercentageCorrect;
            addingStats.TotalTime = gameStats.TotalTime;
            addingStats.WordsPerMinute = gameStats.WordsPerMinute;

            stats.Add(addingStats);
        }
    }
}
