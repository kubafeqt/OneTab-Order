using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using static System.Windows.Forms.LinkLabel;

#region comments

//check for installed -> notepad++, pspad, ... , or notepad -> basic now (just notepad)
//can be extracted to txt, html with <a href=""> (+list for copy) -> basic now (txt)
//remove duplicates from extracted, even in rtbText -> working
//searching - builded in finder - get shortcut ctrl+f -> top now
//delete all from and after - &/?pp=, &/?utm, &/?fbclid, - check other tracking queries -> remove tracking queries checkbox -> basic now (?/$something=)
//get samples - phase 4: enter: same as samplestart, any phase: backspace: back to previous phase
//
//todo:
//image detection - marshal, then RPA like action -> check any sample is there - mouseleftclick, enter - till end (delete all tabs)
//save settings to db or file (json, xml, ini, ...)
//
//db:
//save browser and image detection to db
//create object of image detection from db
//delete previous sample (loaded hash from db), after creating new one
//
//
//
//comment:
//-> operuji s vygenerovaným kódem, aniž bych do detailu musel vědět jak v jádru funguje
//-> Přesně tak – můžeš s tím zacházet jako s “černou skříňkou”: používáš metody a vlastnosti jen pro jejich výsledek, aniž bys musel chápat všechny interní algoritmy.
//

#endregion

namespace OneTab_Order
{
   public partial class Form1 : Form
   {
      #region Load and init
      System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
      public Form1()
      {
         InitializeComponent();
         this.KeyPreview = true; // Formulář dostane všechny key eventy jako první

         timer.Interval = 250; //250 ms
         timer.Tick += Timer_Tick;
         timer.Start();
         cmbRemoveDuplicatesExtractedType.Items.AddRange(new string[] { "from below", "from up" });
         cmbRemoveDuplicatesExtractedType.SelectedIndex = 0;
         cmbRemoveDuplicatesExtractedType.Enabled = cboxRemoveDuplicatesExtracted.Checked;
         cboxRemoveDuplicatesFromUp.CheckedChanged += CboxDef_CheckedChanged;
         cboxRemoveDuplicatesFromBelow.CheckedChanged += CboxDef_CheckedChanged;

         cmbFindType.Items.AddRange(new string[] { "full line", "url only", "content only" });
         cmbFindType.SelectedIndex = 0;

         rtbText.DetectUrls = false; //disable auto url detection

         LoadPanelsSizeAndLocation();
         SwitchPanelVisible(panelMain);

         cmbSelectedBrowser.Items.Add($"default: {GetDefaultBrowserName()}");
         cmbSelectedBrowser.SelectedIndex = 0;
         DetectInstalledBrowsers();

         DoubleBuffered = true;
         KeyPreview = true; // formulář zachytí klávesy
      }

      private void LoadPanelsSizeAndLocation()
      {
         foreach (Panel panel in Controls.OfType<Panel>())
         {
            panel.Size = Settings.panelSize;
            panel.Location = Settings.panelLocation;
         }
      }

      #endregion

      #region Switch panels
      private void SwitchPanelVisible(Panel? panelToShow)
      {
         Dictionary<Panel, Button> panelButtonMap = new Dictionary<Panel, Button>()
         {
            { panelMain, btnMainPanel },
            { panelRPASettings, btnRPASettings }
         };

         foreach (Panel panel in Controls.OfType<Panel>())
         {
            panel.Visible = false;
            if (panelToShow != null)
            {
               panelButtonMap[panel].Visible = true;
               panelButtonMap[panel].Enabled = true;
            }
            else
            {
               panelButtonMap[panel].Visible = false;
            }
         }
         if (panelToShow == null) return;
         panelToShow.Visible = true;
         panelButtonMap[panelToShow].Enabled = false;
         Refresh();
      }

      private void btnMainPanel_Click(object sender, EventArgs e)
      {
         SwitchPanelVisible(panelMain);
      }

      private void btnRPASettings_Click(object sender, EventArgs e)
      {
         SwitchPanelVisible(panelRPASettings);
      }

      #endregion

      #region Panel main only
      private void ReAddTabs(ref bool notRemoveEmptyEntries)
      {
         notRemoveEmptyEntries = !notRemoveEmptyEntries ? cboxRemoveOnly.Checked : notRemoveEmptyEntries; //true = true
         Tabs.TabList.Clear(); //důležitý
         Tabs.AddTabs(rtbText.Text, notRemoveEmptyEntries);
      }

      private void btnOrder_Click(object sender, EventArgs e)
      {
         OrderTabs();
      }

      private void OrderTabs()
      {
         bool notRemoveEmptyEntries = false;
         ReAddTabs(ref notRemoveEmptyEntries);

         if (!cboxRemoveOnly.Checked)
         {
            Tabs.OrderTabs();
         }
         if (cboxRemoveTrackingQueries.Checked)
         {
            Tabs.RemoveTrackingQueries();
            lbTrackingQueriesRemoved.Text = $"Tracking queries removed: {Tabs.trackingQueriesRemoved}";
         }
         if (cboxRemoveDuplicatesFromBelow.Checked)
         {
            Tabs.RemoveDuplicates(Tabs.DuplicateRemoveMode.KeepFirst);
         }
         if (cboxRemoveDuplicatesFromUp.Checked)
         {
            Tabs.RemoveDuplicates(Tabs.DuplicateRemoveMode.KeepLast);
         }
         rtbText.Clear();

         if (!notRemoveEmptyEntries) //order tabs
         {
            foreach (var group in Tabs.GroupTabs()) //zavolá se pouze jednou, na začátku foreach
            {
               foreach (var tab in group)
               {
                  rtbText.AppendText($"{tab.Url} | {tab.Description}\n");
               }
               rtbText.AppendText("\n"); //mezera mezi skupinami
            }
         }
         else //remove duplicates only - not order tabs
         {
            foreach (var tab in Tabs.TabList)
            {
               if (tab.Url == string.Empty && tab.Description == string.Empty)
               {
                  rtbText.AppendText("\n");
                  continue;
               }
               rtbText.AppendText($"{tab.Url} | {tab.Description}\n");
            }
         }

         lbRemovedDuplicates.Text = $"Removed duplicates: {Tabs.removedDuplicates}";
      }

      private void btnCopyAllRtb_Click(object sender, EventArgs e)
      {
         Clipboard.SetText(rtbText.Text);
         SwitchButtonEnabled(btnCopyAllRtb, false);
      }

