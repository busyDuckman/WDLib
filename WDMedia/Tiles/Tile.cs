/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox.AplicationFramework;
using WD_toolbox;
using WDMedia.Rendering;

namespace WDMedia.Tiles
{
    public class Tile : IReloadFile, IDisposable, IPRImage
    {
        public int TileID { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public Bitmap Image { get; protected set; }
        public string Name { get; protected set; }
        public object NativeObject { get; set; }

        FileBoundData<Tile> _dataBinding;

        protected Tile(Bitmap image, string name, int id)
        {
            init(image, name, id);
        }

        protected void init(Bitmap image, string name, int id)
        {
            TileID = id;
            Width = image.Width;
            Height = image.Height;
            Image = image;
            Name = name;
        }

        public static Tile fromBitmap(Bitmap image, string name, int id)
        {
            return new Tile(image, name, id);
        }

        public static Tile fromFile(string path, int id, bool bind = false)
        {
            return fromFile(path, null, id, bind);
        }

        public static Tile fromFile(string path, string name, int id, bool bind = false)
        {
            if(File.Exists(path))
            {
                string tileName = name ?? Path.GetFileNameWithoutExtension(path);

                try 
                {
                    using (Bitmap b = new Bitmap(path))
                    {
                        if (b != null)
                        {
                            Tile t = fromBitmap(b.GetUnFuckedVersion(), tileName, id);
                            if (t != null)
                            {
                                t._dataBinding = new FileBoundData<Tile>(t, path, bind);
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    WDAppLog.logFileOpenError(ErrorLevel.Error, ex, path);
                }
            }
            return null;
        }

        public virtual bool ReloadFile(string path)
        {
             if(File.Exists(path))
            {
                try 
                {
                    using (Bitmap b = new Bitmap(path))
                    {
                        if (b != null)
                        {
                            init(b.GetUnFuckedVersion(), Name, TileID);
                            return true;
                        }
                    }
                }
                catch(Exception ex)
                {
                    WDAppLog.logFileOpenError(ErrorLevel.Error, ex, path);
                }
            }
            return false;
        }

        public void Dispose()
        {
            if (Image != null)
            {
                Image.TryDispose(false);
                Image = null;
            }
        }
    }
}
