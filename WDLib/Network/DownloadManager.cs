/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections;
using System.Threading;
using System.Net;
using System.IO;

using WD_toolbox.AplicationFramework;
using WD_toolbox.Maths.Range;

namespace WD_toolbox.Network
{
/// <summary>
/// Static class that controls the remote fetching of data from urls and network drives
/// </summary>
public class DownloadManager
	{
	const int NETIQUETTE_MAX_CONNECTIONS = 10;
	static volatile Hashtable fetchProcesses;
	static int maxRetreivalThreads = -1;
	static Thread[] retrivalThreads;
	
	static DownloadManager()
		{
		}
	
	/// <summary>
	/// inits the remote fetch handeler
	/// </summary>
	public static void init()
		{
		init(4);
		}
	
	/// <summary>
	/// inits the remote fetch handeler
	/// </summary>
	/// <param name="maxThreads"> How many downloads can happen simutaniously</param>
	public static void init(int maxThreads)
		{
		//can only init once
		if(maxRetreivalThreads != -1)
			return;
		
		fetchProcesses = new Hashtable();
		maxRetreivalThreads = Range.clamp(maxThreads,1, NETIQUETTE_MAX_CONNECTIONS);
		retrivalThreads = new Thread[maxRetreivalThreads];
		int i;
		for(i=0;i<retrivalThreads.Length;i++)
			{
			retrivalThreads[i] = new Thread(new ThreadStart(retreivalThread));
			retrivalThreads[i].Priority = ThreadPriority.BelowNormal;
			retrivalThreads[i].Start();
			}
		}
	
	/// <summary>
	/// Disposes of all retrival threads
	/// </summary>
	public static void Dispose()
		{
		int i;
		for(i=0;i<retrivalThreads.Length;i++)
			{
			retrivalThreads[i].Abort();
			}
		}
	
	#region data retrieval functions
	/// <summary>
	/// Enque the url for downloading (at normal priority)
	/// </summary>
	/// <param name="url">url to download</param>
	public static void prefetch(string url)
		{
		prefetch(url, true);
		}

	/// <summary>
	/// Enque the url for downloading
	/// </summary>
	/// <param name="url">url to download</param>
	/// <param name="highPriority">priority to use</param>
	public static void prefetch(string url, bool highPriority)
		{
		if(fetchProcesses.ContainsKey(url))
			{
			//update the process, as a priority change
			((DownloadThread)fetchProcesses[url]).highPriority = highPriority;
			}
		else
			{
			//create a new process
			DownloadThread f = new DownloadThread(url, highPriority);
			fetchProcesses.Add(url, f);
			}
		}
	
	
	/// <summary>
	/// Download a refreshed copy
	/// </summary>
	/// <param name="url">url to download</param>
	/// <param name="highPriority">priority to use</param>
	public static void refetch(string url, bool highPriority)
		{
		if(fetchProcesses.ContainsKey(url))
			{
			if(((DownloadThread)fetchProcesses[url]).isCompleted())
				{
				fetchProcesses.Remove(url);
				}
			else
				{
				//update the process, as a priority change
				((DownloadThread)fetchProcesses[url]).highPriority = highPriority;
				return;
				}
			}
		
		//create a new process
		DownloadThread f = new DownloadThread(url, highPriority);
		fetchProcesses.Add(url, f);
		}
	
	/// <summary>
	/// If the url has downloaded get the data retrived
	/// </summary>
	/// <param name="url">downloaded url</param>
	/// <returns>object data, null if url is not yet downloaded</returns>
	public static object getData(string url)
		{
		try
			{
			return ((DownloadThread)fetchProcesses[url]).getData();
			}
		catch
			{
			return null;
			}
		}
	
	/// <summary>
	/// Has the specified url been downloaded yet
	/// </summary>
	/// <param name="url">url in question</param>
	/// <returns>true if download is completed, false otherwise</returns>
	public static bool isDownloaded(string url)
		{
		try
			{
			return ((DownloadThread)fetchProcesses[url]).isCompleted();
			}
		catch
			{
			return false;
			}
		}
	
	#endregion
	
	#region retrieval threads
	protected static void retreivalThread()
		{
		string url=null;
		while(true)
			{
			//long wait for 1/10th of a second
			Thread.Sleep(100);
			
			lock(fetchProcesses)
				{
				url = getNextProcessToService();
				}
			
			//is there a url to service
			if(url != null)
				{
				DownloadThread f = (DownloadThread)fetchProcesses[url];
				
				//lock the fetchProcess and start retrieving
				if(f.locked.WaitOne(1000, false))
					{
					f.flagStarted();
					try
						{
						//TODO: must set up a single monitior thread to terminate any instances of this
						//		thread that lock indefinatly in this function call
						object data = retriveURL(f.url);
						if(data == null)
							{
							f.flagFailed();
							}
						else
							{
							//TODO: Parse in the actual file time stamp
							f.setData(data, DateTime.Now);
							}
						}
					finally
						{
						f.locked.ReleaseMutex();
						}
					}
				}
			
			}
		}
	
