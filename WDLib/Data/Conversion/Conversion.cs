/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections;
using System.Reflection;

namespace WD_toolbox.Data.Conversion
{
    /// <summary>
    /// Additional type conversion functions
    /// </summary>
    public abstract class Conversion
    {
        /// <summary>
        /// Depreicated: enum's didn't used to have a GetValues() so this was used. I still have not untangled it from everything.
        /// Converts a enumeration into an array of integers representing the values in the enumeration
        /// </summary>
        /// <param name="enumeration">An enumeration type, use typeof(myEnumerarion)</param>
        /// <dep
        /// <returns>An array of integers</returns>
        [Obsolete]
        public static int[] enumToIntList(Type enumeration)
        {
            ArrayList al = new ArrayList();
            FieldInfo[] fields = enumeration.GetFields();
            foreach (FieldInfo field in fields)
            {
                // Continue only for normal fields
                if (field.IsSpecialName) continue;

                // Parameter should be instance of object, but it is ignored
                // for static fileds (this case) so it is not important
                object valObj = field.GetValue(0);

                // Convert value to integer - this works only for enums which use Int32
                int val = (int)valObj;

                al.Add(val);
            }

            int[] ia = new int[al.Count];
            al.CopyTo(ia, 0);
            return ia;
        }
    }
}
