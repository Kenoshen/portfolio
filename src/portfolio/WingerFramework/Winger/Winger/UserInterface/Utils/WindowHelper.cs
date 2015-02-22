using Winger.Utils;

namespace Winger.UserInterface.Utils
{
    public class WindowHelper
    {
        #region Singleton
        private static WindowHelper instance;

        private WindowHelper()
        {
            Bounds = CRectangle.Empty;
        }

        public static WindowHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WindowHelper();
                }
                return instance;
            }
        }
        #endregion

        public CRectangle Bounds { get; set; }
    }
}
