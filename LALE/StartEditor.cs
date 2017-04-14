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
    public partial class StartEditor : Form
    {
        GBHL.GBFile gb;
        public byte dungeon;
        public byte map;
        public byte xPos;
        public byte yPos;
        public bool overworld;

        public StartEditor(GBHL.GBFile g)
        {
            InitializeComponent();
            gb = g;
            getStartData();
        }

        public void getStartData()
        {
            gb.BufferLocation = 0x53DE;
            xPos = gb.ReadByte();
            gb.BufferLocation = 0x53E3;
            yPos = gb.ReadByte();
            gb.BufferLocation = 0x53DA;
            dungeon = gb.ReadByte();
            gb.BufferLocation = 0x53CB;
            map = gb.ReadByte();
            gb.BufferLocation = 0x53D5;
            byte b = gb.ReadByte();
            if (b == 0)
                overworld = true;
            else
                overworld = false;

            nDungeon.Value = dungeon;
            nMap.Value = map;
            nLinkXPos.Value = xPos;
            nLinkYPos.Value = yPos;
            cOverworld.Checked = overworld;
        }

        private void cOverworld_CheckedChanged(object sender, EventArgs e)
        {
            if (cOverworld.Checked)
            {
                nDungeon.Enabled = false;
                overworld = true;
            }
            else
            {
                nDungeon.Enabled = true;
                overworld = false;
            }
            nDungeon.Value = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void nDungeon_ValueChanged(object sender, EventArgs e)
        {
            dungeon = (byte)nDungeon.Value;
        }

        private void nLinkYPos_ValueChanged(object sender, EventArgs e)
        {
            yPos = (byte)nLinkYPos.Value;
        }

        private void nLinkXPos_ValueChanged(object sender, EventArgs e)
        {
            xPos = (byte)nLinkXPos.Value;
        }

        private void nMap_ValueChanged(object sender, EventArgs e)
        {
            map = (byte)nMap.Value;
        }
    }
}
