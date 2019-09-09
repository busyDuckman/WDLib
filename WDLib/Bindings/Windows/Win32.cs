/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Runtime;
using System.Runtime.InteropServices;

namespace WD_toolbox.Bindings.Windows
{
/// <summary>
/// Windows (32bit version) functions and constants.
/// </summary>
public abstract class Win32
	{
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetProcessDPIAware();

    //[DllImport("user32.dll", SetLastError = true)]
    //public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);


	#region messages
	/// <summary>
	/// Windows event messages.
	/// Enumeration value coresponds to the numerical value of the meesage.
	/// </summary>
	public enum windowsMessage : uint 
		{
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NULL = 0x0000, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CREATE = 0x0001, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_DESTROY = 0x0002, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MOVE = 0x0003, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SIZE = 0x0005, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_ACTIVATE = 0x0006, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SETFOCUS = 0x0007, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_KILLFOCUS = 0x0008, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_ENABLE = 0x000A, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SETREDRAW = 0x000B, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SETTEXT = 0x000C, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_GETTEXT = 0x000D, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_GETTEXTLENGTH = 0x000E, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_PAINT = 0x000F, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CLOSE = 0x0010,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_QUIT = 0x0012, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_ERASEBKGND = 0x0014, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SYSCOLORCHANGE = 0x0015, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SHOWWINDOW = 0x0018, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_WININICHANGE = 0x001A, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SETTINGCHANGE = 0x001A, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_DEVMODECHANGE = 0x001B, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_ACTIVATEAPP = 0x001C, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_FONTCHANGE = 0x001D, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_TIMECHANGE = 0x001E, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CANCELMODE = 0x001F, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SETCURSOR = 0x0020, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MOUSEACTIVATE = 0x0021,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CHILDACTIVATE = 0x0022, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_QUEUESYNC = 0x0023, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_GETMINMAXINFO = 0x0024, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_PAINTICON = 0x0026, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_ICONERASEBKGND = 0x0027, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NEXTDLGCTL = 0x0028, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SPOOLERSTATUS = 0x002A, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_DRAWITEM = 0x002B, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MEASUREITEM = 0x002C, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_DELETEITEM = 0x002D, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_VKEYTOITEM = 0x002E, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CHARTOITEM = 0x002F, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SETFONT = 0x0030,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_GETFONT = 0x0031, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SETHOTKEY = 0x0032, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_GETHOTKEY = 0x0033, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_QUERYDRAGICON = 0x0037, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_COMPAREITEM = 0x0039, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_COMPACTING = 0x0041, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_COMMNOTIFY = 0x0044, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_WINDOWPOSCHANGING = 0x0046, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_WINDOWPOSCHANGED = 0x0047, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_POWER = 0x0048, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_COPYDATA = 0x004A, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CANCELJOURNAL = 0x004B, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NOTIFY = 0x004E,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_INPUTLANGCHANGEREQUEST = 0x0050, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_INPUTLANGCHANGE = 0x0051, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_TCARD = 0x0052, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_HELP = 0x0053, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_USERCHANGED = 0x0054, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NOTIFYFORMAT = 0x0055, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CONTEXTMENU = 0x007B, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_STYLECHANGING = 0x007C, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_STYLECHANGED = 0x007D, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_DISPLAYCHANGE = 0x007E, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_GETICON = 0x007F, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SETICON = 0x0080, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCCREATE = 0x0081,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCDESTROY = 0x0082, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCCALCSIZE = 0x0083, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCHITTEST = 0x0084, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCPAINT = 0x0085, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCACTIVATE = 0x0086, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_GETDLGCODE = 0x0087, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCMOUSEMOVE = 0x00A0, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCLBUTTONDOWN = 0x00A1, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCLBUTTONUP = 0x00A2, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCLBUTTONDBLCLK = 0x00A3, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCRBUTTONDOWN = 0x00A4, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCRBUTTONUP = 0x00A5, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCRBUTTONDBLCLK = 0x00A6,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCMBUTTONDOWN = 0x00A7, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCMBUTTONUP = 0x00A8, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCMBUTTONDBLCLK = 0x00A9, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCXBUTTONDOWN = 0x00AB, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCXBUTTONUP = 0x00AC, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCXBUTTONDBLCLK = 0x00AD, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_INPUT = 0x00FF, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_KEYFIRST = 0x0100, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_KEYDOWN = 0x0100, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_KEYUP = 0x0101, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CHAR = 0x0102, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_DEADCHAR = 0x0103, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SYSKEYDOWN = 0x0104, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SYSKEYUP = 0x0105,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SYSCHAR = 0x0106, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SYSDEADCHAR = 0x0107, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_UNICHAR = 0x0109, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_KEYLAST = 0x0109, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		UNICODE_NOCHAR = 0xFFFF, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_KEYLAST_olderversions = 0x0108, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_IME_STARTCOMPOSITION = 0x010D, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_IME_ENDCOMPOSITION = 0x010E, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_IME_COMPOSITION = 0x010F, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_IME_KEYLAST = 0x010F, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_INITDIALOG = 0x0110, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_COMMAND = 0x0111,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SYSCOMMAND = 0x0112, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_TIMER = 0x0113, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_HSCROLL = 0x0114, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_VSCROLL = 0x0115, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_INITMENU = 0x0116, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_INITMENUPOPUP = 0x0117, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MENUSELECT = 0x011F, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MENUCHAR = 0x0120, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_ENTERIDLE = 0x0121, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MENURBUTTONUP = 0x0122, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MENUDRAG = 0x0123, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MENUGETOBJECT = 0x0124, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_UNINITMENUPOPUP = 0x0125,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MENUCOMMAND = 0x0126, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CHANGEUISTATE = 0x0127, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_UPDATEUISTATE = 0x0128, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_QUERYUISTATE = 0x0129, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CTLCOLORMSGBOX = 0x0132, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CTLCOLOREDIT = 0x0133, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CTLCOLORLISTBOX = 0x0134, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CTLCOLORBTN = 0x0135, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CTLCOLORDLG = 0x0136, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CTLCOLORSCROLLBAR = 0x0137, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CTLCOLORSTATIC = 0x0138, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		MN_GETHMENU = 0x01E1,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MOUSEFIRST = 0x0200, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MOUSEMOVE = 0x0200, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_LBUTTONDOWN = 0x0201, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_LBUTTONUP = 0x0202, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_LBUTTONDBLCLK = 0x0203, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_RBUTTONDOWN = 0x0204, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_RBUTTONUP = 0x0205, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_RBUTTONDBLCLK = 0x0206, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MBUTTONDOWN = 0x0207, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MBUTTONUP = 0x0208, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MBUTTONDBLCLK = 0x0209, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MOUSEWHEEL = 0x020A, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_XBUTTONDOWN = 0x020B,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_XBUTTONUP = 0x020C, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_XBUTTONDBLCLK = 0x020D, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MOUSELAST = 0x020D, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MOUSELAST_oldversion = 0x020A, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MOUSELAST_evenolderversion = 0x0209, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_PARENTNOTIFY = 0x0210, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_ENTERMENULOOP = 0x0211, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_EXITMENULOOP = 0x0212, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NEXTMENU = 0x0213, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SIZING = 0x0214, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CAPTURECHANGED = 0x0215, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MOVING = 0x0216, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_POWERBROADCAST = 0x0218,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_DEVICECHANGE = 0x0219, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MDICREATE = 0x0220, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MDIDESTROY = 0x0221, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MDIACTIVATE = 0x0222, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MDIRESTORE = 0x0223, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MDINEXT = 0x0224, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MDIMAXIMIZE = 0x0225, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MDITILE = 0x0226, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MDICASCADE = 0x0227, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MDIICONARRANGE = 0x0228, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MDIGETACTIVE = 0x0229, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MDISETMENU = 0x0230, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_ENTERSIZEMOVE = 0x0231,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_EXITSIZEMOVE = 0x0232, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_DROPFILES = 0x0233, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MDIREFRESHMENU = 0x0234, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_IME_SETCONTEXT = 0x0281, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_IME_NOTIFY = 0x0282, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_IME_CONTROL = 0x0283, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_IME_COMPOSITIONFULL = 0x0284, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_IME_SELECT = 0x0285, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_IME_CHAR = 0x0286, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_IME_REQUEST = 0x0288, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_IME_KEYDOWN = 0x0290, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_IME_KEYUP = 0x0291, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MOUSEHOVER = 0x02A1,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_MOUSELEAVE = 0x02A3, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCMOUSEHOVER = 0x02A0, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_NCMOUSELEAVE = 0x02A2, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_WTSSESSION_CHANGE = 0x02B1, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_TABLET_FIRST = 0x02c0, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_TABLET_LAST = 0x02df, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CUT = 0x0300, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_COPY = 0x0301, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_PASTE = 0x0302, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CLEAR = 0x0303, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_UNDO = 0x0304, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_RENDERFORMAT = 0x0305, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_RENDERALLFORMATS = 0x0306, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_DESTROYCLIPBOARD = 0x0307,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_DRAWCLIPBOARD = 0x0308, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_PAINTCLIPBOARD = 0x0309, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_VSCROLLCLIPBOARD = 0x030A, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SIZECLIPBOARD = 0x030B, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_ASKCBFORMATNAME = 0x030C, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_CHANGECBCHAIN = 0x030D, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_HSCROLLCLIPBOARD = 0x030E, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_QUERYNEWPALETTE = 0x030F, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_PALETTEISCHANGING = 0x0310, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_PALETTECHANGED = 0x0311, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_HOTKEY = 0x0312, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_PRINT = 0x0317,
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_PRINTCLIENT = 0x0318, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_APPCOMMAND = 0x0319, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_THEMECHANGED = 0x031A, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_HANDHELDFIRST = 0x0358, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_HANDHELDLAST = 0x035F, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_AFXFIRST = 0x0360, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_AFXLAST = 0x037F, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_PENWINFIRST = 0x0380, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_PENWINLAST = 0x038F, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_APP = 0x8000, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_QUERYENDSESSION = 0x0011, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_QUERYOPEN = 0x0013, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_ENDSESSION = 0x0016, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_GETOBJECT = 0x003D, 
		/// <summary>A predefined windows message. <br/>Value was obtained from "winuser.h"./// </summary>
		WM_SYNCPAINT = 0x0088
		};
	#endregion
	
