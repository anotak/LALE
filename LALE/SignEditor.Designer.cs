namespace LALE
{
    partial class SignEditor
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
            this.nMap = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nPointer = new System.Windows.Forms.NumericUpDown();
            this.bCancel = new System.Windows.Forms.Button();
            this.bAccept = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPointer)).BeginInit();
            this.SuspendLayout();
            // 
            // nMap
            // 
            this.nMap.Hexadecimal = true;
            this.nMap.Location = new System.Drawing.Point(108, 12);
            this.nMap.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nMap.Name = "nMap";
            this.nMap.Size = new System.Drawing.Size(120, 20);
            this.nMap.TabIndex = 0;
            this.nMap.ValueChanged += new System.EventHandler(this.nMap_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Map:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Text Pointer:";
            // 
            // nPointer
            // 
            this.nPointer.Enabled = false;
            this.nPointer.Hexadecimal = true;
            this.nPointer.Location = new System.Drawing.Point(108, 50);
            this.nPointer.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nPointer.Name = "nPointer";
            this.nPointer.Size = new System.Drawing.Size(120, 20);
            this.nPointer.TabIndex = 2;
            this.nPointer.ValueChanged += new System.EventHandler(this.nPointer_ValueChanged);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(15, 84);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(97, 23);
            this.bCancel.TabIndex = 4;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bAccept
            // 
            this.bAccept.Location = new System.Drawing.Point(131, 84);
            this.bAccept.Name = "bAccept";
            this.bAccept.Size = new System.Drawing.Size(97, 23);
            this.bAccept.TabIndex = 5;
            this.bAccept.Text = "Accept";
            this.bAccept.UseVisualStyleBackColor = true;
            this.bAccept.Click += new System.EventHandler(this.bAccept_Click);
            // 
            // SignEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 119);
            this.Controls.Add(this.bAccept);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nPointer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SignEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sign Pointer Editor";
            ((System.ComponentModel.ISupportInitialize)(this.nMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPointer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nMap;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nPointer;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bAccept;
    }
}