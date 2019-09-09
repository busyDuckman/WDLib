/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox.AplicationFramework;

namespace WDMedia.Tiles
{
    public static class TileFactory
    {
        public static Tile loadTile(Type type, string path, int id)
        {
            if (type == typeof(Tile))
            {
                return Tile.fromFile(path, id);
            }

            WDAppLog.logFileOpenError(ErrorLevel.Error, path);
            return null;
        }
    }
}
