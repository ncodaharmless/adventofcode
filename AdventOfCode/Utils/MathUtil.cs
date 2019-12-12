using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils
{
    public class MathUtil
    {
        /// <summary>
        /// Least Common Multiple
        /// </summary>
        public static long LCM(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }
        /// <summary>
        /// Greatest Common Divisor / Factor
        /// </summary>
        public static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
        /// <summary>
        /// Least Common Multiple
        /// </summary>
        public static long LCM(long[] numbers)
        {
            return numbers.Aggregate(LCM);
        }

    }
}
