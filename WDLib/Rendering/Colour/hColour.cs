/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Text;
using WD_toolbox.Maths.Range;

namespace WD_toolbox.Rendering.Colour
{
/// <summary>
/// High colour class
/// </summary>
public struct hColor : IColour<hColor>
	{
	public float r;
    public float g;
    public float b;
    public float a;
    
    #region static from* methods
	/// <summary>
	/// Creates a new qColour
	/// </summary>
	/// <param name="red">0-255</param>
	/// <param name="green">0-255</param>
	/// <param name="blue">0-255</param>
	/// <returns></returns>
    public static hColor fromRGB(int red, int green, int blue)
		{
        return (hColor)QColor.fromRGB(red, green, blue);
		}
		
    /// <summary>
    /// Creates a new qColour
    /// </summary>
    /// <param name="red">0-255</param>
    /// <param name="green">0-255</param>
    /// <param name="blue">0-255</param>
    /// <returns></returns>
    public static hColor fromRGB(float red, float green, float blue)
        {
        hColor c;
        c.b = blue;
        c.g = green;
        c.r = red;
        c.a = 255;
        return c;
        }
        
	/// <summary>
	/// Creates a new qColour
	/// </summary>
	/// <param name="red">0-255</param>
	/// <param name="green">0-255</param>
	/// <param name="blue">0-255</param>
	/// <param name="alpha">0-255 (0=transparent, 255=solid)</param>
	/// <returns></returns>
    public static hColor fromRGBA(int red, int green, int blue, int alpha)
		{
        return (hColor)QColor.fromRGBA(red, green, blue, alpha);
		}

    /// <summary>
    /// Creates a new qColour
    /// </summary>
    /// <param name="red">0-255</param>
    /// <param name="green">0-255</param>
    /// <param name="blue">0-255</param>
    /// <param name="alpha">0-255 (0=transparent, 255=solid)</param>
    /// <returns></returns>
    public static hColor fromRGBA(float red, float green, float blue, float alpha)
        {
        hColor c;
        c.b = blue;
        c.g = green;
        c.r = red;
        c.a = alpha;
        return c;
        }	
	
	/// <summary>
	/// Creates a new qColour
	/// </summary>
	/// <param name="H">Hue</param>
	/// <param name="S">Saturation</param>
	/// <param name="V">Value</param>
	/// <returns></returns>
    public static hColor fromHSV(float H, float S, float V)
		{
		return fromHSVA(H, S, V, 255);
		}
	/// <summary>
	/// Creates a new qColour
	/// </summary>
	/// <param name="H">Hue</param>
	/// <param name="S">Saturation</param>
	/// <param name="V">Value</param>
	/// <param name="alpha">0-255 (0=transparent, 255=solid)</param>
	/// <returns></returns>
    public static hColor fromHSVA(float H, float S, float V, byte alpha)
		{
        hColor c;
		c.a = alpha;
		if ( S == 0 )                       //HSV values = From 0 to 1
			{
			c.r = V;                      //RGB results = From 0 to 255
			c.g = V;
			c.b = V;
			}
		else
			{
			float var_h = H * 6;
			float var_i = (int)var_h;             //Or ... var_i = floor( var_h )
			float var_1 = V * ( 1 - S );
			float var_2 = V * ( 1 - S * ( var_h - var_i ) );
			float var_3 = V * ( 1 - S * ( 1 - ( var_h - var_i ) ) );
			
			float var_r = V, var_g = var_1, var_b = var_2;
			if      ( var_i == 0 ) { var_r = V     ; var_g = var_3 ; var_b = var_1; }
			else if ( var_i == 1 ) { var_r = var_2 ; var_g = V     ; var_b = var_1; }
			else if ( var_i == 2 ) { var_r = var_1 ; var_g = V     ; var_b = var_3; }
			else if ( var_i == 3 ) { var_r = var_1 ; var_g = var_2 ; var_b = V;     }
			else if ( var_i == 4 ) { var_r = var_3 ; var_g = var_1 ; var_b = V;     }
			//else                   { var_r = V     ; var_g = var_1 ; var_b = var_2; }

			c.r = var_r;                  //RGB results = From 0 to 255
			c.g = var_g;
			c.b = var_b;
			}
		return c;
		}
	
