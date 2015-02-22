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
    class KeyboardInputBox
    {
        #region Fields
        KeyboardState keyboard;
        string word;
        string typedWord;
        string seenWord;
        Vector2 position;
        char nextLetter;
        Keys nextKey;
        bool complete;
        List<Keys> thisKeysPressed;
        List<Keys> lastKeysPressed;
        int gradeScale;
        SpriteFont font;

        Vector2 lpinky =    new Vector2(120, 440);
        Vector2 lring =     new Vector2(160, 390);
        Vector2 lmid =      new Vector2(225, 380);
        Vector2 lpoint =    new Vector2(280, 410);
        Vector2 lthumb =    new Vector2(330, 520);
        Vector2 rthumb =    new Vector2(450, 520);
        Vector2 rpoint =    new Vector2(500, 410);
        Vector2 rmid =      new Vector2(555, 380);
        Vector2 rring =     new Vector2(620, 390);
        Vector2 rpinky =    new Vector2(660, 440);


        public ChangeFingerHighlight FingerHighlight;
        public WrongKeySound WrongKey;
        #endregion

        #region Stats
        public TypingBoxStats stats;
        float betweenLetterTimer = 0;
        float elapsedGameTimeFromStart = 0;
        #endregion

        #region Properties
        public bool Complete
        {
            get { return complete; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public string Word
        {
            get { return seenWord; }
        }
        #endregion

        #region Start up methods
        public KeyboardInputBox()
        {
            word = "";
            typedWord = "";
            seenWord = "";
            position = Vector2.Zero;
            nextLetter = '1';
            nextKey = Keys.Zoom;
            complete = true;
            thisKeysPressed = new List<Keys>();
            lastKeysPressed = new List<Keys>();
            gradeScale = 0;

            stats = new TypingBoxStats();
        }

        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Screens/Game/gameFont");
        }
        #endregion

        #region Running methods
        public int Update(GameTime gameTime)
        {
            elapsedGameTimeFromStart += gameTime.ElapsedGameTime.Milliseconds;
            if (!complete)
            {
                betweenLetterTimer += gameTime.ElapsedGameTime.Milliseconds;

                gradeScale = 0;

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
                    if (thisKeysPressed[0] == nextKey)
                    {
                        gradeScale = 1;
                        CorrectKeyPress(gameTime);
                    }
                    else
                    {
                        gradeScale = -1;
                        stats.NumberOfMistakes += 1;
                        WrongKey();
                    }
                }
                else if (thisKeysPressed.Count > 1)
                {
                    gradeScale = -1;
                    stats.NumberOfMistakes += 1;
                    WrongKey();
                }
                #endregion

                lastKeysPressed = keysPressed.ToList<Keys>();
            }

            return gradeScale;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!complete)
            {
                spriteBatch.DrawString(font, seenWord, position, Color.White);
                spriteBatch.DrawString(font, typedWord, position, Color.GreenYellow);

                DrawNextLetterOnFingers(spriteBatch);
            }
        }
        #endregion

        #region Public methods
        public void NewBoxWord(string word)
        {
            stats.ZeroOutValues();

            this.word = word;
            typedWord = "";
            seenWord = word;
            nextLetter = word[0];
            complete = false;
            nextKey = GetKeyFromChar(nextLetter);

            stats.NumberOfLettersInWord = word.Length;
            FingerHighlight(GetFingerFromKey(nextKey));
        }

        public Keys GetKeyFromChar(char chrcter)
        {
            Keys newKey = Keys.Zoom;

            switch (chrcter)
            {
                case 'a':
                    newKey = Keys.A;
                    break;

                case 'b':
                    newKey = Keys.B;
                    break;

                case 'c':
                    newKey = Keys.C;
                    break;

                case 'd':
                    newKey = Keys.D;
                    break;

                case 'e':
                    newKey = Keys.E;
                    break;

                case 'f':
                    newKey = Keys.F;
                    break;

                case 'g':
                    newKey = Keys.G;
                    break;

                case 'h':
                    newKey = Keys.H;
                    break;

                case 'i':
                    newKey = Keys.I;
                    break;

                case 'j':
                    newKey = Keys.J;
                    break;

                case 'k':
                    newKey = Keys.K;
                    break;

                case 'l':
                    newKey = Keys.L;
                    break;

                case 'm':
                    newKey = Keys.M;
                    break;

                case 'n':
                    newKey = Keys.N;
                    break;

                case 'o':
                    newKey = Keys.O;
                    break;

                case 'p':
                    newKey = Keys.P;
                    break;

                case 'q':
                    newKey = Keys.Q;
                    break;

                case 'r':
                    newKey = Keys.R;
                    break;

                case 's':
                    newKey = Keys.S;
                    break;

                case 't':
                    newKey = Keys.T;
                    break;

                case 'u':
                    newKey = Keys.U;
                    break;

                case 'v':
                    newKey = Keys.V;
                    break;

                case 'w':
                    newKey = Keys.W;
                    break;

                case 'x':
                    newKey = Keys.X;
                    break;

                case 'y':
                    newKey = Keys.Y;
                    break;

                case 'z':
                    newKey = Keys.Z;
                    break;

                case ';':
                    newKey = Keys.OemSemicolon;
                    break;

                case ',':
                    newKey = Keys.OemComma;
                    break;

                case '.':
                    newKey = Keys.OemPeriod;
                    break;

                default:
                    newKey = Keys.Zoom;
                    break;
            }

            return newKey;
        }

        public Fingers GetFingerFromKey(Keys key)
        {
            if (key == Keys.A || key == Keys.Q || key == Keys.Z)
            {
                return Fingers.Left_Pinky;
            }
            else if (key == Keys.S || key == Keys.W || key == Keys.X)
            {
                return Fingers.Left_Ring;
            }
            else if (key == Keys.D || key == Keys.E || key == Keys.C)
            {
                return Fingers.Left_Middle;
            }
            else if (key == Keys.F || key == Keys.R || key == Keys.V ||
                     key == Keys.G || key == Keys.T || key == Keys.B)
            {
                return Fingers.Left_Pointer;
            }
            else if (key == Keys.J || key == Keys.U || key == Keys.M ||
                     key == Keys.H || key == Keys.Y || key == Keys.N)
            {
                return Fingers.Right_Pointer;
            }
            else if (key == Keys.K || key == Keys.I || key == Keys.OemComma)
            {
                return Fingers.Right_Middle;
            }
            else if (key == Keys.L || key == Keys.O || key == Keys.OemPeriod)
            {
                return Fingers.Right_Ring;
            }
            else if (key == Keys.OemSemicolon || key == Keys.P || key == Keys.OemBackslash)
            {
                return Fingers.Right_Pinky;
            }
            else
            {
                return Fingers.Both_Thumbs;
            }
        }

        public void CorrectKeyPress(GameTime gameTime)
        {
            typedWord += word[0];

            if (typedWord.Length == 1)
            {
                stats.TimeStartedWord = elapsedGameTimeFromStart;
                betweenLetterTimer = 0;
            }
            else if (typedWord.Length == seenWord.Length)
            {
                stats.TimeBetweenEachCorrectStroke.Add(betweenLetterTimer);
                betweenLetterTimer = 0;
                stats.TimeFinishedWord = elapsedGameTimeFromStart;
                stats.CalculateStats();
            }
            else
            {
                stats.TimeBetweenEachCorrectStroke.Add(betweenLetterTimer);
                betweenLetterTimer = 0;
            }

            string wordSwitch = "";
            for (int i = 1; i < word.Length; i++)
            {
                wordSwitch += word[i];
            }
            word = wordSwitch;

            if (word.Length == 0)
            {
                complete = true;
            }
            else
            {
                nextLetter = word[0];
                nextKey = GetKeyFromChar(nextLetter);
                FingerHighlight(GetFingerFromKey(nextKey));
            }
        }

        public void Reset()
        {
            stats.ZeroOutValues();
            elapsedGameTimeFromStart = 0;
            betweenLetterTimer = 0;
        }

        public void DrawNextLetterOnFingers(SpriteBatch spriteBatch)
        {
            Fingers fingerPos = GetFingerFromKey(nextKey);
            Vector2 pos = Vector2.Zero;

            switch (fingerPos)
            {
                case Fingers.Left_Pinky:
                    pos = lpinky;
                    break;

                case Fingers.Left_Ring:
                    pos = lring;
                    break;

                case Fingers.Left_Middle:
                    pos = lmid;
                    break;

                case Fingers.Left_Pointer:
                    pos = lpoint;
                    break;

                case Fingers.Left_Thumb:
                    pos = lthumb;
                    break;

                case Fingers.Right_Thumb:
                    pos = rthumb;
                    break;

                case Fingers.Right_Pointer:
                    pos = rpoint;
                    break;

                case Fingers.Right_Middle:
                    pos = rmid;
                    break;

                case Fingers.Right_Ring:
                    pos = rring;
                    break;

                case Fingers.Right_Pinky:
                    pos = rpinky;
                    break;
            }

            spriteBatch.DrawString(font, "" + nextKey, pos, Color.DarkRed);
        }
        #endregion
    }
}
