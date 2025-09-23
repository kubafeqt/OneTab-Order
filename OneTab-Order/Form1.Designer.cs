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
         label1 = new Label();
         cboxRemoveDuplicatesExtracted = new CheckBox();
         cmbRemoveDuplicatesExtractedType = new ComboBox();
         cboxOpenExtractedFile = new CheckBox();
         btnExtractWebpages = new Button();
         cboxRemoveDuplicatesOnly = new CheckBox();
         cboxRemoveSitesFromDef = new CheckBox();
         SuspendLayout();
         // 
         // rtbText
         // 
         rtbText.Location = new Point(22, 92);
         rtbText.Name = "rtbText";
         rtbText.Size = new Size(1260, 650);
         rtbText.TabIndex = 0;
         rtbText.Text = "";
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
         // label1
         // 
         label1.AutoSize = true;
         label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
         label1.Location = new Point(548, 10);
         label1.Name = "label1";
         label1.Size = new Size(143, 21);
         label1.TabIndex = 8;
         label1.Text = "Extract webpages:";
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
         btnExtractWebpages.Location = new Point(1211, 8);
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
         // Form1
         // 
         AutoScaleDimensions = new SizeF(7F, 15F);
         AutoScaleMode = AutoScaleMode.Font;
         ClientSize = new Size(1303, 754);
         Controls.Add(cboxRemoveSitesFromDef);
         Controls.Add(cboxRemoveDuplicatesOnly);
         Controls.Add(btnExtractWebpages);
         Controls.Add(cboxOpenExtractedFile);
         Controls.Add(cmbRemoveDuplicatesExtractedType);
         Controls.Add(cboxRemoveDuplicatesExtracted);
         Controls.Add(label1);
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
      private Label label1;
      private CheckBox cboxRemoveDuplicatesExtracted;
      private ComboBox cmbRemoveDuplicatesExtractedType;
      private CheckBox cboxOpenExtractedFile;
      private Button btnExtractWebpages;
      private CheckBox cboxRemoveDuplicatesOnly;
      private CheckBox cboxRemoveSitesFromDef;
   }
}
