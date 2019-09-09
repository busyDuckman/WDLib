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

namespace WD_toolbox.Persistance
{
    public interface IStreamReadWrite
    {
        bool Write(StreamWriter sw);
        bool Read(StreamReader sr);
    }

    public interface IStreamReadWriteCoupled<GRAPH>
    {
        bool Write(StreamWriter sw, GRAPH coupledObject);
        bool Read(StreamReader sr, GRAPH coupledObject);
    }
     
    public class StreamReadWriteUtils
    {
        
    }

    public static class WDReadWriteExtensions
    {
        private static string NullDesignator = "_NULL^^_";
        private static string CrDesignator = "__CR^^__";
        private static string NlDesignator = "_NL^^_";
        public delegate string ToSingleLineStringDelegate<T> (T item);
        public delegate T FromSringLineDelegate<T> (string textLine);
        public delegate T NewItemDelegate<T>(string type);
        public delegate T NewCoupledItemDelegate<T, GRAPH>(string type, GRAPH coupledObject);

        public static void WriteLineEnumAsString(this StreamWriter sw, Enum _enum)
        {
            sw.WriteLine(_enum.ToString());
        }

        public static E ReadLineEnumAsString<E>(this StreamReader sr)
        {
            string line = sr.ReadLine().Trim();
            //int pos = Enum.GetNames(typeof(E)).IndexOf(R => R == line);
            return (E)Enum.Parse(typeof(E), line);
            //    GetValues(typeof(E)). [pos];
        }

        /*public static bool Read<T>(this T item, StreamReader sr, FromSringLineDelegate<T> fromLine)
        {
            sw.WriteLine(toLine(item));
            return true;
        }

        public static bool Write<T>(this T item, StreamWriter sw, ToSingleLineStringDelegate<T> toLine)
        {
            sw.WriteLine(toLine(item));
            return true;
        }*/

        public static int ReadFromSingleLines<T>(this IList<T> list, StreamReader sr, FromSringLineDelegate<T> fromLine)
        {
            throw new NotImplementedException();
        }

        public static int WriteAsSingleLines<T>(this IList<T> list, StreamWriter sw, ToSingleLineStringDelegate<T> fromLine)
        {
            throw new NotImplementedException();
        }

        public static string EncodeOneLineSerilisable(this string text)
        {
            if (text == null)
            {
                return (string)NullDesignator.Clone();
            }
            else
            {
                return text.Replace("\r", CrDesignator).Replace("\n", NlDesignator);
            }
        }

        public static string DecodeLineSerilisable(this string text)
        {
            if (text == null)
            {
                return null;
            }
            else if (text.Trim() == NullDesignator)
            {
                return null;
            }
            else
            {
                return text.Replace(CrDesignator, "\r").Replace(NlDesignator, "\n");
            }
        }

        public static int ReadFromSingleLines(this IList<string> list, StreamReader sr)
        {
            int[] nums = sr.ReadLine().ParseAllIntegers();
            if (nums.Length > 0)
            {
                int n = nums.Last();
                for (int i = 0; i < n; i++)
                {
                    string line = sr.ReadLine();
                    string text = line.DecodeLineSerilisable();
                    list.Add(text);
                }
                return n;
            }
            return -1;
        }

        public static bool WriteAsSingleLines(this IList<string> list, StreamWriter sw)
        {
            sw.WriteLine(string.Format("{0} {1}", list.GetType().Name, list.Count));
            foreach (string s in list)
            {
                sw.WriteLine(s.EncodeOneLineSerilisable());
            }

            return true;
        }

