using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WPS
{
    class ParticleCollectionCPU
    {
        private VertexPositionTexture[] verts;
        private GraphicsDevice myDevice;
        private short[] ib = null;
        private Texture2D texture;
        private Effect effect;

        /// <summary>
        /// This class stores a collection of verticies to be used to draw many particles at once
        /// </summary>
        /// <param name="size">the width or length of the data texture (must be a perfect square texture ie. width == height)</param>
        /// <param name="texture">the particle display texture</param>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        /// <param name="Content">just use the Content parameter that is contained within the Game1 class</param>
        public ParticleCollectionCPU(Texture2D texture, GraphicsDevice GraphicsDevice, ContentManager Content)
        {
            myDevice = GraphicsDevice;
            this.effect = Content.Load<Effect>(Global_Variables_WPS.ContentEffects + "DrawParticleCollectionCPU");
            this.texture = texture;

            verts = new VertexPositionTexture[4];
            ib = new short[6];

            verts[0] = new VertexPositionTexture(
                            new Vector3(0, 0, 0),
                            new Vector2(0, 1));
            verts[1] = new VertexPositionTexture(
                            new Vector3(0, 0, 0),
                            new Vector2(1, 1));
            verts[2] = new VertexPositionTexture(
                            new Vector3(0, 0, 0),
                            new Vector2(1, 0));
            verts[3] = new VertexPositionTexture(
                            new Vector3(0, 0, 0),
                            new Vector2(0, 0));

            ib[0] = 0;
            ib[1] = 1;
            ib[2] = 2;
            ib[3] = 2;
            ib[4] = 3;
            ib[5] = 0;
        }

        /// <summary>
        /// Draws the collection of verticies in one call to the GPU
        /// </summary>
        /// <param name="View">the view matrix of the camera</param>
        /// <param name="Projection">the projection matrix of the camera</param>
        /// <param name="camUp">the camera's Up vector</param>
        /// <param name="positionTexture">the textrue containing position data</param>
        /// <param name="dataTexture">the textrue containing alpha, size, and age data</param>
        /// <param name="ageLimit">the age limit of particles</param>
        public void DrawParticleCollection(Matrix View, Matrix Projection, Vector3 camUp, Vector4[] positionValues, Vector4[] dataValues, float ageLimit)
        {
            camUp.Normalize();
            for (int i = 0; i < positionValues.Length; i++)
            {
                if (dataValues[i].Z != -1)
                {
                    effect.Parameters["View"].SetValue(View);
                    effect.Parameters["Projection"].SetValue(Projection);
                    effect.Parameters["Normal"].SetValue(camUp);
                    effect.Parameters["MaxAge"].SetValue(ageLimit);
                    effect.Parameters["PositionValue"].SetValue(positionValues[i]);
                    effect.Parameters["DataValue"].SetValue(dataValues[i]);
                    effect.Parameters["Texture"].SetValue(texture);
                    effect.CurrentTechnique.Passes[0].Apply();

                    myDevice.DrawUserIndexedPrimitives
                        (PrimitiveType.TriangleList, verts, 0, 4, ib, 0, 2);
                }
            }
        }
    }
}
