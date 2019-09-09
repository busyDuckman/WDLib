/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WD_toolbox.AplicationFramework;

namespace WDLibApplicationFramework.AplicationFramework.Menus
{
    public enum MenuItemTypeEnum { Normal, TopLevelMenu, Control, Seperator, Other}


    /// <summary>
    /// Refering to the visable aperance and behaviour of a menu item with no
    /// regard to its heiracy in a larger menu system.
    /// </summary>
    public interface IMenuProperties
    {
        MenuItemTypeEnum Type { get; }

        string Text { get; set; }
        string ToolTip { get; set; }
        string Name { get; }
        object Tag { get; set; }

        CheckState CheckState { get; set; }
        bool Checked { get; set; }
        bool SupportsChecked { get; }
        bool SupportsCheckState { get; }

        bool Enabled { get; set; }
        Image Image { get; set; }

        event EventHandler OnClick;// { add; remove; }
    }

    /// <summary>
    /// .NET has too many menu item types, and they dont implement some sensible interface.
    /// This class works around that, to give consisten acess to multiple menu types
    /// </summary>
    public interface IMenuItemWrapper : IMenuProperties, INativeWrapper
    {
        bool HasSubMenus { get; }
        
        IReadOnlyList<IMenuItemWrapper> SubMenus { get; }

        void AddSubMenu(MenuItemProperties item);
        void RemoveSubMenu(int index);

        bool IsTopMost { get; }
        bool IsImplemented { get; }                

        IMenuItemWrapper ParentMenu { get; }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
    // MenuExtensions
    //-------------------------------------------------------------------------------------------------------------------------------
    public static class MenuExtensions
    {
        //----------------------------------------------------------------------------------------------------------
        // IMenuItemWrapper Extensions
        //----------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------
        // Helpers
        //----------------------------------------------------------------------------------------------------------
        public static IMenuItemWrapper GetWrapper(object menuItem)
        {
            if (menuItem == null)
            {
                return null;
            }
            else if (menuItem is MenuStrip)
            {
                return GetMenuItemWrapper((MenuStrip)menuItem);
            }
            else if (menuItem is ToolStripItem)
            {
                return GetMenuItemWrapper((ToolStripItem)menuItem);
            }
            else
            {
                WDAppLog.LogNeverSupposedToBeHere();
                return null;
            }            
        }

        //----------------------------------------------------------------------------------------------------------
        // Winforms extensions
        //----------------------------------------------------------------------------------------------------------
        public static IMenuItemWrapper GetMenuItemWrapper(this MenuStrip menuStrip)
        {
            return new MenuItemWrapper_MenuStrip(menuStrip);
        }

        public static IMenuItemWrapper GetMenuItemWrapper(this ToolStripItem menuItem)
        {
            if (menuItem is ToolStripMenuItem)
            {
                return new MenuItemWrapper_ToolStripMenuItem((ToolStripMenuItem)menuItem);
            }
            else if (menuItem is ToolStripSeparator)
            {
                return new MenuItemWrapper_ToolStripMisc(menuItem, MenuItemTypeEnum.Seperator);
            }
            else if (menuItem is ToolStripDropDownItem)
            {
                return new MenuItemWrapper_ToolStripMisc(menuItem, MenuItemTypeEnum.Normal);
            }
            else
            {
                return new MenuItemWrapper_ToolStripMisc(menuItem, MenuItemTypeEnum.Control);
            }
        }

