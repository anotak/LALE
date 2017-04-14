using System;
using System.Collections.Generic;
using System.Text;
using GBHL;
using System.Drawing;

namespace LALE
{
    class MinimapDrawer
    {
        GBFile gb;
        Color[] bwPalette = new Color[] { Color.White, Color.LightGray, Color.FromArgb(44, 50, 89), Color.Black };
        Color[] chestPalette = new Color[] { Color.FromArgb(248, 248, 168), Color.FromArgb(216, 168, 32), Color.FromArgb(136, 80, 0), Color.Black };
        Color[,] palette = new Color[8, 4];
        public byte[] minimapGraphics = new byte[64];
        public byte[] roomIndexes = new byte[64];
        public byte[] overworldPal = new byte[256];

        public MinimapDrawer(GBFile g)
        {
            gb = g;
        }

        public Bitmap drawDungeonTiles(byte[, ,] graphicsData)
        {
            Bitmap bmp = new Bitmap(128, 128);
            FastPixel fp = new FastPixel(bmp);
            fp.rgbValues = new byte[128 * 128 * 4];
            fp.Lock();
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    byte miniD = minimapGraphics[x + (y * 8)];
                    for (int y1 = 0; y1 < 8; y1++)
                    {
                        for (int x1 = 0; x1 < 8; x1++)
                        {
                            if (miniD == 0xEF) //Regular room
                                fp.SetPixel(x1 + (x * 8), y1 + (y * 8), bwPalette[graphicsData[2, x1, y1]]);
                            else if (miniD == 0x7D) //Empty room
                                fp.SetPixel(x1 + (x * 8), y1 + (y * 8), Color.FromArgb(44, 50, 89));
                            else if (miniD == 0xED) //Chest room
                                fp.SetPixel(x1 + (x * 8), y1 + (y * 8), chestPalette[graphicsData[0, x1, y1]]);
                            else if (miniD == 0xEE) //Boss room
                                fp.SetPixel(x1 + (x * 8), y1 + (y * 8), bwPalette[graphicsData[1, x1, y1]]);
                        }
                    }
                }
            }
            fp.Unlock(true);
            return bmp;
        }

        public void loadMinimapDData(byte dungeon)
        {
            minimapGraphics = gb.ReadBytes(0xA49A + (64 * dungeon), 64);
            if (dungeon == 0xFF)
                minimapGraphics = gb.ReadBytes(0xA49A + (64 * 0x9), 64);
            roomIndexes = gb.ReadBytes(0x50220 + (64 * dungeon), 64);
            if (dungeon == 0xFF)
                roomIndexes = gb.ReadBytes(0x504E0, 64);
        }

        public byte[, ,] loadMinimapDungeon()
        {
            byte[] tiles = gb.ReadBytes(0xCBFD0, 0x30);
            byte[, ,] data = new byte[3, 16, 16];
            gb.ReadTiles(3, 1, tiles, ref data);
            return data;
        }

        public byte[, ,] loadMinimapOverworld()
        {
            minimapGraphics = gb.ReadBytes(0x81697, 0x100);
            gb.BufferLocation = 0x81797;
            for (int b = 0; b < 256; b++)
                overworldPal[b] = gb.ReadByte();
            gb.BufferLocation = 0x8786E;
            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    palette[i, k] = GetColor(gb.BufferLocation);
                }
            }
            byte[] tiles = gb.ReadBytes(0xB3800, 0x800);
            byte[, ,] data = new byte[128, 16, 16];
            gb.ReadTiles(16, 8, tiles, ref data);
            return data;
        }

        public Color GetColor(int offset)
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

        public Bitmap drawOverworldTiles(byte[, ,] graphicsData)
        {
            Bitmap bmp = new Bitmap(128, 128);
            FastPixel fp = new FastPixel(bmp);
            fp.rgbValues = new byte[128 * 128 * 4];
            fp.Lock();
            byte i;
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    byte miniD = minimapGraphics[x + (y * 16)];
                    if (miniD == 0xFF || miniD == 0xFE || miniD == 0xFD || miniD == 0xFC || miniD == 0xFB || miniD == 0xFA)
                        i = (byte)(miniD - 0xF0);
                    else
                        i = (byte)(miniD + 16);
                    for (int y1 = 0; y1 < 8; y1++)
                    {
                        for (int x1 = 0; x1 < 8; x1++)
                            fp.SetPixel(x1 + (x * 8), y1 + (y * 8), palette[overworldPal[x + (y * 16)], graphicsData[i, x1, y1]]);
                    }
                }
            }
            fp.Unlock(true);
            return bmp;
        }
    }
}
