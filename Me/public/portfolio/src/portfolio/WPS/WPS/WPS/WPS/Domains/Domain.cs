using System;
using Microsoft.Xna.Framework;

namespace WPS
{
    public abstract class Domain
    {
        protected Vector3 first;
        protected Vector3 second;
        protected Vector3 third;
        protected Random rnd;

        public virtual Vector3 GetRandomVectorInDomain()
        {
            return Vector3.Zero;
        }

        public virtual bool IsVectorInDomain(Vector3 v)
        {
            return false;
        }

        protected void GetFreshRandom()
        {
            rnd = new Random();
            for (int i = 0; i < 100; i++)
                rnd.Next();
        }
    }
}
