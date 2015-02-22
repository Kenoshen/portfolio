using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WPS.CustomModel;

namespace WPS
{
    public class PlaneBounceAction : WPSAction
    {
        public Vector3 PlanePoint;
        public Vector3 Normal;
        public float Dampening;
        public float TimeStep;

        private GraphicsDevice GraphicsDevice;

        /// <summary>
        /// This is a particle system action that can be permanently, or for a single frame, applied to a particle system.
        /// 
        /// The purpose of this action is to provide the particles in the particle system an infinite plane to bounce off of.
        /// </summary>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        /// <param name="Content">just use the Content parameter that is contained within the Game1 class</param>
        /// <param name="planePoint">a point that lies on the plane</param>
        /// <param name="normal">the normal of the plane (perpendicular vector to the surface of the plane)</param>
        /// <param name="dampening">the percentage of "force" contained after the bounce (1 means the particle will bounce as high as from where it was dropped onto a flat surface, 0.5 means it will bounce half as high)</param>
        /// <param name="timeStep">the amount of arbitrary time that this action takes to complete (1 means the action takes place at full speed, 0.5 means the action takes place at half-speed)</param>
        public PlaneBounceAction(GraphicsDevice GraphicsDevice, ContentManager Content, Vector3 planePoint, Vector3 normal, float dampening = 1, float timeStep = 1)
        {
            this.PlanePoint = planePoint;
            this.Normal = normal;
            this.Dampening = dampening;
            this.TimeStep = timeStep;
            this.GraphicsDevice = GraphicsDevice;

            effect = Content.Load<Effect>(Global_Variables_WPS.ContentActionEffects + "PlaneBounce");
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

            effect.Parameters["PlanePoint"].SetValue(new Vector4(PlanePoint, 0));
            effect.Parameters["Normal"].SetValue(new Vector4(Normal, 0));
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

            Vector4 planePoint = new Vector4(PlanePoint, 0);
            Vector4 normal = new Vector4(Normal, 0);

            for (int i = 0; i < velocity.Length; i++)
            {
                if (Vector4.Dot(normal, velocity[i] * TimeStep) != 0)
                {
                    float s1 = (Vector4.Dot(-normal, position[i] - planePoint)) / (Vector4.Dot(normal, velocity[i] * TimeStep));

                    if (s1 >= 0 && 1 >= s1)
                    {
                        Vector4 reflectedVelocity = (-2 * Vector4.Dot(normal, velocity[i] * TimeStep) * normal) + (velocity[i] * TimeStep);

                        velocity[i] = reflectedVelocity * Dampening;
                    }
                }
            }
        }
    }
}
