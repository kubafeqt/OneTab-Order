using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;

#region comments

//check for installed -> notepad++, pspad, ... , or notepad -> basic now
//can be extracted to txt, html with <a href=""> (+list for copy) -> basic now (txt)
//remove duplicates from extracted, even in rtbText -> basic now (not rtbText)
//searching - builded in finder - get shortcut ctrl+f -> basic now
//
//delete all from and after - &/?pp=, &/?utm, &/?fbclid, - check other tracking queries -> delete tracking queries checkbox
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
         timer.Interval = 100; //100 ms
         timer.Tick += Timer_Tick;
         timer.Start();
         cmbRemoveDuplicatesExtractedType.Items.AddRange(new string[] { "from below", "from up" });
         cmbRemoveDuplicatesExtractedType.SelectedIndex = 0;
         cmbRemoveDuplicatesExtractedType.Enabled = cboxRemoveDuplicatesExtracted.Checked;
         cboxRemoveDuplicatesFromUp.CheckedChanged += CboxDef_CheckedChanged;
         cboxRemoveDuplicatesFromBelow.CheckedChanged += CboxDef_CheckedChanged;

         cmbFindType.Items.AddRange(new string[] { "full line", "url only", "content only" });
         cmbFindType.SelectedIndex = 0;
      }

      private void ReAddTabs(ref bool notRemoveEmptyEntries)
      {
         notRemoveEmptyEntries = !notRemoveEmptyEntries ? cboxRemoveDuplicatesOnly.Checked : notRemoveEmptyEntries; //true = true
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

         if (!cboxRemoveDuplicatesOnly.Checked)
         {
            Tabs.OrderTabs();
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

         if (!notRemoveEmptyEntries)
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
         else
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

         lbRemovedDuplicates.Text = $"Removed duplicates: {Tabs.RemovedDuplicates}";
      }

      private void btnCopyAllRtb_Click(object sender, EventArgs e)
      {
         Clipboard.SetText(rtbText.Text);
         SwitchButtonEnabled(btnCopyAllRtb, false);
      }

      private void Timer_Tick(object sender, EventArgs e)
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
         string folderPath = MakeExportsPath();
         saveFileDialog.InitialDirectory = folderPath;
         if (saveFileDialog.ShowDialog() == DialogResult.OK)
         {
            File.WriteAllLines(saveFileDialog.FileName, lines);
            MessageBox.Show("Soubor byl úspěšně uložen.", "Hotovo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (cboxRemoveSitesFromDef.Checked)
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

      private void Form1_Paint(object sender, PaintEventArgs e)
      {
         Graphics gfx = e.Graphics;
         Pen penBlack = new Pen(Brushes.Black, 2);
         gfx.DrawLine(penBlack, 519, 0, 519, 200);
      }

      private void cboxRemoveDuplicatesOnly_CheckedChanged(object sender, EventArgs e)
      {
         btnOrder.Text = cboxRemoveDuplicatesOnly.Checked ? "remove" : "order";
      }

      private void btnOpenExtractedFolder_Click(object sender, EventArgs e)
      {
         string exportsPath = MakeExportsPath();
         var psi = new ProcessStartInfo
         {
            FileName = exportsPath,
            UseShellExecute = true
         };
         Process.Start(psi);
      }

      private string MakeExportsPath()
      {
         string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "exports");
         if (!Directory.Exists(folderPath))
         {
            Directory.CreateDirectory(folderPath);
         }
         return folderPath;
      }

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
         if (cmbFindType.SelectedItem.ToString() != "full line" && (!Tabs.TabsAdded || Tabs.TabsChanged))
         {
            bool notRemoveEmptyEntries = false;
            ReAddTabs(ref notRemoveEmptyEntries);
         }
         if (cmbFindType.SelectedItem.ToString() == "url only")
         {

         }
         if (cmbFindType.SelectedItem.ToString() == "content only")
         {

         }
         if (cmbFindType.SelectedItem.ToString() == "full line")
         {
            // Najde výskyt až za poslední pozicí
            int index = rtbText.Find(searchText, lastSearchPosition, RichTextBoxFinds.None);

            if (index == -1 && lastSearchPosition > 0)
            {
               // Začít od začátku textu (cyklické hledání)
               index = rtbText.Find(searchText, 0, RichTextBoxFinds.None);
            }

            if (index != -1)
            {
               // Najde začátek a konec logického řádku (jako dřív)
               int lineStart = index;
               int lineEnd = index;

               while (lineStart > 0 && rtbText.Text[lineStart - 1] != '\r' && rtbText.Text[lineStart - 1] != '\n')
                  lineStart--;

               while (lineEnd < rtbText.Text.Length && rtbText.Text[lineEnd] != '\r' && rtbText.Text[lineEnd] != '\n')
                  lineEnd++;

               if (lineEnd < rtbText.Text.Length - 1 && rtbText.Text[lineEnd] == '\r' && rtbText.Text[lineEnd + 1] == '\n')
                  lineEnd += 2;
               else if (lineEnd < rtbText.Text.Length && (rtbText.Text[lineEnd] == '\r' || rtbText.Text[lineEnd] == '\n'))
                  lineEnd += 1;

               rtbText.Select(lineStart, lineEnd - lineStart);
               rtbText.ScrollToCaret();
               rtbText.Focus();

               // Nastaví lastSearchPosition na první znak za nalezeným textem
               lastSearchPosition = index + searchText.Length;
            }
            else
            {
               MessageBox.Show("Text nebyl nalezen.");
               lastSearchPosition = 0;
            }
         }
      }

      private void FindPrev(string searchText)
      {
         if (cmbFindType.SelectedItem.ToString() != "full line" && (!Tabs.TabsAdded || Tabs.TabsChanged))
         {
            bool notRemoveEmptyEntries = false;
            ReAddTabs(ref notRemoveEmptyEntries);
         }
         if (cmbFindType.SelectedItem.ToString() == "url only")
         {

         }
         if (cmbFindType.SelectedItem.ToString() == "content only")
         {

         }
         if (cmbFindType.SelectedItem.ToString() == "full line")
         {
            // Reset pozice při změně hledaného textu
            if (searchText != lastSearchText)
            {
               lastSearchText = searchText;
               lastSearchPosition = rtbText.TextLength;
            }

            // Najdi předchozí výskyt (od pozice před current lastSearchPosition)
            int start = Math.Max(0, lastSearchPosition - 1);
            int index = rtbText.Find(searchText, 0, start, RichTextBoxFinds.Reverse);

            if (index != -1)
            {
               // Najdi začátek a konec logického řádku (jako dříve)
               int lineStart = index;
               int lineEnd = index;

               while (lineStart > 0 && rtbText.Text[lineStart - 1] != '\r' && rtbText.Text[lineStart - 1] != '\n')
                  lineStart--;

               while (lineEnd < rtbText.Text.Length && rtbText.Text[lineEnd] != '\r' && rtbText.Text[lineEnd] != '\n')
                  lineEnd++;

               if (lineEnd < rtbText.Text.Length - 1 && rtbText.Text[lineEnd] == '\r' && rtbText.Text[lineEnd + 1] == '\n')
                  lineEnd += 2;
               else if (lineEnd < rtbText.Text.Length && (rtbText.Text[lineEnd] == '\r' || rtbText.Text[lineEnd] == '\n'))
                  lineEnd += 1;

               rtbText.Select(lineStart, lineEnd - lineStart);
               rtbText.ScrollToCaret();
               rtbText.Focus();

               lastSearchPosition = index;
            }
            else
            {
               MessageBox.Show("Text nebyl nalezen.");
               lastSearchPosition = rtbText.TextLength;
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
      }

      private void rtbText_KeyDown(object sender, KeyEventArgs e)
      {
         if (rtbText.SelectionLength > 0)
         {
            if (e.KeyCode == Keys.Enter)
            {
               Find();
               e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.Up) //not working correctly
            {
               Find(false);
               e.SuppressKeyPress = true;
            }
         }
      }

      private void rtbText_TextChanged(object sender, EventArgs e)
      {
         Tabs.TabsChanged = true;
      }
   }
}