      private void btnSaveToFile_Click(object sender, EventArgs e)
      {
         string[] lines = rtbText.Text.Split("\n");
         // Uložení do souboru
         var saveFileDialog = new SaveFileDialog();
         saveFileDialog.Filter = "Textové soubory (*.txt)|*.txt|Všechny soubory (*.*)|*.*";
         saveFileDialog.Title = "Uložit exportovaný OneTab weby";
         string folderPath = MakePath("saved");
         saveFileDialog.InitialDirectory = folderPath;
         if (saveFileDialog.ShowDialog() == DialogResult.OK)
         {
            File.WriteAllLines(saveFileDialog.FileName, lines);
            MessageBox.Show("Soubor byl úspěšně uložen.", "Hotovo", MessageBoxButtons.OK, MessageBoxIcon.Information);

         }
      }

      bool rtbTextChanged = true;
      bool tbExtractTextChanged = true;
      string lastClipboardText = "";
      /// <summary>
      /// Timer for buttons enable/disable
      /// </summary>
      private void Timer_Tick(object sender, EventArgs e)
      {
         bool clipBoardChanged = Clipboard.ContainsText() && Clipboard.GetText() != lastClipboardText;
         bool rtbTextIsEmpty = string.IsNullOrWhiteSpace(rtbText.Text);
         if (rtbTextIsEmpty && rtbTextChanged)
         {
            SwitchButtonEnabled(btnOrder, false);
            SwitchButtonEnabled(btnSaveToFile, false);
            SwitchButtonEnabled(btnExtractWebpages, false);
            SwitchButtonEnabled(btnCopyAllRtb, false);
            rtbTextChanged = false;
         }
         else if (!rtbTextIsEmpty && (rtbTextChanged || tbExtractTextChanged || clipBoardChanged)) //rtbText is not empty
         {
            SwitchButtonEnabled(btnOrder, true);
            SwitchButtonEnabled(btnSaveToFile, true);
            string clipboarText = Clipboard.GetText().Trim(); //for button copy to clipboard enable/disable
            lastClipboardText = clipboarText;
            if ((rtbText.Text != clipboarText && !string.IsNullOrWhiteSpace(clipboarText)) || !Clipboard.ContainsText())
            {
               SwitchButtonEnabled(btnCopyAllRtb, true);
            }
            else
            {
               SwitchButtonEnabled(btnCopyAllRtb, false);
            }
            if (!string.IsNullOrWhiteSpace(tbExtractWebpages.Text))
            {
               SwitchButtonEnabled(btnExtractWebpages, true);
            }
            else
            {
               SwitchButtonEnabled(btnExtractWebpages, false);
            }
            rtbTextChanged = false;
            tbExtractTextChanged = false;
         }
      }

      private void rtbText_TextChanged(object sender, EventArgs e) => rtbTextChanged = true;

      private void tbExtractWebpages_TextChanged(object sender, EventArgs e) => tbExtractTextChanged = true;

      private void SwitchButtonEnabled(Button button, bool enable)
      {
         if (button.Enabled != enable)
         {
            button.Enabled = enable;
         }
      }

      private void CboxDef_CheckedChanged(object sender, EventArgs e)
      {
         var changed = sender as CheckBox;
         if (changed == null || !changed.Checked) return;

         UncheckOtherCboxes(changed);
      }

      private void UncheckOtherCboxes(CheckBox changed)
      {
         foreach (var cb in new[] { cboxRemoveDuplicatesFromUp, cboxRemoveDuplicatesFromBelow })
         {
            if (cb != changed)
            {
               cb.Checked = false;
            }
         }
      }

      private void cboxRemoveDuplicatesExtracted_CheckedChanged(object sender, EventArgs e)
      {
         cmbRemoveDuplicatesExtractedType.Enabled = cboxRemoveDuplicatesExtracted.Checked;
      }

