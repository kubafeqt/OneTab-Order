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
      public static HashSet<string> TrackingQueries = new HashSet<string>
      {
          "utm_source", "utm_medium", "utm_campaign", "utm_term", "utm_content",
          "gclid", "fbclid", "mc_cid", "mc_eid", "ref", "referrer", "ref_src",
          "igshid", "mkt_tok", "yclid", "dclid", "pk_campaign", "pk_kwd", "utm_referrer",
          "utm_reader", "utm_name", "utm_social", "utm_social-type", "utm_swu", "utm_expid",
          "utm_id", "utm_lp", "utm_viz_id", "utm_pubreferrer", "utm_scp", "utm_source_platform",
          "utm_source_platform_type", "utm_creative_format", "utm_brand", "utm_audience",
          "utm_placement", "utm_target", "utm_ban", "utm_adgroup", "utm_ad", "utm_keyword",
          "utm_matchtype", "utm_network", "utm_device", "utm_adposition", "utm_feeditemid",
          "utm_targetid", "utm_loc_interest_ms", "utm_loc_physical_ms", "utm_placementtype",
          "utm_productchannel", "utm_productcountry", "utm_productlanguage", "utm_creative",
          "utm_adtype", "utm_targettype", "utm_loc_interest", "utm_loc_physical", "utm_contentid",
          "utm_contenttype", "utm_productid", "utm_productgroupid", "utm_campaignid",
          "utm_campaigntype", "utm_adgroupid", "utm_keywordid", "utm_matchtypeid", "utm_networktype",
          "utm_deviceid", "utm_adpositionid", "utm_feeditemidid", "utm", "click", "referrerid", "sid", "sessionid",
          "session_id", "sessionid2", "sessionToken", "session_token", "token", "aff_id", "affiliate_id",
          "partnerid", "partner_id", "source", "campaign", "medium", "term", "content", "ad_id", "trk",
          "trkCampaign", "trkContact", "trkMsg", "trkModule", "trkSrc", "icid", "soc_src", "soc_trk",
          "soc_platform", "soc_medium", "soc_campaign", "soc_content", "soc_term", "pid", "afid", "dclid_test",
          "gclsrc", "gcl_aw", "gcl_dc", "cx", "ncid", "adgroup", "adset", "adset_id", "adsetid", "creative",
          "creative_id", "creativeid", "cmpid", "placement", "placementid", "matchtype", "matchtypeid",
          "loc_interest_ms", "loc_interest", "loc_physical_ms", "loc_physical", "fb_action_ids",
          "fb_action_types", "fb_ref", "fb_source", "action_object_map", "action_type_map",
          "action_ref_map", "referrer_type", "referrer_source", "referrer_medium", "referrer_term",
          "referrer_content", "referrer_campaign", "_gid", "_ga", "_fbp", "ajs_aid", "ajs_anid",
          "ajs_user_id", "ajs_group_id", "ajs_job_id", "ajs_job_referrer", "tracking_id", "trk_info",
          "trk_info1", "trk_info2", "trk_info3", "trk_info4", "trk_info5", "shopify_y", "shopify_s",
          "shopify_x", "scid", "sc_intid", "sc_lid", "sc_cid", "sc_eid", "sc_campid", "sc_channel",
          "sc_geo", "sc_outlet", "sc_country", "sc_currency", "sc_lang", "sc_version", "sc_affiliate",
          "sc_affiliate_id", "sc_referrer", "sc_referrer_domain", "sc_referrer_url", "sc_keyword",
          "sc_matchtype", "sc_network", "sc_device", "sc_adposition", "sc_feeditemid", "sc_targetid",
          "sc_loc_interest_ms", "sc_loc_physical_ms",
          "clid", "rdid", "msid", "vid", "lid", "zid", "rid", "cmp", "adset_name", "placement_name",
          "adgroup_name", "creative_name", "matchtype_name", "action_id", "action_name",
          "action_source", "action_medium", "action_campaign", "action_content",
          "action_term", "action_platform", "conversion_id", "conversion_label", "conversion_name",
          "pixel_id", "fbclid_source", "ga_session_id", "ga_user_id", "ga_campaign_id",
          "ga_content_id", "ga_tracker", "ga_term_id", "ga_device_id", "ga_source_id", "ga_ref_id",
          "ga_network_id", "ga_matchtype_id", "ga_adgroup_id", "ga_ad_id", "ga_medium_id"
      };

      public static List<Tabs> TabList = new List<Tabs>();
      public static int removedDuplicates = 0;
      public static int trackingQueriesRemoved;
      //public static bool TabsAdded = false;
      //public static bool TabsChanged = false;

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
            removedDuplicates = 0;
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

         removedDuplicates = TabList.Count - newList.Count;
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

      public static void RemoveTrackingQueries()
      {
         trackingQueriesRemoved = 0;
         foreach (var tab in TabList)
         {
            bool queryRemoved; // Flag to track if a query is removed
            do
            {
               queryRemoved = false;
               foreach (var query in TrackingQueries)
               {
                  int queryIndex = tab.Url.IndexOf($"?{query}=", StringComparison.OrdinalIgnoreCase);
                  if (queryIndex == -1)
                  {
                     queryIndex = tab.Url.IndexOf($"&{query}=", StringComparison.OrdinalIgnoreCase);
                  }
                  if (queryIndex != -1)
                  {
                     tab.Url = tab.Url.Substring(0, queryIndex);
                     // Optionally trim any trailing '?' or '&' if they are left at the end
                     tab.Url = tab.Url.TrimEnd('?', '&');
                     trackingQueriesRemoved++;
                     queryRemoved = true; // Mark that a query was removed
                     break; // Break out of the inner loop to start over
                  }
               }
            }
            while (queryRemoved); // Repeat until no queries are removed in the last pass
         }
      }

      public static void RemoveTrackingQueriesFromLines(List<string> lines)
      {
         for (int i = 0; i < lines.Count; i++) // Iterate using indices
         {
            bool queryRemoved;
            do
            {
               queryRemoved = false;
               foreach (var query in TrackingQueries)
               {
                  int queryIndex = lines[i].IndexOf($"?{query}=", StringComparison.OrdinalIgnoreCase);
                  if (queryIndex == -1)
                  {
                     queryIndex = lines[i].IndexOf($"&{query}=", StringComparison.OrdinalIgnoreCase);
                  }
                  if (queryIndex != -1)
                  {
                     lines[i] = lines[i].Substring(0, queryIndex);
                     // Optionally trim any trailing '?' or '&' if left at the end
                     lines[i] = lines[i].TrimEnd('?', '&');
                     trackingQueriesRemoved++;
                     queryRemoved = true; // A query was removed, so continue checking
                     break; // Restart the inner loop to recheck for more queries
                  }
               }
            }
            while (queryRemoved); // Repeat until no queries are removed in the current string
         }
      }

   }
}
