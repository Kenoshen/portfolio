using System;
using Microsoft.Xna.Framework;

namespace WPS
{
    public class RingDomain : Domain
    {
        /// <summary>
        /// A domain is a 3D space that is a defined by values that describe the shape and position of the domain.
        /// 
        /// This is a ring-shaped domain that is a single, circular line.
        /// </summary>
        /// <param name="center">the center of the ring</param>
        /// <param name="normal">the normal of the ring (perpendicular to ring)</param>
        /// <param name="radius">the radius of the ring</param>
        public RingDomain(Vector3 center, Vector3 normal, float radius)
        {
            first = center;
            normal.Normalize();
            second = normal;
            third = new Vector3(Math.Abs(radius), 0, 0);

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

            rV *= third.X;

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
        /// Sets the normal
        /// </summary>
        /// <param name="normal">the normal</param>
        public void SetNormal(Vector3 normal)
        {
            normal.Normalize();
            second = normal;
        }

        /// <summary>
        /// Sets the radius
        /// </summary>
        /// <param name="radius">the radius</param>
        public void SetRadius(float radius)
        {
            third.X = Math.Abs(radius);
        }
        
        /// <summary>
        /// Gets the center
        /// </summary>
        /// <returns>the center</returns>
        public Vector3 GetCenter()
        {
            return first;
        }

        /// <summary>
        /// Gets the normal
        /// </summary>
        /// <returns>the normal</returns>
        public Vector3 GetNormal()
        {
            return second;
        }

        /// <summary>
        /// Gets the radius
        /// </summary>
        /// <returns>the radius</returns>
        public float GetRadius()
        {
            return third.X;
        }

        #endregion
    }
}
