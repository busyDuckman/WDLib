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
using System.Data;

using WD_toolbox.Rendering.Colour;

namespace WD_toolbox.Rendering.Patterns
{
	/// <summary>
	/// A class for generating patterns.
	/// </summary>
	[Serializable]
	public abstract class pattern
		{
		/// <summary>
		/// Creates a new
		/// </summary>
		public pattern()
			{
			}
		
		/// <summary>
		/// Creates a bitmap with the pattern described in this object.
		/// </summary>
		/// <param name="x">Width of the new bitmap</param>
		/// <param name="y">Height of the new Bitmap</param>
		/// <returns>A new bitmap with the pattern described in this object.</returns>
		public Bitmap makeBitmap(int x, int y)
			{
			Bitmap b = new Bitmap(x, y);
			applyPattern(ref b);
			return b;
			}
		
		/// <summary>
		/// Applies the pattern described in this object to the specified bitmap.
		/// </summary>
		/// <param name="b">A bitmap to apply the pattern to.</param>
		/// <remarks>For speed critical operation use this function with a preallocated bitmap rather than call makeBitmap. <see cref="pattern.makeBitmap"/></remarks>
		public abstract void applyPattern(ref Bitmap b);
		
		#region helper functions
		/// <summary>
		/// Mixes two colours together.
		/// </summary>
		/// <param name="a">A colour to be mixed.</param>
		/// <param name="b">A colour to be mixed.</param>
		/// <param name="per">Amount of b to add to a. 0 = 0%, 1.0 = 100%</param>
		/// <returns>A new colour, the result of mixing two colour together.</returns>
		protected static Color mix(Color a, Color b, float per)
			{
			//float bPer = 1.0f-per;
			//float red = ((float)a.R * per) + ((float)b.R * bPer);
			//float green = ((float)a.G * per) + ((float)b.G * bPer);
			//float blue = ((float)a.B * per) + ((float)b.B * bPer);
			//return Color.FromArgb((int)red, (int)green, (int)blue);
			
			byte red = (byte)(a.R + (int)((b.R - a.R)* per));
			byte green = (byte)(a.G + (int)((b.G - a.G)* per));
			byte blue = (byte)(a.B + (int)((b.B - a.B)* per));
			byte alpha = (byte)(a.A + (int)((b.A - a.A)* per));
			return Color.FromArgb(alpha, red, green, blue);
			}
		
		/// <summary>
		/// Mixes two colours together.
		/// </summary>
		/// <param name="a">A colour to be mixed.</param>
		/// <param name="b">A colour to be mixed.</param>
		/// <param name="per">Amount of b to add to a. 0 = 0%, 1.0 = 100%</param>
		/// <returns>A new colour, the result of mixing two colour together.</returns>
		protected static QColor mix(QColor a, QColor b, float per)
			{
			QColor c = new QColor();
			c.a = (byte)(a.a + (byte)((b.a - a.a)* per));
			c.r = (byte)(a.r + (byte)((b.r - a.r)* per));
			c.g = (byte)(a.g + (byte)((b.g - a.g)* per));
			c.b = (byte)(a.b + (byte)((b.b - a.b)* per));
			return c;
			}
		
		/// <summary>
		/// Distance from point (x,y) to point (x2,y2).
		/// </summary>
		/// <param name="x">x co-ordinat of the first point.</param>
		/// <param name="y">y co-ordinat of the first point.</param>
		/// <param name="x2">x co-ordinat of the second point.</param>
		/// <param name="y2">y co-ordinat of the second point.</param>
		/// <returns>Distance from point (x,y) to point (x2,y2).</returns>
		protected static float dist(float x, float y, float x2, float y2)
			{
			float xd = x2-x;
			float yd = y2-y;
			xd *= xd;
			yd *= yd;
			return (float)System.Math.Sqrt(xd+yd);
			}
		#endregion
		
		/// <summary>
		/// Makes a clone of this object.
		/// </summary>
		/// <returns>A clone of this object.</returns>
		public abstract pattern clone();
		
		}
}
