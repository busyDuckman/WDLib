/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
namespace WD_toolbox.Data.DataStructures
{
    public interface ISlidingWindow<TYPE>
    {
        void Next(TYPE value);
        TYPE this[int index] { get; }
        int WindowSize { get; }
        double WindowSizeInUse { get; }
        void AddAll(IEnumerable<TYPE> values);
    }
}
