using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Winger.Utils;

namespace Winger.UI.IMG
{
    public class ImgElementManager
    {
        #region Singleton
        private static ImgElementManager instance;

        private ImgElementManager() { }

        public static ImgElementManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ImgElementManager();
                }
                return instance;
            }
        }
        #endregion


        private List<ImgElement> imgs = new List<ImgElement>();


        public void AddImgElement(string source)
        {
            if (GetImgElementWithSource(source) == null)
            {
                imgs.Add(new ImgElement(source));
            }
        }


        public ImgElement GetImgElementWithSource(string source)
        {
            if (source != null && !source.Equals(""))
            {
                foreach (ImgElement img in imgs)
                {
                    if (img.Src.Equals(source))
                    {
                        return img;
                    }
                }
            }
            return null;
        }


        public void LoadContentForImgElements()
        {
            foreach (ImgElement img in imgs)
            {
                img.LoadContent();
            }
        }


        public void LoadContentForImgElements(ContentManager Content)
        {
            ContentManager c = ContentUtils.Instance.Content;
            ContentUtils.Instance.Content = Content;
            LoadContentForImgElements();
            ContentUtils.Instance.Content = c;
        }
    }
}
