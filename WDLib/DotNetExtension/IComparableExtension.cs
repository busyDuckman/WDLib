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

namespace WD_toolbox
{
    public static class IComparableExtension
    {
        public static T Min<T>(this T a, T b)
            where T : IComparable<T>
        {
            return (a.CompareTo(b) < 0) ? a : b;
        }

        public static T Max<T>(this T a, T b)
            where T : IComparable<T>
        {
            return (b.CompareTo(a) > 0) ? b : a;
        }
    }
}
