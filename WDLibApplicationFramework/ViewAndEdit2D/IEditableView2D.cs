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
using System.Windows.Forms;
using WD_toolbox.AplicationFramework;
using WD_toolbox.Maths.Geometry;
using WD_toolbox.Rendering;
using WDLibApplicationFramework.AplicationFramework.Actions;

namespace WDLibApplicationFramework.ViewAndEdit2D
{
    public delegate void StatusDelegate(object sender, string status);

    public interface IEditableView2D : IInputView2D
    {
        //status
        event StatusDelegate OnViewTransformChange;
        event StatusDelegate OnSceneChange;
        event StatusDelegate OnSelectionChange;
        event StatusDelegate OnMousePosChange;

        int GizmoHandleSize { get; set; }
        int GizmoLineSize { get; set; }

        //gizmos (screen space overlay)
        void RenderGizmoLayer(IRenderer r);
    }
}
