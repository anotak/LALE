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
    public partial class NewObject : Form
    {
        GBHL.GBFile gb;
        public LAObject O = new LAObject();
        bool overworld;

        public NewObject(GBHL.GBFile g, bool overWorld)
        {
            InitializeComponent();
            gb = g;
            O.x = 0;
            O.y = 0;
            overworld = overWorld;
            O.id = (byte)nObjectID.Value;
        }

        private void c3Byte_CheckedChanged(object sender, EventArgs e)
        {
            if (c3Byte.Checked)
            {
                comDirection.SelectedIndex = 0;
                comDirection.Enabled = true;
                nLength.Enabled = true;
                O.length = (byte)nLength.Value;
                O.direction = 8;
                O.is3Byte = true;
            }
            else
            {
                comDirection.SelectedIndex = -1;
                comDirection.Enabled = false;
                nLength.Enabled = false;
                O.is3Byte = false;
            }
        }

        private void comDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (O.is3Byte)
            {
                if (comDirection.SelectedIndex == 0)
                    O.direction = 8;
                else if (comDirection.SelectedIndex == 1)
                    O.direction = 0xC;
            }
        }

        private void nLength_ValueChanged(object sender, EventArgs e)
        {
            if (O.is3Byte)
                O.length = (byte)nLength.Value;
        }

        private void nObjectID_ValueChanged(object sender, EventArgs e)
        {
            O.id = (byte)nObjectID.Value;
            O.gb = gb;
            if (overworld)
                O = O.getOverworldSpecial(O);
            else
                O = O.dungeonDoors(O);
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
    }
}
