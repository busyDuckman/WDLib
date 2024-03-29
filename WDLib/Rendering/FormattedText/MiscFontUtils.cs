/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Drawing;
using Microsoft.Win32;

namespace WD_toolbox.Rendering.FormattedText
{
/// <summary>
/// Finds fonts
/// </summary>
public abstract class MiscFontUtils
	{
	/// <summary>
	/// Gets the file containing a specfied font
	/// </summary>
	/// <param name="ff">font to look for</param>
	/// <returns>File name, including full pacth</returns>
	public static string getFontFile(FontFamily ff)
		{
		RegistryKey[] keys = new RegistryKey[2];
		keys[0] = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Fonts");
		keys[1] = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Fonts");
			
		string name = ff.Name;
		
		//find a fegistry value, most exact match first
		object o;
		o = getkeyToMatch(keys, name, true, true);
		if(o == null)
			o = getkeyToMatch(keys, name, false, true);
		if(o == null)
			o = getkeyToMatch(keys, name, true, false);
		if(o == null)
			o = getkeyToMatch(keys, name, false, false);
		
		if(o == null)
			return null;
		
		try
			{
			string fileName = (string)o;
			
			if(fileName.IndexOfAny(@"/\".ToCharArray()) != -1)
				{
				//we fave a full path
				return fileName;
				}
			else
				{
				//we have a filename
				string wd = Environment.GetEnvironmentVariable("windir");
				return wd + @"\fonts\" + fileName;
				}
			}
		catch
			{
			return null;
			}
		}
	
	
	/// <summary>
	/// Bit of an adhoc function, does not handle nulls well, but is suficient for the purposes of this class
	/// </summary>
	/// <param name="keys"></param>
	/// <param name="what"></param>
	/// <param name="caseSensitive"></param>
	/// <param name="exact"></param>
	/// <returns></returns>
	protected static object getkeyToMatch(RegistryKey[] keys, string what, bool caseSensitive, bool exact)
		{
		string what2 = what;
		if(!caseSensitive)
			what2 = what2.ToUpper();
		
		foreach(RegistryKey r in keys)
			{
			if(r==null)
				continue;
			
			string[] names = r.GetValueNames();
			foreach(string s in names)
				{
				string s2 = s;
				if(!caseSensitive)
					s2 = s2.ToUpper();
				
				if(exact)
					if(what2 == s2)
						return r.GetValue(s);
				else
					if(s2.StartsWith(what2))
						return r.GetValue(s);
				}
			}
		
		return null;
		}
	}
}
