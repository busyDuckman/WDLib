/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System.Collections.Generic;

namespace WD_toolbox.Data.DataStructures
{
    public interface IPipe<MESSAGE> : IEnumerable<MESSAGE>
    {
        void QueueMessage(MESSAGE msg);
        void QueueEndOfMessages();

        bool IsFinished { get; }
    }
}