/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections;
using System.Runtime.Serialization;

using WD_toolbox.AplicationFramework;

namespace WD_toolbox.Data.DataStructures
{
/// <summary>
/// PagingCache is a swapfile system that uses a regeneration delegate to regenerate paged out data.
/// </summary>
[Serializable]
public class PagingCache : IDisposable, IList, ISerializable, ICollection, IEnumerable, ICloneable
	{
	#region datatypes
	/// <summary>
	/// Paging Algorithms, FILO = "First In last out", LRU = "least recently used", 
	/// </summary>
	public enum pagingAlgorithems {FILO=0, LRU};
	
	public delegate object pageInHandeler(object info);
	public delegate void pageOutHandeler(object info, object data);
	public delegate ulong pageSizeHandeler(object info, object data);
	
	protected class proxieObject
		{
		/// <summary>
		/// USING THIS DIRECTLY WILL NOT STAMP THE LRU
		/// Use getData unlesss not stamping the lru is nessesary
		/// </summary>
		private object data;
		public object regenerationInfo;
		//This is used so null values can be stored
		private bool cached;
		
		public DateTime lastUsed;
		
		public proxieObject(object data, object regenerationInfo)
			{
			this.data = data;
			lastUsed = DateTime.Now;
			this.regenerationInfo = regenerationInfo;
			cached = true;
			}
		
		public proxieObject(object regenerationInfo)
			{
			this.data = null;
			this.regenerationInfo = regenerationInfo;
			cached = false;
			lastUsed = DateTime.Now;
			}
		
		/*public static proxieObject empty()
			{
			return new proxieObject(null, null);
			}
		
		/// <summary>
		/// Returns true is this object does not represent anything
		/// </summary>
		/// <returns></returns>
		public bool isEmpty()
			{
			return (regenerationInfo == null);
			}*/
		
		/// <summary>
		/// Returns true if there is a chache of the object present
		/// </summary>
		/// <returns></returns>
		public bool isCached()
			{
			return cached;
			}
			
		/// <summary>
		/// detstorys the paged data
		/// </summary>
		public void pageout()
			{
			data = null;
			cached = false;
			}
		
		public void pagein(object data)
			{
			this.data = data;
			cached = true;
			lastUsed = DateTime.Now;
			}
		
		public object getData()
			{
			lastUsed = DateTime.Now;
			return data;
			}
		
		/// <summary>
		/// Gets data without affecting LRU stamp
		/// </summary>
		/// <returns></returns>
		public object peekData()
			{
			return data;
			}
		}
	#endregion
	
	#region instance data
	protected ArrayList pages;
	
	public readonly bool usesPriority;
	public readonly pagingAlgorithems pagingAlgorithem;
	
	#region depreciated LRU code 
	/*private TimeSpan _LRUTimeOut;
	public TimeSpan LRUTimeOut
		{
		get
			{
			return _LRUTimeOut;
			}
		set
			{
			if(value <= TimeSpan.Zero)
				{
				smartObject.logError(3, "Can not have negative or 0 timespan for LRU");
				return;
				}
			_LRUTimeOut = value;
			}
		}*/
	#endregion
	
	/// <summary>
	/// Size in bytes of the cache
	/// </summary>
	protected ulong _cacheSizeBytes;
	public ulong cacheSizeBytes
		{
		get
			{
			return _cacheSizeBytes;
			}
		set
			{
			if(value > 0)
				{
				_cacheSizeBytes = value;
				}
			}
		}
	
	/// <summary>
	/// Magabyes used, for more acurete reading use the bytes version
	/// </summary>
	public int cacheSizeMB
		{
		get
			{
			return (int)(_cacheSizeBytes / (1024*1024));
			}
		set
			{
			if(value > 0)
				{
				_cacheSizeBytes = ((ulong)value) * (1024*1024);
				}
			}
		}
	
	
	/// <summary>
	/// Event to retrive page data given page metadata (manatory exent)
	/// </summary>
	public event pageInHandeler pageInEvent;
	/// <summary>
	/// (Optional) Fired when a page is deallocated or when the proxieCache is being disposed
	/// </summary>
	public event pageOutHandeler pageOutEvent;
	/// <summary>
	/// (Optional) Fired when it is nessesary to know the size of the page for purposes of memory management
	/// </summary>
	public event pageSizeHandeler pageSizeEvent;
	#endregion
	
