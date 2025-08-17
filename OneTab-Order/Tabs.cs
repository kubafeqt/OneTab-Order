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

      public string Url { get; set; }
      public string Description { get; set; }
      public Tabs(string url, string description)
      {
         Url = url;
         Description = description;
         TabList.Add(this);
      }

      public static void AddTabs(string content)
      {
         var lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries); //Split the content by new lines to get individual tab entries
         foreach (var line in lines)
         {

            var parts = line.Split(new[] { '|' }, 2); //Split each line by the first occurrence of a space to separate URL and description
            if (parts.Length == 2)
            {
               string url = parts[0].Trim();
               string description = parts[1].Trim();
               new Tabs(url, description);
            }
         }
      }

      public static void OrderTabs() => TabList = TabList.OrderByDescending(tab => tab.Url).ThenBy(tab => tab.Description).ToList();
      //TabList = (from tab in TabList orderby tab.Url descending, tab.Description ascending select tab).ToList();

      public static void RemoveDuplicates() //seskupí Tab podle Url a vezme vždy jen první z každé skupiny
      {
         var distinctTabs = TabList
             .GroupBy(tab => tab.Url)
             .Select(x => x.First())
             .ToList();
         RemovedDuplicates = TabList.Count - distinctTabs.Count;
         TabList = distinctTabs;
      }

      public static IEnumerable<IGrouping<string, Tabs>> GroupTabs() => TabList.GroupBy(t => new Uri(t.Url).Host);

   }
}