      private void btnExtractWebpages_Click(object sender, EventArgs e)
      {
         bool notRemoveEmptyEntries = true;
         ReAddTabs(ref notRemoveEmptyEntries);

         string[] webpagesUrls = tbExtractWebpages.Text.Trim()
             .Split(';', StringSplitOptions.RemoveEmptyEntries)
             .Select(url => url.Trim())
             .Where(url => !string.IsNullOrEmpty(url) && url.Length > 1)
             .Distinct()
             .ToArray();

         // Sestavení bloků podle jednotlivých typů extrakce (každý typ = jedna položka z webpagesUrls)
         var linesByType = webpagesUrls
             .Select(url =>
                 Tabs.TabList
                     .Where(tab => !string.IsNullOrWhiteSpace(tab.Url) && tab.Url.Contains(url, StringComparison.OrdinalIgnoreCase))
                     .OrderBy(tab => tab.Url)
                     .Select(tab => string.IsNullOrWhiteSpace(tab.Description) ? tab.Url : $"{tab.Url} | {tab.Description}")
                     .ToList()
             )
             .Where(list => list.Count > 0)
             .ToList();

         // Sestavení výsledných řádků: mezi bloky prázdný řádek
         var lines = linesByType.SelectMany((block, i) =>
             i == 0 ? block : new[] { "" }.Concat(block)
         ).ToList();

         if (cboxExtractedRemoveTrackingQueries.Checked)
         {
            Tabs.RemoveTrackingQueriesFromLines(lines);
            lbTrackingQueriesRemoved.Text = $"Tracking queries removed: {Tabs.trackingQueriesRemoved}";
         }
         else
         {
            lbTrackingQueriesRemoved.Text = $"Tracking queries removed: 0";
         }
         if (cboxRemoveDuplicatesExtracted.Checked)
         {
            // Použijeme naši novou metodu
            var removeMode = cmbRemoveDuplicatesExtractedType.SelectedItem.ToString().Equals("from below")
                      ? ExtractHelper.DuplicateRemoveMode.KeepFirst
                      : ExtractHelper.DuplicateRemoveMode.KeepLast;
            var result = ExtractHelper.RemoveDuplicatesFromLines(lines, removeMode);
            lines = result.lines;
            lbRemovedDuplicates.Text = $"Removed duplicates: {result.removedCount}";
         }
         else
         {
            lbRemovedDuplicates.Text = $"Removed duplicates: 0";
         }

         // Uložení do souboru
         var saveFileDialog = new SaveFileDialog();
         saveFileDialog.Filter = "Textové soubory (*.txt)|*.txt|Všechny soubory (*.*)|*.*";
         saveFileDialog.Title = "Uložit extrahované weby";
         string folderPath = MakePath("exports");
         saveFileDialog.InitialDirectory = folderPath;
         if (saveFileDialog.ShowDialog() == DialogResult.OK)
         {
            File.WriteAllLines(saveFileDialog.FileName, lines);
            MessageBox.Show("Soubor byl úspěšně uložen.", "Hotovo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (cboxRemoveSitesFromDef.Checked) //odeber z rtbText
            {
               var allLines = rtbText.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                               .Select(row => row.TrimEnd())
                               .ToList();

               // Odeber všechny řádky, které obsahují některou z webových adres (ignoruje velikost písmen)
               var remainingLines = allLines.Where(row =>
                   string.IsNullOrWhiteSpace(row) ||
                   !webpagesUrls.Any(url => row.IndexOf(url, StringComparison.OrdinalIgnoreCase) >= 0)
               ).ToList();

               rtbText.Text = string.Join(Environment.NewLine, remainingLines);
            }
            lbExtractedWebpages.Text = $"Extracted webpages: {lines.Count(line => !string.IsNullOrWhiteSpace(line))}";

            if (cboxOpenExtractedFile.Checked)
            {
               try
               {
                  Process.Start(new System.Diagnostics.ProcessStartInfo
                  {
                     FileName = saveFileDialog.FileName,
                     UseShellExecute = true
                  });
               }
               catch (Exception ex)
               {
                  MessageBox.Show($"Nepodařilo se otevřít soubor: {ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
               }
            }
         }
      }

      private void panelMain_Paint(object sender, PaintEventArgs e)
      {
         Graphics g = e.Graphics;
         Pen penBlack = new Pen(Brushes.Black, 2);
         g.DrawLine(penBlack, drawlineX.X, drawlineY.X, drawlineX.Y, drawlineY.Y);
      }

      private void cboxRemoveDuplicatesOnly_CheckedChanged(object sender, EventArgs e)
      {
         btnOrder.Text = cboxRemoveOnly.Checked ? "remove" : "order";
      }

      #region opening and making folders
      private void btnOpenSavedFolder_Click(object sender, EventArgs e)
      {
         OpenFolder("saved");
      }

      private void btnOpenExtractedFolder_Click(object sender, EventArgs e)
      {
         OpenFolder("exports");
      }

      private void OpenFolder(string folderName)
      {
         string folderPath = MakePath(folderName);
         var psi = new ProcessStartInfo
         {
            FileName = folderPath,
            UseShellExecute = true
         };
         Process.Start(psi);
      }

      private string MakePath(string folderName)
      {
         string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
         if (!Directory.Exists(folderPath))
         {
            Directory.CreateDirectory(folderPath);
         }
         return folderPath;
      }

      #endregion

      #region Searching text logic
      private int lastSearchPosition = 0;
      private string lastSearchText = "";
      private void btnFindNext_Click(object sender, EventArgs e)
      {
         Find();
      }

      private void btnFindPrev_Click(object sender, EventArgs e)
      {
         Find(false);
      }

      private void Find(bool next = true)
      {
         string searchText = tbFind.Text;

         if (string.IsNullOrEmpty(searchText))
            return;

         Action<string> Find = next ? FindNext : FindPrev; //override
         Find(searchText);
      }

      private void FindNext(string searchText)
      {
         // Reset search if text changed
         if (searchText != lastSearchText)
         {
            lastSearchText = searchText;
            lastSearchPosition = 0;
         }

         string searchMode = cmbFindType.SelectedItem.ToString();

         // Start from current cursor/selection position if available
         int startPos;
         if (rtbText.SelectionLength > 0)
         {
            // If text is selected, start from end of selection
            startPos = rtbText.SelectionStart + rtbText.SelectionLength;
         }
         else
         {
            // If no selection, use cursor position or previous search position
            startPos = rtbText.SelectionStart > 0 ? rtbText.SelectionStart : lastSearchPosition;
         }

         bool found = false;

         while (!found)
         {
            // Find next occurrence
            int index = rtbText.Find(searchText, startPos, RichTextBoxFinds.None);

            // Try from beginning if not found
            if (index == -1 && startPos > 0)
            {
               startPos = 0;
               index = rtbText.Find(searchText, 0, RichTextBoxFinds.None);
            }

            // Nothing found at all
            if (index == -1)
            {
               MessageBox.Show($"Text nebyl nalezen{(searchMode == "content only" ? " v obsahu (za '|')" : "")}.");
               lastSearchPosition = 0;
               break;
            }

            // Find line boundaries
            int lineStart = index;
            while (lineStart > 0 && rtbText.Text[lineStart - 1] != '\n' && rtbText.Text[lineStart - 1] != '\r')
               lineStart--;

            int lineEnd = index;
            while (lineEnd < rtbText.Text.Length && rtbText.Text[lineEnd] != '\n' && rtbText.Text[lineEnd] != '\r')
               lineEnd++;

            // Handle line ending characters
            if (lineEnd < rtbText.Text.Length)
            {
               if (lineEnd + 1 < rtbText.Text.Length && rtbText.Text[lineEnd] == '\r' && rtbText.Text[lineEnd + 1] == '\n')
                  lineEnd += 2;
               else
                  lineEnd += 1;
            }

            // Get line text and check if match is valid based on search mode
            string lineText = rtbText.Text.Substring(lineStart, Math.Min(lineEnd, rtbText.Text.Length) - lineStart);
            int pipeIndex = lineText.IndexOf('|');
            int matchPosInLine = index - lineStart;

            bool isValidMatch = false;

            switch (searchMode)
            {
               case "url only":
                  isValidMatch = (pipeIndex == -1 || matchPosInLine < pipeIndex);
                  break;
               case "content only":
                  isValidMatch = (pipeIndex != -1 && matchPosInLine > pipeIndex);
                  break;
               case "full line":
                  isValidMatch = true;
                  break;
            }

            if (isValidMatch)
            {
               // Select the line and set up for next search
               rtbText.Select(lineStart, lineEnd - lineStart);
               rtbText.ScrollToCaret();
               rtbText.Focus();
               lastSearchPosition = index + searchText.Length;
               found = true;
            }
            else
            {
               // Continue searching from after this occurrence
               startPos = index + searchText.Length;
            }
         }
      }

      private void FindPrev(string searchText)
      {
         // Reset search if text changed
         if (searchText != lastSearchText)
         {
            lastSearchText = searchText;
            // For reverse search, start from the end when resetting
            lastSearchPosition = rtbText.TextLength;
         }

         // Start from current cursor/selection position if available
         int startPos;
         if (rtbText.SelectionLength > 0)
         {
            // If text is selected, start from beginning of selection
            startPos = rtbText.SelectionStart;
         }
         else
         {
            // If no selection, use cursor position or previous search position
            startPos = rtbText.SelectionStart > 0 ? rtbText.SelectionStart : lastSearchPosition;
         }

         string searchMode = cmbFindType.SelectedItem.ToString();
         bool found = false;

         while (!found)
         {
            // Find previous occurrence
            int index = rtbText.Find(searchText, 0, startPos, RichTextBoxFinds.Reverse);

            // Try from the end if not found
            if (index == -1 && startPos < rtbText.TextLength)
            {
               startPos = rtbText.TextLength;
               index = rtbText.Find(searchText, 0, startPos, RichTextBoxFinds.Reverse);
            }

            // Nothing found at all
            if (index == -1)
            {
               MessageBox.Show($"Text nebyl nalezen{(searchMode == "content only" ? " v obsahu (za '|')" : "")}.");
               lastSearchPosition = rtbText.TextLength;
               break;
            }

            // Find line boundaries
            int lineStart = index;
            while (lineStart > 0 && rtbText.Text[lineStart - 1] != '\n' && rtbText.Text[lineStart - 1] != '\r')
               lineStart--;

            int lineEnd = index;
            while (lineEnd < rtbText.Text.Length && rtbText.Text[lineEnd] != '\n' && rtbText.Text[lineEnd] != '\r')
               lineEnd++;

            // Handle line ending characters
            if (lineEnd < rtbText.Text.Length)
            {
               if (lineEnd + 1 < rtbText.Text.Length && rtbText.Text[lineEnd] == '\r' && rtbText.Text[lineEnd + 1] == '\n')
                  lineEnd += 2;
               else
                  lineEnd += 1;
            }

            // Get line text and check if match is valid based on search mode
            string lineText = rtbText.Text.Substring(lineStart, Math.Min(lineEnd, rtbText.Text.Length) - lineStart);
            int pipeIndex = lineText.IndexOf('|');
            int matchPosInLine = index - lineStart;

            bool isValidMatch = false;

            switch (searchMode)
            {
               case "url only":
                  isValidMatch = (pipeIndex == -1 || matchPosInLine < pipeIndex);
                  break;
               case "content only":
                  isValidMatch = (pipeIndex != -1 && matchPosInLine > pipeIndex);
                  break;
               case "full line":
                  isValidMatch = true;
                  break;
            }

            if (isValidMatch)
            {
               // Select the line and set up for next search
               rtbText.Select(lineStart, lineEnd - lineStart);
               rtbText.ScrollToCaret();
               rtbText.Focus();
               lastSearchPosition = index;
               found = true;
            }
            else
            {
               // Continue searching from before this occurrence
               startPos = index;
               if (startPos <= 0) break; // Prevent infinite loop
            }
         }
      }

      private void tbFind_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.Enter)
         {
            Find();
            e.SuppressKeyPress = true; //prevents bump sound
         }
         if (e.KeyCode == Keys.Up) //find prev
         {
            Find(false);
            e.SuppressKeyPress = true;
         }
      }

