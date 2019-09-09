/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections;
using System.ComponentModel;
//using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

using Microsoft.Win32;

using WD_toolbox.Data.Text;
using WD_toolbox.Data.Manipulation;


namespace WD_toolbox.AplicationFramework
{
/// <summary>
/// A basic application framework
/// </summary>
public class ApplicationHelper //: System.Windows.Forms.Form
	{
	#region instance data
	//private readonly int daysSinceLastUse;
	#endregion
	#region procedural accessors
	/// <summary>
	/// Gets the current product version
	/// </summary>
	public static string version
		{
		get
			{
                return Application.ProductVersion;
			}
		}
	
	/// <summary>
	/// Gets the key the aplication shoul use a a root when acessing the registery
	/// </summary>
	public static RegistryKey appRegistryPath
		{
		get
			{
                return Application.UserAppDataRegistry;
			}
		}
	#endregion
	
	/// <summary>
	/// Finds days sine program last started
	/// </summary>
	/// <returns>days since last started, 0 if first run</returns>
	public static int dateKeeperSetupAndFindDaysIdle()
		{
		DateTime now = DateTime.Now;
		string s = now.ToString();
		if(getSetting("dinfo", "dinstall") == null)
			{
			storeSetting("dinfo", "dinstall", now);
			}
		
		
		DateTime lastUse = (DateTime)getSetting("dinfo", "duse", now);
		
		if(lastUse <= now)
			{	
			storeSetting("dinfo", "duse", DateTime.Now);
			}
			
		return (int)((TimeSpan)(now - lastUse)).TotalDays;
		}
	
	
	/// <summary>
	/// Writes data to the registery relative to the application base path
	/// </summary>
	/// <param name="path">path (relative to the application base path)</param>
	/// <param name="name">key name</param>
	/// <param name="value">data to write (may be serilized if not a basic type)</param>
	public static void storeSetting(string path, string name, object value)
		{
		RegistryKey r = ApplicationHelper.appRegistryPath;
		if(Text.isNotBlank(path))
			{
			r = ApplicationHelper.appRegistryPath.CreateSubKey(path);
			}
		
		//check if in32 compatable
		if(Querey.search(new Type[] {typeof(sbyte), typeof(byte), typeof(char), typeof(short), typeof(ushort), typeof(int), typeof(uint)},  value.GetType(), new Querey.equalDelegate(typesEqual)) > 0)
			{
			//yes int32ish
			r.SetValue(name, (int)value);
			}
		else if(value is string)
			{
			r.SetValue(name, value);
			}
		else
			{
			//attempt to serialise
			byte[] data = serialise(value);
			if(data != null)
				{
				r.SetValue(name, data);
				}
			else
				{
				//nothing else has worked to let windows save it as a string
				r.SetValue(name, value);
				}
			}
		}
	
	
	/// <summary>
	/// Compares the types of two objects
	/// </summary>
	/// <param name="a">First object</param>
	/// <param name="b">Second object</param>
	/// <returns></returns>
	private static bool typesEqual(object a, object b)
		{
		return ((a as Type) == (b as Type));
		}
		
	
	/// <summary>
	/// retrives an objedct written to the registry by storeSetting
	/// </summary>
	/// <param name="path">path (relative to the application base path)</param>
	/// <param name="name">key name</param>
	/// <returns>data at registery (may be deserilized if not a basic type)</returns>
	public static object getSetting(string path, string name)
		{
		RegistryKey r = appRegistryPath;
		if(Text.isNotBlank(path))
			{
			r = appRegistryPath.OpenSubKey(path);
			}
		if(r == null)
			return null;
		
		
		object val = r.GetValue(name);
		
		//attempt deserialisation if appropriate
		if(val is byte[])
			{
			object o = deSerialise(val as byte[]);
			if(o != null)
				{
				return o;
				}
			}
		
