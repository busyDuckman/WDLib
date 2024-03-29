/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections;
using System.Threading;

namespace WD_toolbox.Network
{
/// <summary>
/// A process to fetch data.
/// </summary>
internal class DownloadThread
	{
	#region instance data
	object data;
	
	/// <summary>
	/// Url of object to retrieve
	/// </summary>
	public readonly string url;
	private bool _highPriority;
	
	/// <summary>
	/// Is this retrival active or is it using idle bandwidth
	/// </summary>
	public bool highPriority
		{
		get
			{
			return _highPriority;
			}
		set
			{
			_highPriority = value;
			if(!isCompleted())
				requestedAtTime = DateTime.Now;
			}
		}
		
	/// <summary>
	/// Retrival has started
	/// </summary>
	bool started;
	
	/// <summary>
	/// Retrieval has failed
	/// </summary>
	bool failed;
	
	/// <summary>
	/// When was the last file time the file was modified
	/// </summary>
	DateTime fileTimeStamp;
	
	/// <summary>
	/// When was the last sucsessful download of this file
	/// </summary>
	DateTime dataLastRetrived;
	
	/// <summary>
	/// Locks the process to co-ordinate thread pooling
	/// </summary>
	public Mutex locked;
	
	private DateTime requestedAtTime;
	
	/// <summary>
	/// time of initial request or priority change (while not completed)
	/// </summary>
	public DateTime requestedAt
		{
		get
			{
			return requestedAtTime;
			}
		}
		
		
	private DateTime __lastRead = DateTime.MinValue;
	/// <summary>
	/// For a completed download, when data was last accessed or used
	/// </summary>
	public DateTime lastRead
		{
		get
			{
			return __lastRead;
			}
		}
	#endregion
	
	/// <summary>
	/// Creates a new fetchProcess (at active priority)
	/// </summary>
	/// <param name="url">url to fetch</param>
	public DownloadThread(string url) : this(url, true)
		{
		}

	/// <summary>
	/// Creates a new fetchProcess
	/// </summary>
	/// <param name="url">url to fetch</param>
	/// <param name="highPriority">Is this retrival active or is it using idle bandwidth</param>
	public DownloadThread(string url, bool highPriority)
		{
		this.url = url;
		this.highPriority = highPriority;
		data = null;
		started = false;
		failed = false;
		locked = new Mutex();
		requestedAtTime = DateTime.Now;
		}
	
	#region query accessors
	/// <summary>
	/// True if retrival has completed
	/// </summary>
	/// <returns></returns>
	public bool isCompleted()
		{
		return (data != null)||failed;
		}
	
	/// <summary>
	/// True if retrival has failed
	/// </summary>
	/// <returns></returns>
	public bool isFailed()
		{
		return failed;
		}
	
	/// <summary>
	/// True if retrival has started
	/// </summary>
	/// <returns></returns>
	public bool isStarted()
		{
		return started;
		}
	#endregion
	
	/// <summary>
	/// Get the retrived data
	/// </summary>
	/// <returns></returns>
	public object getData()
		{
		__lastRead = DateTime.Now;
		return data;
		}
	
	/// <summary>
	/// Sets the retrived data
	/// </summary>
	/// <param name="newData">Data</param>
	/// <param name="fileModifiedTimeStamp">Timestamp if aplicable</param>
	internal void setData(object newData, DateTime fileModifiedTimeStamp)
		{
		data = newData;
		dataLastRetrived = DateTime.Now;
		fileTimeStamp = fileModifiedTimeStamp;
		}
	
	/// <summary>
	/// Recheck url to see if timestamp has altered
	/// </summary>
	/// <param name="newModifiedTimeStamp"></param>
	/// <returns></returns>
	public bool hasObjectChanged(DateTime newModifiedTimeStamp)
		{
		return (newModifiedTimeStamp != fileTimeStamp);
		}
	
	#region flag conditions
	/// <summary>
	/// Set process started
	/// </summary>
	internal void flagStarted()
		{
		started = true;
		}
	
	/// <summary>
	/// Set process failed
	/// </summary>
	internal void flagFailed()
		{
		failed = true;
		}
	#endregion
	}
}
