using Winger.Utils;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Winger.Input.Raw;
using Winger.UserInterface.Utils;

namespace Winger.UserInterface.ElementImpl
{
    public class TextBoxElement : Element
    {
        public string Allowed
        {
            get { return (string)Get("allowed"); }
            set { Put("allowed", value); }
        }

        public string NotAllowed
        {
            get { return (string) Get("notallowed"); }
            set { Put("nowallowed", value); }
        }

        public int MaxChar
        {
            get { return Convert.ToInt32(Get("maxchar")); }
            set { Put("maxchar", value); }
        }

        public int Index
        {
            get { return Convert.ToInt32(Get("index")); }
            set { Put("index", value); }
        }

        public TextBoxElement(string json) : base(json) { }

        public TextBoxElement(JSON json) : base(json) { }

        public TextBoxElement(JSON json, JSON settings, JSON globals) : base(json, settings, globals) { }

        public TextBoxElement(JSON json, JSON settings, JSON globals, Element parent) : base(json, settings, globals, parent) { }


        public override void Initialize()
        {
            base.Initialize();

            AddDefaultValues("maxchar", -1, "index", 0);

            Index = Text.Length;
        }


        public override void SendKeyboardInfoToThisElement(CKeyboard keyboard)
        {
            // TODO: need to figure out how to set the index in the right place
            if (Text == null)
            {
                Text = "";
            }
            string t = Text;

            string typedText = TypeUtils.GetTypedText(keyboard);
            for (int i = 0; i < typedText.Length; i++)
            {
                char c = typedText[i];
                switch (c)
                {
                    case '\b':
                        // backspace here
                        if (t.Length > 0 && Index != 0)
                        {
                            t = t.Remove(Index - 1, 1);
                            Index = Index - 1;
                        }
                        typedText = typedText.Remove(i, 1);
                        i--;
                        continue;

                    case '\r':
                        // delete here
                        if (t.Length > 0 && Index != t.Length)
                        {
                            t = t.Remove(Index, 1);
                        }
                        typedText = typedText.Remove(i, 1);
                        i--;
                        continue;
                }
                if (Allowed != null)
                {
                    if (!Allowed.Contains("" + c))
                    {
                        typedText = typedText.Remove(i, 1);
                        i--;
                        continue;
                    }
                }
                if (NotAllowed != null)
                {
                    if (NotAllowed.Contains("" + c))
                    {
                        typedText = typedText.Remove(i, 1);
                        i--;
                        continue;
                    }
                }
            }

            if (MaxChar >= 0 && t.Length >= MaxChar)
            {
                return;
            }

            if (Index < t.Length)
            {
                t = t.Insert(Index, typedText);
            }
            else
            {
                t += typedText;
            }
            Index = Index + typedText.Length;

            Text = t;
        }
    }
}
