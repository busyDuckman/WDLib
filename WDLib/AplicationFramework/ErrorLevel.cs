/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WD_toolbox.AplicationFramework
{
    public enum ErrorLevel : byte
    {
        /// <summary> debuging related information, not nessesarily an error </summary>
        Debug = 0,

        /// <summary> A behaviour seemed unsual but not nessesary invalid</summary>
        Warning,

        /// <summary> error was small and mostly internalised, user was unlikely to notice</summary>
        SmallError,

        /// <summary> The application suffered a serious error BUT DID NOT SHUT DOWN. </summary>
        Error,

        /// <summary> Critical error, the appliaction had to close.</summary>
        TerminalError,

        /// <summary> The system may no longer be stable (computer needs rebooting)</summary>
        SystemError
    };
}
