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
using WD_toolbox.Maths.Geometry;
using WD_toolbox.Maths.Units;
using WD_toolbox.Rendering;
using WD_toolbox.Rendering.FormattedText;

namespace WDLibApplicationFramework.ViewAndEdit2D.Grid
{
    public class DistanceGrid : ViewGridBase
    {
        public Distance XGapDistance { get; set; }
        public Distance YGapDistance { get; set; }
        /// <summary>
        /// Reulves world space units to actual mesurement
        /// </summary>
        public Resolution XWorldSpaceResolution { get; set; }
        /// <summary>
        /// Reulves world space units to actual mesurement
        /// </summary>
        public Resolution YWorldSpaceResolution { get; set; }
        public bool ShowGridLines { get; set; }

        Color MouseLocationCol { get; set; }

        private GridUnitMode unitMode=null;

        public double XGapWorldSpace { get { return XGapDistance.Metres * XWorldSpaceResolution.DotsPerMetre; } }
        public double YGapWorldSpace { get { return YGapDistance.Metres * YWorldSpaceResolution.DotsPerMetre; } }

        public GridUnitMode UnitMode
        {
            get { return unitMode ?? GridUnitMode.MetricMM; }
            set { unitMode = value; RefreshTransientData(); }
        }

        //[NonSerialized]
        TextFormat fmtMajor {get; set;}
        //[NonSerialized]
        TextFormat fmtMinor {get; set;}
        //[NonSerialized]
        Size MaxSeenLabelSize { get; set; }       

        public DistanceGrid(Distance xGapDistance,
                            Distance yGapDistance,
                            Resolution xWorldSpaceResolution,
                            Resolution yWorldSpaceResolution)
            : base()
        {
            this.XGapDistance = xGapDistance;
            this.YGapDistance = yGapDistance;
            this.XWorldSpaceResolution = xWorldSpaceResolution;
            this.YWorldSpaceResolution = yWorldSpaceResolution;
            MouseLocationCol = Color.FromArgb(200, Color.Red);
            ShowGridLines = true;

            RefreshTransientData();
        }

        protected override void RefreshTransientData()
        {
            base.RefreshTransientData();
            fmtMajor = getLabelFormat();
            fmtMinor = getLabelFormat(true);
            MaxSeenLabelSize = new Size(30, 10);
        }

        public override void RenderGizmoLayer(IRenderer r, IView2D view)
        {
            RenderGizmoLayer(r, view, DistanceGridRenderLayer.All);
        }

