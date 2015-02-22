using System.Collections;
using System.Collections.Generic;
using Winger.Utils;

namespace Winger.UI.HTML
{
    public enum HTMLTag
    {
        HTML,
        HEAD,
        TITLE,
        LINK,
        BODY,
        DIV,
        A,
        IMG,
        INPUT,
        H1,
        H2,
        H3,
        P,
        B,
        I,
        T,
        NONE,
    }

    public static class HTMLTagUtils
    {
        public static IEnumerable HTMLTAG_ENUM_VALUES = EnumUtils.GetValues<HTMLTag>();
        public static Dictionary<string, HTMLTag> HTMLTAG_STRING_TO_ENUM = GetEnumStringValues();
        public static Dictionary<HTMLTag, string> HTMLTAG_ENUM_TO_STRING = GetStringEnumValues();

        public static HTMLTag GetCSSPropertyForString(string tagStr)
        {
            string tagTrimmed = tagStr.ToLower().Trim();
            if (HTMLTAG_STRING_TO_ENUM.ContainsKey(tagTrimmed))
            {
                return HTMLTAG_STRING_TO_ENUM[tagTrimmed];
            }
            return HTMLTag.NONE;
        }

        private static Dictionary<string, HTMLTag> GetEnumStringValues()
        {
            Dictionary<string, HTMLTag> strVals = new Dictionary<string, HTMLTag>();
            foreach (HTMLTag tag in HTMLTAG_ENUM_VALUES)
            {
                strVals[tag.GetStringValue()] = tag;
            }
            return strVals;
        }

        private static Dictionary<HTMLTag, string> GetStringEnumValues()
        {
            Dictionary<HTMLTag, string> enumVals = new Dictionary<HTMLTag, string>();
            foreach (string key in HTMLTAG_STRING_TO_ENUM.Keys)
            {
                enumVals[HTMLTAG_STRING_TO_ENUM[key]] = key;
            }
            return enumVals;
        }
    }

    public static class HTMLTagEnumExtension
    {
        public static bool EqualsStr(this HTMLTag tag, string tagStr)
        {
            return (tag.GetStringValue().Equals(tagStr.ToLower().Trim()));
        }

        public static string GetStringValue(this HTMLTag tag)
        {
            return System.Enum.GetName(tag.GetType(), tag).ToLower();
        }

        public static string AsString(this HTMLTag tag)
        {
            return HTMLTagUtils.HTMLTAG_ENUM_TO_STRING[tag];
        }
    }
}
