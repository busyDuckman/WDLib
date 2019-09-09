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
using WD_toolbox.Maths.Geometry;
using WD_toolbox.Rendering;

namespace WDLibApplicationFramework.ViewAndEdit2D.Grid
{
    public class SimpleGrid : ViewGridBase
    {
        double XGapWorldSpace { get; set; }
        double YGapWorldSpace { get; set; }


        public SimpleGrid(int xGapWorldSpace, int yGapWorldSpace)
            : base()
        {
            this.XGapWorldSpace = xGapWorldSpace;
            this.YGapWorldSpace = yGapWorldSpace;
        }

        public SimpleGrid(int gapWorldSpace)
            : this(gapWorldSpace, gapWorldSpace)
        {
        }


        public override void RenderGizmoLayer(IRenderer r, IView2D view)
        {

            //Determine worldspace location of the screen
            Rectangle2D viewRec = view.ScreenSpaceBounds;
            List<Point2D> viewArea = new List<Point2D>() { viewRec.TopLeft, 
                                                           viewRec.TopRight, 
                                                           viewRec.LowerLeft, 
                                                           viewRec.LowerRight };

            viewArea = view.ScreenToWorldTransformMatrix.Transform(viewArea);

            //Iteration bounds
            int xStart = (int)(Math.Floor(viewArea[0].X / XGapWorldSpace));
            int yStart = (int)(Math.Floor(viewArea[0].Y / YGapWorldSpace));
            int xEnd = (int)(Math.Ceiling(viewArea[3].X / XGapWorldSpace));
            int yEnd = (int)(Math.Ceiling(viewArea[3].Y / YGapWorldSpace));
            int xSteps = xEnd - xStart;
            int ySteps = yEnd - yStart;

            //Setup avis of screen in world space
            Point2D xAxis = (viewArea[1] - viewArea[0]);
            Point2D yAxis = (viewArea[2] - viewArea[0]);

            Point2D xAxisDir = xAxis.UnitVector();
            Point2D yAxisDir = yAxis.UnitVector();
            Point2D GridOrigin = (xStart * xAxis) + (yStart * yAxis);

            //color and fonts

            //draw the grid
            for (int y = yStart; y < yEnd; y++)
            {
                Point2D a = (y * YGapWorldSpace) * yAxisDir;
                a += new Point2D(viewArea[0].X, 0);
                Point2D b = a + xAxis;
                a = view.WorldToScreenTransformMatrix.Transform(a);
                b = view.WorldToScreenTransformMatrix.Transform(b);

                r.DrawLine(gridColor, 1, a, b);
            }

            for (int x = xStart; x < xEnd; x++)
            {
                Point2D a = (x * XGapWorldSpace) * xAxisDir;
                a += new Point2D(0, viewArea[0].Y);
                Point2D b = a + yAxis;
                a = view.WorldToScreenTransformMatrix.Transform(a);
                b = view.WorldToScreenTransformMatrix.Transform(b);

                r.DrawLine(gridColor, 1, a, b);
            }

        }

        public override Point2D Snap(Point2D posWS)
        {
            return new Point2D(Math.Truncate(posWS.X / XGapWorldSpace) * XGapWorldSpace,
                               Math.Truncate(posWS.Y / YGapWorldSpace) * YGapWorldSpace);
        }
    }

}
