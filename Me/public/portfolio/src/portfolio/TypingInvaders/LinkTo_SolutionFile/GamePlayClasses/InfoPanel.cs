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
    class InfoPanel
    {
        #region Fields
        Texture2D background;
        #endregion

        #region Properties
        #endregion

        #region Start up methods
        public InfoPanel(GraphicsDevice graphics)
        {
            background = new Texture2D(graphics, 800, 200);
        }

        public void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>("GamePlayContent/panelBackground");
        }
        #endregion

        #region Running methods
        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 350), Color.White);
        }
        #endregion

        #region Private methods
        #endregion
    }
}
