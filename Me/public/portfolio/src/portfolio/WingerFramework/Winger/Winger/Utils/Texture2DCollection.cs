using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Winger.Utils
{
    public class Texture2DCollection
    {
        private List<string> texturePaths = new List<string>();
        private List<Texture2D> textures = new List<Texture2D>();

        public Texture2DCollection()
        {

        }

        public Texture2DCollection(params string[] texturePaths)
        {
            for (int i = 0; i < texturePaths.Length; i++)
            {
                this.texturePaths.Add(texturePaths[i]);
            }
        }

        public Texture2DCollection(params Texture2D[] textures)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                this.textures.Add(textures[i]);
            }
        }

        public void LoadContent()
        {
            LoadContent(ContentUtils.Instance.Content);
        }

        public void LoadContent(ContentManager Content)
        {
            foreach (string texPath in texturePaths)
            {
                textures.Add(Content.Load<Texture2D>(texPath));
            }
        }

        public void Add(params string[] texturePaths)
        {
            for (int i = 0; i < texturePaths.Length; i++)
            {
                this.texturePaths.Add(texturePaths[i]);
            }
        }

        public void Add(params Texture2D[] textures)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                this.textures.Add(textures[i]);
            }
        }

        public Texture2D this[int i]
        {
            get { return Get(i); }
        }

        public Texture2D Get(int index)
        {
            return textures[index];
        }
    }
}
