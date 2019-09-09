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
	/// checkerPattern Generator (stub)
	/// </summary>
	[Serializable]
	public class CheckerPattern : pattern
		{
		#region data
		#endregion
		
		/// <summary>
		/// Creates a new checkerPattern Generator
		/// </summary>
		public CheckerPattern()
			{
			}
		
		/// <summary>
		/// Applies the pattern described in this object to the specified bitmap.
		/// </summary>
		/// <param name="b">A bitmap to apply the pattern to.</param>
		/// <remarks>For speed critical operation use this function with a preallocated bitmap rather than call makeBitmap. <see cref="pattern.makeBitmap"/></remarks>
		public override void applyPattern(ref Bitmap b)
			{
			}
		
		
		/// <summary>
		/// Makes a clone of this object.
		/// </summary>
		/// <returns>A clone of this object.</returns>
		public override pattern clone()
			{
			CheckerPattern p = new CheckerPattern();
			return (pattern)p;
			}
		}
}
