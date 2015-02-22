using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WPS.CustomModel;

namespace WPS
{
    public class KillAllAction : WPSAction
    {
        private GraphicsDevice GraphicsDevice;

        /// <summary>
        /// This is a particle system action that can be permanently, or for a single frame, applied to a particle system.
        /// 
        /// The purpose of this action is to de-activate all the particles in a particle system.
        /// </summary>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        /// <param name="Content">just use the Content parameter that is contained within the Game1 class</param>
        public KillAllAction(GraphicsDevice GraphicsDevice, ContentManager Content)
        {
            this.GraphicsDevice = GraphicsDevice;

            effect = Content.Load<Effect>(Global_Variables_WPS.ContentActionEffects + "KillAll");
        }

        /// <summary>
        /// Calling this method will put the action into effect.
        /// </summary>
        /// <param name="position">this is the data texture that holds the positions of all the particles in the particle system</param>
        /// <param name="velocity">this is the data texture that holds the velocities of all the particles in the particle system</param>
        /// <param name="data">this is the data texture that holds the data (as in alpha, size, and age) of all the particles in the particle system</param>
        /// <param name="maxAge">this is the maximum age of the particles in the particle system</param>
        /// <param name="quad">this is a quad that is designed to provide a place to capture the data from the GPU when the action alters any data texture</param>
        public override void ApplyAction(DataTexture position, DataTexture velocity, DataTexture data, float maxAge, Quad quad)
        {
            effect.Parameters["Data"].SetValue(data.CurrentTexture);
            data.DrawDataToTexture(effect, quad);
        }

        /// <summary>
        /// Calling this method will put the action into effect.
        /// </summary>
        /// <param name="position">this is the data array that holds the positions of all the particles in the particle system</param>
        /// <param name="velocity">this is the data array that holds the velocities of all the particles in the particle system</param>
        /// <param name="data">this is the data array that holds the data (as in alpha, size, and age) of all the particles in the particle system</param>
        /// <param name="maxAge">this is the maximum age of the particles in the particle system</param>
        public override void ApplyActionCPU(Vector4[] position, Vector4[] velocity, Vector4[] data, float maxAge)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i].Z = -1;
            }
        }
    }
}
