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
using WDMedia.Rendering;

namespace WDMedia.SceneGraph
{
    /// <summary>
    /// A set of items that would move together, eg layers in paralax scrolling.
    /// </summary>
    public abstract class Pane : IPRRenderable
    {
        public abstract void Render(I2DPerformanceRenderer renderer, System.Drawing.Rectangle where);
    }
}
