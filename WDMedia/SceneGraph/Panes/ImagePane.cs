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
using WDMedia.Rendering;

namespace WDMedia.SceneGraph.Panes
{
    public class ImagePane : Pane, IPRImage
    {
        public Bitmap Image { get; protected set; }
        public object NativeObject { get; set; }

        public ImagePane(Bitmap image)
        {
            Image = image;
            NativeObject = null;
        }

        public override void Render(Rendering.I2DPerformanceRenderer renderer, System.Drawing.Rectangle where)
        {
            renderer.DrawImage(this, where.X, where.Y);
        }
    }
}
