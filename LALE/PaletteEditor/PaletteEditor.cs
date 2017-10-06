using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LALE
{
    public partial class PaletteEditor : Form
    {
        TileLoader tLoader;
        LALEForm form1;
        public Color[,] palette;
        public GBHL.GBFile gb;
        int dungeonIndex;
        int mapIndex;
        int mapIndexOriginal;
        int dungeonIndexOriginal;
        bool overWorld;
        bool sideView;
        bool crystals;
        bool specialMaps;
        byte[] pointer;
        public byte offset;
        ColorDialog c = new ColorDialog();

        public PaletteEditor(TileLoader t, LALEForm f, Color[,] pal, byte[] g, int dungeon, int map, bool overworld, bool sideview, bool special, bool crystal, byte off)
        {
            InitializeComponent();

            tLoader = t;
            palette = pal;
            form1 = f;
            gb = new GBHL.GBFile(g);

            if (overworld)
                nDungeon.Enabled = false;
            overWorld = overworld;
            sideView = sideview;
            specialMaps = special;
            crystals = crystal;
            mapIndex = map;
            mapIndexOriginal = map;
            dungeonIndex = dungeon;
            dungeonIndexOriginal = dungeon;
            nMap.Value = map;

            if (!overworld)
            {
                nDungeon.Value = dungeon;
                nIndex.Enabled = false;
                gb.BufferLocation = 0x8523A;
                for (int i = 0; i < 0x2D; i++)
                {
                    if (gb.ReadByte() != dungeonIndex)
                    {
                        gb.BufferLocation += 3;
                        continue;
                    }
                    if (gb.ReadByte() != mapIndex)
                    {
                        gb.BufferLocation += 2;
                        continue;
                    }
                    byte q = gb.ReadByte();
                    if (q != 4)
                    {
                        gb.BufferLocation++;
                        continue;
                    }
                    nIndex.Enabled = true;
                    nIndex.Maximum = 0x3F;
                    offset = off;
                    nIndex.Value = off & 0x3F;
                    break;
                }
            }
            if (dungeon == 0xFF)
                nMap.Maximum = 0x15;
            pTileset.Image = form1.pTiles.Image;

            palette = pal;
            Bitmap b = new Bitmap(128, 128);
            Graphics gra = Graphics.FromImage(b);
            for (int k = 0; k < 8; k++)
                for (int i = 0; i < 4; i++)
                    gra.FillRectangle(new SolidBrush(pal[k, i]), i * 32, k * 16, 32, 16);
            pPalette.Image = b;
            getPaletteLocation();
            getPalettePointer();
            if (overworld)
            {
                offset = off;
                nIndex.Value = off;
            }
        }

        private void getPalette()
        {
            tLoader = new TileLoader(gb);
            tLoader.loadPallete((byte)dungeonIndex, (byte)mapIndex, overWorld, sideView);
            label3.Text = "Palette Location: 0x" + tLoader.paletteLocation.ToString("X");
            palette = tLoader.palette;
            drawPalette();
        }

        private void getPalettePointer()
        {
            if (overWorld)
            {
                gb.BufferLocation = 0x842EF + mapIndex;
                int b = gb.ReadByte();
                offset = (byte)b;
                nIndex.Value = offset;
                b *= 2;
                gb.BufferLocation = 0x842B1 + b;
                pointer = gb.ReadBytes(2);
            }
        }

        private void getPaletteLocation()
        {
            tLoader = new TileLoader(gb);
            tLoader.loadPallete((byte)dungeonIndex, (byte)mapIndex, overWorld, sideView);
            label3.Text = "Palette Location: 0x" + tLoader.paletteLocation.ToString("X");
            offset = tLoader.palOffset;
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

        private void drawPalette()
        {
            Bitmap b = new Bitmap(128, 128);
            Graphics g = Graphics.FromImage(b);
            for (int k = 0; k < 8; k++)
                for (int i = 0; i < 4; i++)
                    g.FillRectangle(new SolidBrush(palette[k, i]), i * 32, k * 16, 32, 16);
            pPalette.Image = b;
        }

        private void nDungeon_ValueChanged(object sender, EventArgs e)
        {
            dungeonIndex = (int)nDungeon.Value;
            if (!overWorld)
            {
                nIndex.Enabled = false;
                gb.BufferLocation = 0x8523A;
                for (int i = 0; i < 0x2D; i++)
                {
                    if (gb.ReadByte() != dungeonIndex)
                    {
                        gb.BufferLocation += 3;
                        continue;
                    }
                    if (gb.ReadByte() != mapIndex)
                    {
                        gb.BufferLocation += 2;
                        continue;
                    }
                    byte q = gb.ReadByte();
                    if (q != 4)
                    {
                        gb.BufferLocation++;
                        continue;
                    }
                    nIndex.Enabled = true;
                    nIndex.Maximum = 0x3F;
                    break;
                }
            }
            getPalette();
            getTileset();
        }

        private void nMap_ValueChanged(object sender, EventArgs e)
        {
            mapIndex = (int)nMap.Value;
            if (!overWorld)
            {
                nIndex.Enabled = false;
                gb.BufferLocation = 0x8523A;
                for (int i = 0; i < 0x2D; i++)
                {
                    if (gb.ReadByte() != dungeonIndex)
                    {
                        gb.BufferLocation += 3;
                        continue;
                    }
                    if (gb.ReadByte() != mapIndex)
                    {
                        gb.BufferLocation += 2;
                        continue;
                    }
                    byte q = gb.ReadByte();
                    if (q != 4)
                    {
                        gb.BufferLocation++;
                        continue;
                    }
                    nIndex.Enabled = true;
                    nIndex.Maximum = 0x3F;
                    break;
                }
            }
            getPalette();
            getPalettePointer();
            getTileset();
        }

        private void getTileset()
        {
            tLoader.getAnimations((byte)mapIndexOriginal, (byte)dungeonIndex, overWorld, specialMaps);
            tLoader.getSOG((byte)mapIndexOriginal, overWorld);
            byte[, ,] data = tLoader.loadTileset((byte)dungeonIndex, (byte)mapIndexOriginal, overWorld, crystals, sideView);
            tLoader.palette = palette;
            TileLoader.Tile[] tilez = tLoader.loadPaletteFlipIndexes((byte)mapIndexOriginal, (byte)dungeonIndex);
            pTileset.Image = tLoader.drawTileset(data, tilez);
        }
        private void pPalette_MouseDown(object sender, MouseEventArgs e)
        {

            int i = pPalette.HoverIndex;
            c.Color = palette[i / 4, i % 4];
            if (c.ShowDialog() == DialogResult.OK)
            {
                int r = (int)(c.Color.R / 8 * 8);
                int g = (int)(c.Color.G / 8 * 8);
                int b = (int)(c.Color.B / 8 * 8);
                palette[i / 4, i % 4] = Color.FromArgb(r, g, b);
                drawPalette();
                getTileset();
            }
        }

        private void pPalette_MouseMove(object sender, MouseEventArgs e)
        {
            int i = pPalette.HoverIndex;
            label4.Text = "R: " + palette[i / 4, i % 4].R / 8 + " G: " + palette[i / 4, i % 4].G / 8 + " B: " + palette[i / 4, i % 4].B / 8;
        }

        private void nIndex_ValueChanged(object sender, EventArgs e)
        {
            offset = (byte)nIndex.Value;
            if (overWorld)
            {
                gb.BufferLocation = 0x842B1 + ((offset * 2) & 0xFF);
                gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
            }
            else
            {
                gb.BufferLocation = 0x8523A;
                for (int i = 0; i < 0x2D; i++)
                {
                    if (gb.ReadByte() != dungeonIndex)
                    {
                        gb.BufferLocation += 3;
                        continue;
                    }
                    if (gb.ReadByte() != mapIndex)
                    {
                        gb.BufferLocation += 2;
                        continue;
                    }
                    byte off = gb.ReadByte();
                    if (off != 4)
                    {
                        gb.BufferLocation++;
                        continue;
                    }
                    off = offset;
                    gb.BufferLocation = 0x851F6;
                    off &= 0x3F;
                    off <<= 1;
                    gb.BufferLocation += off;
                    gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                    break;
                }
            }

            label3.Text = "Palette Location: 0x" + gb.BufferLocation.ToString("X");
            palette = gb.GetPalette(gb.BufferLocation);
            drawPalette();
            getTileset();

        }
    }
}
