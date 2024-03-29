/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;

namespace WD_toolbox.Data.DataStructures
{
    /// <summary>
    /// ADT for a trie (Digital Search Tree).
    /// This code was very old, from when I was a student (2000).
    /// I would not trust this in production.
    /// The conversion to generics was done as my first test of the new generics capacity in .net 2.0
    /// No attempt to make the class thread safe was undertaken.
    /// </summary>
    /// <typeparam name="elementType"></typeparam>
    /// <typeparam name="resultType"></typeparam>
    [Obsolete]
    public class DST<elementType, resultType> : IDictionary<IList<elementType>, resultType>, IList<resultType>, ISerializable
    where resultType : IComparable<resultType>
    where elementType : IComparable<elementType>
    {
        DSTNode<elementType, resultType> rootNode;

        public DST()
        {
            rootNode = new DSTNode<elementType, resultType>();
            rootNode.init();
        }

        internal bool isValid()
        {
            return rootNode.isValid();
        }

        #region IDictionary<IList<elementType>,resultType> Members

        public void Add(IList<elementType> key, resultType value)
        {
            rootNode.Add(key, value);
        }

        public bool ContainsKey(IList<elementType> key)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ICollection<IList<elementType>> Keys
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool Remove(IList<elementType> key)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool TryGetValue(IList<elementType> key, out resultType value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ICollection<resultType> Values
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public resultType this[IList<elementType> key]
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region ICollection<KeyValuePair<IList<elementType>,resultType>> Members

        public void Add(KeyValuePair<IList<elementType>, resultType> item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains(KeyValuePair<IList<elementType>, resultType> item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(KeyValuePair<IList<elementType>, resultType>[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Count
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool IsReadOnly
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool Remove(KeyValuePair<IList<elementType>, resultType> item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<KeyValuePair<IList<elementType>,resultType>> Members

        public IEnumerator<KeyValuePair<IList<elementType>, resultType>> GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IList<resultType> Members

        public int IndexOf(resultType item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Insert(int index, resultType item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveAt(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public resultType this[int index]
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region ICollection<resultType> Members

        public void Add(resultType item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains(resultType item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(resultType[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Remove(resultType item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<resultType> Members

        IEnumerator<resultType> IEnumerable<resultType>.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

    }
    /*

    [TestFixture]
    public class tDST
        {
        DST<char, string> testDST;
        Dictionary<char[], string> testDictionary;

        static string[] smallWordList = new string[] {"A",
                                                    "Aam",
                                                    "Aard-vark",
                                                    "Aard-wolf",
                                                    "Aaronic",
                                                    "Aaronical",
                                                    "Aaron's rod",
                                                    "Aaron's rod",
                                                    "Ab",
                                                    "Abaca",
                                                    "Abacinate",
                                                    "Abacination",
                                                    "Abaciscus",
                                                    "Abacist",
                                                    "Aback"
                                                    };

        [SetUp]
        public void init()
            {
            testDST = new DST<char,string>();
            testDictionary = new Dictionary<char[],string>();
            }

        [Test]
        public void DSTtest()
            {
            run();
            }

        public void run()
            {
            testDST = new DST<char, string>();
            testDictionary = new Dictionary<char[], string>();
            int i;
            for(i=0; i<smallWordList.Length; i++)
                {
                testDST.Add(smallWordList[i].ToCharArray() , smallWordList[i] + "moo");
                testDictionary.Add(smallWordList[i].ToCharArray(), smallWordList[i] + "moo");
                Assert.AreEqual(testDST.Count, testDictionary.Count);
                Assert.IsTrue(testDST.isValid());
                }

            for(i=0; i<smallWordList.Length; i++)
                {
                Assert.AreEqual(testDST[smallWordList[i].ToCharArray()], testDictionary[smallWordList[i].ToCharArray()]);
                }

            Assert.Fail("test ");
            }
        }*/
}
