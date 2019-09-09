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
using WD_toolbox.Rendering;

namespace WDLibApplicationFramework.AplicationFramework.Controls
{
    public partial class ReadoutLabel : UserControl
    {
        public ImageList Icons { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        
        public double ErrorLowValue { get; set; }
        public double WarningLowValue { get; set; }
        public double ErrorHighValue { get; set; }
        public double WarningHighValue { get; set; }

        public int OkIndex { get; set; }
        public int ErrorLowIndex { get; set; }
        public int WarningLowIndex { get; set; }
        public int ErrorHighIndex { get; set; }
        public int WarningHighIndex { get; set; }

        public double Value    { get; set; }

        public string FormatString { get; set; }

        public ReadoutLabel()
        {
            OkIndex = -1;
            ErrorLowIndex = -1;
            WarningLowIndex = -1;
            ErrorHighIndex = -1;
            WarningHighIndex = -1;
            FormatString = "{0}";

            InitializeComponent();
            
        }

        private void ReadoutLabel_Load(object sender, EventArgs e)
        {

        }

        private void ReadoutLabel_Paint(object sender, PaintEventArgs e)
        {
            int iconIndex = 0;
            if(Value <= ErrorLowValue)
            {
                iconIndex = ErrorLowIndex;
            }
            if(Value <= WarningLowValue)
            {
                iconIndex = WarningLowIndex;
            }
            if(Value < WarningHighValue)
            {
                iconIndex = OkIndex;
            }
            if(Value < ErrorHighValue)
            {
                iconIndex = WarningHighIndex;
            }
            else
            {
                iconIndex = ErrorHighIndex;
            }
            

            string output = string.Format(FormatString ?? "{0}", Value);
            GDIPlusRenderer g = new GDIPlusRenderer(e.Graphics);
            g.Clear(this.BackColor);
            if(Icons != null)
            {
                if((iconIndex >= 0) && (iconIndex < Icons.Images.Count))
                {
                    g.DrawImage(Icons.Images[iconIndex], 0, 0, this.Height, this.Height);
                }
            }

            g.DrawString(this.ForeColor, output, this.Font, this.Height + 3, 0);

            g.Flush();
        }
    }
}