	#region dll imports
	/// <summary>
	/// Sends a windows message
	/// </summary>
	/// <param name="hWnd">handle to destination window</param>
	/// <param name="Msg">message</param>
	/// <param name="wParam">first message parameter</param>
	/// <param name="lParam">second message parameter</param>
	/// <returns></returns>
	[DllImport("user32.dll")]
	public static extern int SendMessage(
		int hWnd,      // handle to destination window
		uint Msg,       // message
		long wParam,  // first message parameter
		long lParam   // second message parameter
		);
		
	/// <summary>
	/// Sends a windows message
	/// </summary>
	/// <param name="hWnd">handle to destination window</param>
	/// <param name="Msg">message</param>
	/// <param name="wParam">first message parameter</param>
	/// <param name="lParam">second message parameter</param>
	public static int SendMessage(IntPtr hWnd, windowsMessage Msg, long wParam, long lParam)
		{
		return SendMessage(hWnd.ToInt32(), (uint)Msg, wParam, lParam);
		}
	
	/// <summary>
	/// Moves a window
	/// </summary>
	/// <param name="hWnd">handle to destination window</param>
	/// <param name="X">New x position of top left cornder of the window.</param>
	/// <param name="Y">New y position of top left cornder of the window.</param>
	/// <param name="nWidth">New width of the window.</param>
	/// <param name="nHeight">New height of the window.</param>
	/// <param name="Repaint">Set to true to force a repaint.</param>
	/// <returns>True if sucessful; otherwise false.</returns>
	[DllImport("user32.dll")]
	public static extern bool MoveWindow(IntPtr hWnd,
		int X,
		int Y,
		int nWidth,
		int nHeight,
		[MarshalAs(UnmanagedType.Bool)] bool Repaint);

