/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using WD_toolbox;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test_WDLib
{
    
    
    /// <summary>
    ///This is a test class for RandomExtensionTest and is intended
    ///to contain all RandomExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RandomExtensionTest
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
        ///A test for nextBool
        ///</summary>
        [TestMethod()]
        public void nextBoolTest()
        {
            Random r = new Random(0xb00b1e5);
            int trueCount = 0;
            int count = 1000000;
            for (int i = 0; i < count; i++)
            {
                if (RandomExtension.nextBool(r))
                {
                    trueCount++;
                }
            }

            double diffRatio = Math.Abs(((count / 2.0) - trueCount)) / (double) count;
            Assert.IsTrue(diffRatio < 0.0005); //0.005%
        }
    }
}
