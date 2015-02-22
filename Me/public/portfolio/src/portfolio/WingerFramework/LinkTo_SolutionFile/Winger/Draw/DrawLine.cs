using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Winger.Draw
{
    public class DrawLine
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public Texture2D Texture { get; private set; }

        private Vector2 posS = Vector2.Zero;
        public Vector2 StartPos
        {
            get { return posS; }

            set
            {
                posS = value;
                UpdateLength();
                UpdateRotation();
            }
        }

        private Vector2 posE = Vector2.Zero;
        public Vector2 EndPos
        {
            get { return posE; }

            set
            {
                posE = value;
                UpdateLength();
                UpdateRotation();
            }
        }

        public Color Color { get; set; }


        public int LineWeight { get; set; }


        public float Length { get; private set; }


        public float Rotation { get; private set; }


        /// <summary>
        /// Creates a line with default start and end and color
        /// </summary>
        public DrawLine(GraphicsDevice gd)
        {
            GraphicsDevice = gd;
            Initialize();
            UpdateLength();
            UpdateRotation();
        }

        /// <summary>
        /// Creates a line that stretches from start to end point with default color
        /// </summary>
        /// <param name="startPos">first point of the line</param>
        /// <param name="endPos">end point of the line</param>
        public DrawLine(Vector2 startPos, Vector2 endPos, GraphicsDevice gd)
        {
            GraphicsDevice = gd;
            Initialize();
            StartPos = startPos;
            EndPos = endPos;
            UpdateLength();
            UpdateRotation();
        }

        /// <summary>
        /// Draws a line that stretches from start to end with a given color
        /// </summary>
        /// <param name="startPos">start point of the line</param>
        /// <param name="endPos">end point of the line</param>
        /// <param name="color">color to draw the line</param>
        public DrawLine(Vector2 startPos, Vector2 endPos, Color color, GraphicsDevice gd)
        {
            GraphicsDevice = gd;
            Initialize();
            StartPos = startPos;
            EndPos = endPos;
            UpdateLength();
            UpdateRotation();
            Color = color;
        }


        public void Initialize()
        {
            StartPos = Vector2.Zero;
            EndPos = Vector2.One;
            Color = Color.Black;
            LineWeight = 1;
            CreateTexture();
        }


        private void UpdateLength()
        {
            Length = Vector2.Distance(StartPos, EndPos);
        }


        private void UpdateRotation()
        {
            Rotation = (float)Math.Atan2((EndPos.Y - StartPos.Y), (EndPos.X - StartPos.X));
        }


        private void CreateTexture()
        {
            int w = 2;
            int h = 2;
            Texture2D texture = new Texture2D(GraphicsDevice, w, h);

            Color[] data = new Color[w * h];

            for (int i = 0; i < data.Length; i++)
                data[i] = Color.White;

            texture.SetData(data);
            Texture = texture;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,        // Texture2D
                StartPos,       // Vector2 Position
                null,           // Draw Rectangle
                Color,          // Color
                Rotation,          // Rotation
                new Vector2(0, 1),        // Vector2 Origin
                new Vector2(Length / 2f, (float)LineWeight / 2f),         // Vector2 Scale
                SpriteEffects.None,         // SpriteEffects
                1);         // Layer Depth
        }
    }
}
