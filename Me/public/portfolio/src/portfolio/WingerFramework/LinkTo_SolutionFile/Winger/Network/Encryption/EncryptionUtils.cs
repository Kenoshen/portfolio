using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Winger.Network.Encryption
{
    public static class EncryptionUtils
    {
        private static Random rnd = new Random();

        //private static long min = 3474749660383;
        //private static long max = 341550071728321;
        //private static long dif = max - min;
        //private static int[] aVals = new int[] { 2, 3, 4, 7, 11, 13, 17 };


        public static short RandomPrimeNumber(short minP, short maxP)
        {
            if (minP < maxP)
            {
                short difP = (short)(maxP - minP);
                for (int i = 0; i < 100; i++)
                {
                    double perc = rnd.NextDouble();
                    short p = (short)(minP + (difP * perc));
                    if (TestNumberForPrimalitySmall(p))
                    {
                        return p;
                    }
                }
            }
            return 0;
        }


        private static bool TestNumberForPrimalitySmall(short p)
        {
            if (p < 0)
            {
                p = Math.Abs(p);
            }
            if (p == 2)
            {
                return true;
            }
            if (p % 2 == 0)
            {
                return false;
            }
            short sqrt = (short)Math.Sqrt(p);
            for (short i = 3; i <= sqrt; i += 2)
            {
                if (p % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
