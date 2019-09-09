/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
#define USE_WINDOWS_COLOUR_PICKER

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using WD_toolbox.AplicationFramework;
using WD_toolbox.Rendering.FormattedText;

namespace WD_toolbox.AplicationFramework.Controls
{
/// <summary>
/// Summary description for fontSettings.
/// </summary>
public class textStyleSettings : System.Windows.Forms.UserControl
	{
	protected bool sleep=false;

	private TextFormat _Value;
	public TextFormat Value
		{
		get
			{
			return _Value;
			}
		set
			{
			if(sleep)
				return;
			
			sleep=true;
			try
				{
				_Value = value;
				
				if(value.font.FontFamily.Name.StartsWith("Micr"))
					{
					int k=0;
					k++;
					}
				btnFontCol.BackColor = Value.textColour;
				btnShadowCol.BackColor = Value.shadowColour;
				chkTextShadow.Checked = Value.showShadow;
				chkVertical.Checked = Value.printVertical;
				
				string attrib = Value.font.Bold ? " [Bold]" : "";
				attrib += Value.font.Italic ? " [Italic]" : "";
				attrib += Value.font.Underline  ? " [Underline]" : "";
				attrib += Value.font.Strikeout  ? " [Strikeout]" : "";
				attrib = Value.font.Name + " size:" + Value.font.Size + attrib;
				
				lblFont.Text = attrib;
				lblFont.Font = (Font)Value.font.Clone();
				lblFont.Update();
				
				if(ValueChanged != null)
					this.ValueChanged(this, EventArgs.Empty);
				}
			catch(Exception ex)
				{
				WDAppLog.logException(ErrorLevel.Error, ex);
				sleep=false;
				}
			finally
				{
				sleep=false;
				}
			}
		}
	
	[CategoryAttribute("Events"), 
	DescriptionAttribute("Occurs when the value is changed")]
	public event EventHandler ValueChanged;
		
	private System.Windows.Forms.CheckBox chkVertical;
	private System.Windows.Forms.Button btnSetFont;
	private System.Windows.Forms.Label lblFont;
	private System.Windows.Forms.Button btnFontCol;
	private System.Windows.Forms.Label label9;
	private System.Windows.Forms.Button btnShadowCol;
	private System.Windows.Forms.Label label8;
	private System.Windows.Forms.CheckBox chkTextShadow;
	private System.Windows.Forms.Label label1;
	private System.Windows.Forms.FontDialog fdMain;
	/// <summary> 
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.Container components = null;

