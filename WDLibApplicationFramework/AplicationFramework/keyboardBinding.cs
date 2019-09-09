/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections;
using System.Windows.Forms;

using WD_toolbox.Data.Conversion;
using WD_toolbox.Maths.Range;

namespace WD_toolbox.AplicationFramework
{
/// <summary>
/// Stores events related to keyboard actions.
/// </summary>
[Serializable]
public class KeyboardBinding : ICloneable
	{
	#region instance data
	/// <summary>
	/// Stores objects assosiated with key presses. Indexed by keycode.
	/// </summary>
	protected Hashtable bindings;
	#endregion
	
	#region proxy data
	#endregion
	
	#region static data
	/// <summary>
	/// Used when creating strings for user inerfaces that setup key bindings.
	/// </summary>
	protected static string seperator = " - ";
	#endregion
	
	/// <summary>
	/// Creates a new keyboardBinding object.
	/// </summary>
	public KeyboardBinding()
		{
		bindings = new Hashtable();
		}
	
	/// <summary>
	/// Copy constructor
	/// </summary>
	/// <param name="another">keyboardBinding to copy</param>
	protected KeyboardBinding(KeyboardBinding another)
		{
		bindings = (Hashtable)another.bindings.Clone();
		}
	
	#region binding of keys
	/// <summary>
	/// Binds an object to a key.
	/// </summary>
	/// <param name="keycommand">Key</param>
	/// <param name="what">Object to bind to key.</param>
	/// <returns>Returns true if sucsessfull.</returns>
	public bool bindKey(Keys keycommand, object what)
		{
		try
			{
			bindings.Add(keycommand, what);
			}
		catch
			{
			return false;
			}
		return true;
		}
	
	/// <summary>
	/// Removes a key binding.
	/// </summary>
	/// <param name="keycommand">Key to remove binding for.</param>
	public void removeKey(Keys keycommand)
		{
		bindings.Remove(keycommand);
		}
	
	/// <summary>
	/// Binds an object to a key overrideing any existing binding.
	/// </summary>
	/// <param name="keycommand">Key</param>
	/// <param name="what">Object to bind to key.</param>
	public void reBindKey(Keys keycommand, object what)
		{
		if(bindings.Contains(keycommand))
			removeKey(keycommand);
		bindKey(keycommand, what);
		}
	#endregion
	
	#region processing keys
	/// <summary>
	/// Returns the object assosiated with a key.
	/// </summary>
	/// <param name="keycommand">Key to look up.</param>
	/// <returns>Object aassosiated with a key, or null if no binding for the key exists.</returns>
	public object keyPressed(Keys keycommand)
		{
		if(bindings.Contains(keycommand))
			{
			return bindings[keycommand];
			}
		else
			{
			return null;
			}
		}
	#endregion
	
	#region related to creating strings for user inerfaces that setup key bindings
	/// <summary>
	/// Lists all bound keys.
	/// </summary>
	/// <returns>A list of all avaiable keys</returns>
	public string[] listBindingDescriptions()
		{
		int i=0;
		string[] result = new string[bindings.Keys.Count];
		foreach(Keys k in bindings.Keys)
			{
			result[i] = keyToString(k) + seperator + bindings[k].ToString();
			i++;
			}
		return result;
		}
	
	/// <summary>
	/// Get key code refered to by string returned from listBindingDescriptions. <see cref="KeyboardBinding.listBindingDescriptions"/>
	/// </summary>
	/// <param name="desc">A String returned from listBindingDescriptions</param>
	/// <returns>A key code. Thows an exception if there was an error.</returns>
	public Keys keyFromBindingDescription(string desc)
		{
		int i = desc.IndexOf(seperator);
		if(i == -1)
			throw new Exception("can not parse description " + desc);
		
		return stringToKey(desc.Substring(0, i).Trim());
		}
	
	/// <summary>
	/// Converts all keycodes to a array of descriptive strings.
	/// </summary>
	/// <returns>A descriptive string array.</returns>
	public static string[] allKeysToString()
		{
		int i;
		int[] allKeyValues = Conversion.enumToIntList(typeof(Keys));
		string[] keyNames = new string[allKeyValues.Length];
		for(i=0;i<allKeyValues.Length;i++)
			{
			keyNames[i] = keyToString((Keys)allKeyValues[i]);
			}
		return keyNames;
		}
	
	/// <summary>
	/// Convers a keycode to a descriptive string.
	/// </summary>
	/// <param name="keycommand">A key code.</param>
	/// <returns>A descriptive string.</returns>
	public static string keyToString(Keys keycommand)
		{
		KeysConverter kc = new KeysConverter();
		return kc.ConvertToString(keycommand);
		}
	
	/// <summary>
	/// Convers a descriptive string to a keycode.
	/// </summary>
	/// <param name="keyString">String to convert.</param>
	/// <returns>A key code.</returns>
	public static Keys stringToKey(string keyString)
		{
		KeysConverter kc = new KeysConverter();
		return (Keys)kc.ConvertFromString(keyString);
		}
	
	#endregion
	
	#region IClonable Members
	/// <summary>
	/// Makes a clone of this object.
	/// </summary>
	/// <returns>clone of this object</returns>
	public object Clone()
		{
		return new KeyboardBinding(this);
		}
	#endregion
}
	
}
