/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace WD_toolbox.AplicationFramework
    {
    public class WDEvent : IComparable
        {
        public string description;
        public string catagory;
        public DateTime when;

        public WDEvent()
            {
            description = "";
            catagory = "";
            when = DateTime.Now;
            }

        public WDEvent(string description, string catagory) : this(description, catagory, DateTime.Now)
            {
            }
        
        public WDEvent(string description, string catagory, DateTime when)
            {
            this.description = description;
            this.catagory = catagory;
            this.when = when;
            }

        #region IComparable Members

        public int CompareTo(object obj)
            {
            if(obj is DateTime)
                {
                return when.CompareTo((DateTime)obj);
                }
            
            WDEvent other = obj as WDEvent;
            if(other == null)
                {
                throw new InvalidOperationException("WDEvent can not compare to type of " + obj.GetType().ToString());
                }
            
            return when.CompareTo(other.when);
            }

        #endregion
        }
    }
