using Microsoft.Xna.Framework.Graphics;
using Winger.Utils;
using System;
using System.IO;

namespace Winger.UI.IMG
{
    public class ImgElement
    {
        public string Src { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }
        public Texture2D Texture = null;

        public ImgElement(string sourcStr)
        {
            if (sourcStr == null || sourcStr.Equals(""))
            {
                throw new Exception("Invalid ImgElement Source: '" + sourcStr + "'");
            }
            Src = sourcStr;
            Name = Src;
            Path = Src;
            LoadContent();
        }


        public void LoadContent()
        {
            if (ContentUtils.Instance.Content != null)
            {
                //Texture = ContentUtils.Instance.Content.Load<Texture2D>(FileUtils.RemoveExtension(Src));
                FileStream stream = File.Open(Src, FileMode.Open);
                Texture = Texture2D.FromStream(GraphicsUtils.Instance.GraphicsDevice, stream);
                stream.Close();
            }
        }
    }
}
