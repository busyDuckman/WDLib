/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Reflection;
using System.IO;

namespace WD_toolbox.Files
{
/// <summary>
/// Functions for dealing with files
/// </summary>
public class Files
		{
		/// <summary>
		/// Get all the data in a file as an array of bytes.
		/// </summary>
		/// <param name="filename">A path to an existing file.</param>
		/// <returns>An array of bytes with all the data retrivable from the named file.</returns>
		public static byte[] loadFile(string filename)
			{
			FileStream fs = new FileStream(filename, FileMode.Open);
			byte[] data = new byte[fs.Length];
			fs.Read(data, 0, data.Length);
			fs.Close();
			return data;
			}
		
		
		/// <summary>
		/// Save an array of bytes to a file.
		/// </summary>
		/// <param name="filename">A path to a file.</param>
		/// <param name="data">An array of bytes with all the data to be written to the file.</param>
		/// <returns>True is sucsessfull; otherwise false.</returns>
		public static bool saveFile(string filename, byte[] data)
			{
			try
				{
				FileStream fs = new FileStream(filename, FileMode.Create);
				try
					{
					fs.Write(data, 0, data.Length);
					}
				finally
					{
					fs.Close();
					}
				return true;
				}
			catch
				{
				return false;
				}
			}
		
		/// <summary>
		/// Save an string to a file.
		/// </summary>
		/// <param name="filename">A path to a file.</param>
		/// <param name="data">A string (data to be written to the file).</param>
		/// <returns>True is sucsessfull; otherwise false.</returns>
		public static bool saveFile(string filename, string asciiText)
			{
			try
				{
				TextWriter fs = File.CreateText(filename);
				try
					{
					fs.Write(asciiText);
					}
				finally
					{
					fs.Close();
					}
				return true;
				}
			catch
				{
				return false;
				}
			}
		
		
		/// <summary>
		/// Gets the size of a file
		/// This is done by opening the file and examening the lengh.
		/// Use fileinfo for for more general applications.
		/// </summary>
		/// <param name="filename">A path to a file.</param>
		/// <returns>-1 if file can not be accesed, else the file size</returns>
		public static long fileSize(string filename)
			{
			try
				{
				if(exists(filename))
					{
					FileInfo fi = new FileInfo(filename);
					return fi.Length;
					}
				else
					{
					return -1;
					}
				}
			catch
				{
				}
			return -1;			
			}
		
		/// <summary>
		/// Currently all this does is call File.Exists().
		/// This is offered here as a means of determining wheter a function in this class will find a file.
		/// As Linux support comes along and we will have to deal with case sensative filenames this function will appropiatly reflect
		/// how this class finds files.
		/// It is considered best practie to use this exists when working with the functions in this class, and to use File.Exists() when working with .NET file functions.
		/// </summary>
		/// <param name="filename">A path to a file.</param>
		/// <returns>true if the file exists; otherwise false.</returns>
		public static bool exists(string filename)
			{
			return File.Exists(filename);
			}
		
		/// <summary>
		/// Determines whether two files are the same
		/// </summary>
		/// <param name="fileA">Path to file a</param>
		/// <param name="fileB">Path to file b</param>
		/// <returns>True if the files are the same; otherwise false.</returns>
		public static bool compare(string fileA, string fileB)
			{
			if(fileSize(fileA) != fileSize(fileB))
				return false;
			
			int i, j;
			
			FileStream fsA = new FileStream(fileA, FileMode.Open);
			FileStream fsB = new FileStream(fileB, FileMode.Open);
			
			//buffer in 5mb chnks of data
			int bufferSize = 1024*1024*5;
			byte[] bufferA = new byte[bufferSize];
			byte[] bufferB = new byte[bufferSize];
			for(i=0;i<fsA.Length;i+=bufferSize)
				{
				int readSize=bufferSize;
				
				//when the buffer overflows the file
				if((i+readSize) >= fsA.Length)
					{
					readSize = (int)(fsA.Length - 1 - i);
					}
				
				fsA.Read(bufferA,0, readSize);
				fsB.Read(bufferB,0, readSize);
				
				for(j=0;j<readSize;j++)
					{
					if(bufferA[j] != bufferB[j])
						{
						//diference found
						
						fsA.Close();
						fsB.Close();
						return false;
						}
					}
				
				}
			
			fsA.Close();
			fsB.Close();
			
			//no diferences found
			return true;
			}
		
		
		/// <summary>
		/// Opens a file, converts it to ascii and returns an array of strings representing the lines in the file.
		/// </summary>
		/// <param name="filename">the file to open</param>
		/// <returns>an array of strings representing the lines in the file.</returns>
		public static string[] openFileAsStringArray(string filename)
			{
			byte[] data = loadFile(filename);
			if(data == null)
				return null;
			if(data.Length == 0)
				return new string[0];
			
			
			string s = System.Text.Encoding.ASCII.GetString(data);
			s = s.Replace("\r\n", "\n");
			s = s.Replace("\r", "\n");
			return s.Split("\n".ToCharArray());
			}
		
