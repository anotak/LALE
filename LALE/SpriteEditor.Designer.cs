namespace LALE
{
    partial class SpriteEditor
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
            this.nDungeon = new System.Windows.Forms.NumericUpDown();
            this.nMap = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cOverworld = new System.Windows.Forms.CheckBox();
            this.pBox = new System.Windows.Forms.PictureBox();
            this.bAccept = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.nSpriteBank = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nDungeon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpriteBank)).BeginInit();
            this.SuspendLayout();
            // 
            // nDungeon
            // 
            this.nDungeon.Hexadecimal = true;
            this.nDungeon.Location = new System.Drawing.Point(72, 12);
            this.nDungeon.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nDungeon.Name = "nDungeon";
            this.nDungeon.Size = new System.Drawing.Size(120, 20);
            this.nDungeon.TabIndex = 0;
            this.nDungeon.ValueChanged += new System.EventHandler(this.nDungeon_ValueChanged);
            // 
            // nMap
            // 
            this.nMap.Hexadecimal = true;
            this.nMap.Location = new System.Drawing.Point(72, 51);
            this.nMap.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nMap.Name = "nMap";
            this.nMap.Size = new System.Drawing.Size(120, 20);
            this.nMap.TabIndex = 1;
            this.nMap.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Dungeon:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Map:";
            // 
            // cOverworld
            // 
            this.cOverworld.AutoSize = true;
            this.cOverworld.Location = new System.Drawing.Point(201, 13);
            this.cOverworld.Name = "cOverworld";
            this.cOverworld.Size = new System.Drawing.Size(73, 17);
            this.cOverworld.TabIndex = 4;
            this.cOverworld.Text = "Overworld";
            this.cOverworld.UseVisualStyleBackColor = true;
            this.cOverworld.CheckedChanged += new System.EventHandler(this.cOverworld_CheckedChanged);
            // 
            // pBox
            // 
            this.pBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pBox.Location = new System.Drawing.Point(15, 92);
            this.pBox.Name = "pBox";
            this.pBox.Size = new System.Drawing.Size(132, 36);
            this.pBox.TabIndex = 5;
            this.pBox.TabStop = false;
            // 
            // bAccept
            // 
            this.bAccept.Location = new System.Drawing.Point(15, 141);
            this.bAccept.Name = "bAccept";
            this.bAccept.Size = new System.Drawing.Size(117, 23);
            this.bAccept.TabIndex = 6;
            this.bAccept.Text = "Accept";
            this.bAccept.UseVisualStyleBackColor = true;
            this.bAccept.Click += new System.EventHandler(this.bAccept_Click);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(157, 141);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(117, 23);
            this.bCancel.TabIndex = 7;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // nSpriteBank
            // 
            this.nSpriteBank.Hexadecimal = true;
            this.nSpriteBank.Location = new System.Drawing.Point(201, 108);
            this.nSpriteBank.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nSpriteBank.Name = "nSpriteBank";
            this.nSpriteBank.Size = new System.Drawing.Size(73, 20);
            this.nSpriteBank.TabIndex = 8;
            this.nSpriteBank.ValueChanged += new System.EventHandler(this.nSpriteBank_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(154, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Bank:";
            // 
            // SpriteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 176);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nSpriteBank);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bAccept);
            this.Controls.Add(this.pBox);
            this.Controls.Add(this.cOverworld);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nMap);
            this.Controls.Add(this.nDungeon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpriteEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sprite Viewer/Bank Editor";
            ((System.ComponentModel.ISupportInitialize)(this.nDungeon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSpriteBank)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nDungeon;
        private System.Windows.Forms.NumericUpDown nMap;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cOverworld;
        private System.Windows.Forms.PictureBox pBox;
        private System.Windows.Forms.Button bAccept;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.NumericUpDown nSpriteBank;
        private System.Windows.Forms.Label label3;
    }
}