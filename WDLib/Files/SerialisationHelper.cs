/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace WD_toolbox.Files
    {
    public enum serializationFormats { binary, XML };
    public class SerialisationHelper<Type>
        {
        

        /// <summary>
        /// Save the type to the given filename
        /// </summary>
        /// <param name="filename">filename to save to</param>
        /// <param name="data">data to save</param>
        /// <param name="format">The format for use in serialisation.</param>
        /// <remarks>Will orefide file name if file exists</remarks>
        public static void save(string filename, Type data, serializationFormats format)
            {
            Stream s = File.Open(filename, FileMode.OpenOrCreate);
            Serialize(s, data, format);
            s.Close();
            }

        /// <summary>
        /// Serializes a type using the specified formatter
        /// </summary>
        /// <param name="s">stream to serialize to</param>
        /// <param name="data">data to save</param>
        /// <param name="format">The format for use in serialisation.</param>
        public static void Serialize(Stream s, Type data, serializationFormats format)
            {
            switch (format)
                {
                case serializationFormats.binary:
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(s, data);  // serialize the arraylist to the output stream here
                    break;
                case serializationFormats.XML:
                    XmlSerializer xs = new XmlSerializer(typeof(Type));
                    xs.Serialize(s, data);
                    break;
                default:
                    throw new Exception("Invalid serialisation format specified");
                }
            }

        /// <summary>
        /// opens a object from a file
        /// </summary>
        /// <param name="filename">File to open</param>
        /// <param name="format">The format for use in serialisation.</param>
        /// <returns>A new object, thows an expetion if the operation could not be completed</returns>
        public static Type open(string filename, serializationFormats format)
            {
            Stream s = File.Open(filename, FileMode.Open);


            int maxMemBufferMB = 128;
#if _WIN32_WCE
			maxMemBufferMB = 0;
#endif
            if (s.Length < (maxMemBufferMB * 1024 * 1024))
                {
                byte[] buffer = new byte[s.Length];
                s.Read(buffer, 0, buffer.Length);
                s.Close();

                //s is closed so file will not lock if load fails
                MemoryStream m = new MemoryStream(buffer);
                return Deserialize(m, format);
                }
            else
                {
                try
                    {
                    return Deserialize(s, format);
                    }
                finally
                    {
                    s.Close();
                    }
                }

            }

        /// <summary>
        /// Deserialize a object using the specified formatter
        /// </summary>
        /// <param name="s">stream to deserialize</param>
        /// <param name="format">The format for use in serialisation.</param>
        /// <returns>a new object, thows an expetion if the operation could not be completed</returns>
        public static Type Deserialize(Stream s, serializationFormats format)
            {
            switch (format)
                {
                case serializationFormats.binary:
                    BinaryFormatter bf = new BinaryFormatter();
                    Type n = (Type)bf.Deserialize(s);
                    return n;
                case serializationFormats.XML:
                    XmlSerializer xs = new XmlSerializer(typeof(Type));
                    return (Type)xs.Deserialize(s);
                default:
                    throw new Exception("Invalid serialisation format specified");
                }
            }
        } 
    }
