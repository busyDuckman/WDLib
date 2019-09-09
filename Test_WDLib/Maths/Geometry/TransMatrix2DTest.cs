/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WD_toolbox.Maths.Geometry;
using GDIMatrix = System.Drawing.Drawing2D.Matrix;
using PointF = System.Drawing.PointF;
using WD_toolbox.Rendering;
using System.Diagnostics;

namespace Test_WDLib.Maths.Geometry
{
    [TestClass]
    public class TransMatrix2DTest
    {
        [TestMethod]
        public void TestFromGDIPlusMatrix()
        {
            //TODO: HOW THE HELL DOES THIS TAKE 2MS TO COMPLETE???

            //I want to know that the transformation matrix is valid in GDI plus conversions.
            TransMatrix2D t1 = TransMatrix2D.FromTRS(
                new Point2D(22.25, 31.75),
                12,
                new Point2D(51, 37),
                new Point2D(0.5, 0.25));

            GDIMatrix g1 = t1.ToGDIPlusMatrix();
            TransMatrix2D t2 = TransMatrix2D.FromGDIPlusMatrix(g1);

            Point2D test = new Point2D(7.1, -81.2);
            Point2D p1 = t1.Transform(test);

            var temp = new PointF[] { test.AsPointF() };
            g1.TransformPoints(temp);
            Point2D p2 = new Point2D(temp[0]);

            Point2D p3 = t2.Transform(test);

            double errorThreshold = 0.001;
            
            
            //all points should not be on the origen
            Assert.IsTrue(p1.Length() > errorThreshold);
            Assert.IsTrue(p2.Length() > errorThreshold);
            Assert.IsTrue(p3.Length() > errorThreshold);

            //all points should be similar
            Assert.IsTrue(p1.DistanceTo(p2) < errorThreshold);
            Assert.IsTrue(p1.DistanceTo(p3) < errorThreshold);
        }

        [TestMethod]
        public void PerformanceTestTransformMatrix()
        {
            IRendererExtension.PerformanceTestGDIConversion(3);
            Stopwatch sw = new Stopwatch();
            int n = 10000;

            sw.Start();
            IRendererExtension.PerformanceTestGDIConversion(n);
            sw.Stop();

            double opsPerSecond = n / sw.Elapsed.TotalSeconds;

            Trace.WriteLine(string.Format("Performance test {0}(...) gave {1} operations per second.", 
                            "TestGDIConversion", 
                            opsPerSecond.ToString("#,##0")));
            
            //something if very wrong if we don't make this.
            Assert.IsTrue(opsPerSecond > 10000);
        }
    }
}
