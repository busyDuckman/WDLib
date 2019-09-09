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

namespace WDLibApplicationFramework.ViewAndEdit2D
{
    /// <summary>
    /// This interface represents a View that manipulates a IView2D via input actions.
    /// 
    /// If your View implements IInputView2D (as apposed to just IView2D) and you assign
    /// it to the View property of a ViewBox2D, then the controll will automatically become
    /// an editor, not just a viewer.
    /// 
    /// The extension class IInputView2DExtension handles mapping the IInputView2D's
    /// controll scpace actions into the worldspace. Importatly BindToControl sets
    /// up conecting the controll to this interface.
    /// </summary>
    public interface IInputView2D : IInputEventHandeler, IView2D
    {
    }


    /// <summary>
    /// IInputView2DExtension provides methds for IInputView2D.
    /// This extension class handles mapping the IInputView2D's actions into the worldspace.
    /// Importatly BindToControl sets up all the binding.
    /// </summary>
    public static class IInputView2DExtension
    {
        public static void DoMouseMove_ScreenSpace(this IInputView2D view, object sender, MouseEventArgs e)
        {
            view.DoMouseMove(sender, e.Button, view.ScreenSpace2WorldSpace(e.X, e.Y));
        }
        public static void DoMouseDown_ScreenSpace(this IInputView2D view, object sender, MouseEventArgs e)
        {
            view.DoMouseDown(sender, e.Button, view.ScreenSpace2WorldSpace(e.X, e.Y));
        }
        public static void DoMouseUp_ScreenSpace(this IInputView2D view, object sender, MouseEventArgs e)
        {
            view.DoMouseUp(sender, e.Button, view.ScreenSpace2WorldSpace(e.X, e.Y));
        }
        public static void DoMouseClick_ScreenSpace(this IInputView2D view, object sender, MouseEventArgs e)
        {
            view.DoMouseClick(sender, e.Button, view.ScreenSpace2WorldSpace(e.X, e.Y));
        }
        public static void DoMouseDoubleClick_ScreenSpace(this IInputView2D view, object sender, MouseEventArgs e)
        {
            view.DoMouseDoubleClick(sender, e.Button, view.ScreenSpace2WorldSpace(e.X, e.Y));
        }

        public static void BindToControl(this IInputView2D view, Control c)
        {
            //mouse events
            c.MouseDown += view.DoMouseDown_ScreenSpace;
            c.MouseEnter += delegate(object sender, EventArgs e) { view.DoMouseEnter(sender); };
            c.MouseLeave += delegate(object sender, EventArgs e) { view.DoMouseLeave(sender); };
            c.MouseHover += delegate(object sender, EventArgs e) { view.DoMouseHover(sender); };
            c.MouseMove += view.DoMouseMove_ScreenSpace;
            c.MouseUp += view.DoMouseUp_ScreenSpace;
            c.MouseWheel += delegate(object sender, MouseEventArgs e) { view.DoMouseWheel(sender, view.ScreenSpace2WorldSpace(e.X, e.Y)); };
            c.MouseClick += view.DoMouseClick_ScreenSpace;
            c.MouseDoubleClick += view.DoMouseDoubleClick_ScreenSpace;

            //drag drop does not have the current mouse pos.
            c.DragDrop += delegate(object sender, DragEventArgs e) 
            {
                
                //view.DoDragDrop(sender, e.Data, view.ScreenSpace2WorldSpace(e.X, e.Y));
                Point client = c.PointToClient(new Point(e.X, e.Y));
                view.DoDragDrop(sender, e.Data, view.ScreenSpace2WorldSpace(client)); 

            };

            //mouse cursor
            view.OnMouseCursorChange += delegate(object sender, Cursor cursor)
            {
                c.Cursor = cursor ?? (Cursors.Default);
            };

            //context menu

            //clipboard

            //keyboard
            c.KeyDown += view.DoKeyDown;
            c.KeyUp += view.DoKeyUp;
            c.KeyPress += view.DoKeyPress;

            //status
        }

    }
}
