/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WDDevice.Coms
{
    public interface IAuthenticatedSession
    {
        string UserName { get; set; }
        string Password { get; set; }
        int LoginRetyLimit { get; set; }
        TimeSpan LoginRetryDelay { get; set; }

        bool  AuthenticationFailed  { get; }
    }
}
