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
    /// This is the tutorial screen class
    /// </summary>
    public class Tutorial
    {
        #region State change variables
        public bool Complete = false;
        public ChangeScreen ChangeTheScreen;
        #endregion

        #region Sound change variables
        public PlayASound PlaySound;
        #endregion

        #region Fields
        Button menu;
        Button play;
        SpriteFont font;
        MouseState mouse;
        KeyboardState keyboard;

        Hands hands;
        KeyboardTutorial kbTut;
        int currentLevel;
        int pageNumber;
        Keys[] level1Keys;
        string[] level1messages;
        Keys[] level2Keys;
        string[] level2messages;
        Keys[] level3Keys;
        string[] level3messages;
        Keys[] level4Keys;
        string[] level4messages;

        List<Keys> thisKeysPressed;
        List<Keys> lastKeysPressed;
        #endregion

        #region Properties
        #endregion

        #region Start Up Methods
        public Tutorial()
        {

        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            menu = new Button("Menu",   10, 515, 200, 75, graphics.GraphicsDevice);
            play = new Button("Play",   590, 515, 200, 75, graphics.GraphicsDevice);

            hands = new Hands(graphics.GraphicsDevice);
            kbTut = new KeyboardTutorial(new Vector2(150, 50), graphics.GraphicsDevice);
            currentLevel = 0;
            pageNumber = 0;

            EstablishLevels();
            thisKeysPressed = new List<Keys>();
            lastKeysPressed = new List<Keys>();
        }

        public void LoadContent(ContentManager Content)
        {
            // TO-DO: alter the stats in the font file to change the look of the font
            font = Content.Load<SpriteFont>("Screens/Tutorial/tutorialFont");

            menu.LoadContent(font, "General/Buttons/genericBttnN", "General/Buttons/genericBttnH", "General/Buttons/genericBttnC", Content);
            play.LoadContent(font, "General/Buttons/genericBttnN", "General/Buttons/genericBttnH", "General/Buttons/genericBttnC", Content);

            hands.LoadContent(Content);
            kbTut.LoadContent(Content);
        }

        public void UnloadContent()
        {

        }
        #endregion

        #region Running Methods
        public void Update(GameTime gameTime, ContentManager Content)
        {
            mouse = Mouse.GetState();

            #region Updates the menu and play buttons
            menu.Update(mouse);
            play.Update(mouse);

            if (menu.CurrentState == BttnState.FullClick)
            {
                PlaySound(SoundType.Effect, SoundName.HitElectronic);
                menu.CurrentState = BttnState.Neutral;
                ChangeTheScreen(ScreenName.Menu);
            }
            else if (play.CurrentState == BttnState.FullClick)
            {
                PlaySound(SoundType.Effect, SoundName.HitElectronic);
                play.CurrentState = BttnState.Neutral;
                ChangeTheScreen(ScreenName.Game);
            }
            #endregion

            switch (currentLevel)
            {
                case 1:
                    UpdateKeyPresses(level1Keys);
                    break;

                case 2:
                    UpdateKeyPresses(level2Keys);
                    break;

                case 3:
                    UpdateKeyPresses(level3Keys);
                    break;

                case 4:
                    UpdateKeyPresses(level4Keys);
                    break;

                default:
                    break;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
            play.Draw(spriteBatch);

            hands.Draw(spriteBatch);
            kbTut.Draw(spriteBatch);
            Vector2 textPos = new Vector2(5, 250);
            switch (currentLevel)
            {
                case 1:
                    spriteBatch.DrawString(font, level1messages[pageNumber], textPos, Color.White);
                    break;

                case 2:
                    spriteBatch.DrawString(font, level2messages[pageNumber], textPos, Color.White);
                    break;

                case 3:
                    spriteBatch.DrawString(font, level3messages[pageNumber], textPos, Color.White);
                    break;

                case 4:
                    spriteBatch.DrawString(font, level4messages[pageNumber], textPos, Color.White);
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Private Methods
        public void OnScreenEnter(int ChosenLevel)
        {
            currentLevel = ChosenLevel;
            pageNumber = 0;
            kbTut.NoHighlightedKeys();
            hands.TurnOff();

            switch (currentLevel)
            {
                case 1:
                    kbTut.AddKeyToHighlight(level1Keys[pageNumber]);
                    break;

                case 2:
                    kbTut.AddKeyToHighlight(level2Keys[pageNumber]);
                    break;

                case 3:
                    kbTut.AddKeyToHighlight(level3Keys[pageNumber]);
                    break;

                case 4:
                    kbTut.AddKeyToHighlight(level4Keys[pageNumber]);
                    break;

                default:
                    break;
            }
        }

        public void OnScreenExit()
        {
            
        }

        private void EstablishLevels()
        {
            int index = 0;

            #region Level 1
            level1Keys = new Keys[40];
            level1messages = new string[40];
            
            level1messages[index] = "The Home Row consists of these letters: A, S, D, F, G, L, K, J, and H. \n(Press Enter)";
            level1Keys[index] = Keys.Enter;
            index++;
            level1messages[index] = "The Home Row is also where you rest your fingers on the keyboard. \n(Press Enter)";
            level1Keys[index] = Keys.Enter;
            index++;
            level1messages[index] = "Try laying your fingers on the keyboard. \n(Press Enter)";
            level1Keys[index] = Keys.Enter;
            index++;
            level1messages[index] = "The left pinky goes on the letter A. \n(Press A with your left pinky)";
            level1Keys[index] = Keys.A;
            index++;
            level1messages[index] = "The left ring finger goes on the letter S. \n(Press S with your left ring finger)";
            level1Keys[index] = Keys.S;
            index++;
            level1messages[index] = "The left middle finger goes on the letter D. \n(Press D with your left middle finger)";
            level1Keys[index] = Keys.D;
            index++;
            level1messages[index] = "The left pointer goes on the letter F. \n(Press F with your left pointer)";
            level1Keys[index] = Keys.F;
            index++;
            level1messages[index] = "The left pointer is also used for the letter G. \n(Press G with your left pointer)";
            level1Keys[index] = Keys.G;
            index++;
            level1messages[index] = "The right pinky goes on the semicolon. \n(Press ; with your right pinky)";
            level1Keys[index] = Keys.OemSemicolon;
            index++;
            level1messages[index] = "The right ring finger goes on the letter L. \n(Press L with your right ring finger)";
            level1Keys[index] = Keys.L;
            index++;
            level1messages[index] = "The right middle finger goes on the letter K. \n(Press K with your right middle finger)";
            level1Keys[index] = Keys.K;
            index++;
            level1messages[index] = "The right pointer goes on the letter J. \n(Press J with your right pointer)";
            level1Keys[index] = Keys.J;
            index++;
            level1messages[index] = "The right pointer is also used for the letter H. \n(Press H with your right pointer)";
            level1Keys[index] = Keys.H;
            index++;
            level1messages[index] = "Let's try spelling a word using the Home Row. \n(Press Enter)";
            level1Keys[index] = Keys.Enter;
            index++;
            level1messages[index] = "Remember to use the correct fingers for the different letters. \n(Type the word)\n\n\n                  HALL\n\n                  ";
            level1Keys[index] = Keys.H;
            index++;
            level1messages[index] = "Remember to use the correct fingers for the different letters. \n(Type the word)\n\n\n                  HALL\n\n                  H";
            level1Keys[index] = Keys.A;
            index++;
            level1messages[index] = "Remember to use the correct fingers for the different letters. \n(Type the word)\n\n\n                  HALL\n\n                  HA";
            level1Keys[index] = Keys.L;
            index++;
            level1messages[index] = "Remember to use the correct fingers for the different letters. \n(Type the word)\n\n\n                  HALL\n\n                  HAL";
            level1Keys[index] = Keys.L;
            index++;
            level1messages[index] = "Great job!  Let's try another word. \n(Press Enter)\n\n\n                  HALL\n\n                  HALL   :)";
            level1Keys[index] = Keys.Enter;
            index++;
            level1messages[index] = "Remember to use the correct fingers for the different letters. \n(Type the word)\n\n\n                  GAS\n\n                  ";
            level1Keys[index] = Keys.G;
            index++;
            level1messages[index] = "Remember to use the correct fingers for the different letters. \n(Type the word)\n\n\n                  GAS\n\n                  G";
            level1Keys[index] = Keys.A;
            index++;
            level1messages[index] = "Remember to use the correct fingers for the different letters. \n(Type the word)\n\n\n                  GAS\n\n                  GA";
            level1Keys[index] = Keys.S;
            index++;
            level1messages[index] = "Way to go!  Let's type one more word. \n(Press Enter)\n\n\n                  GAS\n\n                  GAS   :)";
            level1Keys[index] = Keys.Enter;
            index++;
            level1messages[index] = "Remember to use the correct fingers for the different letters. \n(Type the word)\n\n\n                ALFALFA\n\n                ";
            level1Keys[index] = Keys.A;
            index++;
            level1messages[index] = "Remember to use the correct fingers for the different letters. \n(Type the word)\n\n\n                ALFALFA\n\n                A";
            level1Keys[index] = Keys.L;
            index++;
            level1messages[index] = "Remember to use the correct fingers for the different letters. \n(Type the word)\n\n\n                ALFALFA\n\n                AL";
            level1Keys[index] = Keys.F;
            index++;
            level1messages[index] = "Remember to use the correct fingers for the different letters. \n(Type the word)\n\n\n                ALFALFA\n\n                ALF";
            level1Keys[index] = Keys.A;
            index++;
            level1messages[index] = "Remember to use the correct fingers for the different letters. \n(Type the word)\n\n\n                ALFALFA\n\n                ALFA";
            level1Keys[index] = Keys.L;
            index++;
            level1messages[index] = "Remember to use the correct fingers for the different letters. \n(Type the word)\n\n\n                ALFALFA\n\n                ALFAL";
            level1Keys[index] = Keys.F;
            index++;
            level1messages[index] = "Remember to use the correct fingers for the different letters. \n(Type the word)\n\n\n                ALFALFA\n\n                ALFALF";
            level1Keys[index] = Keys.A;
            index++;
            level1messages[index] = "You did it! Now you are ready to play! \n(Press Enter)\n\n\n                ALFALFA\n\n                ALFALFA   :)";
            level1Keys[index] = Keys.Enter;
            index++;
            level1messages[index] = "Just like in this practice round, you must type the words that appear. \n(Press Enter)";
            level1Keys[index] = Keys.Enter;
            index++;
            level1messages[index] = "When you finish typing a word, you will destroy the alien ship. \n(Press Enter)";
            level1Keys[index] = Keys.Enter;
            index++;
            level1messages[index] = "Try to type as fast as you can because if you don't type the words \nfast... \n(Press Enter)";
            level1Keys[index] = Keys.Enter;
            index++;
            level1messages[index] = "...enough then you will loose health.  Loose too much health and the \ngame... \n(Press Enter)";
            level1Keys[index] = Keys.Enter;
            index++;
            level1messages[index] = "...is over!  Also, remember to type accurately because you wil loose \nhealth... \n(Press Enter)";
            level1Keys[index] = Keys.Enter;
            index++;
            level1messages[index] = "...for incorrect key presses as well.  Lastly, try to type at the same \nspeed... \n(Press Enter)";
            level1Keys[index] = Keys.Enter;
            index++;
            level1messages[index] = "...throughout the game because a good typist has good Rhythm when they \ntype. \n(Press Enter)";
            level1Keys[index] = Keys.Enter;
            index++;
            level1messages[index] = "In terms of Rhythm, it is better to type slower but smoother than \nfast and choppy. \n(Press Enter)";
            level1Keys[index] = Keys.Enter;
            index++;
            level1messages[index] = "But enough with the rules! Let's play some Typing Invaders!!! \n(Press Enter to Play)";
            level1Keys[index] = Keys.Enter;

            index = 0;
            #endregion

            #region Level 2
            index = 0;

            level2Keys = new Keys[34];
            level2messages = new string[34];

            level2messages[index] = "In this level you will learn about the Top Row. \n(Press Enter)";
            level2Keys[index] = Keys.Enter;
            index++;
            level2messages[index] = "The Top Row consists of these letters: Q, W, E, R, T, P, O, I, U, and Y. \n(Press Enter)";
            level2Keys[index] = Keys.Enter;
            index++;
            level2messages[index] = "From your Home Row position, reach your fingers up to type. \n(Press Enter)";
            level2Keys[index] = Keys.Enter;
            index++;
            level2messages[index] = "Reach your left pinky up to type Q. \n(Press Q with left pinky)";
            level2Keys[index] = Keys.Q;
            index++;
            level2messages[index] = "Reach your left ring finger up to type W. \n(Press W with left ring finger)";
            level2Keys[index] = Keys.W;
            index++;
            level2messages[index] = "Reach your left middle finger up to type E. \n(Press E with left middle finger)";
            level2Keys[index] = Keys.E;
            index++;
            level2messages[index] = "Reach your left pointer up to type R. \n(Press R with left pointer)";
            level2Keys[index] = Keys.R;
            index++;
            level2messages[index] = "Your left pointer is also used to type T. \n(Press T with left pointer)";
            level2Keys[index] = Keys.T;
            index++;
            level2messages[index] = "Reach your right pinky up to type P. \n(Press P with right pinky)";
            level2Keys[index] = Keys.P;
            index++;
            level2messages[index] = "Reach your right ring finger up to type O. \n(Press O with right ring finger)";
            level2Keys[index] = Keys.O;
            index++;
            level2messages[index] = "Reach your right middle finger up to type I. \n(Press I with right middle finger)";
            level2Keys[index] = Keys.I;
            index++;
            level2messages[index] = "Reach your right pointer up to type U. \n(Press U with right pointer)";
            level2Keys[index] = Keys.U;
            index++;
            level2messages[index] = "Your right pointer is also used to type P. \n(Press P with right pointer)";
            level2Keys[index] = Keys.Y;
            index++;
            level2messages[index] = "Ok, let's try typing some words with these letters. \n(Press Enter)";
            level2Keys[index] = Keys.Enter;
            index++;
            level2messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               POUT\n\n               ";
            level2Keys[index] = Keys.P;
            index++;
            level2messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               POUT\n\n               P";
            level2Keys[index] = Keys.O;
            index++;
            level2messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               POUT\n\n               PO";
            level2Keys[index] = Keys.U;
            index++;
            level2messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               POUT\n\n               POU";
            level2Keys[index] = Keys.T;
            index++;
            level2messages[index] = "Good job!  Let's try another word. \n(Press Enter)\n\n\n               POUT\n\n               POUT   :)";
            level2Keys[index] = Keys.Enter;
            index++;
            level2messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               QUIT\n\n               ";
            level2Keys[index] = Keys.Q;
            index++;
            level2messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               QUIT\n\n               Q";
            level2Keys[index] = Keys.U;
            index++;
            level2messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               QUIT\n\n               QU";
            level2Keys[index] = Keys.I;
            index++;
            level2messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               QUIT\n\n               QUI";
            level2Keys[index] = Keys.T;
            index++;
            level2messages[index] = "Way to go!!  Ok, one more word. \n(Press Enter)\n\n\n               QUIT\n\n               QUIT   :)";
            level2Keys[index] = Keys.Enter;
            index++;
            level2messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               TRIED\n\n               ";
            level2Keys[index] = Keys.T;
            index++;
            level2messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               TRIED\n\n               T";
            level2Keys[index] = Keys.R;
            index++;
            level2messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               TRIED\n\n               TR";
            level2Keys[index] = Keys.I;
            index++;
            level2messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               TRIED\n\n               TRI";
            level2Keys[index] = Keys.E;
            index++;
            level2messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               TRIED\n\n               TRIE";
            level2Keys[index] = Keys.D;
            index++;
            level2messages[index] = "Perfect!  You are ready to play! \n(Press Enter)\n\n\n               TRIED\n\n               TRIED   :)";
            level2Keys[index] = Keys.Enter;
            index++;
            level2messages[index] = "Just remember that you should always return your fingers to the home \nrow position after you finish typing a letter. \n(Press Enter)";
            level2Keys[index] = Keys.Enter;
            index++;
            level2messages[index] = "Also remember that you need to type as quickly an as accurately as you can.\n(Press Enter)";
            level2Keys[index] = Keys.Enter;
            index++;
            level2messages[index] = "Try to type without looking at your fingers.  If you can't remember\nwhere a letter is just look at the guide at the bottom of the screen.\n(Press Enter)";
            level2Keys[index] = Keys.Enter;
            index++;
            level2messages[index] = "Have fun and do your best!\n(Press Enter to Play)";
            level2Keys[index] = Keys.Enter;
            index++;

            index = 0;
            #endregion

            #region Level 3
            level3Keys = new Keys[31];
            level3messages = new string[31];

            level3messages[index] = "In this level you will learn about the Bottom Row. \n(Press Enter)";
            level3Keys[index] = Keys.Enter;
            index++;
            level3messages[index] = "The Bottom Row consists of these letters: Z, X, C, V, B, M, and N. \n(Press Enter)";
            level3Keys[index] = Keys.Enter;
            index++;
            level3messages[index] = "From your Home Row position, reach your fingers down to type. \n(Press Enter)";
            level3Keys[index] = Keys.Enter;
            index++;
            level3messages[index] = "Reach your left pinky down to type Z. \n(Press Z with left pinky)";
            level3Keys[index] = Keys.Z;
            index++;
            level3messages[index] = "Reach your left ring finger down to type X. \n(Press X with left ring finger)";
            level3Keys[index] = Keys.X;
            index++;
            level3messages[index] = "Reach your left middle finger down to type C. \n(Press C with left middle finger)";
            level3Keys[index] = Keys.C;
            index++;
            level3messages[index] = "Reach your left pointer down to type V. \n(Press V with left pointer)";
            level3Keys[index] = Keys.V;
            index++;
            level3messages[index] = "Your left pointer is also used to type B. \n(Press B with left pointer)";
            level3Keys[index] = Keys.B;
            index++;
            level3messages[index] = "Reach your right pointer down to type M. \n(Press M with right pointer)";
            level3Keys[index] = Keys.M;
            index++;
            level3messages[index] = "Your right pointer is also used to type N. \n(Press N with right pointer)";
            level3Keys[index] = Keys.N;
            index++;
            level3messages[index] = "Ok, let's try typing some words with these letters. \n(Press Enter)";
            level3Keys[index] = Keys.Enter;
            index++;
            level3messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               BEAN\n\n               ";
            level3Keys[index] = Keys.B;
            index++;
            level3messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               BEAN\n\n               B";
            level3Keys[index] = Keys.E;
            index++;
            level3messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               BEAN\n\n               BE";
            level3Keys[index] = Keys.A;
            index++;
            level3messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               BEAN\n\n               BEA";
            level3Keys[index] = Keys.N;
            index++;
            level3messages[index] = "Good job!  Let's try another word. \n(Press Enter)\n\n\n               BEAN\n\n               BEAN   :)";
            level3Keys[index] = Keys.Enter;
            index++;
            level3messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               CLUB\n\n               ";
            level3Keys[index] = Keys.C;
            index++;
            level3messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               CLUB\n\n               C";
            level3Keys[index] = Keys.L;
            index++;
            level3messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               CLUB\n\n               CL";
            level3Keys[index] = Keys.U;
            index++;
            level3messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               CLUB\n\n               CLU";
            level3Keys[index] = Keys.B;
            index++;
            level3messages[index] = "Way to go!!  Ok, one more word. \n(Press Enter)\n\n\n               CLUB\n\n               CLUB   :)";
            level3Keys[index] = Keys.Enter;
            index++;
            level3messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               ZEBRA\n\n               ";
            level3Keys[index] = Keys.Z;
            index++;
            level3messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               ZEBRA\n\n               Z";
            level3Keys[index] = Keys.E;
            index++;
            level3messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               ZEBRA\n\n               ZE";
            level3Keys[index] = Keys.B;
            index++;
            level3messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               ZEBRA\n\n               ZEB";
            level3Keys[index] = Keys.R;
            index++;
            level3messages[index] = "Remember to use the correct fingers for each letter. \n(Type the word)\n\n\n               ZEBRA\n\n               ZEBR";
            level3Keys[index] = Keys.A;
            index++;
            level3messages[index] = "Perfect!  You are ready to play! \n(Press Enter)\n\n\n               ZEBRA\n\n               ZEBRA   :)";
            level3Keys[index] = Keys.Enter;
            index++;
            level3messages[index] = "Just remember that you should always return your fingers to the home \nrow position after you finish typing a letter. \n(Press Enter)";
            level3Keys[index] = Keys.Enter;
            index++;
            level3messages[index] = "Also remember that you need to type as quickly an as accurately as you can.\n(Press Enter)";
            level3Keys[index] = Keys.Enter;
            index++;
            level3messages[index] = "Try to type without looking at your fingers.  If you can't remember\nwhat finger to type with just look at the guide at the bottom of the screen.\n(Press Enter)";
            level3Keys[index] = Keys.Enter;
            index++;
            level3messages[index] = "Have fun and do your best!\n(Press Enter to Play)";
            level3Keys[index] = Keys.Enter;
            index++;

            index = 0;
            #endregion

            #region Level 4
            level4Keys = new Keys[5];
            level4messages = new string[5];

            level4messages[index] = "By now you have learned all the letters on the keyboard \nand how to type them. \n(Press Enter)";
            level4Keys[index] = Keys.Enter;
            index++;

            level4messages[index] = "This is the final level.  If you can beat this level then you are \non your way to becoming a very fast and effective typist. \n(Press Enter)";
            level4Keys[index] = Keys.Enter;
            index++;

            level4messages[index] = "Just remember, don't look at your fingers when you are typing so \nthat you develop the muscle-memory and you will get \nfaster and faster with less errors. \n(Press Enter)";
            level4Keys[index] = Keys.Enter;
            index++;

            level4messages[index] = "Also, remember that Rhythm is just as important as speed and accuracy.\nThe better Rhythm you have the stronger typist you are and the \nmore you can grow. \n(Press Enter)";
            level4Keys[index] = Keys.Enter;
            index++;

            level4messages[index] = "Ok, you are ready to play! \n(Press Enter to Play)";
            level4Keys[index] = Keys.Enter;
            index++;

            index = 0;
            #endregion
        }

        private void UpdateKeyPresses(Keys[] levelKeys)
        {
            keyboard = Keyboard.GetState();

            Keys[] keysPressed = keyboard.GetPressedKeys();
            thisKeysPressed = keysPressed.ToList<Keys>();

            #region Remove keys from previous presses
            for (int i = 0; i < thisKeysPressed.Count; i++)
            {
                for (int k = 0; k < lastKeysPressed.Count; k++)
                {
                    if (thisKeysPressed[i] == lastKeysPressed[k])
                    {
                        thisKeysPressed.RemoveAt(i);
                        i--;
                        k = lastKeysPressed.Count;
                    }
                }
            }
            #endregion

            #region Check if correct key is pressed
            if (thisKeysPressed.Count == 1)
            {
                if (thisKeysPressed[0] == levelKeys[pageNumber])
                {
                    PlaySound(SoundType.Effect, SoundName.HitElectronic);
                    pageNumber++;
                    if (pageNumber > levelKeys.Length - 1)
                    {
                        ChangeTheScreen(ScreenName.Game);
                    }
                    else
                    {
                        kbTut.NoHighlightedKeys();
                        kbTut.AddKeyToHighlight(levelKeys[pageNumber]);
                        UpdateHands(levelKeys[pageNumber]);
                    }
                }
                else
                {
                    PlaySound(SoundType.Effect, SoundName.Clap);
                }
            }
            else if (thisKeysPressed.Count > 1)
            {
                PlaySound(SoundType.Effect, SoundName.Clap);
            }
            #endregion

            lastKeysPressed = keysPressed.ToList<Keys>();
        }

        private void UpdateHands(Keys key)
        {
            hands.TurnOff();
            if (key == Keys.A || key == Keys.Q || key == Keys.Z)
            {
                hands.Toggle(Fingers.Left_Pinky);
            }
            else if (key == Keys.S || key == Keys.W || key == Keys.X)
            {
                hands.Toggle(Fingers.Left_Ring);
            }
            else if (key == Keys.D || key == Keys.E || key == Keys.C)
            {
                hands.Toggle(Fingers.Left_Middle);
            }
            else if (key == Keys.F || key == Keys.R || key == Keys.V ||
                     key == Keys.G || key == Keys.T || key == Keys.B)
            {
                hands.Toggle(Fingers.Left_Pointer);
            }
            else if (key == Keys.J || key == Keys.U || key == Keys.M ||
                     key == Keys.H || key == Keys.Y || key == Keys.N)
            {
                hands.Toggle(Fingers.Right_Pointer);
            }
            else if (key == Keys.K || key == Keys.I || key == Keys.OemComma)
            {
                hands.Toggle(Fingers.Right_Middle);
            }
            else if (key == Keys.L || key == Keys.O || key == Keys.OemPeriod)
            {
                hands.Toggle(Fingers.Right_Ring);
            }
            else if (key == Keys.OemSemicolon || key == Keys.P || key == Keys.OemBackslash)
            {
                hands.Toggle(Fingers.Right_Pinky);
            }
        }
        #endregion
    }
}

