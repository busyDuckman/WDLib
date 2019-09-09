/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using WD_toolbox.Maths.Range;

//using WD_toolbox.AplicationFramework;

namespace WD_toolbox.TimeRepresentation
{
	/// <summary>
	/// represents a time of the day, 24 hr format, to 1 second accuracy, (no date info)
	/// </summary>
	public class TimeOfDay
	{
	
	public static long milliSecondsPerDay = 1000*60*60*24;
	/// <summary>
	/// Time since 00:00
	/// </summary>
	protected TimeSpan ts;
	
	private bool _overflow;
	/// <summary>
	/// true if the 
	/// </summary>
	public bool overflow
		{
		get
			{
			return _overflow;
			}
		}
	
	#region overflow code
	protected void triggerOverFlow()
		{
		_overflow = true;
		}
	
	protected void resetOverFlow()
		{
		_overflow=false;
		}
	
	protected virtual void testForOverflow()
		{
		//is the time span longer than one day
		if(TotalMilliseconds > TimeOfDay.milliSecondsPerDay)
			{
			triggerOverFlow();
			}
		while(TotalMilliseconds > TimeOfDay.milliSecondsPerDay)
			{
			this.TotalMilliseconds -= TimeOfDay.milliSecondsPerDay;
			}
		if(TotalMilliseconds < 0)
			{
			triggerOverFlow();
			}
		while(TotalMilliseconds < 0)
			{
			this.TotalMilliseconds += TimeOfDay.milliSecondsPerDay;
			}
		}
	#endregion
	
	#region accessors
	/// <summary>
	/// Gets or sets the number of whole Hours represented by this instance.
	/// </summary>
	public int Hours
		{
		get
			{
			return ts.Hours;
			}
		set
			{
			resetOverFlow();
			ts = ts.Subtract(new TimeSpan(0,Hours,0,0,0));
			ts.Add(new TimeSpan(0,value,0,0,0));
			testForOverflow();
			}
		}

	/// <summary>
	/// Gets or sets the number of whole Milliseconds represented by this instance.
	/// </summary>
	public int Milliseconds
		{
		get
			{
			return ts.Milliseconds;
			}
		set
			{
			resetOverFlow();
			ts = ts.Subtract(new TimeSpan(0,0,0,0,Milliseconds));
			ts.Add(new TimeSpan(0,0,0,0,value));
			testForOverflow();
			}
		}

	/// <summary>
	/// Gets or sets the number of whole Minutes represented by this instance.
	/// </summary>
	public int Minutes
		{
		get
			{
			return ts.Minutes;
			}
		set
			{
			resetOverFlow();
			ts = ts.Subtract(new TimeSpan(0,0,Minutes,0,0));
			ts.Add(new TimeSpan(0,0,value,0,0));
			testForOverflow();
			}
		}

	/// <summary>
	/// Gets or sets the number of whole Seconds represented by this instance.
	/// </summary>
	public int Seconds
		{
		get
			{
			return ts.Seconds;
			}
		set
			{
			resetOverFlow();
			ts = ts.Subtract(new TimeSpan(0,0,0,Seconds,0));
			ts.Add(new TimeSpan(0,0,0,value,0));
			testForOverflow();
			}
		}


	/// <summary>
	/// Gets or sets the value of this instance expressed in whole and fractional Hours.
	/// </summary>
	public double TotalHours
		{
		get
			{
			return ts.TotalHours;
			}
		set
			{
			resetOverFlow();
			ts = TimeSpan.FromHours(value);
			testForOverflow();
			}
		}

	/// <summary>
	/// Gets or sets the value of this instance expressed in whole and fractional Milliseconds.
	/// </summary>
	public double TotalMilliseconds
		{
		get
			{
			return ts.TotalMilliseconds;
			}
		set
			{
			resetOverFlow();
			ts = TimeSpan.FromMilliseconds(value);
			testForOverflow();
			}
		}

	/// <summary>
	/// Gets or sets the value of this instance expressed in whole and fractional Minutes.
	/// </summary>
	public double TotalMinutes
		{
		get
			{
			return ts.TotalMinutes;
			}
		set
			{
			resetOverFlow();
			ts = TimeSpan.FromMinutes(value);
			testForOverflow();
			}
		}

	/// <summary>
	/// Gets or sets the value of this instance expressed in whole and fractional Seconds.
	/// </summary>
	public double TotalSeconds
		{
		get
			{
			return ts.TotalSeconds;
			}
		set
			{
			resetOverFlow();
			ts = TimeSpan.FromSeconds(value);
			testForOverflow();
			}
		}

	#endregion
		
	
	public TimeOfDay()
		{
		ts = new TimeSpan(0);
		testForOverflow();
		}
	
