using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverloadClientTool
{
    static public class Program
    {
        [STAThread]
        static public void Main()
        {
            var kh = new KeyboardHook(true);
            kh.KeyDown += Kh_KeyDown;
            Application.Run();
        }

        private static void Kh_KeyDown(Keys key, bool Shift, bool Ctrl, bool Alt)
        {
            //Debug.WriteLine("The Key: " + key);
        }
    }

    // https://stackoverflow.com/questions/34281223/c-sharp-hook-global-keyboard-events-net-4-0

    public class KeyboardHook : IDisposable
    {
        bool Global = false;

        public delegate void ErrorEventHandler(Exception e);
        public delegate void LocalKeyEventHandler(Keys key, bool Shift, bool Ctrl, bool Alt);
        public event LocalKeyEventHandler KeyDown;
        public event LocalKeyEventHandler KeyUp;
        public event ErrorEventHandler OnError;

        public delegate int CallbackDelegate(int Code, IntPtr W, IntPtr L);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct KBDLLHookStruct
        {
            public Int32 vkCode;
            public Int32 scanCode;
            public Int32 flags;
            public Int32 time;
            public Int32 dwExtraInfo;
        }

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr SetWindowsHookEx(HookType idHook, CallbackDelegate lpfn, IntPtr hInstance, int threadId);


        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        public enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        private IntPtr HookID = IntPtr.Zero;
        CallbackDelegate TheHookCB = null;

        //Start hook
        public KeyboardHook(bool Global)
        {
            this.Global = Global;
            TheHookCB = new CallbackDelegate(KeybHookProc);
            if (Global)
            {
                IntPtr hInstance = LoadLibrary("User32");
                HookID = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, TheHookCB,
                    hInstance, //0 for local hook. or hwnd to user32 for global
                    0); //0 for global hook. eller thread for hooken
            }
            else
            {
                HookID = SetWindowsHookEx(HookType.WH_KEYBOARD, TheHookCB,
                    IntPtr.Zero, //0 for local hook. or hwnd to user32 for global
                    GetCurrentThreadId()); //0 for global hook. or thread for the hook
            }
        }

        public void test()
        {
            if (OnError != null) OnError(new Exception("test"));
        }
        bool IsFinalized = false;
        ~KeyboardHook()
        {
            if (!IsFinalized)
            {
                UnhookWindowsHookEx(HookID);
                IsFinalized = true;
            }
        }
        public void Dispose()
        {
            if (!IsFinalized)
            {
                UnhookWindowsHookEx(HookID);
                IsFinalized = true;
            }
        }
        [STAThread]
        //The listener that will trigger events
        private int KeybHookProc(int Code, IntPtr W, IntPtr L)
        {

            // KBDLLHookStruct LS = new KBDLLHookStruct();

            if (Code < 0)
            {
                return CallNextHookEx(HookID, Code, W, L);
            }
            try
            {
                if (!Global)
                {
                    if (Code == 3)
                    {
                        IntPtr ptr = IntPtr.Zero;

                        int keydownup = L.ToInt32() >> 30;
                        if (keydownup == 0)
                        {
                            if (KeyDown != null) KeyDown((Keys)W, GetShiftPressed(), GetCtrlPressed(), GetAltPressed());
                        }
                        if (keydownup == -1)
                        {
                            if (KeyUp != null) KeyUp((Keys)W, GetShiftPressed(), GetCtrlPressed(), GetAltPressed());
                        }
                        //System.Diagnostics.Debug.WriteLine("Down: " + (Keys)W);
                    }
                }
                else
                {
                    KeyEvents kEvent = (KeyEvents)W;

                    Int32 vkCode = Marshal.ReadInt32((IntPtr)L); //Leser vkCode som er de første 32 bits hvor L peker.

                    if (kEvent != KeyEvents.KeyDown && kEvent != KeyEvents.KeyUp && kEvent != KeyEvents.SKeyDown && kEvent != KeyEvents.SKeyUp)
                    {
                    }
                    if (kEvent == KeyEvents.KeyDown || kEvent == KeyEvents.SKeyDown)
                    {
                        if (KeyDown != null) KeyDown((Keys)vkCode, GetShiftPressed(), GetCtrlPressed(), GetAltPressed());
                    }
                    if (kEvent == KeyEvents.KeyUp || kEvent == KeyEvents.SKeyUp)
                    {
                        if (KeyUp != null) KeyUp((Keys)vkCode, GetShiftPressed(), GetCtrlPressed(), GetAltPressed());
                    }
                }
            }
            catch (Exception e)
            {
                if (OnError != null) OnError(e);
                //Ignore all errors...
            }

            return CallNextHookEx(HookID, Code, W, L);

        }

        public enum KeyEvents
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SKeyDown = 0x0104,
            SKeyUp = 0x0105
        }

        [DllImport("user32.dll")]
        static public extern short GetKeyState(System.Windows.Forms.Keys nVirtKey);

        public static bool GetCapslock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.CapsLock)) & true;
        }
        public static bool GetNumlock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.NumLock)) & true;
        }

        public static bool GetShiftPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.ShiftKey);
            if (state > 1 || state < -1) return true;
            return false;
        }

        public static bool GetCtrlPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.ControlKey);
            if (state > 1 || state < -1) return true;
            return false;
        }

        public static bool GetAltPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.Menu);
            if (state > 1 || state < -1) return true;
            return false;
        }

        public static bool ShiftPressed
        {
            get
            {
                int state = GetKeyState(System.Windows.Forms.Keys.ShiftKey);
                if (state > 1 || state < -1) return true;
                return false;
            }
        }

        public static bool CtrlPressed
        {
            get 
            { 
                int state = GetKeyState(System.Windows.Forms.Keys.ControlKey);
                if (state > 1 || state < -1) return true;
                return false;
            }
        }
        public static bool AltPressed
        {
            get
            {
                int state = GetKeyState(System.Windows.Forms.Keys.Menu);
                if (state > 1 || state < -1) return true;
                return false;
            }
        }
    }
}