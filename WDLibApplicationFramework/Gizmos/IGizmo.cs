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
using System.Windows.Forms;
using WD_toolbox.Maths.Geometry;
using WD_toolbox.Rendering;
using WDLibApplicationFramework.ViewAndEdit2D;

namespace WDLibApplicationFramework.Gizmos
{
    public delegate void GizmoDragDelegate(Point2D newPos, IView2D view, bool MouseUp);

    public interface IGizmo
    {
        bool Locked {get; set;}
        bool IsHit(Point2D pos, IView2D view);
        Cursor DesiredMouseCursor(Point2D pos, IView2D view);
      
        IList<IGizmoHandle> Handles {get;}
        void DoHandleDrag(IGizmoHandle handle, Point2D newPos, IView2D view, bool MouseUp);
        void DoDrag(Point2D newPos, IView2D view, bool MouseUp);
        void RenderGizmoLayer(IRenderer r, bool asSelected, IView2D view);

        IGizmoHandle GetSelectedHandle(Point2D posWS, IView2D view);
    }    

    public interface IGizmoHandle
    {
        Point2D Pos {get; set;}
        IGizmo ParentGizmo {get; set;}
        object Tag {get; set;}
        GizmoDragDelegate OnDrag {get; set;}
        Cursor DesiredMouseCursor { get; set; }

        bool IsHit(Point2D pos, IView2D view);
    }

    public class GizmoHandle : IGizmoHandle
    {
        public Point2D Pos { get; set; }
        public IGizmo ParentGizmo { get; set; }
        public object Tag {get; set;}
        public GizmoDragDelegate OnDrag { get; set; }
        public Cursor DesiredMouseCursor { get; set; }

        public GizmoHandle(Point2D pos, IGizmo parentGizmo)
        {
            DesiredMouseCursor = Cursors.Default;
            Pos = pos;
            ParentGizmo = parentGizmo;
        }

        public bool IsHit(Point2D pos, IView2D view)
        {
            return (Pos - pos).Length() < ((1.0/view.Zoom) * 8.0);
        }
    }
}
