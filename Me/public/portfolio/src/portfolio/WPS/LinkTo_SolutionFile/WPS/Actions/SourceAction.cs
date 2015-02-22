using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WPS.CustomModel;

namespace WPS
{
    public class SourceAction : WPSAction
    {
        public float Particle_Rate;
        public float TimeStep;
        public float Size;
        public Domain PositionDomain;
        public Domain VelocityDomain;
        private GraphicsDevice GraphicsDevice;
        private RenderTarget2D tempTex;
        private Vector4[] tempVect;
        private List<int> indexes;
        private float internal_P_Rate;

        /// <summary>
        /// This is a particle system action that can be permanently, or for a single frame, applied to a particle system.
        /// 
        /// The purpose of this action is to spawn/emit new particles from a given position domain with a given velocity domain and a given size and rate.
        /// </summary>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        /// <param name="Content">just use the Content parameter that is contained within the Game1 class</param>
        /// <param name="posDomain">this is a domain that defines the starting positions of the particles that are emitted</param>
        /// <param name="velDomain">this is a domain that defines the starting velocities of the particles that are emitted</param>
        /// <param name="size">this is the scaler size of the particle (1 means a 1x1 sized particle, 0.5 means a 0.5x0.5 sized particle)</param>
        /// <param name="particle_rate">this is the rate at which new particles are emitted (1 means 1 particle emitted every time the action is applied, 5 means 5 particles are emitted)</param>
        /// <param name="timeStep">the amount of arbitrary time that this action takes to complete (1 means the action takes place at full speed, 0.5 means the action takes place at half-speed)</param>
        public SourceAction(GraphicsDevice GraphicsDevice, ContentManager Content, Domain posDomain, Domain velDomain, float size = 1, float particle_rate = 1, float timeStep = 1)
        {
            this.Particle_Rate = particle_rate;
            this.TimeStep = timeStep;
            this.Size = size;
            this.PositionDomain = posDomain;
            this.VelocityDomain = velDomain;
            this.GraphicsDevice = GraphicsDevice;
            internal_P_Rate = 0;

            effect = Content.Load<Effect>(Global_Variables_WPS.ContentActionEffects + "Source");
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
            internal_P_Rate += Particle_Rate * TimeStep;
            if (internal_P_Rate >= 1)
            {
                internal_P_Rate = (int)internal_P_Rate;
                tempTex = new RenderTarget2D(GraphicsDevice, position.CurrentTexture.Width, position.CurrentTexture.Height, false, SurfaceFormat.Vector4, DepthFormat.None);
                tempVect = new Vector4[tempTex.Width * tempTex.Height];
                indexes = new List<int>();

                data.CurrentTexture.GetData(tempVect);
                for (int i = 0; i < tempVect.Length; i++)
                {
                    if (tempVect[i].Z == -1)
                    {
                        indexes.Add(i);
                        tempVect[i].X = 1; // alpha
                        tempVect[i].Y = Size; // size
                        tempVect[i].Z = 0; // age
                        // does not need to initialize the rotation because it is already initialized by a rotation action

                        if (indexes.Count >= internal_P_Rate)
                            break;
                    }
                }
                tempTex.SetData(tempVect);
                effect.Parameters["From"].SetValue(tempTex);
                data.DrawDataToTexture(effect, quad);


                tempTex = new RenderTarget2D(GraphicsDevice, position.CurrentTexture.Width, position.CurrentTexture.Height, false, SurfaceFormat.Vector4, DepthFormat.None);
                position.CurrentTexture.GetData(tempVect);
                for (int i = 0; i < indexes.Count; i++)
                    tempVect[indexes[i]] = new Vector4(PositionDomain.GetRandomVectorInDomain(), 0);
                tempTex.SetData(tempVect);
                effect.Parameters["From"].SetValue(tempTex);
                position.DrawDataToTexture(effect, quad);


                tempTex = new RenderTarget2D(GraphicsDevice, position.CurrentTexture.Width, position.CurrentTexture.Height, false, SurfaceFormat.Vector4, DepthFormat.None);
                velocity.CurrentTexture.GetData(tempVect);
                for (int i = 0; i < indexes.Count; i++)
                {
                    if (VelocityDomain != null)
                        tempVect[indexes[i]] = new Vector4(VelocityDomain.GetRandomVectorInDomain(), 0);
                    else
                        tempVect[indexes[i]] = Vector4.Zero;
                }
                tempTex.SetData(tempVect);
                effect.Parameters["From"].SetValue(tempTex);
                velocity.DrawDataToTexture(effect, quad);

                internal_P_Rate = 0;
            }
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
            internal_P_Rate += Particle_Rate * TimeStep;
            if (internal_P_Rate >= 1)
            {
                internal_P_Rate = (int)internal_P_Rate;
                indexes = new List<int>();

                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i].Z == -1)
                    {
                        indexes.Add(i);
                        data[i].X = 1; // alpha
                        data[i].Y = Size; // size
                        data[i].Z = 0; // age
                        // does not need to initialize the rotation because it is already initialized by a rotation action

                        if (indexes.Count >= internal_P_Rate)
                            break;
                    }
                }

                for (int i = 0; i < indexes.Count; i++)
                    position[indexes[i]] = new Vector4(PositionDomain.GetRandomVectorInDomain(), 0);

                for (int i = 0; i < indexes.Count; i++)
                {
                    if (VelocityDomain != null)
                        velocity[indexes[i]] = new Vector4(VelocityDomain.GetRandomVectorInDomain(), 0);
                    else
                        velocity[indexes[i]] = Vector4.Zero;
                }

                internal_P_Rate = 0;
            }
        }
    }
}