	private ulong currentCacheSizeProxy;
	public ulong currentCacheSize
		{
		get
			{
			if(currentCacheSizeProxy != ulong.MaxValue)
				{
				return currentCacheSizeProxy;
				}
			else
				{
				ulong total=0;
				foreach(proxieObject p in pages)
					{
					total += getSize(p);
					}
				currentCacheSizeProxy = total;
				return total;
				}
			}
		}
	
	protected void invalidateCacheSizeProxy()
		{
		currentCacheSizeProxy = ulong.MaxValue;
		}
	
	
	#region constructors
	public PagingCache(pagingAlgorithems pagingAlgorithem, bool usesPriority, pageInHandeler pageInEvent)
		{
		this.pagingAlgorithem = pagingAlgorithem;
		this.usesPriority = usesPriority;
		this.pageInEvent = pageInEvent;
		pageOutEvent = null;
		pageSizeEvent = null;
		pages = new ArrayList();
		}
	
	protected PagingCache(PagingCache b)
		{
		this.cacheSizeBytes = b.cacheSizeBytes;
		this.pages = (ArrayList)b.pages.Clone();
		this.pagingAlgorithem = b.pagingAlgorithem;
		this.usesPriority = b.usesPriority;
		this.pageInEvent = b.pageInEvent;
		this.pageOutEvent = b.pageOutEvent;
		this.pageSizeEvent = b.pageSizeEvent;
		}
	
	#endregion
	
	#region memory managment support routienes
	protected void clean(ulong bytesToMakeRoomFor, int[] pagesToLock)
		{
		invalidateCacheSizeProxy();
		DateTime oldest = DateTime.Now;
		int oldestPos=-1;
		int pageLockSize=0;
		if(pagesToLock != null)
			pageLockSize = pagesToLock.Length;
		
				
		while(((currentCacheSize + bytesToMakeRoomFor) > cacheSizeBytes) && (this.Count > pageLockSize))
			{
			//find a page to page out
			foreach(proxieObject p in pages)
				{
				if(p.lastUsed < oldest)
					{
					bool notLocked = true;
					int index = pages.IndexOf(p);
					int i;
					
					//check if the page we found is locked
					if(pagesToLock != null)
						{
						for(i=0;i<pagesToLock.Length;i++)
							{
							if(index == pagesToLock[i])
								notLocked = false;
							}
						}
					
					if(notLocked)
						{
						//register this page as the oldest
						oldest = p.lastUsed;
						oldestPos = index;
						}
					}
				}
			
			//page out the page
			pageout(oldestPos, true);
			}
		
		//good time to do a garbage collect
		GC.Collect();
		}
	
	protected ulong getSize(proxieObject page)
		{
		//use the size event if there is one
		if(pageSizeEvent == null)
			return (ulong)System.Runtime.InteropServices.Marshal.SizeOf(page.peekData());
		else
			return pageSizeEvent(page.regenerationInfo, page.peekData());
		}
	
	#endregion
	
	#region page ins and outs
	protected object getData(int index)
		{
		if(((proxieObject)pages[index]).isCached())
			{
			return ((proxieObject)pages[index]).getData();
			}
		else
			{
			invalidateCacheSizeProxy();
			((proxieObject)pages[index]).pagein(pageInEvent(((proxieObject)pages[index]).regenerationInfo));
			ulong newSize = getSize(((proxieObject)pages[index]));
			if((newSize + currentCacheSize) > cacheSizeBytes)
				{
				clean(newSize, new int[]{index});
				}
			object ret = ((proxieObject)pages[index]).getData();
			return ret;	
			}
		}
	
	protected void pageout(int index)
		{
		pageout(index, false);
		}
	
