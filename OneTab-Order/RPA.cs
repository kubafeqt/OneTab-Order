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
      /// <param name="img"></param>
      public static void DeleteAllTabsInBrowser(Images img)
      {
         Point? foundPoint = img.SearchSample();
         while (foundPoint != null)
         {
            //kliknout na to místo
            Point clickPoint = new Point(foundPoint.Value.X + img.ScreenStart.X, foundPoint.Value.Y + img.ScreenStart.Y);
            MouseHandle.LeftClickAtPoint(clickPoint);
            Thread.Sleep(50);
            SendKeys.SendWait("{ENTER}");
            Thread.Sleep(200);
            //znovu hledat
            foundPoint = img.SearchSample();
         }
      }

   }
}