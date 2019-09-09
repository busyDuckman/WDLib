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
using WD_toolbox.Data.DataStructures;
using WD_toolbox;
using WD_toolbox.AplicationFramework;

namespace WDMedia.Tiles
{
    /// <summary>
    /// A collection of tiles
    /// </summary>
    public class TileSet<T> : VList<T>, ITileSet
        where T : Tile
    {
        public string Name {get; protected set;}
        public int TileWidth { get; protected set; }
        public int TileHeight { get; protected set; }

        private TileSet()
        {

        }

        public TileSet(int tileWidth, int tileHeight)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }

        public static TileSet<Tile> fromImageAtlas(string path, int startIndex, int tileWidth, int tileHeight, Func<int, string> getName = null)
        {
            TileSet<Tile> tileSet = new TileSet<Tile>(tileWidth, tileHeight);
            try
            {
                if (File.Exists(path))
                {
                    using (Bitmap b = new Bitmap(path))
                    {
                        string basicName = Path.GetFileNameWithoutExtension(path);
                        int width = b.Width;
                        int height = b.Height;
                        int index = 0;
                        for (int y = 0; y <= (height - tileHeight); y += tileHeight)
                        {
                            for (int x = 0; x <= (width - tileWidth); x += tileWidth)
                            {
                                //get sub-image
                                Bitmap tb = b.GetSubImageClipped(x, y, tileWidth, tileHeight);

                                if (tb == null)
                                {           
                                    WDAppLog.LogNeverSupposedToBeHere();
                                }
                                //get a name
                                string name = (getName != null) ? getName(index) : null;
                                name = String.IsNullOrWhiteSpace(name) ? (basicName + "_" + index) : name;

                                //create and add tile
                                Tile tile = Tile.fromBitmap(tb, name, startIndex + index);
                                tileSet.Add(tile);

                                index++;
                            }
                        }
                    }
                    return tileSet;
                }
            }
            catch(Exception ex)
            {
                WDAppLog.logFileOpenError(ErrorLevel.Error, ex, path);
            }
            
            return null;
        }
        public virtual void Dispose()
        {
            foreach (Tile tile in this)
            {
                tile.Dispose();
            }

            this.Clear();
        }


        Tile IList<Tile>.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                if (value is T)
                {
                    this[index] = (T)value;
                }
            }
        }

        int IList<Tile>.IndexOf(Tile item)
        {
            return IndexOf((T)item);
        }

        void IList<Tile>.Insert(int index, Tile item)
        {
            Insert(index, (T)item);
        }

        void ICollection<Tile>.Add(Tile item)
        {
            Add((T)item);
        }

        bool ICollection<Tile>.Contains(Tile item)
        {
            return Contains((T)item);
        }

        void ICollection<Tile>.CopyTo(Tile[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<Tile>.Remove(Tile item)
        {
            return Remove((T)item);
        }

        IEnumerator<Tile> IEnumerable<Tile>.GetEnumerator()
        {
            return GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
