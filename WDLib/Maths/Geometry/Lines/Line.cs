/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WD_toolbox.Maths.Geometry.Lines
{


    //--------------------------------------------------------------------------------------------------
    // Line
    //--------------------------------------------------------------------------------------------------
    public sealed class Line : IOpenPath, IEquatable<Line>, ICloneable, IReadOnlyList<Point2D>, ILine
    {
        //--------------------------------------------------------------------------------------------------
        // Acessors
        //--------------------------------------------------------------------------------------------------
        //public bool Closed {  get { return false; } }

        public Point2D End { get; private set; }

        public double PathLength { get { return Length(); } }

        public Point2D Start { get; private set; }

        //--------------------------------------------------------------------------------------------------
        // Constructors and factory methods.
        //--------------------------------------------------------------------------------------------------
        public Line(Point2D start, Point2D end)
        {
            this.Start = start;
            this.End = end;
        }

        private Line(Line another)
        {
            Start = another.Start;
            End = another.End;
        }

        //--------------------------------------------------------------------------------------------------
        // IPath
        //--------------------------------------------------------------------------------------------------
        public double DistanceToPath(Point2D point)
        {
            Point2D pointOfIntersection = new Point2D(0, 0);
          
            double u = (((point.X - Start.X) * (End.X - Start.X)) +
                ((point.Y - Start.Y) * (End.Y - Start.Y))) /
                LengthSquared();

            if ((u >= 0.0) && (u <= 1.0))
            {
                pointOfIntersection.X = Start.X + u * (End.X - Start.X);
                pointOfIntersection.Y = Start.Y + u * (End.Y - Start.Y);
            }
            else
            {
                pointOfIntersection = (u < 0.0) ? Start : End;
            }

            return point.DistanceTo(pointOfIntersection);
        }

        public List<PathPoint> Intersections(IPath path)
        {
            throw new NotImplementedException();
        }

        public Point2D PointOnPath(double percent)
        {
            throw new NotImplementedException();
        }

        public List<IOpenPath> Split(double percentPos)
        {
            throw new NotImplementedException();
        }

        public PolyLine ToPolyLine(double lineLenOnCurves)
        {
            throw new NotImplementedException();
        }

        public IOpenPath Reverse()
        {
            return new Line(End, Start);
        }


        //--------------------------------------------------------------------------------------------------
        // ILine
        //--------------------------------------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Point2D VecFromStartToEnd()
        {
            return End - Start;
        }

        public Point2D UnitVecFromStartToEnd()
        {
            return VecFromStartToEnd() * (1.0 / Length());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double LengthSquared()
        {
            Point2D v = VecFromStartToEnd();
            return (v.X * v.X) + (v.Y * v.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Length()
        {
            return Math.Sqrt(LengthSquared());
        }

        public bool IsVertical
        {
            get { return Start.X == End.X; }
        }

        public bool IsHorizontal
        {
            get { return Start.Y == End.Y; }
        }

        public bool EqualsIgnoreDirection(ILine other)
        {
            if (System.Object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (((object)this == null) || ((object)other == null))
            {
                return false;
            }

            return ((this.Start == other.Start) && (this.End == other.End)) ||
                   ((this.Start == other.End) && (this.End == other.Start));
        }

        public Rectangle2D BoundingBox { get { return Rectangle2D.BoundingBox(this); } }


        public LineToPointInfo FindLinePointInfo(Point2D point)
        {
            LineToPointInfo info = new LineToPointInfo();

            

            double u = (((point.X - Start.X) * (End.X - Start.X)) +
                ((point.Y - Start.Y) * (End.Y - Start.Y))) /
                LengthSquared();

            info.PercentAlongLine = u;

            // true if an endpoint is the closest point
            bool EndPointIsClosest = false;
            info.RelativePos = 0;

            if ((u >= 0.0) && (u <= 1.0))
            {
                info.PointOfIntersection = new Point2D(Start.X + u * (End.X - Start.X), 
                                                     Start.Y + u * (End.Y - Start.Y));
            }
            else
            {
                EndPointIsClosest = true;
                info.PointOfIntersection = (u < 0.0) ? Start : End;
                info.RelativePos = (u < 0.0) ? -1 : 1;
            }

            info.DistanceAlongLinetoNearestPointOnLine = u * (End - Start).Length();
            info.Distance = info.PointOfIntersection.DistanceTo(point);

            double sideTest = ((point.X - Start.X) * (End.Y - Start.Y)) - ((point.Y - Start.Y) * (End.X - Start.X));
            if (Math.Abs(sideTest) < 0.000001)
            {
                info.Side = LineToPointInfo.SideEnum.On;
            }
            else
            {
                info.Side = (sideTest < 0) ? LineToPointInfo.SideEnum.Left : LineToPointInfo.SideEnum.Right;
            }


            

            return info;
        }


        //--------------------------------------------------------------------------------------------------
        // Object
        //--------------------------------------------------------------------------------------------------
        public override int GetHashCode()
        {
            return Start.GetHashCode() + End.GetHashCode();
        }

        /// <summary>
        /// Get string representation of the class.
        /// </summary>
        /// <returns>Returns string, which contains values of the like in readable form.</returns>
        public override string ToString()
        {
            return string.Format("Line from {0} to {1}", Start, End);
        }

        //--------------------------------------------------------------------------------------------------
        // IEquatable<Line>
        //--------------------------------------------------------------------------------------------------
        public override bool Equals(object obj)
        {
            return (obj is Line) ? (this == (Line)obj) : false;
        }

        public bool Equals(Line other)
        {
            return this == other;
        }

        public static bool operator ==(Line a, Line b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return ((a.Start == b.Start) && (a.End == b.End));
        }

        public static bool operator !=(Line a, Line b)
        {
            return !(a == b);
        }

        //--------------------------------------------------------------------------------------------------
        // ICloneable
        //--------------------------------------------------------------------------------------------------
        public object Clone()
        {
            return new Line(this);
        }

        //--------------------------------------------------------------------------------------------------
        // IReadOnlyList<Point2D>
        //--------------------------------------------------------------------------------------------------
        public IEnumerator<Point2D> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Point2D this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
        }


    }
}
