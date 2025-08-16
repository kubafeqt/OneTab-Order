namespace OneTab_Order
{
   public partial class Form1 : Form
   {
      public Form1()
      {
         InitializeComponent();
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

         var groupedTabs = Tabs.TabList
    .GroupBy(t => new Uri(t.Url).Host);
         foreach (var group in groupedTabs)
         {
            foreach (var tab in group)
            {
               rtbText.AppendText($"{tab.Url} | {tab.Description}\n");
            }
            rtbText.AppendText("\n");// mezera mezi skupinami
         }

         lbRemovedDuplicates.Text = $"Removed duplicates: {Tabs.RemovedDuplicates}";
      }
   }
}
