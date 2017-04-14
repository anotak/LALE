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
    public partial class ChestEditor : Form
    {

        GBHL.GBFile gb;
        public byte chestData;

        public ChestEditor(GBHL.GBFile g, byte chest)
        {
            InitializeComponent();
            gb = g;
            chestData = chest;
            nItem.Value = chest;
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

        private void nItem_ValueChanged(object sender, EventArgs e)
        {
            chestData = (byte)nItem.Value;
        }
    }
}
