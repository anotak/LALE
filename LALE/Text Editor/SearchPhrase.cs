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
    public partial class SearchPhrase : Form
    {

        GBHL.GBFile gb;
        static readonly int[] Empty = new int[0];
        string keyWord;
        byte[] keyBytes;
        byte[] buffer;
        public List<int> locations;
        public int address;

        public SearchPhrase(GBHL.GBFile g)
        {
            InitializeComponent();
            gb = g;
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void bSearch_Click(object sender, EventArgs e)
        {

            lBox.Items.Clear();
            lBox.SelectedIndex = -1;
            bAccept.Enabled = false;
            if (tSearch.TextLength >= 3)
            {
                keyWord = tSearch.Text;
                keyBytes = Encoding.ASCII.GetBytes(keyWord);
                buffer = gb.Buffer.ToArray();

                locations = new List<int>();
                int i = Array.IndexOf<byte>(buffer, keyBytes[0], 0);
                while (i >= 0 && i <= buffer.Length - keyBytes.Length)
                {
                    byte[] segment = new byte[keyBytes.Length];
                    Buffer.BlockCopy(buffer, i, segment, 0, keyBytes.Length);
                    if (segment.SequenceEqual<byte>(keyBytes))
                        locations.Add(i);
                    i = Array.IndexOf<byte>(buffer, keyBytes[0], i + keyBytes.Length);
                }

                foreach (int b in locations)
                    lBox.Items.Add(b.ToString("X"));

                if (lBox.Items.Count == 0)
                    MessageBox.Show("No strings found.");
            }
            else
            {
                MessageBox.Show("The key word/phrase must contain at least 3 letters and remember the search is case sensitive.");
            }
        }

        private void lBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lBox.SelectedIndex >= 0)
                bAccept.Enabled = true;
            else
                bAccept.Enabled = false;
        }

        private void bAccept_Click(object sender, EventArgs e)
        {
            address = locations[lBox.SelectedIndex];
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void tSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bSearch.PerformClick();
            }
        }
    }
}
