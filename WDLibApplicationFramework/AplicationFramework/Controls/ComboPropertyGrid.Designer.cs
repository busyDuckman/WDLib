/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
namespace WDLibApplicationFramework.AplicationFramework.Controls
{
    partial class ComboPropertyGrid
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbMain = new System.Windows.Forms.ComboBox();
            this.pgMain = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // cmbMain
            // 
            this.cmbMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbMain.FormattingEnabled = true;
            this.cmbMain.Location = new System.Drawing.Point(3, 3);
            this.cmbMain.Name = "cmbMain";
            this.cmbMain.Size = new System.Drawing.Size(254, 21);
            this.cmbMain.TabIndex = 0;
            this.cmbMain.SelectedIndexChanged += new System.EventHandler(this.cmbMain_SelectedIndexChanged);
            // 
            // pgMain
            // 
            this.pgMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgMain.Location = new System.Drawing.Point(0, 30);
            this.pgMain.Name = "pgMain";
            this.pgMain.Size = new System.Drawing.Size(257, 309);
            this.pgMain.TabIndex = 1;
            // 
            // ComboPropertyGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pgMain);
            this.Controls.Add(this.cmbMain);
            this.Name = "ComboPropertyGrid";
            this.Size = new System.Drawing.Size(257, 339);
            this.Load += new System.EventHandler(this.ComboPropertyGrid_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbMain;
        private System.Windows.Forms.PropertyGrid pgMain;
    }
}
