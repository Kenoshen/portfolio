using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Winger.Utils;

namespace Winger.Draw
{
    public class DrawLineRectangle
    {
        public GraphicsDevice GraphicsDevice { get; set; }
        public Texture2D Texture { get; private set; }

        private CRectangle rect = CRectangle.Empty;

        public float X
        {
            get { return rect.X; }
            set { rect.X = value; UpdateLinesPosition(); }
        }

        public float Y
        {
            get { return rect.Y; }
            set { rect.Y = value; UpdateLinesPosition(); }
        }

        public float Width
        {
            get { return rect.Width; }
            set { rect.Width = value; rect.SetOriginAtCenter(); UpdateLinesPosition(); }
        }

        public float Height
        {
            get { return rect.Height; }
            set { rect.Height = value; rect.SetOriginAtCenter(); UpdateLinesPosition(); }
        }

        public float Rotation
        {
            get { return rect.Rotation; }
            set { rect.Rotation = value; UpdateLinesPosition(); }
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2(X, Y);
            }

            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Vector2 Position
        {
            get { return new Vector2(X - (Width / 2f), Y - (Height / 2f)); }
            set
            {
                X = value.X + (Width / 2f);
                Y = value.Y + (Height / 2f);
            }
        }

        public Vector2 TextureCenter
        {
            get { return rect.Origin; }
        }

        public int LineWeight
        {
            get { return lineA.LineWeight; }
            set { UpdateLineWeight(value); }
        }

        public Color OutlineColor
        {
            get { return lineA.Color; }
            set { UpdateLinesColor(value); }
        }
        public Color FillColor { get; set; }
        public Color RotationMarkerColor
        {
            get { return lineE.Color; }
            set { UpdateMarkerColor(value); }
        }

        public bool HasOutline { get; set; }
        public bool HasFill { get; set; }
        public bool HasRotationMarker { get; set; }


        private DrawLine lineA = null;
        private DrawLine lineB = null;
        private DrawLine lineC = null;
        private DrawLine lineD = null;
        private DrawLine lineE = null;


        public DrawLineRectangle(float width, float height, GraphicsDevice gd)
        {
            Initialize(gd);

            Width = width;
            Height = height;
        }

        public DrawLineRectangle(float width, float height, bool hasOutline, bool hasFill, bool hasRotationMarker, GraphicsDevice gd)
        {
            Initialize(gd);

            Width = width;
            Height = height;
            HasOutline = hasOutline;
            HasFill = hasFill;
            HasRotationMarker = hasRotationMarker;
        }

        private void Initialize(GraphicsDevice gd)
        {
            GraphicsDevice = gd;
            CreateTexture();
            InitLines();

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

        private void InitLines()
        {
            lineA = new DrawLine(GraphicsDevice);
            lineB = new DrawLine(GraphicsDevice);
            lineC = new DrawLine(GraphicsDevice);
            lineD = new DrawLine(GraphicsDevice);
            lineE = new DrawLine(GraphicsDevice);
        }

        private void UpdateLinesPosition()
        {
            Vector2[] points = rect.Points;
            lineA.StartPos = points[0];
            lineA.EndPos = points[1];
            lineB.StartPos = points[1];
            lineB.EndPos = points[2];
            lineC.StartPos = points[2];
            lineC.EndPos = points[3];
            lineD.StartPos = points[3];
            lineD.EndPos = points[0];

            lineE.StartPos = Center;
            lineE.EndPos = new Vector2(0, -Height / 2f);
            lineE.EndPos = SimpleMath.VectorMath.RotatePointAroundZero(lineE.EndPos, Rotation) + lineE.StartPos;
        }

        private void UpdateLinesColor(Color color)
        {
            lineA.Color = color;
            lineB.Color = color;
            lineC.Color = color;
            lineD.Color = color;
        }

        private void UpdateMarkerColor(Color color)
        {
            lineE.Color = color;
        }

        private void UpdateLineWeight(int weight)
        {
            lineA.LineWeight = weight;
            lineB.LineWeight = weight;
            lineC.LineWeight = weight;
            lineD.LineWeight = weight;

            lineE.LineWeight = weight;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (HasFill)
            {
                spriteBatch.Draw(Texture, rect.Rect, null, FillColor, rect.Rotation, new Vector2(1, 1), SpriteEffects.None, 1);
            }
            if (HasOutline)
            {
                lineA.Draw(spriteBatch);
                lineB.Draw(spriteBatch);
                lineC.Draw(spriteBatch);
                lineD.Draw(spriteBatch);
            }
            if (HasRotationMarker)
            {
                lineE.Draw(spriteBatch);
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
