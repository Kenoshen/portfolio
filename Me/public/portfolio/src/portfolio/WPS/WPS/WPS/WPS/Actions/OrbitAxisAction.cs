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
using WPS.CustomModel;

namespace WPS
{
    public class OrbitAxisAction : WPSAction
    {
        public Vector3 StartPoint;
        public Vector3 EndPoint;
        public float MaxRange;
        public float Magnitude;
        public float Epsilon;
        public float TimeStep;

        private GraphicsDevice GraphicsDevice;

        /// <summary>
        /// This is a particle system action that can be permanently, or for a single frame, applied to a particle system.
        /// 
        /// The purpose of this action is to attract/repel particles in a particle system to/from a given axis.
        /// </summary>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        /// <param name="Content">just use the Content parameter that is contained within the Game1 class</param>
        /// <param name="startPoint">a point lying on the desired axis</param>
        /// <param name="endPoint">a second point lying on the desired axis (cannot be the same as the first)</param>
        /// <param name="maxRange">the radius of the cylinder-shaped influence of this action</param>
        /// <param name="magnitude">the "strength" of the pulling/pushing force</param>
        /// <param name="epsilon">the drop off rate for particles, this gives the gravitational force a more realistic look, particles closer to the axis are acted on stronger than particles at the edge of the range of influence</param>
        /// <param name="timeStep">the amount of arbitrary time that this action takes to complete (1 means the action takes place at full speed, 0.5 means the action takes place at half-speed)</param>
        public OrbitAxisAction(GraphicsDevice GraphicsDevice, ContentManager Content, Vector3 startPoint, Vector3 endPoint,  
            float maxRange = 5, float magnitude = 1, float epsilon = 1, float timeStep = 1)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.MaxRange = maxRange;
            this.Magnitude = magnitude;
            this.Epsilon = epsilon;
            this.TimeStep = timeStep;
            this.GraphicsDevice = GraphicsDevice;

            effect = Content.Load<Effect>(Global_Variables_WPS.ContentActionEffects + "OrbitAxis");
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
            effect.Parameters["StartPoint"].SetValue(new Vector4(StartPoint, 0));
            effect.Parameters["EndPoint"].SetValue(new Vector4(EndPoint, 0));
            effect.Parameters["MaxRange"].SetValue(MaxRange);
            effect.Parameters["Magnitude"].SetValue(Magnitude);
            effect.Parameters["Epsilon"].SetValue(Epsilon);
            effect.Parameters["TimeStep"].SetValue(TimeStep);
            effect.Parameters["Position"].SetValue(position.CurrentTexture);
            effect.Parameters["Velocity"].SetValue(velocity.CurrentTexture);
            velocity.DrawDataToTexture(effect, quad);
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
            Vector4 endP = new Vector4(EndPoint, 0);
            Vector4 staP = new Vector4(StartPoint, 0);

            for (int i = 0; i < velocity.Length; i++)
            {
                Vector4 V = endP - staP;
                Vector4 W = position[i] - staP;

                float c1 = Vector4.Dot(W, V);
                float c2 = Vector4.Dot(V, V);
                float b = c1 / c2;

                Vector4 Pb = staP + (V * b);

                float currentRange = Math.Abs(Vector4.Distance(position[i], Pb));
                float force = 0;

                if (currentRange < MaxRange)
                    force = Magnitude / ((currentRange * currentRange) + Epsilon);

                velocity[i] += ((Pb - position[i]) * force);
            }
        }
    }
}
