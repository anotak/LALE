using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GBHL;

namespace LALE
{
    public partial class WarpEditor : Form
    {
        public List<Warps> warpList = new List<Warps>();
        int index;

        public WarpEditor(List<Warps> warps)
        {
            InitializeComponent();
            warpList = warps;
            nIndex.Maximum = warpList.Count - 1;
            index = -1;
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void bAccept_Click(object sender, EventArgs e)
        {
            //Form1.WL = warpList;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void nIndex_ValueChanged(object sender, EventArgs e)
        {
            if (nIndex.Value == -1)
            {
                index = -1;
                nDestX.Value = 0;
                nDestY.Value = 0;
                nRegion.Value = 0;
                nMap.Value = 0;
                comboBox1.SelectedIndex = 0;
                nDestX.Enabled = false;
                nDestY.Enabled = false;
                nRegion.Enabled = false;
                comboBox1.Enabled = false;
                nMap.Enabled = false;
                return;
            }
            nMap.Enabled = true;
            nDestX.Enabled = true;
            nDestY.Enabled = true;
            nRegion.Enabled = true;
            comboBox1.Enabled = true;
            index = (int)nIndex.Value;
            comboBox1.SelectedIndex = warpList[index].type;
            nRegion.Value = warpList[index].region;
            nMap.Value = warpList[index].map;
            nDestX.Value = warpList[index].x;
            nDestY.Value = warpList[index].y;
        }

        private void nRegion_ValueChanged(object sender, EventArgs e)
        {
            if (index != -1)
                warpList[index].region = (byte)nRegion.Value;
        }

        private void nMap_ValueChanged(object sender, EventArgs e)
        {
            if (index != -1)
                warpList[index].map = (byte)nMap.Value;
        }

        private void nDestX_ValueChanged(object sender, EventArgs e)
        {
            if (index != -1)
                warpList[index].x = (byte)nDestX.Value;
        }

        private void nDestY_ValueChanged(object sender, EventArgs e)
        {
            if (index != -1)
                warpList[index].y = (byte)nDestY.Value;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (index != -1)
                warpList[index].type = (byte)comboBox1.SelectedIndex;
        }

        private void bCreateWarp_Click(object sender, EventArgs e)
        {
            Warps w = new Warps();
            w.region = 0;
            w.map = 0;
            w.type = 0;
            w.x = 0;
            w.y = 0;
            warpList.Add(w);
            nIndex.Maximum++;
        }

        private void bDeleteWarp_Click(object sender, EventArgs e)
        {
            if (nIndex.Value == -1)
                return;
            warpList.RemoveAt((int)nIndex.Value);
            if ((nIndex.Maximum - 1) == -1)
            {
                index = -1;
                nIndex.Maximum--;
                return;
            }
            nIndex.Maximum--;
            reloadWarp();
        }

        private void reloadWarp()
        {
            comboBox1.SelectedIndex = warpList[index].type;
            nRegion.Value = warpList[index].region;
            nMap.Value = warpList[index].map;
            nDestX.Value = warpList[index].x;
            nDestY.Value = warpList[index].y;
        }
    }
}
