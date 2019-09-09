/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WD_toolbox.Files.XML
{
    public class LocalCacheXmlResolver : XmlResolver
    {
        public string LocalResourcePath { get; set; }

        protected Dictionary<string, byte[]> fileLut;


        public LocalCacheXmlResolver(string path)
        {
            LocalResourcePath = path;
            fileLut = new Dictionary<string, byte[]>();
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            string key = absoluteUri.ToString();
            if (!fileLut.ContainsKey(key))
            {
                string name = Path.GetFileName(key);
                string[] candidates = Directory.GetFiles(LocalResourcePath, name, SearchOption.AllDirectories);
                switch (candidates.Length)
                {
                    case 0:
                        if (absoluteUri.ToString().ToLower().StartsWith("http:"))
                        {
                            byte[] webData = download(key);
                            if (webData != null)
                            {
                                //for next time the aplication is run
                                File.WriteAllBytes(Path.Combine(LocalResourcePath, name), webData);

                                //good to go, why not?
                                fileLut[key] = webData;
                                break;                      // ok !!!! 
                            }
                        }

                        fileLut[key] = null; //maybee it will be ignored?
                        //return null; //maybee it will be ignored?
                        //throw new FileNotFoundException("No candidate available for " + absoluteUri.ToString());
                        break;
                    case 1:
                        fileLut[key] = File.ReadAllBytes(candidates[0]);
                        break;
                    default:
                        throw new NotImplementedException("Guess I need to do this after all.");
                    //fileLut[key] = File.ReadAllBytes(candidates[0]);
                }


            }

            byte[] data = fileLut[key];
            if (ofObjectToReturn == typeof(byte[]))
            {
                return data;
            }
            else if (ofObjectToReturn == typeof(Stream))
            {
                return (data != null) ? new MemoryStream(data) : null;
            }

            throw new ArgumentException("ofObjectToReturn can not be " + ofObjectToReturn);
        }


        public static byte[] download(string url)
        {
            try
            {
                using (WebClient Client = new WebClient())
                {
                    return Client.DownloadData(url);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
