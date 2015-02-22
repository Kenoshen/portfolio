using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WPS.Camera
{
    public class ChaseCamera : Camera
    {
        /// <summary>
        /// The 3D position of the camera
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// The 3D position that the camera is aiming at
        /// </summary>
        public Vector3 Target { get; private set; }

        /// <summary>
        /// The position of the follow target
        /// </summary>
        public Vector3 FollowTargetPosition { get; private set; }

        /// <summary>
        /// The rotation of the follow target
        /// </summary>
        public Vector3 FollowTargetRotation { get; private set; }

        /// <summary>
        /// The offset of position of the camera
        /// </summary>
        public Vector3 PositionOffset { get; set; }

        /// <summary>
        /// The offset on the target position of the camera
        /// </summary>
        public Vector3 TargetOffset { get; set; }

        /// <summary>
        /// The camera rotation relative to the camera's up-right rotation
        /// </summary>
        public Vector3 RelativeCameraRotation { get; set; }

        /// <summary>
        /// The Up vector of the camera
        /// </summary>
        public Vector3 Up
        {
            get
            {
                // Sum the rotations of the model and the camera to ensure it
                // is rotated to the correct position relative to the model's
                // rotation
                Vector3 combinedRotation = FollowTargetRotation + RelativeCameraRotation;

                // Calculate the rotation matrix for the camera
                Matrix rotation = Matrix.CreateFromYawPitchRoll(combinedRotation.Y, combinedRotation.X, combinedRotation.Z);

                // Obtain the up vector from the matrix
                return Vector3.Transform(Vector3.Up, rotation);
            }
        }

        float springiness = 0.15f;

        /// <summary>
        /// The "springiness" is the amount that the camera can "fudge" when following a target
        /// </summary>
        public float Springiness
        {
            get { return springiness; }
            set { springiness = MathHelper.Clamp(value, 0, 1); }
        }

        /// <summary>
        /// This type of camera is useful for 3rd person games where the camera is following behind the playerand rotates with the character
        /// </summary>
        /// <param name="PositionOffset">the relative position of the camera behind the target</param>
        /// <param name="TargetOffset">the relative offset from the target</param>
        /// <param name="RelativeCameraRotation">the relative camera rotation</param>
        /// <param name="graphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        public ChaseCamera(Vector3 PositionOffset, Vector3 TargetOffset, Vector3 RelativeCameraRotation, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.PositionOffset = PositionOffset;
            this.TargetOffset = TargetOffset;
            this.RelativeCameraRotation = RelativeCameraRotation;
        }

        /// <summary>
        /// This moves the camera so that it is following a given target
        /// </summary>
        /// <param name="NewFollowTargetPosition">the newest position of the target</param>
        /// <param name="NewFollowTargetRotation">the newest rotation of the target</param>
        public void Move(Vector3 NewFollowTargetPosition, Vector3 NewFollowTargetRotation)
        {
            this.FollowTargetPosition = NewFollowTargetPosition;
            this.FollowTargetRotation = NewFollowTargetRotation;
        }

        /// <summary>
        /// Rotates the camera relative to the camera
        /// </summary>
        /// <param name="RotationChange">relative rotation change</param>
        public void Rotate(Vector3 RotationChange)
        {
            this.RelativeCameraRotation += RotationChange;
        }

        /// <summary>
        /// This updates the camera so that it is looking in the right direction with the right rotation
        /// </summary>
        public override void Update()
        {
            // Sum the rotations of the model and the camera to ensure it
            // is rotated to the correct position relative to the model's
            // rotation
            Vector3 combinedRotation = FollowTargetRotation + RelativeCameraRotation;

            // Calculate the rotation matrix for the camera
            Matrix rotation = Matrix.CreateFromYawPitchRoll(combinedRotation.Y, combinedRotation.X, combinedRotation.Z);

            // Calculate the position the camera would be without the spring
            // value, using the rotation matrix and target position
            Vector3 desiredPosition = FollowTargetPosition + Vector3.Transform(PositionOffset, rotation);

            // Interpolate between the current position and desired position
            Position = Vector3.Lerp(Position, desiredPosition, Springiness);

            // Calculate the new target using the rotation matrix
            Target = FollowTargetPosition + Vector3.Transform(TargetOffset, rotation);

            // Obtain the up vector from the matrix
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);

            // Recalculate the view matrix
            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}
