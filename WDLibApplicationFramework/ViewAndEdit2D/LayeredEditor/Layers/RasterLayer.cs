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

namespace WDLibApplicationFramework.ViewAndEdit2D.LayeredEditor.Layers
{
    public class RasterLayer : Layer
    {
        Bitmap image;

        public Bitmap Image
        {
            get { return image; }
            set { image = value; }
        }

        public RasterLayer() : this(null)
        {
        }

        public RasterLayer(Bitmap image)
        {
            this.image = image;
        }


        public override void Render(IView2D view, WD_toolbox.Rendering.IRenderer r)
        {
            throw new NotImplementedException();
        }
    }
}
