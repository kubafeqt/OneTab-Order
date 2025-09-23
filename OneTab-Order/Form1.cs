using System.DirectoryServices.ActiveDirectory;

#region comments

//check for installed -> notepad++, pspad, ... , or notepad
//can be extracted to txt, html with <a href=""> (+list for copy)

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
         string[] webpagesUrls = tbExtractWebpages.Text.Trim().Split(';', StringSplitOptions.RemoveEmptyEntries) // rozdělí podle středníku a odstraní prázdné položky
                                                             .Select(url => url.Trim()) // ořízne mezery kolem každé položky
                                                             .Where(url => !string.IsNullOrEmpty(url) && url.Length > 1) // jistota, že nejsou prázdné řetězce po oříznutí
                                                             .Distinct() // odstraní duplicity
                                                             .ToArray(); // převede na pole

         var selectedTabs = Tabs.TabList.Where(tab => !string.IsNullOrWhiteSpace(tab.Url) 
                                                         && webpagesUrls.Any(url => tab.Url.Contains(url, StringComparison.OrdinalIgnoreCase))).ToList();

         // 3. Sestavení obsahu k uložení (například řádek = url)
         var lines = selectedTabs.Select(tab => tab.Url).ToList();

         // 4. Uložení do souboru (např. jako TXT)
         var saveFileDialog = new SaveFileDialog();
         saveFileDialog.Filter = "Textové soubory (*.txt)|*.txt|Všechny soubory (*.*)|*.*";
         saveFileDialog.Title = "Uložit extrahované weby";
         string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "exports");
         if (!Directory.Exists(folderPath))
         {
            Directory.CreateDirectory(folderPath);
         }
         saveFileDialog.InitialDirectory = folderPath;
         if (saveFileDialog.ShowDialog() == DialogResult.OK)
         {
            File.WriteAllLines(saveFileDialog.FileName, lines);
            MessageBox.Show("Soubor byl úspěšně uložen.", "Hotovo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (cboxRemoveSitesFromDef.Checked)
            {
               // Odeber vyexportované URL z rtbText
               // Rozděl text do pole řádek po řádku
               var allLines = rtbText.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                                          .Select(row => row.Trim())
                                          .Where(row => !string.IsNullOrEmpty(row))
                                          .ToList();

               // Smaž ty řádky, které jsou mezi vyexportovanými URL (ignoruje diakritiku a porovnává trimmed)
               var remainingLines = allLines.Where(row => !lines.Contains(row)).ToList();

               // Přepiš rtbText.Text jen zbytkem
               rtbText.Text = string.Join(Environment.NewLine, remainingLines);
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


   }
}