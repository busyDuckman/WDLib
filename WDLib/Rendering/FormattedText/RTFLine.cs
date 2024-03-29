/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Drawing;

namespace WD_toolbox.Rendering.FormattedText
{
/// <summary>
/// A formatted line (a colection of formatted characters)
/// </summary>
public struct RTFLine
	{
	#region data types
	/// <summary>
	/// Represents line mesurements
	/// </summary>
	public struct lineInfo
		{
		/// <summary>
		/// Overall dimensions of the line
		/// </summary>
		public SizeF mesurement;
		
		/// <summary>
		/// Distance from top (0) to the base line as a positive number
		/// </summary>
		public float baseline;
		
		/// <summary>
		/// Assents of each individual characters
		/// </summary>
		public float[] ascents;
		}
	#endregion
	
	//public System.Drawing.StringFormat format;
	
	/// <summary>
	/// Characters on the line
	/// </summary>
	public RTFChar[] data;
	
	/// <summary>
	/// Space above the line
	/// </summary>
	public float spaceAbove;
	
	/// <summary>
	/// Space bleow the line
	/// </summary>
	public float spaceBelow;
	
	/// <summary>
	/// Allingment of the line
	/// </summary>
	public StringAlignment allignment;
	
	/// <summary>
	/// Is the line part of a bulleted list
	/// </summary>
	public bool bulleted;
	
	/// <summary>
	/// Allingment of the line (as a horizontalAllignment struct for compatability woth windows forms)
	/// </summary>
	public System.Windows.Forms.HorizontalAlignment horizontalAllignment
		{
		get
			{
			switch(allignment)
				{
				case StringAlignment.Near:
					return System.Windows.Forms.HorizontalAlignment.Left;
				case StringAlignment.Center:
					return System.Windows.Forms.HorizontalAlignment.Center;
				default:
					return System.Windows.Forms.HorizontalAlignment.Right;
				}	
			}
		set
			{
			switch(value)
				{
				case System.Windows.Forms.HorizontalAlignment.Left:
					allignment = StringAlignment.Near;
					break;
				case System.Windows.Forms.HorizontalAlignment.Center:
					allignment = StringAlignment.Center;
					break;
				default:
					allignment = StringAlignment.Far;
					break;
				}
			}
		}
	
	
	/// <summary>
	/// Set space above and bleow line to a specific value
	/// </summary>
	public float lineSpacing
		{
		set
			{
			spaceAbove = value*0.5f;
			spaceBelow = value*0.5f;
			}
		}
	
	
	/// <summary>
	/// A generic string rendering and mesuring format, the GDI+ default is not suitable for our purposes
	/// </summary>
	/// <returns></returns>
	public static StringFormat defalutFormat()
		{
		/*StringFormat sf = new StringFormat();
		//sf.Alignment = StringAlignment
		sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
		//sf.LineAlignment = StringAlignment
		sf.Trimming = StringTrimming.None;
		return sf;*/
		StringFormat sf = StringFormat.GenericTypographic;
		sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
		sf.Trimming |= StringTrimming.None;
		return sf;
		}
	
	/// <summary>
	/// Concatinated two lines into one.
	/// </summary>
	/// <param name="fl1">First formattedLine</param>
	/// <param name="fl2">Second formattedLine</param>
	/// <returns>Concatinated version of the two lines</returns>
	public static RTFLine operator +(RTFLine fl1, RTFLine fl2)
		{
		RTFLine newLine = fl1;
		
		if(fl1.data == null)
			{
			return fl2;
			}
		
		if(fl2.data == null)
			{
			return fl1;
			}
		
		RTFChar[] newData = new RTFChar[fl1.data.Length + fl2.data.Length];
		fl1.data.CopyTo(newData, 0);
		fl2.data.CopyTo(newData, fl1.data.Length);
		
		newLine.data = newData;
		
		return newLine;
		}
	
	/// <summary>
	/// Concatinates a character and a line.
	/// </summary>
	/// <param name="fl">formattedLine as start of new line</param>
	/// <param name="fc">formattedChar to append to line</param>
	/// <returns>Concatinated version of the two line</returns>
	public static RTFLine operator +(RTFLine fl, RTFChar fc)
		{
		return fl + (RTFLine)fc;
		}
	
