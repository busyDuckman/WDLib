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
    public static class ActionExtension
    {

        public static bool SafeCall(this Action action, Action<Exception> onCallException)
        {
            if (action != null)
            {
                try
                {
                    action();
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

        public static bool SafeCall(this Action action, bool logCallException = false)
        {
            if (action != null)
            {
                try
                {
                    action();
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

        public static bool SafeCall<T>(this Action<T> action, T arg, Action<Exception> onCallException)
        {
            if (action != null)
            {
                try
                {
                    action(arg);
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

        public static bool SafeCall<T>(this Action<T> action, T arg, bool logCallException = false)
        {
            if (action != null)
            {
                try
                {
                    action(arg);
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

        public static bool SafeCall<T, T2>(this Action<T, T2> action, T arg, T2 arg2, Action<Exception> onCallException)
        {
            if (action != null)
            {
                try
                {
                    action(arg, arg2);
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

        public static bool SafeCall<T, T2>(this Action<T, T2> action, T arg, T2 arg2, bool logCallException = false)
        {
            if (action != null)
            {
                try
                {
                    action(arg, arg2);
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



        public static bool SafeCall<T, T2, T3>(this Action<T, T2, T3> action, T arg, T2 arg2, T3 arg3, Action<Exception> onCallException)
        {
            if (action != null)
            {
                try
                {
                    action(arg, arg2, arg3);
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

        public static bool SafeCall<T, T2, T3>(this Action<T, T2, T3> action, T arg, T2 arg2, T3 arg3, bool logCallException = false)
        {
            if (action != null)
            {
                try
                {
                    action(arg, arg2, arg3);
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
