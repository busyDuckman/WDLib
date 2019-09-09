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
    ///This is a test class for IDisposableExtensionTest and is intended
    ///to contain all IDisposableExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IDisposableExtensionTest
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
        ///A test for TryDispose
        ///</summary>
        [TestMethod()]
        public void TryDisposeTest()
        {
            IDisposable item = null; // TODO: Initialize to an appropriate value
            bool logError = false; // TODO: Initialize to an appropriate value
            IDisposableExtension.TryDispose(item, logError);
        }

        /// <summary>
        ///A test for TryDispose
        ///</summary>
        [TestMethod()]
        public void TryDisposeTest1()
        {
            IDisposable item = null; // TODO: Initialize to an appropriate value
            IDisposableExtension.ExceptionThrownDelegate onFail = null; // TODO: Initialize to an appropriate value
            IDisposableExtension.TryDispose(item, onFail);
        }
    }
}
