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
using WD_toolbox.Maths.Geometry.Lines;
using WD_toolbox.Maths.Geometry.Shapes;

namespace WD_toolbox.Maths.Geometry
{
    public class PathPoint
    {
        public Point2D Position { get; protected set; }
        public double PercentAlongPath { get; protected set; }
    }

    public interface IGeometry : ICloneable //: ITransformable2D
    {
        Rectangle2D BoundingBox { get; }
    }

    public interface IPath : IGeometry
    {
        Point2D Start { get; }
        double PathLength { get; }

        Point2D PointOnPath(double percent);
        List<PathPoint> Intersections(IPath path);
        double DistanceToPath(Point2D point);

        PolyLine ToPolyLine(double lineLenOnCurves); 
    }

    public interface IPointPath : IPath
    {
        List<Point2D> Points { get; }

    }

    public interface IOpenPath : IPath
    {
        Point2D End { get; }
        List<IOpenPath> Split(double percentPos);
        IOpenPath Reverse();
    }

    /*public interface IOpenOrClosedPath : IPath
    {
        Point2D End { get; }
        bool Closed { get; }
    }*/

    public static class IPathExtension
    {
        public static IPath SubLine(this IOpenPath path, double perStart, double perEnd)
        {
            IOpenPath p = path.Split(perStart).Last().Split(perEnd-perStart).First();
            return p;
        }

        public static List<IOpenPath> SplitOnDistance(this IOpenPath path, double posDistance)
        {
            return path.Split(posDistance / path.PathLength);
        }

        public static Point2D PointOnPathByDistance(this IPath path, double posDistance)
        {
            return path.PointOnPath(posDistance / path.PathLength);
        }



    }
}
