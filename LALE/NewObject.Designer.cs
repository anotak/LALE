namespace LALE
{
    partial class NewObject
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
            this.c3Byte = new System.Windows.Forms.CheckBox();
            this.nObjectID = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nLength = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.comDirection = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nObjectID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nLength)).BeginInit();
            this.SuspendLayout();
            // 
            // c3Byte
            // 
            this.c3Byte.AutoSize = true;
            this.c3Byte.Location = new System.Drawing.Point(15, 12);
            this.c3Byte.Name = "c3Byte";
            this.c3Byte.Size = new System.Drawing.Size(89, 17);
            this.c3Byte.TabIndex = 0;
            this.c3Byte.Text = "3-Byte Object";
            this.c3Byte.UseVisualStyleBackColor = true;
            this.c3Byte.CheckedChanged += new System.EventHandler(this.c3Byte_CheckedChanged);
            // 
            // nObjectID
            // 
            this.nObjectID.Hexadecimal = true;
            this.nObjectID.Location = new System.Drawing.Point(78, 35);
            this.nObjectID.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nObjectID.Name = "nObjectID";
            this.nObjectID.Size = new System.Drawing.Size(120, 20);
            this.nObjectID.TabIndex = 1;
            this.nObjectID.ValueChanged += new System.EventHandler(this.nObjectID_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Object ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Length:";
            // 
            // nLength
            // 
            this.nLength.Enabled = false;
            this.nLength.Location = new System.Drawing.Point(78, 105);
            this.nLength.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nLength.Name = "nLength";
            this.nLength.Size = new System.Drawing.Size(120, 20);
            this.nLength.TabIndex = 3;
            this.nLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nLength.ValueChanged += new System.EventHandler(this.nLength_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Direction:";
            // 
            // comDirection
            // 
            this.comDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comDirection.Enabled = false;
            this.comDirection.FormattingEnabled = true;
            this.comDirection.Items.AddRange(new object[] {
            "Horizontal",
            "Vertical"});
            this.comDirection.Location = new System.Drawing.Point(78, 70);
            this.comDirection.Name = "comDirection";
            this.comDirection.Size = new System.Drawing.Size(120, 21);
            this.comDirection.TabIndex = 6;
            this.comDirection.SelectedIndexChanged += new System.EventHandler(this.comDirection_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 139);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(114, 139);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Create";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // NewObject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(210, 174);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comDirection);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nLength);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nObjectID);
            this.Controls.Add(this.c3Byte);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "NewObject";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create a new Object";
            ((System.ComponentModel.ISupportInitialize)(this.nObjectID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox c3Byte;
        private System.Windows.Forms.NumericUpDown nObjectID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nLength;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comDirection;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}