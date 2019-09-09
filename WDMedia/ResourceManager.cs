/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WDMedia
{
    public class ResourceManager : IDisposable
    {
        public readonly Dictionary<int, Font> Fonts;
        public readonly Dictionary<int, Brush> Brushes;
        public readonly Dictionary<int, Pen> Pens;
        public readonly Dictionary<int, Bitmap> Bitmaps;
        public readonly Dictionary<int, Object> Objects;

        private Dictionary<Type, IDictionary> dictLut;

        public readonly dataStore dataStores;

        public ResourceManager()
        {
            Fonts = new Dictionary<int, Font>();
            Brushes = new Dictionary<int, Brush>();
            Pens = new Dictionary<int, Pen>();
            Bitmaps = new Dictionary<int, Bitmap>();
            Objects = new Dictionary<int, object>();

            dictLut = new Dictionary<Type, IDictionary>();

            dictLut.Add(typeof(Font), Fonts);
            dictLut.Add(typeof(Brush), Brushes);
            dictLut.Add(typeof(Bitmap), Bitmaps);
            dictLut.Add(typeof(Pen), Pens);

            dataStores = new dataStore();

            dataStores.AddDir(new FileInfo(Application.ExecutablePath).DirectoryName);
        }

        private IDictionary getDictionary<TYPE>()
        {
            return getDictionary(typeof(TYPE));
        }

        private IDictionary getDictionary(Type t)
        {
            //express exact matches
            if (dictLut.ContainsKey(t))
            {
                return dictLut[t];
            }

            //look for an inherited object
            foreach (Type baseType in dictLut.Keys)
            {
                if (t.IsSubclassOf(baseType))
                {
                    return dictLut[baseType];
                }
            }
            return Objects;
        }

        public void Dispose<RESTYPE>(int key)
        {
            Dispose(key, typeof(RESTYPE));
        }

        public void Dispose(int key, Type t)
        {
            IDictionary d = getDictionary(t);
            if (d.Contains(key))
            {
                d.Remove(key);
            }
        }

        public bool Add(object key, object o)
        {
            return Add((int)key, o);
        }

        public bool Add(object key, object o, bool replaceOld)
        {
            return Add((int)key, o, replaceOld);
        }

        /// <summary>
        /// Adds or replaces a resource.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Add(int key, object o)
        {
            return Add(key, o, true);
        }

        public bool Add(int key, object o, bool replaceOld)
        {
            Type t = o.GetType();
            IDictionary d = getDictionary(t);
            if (d.Contains(key))
            {
                if (!replaceOld)
                {
                    return false;
                }
                d.Remove(key);
            }
            d.Add(key, o);
            return true;
        }

        public bool addDerivedFont(int key, int baseFont, FontStyle newStyle)
        {
            return addDerivedFont(key, baseFont, newStyle, true);
        }

        public bool addDerivedFont(int key, int baseFont, FontStyle newStyle, bool replaceOld)
        {
#if PocketPC
        Font newFont = new Font(Fonts[baseFont].Name, Fonts[baseFont].Size, newStyle);
#else
            Font newFont = new Font(Fonts[baseFont], newStyle);
#endif
            return Add(key, newFont, replaceOld);
        }


        #region IDisposable Members

        private void Dispose<RESTYPE>(Dictionary<int, RESTYPE> dic)
        {
            List<int> keyList = new List<int>(dic.Keys);
            foreach (int key in keyList)
            {
                RESTYPE r = dic[key];
                dic.Remove(key);
                if (r is IDisposable)
                {
                    ((IDisposable)r).Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose<Font>(Fonts);
            Dispose<Brush>(Brushes);
            Dispose<Pen>(Pens);
            Dispose<Bitmap>(Bitmaps);
        }

        #endregion



        #region bitmap and image support
        public bool addBitmapFromDataStore(string path, int key)
        {
            return addBitmapFromDataStore(path, key, true);
        }

        public bool addBitmapFromDataStore(string path, int key, bool replaceOld)
        {
            Bitmap b = null;
            {
                byte[] data = dataStores.loadFileFromDataStore(path);
                if (data != null)
                {
                    MemoryStream ms = new MemoryStream(data);
                    b = new Bitmap(ms);
                    ms.Close();
                }
                else
                {
                    if (File.Exists(path))
                    {
                        try
                        {
                            b = new Bitmap(path);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            if (b == null)
            {
                return false;
            }

            return Add(key, b, replaceOld);
        }

        public bool addRandomBitmapFromDataStorePath(string path, int key)
        {
            return addRandomBitmapFromDataStorePath(path, key, true);
        }

        public bool addRandomBitmapFromDataStorePath(string path, int key, bool replaceOld)
        {
            string[] files = dataStores.getFiles(path, "*.jpg");
            if (files.Length == 0)
            {
                return false;
            }
            Random r = new Random();
            string file = files[r.Next(files.Length)];
            return addBitmapFromDataStore(file, key, replaceOld);
        }

        private string appendPath(string path, string file)
        {
            string seperator = (path.IndexOf(@"/") != 0) ? @"/" : @"\";

            //path "\\share\" style is valid so we skip ""\\"
            //think about iterator over the next few lines

            if (!path.EndsWith(seperator) && (!file.StartsWith(seperator)))
            {
                path += seperator;
            }

            return path + file;
        }
        #endregion
    }
}
