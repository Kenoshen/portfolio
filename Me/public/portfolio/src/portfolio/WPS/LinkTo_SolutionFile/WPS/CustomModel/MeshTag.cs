using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WPS.CustomModel
{
    public class MeshTag
    {
        public Vector3 Color;
        public Texture2D Texture;
        public float SpecularPower;
        public Effect CachedEffect = null;

        /// <summary>
        /// Mesh tag for use by the CModel class
        /// </summary>
        /// <param name="Color">color data</param>
        /// <param name="Texture">texture data</param>
        /// <param name="SpecularPower">specular data</param>
        public MeshTag(Vector3 Color, Texture2D Texture, float SpecularPower)
        {
            this.Color = Color;
            this.Texture = Texture;
            this.SpecularPower = SpecularPower;
        }
    }
}