		/// <summary>
		/// given c\foo\bar.txt returns c\foo\
		/// </summary>
		/// <param name="URL">A url</param>
		/// <returns> The path portion of a url.</returns>
		public static string getPath(string URL)
			{
			int sep = URL.LastIndexOfAny(@"/\".ToCharArray());
			if(sep == -1)
				return "";
			return URL.Substring(0, sep+1);
			}
		
		/// <summary>
		/// given c:\foo\bar.txt returns bar.txt
		/// </summary>
		/// <param name="URL">A url</param>
		/// <returns> The name portion of a url.</returns>
		public static string getName(string URL)
			{
			int sep = URL.LastIndexOfAny(@"/\".ToCharArray());
			if(sep == -1)
				return URL;
			if(sep == (URL.Length-1))
				return "";
			try
				{
				return URL.Substring(sep+1);
				}
			catch
				{
				return "";
				}
			}
		
		/// <summary>
		/// given bar.txt returns bar
		/// </summary>
		/// <param name="name">A name (not a URL) use getName to extract the name from a URL. <see cref="helpers.files.getName"/></param>
		/// <returns>The prefix portion of a name.</returns>
		public static string getPrefix(string name)
			{
			int sep = name.LastIndexOfAny(@".".ToCharArray());
			if(sep == -1)
				return name;
			return name.Substring(0, sep);
			}
		
		/// <summary>
		/// given bar.txt returns txt
		/// </summary>
		/// <param name="name">A name (not a URL) use getName to extract the name from a URL. <see cref="helpers.files.getName"/></param>
		/// <returns>The extension portion of a name.</returns>
		public static string getExtension(string name)
			{
			int sep = name.LastIndexOfAny(@".".ToCharArray());
			if(sep == -1)
				return "";
			return name.Substring(sep);
			}
		
		/// <summary>
		/// given elephant100 returns 100
		/// NOTE: if given 9811 returns 811 (there must be at least one character that is not part of the number)
		/// </summary>
		/// <param name="prefix">A prefixof name (not a URL or a name).
		///  Use getPrefic to extract the prefix from a name. <see cref="helpers.files.getPrefix"/></param>
		///  Use getName to extract the name from a URL. <see cref="helpers.files.getName"/></param>
		/// <returns>The numeration portion of a name prefix.</returns>
		public static string getNumerator(string prefix)
			{
			int i = prefix.Length;
			string num = "";
			//i stops at 1 because if the name is all numbers than the first digit is considered the textual name
			for(i=prefix.Length-1;i>1;i--)
				{
				if("0123456789".IndexOf(prefix[i]) != -1)
					{
					num = prefix[i] + num;
					}
				else
					{
					break;
					}
				}
			
			return num;
			}
		
		/// <summary>
		/// given elephant100 returns elephant
		/// NOTE: if given 9811 returns 9 (there must be at least one character that is not part of the number)
		/// </summary>
		/// <param name="prefix">A prefixof name (not a URL or a name).
		///  Use getPrefic to extract the prefix from a name. <see cref="helpers.files.getPrefix"/></param>
		///  Use getName to extract the name from a URL. <see cref="helpers.files.getName"/></param>
		/// <returns>The textual portion of a name prefix.</returns>
		public static string getBeforeNumerator(string prefix)
			{
			string num = getNumerator(prefix);
			return prefix.Substring(0, prefix.Length - num.Length);
			}
		
		/// <summary>
		/// Reformats a path to a new format.
		/// </summary>
		/// <param name="path">A string with a path</param>
		/// <param name="seperator">The diesired subtree seperator.</param>
		/// <param name="seperatorAtEnd">Should ther be a seperator at the end of the path (if there isn't one already).</param>
		/// <returns>A new (reformatted) path string.</returns>
		public static string formatPath(string path, char seperator, bool seperatorAtEnd)
			{
			//unify the seperators
			string path2 = path.Replace(@"\", seperator.ToString());
			path2 = path2.Replace(@"/", seperator.ToString());
			
			int sep = path2.LastIndexOf(seperator);
			if(sep != (path2.Length-1))
				{
				//no seperator a end
				if(seperatorAtEnd)
					{
					//but ther should be
					path2 = path2 + seperator;
					}
				}
			else
				{
				//seperator at end
				if(!seperatorAtEnd)
					{
					//but ther shouldn't be
					path2 = path2.Substring(0, path2.Length -1);
					}
				}
			
			return path2;
			}
		
		/// <summary>
		/// Given c:\foo\bar30.txt returns c:\foo\bar0031.txt
		/// Given c:\foo\bar099.txt returns c:\foo\bar0100.txt
		/// Given c:\foo\bar.txt returns c:\foo\bar0002.txt
		/// Note if given c:\foo\999.txt returns c:\foo\9100.txt (there must be at least one character that is not part of the number)
		/// </summary>
		/// <param name="URL">A string with a URL</param>
		/// <returns>An incremented URL string.</returns>
		public static string incermentURL(string URL)
			{
			string name = getName(URL);
			string path = getPath(URL);
			string prefix = getPrefix(name);
			string extension = getExtension(name);
			string baseName = getBeforeNumerator(prefix);
			string numerator = getNumerator(prefix);
			
			int num = 2;
			try
				{
				num = int.Parse(numerator) + 1;
				}
			catch
				{
				}
			
			return string.Format("{0}{1}{2}.{3}", path, baseName, num.ToString("0000"), extension);
			}
		}
	
}
