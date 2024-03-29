﻿/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace WD_toolbox.Data.DataStructures
    {
    ///



    ///<summary>
    ////// A cheap and nasty SortableArray with no real speed benefits and little security that things remain sorted once the enumerator is given out.
    /// This is a interem untill something more substantial is needed.
    ///</summary>
    public class SortableArray<TYPE> : ICollection<TYPE>, IList<TYPE>, IEnumerable, IEnumerable<TYPE>
    where TYPE : IComparable
        {
        List<TYPE> data;

        public SortableArray()
            {
            data = new List<TYPE>();
            }

        #region ICollection<TYPE> Members

        public void Add(TYPE item)
            {
            data.Add(item);
            data.Sort();
            }

        public void Clear()
            {
            data.Clear();
            }

        public bool Contains(TYPE item)
            {
            return data.Contains(item);
            }

        public void CopyTo(TYPE[] array, int arrayIndex)
            {
            data.CopyTo(array, arrayIndex);
            }

        public int Count
            {
            get { return data.Count; }
            }

        public bool IsReadOnly
            {
            get { return false; }
            }

        public bool Remove(TYPE item)
            {
            return data.Remove(item);
            }

        #endregion

        #region IEnumerable<TYPE> Members

        public IEnumerator<TYPE> GetEnumerator()
            {
            return data.GetEnumerator();
            }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
            {
            return data.GetEnumerator();
            }

        #endregion

        #region IList<TYPE> Members

        public int IndexOf(TYPE item)
            {
            return data.IndexOf(item);
            }

        public void Insert(int index, TYPE item)
            {
            data.Insert(index, item);
            data.Sort();
            }

        public void RemoveAt(int index)
            {
            data.RemoveAt(index);
            }

        public TYPE this[int index]
            {
            get
                {
                return data[index];
                }
            set
                {
                data[index] = value;
                data.Sort();
                }
            }

        #endregion
        }
    }
