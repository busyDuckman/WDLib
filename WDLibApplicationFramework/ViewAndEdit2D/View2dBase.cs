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
using WD_toolbox.Maths.Geometry;
using WD_toolbox.AplicationFramework;

namespace WDLibApplicationFramework.ViewAndEdit2D
{
    public abstract class View2dBase : IView2D
    {
        protected double zoom;
        protected double transformX;
        protected double transformY;
        /// <summary>
        /// in radians
        /// </summary>
        protected double clockwiseRotation;
        /// <summary>
        /// The rectance (on screen) into which the view is rendered.
        /// In pixels or whatever the graphics unit is,
        /// </summary>
        protected Rectangle screenSpaceBounds;

        protected View2DOrientation orientation;
        //public event MouseEventWorldSpace.WSMouseEventHandler mouseClickWS;

        [NonSerialized]
        protected object dataLock = new object();
        [NonSerialized]
        protected TransMatrix2D renderMatrixProxy = null;
        protected TransMatrix2D inverseRenderMatrixProxy = null;

        public Action OnRefreshNeeded { get; set; }

        public View2dBase()
        {
            zoom = 1;
            transformX = 0;
            transformX = 0;
            clockwiseRotation = 0;
        }

        //----------------------------------------------------------------------------------------------
        // IView2D
        //----------------------------------------------------------------------------------------------
        #region Accessors
        public double Zoom
        {
            get { return zoom; }
            set { lock (dataLock) { zoom = value; clearTransformProxies(); } }
        }

        public double TransformX
        {
            get { return transformX; }
            set { lock (dataLock) { transformX = value; clearTransformProxies(); } }
        }

        public double TransformY
        {
            get { return transformY; }
            set { lock (dataLock) { transformY = value; clearTransformProxies(); } }
        }

        /// <summary>
        /// Clockwise rotation in radians
        /// </summary>
        public double ClockwiseRotation
        {
            get { return clockwiseRotation; }
            set { lock (dataLock) { clockwiseRotation = value; clearTransformProxies(); } }
        }

        public View2DOrientation Orientation
        {
            get { return orientation; }
            set { lock (dataLock) { orientation = value; clearTransformProxies(); } }
        }


        public Rectangle ScreenSpaceBounds
        {
            get
            {
                return screenSpaceBounds;
            }
            set
            {
                lock (dataLock)
                {
                    screenSpaceBounds = value;
                    clearTransformProxies();
                }
            }
        }



        /*event MouseEventWorldSpace.WSMouseEventHandler IView2D.MouseClickWS
        {
            add { lock (this.mouseClickWS) { this.mouseClickWS += value; } }
            remove { lock (this.mouseClickWS) { this.mouseClickWS -= value; } }
        }*/


        #endregion

        /*public void RaiseMouseEvwent(object sender, MouseEventWorldSpace e)
        {
            if (mouseClickWS != null)
            {
                mouseClickWS(sender, e);
            }
        }*/

        public void Refresh()
        {
            if (OnRefreshNeeded != null)
            {
                try
                {
                    OnRefreshNeeded();
                }
                catch(Exception ex)
                {
                    WDAppLog.logException(ErrorLevel.Error, ex);
                }
            }
        }



        public abstract Rectangle2D WorldSpaceBounds {get; set;}

        


        
        public TransMatrix2D WorldToScreenTransformMatrix
        {
            get 
            {
                if (renderMatrixProxy == null)
                {
                    renderMatrixProxy = this.CalculateWorldToScreenTransformMatrix();
                }
                return renderMatrixProxy;
            }
        }

        public TransMatrix2D ScreenToWorldTransformMatrix
        {
            get
            {
                if (inverseRenderMatrixProxy == null)
                {
                    inverseRenderMatrixProxy = WorldToScreenTransformMatrix.Inverse();
                }
                return inverseRenderMatrixProxy;
            }
        }


        public abstract void Render(WD_toolbox.Rendering.IRenderer r);


        protected void clearTransformProxies()
        {
            renderMatrixProxy = null;
            inverseRenderMatrixProxy = null;
        }

    }
}
