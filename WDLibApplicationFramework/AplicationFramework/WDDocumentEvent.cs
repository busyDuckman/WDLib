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
    public class WDDocumentEvent : WDEvent
        {
        public enum eventTypes {documentCreated=0, documentSaved, documentSavedAs};
        // 0 is document name
        // 1 is user name
        // 2 is computer name
        public string[] eventTypesDescformat = new string[] {"Document {0} created by {1} on {2}",
                                                            "Document {0} saved by {1} on {2}",
                                                            "Document saved as {0} by {1} on {2}"};
        
        public WDDocumentEvent() : base()
            {
            }
        
        public WDDocumentEvent(eventTypes what, WDDocument doc) : base ("", "Document")
            {
            description = string.Format(eventTypesDescformat[(int)what], doc.GetName(), Environment.UserName, Environment.MachineName);
            }
        }
    }
