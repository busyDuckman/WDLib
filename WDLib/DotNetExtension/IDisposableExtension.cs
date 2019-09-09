/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WD_toolbox.AplicationFramework;

namespace WD_toolbox//.DotNetExtension
{
    public static class IDisposableExtension
    {
        public delegate void ExceptionThrownDelegate(Exception ex);
        public static void TryDispose(this IDisposable item, ExceptionThrownDelegate onFail)
        {
            if (item == null)
            {
                return;
            }
            try
            {
                item.Dispose();
            }
            catch(Exception ex)
            {
                if (onFail != null)
                {
                    onFail(ex);
                }
            }
        }

        public static void TryDispose(this IDisposable item, bool logError=true)
        {
            if (item == null)
            {
                return;
            }

            try
            {
                item.Dispose();
            }
            catch(Exception ex)
            {
                if (logError)
                {
                    WDAppLog.logException(ErrorLevel.Error, ex);
                }
            }
        }
    }
}
