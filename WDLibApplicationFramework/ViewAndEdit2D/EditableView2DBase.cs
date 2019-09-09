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
using WD_toolbox;
using WDLibApplicationFramework.AplicationFramework.Actions;

namespace WDLibApplicationFramework.ViewAndEdit2D
{
    public abstract class EditableView2DBase<T> : View2dBase, IEditableView2D
    {
        public Point2D MousePosInWorldSpace { get; protected set; }
        public event EventHandler<Cursor> OnMouseCursorChange;
        public abstract T What { get; }
        Cursor mouseCursor = Cursors.Default;

        public int GizmoHandleSize { get; set; }
        public int GizmoLineSize { get; set; }

        public event EventHandler<IList<ActionMenuItem>> OnShowContextMenu;

        public EditableView2DBase()
        {
            CanCut = false;
            CanCopy = false;
            CanPaste = false;
            CanDelete = false;

            GizmoHandleSize = 8;
            GizmoLineSize = 2;
        }

        //rendering
        public virtual void RenderGizmoLayer(IRenderer r)
        {
        }

        //mouse events
        public virtual void DoMouseMove(object sender, MouseButtons button, Point2D worldScpacePos)
        {
            MousePosInWorldSpace = worldScpacePos;
        }

        public virtual void DoMouseDown(object sender, MouseButtons button, Point2D worldScpacePos)
        {
            MousePosInWorldSpace = worldScpacePos;
        }

        public virtual void DoMouseUp(object sender, MouseButtons button, Point2D worldScpacePos)
        {
            MousePosInWorldSpace = worldScpacePos;
        }

        public virtual void DoMouseEnter(object sender)
        {
        }

        public virtual void DoMouseLeave(object sender)
        {
        }

        public virtual void DoMouseWheel(object sender, Point2D worldScpacePos)
        {
            MousePosInWorldSpace = worldScpacePos;
        }

        public virtual void DoMouseHover(object sender)
        {

        }

        public virtual void DoMouseDoubleClick(object sender, MouseButtons button, Point2D worldScpacePos)
        {
            MousePosInWorldSpace = worldScpacePos;
        }

        public virtual void DoMouseClick(object sender, MouseButtons button, Point2D worldScpacePos)
        {
            MousePosInWorldSpace = worldScpacePos;
            if (button == MouseButtons.Right)
            {
                OnShowContextMenu.SafeCall(sender, GetContextActions());
            }
        }

        public virtual void DoDragDrop(object sender, object Data, Point2D worldScpacePos)
        {
            //MousePosInWorldSpace = worldScpacePos;
        }


        //mouse cursor
        public virtual Cursor MouseCursor
        {
            get { return mouseCursor; }
        }

        public virtual void ChangeMouseCursor(Cursor cursor)
        {
            mouseCursor = cursor;
            if (OnMouseCursorChange != null)
            {
                WDAppLog.TryCatchLogged(delegate() { OnMouseCursorChange(this, cursor); }, ErrorLevel.Error);
            }
        }


        //context menu
        public virtual IList<ActionMenuItem> GetContextActions()
        {
            return new List<ActionMenuItem>();
        }

        

        //clipboard
        public bool CanCut { get; protected set; }
        public bool CanCopy { get; protected set; }
        public bool CanPaste { get; protected set; }
        public bool CanDelete { get; protected set; }

        public virtual void DoCut()
        {

        }

        public virtual void DoCopy()
        {

        }

        public virtual void DoPaste()
        {

        }

        public virtual void DoDelete()
        {

        }

        //keyboard
        public void DoKeyPress(object sender, KeyPressEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void DoKeyUp(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void DoKeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        //status
        public event StatusDelegate OnViewTransformChange;
        public event StatusDelegate OnSceneChange;
        public event StatusDelegate OnSelectionChange;
        public event StatusDelegate OnMousePosChange;

    }
}
