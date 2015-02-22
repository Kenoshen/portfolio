using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WPS.CustomModel;

namespace WPS
{
    public abstract class WPSAction
    {
        protected Effect effect;

        /// <summary>
        /// This is used by the particle system and is called when the action is added to the particle system.
        /// </summary>
        /// <param name="position">the data texture for the position of the particles</param>
        /// <param name="velocity">the data texture for the velocity of the particles</param>
        /// <param name="data">the data texture for the data values associated with the particles</param>
        /// <param name="maxAge">the max age of the particles</param>
        public virtual void ActionAdded(Random rnd, DataTexture position, DataTexture velocity, DataTexture data, float maxAge)
        {
            
        }

        /// <summary>
        /// This is used by the particle system and is called when the action is removed from the particle system.
        /// </summary>
        /// <param name="position">the data texture for the position of the particles</param>
        /// <param name="velocity">the data texture for the velocity of the particles</param>
        /// <param name="data">the data texture for the data values associated with the particles</param>
        /// <param name="maxAge">the max age of the particles</param>
        public virtual void ActionRemoved(Random rnd, DataTexture position, DataTexture velocity, DataTexture data, float maxAge)
        {

        }

        /// <summary>
        /// Calling this method will put the action into effect.
        /// </summary>
        /// <param name="position">this is the data texture that holds the positions of all the particles in the particle system</param>
        /// <param name="velocity">this is the data texture that holds the velocities of all the particles in the particle system</param>
        /// <param name="data">this is the data texture that holds the data (as in alpha, size, and age) of all the particles in the particle system</param>
        /// <param name="maxAge">this is the maximum age of the particles in the particle system</param>
        /// <param name="quad">this is a quad that is designed to provide a place to capture the data from the GPU when the action alters any data texture</param>
        public virtual void ApplyAction(DataTexture position, DataTexture velocity, DataTexture data, float maxAge, Quad quad)
        {

        }

        /// <summary>
        /// This is used by the particle system and is called when the action is added to the particle system.
        /// </summary>
        /// <param name="position">the data array for the position of the particles</param>
        /// <param name="velocity">the data array for the velocity of the particles</param>
        /// <param name="data">the data array for the data values associated with the particles</param>
        /// <param name="maxAge">the max age of the particles</param>
        public virtual void ActionAddedCPU(Random rnd, Vector4[] position, Vector4[] velocity, Vector4[] data, float maxAge)
        {

        }

        /// <summary>
        /// This is used by the particle system and is called when the action is removed from the particle system.
        /// </summary>
        /// <param name="position">the data array for the position of the particles</param>
        /// <param name="velocity">the data array for the velocity of the particles</param>
        /// <param name="data">the data array for the data values associated with the particles</param>
        /// <param name="maxAge">the max age of the particles</param>
        public virtual void ActionRemovedCPU(Random rnd, Vector4[] position, Vector4[] velocity, Vector4[] data, float maxAge)
        {

        }

        /// <summary>
        /// Calling this method will put the action into effect.
        /// </summary>
        /// <param name="position">this is the data array that holds the positions of all the particles in the particle system</param>
        /// <param name="velocity">this is the data array that holds the velocities of all the particles in the particle system</param>
        /// <param name="data">this is the data array that holds the data (as in alpha, size, and age) of all the particles in the particle system</param>
        /// <param name="maxAge">this is the maximum age of the particles in the particle system</param>
        public virtual void ApplyActionCPU(Vector4[] position, Vector4[] velocity, Vector4[] data, float maxAge)
        {

        }
    }
}
