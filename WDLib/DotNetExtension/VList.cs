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
    /*/// <summary>
    /// A version of List with all methods being virtual.
    /// Used when it is nesseary to overide a few methods, but still have polymorphisim against IList<>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class VList<T> : IList<T>
    {
        private List<T> inner;

        public VList()
        {
            inner = new List<T>();
        }

        public VList(int cpacity)
        {
            inner = new List<T>(cpacity);
        }

        public VList(IEnumerable<T> collection)
        {
            inner = new List<T>(collection);
        }

        public virtual int IndexOf(T item)
        {
            return inner.IndexOf(item);
        }

        public virtual void Insert(int index, T item)
        {
            inner.Insert(index, item);
        }

        public virtual void RemoveAt(int index)
        {
            inner.RemoveAt(index);
        }

        public virtual T this[int index]
        {
            get
            {
                return inner[index];
            }
            set
            {
                inner[index]= value;
            }
        }

        public virtual void Add(T item)
        {
            inner.Add(item);
        }

        public virtual void Clear()
        {
            inner.Clear();
        }

        public virtual bool Contains(T item)
        {
            return inner.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            inner.CopyTo(array, arrayIndex);
        }

        public virtual int Count
        {
            get { return inner.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return ((IList<T>)inner).IsReadOnly; }
        }

        public virtual bool Remove(T item)
        {
            return inner.Remove(item);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return inner.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)inner).GetEnumerator();
        }
    }
     */
}
