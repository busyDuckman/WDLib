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

namespace WD_toolbox
{
    public static class StackExtension
    {
        /// <summary>
        /// Pushes a set of items on a stack, in the order they are enumerated.
        /// </summary>
        public static void PushAll<T>(this Stack<T> stack, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                stack.Push(item);
            }
        }
    }
}
