using System;
using Microsoft.Xna.Framework;

namespace Winger.SimpleMath
{
    public static class VectorMath
    {
        public static float AngleInRadians(Vector2 v1, Vector2 v2)
        {
            float perpDot = (v1.X * v2.Y) - (v1.Y * v2.X);
            return (float)Math.Atan2(perpDot, Vector2.Dot(v1, v2));
        }


        public static Vector2 RotatePointAroundZero(Vector2 point, float rotation)
        {
            return RotatePointAroundZero(point.X, point.Y, rotation);
        }


        public static Vector2 RotatePointAroundZero(float x, float y, float rotation)
        {
            float rotX = (float)Math.Cos(rotation) * x - (float)Math.Sin(rotation) * y;
            float rotY = (float)Math.Sin(rotation) * x + (float)Math.Cos(rotation) * y;
            return new Vector2(rotX, rotY);
        }


        public static bool IsPointInTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 point)
        {
            float ab = Sign(point, a, b);
            float bc = Sign(point, b, c);
            float ca = Sign(point, c, a);
            bool b1 = ab < 0f;
            bool b2 = bc < 0f;
            bool b3 = ca < 0f;
            return (b1 == b2 && b2 == b3);
        }


        private static float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }
    }
}
