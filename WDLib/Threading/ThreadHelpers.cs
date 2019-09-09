﻿/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WD_toolbox.Threading
{
    public static class ThreadHelpers
    {
        public static Thread RunAsBackground(Action action)
        {
            Thread t = new Thread(new ThreadStart(action));
            t.IsBackground = true;
            t.Start();
            return t;
        }


    }
}
