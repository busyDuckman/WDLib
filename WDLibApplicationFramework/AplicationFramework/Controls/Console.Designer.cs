/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
namespace WDLibApplicationFramework.AplicationFramework.Controls
{
    partial class Console
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
            this.vbMain = new WDLibApplicationFramework.AplicationFramework.Controls.ViewBox2D();
            this.SuspendLayout();
            // 
            // vbMain
            // 
            this.vbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vbMain.Location = new System.Drawing.Point(0, 0);
            this.vbMain.Name = "vbMain";
            this.vbMain.ShowToolBar = false;
            this.vbMain.Size = new System.Drawing.Size(405, 307);
            this.vbMain.TabIndex = 0;
            this.vbMain.View = null;
            // 
            // Console
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vbMain);
            this.Name = "Console";
            this.Size = new System.Drawing.Size(405, 307);
            this.Load += new System.EventHandler(this.Console_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ViewBox2D vbMain;
    }
}
