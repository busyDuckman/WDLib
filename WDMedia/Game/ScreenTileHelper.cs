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
using WD_toolbox.Rendering;
using WD_toolbox.Rendering.Patterns;

namespace WDMedia.Game
{
    public class ScreenTileHelper
    {
        public Point topLeft;
        public Point lowerRght;
        public readonly SizeF gapSize;
        public readonly SizeF tileSize;
        public readonly Size totalPixelSize;
        public readonly Size boardSize;

        private Font tileFont = null;
        private Brush tileTextBrush = new SolidBrush(Color.Black);

        protected Point[,] positionLookup;

        public ScreenTileHelper(Size boardSize, Size screenSize, SizeF gapRatio, int marginLeft, int marginRight, int marginTop, int marginBottom)
        {
            topLeft = new Point(marginLeft, marginTop);
            totalPixelSize = new Size(screenSize.Width - marginLeft - marginRight, screenSize.Height - marginTop - marginBottom);
            lowerRght = new Point(topLeft.X + totalPixelSize.Width, topLeft.Y + totalPixelSize.Height);

            float gap, tile;
            GameHelpers.findSpacingDistanceForGrid(marginLeft, marginRight, boardSize.Width, totalPixelSize.Width, gapRatio.Width, out tile, out gap);
            gapSize.Width = gap;
            tileSize.Width = tile;
            GameHelpers.findSpacingDistanceForGrid(marginTop, marginBottom, boardSize.Height, totalPixelSize.Height, gapRatio.Height, out tile, out gap);
            gapSize.Height = gap;
            tileSize.Height = tile;
            //tileFont = GameHelpers.fontSizer.generateGameFont((int)tileSize.Height, FontStyle.Regular);
            tileFont = FontFactory.GetFontByTotalHeight(FontFamily.GenericSerif, FontStyle.Regular, (int)tileSize.Height);

            positionLookup = new Point[boardSize.Width, boardSize.Height];

            for (int h = 0; h < boardSize.Height; h++)
            {
                for (int i = 0; i < boardSize.Width; i++)
                {
                    positionLookup[i, h] = new Point((int)(i * (gapSize.Width + tileSize.Width) + topLeft.X),
                                                     (int)(h * (gapSize.Height + tileSize.Height) + topLeft.Y));
                }
            }
            this.boardSize = boardSize;
        }

        public bool getSelected(Point pos, out int xTile, out int yTile)
        {
            bool xOK = getSelectedTile(pos.X - topLeft.X, gapSize.Width, tileSize.Width, out xTile);
            if (pos.X > lowerRght.X)
            {
                xOK = false;
                xTile = -1;
            }

            bool yOK = getSelectedTile(pos.Y - topLeft.Y, gapSize.Height, tileSize.Height, out yTile);
            if (pos.X > lowerRght.X)
            {
                yOK = false;
                yTile = -1;
            }

            return (xOK && yOK);
        }

        private bool getSelectedTile(int pos, float gap, float tile, out int tilePos)
        {
            int relativePos = pos % (int)(tile + gap);
            if (relativePos <= tile)
            {
                tilePos = (int)(pos / (int)(tile + gap));
                return true;
            }
            tilePos = -1;
            return false;
        }

        public Point getRenderPoint(int tileX, int tileY)
        {
            return positionLookup[tileX, tileY];
        }

        public Rectangle getRenderRect(int tileX, int tileY)
        {
            Point pos = positionLookup[tileX, tileY];
            return new Rectangle(pos.X, pos.Y, (int)tileSize.Width, (int)tileSize.Height);
        }

        public void drawString(Graphics g, int tileX, int TileY, string s)
        {
            Rectangle r = getRenderRect(tileX, TileY);
            g.DrawString(s, tileFont, tileTextBrush, r.Left + (r.Right - r.Left) * 0.3f, r.Top);
        }

        ~ScreenTileHelper()
        {
            tileTextBrush.Dispose();
            tileFont.Dispose();
        }
    }
}
