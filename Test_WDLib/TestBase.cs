/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Test_WDLib
{
    [TestClass()]
    public abstract class TestBase
    {
        public static void AssertEquivilantImages(Bitmap a, Bitmap b)
        {
            Assert.AreEqual(a.Width, b.Width);
            Assert.AreEqual(a.Height, b.Height);
            for (int x = 0; x < a.Width; x++)
            {
                for (int y = 0; y < a.Height; y++)
                {
                    int _a = a.GetPixel(x, y).ToArgb();
                    int _b = b.GetPixel(x, y).ToArgb();
                    Assert.AreEqual(_a, _b);
                }
            }
        }

        public static bool AreImagesTheSame(Bitmap a, Bitmap b)
        {
            if((a == null) || (b == null))
            {
                return (a == null) && (b == null);
            }

            if ((a.Width == b.Width) && (a.Height == b.Height))
            {
                for (int x = 0; x < a.Width; x++)
                {
                    for (int y = 0; y < a.Height; y++)
                    {
                        int _a = a.GetPixel(x, y).ToArgb();
                        int _b = b.GetPixel(x, y).ToArgb();
                        if (_a != _b)
                        {
                            return false; //a pixel was different
                        }
                    }
                }
                return true; //all pixels were the same
            }

            return false; //sizes did not match
        }
    }
}
