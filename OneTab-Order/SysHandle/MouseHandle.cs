using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OneTab_Order
{
   class MouseHandle
   {
      // Importování funkcí z user32.dll
      [DllImport("user32.dll")]
      static extern bool SetCursorPos(int X, int Y);

      [DllImport("user32.dll")]
      public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

      // Konstanty pro akce myši
      private const int MOUSEEVENTF_LEFTDOWN = 0x02;
      private const int MOUSEEVENTF_LEFTUP = 0x04;

      /// <summary>
      /// Přesune kurzor na zadané souřadnice a klikne levým tlačítkem.
      /// </summary>
      /// <param name="x">X souřadnice na obrazovce</param>
      /// <param name="y">Y souřadnice na obrazovce</param>
      public static void LeftClickAtPoint(Point mousePoint)
      {
         int x = mousePoint.X;
         int y = mousePoint.Y;

         SetCursorPos(x, y);  // 1. Přesun kurzoru na danou pozici
         mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);  // 2. Simulace stisknutí tlačítka (Down)
         mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);  // 3. Simulace uvolnění tlačítka (Up)

      }

   }
}
