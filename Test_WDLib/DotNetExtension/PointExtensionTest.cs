/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using WD_toolbox;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

namespace Test_WDLib
{
    
    
    /// <summary>
    ///This is a test class for PointExtensionTest and is intended
    ///to contain all PointExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PointExtensionTest
    {


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
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for getPointAbove
        ///</summary>
        [TestMethod()]
        public void getPointAboveTest()
        {
            Point p = new Point(1, 4);
            Point expected = new Point(1, 3);
            Point actual;
            actual = PointExtension.getPointAbove(p);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getPointBelow
        ///</summary>
        [TestMethod()]
        public void getPointBelowTest()
        {
            Point p = new Point(1, 4);
            Point expected = new Point(1, 5);
            Point actual;
            actual = PointExtension.getPointBelow(p);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getPointLeft
        ///</summary>
        [TestMethod()]
        public void getPointLeftTest()
        {
            Point p = new Point(1, 4);
            Point expected = new Point(0, 4);
            Point actual;
            actual = PointExtension.getPointLeft(p);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for getPointRight
        ///</summary>
        [TestMethod()]
        public void getPointRightTest()
        {
            Point p = new Point(1, 4);
            Point expected = new Point(2, 4);
            Point actual;
            actual = PointExtension.getPointRight(p);
            Assert.AreEqual(expected, actual);
        }
    }
}
