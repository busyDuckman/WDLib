/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WD_toolbox.Maths.Range;

namespace WD_toolbox.DotNetExtension
{
    public static class Int32Extension
    {
        public static Int32 Clamp(this Int32 value, Int32 minInclusive, Int32 maxInclusive)
        {
            if (value < minInclusive)
            {
                return minInclusive;
            }
            if (value > maxInclusive)
            {
                return maxInclusive;
            }
            return value;
        }

    }
}
