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
using WD_toolbox;

namespace WDLibApplicationFramework.ViewAndEdit2D.LayeredEditor.Layers
{
    public enum ComposeModes {Normal, Multiply, Difference};

    public abstract class Layer
    {    
        public double Opacity  { get; set; } //0 .. 1
        public bool Visable { get; set; }
        public bool Locked  { get; set; }
        public ComposeModes ComposeMode { get; set; }

        public abstract void Render(IView2D view, IRenderer r);

        public void doLayerCompose(Bitmap below, IView2D view, IRenderer r)
        {
            r.DrawImage(below, 0, 0);

            if (Visable)
            {
                IRenderer r2 = IRendererFactory.GetPreferredRenderer(new Bitmap(below.Width, below.Height));
                Render(view, r2);
                Bitmap thisLayer = r2.RenderTargetAsGDIBitmap();

                switch (ComposeMode)
                {
                    case ComposeModes.Normal:
                        if (Opacity < 1)
                        {
                            Bitmap thisLayerScaled = thisLayer.MultiplyOpacity(Opacity);
                            r.DrawImage(thisLayerScaled, 0, 0);
                        }
                        else
                        {
                            r.DrawImage(thisLayer, 0, 0);
                        }
                        break;
                    case ComposeModes.Multiply:
                        goto case ComposeModes.Normal;//TODO: ignoring ComposeModes for now
                    case ComposeModes.Difference:
                        goto case ComposeModes.Normal;//TODO: ignoring ComposeModes for now
                }

                //TODO: ignoring ComposeModes for now
              
            }
        }
    }
}
