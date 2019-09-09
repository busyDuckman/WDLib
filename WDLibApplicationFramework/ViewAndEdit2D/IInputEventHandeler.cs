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
using WD_toolbox.Maths.Geometry;
using WDLibApplicationFramework.AplicationFramework.Actions;

namespace WDLibApplicationFramework.ViewAndEdit2D
{
    /// <summary>
    /// This class handles the concept of responding to user events (actions).
    /// It treats all events in the worldspace removing the concept of a view 
    /// to simplify the implementation of the editing code.
    /// 
    /// This class is intended for intefaces like IInputView2D that require encapsulating user input.
    /// EditableView2DBase is probably a good stating point for an IInputEventHandeler implementation.
    /// 
    /// Key Cconcepts:
    ///   - Mouse and keyboard events are as per Win23Forms events, but use a Point2D worldspace co-ordinate.
    ///     - This class is where responding to user input logic goes
    ///   - This class can publish mouse cursor changes (ig in reponce to a mouse over).
    ///   - GetContextActions() can be used to retrive an appropriate context menu structure.
    /// </summary>
    public interface IInputEventHandeler
    {
        //mouse events
        Point2D MousePosInWorldSpace { get; }
        void DoMouseMove(object sender, MouseButtons button, Point2D worldScpacePos);
        void DoMouseDown(object sender, MouseButtons button, Point2D worldScpacePos);
        void DoMouseUp(object sender, MouseButtons button, Point2D worldScpacePos);
        void DoMouseEnter(object sender);
        void DoMouseLeave(object sender);
        void DoMouseWheel(object sender, Point2D worldScpacePos);
        void DoMouseHover(object sender);
        void DoMouseDoubleClick(object sender, MouseButtons button, Point2D worldScpacePos);
        void DoMouseClick(object sender, MouseButtons button, Point2D worldScpacePos);
        void DoDragDrop(object sender, Object Data, Point2D worldScpacePos);

        //mouse cursor
        event EventHandler<Cursor> OnMouseCursorChange;
        Cursor MouseCursor { get; }
        //void ChangeMouseCursor(Cursor cursor);

        //context menu
        /// <summary>
        /// Menu structure (as in context menu) for last recorded mouse position.
        /// </summary>
        /// <returns></returns>
        IList<ActionMenuItem> GetContextActions();
        event EventHandler<IList<ActionMenuItem>> OnShowContextMenu;

        //clipboard
        void DoCut();
        void DoCopy();
        void DoPaste();
        void DoDelete();

        //keyboard
        void DoKeyPress(object sender, KeyPressEventArgs e);
        void DoKeyUp(object sender, KeyEventArgs e);
        void DoKeyDown(object sender, KeyEventArgs e);


    }


  
}
