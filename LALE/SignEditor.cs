using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LALE
{
    public partial class SignEditor : Form
    {
        public GBHL.GBFile gb;

        public SignEditor(byte[] buf, int map)
        {
            InitializeComponent();
            gb = new GBHL.GBFile(buf);
            nMap.Value = map;
        }

        private void nMap_ValueChanged(object sender, EventArgs e)
        {
            nPointer.Enabled = true;

            gb.BufferLocation = 0x51118 + (int)nMap.Value;
            byte b = gb.ReadByte();
            int p = 1;
            if (b == 0x83)
                p = 0;
            if (b == 0x2D)
                p = 2;

            nPointer.Value = b;
            label2.Text = "Text Pointer:  0x0" + p.ToString();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void bAccept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void nPointer_ValueChanged(object sender, EventArgs e)
        {
            if (nPointer.Value == 0x83)
                label2.Text = "Text Pointer:  0x00";
            else if (nPointer.Value == 0x2D)
                label2.Text = "Text Pointer:  0x02";
            else
                label2.Text = "Text Pointer:  0x01";
            gb.BufferLocation = 0x51118 + (int)nMap.Value;
            gb.WriteByte((byte)nPointer.Value);
        }
    }
}
