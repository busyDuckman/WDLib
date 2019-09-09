/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using WD_toolbox;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Test_WDLib
{
    
    
    /// <summary>
    ///This is a test class for IListExtensionTest and is intended
    ///to contain all IListExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IListExtensionTest : TestBase
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
        ///A test for GenerateBindingSourced
        ///</summary>
        public void GenerateBindingSourcedTestHelper<T>()
        {
            IList<T> list = null; // TODO: Initialize to an appropriate value
            bool readOnly = false; // TODO: Initialize to an appropriate value
            BindingSource expected = null; // TODO: Initialize to an appropriate value
            BindingSource actual;
            actual = IListExtension.GenerateBindingSourced<T>(list, readOnly);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GenerateBindingSourcedTest()
        {
            GenerateBindingSourcedTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for GetRandomItem
        ///</summary>
        public void GetRandomItemTestHelper<T>()
        {
            IList<T> list = null; // TODO: Initialize to an appropriate value
            T expected = default(T); // TODO: Initialize to an appropriate value
            T actual;
           // actual = IListExtension.GetRandomItem<T>(list);
            //Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetRandomItemTest()
        {
            GetRandomItemTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for GetRandomItem
        ///</summary>
        public void GetRandomItemTest1Helper<T>()
        {
            IList<T> list = null; // TODO: Initialize to an appropriate value
            Random rnd = null; // TODO: Initialize to an appropriate value
            T expected = default(T); // TODO: Initialize to an appropriate value
            T actual;
            //actual = IListExtension.GetRandomItem<T>(list, rnd);
            //Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetRandomItemTest1()
        {
            GetRandomItemTest1Helper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for IndexOf
        ///</summary>
        public void IndexOfTestHelper<TYPE>(TYPE[] data, TYPE what, int pos)
        {

            int p = IListExtension.IndexOf<TYPE>(data, T => T.Equals(what));
            Assert.AreEqual(p, pos);
        }

        [TestMethod()]
        public void IndexOfTest()
        {
            IndexOfTestHelper<int>(new int[] { 2, 3 , 1 }, 1, 2);
            IndexOfTestHelper<int>(new int[] { 1, 2, 3, 1 }, 1, 0);
            IndexOfTestHelper<int>(new int[] { 1, 2, 3, 1 }, 7, -1);
            IndexOfTestHelper<int>(new int[] {}, 7, -1);
            IndexOfTestHelper<int>(null, 7, -1);
        }

        /// <summary>
        ///A test for RemoveAllNulls
        ///</summary>
        public void RemoveAllNullsTestHelper<T>()
            where T : class
        {
            IList<T> list = null; // TODO: Initialize to an appropriate value
            IListExtension.RemoveAllNulls<T>(list);
        }

        [TestMethod()]
        public void RemoveAllNullsTest()
        {
            RemoveAllNullsTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Shuffle
        ///</summary>
        public void ShuffleTestHelper<T>()
        {
            IList<T> list = null; // TODO: Initialize to an appropriate value
            IListExtension.Shuffle<T>(list);
        }

        [TestMethod()]
        public void ShuffleTest()
        {
            ShuffleTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Shuffle
        ///</summary>
        public void ShuffleTest1Helper<T>()
        {
            IList<T> list = null; // TODO: Initialize to an appropriate value
            Random rnd = null; // TODO: Initialize to an appropriate value
            IListExtension.Shuffle<T>(list, rnd);
        }

        [TestMethod()]
        public void ShuffleTest1()
        {
            ShuffleTest1Helper<GenericParameterHelper>();
        }
    }
}
