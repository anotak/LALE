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
    public partial class MinibossEditor : Form
    {
        public GBHL.GBFile gb;
        bool overWorld;

        public MinibossEditor(byte[] buff, bool overworld, int map)
        {
            InitializeComponent();
            gb = new GBHL.GBFile(buff);
            overWorld = overworld;
            if (!overworld)
                comDungeons.SelectedIndex = 0;
            else
            {
                comDungeons.Enabled = false;
                label2.Text = "Map:";
                label3.Text = "Warp Map:";
                nMap1.Value = map;
            }
        }

        private void bAccept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void comDungeons_SelectedIndexChanged(object sender, EventArgs e)
        {
            gb.BufferLocation = 0x64201 + (comDungeons.SelectedIndex * 2);
            nMap1.Value = gb.ReadByte();
            nMap2.Value = gb.ReadByte();
        }

        private void nMap1_ValueChanged(object sender, EventArgs e)
        {
            if (!overWorld)
            {
                gb.BufferLocation = 0x64201 + (comDungeons.SelectedIndex * 2);
                gb.WriteByte((byte)nMap1.Value);
            }
            else
            {
                gb.BufferLocation = 0x65C6A + (int)nMap1.Value;
                nMap2.Value = gb.ReadByte();
            }
        }

        private void nMap2_ValueChanged(object sender, EventArgs e)
        {
            if (!overWorld)
            {
                gb.BufferLocation = 0x64202 + (comDungeons.SelectedIndex * 2);
                gb.WriteByte((byte)nMap2.Value);
            }
            else
            {
                gb.BufferLocation = 0x65C6A + (int)nMap1.Value;
                gb.WriteByte((byte)nMap2.Value);
            }
        }
    }
}
