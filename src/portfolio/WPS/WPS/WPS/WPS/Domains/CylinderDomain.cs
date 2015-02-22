using System;
using Microsoft.Xna.Framework;

namespace WPS
{
    public class CylinderDomain : Domain
    {
        /// <summary>
        /// A domain is a 3D space that is a defined by values that describe the shape and position of the domain.
        /// 
        /// This is a cylinder-shaped domain
        /// </summary>
        /// <param name="center">center of the cylinder</param>
        /// <param name="normal">the normal of the cylinder (the normal runs up the length of the cylinder)</param>
        /// <param name="length">the length of the cylinder</param>
        /// <param name="outerRadius">the outer radius of the cylinder</param>
        /// <param name="innerRadius">the inner radius of the cylinder (can create a solid cylinder by using 0)</param>
        public CylinderDomain(Vector3 center, Vector3 normal, float length, float outerRadius, float innerRadius)
        {
            first = center;
            normal.Normalize();
            second = normal;
            third = new Vector3(Math.Abs(length), Math.Abs(outerRadius), Math.Abs(innerRadius));

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
            rV.Normalize();

            rV *= (float)rnd.Next((int)(third.Z * 10000f), (int)(third.Y * 10000f)) / 10000f;

            rV.Z = (float)rnd.Next(-10000, 10000) / 10000f;
            rV.Z *= (third.X / 2);

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
        /// Set the length
        /// </summary>
        /// <param name="length">the length</param>
        public void SetLength(float length)
        {
            third.X = Math.Abs(length);
        }

        /// <summary>
        /// Set the outer radius
        /// </summary>
        /// <param name="outerRadius">the outer radius</param>
        public void SetOuterRadius(float outerRadius)
        {
            third.Y = Math.Abs(outerRadius);
        }

        /// <summary>
        /// Set the inner radius
        /// </summary>
        /// <param name="innerRadius">the inner radius</param>
        public void SetInnerRadius(float innerRadius)
        {
            third.Z = Math.Abs(innerRadius);
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
        /// Get the length
        /// </summary>
        /// <returns>the length</returns>
        public float GetLength()
        {
            return third.X;
        }

        /// <summary>
        /// Get the outer radius
        /// </summary>
        /// <returns>the outer radius</returns>
        public float GetOuterRadius()
        {
            return third.Y;
        }

        /// <summary>
        /// Get the inner radius
        /// </summary>
        /// <returns>the inner radius</returns>
        public float GetInnerRadius()
        {
            return third.Z;
        }

        #endregion
    }
}
