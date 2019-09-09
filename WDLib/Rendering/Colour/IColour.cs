/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace WD_toolbox.Rendering.Colour
{
	interface IColour<colType> where colType : IColour<colType>
	{
		float mesureColorantContrast(colType b);
		QColor mix(colType a, float per);
		QColor rgbMix(colType a, float per);
		void toHSV(out float H, out float S, out float V);
		string toName();
		string ToString();
	}
}
