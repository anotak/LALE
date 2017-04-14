namespace LALE
{
    partial class PaletteEditor
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
            this.bCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.nDungeon = new System.Windows.Forms.NumericUpDown();
            this.nMap = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nIndex = new System.Windows.Forms.NumericUpDown();
            this.pPalette = new LALE.GridBox();
            this.pTileset = new LALE.GridBox();
            ((System.ComponentModel.ISupportInitialize)(this.nDungeon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pPalette)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTileset)).BeginInit();
            this.SuspendLayout();
            // 
            // bAccept
            // 
            this.bAccept.Location = new System.Drawing.Point(150, 83);
            this.bAccept.Name = "bAccept";
            this.bAccept.Size = new System.Drawing.Size(123, 23);
            this.bAccept.TabIndex = 19;
            this.bAccept.Text = "Accept";
            this.bAccept.UseVisualStyleBackColor = true;
            this.bAccept.Click += new System.EventHandler(this.bAccept_Click);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(150, 112);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(123, 23);
            this.bCancel.TabIndex = 20;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(147, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Dungeon:";
            // 
            // nDungeon
            // 
            this.nDungeon.Hexadecimal = true;
            this.nDungeon.Location = new System.Drawing.Point(207, 54);
            this.nDungeon.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nDungeon.Name = "nDungeon";
            this.nDungeon.Size = new System.Drawing.Size(66, 20);
            this.nDungeon.TabIndex = 24;
            this.nDungeon.ValueChanged += new System.EventHandler(this.nDungeon_ValueChanged);
            // 
            // nMap
            // 
            this.nMap.Hexadecimal = true;
            this.nMap.Location = new System.Drawing.Point(207, 30);
            this.nMap.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nMap.Name = "nMap";
            this.nMap.Size = new System.Drawing.Size(66, 20);
            this.nMap.TabIndex = 23;
            this.nMap.ValueChanged += new System.EventHandler(this.nMap_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(147, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Map:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 404);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Palette Location:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(156, 404);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "R: G: B:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(147, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Index:";
            // 
            // nIndex
            // 
            this.nIndex.Hexadecimal = true;
            this.nIndex.Location = new System.Drawing.Point(207, 6);
            this.nIndex.Name = "nIndex";
            this.nIndex.Size = new System.Drawing.Size(66, 20);
            this.nIndex.TabIndex = 29;
            this.nIndex.ValueChanged += new System.EventHandler(this.nIndex_ValueChanged);
            // 
            // pPalette
            // 
            this.pPalette.AllowMultiSelection = false;
            this.pPalette.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPalette.BoxSize = new System.Drawing.Size(32, 16);
            this.pPalette.CanvasSize = new System.Drawing.Size(128, 128);
            this.pPalette.HoverBox = true;
            this.pPalette.HoverColor = System.Drawing.Color.White;
            this.pPalette.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            this.pPalette.Location = new System.Drawing.Point(13, 6);
            this.pPalette.Name = "pPalette";
            this.pPalette.Selectable = false;
            this.pPalette.SelectedIndex = 0;
            this.pPalette.SelectionColor = System.Drawing.Color.Red;
            this.pPalette.SelectionRectangle = new System.Drawing.Rectangle(0, 0, 1, 1);
            this.pPalette.Size = new System.Drawing.Size(128, 129);
            this.pPalette.TabIndex = 21;
            this.pPalette.TabStop = false;
            this.pPalette.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pPalette_MouseDown);
            this.pPalette.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pPalette_MouseMove);
            // 
            // pTileset
            // 
            this.pTileset.AllowMultiSelection = false;
            this.pTileset.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pTileset.BoxSize = new System.Drawing.Size(16, 16);
            this.pTileset.CanvasSize = new System.Drawing.Size(256, 256);
            this.pTileset.HoverBox = true;
            this.pTileset.HoverColor = System.Drawing.Color.White;
            this.pTileset.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            this.pTileset.Location = new System.Drawing.Point(13, 141);
            this.pTileset.Name = "pTileset";
            this.pTileset.Selectable = false;
            this.pTileset.SelectedIndex = 0;
            this.pTileset.SelectionColor = System.Drawing.Color.Red;
            this.pTileset.SelectionRectangle = new System.Drawing.Rectangle(0, 0, 1, 1);
            this.pTileset.Size = new System.Drawing.Size(260, 260);
            this.pTileset.TabIndex = 0;
            this.pTileset.TabStop = false;
            // 
            // PaletteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 425);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nIndex);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nDungeon);
            this.Controls.Add(this.nMap);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pPalette);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bAccept);
            this.Controls.Add(this.pTileset);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PaletteEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Palette Editor";
            ((System.ComponentModel.ISupportInitialize)(this.nDungeon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pPalette)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTileset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GridBox pTileset;
        private System.Windows.Forms.Button bAccept;
        private System.Windows.Forms.Button bCancel;
        private GridBox pPalette;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown nDungeon;
        public System.Windows.Forms.NumericUpDown nMap;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.NumericUpDown nIndex;
    }
}