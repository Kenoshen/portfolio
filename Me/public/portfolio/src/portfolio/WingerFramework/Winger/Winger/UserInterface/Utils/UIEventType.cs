
namespace Winger.UserInterface.Utils
{
    public enum UIEventType
    {
        ON_SELECT_START,
        ON_SELECT_END,
        ON_HOVER_START,
        ON_HOVER_END,
    }

    public static class UIEventTypeEnumExtension
    {
        public static bool EqualsStr(this UIEventType eventType, string eventStr)
        {
            if (eventStr != null)
                return eventType.AsString().Equals(eventStr.Trim().ToLower());
            return false;
        }


        public static string AsString(this UIEventType eventType)
        {
            return System.Enum.GetName(eventType.GetType(), eventType).ToLower().Replace('_', '-');
        }
    }
}
