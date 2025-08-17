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
      }

      private void btnOrder_Click(object sender, EventArgs e)
      {
         OrderTabs();
      }

      private void OrderTabs()
      {
         Tabs.AddTabs(rtbText.Text);
         Tabs.OrderTabs();
         Tabs.RemoveDuplicates();
         rtbText.Clear();

         foreach (var group in Tabs.GroupTabs()) //zavolá se pouze jednou, na začátku foreach
         {
            foreach (var tab in group)
            {
               rtbText.AppendText($"{tab.Url} | {tab.Description}\n");
            }
            rtbText.AppendText("\n"); //mezera mezi skupinami
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


   }
}
