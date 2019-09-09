/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WD_toolbox.Threading
{
    public sealed class ObjectLease<T> : IDisposable
    {
        public ObjectPool<T> Pool { get; private set; }
        public T Value { get; private set; }

        public ObjectLease(ObjectPool<T> pool, T value)
        {
            this.Pool = pool;
            this.Value = value;
        }

        public void Dispose()
        {
            Pool.ReturnObject(Value);
        }
    }

    /// <summary>
    /// Holds a pool of objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ObjectPool<T>
    {
        private ConcurrentBag<T> pool;
        private Func<T> createNew;

        public int MaxPoolSize { get; protected set; }


        //If left null, pool size is unlimited.
        private Semaphore objCountSemaphore = null;
        private volatile object poolLock = new object();
        private volatile int countOnLease = 0;
        private volatile int countSpawned = 0;  //Total object in existance

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectGenerator"></param>
        /// <param name="maxPoolSize">Maximum size of the pool. &lteq 0 disables. </param>
        public ObjectPool(Func<T> createNew, int maxPoolSize = -1)
        {
            pool = new ConcurrentBag<T>();
            this.createNew = createNew;
            MaxPoolSize = maxPoolSize;
            if (MaxPoolSize > 0)
            {
                objCountSemaphore = new Semaphore(initialCount: 0, maximumCount: MaxPoolSize);
            }
        }

        /// <summary>
        /// Gets an object from the pool.
        /// NB: Must be retuned via ReturnObject.
        /// </summary>
        /// <returns></returns>
        public T CheckoutObject()
        {
            lock (poolLock)
            {
                objCountSemaphore.WaitOne();
                countOnLease++;

                T obj;
                if (pool.TryTake(out obj))
                {
                    return obj;
                }

                countSpawned++;
                return createNew();
            }
        }

        /// <summary>
        /// Returns an object to the pool.
        /// </summary>
        /// <param name="item">Item to return.</param>
        /// <param name="force">Overrides all checking.</param>
        public void ReturnObject(T item, bool force = false)
        {
            lock (poolLock)
            {
                // State check
                if (!force)
                {
                    if (pool.Contains(item))
                    {
                        throw new InvalidOperationException("Item was already returned to object Pool.");
                    }
                    if(pool.Count >= MaxPoolSize)
                    {
                        throw new InvalidOperationException("Returning object to pool that is already full.");
                    }
                }

                // Return item
                pool.Add(item);
                objCountSemaphore.Release();
                countOnLease--;
            }
        }

        /// <summary>
        /// Creates a IDisposable Lease of an object. Good for use with the using clause.
        /// </summary>
        /// <returns></returns>
        public ObjectLease<T> LeaseOne()
        {
            return new ObjectLease<T>(this, CheckoutObject());
        }

        public override string ToString()
        {
            return String.Format("Object Pool ({0}) in pool = {1}, total spawned = {2}, on lease = {3}", 
                typeof(T).Name, pool.Count, countSpawned, countOnLease);
        }
    }

}
