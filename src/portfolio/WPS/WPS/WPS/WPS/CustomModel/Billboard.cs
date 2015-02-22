using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WPS.CustomModel
{
    public class Billboard
    {
        private VertexPositionNormalTexture[] verts;
        private GraphicsDevice myDevice;
        private short[] ib = null;
        private Effect billboardEffect;
        
        /// <summary>
        /// The width of the billboard
        /// </summary>
        public float Width;

        /// <summary>
        /// The height of the billboard
        /// </summary>
        public float Height;

        /// <summary>
        /// A billboard is a 3D quad that rotates based on the camera position so that the quad is always directly facing the camera
        /// </summary>
        /// <param name="center">the center of the billboard</param>
        /// <param name="width">the width of the billboard</param>
        /// <param name="height">the height of the billboard</param>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        /// <param name="Content">just use the Content parameter that is contained within the Game1 class</param>
        public Billboard(Vector3 center, float width, float height, GraphicsDevice GraphicsDevice, ContentManager Content)
        {
            myDevice = GraphicsDevice;
            Width = width;
            Height = height;
            billboardEffect = Content.Load<Effect>(Global_Variables_WPS.ContentEffects + "BillboardEffect");

            verts = new VertexPositionNormalTexture[]
                        {
                            new VertexPositionNormalTexture(
                                center,
                                new Vector3(0,0,0),
                                new Vector2(1,0)),
                            new VertexPositionNormalTexture(
                                center,
                                new Vector3(0,0,0),
                                new Vector2(1,1)),
                            new VertexPositionNormalTexture(
                                center,
                                new Vector3(0,0,0),
                                new Vector2(0,1)),
                            new VertexPositionNormalTexture(
                                center,
                                new Vector3(0,0,0),
                                new Vector2(0,0))
                        };

            ib = new short[] { 0, 1, 2, 2, 3, 0 };
        }

        /// <summary>
        /// Sets the center of the billboard
        /// </summary>
        /// <param name="center">the center of the billboard</param>
        public void SetVertexCenter(Vector3 center)
        {
            Vector3 normal = Vector3.Up;

            verts[0].Position = center;
            verts[0].Normal = normal;

            verts[1].Position = center;
            verts[1].Normal = normal;

            verts[2].Position = center;
            verts[2].Normal = normal;

            verts[3].Position = center;
            verts[3].Normal = normal;
        }

        /// <summary>
        /// Draws the billboard with the given texture
        /// </summary>
        /// <param name="texture">the texture to place on the billboard</param>
        /// <param name="View">the view matrix of the camera</param>
        /// <param name="Projection">the projection matrix of the camera</param>
        /// <param name="camUp">the camera's Up vector</param>
        public void DrawBillboard(Texture2D texture, Matrix View, Matrix Projection, Vector3 camUp)
        {
            camUp.Normalize();
            billboardEffect.Parameters["View"].SetValue(View);
            billboardEffect.Parameters["Projection"].SetValue(Projection);
            billboardEffect.Parameters["Normal"].SetValue(camUp);
            billboardEffect.Parameters["Texture"].SetValue(texture);
            billboardEffect.Parameters["Width"].SetValue(Width);
            billboardEffect.Parameters["Height"].SetValue(Height);
            billboardEffect.CurrentTechnique.Passes[0].Apply();

            myDevice.DrawUserIndexedPrimitives
                (PrimitiveType.TriangleList, verts, 0, 4, ib, 0, 2);
        }
    }
}
