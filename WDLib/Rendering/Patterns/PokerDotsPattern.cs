/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.ComponentModel;

using WD_toolbox.Maths.Range;
using WD_toolbox.Rendering.Colour;

namespace WD_toolbox.Rendering.Patterns
{
	/// <summary>
	/// A poker Dots Pattern generator.
	/// </summary>
	[DefaultPropertyAttribute("dotColor")]
	[Serializable]
	public class PokerDotsPattern : pattern
		{
		#region data
		private Color _dotColor = Color.Red;
		/// <summary>
		/// Dot color.
		/// </summary>
		[CategoryAttribute("Colours"), 
		DescriptionAttribute("Dot colour")]
		public Color dotColor
			{
			get
				{
				return _dotColor;
				}
			set
				{
				_dotColor = value;
				}
			}
		
		
		private Color _backGroundColor = Color.White;
		/// <summary>
		/// Background color.
		/// </summary>
		[CategoryAttribute("Colours"), 
		DescriptionAttribute("Background colour")]
		public Color backGroundColor
			{
			get
				{
				return _backGroundColor;
				}
			set
				{
				_backGroundColor = value;
				}
			}
			
		
		private int _numberOfDots = 100;
		/// <summary>
		/// Number of dots.
		/// </summary>
		[CategoryAttribute("Pattern"), 
		DescriptionAttribute("Number of dots")]
		public int numberOfDots
			{
			get
				{
				return _numberOfDots;
				}
			set
				{
				_numberOfDots =Range.clamp(value, 1, 1000);
				}
			}
			
		
		private int _dotSize = 32;
		/// <summary>
		/// Size of dots.
		/// </summary>
		[CategoryAttribute("Pattern"), 
		DescriptionAttribute("Size of the dots")]
		public int dotSize
			{
			get
				{
				return _dotSize;
				}
			set
				{
				_dotSize = Range.clamp(value, 1, 64);
				}
			}
			
		
		private bool _allowOverlapping = false;
		/// <summary>
		/// Determines whether dots can overlap.
		/// </summary>
		[CategoryAttribute("Pattern"), 
		DescriptionAttribute("Allow overlapping dots.")]
		public bool allowOverlapping
			{
			get
				{
				return _allowOverlapping;
				}
			set
				{
				_allowOverlapping = value;
				}
			}
		
		
		private bool _useGradiant=true;
		/// <summary>
		/// Controls whether dots have a soft edge.
		/// </summary>
		[CategoryAttribute("Pattern"), 
		DescriptionAttribute("Use soft edge dots.")]
		public bool useGradiant
			{
			get
				{
				return _useGradiant;
				}
			set
				{
				_useGradiant = value;
				}
			}
		#endregion
		
		/// <summary>
		/// Creates a new 
		/// </summary>
		public PokerDotsPattern()
			{
			}
		
		/// <summary>
		/// Applies the pattern described in this object to the specified bitmap.
		/// </summary>
		/// <param name="b">A bitmap to apply the pattern to.</param>
		/// <remarks>For speed critical operation use this function with a preallocated bitmap rather than call makeBitmap. <see cref="pattern.makeBitmap"/></remarks>
		public override void applyPattern(ref Bitmap b)
			{
			int i,j;
			//qColor col;
			//float dotSize = b.Width * 0.1f * percent2;
			float dotRadius = dotSize/2.0f;
			
			//numDots is alter to how many dots actuly fit on the page
			int numDots = numberOfDots;//(int)(percent1 * 100.0f);
			
			int[,] positions = new int[2, numDots];
			int escapeValue = 100;
			int escape=escapeValue;
			int x, y;
			Random r = new Random(9999991); //9999991 is large prime number
			
			#region place dots
			//place dots
			for(i=0;i<numDots;i++)
				{
				x = (int)((float)r.NextDouble()*b.Width);
				y = (int)((float)r.NextDouble()*b.Height);
				positions[0, i] = x;
				positions[1, i] = y;
				
				if(!allowOverlapping)
					{
					//check not overlapping any previous dots
					for(j=0;j<i;j++)
						{
						if(dist(x, y, positions[0, j], positions[1, j]) < (dotSize+3))
							{
							//problem
							i--; //decrement i so as to repeat;
							escape--;
							break;
							}
						}
					
					if(i>0)
						if(j == i)
							escape = escapeValue; //reached end of loop so succsess
					}
				if(escape==0)
					break;
				
				}
			#endregion
			
			if(i<numDots)
				{
				//failed to place all dots
				numDots = i;
				}
			
			Graphics g = Graphics.FromImage(b);
			g.Clear(backGroundColor);
			//place dots
			Bitmap dotBitmap = new Bitmap(dotSize, dotSize);
			Brush dotBrush = new SolidBrush(dotColor);
			if(useGradiant)
				{
				Color c;
				float opacity;
				//Graphics gDot = Graphics.FromImage(b);
				//gDot.Clear(Color.Transparent);
				//int i, j;
				for(i=0;i<dotSize;i++)
					{
					for(j=0;j<dotSize;j++)
						{
						//distacne in radius normal cordinates
						opacity = dist(i,j,dotRadius,dotRadius) / dotRadius;
						
						//alter the distobution of opacity to be more eye frendly
						opacity = 1.0f - (opacity*opacity);
						
						//set the color
						c = Color.FromArgb(Range.clamp((int)(opacity*255.0f),0,255), dotColor);
						dotBitmap.SetPixel(i, j, c);
						}
					}
				//gDot.Dispose();
				}
			
			float xPos, yPos;
			//must iterate to numdots as it may be smaller than the array
			for(i=0;i<numDots;i++)
				{
				xPos = positions[0, i] - dotRadius;
				yPos = positions[1, i] - dotRadius;
				if(useGradiant)
					{
					g.DrawImageUnscaled(dotBitmap, (int)xPos, (int)yPos);
					}
				else
					{
					g.FillEllipse(dotBrush, xPos, yPos, dotSize, dotSize);
					}
				}
			
			g.Dispose();
			}
		
		
		/// <summary>
		/// Makes a clone of this object.
		/// </summary>
		/// <returns>A clone of this object.</returns>
		public override pattern clone()
			{
			PokerDotsPattern p = new PokerDotsPattern();
			p.allowOverlapping = allowOverlapping;
			p.backGroundColor = backGroundColor;
			p.dotColor = dotColor;
			p.dotSize = dotSize;
			p.numberOfDots = numberOfDots;
			p.useGradiant = useGradiant;
			return (pattern)p;
			}
		}
}
