using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneTab_Order
{
   class ExtractHelper
   {
      public enum DuplicateRemoveMode
      {
         KeepFirst,
         KeepLast
      }

      /// <summary>
      /// Odstraní duplicitní řádky podle zvoleného režimu, prázdné řádky zachová.
      /// </summary>
      /// <param name="lines">Seznam řádků ke zpracování.</param>
      /// <param name="removeMode">Režim odstranění duplikátů.</param>
      /// <returns>Nový seznam bez duplicit podle pravidla.</returns>
      public static (List<string> lines, int removedCount) RemoveDuplicatesFromLines(List<string> lines, DuplicateRemoveMode removeMode)
      {
         var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
         var result = new List<string>();
         int removed = 0;
         if (removeMode == DuplicateRemoveMode.KeepLast)
         {
            var reversed = lines.AsEnumerable().Reverse().ToList();
            var temp = new List<string>();
            foreach (var line in reversed)
            {
               if (string.IsNullOrWhiteSpace(line))
               {
                  temp.Add(line);
                  continue;
               }
               if (seen.Add(line))
                  temp.Add(line);
               else
                  removed++;
            }
            result = temp.AsEnumerable().Reverse().ToList();
         }
         else
         {
            foreach (var line in lines)
            {
               if (string.IsNullOrWhiteSpace(line))
               {
                  result.Add(line);
                  continue;
               }
               if (seen.Add(line))
                  result.Add(line);
               else
                  removed++;
            }
         }
         return (result, removed);
      }
   }
}