	/// <summary>
	/// measure the dimensions of the line
	/// </summary>
	/// <param name="g">Graphics context</param>
	/// <param name="fontTable">Table of fonts</param>
	/// <returns>lineInfo struct with information on the line.</returns>
	public lineInfo measure(Graphics g, FontFamily[] fontTable)
		{
		int i;
		lineInfo info;
		info.mesurement = new SizeF(0.0f, 0.0f);
		info.baseline = 0;
		info.ascents = new float[0];
		SizeF charSize;
		float ascent;
		
		if(data==null)
			return info;
		
		info.ascents = new float[data.Length];
		
		string printString;
		Font printFont;
		RTFChar fc;
		int j;
		
		for(i=0;i<data.Length;i++)
			{
			fc = data[i];
			printString = fc.Char.ToString();
			
			//apend any characters drawn in the same style
			for(j=(i+1);(j<data.Length)&&(fc.compareFormat(data[j]));j++)
				{
				printString += data[j].Char;
				}
			
			printFont = fc.getFont(fontTable);
			charSize = g.MeasureString(printString, printFont,999,RTFLine.defalutFormat());
			
			ascent = fontTable[data[i].fontIndex].GetCellAscent(data[i].FontStyle);
			ascent = data[i].size * (ascent / fontTable[data[i].fontIndex].GetEmHeight(data[i].FontStyle))*1.3f;
			
			if(ascent > info.baseline)
				{
				info.baseline = ascent;
				}
			
			info.ascents[i] = ascent;
			for(int j2=i;j2<j;j2++)
				info.ascents[j2] = ascent;
			//update i to the end of the common string
			i = j-1;
			
			//printFont = fc.getFont(fontTable);
			//charSize = g.MeasureString(printString, printFont,999,formattedLine.defalutFormat());
			info.mesurement.Width += charSize.Width;
			
			if(charSize.Height > info.mesurement.Height)
				{
				info.mesurement.Height = charSize.Height;
				}
			
			}
		
		/*
		for(i=0;i<data.Length;i++)
			{
			//set the height to the maximum character height
			
			charSize = data[i].measure(g, fontTable);
			if(charSize.Height > info.mesurement.Height)
				{
				info.mesurement.Height = charSize.Height;
				}
			
			//calculate the acsent of the character
			ascent = fontTable[data[i].fontIndex].GetCellAscent(data[i].FontStyle);
			
			//convert to pixels
			//ascent = data[i].size * (ascent / fontTable[data[i].fontIndex].GetEmHeight(data[i].FontStyle));
			//ascent = data[i].size / fontTable[data[i].fontIndex].GetEmHeight(data[i].FontStyle) * ascent;
			ascent = data[i].size * (ascent / fontTable[data[i].fontIndex].GetEmHeight(data[i].FontStyle))*1.3f;
			
			//cache this in the mesurements for later use when rendering the string
			info.ascents[i] = ascent;
		
			//baseline might not be related to the biggest font, but the one with the most ascent
			if(ascent > info.baseline)
				{
				info.baseline = ascent;
				}
			info.mesurement.Width += charSize.Width;
			}
		*/
		
		return info;
		}
	
	/// <summary>
	/// Draws a string
	/// </summary>
	/// <param name="g"></param>
	/// <param name="pos"></param>
	/// <param name="fontTable"></param>
	public void drawString(Graphics g, PointF pos, FontFamily[] fontTable)
		{
		drawString(g, pos, fontTable, measure(g, fontTable));
		}
	
	
	/// <summary>
	/// Allows for customised or precalculated ascent mesurements to be passed, fastest way to draw a string
	/// </summary>
	/// <param name="g"></param>
	/// <param name="pos"></param>
	/// <param name="fontTable"></param>
	/// <param name="mesurements"></param>
	public void drawString(Graphics g, PointF pos, FontFamily[] fontTable, lineInfo mesurements)
		{
		int i, j;
		string printString;
		Font printFont;
		
		SizeF charSize;
		//float across=pos.X;
		RTFChar fc;
		PointF charPos = pos;
		
		if(data==null)
			return;
		
		for(i=0;i<data.Length;i++)
			{
			fc = data[i];
			printString = fc.Char.ToString();
			
			//adjust character position to meat the baseline
			charPos.Y = pos.Y + mesurements.baseline - mesurements.ascents[i];
			
			//apend any characters drawn in the same style
			for(j=(i+1);(j<data.Length)&&(fc.compareFormat(data[j]));j++)
				{
				printString += data[j].Char;
				}
			
			//update i to the end of the common string
			i = j-1;
			
			//cache the print font
			printFont = fc.getFont(fontTable);
			
			//charSize = fc.measure(g, fontTable);
			charSize = g.MeasureString(printString, printFont,999,RTFLine.defalutFormat());
			
			//draw the string					
			g.DrawString(printString, printFont, new SolidBrush(fc.col), charPos, RTFLine.defalutFormat());
			
			charPos.X += charSize.Width;
			}
		}
	/*
	//all formating defalus to the default formating
	public static formattedLine operator +(formattedChar fc, formattedLine fl)
		{
		}*/
	
	/// <summary>
	/// Amount of characters on the line, including leading and trailing white space.
	/// </summary>
	public int count
		{
		get
			{
			if(data==null)
				return 0;
			
			return data.Length;
			}
		}
	
	
	}
}
