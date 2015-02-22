using Microsoft.Xna.Framework.Content;

namespace Winger.Utils
{
    public class ContentUtils
    {
        private static ContentUtils instance;

        private ContentUtils() { }

        public static ContentUtils Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ContentUtils();
                }
                return instance;
            }
        }

        public ContentManager Content { get; set; }
    }
}
