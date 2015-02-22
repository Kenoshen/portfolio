using System;
using Winger.SimpleMath;

namespace Winger.Network.Encryption.RSA
{
    public static class RSAFactory
    {
        private static Random rnd = new Random();

        private const int min = 1000;
        private const int max = 10000;

        public static void GenerateRSAToken()
        {
            int p = EncryptionUtils.RandomPrimeNumber(min, max);
            int q = EncryptionUtils.RandomPrimeNumber(min, max);
            int n = p * q;

            int eulerN = (p - 1) * (q - 1);
            int e = rnd.Next(eulerN - 2) + 1;
            int attempts = 0;
            while (NumberMath.GCD(eulerN, e) != 1)
            {
                e = rnd.Next(eulerN - 2) + 1;
                attempts++;
            }
            Console.WriteLine(e);
        }
    }
}
