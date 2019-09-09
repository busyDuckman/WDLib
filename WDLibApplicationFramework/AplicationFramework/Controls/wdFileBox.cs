/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WD_toolbox.AplicationFramework.Controls
{
	public class wdFileBox : wdControlls.recentValueControll
	{
		private System.Windows.Forms.ComboBox cmbText;
		private System.Windows.Forms.Button btnBrowse;
		private System.ComponentModel.IContainer components = null;
		private volatile bool sleep = false;
		
		
		private FlatStyle _FlatStyle = FlatStyle.Standard;
		[CategoryAttribute("Apperance"), DescriptionAttribute("Controls how the controll appears")]
		public FlatStyle FlatStyle
			{
			get
				{
				return _FlatStyle;
				}
			set
				{
				_FlatStyle = value;
				btnBrowse.FlatStyle = FlatStyle;
				}
			}
		
		private bool _OpenMode;
		[CategoryAttribute("Behaviour"),	DescriptionAttribute("If true the dialog will operate in file open mode, otherwise it will operate in save mode")]
		public bool OpenMode
			{
			get
				{
				return _OpenMode;
				}
			set
				{
				_OpenMode = value;
				}
			}
		
		private bool _forceFileFound;
		[CategoryAttribute("Behaviour"),	DescriptionAttribute("If true; File must exist for Open or File must not exist save")]
		public bool forceFileFound
			{
			get
				{
				return _forceFileFound;
				}
			set
				{
				_forceFileFound = value;
				}
			}
		
		[CategoryAttribute("Value"),	DescriptionAttribute("The text in the controll"), BrowsableAttribute(true)]
		public override string Text
			{
			get
				{
				return cmbText.Text;
				}
			set
				{
				sleep = true;
				cmbText.Text = value;
				sleep = false;
				raiseValueChanged(this, EventArgs.Empty);
				}
			}
		
		
		private string _filter;
		[CategoryAttribute("Behaviour"), DescriptionAttribute("Extension string as per file dialog box")]
		public string filter
			{
			get
				{
				return _filter;
				}
			set
				{
				_filter = value;
				}
			}
		
		public bool FileExists
			{
			get
				{
				return fileExists(Text);
				}
			}
		
			
		public wdFileBox()
			{
			// This call is required by the Windows Form Designer.
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
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmbText = new System.Windows.Forms.ComboBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cmbText
			// 
			this.cmbText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbText.Location = new System.Drawing.Point(0, 0);
			this.cmbText.Name = "cmbText";
			this.cmbText.Size = new System.Drawing.Size(288, 21);
			this.cmbText.TabIndex = 0;
			this.cmbText.DropDown += new System.EventHandler(this.cmbText_DropDown);
			this.cmbText.TextChanged += new System.EventHandler(this.cmbText_TextChanged);
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(288, 0);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(24, 20);
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "...";
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// wdFileBox
			// 
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.cmbText);
			this.Name = "wdFileBox";
			this.Size = new System.Drawing.Size(312, 20);
			this.ResumeLayout(false);

		}
		#endregion
		
		public override void Remember()
			{
			if(isNotBlank(Text))
				{
				valueRemember(Text);
				}
			}
		
		public void Recall()
			{
			try
				{
				cmbText.Items.Clear();
				string[] oldFiles = valueRecall();
				foreach(string s in oldFiles)
					{
					if(s == null)
						continue;
					cmbText.Items.Add(s);
					}
				}
			catch
				{
				}
			}

		private void btnBrowse_Click(object sender, System.EventArgs e)
			{
			FileDialog fd;
			
			if(OpenMode)
				{
				fd = new OpenFileDialog();
				((OpenFileDialog)fd).CheckFileExists = forceFileFound;
				}
			else
				{
				fd = new SaveFileDialog();
				((SaveFileDialog)fd).CheckFileExists = forceFileFound;
				}
			
			if(isNotBlank(Text))
				{
				try
					{
					fd.FileName = Text;
					}
				catch
					{
					}
				}
			
			try
				{
				fd.Filter = filter;
				}
			catch
				{
				}
			
			if(fd.ShowDialog() == DialogResult.OK)
				{
				Text = fd.FileName;
				Remember();
				}
			}
		
		
		protected bool fileExists(string fileName)
			{
			return System.IO.File.Exists(fileName);
			}

		private void cmbText_TextChanged(object sender, System.EventArgs e)
			{
			if(!sleep)
				{
				Text = cmbText.Text;
				}
			
			cmbText.ForeColor = FileExists ? Color.Black : Color.Red;
			}

		private void cmbText_DropDown(object sender, System.EventArgs e)
			{
			Recall();
			}
	}
}

