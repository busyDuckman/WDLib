/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace WD_toolbox.Hardware
{
/// <summary>
/// Extends the screen class to allow for modification of screen resolutions.
/// </summary>
public class Display
	{
	#region data types
	/// <summary>
	/// Windows DEVMODE structure
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct DEVMODE 
	{
		/// <summary>
		/// Specifies the "friendly" name of the printer or display; for example, "PCL/HP LaserJet" in the case of PCL/HP LaserJet®. This string is unique among device drivers. Note that this name may be truncated to fit in the dmDeviceName array. 
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst=32)]
		public string dmDeviceName;
		
		/// <summary>
		/// Specifies the version number of the initialization data specification on which the structure is based. To ensure the correct version is used for any operating system, use DM_SPECVERSION. 
		/// </summary>
		public short  dmSpecVersion;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short  dmDriverVersion;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short  dmSize;
		
		/// <summary>
		/// 
		/// </summary>
		public short  dmDriverExtra;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public int    dmFields;

		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short dmOrientation;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short dmPaperSize;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short dmPaperLength;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short dmPaperWidth;

		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short dmScale;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short dmCopies;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short dmDefaultSource;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short dmPrintQuality;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short dmColor;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short dmDuplex;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short dmYResolution;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short dmTTOption;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short dmCollate;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string dmFormName;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short dmLogPixels;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public short dmBitsPerPel;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public int   dmPelsWidth;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public int   dmPelsHeight;

		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public int   dmDisplayFlags;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public int   dmDisplayFrequency;

		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public int   dmICMMethod;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public int   dmICMIntent;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public int   dmMediaType;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public int   dmDitherType;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public int   dmReserved1;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public int   dmReserved2;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public int   dmPanningWidth;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public int   dmPanningHeight;
	};
	
	/// <summary>
	/// Windows DISPLAYDEVICE structure
	/// </summary>
	public struct DISPLAYDEVICE 
		{
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public int cb;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string DeviceName;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceString;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		public int StateFlags;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceID;
		
		/// <summary>
		/// See Windows sdk docomentation.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceKey;
		}


	/* VIDEOPARAMETERS.dwCommand constants */
	enum VIDEOPARAMETERSdwCommand : int {VP_COMMAND_GET = 0x00000001, VP_COMMAND_SET = 0x00000002};

	/* VIDEOPARAMETERS.dwFlags constants */
	enum VIDEOPARAMETERSdwFlags : ulong {
										VP_FLAGS_TV_MODE=0x00000001,
										VP_FLAGS_TV_STANDARD=0x00000002,
										VP_FLAGS_FLICKER=0x00000004,
										VP_FLAGS_OVERSCAN=0x00000008,
										VP_FLAGS_MAX_UNSCALED=0x00000010,
										VP_FLAGS_POSITION=0x00000020,
										VP_FLAGS_BRIGHTNESS=0x00000040,
										VP_FLAGS_CONTRAST=0x00000080,
										VP_FLAGS_COPYPROTECT=0x00000100
										};

	/* VIDEOPARAMETERS.dwMode constants */
	enum VIDEOPARAMETERSdwMode : ulong {
									VP_MODE_WIN_GRAPHICS=0x00000001,
									VP_MODE_TV_PLAYBACK=0x00000002
									};

	/* VIDEOPARAMETERS.dwTVStandard/dwAvailableTVStandard constants */
	enum VIDEOPARAMETERSdwTVStandard : ulong {
									VP_TV_STANDARD_NTSC_M = 0x00000001,
									VP_TV_STANDARD_NTSC_M_J = 0x00000002,
									VP_TV_STANDARD_PAL_B = 0x00000004,
									VP_TV_STANDARD_PAL_D = 0x00000008,
									VP_TV_STANDARD_PAL_H = 0x00000010,
									VP_TV_STANDARD_PAL_I = 0x00000020,
									VP_TV_STANDARD_PAL_M = 0x00000040,
									VP_TV_STANDARD_PAL_N = 0x00000080,
									VP_TV_STANDARD_SECAM_B = 0x00000100,
									VP_TV_STANDARD_SECAM_D = 0x00000200,
									VP_TV_STANDARD_SECAM_G = 0x00000400,
									VP_TV_STANDARD_SECAM_H = 0x00000800,
									VP_TV_STANDARD_SECAM_K = 0x00001000,
									VP_TV_STANDARD_SECAM_K1 = 0x00002000,
									VP_TV_STANDARD_SECAM_L = 0x00004000,
									VP_TV_STANDARD_WIN_VGA = 0x00008000,
									VP_TV_STANDARD_NTSC_433 = 0x00010000,
									VP_TV_STANDARD_PAL_G = 0x00020000,
									VP_TV_STANDARD_PAL_60 = 0x00040000,
									VP_TV_STANDARD_SECAM_L1 = 0x00080000
									}

	/* VIDEOPARAMETERS.dwMode constants */
	enum VIDEOPARAMETERSdwCPType : ulong {
									VP_CP_TYPE_APS_TRIGGER = 0x00000001,
									VP_CP_TYPE_MACROVISION = 0x00000002
									}

	/* VIDEOPARAMETERS.dwCPCommand constants */
	enum VIDEOPARAMETERSdwCPCommand : ulong {
									VP_CP_CMD_ACTIVATE = 0x00000001,
									VP_CP_CMD_DEACTIVATE = 0x00000002,
									VP_CP_CMD_CHANGE = 0x00000004
									}
									
	/// <summary>
	/// Change display mode dwflags paramater
	/// </summary>
	public enum ChangeDisplaySettingsDwFalgs : int {
											/// <summary>
											/// Type of display mode change
											/// </summary>
											changeDyamically=0,
											/// <summary>
											/// Persistant dispay mode change
											/// </summary>
											CDS_UPDATEREGISTRY= 0x00000001,
											/// <summary>
											/// Type of display mode change
											/// </summary>
											CDS_TEST= 0x00000002,
											/// <summary>
											/// No start button.
											/// </summary>
											CDS_FULLSCREEN= 0x00000004,
											/// <summary>
											/// Type of display mode change
											/// </summary>
											CDS_GLOBAL= 0x00000008,
											/// <summary>
											/// Type of display mode change
											/// </summary>
											CDS_SET_PRIMARY = 0x00000010,
											/// <summary>
											/// Type of display mode change
											/// </summary>
											CDS_RESET = 0x40000000,
											/// <summary>
											/// Type of display mode change
											/// </summary>
											CDS_SETRECT = 0x20000000,
											/// <summary>
											/// Type of display mode change
											/// </summary>
											CDS_NORESET = 0x10000000
											/// <summary>
											/// Type of display mode change
											/// </summary>
											};
	
	/// <summary>
	/// Change display mode return values
	/// </summary>
	public enum ChangeDisplaySettingsReturnValue : int {
											/// <summary>
											/// Mode was sucsessfuly changes
											/// </summary>							
											DISP_CHANGE_SUCCESSFUL =0,
											/// <summary>
											/// Error ocured, see win sdk doco
											/// </summary>
											DISP_CHANGE_RESTART = 1,
											/// <summary>
											/// Error ocured, see win sdk doco
											/// </summary>
											DISP_CHANGE_FAILED = -1,
											/// <summary>
											/// Error ocured, see win sdk doco
											/// </summary>
											DISP_CHANGE_BADMODE = -2,
											/// <summary>
											/// Error ocured, see win sdk doco
											/// </summary>
											DISP_CHANGE_NOTUPDATED = -3,
											/// <summary>
											/// Error ocured, see win sdk doco
											/// </summary>
											DISP_CHANGE_BADFLAGS = -4,
											/// <summary>
											/// Error ocured, see win sdk doco
											/// </summary>
											DISP_CHANGE_BADPARAM = -5
											/// <summary>
											/// Error ocured, see win sdk doco
											/// </summary>
											};
	/// <summary>
	/// contains information for a video connection.
	/// </summary>
	public struct VIDEOPARAMETERS 
		{
		/// <summary>
		/// Specifies the GUID for this structure. {02C62061-1097-11d1-920F-00A024DF156E}.
		/// </summary>
		Guid  guid; 
		
		/// <summary>
		/// must be zero.
		/// </summary>
		ulong  dwOffset;
		
		/// <summary>
		/// Specifies whether to retrieve or set the values that are indicated by the other members of this structure. This member can be one of the following values.
		/// </summary>
		VIDEOPARAMETERSdwCommand  dwCommand; 
		
		/// <summary>
		/// Indicates which fields contain valid data. For VP_COMMAND_GET, this should be zero.
		/// </summary>
		VIDEOPARAMETERSdwFlags  dwFlags; 
		
		/// <summary>
		/// Specifies the current playback mode. This member is valid for both VP_COMMAND_GET and VP_COMMAND_SET.
		/// </summary>
		VIDEOPARAMETERSdwMode  dwMode; 
		
		/// <summary>
		/// Specifies the TV standard. This field is valid for both VP_COMMAND_GET and VP_COMMAND_SET.
		/// </summary>
		VIDEOPARAMETERSdwTVStandard  dwTVStandard; 
		
		/// <summary>
		/// Specifies which modes are available. This is valid only for VP_COMMAND_GET. It can be any combination of the values specified in dwMode.
		/// </summary>
		ulong  dwAvailableModes; 
		
		/// <summary>
		/// AvailableTVStandard Specifies the TV standards that are available. This is valid only for VP_COMMAND_GET. It can be any combination of the values specified in dwTVStandard.
		/// </summary>
		ulong dwAvailableTVStandard; 
		
		/// <summary>
		/// Specifies the flicker reduction provided by the hardware. This is a percentage value in tenths of a percent, from 0 to 1,000, where 0 is no flicker reduction and 1,000 is maximum flicker reduction. This field is valid for both VP_COMMAND_GET and VP_COMMAND_SET.
		/// </summary>
		ulong  dwFlickerFilter; 
		
		/// <summary>
		/// Specifies the amount of overscan in the horizontal direction. This is a percentage value in tenths of a percent, from 0 to 1,000. A value of 0 indicates no overscan, ensuring that the entire display is visible. A value of 1,000 is maximum overscan and typically causes some of the image to be off the edge of the screen. This field is valid for both VP_COMMAND_GET and VP_COMMAND_SET.
		/// </summary>
		ulong  dwOverScanX; 
		
		/// <summary>
		/// Specifies the amount of overscan in the vertical direction. This is a percentage value in tenths of a percent, from 0 to 1,000. A value of 0 indicates no overscan, ensuring that the entire display is visible. A value of 1,000 is maximum overscan and typically causes some of the image to be off the edge of the screen. This field is valid for both VP_COMMAND_GET and VP_COMMAND_SET.
		/// </summary>
		ulong  dwOverScanY; 
		
		/// <summary>
		/// Specifies the maximum horizontal resolution, in pixels, that is supported when the video is not scaled. This field is valid for both VP_COMMAND_GET.
		/// </summary>
		ulong  dwMaxUnscaledX; 
		
		/// <summary>
		/// Specifies the maximum vertical resolution, in pixels, that is supported when the video is not scaled. This field is valid for both VP_COMMAND_GET.
		/// </summary>
		ulong  dwMaxUnscaledY; 
		
		/// <summary>
		/// Specifies the horizontal adjustment to the center of the image. Units are in pixels. This field is valid for both VP_COMMAND_GET and VP_COMMAND_SET.
		/// </summary>
		ulong  dwPositionX; 
		
		/// <summary>
		/// Specifies the vertical adjustment to the center of the image. Units are in scan lines. This field is valid for both VP_COMMAND_GET and VP_COMMAND_SET.
		/// </summary>
		ulong  dwPositionY; 
		
		/// <summary>
		/// Adjustment to the DC offset of the video signal to increase brightness on the television. It is a percentage value, 0 to 100, where 0 means no adjustment and 100 means maximum adjustment. This field is valid for both VP_COMMAND_GET and VP_COMMAND_SET.
		/// </summary>
		ulong  dwBrightness; 
		
		/// <summary>
		/// Adjustment to the gain of the video signal to increase the intensity of whiteness on the television. It is a percentage value, 0 to 100, where 0 means no adjustment and 100 means maximum adjustment. This field is valid for both VP_COMMAND_GET and VP_COMMAND_SET.
		/// </summary>
		ulong  dwContrast; 
		
		/// <summary>
		/// Specifies the copy protection type. This field is valid for both VP_COMMAND_GET and VP_COMMAND_SET. It can be one of the following.
		/// </summary>
		VIDEOPARAMETERSdwCPType  dwCPType; 
		
		/// <summary>
		/// Specifies the copy protection command. This field is only valid for VP_COMMAND_SET. It can be one of the following.
		/// </summary>
		VIDEOPARAMETERSdwCPCommand  dwCPCommand; 
		
		/// <summary>
		/// Specifies TV standards for which copy protection types are available. This field is valid only for VP_COMMAND_GET.
		/// </summary>
		ulong  dwCPStandard; 
		
		/// <summary>
		/// Specifies the copy protection key returned if dwCPCommand is set to VP_CP_CMD_ACTIVATE. The caller must set this key when the dwCPCommand field is either VP_CP_CMD_DEACTIVATE or VP_CP_CMD_CHANGE. If the caller sets an incorrect key, the driver must not change the current copy protection settings. This field is valid only for VP_COMMAND_SET.
		/// </summary>
		ulong  dwCPKey; 
		
		/// <summary>
		/// Specifies the DVD APS trigger bit flag. This is valid only for VP_COMMAND_SET. Currently, only bits 0 and 1 are valid. It can be one of the following:
		/// 0 No copy protection.
		/// 1, 2, or 3 Macrovision-defined analog protection methods.
		/// </summary>
		ulong  bCP_APSTriggerBits; 
		
		/// <summary>
		/// Specifies the OEM-specific copy protection data. Maximum of 256 characters. This field is valid for both VP_COMMAND_GET and VP_COMMAND_SET.
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		string  bOEMCopyProtection; 
		};

