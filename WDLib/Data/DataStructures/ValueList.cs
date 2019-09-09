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

namespace WD_toolbox.Data.DataStructures
{
    /// <summary>
    /// I honestly have no idea. I must have written it in the middle of the night.
    /// Marked as obsolete until I figure out what it's used in.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Obsolete]
    public class ValueList<T> : VList<T>
         where T : class 
    {
        public delegate T NormaliseDelegate(T value);
        public NormaliseDelegate Normailse {get; protected set;}

        [NonSerialized]
        object _lock = new object();

        public ValueList(NormaliseDelegate normailse=null) : base()
        {
            Normailse = normailse;
        }

        public ValueList(int capacity, NormaliseDelegate normailse = null)
            : base(capacity)
        {
            Normailse = normailse;
        }

        public ValueList(IEnumerable<T> collection, NormaliseDelegate normailse = null)
            : base(collection)
        {
            Normailse = normailse;
        }

        public T NormaliseValue(T value)
        {
            if(Normailse != null)
            {
                return Normailse(value);
            }
            else
            {
                return value;
            }
        }

        public int FindOrInsert(T item)
        {
            if(item == null)
            {
                return -1;
            }
            
            T norm = NormaliseValue(item);
            int pos = this.IndexOf(norm);
            if(pos < 0)
            {
                Add(norm);
                return this.Count - 1;
            }
            return pos;
        }

    }

    [Obsolete]
    public class ListValue<T>  
        where T : class 
    {
        ValueList<T> list;
        int index;

        public int Index { get { return index; } }

        public T Value 
        {
            get
            {
                return (index < 0) ? null : list[index];
            }

            set
            {
                index = list.FindOrInsert(value);
            }
        }

        public ListValue(T value, ValueList<T> backedList)
        {
            list = backedList;
            Value = value;
        }

        protected ListValue(ValueList<T> backedList)
        {
            list = backedList;
        }

        public static ListValue<T> FromIndex(ValueList<T> backedList, int index)
        {
            ListValue<T> lv = new ListValue<T>(backedList);
            lv.index = index;
            return lv;
        }

        public static implicit operator T(ListValue<T> val)
        {
            return val.Value;
        }
    }
}
