namespace LALE
{
    partial class MinibossEditor
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
            this.comDungeons = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nMap1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nMap2 = new System.Windows.Forms.NumericUpDown();
            this.bCancel = new System.Windows.Forms.Button();
            this.bAccept = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nMap1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMap2)).BeginInit();
            this.SuspendLayout();
            // 
            // comDungeons
            // 
            this.comDungeons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comDungeons.FormattingEnabled = true;
            this.comDungeons.Items.AddRange(new object[] {
            "Level 1",
            "Level 2",
            "Level 3",
            "Level 4",
            "Level 5",
            "Level 6",
            "Level 7",
            "Level 8"});
            this.comDungeons.Location = new System.Drawing.Point(72, 12);
            this.comDungeons.Name = "comDungeons";
            this.comDungeons.Size = new System.Drawing.Size(121, 21);
            this.comDungeons.TabIndex = 0;
            this.comDungeons.SelectedIndexChanged += new System.EventHandler(this.comDungeons_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Dungeon:";
            // 
            // nMap1
            // 
            this.nMap1.Hexadecimal = true;
            this.nMap1.Location = new System.Drawing.Point(72, 49);
            this.nMap1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nMap1.Name = "nMap1";
            this.nMap1.Size = new System.Drawing.Size(121, 20);
            this.nMap1.TabIndex = 2;
            this.nMap1.ValueChanged += new System.EventHandler(this.nMap1_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Map One:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Map Two:";
            // 
            // nMap2
            // 
            this.nMap2.Hexadecimal = true;
            this.nMap2.Location = new System.Drawing.Point(72, 86);
            this.nMap2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nMap2.Name = "nMap2";
            this.nMap2.Size = new System.Drawing.Size(121, 20);
            this.nMap2.TabIndex = 4;
            this.nMap2.ValueChanged += new System.EventHandler(this.nMap2_ValueChanged);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(15, 123);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(82, 23);
            this.bCancel.TabIndex = 6;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bAccept
            // 
            this.bAccept.Location = new System.Drawing.Point(111, 123);
            this.bAccept.Name = "bAccept";
            this.bAccept.Size = new System.Drawing.Size(82, 23);
            this.bAccept.TabIndex = 7;
            this.bAccept.Text = "Accept";
            this.bAccept.UseVisualStyleBackColor = true;
            this.bAccept.Click += new System.EventHandler(this.bAccept_Click);
            // 
            // MinibossEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(206, 158);
            this.Controls.Add(this.bAccept);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nMap2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nMap1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comDungeons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MinibossEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Portal Editor";
            ((System.ComponentModel.ISupportInitialize)(this.nMap1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMap2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comDungeons;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nMap1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nMap2;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bAccept;

    }
}