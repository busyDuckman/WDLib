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
using System.Windows.Forms;
using WD_toolbox.Maths.Geometry;

namespace WDLibApplicationFramework.ViewAndEdit2D
{
    public class MouseEventWorldSpace
    {
        public delegate void WSMouseEventHandler(object sender, MouseEventWorldSpace e);

        public MouseButtons Button { get; private set; }
        public Point2D location { get; private set; }

        public MouseEventWorldSpace(int x, int y, MouseButtons button, IView2D view)
        {
            location = view.ScreenSpace2WorldSpace(new Point2D(x, y));
            this.Button = button;
        }
    }
}
