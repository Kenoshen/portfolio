using System;
using Microsoft.Xna.Framework;

namespace WPS
{
    public class BoxDomain : Domain
    {
        /// <summary>
        /// A domain is a 3D space that is a defined by values that describe the shape and position of the domain.
        /// 
        /// This is a box-shaped domain (box == cube)
        /// </summary>
        /// <param name="center">the center of the box</param>
        /// <param name="width">the width of the box</param>
        /// <param name="height">the height of the box</param>
        /// <param name="depth">the depth of the box</param>
        public BoxDomain(Vector3 center, float width, float height, float depth)
        {
            first = center;
            second = new Vector3(Math.Abs(width), Math.Abs(height), Math.Abs(depth));
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

            rV *= (second / 2);

            return rV + first;
        }

        /// <summary>
        /// Checks if the given vector is contained within the domain
        /// </summary>
        /// <param name="v">the vector to check</param>
        /// <returns>true if the vector is contained within the domain</returns>
        public override bool IsVectorInDomain(Vector3 v)
        {
            if (first.X - (second.X / 2) <= v.X && v.X <= first.X + (second.X / 2))
                if (first.Y - (second.Y / 2) <= v.Y && v.Y <= first.Y + (second.Y / 2))
                    if (first.Z - (second.Z / 2) <= v.Z && v.Z <= first.Z + (second.Z / 2))
                        return true;
            
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
        /// Set the width
        /// </summary>
        /// <param name="width">the width</param>
        public void SetWidth(float width)
        {
            second.X = Math.Abs(width);
        }

        /// <summary>
        /// Set the height
        /// </summary>
        /// <param name="height">the height</param>
        public void SetHeight(float height)
        {
            second.Y = Math.Abs(height);
        }

        /// <summary>
        /// Set the depth
        /// </summary>
        /// <param name="depth">the depth</param>
        public void SetDepth(float depth)
        {
            second.Z = Math.Abs(depth);
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
        /// Get the width
        /// </summary>
        /// <returns>the width</returns>
        public float GetWidth()
        {
            return second.X;
        }

        /// <summary>
        /// Get the height
        /// </summary>
        /// <returns>the height</returns>
        public float GetHeight()
        {
            return second.Y;
        }

        /// <summary>
        /// Get the depth
        /// </summary>
        /// <returns>the depth</returns>
        public float GetDepth()
        {
            return second.Z;
        }
        #endregion
    }
}
