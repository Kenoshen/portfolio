using System;
using Microsoft.Xna.Framework;

namespace WPS
{
    public class LineDomain : Domain
    {
        /// <summary>
        /// A domain is a 3D space that is a defined by values that describe the shape and position of the domain.
        /// 
        /// This is a line-shaped domain.
        /// </summary>
        /// <param name="startPoint">the start point of the line</param>
        /// <param name="endPoint">the end point of the line</param>
        public LineDomain(Vector3 startPoint, Vector3 endPoint)
        {
            first = startPoint;
            second = endPoint;
            third = Vector3.Zero;

            GetFreshRandom();
        }

        /// <summary>
        /// Gets a random position that is contained within the domain
        /// </summary>
        /// <returns>3D position contained within the domain</returns>
        public override Vector3 GetRandomVectorInDomain()
        {
            float lerp = (float)rnd.Next(0, 10000) / 10000f;

            return first + (second - first) * lerp;
        }

        /// <summary>
        /// Checks if the given vector is contained within the domain
        /// </summary>
        /// <param name="v">the vector to check</param>
        /// <returns>true if the vector is contained within the domain</returns>
        public override bool IsVectorInDomain(Vector3 v)
        {
            if (Math.Abs(Vector3.Distance(first, second) - Vector3.Distance(first, v) - Vector3.Distance(v, second)) <= 0.001f)
                return true;

            return false;
        }

        #region Setters and Getters
        /// <summary>
        /// Set the start point
        /// </summary>
        /// <param name="startPoint">the start point</param>
        public void SetStartPoint(Vector3 startPoint)
        {
            first = startPoint;
        }

        /// <summary>
        /// Set the end point
        /// </summary>
        /// <param name="endPoint">the end point</param>
        public void SetEndPoint(Vector3 endPoint)
        {
            second = endPoint;
        }

        /// <summary>
        /// Get the start point
        /// </summary>
        /// <returns>the start point</returns>
        public Vector3 GetStartPoint()
        {
            return first;
        }

        /// <summary>
        /// Get the end point
        /// </summary>
        /// <returns>the end point</returns>
        public Vector3 GetEndPoint()
        {
            return second;
        }
        #endregion
    }
}
