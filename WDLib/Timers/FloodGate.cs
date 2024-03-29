/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;

namespace WD_toolbox.Timers
{
/// <summary>
/// Times durations between operations, used to stop some denial of service attacks and prevent resource hogging.
/// </summary>
public class FloodGate
	{
	//public Stopwatch sw;
	DateTime lastOperation;
	TimeSpan maximumFrequency;
	
	/// <summary>
	/// Creates a flood gate
	/// </summary>
	/// <param name="milliSecondsFrequency">Maximum freqency at wich operations should be performed in miliseconds</param>
	public FloodGate(int milliSecondsFrequency) : this(new TimeSpan(0,0,0,0, milliSecondsFrequency))
		{
		
		}
	
	/// <summary>
	/// Creates a flood gate
	/// </summary>
	/// <param name="maximumFrequency">Maximum freqency at wich operations should be performed</param>
	public FloodGate(TimeSpan maximumFrequency)
		{
		//sw = new Stopwatch();
		this. maximumFrequency = maximumFrequency;
		lastOperation = DateTime.Now;
		}
	
	/// <summary>
	/// Gets the state of the floodgate as well as pinging the floodgate at the same time.
	/// </summary>
	/// <returns>true if the operation should be allowed</returns>
	/// <example>
	/// if(myFloodGate.allowOperation())
	///		doSomething();
	///</example>
	public bool allowOperation()
		{
		DateTime now = DateTime.Now;
		if((now -lastOperation) >= maximumFrequency)
			{
			lastOperation = now;
			return true;
			}
		return false;
		}
	
	
	}
}
