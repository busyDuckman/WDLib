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

namespace WDMedia
{
    public class dataStore
    {
        public readonly List<DirectoryInfo> dataStoreDirs;
        public readonly List<FileInfo> dataStoreArchives;

        public dataStore()
        {
            dataStoreDirs = new List<DirectoryInfo>();
            dataStoreArchives = new List<FileInfo>();
        }

        public void addAplicationDir()
        {

        }

        public void addAplicationDirArchives()
        {

        }

        internal void AddDir(string p)
        {
            dataStoreDirs.Add(new DirectoryInfo(p));
        }

        internal void AddArchieve(string p)
        {
            dataStoreArchives.Add(new FileInfo(p));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">File to load. 
        /// A string in the form "archieve.zip@foo/bar/file.txt" specifies a file inside the specified archive should be loaded.
        /// </param>
        /// <returns></returns>
        public byte[] loadFileFromDataStore(string path)
        {
            int zipPos = path.IndexOf('@');
            if (zipPos != -1)
            {
                /*
                //ZipInputStream zis = new ZipInputStream(File.OpenRead(path.Substring(0, zipPos));
                //zis.
                ZipFile zf = new ZipFile(path.Substring(0, zipPos));
                //zf[path.Substring(zipPos+1)].
                ZipEntry ze = zf.GetEntry(path.Substring(zipPos + 1));
                Stream s = zf.GetInputStream(ze);

                //read the file
                byte[] data = new byte[ze.Size];
                int pos = 0;
                int readCount = 1;

                //important to read zips like this because the whole file often does not come out in the first read.
                while ((pos < data.Length) && (readCount > 0))
                {
                    readCount = s.Read(data, pos, data.Length - pos);
                    pos += readCount;
                }

                //cleanup
                s.Close();
                zf.Close();

                return data;
                 */

                throw new NotImplementedException();
            }
            else
            {
                FileStream fs = File.OpenRead(path);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();
                return data;
            }
        }
        private bool fileExists(FileInfo zipFile, string relativeFilePath)
        {
            /*ZipFile zf = new ICSharpCode.SharpZipLib.Zip.ZipFile(zipFile.FullName);
            bool exists = (zf.GetEntry(relativeFilePath) != null);
            zf.Close();
            return exists;*/
            throw new NotImplementedException();
        }

        /*private string[] findFiles(FileInfo zipFile, string relativePath, string filter, bool recurseSubDirs)
        {
            List<string> results = new List<string>();
            ZipFile zf = new ICSharpCode.SharpZipLib.Zip.ZipFile(zipFile.FullName);

            zf.Close();
            return results.ToArray();
        }*/

        private string[] findFiles(FileInfo zipFile, string relativePath, string filter, bool recurseSubDirs)
        {
            /*
            List<string> results = new List<string>();
            relativePath = relativePath.Replace(@"\", @"/");
            ZipFile zf = new ICSharpCode.SharpZipLib.Zip.ZipFile(zipFile.FullName);
            //ICSharpCode.SharpZipLib.Core.PathFilter pf = new ICSharpCode.SharpZipLib.Core.PathFilter(relativePath);
            foreach (ZipEntry item in zf)
            {
                if (item.Name.StartsWith(relativePath, StringComparison.CurrentCultureIgnoreCase) && item.IsFile)
                {
                    results.Add(string.Format("{0}@{1}", zipFile, item.Name));
                }
                else if (item.IsDirectory)
                {
                    //todo recurse dirs
                }

            }
            zf.Close();
            return results.ToArray();*/
            throw new NotImplementedException();
        }


        internal string[] getFiles(string path, string filter)
        {
            List<string> files = new List<string>();
            foreach (FileInfo zipFile in dataStoreArchives)
            {
                files.AddRange(findFiles(zipFile, path, filter, true));
            }

            //to do paths


            return files.ToArray();
        }


    }
}
