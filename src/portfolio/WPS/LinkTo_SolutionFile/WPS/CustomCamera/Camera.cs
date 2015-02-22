using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WPS.Camera
{
    public abstract class Camera
    {
        Matrix view;
        Matrix projection;

        /// <summary>
        /// The view matrix for the camera
        /// </summary>
        public Matrix View
        {
            get { return projection; }
            protected set
            {
                projection = value;
                generateFrustum();
            }
        }

        /// <summary>
        /// The projection matrix for the camera
        /// </summary>
        public Matrix Projection
        {
            get { return view; }
            protected set
            {
                view = value;
                generateFrustum();
            }
        }

        private void generateFrustum()
        {
            Matrix viewProjection = View * Projection;
            Frustum = new BoundingFrustum(viewProjection);
        }

        protected GraphicsDevice GraphicsDevice { get; set; }

        /// <summary>
        /// The bounding frustrum for the camera
        /// </summary>
        public BoundingFrustum Frustum { get; private set; }

        /// <summary>
        /// This is the generic camera class that is used by other camera types like: TargetCamera, FreeCamera, ChaseCamera, and ArcBallCamera
        /// </summary>
        /// <param name="graphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        public Camera(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            generatePerspectiveProjectionMatrix(MathHelper.PiOver4);
        }

        private void generatePerspectiveProjectionMatrix(float FieldOfView)
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;

            float aspectRatio = (float)pp.BackBufferWidth / (float)pp.BackBufferHeight;

            this.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), aspectRatio, 0.1f, 1000000.0f);
        }

        /// <summary>
        /// Updates the camera
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// Checks if a bounding sphere is within the view of the camera
        /// </summary>
        /// <param name="sphere">the bounding sphere to check</param>
        /// <returns>true if the bounding sphere is within the view of the camera</returns>
        public bool BoundingVolumeIsInView(BoundingSphere sphere)
        {
            return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
        }

        /// <summary>
        /// Checks if a bounding box is within the view of the camera
        /// </summary>
        /// <param name="sphere">the bounding box to check</param>
        /// <returns>true if the bounding box is within the view of the camera</returns>
        public bool BoundingVolumeIsInView(BoundingBox box)
        {
            return (Frustum.Contains(box) != ContainmentType.Disjoint);
        }
    }
}