	public TimeOfDay(int hours, int mins, int sec)
		{
		ts = new TimeSpan(0, Range.clampIfBelow(hours, 0), Range.clampIfBelow(mins, 0),Range.clampIfBelow(sec, 0), 0);
		testForOverflow();
		}
		
	public string toString()
		{
		DateTime dt = new DateTime(0,0,0,Hours, Minutes, Seconds, Milliseconds);
		//TODO: this formating is incorect, t will display day and month as well
		return dt.ToShortTimeString();
		}

	#region operators
	public static TimeOfDay operator + (TimeOfDay a, TimeSpan b)
		{
		TimeOfDay c = a;
		c.TotalMilliseconds += (int)b.TotalMilliseconds;
		return c;
		}
	
	public static TimeOfDay operator - (TimeOfDay a, TimeSpan b)
		{
		TimeOfDay c = a;
		c.TotalMilliseconds -= (int)b.TotalMilliseconds;
		return c;
		}
	
	public static bool operator < (TimeOfDay a, TimeOfDay b)
		{
		return (a.ts.TotalMilliseconds < b.ts.TotalMilliseconds);
		}
	
	public static bool operator > (TimeOfDay a, TimeOfDay b)
		{
			return (a.ts.TotalMilliseconds > b.ts.TotalMilliseconds);
		}
	
	/*public static bool operator == (timeOfDay a, timeOfDay b)
		{
			return (a.ts == b.ts);
		}
	
	public static bool operator != (timeOfDay a, timeOfDay b)
		{
			return (a.ts != b.ts);
		}*/
	
	public static bool operator <= (TimeOfDay a, TimeOfDay b)
		{
			return (a.ts <= b.ts);
		}
	
	public static bool operator >= (TimeOfDay a, TimeOfDay b)
		{
			return (a.ts >= b.ts);
		}
	#endregion
	
	
	#region testing
	private static bool testValid(TimeOfDay a)
		{
		if(a.TotalMilliseconds > TimeOfDay.milliSecondsPerDay)
			return false;
		if(a.TotalMilliseconds < 0)
				return false;
		
		//should not have any stay milliseconds (whole seconds only)
		if(a.Milliseconds != 0)
			return false;
		return true;
		}
	
	private delegate bool evaluateObjectDelegate(TimeOfDay b);

	private static bool evaluateObject(ref TimeOfDay obj, evaluateObjectDelegate testFunction, ref string errorMessage, string onError, ref int errorCount)
		{
		if(!testFunction(obj))
			{
			errorCount++;
			errorMessage += onError + "\r";
			return false;
			}
		return true;
		}
	
	private static bool comparisonTest(TimeOfDay a, TimeOfDay b)
		{
		int testCount=0;
		if(a > b)
			testCount++;
		if(a < b)
			testCount++;
		if(a == b)
			testCount++;
		
		return (testCount==1);
		}
	
	public static string _blackBoxTest()
		{
			TimeOfDay a, b, c;
			int i;
			string message = "timeOfDay test\r";
			Random r = new Random(0xffffff);
			int errCount=0;
			int testLength=10000;
			int maxErrors = 10;
			evaluateObjectDelegate objectValidTest = new evaluateObjectDelegate(testValid);
			
			message += "general testing\r";
			
			for(i=0;(i<testLength)&&(errCount<maxErrors);i++)
				{
				a = new TimeOfDay(r.Next(-124,124), r.Next(-160, 160),r.Next(-160, 160));
				b = new TimeOfDay(r.Next(0,24), r.Next(0, 60),r.Next(0, 60));
				c = new TimeOfDay(r.Next(0,24), r.Next(0, 60),r.Next(0, 60));
				evaluateObject(ref a, objectValidTest, ref message, "invalid object", ref errCount);
				evaluateObject(ref b, objectValidTest, ref message, "invalid object", ref errCount);
				evaluateObject(ref c, objectValidTest, ref message, "invalid object", ref errCount);
				
				//test comarisons
				if(!comparisonTest(a, b) && comparisonTest(a, c) && comparisonTest(b, c))
					{
					message += " could not make valid < > == comparisions\r";
					errCount++;
					}
				
				//test overflow
				TimeSpan ts = new TimeSpan(0,24,0,0);
				TimeOfDay t = a;
				t += ts;
				if((!t.overflow)||(!testValid(t)))
					{
					message += "time failed to overflow\r";
					errCount++;
					}
				}
			if(errCount>=maxErrors)
				{
				message += String.Format("testing halted because of to many errors (max errors = {0})\r", maxErrors);
				}
			message += String.Format("{0} errors in {1} tests\r", errCount, i);
			errCount=0;
			return message;
		}
	#endregion
}
	
}
