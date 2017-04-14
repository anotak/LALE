namespace LALE
{
    partial class TextEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextEditor));
            this.nTextBank = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tTextBox = new System.Windows.Forms.TextBox();
            this.bCancel = new System.Windows.Forms.Button();
            this.bAccept = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tRepoint = new System.Windows.Forms.ToolStripButton();
            this.tSearch = new System.Windows.Forms.ToolStripButton();
            this.cQuestion = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchPhraseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nAddress = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.pText = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nTextBank)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pText)).BeginInit();
            this.SuspendLayout();
            // 
            // nTextBank
            // 
            this.nTextBank.Hexadecimal = true;
            this.nTextBank.Location = new System.Drawing.Point(88, 35);
            this.nTextBank.Maximum = new decimal(new int[] {
            687,
            0,
            0,
            0});
            this.nTextBank.Name = "nTextBank";
            this.nTextBank.Size = new System.Drawing.Size(120, 20);
            this.nTextBank.TabIndex = 0;
            this.nTextBank.ValueChanged += new System.EventHandler(this.bGetText_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Text Pointer:";
            // 
            // tTextBox
            // 
            this.tTextBox.Location = new System.Drawing.Point(15, 113);
            this.tTextBox.Multiline = true;
            this.tTextBox.Name = "tTextBox";
            this.tTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tTextBox.Size = new System.Drawing.Size(298, 172);
            this.tTextBox.TabIndex = 5;
            this.tTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tTextBox_KeyDown);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(227, 35);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(86, 23);
            this.bCancel.TabIndex = 7;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bAccept
            // 
            this.bAccept.Location = new System.Drawing.Point(227, 75);
            this.bAccept.Name = "bAccept";
            this.bAccept.Size = new System.Drawing.Size(86, 23);
            this.bAccept.TabIndex = 8;
            this.bAccept.Text = "Accept";
            this.bAccept.UseVisualStyleBackColor = true;
            this.bAccept.Click += new System.EventHandler(this.bAccept_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tSave,
            this.toolStripSeparator1,
            this.tRepoint,
            this.tSearch});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(333, 25);
            this.toolStrip1.TabIndex = 10;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tSave
            // 
            this.tSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSave.Image = ((System.Drawing.Image)(resources.GetObject("tSave.Image")));
            this.tSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSave.Name = "tSave";
            this.tSave.Size = new System.Drawing.Size(23, 22);
            this.tSave.Text = "toolStripButton1";
            this.tSave.ToolTipText = "Save Text Changes";
            this.tSave.Click += new System.EventHandler(this.tSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tRepoint
            // 
            this.tRepoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tRepoint.Image = ((System.Drawing.Image)(resources.GetObject("tRepoint.Image")));
            this.tRepoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tRepoint.Name = "tRepoint";
            this.tRepoint.Size = new System.Drawing.Size(23, 22);
            this.tRepoint.Text = "toolStripButton1";
            this.tRepoint.ToolTipText = "Repoint Text Pointers";
            this.tRepoint.Click += new System.EventHandler(this.tRepoint_Click);
            // 
            // tSearch
            // 
            this.tSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tSearch.Image = ((System.Drawing.Image)(resources.GetObject("tSearch.Image")));
            this.tSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tSearch.Name = "tSearch";
            this.tSearch.Size = new System.Drawing.Size(23, 22);
            this.tSearch.Text = "toolStripButton1";
            this.tSearch.ToolTipText = "Search Phrase";
            this.tSearch.Click += new System.EventHandler(this.tSearch_Click);
            // 
            // cQuestion
            // 
            this.cQuestion.AutoSize = true;
            this.cQuestion.Enabled = false;
            this.cQuestion.Location = new System.Drawing.Point(15, 536);
            this.cQuestion.Name = "cQuestion";
            this.cQuestion.Size = new System.Drawing.Size(107, 17);
            this.cQuestion.TabIndex = 12;
            this.cQuestion.Text = "Yes/No Question";
            this.cQuestion.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.searchPhraseToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(329, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.tSave_Click);
            // 
            // searchPhraseToolStripMenuItem
            // 
            this.searchPhraseToolStripMenuItem.Name = "searchPhraseToolStripMenuItem";
            this.searchPhraseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.searchPhraseToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.searchPhraseToolStripMenuItem.Text = "Search Phrase";
            this.searchPhraseToolStripMenuItem.Click += new System.EventHandler(this.tSearch_Click);
            // 
            // nAddress
            // 
            this.nAddress.Hexadecimal = true;
            this.nAddress.Location = new System.Drawing.Point(88, 75);
            this.nAddress.Maximum = new decimal(new int[] {
            1048575,
            0,
            0,
            0});
            this.nAddress.Name = "nAddress";
            this.nAddress.Size = new System.Drawing.Size(120, 20);
            this.nAddress.TabIndex = 14;
            this.nAddress.ValueChanged += new System.EventHandler(this.nAddress_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Text Address:";
            // 
            // pText
            // 
            this.pText.BackColor = System.Drawing.Color.Black;
            this.pText.Location = new System.Drawing.Point(15, 291);
            this.pText.Name = "pText";
            this.pText.Size = new System.Drawing.Size(298, 236);
            this.pText.TabIndex = 16;
            this.pText.TabStop = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(398, 147);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(256, 203);
            this.richTextBox1.TabIndex = 18;
            this.richTextBox1.Text = "";
            // 
            // TextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 561);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.pText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nAddress);
            this.Controls.Add(this.cQuestion);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.bAccept);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.tTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nTextBank);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Link\'s Awakening Text Editor";
            ((System.ComponentModel.ISupportInitialize)(this.nTextBank)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pText)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nTextBank;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tTextBox;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bAccept;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tSearch;
        private System.Windows.Forms.CheckBox cQuestion;
        private System.Windows.Forms.ToolStripButton tRepoint;
        private System.Windows.Forms.ToolStripButton tSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchPhraseToolStripMenuItem;
        private System.Windows.Forms.NumericUpDown nAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pText;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}