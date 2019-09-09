/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using WD_toolbox;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Test_WDLib
{
    
    
    /// <summary>
    ///This is a test class for StringExtensionTest and is intended
    ///to contain all StringExtensionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StringExtensionTest
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
        ///A test for DequeueFirstChar
        ///</summary>
        [TestMethod()]
        public void DequeueFirstCharTest()
        {
            string[] tests = new string[] {"foobar", "", null, "f", "\tmoo"};
            string[] resA = new string[] { "oobar", "", "", "", "moo" };
            char[] resB = new char[] { 'f', '\0', '\0', 'f', '\t' };

            for(int i=0; i<tests.Length; i++)
            {
                char actualB;
                string actualA = StringExtension.DequeueFirstChar(tests[i], out actualB);
                Assert.AreEqual(resA[i], actualA);
                Assert.AreEqual(resB[i], actualB);
            }
        }

        /// <summary>
        ///A test for getLines
        ///</summary>
        [TestMethod()]
        public void getLinesTest()
        {
            string text = "test\r\n"+
                           "f\r"+
                           "\r\n"+
                           " \r\n"+
                           " boo\n"+
                           "last ";

            List<string> expected = new List<string>()
                {"test", "f", "boo", "last"};

            List<string> actual = StringExtension.GetLines(text, StringExtension.PruneOptions.EmptyOrWhiteSpaceLines, true);
            Assert.IsTrue(expected.CompareElementsInOrderAndEqual(actual));
        }

        /// <summary>
        ///A test for ChangeCaseAndLayout
        ///</summary>
        [TestMethod()]
        public void ChangeCaseAndLayoutTest()
        {
            Assert.AreEqual(" Does it work ".ChangeCaseAndLayout(CaseAndLayout.Camel_Hyphen),
                            "Does_It_Work");

            Assert.AreEqual(" Does it work ".ChangeCaseAndLayout(CaseAndLayout.camelBack),
                            "doesItWork");

            Assert.AreEqual(" Does it work ".ChangeCaseAndLayout(CaseAndLayout.CamelCase),
                            "DoesItWork");

            Assert.AreEqual(" Does it work ".ChangeCaseAndLayout(CaseAndLayout.lower_hyphen),
                            "does_it_work");

            Assert.AreEqual(" Does it work ".ChangeCaseAndLayout(CaseAndLayout.NoChange),
                            " Does it work ");

            Assert.AreEqual(" does it work ".ChangeCaseAndLayout(CaseAndLayout.SentanceWithFullStop),
                            "Does it work.");

            Assert.AreEqual(" Does it work ".ChangeCaseAndLayout(CaseAndLayout.SentanceWithoutFullStop),
                            "Does it work");

            Assert.AreEqual(" Does it work ".ChangeCaseAndLayout(CaseAndLayout.UPPER_HYPHEN),
                            "DOES_IT_WORK");

        }

        [TestMethod()]
        public void ParseAllIntegersTest()
        {
            Assert.IsTrue("0 1 2 3 45".ParseAllIntegers().CompareElementsInOrderAndEqual(
                             new int[] {0, 1, 2, 3, 45}));
            Assert.IsTrue("0".ParseAllIntegers().CompareElementsInOrderAndEqual(
                             new int[] { 0, }));
            Assert.IsTrue("0 -1 2 3 -45".ParseAllIntegers().CompareElementsInOrderAndEqual(
                             new int[] { 0, -1, 2, 3, -45 }));

            Assert.IsTrue("+3-1-45".ParseAllIntegers().CompareElementsInOrderAndEqual(
                             new int[] { 3, -1, -45}));

            Assert.IsTrue("the 4th character on line 2.".ParseAllIntegers().CompareElementsInOrderAndEqual(
                             new int[] { 4, 2}));

            Assert.IsTrue("skjfdhkjdhsfkj".ParseAllIntegers().Length == 0);
            Assert.IsTrue("".ParseAllIntegers().Length == 0);
            Assert.IsTrue("-".ParseAllIntegers().Length == 0);
            Assert.IsTrue("+".ParseAllIntegers().Length == 0);
        }
    }
}
