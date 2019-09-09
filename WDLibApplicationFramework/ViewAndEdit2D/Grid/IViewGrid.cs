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
using WD_toolbox.Maths;
using WD_toolbox.Maths.Geometry;
using WD_toolbox.Maths.Units;
using WD_toolbox.Rendering;
using WD_toolbox.Rendering.FormattedText;

namespace WDLibApplicationFramework.ViewAndEdit2D.Grid
{
    public interface IViewGrid
    {
        bool Visable { get; set; }
        //Percentage Opacity { get; set; }
        Color GridColor { get; set; }
        void RenderGizmoLayer(IRenderer r, IView2D view);

        Point2D Snap(Point2D posWS);
    }

    public abstract class ViewGridBase : IViewGrid
    {
        protected string FormatString { get; set; }
        public bool Visable { get; set; }
        //protected Percentage opacity;
        protected Color gridColor;
        //protected Color gridColProxy;

        private Font gridFont;
        public Font GridFont
        {
            get { return gridFont; }
            set { gridFont = value; RefreshTransientData(); }
        }

        private Font gridFontMinor;
        public Font GridFontMinor
        {
            get { return gridFontMinor; }
            set { gridFontMinor = value; RefreshTransientData(); }
        }

        /*
        public Percentage Opacity
        {
            get { return opacity; }
            set { opacity = value; RefreshTransientData(); }
        }*/  

        public Color GridColor
        {
            get { return gridColor; }
            set { gridColor = value; RefreshTransientData(); }
        }

        protected ViewGridBase()
        {
            //Opacity = Percentage.FromZeroToOne(1.0);
            GridColor = Color.DarkGreen;
            GridFont = new Font("Ariel", 8);
            GridFontMinor = gridFont;
            //GridFontMinor = new Font(GridFont, FontStyle.Italic);
        }

        protected virtual void RefreshTransientData()
        {
            //gridColProxy = Color.FromArgb(Opacity.ScaleToByte(), gridColor);
        }

        public abstract void RenderGizmoLayer(IRenderer r, IView2D view);

        public abstract Point2D Snap(Point2D posWS);
    }
}
