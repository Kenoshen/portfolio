using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WPS
{
    public class DataTexture
    {
        private GraphicsDevice GraphicsDevice;
        private RenderTarget2D target1;
        private RenderTarget2D target2;
        private bool doubleBuffer = true;
        private int size;

        /// <summary>
        /// The current usable texture
        /// </summary>
        public RenderTarget2D CurrentTexture
        {
            get
            {
                if (!doubleBuffer)
                    return target1;
                else
                    return target2;
            }
        }

        /// <summary>
        /// A data texture is a pair of textures that is used to store information instead of color data.  It automatically uses double buffering to avoid reading and writing to the same target.
        /// </summary>
        /// <param name="size">the width or height of the data texture (data textures must be perfect squares ie. width == height)</param>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        public DataTexture(int size, GraphicsDevice GraphicsDevice)
        {
            this.GraphicsDevice = GraphicsDevice;
            this.size = size;

            target1 = new RenderTarget2D(GraphicsDevice, size, size, false, SurfaceFormat.Vector4, DepthFormat.None);
            target2 = new RenderTarget2D(GraphicsDevice, size, size, false, SurfaceFormat.Vector4, DepthFormat.None);
        }

        /// <summary>
        /// A data texture is a pair of textures that is used to store information instead of color data.  It automatically uses double buffering to avoid reading and writing to the same target.
        /// </summary>
        /// <param name="size">the width or height of the data texture (data textures must be perfect squares ie. width == height)</param>
        /// <param name="data">the vector4 data to fill the data texture with</param>
        /// <param name="GraphicsDevice">just use the GraphicsDevice parameter that is contained within the Game1 class</param>
        public DataTexture(int size, Vector4[] data, GraphicsDevice GraphicsDevice)
        {
            this.GraphicsDevice = GraphicsDevice;
            this.size = size;

            SetTextureData(data);
        }

        /// <summary>
        /// Sets the texture data
        /// </summary>
        /// <param name="data">texture data</param>
        public void SetTextureData(Vector4[] data)
        {
            target1 = new RenderTarget2D(GraphicsDevice, size, size, false, SurfaceFormat.Vector4, DepthFormat.None);
            target1.SetData(data);

            target2 = new RenderTarget2D(GraphicsDevice, size, size, false, SurfaceFormat.Vector4, DepthFormat.None);
            target2.SetData(data);
        }

        /// <summary>
        /// Sets the texture data to all zero
        /// </summary>
        public void SetTextureDataToZeros()
        {
            target1 = new RenderTarget2D(GraphicsDevice, size, size, false, SurfaceFormat.Vector4, DepthFormat.None);
            target1.SetData(GetData_Zeros(size));

            target2 = new RenderTarget2D(GraphicsDevice, size, size, false, SurfaceFormat.Vector4, DepthFormat.None);
            target2.SetData(GetData_Zeros(size));
        }

        /// <summary>
        /// Sets the texture data to all negative one
        /// </summary>
        public void SetTextureDataToNegOnes()
        {
            target1 = new RenderTarget2D(GraphicsDevice, size, size, false, SurfaceFormat.Vector4, DepthFormat.None);
            target1.SetData(GetData_NegOne(size));

            target2 = new RenderTarget2D(GraphicsDevice, size, size, false, SurfaceFormat.Vector4, DepthFormat.None);
            target2.SetData(GetData_NegOne(size));
        }

        /// <summary>
        /// Sets the texture data to all negative one million
        /// </summary>
        public void SetTextureDataToNegMil()
        {
            target1 = new RenderTarget2D(GraphicsDevice, size, size, false, SurfaceFormat.Vector4, DepthFormat.None);
            target1.SetData(GetData_NegMil(size));

            target2 = new RenderTarget2D(GraphicsDevice, size, size, false, SurfaceFormat.Vector4, DepthFormat.None);
            target2.SetData(GetData_NegMil(size));
        }

        /// <summary>
        /// Draw the data from the graphics buffer to the texture
        /// </summary>
        /// <param name="effect">the effect to use when drawing the quad</param>
        /// <param name="fullScreenQuad">the quad to use to draw to</param>
        public void DrawDataToTexture(Effect effect, WPS.CustomModel.Quad fullScreenQuad)
        {
            // Set the render target
            if (doubleBuffer)
                GraphicsDevice.SetRenderTarget(target1);
            else
                GraphicsDevice.SetRenderTarget(target2);

            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            // Draw the scene
            GraphicsDevice.Clear(Color.Black);

            fullScreenQuad.RenderFullScreenQuad(effect, size);

            // Drop the render target
            GraphicsDevice.SetRenderTarget(null);

            doubleBuffer = !doubleBuffer;
            
        }

        private static Vector4[] GetData_Zeros(int size)
        {
            Vector4[] data = new Vector4[size * size];

            for (int i = 0; i < data.Length; i++)
                data[i] = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);

            return data;
        }

        private static Vector4[] GetData_NegOne(int size)
        {
            Vector4[] data = new Vector4[size * size];

            for (int i = 0; i < data.Length; i++)
                data[i] = new Vector4(-1.0f, -1.0f, -1.0f, 0.0f);

            return data;
        }

        private static Vector4[] GetData_NegMil(int size)
        {
            Vector4[] data = new Vector4[size * size];

            for (int i = 0; i < data.Length; i++)
                data[i] = new Vector4(-1000000.0f, -1000000.0f, -1000000.0f, -1000000.0f);

            return data;
        }
    }
}
