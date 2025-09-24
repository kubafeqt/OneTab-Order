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
      }

      private void btnOrder_Click(object sender, EventArgs e)
      {
         OrderTabs();
      }

      private void OrderTabs()
      {
         bool notRemoveEmptyEntries = cboxRemoveDuplicatesOnly.Checked;
         Tabs.TabList.Clear(); //důležitý
         Tabs.AddTabs(rtbText.Text, notRemoveEmptyEntries);
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
         if (Clipboard.ContainsText())
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
         Tabs.TabList.Clear(); // důležité
         Tabs.AddTabs(rtbText.Text, notRemoveEmptyEntries);

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
      private void btnFindNext_Click(object sender, EventArgs e)
      {
         string searchText = tbFind.Text;


         if (string.IsNullOrEmpty(searchText))
            return;

         int index = rtbText.Find(searchText, lastSearchPosition, RichTextBoxFinds.None);

         if (index == -1 && lastSearchPosition > 0)
         {
            lastSearchPosition = 0;
            index = rtbText.Find(searchText, 0, RichTextBoxFinds.None);
         }

         if (index != -1)
         {
            // Najde začátek a konec logického řádku (včetně zalamování)
            int lineStart = index;
            int lineEnd = index;

            // Najde začátek řádku - jde zpět dokud nenarazí na \r nebo \n nebo začátek textu
            while (lineStart > 0 && rtbText.Text[lineStart - 1] != '\r' && rtbText.Text[lineStart - 1] != '\n')
            {
               lineStart--;
            }

            // Najde konec řádku - jde dopředu dokud nenarazí na \r nebo \n nebo konec textu
            while (lineEnd < rtbText.Text.Length && rtbText.Text[lineEnd] != '\r' && rtbText.Text[lineEnd] != '\n')
            {
               lineEnd++;
            }

            // Pokud je na konci \r\n, zahrne je do výběru
            if (lineEnd < rtbText.Text.Length - 1 && rtbText.Text[lineEnd] == '\r' && rtbText.Text[lineEnd + 1] == '\n')
            {
               lineEnd += 2; // \r\n
            }
            else if (lineEnd < rtbText.Text.Length && (rtbText.Text[lineEnd] == '\r' || rtbText.Text[lineEnd] == '\n'))
            {
               lineEnd += 1; // \r nebo \n
            }

            // Označí celý řádek
            rtbText.Select(lineStart, lineEnd - lineStart);
            rtbText.ScrollToCaret();
            rtbText.Focus();

            lastSearchPosition = index + searchText.Length;
         }
         else
         {
            MessageBox.Show("Text nebyl nalezen.");
            lastSearchPosition = 0;
         }
      }

      private void btnFindPrev_Click(object sender, EventArgs e)
      {
         string searchText = tbFind.Text;

         if (string.IsNullOrEmpty(searchText))
            return;

         // Najde všechny výskyty textu
         List<int> allPositions = new List<int>();
         int pos = 0;
         while ((pos = rtbText.Find(searchText, pos, RichTextBoxFinds.None)) != -1)
         {
            allPositions.Add(pos);
            pos += searchText.Length;
         }

         if (allPositions.Count == 0)
         {
            MessageBox.Show("Text nebyl nalezen.");
            return;
         }

         // Najde aktuální pozici v seznamu
         int currentIndex = -1;
         for (int i = 0; i < allPositions.Count; i++)
         {
            if (allPositions[i] >= lastSearchPosition)
            {
               currentIndex = i;
               break;
            }
         }

         // Určí předchozí pozici
         int prevIndex;
         if (currentIndex <= 0)
         {
            // Pokud jsme na začátku nebo před prvním, jde na poslední
            prevIndex = allPositions.Count - 1;
         }
         else
         {
            prevIndex = currentIndex - 1;
         }

         int index = allPositions[prevIndex];

         // Najde začátek a konec logického řádku (včetně zalamování)
         int lineStart = index;
         int lineEnd = index;

         // Najde začátek řádku - jde zpět dokud nenarazí na \r nebo \n nebo začátek textu
         while (lineStart > 0 && rtbText.Text[lineStart - 1] != '\r' && rtbText.Text[lineStart - 1] != '\n')
         {
            lineStart--;
         }

         // Najde konec řádku - jde dopředu dokud nenarazí na \r nebo \n nebo konec textu
         while (lineEnd < rtbText.Text.Length && rtbText.Text[lineEnd] != '\r' && rtbText.Text[lineEnd] != '\n')
         {
            lineEnd++;
         }

         // Pokud je na konci \r\n, zahrne je do výběru
         if (lineEnd < rtbText.Text.Length - 1 && rtbText.Text[lineEnd] == '\r' && rtbText.Text[lineEnd + 1] == '\n')
         {
            lineEnd += 2; // \r\n
         }
         else if (lineEnd < rtbText.Text.Length && (rtbText.Text[lineEnd] == '\r' || rtbText.Text[lineEnd] == '\n'))
         {
            lineEnd += 1; // \r nebo \n
         }

         // Označí celý řádek
         rtbText.Select(lineStart, lineEnd - lineStart);
         rtbText.ScrollToCaret();
         rtbText.Focus();

         // Nastaví pozici pro další hledání
         lastSearchPosition = index;
      }

      private void tbFind_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyCode == Keys.Enter)
         {
            btnFindNext.PerformClick();
            //tbFind.Focus();
            e.SuppressKeyPress = true; //prevents bump sound
         }
      }

      private void rtbText_KeyDown(object sender, KeyEventArgs e)
      {
         if (rtbText.SelectionLength > 0)
         {
            if (e.KeyCode == Keys.Enter)
            {
               btnFindNext.PerformClick();
               e.SuppressKeyPress = true;
            }
            //if (e.KeyCode == Keys.Up) //not working correctly
            //{
            //   btnFindPrev.PerformClick();
            //   e.SuppressKeyPress = true;
            //}
         }
      }
   }
}