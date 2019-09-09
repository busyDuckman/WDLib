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
using WD_toolbox.AplicationFramework;

namespace WD_toolbox
{
    public static class EventHandlerExtension
    {
        public static bool SafeCall(this EventHandler e, object sender, Action<Exception> onCallException)
        {
            if (e != null)
            {
                try
                {
                    e(sender, null);
                    return true;
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (onCallException != null)
                        {
                            onCallException(ex);
                        }
                    }
                    catch (Exception ex2)
                    {
                    }
                }
            }
            return false;
        }

        public static bool SafeCall(this EventHandler e, object sender, bool logCallException = false)
        {
            if (e != null)
            {
                try
                {
                    e(sender, null);
                    return true;
                }
                catch (Exception ex)
                {
                    if (logCallException)
                    {
                        WDAppLog.logException(ErrorLevel.Error, ex);
                    }
                }
            }
            return false;
        }

        public static bool SafeCall<T>(this EventHandler<T> e, object sender, T arg, Action<Exception> onCallException)
        {
            if(e != null)
            {
                try
                {
                    e(sender, arg);
                    return true;
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (onCallException != null)
                        {
                            onCallException(ex);
                        }
                    }
                    catch (Exception ex2)
                    {
                    }
                }
            }
            return false;
        }

        public static bool SafeCall<T>(this EventHandler<T> e, object sender, T arg, bool logCallException = false)
        {
            if (e != null)
            {
                try
                {
                    e(sender, arg);
                    return true;
                }
                catch (Exception ex)
                {
                    if (logCallException)
                    {
                        WDAppLog.logException(ErrorLevel.Error, ex);
                    }
                }
            }
            return false;
        }


    }
}
