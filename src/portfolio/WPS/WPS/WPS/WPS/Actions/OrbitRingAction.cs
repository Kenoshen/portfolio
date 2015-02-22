using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WPS.CustomModel;

namespace WPS
{
    public class OrbitRingAction : WPSAction
    {
        public Vector3 Center;
        public Vector3 Normal;
        public float Radius;
        public float MaxRange;
        public float Magnitude;
        public float Epsilon;
        public float TimeStep;

        private GraphicsDevice GraphicsDevice;

        /// <summary>
        /// This is a particle system action that can be permanently, or for a single frame, applied to a particle system.
        /// 
        /// The purpose of this action is to attract/repel particles in a particle system to/from a given ring-shaped line.
        /// </summary>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        /// <param name="Content">just use the Content parameter that is contained within the Game1 class</param>
        /// <param name="center">the 3D point where the center of the ring is</param>
        /// <param name="normal">the normal of the ring (perpendicular to the ring)</param>
        /// <param name="radius">the radius of the ring from its center</param>
        /// <param name="maxRange">the radius of the donut-shaped influence of this action</param>
        /// <param name="magnitude">the "strength" of the pulling/pushing force</param>
        /// <param name="epsilon">the drop off rate for particles, this gives the gravitational force a more realistic look, particles closer to the line are acted on stronger than particles at the edge of the range of influence</param>
        /// <param name="timeStep">the amount of arbitrary time that this action takes to complete (1 means the action takes place at full speed, 0.5 means the action takes place at half-speed)</param>
        public OrbitRingAction(GraphicsDevice GraphicsDevice, ContentManager Content, Vector3 center, Vector3 normal, float radius,  
            float maxRange = 5, float magnitude = 1, float epsilon = 1, float timeStep = 1)
        {
            this.Center = center;
            this.Normal = normal;
            this.Radius = radius;
            this.MaxRange = maxRange;
            this.Magnitude = magnitude;
            this.Epsilon = epsilon;
            this.TimeStep = timeStep;
            this.GraphicsDevice = GraphicsDevice;

            effect = Content.Load<Effect>(Global_Variables_WPS.ContentActionEffects + "OrbitRing");
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
            //Normal.Normalize();
            //effect.Parameters["Center"].SetValue(Center);
            //effect.Parameters["Normal"].SetValue(Normal);
            //effect.Parameters["Radius"].SetValue(Radius);
            //effect.Parameters["MaxRange"].SetValue(MaxRange);
            //effect.Parameters["Magnitude"].SetValue(Magnitude);
            //effect.Parameters["Epsilon"].SetValue(Epsilon);
            //effect.Parameters["TimeStep"].SetValue(TimeStep);
            //effect.Parameters["Position"].SetValue(position.CurrentTexture);
            //effect.Parameters["Velocity"].SetValue(velocity.CurrentTexture);
            //velocity.DrawDataToTexture(effect, quad);

            Vector4[] vel = new Vector4[velocity.CurrentTexture.Width * velocity.CurrentTexture.Height];
            velocity.CurrentTexture.GetData(vel);
            Vector4[] pos = new Vector4[velocity.CurrentTexture.Width * velocity.CurrentTexture.Height];
            position.CurrentTexture.GetData(pos);
            Normal.Normalize();

            for (int i = 0; i < vel.Length; i++)
            {
                Vector3 pPosition = new Vector3(pos[i].X, pos[i].Y, pos[i].Z);
                Vector3 pPos = pPosition - Center;

                Vector3 cross = Vector3.Cross(pPos, Normal);
                Vector3 point = Vector3.Cross(Normal, cross);
                point.Normalize();

                point *= Radius;

                float currentRange = Vector3.Distance(pPosition, point);
                float force = 0;
                if (currentRange <= MaxRange)
                    force = Magnitude / ((currentRange * currentRange) + Epsilon);

                Vector4 addVel = new Vector4(((Center + point) - pPosition) * force, 0);
                Vector4 pVelocity = vel[i];
                vel[i] = pVelocity + addVel;
            }

            velocity.SetTextureData(vel);
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
            // not implemented yet
        }
    }
}
