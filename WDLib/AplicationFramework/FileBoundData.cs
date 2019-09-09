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
using System.Threading;
using System.Threading.Tasks;

namespace WD_toolbox.AplicationFramework
{
    /// <summary>
    /// Handels the situation where you want to reload a document every time the file is changed (externelly).
    /// eg: WDMedia.Tiles.Tile uses this so bitmaps can be edited and the results seen instantly in the game.
    /// </summary>
    public class FileBoundData<T>
        where T : class, IReloadFile
    {
        //-------------------------------------------------------------------------------------------
        // Instance Data
        //-------------------------------------------------------------------------------------------
        bool _boundToFile { get; set; }
        bool BoundToFile {
            get { return _boundToFile; } 
            set
            {
                lock (FileBindingLock)
                {
                    _boundToFile = value;
                }
                init();
            } 
        }

        string _sourceFile;
        public string SourceFile
        {
            get { return _sourceFile; }
            set 
            {
                lock (FileBindingLock)
                {
                    _sourceFile = value;
                }
                init();
            }
        }

        bool SourceFileExists
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SourceFile))
                {
                    if (File.Exists(SourceFile))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        WeakReference<T> Data;

        //-------------------------------------------------------------------------------------------
        // Not persisted
        //-------------------------------------------------------------------------------------------
        [NonSerialized]
        protected FileSystemWatcher SourceFileWatcher;
        [NonSerialized]
        private object _fileBindingLock = new object();
        public object FileBindingLock
        {
            get 
            {
                if (_fileBindingLock == null)
                {
                    _fileBindingLock = new object();
                }
                return _fileBindingLock; 
            }
        }

        //-------------------------------------------------------------------------------------------
        // Contstructors and setup
        //-------------------------------------------------------------------------------------------

        public FileBoundData(T data, string path) : this(data, path, true)
        {
        }

        public FileBoundData(T data, string path, bool bound)
        {
            Data = new WeakReference<T>(data);
            SourceFile = path;
            BoundToFile = bound;
        }

        public void init()
        {
            lock (FileBindingLock)
            {
                if (SourceFileWatcher != null)
                {
                    SourceFileWatcher.Dispose();
                    SourceFileWatcher = null;
                }

                if (SourceFileExists && BoundToFile)
                {
                    SourceFileWatcher = new FileSystemWatcher(Path.GetDirectoryName(SourceFile), Path.GetFileName(SourceFile));
                    SourceFileWatcher.Changed += OnChanged;
                }
            }
        }

        //-------------------------------------------------------------------------------------------
        // Members
        //-------------------------------------------------------------------------------------------
        // Define the event handlers. 
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            T item;
            if (Data.TryGetTarget(out item))
            {
                item.ReloadFile(e.FullPath);
            }
            else
            {
                //items was disposed, remove the watcher.
                if (SourceFileWatcher != null)
                {
                    SourceFileWatcher.Dispose();
                    SourceFileWatcher = null;
                }
            }
        }
    }
}
