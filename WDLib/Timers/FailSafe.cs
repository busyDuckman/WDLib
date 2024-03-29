/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Threading;

namespace WD_toolbox.Timers
{
	/// <summary>
	/// Does something if it is not pinged in a set period of time.
	/// Used to handle stalled processes
	/// </summary>
	public class FailSafe : IDisposable
	{
	/// <summary>
	/// delgate definition for event to execute when failsafe triggers.
	/// </summary>
	public delegate void failSafeEvent();
	
	/// <summary>
	/// If true, kills all threads.
	/// </summary>
	protected static volatile bool disposeAll = false;
	
	/// <summary>
	/// Event to execute when failsafe triggers.
	/// </summary>
	protected event failSafeEvent onNoPing = null;
	
	/// <summary>
	/// Failsafe monitoring thread.
	/// </summary>
	protected Thread monitorPing = null;
	
	private DateTime LastPing;
	/// <summary>
	/// Time before failsafe triggers
	/// </summary>
	public readonly TimeSpan idleTimeOut;
	
	/// <summary>
	/// Creates a new failSafe
	/// </summary>
	/// <param name="idleTimeOut">Time to wait before failsafe triggers</param>
	/// <param name="onNoPing">null OR an event to trigger (will cause a thread to be created, the dispose method should be used on this type of failsafe) </param>
	public FailSafe(TimeSpan idleTimeOut, failSafeEvent onNoPing)
		{
		this.idleTimeOut = idleTimeOut;
		LastPing = DateTime.Now;
		this.onNoPing = onNoPing;
		if(this.onNoPing != null)
			{
			monitorPing = new Thread(new ThreadStart(timeOutThread));
			monitorPing.Start();
			}
		}
	
	/// <summary>
	/// A quick way to kill all failsafes
	/// </summary>
	public static void shutDownAllFailSafes()
		{
		disposeAll = true;
		}
	
	/// <summary>
	/// Presses the failsafe button preventing time out, for predefined idle Time Out amount of time
	/// </summary>
	public void ping()
		{
		LastPing = DateTime.Now;
		}
	
	private void timeOutThread()
		{
		TimeSpan timeSincePing = DateTime.Now - LastPing;
		
		while((timeSincePing < idleTimeOut) && (!disposeAll))
			{
			Thread.Sleep(1000);
			timeSincePing = DateTime.Now - LastPing;
			}
		
		if(!disposeAll)
			this.onNoPing();
		
		}
		
		#region IDisposable Members

		/// <summary>
		/// Disposes of failsafe and related threads
		/// </summary>
		public void Dispose()
			{
			if(monitorPing != null)
				{
				if(monitorPing.IsAlive)
					monitorPing.Abort();
				}	
			}

		#endregion
	}
}
