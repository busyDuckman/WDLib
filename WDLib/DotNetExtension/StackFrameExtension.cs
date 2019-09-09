/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WD_toolbox//.DotNetExtension
{
    public static class StackFrameExtension
    {
        public static string GetADecentExplination(this StackFrame sf)
        {
            string expl = "";
            string file = sf.GetFileName();
            MethodBase func = sf.GetMethod();
            string funcName = func.Name;
            int line = sf.GetFileLineNumber();
            int col = sf.GetFileColumnNumber();

            if (!string.IsNullOrEmpty(file))
            {
                expl += "File: " + file;
            }

            if (!string.IsNullOrEmpty(funcName))
            {
                expl += "Method: " + funcName;
            }

            if (line >= 0)
            {
                expl += "Line: " + line;
                if (col >= 0)
                {
                    expl += "Col: " + line;
                }
            }



            return expl;
        }
    }
}
