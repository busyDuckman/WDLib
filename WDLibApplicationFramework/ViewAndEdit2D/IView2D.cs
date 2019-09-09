/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WD_toolbox.Rendering;
using WD_toolbox.Maths.Geometry;

namespace WDLibApplicationFramework.ViewAndEdit2D
{
    public enum View2DOrientation
    {
        /// <summary>
        /// Default
        /// </summary>
        YGoesDownward,
        /// <summary>
        /// Like on paper
        /// </summary>
        YGoesUpward
    }

    public enum AxisSelection { Both, Vertical, Horizontal }

    public interface IView2D
    {
        //Matrix WorldSpaceToScreenSpace {get;}
        double Zoom { get; set; }
        double TransformX  { get; set; }
        double TransformY { get; set; }
        View2DOrientation Orientation { get; set; }
        
        /// <summary>
        /// Clockwise rotation in radians
        /// </summary>
        double ClockwiseRotation { get; set; }

        /// <summary>
        /// The world limits
        /// </summary>
        Rectangle2D WorldSpaceBounds { get; set; }

        /// <summary>
        /// Where on ths screen to render
        /// </summary>
        Rectangle ScreenSpaceBounds { get; set; }

        void Render(IRenderer r);

        /// <summary>
        /// 
        /// </summary>
        Action OnRefreshNeeded { get; set; }

        /// <summary>
        /// Call this to cause a screen refresh.
        /// This will raise OnRefreshNeeded();
        /// </summary>
        void Refresh();

        TransMatrix2D WorldToScreenTransformMatrix { get; }
        TransMatrix2D ScreenToWorldTransformMatrix { get; }
        
        //mouse stuff
        //
        // Summary:
        //     Occurs when the control is clicked by the mouse.
        //event MouseEventWorldSpace.WSMouseEventHandler MouseClickWS;
        
        //void RaiseMouseEvwent(object sender, MouseEventWorldSpace e);
    }

    public static class IView2DExtension
    {
        public static TransMatrix2D CalculateWorldToScreenTransformMatrix(this IView2D view)
        {
            return TransMatrix2D.FromTRS(new Point2D(view.TransformX, view.TransformY),
                                                     view.ClockwiseRotation,
                //screenSpaceBounds.Middle(),
                                                     Point2D.Origen,
                                                     new Point2D(view.Zoom, view.Zoom));
        }

        public static Point2D WorldSpace2ScreenSpace(this IView2D view, Point2D worldSpace)
        {
            //view.WorldSpaceToScreenSpace
            /*Point2D trans = new Point2D(view.TransformX, view.TransformX);
            Point2D scn = (worldSpace - trans) * view.Zoom;
            return scn.RotateAboutOrigin(view.ClockwiseRotation);*/
            return view.WorldToScreenTransformMatrix * worldSpace;
        }

        public static Point2D ScreenSpace2WorldSpace(this IView2D view, Point2D screenSpace)
        {
            return view.ScreenSpace2WorldSpace(screenSpace.X, screenSpace.Y);
        }

        public static Point2D ScreenSpace2WorldSpace(this IView2D view, Point screenSpace)
        {
            return view.ScreenSpace2WorldSpace(screenSpace.X, screenSpace.Y);
        }

        public static Point2D ScreenSpace2WorldSpace(this IView2D view, double screenX, double screenY)
        {
            return view.ScreenToWorldTransformMatrix.Transform(new Point2D(screenX, screenY));
            //return WorldSpace2ScreenSpace(view, new Point2D(screenX, screenY));
            /*double x = screenX - view.TransformX;
            double y = screenY - view.TransformY;
            x /= view.Zoom;
            y /= view.Zoom;
            //Point2D p = new Point2D(x, y);
            return p.RotateAboutOrigin(view.ClockwiseRotation);*/
            /*
            //alternative
            double x = screenX;
            double y = screenY;

            

            //transform
            x -= view.TransformX;
            y -= view.TransformY;

            //rotate
            Point2D p = new Point2D(x, y);
            p = p.RotateAboutOrigin(view.ClockwiseRotation);
            x = p.X;
            y = p.Y;

            //scale
            x /= view.Zoom;
            y /= view.Zoom;

            return new Point2D(x, y);*/
            
        }

        public static void Render(this IView2D view, Graphics g)
        {
            GDIPlusRenderer r = new GDIPlusRenderer(g);
            view.Render(r);
            r.Close();
        }

        public static void AlterRotationAboutScreenMiddle(this IView2D view, double newRotation)
        {
        }

        public static void TransposeToAllignPoints(this IView2D view, Point2D world, Point screen, AxisSelection mode = AxisSelection.Both)
        {
            Point2D screenPosWS = view.ScreenSpace2WorldSpace(screen.X, screen.Y);

            //nudge the screen so the mouse is zooming in on a point;
            Point2D nudge = screenPosWS - world;

            nudge *= view.Zoom;

            switch (mode)
            {
                case AxisSelection.Both:
                    view.TransformX += nudge.X;
                    view.TransformY += nudge.Y;
                    break;
                case AxisSelection.Vertical:
                    view.TransformY += nudge.Y;
                    break;
                case AxisSelection.Horizontal:
                     view.TransformX += nudge.X;
                    break;
            }
        }

    }
}
