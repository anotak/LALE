namespace LALE
{
    partial class StartEditor
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
            this.nDungeon = new System.Windows.Forms.NumericUpDown();
            this.nLinkYPos = new System.Windows.Forms.NumericUpDown();
            this.nLinkXPos = new System.Windows.Forms.NumericUpDown();
            this.cOverworld = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDungeon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nLinkYPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nLinkXPos)).BeginInit();
            this.SuspendLayout();
            // 
            // nMap
            // 
            this.nMap.Hexadecimal = true;
            this.nMap.Location = new System.Drawing.Point(86, 38);
            this.nMap.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nMap.Name = "nMap";
            this.nMap.Size = new System.Drawing.Size(110, 20);
            this.nMap.TabIndex = 0;
            this.nMap.ValueChanged += new System.EventHandler(this.nMap_ValueChanged);
            // 
            // nDungeon
            // 
            this.nDungeon.Hexadecimal = true;
            this.nDungeon.Location = new System.Drawing.Point(86, 12);
            this.nDungeon.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nDungeon.Name = "nDungeon";
            this.nDungeon.Size = new System.Drawing.Size(110, 20);
            this.nDungeon.TabIndex = 1;
            this.nDungeon.ValueChanged += new System.EventHandler(this.nDungeon_ValueChanged);
            // 
            // nLinkYPos
            // 
            this.nLinkYPos.Hexadecimal = true;
            this.nLinkYPos.Location = new System.Drawing.Point(86, 90);
            this.nLinkYPos.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.nLinkYPos.Name = "nLinkYPos";
            this.nLinkYPos.Size = new System.Drawing.Size(110, 20);
            this.nLinkYPos.TabIndex = 2;
            this.nLinkYPos.ValueChanged += new System.EventHandler(this.nLinkYPos_ValueChanged);
            // 
            // nLinkXPos
            // 
            this.nLinkXPos.Hexadecimal = true;
            this.nLinkXPos.Location = new System.Drawing.Point(86, 64);
            this.nLinkXPos.Maximum = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.nLinkXPos.Name = "nLinkXPos";
            this.nLinkXPos.Size = new System.Drawing.Size(110, 20);
            this.nLinkXPos.TabIndex = 3;
            this.nLinkXPos.ValueChanged += new System.EventHandler(this.nLinkXPos_ValueChanged);
            // 
            // cOverworld
            // 
            this.cOverworld.AutoSize = true;
            this.cOverworld.Location = new System.Drawing.Point(214, 13);
            this.cOverworld.Name = "cOverworld";
            this.cOverworld.Size = new System.Drawing.Size(113, 17);
            this.cOverworld.TabIndex = 4;
            this.cOverworld.Text = "Start on Overworld";
            this.cOverworld.UseVisualStyleBackColor = true;
            this.cOverworld.CheckedChanged += new System.EventHandler(this.cOverworld_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Dungeon:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Map:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Link\'s X Pos:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Link\'s Y Pos:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(232, 80);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(232, 42);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Accept";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // StartEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 128);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cOverworld);
            this.Controls.Add(this.nLinkXPos);
            this.Controls.Add(this.nLinkYPos);
            this.Controls.Add(this.nDungeon);
            this.Controls.Add(this.nMap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Start Editor";
            ((System.ComponentModel.ISupportInitialize)(this.nMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDungeon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nLinkYPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nLinkXPos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nMap;
        private System.Windows.Forms.NumericUpDown nDungeon;
        private System.Windows.Forms.NumericUpDown nLinkYPos;
        private System.Windows.Forms.NumericUpDown nLinkXPos;
        private System.Windows.Forms.CheckBox cOverworld;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}