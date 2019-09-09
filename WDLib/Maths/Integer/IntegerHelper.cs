/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD_toolbox.Maths.Integer
{
    public static class IntegerHelper
    {
        private static ulong NegaativeLongMin = ((ulong)long.MaxValue) + (ulong)1;

        public static long[] FindDivisors(long n)
        {
            if (n == 0)
            {
                return new long[] { 0 };
            }
            if(n == long.MinValue) //2's compliment min cant be made positive
            {
                return DivisorsNegativeFix(FindDivisors(NegaativeLongMin));

            }
            if (n < 0)
            {
                return DivisorsNegativeFix(FindDivisors((ulong)Math.Abs(n)));
            }
            else
            {
                ulong[] divisors = FindDivisors((ulong)n);
                return (from N in divisors select (long)N).ToArray();
            }
        }

        /// <summary>
        /// All divisors of a number, 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static ulong[] FindDivisors(ulong n)
        {
            List<ulong> res = new List<ulong>();
            if (n == 0)
            {
                return new ulong[] { 0 };
            }
            else
            {
                //This algorithe looks slow. Shoul hunt around for something better.
                ulong i = 1;
                ulong sqrtN = (ulong)Math.Sqrt(n);
                while (i <= sqrtN)
                {
                    if (n % i == 0)
                    {
                        res.Add(i);
                        if (i != (n / i))
                        {
                            res.Add(n / i);
                        }
                    }
                    i++;
                }

                res.Sort();
                return res.ToArray();
            }
        }

        private static long[] DivisorsNegativeFix(ulong[] nums)
        {
            List<long> res = new List<long>();
            foreach (ulong num in nums)
            {
                if (num == NegaativeLongMin)
                {
                    res.Add(long.MinValue);
                }
                if (num < long.MaxValue)
                {
                    res.Add((long)num);
                    res.Add(-((long)num));
                }
                else
                {
                    //skip, too long
                }
            }

            res.Sort();
            return res.ToArray();
        }
    }
}