      private void rtbText_KeyDown(object sender, KeyEventArgs e)
      {
         if (rtbText.SelectionLength > 0)
         {
            if (e.KeyCode == Keys.Enter) //find next
            {
               Find();
               e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.Up) //find prev
            {
               Find(false);
               e.SuppressKeyPress = true;
            }
         }
      }

      #endregion

      #endregion

      #region Open Browser logic
      private void btnOpenSelectedBrowserOnOneTabUrl_Click(object sender, EventArgs e)
      {
         //Process.Start(new ProcessStartInfo(tbOneTabUrl.Text) { UseShellExecute = true });
         if (cmbSelectedBrowser.SelectedIndex == 0) //default
         {
            LaunchDefaultBrowser(tbOneTabUrl.Text);
         }
         else
         {
            LaunchBrowser();
         }
      }

      void LaunchBrowser()
      {
         Dictionary<string, string> browserExecutables = new()
             {
                 { "Google Chrome", "chrome.exe" },
                 { "Mozilla Firefox", "firefox.exe" },
                 { "Microsoft Edge", "msedge.exe" },
                 { "Opera", "opera.exe" },
                 { "Opera GX", "opera.exe" },
                 { "Brave", "brave.exe" },
                 { "Vivaldi", "vivaldi.exe" },
                 { "Safari", "safari.exe" },
                 { "Internet Explorer", "iexplore.exe" }
             };

         string[] chromiumBasedBrowser = { "chrome.exe", "msedge.exe", "opera.exe", "brave.exe", "vivaldi.exe" };

         string browserName = browserExecutables[cmbSelectedBrowser.SelectedItem.ToString()];
         //string browserPath = GetInstalledBrowserPath(browserName);
         if (browserName != null)
         {
            Process.Start(new ProcessStartInfo
            {
               FileName = browserName,
               Arguments = tbOneTabUrl.Text,
               UseShellExecute = true
            });
            //it is chromium based browser - go to extension page manually:
            if (chromiumBasedBrowser.Contains(browserName) && !string.IsNullOrWhiteSpace(tbOneTabUrl.Text) && tbOneTabUrl.Text.StartsWith("extension://", StringComparison.OrdinalIgnoreCase))
            {
               if (cboxUseClipboard.Checked)
               {
                  Clipboard.SetText(tbOneTabUrl.Text);
               }
               // Wait until any Chrome window exists
               string pureBrowserName = browserName.Split('.')[0];
               while (!BrowserWindowExists(pureBrowserName))
               {
                  Thread.Sleep(100);
               }
               Thread.Sleep(cboxUseClipboard.Checked ? 100 : 200); //ensure UI is fully responsive
               SendKeys.SendWait("^l");
               Thread.Sleep(100);
               if (cboxUseClipboard.Checked)
               {
                  SendKeys.SendWait("^v");
               }
               else
               {
                  SendKeys.SendWait(tbOneTabUrl.Text); //for the case if Clipboard.SetText() won't work or user don't want use/override clipboard
               }
               Thread.Sleep(100);
               SendKeys.SendWait("{ENTER}");
            }
         }
         else
         {
            MessageBox.Show($"Could not find the path for {browserName}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      [DllImport("user32.dll")]
      [return: MarshalAs(UnmanagedType.Bool)]
      private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

      private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

      [DllImport("user32.dll")]
      private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

      static bool BrowserWindowExists(string processName)
      {
         bool found = false;

         EnumWindows((hWnd, lParam) =>
         {
            GetWindowThreadProcessId(hWnd, out uint pid);
            try
            {
               var p = Process.GetProcessById((int)pid);
               if (p.ProcessName.Equals(processName, StringComparison.OrdinalIgnoreCase))
               {
                  found = true;
                  return false; // stop enumeration
               }
            }
            catch { }
            return true; // continue enumeration
         }, IntPtr.Zero);

         return found;
      }

      static void LaunchDefaultBrowser(string url)
      {
         string browserCmd = GetDefaultBrowserCommand();
         if (browserCmd == null)
         {
            Console.WriteLine("Could not detect default browser.");
            return;
         }

         // Insert the URL into the command string
         string command = browserCmd.Replace("%1", url);

         // Split into exe + args
         ParseCommand(command, out string exePath, out string args);

         Console.WriteLine($"Starting: {exePath} {args}");
         Process.Start(exePath, args);
      }

      static string GetDefaultBrowserCommand()
      {
         const string userChoicePath =
             @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";

         using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(userChoicePath))
         {
            if (userChoiceKey == null) return null;

            string progId = userChoiceKey.GetValue("ProgId") as string;
            if (string.IsNullOrEmpty(progId)) return null;

            using (RegistryKey cmdKey = Registry.ClassesRoot.OpenSubKey(progId + @"\shell\open\command"))
            {
               if (cmdKey == null) return null;
               return cmdKey.GetValue(null) as string;
            }
         }
      }

      static void ParseCommand(string command, out string exePath, out string args)
      {
         exePath = null;
         args = null;

         if (string.IsNullOrEmpty(command)) return;

         command = command.Trim();

         // If quoted path
         if (command.StartsWith("\""))
         {
            int endQuote = command.IndexOf("\"", 1);
            exePath = command.Substring(1, endQuote - 1);
            args = command.Substring(endQuote + 1).Trim();
         }
         else
         {
            int firstSpace = command.IndexOf(" ");
            if (firstSpace > 0)
            {
               exePath = command.Substring(0, firstSpace);
               args = command.Substring(firstSpace + 1).Trim();
            }
            else
            {
               exePath = command;
               args = "";
            }
         }
      }

      static string GetDefaultBrowserName()
      {
         const string userChoicePath =
             @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";

         using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(userChoicePath))
         {
            if (userChoiceKey == null)
               return "Not found";

            object progIdValue = userChoiceKey.GetValue("ProgId");
            if (progIdValue == null)
               return "Not found";

            string progId = progIdValue.ToString();

            // Normalize in case ProgId has suffix (like ChromeHTML.7)
            //string normalized = progId.Split('.')[0];

            return progId switch
            {
               var s when s?.StartsWith("ChromeHTML") == true => "Google Chrome",
               var s when s?.StartsWith("MSEdgeHTM") == true => "Microsoft Edge",
               var s when s?.StartsWith("FirefoxURL") == true => "Mozilla Firefox",
               var s when s?.StartsWith("IE.HTTP") == true => "Internet Explorer",
               var s when s?.StartsWith("OperaStable") == true => "Opera",
               var s when s?.StartsWith("OperaGXStable") == true => "Opera GX",
               var s when s?.StartsWith("BraveHTML") == true => "Brave",
               var s when s?.StartsWith("VivaldiHTM") == true => "Vivaldi",
               var s when s?.StartsWith("SafariHTML") == true => "Safari",
               _ => progId ?? "Unknown Browser" // fallback: return the raw ProgId
            };
         }
      }

      void DetectInstalledBrowsers()
      {
         DetectInstalledBrowsers(Registry.LocalMachine, @"SOFTWARE\Clients\StartMenuInternet");
         DetectInstalledBrowsers(Registry.CurrentUser, @"SOFTWARE\Clients\StartMenuInternet");
         DetectInstalledBrowsers(Registry.LocalMachine, @"SOFTWARE\WOW6432Node\Clients\StartMenuInternet");
      }

      void DetectInstalledBrowsers(RegistryKey root, string path)
      {
         using (RegistryKey key = root.OpenSubKey(path))
         {
            if (key == null) return;

            foreach (var subKeyName in key.GetSubKeyNames())
            {
               using (RegistryKey browserKey = key.OpenSubKey(subKeyName))
               {
                  string browserName = (string)browserKey.GetValue(null);
                  bool browserExist = cmbSelectedBrowser.Items.Cast<object>()
                     .Any(item => item.ToString().Replace("default: ", "").Trim().Contains(browserName));
                  if (browserExist || browserName.Contains("explorer", StringComparison.OrdinalIgnoreCase)) continue;
                  cmbSelectedBrowser.Items.Add(browserName);
               }
            }
         }
      }

      #endregion

      Point drawlineX = new Point(519, 519);
      Point drawlineY = new Point(0, 200);
      private void Form1_Paint(object sender, PaintEventArgs e)
      {
         Graphics gfx = e.Graphics;
         if (panelMain.Visible)
         {
            Pen penBlack = new Pen(Brushes.Black, 2);
            gfx.DrawLine(penBlack, panelMain.Location.X + drawlineX.X, drawlineY.X, panelMain.Location.X + drawlineX.Y, drawlineY.Y);
         }

         foreach (var (refRect, sampleRect, searchStart) in entries)
         {
            using (Pen p1 = new Pen(Color.Red, 2)) gfx.DrawRectangle(p1, refRect);
            using (Pen p2 = new Pen(Color.Blue, 2)) gfx.DrawRectangle(p2, sampleRect);
         }

         if (firstRef.HasValue && clickPhase >= 1)
            gfx.FillRectangle(Brushes.Red, firstRef.Value.X - 2, firstRef.Value.Y - 2, 4, 4);

         if (firstRef.HasValue && secondRef.HasValue && clickPhase >= 2)
            gfx.DrawRectangle(Pens.Red, Rectangle.FromLTRB(
                Math.Min(firstRef.Value.X, secondRef.Value.X),
                Math.Min(firstRef.Value.Y, secondRef.Value.Y),
                Math.Max(firstRef.Value.X, secondRef.Value.X),
                Math.Max(firstRef.Value.Y, secondRef.Value.Y)
            ));

         if (firstSample.HasValue && clickPhase >= 3)
            gfx.FillRectangle(Brushes.Blue, firstSample.Value.X - 2, firstSample.Value.Y - 2, 4, 4);

         if (firstSample.HasValue && secondSample.HasValue && clickPhase >= 3)
            gfx.DrawRectangle(Pens.Blue, Rectangle.FromLTRB(
                Math.Min(firstSample.Value.X, secondSample.Value.X),
                Math.Min(firstSample.Value.Y, secondSample.Value.Y),
                Math.Max(firstSample.Value.X, secondSample.Value.X),
                Math.Max(firstSample.Value.Y, secondSample.Value.Y)
            ));
      }

      private void Form1_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.Control && e.KeyCode == Keys.F) // Detect Ctrl+F
         {
            tbFind.Focus();
            tbFind.SelectAll();
            e.SuppressKeyPress = true; // Prevent the default behavior
         }
         if (!Settings.originalWindowState)
         {
            if (e.KeyCode == Keys.Escape)
            {
               RestoreOriginalState();
            }
            if (e.KeyCode == Keys.Enter)
            {
               if (clickPhase == 4)
               {
                  // Simuluj kliknutí searchStart na currentMouse (nebo nic, pokud není)
                  if (currentMouse.HasValue && GetRefRect().Contains(currentMouse.Value))
                  {
                     searchStart = firstSample.Value;

                     Rectangle lastRefRect = GetRefRect();
                     Rectangle lastSampleRect = GetSampleRect();

                     entries.Add((lastRefRect, lastSampleRect, this.PointToScreen(searchStart.Value)));

                     // Reset a návrat
                     firstRef = secondRef = firstSample = secondSample = searchStart = null;
                     clickPhase = 0;
                     RestoreOriginalState();
                     PaintLayeredForm();
                  }
               }

               e.Handled = true;
               return;
            }

            if (e.KeyCode == Keys.Back)
            {
               switch (clickPhase)
               {
                  case 1:
                     firstRef = null;
                     clickPhase = 0;
                     break;
                  case 2:
                     secondRef = null;
                     clickPhase = 1;
                     break;
                  case 3:
                     firstSample = null;
                     clickPhase = 2;
                     break;
                  case 4:
                     secondSample = null;
                     clickPhase = 3;
                     break;
                  default:
                     break;
               }

               PaintLayeredForm();
               e.Handled = true;
               return;
            }
         }
      }