        public static IMenuItemWrapper GetMenuItemWrapper(this ToolStrip menuItem)
        {
            if (menuItem is ToolStripDropDownMenu)
            {
                return new MenuItemWrapper_ToolStripDropDownMenu((ToolStripDropDownMenu)menuItem);
            }
            else
            {
                return null;
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------
    // MenuItemProperties
    //-------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Used for adding a new sub menu to a manu item.
    /// This stub can be "exported" to a menu item.
    /// </summary>
    public class MenuItemProperties : IMenuProperties
    {
        public MenuItemTypeEnum Type { get; set; }

        public string Text { get; set; }
        public string ToolTip { get; set; }
        public string Name { get; set; }
        public object Tag { get; set; }

        public CheckState CheckState { get; set; }
        public bool Checked { get; set; }
        public bool SupportsChecked { get; set; }
        public bool SupportsCheckState { get; set; }

        public bool Enabled { get; set; }
        public Image Image { get; set; }

        public event EventHandler OnClick;

        public ToolStripItem AsToolStripItem()
        {
            ToolStripMenuItem mnu = new ToolStripMenuItem(Text??"");
            if (ToolTip != null) { mnu.ToolTipText = ToolTip; }
            if (Name != null) { mnu.Name = Name; }
            if (Tag != null) { mnu.Tag = Tag; }
            if (Image != null) { mnu.Image = Image; }
            if (OnClick != null) { mnu.Click += OnClick; }

            mnu.Checked = Checked;
            mnu.CheckState = CheckState;
            mnu.Enabled = Enabled;

            if (SupportsChecked)
            {
                mnu.CheckOnClick = true;
            }

            return mnu;
        }

    }

    //-------------------------------------------------------------------------------------------------------------------------------
    // MenuItemWrapper_ToolStripItemBase
    //-------------------------------------------------------------------------------------------------------------------------------
    public abstract class MenuItemWrapperBase<T> : NativeWrapperBase<T>, IMenuItemWrapper
        where T : class
    {
        public MenuItemWrapperBase(T menuItem)
            : base(menuItem)
        {
            Type = MenuItemTypeEnum.Normal;
        }

        public virtual MenuItemTypeEnum Type { get; protected set; }

        public abstract string ToolTip { get; set; }

        public virtual CheckState CheckState
        {
            get { return CheckState.Unchecked; }
            set { }
        }
        public virtual bool Checked
        {
            get { return false; }
            set { }
        }

        public virtual bool SupportsChecked { get { return false; } }
        public virtual bool SupportsCheckState { get { return false; } }

        public virtual Image Image
        {
            get { return null; }
            set { }
        }

        public virtual bool IsImplemented { get { return HasEventHandler(nativeObject, "Click") || HasEventHandler(nativeObject, "click"); } }

        public abstract string Text { get; set; }
        public abstract string Name { get; }

        public abstract object Tag { get; set; }



        //because I can't make this abstract
        public virtual event EventHandler OnClick { add { throw new NotImplementedException(); } remove { throw new NotImplementedException(); } }

        public abstract bool Enabled { get; set; }


        public virtual bool HasSubMenus { get { return SubMenus.Count != 0; } }
        public abstract IReadOnlyList<IMenuItemWrapper> SubMenus { get; }
        public abstract void AddSubMenu(MenuItemProperties item);
        public abstract void RemoveSubMenu(int index);

        public virtual bool IsTopMost { get { return ParentMenu != null; } }
        public abstract IMenuItemWrapper ParentMenu { get; }
        

        /// <summary>
        /// Code found floating on forums... (http://stackoverflow.com/questions/3212721/check-if-the-control-has-events-on-click-eventhandler)
        /// </summary>
        protected static bool HasEventHandler(object control, string eventName = "EventClick")
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

    public abstract class MenuItemWrapperBaseControl<T> : MenuItemWrapperBase<T>, IMenuItemWrapper
        where T : Control
    {
        public MenuItemWrapperBaseControl(T menuItem)
            : base(menuItem)
        {
            Type = MenuItemTypeEnum.Normal;
        }

        public override string Text
        {
            get { return nativeObject.Text; }
            set { nativeObject.Text = value; }
        }

        public override string ToolTip
        {
            get
            {
                using (System.Windows.Forms.ToolTip ts = new ToolTip())
                {
                    return ts.GetToolTip(nativeObject);
                }
            }
            set
            {
                using (System.Windows.Forms.ToolTip ts = new ToolTip())
                {
                    ts.SetToolTip(nativeObject, value);
                }
            }
        }

        public override string Name { get { return nativeObject.Name; } }

        public override object Tag
        {
            get { return nativeObject.Tag; }
            set { nativeObject.Tag = value; }
        }

        public override event EventHandler OnClick
        {
            add { nativeObject.Click += value; }
            remove { nativeObject.Click -= value; }
        }

        public override bool Enabled
        {
            get { return nativeObject.Enabled; }
            set { nativeObject.Enabled = value; }
        }

    }



    //-------------------------------------------------------------------------------------------------------------------------------
    // MenuItemWrapper_MenuStrip
    //-------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// .NET has too many menu item types, and they dont implement some sensible interface.
    /// This class works around that, to give consisten acess to multiple menu types
    /// </summary>
    public class MenuItemWrapper_MenuStrip : MenuItemWrapperBaseControl<MenuStrip>, IMenuItemWrapper
    {
        public MenuItemWrapper_MenuStrip(MenuStrip menuStrip) : base(menuStrip)
        {
        }

        public static IMenuItemWrapper IMenuItemWrapper(MenuStrip menuStrip)
        {
            return menuStrip.GetMenuItemWrapper();
        }

        public override bool IsTopMost { get { return true; } }
        public override MenuItemTypeEnum Type { get { return MenuItemTypeEnum.TopLevelMenu; } protected set { } }

        public override bool HasSubMenus { get { return nativeObject.Items.Count > 0; } }

        public override IReadOnlyList<IMenuItemWrapper> SubMenus
        {
            get 
            { 
                var allItems = from ToolStripItem item in nativeObject.Items select MenuExtensions.GetWrapper(item);
                var validItems = allItems.Where(M => M != null);

                return validItems.ToList().AsReadOnly();
            }
        }

        public override void AddSubMenu(MenuItemProperties item)
        {
            nativeObject.Items.Add(item.AsToolStripItem());
        }

        public override void RemoveSubMenu(int index)
        {
            nativeObject.Items.RemoveAt(index);
        }


        public override IMenuItemWrapper ParentMenu { get { return null; } }
    }


    //-------------------------------------------------------------------------------------------------------------------------------
    // MenuItemWrapper_ToolStripItemBase
    //-------------------------------------------------------------------------------------------------------------------------------
    public abstract class MenuItemWrapper_ToolStripItemBase<T> : MenuItemWrapperBase<T>, IMenuItemWrapper
        where T : ToolStripItem
    {
        public MenuItemWrapper_ToolStripItemBase(T menuItem) : base(menuItem)
        {
        }

        public override string Text
        {
            get { return nativeObject.Text; }
            set { nativeObject.Text = value; }
        }

        public override bool IsTopMost { get { return ParentMenu != null; } }

        public override string ToolTip
        {
            get { return nativeObject.ToolTipText; }
            set { nativeObject.ToolTipText = value; }
        }

        public override string Name { get { return nativeObject.Name; } }

        public override object Tag
        {
            get { return nativeObject.Tag; }
            set { nativeObject.Tag = value; }
        }

        public override Image Image
        {
            get { return nativeObject.Image; }
            set { nativeObject.Image = value; }
        }

        public override bool IsImplemented { get { return HasEventHandler(nativeObject, "Click") || HasEventHandler(nativeObject, "click"); } }
        public override event EventHandler OnClick
        {
            add { nativeObject.Click += value; }
            remove { nativeObject.Click -= value; }
        }

        public override bool Enabled
        {
            get { return nativeObject.Enabled; }
            set { nativeObject.Enabled = value; }
        }

        public override IMenuItemWrapper ParentMenu
        {
            get
            {
                ToolStrip toolStrip = nativeObject.GetCurrentParent();
                return toolStrip.GetMenuItemWrapper();
            }
        }
    }


    //-------------------------------------------------------------------------------------------------------------------------------
    // MenuItemWrapper_ToolStripMenuItem
    //-------------------------------------------------------------------------------------------------------------------------------
    public class MenuItemWrapper_ToolStripMenuItem : MenuItemWrapper_ToolStripItemBase<ToolStripMenuItem>, IMenuItemWrapper
    {
        public MenuItemWrapper_ToolStripMenuItem(ToolStripMenuItem menuItem) : base(menuItem)
        {
            Type = MenuItemTypeEnum.Normal;
        }

        public override bool Checked
        {
            get
            {
                return nativeObject.Checked;
            }
            set
            {
                nativeObject.Checked = value;
            }
        }

        public override CheckState CheckState
        {
            get
            {
                return nativeObject.CheckState;
            }
            set
            {
                nativeObject.CheckState = value;
            }
        }

        public override bool SupportsChecked { get { return true; } }
        public override bool SupportsCheckState { get { return true; } }

        public override bool IsImplemented
        {
            get
            {
                var eventList = typeof(ToolStripMenuItem).GetEvents();
                var click = eventList.FirstOrDefault(E => E.Name.Equals("click", StringComparison.OrdinalIgnoreCase));
                if(click != null)
                {
                    return HasEventHandler(nativeObject, click.Name);
                }
                return false;
            }
        }

        public override bool HasSubMenus { get { return nativeObject.DropDownItems.Count > 0; } }

        public override IReadOnlyList<IMenuItemWrapper> SubMenus
        {
            get 
            { 
                var allItems = from ToolStripItem item in nativeObject.DropDownItems select MenuExtensions.GetWrapper(item);
                var validItems = allItems.Where(M => M != null);

                return validItems.ToList().AsReadOnly();
            }
        }

        public override void AddSubMenu(MenuItemProperties item)
        {
            nativeObject.DropDownItems.Add(item.AsToolStripItem());
        }

        public override void RemoveSubMenu(int index)
        {
            nativeObject.DropDownItems.RemoveAt(index);
        }

       
    }

    //-------------------------------------------------------------------------------------------------------------------------------
    // MenuItemWrapper_ToolStripMisc
    //-------------------------------------------------------------------------------------------------------------------------------
    public class MenuItemWrapper_ToolStripMisc : MenuItemWrapper_ToolStripItemBase<ToolStripItem>, IMenuItemWrapper
    {
        public MenuItemWrapper_ToolStripMisc(ToolStripItem menuItem, MenuItemTypeEnum type) : base(menuItem)
        {
            Type = type;
        }

        public static IMenuItemWrapper IMenuItemWrapper(ToolStripItem menuStrip)
        {
            return menuStrip.GetMenuItemWrapper();
        }

        public static IMenuItemWrapper IMenuItemWrapper(ToolStripDropDownItem menuStrip)
        {
            return menuStrip.GetMenuItemWrapper();
        }
        

        //public override bool IsImplemented { get { return true; } } //For all intents and purposes.

        public override bool HasSubMenus { get { return false; } }

        public override IReadOnlyList<IMenuItemWrapper> SubMenus { get { return new List<IMenuItemWrapper>().AsReadOnly(); } }

        public override void AddSubMenu(MenuItemProperties item) { }
        public override void RemoveSubMenu(int index) { }
    }


    //-------------------------------------------------------------------------------------------------------------------------------
    // MenuItemWrapper_ToolStripBase
    //-------------------------------------------------------------------------------------------------------------------------------
    public abstract class MenuItemWrapper_ToolStripBase<T> : MenuItemWrapperBaseControl<T>, IMenuItemWrapper
        where T : ToolStrip
    {
        public MenuItemWrapper_ToolStripBase(T menuItem)
            : base(menuItem)
        {
            Type = MenuItemTypeEnum.Normal;
        }

        public override bool HasSubMenus
        {
            get { return nativeObject.Items.Count > 0; }
        }

        public override IReadOnlyList<IMenuItemWrapper> SubMenus
        {
            get
            {
                var allItems = from ToolStripItem item in nativeObject.Items select MenuExtensions.GetWrapper(item);
                var validItems = allItems.Where(M => M != null);

                return validItems.ToList().AsReadOnly();
            }
        }

        public override Image Image
        {
            get
            {
                return nativeObject.BackgroundImage;
            }
            set
            {
                nativeObject.BackgroundImage = value;
            }
        }

    }

    //-------------------------------------------------------------------------------------------------------------------------------
    // MenuItemWrapper_ToolStripDropDownMenu
    //-------------------------------------------------------------------------------------------------------------------------------
    public class MenuItemWrapper_ToolStripDropDownMenu : MenuItemWrapper_ToolStripBase<ToolStripDropDownMenu>, IMenuItemWrapper
    {
        public MenuItemWrapper_ToolStripDropDownMenu(ToolStripDropDownMenu menuItem) : base(menuItem)
        {
            Type = MenuItemTypeEnum.Normal;
        }

        public static explicit operator MenuItemWrapper_ToolStripDropDownMenu(ToolStripDropDownMenu menuStrip)
        {
            return new MenuItemWrapper_ToolStripDropDownMenu(menuStrip);
        }

        public static IMenuItemWrapper IMenuItemWrapper(ToolStripDropDownMenu menuStrip)
        {
            return menuStrip.GetMenuItemWrapper();
        }

        public static IMenuItemWrapper IMenuItemWrapper(MenuItemWrapper_ToolStripDropDownMenu menuStrip)
        {
            return menuStrip;
        }

        //TODO fix 
        public override bool IsTopMost { get { return ParentMenu != null; } }


        public override IMenuItemWrapper ParentMenu
        {
            get { return MenuExtensions.GetWrapper(nativeObject.Parent); }
        }

        public override void AddSubMenu(MenuItemProperties item) { }
        public override void RemoveSubMenu(int index) { }
    }

}
