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
    public class WDDocument : ICloneable
        {
        public WDDocument()
        {
        }
        
        WDEventJornal<WDDocumentEvent> history;
        
        protected WDDocument(WDDocument another)
            {
            history = new WDEventJornal<WDDocumentEvent>();
            history.Add("Document Created", "edit");
            }
        
        #region ICloneable Members
        
        public object Clone()
            {
            return new WDDocument(this);            
            }

        #endregion

        internal string GetName()
            {
            throw new NotImplementedException();
            }
        }
    }
