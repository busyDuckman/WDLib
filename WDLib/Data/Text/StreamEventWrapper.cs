/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox;

namespace WD_toolbox.Data.Text
{
    /// <summary>
    /// Lets you use events to deal with stream events.
    /// </summary>
    public class StreamEventWrapper : Stream
    {
        public Stream Proxy {get; protected set;}
        public bool HasProxy { get { return Proxy != null; } }

        public Action<byte[], int , int > OnRead {get; set;}
        public Action<byte[], int, int> OnWrite { get; set; }


        public StreamEventWrapper() : this (null)
        {
        }

        public StreamEventWrapper(Stream proxy)
        {
            Proxy = proxy;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            OnRead.SafeCall(buffer, offset, count);
            return HasProxy ? Proxy.Read(buffer, offset, count) : count;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            OnWrite.SafeCall(buffer, offset, count);
            if (HasProxy)
            {
                Proxy.Write(buffer, offset, count);
            }
        }

        public override bool CanRead { get { return HasProxy ? Proxy.CanRead : (OnRead != null); } }

        public override bool CanSeek { get { return HasProxy ? Proxy.CanSeek : false; } }

        public override bool CanWrite { get { return HasProxy ? Proxy.CanWrite: (OnWrite != null); } }

        public override void Flush() { if (HasProxy) { Proxy.Flush(); } }

        public override long Length { get { return HasProxy ? Proxy.Length : 0; } }

        public override long Position 
        {
            get
            {
                return HasProxy ? Proxy.Position : 0;
            }
            set
            {
                if (HasProxy)
                {
                    Proxy.Position = value;
                }
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return HasProxy ? Proxy.Seek(offset, origin) : 0;
        }

        public override void SetLength(long value)
        {
            if (HasProxy)
            {
                Proxy.SetLength(value);
            }
        }

        public override void Close()
        {
            if (HasProxy)
            {
                Proxy.Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (HasProxy)
            {
                Proxy.Dispose();
            }
        }

    }
}
