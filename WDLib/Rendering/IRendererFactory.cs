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

namespace WD_toolbox.Rendering
{
    public static class IRendererFactory
    {
        /// <summary>
        /// Gets the renderer that is best supported by the system.
        /// </summary>
        /// <param name="b">An Image to draw onto.</param>
        /// <returns>A new IRenderer instance.</returns>
        public static IRenderer GetPreferredRenderer(Bitmap b)
        {
            return new GDIPlusRenderer(b);
        }

        /// <summary>
        /// Gets the renderer that is best supported by the system.
        /// The renderer will be for an ARGB based colour scheme.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>A new IRenderer instance.</returns>
        public static IRenderer GetPreferredRenderer(int width, int height)
        {
            return new GDIPlusRenderer(width, height);
        }

        public static IRenderer GetPreferredRenderer(Size size)
        {
            return GetPreferredRenderer(size.Width, size.Height);
        }




    }

}
