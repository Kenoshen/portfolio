using System;
using Microsoft.Xna.Framework;

namespace Winger.Utils
{
    public static class ColorUtils
    {
        private static Random rnd = new Random();

        public static Color GetRandomColor()
        {
            Vector4 col = new Vector4(0, 0, 0, 1);

            float max = 1000;

            col.X = ((float)rnd.Next((int)max)) / max;
            col.Y = ((float)rnd.Next((int)max)) / max;
            col.Z = ((float)rnd.Next((int)max)) / max;

            return new Color(col);
        }

        public static string ColorToHexString(Color color)
        {
            Vector4 col = color.ToVector4();

            int redInt = (int)(col.X * 255f);
            int greenInt = (int)(col.Y * 255f);
            int blueInt = (int)(col.Z * 255f);
            int alphaInt = (int)(col.W * 255f);

            string redHex = ParseUtils.HexChars[redInt / 16] + ParseUtils.HexChars[redInt % 16];
            string greenHex = ParseUtils.HexChars[greenInt / 16] + ParseUtils.HexChars[greenInt % 16];
            string blueHex = ParseUtils.HexChars[blueInt / 16] + ParseUtils.HexChars[blueInt % 16];
            string alphaHex = ParseUtils.HexChars[alphaInt / 16] + ParseUtils.HexChars[alphaInt % 16];

            return "#" + redHex + greenHex + blueHex + alphaHex;
        }
    }
}
