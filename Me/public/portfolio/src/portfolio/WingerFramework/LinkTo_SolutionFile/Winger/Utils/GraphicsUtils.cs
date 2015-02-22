using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Winger.Utils
{
    public class GraphicsUtils
    {
        #region Singleton
        private static GraphicsUtils instance;

        private GraphicsUtils() { }

        public static GraphicsUtils Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GraphicsUtils();
                }
                return instance;
            }
        }
        #endregion

        public GraphicsDeviceManager Graphics { get; set; }
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                if (Graphics != null)
                {
                    return Graphics.GraphicsDevice;
                }
                return null;
            }
        }
    }
}
