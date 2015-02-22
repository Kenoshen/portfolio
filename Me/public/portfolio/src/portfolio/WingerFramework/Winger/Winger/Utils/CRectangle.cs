using System;
using System.Text;
using Microsoft.Xna.Framework;

namespace Winger.Utils
{
    public class CRectangle
    {
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }
        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }
        public Rectangle Rect
        {
            get
            {
                _rect.X = (int)X;
                _rect.Y = (int)Y;
                _rect.Width = (int)Width;
                _rect.Height = (int)Height;
                return _rect;
            }
        }
        public Vector2[] Points
        {
            get
            {
                Vector2[] points = new Vector2[4];
                float localX = -Origin.X;
                float localY = -Origin.Y;
                points[0] = new Vector2(localX, localY);
                points[1] = new Vector2(localX + Width, localY);
                points[2] = new Vector2(localX + Width, localY + Height);
                points[3] = new Vector2(localX, localY + Height);

                if (Rotation != 0)
                {
                    points[0] = SimpleMath.VectorMath.RotatePointAroundZero(points[0], Rotation);
                    points[1] = SimpleMath.VectorMath.RotatePointAroundZero(points[1], Rotation);
                    points[2] = SimpleMath.VectorMath.RotatePointAroundZero(points[2], Rotation);
                    points[3] = SimpleMath.VectorMath.RotatePointAroundZero(points[3], Rotation);
                }

                Vector2 worldPoint = new Vector2(X, Y);
                points[0] += worldPoint;
                points[1] += worldPoint;
                points[2] += worldPoint;
                points[3] += worldPoint;

                return points;
            }
        }

        private float _x = 0;
        private float _y = 0;
        private float _width = 0;
        private float _height = 0;
        private Vector2 _origin = Vector2.Zero;
        private float _rotation = 0;
        private Rectangle _rect = Rectangle.Empty;

        public CRectangle(Rectangle rectangle)
        {
            Initialize(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, Vector2.Zero, 0);
        }

        public CRectangle(float x, float y, float width, float height)
        {
            Initialize(x, y, width, height, Vector2.Zero, 0);
        }

        public CRectangle(float x, float y, float width, float height, Vector2 origin)
        {
            Initialize(x, y, width, height, origin, 0);
        }

        public CRectangle(float x, float y, float width, float height, Vector2 origin, float rotation)
        {
            Initialize(x, y, width, height, origin, rotation);
        }

        public CRectangle(float x, float y, float width, float height, float rotation)
        {
            Initialize(x, y, width, height, Vector2.Zero, rotation);
        }

        private void Initialize(float x, float y, float width, float height, Vector2 origin, float rotation)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Origin = origin;
            Rotation = rotation;
        }

        public void SetOriginAtCenter()
        {
            Origin = new Vector2(Width / 2f, Height / 2f);
        }

        public bool Contains(Point point)
        {
            return Contains(point.X, point.Y);
        }

        public bool Contains(int x, int y)
        {
            return Contains((float)x, (float)y);
        }

        public bool Contains(Vector2 point)
        {
            return Contains(point.X, point.Y);
        }

        public bool Contains(float x, float y)
        {
            if (Rotation == 0)
            {
                return Rect.Contains((int)(x + Origin.X), (int)(y + Origin.Y));
            }
            else
            {
                float localX = x - X;
                float localY = y - Y;

                Vector2 rot = SimpleMath.VectorMath.RotatePointAroundZero(localX, localY, -Rotation);

                return Rect.Contains((int)(rot.X + X + Origin.X), (int)(rot.Y + Y + Origin.Y));
            }
        }

        public bool Contains(Rectangle rectangle)
        {
            if (Rotation == 0)
            {
                return Rect.Contains(rectangle);
            }
            else
            {
                CRectangle temp = new CRectangle(rectangle);
                Vector2[] points = temp.Points;
                for (int i = 0; i < points.Length; i++)
                    if (!Contains(points[i]))
                        return false;
                return true;
            }
        }

        public bool Contains(CRectangle rectangle)
        {
            Vector2[] points = rectangle.Points;
            for (int i = 0; i < points.Length; i++)
                if (!Contains(points[i]))
                    return false;
            return true;
        }

        public bool Intersects(Rectangle rectangle)
        {
            return Intersects(new CRectangle(rectangle));
        }

        public bool Intersects(CRectangle rectangle)
        {
            Vector2[] points = rectangle.Points;
            bool[] results = new bool[4];
            for (int i = 0; i < points.Length; i++)
                results[i] = Contains(points[i]);

            if (results[0] && results[1] && results[2] && results[3])
            {
                return false;
            }
            else if (!results[0] && !results[1] && !results[2] && !results[3])
            {
                return false;
            }
            else
                return true;
        }

        public CRectangle Clone()
        {
            CRectangle clone = new CRectangle(X, Y, Width, Height, Origin, Rotation);
            return clone;
        }

        public static CRectangle Empty
        {
            get { return new CRectangle(Rectangle.Empty); }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            sb.Append("x:" + Math.Round(X, 2) + ", ");
            sb.Append("y:" + Math.Round(Y, 2) + ", ");
            sb.Append("width:" + Math.Round(Width, 2) + ", ");
            sb.Append("height:" + Math.Round(Height, 2) + ", ");
            sb.Append("rotation:" + Math.Round(Rotation, 2) + ", ");
            sb.Append("origin:(" + Math.Round(Origin.X, 2) + ", " + Math.Round(Origin.Y, 2) + "]");
            return sb.ToString();
        }
    }
}
