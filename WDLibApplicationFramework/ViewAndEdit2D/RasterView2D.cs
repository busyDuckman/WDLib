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
using WD_toolbox.Maths.Geometry;
using WD_toolbox;
using WD_toolbox.AplicationFramework;
using WD_toolbox.Rendering;

namespace WDLibApplicationFramework.ViewAndEdit2D
{
    public class RasterView2D : View2dBase, IView2D
    {
       
        Bitmap image;
        public Bitmap Image { get { return image; } 
            set 
            { 
                image = value;
                if (OnRefreshNeeded != null)
                {
                    OnRefreshNeeded();
                }
            } 
        }

        //----------------------------------------------------------------------------------------------
        // Constructors
        //----------------------------------------------------------------------------------------------
        public RasterView2D() : this(null)
        {
        }
        
        public RasterView2D(Bitmap sourceImage) : base()
        {
            Image = sourceImage;
        }

        public override Rectangle2D WorldSpaceBounds
        {
            get
            {
                if (Image != null)
                {
                    return new Rectangle2D(0, 0, Image.Width, Image.Height);
                }
                else
                {
                    return new Rectangle2D(0, 0, 0, 0);
                }
            }
            set
            {
                //cant set
            }
        }


        public override void Render(IRenderer g)
        {
            if (Image == null)
            {
                return;
            }

            try
            {

                if (renderMatrixProxy == null)
                {
                    renderMatrixProxy = this.CalculateWorldToScreenTransformMatrix();
                }
                g.SetTransform(renderMatrixProxy);
                g.DrawImage(Image, 0, 0);
                g.ResetTransform();

                /* 
                //Renderer method, has issues because g.DrawImage is not what it appears
                Point[] points = new Point[3] {new Point(0,0),
                                            new Point(Image.Width,0),
                                            new Point(0,Image.Height)};
                points[0] = renderMatrixProxy.Transform(points[0]).AsPoint();
                points[1] = renderMatrixProxy.Transform(points[1]).AsPoint();
                points[2] = renderMatrixProxy.Transform(points[2]).AsPoint();

                g.DrawImage(Image, points);
                */


            }
            catch (Exception ex)
            {
                WDAppLog.logException(ErrorLevel.Error, ex);
            }
        }
    }
}
