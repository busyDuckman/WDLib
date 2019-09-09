/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WD_toolbox.AplicationFramework;

namespace Test_WDLib.AplicationFramework
{
    [TestClass]
    public class WDAppLogTest : TestBase
    {
        [TestMethod]
        public void CreateErrors()
        {
            WDAppLog.UseErrorLogString = true;
            foreach(ErrorLevel level in Enum.GetValues(typeof(ErrorLevel)))
            {
                WDAppLog.logError(level, "Testing log error for " + level);
                WDAppLog.logError(level, "This one has aditional Info", "Extra information goes here");

                NullReferenceException ex = new NullReferenceException("Just a test");
                WDAppLog.logException(level, ex);
                
                string file = @"c:\filedoesnotexist.txt";
                try 
                {
                    string[] na = File.ReadAllLines(file);
                }
                catch(Exception ex2)
                {
                    WDAppLog.logFileOpenError(ErrorLevel.Error, ex2, file);
                }

                //create an exception deep in a call stack
                CreateExceptionA(level);
            }

            //Debug.WriteLine(WDAppLog.errorLogString);
        }

        private static void CreateExceptionA(ErrorLevel level)
        {
            try
            {
                CreateExceptionB();
            }
            catch (Exception ex2)
            {
                WDAppLog.logException(level, ex2);
            }
        }

        private static void CreateExceptionB()
        {
            string s = null;
            int i = s.IndexOf('3');
        }
    }
}
