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
    public partial class TextEditor : Form
    {
        char[] hexChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        Color[] bwPalette = new Color[] { Color.FromArgb(248, 248, 168), Color.FromArgb(152, 120, 180), Color.FromArgb(56, 24, 90), Color.Black };
        public GBHL.GBFile gb;
        int textBank;
        int textOffset;
        byte[] textBytes;
        byte[] textASCII;
        int textLoc;
        List<int> textAddresses;

        public TextEditor(byte[] buf)
        {
            InitializeComponent();
            gb = new GBHL.GBFile(buf);
            getTextAddresses();
            nAddress.Value = textAddresses[0];
        }

        private void getTextAddresses()
        {
            textAddresses = new List<int>();
            for (int i = 0; i < 0x2B0; i++)
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

                textAddresses.Add(gb.BufferLocation);
            }
        }

        private void bGetText_Click(object sender, EventArgs e)
        {
            //tTextBox.Clear();
            //pText.Image = null;
            int i = 0;
            cQuestion.Enabled = true;
            cQuestion.Checked = false;

            textLoc = textAddresses[(int)nTextBank.Value];
            gb.BufferLocation = textLoc;
            byte h = gb.ReadByte();
            while (h != 0xFF)
            {
                i++;
                h = gb.ReadByte();
                if (h == 0xFE)
                    break;
            }
            int length = i;
            textASCII = new byte[i];
            if (length % 16 != 0)
            {
                int add = 16 - (length % 16);
                i += add;
                textBytes = new byte[0x10 * i];
                for (int black = 0; black < (add * 0x10); black++)
                    textBytes[(length * 0x10) + black] = 0xFF;
                length = i;
            }
            else
                textBytes = new byte[0x10 * i];
            i = 0;
            int g;
            gb.BufferLocation = textLoc;
            h = gb.ReadByte();
            while (h != 0xFF)
            {
                if (h == 0xFE)
                {
                    cQuestion.Checked = true;
                    break;
                }
                textASCII[i] = h;
                if (h == 0x20)
                {
                    for (int i2 = 0; i2 < 0x10; i2++)
                        textBytes[(i * 0x10) + i2] = 0xFF;
                    i++;
                    h = gb.ReadByte();
                    continue;
                }
                g = (gb.ReadByte(0x70641 + h) << 4);
                gb.BufferLocation = g + 0x3D000;
                for (int i2 = 0; i2 < 0x10; i2++)
                    textBytes[(i * 0x10) + i2] = gb.ReadByte();
                i++;
                gb.BufferLocation = textLoc + i;
                h = gb.ReadByte();
            }

            //byte[, ,] data = new byte[length, 8, 8];
            //gb.ReadTiles(16, length / 16, textBytes, ref data);
            //Bitmap bmp = new Bitmap(128, length / 2);
            //FastPixel fp = new FastPixel(bmp);
            //fp.rgbValues = new byte[128 * (length / 2) * 4];
            //fp.Lock();
            //for (int q = 0; q < length; q++)
            //{
            //    for (int y = 0; y < 8; y++)
            //    {
            //        for (int x = 0; x < 8; x++)
            //        {
            //            fp.SetPixel(x + ((q % 16) * 8), y + ((q / 16) * 8), bwPalette[data[q, x, y]]);
            //        }
            //    }
            //}
            //fp.Unlock(true);
            //pText.Image = bmp;

            nAddress.Value = textLoc;
        }

        private void tTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                pText.Image = null;
                int index = 0;
                int skip = 0;
                int r = 0;
                textASCII = new byte[tTextBox.Text.Length];
                foreach (char c in tTextBox.Text)
                {
                    if (c == '#')
                    {
                        if (index + 1 != textASCII.Length)
                        {
                            char ch = tTextBox.Text[index + r + 1];
                            if (Array.IndexOf(hexChar, ch) != -1 && Array.IndexOf(hexChar, tTextBox.Text[index + r + 2]) != -1)
                            {
                                int b1 = Array.IndexOf(hexChar, ch);
                                int b2 = Array.IndexOf(hexChar, tTextBox.Text[index + r + 2]);
                                int b = ((b1 << 4) + b2);
                                textASCII[index] = (byte)b;
                                r += 2;
                                byte[] textASCIIp = new byte[tTextBox.Text.Length - r];
                                skip = 2;
                                Array.Copy(textASCII, textASCIIp, tTextBox.Text.Length - r);
                                textASCII = textASCIIp;
                                index++;
                                continue;
                            }
                        }
                    }
                    if (skip != 0)
                    {
                        skip--;
                        continue;
                    }
                    textASCII[index] = Convert.ToByte(c);
                    index++;
                }
                int i = textASCII.Length;
                int length = i;
                if (length != 0)
                {
                    if (length % 16 != 0)
                    {
                        int add = 16 - (length % 16);
                        i += add;
                        textBytes = new byte[0x10 * i];
                        for (int black = 0; black < (add * 0x10); black++)
                            textBytes[(length * 0x10) + black] = 0xFF;
                        length = i;
                    }
                    else
                        textBytes = new byte[0x10 * i];

                    i = 0;
                    int g;

                    foreach (byte q in textASCII)
                    {
                        if (q == 0x20)
                        {
                            for (int i2 = 0; i2 < 0x10; i2++)
                                textBytes[(i * 0x10) + i2] = 0xFF;
                            i++;
                            continue;
                        }
                        g = (gb.ReadByte(0x70641 + q) << 4);
                        gb.BufferLocation = g + 0x3D000;
                        for (int i2 = 0; i2 < 0x10; i2++)
                            textBytes[(i * 0x10) + i2] = gb.ReadByte();
                        i++;
                        gb.BufferLocation = textLoc + i;
                    }

                    byte[, ,] data = new byte[length, 8, 8];
                    gb.ReadTiles(16, length / 16, textBytes, ref data);
                    Bitmap bmp = new Bitmap(128, length / 2);
                    FastPixel fp = new FastPixel(bmp);
                    fp.rgbValues = new byte[128 * (length / 2) * 4];
                    fp.Lock();
                    for (int q = 0; q < length; q++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            for (int x = 0; x < 8; x++)
                            {
                                fp.SetPixel(x + ((q % 16) * 8), y + ((q / 16) * 8), bwPalette[data[q, x, y]]);
                            }
                        }
                    }
                    fp.Unlock(true);

                    pText.Image = bmp;
                }
                else
                    pText.Image = null;
            }
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

        private void tSearch_Click(object sender, EventArgs e)
        {
            SearchPhrase SP = new SearchPhrase(gb);
            SP.ShowDialog();
            if (SP.DialogResult == DialogResult.OK)
            {
                textLoc = SP.address;
                nAddress.Value = textLoc;
            }
        }

        private void tRepoint_Click(object sender, EventArgs e)
        {
            RepointText RT = new RepointText(gb);
            RT.ShowDialog();
            if (RT.DialogResult == DialogResult.OK)
            {
                int address = RT.textAddress;
                int pointer = RT.textPointer;
                int bank = address / 0x4000;
                int writeAddress = (address - (bank * 0x4000)) + 0x4000;

                textBank = (pointer >> 8);
                textOffset = (pointer & 0xFF);

                int t = (((textBank << 1) & 0xFF) * 0x100);
                int o = (textOffset << 1);
                int n = (t + o);

                textBank = (writeAddress >> 8);
                textOffset = (writeAddress & 0xFF);

                gb.BufferLocation = (0x1C * 0x4000) + (n + 1);
                gb.WriteByte((byte)textOffset);
                gb.WriteByte((byte)textBank);

                gb.BufferLocation = ((0x741 + pointer) + (0x1C * 0x4000));
                gb.WriteByte((byte)bank);

                getTextAddresses();

            }
        }

        private void tSave_Click(object sender, EventArgs e)
        {
            gb.BufferLocation = textLoc;
            int index = 0;
            int skip = 0;
            int r = 0;
            textASCII = new byte[tTextBox.Text.Length];
            foreach (char c in tTextBox.Text)
            {
                if (c == '#')
                {
                    char ch = tTextBox.Text[index + r + 1];
                    if (Array.IndexOf(hexChar, ch) != -1 && Array.IndexOf(hexChar, tTextBox.Text[index + r + 2]) != -1)
                    {
                        int b1 = Array.IndexOf(hexChar, ch);
                        int b2 = Array.IndexOf(hexChar, tTextBox.Text[index + r + 2]);
                        int b = ((b1 << 4) + b2);
                        textASCII[index] = (byte)b;
                        r += 2;
                        byte[] textASCIIp = new byte[tTextBox.Text.Length - r];
                        skip = 2;
                        Array.Copy(textASCII, textASCIIp, tTextBox.Text.Length - r);
                        textASCII = textASCIIp;
                        index++;
                        continue;
                    }
                }
                if (skip != 0)
                {
                    skip--;
                    continue;
                }
                textASCII[index] = Convert.ToByte(c);
                index++;
            }

            gb.WriteBytes(textASCII);

            if (cQuestion.Checked == true)
                gb.WriteByte(0xFE);
            else
                gb.WriteByte(0xFF);


        }

        private void nAddress_ValueChanged(object sender, EventArgs e)
        {
            tTextBox.Clear();
            pText.Image = null;
            cQuestion.Enabled = true;
            cQuestion.Checked = false;
            int index;

            textLoc = (int)nAddress.Value;
            gb.BufferLocation = textLoc;
            int i = 0;
            byte h = gb.ReadByte();
            while (h != 0xFF)
            {
                i++;
                h = gb.ReadByte();
                if (h == 0xFE)
                    break;
            }

            int length = i;
            if (length != 0)
            {
                textASCII = new byte[i];
                if (length % 16 != 0)
                {
                    int add = 16 - (length % 16);
                    i += add;
                    textBytes = new byte[0x10 * i];
                    for (int black = 0; black < (add * 0x10); black++)
                        textBytes[(length * 0x10) + black] = 0xFF;
                    length = i;
                }
                else
                    textBytes = new byte[0x10 * i];
                i = 0;
                int g;
                gb.BufferLocation = textLoc;
                h = gb.ReadByte();
                while (h != 0xFF)
                {
                    if (h == 0xFE)
                    {
                        cQuestion.Checked = true;
                        break;
                    }
                    textASCII[i] = h;
                    if (h == 0x20)
                    {
                        for (int i2 = 0; i2 < 0x10; i2++)
                            textBytes[(i * 0x10) + i2] = 0xFF;
                        i++;
                        h = gb.ReadByte();
                        continue;
                    }
                    g = (gb.ReadByte(0x70641 + h) << 4);
                    gb.BufferLocation = g + 0x3D000;
                    for (int i2 = 0; i2 < 0x10; i2++)
                        textBytes[(i * 0x10) + i2] = gb.ReadByte();
                    i++;
                    gb.BufferLocation = textLoc + i;
                    h = gb.ReadByte();
                }

                byte[, ,] data = new byte[length, 8, 8];
                gb.ReadTiles(16, length / 16, textBytes, ref data);
                Bitmap bmp = new Bitmap(128, length / 2);
                FastPixel fp = new FastPixel(bmp);
                fp.rgbValues = new byte[128 * (length / 2) * 4];
                fp.Lock();
                for (int q = 0; q < length; q++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        for (int x = 0; x < 8; x++)
                        {
                            fp.SetPixel(x + ((q % 16) * 8), y + ((q / 16) * 8), bwPalette[data[q, x, y]]);
                        }
                    }
                }
                fp.Unlock(true);

                pText.Image = bmp;

                StringBuilder builder = new StringBuilder();
                for (int s = 0; s < i; s++)
                {
                    if ((textASCII[s] >> 4) > 7)
                    {
                        builder.Append('#');
                        builder.Append(hexChar[textASCII[s] >> 4]);
                        builder.Append(hexChar[textASCII[s] & 0xF]);
                        continue;
                    }
                    builder.Append((char)textASCII[s]);
                }
                tTextBox.Text = builder.ToString();
            }
            else
                pText.Image = null;
            try
            {
                index = textAddresses.FindIndex(item => item == textLoc);
                nTextBank.Value = index;
            }
            catch
            {
                ;
            }

        }
    }
}
