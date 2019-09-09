/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WDLibApplicationFramework.ViewAndEdit2D;
using WD_toolbox.Maths.Trigonometry;
using System.Runtime.InteropServices;
using WD_toolbox.AplicationFramework;
using WD_toolbox.Rendering;
using WD_toolbox;
using WD_toolbox.Maths.Geometry;

namespace WDLibApplicationFramework.AplicationFramework.Controls
{
    /// <summary>
    /// This is a conrol that displays and manipulates a IView2D object (View);
    /// </summary>
    public partial class ViewBox2D : UserControl, IMessageFilter
    {

        public enum ViewInteractionStates { Normal=0, Dragging }

        //----------------------------------------------------------------------------------------------------------------
        // static data
        //----------------------------------------------------------------------------------------------------------------
       
        private struct POINT
        {
            Int32 x;
            Int32 y;

            public POINT(Point p)
            {
                x = p.X;
                y = p.Y;
            }
        }
        IntPtr WindowFromPoint(Point pt) {return WindowFromPoint(new POINT(pt));}
        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(POINT pt);
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        const int WM_MOUSEMOVE = 0x200;
        const int WM_MOUSEWHEEL = 0x20a;
        const int WM_KEYDOWN = 0x100;
        const int VK_NUMPAD0 = 0x60;

        protected static double[] zoomLevels = new double[] { 0.1, 0.2, 0.25, 0.3, 0.4, 0.5, 0.6, 0.7, 0.75, 0.8, 0.9, 1.0, 1.1, 1.25, 1.5, 1.75, 2, 2.5, 3, 3.5, 4, 5, 6, 7, 8, 9, 10 };

        //----------------------------------------------------------------------------------------------------------------
        // Instance date
        //----------------------------------------------------------------------------------------------------------------

        double minZoom = 0.01;
        public double MinZoom
        {
            get { return minZoom; }
            set { minZoom = value; }
        }
        double maxZoom = 100.0;

        public double MaxZoom
        {
            get { return maxZoom; }
            set { maxZoom = value; }
        }

        public MouseButtons DragButton { get; set; }

        protected Point2D GrabbedPoint = new Point2D();

        public ViewInteractionStates State { get; protected set; }


        bool ctrlPressed = false;

        bool picMainIsActive = false;

        Point picMainPos = new Point(0, 0);
        Rectangle picMainBounds = new Rectangle(0, 0, 0, 0);

        public delegate void ViewChangeDelegate(IView2D newState);
        public event ViewChangeDelegate onViewChange;
        public event ViewChangeDelegate onStatusChange;
        private IView2D view;
        protected volatile bool redawNeeded = false;
        bool updatingControls = false;

        public IView2D View
        {
            get { return view; }
            set 
            { 
                view = value;
                RebindScreenBounds();
                if (view != null)
                {
                    view.OnRefreshNeeded = this.FlagRedraw;

                    if (view is IEditableView2D)
                    {
                        IEditableView2D ev = view as IEditableView2D;
                        ev.BindToControl(picMain);
                    }
                }
                DoViewChangeTasks();
            }
        }

        [Browsable(true),
            DescriptionAttribute("Is the toolbar visiable?"),
           CategoryAttribute("Custom Appearance")
        ]
        public bool ShowToolBar
        {
            get { return tbView.Visible; }
            set { tbView.Visible = value; }
        }



         [Browsable(true),
            DescriptionAttribute("Does the mose wheel scroll vertically?"),
           CategoryAttribute("Custom Behaviour")
        ]
        public bool SwapWheelZoomDir { get; set; }


         /*protected override CreateParams CreateParams
         {
             get
             {
                 CreateParams cp = base.CreateParams;
                 cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
                 return cp;
             }
         }
         protected override void WndProc(ref Message m)
         {
             const int WM_NCHITTEST = 0x0084;
             const int HTTRANSPARENT = (-1);

             if (m.Msg == WM_NCHITTEST)
             {
                 m.Result = (IntPtr)HTTRANSPARENT;
             }
             else
             {
                 base.WndProc(ref m);
             }
         }*/


        //----------------------------------------------------------------------------------------------------------------
        // Constructor / Setup
        //----------------------------------------------------------------------------------------------------------------
        public ViewBox2D()
        {
            InitializeComponent();
            DragButton = MouseButtons.Right;
        }

        private void ViewBox2D_Load(object sender, EventArgs e)
        {
            Application.AddMessageFilter(this);
            picMain.MouseWheel += new MouseEventHandler(picMain_MouseWheel);
            picMain.AllowDrop = true;
            State = ViewInteractionStates.Normal;

            
        }

