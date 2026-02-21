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
      /// <summary>
      /// checkovat image jestli je na obrazovce
      /// pokud ano tak kliknout na to místo - ovládat myš
      /// </summary>
      /// <param name="imgs"></param>
      public static void DeleteAllTabsInBrowser(List<Images> imgs)
      {
         while (true)
         {
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
               MessageBox.Show("No further matches detected!");
               break;
            }

            // Kliknout
            Point clickPoint = new Point(
                foundPoint.Value.X + foundImage.ScreenStart.X,
                foundPoint.Value.Y + foundImage.ScreenStart.Y);

            MouseHandle.LeftClickAtPoint(clickPoint);
            Thread.Sleep(80);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(60);
         }
      }

   }
}