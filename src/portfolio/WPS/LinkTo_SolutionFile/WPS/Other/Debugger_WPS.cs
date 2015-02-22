using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WPS
{
    public class Debugger_WPS
    {
        /// <summary>
        /// The font color
        /// </summary>
        public Color Color;

        /// <summary>
        /// The debugger font
        /// </summary>
        public SpriteFont Font;

        private GraphicsDevice GraphicsDevice;
        private ContentManager Content;

        private int frameIndex;
        private float[] frameCounter;
        private float frameRate;

        private int particleCounter;

        /// <summary>
        /// A debugger for the WPS particle system
        /// </summary>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        /// <param name="Content">just use the Content parameter that is contained within the Game1 class</param>
        public Debugger_WPS(GraphicsDevice GraphicsDevice, ContentManager Content)
        {
            this.GraphicsDevice = GraphicsDevice;
            this.Content = Content;

            Font = Content.Load<SpriteFont>(Global_Variables_WPS.ContentOther + "DebugFont");
            frameCounter = new float[25];
            frameIndex = 0;

            Color = Color.White;
        }

        /// <summary>
        /// Displays a frames per second counter in the given position
        /// 
        /// *** Does NOT Begin and End the spriteBatch ***
        /// </summary>
        /// <param name="spriteBatch">the spriteBatch</param>
        /// <param name="gameTime">the game time</param>
        /// <param name="pos">position to draw the data at</param>
        public void DisplayFPSCounter(SpriteBatch spriteBatch, GameTime gameTime, Vector2 pos)
        {
            frameCounter[frameIndex] = 1 / ((float)gameTime.ElapsedGameTime.Milliseconds * 0.001f);

            for (int i = 0; i < frameCounter.Length; i++)
                frameRate += frameCounter[i];
            frameRate /= (float)frameCounter.Length;

            spriteBatch.DrawString(Font, "FPS: " + (int)frameRate, pos, Color);

            frameIndex++;
            if (frameIndex >= frameCounter.Length)
                frameIndex = 0;
        }

        /// <summary>
        /// Displays a particle count for a given particle system at the given position
        /// 
        /// *** Does NOT Begin and End the spriteBatch ***
        /// </summary>
        /// <param name="spriteBatch">the spriteBatch</param>
        /// <param name="tps">the particle system to draw</param>
        /// <param name="pos">the position to draw the data at</param>
        public void DisplayParticleCount(SpriteBatch spriteBatch, ParticleSystem tps, Vector2 pos)
        {
            string count = "" + tps.GetActiveParticleCount();
            string readableCount = "";
            int commaCounter = 0;
            for (int i = count.Length - 1; i >= 0; i--)
            {
                if ((readableCount.Length - commaCounter) % 3 == 0 && readableCount.Length != 0)
                {
                    readableCount = count[i] + "," + readableCount;
                    commaCounter++;
                }
                else
                    readableCount = count[i] + readableCount;
            }

            spriteBatch.DrawString(Font, "Active Particles: " + readableCount, pos, Color);
        }

        /// <summary>
        /// Displays a particle count for a given particle system at the given position
        /// 
        /// *** Does NOT Begin and End the spriteBatch ***
        /// </summary>
        /// <param name="spriteBatch">the spriteBatch</param>
        /// <param name="tps">the particle system to draw</param>
        /// <param name="pos">the position to draw the data at</param>
        public void DisplayParticleCount(SpriteBatch spriteBatch, ParticleSystemCPU tps, Vector2 pos)
        {
            string count = "" + tps.GetActiveParticleCount();
            string readableCount = "";
            int commaCounter = 0;
            for (int i = count.Length - 1; i >= 0; i--)
            {
                if ((readableCount.Length - commaCounter) % 3 == 0 && readableCount.Length != 0)
                {
                    readableCount = count[i] + "," + readableCount;
                    commaCounter++;
                }
                else
                    readableCount = count[i] + readableCount;
            }

            spriteBatch.DrawString(Font, "Active Particles: " + readableCount, pos, Color);
        }
    }
}