		return val;
		}
		
	
	/// <summary>
	/// This function will return def insted of null is the setting is not found
	/// </summary>
	/// <param name="path">path (relative to the application base path)</param>
	/// <param name="name">key name</param>
	/// <param name="defualt"></param>
	/// <returns>data at registery (may be deserilized if not a basic type), or def</returns>
	public static object getSetting(string path, string name, object def)
		{
		return Querey.NVL(getSetting(path, name), def);
		}
	
	
	#region trial period code
	/// <summary>
	/// Calculates how many days ago the program was installed
	/// </summary>
	/// <returns>How many days ago the program was installed</returns>
	public static int daysSinceInstalled()
		{
		try
			{
			object installObj = getSetting("dinfo", "dinstall");
			if(installObj == null)
				return -1;
			
			DateTime install = (DateTime)installObj;
			return (int)((TimeSpan)(DateTime.Now - install)).TotalDays;
			}
		catch
			{
			}
		return -1;
		}
	
	
	/// <summary>
	/// Evaluates if the trail period has expired
	/// Checks if clock is greater than last use to validate result
	/// </summary>
	/// <param name="days">duration of trial period</param>
	/// <returns>true if trail has expired, otherwise false</returns>
	public static bool hasTrialPeriodExpired(int days)
		{
		int install = daysSinceInstalled();
		int daysSinceUsed = dateKeeperSetupAndFindDaysIdle();
		
		if(install == -1)
			{
			return false;
			}
		
		if(daysSinceUsed > install)
			{
			return true;
			}
		if(daysSinceUsed < 0)
			{
			return true;
			}
		if(install > days)
			{
			return true;
			}
		
		// no problems so continue trial
		return false;
		}
	#endregion
	
	#region serialization of registry values
	/// <summary>
	/// Creates a byte version of a serializable object
	/// </summary>
	/// <param name="obj">object to serialize</param>
	/// <returns>bytes representing the object, null if object can not be serilized</returns>
	public static byte[] serialise(object obj)
		{
		Stream s = new MemoryStream();
		try
			{
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(s, obj);
			byte[] data= new byte[s.Length];
			s.Position =0;
			s.Read(data, 0, data.Length);
			s.Close();
			return data;
			}
		catch
			{
			s.Close();
			return null;
			}
		}
	
	
	/// <summary>
	/// Creates an object from it's serilized byte version
	/// </summary>
	/// <param name="data"></param>
	/// <returns></returns>
	public static object deSerialise(byte[] data)
		{
		object r = null;
		Stream s = new MemoryStream(data);
		
		try
			{
			BinaryFormatter bf = new BinaryFormatter();
			r = bf.Deserialize(s);
			}
		catch
			{
			}
		
		s.Close();
		return r;
		}
	#endregion
	
	#region serial number support
	/*
	/// <summary>
	/// load serial number and request code from the registery
	/// If no code is present prompt for one
	/// </summary>
	/// <param name="id">The product ID for evaluation purposes</param>
	/// <returns></returns>
	public static bool checkAndGetSerial(int id)
		{
		string serial = getSerial();
		string rc = getRC();
		if((serial == null)||(rc == null))
			{
			registerUser ru = new registerUser(id);
			return (ru.ShowDialog() == DialogResult.Yes);
			}
		if(!verifySerial(id, rc, serial, "xxx-xxx-xxx"))
			{
			MessageBox.Show("Stored serial number is not valid");
			registerUser ru = new registerUser(id);
			return (ru.ShowDialog() == DialogResult.Yes);
			}
		
		return true;
		}*/
	
	/// <summary>
	/// Get serial from the registery
	/// </summary>
	/// <returns>The serial Number</returns>
	public static string getSerial()
		{
		try
			{
			return (string)getSetting("LIC", "SN");
			}
		catch
			{
			return null;
			}
		}
	
	/// <summary>
	/// Write sewrial number to the registery
	/// </summary>
	/// <param name="serial">Serial number</param>
	public static void setSerial(string serial)
		{
		storeSetting("LIC", "SN", serial);
		}
	
	/// <summary>
	/// Get request code from the registery
	/// </summary>
	/// <returns>The request code</returns>
	public static string getRC()
		{
		try
			{
			return (string)getSetting("LIC", "RC");
			}
		catch
			{
			return null;
			}
		}
	
	/// <summary>
	/// Write request code to the registery
	/// </summary>
	/// <param name="rc">request code to write</param>
	public static void setRC(string rc)
		{
		storeSetting("LIC", "RC", rc);
		}
	
	/// <summary>
	/// Get user name from the registery
	/// </summary>
	/// <returns>The user name</returns>
	public static string getUN()
		{
		try
			{
			return (string)getSetting("LIC", "UN");
			}
		catch
			{
			return null;
			}
		}
	
	/// <summary>
	/// Write user name to the registery
	/// </summary>
	/// <param name="un">user name  to write</param>
	public static void setUN(string un)
		{
		storeSetting("LIC", "UN", un);
		}
	
	/// <summary>
	/// Verifies the Serial Number
	///serial must be in the form xxx-xxx-xxx
	/// </summary>
	/// <param name="productCode"></param>
	public static bool verifySerial(int productCode, string requestCode, string serial)
		{
		return verifySerial(productCode, requestCode, serial, "xxx-xxx-xxx");
		}
	
