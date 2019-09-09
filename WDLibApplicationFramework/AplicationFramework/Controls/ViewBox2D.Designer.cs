/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
namespace WDLibApplicationFramework.AplicationFramework.Controls
{
    partial class ViewBox2D
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewBox2D));
            this.panel2 = new System.Windows.Forms.Panel();
            this.picMain = new System.Windows.Forms.PictureBox();
            this.scrlVert = new System.Windows.Forms.VScrollBar();
            this.scrlHoriz = new System.Windows.Forms.HScrollBar();
            this.tbView = new System.Windows.Forms.ToolStrip();
            this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.btnZoomFit = new System.Windows.Forms.ToolStripButton();
            this.btnZoomActual = new System.Windows.Forms.ToolStripButton();
            this.lblZoom = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRotateLeft = new System.Windows.Forms.ToolStripButton();
            this.btnRotateRight = new System.Windows.Forms.ToolStripButton();
            this.lblRotate = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lblMousePos = new System.Windows.Forms.ToolStripLabel();
            this.timRedraw = new System.Windows.Forms.Timer(this.components);
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
            this.tbView.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.picMain);
            this.panel2.Controls.Add(this.scrlVert);
            this.panel2.Controls.Add(this.scrlHoriz);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(492, 271);
            this.panel2.TabIndex = 2;
            // 
            // picMain
            // 
            this.picMain.BackColor = System.Drawing.Color.Black;
            this.picMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picMain.Location = new System.Drawing.Point(0, 0);
            this.picMain.Margin = new System.Windows.Forms.Padding(0);
            this.picMain.Name = "picMain";
            this.picMain.Size = new System.Drawing.Size(472, 251);
            this.picMain.TabIndex = 0;
            this.picMain.TabStop = false;
            this.picMain.DragDrop += new System.Windows.Forms.DragEventHandler(this.picMain_DragDrop);
            this.picMain.DragEnter += new System.Windows.Forms.DragEventHandler(this.picMain_DragEnter);
            this.picMain.Paint += new System.Windows.Forms.PaintEventHandler(this.picMain_Paint);
            this.picMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picMain_MouseDown);
            this.picMain.MouseEnter += new System.EventHandler(this.picMain_MouseEnter);
            this.picMain.MouseLeave += new System.EventHandler(this.picMain_MouseLeave);
            this.picMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picMain_MouseMove);
            this.picMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picMain_MouseUp);
            this.picMain.Resize += new System.EventHandler(this.picMain_Resize);
            // 
            // scrlVert
            // 
            this.scrlVert.Dock = System.Windows.Forms.DockStyle.Right;
            this.scrlVert.Location = new System.Drawing.Point(472, 0);
            this.scrlVert.Name = "scrlVert";
            this.scrlVert.Size = new System.Drawing.Size(20, 251);
            this.scrlVert.TabIndex = 1;
            this.scrlVert.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrlVert_Scroll);
            // 
            // scrlHoriz
            // 
            this.scrlHoriz.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.scrlHoriz.Location = new System.Drawing.Point(0, 251);
            this.scrlHoriz.Name = "scrlHoriz";
            this.scrlHoriz.Size = new System.Drawing.Size(492, 20);
            this.scrlHoriz.TabIndex = 2;
            this.scrlHoriz.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrlHoriz_Scroll);
            // 
            // tbView
            // 
            this.tbView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZoomIn,
            this.btnZoomOut,
            this.btnZoomFit,
            this.btnZoomActual,
            this.lblZoom,
            this.toolStripSeparator1,
            this.btnRotateLeft,
            this.btnRotateRight,
            this.lblRotate,
            this.toolStripSeparator2,
            this.lblMousePos});
            this.tbView.Location = new System.Drawing.Point(0, 0);
            this.tbView.Name = "tbView";
            this.tbView.Size = new System.Drawing.Size(492, 25);
            this.tbView.TabIndex = 3;
            this.tbView.Text = "toolStrip1";
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.Image")));
            this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(23, 22);
            this.btnZoomIn.Text = "toolStripButton1";
            this.btnZoomIn.ToolTipText = "Zoom In";
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
            this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(23, 22);
            this.btnZoomOut.Text = "Zoom Out";
            this.btnZoomOut.ToolTipText = "Zoom Out";
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomFit
            // 
            this.btnZoomFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomFit.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomFit.Image")));
            this.btnZoomFit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomFit.Name = "btnZoomFit";
            this.btnZoomFit.Size = new System.Drawing.Size(23, 22);
            this.btnZoomFit.Text = "Zoom All";
            this.btnZoomFit.ToolTipText = "Zoom Fit";
            this.btnZoomFit.Click += new System.EventHandler(this.btnZoomFit_Click);
            // 
            // btnZoomActual
            // 
            this.btnZoomActual.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomActual.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomActual.Image")));
            this.btnZoomActual.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomActual.Name = "btnZoomActual";
            this.btnZoomActual.Size = new System.Drawing.Size(23, 22);
            this.btnZoomActual.Text = "Zoom 100%";
            this.btnZoomActual.ToolTipText = "Zoom 100%";
            this.btnZoomActual.Click += new System.EventHandler(this.btnZoomActual_Click);
            // 
            // lblZoom
            // 
            this.lblZoom.AutoSize = false;
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new System.Drawing.Size(40, 22);
            this.lblZoom.Text = "100%";
            this.lblZoom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRotateLeft
            // 
            this.btnRotateLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRotateLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateLeft.Image")));
            this.btnRotateLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRotateLeft.Name = "btnRotateLeft";
            this.btnRotateLeft.Size = new System.Drawing.Size(23, 22);
            this.btnRotateLeft.Text = "Rotate Left";
            this.btnRotateLeft.Click += new System.EventHandler(this.btnRotateLeft_Click);
            // 
            // btnRotateRight
            // 
            this.btnRotateRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRotateRight.Image = ((System.Drawing.Image)(resources.GetObject("btnRotateRight.Image")));
            this.btnRotateRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRotateRight.Name = "btnRotateRight";
            this.btnRotateRight.Size = new System.Drawing.Size(23, 22);
            this.btnRotateRight.Text = "Rotate Right";
            this.btnRotateRight.Click += new System.EventHandler(this.btnRotateRight_Click);
            // 
            // lblRotate
            // 
            this.lblRotate.AutoSize = false;
            this.lblRotate.Name = "lblRotate";
            this.lblRotate.Size = new System.Drawing.Size(40, 22);
            this.lblRotate.Text = " ";
            this.lblRotate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // lblMousePos
            // 
            this.lblMousePos.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblMousePos.Name = "lblMousePos";
            this.lblMousePos.Size = new System.Drawing.Size(25, 22);
            this.lblMousePos.Text = "0, 0";
            this.lblMousePos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timRedraw
            // 
            this.timRedraw.Enabled = true;
            this.timRedraw.Interval = 18;
            this.timRedraw.Tick += new System.EventHandler(this.timRedraw_Tick);
            // 
            // ViewBox2D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tbView);
            this.Name = "ViewBox2D";
            this.Size = new System.Drawing.Size(492, 296);
            this.Load += new System.EventHandler(this.ViewBox2D_Load);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
            this.tbView.ResumeLayout(false);
            this.tbView.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picMain;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStrip tbView;
        private System.Windows.Forms.HScrollBar scrlHoriz;
        private System.Windows.Forms.VScrollBar scrlVert;
        private System.Windows.Forms.ToolStripButton btnZoomIn;
        private System.Windows.Forms.ToolStripButton btnZoomOut;
        private System.Windows.Forms.ToolStripButton btnZoomFit;
        private System.Windows.Forms.ToolStripButton btnZoomActual;
        private System.Windows.Forms.ToolStripLabel lblZoom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnRotateLeft;
        private System.Windows.Forms.ToolStripButton btnRotateRight;
        private System.Windows.Forms.ToolStripLabel lblRotate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel lblMousePos;
        private System.Windows.Forms.Timer timRedraw;
    }
}
