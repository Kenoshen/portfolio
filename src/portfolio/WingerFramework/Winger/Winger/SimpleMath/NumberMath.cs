using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Winger.SimpleMath
{
    public static class NumberMath
    {
        public static int GCD(params int[] numbers)
        {
            if (numbers == null)
            {
                return 0;
            }
            else if (numbers.Length == 1)
            {
                return numbers[0];
            }
            else
            {
                return numbers.Aggregate(GCDRecurse);
            }
        }

        private static int GCDRecurse(int a, int b)
        {
            return (b == 0 ? a : GCDRecurse(b, a % b));
        }
    }
}
