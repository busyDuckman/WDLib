/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using WD_toolbox;
using WD_toolbox.AplicationFramework;
using WD_toolbox.Data;

namespace WDLibApplicationFramework.AplicationFramework.Controls
{
    public partial class ComboPropertyGrid : UserControl
    {
        private bool internalUpdate = false;

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public ObservableCollection<object> Items { get; protected set; }

        public object SelectedItem 
        {
            get
            {
                return Items.NthOrDefault(selectedIndex);
            } 
            set
            {
                if (value == null)
                {
                    selectedIndex = -1;
                }
                else if(Items.Contains(value))
                {
                    SelectedIndex = Items.IndexOf(value);
                }
            }
        }

        private int selectedIndex;
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        protected int SelectedIndex
        {
            get { return selectedIndex; }
            set 
            { 
                selectedIndex = value;
                try
                {
                    internalUpdate = true;
                    cmbMain.SelectedIndex = selectedIndex;
                    RebindPropertyGrid();
                }
                finally
                {
                    internalUpdate = false;
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        protected bool SelectedIndexValid { get { return (SelectedIndex >= 0) && (SelectedIndex < Items.Count); } }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public Func<object, string> GetName { get; set; }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public bool HelpVisible
        {
            get { return pgMain.HelpVisible; }
            set { pgMain.HelpVisible = value; }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public bool ToolbarVisible
        {
            get { return pgMain.ToolbarVisible; }
            set { pgMain.ToolbarVisible = value; }
        }

        public ComboPropertyGrid()
        {
            InitializeComponent();

            Items = new ObservableCollection<object>();
            Items.CollectionChanged += (sender, e) => { rebuild(); };
            cmbMain.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        protected void rebuild()
        {
            cmbMain.Items.Clear();
            foreach (object item in Items)
            {
                Tag<object> t = new Tag<object>(item, itemToString(item));
                cmbMain.Items.Add(t);
            }

            RebindPropertyGrid();
        }

        public void RebindPropertyGrid()
        {
            if (!SelectedIndexValid)
            {
                selectedIndex = -1;
            }

            try
            {
                pgMain.SelectedObject = SelectedItem;
            }
            catch (Exception ex)
            {
                WDAppLog.logException(ErrorLevel.Error, ex);
            }
        }

        protected string itemToString(object item)
        {
            string name = "?";
            try
            {
                if (GetName != null)
                {
                    name = GetName(item);
                    if (name.IsSomething())
                    {
                        return name.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                WDAppLog.logException(ErrorLevel.Error, ex);
            }

            try
            {
                name = (item ?? "NULL").ToString();
            }
            catch (Exception ex)
            {
                WDAppLog.logException(ErrorLevel.Error, ex);
            }

            return name;
            
        }

        private void ComboPropertyGrid_Load(object sender, EventArgs e)
        {
            
        }

        private void cmbMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!internalUpdate)
            {
                SelectedIndex = cmbMain.SelectedIndex;
            }
        }

        
    }
}