    /// <summary>
    /// Copies memory
    /// </summary>
    /// <param name="dest">Dest (cant overlap with source)</param>
    /// <param name="src">Source (cant overlap with dest)</param>
    /// <param name="count">Number of bytes to copy</param>
    /// <returns>The value of dest.</returns>
    [DllImport("msvcrt.dll", SetLastError = false)]
    public static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);
	#endregion
    
    #region stuff to port
    /*
	WINUSERAPI
BOOL
WINAPI
ShowOwnedPopups(
    __in  HWND hWnd,
    __in  BOOL fShow);

WINUSERAPI
BOOL
WINAPI
OpenIcon(
    __in  HWND hWnd);

WINUSERAPI
BOOL
WINAPI
CloseWindow(
    __in  HWND hWnd);



WINUSERAPI
BOOL
WINAPI
SetWindowPos(
    __in HWND hWnd,
    __in_opt HWND hWndInsertAfter,
    __in int X,
    __in int Y,
    __in int cx,
    __in int cy,
    __in UINT uFlags);

WINUSERAPI
BOOL
WINAPI
GetWindowPlacement(
    __in HWND hWnd,
    __inout WINDOWPLACEMENT *lpwndpl);

WINUSERAPI
BOOL
WINAPI
SetWindowPlacement(
    __in HWND hWnd,
    __in CONST WINDOWPLACEMENT *lpwndpl);


#ifndef NODEFERWINDOWPOS

WINUSERAPI
HDWP
WINAPI
BeginDeferWindowPos(
    __in int nNumWindows);

WINUSERAPI
HDWP
WINAPI
DeferWindowPos(
    __in HDWP hWinPosInfo,
    __in HWND hWnd,
    __in_opt HWND hWndInsertAfter,
    __in int x,
    __in int y,
    __in int cx,
    __in int cy,
    __in UINT uFlags);

WINUSERAPI
BOOL
WINAPI
EndDeferWindowPos(
    __in HDWP hWinPosInfo);

#endif // !NODEFERWINDOWPOS 

WINUSERAPI
BOOL
WINAPI
IsWindowVisible(
    __in HWND hWnd);

WINUSERAPI
BOOL
WINAPI
IsIconic(
    __in HWND hWnd);

WINUSERAPI
BOOL
WINAPI
AnyPopup(
    VOID);

WINUSERAPI
BOOL
WINAPI
BringWindowToTop(
    __in HWND hWnd);

WINUSERAPI
BOOL
WINAPI
IsZoomed(
    __in HWND hWnd);


 // SetWindowPos Flags
 
#define SWP_NOSIZE          0x0001
#define SWP_NOMOVE          0x0002
#define SWP_NOZORDER        0x0004
#define SWP_NOREDRAW        0x0008
#define SWP_NOACTIVATE      0x0010
#define SWP_FRAMECHANGED    0x0020   //The frame changed: send WM_NCCALCSIZE
#define SWP_SHOWWINDOW      0x0040
#define SWP_HIDEWINDOW      0x0080
#define SWP_NOCOPYBITS      0x0100
#define SWP_NOOWNERZORDER   0x0200  // Don't do owner Z ordering
#define SWP_NOSENDCHANGING  0x0400  // Don't send WM_WINDOWPOSCHANGING

#define SWP_DRAWFRAME       SWP_FRAMECHANGED
#define SWP_NOREPOSITION    SWP_NOOWNERZORDER

#if(WINVER >= 0x0400)
#define SWP_DEFERERASE      0x2000
#define SWP_ASYNCWINDOWPOS  0x4000
#endif // WINVER >= 0x0400 


#define HWND_TOP        ((HWND)0)
#define HWND_BOTTOM     ((HWND)1)
#define HWND_TOPMOST    ((HWND)-1)
#define HWND_NOTOPMOST  ((HWND)-2)

#ifndef NOCTLMGR

/*
 * WARNING:
 * The following structures must NOT be DWORD padded because they are
 * followed by strings, etc that do not have to be DWORD aligned.
 */
 /*
#include <pshpack2.h>

/*
 * original NT 32 bit dialog template:
 */
 /*
typedef struct {
    DWORD style;
    DWORD dwExtendedStyle;
    WORD cdit;
    short x;
    short y;
    short cx;
    short cy;
} DLGTEMPLATE;
typedef DLGTEMPLATE *LPDLGTEMPLATEA;
typedef DLGTEMPLATE *LPDLGTEMPLATEW;
#ifdef UNICODE
typedef LPDLGTEMPLATEW LPDLGTEMPLATE;
#else
typedef LPDLGTEMPLATEA LPDLGTEMPLATE;
#endif // UNICODE
typedef CONST DLGTEMPLATE *LPCDLGTEMPLATEA;
typedef CONST DLGTEMPLATE *LPCDLGTEMPLATEW;
#ifdef UNICODE
typedef LPCDLGTEMPLATEW LPCDLGTEMPLATE;
#else
typedef LPCDLGTEMPLATEA LPCDLGTEMPLATE;
#endif // UNICODE

/*
 * 32 bit Dialog item template.
 */
 /*
typedef struct {
    DWORD style;
    DWORD dwExtendedStyle;
    short x;
    short y;
    short cx;
    short cy;
    WORD id;
} DLGITEMTEMPLATE;
typedef DLGITEMTEMPLATE *PDLGITEMTEMPLATEA;
typedef DLGITEMTEMPLATE *PDLGITEMTEMPLATEW;
#ifdef UNICODE
typedef PDLGITEMTEMPLATEW PDLGITEMTEMPLATE;
#else
typedef PDLGITEMTEMPLATEA PDLGITEMTEMPLATE;
#endif // UNICODE
typedef DLGITEMTEMPLATE *LPDLGITEMTEMPLATEA;
typedef DLGITEMTEMPLATE *LPDLGITEMTEMPLATEW;
#ifdef UNICODE
typedef LPDLGITEMTEMPLATEW LPDLGITEMTEMPLATE;
#else
typedef LPDLGITEMTEMPLATEA LPDLGITEMTEMPLATE;
#endif // UNICODE
*/
	#endregion
	}
}
