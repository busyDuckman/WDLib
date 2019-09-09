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
    public enum IntegerConversionMode 
    {
        Floor, Ceiling, ToZero, AwayFromZero
    }
    public static class NumberExtensions
    {

        public static int ToInteger(this double d, IntegerConversionMode mode)
        {
            switch (mode)
	        {
		        case IntegerConversionMode.Floor:
                    return (int)Math.Floor(d);
                case IntegerConversionMode.Ceiling:
                    return (int)Math.Ceiling(d);
                case IntegerConversionMode.ToZero:
                    return (d > 0) ? (int)Math.Floor(d) : (int)Math.Ceiling(d);
                case IntegerConversionMode.AwayFromZero:
                    return (d > 0) ? (int)Math.Ceiling(d) : (int)Math.Floor(d);
	        }

            return (int)d;
        }
    }
}
