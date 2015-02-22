using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WPS.Camera
{
    public class ArcBallCamera : Camera
    {
        /// <summary>
        /// Rotation around the X axis
        /// </summary>
        public float RotationX { get; set; }

        /// <summary>
        /// Rotation around the Y axis
        /// </summary>
        public float RotationY { get; set; }

        /// <summary>
        /// The Y axis rotation minimum limit (radians)
        /// </summary>
        public float MinRotationY { get; set; }

        /// <summary>
        /// The Y axis rotation maximum limit (radians)
        /// </summary>
        public float MaxRotationY { get; set; }

        /// <summary>
        /// Distance between the target and camera
        /// </summary>
        public float Distance { get; set; }

        /// <summary>
        /// The distance minimum limit
        /// </summary>
        public float MinDistance { get; set; }

        /// <summary>
        /// The distance maximum limit
        /// </summary>
        public float MaxDistance { get; set; }

        /// <summary>
        /// The position of the camera
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// The position the camera is aiming at
        /// </summary>
        public Vector3 Target { get; set; }

        /// <summary>
        /// The up vector of the camera
        /// </summary>
        public Vector3 Up
        {
            get
            {
                Matrix rotation = Matrix.CreateFromYawPitchRoll(RotationX, -RotationY, 0);
                return Vector3.Transform(Vector3.Up, rotation);
            }
        }

        /// <summary>
        /// This type of camera is useful for inspecting a single object or scene as it rotates around a given point in 3D.
        /// </summary>
        /// <param name="Target">the target position of the camera</param>
        /// <param name="RotationX">the x rotation of the camera</param>
        /// <param name="RotationY">the y rotation of the camera</param>
        /// <param name="MinRotationY">the minimum y rotation limit</param>
        /// <param name="MaxRotationY">the maximum y rotation limit</param>
        /// <param name="Distance">the distance of the camera from the target</param>
        /// <param name="MinDistance">the minimum distance limit of the camera</param>
        /// <param name="MaxDistance">the maximum distance limit of the camera</param>
        /// <param name="graphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        public ArcBallCamera(Vector3 Target, float RotationX, float RotationY, float MinRotationY, float MaxRotationY,
            float Distance, float MinDistance, float MaxDistance, GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            this.Target = Target;
            this.MinRotationY = MinRotationY;
            this.MaxRotationY = MaxRotationY;
            // Lock the y axis rotation between the min and max values
            this.RotationY = MathHelper.Clamp(RotationY, MinRotationY,
            MaxRotationY);
            this.RotationX = RotationX;
            this.MinDistance = MinDistance;
            this.MaxDistance = MaxDistance;
            // Lock the distance between the min and max values
            this.Distance = MathHelper.Clamp(Distance, MinDistance,
            MaxDistance);
        }

        /// <summary>
        /// Basically zooms the camera in and out by a given about
        /// </summary>
        /// <param name="DistanceChange">the change in distance</param>
        public void Move(float DistanceChange)
        {
            this.Distance += DistanceChange;
            this.Distance = MathHelper.Clamp(Distance, MinDistance,
            MaxDistance);
        }

        /// <summary>
        /// Rotates the camera about a target point by a given x and y rotational change
        /// </summary>
        /// <param name="RotationXChange">the x rotation change</param>
        /// <param name="RotationYChange">the y rotation change</param>
        public void Rotate(float RotationXChange, float RotationYChange)
        {
            this.RotationX += RotationXChange;
            this.RotationY += -RotationYChange;
            this.RotationY = MathHelper.Clamp(RotationY, MinRotationY,
            MaxRotationY);
        }

        /// <summary>
        /// Moves the camera by a given amount
        /// </summary>
        /// <param name="PositionChange">the vector to add to the position of the camera</param>
        public void Translate(Vector3 PositionChange)
        {
            this.Position += PositionChange;
        }

        /// <summary>
        /// Updates the camera so that it is pointing in the correct direction and has the correct rotation
        /// </summary>
        public override void Update()
        {
            // Calculate rotation matrix from rotation values
            Matrix rotation = Matrix.CreateFromYawPitchRoll(RotationX, -
            RotationY, 0);
            // Translate down the Z axis by the desired distance
            // between the camera and object, then rotate that
            // vector to find the camera offset from the target
            Vector3 translation = new Vector3(0, 0, Distance);
            translation = Vector3.Transform(translation, rotation);
            Position = Target + translation;
            // Calculate the up vector from the rotation matrix
            Vector3 up = Vector3.Transform(Vector3.Up, rotation);
            View = Matrix.CreateLookAt(Position, Target, up);
        }
    }
}
