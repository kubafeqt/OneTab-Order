using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OneTab_Order
{
   internal class Tabs
   {
      public static List<Tabs> TabList = new List<Tabs>();
      public static int RemovedDuplicates = 0;
      public static bool TabsAdded = false;
      public static bool TabsChanged = false;

      public string Url { get; set; }
      public string Description { get; set; }
      public Tabs(string url, string description)
      {
         Url = url;
         Description = description;
         TabList.Add(this);
      }

      public static void AddTabs(string content, bool notRemoveEmptyEntries = false)
      {
         StringSplitOptions sso = notRemoveEmptyEntries ? StringSplitOptions.None : StringSplitOptions.RemoveEmptyEntries;
         var lines = content.Split(new[] { "\r\n", "\r", "\n" }, sso); //Split the content by new lines to get individual tab entries
         foreach (var line in lines)
         {
            if (string.IsNullOrWhiteSpace(line))
            {
               // přidáme "prázdný tab" (můžeš si upravit podle svého modelu)
               new Tabs(string.Empty, string.Empty);
               continue;
            }

            var parts = line.Split(new[] { '|' }, 2); //Split each line by the first occurrence of a space to separate URL and description
            if (parts.Length == 2)
            {
               string url = parts[0].Trim();
               string description = parts[1].Trim();
               new Tabs(url, description);
            }
            else
            {
               // pokud není ani '|', ale řádek není prázdný
               new Tabs(parts[0].Trim(), string.Empty);
            }
         }
         TabsAdded = true;
      }

      public static void OrderTabs() => TabList = TabList.OrderByDescending(tab => tab.Url).ThenBy(tab => tab.Description).ToList();
      //TabList = (from tab in TabList orderby tab.Url descending, tab.Description ascending select tab).ToList();


      public enum DuplicateRemoveMode
      {
         KeepFirst, //nechá první (nahoře), smaže spodní
         KeepLast   //nechá poslední (dole), smaže horní
      }

      public static void RemoveDuplicates(DuplicateRemoveMode mode)
      {
         #region default code
         //IEnumerable<Tabs> distinctTabs;

         //var emptyTabs = TabList.Where(t => string.IsNullOrWhiteSpace(t.Url)).ToList();
         //var nonEmptyTabs = TabList.Where(t => !string.IsNullOrWhiteSpace(t.Url)).ToList();

         //switch (mode)
         //{
         //   case DuplicateRemoveMode.KeepLast:
         //      {
         //         distinctTabs = nonEmptyTabs.GroupBy(tab => tab.Url).Select(g => g.Last());
         //         break;
         //      }

         //   case DuplicateRemoveMode.KeepFirst:
         //   default:
         //      {
         //         distinctTabs = nonEmptyTabs.GroupBy(tab => tab.Url).Select(g => g.First());
         //         break;
         //      }
         //}

         //var newList = distinctTabs.Concat(emptyTabs).ToList();
         //RemovedDuplicates = TabList.Count - newList.Count;
         //TabList = newList;

         #endregion

         if (TabList == null || TabList.Count == 0)
         {
            RemovedDuplicates = 0;
            return;
         }

         var seen = new HashSet<string>();
         var newList = new List<Tabs>();

         // pokud chceme KeepLast → projdeme list obráceně
         var source = (mode == DuplicateRemoveMode.KeepLast
             ? TabList.AsEnumerable().Reverse()
             : TabList);

         foreach (var tab in source)
         {
            if (string.IsNullOrWhiteSpace(tab.Url))
            {
               // Do HashSetu NIKDY nedávej prázdné položky!
               // Mezery přidáme vždy, nikdy nejsou považovány za duplicitu.
               newList.Add(tab); // prázdné vždy přidáme
               continue;
            }
            if (seen.Add(tab.Url))
            {
               newList.Add(tab); // první/poslední výskyt se přidá
            }
         }

         // pokud jsme šli odzadu (KeepLast), musíme výsledek otočit zpět
         if (mode == DuplicateRemoveMode.KeepLast)
            newList.Reverse();

         RemovedDuplicates = TabList.Count - newList.Count;
         TabList = newList;
      }

      //public static void RemoveDuplicates() //seskupí Tab podle Url a vezme vždy jen první z každé skupiny
      //{
      //   var distinctTabs = TabList
      //       .GroupBy(tab => tab.Url)
      //       .Select(x => x.First())
      //       .ToList();
      //   RemovedDuplicates = TabList.Count - distinctTabs.Count;
      //   TabList = distinctTabs;
      //}

      public static IEnumerable<IGrouping<string, Tabs>> GroupTabs() => TabList.GroupBy(t => new Uri(t.Url).Host);


   }
}