	protected void pageout(int index, bool deferGCHint)
		{
		if(pageOutEvent!=null)
			{
			pageOutEvent(((proxieObject)pages[index]).regenerationInfo, ((proxieObject)pages[index]).getData());
			}	
		
		((proxieObject)pages[index]).pageout();
		if(!deferGCHint)
			GC.Collect();
		}
	
	protected void addPage(object metaData)
		{
		invalidateCacheSizeProxy();
		pages.Add(new proxieObject(metaData));
		}
	
	#endregion
	
	#region data peaking
	/// <summary>
	/// Peeks at meta data without retrieving object
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public object getMetaData(int index)
		{
		return ((proxieObject)pages[index]).regenerationInfo;
		}
	#endregion
	
	#region ICloneable Members
	/// <summary>
	/// Makes a clone of this object.
	/// </summary>
	/// <returns>clone of this object</returns>
	public object Clone()
		{
		return new PagingCache(this);
		}

	#endregion

	#region ICollection Members

	public bool IsSynchronized
	{
		get
		{
			// TODO:  Add proxieCache.IsSynchronized getter implementation
			return false;
		}
	}

	public int Count
	{
		get
		{
			return pages.Count;
		}
	}

	public void CopyTo(Array array, int index)
	{
		// TODO:  Add proxieCache.CopyTo implementation
	}

	public object SyncRoot
	{
		get
		{
			// TODO:  Add proxieCache.SyncRoot getter implementation
			return null;
		}
	}

	#endregion

	#region IEnumerable Members
	IEnumerator System.Collections.IEnumerable.GetEnumerator()
	{
		// TODO:  Add proxieCache.System.Collections.IEnumerable.GetEnumerator implementation
		return null;
	}
	#endregion

	#region ISerializable Members

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		// TODO:  Add proxieCache.GetObjectData implementation
	}

	#endregion

	#region IDisposable Members
	public void Dispose()
		{
		int i;
		for(i=0;i<pages.Count;i++)
			{
			pageout(i, true);
			}
		}

	#endregion

	#region IList Members

	public bool IsReadOnly
		{
		get
			{
			return false;
			}
		}
		
	object System.Collections.IList.this[int index]
		{
		get
			{
			return getData(index);
			}
		set
			{
			invalidateCacheSizeProxy();
			if(((proxieObject)pages[index]).isCached())
				{
				pageout(index);
				}
			((proxieObject)pages[index]).pagein(value);
			}
		}

	public void RemoveAt(int index)
		{
		pageout(index);
		pages.RemoveAt(index);
		invalidateCacheSizeProxy();
		}

	public void Insert(int index, object value)
		{
            WDAppLog.logCallToMethodStub();
		}

	public void Remove(object value)
		{
		int i;
		for(i=0;i<pages.Count;i++)
			{
			if(((proxieObject)pages[i]).peekData() == value)
				{
				RemoveAt(i);
				return;
				}
			}
		}

	public bool Contains(object value)
	{
		// TODO:  Add proxieCache.Contains implementation
        WDAppLog.logCallToMethodStub();
		return false;
	}

	public void Clear()
	{
		// TODO:  Add proxieCache.Clear implementation
        WDAppLog.logCallToMethodStub();
	}

	public int IndexOf(object value)
	{
		// TODO:  Add proxieCache.IndexOf implementation
        WDAppLog.logCallToMethodStub();
		return 0;
	}

	public int Add(object value)
	{
		addPage(value);
		invalidateCacheSizeProxy();
		return 0;
	}

	public bool IsFixedSize
	{
		get
		{
			return false;
		}
	}

	#endregion
		
	/// <summary>
	/// Because the stupid [] operator breaks between namespaces or something
	/// </summary>
	/// <returns></returns>
	public object getItem(int index)
		{
		return ((IList)this)[index];
		}
	
	/// <summary>
	/// Because the stupid [] operator breaks between namespaces or something
	/// </summary>
	/// <returns></returns>
	public void setItem(int index, object item)
		{
		((IList)this)[index] = item;
		}
	}
}
