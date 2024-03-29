/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;

namespace WD_toolbox.Maths.Trigonometry
{
/// <summary>
/// Helper Math functions
/// </summary>
public class Trig
	{
	static float[] sinLut4 = {};
	static float[] cosLut4 = {};
	//static float[] tanLut4 = {};
	
	/// <summary>
	/// A conversion constant.
	/// deg = rad * rad2Deg;
	/// </summary>
	public static readonly double Rad2Deg = (float) (180.0f / System.Math.PI);
	
	/// <summary>
	/// A conversion constant.
	/// rad = deg * rad2Deg;
	/// </summary>
    public static readonly double Deg2Rad = (float)(System.Math.PI / 180.0f);
	
	/// <summary>
	/// Inits the System.Math class.
	/// </summary>
	static Trig()
		{
		int division = 4;
		sinLut4 = new float[360*division];
		cosLut4 = new float[360*division];
		
		int i;
		float deg;
		
		for(i=0; i<sinLut4.Length; i++)
			{
			deg = (float)i / (float)division;;
			sinLut4[i] = (float)System.Math.Sin(deg * Deg2Rad);
			cosLut4[i] = (float)System.Math.Cos(deg * Deg2Rad);
			}
		}
	
	/// <summary>
	/// sin acurate to a forth of a degree
	/// </summary>
	/// <param name="v">value to find sin of.</param>
	/// <returns>Approcimate sin of v.</returns>
	public static float degSinQuick(float v)
		{
		int  V = (int)v;
		while(V<0)
			v += 360.0f;
		while(V>360)
			v -= 360.0f;
		
		// divide v by for
		V = V >> 2;
		return sinLut4[V];
		}
	
	/// <summary>
	/// cos acurate to a forth of a degree
	/// </summary>
	/// <param name="v">value to find cos of.</param>
	/// <returns>Approcimate cos of v.</returns>
	public static float degCosQuick(float v)
		{
		int  V = (int)v;
		while(V<0)
			v += 360.0f;
		while(V>360)
			v -= 360.0f;
		
		// divide v by for
		V = V >> 2;
		return cosLut4[V];
		}
	
	/// <summary>
	/// A degres vesion of the relating geomertical function.
	/// </summary>
	/// <param name="v">A value with will undergo geometric transform.</param>
	/// <returns>Result of the geometric transform.</returns>
	public static float degTan(float v)
		{
		return (float)System.Math.Tan(v*Deg2Rad);
		}
	
	/// <summary>
	/// A degres vesion of the relating geomertical function.
	/// </summary>
	/// <param name="v">A value with will undergo geometric transform.</param>
	/// <returns>Result of the geometric transform.</returns>
	public static float degATan(float v)
		{
		return (float)(System.Math.Atan(v)*Rad2Deg);
		}
	
	/// <summary>
	/// A degres vesion of the relating geomertical function.
	/// </summary>
	/// <param name="v">A value with will undergo geometric transform.</param>
	/// <returns>Result of the geometric transform.</returns>
	public static float degCos(float v)
		{
		return (float)System.Math.Cos(v*Deg2Rad);
		}
	
	/// <summary>
	/// A degres vesion of the relating geomertical function.
	/// </summary>
	/// <param name="v">A value with will undergo geometric transform.</param>
	/// <returns>Result of the geometric transform.</returns>
	public static float degACos(float v)
		{
		return (float)(System.Math.Acos(v)*Rad2Deg);
		}
	
	/// <summary>
	/// A degres vesion of the relating geomertical function.
	/// </summary>
	/// <param name="v">A value with will undergo geometric transform.</param>
	/// <returns>Result of the geometric transform.</returns>
	public static float degSin(float v)
		{
		return (float)System.Math.Sin(v*Deg2Rad);
		}
	
	/// <summary>
	/// A degres vesion of the relating geomertical function.
	/// </summary>
	/// <param name="v">A value with will undergo geometric transform.</param>
	/// <returns>Result of the geometric transform.</returns>
	public static float degASin(float v)
		{
		return (float)(System.Math.Asin(v)*Rad2Deg);
		}
	
	/// <summary>
	/// Performs a calculation using pyhagorises theorem
	/// </summary>
	/// <param name="a">A lenght</param>
	/// <param name="b">A lenght</param>
	/// <returns>Lenght of the long sid of a right triange.</returns>
    public static float pythag(float a, float b)
		{
		return (float)System.Math.Sqrt(a*a+b*b);
		}
	
	/// <summary>
	/// Performs a calculation using pyhagorises theorem
	/// </summary>
	/// <param name="a">A lenght</param>
	/// <param name="b">A lenght</param>
	/// <returns>Lenght of the long sid of a right triange.</returns>
    public static double pythag(double a, double b)
		{
		return System.Math.Sqrt(a*a+b*b);
		}

    /// <summary>
    /// Returns an angle as 0 - 359 deg
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static int NormaliseDegAngle(int angle)
    {
        angle = angle % 360;
        return (angle < 0) ? (angle+360):angle;
    }

    public static double NormaliseDegAngle(double angle)
    {
        return (angle - (Math.Floor(angle / 360.0) * 360.0));
    }

	
	}
}
