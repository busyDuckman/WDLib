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

namespace WD_toolbox.AplicationFramework
{
    /// <summary>
    /// Used by the FileBoundData class.
    /// </summary>
    public interface IReloadFile
    {
        bool ReloadFile(string path);
    }
}
