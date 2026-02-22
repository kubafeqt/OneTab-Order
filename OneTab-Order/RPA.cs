using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OneTab_Order
{
   class RPA
   {
      public static bool ActionInProgress { get; set; } = false;

      /// <summary>
      /// checkovat image jestli je na obrazovce
      /// pokud ano tak kliknout na to místo - ovládat myš
      /// </summary>
      /// <param name="imgs"></param>
      public static void DeleteAllTabsInBrowser(List<Images> imgs)
      {
         ActionInProgress = true;
         while (true) 
         {
            if (Keyboard.KeyPressed) //when any keyboard key is pressed - stop doing RPA actions
            {
               Keyboard.KeyPressed = false;
               break;
            }

            Point? foundPoint = null;
            Images? foundImage = null;

            // Najdi první odpovídající sample
            foreach (var img in imgs)
            {
               var point = img.SearchSampleFast();
               if (point != null)
               {
                  foundPoint = point;
                  foundImage = img;
                  break; // důležité!
               }
            }

            // Nic nenalezeno → konec
            if (foundPoint == null || foundImage == null)
            {
               ActionInProgress = false;
               MessageBox.Show("No further matches detected!");
               break;
            }

            // Kliknout
            Point clickPoint = new Point(
                foundPoint.Value.X + foundImage.ScreenStart.X,
                foundPoint.Value.Y + foundImage.ScreenStart.Y);

            MouseHandle.LeftClickAtPoint(clickPoint);
            Thread.Sleep(120);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(80);
         }
      }

   }
}