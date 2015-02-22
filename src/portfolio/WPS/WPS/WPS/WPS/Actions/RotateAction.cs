using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WPS.CustomModel;

namespace WPS
{
    public class RotateAction : WPSAction
    {
        private float MinRotRevs;
        private float MaxRotRevs;
        private GraphicsDevice GraphicsDevice;

        /// <summary>
        /// This is a particle system action that can be permanently, or for a single frame, applied to a particle system.
        /// 
        /// The purpose of this action is to rotate the particles in a particle system by a random amount.
        /// </summary>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        /// <param name="Content">just use the Content parameter that is contained within the Game1 class</param>
        /// <param name="minRotRevs">this is the minumum number of revolutions that the particle will make during it's lifetime</param>
        /// <param name="maxRotRevs">this is the minumum number of revolutions that the particle will make during it's lifetime</param>
        /// <param name="ageStep">the amount of arbitrary time that this action takes to complete (1 means the action takes place at full speed, 0.5 means the action takes place at half-speed)</param>
        public RotateAction(GraphicsDevice GraphicsDevice, ContentManager Content, float minRotRevs, float maxRotRevs)
        {
            this.MinRotRevs = minRotRevs;
            this.MaxRotRevs = maxRotRevs;
            this.GraphicsDevice = GraphicsDevice;
        }

        /// <summary>
        /// This is used by the particle system and is called when the action is added to the particle system.
        /// </summary>
        /// <param name="rnd">the random object</param>
        /// <param name="position">the data array for the position of the particles</param>
        /// <param name="velocity">the data array for the velocity of the particles</param>
        /// <param name="data">the data array for the data values associated with the particles</param>
        /// <param name="maxAge">the max age of the particles</param>
        public override void ActionAdded(Random rnd, DataTexture position, DataTexture velocity, DataTexture data, float maxAge)
        {
            Vector4[] dataValues = new Vector4[data.CurrentTexture.Width * data.CurrentTexture.Height];
            data.CurrentTexture.GetData(dataValues);

            for (int i = 0; i < dataValues.Length; i++)
                dataValues[i].W = rnd.Next((int)(MinRotRevs * 10000f), (int)(MaxRotRevs * 10000f)) / 10000f;

            data.CurrentTexture.SetData(dataValues);
        }

        /// <summary>
        /// This is used by the particle system and is called when the action is added to the particle system.
        /// </summary>
        /// <param name="rnd">the random object</param>
        /// <param name="position">the data texture for the position of the particles</param>
        /// <param name="velocity">the data texture for the velocity of the particles</param>
        /// <param name="data">the data texture for the data values associated with the particles</param>
        /// <param name="maxAge">the max age of the particles</param>
        public override void ActionRemoved(Random rnd, DataTexture position, DataTexture velocity, DataTexture data, float maxAge)
        {
            Vector4[] dataValues = new Vector4[data.CurrentTexture.Width * data.CurrentTexture.Height];
            data.CurrentTexture.GetData(dataValues);

            for (int i = 0; i < dataValues.Length; i++)
                dataValues[i].W = 0;

            data.CurrentTexture.SetData(dataValues);
        }

        /// <summary>
        /// This is used by the particle system and is called when the action is added to the particle system.
        /// </summary>
        /// <param name="position">the data array for the position of the particles</param>
        /// <param name="velocity">the data array for the velocity of the particles</param>
        /// <param name="data">the data array for the data values associated with the particles</param>
        /// <param name="maxAge">the max age of the particles</param>
        public override void ActionAddedCPU(Random rnd, Vector4[] position, Vector4[] velocity, Vector4[] data, float maxAge)
        {
            for (int i = 0; i < data.Length; i++)
                data[i].W = rnd.Next((int)(MinRotRevs * 10000f), (int)(MaxRotRevs * 10000f)) / 10000f;
        }

        /// <summary>
        /// This is used by the particle system and is called when the action is removed from the particle system.
        /// </summary>
        /// <param name="position">the data array for the position of the particles</param>
        /// <param name="velocity">the data array for the velocity of the particles</param>
        /// <param name="data">the data array for the data values associated with the particles</param>
        /// <param name="maxAge">the max age of the particles</param>
        public override void ActionRemovedCPU(Random rnd, Vector4[] position, Vector4[] velocity, Vector4[] data, float maxAge)
        {
            for (int i = 0; i < data.Length; i++)
                data[i].W = 0;
        }
    }
}