        public enum DistanceGridRenderLayer {None =0, Ruler=1, Grid=2, All = 255}
        public void RenderGizmoLayer(IRenderer r, IView2D view, DistanceGridRenderLayer layers)
        {
            double XGapScreenSpace;
            double YGapScreenSpace;
            getScreenSpaceGridGap(view, XGapWorldSpace, YGapWorldSpace, out XGapScreenSpace, out YGapScreenSpace);

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

            //draw the grid
            Point2D a;
            Point2D b;

            int NumberOfGridLinesThreshold = (int)(Math.Min(viewRec.Width, viewRec.Height) / 2);
            bool gridRenderIsSensible = ((xSteps <= NumberOfGridLinesThreshold) && (ySteps <= NumberOfGridLinesThreshold));

            //lines
            if (ShowGridLines && ((layers & DistanceGridRenderLayer.Grid) != 0))
            {
                if (gridRenderIsSensible)
                {
                    for (int y = yStart; y < yEnd; y++)
                    {
                        getScreenPosForGridLineY(view, YGapWorldSpace, viewArea, ref xAxis, ref yAxisDir, y, out a, out b);
                        r.DrawLine(gridColor, 1, a, b);
                    }

                    for (int x = xStart; x < xEnd; x++)
                    {
                        getScreenPosForGridLineX(view, XGapWorldSpace, viewArea, ref yAxis, ref xAxisDir, x, out a, out b);
                        r.DrawLine(gridColor, 1, a, b);
                    }
                }
            }

            //rulers
            if ((layers & DistanceGridRenderLayer.Ruler) != 0)
            {
                Rectangle ssb = view.ScreenSpaceBounds;
                int gutterSizeTop = 20;
                int gutterSizeLeft = 40;

                //------------ LEFT GUTTER
                r.FillRectangle(Color.White, ssb.Left, ssb.Top + gutterSizeTop, gutterSizeLeft, ssb.Height - gutterSizeTop);
                r.DrawRectangle(Color.Black, 1, ssb.Left, ssb.Top + gutterSizeTop, gutterSizeLeft, ssb.Height - gutterSizeTop);

                //int pixelsSinceLastStringThreshold = 10;
                //int pixelPosOfLastString = 0;
                //int labelMod = (int)(Math.Max(YGapScreenSpace, 1)) / 5;
                //labelMod = Math.Max(labelMod, 1);
                int labelMod = 1;
                int textDistanceInPixels = 15;
                while ((YGapScreenSpace * labelMod) < MaxSeenLabelSize.Height) //12
                {
                    //labelMod *= 10;
                    labelMod *= UnitMode.UsesFractions ? 2 : 10;
                }
                for (int y = yStart; y < yEnd; y++)
                {
                    getScreenPosForGridLineY(view, YGapWorldSpace, viewArea, ref xAxis, ref yAxisDir, y, out a, out b);

                    double distMesure = (y * YGapWorldSpace) / YWorldSpaceResolution.DotsPerMetre;
                    if ((y % labelMod) == 0)
                    {
                        r.DrawLine(Color.Black, 1, (int)gutterSizeLeft - 15, (int)a.Y, (int)gutterSizeLeft, (int)a.Y);

                        if (a.Y > (gutterSizeTop + 5))
                        {
                            bool isMinor;
                            Point2D TextOffset;
                            string rulertext = getRulerText(r, distMesure, out isMinor, out TextOffset, true);
                            r.DrawString(isMinor ? fmtMinor : fmtMajor, rulertext, (a + TextOffset).AsPoint());
                        }
                    }
                    else
                    {
                        r.DrawLine(Color.Black, 1, (int)gutterSizeLeft - 5, (int)a.Y, (int)gutterSizeLeft, (int)a.Y);
                    }
                }

                //------------ TOP GUTTER
                //fmt.printVertical = true;
                labelMod = 1;
                textDistanceInPixels = UnitMode.UsesFractions ? 120 : 60;
                while ((XGapScreenSpace * labelMod) < MaxSeenLabelSize.Width) //30
                {
                    //labelMod *= (UnitMode.LastUnitModulus <= 1) ? 10 : UnitMode.LastUnitModulus;
                    labelMod *= UnitMode.UsesFractions ? 2 : 10;
                }

                r.FillRectangle(Color.White, ssb.Left, ssb.Top, ssb.Width, gutterSizeTop);
                r.DrawRectangle(Color.Black, 1, ssb.Left, ssb.Top, ssb.Width, gutterSizeTop);
                for (int x = xStart; x < xEnd; x++)
                {
                    getScreenPosForGridLineX(view, XGapWorldSpace, viewArea, ref yAxis, ref xAxisDir, x, out a, out b);

                    double distMesure = (x * XGapWorldSpace) / XWorldSpaceResolution.DotsPerMetre;
                    if ((x % labelMod) == 0) //labelMod = 5
                    {
                        r.DrawLine(Color.Black, 1, (int)a.X, (int)gutterSizeTop - 10, (int)a.X, (int)gutterSizeTop);
                        //if (gridRenderIsSensible)
                        {
                            /*
                            string rulertext = UnitMode.GetDistanceString(Distance.FromMetres(distMesure), "?");
                            bool isMinor = rulertext.StartsWith("?");
                            rulertext = rulertext.TrimStart("?".ToCharArray());

                            //Point2D TextOffset = new Point2D(-12, 2);
                            Point2D TextOffset = new Point2D(-(r.MeasureString(rulertext, fmt.font).Width / 2), 2);

                            r.DrawString(isMinor ? fmtMinor : fmt, rulertext, (a + TextOffset).AsPoint());*/

                            bool isMinor;
                            Point2D TextOffset;
                            string rulertext = getRulerText(r, distMesure, out isMinor, out TextOffset, false);
                            r.DrawString(isMinor ? fmtMinor : fmtMajor, rulertext, (a + TextOffset).AsPoint());
                        }
                    }
                    else
                    {
                        r.DrawLine(Color.Black, 1, (int)a.X, (int)gutterSizeTop - 3, (int)a.X, (int)gutterSizeTop);
                    }
                }

                //draw mouse location marker
                IEditableView2D ev = view as IEditableView2D;
                if (ev != null)
                {
                    Point2D pos = view.WorldToScreenTransformMatrix.Transform(ev.MousePosInWorldSpace);
                    //top
                    r.DrawLine(Color.Red, 1, (int)pos.X, (int)gutterSizeTop - 15, (int)pos.X, (int)gutterSizeTop);
                    //left
                    if (pos.Y > gutterSizeTop)
                    {
                        r.DrawLine(Color.Red, 1, (int)gutterSizeLeft - 10, (int)pos.Y, (int)gutterSizeLeft, (int)pos.Y);
                    }
                }
            }

        }

