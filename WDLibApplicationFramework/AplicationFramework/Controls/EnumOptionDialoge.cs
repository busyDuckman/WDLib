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
using WD_toolbox;

namespace WDLibApplicationFramework.AplicationFramework.Controls
{
    public partial class EnumOptionDialoge : Form
    {
        public string[] Options { get; set; }
        public int SelectedIndex { get; set; }
        public string SelectedOption { get { return Options.NthOrDefault(SelectedIndex); } }
        public string Question { get; set; }

        public EnumOptionDialoge()
        {
            InitializeComponent();
            Options = new string[0];
            SelectedIndex = 0;
        }

        public static object ShowDialog(IWin32Window owner, Type type, string question, string title)
        {
            EnumOptionDialoge dlg = new EnumOptionDialoge();
            dlg.Options = Enum.GetNames(type);
            dlg.Question = question;
            dlg.Text = title;
            DialogResult dr = dlg.ShowDialog(owner);
            if (dr == DialogResult.OK)
            {
                if(dlg.SelectedOption != null)
                {
                    return Enum.Parse(type, dlg.SelectedOption);
                }
            }

            return null;
        }

        public static int ShowDialog(IWin32Window owner, string[] options, string question, string title)
        {
            EnumOptionDialoge dlg = new EnumOptionDialoge();
            dlg.Options = options;
            dlg.Question = question;
            dlg.Text = title;
            DialogResult dr = dlg.ShowDialog(owner);
            if (dr == DialogResult.OK)
            {
                if (options.IsIndexValid(dlg.SelectedIndex))
                {
                    return dlg.SelectedIndex;
                }
            }

            return -1;
        }

        private void EnumOptionDialoge_Load(object sender, EventArgs e)
        {
            cmbOptions.Items.AddRange(Options);
            cmbOptions.SelectedIndex = SelectedIndex;
            txtQuestion.Text = Question;
        }

        private void cmbOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndex = cmbOptions.SelectedIndex;
        }
    }
}
