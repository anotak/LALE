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
    public partial class RepointText : Form
    {

        GBHL.GBFile gb;
        public int textPointer = 0;
        int textBank;
        int textOffset;
        public int textAddress;

        public RepointText(GBHL.GBFile g)
        {
            InitializeComponent();
            gb = g;
            getAddress((int)nPointer.Value);
        }

        private void bAccept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void getAddress(int i)
        {

            textBank = (i >> 8);
            textOffset = (i & 0xFF);

            int t = (((textBank << 1) & 0xFF) * 0x100);
            int o = (textOffset << 1);
            int n = (t + o);

            gb.BufferLocation = (0x1C * 0x4000) + (n + 1);
            int loc = (gb.ReadByte() + (gb.ReadByte() * 0x100));

            gb.BufferLocation = ((0x741 + i) + (0x1C * 0x4000));
            int bank = gb.ReadByte() & 0x3F;

            gb.BufferLocation = ((bank * 0x4000) + (loc - 0x4000));
            textAddress = gb.BufferLocation;
            nAddress.Value = textAddress;
        }

        private void nPointer_ValueChanged(object sender, EventArgs e)
        {
            getAddress((int)nPointer.Value);
            textPointer = (int)nPointer.Value;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void nAddress_ValueChanged(object sender, EventArgs e)
        {
            textAddress = (int)nAddress.Value;
        }
    }
}
