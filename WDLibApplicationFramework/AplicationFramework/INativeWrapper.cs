/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WDLibApplicationFramework.AplicationFramework
{
    /// <summary>
    /// When we need to hide a native object.
    /// Often used to wrap classes in a common interface.
    /// </summary>
    public interface INativeWrapper
    {
        object NativeObject { get; }
    }

    public class NativeWrapperBase<T> : INativeWrapper
    {
        protected T nativeObject;
        public object NativeObject { get { return nativeObject; } }

        public NativeWrapperBase()
        {
            nativeObject = default(T);
        }

        public NativeWrapperBase(T item)
        {
            nativeObject = item;
        }

        public override string ToString()
        {
            return nativeObject.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is INativeWrapper)
            {
                return nativeObject.Equals(((INativeWrapper)obj).NativeObject);
            }
            else
            {
                return nativeObject.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return nativeObject.GetHashCode();
        }

        public static explicit operator T(NativeWrapperBase<T> wrapper)
        {
            return wrapper.nativeObject;
        }
    }
}
