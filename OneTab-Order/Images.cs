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

         string dir = Path.Combine(Application.StartupPath, "Samples");
         string samplePath = Path.Combine(dir, $"Sample_{sampleHash}.png");
         Sample = (Bitmap)Image.FromFile(samplePath);
      }

      public Point? SearchSample() //test jestli to bude fungovat tak
      {
         using (Bitmap screen = ExactScreenshot(ScreenStart, ScreenEnd))
         {
            // Ensure the sample fits within the screen
            int maxX = screen.Width - Sample.Width;
            int maxY = screen.Height - Sample.Height;

            string dir = Path.Combine(Application.StartupPath, "Samples");
            string screenPath = Path.Combine(dir, $"screen.png");
            screen.Save(screenPath);

            for (int x = 0; x < maxX; x++)
            {
               for (int y = 0; y < maxY; y++)
               {
                  // Compare the first pixel and then check the whole sample
                  if (Sample.GetPixel(0, 0) == screen.GetPixel(x, y) &&
                      IsInnerImage(x, y, Sample, screen))
                  {
                     return new Point(x, y);
                  }
               }
            }

     
            ////screen.Save(Path.Combine(basePath, "test.png"));

            ////if (maxX < 0 || maxY < 0)
            ////   return false; // Sample is larger than the screenshot area

            ////++ošetření aby startSearch byl v rámci maxX a maxY
            //if (StartSearch.X > maxX)
            //   StartSearch = new Point(0, StartSearch.Y);
            //if (StartSearch.Y > maxY)
            //   StartSearch = new Point(StartSearch.X, 0);
            //for (int y = StartSearch.Y; y <= StartSearch.Y - 1; y++)
            //{
            //   for (int x = StartSearch.X; x <= StartSearch.X - 1; x++)
            //   {
            //      if (x > maxX)
            //      {
            //         x = 0;
            //      }
            //      // Compare the first pixel and then check the whole sample
            //      if (Sample.GetPixel(0, 0) == screen.GetPixel(x, y) &&
            //          IsInnerImage(x, y, Sample, screen))
            //      {
            //         return new Point(x, y);
            //      }
            //   }
            //   if (y > maxY)
            //   {
            //      y = 0;
            //   }
            //}
            return null;
         }
      }

      /// <summary>
      /// Check if sample is inner image of screen, when first pixel is matched
      /// </summary>
      private bool IsInnerImage(int left, int top, Bitmap sample, Bitmap screen)
      {
         for (int y = 0; y < sample.Height; y++)
         {
            for (int x = 0; x < sample.Width; x++)
            {
               if (sample.GetPixel(x, y) != screen.GetPixel(left + x, top + y))
               {
                  return false; //sample is not inner image of screen
               }
            }
         }
         return true; //sample is inner image of screen
      }

      /// <summary>
      /// Get screenshot of exact area on screen.
      /// </summary>
      /// <param name="startScreen">start point of screen (left, up)</param>
      /// <param name="endScreen">end point of screen (right, down)</param>
      /// <returns>screenshot bitmap</returns>
      private Bitmap ExactScreenshot(Point startScreen, Point endScreen)
      {
         Size size = new Size(endScreen.X - startScreen.X, endScreen.Y - startScreen.Y);
         Bitmap screenshot = new Bitmap(size.Width, size.Height);
         Graphics gfx = Graphics.FromImage(screenshot);
         gfx.CopyFromScreen(startScreen.X, startScreen.Y, 0, 0, size);
         return screenshot;
      }

   }
}