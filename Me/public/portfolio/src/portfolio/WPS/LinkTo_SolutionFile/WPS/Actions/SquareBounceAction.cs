using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WPS.CustomModel;

namespace WPS
{
    public class SquareBounceAction : WPSAction
    {
        public Vector3 PlanePoint;
        public Vector3 Normal;
        public float Width;
        public float Height;
        public float Dampening;
        public float TimeStep;

        private GraphicsDevice GraphicsDevice;

        /// <summary>
        /// This is a particle system action that can be permanently, or for a single frame, applied to a particle system.
        /// 
        /// The purpose of this action is to provide the particles in a particle system a square-shaped-3D surface to bounce off of.
        /// </summary>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        /// <param name="Content">just use the Content parameter that is contained within the Game1 class</param>
        /// <param name="planePoint">the center point of the square in 3D space</param>
        /// <param name="normal">the normal of the square shape (the vector that is perpendicular to the surface of the shape)</param>
        /// <param name="width">the width of the square shape</param>
        /// <param name="height">the height of the square shape</param>
        /// <param name="dampening">the percentage of "force" contained after the bounce (1 means the particle will bounce as high as from where it was dropped onto a flat surface, 0.5 means it will bounce half as high)</param>
        /// <param name="timeStep">the amount of arbitrary time that this action takes to complete (1 means the action takes place at full speed, 0.5 means the action takes place at half-speed)</param>
        public SquareBounceAction(GraphicsDevice GraphicsDevice, ContentManager Content, Vector3 planePoint, Vector3 normal, float width, float height, float dampening = 1, float timeStep = 1)
        {
            this.PlanePoint = planePoint;
            this.Normal = normal;
            this.Width = width;
            this.Height = height;
            this.Dampening = dampening;
            this.TimeStep = timeStep;
            this.GraphicsDevice = GraphicsDevice;

            effect = Content.Load<Effect>(Global_Variables_WPS.ContentActionEffects + "SquareBounce");
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
            effect.Parameters["Width"].SetValue(Width);
            effect.Parameters["Height"].SetValue(Height);
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
            for (int i = 0; i < velocity.Length; i++)
            {
                Vector3 pos = new Vector3(position[i].X, position[i].Y, position[i].Z);
                Vector3 vel = new Vector3(velocity[i].X, velocity[i].Y, velocity[i].Z);


                if (Vector3.Dot(Normal, vel * TimeStep) != 0)
                {
                    float s1 = Vector3.Dot(-Normal, pos - PlanePoint) / Vector3.Dot(Normal, vel * TimeStep);

                    if (s1 >= 0 && 1 >= s1)
                    {
                        Vector3 secondVect = Vector3.Up;
                        if ((secondVect - Normal).Length() == 0)
                            secondVect = Vector3.Right;

                        Vector3 right = Vector3.Cross(Normal, secondVect);
                        Vector3 up = Vector3.Cross(Normal, right);
                        float hW = Width / 2;
                        float hH = Height / 2;

                        Vector3 topRight = (right * hW) + (up * hH);
                        Vector3 topLeft = (right * -hW) + (up * hH);
                        Vector3 botRight = (right * hW) + (up * -hH);
                        Vector3 botLeft = (right * -hW) + (up * -hH);

                        Vector3 v0 = topRight;
                        Vector3 v1 = topLeft;
                        Vector3 v2 = botRight;
                        Vector3 p0 = pos;
                        Vector3 p1 = pos + (vel * TimeStep);
                        Vector3 pi = pos + (vel * TimeStep * s1);
                        Vector3 n = Normal;
                        Vector3 u = v1 - v0;
                        Vector3 v = v2 - v0;
                        Vector3 w = pi - v0;

                        float si = ((Vector3.Dot(u, v) * Vector3.Dot(w, v)) - (Vector3.Dot(v, v) * Vector3.Dot(w, u))) / ((Vector3.Dot(u, v) * Vector3.Dot(u, v)) - (Vector3.Dot(u, u) * Vector3.Dot(v, v)));
                        float ti = ((Vector3.Dot(u, v) * Vector3.Dot(w, u)) - (Vector3.Dot(u, u) * Vector3.Dot(w, v))) / ((Vector3.Dot(u, v) * Vector3.Dot(u, v)) - (Vector3.Dot(u, u) * Vector3.Dot(v, v)));

                        if (si < 0 || ti < 0 || si + ti > 1)
                        {
                            v0 = topLeft;
                            v1 = botRight;
                            v2 = botLeft;
                            u = v1 - v0;
                            v = v2 - v0;
                            w = pi - v0;

                            si = ((Vector3.Dot(u, v) * Vector3.Dot(w, v)) - (Vector3.Dot(v, v) * Vector3.Dot(w, u))) / ((Vector3.Dot(u, v) * Vector3.Dot(u, v)) - (Vector3.Dot(u, u) * Vector3.Dot(v, v)));
                            ti = ((Vector3.Dot(u, v) * Vector3.Dot(w, u)) - (Vector3.Dot(u, u) * Vector3.Dot(w, v))) / ((Vector3.Dot(u, v) * Vector3.Dot(u, v)) - (Vector3.Dot(u, u) * Vector3.Dot(v, v)));

                            if (si >= 0 && ti >= 0 && si + ti <= 1)
                            {
                                Vector3 reflectedVelocity = (-2 * Vector3.Dot(Normal, vel * TimeStep) * Normal) + (vel * TimeStep);

                                velocity[i] = new Vector4(reflectedVelocity * Dampening, 0);
                            }
                        }
                        else
                        {
                            Vector3 reflectedVelocity = (-2 * Vector3.Dot(Normal, vel * TimeStep) * Normal) + (vel * TimeStep);

                            velocity[i] = new Vector4(reflectedVelocity * Dampening, 0);
                        }
                    }
                }
            }
        }
    }
}
