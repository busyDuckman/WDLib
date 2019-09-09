/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections;

using WD_toolbox.Data.Conversion;
using System.Collections.Generic;

namespace WD_toolbox.Data.Manipulation
{
    /// <summary>
    /// This was mande before Linq in a basic attempt to querey Ilists with an sql mindset.
    /// It's redundent but intertwined with other code.
    /// </summary>
    [Obsolete]
    public sealed class Querey
    {
        #region selection and searching
        /// <summary>
        /// A delegate for a custom trinary operator
        /// </summary>
        public delegate bool multipleConditionDelegate(object a, object p1, object p2);
        /// <summary>
        /// A delegate for a custom binary operator
        /// </summary>
        public delegate bool equalDelegate(object a, object b);
        /// <summary>
        /// A delegate for a custom unary operator
        /// </summary>
        public delegate bool conditionDelegate(object a);
        /// <summary>
        /// Finds the first object in a list where a certain user defined equality is true
        /// </summary>
        /// <param name="data">Dataset to search.</param>
        /// <param name="what">What to lookfor.</param>
        /// <param name="equality">How to determine if the object in the list is equivilent to the item we are searching for.</param>
        /// <returns>Index of the value in the list, -1 if item was not found.</returns>
        public static int search(IList data, object what, equalDelegate equality)
        {
            int i;
            for (i = 0; i < data.Count; i++)
            {
                if (equality(what, data[i]))
                {
                    return i;
                }
            }
            return -1;
        }


        /// <summary>
        /// akin to "select * from data where equality(what, data)"
        /// </summary>
        /// <param name="data">Dataset to search.</param>
        /// <param name="what">What to lookfor.</param>
        /// <param name="equality">How to determine if the object in the list is equivilent to the item we are searching for.</param>
        /// <returns>A new dataset representing the information meeting the specified criteria.</returns>
        public static object[] allFormListWhere(IList data, object what, equalDelegate equality)
        {
            ArrayList items = new ArrayList();
            int i;
            for (i = 0; i < data.Count; i++)
            {
                if (equality(what, data[i]))
                {
                    items.Add(data[i]);
                }
            }
            return items.ToArray();
        }

        /// <summary>
        /// akin to "select * from data where condition(data)"
        /// </summary>
        /// <param name="data">Dataset to search.</param>
        /// <param name="condition"></param>
        /// <returns>A new dataset representing the information meeting the specified criteria.</returns>
        public static object[] allFormListWhere(IList data, conditionDelegate condition)
        {
            ArrayList items = new ArrayList();
            int i;
            for (i = 0; i < data.Count; i++)
            {
                if (condition(data[i]))
                {
                    items.Add(data[i]);
                }
            }
            return items.ToArray();
        }

        /// <summary>
        /// akin to "select * from data where condition(data, a, b)"
        /// </summary>
        /// <param name="data"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="condition"></param>
        /// <returns>A new dataset representing the information meeting the specified criteria.</returns>
        public static object[] allFormListWhere(IList data, object a, object b, multipleConditionDelegate condition)
        {
            ArrayList items = new ArrayList();
            int i;
            for (i = 0; i < data.Count; i++)
            {
                if (condition(data[i], a, b))
                {
                    items.Add(data[i]);
                }
            }
            return items.ToArray();
        }


        /// <summary>
        /// akin to "select * from data where (data as IComparable) in (min(a,b), max(a,b))"
        /// </summary>
        /// <param name="data">Dataset to search.</param>
        /// <param name="high">Range bound.</param>
        /// <param name="low">Range bound.</param>
        /// <returns>A new dataset representing the information meeting the specified criteria.</returns>
        ///<remarks>data[n] must implement IComparable or an exeption will be thrown</remarks>
        public static object[] allFormListWhereIN(IList data, IComparable high, IComparable low)
        {
            IComparable d, l, h;
            ArrayList items = new ArrayList();
            int i;

            if (less(low, high))
            {
                l = low;
                h = high;
            }
            else
            {
                l = high;
                h = low;
            }

            for (i = 0; i < data.Count; i++)
            {
                //let the execption be generated
                d = (IComparable)data[i];
                if (greaterOrEqual(d, l) && lessOrEqual(d, h))
                {
                    items.Add(data[i]);
                }
            }
            return items.ToArray();
        }

        /// <summary>
        /// akin to sql plus IN statment. (range checking).
        /// </summary>
        /// <param name="a">Value to check</param>
        /// <param name="lower">Range bound.</param>
        /// <param name="higher">Range bound</param>
        /// <returns>True if the value was in the range.</returns>
        ///<remarks>Automaticaly exchanges lower and higher if higher less than lower</remarks>
        public static bool IN(IComparable a, IComparable lower, IComparable higher)
        {
            IComparable l, h;
            if (less(lower, higher))
            {
                l = lower;
                h = higher;
            }
            else
            {
                l = higher;
                h = lower;
            }

            return (greaterOrEqual(a, l) && lessOrEqual(a, h));
        }

        public static int indexOf<Type>(Type[] list, Type value)
        where Type : IEquatable<Type>
        {
            int i;
            for (i = 0; i < list.Length; i++)
            {
                if (list[i].Equals(value))
                {
                    return i;
                }
            }
            return -1;
        }

        public static int indexOfCastInt(IList list, int value)
        {
            int i;
            for (i = 0; i < list.Count; i++)
            {
                if (((int)list[i]) == value)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion

        /// <summary>
        /// Akin to sql plus NVL, used to provide a substitute value if an object is null.
        /// </summary>
        /// <param name="a">Object in question.</param>
        /// <param name="ifNull">Substitute value to "a" if "a" is null.</param>
        /// <returns>"a" iff "a" not null; else "ifNull"</returns>
        public static object NVL(object a, object ifNull)
        {
            if (a == null)
                return ifNull;
            else
                return a;
        }

        #region comparision
        /// <summary>
        /// a less than b
        /// </summary>
        /// <param name="a">a value</param>
        /// <param name="b">another value</param>
        /// <returns>true if a is less than b; otherwise false</returns>
        public static bool less(IComparable a, IComparable b)
        {
            return (a.CompareTo(b) < 0);
        }

        /// <summary>
        /// a > b
        /// </summary>
        /// <param name="a">a value</param>
        /// <param name="b">another value</param>
        /// <returns>true if a > b; otherwise false</returns>
        public static bool greater(IComparable a, IComparable b)
        {
            return (a.CompareTo(b) > 0);
        }

        /// <summary>
        /// a less than or equal to b
        /// </summary>
        /// <param name="a">a value</param>
        /// <param name="b">another value</param>
        /// <returns>true if a less than or equal to b; otherwise false</returns>
        public static bool lessOrEqual(IComparable a, IComparable b)
        {
            return (a.CompareTo(b) <= 0);
        }

        /// <summary>
        /// a >= b
        /// </summary>
        /// <param name="a">a value</param>
        /// <param name="b">another value</param>
        /// <returns>true if a >= b; otherwise false</returns>
        public static bool greaterOrEqual(IComparable a, IComparable b)
        {
            return (a.CompareTo(b) >= 0);
        }

        /// <summary>
        /// a == b
        /// </summary>
        /// <param name="a">a value</param>
        /// <param name="b">another value</param>
        /// <returns>true if a == b; otherwise false</returns>
        public static bool equal(IComparable a, IComparable b)
        {
            return (a.CompareTo(b) == 0);
        }
        #endregion
    }
}
