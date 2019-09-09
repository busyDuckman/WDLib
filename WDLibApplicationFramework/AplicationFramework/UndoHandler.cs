/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections;
using System.Collections.Generic;

using WD_toolbox.AplicationFramework;
using WD_toolbox.Data.DataStructures;
using WD_toolbox.Maths.Range;

namespace WD_toolbox.AplicationFramework
{
/// <summary>
/// A generic process fo undo / redo tye operations.
/// Requires that the object for which ondo/redo functionality is made avalable implements ICloneable.
/// </summary>
public class UndoHandler<DOCUMENT_TYPE>
where DOCUMENT_TYPE : ICloneable
	{
	#region data types
    public delegate void undoEventHandler(DOCUMENT_TYPE newObject);
	public class UndoEvent
		{
        public readonly DOCUMENT_TYPE obj;
		public readonly string actionName;

        public UndoEvent(string actionName, DOCUMENT_TYPE obj)
			{
            this.obj = (DOCUMENT_TYPE)obj.Clone();
			this.actionName = actionName;
			}
		}
	#endregion
	
	#region instance data
    Quack<UndoEvent> undoBuffer = new Quack<UndoEvent>();
    Stack<UndoEvent> redoBuffer = new Stack<UndoEvent>();
	
	private int _maxUndo=10;
	public int maxUndo
		{
		get
			{
			return _maxUndo;
			}
		set
			{
			_maxUndo = Range.clamp(value, 100);
			}
		}
	
	
	public event undoEventHandler onUndo;
	#endregion
	
	
	public UndoHandler(undoEventHandler onUndo)
		{
		this.onUndo = onUndo;
		}
		
	#region undo / redo
    public void undo(DOCUMENT_TYPE currentState)
		{
		if(canUndo())
			{
			UndoEvent state = (UndoEvent)undoBuffer.Pop();
			redoBuffer.Push(new UndoEvent(state.actionName, currentState));
            onUndo((DOCUMENT_TYPE)state.obj.Clone());
			}
		}

    public void redo(DOCUMENT_TYPE currentState)
		{
		if(canRedo())
			{
			UndoEvent state = redoBuffer.Pop();
			undoBuffer.Push(new UndoEvent(state.actionName, currentState));
            onUndo((DOCUMENT_TYPE)state.obj.Clone());
			}
		}
	
	public bool canUndo()
		{
		return (undoBuffer.Count > 0);
		}
	
	public bool canRedo()
		{
		return (redoBuffer.Count > 0);
		}

    public void setUndoPoint(DOCUMENT_TYPE obj)
		{
		setUndoPoint("last action", obj);
		}

    public void setUndoPoint(string name, DOCUMENT_TYPE obj)
		{
		//undoBuffer is inactive if maxUndo is set to 0
		if(maxUndo == 0)
			return;
		
		redoBuffer.Clear();
		if(undoBuffer.Count >= maxUndo)
			{
			if(undoBuffer.Count>0)
				{
				//undoBuffer.Pop();
				undoBuffer.Dequeue();
				}
			}
		try
			{
			undoBuffer.Push(new UndoEvent(name, obj));
			}
		catch(System.InvalidOperationException ex)
			{
			WDAppLog.logError(ErrorLevel.Error, "Undo is failing, critical memory shortage is likely", ex.Message);
			}
		}
	
	public string getUndoName()
		{
		if(canUndo())
			{
			return ((UndoEvent)undoBuffer.PeekTop()).actionName;
			}
		else
			return "";
		}
	
	public string getRedoName()
		{
		if(canRedo())
			{
			return ((UndoEvent)redoBuffer.Peek()).actionName;
			}
		else
			return "";
		}
	
	#endregion
	}
}