	/// <summary>
	/// Verifies the Serial Number
	/// </summary>
	/// <param name="productCode"></param>
	/// <param name="format">eg xxxx-xxxx-xxxx-xxxx</param>
	public static bool verifySerial(int productCode, string requestCode, string serial, string format)
		{
		if(format == null)
			return false;
		if(serial == null)
			return false;
		if(requestCode == null)
			return false;
		if(format.Length != serial.Length)
			return false;
		if(format.Length != requestCode.Length)
			return false;
		if(productCode == 0)
			return false;
		
		int i;
		for(i=0;i<format.Length;i++)
			{
			switch(format.ToUpper()[i])
				{
				case 'X':
					if(!char.IsLetterOrDigit(serial, i))
						return false;
					break;
				case '-':
					if(serial[i] != '-')
						return false;
					break;
				case ' ':
					if(!char.IsWhiteSpace(serial, i))
						return false;
					break;
				}
			}
		
		
		if(serial.ToUpper() != generateSerial(requestCode.ToUpper()))
			return false;
		
		i = hash(requestCode.ToUpper());
		
		return verifyProductCode(productCode, i);
		}	
	
	
	/// <summary>
	/// Creates a serial number from a request code
	/// </summary>
	/// <param name="requestCode">request code</param>
	/// <returns>valid serial number</returns>
	public static string generateSerial(string requestCode)
		{
		string s = "";
		int i;
		for(i=0;i<requestCode.Length;i++)
			{
			switch(requestCode.ToUpper()[i])
				{
				case '-':
					s += "-";
					break;
				case ' ':
					s += " ";
					break;
				default:
					s += requestCharLUT(requestCode[requestCode.Length - 1 - i]);
					break;
				
				}
			}
		return s.ToUpper();
		}
	
	private static char requestCharLUT(char code)
		{
		char[] lut = "plmoknijbuhvygctfxrdzeswaq6574839201plmoknijbuhvygctfxrdzeswaq6574839201plmoknijbuhvygctfxrdzeswaq6574839201plmoknijbuhvygctfxrdzeswaq6574839201plmoknijbuhvygctfxrdzeswaq6574839201plmoknijbuhvygctfxrdzeswaq6574839201plmoknijbuhvygctfxrdzeswaq6574839201plmo".ToCharArray();
		string s = "" + code;
		return lut[((byte)code)];
		}
	
	private static int hash(string what)
		{
		int i;
		int /*e=1,*/ r=0;
		for(i=0;i<what.Length;i++)
			{
			//n = (int)what[i];
			//e *= n;
			//e += 1;
			//r += n + e;
			//large prime mult to even out the hash a little
			//r *= 7919;
			r += (int)what[i]*3;
			}
		//r = r % 10;
		return r;
		}
	
	private static bool verifyProductCode(int productCode, int hash)
		{
		return ((hash%90) == (productCode%90));
		}
		
	/// <summary>
	/// Used as a keygen, #if'd out for all relase builds
	/// </summary>
	/// <param name="productCode">code for product</param>
	/// <param name="format">format for product</param>
	/// <param name="max">max codes to return</param>
	/// <returns></returns>
	public static ArrayList getRequestCodes(int productCode, string format, int max)
		{
		ArrayList al = new ArrayList();
		#if DEBUG
		long i, c=0;
		//long absoluteMax = 0;
		//36 chars total
		
		string fString = "";
		for(i=0;i<format.Length;i++)
			{
			if(format.ToUpper()[(int)i] == 'X')
				{
				fString += "{" + c + "}";
				c++;
				}
			else
				{
				fString += format[(int)i];
				}
			}
		
		//create a vector for all points int the format scring
		object[] vector = new object[c];
		//for(i=0;i<absoluteMax;i++)
		char[] lut = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
		c=0;
		Random r = new Random(0xffff);
		string test;
		int percent = 0;
		ulong misses = 0;
		
		while(al.Count < max)
			{
			for(i=0;i<vector.Length;i++)
				vector[i] = lut[r.Next(lut.Length)];
			
			test = string.Format(fString, vector);
			
			if(verifyProductCode(productCode, hash(test)))
				{
				if(!al.Contains(test))
					{
					al.Add(test);
					int np = (int)(((float)al.Count / (float)max)*100.0f);
					if(np != percent)
						{
						Console.Write(" {0}% ", np);
						}
					percent = np;
					}
				}
			else
				{
				misses++;
				}
			
			}
		Console.WriteLine();
		Console.WriteLine("{0} keys from {1} attampts = {2}% safe", al.Count, misses + (ulong)al.Count, 100.0f-((al.Count/(float)(misses + (ulong)al.Count))*100.0f));
		//#else
		//al.Add("Insufficient credentials");
		#endif
		return al;
		}
	#endregion
	}
}
