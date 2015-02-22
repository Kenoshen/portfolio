using System;
using Microsoft.Xna.Framework;

namespace Winger.Utils
{
    public static class ParseUtils
    {
        public static string[] HexChars = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };


        public static int HexValue(string hex)
        {
            if (hex == null)
                return -1;

            hex = hex.Replace("#", "").ToUpper();

            if (hex.Length % 2 != 0)
                hex = "0" + hex;

            if (hex.Length > 8)
                return -1;

            int val = 0;
            int power = 0;
            for (int i = hex.Length - 1; i >= 0; i--)
            {
                for (int k = 0; k < HexChars.Length; k++)
                {
                    if (hex[i].Equals(HexChars[k][0]))
                    {
                        val += k * (int)Math.Pow(16, power);
                    }
                }
                power++;
            }
            return val;
        }


        public static Color DecodeColor(string colorStr)
        {
            return DecodeColor(colorStr, Color.White);
        }


        public static Color DecodeColor(string colorStr, Color defaultColor)
        {
            if (colorStr == null)
                return defaultColor;

            colorStr = colorStr.ToLower();

            float red = -1;
            float green = -1;
            float blue = -1;
            float alpha = 1;
            if (colorStr.Contains("#"))
            {
                colorStr = colorStr.Substring(1);
                string redHex = "";
                string greenHex = "";
                string blueHex = "";
                string alphaHex = "";
                if (colorStr.Length == 3 || colorStr.Length == 4)
                {
                    redHex = colorStr.Substring(0, 1);
                    redHex += redHex;
                    greenHex = colorStr.Substring(1, 1);
                    greenHex += greenHex;
                    blueHex = colorStr.Substring(2, 1);
                    blueHex += blueHex;
                    if (colorStr.Length == 4)
                    {
                        alphaHex = colorStr.Substring(3, 1);
                        alphaHex += alphaHex;
                        alpha = ((float)HexValue(alphaHex)) / 255f;
                    }
                }
                else if (colorStr.Length == 6 || colorStr.Length == 8)
                {
                    redHex = colorStr.Substring(0, 2);
                    greenHex = colorStr.Substring(2, 2);
                    blueHex = colorStr.Substring(4, 2);
                    if (colorStr.Length == 8)
                    {
                        alphaHex = colorStr.Substring(6, 2);
                        alpha = ((float)HexValue(alphaHex)) / 255f;
                    }
                }
                else
                {
                    throw new Exception("Unsupported format for color: " + colorStr);
                }
                red = ((float)HexValue(redHex)) / 255f;
                green = ((float)HexValue(greenHex)) / 255f;
                blue = ((float)HexValue(blueHex)) / 255f;
            }
            else if (colorStr.Contains("rgb"))
            {
                colorStr = colorStr.Substring(4, 5);
                string[] colorArray = colorStr.Split(',');
                red = float.Parse(colorArray[0]);
                green = float.Parse(colorArray[1]);
                blue = float.Parse(colorArray[2]);
                if (colorArray.Length == 4)
                    alpha = float.Parse(colorArray[3]);
            }
            else
            {
                switch (colorStr)
                {
                    case "red":
                        red = 1;
                        break;
                    case "darkred":
                        red = 0.3f;
                        break;
                    case "orange":
                        red = 250f / 255f;
                        green = 120f / 255f;
                        break;
                    case "yellow":
                        red = 250f / 255f;
                        green = 240f / 255f;
                        break;
                    case "limegreen":
                        red = 140f / 255f;
                        green = 240f / 255f;
                        break;
                    case "yellowgreen":
                        red = 140f / 255f;
                        green = 240f / 255f;
                        break;
                    case "lightgreen":
                        red = 140f / 255f;
                        green = 240f / 255f;
                        break;
                    case "green":
                        green = 1;
                        break;
                    case "darkgreen":
                        green = 0.3f;
                        break;
                    case "indigo":
                        green = 240f / 255f;
                        blue = 240f / 255f;
                        break;
                    case "teal":
                        green = 240f / 255f;
                        blue = 240f / 255f;
                        break;
                    case "lightblue":
                        green = 220f / 255f;
                        blue = 240f / 255f;
                        break;
                    case "blue":
                        blue = 1;
                        break;
                    case "darkblue":
                        blue = 0.3f;
                        break;
                    case "pink":
                        red = 220f / 255f;
                        blue = 240f / 255f;
                        break;
                    case "purple":
                        red = 120f / 255f;
                        green = 20f / 255f;
                        blue = 160f / 255f;
                        break;
                    case "black":
                        red = 0;
                        green = 0;
                        blue = 0;
                        break;
                    case "white":
                        red = 1;
                        green = 1;
                        blue = 1;
                        break;
                }
            }

            if (red == -1 && green == -1 && blue == -1)
                return defaultColor;
            else
            {
                if (red == -1)
                    red = 0;
                if (green == -1)
                    green = 0;
                if (blue == -1)
                    blue = 0;
            }

            return new Color(red, green, blue, alpha);
        }


        public static Vector2 Vector2FromString(string vectorStr)
        {
            float[] vals = VectorFromString(vectorStr);
            if (vals == null)
                return Vector2.Zero;
            return new Vector2(vals[0], vals[1]);
        }


        public static Vector3 Vector3FromString(string vectorStr)
        {
            float[] vals = VectorFromString(vectorStr);
            if (vals == null)
                return Vector3.Zero;
            return new Vector3(vals[0], vals[1], vals[2]);
        }


        public static Vector4 Vector4FromString(string vectorStr)
        {
            float[] vals = VectorFromString(vectorStr);
            if (vals == null)
                return Vector4.Zero;
            return new Vector4(vals[0], vals[1], vals[2], vals[3]);
        }


        private static float[] VectorFromString(string vectorStr)
        {
            if (vectorStr == null)
                return null;

            vectorStr = vectorStr.Replace("(", "").Replace(")", "");
            string[] vals = vectorStr.Split(',', ';', '/', '|');
            float[] values = new float[] { 0, 0, 0, 0 };
            for (int i = 0; i < vals.Length && i < 4; i++)
            {
                try
                {
                    values[i] = float.Parse(vals[i]);
                }
                catch (Exception) { }
            }
            return values;
        }
    }
}