        /// <summary>
        /// Reads a list of IStreamReadWrite objects as saved by WriteLines
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sr"></param>
        /// <returns>number of items read, -1 on error.</returns>
        public static int ReadList<T>(this IList<T> list, StreamReader sr, NewItemDelegate<T> newItem)
            where T : class, IStreamReadWrite
        {
            int[] nums = sr.ReadLine().ParseAllIntegers();
            if (nums.Length > 0)
            {
                int n = nums.Last();
                for (int i = 0; i < n; i++)
                {
                    string type = sr.ReadLine().Trim().Trim(":".ToCharArray());
                    if (type == NullDesignator)
                    {
                        list.Add(null);
                    }
                    else
                    {
                        T item = newItem(type);
                        if (item != null)
                        {
                            if (!item.Read(sr))
                            {
                                return -1;
                            }
                            list.Add(item);
                        }
                    }
                    string blank = sr.ReadLine();
                    if (!string.IsNullOrWhiteSpace(blank))
                    {
                        return -1;
                    }
                }

                return n;
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sw"></param>
        /// <remarks> DataFormat:
        /// # comment
        /// arrayName: [n]
        /// ObjectType:
        /// object data   
        /// 
        /// ObjectType:
        /// object data
        /// 
        /// ... till n
        /// </remarks>
        /// <returns></returns>
        public static bool WriteList<T>(this IList<T> list, StreamWriter sw)
            where T : IStreamReadWrite
        {
            int n = list.Count;
            sw.WriteLine(string.Format("{0} {1}", list.GetType().Name, n));
            for (int i = 0; i < n; i++)
            {
                IStreamReadWrite item = list[i];
                if (item == null)
                {
                    sw.WriteLine(NullDesignator);
                }
                else
                {
                    sw.WriteLine(item.GetType().Name);
                    if (!item.Write(sw))
                    {
                        return false;
                    }
                }
                sw.WriteLine();//blank
            }
            return true;
        }

        //----------------------------------------------------------------------------------
        // Graph serialisation
        //----------------------------------------------------------------------------------

        /// <summary>
        /// Reads a list of IStreamReadWrite objects as saved by WriteLines
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sr"></param>
        /// <returns>number of items read, -1 on error.</returns>
        public static int ReadList<T, GRAPH>(this IList<T> list, 
                                                StreamReader sr, 
                                                GRAPH coupledObject, 
                                                NewCoupledItemDelegate<T, GRAPH> newItem)
            where T : class, IStreamReadWriteCoupled<GRAPH>
        {
            int[] nums = sr.ReadLine().ParseAllIntegers();
            if (nums.Length > 0)
            {
                int n = nums.Last();
                for (int i = 0; i < n; i++)
                {
                    string type = sr.ReadLine().Trim().Trim(":".ToCharArray());
                    if (type == NullDesignator)
                    {
                        list.Add(null);
                    }
                    else
                    {
                        T item = newItem(type, coupledObject);
                        if (item != null)
                        {
                            if (!item.Read(sr, coupledObject))
                            {
                                return -1;
                            }
                            list.Add(item);
                        }
                    }
                    string blank = sr.ReadLine();
                    if (!string.IsNullOrWhiteSpace(blank))
                    {
                        return -1;
                    }
                }

                return n;
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sw"></param>
        /// <remarks> DataFormat:
        /// # comment
        /// arrayName: [n]
        /// ObjectType:
        /// object data   
        /// 
        /// ObjectType:
        /// object data
        /// 
        /// ... till n
        /// </remarks>
        /// <returns></returns>
        public static bool WriteList<T, GRAPH>(this IList<T> list, StreamWriter sw, GRAPH coupledObject)
            where T : IStreamReadWriteCoupled<GRAPH>
        {
            int n = list.Count;
            sw.WriteLine(string.Format("{0} {1}", list.GetType().Name, n));
            for (int i = 0; i < n; i++)
            {
                IStreamReadWriteCoupled<GRAPH> item = list[i];
                if (item == null)
                {
                    sw.WriteLine(NullDesignator);
                }
                else
                {
                    sw.WriteLine(item.GetType().Name);
                    if (!item.Write(sw, coupledObject))
                    {
                        return false;
                    }
                }
                sw.WriteLine();//blank
            }
            return true;
        }

    }
}
