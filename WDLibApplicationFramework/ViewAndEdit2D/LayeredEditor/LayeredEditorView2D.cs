/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using WD_toolbox.Maths.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox.Rendering;
using WDLibApplicationFramework.ViewAndEdit2D.LayeredEditor.Layers;

namespace WDLibApplicationFramework.ViewAndEdit2D.LayeredEditor
{
    public abstract class LayeredEditorView2D<TYPE> : EditableView2DBase<TYPE>
    {
        public List<Layer> layers;
        Rectangle2D worldSpaceBounds;

        public LayeredEditorView2D() : base()
        {
            layers = new List<Layer>();
            worldSpaceBounds = new Rectangle2D(0, 0, 256, 256);
        }

        public LayeredEditorView2D(Bitmap firstLayer) : base()
        {
            layers = new List<Layer>();
            layers.Add(new RasterLayer(firstLayer));
            UpdateBoundsFromLargestLayer();
        }

        private void UpdateBoundsFromLargestLayer()
        {
            throw new NotImplementedException();
        }

        public override Rectangle2D WorldSpaceBounds
        {
            get
            {
                return worldSpaceBounds;
            }
            set
            {
                worldSpaceBounds = value;
            }
        }

        public override void Render(IRenderer r)
        {

        }
    }
}
