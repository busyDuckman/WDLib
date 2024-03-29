/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */

/*
using System;
using System.Collections.Generic;
using System.Text;

namespace WD_toolbox.Data.DataStructures
{
class ManagedCollection<ColectionType, DataType> : ICollection<DataType>
where ColectionType : ICollection<DataType>
    {
    #region constraints
    public enum constraintFailAction { Remove = 0, ThrowException};
    public interface DataConstraint<ConDataType>
        {
        bool verify(ConDataType d);
        }
    
    #endregion
    ColectionType data;
    
    public ManagedCollection()
        {
        }
    
    
    #region ICollection<DataType> Members

    public void  Add(DataType item)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public void  Clear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public bool  Contains(DataType item)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public void  CopyTo(DataType[] array, int arrayIndex)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public int  Count
    {
        get { throw new Exception("The method or operation is not implemented."); }
    }

    public bool  IsReadOnly
    {
        get { throw new Exception("The method or operation is not implemented."); }
    }

    public bool  Remove(DataType item)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    #endregion

    #region IEnumerable<DataType> Members

    public IEnumerator<DataType>  GetEnumerator()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator  System.Collections.IEnumerable.GetEnumerator()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    #endregion
    }
}
*/