        private string getRulerText(IRenderer r, double distMesure, out bool isMinor, out Point2D TextOffset, bool isVerticalAxis)
        {
            string rulertext = UnitMode.GetDistanceString(Distance.FromMetres(distMesure), "?");
            isMinor = rulertext.StartsWith("?");
            rulertext = rulertext.TrimStart("?".ToCharArray());

            if (isVerticalAxis && (rulertext.Length > 6))
            {
                rulertext = rulertext.Replace("mm", "\r\n  mm");
                rulertext = rulertext.Replace("mils", "\r\n  mils");
            }

            TextFormat fmt = isMinor ? fmtMinor : fmtMajor;
            Size size = r.MeasureString(rulertext, fmt.font);

            //update the max seen zise
            if ((MaxSeenLabelSize.Width < size.Width) || (MaxSeenLabelSize.Height < size.Height))
            {
                MaxSeenLabelSize = new Size(Math.Max(MaxSeenLabelSize.Width, size.Width), Math.Max(MaxSeenLabelSize.Height, size.Height));
            }
            
            //calculate an appropriate text offset
            if(isVerticalAxis)
            {
                //TextOffset = new Point2D(1, -7);
                TextOffset = new Point2D(1, -(size.Height / 2));
            }
            else
            {
                TextOffset = new Point2D(-(size.Width / 2), 2);
            }

            return rulertext;
        }

        private static void getScreenSpaceGridGap(IView2D view, double XGapWorldSpace, double YGapWorldSpace, out double XGapScreenSpace, out double YGapScreenSpace)
        {
            XGapScreenSpace = XGapWorldSpace;
            YGapScreenSpace = YGapWorldSpace;
            view.WorldToScreenTransformMatrix.Transform(ref XGapScreenSpace, ref YGapScreenSpace);

            double originX = 0;
            double originY = 0;
            view.WorldToScreenTransformMatrix.Transform(ref originX, ref originY);
            XGapScreenSpace -= originX;
            YGapScreenSpace -= originY;

            XGapScreenSpace = Math.Abs(XGapScreenSpace);
            YGapScreenSpace = Math.Abs(YGapScreenSpace);
        }

        private TextFormat getLabelFormat(bool minor=false)
        {
            TextFormat fmt = new TextFormat();
            fmt.font = minor ? GridFontMinor : GridFont;
            fmt.printVertical = false;
            fmt.shadowColour = Color.White;
            fmt.shadowDir = WD_toolbox.Maths.Space.Octants2D.diagonals;
            fmt.showShadow = false;
            fmt.textColour = minor ? Color.LightGray : Color.Black;
            return fmt;
        }

        private static void getScreenPosForGridLineX(IView2D view, double XGapWorldSpace, List<Point2D> viewArea,
            ref Point2D yAxis, ref Point2D xAxisDir, int x,
            out Point2D a, out Point2D b)
        {
            double distWS = x * XGapWorldSpace;
            a = distWS * xAxisDir;
            a += new Point2D(0, viewArea[0].Y);
            b = a + yAxis;
            a = view.WorldToScreenTransformMatrix.Transform(a);
            b = view.WorldToScreenTransformMatrix.Transform(b);
        }

        private static void getScreenPosForGridLineY(IView2D view, double YGapWorldSpace, List<Point2D> viewArea,
            ref Point2D xAxis, ref Point2D yAxisDir, int y,
            out Point2D a, out Point2D b)
        {
            double distWS = y * YGapWorldSpace;
            a = (y * YGapWorldSpace) * yAxisDir;
            a += new Point2D(viewArea[0].X, 0);
            b = a + xAxis;
            a = view.WorldToScreenTransformMatrix.Transform(a);
            b = view.WorldToScreenTransformMatrix.Transform(b);
        }

        public override Point2D Snap(Point2D posWS)
        {
            return new Point2D(Math.Truncate(posWS.X / XGapWorldSpace) * XGapWorldSpace,
                               Math.Truncate(posWS.Y / YGapWorldSpace) * YGapWorldSpace);
        }
        
    }
}
