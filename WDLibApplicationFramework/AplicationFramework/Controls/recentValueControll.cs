/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */

//#define USER_READABLE
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Microsoft.Win32;

using WD_toolbox.AplicationFramework;
using WD_toolbox.Data.DataStructures;

namespace wdControlls
{
	/// <summary>
	/// Summary description for recentValueControll.
	/// </summary>
	public class recentValueControll : System.Windows.Forms.UserControl
	{

		public const string recentPrefix = "value_";
		
		private int _maxRecent=4;
		[CategoryAttribute("Behaviour"),	DescriptionAttribute("Maximum recent values")]
		public int maxRecent
			{
			get
				{
				return _maxRecent;
				}
			set
				{
				_maxRecent = value;
				}
			}
		
		[CategoryAttribute("Events"), 
		DescriptionAttribute("Occurs when the value is changed")]
		public event EventHandler ValueChanged;
		
		[CategoryAttribute("Behaviour"),	DescriptionAttribute("Customised registry key (key only, not path), for dialogs sharing recent values")]		
		protected string _customRegistryKey=null;
		public string customRegistryKey
			{
			get
				{
				return _customRegistryKey;
				}
			set
				{
				_customRegistryKey = value;
				}
			}
		
		private Guid _identifier;
		public Guid identifier
			{
			get
				{
				return _identifier;
				}
			set
				{
				_identifier = value;
				}
			}
		
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public recentValueControll()
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
			// 
			// recentValueControll
			// 
			this.Name = "recentValueControll";
			this.Load += new System.EventHandler(this.recentValueControll_Load);

		}
		#endregion

		private void recentValueControll_Load(object sender, System.EventArgs e)
		{
		
		}
		
		public string myID()
			{
			if(isNotBlank(customRegistryKey))
				return customRegistryKey.Trim();
			
			string thisName = "";
			Control c = this;
			while(c != null)
				{
				thisName += c.Name + "_";
				c = c.Parent;
				}
			
			string className = this.GetType().ToString();
			
			return className + "@" + thisName;
			}
		
		protected void valueRemember(string what)
			{
			RegistryKey rk =  Application.UserAppDataRegistry.CreateSubKey(myID());
			
			#if USER_READABLE
			///send the recent list back
			for(i=maxRecent-1;i>1;i--)
				{
				found = rk.GetValue(recentPrefix + i);
				if(found != null)
					rk.SetValue(recentPrefix + (i+1), found); 
				}
			rk.SetValue(recentPrefix + 0, what);
			#else
            object s = ApplicationHelper.getSetting(@"recent", myID());
			if(!(s is Quack))
				s = new Quack();
			
			if(((Quack)s).Contains(what))
				((Quack)s).Remove(what);
			
			while(((Quack)s).Count >= maxRecent)
				((Quack)s).Dequeue();
			
			((Quack)s).Enqueue(what);

            ApplicationHelper.storeSetting(@"recent", myID(), s);
			//rk.SetValue(recentPrefix + "list", s);
			#endif
			}
		
		protected string[] valueRecall()
			{
			int i;
			RegistryKey rk =  Application.UserAppDataRegistry.CreateSubKey(myID());
			string [] result = new string[0];
			
			#if USER_READABLE
			ArrayList previous = new ArrayList();
			object found;
			for(i=0;i<maxRecent;i++)
				{
				found = rk.GetValue(recentPrefix + i);
				if(found != null)
					previous.Add(found.ToString());
				}
			
			result = new string[previous.Count];
			previous.CopyTo(result, 0);
			#else

            object s = ApplicationHelper.getSetting(@"recent", myID());
			if(s is Quack)
				{
				result = new string[((Quack)s).Count];
				i=0;
				while(((Quack)s).Count > 0)
					{
					result[i] = (string)((Quack)s).Pop(); 
					i++;
					}
				//((stew)s).CopyTo(result, 0);
				}
			
			#endif
			
			
			return result;
			}
		
		public virtual void Remember()
			{
			}
		
		protected bool isNotBlank(string text)
			{
			if(text == null)
				return false;
			return (text.Trim() != "");
			}
		
		protected void raiseValueChanged(object sender, EventArgs args)
			{
			if(ValueChanged != null)
				{
				ValueChanged(sender,  args);
				}
			}
	}
}
