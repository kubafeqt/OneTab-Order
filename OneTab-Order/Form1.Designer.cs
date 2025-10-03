namespace OneTab_Order
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

      #region Windows Form Designer generated code

      /// <summary>
      ///  Required method for Designer support - do not modify
      ///  the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         rtbText = new RichTextBox();
         btnOrder = new Button();
         lbRemovedDuplicates = new Label();
         btnCopyAllRtb = new Button();
         cboxRemoveDuplicatesFromUp = new CheckBox();
         cboxRemoveDuplicatesFromBelow = new CheckBox();
         tbExtractWebpages = new TextBox();
         lbExtractedWebpages = new Label();
         cboxRemoveDuplicatesExtracted = new CheckBox();
         cmbRemoveDuplicatesExtractedType = new ComboBox();
         cboxOpenExtractedFile = new CheckBox();
         btnExtractWebpages = new Button();
         cboxRemoveOnly = new CheckBox();
         btnOpenExtractedFolder = new Button();
         tbFind = new TextBox();
         lbFind = new Label();
         btnFindPrev = new Button();
         btnFindNext = new Button();
         lbFindBtn = new Label();
         cboxRemoveSitesFromDef = new CheckBox();
         cboxRemoveTrackingQueries = new CheckBox();
         cboxExtractedRemoveTrackingQueries = new CheckBox();
         lbExtracted = new Label();
         cmbFindType = new ComboBox();
         lbFindType = new Label();
         lbTrackingQueriesRemoved = new Label();
         btnSaveToFile = new Button();
         btnOpenSavedFolder = new Button();
         panelMain = new Panel();
         panelRPASettings = new Panel();
         btnMainPanel = new Button();
         btnRPASettings = new Button();
         tbOneTabUrl = new TextBox();
         lbOneTabUrl = new Label();
         cmbSelectedBrowser = new ComboBox();
         lbSelectedBrowser = new Label();
         btnSaveSelectedBrowserOneTabUrl = new Button();
         btnOpenSelectedBrowserOnOneTabUrl = new Button();
         panelMain.SuspendLayout();
         panelRPASettings.SuspendLayout();
         SuspendLayout();
         // 
         // rtbText
         // 
         rtbText.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
         rtbText.Location = new Point(7, 105);
         rtbText.Name = "rtbText";
         rtbText.Size = new Size(1071, 631);
         rtbText.TabIndex = 0;
         rtbText.Text = "";
         rtbText.TextChanged += rtbText_TextChanged;
         rtbText.KeyDown += rtbText_KeyDown;
         // 
         // btnOrder
         // 
         btnOrder.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnOrder.Location = new Point(20, 56);
         btnOrder.Name = "btnOrder";
         btnOrder.Size = new Size(94, 35);
         btnOrder.TabIndex = 1;
         btnOrder.Text = "order";
         btnOrder.UseVisualStyleBackColor = true;
         btnOrder.Click += btnOrder_Click;
         // 
         // lbRemovedDuplicates
         // 
         lbRemovedDuplicates.AutoSize = true;
         lbRemovedDuplicates.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbRemovedDuplicates.Location = new Point(533, 56);
         lbRemovedDuplicates.Name = "lbRemovedDuplicates";
         lbRemovedDuplicates.Size = new Size(167, 21);
         lbRemovedDuplicates.TabIndex = 2;
         lbRemovedDuplicates.Text = "Removed duplicates: ";
         // 
         // btnCopyAllRtb
         // 
         btnCopyAllRtb.Enabled = false;
         btnCopyAllRtb.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnCopyAllRtb.Location = new Point(126, 57);
         btnCopyAllRtb.Name = "btnCopyAllRtb";
         btnCopyAllRtb.Size = new Size(94, 35);
         btnCopyAllRtb.TabIndex = 2;
         btnCopyAllRtb.Text = "copy text";
         btnCopyAllRtb.UseVisualStyleBackColor = true;
         btnCopyAllRtb.Click += btnCopyAllRtb_Click;
         // 
         // cboxRemoveDuplicatesFromUp
         // 
         cboxRemoveDuplicatesFromUp.AutoSize = true;
         cboxRemoveDuplicatesFromUp.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         cboxRemoveDuplicatesFromUp.Location = new Point(248, 50);
         cboxRemoveDuplicatesFromUp.Name = "cboxRemoveDuplicatesFromUp";
         cboxRemoveDuplicatesFromUp.Size = new Size(210, 23);
         cboxRemoveDuplicatesFromUp.TabIndex = 5;
         cboxRemoveDuplicatesFromUp.Text = "remove duplicates from up";
         cboxRemoveDuplicatesFromUp.UseVisualStyleBackColor = true;
         // 
         // cboxRemoveDuplicatesFromBelow
         // 
         cboxRemoveDuplicatesFromBelow.AutoSize = true;
         cboxRemoveDuplicatesFromBelow.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         cboxRemoveDuplicatesFromBelow.Location = new Point(248, 76);
         cboxRemoveDuplicatesFromBelow.Name = "cboxRemoveDuplicatesFromBelow";
         cboxRemoveDuplicatesFromBelow.Size = new Size(234, 23);
         cboxRemoveDuplicatesFromBelow.TabIndex = 6;
         cboxRemoveDuplicatesFromBelow.Text = "remove duplicates from below";
         cboxRemoveDuplicatesFromBelow.UseVisualStyleBackColor = true;
         // 
         // tbExtractWebpages
         // 
         tbExtractWebpages.Location = new Point(533, 31);
         tbExtractWebpages.Name = "tbExtractWebpages";
         tbExtractWebpages.Size = new Size(734, 23);
         tbExtractWebpages.TabIndex = 7;
         tbExtractWebpages.TextChanged += tbExtractWebpages_TextChanged;
         // 
         // lbExtractedWebpages
         // 
         lbExtractedWebpages.AutoSize = true;
         lbExtractedWebpages.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbExtractedWebpages.Location = new Point(533, 4);
         lbExtractedWebpages.Name = "lbExtractedWebpages";
         lbExtractedWebpages.Size = new Size(143, 21);
         lbExtractedWebpages.TabIndex = 8;
         lbExtractedWebpages.Text = "Extract webpages:";
         // 
         // cboxRemoveDuplicatesExtracted
         // 
         cboxRemoveDuplicatesExtracted.AutoSize = true;
         cboxRemoveDuplicatesExtracted.Checked = true;
         cboxRemoveDuplicatesExtracted.CheckState = CheckState.Checked;
         cboxRemoveDuplicatesExtracted.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         cboxRemoveDuplicatesExtracted.Location = new Point(850, 2);
         cboxRemoveDuplicatesExtracted.Name = "cboxRemoveDuplicatesExtracted";
         cboxRemoveDuplicatesExtracted.Size = new Size(160, 23);
         cboxRemoveDuplicatesExtracted.TabIndex = 8;
         cboxRemoveDuplicatesExtracted.Text = "Remove Duplicates:";
         cboxRemoveDuplicatesExtracted.UseVisualStyleBackColor = true;
         cboxRemoveDuplicatesExtracted.CheckedChanged += cboxRemoveDuplicatesExtracted_CheckedChanged;
         // 
         // cmbRemoveDuplicatesExtractedType
         // 
         cmbRemoveDuplicatesExtractedType.DropDownStyle = ComboBoxStyle.DropDownList;
         cmbRemoveDuplicatesExtractedType.FormattingEnabled = true;
         cmbRemoveDuplicatesExtractedType.Location = new Point(1012, 2);
         cmbRemoveDuplicatesExtractedType.Name = "cmbRemoveDuplicatesExtractedType";
         cmbRemoveDuplicatesExtractedType.Size = new Size(178, 23);
         cmbRemoveDuplicatesExtractedType.TabIndex = 11;
         // 
         // cboxOpenExtractedFile
         // 
         cboxOpenExtractedFile.AutoSize = true;
         cboxOpenExtractedFile.Checked = true;
         cboxOpenExtractedFile.CheckState = CheckState.Checked;
         cboxOpenExtractedFile.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         cboxOpenExtractedFile.Location = new Point(1097, 59);
         cboxOpenExtractedFile.Name = "cboxOpenExtractedFile";
         cboxOpenExtractedFile.Size = new Size(156, 23);
         cboxOpenExtractedFile.TabIndex = 10;
         cboxOpenExtractedFile.Text = "Open Extracted file";
         cboxOpenExtractedFile.UseVisualStyleBackColor = true;
         // 
         // btnExtractWebpages
         // 
         btnExtractWebpages.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnExtractWebpages.Location = new Point(1196, 0);
         btnExtractWebpages.Name = "btnExtractWebpages";
         btnExtractWebpages.Size = new Size(68, 26);
         btnExtractWebpages.TabIndex = 12;
         btnExtractWebpages.Text = "Extract";
         btnExtractWebpages.UseVisualStyleBackColor = true;
         btnExtractWebpages.Click += btnExtractWebpages_Click;
         // 
         // cboxRemoveOnly
         // 
         cboxRemoveOnly.AutoSize = true;
         cboxRemoveOnly.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         cboxRemoveOnly.Location = new Point(248, 1);
         cboxRemoveOnly.Name = "cboxRemoveOnly";
         cboxRemoveOnly.Size = new Size(113, 23);
         cboxRemoveOnly.TabIndex = 3;
         cboxRemoveOnly.Text = "remove only";
         cboxRemoveOnly.UseVisualStyleBackColor = true;
         cboxRemoveOnly.CheckedChanged += cboxRemoveDuplicatesOnly_CheckedChanged;
         // 
         // btnOpenExtractedFolder
         // 
         btnOpenExtractedFolder.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnOpenExtractedFolder.Location = new Point(1093, 99);
         btnOpenExtractedFolder.Name = "btnOpenExtractedFolder";
         btnOpenExtractedFolder.Size = new Size(167, 26);
         btnOpenExtractedFolder.TabIndex = 14;
         btnOpenExtractedFolder.Text = "Open Extracted Folder";
         btnOpenExtractedFolder.UseVisualStyleBackColor = true;
         btnOpenExtractedFolder.Click += btnOpenExtractedFolder_Click;
         // 
         // tbFind
         // 
         tbFind.Location = new Point(1090, 226);
         tbFind.Name = "tbFind";
         tbFind.Size = new Size(186, 23);
         tbFind.TabIndex = 16;
         tbFind.KeyDown += tbFind_KeyDown;
         // 
         // lbFind
         // 
         lbFind.AutoSize = true;
         lbFind.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbFind.Location = new Point(1084, 202);
         lbFind.Name = "lbFind";
         lbFind.Size = new Size(45, 21);
         lbFind.TabIndex = 17;
         lbFind.Text = "Find:";
         // 
         // btnFindPrev
         // 
         btnFindPrev.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnFindPrev.Location = new Point(1214, 255);
         btnFindPrev.Name = "btnFindPrev";
         btnFindPrev.Size = new Size(62, 26);
         btnFindPrev.TabIndex = 18;
         btnFindPrev.Text = "Prev";
         btnFindPrev.UseVisualStyleBackColor = true;
         btnFindPrev.Click += btnFindPrev_Click;
         // 
         // btnFindNext
         // 
         btnFindNext.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnFindNext.Location = new Point(1146, 255);
         btnFindNext.Name = "btnFindNext";
         btnFindNext.Size = new Size(62, 26);
         btnFindNext.TabIndex = 17;
         btnFindNext.Text = "Next";
         btnFindNext.UseVisualStyleBackColor = true;
         btnFindNext.Click += btnFindNext_Click;
         // 
         // lbFindBtn
         // 
         lbFindBtn.AutoSize = true;
         lbFindBtn.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbFindBtn.Location = new Point(1095, 257);
         lbFindBtn.Name = "lbFindBtn";
         lbFindBtn.Size = new Size(45, 21);
         lbFindBtn.TabIndex = 20;
         lbFindBtn.Text = "Find:";
         // 
         // cboxRemoveSitesFromDef
         // 
         cboxRemoveSitesFromDef.AutoSize = true;
         cboxRemoveSitesFromDef.Checked = true;
         cboxRemoveSitesFromDef.CheckState = CheckState.Checked;
         cboxRemoveSitesFromDef.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         cboxRemoveSitesFromDef.Location = new Point(869, 58);
         cboxRemoveSitesFromDef.Name = "cboxRemoveSitesFromDef";
         cboxRemoveSitesFromDef.Size = new Size(209, 23);
         cboxRemoveSitesFromDef.TabIndex = 9;
         cboxRemoveSitesFromDef.Text = "Remove Sites From Default";
         cboxRemoveSitesFromDef.UseVisualStyleBackColor = true;
         // 
         // cboxRemoveTrackingQueries
         // 
         cboxRemoveTrackingQueries.AutoSize = true;
         cboxRemoveTrackingQueries.Checked = true;
         cboxRemoveTrackingQueries.CheckState = CheckState.Checked;
         cboxRemoveTrackingQueries.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         cboxRemoveTrackingQueries.Location = new Point(248, 25);
         cboxRemoveTrackingQueries.Name = "cboxRemoveTrackingQueries";
         cboxRemoveTrackingQueries.Size = new Size(192, 23);
         cboxRemoveTrackingQueries.TabIndex = 4;
         cboxRemoveTrackingQueries.Text = "remove tracking queries";
         cboxRemoveTrackingQueries.UseVisualStyleBackColor = true;
         // 
         // cboxExtractedRemoveTrackingQueries
         // 
         cboxExtractedRemoveTrackingQueries.AutoSize = true;
         cboxExtractedRemoveTrackingQueries.Checked = true;
         cboxExtractedRemoveTrackingQueries.CheckState = CheckState.Checked;
         cboxExtractedRemoveTrackingQueries.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         cboxExtractedRemoveTrackingQueries.Location = new Point(1088, 164);
         cboxExtractedRemoveTrackingQueries.Name = "cboxExtractedRemoveTrackingQueries";
         cboxExtractedRemoveTrackingQueries.Size = new Size(192, 23);
         cboxExtractedRemoveTrackingQueries.TabIndex = 15;
         cboxExtractedRemoveTrackingQueries.Text = "remove tracking queries";
         cboxExtractedRemoveTrackingQueries.UseVisualStyleBackColor = true;
         // 
         // lbExtracted
         // 
         lbExtracted.AutoSize = true;
         lbExtracted.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbExtracted.Location = new Point(1088, 140);
         lbExtracted.Name = "lbExtracted";
         lbExtracted.Size = new Size(83, 21);
         lbExtracted.TabIndex = 23;
         lbExtracted.Text = "Extracted:";
         // 
         // cmbFindType
         // 
         cmbFindType.DropDownStyle = ComboBoxStyle.DropDownList;
         cmbFindType.FormattingEnabled = true;
         cmbFindType.Location = new Point(1090, 318);
         cmbFindType.Name = "cmbFindType";
         cmbFindType.Size = new Size(186, 23);
         cmbFindType.TabIndex = 19;
         // 
         // lbFindType
         // 
         lbFindType.AutoSize = true;
         lbFindType.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbFindType.Location = new Point(1084, 290);
         lbFindType.Name = "lbFindType";
         lbFindType.Size = new Size(84, 21);
         lbFindType.TabIndex = 25;
         lbFindType.Text = "Find Type:";
         // 
         // lbTrackingQueriesRemoved
         // 
         lbTrackingQueriesRemoved.AutoSize = true;
         lbTrackingQueriesRemoved.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbTrackingQueriesRemoved.Location = new Point(533, 78);
         lbTrackingQueriesRemoved.Name = "lbTrackingQueriesRemoved";
         lbTrackingQueriesRemoved.Size = new Size(203, 21);
         lbTrackingQueriesRemoved.TabIndex = 26;
         lbTrackingQueriesRemoved.Text = "Tracking queries removed:";
         // 
         // btnSaveToFile
         // 
         btnSaveToFile.Enabled = false;
         btnSaveToFile.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnSaveToFile.Location = new Point(126, 9);
         btnSaveToFile.Name = "btnSaveToFile";
         btnSaveToFile.Size = new Size(94, 39);
         btnSaveToFile.TabIndex = 27;
         btnSaveToFile.Text = "save to file";
         btnSaveToFile.UseVisualStyleBackColor = true;
         btnSaveToFile.Click += btnSaveToFile_Click;
         // 
         // btnOpenSavedFolder
         // 
         btnOpenSavedFolder.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
         btnOpenSavedFolder.Location = new Point(20, 7);
         btnOpenSavedFolder.Name = "btnOpenSavedFolder";
         btnOpenSavedFolder.Size = new Size(94, 41);
         btnOpenSavedFolder.TabIndex = 28;
         btnOpenSavedFolder.Text = "open saved folder";
         btnOpenSavedFolder.UseVisualStyleBackColor = true;
         btnOpenSavedFolder.Click += btnOpenSavedFolder_Click;
         // 
         // panelMain
         // 
         panelMain.Controls.Add(btnOpenSavedFolder);
         panelMain.Controls.Add(rtbText);
         panelMain.Controls.Add(btnSaveToFile);
         panelMain.Controls.Add(btnOrder);
         panelMain.Controls.Add(lbTrackingQueriesRemoved);
         panelMain.Controls.Add(lbRemovedDuplicates);
         panelMain.Controls.Add(lbFindType);
         panelMain.Controls.Add(btnCopyAllRtb);
         panelMain.Controls.Add(cmbFindType);
         panelMain.Controls.Add(cboxRemoveDuplicatesFromUp);
         panelMain.Controls.Add(lbExtracted);
         panelMain.Controls.Add(cboxRemoveDuplicatesFromBelow);
         panelMain.Controls.Add(cboxExtractedRemoveTrackingQueries);
         panelMain.Controls.Add(tbExtractWebpages);
         panelMain.Controls.Add(cboxRemoveTrackingQueries);
         panelMain.Controls.Add(lbExtractedWebpages);
         panelMain.Controls.Add(lbFindBtn);
         panelMain.Controls.Add(cboxRemoveDuplicatesExtracted);
         panelMain.Controls.Add(btnFindNext);
         panelMain.Controls.Add(cmbRemoveDuplicatesExtractedType);
         panelMain.Controls.Add(btnFindPrev);
         panelMain.Controls.Add(cboxOpenExtractedFile);
         panelMain.Controls.Add(lbFind);
         panelMain.Controls.Add(btnExtractWebpages);
         panelMain.Controls.Add(tbFind);
         panelMain.Controls.Add(cboxRemoveOnly);
         panelMain.Controls.Add(btnOpenExtractedFolder);
         panelMain.Controls.Add(cboxRemoveSitesFromDef);
         panelMain.Location = new Point(12, 32);
         panelMain.Name = "panelMain";
         panelMain.Size = new Size(66, 69);
         panelMain.TabIndex = 29;
         panelMain.Paint += panelMain_Paint;
         // 
         // panelRPASettings
         // 
         panelRPASettings.Controls.Add(btnOpenSelectedBrowserOnOneTabUrl);
         panelRPASettings.Controls.Add(btnSaveSelectedBrowserOneTabUrl);
         panelRPASettings.Controls.Add(lbSelectedBrowser);
         panelRPASettings.Controls.Add(cmbSelectedBrowser);
         panelRPASettings.Controls.Add(lbOneTabUrl);
         panelRPASettings.Controls.Add(tbOneTabUrl);
         panelRPASettings.Location = new Point(84, 32);
         panelRPASettings.Name = "panelRPASettings";
         panelRPASettings.Size = new Size(1204, 736);
         panelRPASettings.TabIndex = 30;
         // 
         // btnMainPanel
         // 
         btnMainPanel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnMainPanel.Location = new Point(18, 2);
         btnMainPanel.Name = "btnMainPanel";
         btnMainPanel.Size = new Size(108, 27);
         btnMainPanel.TabIndex = 31;
         btnMainPanel.Text = "Main Panel";
         btnMainPanel.UseVisualStyleBackColor = true;
         btnMainPanel.Click += btnMainPanel_Click;
         // 
         // btnRPASettings
         // 
         btnRPASettings.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnRPASettings.Location = new Point(132, 2);
         btnRPASettings.Name = "btnRPASettings";
         btnRPASettings.Size = new Size(108, 27);
         btnRPASettings.TabIndex = 32;
         btnRPASettings.Text = "RPA Settings";
         btnRPASettings.UseVisualStyleBackColor = true;
         btnRPASettings.Click += btnRPASettings_Click;
         // 
         // tbOneTabUrl
         // 
         tbOneTabUrl.Location = new Point(101, 11);
         tbOneTabUrl.Name = "tbOneTabUrl";
         tbOneTabUrl.Size = new Size(431, 23);
         tbOneTabUrl.TabIndex = 0;
         // 
         // lbOneTabUrl
         // 
         lbOneTabUrl.AutoSize = true;
         lbOneTabUrl.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbOneTabUrl.Location = new Point(15, 11);
         lbOneTabUrl.Name = "lbOneTabUrl";
         lbOneTabUrl.Size = new Size(80, 17);
         lbOneTabUrl.TabIndex = 1;
         lbOneTabUrl.Text = "OneTab url:";
         // 
         // cmbSelectedBrowser
         // 
         cmbSelectedBrowser.DropDownStyle = ComboBoxStyle.DropDownList;
         cmbSelectedBrowser.FormattingEnabled = true;
         cmbSelectedBrowser.Location = new Point(137, 48);
         cmbSelectedBrowser.Name = "cmbSelectedBrowser";
         cmbSelectedBrowser.Size = new Size(232, 23);
         cmbSelectedBrowser.TabIndex = 2;
         // 
         // lbSelectedBrowser
         // 
         lbSelectedBrowser.AutoSize = true;
         lbSelectedBrowser.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbSelectedBrowser.Location = new Point(14, 52);
         lbSelectedBrowser.Name = "lbSelectedBrowser";
         lbSelectedBrowser.Size = new Size(117, 17);
         lbSelectedBrowser.TabIndex = 3;
         lbSelectedBrowser.Text = "Selected browser:";
         // 
         // btnSaveSelectedBrowserOneTabUrl
         // 
         btnSaveSelectedBrowserOneTabUrl.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
         btnSaveSelectedBrowserOneTabUrl.Location = new Point(375, 47);
         btnSaveSelectedBrowserOneTabUrl.Name = "btnSaveSelectedBrowserOneTabUrl";
         btnSaveSelectedBrowserOneTabUrl.Size = new Size(75, 25);
         btnSaveSelectedBrowserOneTabUrl.TabIndex = 4;
         btnSaveSelectedBrowserOneTabUrl.Text = "Save";
         btnSaveSelectedBrowserOneTabUrl.UseVisualStyleBackColor = true;
         // 
         // btnOpenSelectedBrowserOnOneTabUrl
         // 
         btnOpenSelectedBrowserOnOneTabUrl.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
         btnOpenSelectedBrowserOnOneTabUrl.Location = new Point(457, 46);
         btnOpenSelectedBrowserOnOneTabUrl.Name = "btnOpenSelectedBrowserOnOneTabUrl";
         btnOpenSelectedBrowserOnOneTabUrl.Size = new Size(75, 26);
         btnOpenSelectedBrowserOnOneTabUrl.TabIndex = 5;
         btnOpenSelectedBrowserOnOneTabUrl.Text = "Open";
         btnOpenSelectedBrowserOnOneTabUrl.UseVisualStyleBackColor = true;
         btnOpenSelectedBrowserOnOneTabUrl.Click += btnOpenSelectedBrowserOnOneTabUrl_Click;
         // 
         // Form1
         // 
         AutoScaleDimensions = new SizeF(7F, 15F);
         AutoScaleMode = AutoScaleMode.Font;
         ClientSize = new Size(1303, 781);
         Controls.Add(btnRPASettings);
         Controls.Add(btnMainPanel);
         Controls.Add(panelRPASettings);
         Controls.Add(panelMain);
         FormBorderStyle = FormBorderStyle.FixedSingle;
         MaximizeBox = false;
         Name = "Form1";
         Text = "OneTab order";
         Paint += Form1_Paint;
         KeyDown += Form1_KeyDown;
         panelMain.ResumeLayout(false);
         panelMain.PerformLayout();
         panelRPASettings.ResumeLayout(false);
         panelRPASettings.PerformLayout();
         ResumeLayout(false);
      }

      #endregion

      private RichTextBox rtbText;
      private Button btnOrder;
      private Label lbRemovedDuplicates;
      private Button btnCopyAllRtb;
      private CheckBox cboxRemoveDuplicatesFromUp;
      private CheckBox cboxRemoveDuplicatesFromBelow;
      private TextBox tbExtractWebpages;
      private Label lbExtractedWebpages;
      private CheckBox cboxRemoveDuplicatesExtracted;
      private ComboBox cmbRemoveDuplicatesExtractedType;
      private CheckBox cboxOpenExtractedFile;
      private Button btnExtractWebpages;
      private CheckBox cboxRemoveOnly;
      private Button btnOpenExtractedFolder;
      private TextBox tbFind;
      private Label lbFind;
      private Button btnFindPrev;
      private Button btnFindNext;
      private Label lbFindBtn;
      private CheckBox cboxRemoveSitesFromDef;
      private CheckBox cboxRemoveTrackingQueries;
      private CheckBox cboxExtractedRemoveTrackingQueries;
      private Label lbExtracted;
      private ComboBox cmbFindType;
      private Label lbFindType;
      private Label lbTrackingQueriesRemoved;
      private Button btnSaveToFile;
      private Button btnOpenSavedFolder;
      private Panel panelMain;
      private Panel panelRPASettings;
      private Button btnMainPanel;
      private Button btnRPASettings;
      private ComboBox cmbSelectedBrowser;
      private Label lbOneTabUrl;
      private TextBox tbOneTabUrl;
      private Button btnOpenSelectedBrowserOnOneTabUrl;
      private Button btnSaveSelectedBrowserOneTabUrl;
      private Label lbSelectedBrowser;
   }
}
