namespace LALE
{
    partial class ChestEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.nItem = new System.Windows.Forms.NumericUpDown();
            this.bCancel = new System.Windows.Forms.Button();
            this.bAccept = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nItem)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Item:";
            // 
            // nItem
            // 
            this.nItem.Hexadecimal = true;
            this.nItem.Location = new System.Drawing.Point(61, 12);
            this.nItem.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nItem.Name = "nItem";
            this.nItem.Size = new System.Drawing.Size(120, 20);
            this.nItem.TabIndex = 2;
            this.nItem.ValueChanged += new System.EventHandler(this.nItem_ValueChanged);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(15, 48);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 4;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bAccept
            // 
            this.bAccept.Location = new System.Drawing.Point(106, 48);
            this.bAccept.Name = "bAccept";
            this.bAccept.Size = new System.Drawing.Size(75, 23);
            this.bAccept.TabIndex = 5;
            this.bAccept.Text = "Accept";
            this.bAccept.UseVisualStyleBackColor = true;
            this.bAccept.Click += new System.EventHandler(this.bAccept_Click);
            // 
            // ChestEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(193, 82);
            this.Controls.Add(this.bAccept);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nItem);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "ChestEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chest Editor";
            ((System.ComponentModel.ISupportInitialize)(this.nItem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nItem;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bAccept;
    }
}