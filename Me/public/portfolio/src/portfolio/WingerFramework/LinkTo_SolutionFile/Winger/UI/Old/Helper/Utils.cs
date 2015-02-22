using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Winger.UI.UI;

namespace Winger.UI.Helper
{
    public static class Utils
    {
        public static string[] HexChars = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };

        public static int HexValue(string hex)
        {
            if (hex == null)
                return -1;

            hex = hex.Replace("#", "");

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
                    if (hex[i].Equals(HexChars[k]))
                    {
                        val += k * (int)Math.Pow(16, power);
                    }
                }
                power++;
            }
            return val;
        }

        public static Color ColorDecoder(string colorStr, Color defaultColor)
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
                string redHex = colorStr.Substring(0, 2);
                string greenHex = colorStr.Substring(2, 2);
                string blueHex = colorStr.Substring(4, 2);

                red = ((float)Utils.HexValue(redHex)) / 255f;
                green = ((float)Utils.HexValue(greenHex)) / 255f;
                blue = ((float)Utils.HexValue(blueHex)) / 255f;

                if (colorStr.Length == 8)
                {
                    string alphaHex = colorStr.Substring(6, 2);
                    alpha = ((float)Utils.HexValue(alphaHex)) / 255f;
                }
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

        public static Color ColorDecoder(string colorStr)
        {
            return ColorDecoder(colorStr, Color.White);
        }

        public static Tree<UIObject> GetTreeFromFile(string fileLocation)
        {
            StreamReader file = new StreamReader(fileLocation);
            XmlReader reader = XmlTextReader.Create(file);
            Stack<UIObject> elementStack = new Stack<UIObject>();
            Tree<UIObject> tree = new Tree<UIObject>(new UIObject());

            while (!reader.EOF)
            {
                if (!reader.IsEmptyElement)
                {
                    if (reader.IsStartElement())
                    {
                        elementStack.Push(UIObjectFactory.CreateWithTag(reader.Name));
                        UIObject[] objArr = elementStack.ToArray();
                        UIObject[] revArr = new UIObject[objArr.Length];
                        for (int i = 0; i < objArr.Length; i++)
                            revArr[(objArr.Length - 1) - i] = objArr[i];
                        tree.AddBranch(revArr);
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        UIObject lastObj = elementStack.Pop();
                        if (elementStack.Count > 0 && elementStack.Peek().Tween == null && lastObj.Tag == "tween")
                            elementStack.Peek().Tween = Winger.UI.Tween.Tween.TweenFromUIObject(lastObj);
                    }
                    else if (reader.NodeType == XmlNodeType.Text)
                    {
                        elementStack.Peek().Text = reader.Value;
                    }

                    for (int i = 0; i < reader.AttributeCount; i++)
                    {
                        reader.MoveToAttribute(i);
                        elementStack.Peek().Put(reader.Name, reader.Value);
                    }
                }

                reader.Read();
            }

            reader.Close();
            file.Close();

            tree = tree.GetChild(0);
            return tree;
        }

        public static Alignment GetAlignmentFromString(string s, Alignment defaultAlignment)
        {
            if (s == null)
                return defaultAlignment;

            string str = s.ToUpper().Replace(" ", "").Replace("_", "");

            if (str == "0" || str == "TOPLEFT")
                return Alignment.TOP_LEFT;
            else if (str == "1" || str == "TOP")
                return Alignment.TOP;
            else if (str == "2" || str == "TOPRIGHT")
                return Alignment.TOP_RIGHT;
            else if (str == "3" || str == "RIGHT")
                return Alignment.RIGHT;
            else if (str == "4" || str == "BOTTOMRIGHT")
                return Alignment.BOTTOM_RIGHT;
            else if (str == "5" || str == "BOTTOM")
                return Alignment.BOTTOM;
            else if (str == "6" || str == "BOTTOMLEFT")
                return Alignment.BOTTOM_LEFT;
            else if (str == "7" || str == "LEFT")
                return Alignment.LEFT;
            else if (str == "8" || str == "CENTER")
                return Alignment.CENTER;
            else
                return defaultAlignment;
        }

        public static Alignment GetAlignmentFromString(string s)
        {
            return GetAlignmentFromString(s, Alignment.TOP_LEFT);
        }

        public static Vector2 GetTextOffset(Alignment alignment, Vector2 size)
        {
            Vector2 offset = Vector2.Zero;
            switch (alignment)
            {
                case Alignment.TOP_LEFT:
                    break;

                case Alignment.TOP:
                    offset.X = size.X / 2f;
                    break;

                case Alignment.TOP_RIGHT:
                    offset.X = size.X;
                    break;

                case Alignment.RIGHT:
                    offset.X = size.X;
                    offset.Y = size.Y / 2f;
                    break;

                case Alignment.BOTTOM_RIGHT:
                    offset.X = size.X;
                    offset.Y = size.Y;
                    break;

                case Alignment.BOTTOM:
                    offset.X = size.X / 2f;
                    offset.Y = size.Y;
                    break;

                case Alignment.BOTTOM_LEFT:
                    offset.Y = size.Y;
                    break;

                case Alignment.LEFT:
                    offset.Y = size.Y / 2f;
                    break;

                case Alignment.CENTER:
                    offset.X = size.X / 2f;
                    offset.Y = size.Y / 2f;
                    break;
            }
            return offset;
        }

        public static Vector2 Vector2FromString(string vectorStr)
        {
            if (vectorStr == null)
                return Vector2.Zero;

            vectorStr = vectorStr.Replace("(", "").Replace(")", "");
            string[] vals = vectorStr.Split(',', ';', '/', '|');

            Vector2 vect = Vector2.Zero;

            try
            {
                vect.X = float.Parse(vals[0]);
            }
            catch (Exception) { }

            try
            {
                vect.Y = float.Parse(vals[1]);
            }
            catch (Exception) { }

            return vect;
        }

        public static Vector3 Vector3FromString(string vectorStr)
        {
            if (vectorStr == null)
                return Vector3.Zero;

            vectorStr = vectorStr.Replace("(", "").Replace(")", "");
            string[] vals = vectorStr.Split(',', ';', '/', '|');

            Vector3 vect = Vector3.Zero;

            try
            {
                vect.X = float.Parse(vals[0]);
            }
            catch (Exception) { }

            try
            {
                vect.Y = float.Parse(vals[1]);
            }
            catch (Exception) { }

            try
            {
                vect.Z = float.Parse(vals[2]);
            }
            catch (Exception) { }

            return vect;
        }

        public static Vector4 Vector4FromString(string vectorStr)
        {
            if (vectorStr == null)
                return Vector4.Zero;

            vectorStr = vectorStr.Replace("(", "").Replace(")", "");
            string[] vals = vectorStr.Split(',', ';', '/', '|');

            Vector4 vect = Vector4.Zero;

            try
            {
                vect.X = float.Parse(vals[0]);
            }
            catch (Exception) { }

            try
            {
                vect.Y = float.Parse(vals[1]);
            }
            catch (Exception) { }

            try
            {
                vect.Z = float.Parse(vals[2]);
            }
            catch (Exception) { }

            try
            {
                vect.W = float.Parse(vals[3]);
            }
            catch (Exception) { }

            return vect;
        }
    }
}
