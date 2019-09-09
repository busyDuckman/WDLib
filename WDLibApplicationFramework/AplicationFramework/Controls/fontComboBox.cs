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
	/// Summary description for fontPicker.
	/// </summary>
	public class FontSelectBox :  ComboBox
	{
		protected ArrayList listedFonts;

		public FontFamily selectedFont
			{
			get
				{
				//return FontFamily.Families[SelectedIndex];
				return (FontFamily)listedFonts[SelectedIndex];
				}
			set
				{
				int i;
				for(i=0;i<Items.Count;i++)
					{
					if(Items[i].ToString() == value.Name)
						SelectedIndex =  i;
					}
				}
			}
		
		private int maxwid = 0;
		private Image ttimg;

		public FontSelectBox()
		{			
			ttimg = new Bitmap(16,16);	
			MaxDropDownItems = 20;
			IntegralHeight = false;
			Sorted = false;
			DropDownStyle = ComboBoxStyle.DropDownList;
			DrawMode = DrawMode.OwnerDrawVariable;
			listedFonts = new ArrayList();
			PopulateFonts();						
		}

		public void PopulateFonts()
			{
			listedFonts = new ArrayList();
			Items.Add("Error occored");
			Items.Clear();
			//foreach (FontFamily ff in FontFamily.Families)
			int i;
			for(i=0;i<FontFamily.Families.Length;i++)
				{
				FontFamily ff = FontFamily.Families[i];
				if(ff.IsStyleAvailable(FontStyle.Regular))
					{
					Items.Add(ff.Name);
					listedFonts.Add(ff);
					}
				}			
			if(Items.Count > 0)
				SelectedIndex=0;
			ttimg = new Bitmap(16,16);//(GetType(),"ttfbmp.bmp");
			}

		protected override void OnMeasureItem(System.Windows.Forms.MeasureItemEventArgs e)
			{	
			if(e.Index > -1)
				{
				int w = 0;
				string fontstring = Items[e.Index].ToString();						
				Graphics g = CreateGraphics();
				e.ItemHeight = (int)g.MeasureString(fontstring, new Font(fontstring,10)).Height;
				w = (int)g.MeasureString(fontstring, new Font(fontstring,10)).Width;
				/*if(both)
					{
					int h1 = (int)g.MeasureString(samplestr, new Font(fontstring,10)).Height;
					int h2 = (int)g.MeasureString(Items[e.Index].ToString(), new Font("Arial",10)).Height;
					int w1 = (int)g.MeasureString(samplestr, new Font(fontstring,10)).Width;
					int w2 = (int)g.MeasureString(Items[e.Index].ToString(), new Font("Arial",10)).Width;
					if(h1 > h2 )
						h2 = h1;
					e.ItemHeight = h2;
					w = w1 + w2;
					}*/
				w += ttimg.Width*2;
				if(w > maxwid)
					maxwid=w;
				if(e.ItemHeight > 20)
					e.ItemHeight = 20;
				}
							
			base.OnMeasureItem(e);
			}

		protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
			{	
			Font nfont;
			Font afont = new Font("Arial",10);
			
			if(e.Index > -1)
				{
				string fontstring = Items[e.Index].ToString();
				nfont = new Font(fontstring,10);

				/*if(both)
					{
					Graphics g = CreateGraphics();
					int w = (int)g.MeasureString(fontstring, afont).Width;

					if((e.State & DrawItemState.Focus)==0)
						{
						e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window),
							e.Bounds.X+ttimg.Width,e.Bounds.Y,e.Bounds.Width,e.Bounds.Height);
						e.Graphics.DrawString(fontstring,afont,new SolidBrush(SystemColors.WindowText),
							e.Bounds.X+ttimg.Width*2,e.Bounds.Y);	
						e.Graphics.DrawString(samplestr,nfont,new SolidBrush(SystemColors.WindowText),
							e.Bounds.X+w+ttimg.Width*2,e.Bounds.Y);	
						}
					else
						{
						e.Graphics.FillRectangle(new SolidBrush(SystemColors.Highlight),
							e.Bounds.X+ttimg.Width,e.Bounds.Y,e.Bounds.Width,e.Bounds.Height);
						e.Graphics.DrawString(fontstring,afont,new SolidBrush(SystemColors.HighlightText),
							e.Bounds.X+ttimg.Width*2,e.Bounds.Y);
						e.Graphics.DrawString(samplestr,nfont,new SolidBrush(SystemColors.HighlightText),
							e.Bounds.X+w+ttimg.Width*2,e.Bounds.Y);
						}	
					}*/
				//else
					{

					if((e.State & DrawItemState.Focus)==0)
						{
						e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window),
							e.Bounds.X+ttimg.Width,e.Bounds.Y,e.Bounds.Width,e.Bounds.Height);
						e.Graphics.DrawString(fontstring,nfont,new SolidBrush(SystemColors.WindowText),
							e.Bounds.X+ttimg.Width*2,e.Bounds.Y);
			
						}
					else
						{
						e.Graphics.FillRectangle(new SolidBrush(SystemColors.Highlight),
							e.Bounds.X+ttimg.Width,e.Bounds.Y,e.Bounds.Width,e.Bounds.Height);
						e.Graphics.DrawString(fontstring,nfont,new SolidBrush(SystemColors.HighlightText),
							e.Bounds.X+ttimg.Width*2,e.Bounds.Y);
						}			

					}

				e.Graphics.DrawImage(ttimg, new Point(e.Bounds.X, e.Bounds.Y)); 
				}
			base.OnDrawItem(e);
			}

		protected override void OnDropDown(System.EventArgs e)
			{
			this.DropDownWidth = maxwid+30;
			}		

	}
}