      #region Get Samples logic
      private List<(Rectangle refRect, Rectangle sampleRect, Point searchStart)> entries = new();

      private Point? firstRef = null;
      private Point? secondRef = null;
      private Point? firstSample = null;
      private Point? secondSample = null;
      private Point? searchStart = null;

      private int clickPhase = 0; // 0-4 podle fáze kliknutí
      private int sampleIndex = 1;
      Rectangle screenRect = Rectangle.Empty;
      private void Form1_MouseDown(object sender, MouseEventArgs e)
      {
         if (!Settings.originalWindowState)
         {
            switch (clickPhase)
            {
               case 0: // první bod REF
                  firstRef = e.Location;
                  clickPhase = 1;
                  break;

               case 1: // druhý bod REF
                  secondRef = e.Location;
                  clickPhase = 2;
                  break;

               case 2: // začátek SAMPLE – jen uvnitř REF
                  if (GetRefRect().Contains(e.Location))
                  {
                     firstSample = e.Location;
                     clickPhase = 3;
                  }
                  break;

               case 3: // konec SAMPLE – jen uvnitř REF
                  if (GetRefRect().Contains(e.Location))
                  {
                     secondSample = e.Location;

                     // uloží obdélníky
                     Rectangle refRect = GetRefRect();
                     Rectangle sampleRect = GetSampleRect();

                     // screenshot sample oblasti
                     Point s1 = PointToScreen(firstSample.Value);
                     Point s2 = PointToScreen(secondSample.Value);
                     screenRect = Rectangle.FromLTRB(
                         Math.Min(s1.X, s2.X),
                         Math.Min(s1.Y, s2.Y),
                         Math.Max(s1.X, s2.X),
                         Math.Max(s1.Y, s2.Y)
                     );

                     clickPhase = 4; // čeká pátý klik
                  }
                  break;

               case 4: // searchStart – jen uvnitř REF
                  if (GetRefRect().Contains(e.Location))
                  {
                     // --- ULOŽ SAMPLE BEZ OVERLAY ---
                     bool wasVisible = Visible;
                     Visible = false; // dočasně schováme overlay

                     // malá pauza, aby se systém stihl překreslit (jinak se overlay může zachytit)
                     Application.DoEvents();
                     Thread.Sleep(100);

                     using (Bitmap bmp = new Bitmap(screenRect.Width, screenRect.Height))
                     {
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                           g.CopyFromScreen(screenRect.Left, screenRect.Top, 0, 0, screenRect.Size);
                        }

                        using (var md5 = MD5.Create())
                        {
                           string timeData = DateTime.UtcNow.ToString(); // unikátní časový údaj
                           byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(timeData));
                           string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                           //todo: potom uložit do db

                           string dir = Path.Combine(Application.StartupPath, "Samples");
                           Directory.CreateDirectory(dir);
                           string file = Path.Combine(dir, $"Sample_{hash}.png");
                           bmp.Save(file);
                        }
                     }

                     Visible = wasVisible; // vrátíme overlay zpět
                     Application.DoEvents();
                     PaintLayeredForm(); // obnovíme průhlednou vrstvu

                     //poslední krok:
                     searchStart = e.Location;

                     // uloží kompletní záznam
                     Rectangle lastRefRect = GetRefRect();
                     Rectangle lastSampleRect = GetSampleRect();

                     entries.Add((lastRefRect, lastSampleRect, PointToScreen(searchStart.Value)));

                     // reset a návrat do původního stavu
                     firstRef = secondRef = firstSample = secondSample = searchStart = null;
                     clickPhase = 0;
                     RestoreOriginalState();
                  }
                  break;
            }

