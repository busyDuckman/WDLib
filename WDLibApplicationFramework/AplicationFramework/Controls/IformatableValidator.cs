/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace WD_toolbox.AplicationFramework.Controls
{
	/// <summary>
	/// Summary description for IformatableValidator.
	/// </summary>
	public class IformatableValidator : System.Windows.Forms.UserControl
	{
		protected bool sleep=false;
		
		[CategoryAttribute("Data"), 
		DescriptionAttribute("The duration to be specified")]
		private string _Value;
		public string Value
			{
			get
				{
				return _Value;
				}
			set
				{
				sleep=true;
				_Value = value;
				try
					{
					txtFormat.Text = Value;
					if(formatObject != null)
						{
						try
							{
							lblExample.Text = formatObject.ToString(Value, null);
							lblExample.ForeColor = Color.Black;
							}
						catch
							{
							lblExample.Text = "Invalid format";
							lblExample.ForeColor = Color.Red;
							}
						
						if(ValueChanged != null)
							this.ValueChanged(this, EventArgs.Empty);
						}
					}
				catch
					{
					}
				sleep=false;
				}
			}
		
		private IFormattable _formatObject;
		public IFormattable formatObject 
			{
			get
				{
				return _formatObject;
				}
			set
				{
				_formatObject = value;
				
				//reset the controls example dialog
				updateData();
				}
			}
		
		[CategoryAttribute("Events"), 
		DescriptionAttribute("Occurs when the value is changed")]
		public event EventHandler ValueChanged;
	
		private System.Windows.Forms.Label lblExample;
		private System.Windows.Forms.TextBox txtFormat;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public IformatableValidator()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblExample = new System.Windows.Forms.Label();
			this.txtFormat = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// lblExample
			// 
			this.lblExample.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lblExample.Location = new System.Drawing.Point(0, 24);
			this.lblExample.Name = "lblExample";
			this.lblExample.Size = new System.Drawing.Size(168, 24);
			this.lblExample.TabIndex = 5;
			this.lblExample.Text = "Example:";
			// 
			// txtFormat
			// 
			this.txtFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFormat.Location = new System.Drawing.Point(0, 0);
			this.txtFormat.Name = "txtFormat";
			this.txtFormat.Size = new System.Drawing.Size(168, 20);
			this.txtFormat.TabIndex = 4;
			this.txtFormat.Text = "hhh mmm sss";
			this.txtFormat.TextChanged += new System.EventHandler(this.txtFormat_TextChanged);
			// 
			// IformatableValidator
			// 
			this.Controls.Add(this.lblExample);
			this.Controls.Add(this.txtFormat);
			this.Name = "IformatableValidator";
			this.Size = new System.Drawing.Size(168, 48);
			this.Load += new System.EventHandler(this.IformatableValidator_Load);
			this.ResumeLayout(false);

		}
		#endregion
		
		public void updateData()
			{
			if(!sleep)
				{
				try
					{
					Value = txtFormat.Text;
					}
				catch
					{
					}
				}
			}
		
		private void txtFormat_TextChanged(object sender, System.EventArgs e)
			{
			updateData();
			}

		private void IformatableValidator_Load(object sender, System.EventArgs e)
			{
			updateData();
			}
		
		
	}
}
