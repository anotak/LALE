using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GBHL;

namespace LALE
{
    public partial class MinimapEditor : Form
    {
        public GBFile gb;
        Color[] bwPalette = new Color[] { Color.White, Color.LightGray, Color.FromArgb(44, 50, 89), Color.Black };
        Color[] chestPalette = new Color[] { Color.FromArgb(248, 248, 168), Color.FromArgb(216, 168, 32), Color.FromArgb(136, 80, 0), Color.Black };
        Color[,] palette = new Color[8, 4];
        byte[, ,] graphicsData;
        public byte[] minimapData;
        public byte[] roomIndexes;
        byte selectedTile;
        byte selectedPal;
        int index;
        int dung;
        public byte[] overworldpal = new byte[256];


        public MinimapEditor(GBHL.GBFile g, Image map, byte[] roomindexes, byte[, ,] graphicsdata, byte[] minimapdata, bool overWorld, byte[] overworldPal, byte dungeon)
        {
            InitializeComponent();
            gb = g;
            dung = dungeon;
            if (!overWorld)
            {
                this.Width = 215;
                this.Height = 263;
                gBoxDungeon.Visible = true;
                gBoxOverWorld.Visible = false;
                gBoxDungeon.Location = new Point(4, 4);
                pMinimap.Image = map;
                roomIndexes = roomindexes;
                if (dungeon == 0xFF)
                {
                    nMap.Maximum = 0x15;
                    gb.BufferLocation = 0x19B3;
                    nDeathMinimap.Value = gb.ReadByte();
                    gb.BufferLocation = 0x50E3D;
                    nDeathDungeon.Value = gb.ReadByte();
                    nDeathMap.Value = gb.ReadByte();
                    gb.BufferLocation = 0x6E8D + (0xF * 2);
                    nArrow.Value = gb.ReadByte() - 0xB;
                }
                else
                {
                    gb.BufferLocation = 0x50E41 + dungeon;
                    nDeathMinimap.Value = gb.ReadByte();
                    gb.BufferLocation = 0x6E8D + (dungeon * 2);
                    nArrow.Value = gb.ReadByte() - 0xB;
                    gb.BufferLocation = 0x50DF2 + (dungeon * 5);
                    nDeathDungeon.Value = gb.ReadByte();
                    nDeathMap.Value = gb.ReadByte();
                }
            }
            else
            {
                gb.BufferLocation = 0x8786E;
                for (int i = 0; i < 8; i++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        palette[i, k] = GetColor(gb.BufferLocation);
                    }
                }
                pMinimapO.Image = map;
                overworldpal = overworldPal;
            }
            graphicsData = graphicsdata;
            minimapData = minimapdata;

            if (!overWorld)
                drawDungeonItems();
            else
                pTile.Image = drawOverworldTile();
        }

        private Color GetColor(int offset)
        {
            int value = gb.ReadByte(offset) + (gb.ReadByte(offset + 1) << 8);
            UInt16 color2B = (UInt16)value;
            int red = (color2B & 31) << 3;
            color2B >>= 5;
            int green = (color2B & 31) << 3;
            color2B >>= 5;
            int blue = (color2B & 31) << 3;
            return Color.FromArgb(red, green, blue);
        }

        private void drawDungeonItems()
        {
            Bitmap bmp = new Bitmap(128, 128);
            FastPixel fp = new FastPixel(bmp);
            fp.rgbValues = new byte[128 * 128 * 4];
            fp.Lock();
            for (int x = 0; x < 4; x++)
            {
                for (int y1 = 0; y1 < 8; y1++)
                {
                    for (int x1 = 0; x1 < 8; x1++)
                    {
                        if (x == 0)
                            fp.SetPixel(x1 + (x * 8), y1, chestPalette[graphicsData[0, x1, y1]]);
                        else if (x == 3)
                            fp.SetPixel(x1 + (x * 8), y1, Color.FromArgb(44, 50, 89));
                        else
                            fp.SetPixel(x1 + (x * 8), y1, bwPalette[graphicsData[x, x1, y1]]);
                    }
                }
            }
            fp.Unlock(true);
            pTiles.Image = bmp;

        }

