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
         SuspendLayout();
         // 
         // rtbText
         // 
         rtbText.Location = new Point(23, 80);
         rtbText.Name = "rtbText";
         rtbText.Size = new Size(1106, 650);
         rtbText.TabIndex = 0;
         rtbText.Text = "";
         // 
         // btnOrder
         // 
         btnOrder.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnOrder.Location = new Point(36, 30);
         btnOrder.Name = "btnOrder";
         btnOrder.Size = new Size(85, 33);
         btnOrder.TabIndex = 1;
         btnOrder.Text = "order";
         btnOrder.UseVisualStyleBackColor = true;
         btnOrder.Click += btnOrder_Click;
         // 
         // lbRemovedDuplicates
         // 
         lbRemovedDuplicates.AutoSize = true;
         lbRemovedDuplicates.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
         lbRemovedDuplicates.Location = new Point(347, 35);
         lbRemovedDuplicates.Name = "lbRemovedDuplicates";
         lbRemovedDuplicates.Size = new Size(167, 21);
         lbRemovedDuplicates.TabIndex = 2;
         lbRemovedDuplicates.Text = "Removed duplicates: ";
         // 
         // btnCopyAllRtb
         // 
         btnCopyAllRtb.Enabled = false;
         btnCopyAllRtb.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
         btnCopyAllRtb.Location = new Point(142, 30);
         btnCopyAllRtb.Name = "btnCopyAllRtb";
         btnCopyAllRtb.Size = new Size(85, 33);
         btnCopyAllRtb.TabIndex = 3;
         btnCopyAllRtb.Text = "copy text";
         btnCopyAllRtb.UseVisualStyleBackColor = true;
         btnCopyAllRtb.Click += btnCopyAllRtb_Click;
         // 
         // Form1
         // 
         AutoScaleDimensions = new SizeF(7F, 15F);
         AutoScaleMode = AutoScaleMode.Font;
         ClientSize = new Size(1156, 754);
         Controls.Add(btnCopyAllRtb);
         Controls.Add(lbRemovedDuplicates);
         Controls.Add(btnOrder);
         Controls.Add(rtbText);
         FormBorderStyle = FormBorderStyle.FixedSingle;
         MaximizeBox = false;
         Name = "Form1";
         Text = "OneTab order";
         ResumeLayout(false);
         PerformLayout();
      }

      #endregion

      private RichTextBox rtbText;
      private Button btnOrder;
      private Label lbRemovedDuplicates;
      private Button btnCopyAllRtb;
   }
}
