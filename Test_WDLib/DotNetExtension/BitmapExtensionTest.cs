/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using WD_toolbox;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using WD_toolbox.Rendering.Patterns;

namespace Test_WDLib
{
    
    
    /// <summary>
    ///This is a test class for BitmapExtensionTest and is intended
    ///to contain all BitmapExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BitmapExtensionTest : TestBase
    {

        private Bitmap[] testImages;
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        { 
            testImages = new Bitmap[5];
            testImages[0] = new Bitmap(500, 500, PixelFormat.Format32bppArgb);
            testImages[1] = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
            testImages[2] = new Bitmap(500, 500, PixelFormat.Format24bppRgb);
            testImages[3] = new Bitmap(500, 500, PixelFormat.Format64bppArgb);
            testImages[4] = new Bitmap(500, 500, PixelFormat.Format32bppRgb);

            PokerDotsPattern patern = new PokerDotsPattern();
            Random rand = new Random(123);

            for (int i = 0; i < testImages.Length; i++)
            {
                patern.dotColor = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
                patern.backGroundColor = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
                patern.dotSize = rand.Next(32) + 16;
                patern.applyPattern(ref testImages[i]);
            }

        }
        
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion
      
        /// <summary>
        ///A test for ConvetTo
        ///</summary>
        [TestMethod()]
        public void ConvetToTest()
        {
            foreach (Bitmap a in testImages)
            {
                Bitmap b = BitmapExtension.ConvetTo(a, PixelFormat.Format32bppArgb);
                AssertEquivilantImages(a, b);
            }
        }

        /// <summary>
        ///A test for GetUnFuckedVersion
        ///</summary>
        [TestMethod()]
        public void GetUnFuckedVersionTest()
        {
            foreach (Bitmap a in testImages)
            {
                Bitmap b = BitmapExtension.GetUnFuckedVersion(a);
                AssertEquivilantImages(a, b);
            }
        }
    }
}
