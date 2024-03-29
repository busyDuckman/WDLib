/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;


namespace WD_toolbox.Rendering.FormattedText
{
/// <summary>
/// A class for converting a RTF string to a RTFString.
/// </summary>
internal class RTFParser
	{
	RichTextBox rtCapture=null;
	
	/// <summary>
	/// C reates a new leachRTF object
	/// </summary>
	public RTFParser()
		{
		rtCapture = new RichTextBox();
		rtCapture.Size = new System.Drawing.Size(1024, 768);
		rtCapture.WordWrap = false;
		}
	
	/// <summary>
	/// Creates a RTFString from rtf code.
	/// </summary>
	/// <param name="rtf">rtf code.</param>
	/// <returns>A formattedString version of the rtf code.</returns>
	public RTFString parseRTF(string rtf)
		{
		rtCapture.Rtf = rtf;
		
		rtCapture.SelectAll();
		int numChars = rtCapture.SelectionLength; 
		
		RTFString text = new RTFString();
		text.invalidateMesurements();
		RTFLine currentLine = new RTFLine();
		RTFChar currentChar = new RTFChar();
		ArrayList fontTable = new ArrayList();
		int fontIndex;
		
		int i;
		
		for(i=0;i<numChars;i++)
			{
			rtCapture.Select(i, 1);
			currentChar.FontStyle = rtCapture.SelectionFont.Style;
			currentChar.size = rtCapture.SelectionFont.Size;
			currentChar.Char = rtCapture.SelectedText[0];
			currentChar.col = rtCapture.SelectionColor;
			if(currentChar.Char == '\r')
				{
				//do nothing
				}
			else if (currentChar.Char == '\n')
				{
				if(currentLine.count == 0 )
					{
					fontIndex = fontTable.IndexOf(rtCapture.SelectionFont.FontFamily);
					if(fontIndex == -1)
						{
						fontIndex = fontTable.Add(rtCapture.SelectionFont.FontFamily);
						}
				
					currentChar.fontIndex = fontIndex;
					
					//append to line
					currentLine += currentChar;
					}

				//end line
				currentLine.horizontalAllignment = rtCapture.SelectionAlignment;
				currentLine.bulleted = rtCapture.SelectionBullet;
				text += currentLine;
				currentLine = new RTFLine();
				}
			else
				{
				//check font table
				fontIndex = fontTable.IndexOf(rtCapture.SelectionFont.FontFamily);
				if(fontIndex == -1)
					{
					fontIndex = fontTable.Add(rtCapture.SelectionFont.FontFamily);
					}
				
				currentChar.fontIndex = fontIndex;
				
				//append to line
				currentLine += currentChar;
				
				}
			}
		
		//check if last line was not terminated and needs apending
		if(currentLine.data!=null)
			{
			if(currentLine.data.Length > 0)
				{
				currentLine.horizontalAllignment = rtCapture.SelectionAlignment;
				text += currentLine;
				}
			}
		
		//build the font table
		text.fontTable = new FontFamily[fontTable.Count];
		for(i=0;i<fontTable.Count;i++)
			{
			text.fontTable[i] = (FontFamily)fontTable[i];
			}
		
		return text;
		}
	}
}
