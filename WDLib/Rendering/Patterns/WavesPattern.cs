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
	/// A wave pattern generator.
	/// </summary>
	[DefaultPropertyAttribute("waveLenth")]
	[Serializable]
	public class WavesPattern : pattern
		{
		#region data
		private int _waveLenth=10;
		/// <summary>
		/// Size of wave
		/// </summary>
		[CategoryAttribute("Pattern"), 
		DescriptionAttribute("")]
		public int waveLenth
			{
			get
				{
				return _waveLenth;
				}
			set
				{
				_waveLenth = Range.clamp(value, 1, 100);
				}
			}
		
		
		private Color _col1 = Color.Yellow;
		/// <summary>
		/// A color for the first wave
		/// </summary>
		[CategoryAttribute("Colours"), 
		DescriptionAttribute("")]
		public Color col1
			{
			get
				{
				return _col1;
				}
			set
				{
				_col1 = value;
				}
			}
			
		
		private Color _col2 = Color.Magenta;
		/// <summary>
		/// A color for the second wave
		/// </summary>
		[CategoryAttribute("Colours"), 
		DescriptionAttribute("")]
		public Color col2
			{
			get
				{
				return _col2;
				}
			set
				{
				_col2 = value;
				}
			}
		
		#endregion
		
		/// <summary>
		/// Creates a new 
		/// </summary>
		public WavesPattern()
			{
			}
		
		/// <summary>
		/// Applies the pattern described in this object to the specified bitmap.
		/// </summary>
		/// <param name="b">A bitmap to apply the pattern to.</param>
		/// <remarks>For speed critical operation use this function with a preallocated bitmap rather than call makeBitmap. <see cref="pattern.makeBitmap"/></remarks>
		public override void applyPattern(ref Bitmap b)
			{
			//int i,j;
			float centreX=0.5f, centreY=0.5f;
			float[] sinLut =new float[256] {0.000000f, 0.012320f, 0.024637f, 0.036951f, 0.049260f, 0.061561f, 0.073853f, 0.086133f, 0.098400f, 0.110653f, 0.122888f, 0.135105f, 0.147302f, 0.159476f, 0.171626f, 0.183750f, 0.195845f, 0.207912f, 0.219946f, 0.231948f, 0.243914f, 0.255843f, 0.267733f, 0.279583f, 0.291390f, 0.303153f, 0.314870f, 0.326539f, 0.338158f, 0.349727f, 0.361242f, 0.372702f, 0.384106f, 0.395451f, 0.406737f, 0.417960f, 0.429121f, 0.440216f, 0.451244f, 0.462204f, 0.473094f, 0.483911f, 0.494656f, 0.505325f, 0.515918f, 
											0.526432f, 0.536867f, 0.547220f, 0.557489f, 0.567675f, 0.577774f, 0.587785f, 0.597707f, 0.607539f, 0.617278f, 0.626924f, 0.636474f, 0.645928f, 0.655284f, 0.664540f, 0.673696f, 0.682749f, 0.691698f, 0.700543f, 0.709281f, 0.717912f, 0.726434f, 0.734845f, 0.743145f, 0.751332f, 0.759405f, 0.767363f, 0.775204f, 0.782928f, 0.790532f, 0.798017f, 0.805381f, 0.812622f, 0.819740f, 0.826734f, 0.833602f, 0.840344f, 0.846958f, 0.853444f, 0.859800f, 0.866025f, 0.872120f, 0.878081f, 0.883910f, 0.889604f, 
											0.895163f, 0.900587f, 0.905873f, 0.911023f, 0.916034f, 0.920906f, 0.925638f, 0.930229f, 0.934680f, 0.938988f, 0.943154f, 0.947177f, 0.951057f, 0.954791f, 0.958381f, 0.961826f, 0.965124f, 0.968276f, 0.971281f, 0.974139f, 0.976848f, 0.979410f, 0.981823f, 0.984086f, 0.986201f, 0.988165f, 0.989980f, 0.991645f, 0.993159f, 0.994522f, 0.995734f, 0.996795f, 0.997705f, 0.998464f, 0.999070f, 0.999526f, 0.999829f, 0.999981f, 0.999981f, 0.999829f, 0.999526f, 0.999070f, 0.998464f, 0.997705f, 0.996795f, 
											0.995734f, 0.994522f, 0.993159f, 0.991645f, 0.989980f, 0.988165f, 0.986201f, 0.984086f, 0.981823f, 0.979410f, 0.976848f, 0.974139f, 0.971281f, 0.968276f, 0.965124f, 0.961826f, 0.958381f, 0.954791f, 0.951057f, 0.947177f, 0.943154f, 0.938988f, 0.934680f, 0.930229f, 0.925638f, 0.920906f, 0.916034f, 0.911023f, 0.905873f, 0.900587f, 0.895163f, 0.889604f, 0.883910f, 0.878081f, 0.872120f, 0.866025f, 0.859800f, 0.853444f, 0.846958f, 0.840344f, 0.833602f, 0.826734f, 0.819740f, 0.812622f, 0.805381f, 
											0.798017f, 0.790532f, 0.782928f, 0.775204f, 0.767363f, 0.759405f, 0.751332f, 0.743145f, 0.734845f, 0.726434f, 0.717912f, 0.709281f, 0.700543f, 0.691698f, 0.682749f, 0.673696f, 0.664540f, 0.655284f, 0.645928f, 0.636474f, 0.626924f, 0.617278f, 0.607539f, 0.597707f, 0.587785f, 0.577774f, 0.567675f, 0.557489f, 0.547220f, 0.536867f, 0.526432f, 0.515918f, 0.505325f, 0.494656f, 0.483911f, 0.473094f, 0.462204f, 0.451244f, 0.440216f, 0.429121f, 0.417960f, 0.406737f, 0.395451f, 0.384106f, 0.372702f, 
											0.361242f, 0.349727f, 0.338158f, 0.326539f, 0.314870f, 0.303153f, 0.291390f, 0.279583f, 0.267733f, 0.255843f, 0.243914f, 0.231948f, 0.219946f, 0.207912f, 0.195845f, 0.183750f, 0.171626f, 0.159476f, 0.147302f, 0.135105f, 0.122888f, 0.110653f, 0.098400f, 0.086133f, 0.073853f, 0.061561f, 0.049260f, 0.036951f, 0.024637f, 0.012320f, 0.000000f};

			
			float perX=0.0f, perY=0.0f;
			float stepX=1.0f/(float)b.Width, stepY=1.0f/(float)b.Height;
			QColor col = Color.Blue;
			QColor qcol1=(QColor)col1, qcol2=(QColor)col2;
			
			float endLineValue = 1.0f-(stepX*0.9f);
			
			BitmapData bd = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			unsafe
				{
				QColor* pixelPtr = (QColor*)bd.Scan0.ToPointer();
				
				float d;
				QColor* end = pixelPtr+(bd.Width*bd.Height);
				
				for(;pixelPtr<end;pixelPtr++)
					{
					d = dist(perX, perY, centreX, centreY);
					col = mix(col1, col2, sinLut[(byte)(d*255*(waveLenth))]);
					*pixelPtr = *(&col);
					perX += stepX;
					if(perX>endLineValue)
						{
						perX=0.0f;
						perY += stepY;
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
			WavesPattern p = new WavesPattern();
			p.col1 = col1;
			p.col2 = col2;
			p.waveLenth = waveLenth;
			return (pattern)p;
			}
		}
}
