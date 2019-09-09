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
    public class TransformedRectangle2D : IReadOnlyList<Point2D>, IGeometry
    {
        const int TopLeftIndex = 0, TopRightIndex = 1, LowerRightIndex = 2, LowerLeftIndex = 3;
        public Point2D NewTopLeft { get {return points[TopLeftIndex];} }
        public Point2D NewTopRight { get { return points[TopRightIndex]; } }
        public Point2D NewLowerLeft { get { return points[LowerLeftIndex]; } }
        public Point2D NewLowerRight { get { return points[LowerRightIndex]; } }

        /// <summary>
        /// eg, True if ithe the rotation is a multiple of 90 deg.
        /// </summary>
        public bool isAllignedOrthogonally
        {
            get { return Rectangle2D.BoundingBox(new Point2D[] { NewTopLeft, NewLowerRight }).Equals(Rectangle2D.BoundingBox(new Point2D[] { NewTopRight, NewLowerLeft })); }
        }
        
        protected List<Point2D> points;
        public IReadOnlyList<Point2D> Points { get { return points.AsReadOnly(); } }

        public TransformedRectangle2D(Point2D newTopLeft, Point2D newTopRight, Point2D newLowerRight, Point2D newLowerLeft)
        {
            points = new List<Point2D>();
            
            //this used to be an array, untill the enumerator got funny
            /*points[TopLeftIndex] = newTopLeft;
            points[TopRightIndex] = newTopRight;
            points[LowerRightIndex] = newLowerRight;
            points[LowerLeftIndex] = newLowerLeft;*/

            points.Add(newTopLeft);
            points.Add(newTopRight);
            points.Add(newLowerRight);
            points.Add(newLowerLeft);
            
        }

        public TransformedRectangle2D(TransformedRectangle2D another)
        {
            points = another.points.Select(P => (Point2D)P.Clone()).ToList();
        }
        
        public Point2D this[int index]
        {
            get 
            {
                switch (index)
                {
                    case 0:
                        return NewTopLeft;
                    case 1:
                        return NewTopLeft;
                    case 2:
                        return NewTopLeft;
                    case 3:
                        return NewTopLeft;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public int Count
        {
            get { return 4;  }
        }

        public Rectangle2D BoundingBox
        {
            get
            {
                return Rectangle2D.BoundingBox(points);
            }
        }

        public IEnumerator<Point2D> GetEnumerator()
        {
            return points.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)points).GetEnumerator();
        }

        public object Clone()
        {
            return new TransformedRectangle2D(this);
        }
    }
}
