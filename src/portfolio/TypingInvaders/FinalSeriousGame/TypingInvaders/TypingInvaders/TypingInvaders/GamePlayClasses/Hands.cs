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
    public enum Fingers
    {
        Left_Pinky,
        Left_Ring,
        Left_Middle,
        Left_Pointer,
        Left_Thumb,
        Right_Pinky,
        Right_Ring,
        Right_Middle,
        Right_Pointer,
        Right_Thumb,
        Both_Thumbs,
    }

    class Hands
    {
        #region Fields
        Texture2D hands;
        Vector2 position;
        SpriteStrip rightHighlight;
        SpriteStrip leftHighlight;
        bool rightOn;
        bool leftOn;
        #endregion

        #region Start up methods
        public Hands(GraphicsDevice graphics)
        {
            hands = new Texture2D(graphics, 800, 200);
            position = new Vector2(0, 405);
            rightHighlight = new SpriteStrip(250, 200, 1250, 200, 5, 5, graphics);
            leftHighlight = new SpriteStrip(250, 200, 1250, 200, 5, 5, graphics);

            rightHighlight.AnimationActive = false;
            leftHighlight.AnimationActive = false;

            rightHighlight.Position = new Vector2(position.X + 450, position.Y);
            leftHighlight.Position = new Vector2(position.X + 100, position.Y);

            rightOn = false;
            leftOn = false;
        }

        public void LoadContent(ContentManager Content)
        {
            hands = Content.Load<Texture2D>("GamePlayContent/alienHands");
            rightHighlight.LoadContent("GamePlayContent/rightHighlight", Content);
            leftHighlight.LoadContent("GamePlayContent/leftHighlight", Content);
        }
        #endregion

        #region Running methods
        public void Draw(SpriteBatch spriteBatch)
        {
            if (rightOn)
            {
                rightHighlight.Draw(spriteBatch);
            }

            if (leftOn)
            {
                leftHighlight.Draw(spriteBatch);
            }

            spriteBatch.Draw(hands, position, Color.White);
        }
        #endregion

        #region Public methods
        public bool Toggle(Fingers finger)
        {
            switch (finger)
            {
                #region Left fingers
                case Fingers.Left_Pinky:
                    leftHighlight.AnimationFrame = 0;
                    leftOn = true;
                    rightOn = false;
                    return leftOn;
                    

                case Fingers.Left_Ring:
                    leftHighlight.AnimationFrame = 1;
                    leftOn = true;
                    rightOn = false;
                    return leftOn;
                    

                case Fingers.Left_Middle:
                    leftHighlight.AnimationFrame = 2;
                    leftOn = true;
                    rightOn = false;
                    return leftOn;
                    

                case Fingers.Left_Pointer:
                    leftHighlight.AnimationFrame = 3;
                    leftOn = true;
                    rightOn = false;
                    return leftOn;
                    

                case Fingers.Left_Thumb:
                    leftHighlight.AnimationFrame = 4;
                    leftOn = true;
                    rightOn = false;
                    return leftOn;
                    
                #endregion

                #region Right fingers
                case Fingers.Right_Pinky:
                    rightHighlight.AnimationFrame = 4;
                    rightOn = true;
                    leftOn = false;
                    return rightOn;
                    

                case Fingers.Right_Ring:
                    rightHighlight.AnimationFrame = 3;
                    rightOn = true;
                    leftOn = false;
                    return rightOn;
                    

                case Fingers.Right_Middle:
                    rightHighlight.AnimationFrame = 2;
                    rightOn = true;
                    leftOn = false;
                    return rightOn;
                    

                case Fingers.Right_Pointer:
                    rightHighlight.AnimationFrame = 1;
                    rightOn = true;
                    leftOn = false;
                    return rightOn;
                    

                case Fingers.Right_Thumb:
                    rightHighlight.AnimationFrame = 0;
                    rightOn = true;
                    leftOn = false;
                    return rightOn;
                    
                #endregion

                default:
                    return false;
            }


        }

        public void TurnOff()
        {
            leftOn = false;
            rightOn = false;
        }
        #endregion
    }
}
