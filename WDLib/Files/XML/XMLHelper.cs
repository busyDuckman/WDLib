/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WD_toolbox.Files.XML
{
    public static class XMLHelper
    {
        public delegate bool UseNodeDelegate(XmlNode node);
        public delegate string TextGenerateDelegate(XmlNode node);

        public static string XMLToString(XmlNode startNode, UseNodeDelegate useNode)
        {
            StringBuilder sb = new StringBuilder();

            Stack<XmlNode> nodes = new Stack<XmlNode>();
            nodes.Push(startNode);

            while (nodes.Count > 0)
            {
                XmlNode currentNode = nodes.Pop();

                if (useNode(currentNode))
                {
                    if (currentNode.Name == "#text")
                    {
                        //Console.Write(indent + node.InnerText);
                        sb.Insert(0, currentNode.InnerText);
                    }

                    foreach (XmlNode subNode in currentNode)
                    {
                        nodes.Push(subNode);
                    }
                }
            }

            return sb.ToString();
        }

        public static string XMLToString(XmlNode startNode, UseNodeDelegate useNode, TextGenerateDelegate toString)
        {
            StringBuilder sb = new StringBuilder();

            Stack<XmlNode> nodes = new Stack<XmlNode>();
            nodes.Push(startNode);

            while (nodes.Count > 0)
            {
                XmlNode currentNode = nodes.Pop();
                if (useNode(currentNode))
                {
                    if (currentNode.Name == "#text")
                    {
                        //Console.Write(indent + node.InnerText);
                        sb.Insert(0, currentNode.InnerText);
                    }
                    else
                    {
                        string s = toString(currentNode);
                        if (s != null)
                        {
                            sb.Insert(0, s);
                        }
                    }

                    foreach (XmlNode subNode in currentNode)
                    {
                        nodes.Push(subNode);
                    }
                }
            }

            return sb.ToString();
        }

    }
}
