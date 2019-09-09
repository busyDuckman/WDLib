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

namespace WD_toolbox.Maths.Geometry.Lines
{
    public class PolyLine : IOpenPath, ICloneable, IPointPath
    {
        List<Point2D> points;
        public List<Point2D> Points {
            get { return points; }
        }

        public PolyLine(IEnumerable<Point2D> points)
        {
            this.points = new List<Point2D>(points);
        }

        public PolyLine(PolyLine another)
        {
            this.points = another.points.Select(P => (Point2D)P.Clone()).ToList();
        }

        public Rectangle2D BoundingBox { get { return Rectangle2D.BoundingBox(points); } }

        public IList<Line> GetLines()
        {
            IList<Line> lines = new List<Line>();
            for(int i=1; i<points.Count; i++)
            {
                lines.Add(new Line(points[i - 1], points[i]));
            }

            return lines;
        }

        public Point2D End
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double PathLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Point2D Start
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double DistanceToPath(Point2D point)
        {
            IList<Line> lines = GetLines();
            if (lines.Count > 0)
            {
                return lines.Min(L => L.DistanceToPath(point));
            }
            else
            {
                return this.points[0].DistanceTo(point);
            }

        }

        public List<PathPoint> Intersections(IPath path)
        {
            throw new NotImplementedException();
        }

        public Point2D PointOnPath(double percent)
        {
            throw new NotImplementedException();
        }

        public IOpenPath Reverse()
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

        public object Clone()
        {
            return new PolyLine(this);
        }
    }
}
