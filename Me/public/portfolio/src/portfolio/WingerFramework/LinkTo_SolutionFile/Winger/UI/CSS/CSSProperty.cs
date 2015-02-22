using System.Collections;
using System.Collections.Generic;
using Winger.Utils;

namespace Winger.UI.CSS
{
    public enum CSSProperty
    {
        BACKGROUND_COLOR,
        BACKGROUND_IMAGE,
        BACKGROUND_POSITION,
        BACKGROUND_REPEAT,
        BACKGROUND_ORIGIN,
        BORDER,
        BORDER_TOP,
        BORDER_RIGHT,
        BORDER_BOTTOM,
        BORDER_LEFT,
        BORDER_RADIUS,
        ROTATION,
        ROTATION_POINT,
        HEIGHT,
        WIDTH,
        MAX_HEIGHT,
        MAX_WIDTH,
        MIN_HEIGHT,
        MIN_WIDTH,
        FONT_FAMILY,
        FONT_SIZE,
        FONT_STYLE,
        FONT_WEIGHT,
        LIST_STYLE,
        LIST_STYLE_IMAGE,
        LIST_STYLE_POSITION,
        LIST_STYLE_TYPE,
        MARGIN,
        MARGIN_TOP,
        MARGIN_RIGHT,
        MARGIN_BOTTOM,
        MARGIN_LEFT,
        PADDING,
        PADDING_TOP,
        PADDING_RIGHT,
        PADDING_BOTTOM,
        PADDING_LEFT,
        TOP,
        RIGHT,
        BOTTOM,
        LEFT,
        POSITION,
        Z_INDEX,
        VISIBILITY,
        CURSOR,
        COLOR,
        TEXT_ALIGN,
        LINE_HEIGHT,
        TEXT_WRAP,
        NONE,
    }

    public static class CSSPropertyUtils
    {
        public static IEnumerable CSSPROPERTY_ENUM_VALUES = EnumUtils.GetValues<CSSProperty>();
        public static Dictionary<string, CSSProperty> CSSPROPERTY_STRING_TO_ENUM = GetEnumStringValues();
        public static Dictionary<CSSProperty, string> CSSPROPERTY_ENUM_TO_STRING = GetStringEnumValues();

        public static CSSProperty GetCSSPropertyForString(string propertyStr)
        {
            string propertyTrimmed = propertyStr.Trim();
            if (CSSPROPERTY_STRING_TO_ENUM.ContainsKey(propertyTrimmed))
            {
                return CSSPROPERTY_STRING_TO_ENUM[propertyTrimmed];
            }
            return CSSProperty.NONE;
        }

        private static Dictionary<string, CSSProperty> GetEnumStringValues()
        {
            Dictionary<string, CSSProperty> strVals = new Dictionary<string, CSSProperty>();
            foreach (CSSProperty prop in CSSPROPERTY_ENUM_VALUES)
            {
                strVals[prop.GetStringValue()] = prop;
            }
            return strVals;
        }

        private static Dictionary<CSSProperty, string> GetStringEnumValues()
        {
            Dictionary<CSSProperty, string> enumVals = new Dictionary<CSSProperty, string>();
            foreach (string key in CSSPROPERTY_STRING_TO_ENUM.Keys)
            {
                enumVals[CSSPROPERTY_STRING_TO_ENUM[key]] = key;
            }
            return enumVals;
        }
    }

    public static class CSSPropertyEnumExtension
    {
        public static bool EqualsStr(this CSSProperty property, string propertyStr)
        {
            return (property.GetStringValue().Equals(propertyStr.Trim()));
        }

        public static string GetStringValue(this CSSProperty property)
        {
            return System.Enum.GetName(property.GetType(), property).ToLower().Replace('_', '-');
        }

        public static string AsString(this CSSProperty property)
        {
            return CSSPropertyUtils.CSSPROPERTY_ENUM_TO_STRING[property];
        }
    }
}