            Invalidate();
            PaintLayeredForm();
         }
      }

      // --- pomocné metody pro získání obdélníků ---
      private Rectangle GetRefRect()
      {
         return Rectangle.FromLTRB(
             Math.Min(firstRef.Value.X, secondRef.Value.X),
             Math.Min(firstRef.Value.Y, secondRef.Value.Y),
             Math.Max(firstRef.Value.X, secondRef.Value.X),
             Math.Max(firstRef.Value.Y, secondRef.Value.Y)
         );
      }

      private Rectangle GetSampleRect()
      {
         return Rectangle.FromLTRB(
             Math.Min(firstSample.Value.X, secondSample.Value.X),
             Math.Min(firstSample.Value.Y, secondSample.Value.Y),
             Math.Max(firstSample.Value.X, secondSample.Value.X),
             Math.Max(firstSample.Value.Y, secondSample.Value.Y)
         );
      }

      private Point ConstrainToRefRect(Point p)
      {
         if (!firstRef.HasValue || !secondRef.HasValue)
            return p; // ještě není definován REF

         Rectangle refRect = Rectangle.FromLTRB(
             Math.Min(firstRef.Value.X, secondRef.Value.X),
             Math.Min(firstRef.Value.Y, secondRef.Value.Y),
             Math.Max(firstRef.Value.X, secondRef.Value.X),
             Math.Max(firstRef.Value.Y, secondRef.Value.Y)
         );

         int x = Math.Max(refRect.Left, Math.Min(p.X, refRect.Right));
         int y = Math.Max(refRect.Top, Math.Min(p.Y, refRect.Bottom));

         return new Point(x, y);
      }

      private Point? currentMouse = null; // pro dynamické vykreslení
      private void Form1_MouseMove(object sender, MouseEventArgs e)
      {
         currentMouse = e.Location;
         if (!Settings.originalWindowState)
         {
            // pokud jsme ve fázi sample nebo search, omezíme myš na REF obdélník
            if ((clickPhase == 2 || clickPhase == 3 || clickPhase == 4) && firstRef.HasValue && secondRef.HasValue)
            {
               currentMouse = ConstrainToRefRect(currentMouse.Value);
            }

            PaintLayeredForm();
         }
      }

      private void btnGetSamples_Click(object sender, EventArgs e)
      {
         SwitchPanelVisible(null);

         // uložíme původní stav
         originalBorderStyle = FormBorderStyle;
         originalWindowState = WindowState;
         originalBounds = Bounds;
         Settings.originalWindowState = false; //to keydown esc for escape to original window state

         // Nastavení okna
         FormBorderStyle = FormBorderStyle.None; // Bez okrajů
         WindowState = FormWindowState.Maximized; // Fullscreen
         TopMost = true; // Nad všemi okny (volitelné)

         // Přidáme styl WS_EX_LAYERED
         int exStyle = GetWindowLong(Handle, GWL_EXSTYLE);
         SetWindowLong(Handle, GWL_EXSTYLE, exStyle | WS_EX_LAYERED);

         // vykreslení „téměř průhledného“ pozadí (alfa = 1)
         PaintLayeredForm();

      }

      private FormBorderStyle originalBorderStyle;
      private FormWindowState originalWindowState;
      private Rectangle originalBounds;
      private void RestoreOriginalState()
      {
         // zrušíme layered mód
         int exStyle = GetWindowLong(Handle, GWL_EXSTYLE);
         exStyle &= ~WS_EX_LAYERED;
         SetWindowLong(Handle, GWL_EXSTYLE, exStyle);

         // obnovíme původní parametry
         FormBorderStyle = originalBorderStyle;
         WindowState = originalWindowState;
         Bounds = originalBounds;
         TopMost = false;
         Settings.originalWindowState = true;
         SwitchPanelVisible(panelRPASettings);

         clickPhase = 0;
         firstRef = secondRef = firstSample = secondSample = searchStart = null;

         Refresh();
      }

      #region transparent window
      // --- níže jsou WinAPI metody + UpdateLayeredWindow logika ---

      const int WS_EX_LAYERED = 0x80000;
      const int GWL_EXSTYLE = -20;
      const int ULW_ALPHA = 0x02;
      const byte AC_SRC_OVER = 0x00;
      const byte AC_SRC_ALPHA = 0x01;

      [DllImport("user32.dll")] static extern int GetWindowLong(IntPtr hWnd, int nIndex);
      [DllImport("user32.dll")] static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
      [DllImport("user32.dll")] static extern IntPtr GetDC(IntPtr hWnd);
      [DllImport("user32.dll")] static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
      [DllImport("gdi32.dll")] static extern IntPtr CreateCompatibleDC(IntPtr hdc);
      [DllImport("gdi32.dll")] static extern bool DeleteDC(IntPtr hdc);
      [DllImport("gdi32.dll")] static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
      [DllImport("gdi32.dll")] static extern bool DeleteObject(IntPtr hObject);
      [DllImport("user32.dll")]
      static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst,
          ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pptSrc, int crKey,
          ref BLENDFUNCTION pblend, int dwFlags);

      [StructLayout(LayoutKind.Sequential)] struct POINT { public int x, y; public POINT(int X, int Y) { x = X; y = Y; } }
      [StructLayout(LayoutKind.Sequential)] struct SIZE { public int cx, cy; public SIZE(int CX, int CY) { cx = CX; cy = CY; } }
      [StructLayout(LayoutKind.Sequential, Pack = 1)]
      struct BLENDFUNCTION { public byte BlendOp, BlendFlags, SourceConstantAlpha, AlphaFormat; }

      private void PaintLayeredForm()
      {
         int width = Width;
         int height = Height;

         using (var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
         using (var g = Graphics.FromImage(bmp))
         {
            // průhledné pozadí (alfa=1 -> zachytí myš)
            g.Clear(Color.FromArgb(1, 0, 0, 0));
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // --- vykreslíme již uložené páry (v client souřadnicích) ---
            using (var penSavedRef = new Pen(Color.FromArgb(200, Color.Lime), 2))
            using (var penSavedSample = new Pen(Color.FromArgb(200, Color.DeepSkyBlue), 2))
            {
               foreach (var (refRect, sampleRect, searchPtScreen) in entries)
               {
                  g.DrawRectangle(penSavedRef, refRect);
                  g.DrawRectangle(penSavedSample, sampleRect);

                  // pokud je uložený searchStart v entries jako screen point, překlopíme do client coords
                  try
                  {
                     Point searchClient = this.PointToClient(searchPtScreen);
                     g.FillEllipse(Brushes.OrangeRed, searchClient.X - 4, searchClient.Y - 4, 8, 8);
                  }
                  catch { /* ignore */ }
               }
            }

            // pera / štětce pro aktivní kreslení
            using (var penRef = new Pen(Color.Lime, 2))
            using (var penSample = new Pen(Color.DeepSkyBlue, 2))
            using (var penSearch = new Pen(Color.Orange, 2))
            using (var brushPoint = new SolidBrush(Color.Red))
            using (var font = new Font("Segoe UI", 11, FontStyle.Bold))
            using (var textBrush = new SolidBrush(Color.White))
            {
               // --- DYNAMICKÁ TEČKA před prvním klikem (clickPhase == 0) ---
               if (clickPhase == 0 && currentMouse.HasValue && !firstRef.HasValue)
               {
                  var p = currentMouse.Value;
                  g.FillEllipse(brushPoint, p.X - 4, p.Y - 4, 8, 8);
                  //g.DrawString("Ref start (preview)", font, textBrush, p.X + 8, p.Y - 10);
               }

               // === REF obdélník náhled (fáze 1: máme firstRef, druhý bod je myš) ===
               if (firstRef.HasValue && (secondRef.HasValue || (clickPhase == 1 && currentMouse.HasValue)))
               {
                  var second = secondRef ?? currentMouse.Value;
                  Rectangle refRect = Rectangle.FromLTRB(
                      Math.Min(firstRef.Value.X, second.X),
                      Math.Min(firstRef.Value.Y, second.Y),
                      Math.Max(firstRef.Value.X, second.X),
                      Math.Max(firstRef.Value.Y, second.Y)
                  );
                  g.DrawRectangle(penRef, refRect);
                  g.DrawString("Screenshot", font, textBrush, refRect.Left + 4, Math.Max(refRect.Top - 22, 4));
               }

               // --- DYNAMICKÁ TEČKA před třetím klikem (clickPhase == 2) ---
               if (clickPhase == 2 && currentMouse.HasValue && !firstSample.HasValue)
               {
                  var p = currentMouse.Value;
                  g.FillEllipse(brushPoint, p.X - 4, p.Y - 4, 8, 8);
                  //g.DrawString("Sample start (preview)", font, textBrush, p.X + 8, p.Y - 10);
               }

               // === SAMPLE obdélník náhled (fáze 3: máme firstSample, druhý bod je myš) ===
               if (firstSample.HasValue && (secondSample.HasValue || (clickPhase == 3 && currentMouse.HasValue)))
               {
                  var second = secondSample ?? currentMouse.Value;
                  Rectangle sampleRect = Rectangle.FromLTRB(
                      Math.Min(firstSample.Value.X, second.X),
                      Math.Min(firstSample.Value.Y, second.Y),
                      Math.Max(firstSample.Value.X, second.X),
                      Math.Max(firstSample.Value.Y, second.Y)
                  );
                  g.DrawRectangle(penSample, sampleRect);
                  g.DrawString("Sample", font, textBrush, sampleRect.Left + 4, Math.Max(sampleRect.Top - 22, 4));
               }

               // --- DYNAMICKÁ TEČKA pro searchStart (clickPhase == 4) ---
               if (clickPhase == 4 && currentMouse.HasValue && !searchStart.HasValue)
               {
                  var p = currentMouse.Value;
                  //g.DrawEllipse(penSearch, p.X - 6, p.Y - 6, 12, 12);
                  g.FillEllipse(brushPoint, p.X - 3, p.Y - 3, 6, 6);
                  //g.DrawString("Search start (preview)", font, textBrush, p.X + 8, p.Y - 10);
               }

               // --- UZAMČENÉ BOD Y (po kliknutí) ---
               if (firstRef.HasValue)
                  g.FillEllipse(brushPoint, firstRef.Value.X - 3, firstRef.Value.Y - 3, 6, 6);
               if (secondRef.HasValue)
                  g.FillEllipse(brushPoint, secondRef.Value.X - 3, secondRef.Value.Y - 3, 6, 6);
               if (firstSample.HasValue)
                  g.FillEllipse(brushPoint, firstSample.Value.X - 3, firstSample.Value.Y - 3, 6, 6);
               if (secondSample.HasValue)
                  g.FillEllipse(brushPoint, secondSample.Value.X - 3, secondSample.Value.Y - 3, 6, 6);
               if (searchStart.HasValue)
               {
                  g.DrawEllipse(penSearch, searchStart.Value.X - 6, searchStart.Value.Y - 6, 12, 12);
                  g.FillEllipse(brushPoint, searchStart.Value.X - 3, searchStart.Value.Y - 3, 6, 6);
               }

               // --- Stavová nápověda v rohu ---
               string phaseText = clickPhase switch
               {
                  0 => "Krok 1: Klikni pro začátek Screenshot oblasti",
                  1 => "Krok 2: Klikni pro konec Screenshot oblasti",
                  2 => "Krok 3: Klikni pro začátek SAMPLE oblasti",
                  3 => "Krok 4: Klikni pro konec SAMPLE oblasti",
                  4 => "Krok 5: Klikni pro Start SEARCH bod",
                  _ => ""
               };
               if (!string.IsNullOrEmpty(phaseText))
               {
                  var shadowBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0));
                  g.DrawString(phaseText, font, shadowBrush, 21, 21);
                  g.DrawString(phaseText, font, Brushes.Yellow, 20, 20);
                  shadowBrush.Dispose();
               }
            }

            // --- aktualizace layered okna ---
            ApplyLayerBitmap(bmp);
         }
      }

      private void ApplyLayerBitmap(Bitmap bmp)
      {
         IntPtr screenDc = GetDC(IntPtr.Zero);
         IntPtr memDc = CreateCompatibleDC(screenDc);
         IntPtr hBitmap = bmp.GetHbitmap(Color.FromArgb(0));
         IntPtr oldBitmap = SelectObject(memDc, hBitmap);

         POINT topPos = new POINT(Left, Top);
         SIZE size = new SIZE(bmp.Width, bmp.Height);
         POINT pointSource = new POINT(0, 0);

         BLENDFUNCTION blend = new BLENDFUNCTION
         {
            BlendOp = AC_SRC_OVER,
            BlendFlags = 0,
            SourceConstantAlpha = 255,
            AlphaFormat = AC_SRC_ALPHA
         };

         UpdateLayeredWindow(this.Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, ULW_ALPHA);

         SelectObject(memDc, oldBitmap);
         DeleteObject(hBitmap);
         DeleteDC(memDc);
         ReleaseDC(IntPtr.Zero, screenDc);
      }

      #endregion

      #endregion

      private void btnSaveSelectedBrowserOneTabUrl_Click(object sender, EventArgs e)
      {
         //save to db
      }

      private void cmbSelectedBrowser_SelectedIndexChanged(object sender, EventArgs e)
      {
         tbOneTabUrl.Clear(); //basic - then load from db
      }

      private void btnConnectionTest_Click(object sender, EventArgs e)
      {
         DB_Access.ConnectionTest();
      }
   }
}