using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace ParticleGame
{
    class TextureObjectBuilder
    {
        public static List<Vector4> BuildSceneObjectFromTexture(Texture2D texture, float baseX, float baseY, float baseZ, float size = 1)
        {
            List<Vector4> obj = new List<Vector4>();

            Color[] colorData = new Color[(texture.Width) * (texture.Height)];
            texture.GetData<Color>(colorData);

            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                {
                    Color color = colorData[(x + (y * texture.Width))];
                    byte threshold = 1;
                    if (color.R < threshold && color.G < threshold && color.B < threshold)
                    {
                        obj.Add(new Vector4(baseX + x, baseY - y + texture.Height, baseZ, 1) * size);
                    }
                    else
                    {
                        //obj.Add(new Vector4(baseX + x, baseY + y, baseZ, 0));
                    }
                }

            return obj;
        }

        public static List<Vector4> BuildPhysicalObjectFromTexture(Texture2D texture, float baseX, float baseY, float baseZ)
        {
            List<Vector4> obj = new List<Vector4>();

            Color[] colorData = new Color[(texture.Width) * (texture.Height)];
            texture.GetData<Color>(colorData);

            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                {
                    Color color = colorData[(x + (y * texture.Width))];
                    byte threshold = 10;
                    if (color.R < threshold && color.G < threshold && color.B < threshold)
                        obj.Add(new Vector4(baseX + x, baseY - y + texture.Height, baseZ, 0));
                    else if (color.G < threshold && color.B < threshold)
                        obj.Add(new Vector4(baseX + x, baseY - y + texture.Height, baseZ, 1));
                }

            return obj;
        }
    }
}
