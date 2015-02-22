using Microsoft.Xna.Framework.Input;
using Winger.Input.Raw;

namespace Winger.UserInterface.Utils
{
    public static class TypeUtils
    {
        public const string AlphaKeys = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string NumKeys = "1234567890";
        public const string SpecialKeys = "`~!@#$%^&*()-_=+[{]}\\|;:'\",<.>/? \n\r\t";

        public static string GetTypedText(CKeyboard keyboard)
        {
            string text = "";
            Keys[] justPressedKeys = keyboard.KeysJustDown();
            if (justPressedKeys.Length > 0)
            {
                bool shift = (keyboard.IsKeyBeingPressed(Keys.LeftShift) || keyboard.IsKeyBeingPressed(Keys.RightShift));
                foreach (Keys key in justPressedKeys)
                {
                    string keyStr = key.AsString().Trim();
                    if (AlphaKeys.Contains(keyStr))
                    {
                        if (shift)
                        {
                            keyStr = keyStr.ToUpper();
                        }
                        text += keyStr;
                        continue;
                    }
                    string numStr = keyStr.Replace("numpad", "").Replace("d", "");
                    if (NumKeys.Contains(numStr))
                    {
                        if (shift)
                        {
                            switch (numStr)
                            {
                                case "0":
                                    numStr = ")";
                                    break;

                                case "1":
                                    numStr = "!";
                                    break;

                                case "2":
                                    numStr = "@";
                                    break;

                                case "3":
                                    numStr = "#";
                                    break;

                                case "4":
                                    numStr = "$";
                                    break;

                                case "5":
                                    numStr = "%";
                                    break;

                                case "6":
                                    numStr = "^";
                                    break;

                                case "7":
                                    numStr = "&";
                                    break;

                                case "8":
                                    numStr = "*";
                                    break;

                                case "9":
                                    numStr = "(";
                                    break;
                            }
                        }
                        text += numStr;
                        continue;
                    }
                    string spKey = "";
                    switch (keyStr)
                    {
                        case "back":
                            spKey = "\b";
                            break;

                        case "delete":
                            spKey = "\r";
                            break;

                        case "enter":
                            spKey = "\n";
                            break;

                        case "oembackslash":
                            if (shift)
                                spKey = "|";
                            else
                                spKey = "\\";
                            break;

                        case "oemclosebrackets":
                            if (shift)
                                spKey = "}";
                            else
                                spKey = "]";
                            break;

                        case "oemcomma":
                            if (shift)
                                spKey = "<";
                            else
                                spKey = ",";
                            break;

                        case "oemminus":
                            if (shift)
                                spKey = "_";
                            else
                                spKey = "-";
                            break;

                        case "oemopenbrackets":
                            if (shift)
                                spKey = "{";
                            else
                                spKey = "[";
                            break;

                        case "oemperiod":
                            if (shift)
                                spKey = ">";
                            else
                                spKey = ".";
                            break;

                        case "oempipe":
                            if (shift)
                                spKey = "|";
                            else
                                spKey = "\\";
                            break;

                        case "oemplus":
                            if (shift)
                                spKey = "+";
                            else
                                spKey = "=";
                            break;

                        case "oemquestion":
                            if (shift)
                                spKey = "?";
                            else
                                spKey = "/";
                            break;

                        case "oemquotes":
                            if (shift)
                                spKey = "\"";
                            else
                                spKey = "'";
                            break;

                        case "oemsemicolon":
                            if (shift)
                                spKey = ":";
                            else
                                spKey = ";";
                            break;

                        case "oemtilde":
                            if (shift)
                                spKey = "~";
                            else
                                spKey = "`";
                            break;
                    }
                    text += spKey;
                    continue;
                }
            }
            return text;
        }
    }

    public static class KeysEnumExtension
    {
        public static string AsString(this Keys key)
        {
            string keyStr = System.Enum.GetName(key.GetType(), key).ToLower();
            return keyStr;
        }
    }
}
