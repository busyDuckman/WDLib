/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDMedia.Game
{
    public class ScreenTileManager<TILEDATA> : ScreenTileHelper
        where TILEDATA : new()
    {
        public TILEDATA[,] tiles;

        public delegate void renderTileDelegate(Graphics g, Rectangle Pos, TILEDATA data, Point tilePos);

        public ScreenTileManager(Size boardSize, Size screenSize, SizeF gapRatio, int marginLeft, int marginRight, int marginTop, int marginBottom)
            : base(boardSize, screenSize, gapRatio, marginLeft, marginRight, marginTop, marginBottom)
        {
            tiles = new TILEDATA[boardSize.Width, boardSize.Height];
            tiles.Initialize();
            for (int h = 0; h < boardSize.Height; h++)
            {
                for (int i = 0; i < boardSize.Width; i++)
                {
                    tiles[i, h] = new TILEDATA();
                }
            }
        }

        public TILEDATA this[Point pos]
        {
            get
            {
                return tiles[pos.X, pos.Y];
            }

            set
            {
                tiles[pos.X, pos.Y] = value;
            }
        }

        public void renderGDI(Graphics g, renderTileDelegate renderTile)
        {
            for (int h = 0; h < boardSize.Height; h++)
            {
                for (int i = 0; i < boardSize.Width; i++)
                {
                    renderTile(g, this.getRenderRect(i, h), tiles[i, h], new Point(i, h));
                }
            }
        }

        public TILEDATA[,] exportData()
        {
            return tiles;
        }

        public void importData(TILEDATA[,] data)
        {
            tiles = data;
        }
    }
}
