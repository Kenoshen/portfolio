using Microsoft.Xna.Framework;

namespace WPS
{
    public class PointDomain : Domain
    {
        /// <summary>
        /// A domain is a 3D space that is a defined by values that describe the shape and position of the domain.
        /// 
        /// This is a single point in space which returns this point when getting a random vector in the domain
        /// </summary>
        /// <param name="point">the point</param>
        public PointDomain(Vector3 point)
        {
            first = point;
            second = Vector3.Zero;
            third = Vector3.Zero;
        }

        /// <summary>
        /// Gets a random position that is contained within the domain
        /// </summary>
        /// <returns>3D position contained within the domain</returns>
        public override Vector3 GetRandomVectorInDomain()
        {
            return first;
        }

        /// <summary>
        /// Checks if the given vector is contained within the domain
        /// </summary>
        /// <param name="v">the vector to check</param>
        /// <returns>true if the vector is contained within the domain</returns>
        public override bool IsVectorInDomain(Vector3 v)
        {
            if (first == v)
                return true;
            return false;
        }

        #region Setters and Getters
        /// <summary>
        /// Sets the point
        /// </summary>
        /// <param name="point">the point</param>
        public void SetPoint(Vector3 point)
        {
            first = point;
        }

        /// <summary>
        /// Gets the point
        /// </summary>
        /// <returns>the point</returns>
        public Vector3 GetPoint()
        {
            return first;
        }
        #endregion
    }
}