	/// <summary>
	/// Determines wich fetch request should be served next
	/// </summary>
	/// <returns>URL identifing the request</returns>
	private static string getNextProcessToService()
		{
		DateTime min = DateTime.Now + TimeSpan.FromDays(1000);
		String url=null;
		bool highPriorityServiceFound = false;
		
		//foreach (fetchProcess f in fetchProcesses)
		//int i;
		//for(i=0;i<fetchProcesses.Count;i++)
		foreach (DownloadThread f in fetchProcesses.Values)
			{
			//fetchProcess f = fetchProcesses.Values ;
			if(!(f.isStarted() || f.isCompleted()))
				{
				if(f.highPriority)
					{
					highPriorityServiceFound = true;
					}
				
				//dont check a low priority request id a high priorirty request has already been found
				if(!(highPriorityServiceFound &!  f.highPriority))
					{
					if(f.requestedAt < min)
						{
						//older request
						url = f.url;
						}
					}
				}
			}
		return url;
		}
	
	/// <summary>
	/// Remove fetched Items that have not been used in a certain perioud of time
	/// </summary>
	/// <param name="minAge">perioud of time used as a cull point</param>
	public static void clearUnusedDownloads(TimeSpan minAge)
		{
		DateTime cutoff = DateTime.Now - minAge;
		foreach (DownloadThread f in fetchProcesses)
			{
			if(f.lastRead != DateTime.MinValue)
				{
				if(f.lastRead < cutoff)
					{
					fetchProcesses.Remove(f.url);
					}
				}
			}
		}
	
	/// <summary>
	/// If there is no metedata for content type determination od the url then guess by url type
	/// </summary>
	/// <returns></returns>
	protected static bool doesURLHintText(string url)
		{
		string[] knownAsciiTextFormats = new string[] {".txt", ".rtf", ".htm", ".html"};
		string trimmedURL = url.Trim();
		foreach(string s in knownAsciiTextFormats)
			{
			if(trimmedURL.EndsWith(s))
				{
				return true;
				}
			}
		return false;
		}
	
	/// <summary>
	/// Function returns when object is downloaded or download fails
	/// </summary>
	/// <returns></returns>
	protected static object retriveURL(string url)
		{
		WebRequest  req = WebRequest.Create(url);
        WebResponse res = null;
		
		object data=null;
		
		bool downloadPossible = false;
		bool isText=false;
		string statusDescription; // HTTP status description, or exception message

         try
			{
			// Issue a response against the request.
			// will fail for many reasons
			res = req.GetResponse();
			//res.Headers.

			if (res is HttpWebResponse )
				{
				HttpStatusCode statusCode = ((HttpWebResponse)res).StatusCode;
				if(statusCode == HttpStatusCode.OK)
					{
					downloadPossible = true;
					//use the http content type metadata to determine of the object is text
					string contentType = ((HttpWebResponse)res).ContentType;
					
					if(contentType == null)
						{
						isText=doesURLHintText(res.ResponseUri.AbsoluteUri);
						}
					else if(contentType.Trim() == "")
						{
						isText=doesURLHintText(res.ResponseUri.AbsoluteUri);
						}
					if(contentType.Trim().StartsWith("text"))
						{
						isText=true;
						}
					}
				else
					{
					WDAppLog.logError(ErrorLevel.Error, 
                        String.Format("Could not retive url {0}", url), 
                        statusCode.ToString());
					}					
				statusDescription = ((HttpWebResponse)res).StatusDescription;
				}
			
			if (res is FileWebResponse )
				{
				//anything > 64mb has to be written to file
				if(((FileWebResponse)res).ContentLength > (64*1024*1024))
					{
					}
				else
					{
					downloadPossible = true;
					isText=doesURLHintText(res.ResponseUri.AbsoluteUri);
					}
				}

			if (downloadPossible)
				{
				// Read the contents into our state 
				// object and fire the content handlers
				if(isText)
					{
					//download text
					StreamReader sr=null;
					try
						{
						sr = new StreamReader(res.GetResponseStream());
						data = sr.ReadToEnd();
						}
					catch
						{
						data = null;
						}
					if(sr!=null)
						sr.Close();
					}
				else
					{
					//download file
					Stream sr=null;
					try
						{
						sr = res.GetResponseStream();
						try
							{
							//try sr.Length for accelerated download
							byte[] rawData = new byte[sr.Length];
							sr.Read(rawData, 0, rawData.Length);
							data = rawData;
							}
						catch
							{
							//try sr.Length was not available?
							
							BinaryReader bs = new BinaryReader(sr);
							int bufferSize = 1024*1024*5;
							byte[] rawData = new byte[bufferSize];
							ArrayList al = new ArrayList();
							try{
							
							while(rawData.Length == bufferSize)
								{
								rawData = bs.ReadBytes(bufferSize);
								al.AddRange(rawData);
								}
							
							data = new byte[al.Count];
							al.CopyTo((byte[])data);
							//data = rawData;
							
							}
							catch(Exception ex2)
								{
								data = null;
								WDAppLog.logError(ErrorLevel.Error, "Could not load downloaded object", ex2.Message);
								}
							}						
						}
					catch
						{
						data = null;
						}
					if(sr!=null)
						sr.Close();
					}
				}
			}
		catch
			{
			}
		finally
			{
			if ( res != null )
				{
				res.Close();
				}
			}
		return data;
		}
	#endregion
	}
}
