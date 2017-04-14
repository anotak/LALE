namespace LALE
{
    partial class MinimapEditor
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
            this.gBoxDungeon = new System.Windows.Forms.GroupBox();
            this.lblHoverPos = new System.Windows.Forms.Label();
            this.nDeathMinimap = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.nDeathDungeon = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.bCancel = new System.Windows.Forms.Button();
            this.nDeathMap = new System.Windows.Forms.NumericUpDown();
            this.bAccept = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.nArrow = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nMap = new System.Windows.Forms.NumericUpDown();
            this.pTiles = new LALE.GridBox();
            this.pMinimap = new LALE.GridBox();
            this.gBoxOverWorld = new System.Windows.Forms.GroupBox();
            this.bAccept1 = new System.Windows.Forms.Button();
            this.bCancel1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.nPalette = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nTile = new System.Windows.Forms.NumericUpDown();
            this.pTile = new System.Windows.Forms.PictureBox();
            this.pMinimapO = new LALE.GridBox();
            this.gBoxDungeon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nDeathMinimap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDeathDungeon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDeathMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nArrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMinimap)).BeginInit();
            this.gBoxOverWorld.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPalette)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nTile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMinimapO)).BeginInit();
            this.SuspendLayout();
            // 
            // gBoxDungeon
            // 
            this.gBoxDungeon.BackColor = System.Drawing.SystemColors.Control;
            this.gBoxDungeon.Controls.Add(this.lblHoverPos);
            this.gBoxDungeon.Controls.Add(this.nDeathMinimap);
            this.gBoxDungeon.Controls.Add(this.label7);
            this.gBoxDungeon.Controls.Add(this.nDeathDungeon);
            this.gBoxDungeon.Controls.Add(this.label6);
            this.gBoxDungeon.Controls.Add(this.bCancel);
            this.gBoxDungeon.Controls.Add(this.nDeathMap);
            this.gBoxDungeon.Controls.Add(this.bAccept);
            this.gBoxDungeon.Controls.Add(this.label4);
            this.gBoxDungeon.Controls.Add(this.nArrow);
            this.gBoxDungeon.Controls.Add(this.label5);
            this.gBoxDungeon.Controls.Add(this.label1);
            this.gBoxDungeon.Controls.Add(this.nMap);
            this.gBoxDungeon.Controls.Add(this.pTiles);
            this.gBoxDungeon.Controls.Add(this.pMinimap);
            this.gBoxDungeon.ForeColor = System.Drawing.SystemColors.ControlText;
            this.gBoxDungeon.Location = new System.Drawing.Point(276, 4);
            this.gBoxDungeon.Name = "gBoxDungeon";
            this.gBoxDungeon.Size = new System.Drawing.Size(202, 228);
            this.gBoxDungeon.TabIndex = 1;
            this.gBoxDungeon.TabStop = false;
            this.gBoxDungeon.Visible = false;
            // 
            // lblHoverPos
            // 
            this.lblHoverPos.AutoSize = true;
            this.lblHoverPos.Location = new System.Drawing.Point(6, 210);
            this.lblHoverPos.Name = "lblHoverPos";
            this.lblHoverPos.Size = new System.Drawing.Size(73, 13);
            this.lblHoverPos.TabIndex = 15;
            this.lblHoverPos.Text = "Minimap Byte:";
            // 
            // nDeathMinimap
            // 
            this.nDeathMinimap.Hexadecimal = true;
            this.nDeathMinimap.Location = new System.Drawing.Point(98, 182);
            this.nDeathMinimap.Maximum = new decimal(new int[] {
            63,
            0,
            0,
            0});
            this.nDeathMinimap.Name = "nDeathMinimap";
            this.nDeathMinimap.Size = new System.Drawing.Size(68, 20);
            this.nDeathMinimap.TabIndex = 14;
            this.nDeathMinimap.ValueChanged += new System.EventHandler(this.nDeathMinimap_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Death Map:";
            // 
            // nDeathDungeon
            // 
            this.nDeathDungeon.Hexadecimal = true;
            this.nDeathDungeon.Location = new System.Drawing.Point(98, 156);
            this.nDeathDungeon.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nDeathDungeon.Name = "nDeathDungeon";
            this.nDeathDungeon.Size = new System.Drawing.Size(68, 20);
            this.nDeathDungeon.TabIndex = 12;
            this.nDeathDungeon.ValueChanged += new System.EventHandler(this.nDeathDungeon_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Death Dungeon:";
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(85, 72);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 5;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // nDeathMap
            // 
            this.nDeathMap.Hexadecimal = true;
            this.nDeathMap.Location = new System.Drawing.Point(98, 130);
            this.nDeathMap.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nDeathMap.Name = "nDeathMap";
            this.nDeathMap.Size = new System.Drawing.Size(68, 20);
            this.nDeathMap.TabIndex = 10;
            this.nDeathMap.ValueChanged += new System.EventHandler(this.nDeathMap_ValueChanged);
            // 
            // bAccept
            // 
            this.bAccept.Location = new System.Drawing.Point(85, 41);
            this.bAccept.Name = "bAccept";
            this.bAccept.Size = new System.Drawing.Size(75, 23);
            this.bAccept.TabIndex = 4;
            this.bAccept.Text = "Accept";
            this.bAccept.UseVisualStyleBackColor = true;
            this.bAccept.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Death Map Tiles:";
            // 
            // nArrow
            // 
            this.nArrow.Hexadecimal = true;
            this.nArrow.Location = new System.Drawing.Point(98, 104);
            this.nArrow.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nArrow.Name = "nArrow";
            this.nArrow.Size = new System.Drawing.Size(68, 20);
            this.nArrow.TabIndex = 8;
            this.nArrow.ValueChanged += new System.EventHandler(this.nArrow_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Arrow Location:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(82, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Map:";
            // 
            // nMap
            // 
            this.nMap.Hexadecimal = true;
            this.nMap.Location = new System.Drawing.Point(119, 15);
            this.nMap.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nMap.Name = "nMap";
            this.nMap.Size = new System.Drawing.Size(68, 20);
            this.nMap.TabIndex = 2;
            this.nMap.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // pTiles
            // 
            this.pTiles.AllowMultiSelection = false;
            this.pTiles.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pTiles.BoxSize = new System.Drawing.Size(8, 8);
            this.pTiles.CanvasSize = new System.Drawing.Size(160, 128);
            this.pTiles.HoverBox = true;
            this.pTiles.HoverColor = System.Drawing.Color.White;
            this.pTiles.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            this.pTiles.Location = new System.Drawing.Point(8, 85);
            this.pTiles.Name = "pTiles";
            this.pTiles.Selectable = true;
            this.pTiles.SelectedIndex = -1;
            this.pTiles.SelectionColor = System.Drawing.Color.Red;
            this.pTiles.SelectionRectangle = new System.Drawing.Rectangle(-1, 0, 1, 1);
            this.pTiles.Size = new System.Drawing.Size(36, 12);
            this.pTiles.TabIndex = 1;
            this.pTiles.TabStop = false;
            this.pTiles.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pTiles_MouseClick);
            // 
            // pMinimap
            // 
            this.pMinimap.AllowMultiSelection = false;
            this.pMinimap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pMinimap.BoxSize = new System.Drawing.Size(8, 8);
            this.pMinimap.CanvasSize = new System.Drawing.Size(128, 128);
            this.pMinimap.HoverBox = true;
            this.pMinimap.HoverColor = System.Drawing.Color.White;
            this.pMinimap.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            this.pMinimap.Location = new System.Drawing.Point(8, 11);
            this.pMinimap.Name = "pMinimap";
            this.pMinimap.Selectable = true;
            this.pMinimap.SelectedIndex = 0;
            this.pMinimap.SelectionColor = System.Drawing.Color.Red;
            this.pMinimap.SelectionRectangle = new System.Drawing.Rectangle(0, 0, 1, 1);
            this.pMinimap.Size = new System.Drawing.Size(68, 68);
            this.pMinimap.TabIndex = 0;
            this.pMinimap.TabStop = false;
            this.pMinimap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pMinimap_MouseDown);
            this.pMinimap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pMinimap_MouseMove);
            // 
            // gBoxOverWorld
            // 
            this.gBoxOverWorld.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.gBoxOverWorld.Controls.Add(this.bAccept1);
            this.gBoxOverWorld.Controls.Add(this.bCancel1);
            this.gBoxOverWorld.Controls.Add(this.label3);
            this.gBoxOverWorld.Controls.Add(this.nPalette);
            this.gBoxOverWorld.Controls.Add(this.label2);
            this.gBoxOverWorld.Controls.Add(this.nTile);
            this.gBoxOverWorld.Controls.Add(this.pTile);
            this.gBoxOverWorld.Controls.Add(this.pMinimapO);
            this.gBoxOverWorld.Location = new System.Drawing.Point(5, 4);
            this.gBoxOverWorld.Name = "gBoxOverWorld";
            this.gBoxOverWorld.Size = new System.Drawing.Size(265, 138);
            this.gBoxOverWorld.TabIndex = 2;
            this.gBoxOverWorld.TabStop = false;
            // 
            // bAccept1
            // 
            this.bAccept1.Location = new System.Drawing.Point(144, 103);
            this.bAccept1.Name = "bAccept1";
            this.bAccept1.Size = new System.Drawing.Size(75, 23);
            this.bAccept1.TabIndex = 7;
            this.bAccept1.Text = "Accept";
            this.bAccept1.UseVisualStyleBackColor = true;
            this.bAccept1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bCancel1
            // 
            this.bCancel1.Location = new System.Drawing.Point(144, 69);
            this.bCancel1.Name = "bCancel1";
            this.bCancel1.Size = new System.Drawing.Size(75, 23);
            this.bCancel1.TabIndex = 6;
            this.bCancel1.Text = "Cancel";
            this.bCancel1.UseVisualStyleBackColor = true;
            this.bCancel1.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(141, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Palette:";
            // 
            // nPalette
            // 
            this.nPalette.Location = new System.Drawing.Point(191, 41);
            this.nPalette.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nPalette.Name = "nPalette";
            this.nPalette.Size = new System.Drawing.Size(68, 20);
            this.nPalette.TabIndex = 4;
            this.nPalette.ValueChanged += new System.EventHandler(this.nPalette_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(141, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tile:";
            // 
            // nTile
            // 
            this.nTile.Hexadecimal = true;
            this.nTile.Location = new System.Drawing.Point(191, 11);
            this.nTile.Maximum = new decimal(new int[] {
            117,
            0,
            0,
            0});
            this.nTile.Name = "nTile";
            this.nTile.Size = new System.Drawing.Size(68, 20);
            this.nTile.TabIndex = 2;
            this.nTile.ValueChanged += new System.EventHandler(this.nTile_ValueChanged);
            // 
            // pTile
            // 
            this.pTile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pTile.Location = new System.Drawing.Point(235, 91);
            this.pTile.Name = "pTile";
            this.pTile.Size = new System.Drawing.Size(12, 12);
            this.pTile.TabIndex = 1;
            this.pTile.TabStop = false;
            // 
            // pMinimapO
            // 
            this.pMinimapO.AllowMultiSelection = false;
            this.pMinimapO.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pMinimapO.BoxSize = new System.Drawing.Size(8, 8);
            this.pMinimapO.CanvasSize = new System.Drawing.Size(128, 128);
            this.pMinimapO.HoverBox = true;
            this.pMinimapO.HoverColor = System.Drawing.Color.White;
            this.pMinimapO.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            this.pMinimapO.Location = new System.Drawing.Point(3, 3);
            this.pMinimapO.Name = "pMinimapO";
            this.pMinimapO.Selectable = true;
            this.pMinimapO.SelectedIndex = 0;
            this.pMinimapO.SelectionColor = System.Drawing.Color.Red;
            this.pMinimapO.SelectionRectangle = new System.Drawing.Rectangle(0, 0, 1, 1);
            this.pMinimapO.Size = new System.Drawing.Size(132, 132);
            this.pMinimapO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pMinimapO.TabIndex = 0;
            this.pMinimapO.TabStop = false;
            this.pMinimapO.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pMinimapO_MouseDown);
            // 
            // MinimapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 146);
            this.Controls.Add(this.gBoxOverWorld);
            this.Controls.Add(this.gBoxDungeon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "MinimapEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Minimap Editor";
            this.gBoxDungeon.ResumeLayout(false);
            this.gBoxDungeon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nDeathMinimap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDeathDungeon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nDeathMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nArrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMinimap)).EndInit();
            this.gBoxOverWorld.ResumeLayout(false);
            this.gBoxOverWorld.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nPalette)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nTile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pTile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMinimapO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gBoxDungeon;
        private GridBox pTiles;
        private System.Windows.Forms.NumericUpDown nMap;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bAccept;
        private System.Windows.Forms.Button bCancel;
        public GridBox pMinimap;
        private System.Windows.Forms.GroupBox gBoxOverWorld;
        public GridBox pMinimapO;
        private System.Windows.Forms.Button bAccept1;
        private System.Windows.Forms.Button bCancel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nPalette;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nTile;
        private System.Windows.Forms.PictureBox pTile;
        private System.Windows.Forms.NumericUpDown nArrow;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nDeathMap;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nDeathMinimap;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nDeathDungeon;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblHoverPos;
    }
}