	/// <summary>
	/// Creates a colour from a string, Returns black when an error occurs
	/// String can be int the form (i)(i a)(r g b)(r g b a) OR a prenamed color string 
	/// </summary>
	/// <param name="s">String to parse</param>
	/// <returns></returns>
    public static hColor fromString(string s)
		{
		try
			{
			string[] elements = s.Trim().Replace("  ", " ").Split(" ".ToCharArray());
			float c;
			if(s.IndexOfAny(".".ToCharArray()) != -1)
				{
				//floating point information string
				switch(elements.Length)
					{
					case 0:
                        return hColor.fromRGB(0, 0, 0);
					case 1:
					    c = float.Parse(elements[0]);
                        return hColor.fromRGB(c, c, c);
					case 2:
						//assume c c c a (intensity + opacity)
                        c = float.Parse(elements[0]);
                        return hColor.fromRGBA(c, c, c, float.Parse(elements[1]));
					case 3:
                        return hColor.fromRGB(float.Parse(elements[0]), float.Parse(elements[1]), float.Parse(elements[2]));
					default:
						//four or more
                        return hColor.fromRGBA(float.Parse(elements[0]), float.Parse(elements[1]), float.Parse(elements[2]), float.Parse(elements[2]));
					}
				}
			else
				{
				//integer(0-255) or named string
				QColor col = QColor.fromString(s);
				return (hColor)col;
				}
			}
		catch
			{
			return QColor.fromRGB(0,0,0);
			}
		}
	
	#endregion
    
	#region IColour<hColour> Members

	public float mesureColorantContrast(hColor b)
		{
		throw new Exception("The method or operation is not implemented.");
		}

	public QColor mix(hColor a, float per)
		{
		throw new Exception("The method or operation is not implemented.");
		}

	public QColor rgbMix(hColor a, float per)
		{
		throw new Exception("The method or operation is not implemented.");
		}

	public void toHSV(out float H, out float S, out float V)
		{
		throw new Exception("The method or operation is not implemented.");
		}

	public string toName()
		{
		throw new Exception("The method or operation is not implemented.");
		}

	#endregion

    #region operators
    /// <summary>
    /// Cast to a windows colour
    /// </summary>
    /// <param name="c">a hColor</param>
    /// <returns>a windows colour</returns>
    public static unsafe explicit operator System.Drawing.Color(hColor c)
        {
        //FIX:
        return System.Drawing.Color.FromArgb(*((int*)&c.b));
        //return Color.FromArgb(c.a, c.r, c.g, c.b);
        }

    
    /// <summary>
    /// Cast from a windows colour
    /// </summary>
    /// <param name="c">a windows colour</param>
    /// <returns>a hColor</returns>
    /*public static implicit operator hColor(System.Drawing.Color c)
        {
        QColor r;
        r.a = c.A;
        r.r = c.R;
        r.g = c.G;
        r.b = c.B;
        return r;
        }
        */

    /// <summary>
    /// Cast to a qColor (will underflow with negative colourants)
    /// </summary>
    /// <param name="c">a hColor</param>
    /// <returns>a qColor</returns>
    public static unsafe explicit operator QColor(hColor c)
        {
        return QColor.fromRGBA((int)c.r*255, (int)c.g*255, (int)c.b*255, (int)c.a*255);
        }


    /// <summary>
    /// Cast from a qColor
    /// </summary>
    /// <param name="c">a qColor</param>
    /// <returns>a hColor</returns>
    public static implicit operator hColor(QColor c)
        {
        const float byteTofloat = (1.0f/255.0f);
        return hColor.fromRGBA(c.r*byteTofloat, c.g*byteTofloat, c.b*byteTofloat, c.a*byteTofloat);
        }
    #endregion
    
    private unsafe void _clamp()
        {
        unsafe
            {
            fixed(float *pos = &this.r)
                {
                Range.clamp(pos, 4, 0.0f, 1.0f);
                }
            }
        }
    
    private unsafe void _clampRGBOnly()
        {
        unsafe
            {
            fixed (float* pos = &this.r)
                {
                Range.clamp(pos, 4, 0.0f, 1.0f);
                }
            }
        }
    
    /*public void MixRGB_TO_RGBA(ref hColor col)
        {
        float aInv = 1.0f/a;
        col.r += r * aInv;
        col.g += g * aInv;
        col.b += b * aInv;
        col._clampRGBOnly();
        }*/

    public void MixRGBWithRGBA(hColor col)
        {
        float aInv = 1.0f / col.a;
        r = r * aInv;
        g = g * aInv;
        b = b * aInv;
        _clampRGBOnly();
        }
        
	}
}
