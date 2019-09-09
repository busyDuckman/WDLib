/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD_toolbox
{
    public static  class ColorExtension
    {
        public static Color WithNewAlpha(this Color color, int alpha)
        {
            return Color.FromArgb(alpha, color);
        }

        /// <summary>
        /// Quickly gets a luminance (2 parts red, 1 part blue, 3 parts green
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static byte GetQuickLuminance(this Color color)
        {
            return  (byte) ((color.R + color.R + 
                             color.B +
                             color.G + color.G + color.G
                             ) / 6);
        }
    }
}
