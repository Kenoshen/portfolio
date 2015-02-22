using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WPS.Camera
{
    public class FreeCamera : Camera
    {
        /// <summary>
        /// The yaw of the camera (similar to the Y rotation)
        /// </summary>
        public float Yaw { get; set; }

        /// <summary>
        /// The pitch of the camera (similar to the X rotation)
        /// </summary>
        public float Pitch { get; set; }

        /// <summary>
        /// The 3D position of the camera
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// THe 3D position the camera is aimed at
        /// </summary>
        public Vector3 Target { get; private set; }

        /// <summary>
        /// The Up vector of the camera
        /// </summary>
        public Vector3 Up
        {
            get
            {
                // Calculate the rotation matrix
                Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);

                // Calculate the up vector
                return Vector3.Transform(Vector3.Up, rotation);
            }
        }

        private Vector3 translation;

        /// <summary>
        /// This type of camera is good for a free-flying camera such as one used in a 3D map editor
        /// </summary>
        /// <param name="Position">position of the camera</param>
        /// <param name="Yaw">the yaw rotation of the camera</param>
        /// <param name="Pitch">the pitch rotation of the camera</param>
        /// <param name="graphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        public FreeCamera(Vector3 Position, float Yaw, float Pitch, GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            this.Position = Position;
            this.Yaw = Yaw;
            this.Pitch = Pitch;

            translation = Vector3.Zero;
        }

        /// <summary>
        /// This rotates the camera given a yaw and a pitch rotation change
        /// </summary>
        /// <param name="YawChange">the change in yaw rotation</param>
        /// <param name="PitchChange">the change in pitch rotation</param>
        public void Rotate(float YawChange, float PitchChange)
        {
            this.Yaw += YawChange;
            this.Pitch += PitchChange;
        }

        /// <summary>
        /// This moves the camera in the given direction by the given amount
        /// </summary>
        /// <param name="Translation">the vector to add to the position</param>
        public void Move(Vector3 Translation)
        {
            this.translation += Translation;
        }

        /// <summary>
        /// This updates the camera so that it is looking in the correct direction with the correct rotations
        /// </summary>
        public override void Update()
        {
            // Calculate the rotation matrix
            Matrix rotation = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);
            
            // Offset the position and reset the translation
            translation = Vector3.Transform(translation, rotation);
            Position += translation;
            translation = Vector3.Zero;

            // Calculate the new target
            Vector3 forward = Vector3.Transform(Vector3.Forward, rotation);
            Target = Position + forward;

            // Calculate the up vector
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            // Calculate the view matrix
            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}
