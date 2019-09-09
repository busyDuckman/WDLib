/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;

namespace WD_toolbox.AplicationFramework
{
/// <summary>
/// A long message version of the windows message box.
/// Can handle RTF text.
/// </summary>
public class LongMessageBox : System.Windows.Forms.Form
	{
	private string _Message;
	/// <summary>
	/// A Message to display.
	/// Using this value to set a message over-rides any RTF information. <see cref="LongMessageBox.rtf"/> 
	/// </summary>
	public string Message
		{
		get
			{
			return _Message;
			}
		set
			{
			_Message = value;
			rtMessage.Text = _Message;
			}
		}

	/// <summary>
	/// Title for the dialoge.
	/// </summary>
	public string Title
		{
		get
			{
			return this.Text;
			}
		set
			{
			this.Text = value;
			}
		}
	
	/// <summary>
	/// rtf version of the message being displayed.
	/// </summary>
	public string rtf
		{
		get
			{
			return rtMessage.Rtf;
			}
		set
			{
			rtMessage.Rtf = value;
			}
		}
	
	/// <summary>
	/// Appends a bitmap to the end of the message.
	/// </summary>
	public void appendImage(Bitmap image)
		{
		//I hate code that shunts stuff through the clipboard
		//This is a bit of a hack
		try
			{
			IDataObject oldClipBoard =  Clipboard.GetDataObject();
			try
				{
				Clipboard.SetDataObject(new Bitmap((Bitmap)image.Clone()));
				rtMessage.SelectionStart = rtMessage.Text.Length;
				rtMessage.Paste();
				}
			finally
				{
				Clipboard.SetDataObject(oldClipBoard);
				}
			}
		catch
			{
			}
		}
	
	
	private System.Windows.Forms.Button btnOk;
	private System.Windows.Forms.Button btnCancel;
	private System.Windows.Forms.Button btnRetry;
	private System.Windows.Forms.Button btnAbort;
	private System.Windows.Forms.Button btnIgnore;
	private System.Windows.Forms.Button btnYes;
	private System.Windows.Forms.Button btnNo;
	private System.Windows.Forms.RichTextBox rtMessage;
	private System.Windows.Forms.StatusBar statusBar1;
	private System.Windows.Forms.Button btnPrint;
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.Container components = null;

	/// <summary>
	/// Creates a new frmLongMessageBox form object. Title will be "Message" and form will have an Ok button.
	/// </summary>
	public LongMessageBox(string message) : this("Message", message)
		{
		}
	
	/// <summary>
	/// Creates a new frmLongMessageBox form object (with an Ok button).
	/// </summary>
	/// <param name="title">Title for new window</param>
	/// <param name="message">Message to display</param>
	public LongMessageBox(string title, string message) : this(title, message, MessageBoxButtons.OK)
		{
		}
	
	/// <summary>
	/// Creates a new frmLongMessageBox form object.
	/// </summary>
	/// <param name="title">Title for new window</param>
	/// <param name="message">Message to display</param>
	/// <param name="buttons">Buttons to be present</param>
	public LongMessageBox(string title, string message, MessageBoxButtons buttons)
		{
		//
		// Required for Windows Form Designer support
		//
		InitializeComponent();
		
		enableButtons(buttons);
		Title = title;
		Message = message;
		}
	
