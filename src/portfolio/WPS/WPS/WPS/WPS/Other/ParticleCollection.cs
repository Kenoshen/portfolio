using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WPS
{
    class ParticleCollection
    {
        private VertexPositionNormalTexture[] verts;
        private GraphicsDevice myDevice;
        private short[] ib = null;
        private Texture2D texture;
        private Effect effect;
        private int numOfParticles;

        /// <summary>
        /// This class stores a collection of verticies to be used to draw many particles at once
        /// </summary>
        /// <param name="size">the width or length of the data texture (must be a perfect square texture ie. width == height)</param>
        /// <param name="texture">the particle display texture</param>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        /// <param name="Content">just use the Content parameter that is contained within the Game1 class</param>
        public ParticleCollection(int size, Texture2D texture, GraphicsDevice GraphicsDevice, ContentManager Content)
        {
            this.numOfParticles = size * size;
            myDevice = GraphicsDevice;
            this.effect = Content.Load<Effect>(Global_Variables_WPS.ContentEffects + "DrawParticleCollection");
            this.texture = texture;

            verts = new VertexPositionNormalTexture[numOfParticles * 4];
            ib = new short[numOfParticles * 6];

            for (int i = 0; i < numOfParticles; i++)
            {
                float x = ((float)i - ((i / size) * (float)size)) / (float)size;
                float y = (i / size) / (float)size;

                float jitter = 0.25f / (float)size;

                Vector3 index_pv = new Vector3(new Vector2(x + jitter, y + jitter), 0);
                verts[i * 4] = new VertexPositionNormalTexture(
                                new Vector3(0, 0, 0),
                                index_pv,
                                new Vector2(0, 1));
                verts[(i * 4) + 1] = new VertexPositionNormalTexture(
                                new Vector3(0, 0, 0),
                                index_pv,
                                new Vector2(1, 1));
                verts[(i * 4) + 2] = new VertexPositionNormalTexture(
                                new Vector3(0, 0, 0),
                                index_pv,
                                new Vector2(1, 0));
                verts[(i * 4) + 3] = new VertexPositionNormalTexture(
                                new Vector3(0, 0, 0),
                                index_pv,
                                new Vector2(0, 0));

                ib[i * 6] = (short)(0 + (i * 4));
                ib[(i * 6) + 1] = (short)(1 + (i * 4));
                ib[(i * 6) + 2] = (short)(2 + (i * 4));
                ib[(i * 6) + 3] = (short)(2 + (i * 4));
                ib[(i * 6) + 4] = (short)(3 + (i * 4));
                ib[(i * 6) + 5] = (short)(0 + (i * 4));
            }
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
        public void DrawParticleCollection(Matrix View, Matrix Projection, Vector3 camUp, Texture2D positionTexture, Texture2D dataTexture, float ageLimit)
        {
            camUp.Normalize();
            effect.Parameters["View"].SetValue(View);
            effect.Parameters["Projection"].SetValue(Projection);
            effect.Parameters["Normal"].SetValue(camUp);
            effect.Parameters["MaxAge"].SetValue(ageLimit);
            effect.Parameters["PositionTexture"].SetValue(positionTexture);
            effect.Parameters["DataTexture"].SetValue(dataTexture);
            effect.Parameters["Texture"].SetValue(texture);
            effect.CurrentTechnique.Passes[0].Apply();

            myDevice.DrawUserIndexedPrimitives
                (PrimitiveType.TriangleList, verts, 0, numOfParticles * 4, ib, 0, numOfParticles * 2);
        }
    }
}
