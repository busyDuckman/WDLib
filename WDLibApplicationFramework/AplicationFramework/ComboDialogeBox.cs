/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WDLibApplicationFramework.AplicationFramework
{
    public partial class ComboDialogeBox : Form
    {
        public String Title { get { return this.Text; } set { this.Text = value; } }
        public String Prompt { get { return lblPrompt.Text; } set { lblPrompt.Text = value; } }
        public object[] Options 
        { 
            get 
            {
                object[] objs = new object[cmbOptions.Items.Count];
                cmbOptions.Items.CopyTo(objs, 0);
                return objs;
            } 
            set 
            {
                cmbOptions.Items.Clear();
                cmbOptions.Items.AddRange(value);
            } 
        }
        public object SelectedOption {
            get { return cmbOptions.SelectedItem; } 
            set { cmbOptions.SelectedItem = value; } 
        }

        public ComboDialogeBox()
        {
            InitializeComponent();
        }

        public ComboDialogeBox(Enum values)
        {
            InitializeComponent();
        }

        private void ComboDialogeBox_Load(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {

        }

        public void OptionsFromEnum(Type type, int def)
        {
            string selectedName = "";
            List<object> list = new List<object>();
            foreach(string name in Enum.GetNames(type))
            {
                list.Add(name);
                if ((int)Enum.Parse(type, name) == def)
                {
                    selectedName = name;
                }
            }

            this.Options = list.ToArray();
            this.SelectedOption = selectedName;
        }

        public int SelectedEnumValue<TYPE>()
        {
            if (this.SelectedOption == null)
            {
                return -1;
            }

            return (int)Enum.Parse(typeof(TYPE), this.SelectedOption.ToString(), false);
        }
    }
}
