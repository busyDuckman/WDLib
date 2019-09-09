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

namespace WD_toolbox.Maths.Geometry
{

    public struct LineToPointInfo
    {
        public double Distance { get; internal set; }
        public enum SideEnum { Left = -1, On = 0, Right = 1 };
        public double DistanceAlongLinetoNearestPointOnLine { get; internal set; }
        public int RelativePos { get; internal set; }
        public Point2D PointOfIntersection { get; internal set; }
        public SideEnum Side { get; internal set; }
        public double PercentAlongLine { get; internal set; }
    }

    public struct LineLineIntersectInfo
    {
        public bool linesAreIdentical;
        public bool linesIntersect;
        public ILine ShortestLineFromLineAtoLineB;
    }

    public interface ILine : IOpenPath
    {
        Point2D VecFromStartToEnd();

        Point2D UnitVecFromStartToEnd();

        double LengthSquared();

        double Length();

        bool IsVertical { get; }

        bool IsHorizontal { get; }

        bool EqualsIgnoreDirection(ILine other);

        LineToPointInfo FindLinePointInfo(Point2D p);
    }
}