        //----------------------------------------------------------------------------------------------------------------
        // Misc
        //----------------------------------------------------------------------------------------------------------------
        public void FlagRedraw()
        {
            //this.picMain.Invalidate();
            //this.picMain.Refresh();
            redawNeeded = true;
        }

        protected void DoViewChangeTasks(bool updateScrollBars = true)
        {
            updatingControls = true;
            try
            {
                if (view != null)
                {
                    lblZoom.Text = ((int)(view.Zoom * 100.0)).ToString() +"%";

                    if (updateScrollBars)
                    {
                        double xOff = 0;// (view.WorldSpaceBounds.X * view.Zoom);
                        double yOff = 0; //(view.WorldSpaceBounds.Y * view.Zoom);

                        int newVScroll = -(int)(view.TransformY - xOff);
                        int newHScroll = -(int)(view.TransformX - yOff);

                        //these 2 lines are to enable a infinite scolling, which may be bad.
                        adjustRangeToSuite(scrlVert, newVScroll);
                        adjustRangeToSuite(scrlHoriz, newHScroll);


                        scrlVert.Value = newVScroll;
                        scrlHoriz.Value = newHScroll;
                    }

                    if (view is IEditableView2D)
                    {
                        IEditableView2D ev = view as IEditableView2D;
                        //resend the mouse pos.
                        //The mouse did not move on the screen, but the view window is different


                        MouseEventArgs arg = new MouseEventArgs(System.Windows.Forms.MouseButtons.None,
                                                                0,
                                                                ((Control)picMain).PointToClient(Cursor.Position).X,
                                                                ((Control)picMain).PointToClient(Cursor.Position).Y,
                                                                0);

                        ev.DoMouseMove_ScreenSpace(this, arg);
                    }

                    if (onViewChange != null)
                    {
                        onViewChange(view);
                    }
                }
            } 
            catch(Exception ex) 
            {
                WDAppLog.logException(ErrorLevel.SmallError, ex);
            }

            updatingControls = false;
        }

        private void adjustRangeToSuite(ScrollBar scrlBar, int newValue)
        {
            if (scrlBar.Maximum < newValue)
            {
                scrlBar.Maximum = newValue;
            }

            if (scrlBar.Minimum > newValue)
            {
                scrlBar.Minimum = newValue;
            }
        }

        void adjustTransformByScrollBars()
        {
            if ((view != null) && (!updatingControls))
            {
                double xOff = 0;// (view.WorldSpaceBounds.X * view.Zoom);
                double yOff = 0; //(view.WorldSpaceBounds.Y * view.Zoom);
                view.TransformY = -(scrlVert.Value - (int)xOff);
                view.TransformX = -(scrlHoriz.Value - (int)yOff);
                DoViewChangeTasks(false);
            }
        }


