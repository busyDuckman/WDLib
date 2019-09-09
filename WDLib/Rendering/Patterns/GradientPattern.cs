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

using WD_toolbox.Rendering.Colour;

namespace WD_toolbox.Rendering.Patterns
{
	/// <summary>
	/// Generates a gradient pattern.
	/// </summary>
	[DefaultPropertyAttribute("topLeft")]
	[Serializable]
	public class GradientPattern : pattern
		{
		#region data
		private Color _topLeft;
		/// <summary>
		/// Top Left Colour
		/// </summary>
		[CategoryAttribute("Colours"), 
		DescriptionAttribute("Top Left Colour")]
		public Color topLeft
			{
			get
				{
				return _topLeft;
				}
			set
				{
				_topLeft = value;
				}
			}
			
			
		private Color _topRight;
		/// <summary>
		/// Top Right Colour
		/// </summary>
		[CategoryAttribute("Colours"), 
		DescriptionAttribute("Top Right Colour")]
		public Color topRight
			{
			get
				{
				return _topRight;
				}
			set
				{
				_topRight = value;
				}
			}
			
			
		private Color _bottomLeft;
		/// <summary>
		/// Lower Left Colour
		/// </summary>
		[CategoryAttribute("Colours"), 
		DescriptionAttribute("Lower Left Colour")]
		public Color bottomLeft
			{
			get
				{
				return _bottomLeft;
				}
			set
				{
				_bottomLeft = value;
				}
			}
			
			
		private Color _bottomRight;
		/// <summary>
		/// Lower Right Colour
		/// </summary>
		[CategoryAttribute("Colours"), 
		DescriptionAttribute("Lower Right Colour")]
		public Color bottomRight
			{
			get
				{
				return _bottomRight;
				}
			set
				{
				_bottomRight = value;
				}
			}
		#endregion
		
		/// <summary>
		/// Creates a new 
		/// </summary>
		public GradientPattern()
			{
			topLeft = Color.Blue;
			topRight = Color.Blue;
			bottomLeft = Color.White;
			bottomRight = Color.White;
			}
		
		/// <summary>
		/// Applies the pattern described in this object to the specified bitmap.
		/// </summary>
		/// <param name="b">A bitmap to apply the pattern to.</param>
		/// <remarks>For speed critical operation use this function with a preallocated bitmap rather than call makeBitmap. <see cref="pattern.makeBitmap"/></remarks>
		public override void applyPattern(ref Bitmap b)
			{
			float perX=0.0f, perY=0.0f;
			float stepX=1.0f/(float)b.Width, stepY=1.0f/(float)b.Height;
			QColor col = Color.Blue;
			QColor qcol1=(QColor)topLeft, qcol2=(QColor)topRight, qcol3=(QColor)bottomLeft, qcol4=(QColor)bottomRight;
			
			BitmapData bd = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			
			///added i and j to fix a bug
			///int i=0,j=0;
			
			unsafe
				{
				QColor* pixelPtr = (QColor*)bd.Scan0.ToPointer();
				
				QColor t1=mix(qcol1, qcol3, perY), t2=mix(qcol2, qcol4, perY);
				QColor* end = pixelPtr+(bd.Width*bd.Height);
				
				float endLineValue = 1.0f-(stepX*0.9f);
								
				for(;pixelPtr<end;pixelPtr++)
					{
					//col = mix(mix(qcol1, qcol2, perX), mix(qcol3, qcol4, perX), perY);
					///perX = stepX * i++;
					col = mix(t1, t2, perX);
					*pixelPtr = *(&col);
					
					perX += stepX;
					
					
					
					if(perX > endLineValue)//stupid float comparison
					///if(i>=bd.Width)
						{
						///i=0;
						perX=0.0f;
						//fast but fp accumulation problem
						perY += stepY;
						
						///perY = stepY * j++;
						
						t1=mix(qcol1, qcol3, perY);
						t2=mix(qcol2, qcol4, perY);
						}
					}
				}
			b.UnlockBits(bd);
			}
			
		/// <summary>
		/// Makes a clone of this object.
		/// </summary>
		/// <returns>A clone of this object.</returns>
		public override pattern clone()
			{
			GradientPattern p = new GradientPattern();
			p.bottomLeft = bottomLeft;
			p.bottomRight = bottomRight;
			p.topLeft = topLeft;
			p.topRight = topRight;
			return (pattern)p;
			}
		}
}
