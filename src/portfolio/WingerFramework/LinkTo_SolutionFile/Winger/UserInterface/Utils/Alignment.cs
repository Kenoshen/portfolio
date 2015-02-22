
namespace Winger.UserInterface.Utils
{
    public enum Alignment
    {
        TOP_LEFT,
        TOP,
        TOP_RIGHT,
        RIGHT,
        BOTTOM_RIGHT,
        BOTTOM,
        BOTTOM_LEFT,
        LEFT,
        CENTER,
    }


    public static class AlignmentEnumExtension
    {
        public static bool EqualsStr(this Alignment alignment, string alignmentStr)
        {
            if (alignmentStr != null)
                return alignment.AsString().Equals(alignmentStr.Trim().ToLower());
            return false;
        }


        public static string AsString(this Alignment alignment)
        {
            return System.Enum.GetName(alignment.GetType(), alignment).ToLower().Replace('_', '-');
        }
    }
}
