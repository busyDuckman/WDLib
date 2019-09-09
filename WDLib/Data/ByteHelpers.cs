/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD_toolbox.Data
{
    public static class ByteHelpers
    {
        /// <summary>
        /// Memset
        /// NB: this is not as effiecint as I would like, but the environment is not ideal for this.
        /// This code is based on ideas presented at: https://stackoverflow.com/questions/1897555/what-is-the-equivalent-of-memset-in-c
        /// </summary>
        /// <param name="array"></param>
        /// <param name="value"></param>
        public static void Memset<T>(T[] data, T value, int start, int len)
        where T: struct
        {
            // Validate input
            if (data== null) {
                throw new ArgumentNullException("data can not be null");
            }
            if((start < 0) || (start >= data.Length)) {
                throw new InvalidOperationException("start is not valid");
            }
            if ((len < 0) || ((start+len) > data.Length)) {
                throw new InvalidOperationException("len is not valid");
            }

            if(len == 0) {
                return;
            }

            int blockSize = 64;
            int pos = start;

            // A for loop to create the first block
            for (int i = 0; i < Math.Min(blockSize, len); i++) {
                data[pos + i] = value;
            }

            // Memcopies (doubling) to fill memory.
            while (pos < len) {
                Buffer.BlockCopy(data, start, 
                                 data, pos, 
                                 Math.Min(blockSize, len - pos));
                pos += blockSize;
                blockSize *= 2;
            }
        }

        public static unsafe void Memset(void* ptr, void* value, int valueLen, int start, int len)
        {
            // Validate input
            if (ptr == null)
            {
                throw new ArgumentNullException("data can not be null");
            }
            if (start < 0)
            {
                throw new InvalidOperationException("start is not valid");
            }
            if (len < 0)
            {
                throw new InvalidOperationException("len is not valid");
            }
            if (valueLen <= 0)
            {
                throw new InvalidOperationException("valueLen is not valid");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value can not be null");
            }


            if (len == 0)
            {
                return;
            }

            int blockSize = 1;
            int pos = start;

            // first value
            Buffer.MemoryCopy(value, ptr, valueLen, valueLen);

            // Memcopies (doubling) to fill memory.
            while (pos < len)
            {
                Buffer.MemoryCopy(ptr,                             //src
                                   ((byte*) ptr)+(pos*valueLen),   //dest
                                    blockSize * valueLen,                      //size of dest array...
                                    Math.Min(blockSize, len - pos)* valueLen); //len

                pos += blockSize;
                blockSize *= 2;
            }
            
        }
    }
}
