/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WD_toolbox//.DotNetExtension
{
    public static class RandomExtension
    {
        public static bool nextBool(this Random r)
        {
            return r.Next(2) == 0;
        }
    }
}
