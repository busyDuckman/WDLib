/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using WD_toolbox.Maths.Geometry;

namespace WD_toolbox//.DotNetExtension
{
    public static class RectangleExtension
    {
        public static int CalculateSurfaceArea(this Rectangle rec)
        {
            return rec.Width * rec.Height;
        }

        public static Point2D Middle(this Rectangle rec)
        { 
            double mx = rec.X + (rec.Width/2.0);
            double my = rec.Y + (rec.Height/2.0);
            return new Point2D(mx, my);
        }
    }
}
