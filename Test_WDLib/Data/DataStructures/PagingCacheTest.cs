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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WD_toolbox.Data.DataStructures;

namespace Test_WDLib.Data.DataStructures
{
    [TestClass()]
    public class PagingCacheTest
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

        /// <summary>
        ///A test for DoesStringMeanFalse
        ///</summary>
        [TestMethod()]
        public void DoPagingCacheTest()
        {
            //PagingCache pc = new PagingCache(PagingCache.pagingAlgorithems.LRU, false, 
        }
    }
}
