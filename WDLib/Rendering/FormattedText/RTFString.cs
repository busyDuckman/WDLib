/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */

//#define SHOW_DEBUG_HELPERS
using System;
using System.Collections;
using System.Drawing;

namespace WD_toolbox.Rendering.FormattedText
{
/// <summary>
/// A rich text format string class.
/// This calss can import RTF files.
/// </summary>
public struct RTFString
	{
	#region instance data
	/// <summary>
	/// Lines in the string.
	/// </summary>
	RTFLine[] lines;
	/// <summary>
	/// Table of fonts used in the string.
	/// </summary>
	public FontFamily[] fontTable;
	#endregion
	
	#region proxy data (not yet marked unserializable)
	private RTFLine.lineInfo[] proxyMesurements;
	long totalLength;
	#endregion
	
	/// <summary>
	/// Returns an empty formatted string.
	/// </summary>
	public static RTFString emptyString
		{
		get
			{
			RTFString fs = new RTFString();
			fs.fontTable = null;
			fs.lines = null;
			fs.proxyMesurements = null;
			return fs;
			}
		}
	
	/*public formattedString()
		{
		}*/
	
	/// <summary>
	/// Invalidate all mesurments used to accelerate string rendering.
	/// </summary>
	public void invalidateMesurements()
		{
		proxyMesurements = null;
		}
	
	/// <summary>
	/// Create a new formattedString using rtf code.
	/// </summary>
	/// <param name="rtf"></param>
	/// <returns></returns>
	public static RTFString fromRTF(string rtf)
		{
		//ArrayList fontTable;
		RTFParser rtfParser = new RTFParser();
		return rtfParser.parseRTF(rtf);
		}
	
	/// <summary>
	/// Renderes the formatted string
	/// </summary>
	/// <param name="g">Graphics object to render to</param>
	/// <param name="region">Bounds for the rendering.</param>
	/// <param name="movement">Pixels int hey Y direction to scroll.</param>
	public void drawString(Graphics g, Rectangle region, long movement)
		{
		if(lines == null)
			return;
		if(lines.Length == 0)
			return;
		
		int i;
		float depth = 0;
		SizeF stringSize;
		
		
		
		if((proxyMesurements == null) || (proxyMesurements.Length != lines.Length))
			{
			proxyMesurements = new RTFLine.lineInfo[lines.Length];
			totalLength = 0;
			for(i=0;i<lines.Length;i++)
				{
				proxyMesurements[i] = lines[i].measure(g, fontTable);
				totalLength += (long)proxyMesurements[i].mesurement.Height;
				totalLength += (long)(lines[i].spaceAbove + lines[i].spaceBelow);
				}
			}
		
		long vl = totalLength + region.Height;
		int offset=0;
		if(movement > 0)
			{
			offset = (int)((((long)movement+totalLength) % (long)vl) - totalLength);
			}
		else if(movement < 0)
			{
			offset = (int)( -(((long)-movement+totalLength) % (long)vl) + region.Height);
			}
		
		//formattedLine.lineInfo mesurements;
		
		for(i=0;i<lines.Length;i++)
			{
			//mesurements = lines[i].measure(g, fontTable);
			stringSize = proxyMesurements[i].mesurement;
			
			depth += lines[i].spaceAbove;
			
			int alignmentAlteration = 0;
			if(lines[i].allignment == StringAlignment.Center)
				{
				alignmentAlteration = (int)(region.Width * 0.5f - stringSize.Width * 0.5f);
				}
			else if(lines[i].allignment == StringAlignment.Far)
				{
				alignmentAlteration = (int)(region.Width - stringSize.Width);
				}
			
			//draw the string for this line
			lines[i].drawString(g, new PointF(region.X + alignmentAlteration + 1, region.Y + depth + offset), fontTable, proxyMesurements[i]);
			
			#if SHOW_DEBUG_HELPERS
			//draw the xy pos
			g.DrawRectangle(Pens.Red, region.X, region.Y + depth, 1, 1);
			
			//draw the baseline
			g.DrawLine(Pens.Blue, region.X, region.Y + depth + proxyMesurements[i].baseline, region.X + stringSize.Width, region.Y + depth + proxyMesurements[i].baseline);
			#endif
			
			depth += stringSize.Height;
			
			depth += lines[i].spaceBelow;
			}
		}
	
	
	/// <summary>
	/// Add a line to a formatted string.
	/// </summary>
	/// <param name="fs">A formattedString</param>
	/// <param name="fl">A formattedLine</param>
	/// <returns>fl appended to fs.</returns>
	public static RTFString operator +(RTFString fs, RTFLine fl)
		{
		RTFString fsNew = fs;
		if(fsNew.lines == null)
			{
			fsNew.lines = new RTFLine[1] {fl};
			}
		else
			{
			RTFLine[] newlines = new RTFLine[fs.lines.Length + 1];
			fs.lines.CopyTo(newlines, 0);
			newlines[fs.lines.Length] = fl;
			fsNew.lines = newlines;
			}
		return fsNew;		
		}
	
	
	/// <summary>
	/// Used to test if the formattedString is empty.
	/// </summary>
	/// <returns>Boolean indicating if the formattedString is empty.</returns>
	public bool isEmpty()
		{
		return (lines == null);
		}
	}
}
