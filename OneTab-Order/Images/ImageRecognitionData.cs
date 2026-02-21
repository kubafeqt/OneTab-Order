using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneTab_Order
{
   public record ImageRecognitionData
   {
      public int RecognitionId { get; set; }

      public Point ScreenStart;
      public Point ScreenEnd;

      public Point SearchStartScreen { get; set; } // SEARCH start – ve SCREEN souřadnicích
      

   }
}
