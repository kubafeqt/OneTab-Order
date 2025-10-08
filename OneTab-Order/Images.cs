using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneTab_Order
{
   class Images
   {
      public Point ScreenStart { get; set; }
      public Point ScreenEnd { get; set; }
      public Point StartSearch { get; set; }
      public Bitmap Sample { get; set; }

      public Images(Point screenStart, Point screenEnd, Point startSearch, string sampleHash)
      {
         ScreenStart = screenStart;
         ScreenEnd = screenEnd;
         StartSearch = startSearch;

         string samplePath = Path.Combine(Application.StartupPath, $"Sample_{sampleHash}");
         Sample = (Bitmap)Image.FromFile(samplePath);
      }

      public Point SearchSample()
      {




         return new Point();
      }

   }
}