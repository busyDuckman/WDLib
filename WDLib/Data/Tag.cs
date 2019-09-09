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

namespace WD_toolbox.Data
{
    /// <summary>
    /// useful for items in list boxes, etc
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Tag<T>
    {
        public string Text { get; protected set; }
        public T      Data{ get; protected set; }

        public Tag(T data, string text)
        {
            Text = text;
            Data = data;
        }

        private Tag(T data)
        {
            Data = data;
            Text = data.ToString();
        }

        public override string ToString()
        {
            return Text;
        }

        public static implicit operator T(Tag<T> tag)
        {
            return tag.Data;
        }

        public static Tag<T> fromTagedWithToString(T data)
        {
            return new Tag<T>(data);
        }
    }
}