        //----------------------------------------------------------------------------------------------------------------
        // Events
        //----------------------------------------------------------------------------------------------------------------
        void picMain_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                adjustZoomByWheel(e.Delta, e.X, e.Y);
            }
            else
            {
                adjustScrollBarsByWheel(e.Delta);
            }
        }

        private void scrlVert_Scroll(object sender, ScrollEventArgs e)
        {
            if(view != null) 
            {
                adjustTransformByScrollBars();
                picMain.Refresh();
            }
        }

        private void scrlHoriz_Scroll(object sender, ScrollEventArgs e)
        {
            if (view != null)
            {
                adjustTransformByScrollBars();
                picMain.Refresh();
            }
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            if (view != null)
            {
                doZoomIn();

                //view.Zoom += 0.1;
                //RebindScreenBounds();
                //picMain.Refresh();
            }
        }


        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            if (view != null)
            {
                doZoomOut();

                //view.Zoom -= 0.1;
                //RebindScreenBounds();
                //picMain.Refresh();
            }
        }

       

        
        private void picMain_Click(object sender, EventArgs e)
        {
            //this.MouseClick(sender, e);
            //this.per
        }

        private void picMain_MouseMove(object sender, MouseEventArgs e)
        {
            switch (State)
            {
                case ViewInteractionStates.Normal:
                    break;
                case ViewInteractionStates.Dragging:
                    view.TransposeToAllignPoints(GrabbedPoint, e.Location);
                    DoViewChangeTasks();
                    picMain.Refresh();
                    break;
            }

            if (view is IEditableView2D)
            {
                var location = view.ScreenToWorldTransformMatrix.Transform(e.Location);
                lblMousePos.Text = view.WorldSpaceBounds.Contains(location) ? location.AsPoint().ToString() : "";

                if (onStatusChange != null)
                {
                    onStatusChange(view);
                }
            }
        }

        private void picMain_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == this.DragButton)
            {
                GrabbedPoint = view.ScreenToWorldTransformMatrix.Transform(e.Location);
                State = ViewInteractionStates.Dragging;
            }
        }

        private void picMain_MouseEnter(object sender, EventArgs e)
        {

        }

        private void picMain_MouseLeave(object sender, EventArgs e)
        {

        }

        private void picMain_MouseUp(object sender, MouseEventArgs e)
        {
            //do we need to stop dragging?
            if ((e.Button == this.DragButton) && (State == ViewInteractionStates.Dragging))
            {
                State = ViewInteractionStates.Normal;
            }
        }

        private void btnZoomFit_Click(object sender, EventArgs e)
        {
            doZoomAll();

        }

        private void btnZoomActual_Click(object sender, EventArgs e)
        {
            doZoomActual();
        }

        private void btnRotateLeft_Click(object sender, EventArgs e)
        {
            if(view != null)
            {
                view.ClockwiseRotation += Trig.Deg2Rad * 15;
                lblRotate.Text = "" + (int)Math.Round(view.Zoom * Trig.Rad2Deg);
                picMain.Refresh();
            }
        }

        private void btnRotateRight_Click(object sender, EventArgs e)
        {
            if (view != null)
            {
                view.ClockwiseRotation -= Trig.Deg2Rad * 15;
                lblRotate.Text = ""+ (int)Math.Round(view.Zoom * Trig.Rad2Deg);
                picMain.Refresh();
            }
        }

        private void picMain_Resize(object sender, EventArgs e)
        {
            RebindScreenBounds();
        }

        public void RebindScreenBounds()
        {
            if (view != null)
            {
                view.ScreenSpaceBounds = new Rectangle(0, 0, picMain.Width, picMain.Height);
                //scrlHoriz.Maximum = (int)(view.WorldSpaceBounds.Width * view.Zoom);
                //scrlVert.Maximum = (int)(view.WorldSpaceBounds.Height * view.Zoom);

                scrlHoriz.Maximum = (int)(view.WorldSpaceBounds.Right * view.Zoom);
                scrlHoriz.Minimum = (int)(view.WorldSpaceBounds.X * view.Zoom);
                scrlVert.Maximum = (int)(view.WorldSpaceBounds.Bottom * view.Zoom);
                scrlVert.Minimum = (int)(view.WorldSpaceBounds.Y * view.Zoom);

                picMain.Refresh();
            }
        }

        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_MOUSEWHEEL:
                    {
                        // WM_MOUSEWHEEL, find the control at screen position m.LParam

                        Int16 x = (Int16)(m.LParam.ToInt32() & 0xffff);
                        Int16 y = (Int16)(m.LParam.ToInt32() >> 16);
                        Point pos = new Point(x, y);
                        IntPtr hWnd = WindowFromPoint(pos);
                        if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
                        {
                            SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
                            return true;
                        }
                    }
                    break;

                case WM_MOUSEMOVE:                    
                    picMainIsActive = picMainBounds.Contains(ViewBox2D.MousePosition);
                    break;

                case WM_KEYDOWN:
                  
                    break;

            }
            return false;
        }

        protected void adjustScrollBarsByWheel(int delta)
        {
            try
            {
                if (delta != 0)
                {
                    //what is the wheel altering
                    ScrollBar sb = SwapWheelZoomDir ? (ScrollBar)scrlHoriz : (ScrollBar)scrlVert;
                    if (!scrlVert.Enabled)
                    {
                        sb = scrlHoriz;
                    }
                    if (!scrlHoriz.Enabled)
                    {
                        sb = scrlVert;
                    }

                    if (sb != null)
                    {
                        int newValue = sb.Value - (delta / 5);
                        if (newValue < sb.Minimum)
                        {
                            newValue = sb.Minimum;
                        }
                        if (newValue > sb.Maximum)
                        {
                            newValue = sb.Maximum;
                        }
                        sb.Value = newValue;
                        //picMain.Invalidate();
                        adjustTransformByScrollBars();
                        DoViewChangeTasks();
                        picMain.Refresh();
                    }
                }
            }
            catch { }
        }

        protected void adjustZoomByWheel(int delta, int x, int y)
        {
            try
            {
                if (delta != 0)
                {
                    if (view != null)
                    {
                        Point2D oldMousePosWS = view.ScreenSpace2WorldSpace(x, y);
                        
                        //double oldZoom = view.Zoom;
                        //double newZoom = oldZoom + (delta / 1200.0);

                        //double factor = Math.Pow(view.Zoom, Math.Log10(Math.Abs(delta)));
                        double factor = Math.Abs((view.Zoom / 15) * (delta/120));
                        factor = (delta > 0) ? factor : -factor;
                        double newZoom = view.Zoom + factor;

                        if (isValidZoomLevel(newZoom))
                        {
                            view.Zoom = newZoom;
                            view.TransposeToAllignPoints(oldMousePosWS, new Point(x, y));
                        }
                        else
                        {
                            //WDAppLog.Debug("bad zoom: " + newZoom);
                            view.Zoom = Math.Max(Math.Min(newZoom, MaxZoom), MinZoom);
                            view.TransposeToAllignPoints(oldMousePosWS, new Point(x, y));
                        }

                        DoViewChangeTasks();
                        picMain.Refresh();
                        
                    }    
                }
            }
            catch { }
        }

        private bool isValidZoomLevel(double newZoom)
        {
            return (newZoom <= MaxZoom) && (newZoom >= MinZoom);
        }

        public void doZoomIn()
        {
            try
            {
                if (view != null)
                {
                    double nextZoom = zoomLevels.First(D => D > view.Zoom);
                    view.Zoom = nextZoom;
                    DoViewChangeTasks();
                    picMain.Refresh();
                }
            }
            catch
            {
            }
        }

        public void doZoomOut()
        {
            try
            {
                if (view != null)
                {
                    double nextZoom = zoomLevels.Last(D => D < view.Zoom);
                    if (isValidZoomLevel(nextZoom))
                    {
                        view.Zoom = nextZoom;
                        DoViewChangeTasks();
                        picMain.Refresh();
                    }
                }
            }
            catch
            {
            }
        }

        public void doZoomAll(AxisSelection mode = AxisSelection.Both)
        {
            try
            {
                if (view != null)
                {
                    double zoom = 1;

                    switch (mode)
                    {
                        case AxisSelection.Both:
                            zoom = Math.Min(view.ScreenSpaceBounds.Width / view.WorldSpaceBounds.Width,
                                    view.ScreenSpaceBounds.Height / view.WorldSpaceBounds.Height);
                            break;
                        case AxisSelection.Vertical:
                            zoom = view.ScreenSpaceBounds.Height / view.WorldSpaceBounds.Height;
                            break;
                        case AxisSelection.Horizontal:
                            zoom = view.ScreenSpaceBounds.Width / view.WorldSpaceBounds.Width;
                            break;
                    }

                    if (zoomLevels.Any(D => D < zoom))
                    {
                        zoom = zoomLevels.Last(D => D < zoom);
                    }
                    view.Zoom = zoom;

                    view.TransposeToAllignPoints(view.WorldSpaceBounds.Middle, 
                                                 view.ScreenSpaceBounds.Middle().AsPoint(),
                                                 mode);

                    DoViewChangeTasks();
                    picMain.Refresh();
                }
            }
            catch
            {
            }
        }

        public void doZoomActual()
        {
            try
            {
                if (view != null)
                {
                    view.Zoom = 1;

                    DoViewChangeTasks();
                    picMain.Refresh();
                }
            }
            catch
            {
            }
        }
        
        private void picMain_Paint(object sender, PaintEventArgs e)
        {
            if (view != null)
            {
                try
                {
                    GDIPlusRenderer r = new GDIPlusRenderer(e.Graphics);
                    r.Clear(this.BackColor);
                    view.Render(r);
                    if (view is IEditableView2D)
                    {
                        IEditableView2D ev = view as IEditableView2D;
                        ev.RenderGizmoLayer(r);
                    }
                    r.Close();
                }
                catch (Exception ex)
                {
                    e.Graphics.DrawString(ex.GetADecentExplination(), this.Font, Brushes.Red, 20, 20);
                    e.Graphics.DrawString(ex.StackTrace.ToString().Wrap(40), this.Font, Brushes.Red, 20, 50);
                    WDAppLog.logException(ErrorLevel.Error, ex);
                }
            }
        }

        private void timRedraw_Tick(object sender, EventArgs e)
        {
            if (redawNeeded)
            {
                redawNeeded = false;
                picMain.Refresh();
            }
        }

        private void picMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void picMain_DragDrop(object sender, DragEventArgs e)
        {
            /*if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                var item = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;
                //e.
                //item.ListView.Items.Remove(item);
            }*/
        }

    }
}