	/// <summary>
	/// Used to make visable the correct dialoge buttons.
	/// </summary>
	/// <param name="buttons">Buttons to make visable.</param>
	protected void enableButtons(MessageBoxButtons buttons)
		{
		switch(buttons)
			{
			case MessageBoxButtons.AbortRetryIgnore:
				btnAbort.Visible = true;
				btnCancel.Visible = false;
				btnIgnore.Visible = true;
				btnNo.Visible = false;
				btnOk.Visible = false;
				btnRetry.Visible = true;
				btnYes.Visible = false;
				break;
			case MessageBoxButtons.OK:
				btnAbort.Visible = false;
				btnCancel.Visible = false;
				btnIgnore.Visible = false;
				btnNo.Visible = false;
				btnOk.Visible = true;
				btnRetry.Visible = false;
				btnYes.Visible = false;
				break;
			case MessageBoxButtons.OKCancel:
				btnAbort.Visible = false;
				btnCancel.Visible = true;
				btnIgnore.Visible = false;
				btnNo.Visible = false;
				btnOk.Visible = true;
				btnRetry.Visible = false;
				btnYes.Visible = false;
				break;
			case MessageBoxButtons.RetryCancel:
				btnAbort.Visible = false;
				btnCancel.Visible = true;
				btnIgnore.Visible = false;
				btnNo.Visible = false;
				btnOk.Visible = false;
				btnRetry.Visible = true;
				btnYes.Visible = false;
				break;
			case MessageBoxButtons.YesNo:
				btnAbort.Visible = false;
				btnCancel.Visible = false;
				btnIgnore.Visible = false;
				btnNo.Visible = true;
				btnOk.Visible = false;
				btnRetry.Visible = false;
				btnYes.Visible = true;
				break;
			case MessageBoxButtons.YesNoCancel:
				btnAbort.Visible = false;
				btnCancel.Visible = true;
				btnIgnore.Visible = false;
				btnNo.Visible = true;
				btnOk.Visible = false;
				btnRetry.Visible = false;
				btnYes.Visible = true;
				break;
			}
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

	#region Windows Form Designer generated code
	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
		{
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRetry = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnIgnore = new System.Windows.Forms.Button();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.rtMessage = new System.Windows.Forms.RichTextBox();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.btnPrint = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.CausesValidation = false;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(352, 392);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(48, 24);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(416, 392);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(48, 24);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Visible = false;
            // 
            // btnRetry
            // 
            this.btnRetry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRetry.CausesValidation = false;
            this.btnRetry.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btnRetry.Location = new System.Drawing.Point(72, 392);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(48, 24);
            this.btnRetry.TabIndex = 2;
            this.btnRetry.Text = "Retry";
            this.btnRetry.Visible = false;
            // 
            // btnAbort
            // 
            this.btnAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAbort.CausesValidation = false;
            this.btnAbort.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnAbort.Location = new System.Drawing.Point(8, 392);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(48, 24);
            this.btnAbort.TabIndex = 3;
            this.btnAbort.Text = "Abort";
            this.btnAbort.Visible = false;
            // 
            // btnIgnore
            // 
            this.btnIgnore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnIgnore.CausesValidation = false;
            this.btnIgnore.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnIgnore.Location = new System.Drawing.Point(136, 392);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(48, 24);
            this.btnIgnore.TabIndex = 4;
            this.btnIgnore.Text = "Ignore";
            this.btnIgnore.Visible = false;
            // 
            // btnYes
            // 
            this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYes.CausesValidation = false;
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnYes.Location = new System.Drawing.Point(288, 392);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(48, 24);
            this.btnYes.TabIndex = 5;
            this.btnYes.Text = "Yes";
            this.btnYes.Visible = false;
            // 
            // btnNo
            // 
            this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNo.CausesValidation = false;
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.Location = new System.Drawing.Point(352, 392);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(48, 24);
            this.btnNo.TabIndex = 6;
            this.btnNo.Text = "No";
            this.btnNo.Visible = false;
            // 
            // rtMessage
            // 
            this.rtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtMessage.BackColor = System.Drawing.SystemColors.Control;
            this.rtMessage.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtMessage.Location = new System.Drawing.Point(8, 16);
            this.rtMessage.Name = "rtMessage";
            this.rtMessage.ReadOnly = true;
            this.rtMessage.ShowSelectionMargin = true;
            this.rtMessage.Size = new System.Drawing.Size(456, 344);
            this.rtMessage.TabIndex = 7;
            this.rtMessage.Text = "";
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 406);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(472, 24);
            this.statusBar1.TabIndex = 8;
            this.statusBar1.Visible = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPrint.Location = new System.Drawing.Point(416, 360);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(48, 24);
            this.btnPrint.TabIndex = 9;
            this.btnPrint.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // LongMessageBox
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(472, 430);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnIgnore);
            this.Controls.Add(this.btnAbort);
            this.Controls.Add(this.btnRetry);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.rtMessage);
            this.Name = "LongMessageBox";
            this.Text = "Message";
            this.Load += new System.EventHandler(this.frmLongMessageBox_Load);
            this.ResumeLayout(false);

	}
	#endregion

	private void frmLongMessageBox_Load(object sender, System.EventArgs e)
		{
		
		}
	
	/// <summary>
	/// Loads a RTF file to be displayed in the dialoge.
	/// </summary>
	/// <param name="fileName">URL of the file to load.</param>
	/// <returns>True if sucessful; otherwise false.</returns>
	public bool loadRTFFile(string fileName)
		{
		try
			{
			rtMessage.LoadFile(fileName);
			return true;
			}
		catch
			{
			return false;
			}
		}

    private void btnPrint_Click(object sender, EventArgs e)
    {
        PrintDialog printDialog = new PrintDialog();
        PrintDocument documentToPrint = new PrintDocument();
        printDialog.Document = documentToPrint;

        if (printDialog.ShowDialog() == DialogResult.OK)
        {
            StringReader reader = new StringReader(rtMessage.Text);
            documentToPrint.PrintPage += new PrintPageEventHandler(DocumentToPrint_PrintPage);
            documentToPrint.Print();
        }
    }

    private void DocumentToPrint_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
    {
        StringReader reader = new StringReader(rtMessage.Text);
        float LinesPerPage = 0;
        float YPosition = 0;
        int Count = 0;
        float LeftMargin = e.MarginBounds.Left;
        float TopMargin = e.MarginBounds.Top;
        string Line = null;
        Font PrintFont = this.rtMessage.Font;
        SolidBrush PrintBrush = new SolidBrush(Color.Black);

        LinesPerPage = e.MarginBounds.Height / PrintFont.GetHeight(e.Graphics);

        while (Count < LinesPerPage && ((Line = reader.ReadLine()) != null))
        {
            YPosition = TopMargin + (Count * PrintFont.GetHeight(e.Graphics));
            e.Graphics.DrawString(Line, PrintFont, PrintBrush, LeftMargin, YPosition, new StringFormat());
            Count++;
        }

        if (Line != null)
        {
            e.HasMorePages = true;
        }
        else
        {
            e.HasMorePages = false;
        }
        PrintBrush.Dispose();
    }

	}
}