/*
	/// <summary>
	/// 
	/// </summary>
	public enum DisplaySettingsMode : int {
	/// <summary>
	/// 
	/// </summary>
	ENUM_CURRENT_SETTINGS = -1, 
	/// <summary>
	/// 
	/// </summary>
	ENUM_REGISTRY_SETTINGS  = -2};*/
	
	/// <summary>
	/// Flag when passed to windows functions means "What the video card can produce not what the monitor can handle".
	/// </summary>
	public const int EDS_RAWMODE = 2;
	#endregion
	
	/// <summary>
	/// 
	/// </summary>
	public struct graphicsMode
		{
		/// <summary>
		/// Size of the matrix of pixels
		/// </summary>
		public Size dimensions;
		
		/// <summary>
		/// Scan rate
		/// </summary>
		public int refreshRate;
		
		/// <summary>
		/// Pixel depth
		/// </summary>
		public int bitsPerPixel;
		
		#region string based accessors
		/// <summary>
		/// Size of the matrix of pixels (as a string)
		/// </summary>
		public string dimensionString
			{
			get
				{
				return string.Format("{0}x{1}", dimensions.Width, dimensions.Height);
				}
			set
				{
				int i;
				int p;
				int pos=0;
				int w=0,h=0;
				string[] tokens = value.Trim().Split("xby ,XBY:;.-\t".ToCharArray());
				
				for(i=0;i<tokens.Length;i++)
					{
					try
						{
						p = int.Parse(tokens[i]);
						
						if(pos == 0)
							w = p;
						else if(pos == 1)
							h = p;
						pos++;
						}
					catch
						{
						}
					
					if(pos == 2)
						{
						this.dimensions = new Size(w, h);
						return;
						}
					}
				
				}
			}
		
		
		/// <summary>
		/// Scan rate (as a string)
		/// </summary>
		public string refreshRateString
			{
			get
				{
				return string.Format("{0}Hz", refreshRate);
				}
			set
				{
				try
					{
					string num = value.Trim(@" -.!@#$%^&*()_+=`~qwertyuiop[]asdfghjkl;'zxcvbnm,./QWERTYUIOP{}|\ASDFGHJKL:ZXCVBNM<>?".ToCharArray()).Split(" ".ToCharArray())[0];
					refreshRate = int.Parse(num);
					}
				catch
					{
					}
				}
			}
		
		
		/// <summary>
		/// Pixel depth (as a string)
		/// </summary>
		public string bitsPerPixelString
			{
			get
				{
				return string.Format("{0} Bpp", bitsPerPixel);
				}
			set
				{
				try
					{
					string num = value.Trim(@" -.!@#$%^&*()_+=`~qwertyuiop[]asdfghjkl;'zxcvbnm,./QWERTYUIOP{}|\ASDFGHJKL:ZXCVBNM<>?".ToCharArray()).Split(" ".ToCharArray())[0];
					bitsPerPixel = int.Parse(num);
					}
				catch
					{
					}
				}
			}
		
		#endregion
		
		/// <summary>
		/// Mode as a string
		/// </summary>
		/// <returns></returns>
		public string toString()
			{
			return string.Format("{0} by {1}, {2} bits per pixel, at {3}Hz", dimensions.Width, dimensions.Height, bitsPerPixel, refreshRate);
			}
		
		/// <summary>
		/// Create a Mode from a string
		/// </summary>
		/// <param name="s">String representing mode</param>
		/// <returns>new graphics mode in a graphicsMode structure</returns>
		public static graphicsMode fromString(string s)
			{
			graphicsMode g = new graphicsMode();
			try
				{
				if(s.IndexOfAny(@"\/".ToCharArray()) != -1)
					{
					//tree based mode specifier
					string[] tokens = s.Split(@"\/".ToCharArray());
					g.dimensionString = tokens[0];
					g.bitsPerPixelString = tokens[1];
					g.refreshRateString = tokens[2];
					return g;
					}
				
				throw new Exception("TODO: better error handelling");
				}
			catch
				{
				//return null;
				throw new Exception("TODO: better error handelling");
				}
			}
		
		
		
		/// <summary>
		/// Create a Mode from a set of paramaters
		/// </summary>
		/// <param name="width">x resolution of display mode</param>
		/// <param name="height">y resolution of display mode</param>
		/// <param name="bitsPerPixel">pixel depth</param>
		/// <param name="refreshRate">scan rate</param>
		/// <returns>new graphics mode in a graphicsMode structure</returns>
		public static graphicsMode fromSpecs(int width, int height, int bitsPerPixel, int refreshRate)
			{
			return graphicsMode.fromSpecs(new Size(width, height), bitsPerPixel, refreshRate);
			}
		
		/// <summary>
		/// Create a Mode from a set of paramaters
		/// </summary>
		///<param name="dimensions">Size of the matrix of pixels.</param>
		/// <param name="bitsPerPixel">pixel depth</param>
		/// <param name="refreshRate">scan rate</param>
		/// <returns>new graphics mode in a graphicsMode structure</returns>
		public static graphicsMode fromSpecs(Size dimensions, int bitsPerPixel, int refreshRate)
			{
			graphicsMode g = new graphicsMode();
			g.bitsPerPixel = bitsPerPixel;
			g.refreshRate = refreshRate;
			g.dimensions = dimensions;
			return g;
			}
		
		/// <summary>
		/// Create a Mode from a windows DEVMODE structure
		/// </summary>
		/// <param name="d">windows DEVMODE structure</param>
		/// <returns>new graphics mode in a graphicsMode structure</returns>
		public static graphicsMode fromDevMode(DEVMODE d)
			{
			graphicsMode g = new graphicsMode();
			g.bitsPerPixel = d.dmBitsPerPel;
			g.dimensions = new Size(d.dmPelsWidth, d.dmPelsHeight);
			g.refreshRate =  d.dmDisplayFrequency;
			return g;
			}
		
		/// <summary>
		/// Compares to graphics modes
		/// </summary>
		/// <returns>-1 if other is less than, 1 if greater than, 0 if equal </returns>
		public int compare(graphicsMode other)
			{
			int c;
			c = compare(refreshRate, other.refreshRate);
			if(c!=0)
				return c;
			c = compare(dimensions.Height, other.dimensions.Height);
			if(c!=0)
				return c;
			c = compare(dimensions.Width, other.dimensions.Width);
			if(c!=0)
				return c;
			c = compare(bitsPerPixel, other.bitsPerPixel);
			return c;
			}
		
		/// <summary>
		/// -1 if b less than a, 1 if b greater than a, 0 if equal
		/// </summary>
		private int compare(int a, int b)
			{
			if(a<b)
				return 1;
			else if(a>b)
				return -1;
			else
				return 0;
			}
		
		}
	
	
	/// <summary>
	/// This is a externaly safe version of the displayChangeType enumeration (dangerous elements left out)
	/// </summary>
	public enum displayChangeType
		{
		/// <summary>
		/// Games type app
		/// </summary>
		noStartMenuOrToolBars = ChangeDisplaySettingsDwFalgs.CDS_FULLSCREEN,
		/// <summary>
		/// Generic app
		/// </summary>
		temporyDesktopChange = ChangeDisplaySettingsDwFalgs.changeDyamically,
		/// <summary>
		/// Screen utility app
		/// </summary>
		perminantDesktopChange = ChangeDisplaySettingsDwFalgs.CDS_UPDATEREGISTRY,
		/// <summary>
		/// Test mode is possible
		/// </summary>
		test = ChangeDisplaySettingsDwFalgs.CDS_TEST
		}
	
	#region properties
	/// <summary>
	/// A Rectangle, representing the bounds of the display.
	/// </summary>
	public readonly Rectangle Bounds;
	/// <summary>
	/// The device name associated with a display.
	/// </summary>
	public readonly string DeviceName;
	/// <summary>
	/// Gets a value indicating whether a particular display is the primary device
	/// </summary>
	public readonly bool Primary;
	/// <summary>
	/// Gets the working area of the display. The working area is the desktop area of the display, excluding taskbars, docked windows, and docked tool bars.
	/// </summary>
	public readonly Rectangle WorkingArea;
	#endregion
	
	#region dll imports
	/// <summary>
	/// The EnumDisplaySettingsEx function retrieves information about one of the graphics modes for a display device. To retrieve information for all the graphics modes for a display device, make a series of calls to this function.
	/// </summary>
	/// <param name="deviceName">specifies the display device about whose graphics mode the function will obtain information.</param>
	/// <param name="modeNum">graphics mode index or DisplaySettingsMode</param>
	/// <param name="devMode">returns a devmode structure sets (dmBitsPerPel, dmPelsWidth, dmPelsHeight, dmDisplayFlags, dmDisplayFrequency)</param>
	/// <returns></returns>
	[DllImport("user32.dll")]
	public static extern int EnumDisplaySettings (string deviceName, int modeNum, ref DEVMODE devMode );
	
	/// <summary>
	/// The EnumDisplaySettingsEx function retrieves information about one of the graphics modes for a display device. To retrieve information for all the graphics modes for a display device, make a series of calls to this function.
	/// </summary>
	/// <param name="DeviceName">specifies the display device about whose graphics mode the function will obtain information.</param>
	/// <param name="iModeNum">graphics mode index or DisplaySettingsMode</param>
	/// <param name="DevMode">returns a devmode structure sets (dmBitsPerPel, dmPelsWidth, dmPelsHeight, dmDisplayFlags, dmDisplayFrequency)</param>
	/// <param name="dwFlags">if EDS_RAWMODE then return what the video card can produce not what the monitor can handle</param>
	/// <returns></returns>
	[DllImport("user32.dll")]
	public static extern bool EnumDisplaySettingsEx(
		string DeviceName,  // display device
		int iModeNum,          // graphics mode
		ref DEVMODE DevMode,      // graphics mode settings
		int dwFlags            // options
		);
		
	/// <summary>
	/// The EnumDisplayDevices function lets you obtain information about the display devices in a system.
	/// </summary>
	/// <param name="Device">the device name. If NULL, function returns information for the display adapter(s) on the machine, based on iDevNum.</param>
	/// <param name="iDevNum">Index value that specifies the display device of interest. The operating system identifies each display device with an index value. The index values are consecutive integers, starting at 0. If a system has three display devices, for example, they are specified by the index values 0, 1, and 2. </param>
	/// <param name="lpDisplayDevice">a DISPLAY_DEVICE structure that receives information about the display device specified by iDevNum. Before calling EnumDisplayDevices, you must initialize the cb member of DISPLAY_DEVICE to the size, in bytes, of DISPLAY_DEVICE. </param>
	/// <param name="dwFlags">This parameter is currently not used and should be set to zero.</param>
	/// <returns></returns>
	[DllImport("user32.dll")]
	public static extern bool EnumDisplayDevices(
		string Device,                // device name
		int iDevNum,                   // display device
		ref DISPLAYDEVICE lpDisplayDevice, // device information
		int dwFlags                    // reserved
		);
	
	
	/// <summary>
	/// Changes the settings of the default display device to the specified graphics mode.
	/// </summary>
	/// <param name="devMode">a DEVMODE structure that describes the new graphics mode. If lpDevMode is NULL, all the values currently in the registry will be used for the display setting. Passing NULL for the lpDevMode parameter and 0 for the dwFlags parameter is the easiest way to return to the default mode after a dynamic mode change.</param>
	/// <param name="flags">Indicates how the graphics mode should be changed. This parameter can be one of the following values.</param>
	/// <returns></returns>
	[DllImport("user32.dll")]
	public static extern ChangeDisplaySettingsReturnValue ChangeDisplaySettings(ref DEVMODE devMode, ChangeDisplaySettingsDwFalgs flags);
	
	/// <summary>
	/// Changes the settings of the default display device to the specified graphics mode.
	/// </summary>
	/// <param name="devMode">a DEVMODE structure that describes the new graphics mode. If lpDevMode is NULL, all the values currently in the registry will be used for the display setting. Passing NULL for the lpDevMode parameter and 0 for the dwFlags parameter is the easiest way to return to the default mode after a dynamic mode change.</param>
	/// <param name="flags">Indicates how the graphics mode should be changed. This parameter can be one of the following values.</param>
	/// <returns></returns>
	[DllImport("user32.dll")]
	public static extern ChangeDisplaySettingsReturnValue ChangeDisplaySettings(IntPtr DevMode, ChangeDisplaySettingsDwFalgs flags);
	
	/// <summary>
	/// changes the settings of the specified display device to the specified graphics mode.
	/// </summary>
	/// <param name="deviceName">string that specifies the display device whose graphics mode will change.</param>
	/// <param name="DevMode">DEVMODE structure that describes the new graphics mode. If lpDevMode is NULL, all the values currently in the registry will be used for the display setting. Passing NULL for the lpDevMode parameter and 0 for the dwFlags parameter is the easiest way to return to the default mode after a dynamic mode change.
	/// The dmSize member must be initialized to the size, in bytes, of the DEVMODE structure.
	/// </param>
	/// <param name="hwnd">must be NULL.</param>
	/// <param name="dwflags">Indicates how the graphics mode should be changed.</param>
	/// <param name="lParam">If dwFlags is CDS_VIDEOPARAMETERS, lParam is a pointer to a VIDEOPARAMETERS structure. Otherwise lParam must be NULL.</param>
	/// <returns></returns>
	[DllImport("user32.dll")]	
	public static extern ChangeDisplaySettingsReturnValue ChangeDisplaySettingsEx(
		string deviceName,
		ref DEVMODE DevMode,  // graphics mode
		int hwnd,               // not used; must be NULL
		uint dwflags,            // graphics mode options
		ref VIDEOPARAMETERS lParam            // video parameters (or NULL)	
		//IntPtr pLparam
		);
	
	/// <summary>
	/// changes the settings of the specified display device to the specified graphics mode.
	/// </summary>
	/// <param name="deviceName">string that specifies the display device whose graphics mode will change.</param>
	/// <param name="DevMode">DEVMODE structure that describes the new graphics mode. If lpDevMode is NULL, all the values currently in the registry will be used for the display setting. Passing NULL for the lpDevMode parameter and 0 for the dwFlags parameter is the easiest way to return to the default mode after a dynamic mode change.
	/// The dmSize member must be initialized to the size, in bytes, of the DEVMODE structure.
	/// </param>
	/// <param name="hwnd">must be NULL.</param>
	/// <param name="dwflags">Indicates how the graphics mode should be changed.</param>
	/// <param name="lParam">If dwFlags is CDS_VIDEOPARAMETERS, lParam is a pointer to a VIDEOPARAMETERS structure. Otherwise lParam must be NULL.</param>
	/// <returns></returns>
	[DllImport("user32.dll")]	
	public static extern ChangeDisplaySettingsReturnValue ChangeDisplaySettingsEx(
		string deviceName,
		IntPtr DevMode,  // graphics mode
		int hwnd,               // not used; must be NULL
		uint dwflags,            // graphics mode options
		ref VIDEOPARAMETERS lParam            // video parameters (or NULL)	
		//IntPtr pLparam
		);
	
	
	
	#endregion
	
	
	
	/// <summary>
	/// Gets an array of all displays on the system (as display classes)
	/// </summary>
	public static Display[] AllDisplays
		{
		get
			{
			int i;
			Screen[] screens = Screen.AllScreens;
			Display[] displays = new Display[screens.Length];
			
			for(i=0;i<screens.Length;i++)
				{
				displays[i] = new Display(screens[i]);
				}
			
			return displays;
			}
		}
	
	/// <summary>
	/// Function to create a display from a screen
	/// </summary>
	/// <param name="s"></param>
	public Display(Screen s)
		{
		this.Bounds = s.Bounds;
		this.DeviceName = s.DeviceName;
		this.Primary = s.Primary;
		this.WorkingArea = s.WorkingArea;
		}
	
	
	/// <summary>
	/// All modes that will function on the attached display
	/// </summary>
	public graphicsMode[] availableModes
		{
		get
			{
			return enumerateModes(true);
			}
		}
	
	/// <summary>
	/// All modes that the adapter can produce (but the attached display might not support)
	/// </summary>
	public graphicsMode[] allModes
		{
		get
			{
			return enumerateModes(true);
			}
		}
	
	/// <summary>
	/// Get all available graphics modes
	/// </summary>
	/// <param name="displayCompatableOnly">Use all modes that the adapter can produce (but the attached display might not support)</param>
	/// <returns>An array of available graphics modes</returns>
	protected graphicsMode[] enumerateModes(bool displayCompatableOnly)
		{
		int i = 0;
		DEVMODE devmode = initNewDEVMODE();
		ArrayList modes = new ArrayList();
		
		while(EnumDisplaySettingsEx(this.DeviceName,i ,ref devmode, displayCompatableOnly ? 0 : EDS_RAWMODE))
			{
			i++;
			modes.Add(graphicsMode.fromDevMode(devmode));
			}
		
		graphicsMode[] finalModes = new graphicsMode[modes.Count];
		for(i=0;i<finalModes.Length;i++)
			{
			finalModes[i] = (graphicsMode)modes[i];
			}
		
		return finalModes;
		}
	
	private DEVMODE initNewDEVMODE()
		{
		DEVMODE devmode = new DEVMODE();
		devmode.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
		return devmode;
		}
	
	/// <summary>
	/// Gets mode information for a mode index
	/// </summary>
	/// <param name="index">index of mode</param>
	/// <param name="displayCompatableOnly">Use all modes that the adapter can produce (but the attached display might not support)</param>
	/// <returns>Mode information in a graphicsMode structure</returns>
	public graphicsMode getMode(int index, bool displayCompatableOnly)
		{
		DEVMODE devmode = initNewDEVMODE();
		EnumDisplaySettingsEx(this.DeviceName, index, ref devmode, displayCompatableOnly ? 0 : EDS_RAWMODE);
		return graphicsMode.fromDevMode(devmode);
		}
	
	/// <summary>
	/// Gets the index of the specified resolution if available
	/// </summary>
	/// <param name="g">The graphics mode</param>
	/// <param name="displayCompatableOnly">Include modes the video card can produce, but the monitor can't use.</param>
	/// <returns>Returns -1 if mode is not supported</returns>
	public int getIndexOfMode(graphicsMode g, bool displayCompatableOnly)
		{
		int i;
		graphicsMode[] modes = enumerateModes(displayCompatableOnly);
		for(i=0;i<modes.Length;i++)
			{
			if(g.compare(modes[i]) == 0)
				return i;
			}
		return -1;
		}
	
	/// <summary>
	/// Changes the screen mode of the specified device
	/// </summary>
	/// <param name="g">The graphics mode</param>
	/// <param name="displayCompatableOnly">Include modes the video card can produce, but the monitor can't use.</param>
	/// <param name="changeType">Nature of change to perform</param>
	/// <returns>ChangeDisplaySettingsReturnValue as per winsdk</returns>
	public ChangeDisplaySettingsReturnValue changeResolution(graphicsMode g, bool displayCompatableOnly, displayChangeType changeType)
		{
		int index = getIndexOfMode(g, displayCompatableOnly);
		if(index != -1)
			return changeResolution(index, displayCompatableOnly, changeType);
		else
			return ChangeDisplaySettingsReturnValue.DISP_CHANGE_BADMODE;
		}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="index">The graphics mode index</param>
	/// <param name="displayCompatableOnly">Include modes the video card can produce, but the monitor can't use.</param>
	/// <param name="changeType">Nature of change to perform</param>
	/// <returns></returns>
	public ChangeDisplaySettingsReturnValue changeResolution(int index, bool displayCompatableOnly, displayChangeType changeType)
		{
		DEVMODE devmode = initNewDEVMODE();
		EnumDisplaySettingsEx(this.DeviceName, index, ref devmode, displayCompatableOnly ? 0 : EDS_RAWMODE);
		//return ChangeDisplaySettingsEx(DeviceName, devmode, 0/*null*/, (int)changeType, IntPtr.Zero);
		VIDEOPARAMETERS vp = new VIDEOPARAMETERS();
		return ChangeDisplaySettingsEx(DeviceName, ref devmode, 0/*null*/, (uint)changeType, ref vp);
		}
	
	private TreeNodeCollection add(string desc, TreeNodeCollection start)
		{
		foreach(TreeNode n in  start)
			{
			if(n.Text == desc)
				return n.Nodes;
			}
		return start.Add(desc).Nodes;
		}
	
	/// <summary>
	/// Updates a tree view controll with acceptable display resolutions
	/// </summary>
	/// <param name="displayCompatableOnly">Include modes the video card can produce, but the monitor can't use.</param>
	/// <param name="nodes">tree view controll root node</param>
	public void fillUserTreeWithResolutions(bool displayCompatableOnly, ref TreeNodeCollection nodes)
		{
		//TreeNodeCollection nodes = new TreeNodeCollection();
		TreeNodeCollection currentNodes = nodes;
		
		int i;
		graphicsMode[] modes = enumerateModes(displayCompatableOnly);
		
		for(i=0;i<modes.Length;i++)
			{
			currentNodes = nodes;
			currentNodes = add(modes[i].dimensionString, currentNodes);
			currentNodes = add(modes[i].bitsPerPixelString, currentNodes);
			currentNodes = add(modes[i].refreshRateString, currentNodes);
			}
		}
		
	/// <summary>
	/// Nearest Mode of less than or equal specs
	/// </summary>
	/// <param name="width">width of display mode</param>
	/// <param name="height">height of display mode</param>
	/// <param name="useNearest">find nearest is exact nt possible</param>
	/// <param name="newSize">The new display size</param>
	/// <returns>True if sucsessful, otherwise false.</returns>
	public bool changeToMode(int width, int height, bool useNearest, out Size newSize)
		{
		graphicsMode[] modes = availableModes;
		newSize = this.Bounds.Size;
		
		if(modes.Length <=0)
			return false;
		
		//are we already in a similar mode
		if((this.Bounds.Size.Width == width) && (this.Bounds.Size.Height == height))
			return true;
		
		//lets not exceed 75Hz
		graphicsMode g = graphicsMode.fromSpecs(width, height, 32, 75);
		
		//changeResolution
		int nearest = -1;
		int i;
		
		for(i=0;i<modes.Length;i++)
			{
			//-1 if other is less than, 1 if greater than, 0 if equal
			if(g.compare(modes[i]) <= 0)
				{
				if(nearest == -1)
					{
					nearest = i;
					}
				else
					{
					if(modes[nearest].compare(modes[i]) > 0)
						{
						nearest = i;
						}
					}
				}
			}//end for
		
		if(nearest == -1)
			return false;
		
		if(!useNearest)
			{
			if(modes[nearest].dimensions.Width != width)
				return false;
			
			if(modes[nearest].dimensions.Height != height)
				return false;
			}
		
		ChangeDisplaySettingsReturnValue r = changeResolution(nearest, true, displayChangeType.noStartMenuOrToolBars);
		if(r == ChangeDisplaySettingsReturnValue.DISP_CHANGE_SUCCESSFUL)
			{
			newSize = modes[nearest].dimensions;
			return true;
			}
		else
			{
			return false;
			}
		}
	
	/// <summary>
	/// If the display mode change was tempory then return to the default mode.
	/// </summary>
	public void recoverFromDynamicModeChange()
		{
		VIDEOPARAMETERS vp = new VIDEOPARAMETERS();
		ChangeDisplaySettingsEx(this.DeviceName, IntPtr.Zero, 0, 0, ref vp);
		}
	
	}
}
