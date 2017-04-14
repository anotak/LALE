namespace LALE
{
    partial class WarpEditor
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
            this.bAccept = new System.Windows.Forms.Button();
            this.nIndex = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nRegion = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nDestX = new System.Windows.Forms.NumericUpDown();
            this.nDestY = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.nMap = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.bCreateWarp = new System.Windows.Forms.Button();
            this.bDeleteWarp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nRegion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDestX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDestY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMap)).BeginInit();
            this.SuspendLayout();
            // 
            // bAccept
            // 
            this.bAccept.Location = new System.Drawing.Point(106, 247);
            this.bAccept.Name = "bAccept";
            this.bAccept.Size = new System.Drawing.Size(76, 23);
            this.bAccept.TabIndex = 1;
            this.bAccept.Text = "Accept";
            this.bAccept.UseVisualStyleBackColor = true;
            this.bAccept.Click += new System.EventHandler(this.bAccept_Click);
            // 
            // nIndex
            // 
            this.nIndex.Location = new System.Drawing.Point(83, 12);
            this.nIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nIndex.Name = "nIndex";
            this.nIndex.Size = new System.Drawing.Size(99, 20);
            this.nIndex.TabIndex = 2;
            this.nIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nIndex.ValueChanged += new System.EventHandler(this.nIndex_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Warp Index:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Region:";
            // 
            // nRegion
            // 
            this.nRegion.Enabled = false;
            this.nRegion.Hexadecimal = true;
            this.nRegion.Location = new System.Drawing.Point(83, 76);
            this.nRegion.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nRegion.Name = "nRegion";
            this.nRegion.Size = new System.Drawing.Size(99, 20);
            this.nRegion.TabIndex = 5;
            this.nRegion.ValueChanged += new System.EventHandler(this.nRegion_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Dest. X Pos:";
            // 
            // nDestX
            // 
            this.nDestX.Enabled = false;
            this.nDestX.Hexadecimal = true;
            this.nDestX.Location = new System.Drawing.Point(83, 140);
            this.nDestX.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nDestX.Name = "nDestX";
            this.nDestX.Size = new System.Drawing.Size(99, 20);
            this.nDestX.TabIndex = 7;
            this.nDestX.ValueChanged += new System.EventHandler(this.nDestX_ValueChanged);
            // 
            // nDestY
            // 
            this.nDestY.Enabled = false;
            this.nDestY.Hexadecimal = true;
            this.nDestY.Location = new System.Drawing.Point(83, 172);
            this.nDestY.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nDestY.Name = "nDestY";
            this.nDestY.Size = new System.Drawing.Size(99, 20);
            this.nDestY.TabIndex = 8;
            this.nDestY.ValueChanged += new System.EventHandler(this.nDestY_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Dest. Y Pos:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Type:";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Overworld",
            "Dungeon",
            "Side-Scroller"});
            this.comboBox1.Location = new System.Drawing.Point(83, 44);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(99, 21);
            this.comboBox1.TabIndex = 11;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 247);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(76, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // nMap
            // 
            this.nMap.Enabled = false;
            this.nMap.Hexadecimal = true;
            this.nMap.Location = new System.Drawing.Point(83, 108);
            this.nMap.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nMap.Name = "nMap";
            this.nMap.Size = new System.Drawing.Size(99, 20);
            this.nMap.TabIndex = 14;
            this.nMap.ValueChanged += new System.EventHandler(this.nMap_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Map:";
            // 
            // bCreateWarp
            // 
            this.bCreateWarp.Location = new System.Drawing.Point(11, 209);
            this.bCreateWarp.Name = "bCreateWarp";
            this.bCreateWarp.Size = new System.Drawing.Size(76, 23);
            this.bCreateWarp.TabIndex = 15;
            this.bCreateWarp.Text = "Create Warp";
            this.bCreateWarp.UseVisualStyleBackColor = true;
            this.bCreateWarp.Click += new System.EventHandler(this.bCreateWarp_Click);
            // 
            // bDeleteWarp
            // 
            this.bDeleteWarp.Location = new System.Drawing.Point(106, 209);
            this.bDeleteWarp.Name = "bDeleteWarp";
            this.bDeleteWarp.Size = new System.Drawing.Size(76, 23);
            this.bDeleteWarp.TabIndex = 16;
            this.bDeleteWarp.Text = "Delete Warp";
            this.bDeleteWarp.UseVisualStyleBackColor = true;
            this.bDeleteWarp.Click += new System.EventHandler(this.bDeleteWarp_Click);
            // 
            // WarpEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(197, 282);
            this.Controls.Add(this.bDeleteWarp);
            this.Controls.Add(this.bCreateWarp);
            this.Controls.Add(this.nMap);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nDestY);
            this.Controls.Add(this.nDestX);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nRegion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nIndex);
            this.Controls.Add(this.bAccept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "WarpEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WarpEditor";
            ((System.ComponentModel.ISupportInitialize)(this.nIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nRegion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDestX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDestY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bAccept;
        private System.Windows.Forms.NumericUpDown nIndex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nRegion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nDestY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.NumericUpDown nDestX;
        private System.Windows.Forms.NumericUpDown nMap;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button bCreateWarp;
        private System.Windows.Forms.Button bDeleteWarp;
    }
}