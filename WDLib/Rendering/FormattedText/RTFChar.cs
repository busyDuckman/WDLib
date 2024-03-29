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
/// A formatted character
/// </summary>
public struct RTFChar
	{
	#region instance data
	/// <summary>
	/// Character represented
	/// </summary>
	public char Char;
	
	/// <summary>
	/// Index into font table entry for the characters font.
	/// </summary>
	public int fontIndex;
	
	/// <summary>
	/// Is the character bold
	/// </summary>
	public bool bold;
	
	/// <summary>
	/// Is the character in italics
	/// </summary>
	public bool italic;
	
	/// <summary>
	/// Is the character underlined
	/// </summary>
	public bool underline;
	
	/// <summary>
	/// Is the character stiked out
	/// </summary>
	public bool strikeout;
	
	/// <summary>
	/// What colour is the character
	/// </summary>
	public Color col;

	/// <summary>
	/// What size is the character
	/// </summary>
	public float size;
	#endregion
	
	
	/*public formattedChar(char c, int fontIndex)
		{
		Char = c;C:\Documents and Settings\Administrator\My Documents\Visual Studio 2005\Projects\WD 40\WD toolbox 2k5\Rendering\FormattedText\leachRTF.cs
		this.fontIndex = fontIndex;
		}*/

	/// <summary>
	/// Access character style using the font style structure
	/// </summary>
	public FontStyle FontStyle
		{
		get
			{
			FontStyle fs = FontStyle.Regular;
			if(bold)
				fs |= FontStyle.Bold;
			if(italic)
				fs |= FontStyle.Italic;
			if(underline)
				fs |= FontStyle.Underline;
			if(strikeout)
				fs |= FontStyle.Strikeout;
			
			return fs;
			}
		set
			{
			bold = ((value & FontStyle.Bold) !=0);
			italic = ((value & FontStyle.Italic) !=0);
			underline = ((value & FontStyle.Underline) !=0);
			strikeout = ((value & FontStyle.Strikeout) !=0);
			}
		}
	
	
	/// <summary>
	/// Mesure the size of the character in the given graphics context
	/// </summary>
	/// <param name="g">graphics context</param>
	/// <param name="fontTable">Table of fonts</param>
	/// <returns>SizeF structure representing the size of the character in the given graphics context</returns>
	public SizeF measure(Graphics g, FontFamily[] fontTable)
		{
		return g.MeasureString(Char.ToString(), getFont(fontTable));
		}
	
	/// <summary>
	/// Concatination of characters
	/// </summary>
	/// <param name="fc1">A character</param>
	/// <param name="fc2">A character</param>
	/// <returns>formattedLine representing two concatinated formattedChar elements</returns>
	public static RTFLine operator +(RTFChar fc1, RTFChar fc2)
		{
		return (RTFLine)fc1 + (RTFLine)fc2;
		}
	
	/// <summary>
	/// Cast a formattedChar to a formattedLine of single length.
	/// </summary>
	/// <param name="fc"></param>
	/// <returns></returns>
	public static explicit operator RTFLine(RTFChar fc)
		{
		RTFLine fl = new RTFLine();
		fl.data = new RTFChar[1];
		fl.data[0] = fc;
		//fl.format = formattedLine.defalutFormat();
		fl.spaceAbove = 3.0f;
		fl.spaceBelow = 3.0f;
		return fl;
		}
		
	/// <summary>
	/// Checks if format of two characters is identical
	/// </summary>
	/// <param name="other"></param>
	/// <returns></returns>
	public bool compareFormat(RTFChar other)
		{
		if(this.fontIndex != other.fontIndex)
			return false;
		if(this.col != other.col)
			return false;
		if(this.FontStyle != other.FontStyle)
			return false;
		if(this.size != other.size)
			return false;
		
		//no formating diferences were found
		return true;
		}
	
	/// <summary>
	/// Uses the font table to create a font object required to render this character
	/// </summary>
	/// <param name="fontTable"></param>
	/// <returns></returns>
	public Font getFont(FontFamily[] fontTable)
		{
		return new Font(fontTable[fontIndex], size, FontStyle);
		}
	}


}
