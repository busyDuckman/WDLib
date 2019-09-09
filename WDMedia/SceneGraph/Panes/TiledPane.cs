/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox.AplicationFramework;
using WDMedia.Rendering;
using WDMedia.Tiles;

namespace WDMedia.SceneGraph.Panes
{
    public class TiledPane : Pane
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public IList<int> Tiles { get; protected set; }
        public World parentWorld;

        public TiledPane(int width, int height, IList<int> tileIndicies, World world)
        {
            Width = width;
            Height = height;
            Tiles = tileIndicies.ToArray();
            parentWorld = world;
        }

        public int this[int x, int y]
        {
            get { return Tiles[(y*Width) + x];  }
        }


        public override void Render(I2DPerformanceRenderer renderer, System.Drawing.Rectangle where)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Tile tile = parentWorld.GetTile(this[x, y]);
                    if (tile != null) //null tile indicates transparent block
                    {
                        int xPos = x * tile.Width;
                        int yPos = y * tile.Height;
                        int width = tile.Width;
                        int heght = tile.Height;
                        //IntRect rec = new IntRect(x, y, width, heght);

                        //Sprite texSprite = new Sprite(new Texture(tile.Image), rec);
                        renderer.DrawImage(tile, xPos, yPos);
                    }
                }
            }
        }
    }
}
