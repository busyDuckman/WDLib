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
using WD_toolbox.Maths.Geometry;

namespace WDLibApplicationFramework.AplicationFramework.Controls
{
    public partial class Console : UserControl
    {
        public class ConsoleView : IView2D
        {
            protected Console console;

            public ConsoleView(Console console)
            {
                this.console = console;
            }

            public double Zoom
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

            public double TransformX
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

            public double TransformY
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

            public View2DOrientation Orientation
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

            public double ClockwiseRotation
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

            public Rectangle2D WorldSpaceBounds
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

            public Rectangle ScreenSpaceBounds
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

            public void Render(WD_toolbox.Rendering.IRenderer r)
            {
                throw new NotImplementedException();
            }

            public Action OnRefreshNeeded
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

            //public event MouseEventWorldSpace.WSMouseEventHandler MouseClickWS;

            /*public void RaiseMouseEvwent(object sender, MouseEventWorldSpace e)
            {
                throw new NotImplementedException();
            }*/


            public TransMatrix2D WorldToScreenTransformMatrix
            {
                get { throw new NotImplementedException(); }
            }


            public TransMatrix2D ScreenToWorldTransformMatrix
            {
                get { throw new NotImplementedException(); }
            }


            public void Refresh()
            {
                throw new NotImplementedException();
            }
        }

        public Console()
        {
            InitializeComponent();
        }

        private void Console_Load(object sender, EventArgs e)
        {
            //vbMain.View =
        }
    }
}
