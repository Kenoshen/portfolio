using System;

namespace Winger.Utils
{
    public static class Timestamp
    {
        public static long Now
        {
            get { return DateTime.Now.ToFileTimeUtc() / 10000; }
        }
    }
}
