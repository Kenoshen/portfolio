using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Winger.Draw
{
    public class FlatBar
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public bool IsUpDownBar { get; set; }
        public bool IsReversed { get; set; }
        public Rectangle Fill { get; private set; }
        public Rectangle Bar { get; private set; }
        public Texture2D Texture { get; set; }
        public Color FillColor { get; set; }
        public Color BarColor { get; set; }
        public int PosX
        {
            get { return Bar.X; }
            set
            {
                int diff = value - Bar.X;
                Rectangle temp;

                temp = Bar;
                temp.X += diff;
                Bar = temp;

                temp = Fill;
                temp.X += diff;
                Fill = temp;
            }
        }
        public int PosY
        {
            get { return Bar.Y; }
            set
            {
                int diff = value - Bar.Y;
                Rectangle temp;

                temp = Bar;
                temp.Y += diff;
                Bar = temp;

                temp = Fill;
                temp.Y += diff;
                Fill = temp;
            }
        }
        public bool FullBar { get; private set; }
        public bool EmptyBar { get; private set; }

        public FlatBar(float x, float y, float width, float height, bool isVertical, bool isReversed, Color fillColor, Color barColor, GraphicsDevice gd)
        {
            GraphicsDevice = gd;
            CreateTexture();
            Bar = new Rectangle((int)x, (int)y, (int)width, (int)height);
            Fill = Bar;
            IsUpDownBar = isVertical;
            IsReversed = isReversed;
            FillColor = fillColor;
            BarColor = barColor;

            FullBar = true;
            EmptyBar = false;
        }

        public float GetFillPercentage()
        {
            if (!IsUpDownBar)
                return (float)Fill.Width / (float)Bar.Width;
            else
                return (float)Fill.Height / (float)Bar.Height;
        }

        /// <summary>
        /// Clamps to 0-1
        /// </summary>
        /// <param name="percentage"></param>
        public void SetFillPercentage(float percentage)
        {
            percentage = MathHelper.Clamp(percentage, 0, 1);
            Rectangle tempFill = Fill;

            if (!IsUpDownBar)
            {
                float fillTo = (float)Bar.Width * percentage;
                tempFill.Width = (int)fillTo;

                if (IsReversed)
                {
                    int diff = Fill.Width - tempFill.Width;
                    tempFill.X += diff;
                }
            }
            else
            {
                float fillTo = (float)Bar.Height * percentage;
                tempFill.Height = (int)fillTo;

                if (IsReversed)
                {
                    int diff = Fill.Height - tempFill.Height;
                    tempFill.Y += diff;
                }
            }

            Fill = tempFill;

            float theFill = GetFillPercentage();
            
            FullBar = false;
            EmptyBar = false;

            if (theFill == 0)
                EmptyBar = true;
            else if (theFill == 1)
                FullBar = true;
        }

        public void AddPercentage(float percentage)
        {
            float totalPercentage = GetFillPercentage() + percentage;
            SetFillPercentage(totalPercentage);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Bar, BarColor);
                spriteBatch.Draw(Texture, Fill, FillColor);
            }
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
    }
}
