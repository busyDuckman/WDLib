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
using System.Windows.Forms;
using WD_toolbox.Data.DataStructures;

namespace WDLibApplicationFramework.AplicationFramework.Actions
{
    /// <summary>
    /// This is a data structure for holding a menu.
    /// It can be realised as different widgets for different API's.
    /// </summary>
    public class ActionMenuItem
    {
        public enum MenuType { Item, Parent, Seperator, Checked}

        //Apearance
        public string Name { get; protected set; }
        public MenuType Type { get; protected set; }
        public Why Enabled { get; set; }
        public Icon MenuIcon { get; protected set; }

        //Parent
        public List<ActionMenuItem> SubMenuItems { get; protected set; }

        //Item
        public WDAction Action { get; set; }

        //Checked
        public bool Checked { get; set; }

        protected ActionMenuItem(string name, MenuType type)
        {
            Type = type;
            Name = name;
            Enabled = true;
            MenuIcon = null;
            Checked = false;
        }

        public static ActionMenuItem CreateParentMenu(string name, List<ActionMenuItem> subMenuItems)
        {
            ActionMenuItem item = new ActionMenuItem(name, MenuType.Parent);
            item.SubMenuItems = new List<ActionMenuItem>(subMenuItems);
            return item;
        }

        public static ActionMenuItem CreateSeperator(string name, List<ActionMenuItem> subMenuItems)
        {
            ActionMenuItem item = new ActionMenuItem("", MenuType.Seperator);
            return item;
        }

        public static ActionMenuItem CreatetMenuItem(WDAction action)
        {
            ActionMenuItem item = new ActionMenuItem(action.Name, MenuType.Item);
            item.MenuIcon = action.Icon;
            return item;
        }

        public static ActionMenuItem CreateCheckedMenu(string name, bool isChecked)
        {
            ActionMenuItem item = new ActionMenuItem(name, MenuType.Checked);
            item.Checked = isChecked;
            return item;
        }

        public static ActionMenuItem CreateCheckedMenu(string name, bool isChecked, WDAction action)
        {
            ActionMenuItem item = new ActionMenuItem(name, MenuType.Checked);
            item.Checked = isChecked;
            item.Action = action;
            return item;
        }

        MenuItem AsMenuItem()
        {
            MenuItem item = new MenuItem(Name);
            item.Enabled = Enabled;
            switch (Type)
            {
                case MenuType.Item:
                    if (Action != null)
                    {
                        item.Click += delegate(object sender, EventArgs e) { Action.DoAction(sender, e); };
                        //no icon possible?
                    }
                    break;

                case MenuType.Parent:
                    foreach(ActionMenuItem child in SubMenuItems)
                    {
                        item.MenuItems.Add(child.AsMenuItem());
                    }
                    break;
                case MenuType.Seperator:
                    //?
                    item.Text = "-----------";
                    break;
                case MenuType.Checked:
                    item.Checked = Checked;
                    break;
            }

            return item;
        }

        ToolStripItem AsToolStripMenuItem()
        {
            ToolStripMenuItem item = new ToolStripMenuItem(Name);
            item.Enabled = Enabled;
            switch (Type)
            {
                case MenuType.Item:
                    if (Action != null)
                    {
                        item.Click += delegate(object sender, EventArgs e) { Action.DoAction(sender, e); };
                        Icon icn = MenuIcon ?? Action.Icon;
                        if(icn != null) {
                            item.Image = icn.ToBitmap();
                        }
                        item.ToolTipText = Action.Description;
                    }
                    break;

                case MenuType.Parent:
                    foreach (ActionMenuItem child in SubMenuItems)
                    {
                        item.DropDownItems.Add(child.AsToolStripMenuItem());
                    }
                    break;
                case MenuType.Seperator:
                    return new ToolStripSeparator();  //------------------------EARLY RETURN HACK!!!!!!!!!!!!!!!!!!!!!!
                    //break;
                case MenuType.Checked:
                    item.Checked = Checked;
                    item.CheckOnClick = true;
                    if (Action != null)
                    {
                        item.Click += delegate(object sender, EventArgs e) { Action.DoAction(sender, e); };
                        item.ToolTipText = Action.Description;
                    }
                    break;
            }

            return item;
        }
    }

    
}
