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
using System.Threading;
using System.Threading.Tasks;

namespace WD_toolbox.Data.DataStructures
{
    /// <summary>
    /// This is similar to BlockingCollection<>
    /// It's not yet tested.
    /// </summary>
    /// <typeparam name="MESSAGE"></typeparam>
    public class Pipe<MESSAGE> : IPipe<MESSAGE>
    {
        /// <summary>
        /// Internal class for any node
        /// </summary>
        private class PipeNode
        {
            public MESSAGE Msg { get; protected set; }
            public bool IsEOF { get; protected set; }

            public PipeNode(MESSAGE msg) { 
                this.Msg = msg;
            }

            private PipeNode() { IsEOF = true; Msg = default(MESSAGE); }
            public static PipeNode fromEOF() { return new PipeNode(); }

        }

        // contents of the pipe.
        List<PipeNode> messages;
        private readonly object newItemLock = new object();

        public Pipe()
        {
            messages = new List<PipeNode>();
        }

        /// <summary>
        /// True iff queueEndOfMessages was called to finish ingress of new messages.
        /// </summary>
        /// <returns></returns>
        public bool IsFinished { get {
                return messages[messages.Count - 1].IsEOF;
            }
        }

        /// <summary>
        /// Called to finish ingress of new messages.
        /// </summary>
        public void QueueEndOfMessages()
        {
            messages.Add(PipeNode.fromEOF());
            lock (newItemLock) {
                Monitor.PulseAll(newItemLock);
            }
        }

        /// <summary>
        /// Adds a new message.
        /// </summary>
        /// <param name="msg"></param>
        public void QueueMessage(MESSAGE msg)
        {
            messages.Add(new PipeNode(msg));
            lock (newItemLock)
            {
                Monitor.PulseAll(newItemLock);
            }
        }

        /// <summary>
        /// Enumerates through all messages untill eof. Will wait for more info if EOF is not encountered.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<MESSAGE> GetEnumerator()
        {
            int i = 0;
            while (!messages[i].IsEOF)
            {
                while(i >= this.messages.Count)
                {
                    lock (newItemLock)
                    {
                        Monitor.Wait(newItemLock);
                    }
                }
                if(messages[i].IsEOF)
                {
                    break;
                }
                yield return messages[i].Msg;
                i++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
