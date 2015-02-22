using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Winger.Utils;

namespace Winger.Utils
{
    public class TextureManager
    {
        #region Singleton
        private static TextureManager instance;

        private TextureManager() { }

        public static TextureManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TextureManager();
                }
                return instance;
            }
        }
        #endregion

        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();


        public List<string> AddTexturesInDirectory(string absoluteDirectoryPath)
        {
            List<string> imageFilePaths = Directory.GetFiles(absoluteDirectoryPath, "*.*")
                .Where(file => file.ToLower().EndsWith("bmp") || 
                    file.ToLower().EndsWith("jpg") ||
                    file.ToLower().EndsWith("png") ||
                    file.ToLower().EndsWith("tga") || 
                    file.ToLower().EndsWith("dds")).ToList<string>();

            List<string> imageNames = new List<string>();

            foreach (string image in imageFilePaths)
            {
                string imageName = Path.GetFileNameWithoutExtension(image);
                imageNames.Add(imageName);
                FileStream stream = File.Open(image, FileMode.Open);
                try
                {
                    Instance.PutTexture(imageName, Texture2D.FromStream(GraphicsUtils.Instance.GraphicsDevice, stream));
                }
                finally
                {
                    stream.Close();
                }
            }

            // add sub directories
            List<string> directories = new List<string>(Directory.EnumerateDirectories(absoluteDirectoryPath));
            foreach (string directory in directories)
            {
                imageNames.AddRange(AddTexturesInDirectory(directory));
            }

            return imageNames;
        }

        public List<string> AddSpriteStripsInDirectory(string absoluteDirectoryPath)
        {
            List<string> imageFilePaths = Directory.GetFiles(absoluteDirectoryPath, "*.*")
                .Where(file => file.ToLower().EndsWith("bmp") ||
                    file.ToLower().EndsWith("jpg") ||
                    file.ToLower().EndsWith("png") ||
                    file.ToLower().EndsWith("tga") ||
                    file.ToLower().EndsWith("dds")).ToList<string>();

            List<string> imageNames = new List<string>();

            foreach (string image in imageFilePaths)
            {
                string imageName = Path.GetFileNameWithoutExtension(image);
                imageName = imageName.Substring(imageName.IndexOf("_") + 1);

                imageNames.Add(imageName);
                FileStream stream = File.Open(image, FileMode.Open);
                try
                {
                    Instance.PutTexture(imageName, Texture2D.FromStream(GraphicsUtils.Instance.GraphicsDevice, stream));
                }
                finally
                {
                    stream.Close();
                }
            }

            // add sub directories
            List<string> directories = new List<string>(Directory.EnumerateDirectories(absoluteDirectoryPath));
            foreach (string directory in directories)
            {
                imageNames.AddRange(AddTexturesInDirectory(directory));
            }

            return imageNames;
        }


        public void PutTexture(string name, Texture2D texture)
        {
            textures[name] = texture;
        }


        public Texture2D GetTexture(string name)
        {
            if (textures.ContainsKey(name))
            {
                return textures[name];
            }
            return null;
        }


        public bool RemoveTexture(string name)
        {
            if (textures.ContainsKey(name))
            {
                return textures.Remove(name);
            }
            return false;
        }
    }
}
