/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
#define HIDE_EXCEPTIONS
using System;
using System.Collections;
using System.Collections.Generic;

namespace WD_toolbox.Data.DataStructures
{
    /// <summary>
    /// Ancient code from .NET 1.0 (updated to 2.0). NB: I was a student when I made this.
    /// Not thread safe, and can probably be converted to extension methods.
    /// A first in, first or last out data storage structure
    /// It's called a Quack (QUeue stACK).
    /// 
    /// It only exists for legacy purposes now.
    /// </summary>
    [Serializable]
    [Obsolete]
    public class Quack : ICollection, IList, IQuack
    {
        #region instance data
        protected ArrayList buffer;
        #endregion

        #region procedural accessors
        #endregion

        public Quack()
        {
            buffer = new ArrayList();
        }

        public Quack(int initialSize)
        {
            buffer = new ArrayList(initialSize);
        }

        #region IQuack Members
        /// <summary>
        /// Adds an item to the front of the quack
        /// aka. top od the stack, or head of the queeue
        /// Same operation as Enqueue
        /// </summary>
        /// <param name="item"></param>
        public void Push(object item)
        {
            buffer.Add(item);
        }

        /// <summary>
        /// Remove last in
        /// </summary>
        /// <param name="item"></param>
        public object Pop()
        {
#if HIDE_EXCEPTIONS
            if (IsEmpty())
                return null;
#endif

            object item = buffer[buffer.Count - 1];
            buffer.RemoveAt(buffer.Count - 1);
            return item;
        }

        /// <summary>
        /// Adds an item to the front of the quack
        /// aka. top od the stack, or head of the queeue
        /// Same operation as Push
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(object item)
        {
            Push(item);
        }

        /// <summary>
        /// Remove from begining (fifo) 
        /// </summary>
        /// <returns></returns>
        public object Dequeue()
        {
#if HIDE_EXCEPTIONS
            if (IsEmpty())
                return null;
#endif

            object item = buffer[0];
            buffer.RemoveAt(0);
            return item;
        }

        /// <summary>
        /// Returns true if the quack is empty; otherwise false
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return (buffer.Count == 0);
        }

        /// <summary>
        /// Removes all items from the quack
        /// </summary>
        public void Clear()
        {
            buffer.Clear();
        }

        /// <summary>
        /// Peek from the stack perspective
        /// </summary>
        /// <returns></returns>
        public object PeekTop()
        {
#if HIDE_EXCEPTIONS
            if (IsEmpty())
                return null;
#endif

            return buffer[buffer.Count - 1];
        }

        /// <summary>
        /// Peek from the queue perspective (queue obviously has an ass fetish)
        /// </summary>
        /// <returns></returns>
        public object PeekBottom()
        {
#if HIDE_EXCEPTIONS
            if (IsEmpty())
                return null;
#endif

            return buffer[0];
        }

        #endregion

        #region ICollection Members

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public int Count
        {
            get
            {
                return buffer.Count;
            }
        }

        public void CopyTo(Array array, int index)
        {
            buffer.CopyTo(array, index);
        }

        public object SyncRoot
        {
            get
            {
                return null;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return buffer.GetEnumerator();
        }

        #endregion

        #region IList Members

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public object this[int index]
        {
            get
            {
                return buffer[index];
            }
            set
            {
                buffer[index] = value;
            }
        }

        public void RemoveAt(int index)
        {
            buffer.RemoveAt(index);
        }

        public void Insert(int index, object value)
        {
            buffer.Insert(index, value);
        }

        public void Remove(object value)
        {
            buffer.Remove(value);
        }

        public bool Contains(object value)
        {
            return buffer.Contains(value);
        }

        public int IndexOf(object value)
        {
            return buffer.IndexOf(value);
        }

        public int Add(object value)
        {
            return buffer.Add(value);
        }

        public bool IsFixedSize
        {
            get
            {
                return buffer.IsFixedSize;
            }
        }

        #endregion
    }

    /// <summary>
    /// Ancient code from .NET 1.0 (updated to 2.0). NB: I was a student when I made this.
    /// Not thread safe, and can probably be converted to extension methods.
    /// A first in, first or last out data storage structure
    /// It's called a Quack (QUeue stACK).
    /// 
    /// It only exists for legacy purposes now.
    /// </summary>
    [Serializable]
    [Obsolete]
    public class Quack<datumType> : ICollection<datumType>, IList<datumType>, IQuack<datumType>
    {
        #region instance data
        protected List<datumType> buffer;
        #endregion

        #region procedural accessors
        #endregion

        public Quack()
        {
            buffer = new List<datumType>();
        }

        public Quack(int initialSize)
        {
            buffer = new List<datumType>(initialSize);
        }

        #region IQuack<datumType> Members


        /// <summary>
        /// Remove from begining (fifo) 
        /// </summary>
        /// <returns></returns>
        public object Dequeue()
        {
#if HIDE_EXCEPTIONS
            if (IsEmpty())
                return null;
#endif

            object item = buffer[0];
            buffer.RemoveAt(0);
            return item;
        }

        /// <summary>
        /// Adds an item to the front of the quack
        /// aka. top od the stack, or head of the queeue
        /// Same operation as push
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(datumType item)
        {
            Push(item);
        }

        /// <summary>
        /// Returns true if the quack is empty; otherwise false
        /// </summary>
        public bool IsEmpty()
        {
            return (buffer.Count == 0);
        }

        /// <summary>
        /// Peek from the queue perspective (queue obviously has an ass fetish)
        /// </summary>
        /// <returns></returns>
        public datumType PeekBottom()
        {
#if HIDE_EXCEPTIONS
            if (IsEmpty())
                return default(datumType);
#endif

            return buffer[0];
        }

        /// <summary>
        /// Peek from the stack perspective
        /// </summary>
        /// <returns></returns>
        public datumType PeekTop()
        {
#if HIDE_EXCEPTIONS
            if (IsEmpty())
                return default(datumType);
#endif

            return buffer[buffer.Count - 1];
        }

        /// <summary>
        /// Remove last in
        /// </summary>
        /// <param name="item"></param>
        public datumType Pop()
        {
#if HIDE_EXCEPTIONS
            if (IsEmpty())
                return default(datumType);
#endif

            datumType item = buffer[buffer.Count - 1];
            buffer.RemoveAt(buffer.Count - 1);
            return item;
        }

        /// <summary>
        /// Adds an item to the front of the quack
        /// aka. top od the stack, or head of the queeue
        /// Same operation as Enqueue
        /// </summary>
        /// <param name="item"></param>
        public void Push(datumType item)
        {
            buffer.Add(item);
        }

        #endregion

        #region ICollection<datumType> Members

        public void Add(datumType item)
        {
            buffer.Add(item);
        }

        public void Clear()
        {
            buffer.Clear();
        }

        public bool Contains(datumType item)
        {
            return buffer.Contains(item);
        }

        public void CopyTo(datumType[] array, int arrayIndex)
        {
            buffer.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return buffer.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(datumType item)
        {
            return buffer.Remove(item);
        }

        #endregion

        #region IEnumerable<datumType> Members

        public IEnumerator<datumType> GetEnumerator()
        {
            return buffer.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return buffer.GetEnumerator();
        }

        #endregion

        #region IList<datumType> Members

        public int IndexOf(datumType item)
        {
            return buffer.IndexOf(item);
        }

        public void Insert(int index, datumType item)
        {
            buffer.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            buffer.RemoveAt(index);
        }

        public datumType this[int index]
        {
            get
            {
                return buffer[index];
            }
            set
            {
                buffer[index] = value;
            }
        }

        #endregion
    }
}
