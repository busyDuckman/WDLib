/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox.Data.DataStructures;

namespace WDLibApplicationFramework.AplicationFramework.Actions
{
    public class WDAction
    {
        public delegate Why WDActionDelegate(object sender, EventArgs args);

        public String Name { get; protected set; }
        public Icon Icon                { get; protected set; }
        public String Description       { get; protected set; }
        public WDActionDelegate Action  { get; protected set; }

        public WDAction(String name, Icon icon, WDActionDelegate action)
        {
            init(name, icon, null, action);
        }


        public WDAction(String name, WDActionDelegate action)
        {
            init(name, null, null, action);
        }

        public WDAction(String name, Icon icon, String description, WDActionDelegate action)
        {
            init(name, icon, description, action);
        }

        public WDAction(String name, String description, WDActionDelegate action)
        {
            init(name, null, description, action);
        }

        protected void init(String name, Icon icon, String description, WDActionDelegate action)
        {
            this.Name = name;
            this.Icon = icon;
            this.Description = description;
            this.Action = action;
        }

        public virtual Why DoAction(object sender, EventArgs args)
        {
            if(Action != null)
            {
                return Action(sender, args);
            }

            return Why.FalseBecause("No action method supplied");
        }
    }

    

 
}
