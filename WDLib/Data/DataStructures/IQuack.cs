/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
namespace WD_toolbox.Data.DataStructures
{
    interface IQuack<datumType>
    {
        void Clear();
        bool Contains(datumType value);
        object Dequeue();
        void Enqueue(datumType item);
        bool IsEmpty();
        datumType PeekBottom();
        datumType PeekTop();
        datumType Pop();
        void Push(datumType item);
    }

    interface IQuack : IQuack<object>
    {
    };
}
