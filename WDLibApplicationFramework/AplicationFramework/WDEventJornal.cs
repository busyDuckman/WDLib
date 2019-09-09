/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Text;
using WD_toolbox.Data.DataStructures;

namespace WD_toolbox.AplicationFramework
    {
    public class WDEventJornal<EVENTTYPE>
    where EVENTTYPE : WDEvent, new()
        {
        public readonly SortableArray<EVENTTYPE> events;

        public WDEventJornal()
            {
            events = new SortableArray<EVENTTYPE>();
            }
        
        public void Add(string what, string catagory)
            {
            EVENTTYPE e = new EVENTTYPE();
            e.catagory = "";
            e.description = what;
            events.Add(e);
            }
        }
    }
