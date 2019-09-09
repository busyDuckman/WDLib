/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WD_toolbox//.DotNetExtension
{
    public static class StringBuilderExtension
    {
        /// <summary>
        /// Appeands all items to the builder
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static void Append(this StringBuilder sb, IEnumerable<string> strings)
        {
            foreach(string s in strings)
            {
                sb.Append(s);
            }
        }
    }
}
