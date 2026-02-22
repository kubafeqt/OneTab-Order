using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OneTab_Order
{
   class Keyboard
   {
      public static bool KeyPressed = false;

      private const int WH_KEYBOARD_LL = 13;
      private const int WM_KEYDOWN = 0x0100;
      private const int WM_KEYUP = 0x0101;

      public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

      [DllImport("user32.dll")]
      private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

      [DllImport("user32.dll")]
      public static extern bool UnhookWindowsHookEx(IntPtr hhk);

      [DllImport("user32.dll")]
      private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

      [DllImport("kernel32.dll")]
      private static extern IntPtr GetModuleHandle(string lpModuleName);

      public static LowLevelKeyboardProc _proc = HookCallback;
      public static IntPtr _hookID = IntPtr.Zero;

      [StructLayout(LayoutKind.Sequential)]
      private struct KBDLLHOOKSTRUCT
      {
         public uint vkCode;
         public uint scanCode;
         public uint flags;
         public uint time;
         public IntPtr dwExtraInfo;
      }

      private const int LLKHF_INJECTED = 0x10;

      public static IntPtr SetHook(LowLevelKeyboardProc proc)
      {
         using (Process curProcess = Process.GetCurrentProcess())
         using (ProcessModule curModule = curProcess.MainModule)
         {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                GetModuleHandle(curModule.ModuleName), 0);
         }
      }

      /// <summary>
      /// zde zpracování klávesy
      /// </summary>
      private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
      {
         if (nCode >= 0 && RPA.ActionInProgress)
         {
            var kb = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);

            // ❌ ignoruj klávesy vytvořené SendKeys / SendInput
            if ((kb.flags & LLKHF_INJECTED) != 0)
               return CallNextHookEx(_hookID, nCode, wParam, lParam);

            if ((int)wParam == WM_KEYDOWN)
            {
               KeyPressed = true;
            }
         }

         return CallNextHookEx(_hookID, nCode, wParam, lParam);
      }
   }
}
