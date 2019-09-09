/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
namespace WDMedia.Tiles
{
    public interface ITileSet : IList<Tile>, IDisposable
    {
        string Name { get; }
        int TileHeight { get; }
        int TileWidth { get; }
        //Tile this[int index] { get; set; }
    }
}
