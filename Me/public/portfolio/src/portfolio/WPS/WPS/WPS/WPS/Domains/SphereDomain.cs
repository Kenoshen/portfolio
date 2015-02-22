using System;
using Microsoft.Xna.Framework;

namespace WPS
{
    public class SphereDomain : Domain
    {
        /// <summary>
        /// A domain is a 3D space that is a defined by values that describe the shape and position of the domain.
        /// 
        /// This is a sphere-shaped domain.
        /// </summary>
        /// <param name="center">center of the sphere</param>
        /// <param name="radius">the radius of the sphere</param>
        public SphereDomain(Vector3 center, float radius)
        {
            first = center;
            second = new Vector3(Math.Abs(radius), 0, 0);
            third = Vector3.Zero;

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
                (float)rnd.Next(-10000, 10000) / 10000f);
            rV.Normalize();

            float scaler = (float)rnd.Next(0, (int)(second.X * 10000)) / 10000f;

            return (rV * scaler) + first;
        }

        /// <summary>
        /// Checks if the given vector is contained within the domain
        /// </summary>
        /// <param name="v">the vector to check</param>
        /// <returns>true if the vector is contained within the domain</returns>
        public override bool IsVectorInDomain(Vector3 v)
        {
            if (Vector3.Distance(first, v) <= second.X)
                return true;
            else
                return false;
        }

        #region Setters and Getters
        /// <summary>
        /// Sets the center
        /// </summary>
        /// <param name="center">the center</param>
        public void SetCenter(Vector3 center)
        {
            first = center;
        }

        /// <summary>
        /// Sets the radius
        /// </summary>
        /// <param name="radius">the radius</param>
        public void SetRadius(float radius)
        {
            second.X = Math.Abs(radius);
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
        /// Gets the radius
        /// </summary>
        /// <returns>the radius</returns>
        public float GetRadius()
        {
            return second.X;
        }
        #endregion
    }
}
