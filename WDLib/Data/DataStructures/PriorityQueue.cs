/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox;

namespace WD_toolbox.Data.DataStructures
{
    public class PriorityQueue<T> : IEnumerable<T>//, ICollection<T>
        where T : IEquatable<T>
    {
        public enum PriorityAlgorithems {FILO=0, FIFO, LRU, MRU, Random, ManualPriority, LeastFrequentlyUsed, MostFrequentlyUsed};
        public enum UseageMechanisims {Read, Requeue, ReadPlusRequeue};

        public delegate int GetPriorityDelegate(T item);

        class ItemInfo
        {
            public DateTime lastQueeued {get; set;}
            public DateTime lastRead {get; set;}
            public int reads { get; set; }
            public int requeues { get; set; }
            public int lastPriorityQuerey { get; set; }
            internal int queueIndex{ get; set; }

            public ItemInfo() : this(0) {}

            public ItemInfo(int priority)
            {
                lastQueeued = DateTime.Now;
                lastRead = lastQueeued;
                reads = 0;
                requeues =0;
                lastPriorityQuerey = priority;
            }
        }

        private int UseMetric(ItemInfo itemInfo) 
        {
            switch (useageMechanisim)
            {
                case UseageMechanisims.Read:
                    return itemInfo.reads;
                case UseageMechanisims.Requeue:
                    return itemInfo.requeues;
                case UseageMechanisims.ReadPlusRequeue:
                    return itemInfo.reads + itemInfo.requeues;
                default:
                    goto case UseageMechanisims.Read;
            }
        }

        private DateTime timeMetric(ItemInfo itemInfo, bool LRU)
        {
            switch (useageMechanisim)
            {
                case UseageMechanisims.Read:
                    return itemInfo.lastRead;
                case UseageMechanisims.Requeue:
                    return itemInfo.lastQueeued;
                case UseageMechanisims.ReadPlusRequeue:
                    return LRU ? 
                                itemInfo.lastRead.Min(itemInfo.lastQueeued)
                                :
                                itemInfo.lastRead.Max(itemInfo.lastQueeued);
                default:
                    goto case UseageMechanisims.Read;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        //Instance data
        //-------------------------------------------------------------------------------------------------------
        
        //this queue is alway in order added
        //new ItemInfo always added to end, items removed from anywhere
        List<T> queue;
        PriorityAlgorithems type;
        UseageMechanisims useageMechanisim;
        //highest to lowest
        List<int> orderLut;
        List<ItemInfo> priorityLut;
        GetPriorityDelegate getPriority;
        

        public PriorityAlgorithems Type
        {
          get { return type; }
          set { type = value; RebuildOrderLut();}
        }

        public UseageMechanisims UseageMechanisim
        {
          get { return useageMechanisim; }
          set { useageMechanisim = value; RebuildOrderLut();}
        }

        //-------------------------------------------------------------------------------------------------------
        // Constructors
        //-------------------------------------------------------------------------------------------------------
        public PriorityQueue() : this(PriorityAlgorithems.LRU)
        {
        }

        public PriorityQueue(IEnumerable<T> collection) : this(collection, PriorityAlgorithems.LRU)
        {
        }
        
        public PriorityQueue(int capacity) : this(capacity, PriorityAlgorithems.LRU)
        {
        }

        public PriorityQueue(PriorityAlgorithems type)
        {
            init(type);
        }

        public PriorityQueue(IEnumerable<T> collection, PriorityAlgorithems type)
        {
            init(type);
            foreach (T item in collection)
            {
                Requeue(item);
            }   
        }

        public PriorityQueue(int capacity, PriorityAlgorithems type)
        {
            init(type, capacity);
        }

        public PriorityQueue(GetPriorityDelegate manualPpriority)
        {
            init(PriorityAlgorithems.ManualPriority);
            getPriority = manualPpriority;
        }

        public PriorityQueue(IEnumerable<T> collection, GetPriorityDelegate manualPpriority)
        {
            init(PriorityAlgorithems.ManualPriority);
            getPriority = manualPpriority;

            foreach (T item in collection)
            {
                Requeue(item);
            }
        }

        public PriorityQueue(int capacity, GetPriorityDelegate manualPpriority)
        {
            init(type, capacity);
            getPriority = manualPpriority;
        }

        private void init(PriorityAlgorithems type, int? capacity=null)
        {
            queue = (capacity == null) ? new List<T>() : new List<T>(capacity.Value);
            priorityLut = new List<ItemInfo>();
            Type = type;
            RebuildOrderLut();
        }

        //-------------------------------------------------------------------------------------------------------
        // Internal methods
        //-------------------------------------------------------------------------------------------------------
        public void RebuildOrderLut()
        {
            //orderLut = new List<int>();
            for(int i=0; i< Count; i++)
            {
                priorityLut[i].queueIndex = i;
            }
            
            switch (Type)
	        {
		        case PriorityAlgorithems.FILO:
                    for (int i = 0; i < Count; i++)
                    {
                        orderLut[i] = Count - i - 1;
                    }
                    break;
                case PriorityAlgorithems.FIFO:
                    for (int i = 0; i < Count; i++)
                    {
                        orderLut[i] = i;
                    }
                    break;
                case PriorityAlgorithems.LRU:
                    var orderLRU = from n in priorityLut orderby timeMetric(n, true) select n.queueIndex;
                     orderLut = orderLRU.ToList();
                    break;
                case PriorityAlgorithems.MRU:
                    var orderMRU = from n in priorityLut orderby timeMetric(n, false) descending select n.queueIndex;
                     orderLut = orderMRU.ToList();
                    break;
                case PriorityAlgorithems.Random:
                    for (int i = 0; i < Count; i++)
                    {
                        orderLut[i] = i;
                    }
                    orderLut.Shuffle();
                    break;
                case PriorityAlgorithems.ManualPriority:
                    var orderManual = from n in priorityLut orderby n.lastPriorityQuerey select n.queueIndex;
                    orderLut = orderManual.ToList();
                    break;
                case PriorityAlgorithems.LeastFrequentlyUsed:
                    var orderLFU = from n in priorityLut orderby -UseMetric(n) select n.queueIndex;
                    orderLut = orderLFU.ToList();
                    break;
                case PriorityAlgorithems.MostFrequentlyUsed:
                    var orderMFU = from n in priorityLut orderby UseMetric(n) select n.queueIndex;
                    orderLut = orderMFU.ToList();
                    break;
                default:
                    break;
	        }
        }

        public void RequereyManualPriorities()
        {
            if(getPriority == null)
            {
                return;
            }

            for (int i = 0; i < Count; i++)
            {
                priorityLut[i].lastPriorityQuerey = getPriority(queue[i]);
            }
            RebuildOrderLut();
        }

        private T removeNthItem(int n)
        {
            T val = queue[n];
            queue.RemoveAt(n);
            priorityLut.RemoveAt(n);
            RebuildOrderLut();
            return val;
        }

        //-------------------------------------------------------------------------------------------------------
        // IEnumerable<T>, ICollection<T>
        //-------------------------------------------------------------------------------------------------------
        public T Dequeue() 
        {
            return removeNthItem(orderLut[0]);
        }

        /// <summary>
        /// places item in queue. If item is already in queue then its prioity is altered acording to the priority algorithem.
        /// </summary>
        /// <param name="item"></param>
        public void Requeue(T item)   
        {
            if(queue.Contains(item))
            {
                int index = queue.IndexOf(item);
            }

             switch (Type)
	         {
		        case PriorityAlgorithems.FILO:
                    break;
                case PriorityAlgorithems.FIFO:
                    break;
                case PriorityAlgorithems.LRU:
                    break;
                case PriorityAlgorithems.MRU:
                    break;
                case PriorityAlgorithems.Random:
                    break;
                case PriorityAlgorithems.ManualPriority:
                    break;
                case PriorityAlgorithems.LeastFrequentlyUsed:
                    break;
                case PriorityAlgorithems.MostFrequentlyUsed:
                    break;
                default:
                    break;
	        }
        }


        public T Peek()   
        {
            if (Count > 0)
            {
                return queue[orderLut[0]];
            }
            return default(T);
        }

        public T[] ToArray()   
        {
            T[] array = new T[Count];
            int i = 0;
            foreach(T item in this)
            {
                array[i] = item;
                i++;
            }

            return array;
        }

        public int Count { get { return queue.Count; } }

        public void Clear()
        {
            queue.Clear();
            orderLut.Clear();
        }

        public bool Contains(T item) { return queue.Contains(item); }

        public void CopyTo(T[] array, int arrayIndex) { queue.CopyTo(array, arrayIndex); }


        public IEnumerator<T> GetEnumerator()
        {
            try
            {
                for (int i = 1; i <= orderLut.Count; i++)
                {
                    yield return queue[orderLut[i]];
                }
            }
            finally
            {
                //a foreach was terminated early...    
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            try
            {
                for (int i = 1; i <= orderLut.Count; i++)
                {
                    yield return queue[orderLut[i]];
                }
            }
            finally
            {
                //a foreach was terminated early...    
            }
        }
    }
}