        private Bitmap drawOverworldTile()
        {
            Bitmap bmp = new Bitmap(8, 8);
            FastPixel fp = new FastPixel(bmp);
            fp.rgbValues = new byte[8 * 8 * 4];
            fp.Lock();
            byte i;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    byte miniD = (byte)nTile.Value;
                    byte pal = (byte)nPalette.Value;
                    if (miniD == 0x70 || miniD == 0x71 || miniD == 0x72 || miniD == 0x73 || miniD == 0x74 || miniD == 0x75)
                        i = (byte)(miniD - 0x66);
                    else
                        i = (byte)(miniD + 16);
                    fp.SetPixel(x, y, palette[pal, graphicsData[i, x, y]]);
                }
            }
            fp.Unlock(true);
            return bmp;

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            roomIndexes[index] = (byte)nMap.Value;
        }

        private void pMinimap_MouseDown(object sender, MouseEventArgs e)
        {
            int s = (e.X / 8) + ((e.Y / 8) * 8);
            index = s;
            nMap.Value = roomIndexes[s];
            if (pTiles.SelectedIndex != -1)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Graphics g = Graphics.FromImage(pMinimap.Image);
                    if (pTiles.SelectionRectangle.Width == 1 && pTiles.SelectionRectangle.Height == 1)
                    {
                        g.DrawImage(pTiles.Image, new Rectangle(e.X / 8 * 8, e.Y / 8 * 8, 8, 8), (selectedTile % 8) * 8, (selectedTile / 8) * 8, 8, 8, GraphicsUnit.Pixel);
                        if (pTiles.SelectedIndex != 3)
                            minimapData[index] = (byte)(pTiles.SelectedIndex + 0xED);
                        else
                            minimapData[index] = 0x7D;
                    }
                }
            }
        }

        private void pTiles_MouseClick(object sender, MouseEventArgs e)
        {
            selectedTile = (byte)pTiles.SelectedIndex;
            if (e.Button == MouseButtons.Right)
                pTiles.SelectedIndex = -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void nTile_ValueChanged(object sender, EventArgs e)
        {
            pTile.Image = drawOverworldTile();
            selectedTile = (byte)nTile.Value;
        }

        private void nPalette_ValueChanged(object sender, EventArgs e)
        {
            pTile.Image = drawOverworldTile();
            selectedPal = (byte)nPalette.Value;
        }

        private void pMinimapO_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Graphics g = Graphics.FromImage(pMinimapO.Image);
                g.DrawImage(pTile.Image, new Rectangle(e.X / 8 * 8, e.Y / 8 * 8, 8, 8));

                overworldpal[((e.X / 8) + (e.Y / 8) * 16)] = selectedPal;
                if (selectedTile > 0x6F)
                    selectedTile += 0x8A;
                minimapData[(e.X / 8) + (e.Y / 8) * 16] = selectedTile;
                if (selectedTile > 0x6F)
                    selectedTile -= 0x8A;
            }
            else if (e.Button == MouseButtons.Right)
            {
                selectedTile = minimapData[(e.X / 8) + (e.Y / 8) * 16];
                gb.BufferLocation = 0x81797;
                selectedPal = overworldpal[((e.X / 8) + (e.Y / 8) * 16)];
                if (selectedTile > 0x6F)
                    selectedTile -= 0x8A;
                nTile.Value = selectedTile;
                nPalette.Value = selectedPal;
            }
        }

        private void nDeathMinimap_ValueChanged(object sender, EventArgs e)
        {
            if (dung == 0xFF)
                gb.BufferLocation = 0x19B3;
            else
                gb.BufferLocation = 0x50E41 + dung;
            gb.WriteByte((byte)nDeathMinimap.Value);
        }

        private void nDeathDungeon_ValueChanged(object sender, EventArgs e)
        {
            if (dung == 0xFF)
                gb.BufferLocation = 0x50E3D;
            else
                gb.BufferLocation = 0x50DF2 + (dung * 5);

            gb.WriteByte((byte)nDeathDungeon.Value);
        }

        private void nDeathMap_ValueChanged(object sender, EventArgs e)
        {
            if (dung == 0xFF)
                gb.BufferLocation = 0x50E3D + 1;
            else
                gb.BufferLocation = 0x50DF2 + ((dung * 5) + 1);

            gb.WriteByte((byte)nDeathMap.Value);
        }

        private void nArrow_ValueChanged(object sender, EventArgs e)
        {
            if (dung == 0xFF)
                gb.BufferLocation = 0x6E8D + (0xF * 2);
            else
                gb.BufferLocation = 0x6E8D + (dung * 2);
            gb.WriteByte((byte)(nArrow.Value + 0xB));
        }

        private void pMinimap_MouseMove(object sender, MouseEventArgs e)
        {
            lblHoverPos.Text = "Minimap Byte: " + ((e.X / 8) + ((e.Y / 8) * 8)).ToString("X");
        }
    }
}
