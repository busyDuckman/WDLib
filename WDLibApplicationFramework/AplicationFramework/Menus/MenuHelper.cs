/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WDLibApplicationFramework.AplicationFramework.Menus
{
    public static class MenuHelper
    {
        /*public static void visitAllUnimplementedMenuItems(IMenuItemWrapper baseItem, Action<IMenuItemWrapper> action)
        {
            visitAllMenuItems(baseItem, M => action(M), M => !(M.IsImplemented || M.HasSubMenus));
        }*/

        public static void visitAllMenuItems(IMenuItemWrapper baseItem, Action<IMenuItemWrapper> action, Predicate<IMenuItemWrapper> where)
        {
            visitAllMenuItems(baseItem, M => { if (where(M)) { action(M); } });
        }

        public static void visitAllMenuItems(IMenuItemWrapper item, Action<IMenuItemWrapper> action)
        {
            //recursive, because menus are shallow.
            action(item);
            foreach(IMenuItemWrapper subMenuItem in item.SubMenus)
            {
                visitAllMenuItems(subMenuItem, action);
            }
        }

        /// <summary>
        /// Code found floating on forums... (http://stackoverflow.com/questions/3212721/check-if-the-control-has-events-on-click-eventhandler)
        /// </summary>
        public static bool HasEventHandler(object control, string eventName = "EventClick")
        {
            EventHandlerList events =
                (EventHandlerList)
                typeof(Component)
                 .GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance)
                 .GetValue(control, null);

            object key = typeof(Control)
                .GetField(eventName, BindingFlags.NonPublic | BindingFlags.Static)
                .GetValue(null);

            Delegate handlers = events[key];

            return handlers != null && handlers.GetInvocationList().Any();
        }
    }
}
