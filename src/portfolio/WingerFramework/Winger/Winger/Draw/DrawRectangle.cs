using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Winger.Draw
{
    public class DrawRectangle
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public Texture2D Texture { get; private set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public Vector2 Center
        {
            get
            {
                return new Vector2(X + (Width / 2f), Y + (Height / 2f));
            }

            set
            {
                X = value.X - (Width / 2f);
                Y = value.Y - (Height / 2f);
            }
        }

        public float X { get; set; }

        public float Y { get; set; }

        public Vector2 Position
        {
            get { return new Vector2(X, Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public float Rotation { get; set; }

        public Vector2 TextureCenter
        {
            get
            {
                return new Vector2(Width / 2f, Height / 2f);
            }
        }

        public int LineWeight { get; set; }

        public Color OutlineColor { get; set; }
        public Color FillColor { get; set; }
        public Color RotationMarkerColor { get; set; }

        public bool HasOutline { get; set; }
        public bool HasFill { get; set; }
        public bool HasRotationMarker { get; set; }


        public DrawRectangle(float width, float height, GraphicsDevice gd)
        {
            Initialize();

            Width = width;
            Height = height;

            GraphicsDevice = gd;

            UpdateTexture();
        }


        public DrawRectangle(float width, float height, bool hasOutline, bool hasFill, bool hasRotationMarker, GraphicsDevice gd)
        {
            Initialize();

            Width = width;
            Height = height;
            HasOutline = hasOutline;
            HasFill = hasFill;
            HasRotationMarker = hasRotationMarker;

            GraphicsDevice = gd;

            UpdateTexture();
        }

        private void Initialize()
        {
            Width = 1;
            Height = 1;
            X = 0;
            Y = 0;
            Rotation = 0;
            LineWeight = 1;
            OutlineColor = Color.Black;
            FillColor = Color.White;
            RotationMarkerColor = Color.Black;
            HasOutline = false;
            HasFill = true;
            HasRotationMarker = false;
        }

        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Center, null, Color.White, Rotation, TextureCenter, 1, SpriteEffects.None, 1);
            }
        }


        public void UpdateTexture()
        {
            int w = (int) Width;
            int h = (int) Height;
            if (w == 0 || h == 0)
            {
                Texture = null;
                return;
            }
            Texture2D texture = new Texture2D(GraphicsDevice, w, h);

            Color[] data = new Color[w * h];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            Vector2 center = TextureCenter;
            if (HasFill || HasRotationMarker || HasOutline)
            {
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        // Draw the fill color
                        if (HasFill)
                        {
                            data[y * w + x] = FillColor;
                        }
                        // Draw the rotation marker
                        if (HasRotationMarker)
                        {
                            if ((int)center.X == x && (int)center.Y >= y)
                            {
                                data[y * w + x] = RotationMarkerColor;
                            }
                        }
                        // Draw the outline
                        if (HasOutline)
                        {
                            if (x < LineWeight || y < LineWeight || x >= w - LineWeight || y >= h - LineWeight)
                            {
                                data[y * w + x] = OutlineColor;
                            }
                        }
                    }
                }
            }
            texture.SetData(data);
            Texture = texture;
        }
    }
}
