using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WPS.CustomModel
{
    public class Quad
    {
        private VertexPositionTexture[] verts;
        private GraphicsDevice myDevice;
        private short[] ib = null;

        /// <summary>
        /// The quad is basically a 2D square in 3D space
        /// </summary>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        public Quad(GraphicsDevice GraphicsDevice)
        {
            myDevice = GraphicsDevice;         

            verts = new VertexPositionTexture[]
                        {
                            new VertexPositionTexture(
                                new Vector3(0,0,0),
                                new Vector2(1,1)),
                            new VertexPositionTexture(
                                new Vector3(0,0,0),
                                new Vector2(0,1)),
                            new VertexPositionTexture(
                                new Vector3(0,0,0),
                                new Vector2(0,0)),
                            new VertexPositionTexture(
                                new Vector3(0,0,0),
                                new Vector2(1,0))
                        };

             ib = new short[] { 0, 1, 2, 2, 3, 0 };

        }             

        /// <summary>
        /// Draws a quad that takes up the entire screen.  Used for data textures that use the GPU to store data into textures.
        /// </summary>
        /// <param name="effect">the effect to be used when drawing the quad</param>
        /// <param name="size">the length or width of the texture (must be a perfectly square texture to work correctly)</param>
        public void RenderFullScreenQuad(Effect effect, int size)
        {
            effect.CurrentTechnique.Passes[0].Apply();
            RFSQ(new Vector2(-1f - (1f / (float)size), -1f + (1f / (float)size)), new Vector2(1f - (1f / (float)size), 1f + (1f / (float)size)));
        }

        private void RFSQ(Vector2 v1, Vector2 v2)
        {
            verts[0].Position.X = v2.X;
            verts[0].Position.Y = v1.Y;

            verts[1].Position.X = v1.X;
            verts[1].Position.Y = v1.Y;

            verts[2].Position.X = v1.X;
            verts[2].Position.Y = v2.Y;

            verts[3].Position.X = v2.X;
            verts[3].Position.Y = v2.Y;

             myDevice.DrawUserIndexedPrimitives
                (PrimitiveType.TriangleList, verts, 0, 4, ib, 0, 2);
        }

        /// <summary>
        /// Draws a quad with a given effect at a given position
        /// </summary>
        /// <param name="effect">the effect to be used to draw the quad</param>
        /// <param name="v1">the top right vertex</param>
        /// <param name="v2">the top left vertex</param>
        /// <param name="v3">the bottom left vertex</param>
        /// <param name="v4">the bottom right vertex</param>
        public void RenderQuad(Effect effect, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            effect.CurrentTechnique.Passes[0].Apply();

            verts[0].Position = v1;
            verts[1].Position = v2;
            verts[2].Position = v3;
            verts[3].Position = v4;

            myDevice.DrawUserIndexedPrimitives
                (PrimitiveType.TriangleList, verts, 0, 4, ib, 0, 2);
        }
    }
}
