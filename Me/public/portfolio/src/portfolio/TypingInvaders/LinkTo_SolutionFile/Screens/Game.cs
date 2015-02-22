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
    public delegate void ChangeFingerHighlight(Fingers finger);
    public delegate void WrongKeySound();

    /// <summary>
    /// This is the game screen class
    /// </summary>
    public class Game
    {
        #region State change variables
        public bool Complete = false;
        public ChangeScreen ChangeTheScreen;
        #endregion

        #region Sound change variables
        public PlayASound PlaySound;
        public ToggleLoopMusic PlayMusic;
        #endregion

        #region Stat transfer
        public StatisticTransferFromGame StatTransfer;
        #endregion

        #region Fields
        Random rnd;
        TextReader textReader;
        List<AlienShip> alienShips;
        SpriteStrip destroyedShip;
        Vector2 destroyedShipSpeed;
        bool destroyed;
        Texture2D greenLaser;
        bool greenFiring;
        int greenTimer;
        float shipSpacing;
        float shipVerticalSpeed;
        Hands hands;
        KeyboardState keyboard;
        KeyboardInputBox testBox;
        InfoPanel infoPanel;
        InfoDisplay[] displays;
        HealthBar healthBar;
        GamePlayStats stats;
        List<string> words1;
        List<string> words2;
        List<string> words3;
        List<string> words4;
        int timeUpTimer = 0;
        int timeUp = 60000 * 1;
        bool timerStart = false;
        SpriteFont font;
        int ChosenLevel = 0;
        #endregion

        #region Properties
        #endregion

        #region Start Up Methods
        public Game()
        {

        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            rnd = new Random();
            textReader = new TextReader();

            alienShips = new List<AlienShip>();
            destroyedShip = new SpriteStrip(100, 100, 1000, 400, 39, 10, graphics.GraphicsDevice);
            destroyedShipSpeed = Vector2.Zero;
            destroyed = false;
            greenLaser = new Texture2D(graphics.GraphicsDevice, 100, 10);
            greenFiring = false;
            greenTimer = 0;
            shipSpacing = 75;
            shipVerticalSpeed = 1;
            for (int i = 0; i < 10; i++)
            {
                alienShips.Add(new AlienShip(graphics.GraphicsDevice, (float)rnd.Next(-30, 30) / 10, shipVerticalSpeed));
                alienShips[i].Reset(i * -shipSpacing, shipVerticalSpeed);
            }

            hands = new Hands(graphics.GraphicsDevice);

            words1 = textReader.LoadHomeRow("../../../GamePlayClasses/words.txt");
            words2 = textReader.LoadHomeTopRow("../../../GamePlayClasses/words.txt");
            words3 = textReader.LoadHomeBottomRow("../../../GamePlayClasses/words.txt");
            words4 = textReader.LoadAllRows("../../../GamePlayClasses/words.txt");

            testBox = new KeyboardInputBox();
            testBox.FingerHighlight = new ChangeFingerHighlight(FingerPicker);
            testBox.WrongKey = new TypingInvaders.WrongKeySound(WrongKeySound);
            testBox.NewBoxWord(PickRandomWord(1));
            
            stats = new GamePlayStats();

            infoPanel = new InfoPanel(graphics.GraphicsDevice);
            displays = new InfoDisplay[5];
            displays[0] = new InfoDisplay("", "WPM",new Vector2(50, 450), 50, 50, graphics.GraphicsDevice);
            displays[1] = new InfoDisplay("", "Words",new Vector2(50, 525), 50, 50, graphics.GraphicsDevice);
            displays[2] = new InfoDisplay("", "Time",new Vector2(375, 450), 50, 50, graphics.GraphicsDevice);
            displays[3] = new InfoDisplay("", "Mistakes",new Vector2(700, 450), 50, 50, graphics.GraphicsDevice);
            displays[4] = new InfoDisplay("", "Rhythm", new Vector2(700, 525), 50, 50, graphics.GraphicsDevice);
            healthBar = new HealthBar(new Vector2(300, 360), 200, 50, graphics.GraphicsDevice);
        }

        public void LoadContent(ContentManager Content)
        {
            hands.LoadContent(Content);
            testBox.LoadContent(Content);
            infoPanel.LoadContent(Content);
            foreach (InfoDisplay info in displays)
            {
                info.LoadContent(Content);
            }
            healthBar.LoadContent(Content);
            font = Content.Load<SpriteFont>("Screens/Game/gameFont");

            foreach (AlienShip alien in alienShips)
            {
                alien.LoadContent(Content);
            }
            destroyedShip.LoadContent("GamePlayContent/alienExplosion", Content);
            greenLaser = Content.Load<Texture2D>("GamePlayContent/greenLaser");
        }

        public void UnloadContent()
        {

        }
        #endregion

        #region Running Methods
        public void Update(GameTime gameTime, ContentManager Content)
        {
            keyboard = Keyboard.GetState();
            if (keyboard.GetPressedKeys().Length != 0)
            {
                if(keyboard.IsKeyUp(Keys.Enter))
                {
                    timerStart = true;
                }
            }

            if (timerStart == false)
            {
                shipVerticalSpeed = 0;
            }
            else
            {
                if(timeUpTimer < 2)
                {
                    shipVerticalSpeed = 1;
                }
            }

            foreach (AlienShip alien in alienShips)
            {
                alien.Update(shipVerticalSpeed);
            }

            testBox.Position = new Vector2(alienShips[0].Position.X + 50 - 5 * testBox.Word.Length, alienShips[0].Position.Y);
            testBox.Update(gameTime);

            if (testBox.Complete)
            {
                NewTypingBox();
            }

            infoPanel.Update();
            UpdateDisplays();

            if (timerStart)
            {
                if (healthBar.Value == 0)
                {
                    timeUpTimer += timeUp;
                }

                timeUpTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (timeUpTimer > timeUp)
                {
                    timeUpTimer = 0;
                    timerStart = false;
                    ChangeTheScreen(ScreenName.Gameover);
                }
            }

            CheckAlienAttack();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (AlienShip alien in alienShips)
            {
                alien.Draw(spriteBatch);
            }

            DrawGreenLaser(spriteBatch);
            DrawDestroyedShip(spriteBatch);

            infoPanel.Draw(spriteBatch);
            foreach (InfoDisplay info in displays)
            {
                info.Draw(spriteBatch);
            }
            healthBar.Draw(spriteBatch);
            hands.Draw(spriteBatch);
            testBox.Draw(spriteBatch);

            //spriteBatch.DrawString(font, "" + (((timeUp) - (timeUpTimer)) / 1000), new Vector2(375, 475), Color.White);
        }
        #endregion

        #region Private Methods
        public void OnScreenEnter(int ChosenLevel)
        {
            PlayMusic(SoundName.TranceHouseLoop, true);
            stats.ZeroOutValues();
            stats.ZeroOutGameStats();
            testBox.Reset();
            this.ChosenLevel = ChosenLevel;
            testBox.NewBoxWord(PickRandomWord(this.ChosenLevel));
            shipVerticalSpeed = 1;

            destroyed = false;
            greenTimer = 0;
            greenFiring = false;

            healthBar.AddPercentageTotalHealth(120);
        }

        public void OnScreenExit()
        {
            PlayMusic(SoundName.TranceHouseLoop, false);
            stats.CalculateStats();
            StatTransfer(stats);

            for (int i = 0; i < 10; i++)
            {
                alienShips[i].Reset(i * -shipSpacing, shipVerticalSpeed);
            }
        }

        public void NewTypingBox()
        {
            TypingBoxStats addingStats = new TypingBoxStats();
            addingStats.AvgTimeBetweenCorrectStrokes = testBox.stats.AvgTimeBetweenCorrectStrokes;
            addingStats.NumberOfLettersInWord = testBox.stats.NumberOfLettersInWord;
            addingStats.NumberOfMistakes = testBox.stats.NumberOfMistakes;
            addingStats.PercentageCorrect = testBox.stats.PercentageCorrect;
            addingStats.Rhythm = testBox.stats.Rhythm;
            addingStats.TimeBetweenEachCorrectStroke = testBox.stats.TimeBetweenEachCorrectStroke;
            addingStats.TimeFinishedWord = testBox.stats.TimeFinishedWord;
            addingStats.TimeStartedWord = testBox.stats.TimeStartedWord;
            addingStats.TimeToCompleteWord = testBox.stats.TimeToCompleteWord;

            stats.EachWordStats.Add(addingStats);
            stats.CalculateStats();
            testBox.NewBoxWord(PickRandomWord(ChosenLevel));

            destroyed = true;
            destroyedShip.Position = alienShips[0].Position;
            destroyedShipSpeed = new Vector2(alienShips[0].Speed.X, -8);
            destroyedShip.AnimationFrame = 0;
            destroyedShip.FrameSpeed = 4;

            greenFiring = true;

            if (stats.WordsPerMinute > 30)
            {
                if (shipVerticalSpeed < 1.1f)
                {
                    shipVerticalSpeed += 0.05f;
                }

                if (shipSpacing > 75)
                {
                    shipSpacing -= 5;
                }

                for (int i = 0; i < alienShips.Count - 1; i++)
                {
                    alienShips[i].Reset(alienShips[i].Position.Y, shipVerticalSpeed);
                }
            }

            alienShips[0].Reset(alienShips[alienShips.Count - 1].Position.Y - shipSpacing, shipVerticalSpeed);
            AlienShip tempAlien = alienShips[0];
            alienShips.RemoveAt(0);
            alienShips.Add(tempAlien);
            testBox.Position = alienShips[0].Position;

            healthBar.AddPercentageTotalHealth(3);

            PlaySound(SoundType.Effect, SoundName.Explosion);
        }

        public void MakeWords()
        {
            #region Home row words
            words1.Add("aah"); 
            words1.Add("aa"); 
            words1.Add("aahs"); 
            words1.Add("aal"); 
            words1.Add("aals"); 
            words1.Add("aas"); 
            words1.Add("ad"); 
            words1.Add("add"); 
            words1.Add("adds"); 
            words1.Add("ads"); 
            words1.Add("aff"); 
            words1.Add("ag"); 
            words1.Add("aga"); 
            words1.Add("agas"); 
            words1.Add("agha"); 
            words1.Add("aghas"); 
            words1.Add("ah"); 
            words1.Add("aha"); 
            words1.Add("al"); 
            words1.Add("ala"); 
            words1.Add("alas"); 
            words1.Add("alaska"); 
            words1.Add("alaskas");
            words1.Add("alfa"); 
            words1.Add("alfalfa"); 
            words1.Add("alfalfas"); 
            words1.Add("alfas"); 
            words1.Add("alga"); 
            words1.Add("algal"); 
            words1.Add("algas"); 
            words1.Add("all"); 
            words1.Add("alls"); 
            words1.Add("als"); 
            words1.Add("as"); 
            words1.Add("ash"); 
            words1.Add("ashfall"); 
            words1.Add("ashfalls"); 
            words1.Add("ask"); 
            words1.Add("asks");
            words1.Add("dad"); 
            words1.Add("dada");
            words1.Add("dadas"); 
            words1.Add("dads");
            words1.Add("daff"); 
            words1.Add("daffs"); 
            words1.Add("dag"); 
            words1.Add("dagga"); 
            words1.Add("daggas"); 
            words1.Add("dags"); 
            words1.Add("dah"); 
            words1.Add("dahl"); 
            words1.Add("dahls"); 
            words1.Add("dahs"); 
            words1.Add("dak"); 
            words1.Add("daks"); 
            words1.Add("dal"); 
            words1.Add("dals"); 
            words1.Add("dash"); 
            words1.Add("dhak"); 
            words1.Add("dhaks");
            words1.Add("dhal"); 
            words1.Add("dhals"); 
            words1.Add("fa"); 
            words1.Add("fad"); 
            words1.Add("fads"); 
            words1.Add("fall"); 
            words1.Add("fallal"); 
            words1.Add("fallals"); 
            words1.Add("falls"); 
            words1.Add("fas"); 
            words1.Add("fash"); 
            words1.Add("flag"); 
            words1.Add("flags"); 
            words1.Add("flak"); 
            words1.Add("flash"); 
            words1.Add("flask"); 
            words1.Add("flasks"); 
            words1.Add("gad"); 
            words1.Add("gads"); 
            words1.Add("gaff"); 
            words1.Add("gaffs"); 
            words1.Add("gag"); 
            words1.Add("gaga");
            words1.Add("gags"); 
            words1.Add("gal"); 
            words1.Add("gala"); 
            words1.Add("galah"); 
            words1.Add("galahs"); 
            words1.Add("galas"); 
            words1.Add("gall"); 
            words1.Add("galls"); 
            words1.Add("gals"); 
            words1.Add("gas"); 
            words1.Add("gash"); 
            words1.Add("glad"); 
            words1.Add("glads"); 
            words1.Add("glass"); 
            words1.Add("ha"); 
            words1.Add("haaf"); 
            words1.Add("haafs"); 
            words1.Add("had");
            words1.Add("hadal"); 
            words1.Add("hadj"); 
            words1.Add("hag");
            words1.Add("haggada");
            words1.Add("haggadah");
            words1.Add("haggadahs"); 
            words1.Add("haggadas"); 
            words1.Add("hags"); 
            words1.Add("hah");
            words1.Add("haha");
            words1.Add("hahas"); 
            words1.Add("hahs"); 
            words1.Add("haj");
            words1.Add("hajj");
            words1.Add("halakah");
            words1.Add("halakahs"); 
            words1.Add("halakha");
            words1.Add("halakhas"); 
            words1.Add("halala");
            words1.Add("halalah");
            words1.Add("halalahs"); 
            words1.Add("halalas"); 
            words1.Add("half");
            words1.Add("hall");
            words1.Add("hallah");
            words1.Add("hallahs"); 
            words1.Add("halls"); 
            words1.Add("has");
            words1.Add("hash"); 
            words1.Add("jag");
            words1.Add("jagg");
            words1.Add("jaggs"); 
            words1.Add("jags"); 
            words1.Add("ka");
            words1.Add("kaas");
            words1.Add("kaf");
            words1.Add("kafs");
            words1.Add("kaka");
            words1.Add("kakas");
            words1.Add("kas");
            words1.Add("kasha"); 
            words1.Add("kashas");
            words1.Add("khaf");
            words1.Add("khafs"); 
            words1.Add("la");
            words1.Add("lad");
            words1.Add("lads");
            words1.Add("lag");
            words1.Add("lags");
            words1.Add("lakh");
            words1.Add("lakhs");
            words1.Add("lall"); 
            words1.Add("lalls");
            words1.Add("las"); 
            words1.Add("lash");
            words1.Add("lass");
            words1.Add("sad"); 
            words1.Add("sag"); 
            words1.Add("saga");
            words1.Add("sagas");
            words1.Add("sags");
            words1.Add("sal");
            words1.Add("salad");
            words1.Add("salads");
            words1.Add("salal"); 
            words1.Add("salals"); 
            words1.Add("sall"); 
            words1.Add("sals"); 
            words1.Add("salsa"); 
            words1.Add("salsas"); 
            words1.Add("sash");
            words1.Add("sass"); 
            words1.Add("sh");
            words1.Add("sha");
            words1.Add("shad");
            words1.Add("shads"); 
            words1.Add("shag");
            words1.Add("shags");
            words1.Add("shah");
            words1.Add("shahs"); 
            words1.Add("shall"); 
            words1.Add("shh");
            words1.Add("ska");
            words1.Add("skag"); 
            words1.Add("skags");
            words1.Add("skald");
            words1.Add("skalds"); 
            words1.Add("skas");
            words1.Add("slag");
            words1.Add("slags");
            words1.Add("slash");
            #endregion

            #region Upper row words
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            words2.Add("qwerty");
            #endregion

            #region Lower row words
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            words3.Add("zxcvbn");
            #endregion

            #region Copy lvl_1 words to lvl_2 and lvl_3
            foreach (string word in words1)
            {
                words2.Add(word);
                words3.Add(word);
            }
            #endregion

            #region Copy lvl_2 and lvl_3 words to lvl_4
            foreach (string word in words2)
            {
                words4.Add(word);
            }
            foreach (string word in words3)
            {
                words4.Add(word);
            }
            #endregion
        }

        public string PickRandomWord(int gameLevel)
        {
            int index = -1;

            switch (gameLevel)
            {
                case 1:
                    index = rnd.Next(0, words1.Count);
                    return words1[index];

                case 2:
                    index = rnd.Next(0, words2.Count);
                    return words2[index];

                case 3:
                    index = rnd.Next(0, words3.Count);
                    return words3[index];

                case 4:
                    index = rnd.Next(0, words4.Count);
                    return words4[index];

                default:
                    return words1[index];
            }
        }

        public void CheckAlienAttack()
        {
            if (alienShips[0].Position.Y > 325)
            {
                shipVerticalSpeed = 0.5f;
                shipSpacing = 120;

                for (int i = 0; i < alienShips.Count - 1; i++)
                {
                    alienShips[i].Reset(alienShips[i].Position.Y, shipVerticalSpeed);
                }

                alienShips[0].Reset(alienShips[alienShips.Count - 1].Position.Y - shipSpacing, shipVerticalSpeed);
                AlienShip tempAlien = alienShips[0];
                alienShips.RemoveAt(0);
                alienShips.Add(tempAlien);
                testBox.Position = alienShips[0].Position;

                healthBar.MinusPercentageTotalHealth(20);
            }
        }

        public void FingerPicker(Fingers finger)
        {
            hands.Toggle(finger);
        }

        public void WrongKeySound()
        {
            if (timerStart)
            {
                PlaySound(SoundType.Effect, SoundName.SynthBeep);

                if (shipVerticalSpeed > 0.5f)
                {
                    shipVerticalSpeed -= 0.03f;
                }

                if (shipSpacing < 100)
                {
                    shipSpacing += 3;
                }

                for (int i = 0; i < alienShips.Count - 1; i++)
                {
                    alienShips[i].Reset(alienShips[i].Position.Y, shipVerticalSpeed);
                }

                healthBar.MinusPercentageTotalHealth(2);
            }
        }

        public void DrawDestroyedShip(SpriteBatch spriteBatch)
        {
            if (destroyed)
            {
                destroyedShipSpeed += new Vector2(0, 0.5f);
                destroyedShip.Position += destroyedShipSpeed;

                destroyedShip.Draw(spriteBatch);

                if (destroyedShip.AnimationFrame > 38)
                {
                    destroyed = false;
                    destroyedShipSpeed = Vector2.Zero;
                    destroyedShip.Position = new Vector2(-100, -100);
                    destroyedShip.AnimationFrame = 0;
                }
            }
        }

        public void DrawGreenLaser(SpriteBatch spriteBatch)
        {
            if (greenFiring)
            {
                Vector2 origin = new Vector2(400, 425);
                Vector2 aiming = destroyedShip.Position + new Vector2(50, 50);
                float distance = Vector2.Distance(origin, aiming);
                Vector2 aimingN = new Vector2(aiming.X - origin.X, aiming.Y - origin.Y);
                aimingN.Normalize();
                float angle = (float)Math.Acos(Vector2.Dot(new Vector2(1, 0), aimingN));
                //angle *= ((float)Math.PI / 180);

                spriteBatch.Draw(greenLaser, origin, null, Color.White, -angle, new Vector2(0, 5), new Vector2(distance / 100, 1), SpriteEffects.None, 0);

                greenTimer++;

                if (greenTimer > 30)
                {
                    greenFiring = false;
                    greenTimer = 0;
                }
            }
        }

        public void UpdateDisplays()
        {
            displays[0].Text = "" + stats.WordsPerMinute;
            displays[1].Text = "" + stats.NumberOfWords;
            displays[2].Text = "" + (((timeUp) - (timeUpTimer)) / 1000);
            displays[3].Text = "" + stats.NumberOfMistakes;
            displays[4].Text = "" + stats.AvgRhythm;
        }
        #endregion
    }
}

