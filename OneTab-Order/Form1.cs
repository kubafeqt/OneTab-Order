using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using static System.Windows.Forms.LinkLabel;

#region comments

//check for installed -> notepad++, pspad, ... , or notepad -> basic now (just notepad)
//can be extracted to txt, html with <a href=""> (+list for copy) -> basic now (txt)
//remove duplicates from extracted, even in rtbText -> working
//searching - builded in finder - get shortcut ctrl+f -> top now
//
//delete all from and after - &/?pp=, &/?utm, &/?fbclid, - check other tracking queries -> remove tracking queries checkbox
//
//

#endregion

namespace OneTab_Order
{
   public partial class Form1 : Form
   {
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
      }

      private void LoadPanelsSizeAndLocation()
      {
         foreach (Panel panel in Controls.OfType<Panel>())
         {
            panel.Size = Settings.panelSize;
            panel.Location = Settings.panelLocation;
         }
      }

      #region switch panels
      private void SwitchPanelVisible(Panel panelToShow)
      {
         Dictionary<Panel, Button> panelButtonMap = new Dictionary<Panel, Button>()
         {
            { panelMain, btnMainPanel },
            { panelRPASettings, btnRPASettings }
         };

         foreach (Panel panel in Controls.OfType<Panel>())
         {
            panel.Visible = false;
            panelButtonMap[panel].Enabled = true;
         }
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

      /// <summary>
      /// Timer for buttons enable/disable
      /// </summary>
      private void Timer_Tick(object sender, EventArgs e)
      {
         if (string.IsNullOrWhiteSpace(rtbText.Text))
         {
            SwitchButtonEnabled(btnOrder, false);
            SwitchButtonEnabled(btnSaveToFile, false);
            SwitchButtonEnabled(btnExtractWebpages, false);
            SwitchButtonEnabled(btnCopyAllRtb, false);
         }
         else //rtbText is not empty
         {
            if (Clipboard.ContainsText()) //for button copy to clipboard enable/disable
            {
               string clipboarText = Clipboard.GetText().Trim();
               if (rtbText.Text != clipboarText && !string.IsNullOrWhiteSpace(clipboarText))
               {
                  SwitchButtonEnabled(btnCopyAllRtb, true);
               }
               else
               {
                  SwitchButtonEnabled(btnCopyAllRtb, false);
               }
            }
            SwitchButtonEnabled(btnOrder, true);
            SwitchButtonEnabled(btnSaveToFile, true);
            if (!string.IsNullOrWhiteSpace(tbExtractWebpages.Text))
            {
               SwitchButtonEnabled(btnExtractWebpages, true);
            }
            else
            { 
               SwitchButtonEnabled(btnExtractWebpages, false);
            }
         }
      }

      private void SwitchButtonEnabled(Button button, bool enable) => button.Enabled = enable;

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

      #region searching text
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

      private void Form1_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.Control && e.KeyCode == Keys.F) // Detect Ctrl+F
         {
            tbFind.Focus();
            tbFind.SelectAll();
            e.SuppressKeyPress = true; // Prevent the default behavior
         }
      }

      #endregion


   }
}