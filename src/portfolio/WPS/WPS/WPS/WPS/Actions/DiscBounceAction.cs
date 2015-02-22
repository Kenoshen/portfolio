using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WPS.CustomModel;

namespace WPS
{
    public class DiscBounceAction : WPSAction
    {
        public Vector3 Center;
        public Vector3 Normal;
        public float Radius;
        public float Dampening;
        public float TimeStep;

        private GraphicsDevice GraphicsDevice;

        /// <summary>
        /// This is a particle system action that can be permanently, or for a single frame, applied to a particle system.
        /// 
        /// The purpose of this action is to provide the particles in a particle system a disc-shaped-3D surface for particles to bounce off of.
        /// </summary>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        /// <param name="Content">just use the Content parameter that is contained within the Game1 class</param>
        /// <param name="center">the center point of the disc</param>
        /// <param name="normal">the normal vector of the disc (perpendicular to the surface of the disc)</param>
        /// <param name="radius">the radius of the disc</param>
        /// <param name="dampening">the percentage of "force" contained after the bounce (1 means the particle will bounce as high as from where it was dropped onto a flat surface, 0.5 means it will bounce half as high)</param>
        /// <param name="timeStep">the amount of arbitrary time that this action takes to complete (1 means the action takes place at full speed, 0.5 means the action takes place at half-speed)</param>
        public DiscBounceAction(GraphicsDevice GraphicsDevice, ContentManager Content, Vector3 center, Vector3 normal, float radius, float dampening = 1, float timeStep = 1)
        {
            this.Center = center;
            this.Normal = normal;
            this.Radius = radius;
            this.Dampening = dampening;
            this.TimeStep = timeStep;
            this.GraphicsDevice = GraphicsDevice;

            effect = Content.Load<Effect>(Global_Variables_WPS.ContentActionEffects + "DiscBounce");
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
            Normal.Normalize();

            effect.Parameters["Center"].SetValue(new Vector4(Center, 0));
            effect.Parameters["Normal"].SetValue(new Vector4(Normal, 0));
            effect.Parameters["Radius"].SetValue(Radius);
            effect.Parameters["Dampening"].SetValue(Dampening);
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
            Normal.Normalize();
            Vector4 Norm = new Vector4(Normal, 0);
            Vector4 Cent = new Vector4(Center, 0);
            for (int i = 0; i < data.Length; i++)
            {
                if (Vector4.Distance(position[i], Cent) <= Radius &&
                    Vector4.Dot(Norm, velocity[i] * TimeStep) != 0)
                {
                    float s1 = (Vector4.Dot(-Norm, position[i] - Cent)) / (Vector4.Dot(Norm, velocity[i] * TimeStep));

                    if (s1 >= 0 && 1 >= s1)
                    {
                        velocity[i] = ((-2 * Vector4.Dot(Norm, velocity[i] * TimeStep) * Norm) + (velocity[i] * TimeStep)) * Dampening;
                    }
                }

            }
        }
    }
}
