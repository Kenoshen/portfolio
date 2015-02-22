using System;
using Microsoft.Xna.Framework;

namespace WPS
{
    public class SquareDomain : Domain
    {
        /// <summary>
        /// A domain is a 3D space that is a defined by values that describe the shape and position of the domain.
        /// 
        /// This is a 2D-square-shape in 3D domain.
        /// </summary>
        /// <param name="center">the center of the square</param>
        /// <param name="normal">the normal of the square (perpendicular to the surface of the square)</param>
        /// <param name="width">the width of the square</param>
        /// <param name="height">the height of the square</param>
        public SquareDomain(Vector3 center, Vector3 normal, float width, float height)
        {
            first = center;
            normal.Normalize();
            second = normal;
            third = new Vector3(Math.Abs(width), Math.Abs(height), 0);

            GetFreshRandom();
        }

        /// <summary>
        /// Gets a random position that is contained within the domain
        /// </summary>
        /// <returns>3D position contained within the domain</returns>
        public override Vector3 GetRandomVectorInDomain()
        {
            Vector3 rV = new Vector3(
                (float)rnd.Next(-10000, 10000) / 10000f,
                (float)rnd.Next(-10000, 10000) / 10000f,
                0);

            rV *= (third / 2);

            Vector3 cross = Vector3.Cross(Vector3.UnitZ, second);
            float dot = Vector3.Dot(Vector3.UnitZ, second);

            rV = Vector3.Transform(rV, Matrix.CreateFromAxisAngle(cross, MathHelper.ToRadians((1 - dot) * 90)));

            return rV + first;
        }

        /// <summary>
        /// Checks if the given vector is contained within the domain *currently not implemented
        /// </summary>
        /// <param name="v">the vector to check</param>
        /// <returns>true if the vector is contained within the domain</returns>
        public override bool IsVectorInDomain(Vector3 v)
        {
            return false;
        }

        #region Setters and Getters

        /// <summary>
        /// Set the center
        /// </summary>
        /// <param name="center">the center</param>
        public void SetCenter(Vector3 center)
        {
            first = center;
        }

        /// <summary>
        /// Set the normal
        /// </summary>
        /// <param name="normal">the normal</param>
        public void SetNormal(Vector3 normal)
        {
            normal.Normalize();
            second = normal;
        }
        
        /// <summary>
        /// Set the width
        /// </summary>
        /// <param name="width">the width</param>
        public void SetWidth(float width)
        {
            third.X = Math.Abs(width);
        }

        /// <summary>
        /// Set the height
        /// </summary>
        /// <param name="height">the height</param>
        public void SetHeight(float height)
        {
            third.Y = Math.Abs(height);
        }
        
        /// <summary>
        /// Get the center
        /// </summary>
        /// <returns>the center</returns>
        public Vector3 GetCenter()
        {
            return first;
        }

        /// <summary>
        /// Get the normal
        /// </summary>
        /// <returns>the normal</returns>
        public Vector3 GetNormal()
        {
            return second;
        }

        /// <summary>
        /// Get the width
        /// </summary>
        /// <returns>the width</returns>
        public float GetWidth()
        {
            return third.X;
        }
        
        /// <summary>
        /// Get the height
        /// </summary>
        /// <returns>the height</returns>
        public float GetHeight()
        {
            return third.Y;
        }

        #endregion
    }
}
