/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections.Specialized;


namespace WD_toolbox.Maths.Space
{
    [Flags]
	public enum Dir2D {None=0, Up=1, Down=2, Left=4, Right=8};
	[Flags]
	public enum Octants2D { None=0, Up=1, Down=2, Left=4, Right=8,
							topLeft=16, topRight=32, bottomLeft=64, bottomRight=128,
							all=Up|Down|Left|Right|topLeft|topRight|bottomLeft|bottomRight,
                            orthognal = Up | Down | Left | Right,
                            diagonals = topLeft | topRight | bottomLeft | bottomRight
    };

public class OrthoDirection
	{
	
	
	protected static readonly Point[] Octants2DOffsetVecs = new Point[] {new Point(0,0), //none
																		new Point(0,1),  //top
																		new Point(0,-1), //down
																		new Point(-1,0), //left
																		new Point(1,0),  //right
																		new Point(-1,1), //top left
																		new Point(1,1),  //top right
																		new Point(-1,-1),  //bottom left
																		new Point(1,-1)   //botom right
																		};

	/// <summary>
	/// Gets a set of 2d offsets as specified by the direction.
	/// The magnitude of the offsets is NOT always one.
	/// NOTE: Top right is desginated (+1, +1)
	/// </summary>
	/// <param name="dir">The dir flag.</param>
	/// <returns>A list of points</returns>
	public static Point[] getOffsets(Octants2D dir)
		{
		List<Point> offsets = new List<Point>();
		BitVector32 bv = new BitVector32((int)dir);
		int i;
		for(i=1;i<Octants2DOffsetVecs.Length;i++)
			{
			if(bv[1<<(i-1)])
				{
				offsets.Add(Octants2DOffsetVecs[i]);
				}
			}
		return offsets.ToArray();
		}
	
	}
}
