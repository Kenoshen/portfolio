using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Winger.Camera
{
    public abstract class Camera
    {
        Matrix view;
        Matrix projection;

        public Matrix View
        {
            get { return projection; }
            protected set
            {
                projection = value;
                generateFrustum();
            }
        }
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
        public BoundingFrustum Frustum { get; private set; }

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

        public virtual void Update()
        {

        }

        public bool BoundingVolumeIsInView(BoundingSphere sphere)
        {
            return (Frustum.Contains(sphere) != ContainmentType.Disjoint);
        }

        public bool BoundingVolumeIsInView(BoundingBox box)
        {
            return (Frustum.Contains(box) != ContainmentType.Disjoint);
        }
    }
}
