/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WD_toolbox.Data.DataStructures
{
    /// <summary>
    /// An implementation of a 1D Kernel.
    /// TODO: Rather ineffecint, convert to circular buffer.
    /// 
    /// I think this was created for my PhD work.
    /// </summary>
    /// <typeparam name="TYPE"></typeparam>
    public class SlidingWindow<TYPE> : WD_toolbox.Data.DataStructures.ISlidingWindow<TYPE>
    {
        protected List<TYPE> _list;
        int _windowSize;

        public SlidingWindow(int windowSize)
        {
            _list = new List<TYPE>();
            this._windowSize = windowSize;
        }

        public int WindowSize
        {
            get { return _windowSize; }
        }

        public double WindowSizeInUse
        {
            get { return _list.Count; }
        }

        /// <summary>
        /// Progress the kernel to the next item.
        /// </summary>
        /// <param name="value"></param>
        public virtual void Next(TYPE value)
        {
            _list.Add(value);
            while (_list.Count > WindowSize)
            {
                _list.RemoveAt(0);
            }
        }

        public TYPE this[int index]
        {
            get { return _list[index]; }
        }

        public void AddAll(IEnumerable<TYPE> values)
        {
            foreach (TYPE value in values)
            {
                Next(value);
            }
        }

    }
}
