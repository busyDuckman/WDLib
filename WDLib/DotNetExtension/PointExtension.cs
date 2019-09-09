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
using WD_toolbox.Maths.Space;

namespace WD_toolbox//.DotNetExtension
{
    public static class PointExtension
    {
        /// <summary>
        /// Gets a point (in screen space) 1 unit above this point
        /// </summary>
        /// <returns>new Point(p.X,   p.Y-1)</returns>
        public static Point getPointAbove(this Point p) { return new Point(p.X,   p.Y-1); }

        /// <summary>
        /// Gets a point (in screen space) 1 unit below this point
        /// </summary>
        /// <returns>new Point(p.X,   p.Y+1)</returns>
        public static Point getPointBelow(this Point p) { return new Point(p.X, p.Y + 1); }

        /// <summary>
        /// Gets a point (in screen space) 1 unit left of this point
        /// </summary>
        /// <returns>new Point(p.X-1, p.Y)</returns>
        public static Point getPointLeft(this Point p) { return new Point(p.X - 1, p.Y); }

        /// <summary>
        /// Gets a point (in screen space) 1 unit right of this point
        /// </summary>
        /// <returns>new Point(p.X+1, p.Y)</returns>
        public static Point getPointRight(this Point p) { return new Point(p.X + 1, p.Y); }


        public static Point NextPoint(this Point p, Dir2D dir)
        {
            switch (dir)
            {
                case Dir2D.None:
                    return p;
                case Dir2D.Up:
                    return p.getPointAbove();
                case Dir2D.Down:
                    return p.getPointBelow();
                case Dir2D.Left:
                    return p.getPointLeft();
                case Dir2D.Right:
                    return p.getPointRight();
                default:
                    return p;
            }
        }

        public static Point NextPoint(this Point p, Dir2D dir, int steps)
        {
            switch (dir)
            {
                case Dir2D.None:
                    return p;
                case Dir2D.Up:
                    return new Point(p.X, p.Y - steps);
                case Dir2D.Down:
                    return new Point(p.X, p.Y + steps);
                case Dir2D.Left:
                    return new Point(p.X - steps, p.Y);
                case Dir2D.Right:
                    return new Point(p.X + steps, p.Y);
                default:
                    return p;
            }
        }
    }
}
