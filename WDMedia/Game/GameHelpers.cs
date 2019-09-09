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

namespace WDMedia.Game
{
    public static class GameHelpers
    {
        public static void findSpacingDistanceForGrid(float edgeSizeBefore, float edgeSizeAfter, int numPeices, int screenSize, float gapRatio,
         out float tileSize, out float gapSize)
        {
            float totalSize = screenSize - (edgeSizeAfter + edgeSizeBefore);

            //result of doing some math
            tileSize = totalSize / (numPeices + numPeices * gapRatio - gapRatio);
            gapSize = gapRatio * tileSize;
        }

    }
}
