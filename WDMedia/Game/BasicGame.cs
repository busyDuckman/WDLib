/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WD_toolbox.AplicationFramework;
using WD_toolbox.Maths.Geometry;
using WDLibApplicationFramework.AplicationFramework.Actions;
using WDLibApplicationFramework.ViewAndEdit2D;

namespace WDMedia.Game
{

    public abstract class BasicGame : View2dBase, IGame, IDisposable
    {
        protected bool _refreshNeeded;
        protected ResourceManager resources;
        public PlayerStateType playerState;
        public DificultyType Difficulty { get; protected set; }
        public IGameHostWindow Host { get; protected set; }


        public BasicGame(IGameHostWindow host)
        {
            Host = host;
            _refreshNeeded = false;
            resources = new ResourceManager();
            playerState = new PlayerStateType();
        }

        //----------------------------------------------------------------------------------
        // IGame Members       
        //----------------------------------------------------------------------------------

        public virtual bool setDificulty(DificultyType dificulty)
        {
            return false;
        }

        public virtual bool newGame(DificultyType difficulty)
        {
            Difficulty = difficulty;
            return true;
        }

        public virtual bool checkRefreshNeeded()
        {
            if (_refreshNeeded)
            {
                _refreshNeeded = false;
                return true;
            }
            return false;
        }

        public abstract string Title {get;}


        //----------------------------------------------------------------------------------
        // IDisposable Members
        //----------------------------------------------------------------------------------
        public virtual void Dispose()
        {
            resources.Dispose();
        }

        //----------------------------------------------------------------------------------
        // IGame::IInputEventHandeler
        //----------------------------------------------------------------------------------
        public event EventHandler<Cursor> OnMouseCursorChange;
        public event EventHandler<IList<ActionMenuItem>> OnShowContextMenu;

        public Point2D MousePosInWorldSpace
        {
            get;
            protected set;
        }

        public virtual void DoMouseMove(object sender, System.Windows.Forms.MouseButtons button, Point2D worldScpacePos)
        {
            MousePosInWorldSpace = worldScpacePos;
        }

        public virtual void DoMouseDown(object sender, System.Windows.Forms.MouseButtons button, Point2D worldScpacePos)
        {
            MousePosInWorldSpace = worldScpacePos;
        }

        public virtual void DoMouseUp(object sender, System.Windows.Forms.MouseButtons button, Point2D worldScpacePos)
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

        public virtual void DoMouseDoubleClick(object sender, System.Windows.Forms.MouseButtons button, Point2D worldScpacePos)
        {
            MousePosInWorldSpace = worldScpacePos;
        }

        public virtual void DoMouseClick(object sender, System.Windows.Forms.MouseButtons button, Point2D worldScpacePos)
        {
            MousePosInWorldSpace = worldScpacePos;
        }

        public virtual void DoDragDrop(object sender, object Data, Point2D worldScpacePos)
        {
            MousePosInWorldSpace = worldScpacePos;
        }

        public System.Windows.Forms.Cursor MouseCursor
        {
            get;
            protected set;
        }

        public void ChangeMouseCursor(Cursor cursor)
        {
            MouseCursor = cursor;
            if (OnMouseCursorChange != null)
            {
                WDAppLog.TryCatchLogged(delegate() { OnMouseCursorChange(this, null); }, ErrorLevel.Error);
            }
        }

        public virtual IList<ActionMenuItem> GetContextActions()
        {
            return null;
        }

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

        public virtual void DoKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            
        }

        public virtual void DoKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            
        }

        public virtual void DoKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            
        }
        /*
        public double Zoom {get; set;}

        public double TransformX { get; set; }

        public double TransformY { get; set; }

        public View2DOrientation Orientation { get; set; }

        public double ClockwiseRotation { get; set; }

        public Rectangle2D WorldSpaceBounds { get; set; }

        public System.Drawing.Rectangle ScreenSpaceBounds { get; set; }

        public void Render(WD_toolbox.Rendering.IRenderer r)
        {
            throw new NotImplementedException();
        }

        public Action OnRefreshNeeded { get; set; }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public TransMatrix2D WorldToScreenTransformMatrix { get; protected set; }
        public TransMatrix2D ScreenToWorldTransformMatrix { get; protected set; }*/
    }
}

