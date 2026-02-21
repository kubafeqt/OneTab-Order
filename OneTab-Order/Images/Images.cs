using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
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

      #region older - GetPixel - slow
      public Point? SearchSample() //test jestli to bude fungovat tak
      {
         using (Bitmap screen = ExactScreenshot(ScreenStart, ScreenEnd))
         {
            // Ensure the sample fits within the screen
            int maxX = screen.Width - Sample.Width;
            int maxY = screen.Height - Sample.Height;

            //string dir = Path.Combine(Application.StartupPath, "Samples");
            //string screenPath = Path.Combine(dir, $"screen.png");
            //screen.Save(screenPath);

            for (int x = 0; x < maxX; x++)
            {
               for (int y = 0; y < maxY; y++)
               {
                  // Compare the first pixel and then check the whole sample
                  if (ColorsAreSimilar(Sample.GetPixel(0, 0), screen.GetPixel(x, y)) && IsInnerImage(x, y, Sample, screen))
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

      private bool ColorsAreSimilar(Color sample, Color screen, int tolerance = 5)
      {
         return Math.Abs(sample.R - screen.R) <= tolerance &&
                Math.Abs(sample.G - screen.G) <= tolerance &&
                Math.Abs(sample.B - screen.B) <= tolerance;
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
               Color sampleColor = sample.GetPixel(x, y);
               Color screenColor = screen.GetPixel(left + x, top + y);

               if (!ColorsAreSimilar(sampleColor, screenColor))
               {
                  return false; // sample is not inner image of screen
               }
            }
         }
         return true; //sample is inner image of screen
      }
      #endregion

      #region newer - Marshal - fast
      /// <summary>
      /// Public wrapper – zavoláš takto: var point = img.SearchSampleFast();
      /// </summary>
      public Point? SearchSampleFast(int tolerance = 5)
      {
         using (Bitmap screen = ExactScreenshot(ScreenStart, ScreenEnd))
         {
            return SearchSampleFastInternal(Sample, screen, tolerance);
         }
      }

      // === Interní logika s Marshal.Copy ===
      private Point? SearchSampleFastInternal(Bitmap sample, Bitmap screen, int tolerance = 5)
      {
         // Lock sample
         BitmapData sampleData = sample.LockBits(
             new Rectangle(0, 0, sample.Width, sample.Height),
             ImageLockMode.ReadOnly,
             PixelFormat.Format24bppRgb);

         // Lock screen
         BitmapData screenData = screen.LockBits(
             new Rectangle(0, 0, screen.Width, screen.Height),
             ImageLockMode.ReadOnly,
             PixelFormat.Format24bppRgb);

         int maxX = screen.Width - sample.Width;
         int maxY = screen.Height - sample.Height;

         try
         {
            // Copy data to byte arrays
            byte[] sampleBytes = new byte[sampleData.Stride * sampleData.Height];
            byte[] screenBytes = new byte[screenData.Stride * screenData.Height];

            Marshal.Copy(sampleData.Scan0, sampleBytes, 0, sampleBytes.Length);
            Marshal.Copy(screenData.Scan0, screenBytes, 0, screenBytes.Length);

            int sStride = sampleData.Stride;
            int scStride = screenData.Stride;

            // First pixel of sample
            byte sB0 = sampleBytes[0];
            byte sG0 = sampleBytes[1];
            byte sR0 = sampleBytes[2];

            for (int y = 0; y <= maxY; y++)
            {
               for (int x = 0; x <= maxX; x++)
               {
                  int scIndex = y * scStride + x * 3;
                  byte scB = screenBytes[scIndex + 0];
                  byte scG = screenBytes[scIndex + 1];
                  byte scR = screenBytes[scIndex + 2];

                  // 1️⃣ Check first pixel
                  if (!ColorsAreSimilarLockBits(sR0, sG0, sB0, scR, scG, scB, tolerance))
                     continue;

                  // 2️⃣ Check whole sample
                  if (IsInnerImageLockBits(x, y, sampleBytes, screenBytes, sample.Width, sample.Height, sStride, scStride, tolerance))
                     return new Point(x, y);
               }
            }
         }
         finally
         {
            sample.UnlockBits(sampleData);
            screen.UnlockBits(screenData);
         }

         return null;
      }

      private bool ColorsAreSimilarLockBits(byte r1, byte g1, byte b1, byte r2, byte g2, byte b2, int tolerance = 5)
      {
         return Math.Abs(r1 - r2) <= tolerance &&
                Math.Abs(g1 - g2) <= tolerance &&
                Math.Abs(b1 - b2) <= tolerance;
      }

      private bool IsInnerImageLockBits(
          int left, int top,
          byte[] sampleBytes, byte[] screenBytes,
          int width, int height,
          int sStride, int scStride,
          int tolerance = 5)
      {
         for (int y = 0; y < height; y++)
         {
            int sRow = y * sStride;
            int scRow = (top + y) * scStride + left * 3;

            for (int x = 0; x < width; x++)
            {
               int sIdx = sRow + x * 3;
               int scIdx = scRow + x * 3;

               byte sB = sampleBytes[sIdx + 0];
               byte sG = sampleBytes[sIdx + 1];
               byte sR = sampleBytes[sIdx + 2];

               byte scB = screenBytes[scIdx + 0];
               byte scG = screenBytes[scIdx + 1];
               byte scR = screenBytes[scIdx + 2];

               if (!ColorsAreSimilarLockBits(sR, sG, sB, scR, scG, scB, tolerance))
                  return false;
            }
         }
         return true;
      }
      #endregion

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
         using (Graphics gfx = Graphics.FromImage(screenshot))
         {
            gfx.CopyFromScreen(startScreen.X, startScreen.Y, 0, 0, size);
         }
         return screenshot;
      }

   }
}