	public textStyleSettings()
	{
		// This call is required by the Windows.Forms Form Designer.
		InitializeComponent();

		// TODO: Add any initialization after the InitializeComponent call
		Value = new TextFormat();
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
		this.chkVertical = new System.Windows.Forms.CheckBox();
		this.btnSetFont = new System.Windows.Forms.Button();
		this.lblFont = new System.Windows.Forms.Label();
		this.btnFontCol = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.btnShadowCol = new System.Windows.Forms.Button();
		this.label8 = new System.Windows.Forms.Label();
		this.chkTextShadow = new System.Windows.Forms.CheckBox();
		this.label1 = new System.Windows.Forms.Label();
		this.fdMain = new System.Windows.Forms.FontDialog();
		this.SuspendLayout();
		// 
		// chkVertical
		// 
		this.chkVertical.BackColor = System.Drawing.Color.Transparent;
		this.chkVertical.Location = new System.Drawing.Point(152, 40);
		this.chkVertical.Name = "chkVertical";
		this.chkVertical.Size = new System.Drawing.Size(80, 24);
		this.chkVertical.TabIndex = 25;
		this.chkVertical.Text = "Vertical";
		this.chkVertical.CheckedChanged += new System.EventHandler(this.chkVertical_CheckedChanged);
		// 
		// btnSetFont
		// 
		this.btnSetFont.BackColor = System.Drawing.SystemColors.Control;
		this.btnSetFont.Location = new System.Drawing.Point(16, 64);
		this.btnSetFont.Name = "btnSetFont";
		this.btnSetFont.Size = new System.Drawing.Size(60, 21);
		this.btnSetFont.TabIndex = 21;
		this.btnSetFont.Text = "Set Font";
		this.btnSetFont.Click += new System.EventHandler(this.btnSetFont_Click);
		// 
		// lblFont
		// 
		this.lblFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
		this.lblFont.BackColor = System.Drawing.Color.White;
		this.lblFont.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFont.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
		this.lblFont.Location = new System.Drawing.Point(16, 112);
		this.lblFont.Name = "lblFont";
		this.lblFont.Size = new System.Drawing.Size(216, 64);
		this.lblFont.TabIndex = 22;
		this.lblFont.Text = "Times New Roman, size 12 BOLD";
		// 
		// btnFontCol
		// 
		this.btnFontCol.BackColor = System.Drawing.Color.Black;
		this.btnFontCol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnFontCol.Location = new System.Drawing.Point(16, 16);
		this.btnFontCol.Name = "btnFontCol";
		this.btnFontCol.Size = new System.Drawing.Size(40, 16);
		this.btnFontCol.TabIndex = 24;
		this.btnFontCol.Click += new System.EventHandler(this.btnFontCol_Click);
		// 
		// label9
		// 
		this.label9.BackColor = System.Drawing.Color.Transparent;
		this.label9.Location = new System.Drawing.Point(64, 16);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(72, 24);
		this.label9.TabIndex = 27;
		this.label9.Text = "Font Colour";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		// 
		// btnShadowCol
		// 
		this.btnShadowCol.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(128)), ((System.Byte)(164)), ((System.Byte)(164)), ((System.Byte)(164)));
		this.btnShadowCol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShadowCol.Location = new System.Drawing.Point(16, 40);
		this.btnShadowCol.Name = "btnShadowCol";
		this.btnShadowCol.Size = new System.Drawing.Size(40, 16);
		this.btnShadowCol.TabIndex = 26;
		this.btnShadowCol.Click += new System.EventHandler(this.btnShadowCol_Click);
		// 
		// label8
		// 
		this.label8.BackColor = System.Drawing.Color.Transparent;
		this.label8.Location = new System.Drawing.Point(64, 40);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(88, 24);
		this.label8.TabIndex = 28;
		this.label8.Text = "Shadow Colour";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		// 
		// chkTextShadow
		// 
		this.chkTextShadow.Location = new System.Drawing.Point(152, 16);
		this.chkTextShadow.Name = "chkTextShadow";
		this.chkTextShadow.Size = new System.Drawing.Size(80, 24);
		this.chkTextShadow.TabIndex = 23;
		this.chkTextShadow.Text = "Shadow";
		this.chkTextShadow.CheckedChanged += new System.EventHandler(this.chkTextShadow_CheckedChanged);
		// 
		// label1
		// 
		this.label1.BackColor = System.Drawing.Color.Transparent;
		this.label1.Location = new System.Drawing.Point(16, 96);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(64, 16);
		this.label1.TabIndex = 29;
		this.label1.Text = "Preview";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		// 
		// textStyleSettings
		// 
		this.Controls.Add(this.lblFont);
		this.Controls.Add(this.chkVertical);
		this.Controls.Add(this.btnSetFont);
		this.Controls.Add(this.btnFontCol);
		this.Controls.Add(this.label9);
		this.Controls.Add(this.btnShadowCol);
		this.Controls.Add(this.chkTextShadow);
		this.Controls.Add(this.label1);
		this.Controls.Add(this.label8);
		this.Name = "textStyleSettings";
		this.Size = new System.Drawing.Size(248, 192);
		this.Load += new System.EventHandler(this.fontSettings_Load);
		this.ResumeLayout(false);

	}
	#endregion

	private void fontSettings_Load(object sender, System.EventArgs e)
		{
		//Value = new textStyle();
		//updateData();
		}
	
	public void updateData()
		{
		if(!sleep)
			{
			try
				{
				Value = new TextFormat(lblFont.Font, btnFontCol.BackColor, chkTextShadow.Checked, btnShadowCol.BackColor, chkVertical.Checked);
				}
			catch
				{
				}
			}
		}
	
	private void doColorSelection(Button b)
		{
		#if USE_WINDOWS_COLOUR_PICKER
		System.Windows.Forms.ColorDialog cdColor = new ColorDialog();
		#else
		frmExtColPick cdColor = new frmExtColPick();
		#endif
		cdColor.Color = b.BackColor;
		if(cdColor.ShowDialog(this) == DialogResult.OK)
			{
			b.BackColor = cdColor.Color;
			}
		}

	private void btnSetFont_Click(object sender, System.EventArgs e)
		{
		try
			{
			Font temp = (Font)fdMain.Font.Clone();
			fdMain.Font = (Font)Value.font.Clone();
			fdMain.ShowApply = false;
			if(fdMain.ShowDialog() == DialogResult.OK)
				{
				lblFont.Font = (Font)fdMain.Font.Clone();
				updateData();
				}
			else
				{
				//restore old font
				fdMain.Font = temp;
				}
			}
		catch(Exception exception)
			{
			MessageBox.Show(this, "Could not change font" + exception.Message, "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

	private void btnFontCol_Click(object sender, System.EventArgs e)
		{
		doColorSelection(btnFontCol);
		updateData();
		}

	private void btnShadowCol_Click(object sender, System.EventArgs e)
		{
		doColorSelection(btnShadowCol);
		updateData();
		}

	private void chkTextShadow_CheckedChanged(object sender, System.EventArgs e)
		{
		updateData();
		}

	private void chkVertical_CheckedChanged(object sender, System.EventArgs e)
		{
		updateData();
		}
	}
}
