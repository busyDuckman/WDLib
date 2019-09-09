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

namespace WD_toolbox.Rendering
{
    public static class FontFactory
    {
        /// <summary>
        /// A font creation routine that is better suited to raster orientated work
        /// </summary>
        /// <param name="fontFamily"></param>
        /// <param name="style"></param>
        /// <param name="pixelHeight"> Font height, in pixels</param>
        /// <param name="intergralHeight"> True if the font size should be an integer</param>
        /// <returns></returns>
        public static Font GetFontByTotalHeight(FontFamily fontFamily, FontStyle style, int pixelHeight, bool intergralHeight = true)
        {
            double ascent = fontFamily.GetCellAscent(FontStyle.Regular);
            double descent = fontFamily.GetCellDescent(FontStyle.Regular);

            double fontTestSize = 48;
            double ascentPixel = fontTestSize * ascent / fontFamily.GetEmHeight(FontStyle.Regular);
            double descentPixel = fontTestSize * descent / fontFamily.GetEmHeight(FontStyle.Regular);

            //remove +descentPixel line for basline
            double fontTestSizePixels = ascentPixel + descentPixel;
            
            double fontSize = (fontTestSize / fontTestSizePixels) * (double) pixelHeight;

            fontSize *= 0.8;  //kludge

            return new Font(fontFamily, intergralHeight ? ((float)Math.Floor(fontSize)) : ((float)fontSize), style);
        }
    }
}
