using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WPS.Camera
{
    public class TargetCamera : Camera
    {
        /// <summary>
        /// The 3D position of the camera
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// The 3D position that the camera is pointing at
        /// </summary>
        public Vector3 Target { get; set; }

        /// <summary>
        /// The Up vector for the camera
        /// </summary>
        public Vector3 Up
        {
            get
            {
                Vector3 forward = Target - Position;
                Vector3 side = Vector3.Cross(forward, Vector3.Up);
                return Vector3.Cross(forward, side);
            }
        }

        /// <summary>
        /// This type of camera takes a position and aims at a given target
        /// </summary>
        /// <param name="Position">the position of the camera</param>
        /// <param name="Target">the position that the camera will point at</param>
        /// <param name="graphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        public TargetCamera(Vector3 Position, Vector3 Target, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.Position = Position;
            this.Target = Target;
        }

        /// <summary>
        /// This updates the camera so that it is looking at the correct target
        /// </summary>
        public override void Update()
        {
            Vector3 forward = Target - Position;
            Vector3 side = Vector3.Cross(forward, Vector3.Up);
            Vector3 up = Vector3.Cross(forward, side);
            this.View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}
