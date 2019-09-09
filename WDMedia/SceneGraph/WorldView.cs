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
using WD_toolbox.Rendering;
using WDLibApplicationFramework.ViewAndEdit2D;

namespace WDMedia.SceneGraph
{
    class WorldView : EditableView2DBase<World>
    {
        public override World What
        {
            get { throw new NotImplementedException(); }
        }

        public override WD_toolbox.Maths.Geometry.Rectangle2D WorldSpaceBounds
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void Render(IRenderer r)
        {
            throw new NotImplementedException();
        }
    }
}
