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
         cboxRemoveDuplicatesOnly = new CheckBox();
         btnOpenExtractedFolder = new Button();
         tbFind = new TextBox();
         lbFind = new Label();
         btnFindPrev = new Button();
         btnFindNext = new Button();
         lbFindBtn = new Label();
         cboxRemoveSitesFromDef = new CheckBox();
         cboxRemoveTrackingQueries = new CheckBox();
         checkBox1 = new CheckBox();
         lbExtracted = new Label();
         cmbFindType = new ComboBox();
         lbFindType = new Label();
         SuspendLayout();
         // 
         // rtbText
         // 
         rtbText.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
         rtbText.Location = new Point(22, 92);
         rtbText.Name = "rtbText";
         rtbText.Size = new Size(1071, 650);
         rtbText.TabIndex = 0;
         rtbText.Text = "";
         rtbText.TextChanged += rtbText_TextChanged;
         rtbText.KeyDown += rtbText_KeyDown;
         // 
         // btnOrder
         // 
         btnOrder.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnOrder.Location = new Point(35, 51);
         btnOrder.Name = "btnOrder";
         btnOrder.Size = new Size(85, 35);
         btnOrder.TabIndex = 1;
         btnOrder.Text = "order";
         btnOrder.UseVisualStyleBackColor = true;
         btnOrder.Click += btnOrder_Click;
         // 
         // lbRemovedDuplicates
         // 
         lbRemovedDuplicates.AutoSize = true;
         lbRemovedDuplicates.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbRemovedDuplicates.Location = new Point(548, 63);
         lbRemovedDuplicates.Name = "lbRemovedDuplicates";
         lbRemovedDuplicates.Size = new Size(167, 21);
         lbRemovedDuplicates.TabIndex = 2;
         lbRemovedDuplicates.Text = "Removed duplicates: ";
         // 
         // btnCopyAllRtb
         // 
         btnCopyAllRtb.Enabled = false;
         btnCopyAllRtb.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnCopyAllRtb.Location = new Point(141, 51);
         btnCopyAllRtb.Name = "btnCopyAllRtb";
         btnCopyAllRtb.Size = new Size(94, 35);
         btnCopyAllRtb.TabIndex = 3;
         btnCopyAllRtb.Text = "copy text";
         btnCopyAllRtb.UseVisualStyleBackColor = true;
         btnCopyAllRtb.Click += btnCopyAllRtb_Click;
         // 
         // cboxRemoveDuplicatesFromUp
         // 
         cboxRemoveDuplicatesFromUp.AutoSize = true;
         cboxRemoveDuplicatesFromUp.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         cboxRemoveDuplicatesFromUp.Location = new Point(263, 33);
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
         cboxRemoveDuplicatesFromBelow.Location = new Point(263, 59);
         cboxRemoveDuplicatesFromBelow.Name = "cboxRemoveDuplicatesFromBelow";
         cboxRemoveDuplicatesFromBelow.Size = new Size(234, 23);
         cboxRemoveDuplicatesFromBelow.TabIndex = 6;
         cboxRemoveDuplicatesFromBelow.Text = "remove duplicates from below";
         cboxRemoveDuplicatesFromBelow.UseVisualStyleBackColor = true;
         // 
         // tbExtractWebpages
         // 
         tbExtractWebpages.Location = new Point(548, 37);
         tbExtractWebpages.Name = "tbExtractWebpages";
         tbExtractWebpages.Size = new Size(734, 23);
         tbExtractWebpages.TabIndex = 7;
         // 
         // lbExtractedWebpages
         // 
         lbExtractedWebpages.AutoSize = true;
         lbExtractedWebpages.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbExtractedWebpages.Location = new Point(548, 10);
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
         cboxRemoveDuplicatesExtracted.Location = new Point(865, 8);
         cboxRemoveDuplicatesExtracted.Name = "cboxRemoveDuplicatesExtracted";
         cboxRemoveDuplicatesExtracted.Size = new Size(160, 23);
         cboxRemoveDuplicatesExtracted.TabIndex = 9;
         cboxRemoveDuplicatesExtracted.Text = "Remove Duplicates:";
         cboxRemoveDuplicatesExtracted.UseVisualStyleBackColor = true;
         cboxRemoveDuplicatesExtracted.CheckedChanged += cboxRemoveDuplicatesExtracted_CheckedChanged;
         // 
         // cmbRemoveDuplicatesExtractedType
         // 
         cmbRemoveDuplicatesExtractedType.DropDownStyle = ComboBoxStyle.DropDownList;
         cmbRemoveDuplicatesExtractedType.FormattingEnabled = true;
         cmbRemoveDuplicatesExtractedType.Location = new Point(1027, 8);
         cmbRemoveDuplicatesExtractedType.Name = "cmbRemoveDuplicatesExtractedType";
         cmbRemoveDuplicatesExtractedType.Size = new Size(178, 23);
         cmbRemoveDuplicatesExtractedType.TabIndex = 10;
         // 
         // cboxOpenExtractedFile
         // 
         cboxOpenExtractedFile.AutoSize = true;
         cboxOpenExtractedFile.Checked = true;
         cboxOpenExtractedFile.CheckState = CheckState.Checked;
         cboxOpenExtractedFile.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         cboxOpenExtractedFile.Location = new Point(1112, 65);
         cboxOpenExtractedFile.Name = "cboxOpenExtractedFile";
         cboxOpenExtractedFile.Size = new Size(156, 23);
         cboxOpenExtractedFile.TabIndex = 11;
         cboxOpenExtractedFile.Text = "Open Extracted file";
         cboxOpenExtractedFile.UseVisualStyleBackColor = true;
         // 
         // btnExtractWebpages
         // 
         btnExtractWebpages.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnExtractWebpages.Location = new Point(1211, 6);
         btnExtractWebpages.Name = "btnExtractWebpages";
         btnExtractWebpages.Size = new Size(68, 26);
         btnExtractWebpages.TabIndex = 12;
         btnExtractWebpages.Text = "Extract";
         btnExtractWebpages.UseVisualStyleBackColor = true;
         btnExtractWebpages.Click += btnExtractWebpages_Click;
         // 
         // cboxRemoveDuplicatesOnly
         // 
         cboxRemoveDuplicatesOnly.AutoSize = true;
         cboxRemoveDuplicatesOnly.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         cboxRemoveDuplicatesOnly.Location = new Point(22, 22);
         cboxRemoveDuplicatesOnly.Name = "cboxRemoveDuplicatesOnly";
         cboxRemoveDuplicatesOnly.Size = new Size(222, 23);
         cboxRemoveDuplicatesOnly.TabIndex = 13;
         cboxRemoveDuplicatesOnly.Text = "remove duplicates from only";
         cboxRemoveDuplicatesOnly.UseVisualStyleBackColor = true;
         cboxRemoveDuplicatesOnly.CheckedChanged += cboxRemoveDuplicatesOnly_CheckedChanged;
         // 
         // btnOpenExtractedFolder
         // 
         btnOpenExtractedFolder.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnOpenExtractedFolder.Location = new Point(1108, 105);
         btnOpenExtractedFolder.Name = "btnOpenExtractedFolder";
         btnOpenExtractedFolder.Size = new Size(167, 26);
         btnOpenExtractedFolder.TabIndex = 15;
         btnOpenExtractedFolder.Text = "Open Extracted Folder";
         btnOpenExtractedFolder.UseVisualStyleBackColor = true;
         btnOpenExtractedFolder.Click += btnOpenExtractedFolder_Click;
         // 
         // tbFind
         // 
         tbFind.Location = new Point(1105, 232);
         tbFind.Name = "tbFind";
         tbFind.Size = new Size(186, 23);
         tbFind.TabIndex = 16;
         tbFind.KeyDown += tbFind_KeyDown;
         // 
         // lbFind
         // 
         lbFind.AutoSize = true;
         lbFind.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbFind.Location = new Point(1099, 208);
         lbFind.Name = "lbFind";
         lbFind.Size = new Size(45, 21);
         lbFind.TabIndex = 17;
         lbFind.Text = "Find:";
         // 
         // btnFindPrev
         // 
         btnFindPrev.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnFindPrev.Location = new Point(1229, 261);
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
         btnFindNext.Location = new Point(1161, 261);
         btnFindNext.Name = "btnFindNext";
         btnFindNext.Size = new Size(62, 26);
         btnFindNext.TabIndex = 19;
         btnFindNext.Text = "Next";
         btnFindNext.UseVisualStyleBackColor = true;
         btnFindNext.Click += btnFindNext_Click;
         // 
         // lbFindBtn
         // 
         lbFindBtn.AutoSize = true;
         lbFindBtn.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbFindBtn.Location = new Point(1110, 263);
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
         cboxRemoveSitesFromDef.Location = new Point(884, 64);
         cboxRemoveSitesFromDef.Name = "cboxRemoveSitesFromDef";
         cboxRemoveSitesFromDef.Size = new Size(209, 23);
         cboxRemoveSitesFromDef.TabIndex = 14;
         cboxRemoveSitesFromDef.Text = "Remove Sites From Default";
         cboxRemoveSitesFromDef.UseVisualStyleBackColor = true;
         // 
         // cboxRemoveTrackingQueries
         // 
         cboxRemoveTrackingQueries.AutoSize = true;
         cboxRemoveTrackingQueries.Checked = true;
         cboxRemoveTrackingQueries.CheckState = CheckState.Checked;
         cboxRemoveTrackingQueries.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         cboxRemoveTrackingQueries.Location = new Point(263, 8);
         cboxRemoveTrackingQueries.Name = "cboxRemoveTrackingQueries";
         cboxRemoveTrackingQueries.Size = new Size(192, 23);
         cboxRemoveTrackingQueries.TabIndex = 21;
         cboxRemoveTrackingQueries.Text = "remove tracking queries";
         cboxRemoveTrackingQueries.UseVisualStyleBackColor = true;
         // 
         // checkBox1
         // 
         checkBox1.AutoSize = true;
         checkBox1.Checked = true;
         checkBox1.CheckState = CheckState.Checked;
         checkBox1.Font = new Font("Segoe UI", 10.181818F, FontStyle.Bold, GraphicsUnit.Point, 0);
         checkBox1.Location = new Point(1103, 170);
         checkBox1.Name = "checkBox1";
         checkBox1.Size = new Size(192, 23);
         checkBox1.TabIndex = 22;
         checkBox1.Text = "remove tracking queries";
         checkBox1.UseVisualStyleBackColor = true;
         // 
         // lbExtracted
         // 
         lbExtracted.AutoSize = true;
         lbExtracted.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbExtracted.Location = new Point(1103, 146);
         lbExtracted.Name = "lbExtracted";
         lbExtracted.Size = new Size(83, 21);
         lbExtracted.TabIndex = 23;
         lbExtracted.Text = "Extracted:";
         // 
         // cmbFindType
         // 
         cmbFindType.DropDownStyle = ComboBoxStyle.DropDownList;
         cmbFindType.FormattingEnabled = true;
         cmbFindType.Location = new Point(1105, 324);
         cmbFindType.Name = "cmbFindType";
         cmbFindType.Size = new Size(186, 23);
         cmbFindType.TabIndex = 24;
         // 
         // lbFindType
         // 
         lbFindType.AutoSize = true;
         lbFindType.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbFindType.Location = new Point(1099, 296);
         lbFindType.Name = "lbFindType";
         lbFindType.Size = new Size(84, 21);
         lbFindType.TabIndex = 25;
         lbFindType.Text = "Find Type:";
         // 
         // Form1
         // 
         AutoScaleDimensions = new SizeF(7F, 15F);
         AutoScaleMode = AutoScaleMode.Font;
         ClientSize = new Size(1303, 754);
         Controls.Add(lbFindType);
         Controls.Add(cmbFindType);
         Controls.Add(lbExtracted);
         Controls.Add(checkBox1);
         Controls.Add(cboxRemoveTrackingQueries);
         Controls.Add(lbFindBtn);
         Controls.Add(btnFindNext);
         Controls.Add(btnFindPrev);
         Controls.Add(lbFind);
         Controls.Add(tbFind);
         Controls.Add(btnOpenExtractedFolder);
         Controls.Add(cboxRemoveSitesFromDef);
         Controls.Add(cboxRemoveDuplicatesOnly);
         Controls.Add(btnExtractWebpages);
         Controls.Add(cboxOpenExtractedFile);
         Controls.Add(cmbRemoveDuplicatesExtractedType);
         Controls.Add(cboxRemoveDuplicatesExtracted);
         Controls.Add(lbExtractedWebpages);
         Controls.Add(tbExtractWebpages);
         Controls.Add(cboxRemoveDuplicatesFromBelow);
         Controls.Add(cboxRemoveDuplicatesFromUp);
         Controls.Add(btnCopyAllRtb);
         Controls.Add(lbRemovedDuplicates);
         Controls.Add(btnOrder);
         Controls.Add(rtbText);
         FormBorderStyle = FormBorderStyle.FixedSingle;
         MaximizeBox = false;
         Name = "Form1";
         Text = "OneTab order";
         Paint += Form1_Paint;
         ResumeLayout(false);
         PerformLayout();
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
      private CheckBox cboxRemoveDuplicatesOnly;
      private Button btnOpenExtractedFolder;
      private TextBox tbFind;
      private Label lbFind;
      private Button btnFindPrev;
      private Button btnFindNext;
      private Label lbFindBtn;
      private CheckBox cboxRemoveSitesFromDef;
      private CheckBox cboxRemoveTrackingQueries;
      private CheckBox checkBox1;
      private Label lbExtracted;
      private ComboBox cmbFindType;
      private Label lbFindType;
   }
}
