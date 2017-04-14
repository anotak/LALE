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
    public partial class OwlStatueEditor : Form
    {
        public GBHL.GBFile gb;
        int index;

        public OwlStatueEditor(byte[] buf)
        {
            InitializeComponent();
            gb = new GBHL.GBFile(buf);
            cDungeon.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lMaps.Items.Clear();
            index = -1;
            nMap.Enabled = false;
            if (cDungeon.SelectedIndex != 8)
                nPointer.Enabled = false;
            else
                nPointer.Enabled = true;
            nMap.Value = 0;
            nPointer.Value = 0;
            int dungeon = cDungeon.SelectedIndex;
            if (dungeon != 0x8)
            {
                gb.BufferLocation = 0xD8A3C + ((dungeon << 1) & 0xFF);
                gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;

                int q = 0;
                for (int i = 0; i < 3; i++)
                {
                    q++;
                    lMaps.Items.Add(q.ToString());
                }
                //gb.BufferLocation = 0xD8A14 + ((dungeon << 1) & 0xFF);
                //gb.BufferLocation = (gb.Get2BytePointerAddress(gb.BufferLocation).Address) + q;
            }
            else
            {
                nMap.Enabled = false;
                nPointer.Value = gb.ReadByte(0x61EB5);
            }
        }

        private void nMap_ValueChanged(object sender, EventArgs e)
        {
            if (cDungeon.SelectedIndex != 8 && index != -1)
            {
                gb.BufferLocation = 0xD8A3C + ((cDungeon.SelectedIndex << 1) & 0xFF);
                gb.BufferLocation = (gb.Get2BytePointerAddress(gb.BufferLocation).Address) + index;
                gb.WriteByte((byte)nMap.Value);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            index = lMaps.SelectedIndex;
            if (index != -1)
            {
                nMap.Enabled = true;
                nPointer.Enabled = true;
            }
            if (cDungeon.SelectedIndex != 8)
            {
                gb.BufferLocation = 0xD8A14 + ((cDungeon.SelectedIndex << 1) & 0xFF);
                gb.BufferLocation = (gb.Get2BytePointerAddress(gb.BufferLocation).Address) + lMaps.SelectedIndex;
                nPointer.Value = gb.ReadByte();

                gb.BufferLocation = 0xD8A3C + ((cDungeon.SelectedIndex << 1) & 0xFF);
                gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                nMap.Value = gb.ReadByte(gb.BufferLocation + lMaps.SelectedIndex);
            }
        }

        private void bAccept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void nPointer_ValueChanged(object sender, EventArgs e)
        {
            if (index != -1)
            {
                if (cDungeon.SelectedIndex != 8)
                {
                    gb.BufferLocation = 0xD8A14 + ((cDungeon.SelectedIndex << 1) & 0xFF);
                    gb.BufferLocation = (gb.Get2BytePointerAddress(gb.BufferLocation).Address) + lMaps.SelectedIndex;
                    gb.WriteByte((byte)nPointer.Value);
                }
                else
                    gb.WriteByte(0x61EB5, (byte)nPointer.Value);
            }
        }
    }
}
