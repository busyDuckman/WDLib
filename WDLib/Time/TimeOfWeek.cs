/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace WD_toolbox.TimeRepresentation
{
/// <summary>
/// represents a time of the week, day + 24 hr format, to 1 second accuracy, (no date info)
/// </summary>
public class TimeOfWeek : TimeOfDay
	{
	
	public static long milliSecondsPerWeek = TimeOfDay.milliSecondsPerDay*7;
	
	/// <summary>
	/// Overrides the overflow detection to cange the behaviour of the class
	/// </summary>
	protected override void testForOverflow()
		{
		//is the time span longer than one day
		if(TotalMilliseconds > TimeOfWeek.milliSecondsPerWeek)
			{
			triggerOverFlow();
			}
		while(TotalMilliseconds > TimeOfWeek.milliSecondsPerWeek)
			{
			this.TotalMilliseconds -= TimeOfWeek.milliSecondsPerWeek;
			}
		if(TotalMilliseconds < 0)
			{
			triggerOverFlow();
			}
		while(TotalMilliseconds < 0)
			{
			this.TotalMilliseconds += TimeOfWeek.milliSecondsPerWeek;
			}
		}
	/// <summary>
	/// Gets or sets the number of whole days represented by this instance.
	/// </summary>
	public int Days
		{
		get
			{
			return ts.Days;
			}
		set
			{
			resetOverFlow();
			ts = ts.Subtract(new TimeSpan(Days,0,0,0,0));
			ts.Add(new TimeSpan(Days,0,0,0,0));
			testForOverflow();
			}
		}
	
	/// <summary>
	/// Gets or sets the value of this instance expressed in whole and fractional days.
	/// </summary>
	public double TotalDays
		{
		get
			{
			return ts.TotalDays;
			}
		set
			{
			resetOverFlow();
			ts = TimeSpan.FromDays(value);
			testForOverflow();
			}
		}
	
	public TimeOfDay getTimeOfDay()
		{
		TimeOfDay t = new TimeOfDay();
		// time of day will automaticaly make a valid day time from this
		t.TotalMilliseconds = TotalMilliseconds;
		return t;
		}
		
	public TimeOfWeek()
		{
		}
	public TimeOfWeek(int day, TimeOfDay time)
		{
		//let the accesor validate the numbers
		Milliseconds = (day * (int)TimeOfDay.milliSecondsPerDay) + (int)time.TotalMilliseconds;
		}
	
	}
}
