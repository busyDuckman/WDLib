﻿/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
namespace WDLibApplicationFramework.AplicationFramework.Controls
{
    partial class PropertyGridDialogeForRenderable
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.viewBox2D1 = new WDLibApplicationFramework.AplicationFramework.Controls.ViewBox2D();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1240, 952);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(153, 56);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1466, 944);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(151, 64);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(12, 28);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(528, 906);
            this.propertyGrid1.TabIndex = 2;
            // 
            // viewBox2D1
            // 
            this.viewBox2D1.Location = new System.Drawing.Point(565, 28);
            this.viewBox2D1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.viewBox2D1.MaxZoom = 10D;
            this.viewBox2D1.MinZoom = 0.01D;
            this.viewBox2D1.Name = "viewBox2D1";
            this.viewBox2D1.ShowToolBar = true;
            this.viewBox2D1.Size = new System.Drawing.Size(1066, 906);
            this.viewBox2D1.TabIndex = 3;
            this.viewBox2D1.View = null;
            // 
            // PropertyGridDialogeForRenderable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1643, 1040);
            this.Controls.Add(this.viewBox2D1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "PropertyGridDialogeForRenderable";
            this.Text = "PropertyGridDialogeForRenderable";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private ViewBox2D viewBox2D1;
    }
}