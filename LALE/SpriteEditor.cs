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
    public partial class SpriteEditor : Form
    {

        public GBHL.GBFile gb;
        Color[] bwPalette = new Color[] { Color.White, Color.LightGray, Color.DarkGray, Color.Black };
        Color[,] palette = new Color[7,4];
        byte[] spriteInfo;
        byte[] spriteGraphics;
        bool overWorld;
        byte dungeon;
        byte map;
        byte spriteBank;

        public SpriteEditor(GBHL.GBFile g, bool overworld, byte Map, byte Dungeon)
        {
            InitializeComponent();
            gb = g;
            if (overworld)
            {
                overWorld = overworld;
                cOverworld.Checked = true;
            }
            map = Map;
            nMap.Value = map;
            if (!overworld)
            {
                dungeon = Dungeon;
                nDungeon.Value = dungeon;
            }
            loadSpriteBanks();
            getSpriteLocation();
            drawSprites();
        }

        public void loadSpriteBanks()
        {

            gb.BufferLocation = 0x830DB + map;
            if (dungeon >= 6 && dungeon < 0x1A)
                gb.BufferLocation = 0x831DB + map;
            if (!overWorld)
                gb.BufferLocation += 0x100;
            spriteBank = gb.ReadByte();
            nSpriteBank.Value = spriteBank;

            if (!overWorld)
            {
                if (dungeon == 0x10 && map == 0xB5)
                    spriteBank = 0x3D;
            }
            //else
            //{
            //0x0DBD Look into??
            //}

            int a = (spriteBank << 2);
            if (dungeon != 0xFF)
            {
                gb.BufferLocation = 0x833FB + a;
                if (!overWorld)
                    gb.BufferLocation = 0x83643 + a;
                int loc = gb.BufferLocation;

                int i = 0;
                byte h = gb.ReadByte();
                spriteInfo = new byte[4];

                while (i < 4)
                {
                    if (h == 0xFF)
                        h = 0;
                    spriteInfo[i] = h;
                    h = gb.ReadByte();
                    i++;
                }
            }
        }

        public void getSpriteLocation()
        {
            spriteGraphics = new byte[0x400];
            int skip = 0;

            for (int i = 0; i < 4; i++)
            {
                byte B1 = spriteInfo[i];

                if (B1 != 0)
                {
                    int b = (byte)(B1 & 0x3F);
                    int bank = ((((B1 & 0xF) * 0x10) + (B1 >> 4)) >> 2) & 3;

                    gb.BufferLocation = 0x2E66 + bank;
                    byte B2 = gb.ReadByte();
                    if (B2 != 0)
                    {
                        bank = (byte)(B2 | 0x20);
                    }
                    gb.BufferLocation = (bank * 0x4000) + (b * 0x100);

                    for (int h = 0; h < 0x100; h++)
                    {
                        spriteGraphics[h + skip] = gb.ReadByte();
                    }
                }
                skip += 0x100;
            }
        }

        private void cOverworld_CheckedChanged(object sender, EventArgs e)
        {
            if (cOverworld.Checked == true)
            {
                nDungeon.Value = 0;
                nDungeon.Enabled = false;
                overWorld = true;
            }
            else
            {
                nDungeon.Enabled = true;
                overWorld = false;
            }
            loadSpriteBanks();
            getSpriteLocation();
            drawSprites();
        }

        private void nDungeon_ValueChanged(object sender, EventArgs e)
        {
            dungeon = (byte)nDungeon.Value;
            loadSpriteBanks();
            getSpriteLocation();
            drawSprites();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            map = (byte)nMap.Value;
            loadSpriteBanks();
            getSpriteLocation();
            drawSprites();
        }

        private void drawSprites()
        {
            byte[, ,] data = new byte[64, 8, 8];
            gb.ReadTiles(16, 4, spriteGraphics, ref data);

            Bitmap bmp = new Bitmap(128, 32);
            FastPixel fp = new FastPixel(bmp);
            fp.rgbValues = new byte[128 * 32 * 4];
            fp.Lock();
            for (int i = 0; i < 64; i++)
            {

                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        fp.SetPixel(x + ((i % 16) * 8), y + ((i / 16) * 8), bwPalette[data[i, x, y]]);
                    }
                }

            }
            fp.Unlock(true);

            pBox.Image = bmp;
        }

        private void getPalette()
        {
            palette = gb.GetPalette(0x85518);
        }

        private void nSpriteBank_ValueChanged(object sender, EventArgs e)
        {
            spriteBank = (byte)nSpriteBank.Value;
            gb.BufferLocation = 0x830DB + map;
            if (dungeon >= 6 && dungeon < 0x1A)
                gb.BufferLocation = 0x831DB + map;
            if (!overWorld)
            {
                gb.BufferLocation += 0x100;
               // if (dungeon == 0x10 && map == 0xB5)
                //    spriteBank = 0x3D;
            }
            gb.WriteByte(spriteBank);

            loadSpriteBanks();
            getSpriteLocation();
            drawSprites();
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
    }
}
