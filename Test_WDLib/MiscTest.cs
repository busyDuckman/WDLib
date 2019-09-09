/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using WD_toolbox;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Test_WDLib
{
    
    
    /// <summary>
    ///This is a test class for MiscTest and is intended
    ///to contain all MiscTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MiscTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for DoesStringMeanFalse
        ///</summary>
        [TestMethod()]
        public void DoesStringMeanFalseTest()
        {
            string s = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = Misc.DoesStringMeanFalse(s);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DoesStringMeanTrue
        ///</summary>
        [TestMethod()]
        public void DoesStringMeanTrueTest()
        {
            string s = string.Empty;
            bool expected = false;
            bool actual;
            actual = Misc.DoesStringMeanTrue(s);
            Assert.AreEqual(expected, actual);

            Assert.IsTrue(Misc.DoesStringMeanTrue("yes"));
            Assert.IsTrue(Misc.DoesStringMeanTrue("true"));
            //Assert.IsTrue(Misc.DoesStringMeanTrue("ok"));
            Assert.IsTrue(Misc.DoesStringMeanTrue(" True "));
            Assert.IsFalse(Misc.DoesStringMeanTrue("no"));
            Assert.IsFalse(Misc.DoesStringMeanTrue(""));

            Assert.IsTrue(Misc.DoesStringMeanFalse("no"));
            Assert.IsTrue(Misc.DoesStringMeanFalse("false"));
            Assert.IsTrue(Misc.DoesStringMeanFalse(" false "));
            Assert.IsFalse(Misc.DoesStringMeanFalse("yes"));
            Assert.IsFalse(Misc.DoesStringMeanFalse(""));
        }



        [TestMethod()]
        public void EnumerateNodesTest()
        {
            /*
            * Test tree
            *                  A
            *                / | \ 
            *              B   C   D
            *            / |       | \
            *          E   F       G   H
            *        / |         / | \
            *       I  J        K  L  M
            */
            Node<char> tree = Node<char>.TestTree();

            Node<char> coruptTree = Node<char>.TestTree();
            //point L  to A
            coruptTree.Nodes[2].Nodes[0].Nodes[1].Nodes.Add(coruptTree); 

            //DepthFirstPreOrder
            string res = new String(Misc.EnumerateNodes(tree, N => N.Nodes, 
                                                        NodeVisitOrder.DepthFirstPreOrder, 
                                                        CircularRefernceBehaviour.ThrowException).Select(N => N.Item).ToArray());



            //System.Diagnostics.Trace.WriteLine("Hello World");
            Assert.AreEqual("ABEIJFCDGKLMH", res);
            
            //DepthFirstProstOrder
            res = new String(Misc.EnumerateNodes(tree, N => N.Nodes, 
                                                    NodeVisitOrder.DepthFirstPostOrder,
                                                    CircularRefernceBehaviour.ThrowException).Select(N => N.Item).ToArray());
            Assert.IsTrue(res == "IJEFBCKLMGHDA");

            //BredthFirst
            res = new String(Misc.EnumerateNodes(tree, N => N.Nodes, 
                                                 NodeVisitOrder.BredthFirst,
                                                 CircularRefernceBehaviour.ThrowException).Select(N => N.Item).ToArray());
            Assert.AreEqual("ABCDEFGHIJKLM", res);

            //just to check no exception thrown
            TreeNode n = Misc.RebuildTree(tree,  
                                         N => N.Nodes, 
                                         N => new TreeNode("" + N.Item),
                                         (T, N) => T.Nodes.AddRange(N.ToArray()), 
                                         CircularRefernceBehaviour.ThrowException);

            //Check an exception is thrown for curupted tree

            try
            {
                n = Misc.RebuildTree(coruptTree,
                                             N => N.Nodes,
                                             N => new TreeNode("" + N.Item),
                                             (T, N) => T.Nodes.AddRange(N.ToArray()),
                                             CircularRefernceBehaviour.ThrowException);

                Assert.Fail("Should never reach this line of code");
            }
            catch (Exception)
            {
            }

            //Just handle a corupt tree by not going backward.
            n = Misc.RebuildTree(coruptTree,
                                 N => N.Nodes,
                                 N => new TreeNode("" + N.Item),
                                 (T, N) => T.Nodes.AddRange(N.ToArray()),
                                 CircularRefernceBehaviour.Skip);

            //NB: Infinite loop on fail

        }

        

        public class Node<T>
        {
            public List<Node<T>> Nodes = new List<Node<T>>();
            public T Item;
            public Node(T item, IList<Node<T>> nodes = null)
            {
                Item = item;
                if (nodes != null)
                {
                    Nodes.AddRange(nodes);
                }
            }

            private static Node<T> NODE<T>(T item, params Node<T>[] Nodes)
            {
                return new Node<T>(item, Nodes);
            }

            public static Node<char> TestTree()
            {
                /*
                * Test tree
                *                  A
                *                / | \ 
                *              B   C   D
                *            / |       | \
                *          E   F       G   H
                *        / |         / | \
                *       I  J        K  L  M
                */


                Node<char> tree =
                    NODE('A',
                        NODE('B',
                            NODE('E',
                                NODE('I'),
                                NODE('J')
                                ),
                            NODE('F')
                            ),
                        NODE('C'),
                        NODE('D',
                            NODE('G',
                                NODE('K'),
                                NODE('L'),
                                NODE('M')
                                ),
                            NODE('H')
                            )
                        );

                return tree;
            }
        }
    }
}
