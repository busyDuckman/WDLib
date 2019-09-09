/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Runtime.InteropServices;

//using WD_toolbox.Audio;

namespace WD_toolbox.Hardware
{
/// <summary>
/// powerSave functionality.
/// </summary>
public abstract class PowerSave
	{
	/// <summary>
	/// Sends a windows message
	/// </summary>
	/// <param name="hWnd"></param>
	/// <param name="Msg"></param>
	/// <param name="wParam"></param>
	/// <param name="lParam"></param>
	/// <returns></returns>
	[DllImport("user32.dll")]
	static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
	
	/// <summary>
	/// Status of the PC's display devices.
	/// </summary>
	public enum monitorStatus {
		/// <summary>
		/// Monitor is on.
		/// </summary>
		MONITOR_ON = -1,
		/// <summary>
		/// Monitor is in standby mode.
		/// </summary>
		MONITOR_STANDBY  = 1,
		/// <summary>
		/// Monitor is off.
		/// </summary>
		MONITOR_OFF = 2};
	
	/// <summary>
	/// True if the monitor is on.
	/// </summary>
	protected static bool _monitorOn=true;
	/// <summary>
	/// Returns true if the monitor is on.
	/// </summary>
	public static bool monitorOn
		{
		get
			{
			return _monitorOn;
			}
		}
	
	/// <summary>
	/// Turns monitors on and off
	/// </summary>
	/// <param name="status">Statis of monitor required</param>
	/// <param name="Handle">Aplication requesting power state</param>
	/// <param name="quite">If false audiable noise will indicate power event</param>
	public static void setMonitorState(monitorStatus status, IntPtr Handle, bool quite)
			{
			uint WM_SYSCOMMAND = 0x0112;
			int SC_MONITORPOWER  = 0xF170;
			
			_monitorOn = (status == monitorStatus.MONITOR_ON);
			
			SendMessage(Handle, WM_SYSCOMMAND, SC_MONITORPOWER, (int)status);
			}
	
	/// <summary>
	/// Turns monitors on and off, no audiable noise will indicate power event.
	/// </summary>
	/// <param name="status">Statis of monitor required</param>
	/// <param name="Handle">Aplication requesting power state</param>
	public static void setMonitorState(monitorStatus status, IntPtr Handle)
		{
		setMonitorState(status, Handle, true);
		}
	}
}
