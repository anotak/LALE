using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using GBHL;

namespace LALE
{
    public class TileLoader
    {
        GBFile gb;
        public Color[,] palette = new Color[8, 4];
        public byte SOG; //Special Object Graphics
        public byte Animations;
        byte cDungeon;
        byte cMap;
        bool overWorld;
        bool cSideView;
        public int paletteLocation;
        public byte palOffset;

        public struct Tile
        {
            public byte[] palette;
            public bool[] hFlip;
            public bool[] vFlip;
        }

        public TileLoader(GBFile g)
        {
            gb = g;
        }

        public byte[, ,] loadTileset(byte dungeon, byte map, bool overworld, bool crystals, bool sideView)
        {
            List<byte> final = new List<byte>();
            byte[] animated = new byte[0x40];
            byte[] walls = new byte[0x200];
            cDungeon = dungeon;
            cMap = map;
            overWorld = overworld;
            cSideView = sideView;

            foreach (byte b in loadFirstRow())
                final.Add(b);
            animated = Animate();

            if (!cSideView)
            {
                foreach (byte b in loadSOG(cMap, crystals))
                    final.Add(b);
                if (!overWorld)
                {
                    foreach (byte b in loadThird())
                        final.Add(b);
                }
                foreach (byte b in loadMain())
                    final.Add(b);

                walls = Walls();

                if (!overWorld)
                {
                    for (int w = 0; w < 0x200; w++)
                    {
                        final[(0x300) + w] = walls[w];
                    }
                }
            }
            else
            {
                if ((dungeon < 10 || map == 0xE9) && dungeon != 6)
                    gb.BufferLocation = 0xB7800;
                else
                    gb.BufferLocation = 0xB7000;
                foreach (byte b in gb.ReadBytes(0x800))
                    final.Add(b);
            }
            for (int i = 0; i < 0x40; i++)
            {
                final[(0x7C * 16) + i] = animated[i];
            }

            byte[, ,] data = new byte[144, 8, 8];
            gb.ReadTiles(16, 9, final.ToArray(), ref data);
            return data;
        }

        public byte[] loadFirstRow()
        {
            //0x8F00-8E00
            if (overWorld)
                return gb.ReadBytes(0xB0F00, 0x100);
            else
            {
                if (cDungeon == 0xFF) //0xFF = Colour dungeon
                {
                    return gb.ReadBytes(0xD6100, 0x100);
                }
                gb.BufferLocation = 0x805CA + cDungeon;
                byte b = gb.ReadByte();
                gb.BufferLocation = 0xC8000 + ((b - 0x40) * 0x100);
                return gb.ReadBytes(0x100);
            }
        }

        public byte[] loadThird()
        {
            //9100-91FF (dungeon only)
            if (cDungeon == 0xFF)
                return gb.ReadBytes(0xD6000, 0x100);

            gb.BufferLocation = 0x80589 + cDungeon;
            byte b = gb.ReadByte();
            gb.BufferLocation = 0xB4000 + ((b - 0x40) * 0x100);
            return gb.ReadBytes(0x100);
        }

        public void getSOG(byte map, bool overworld)
        {
            if (overworld)
            {
                //0x9000-0x9200
                if (map == 0x7)
                    map++;
                byte b = (byte)((map >> 2) & 0xF8);
                byte b1 = (byte)(((map >> 1) & 0x07) | b);
                gb.BufferLocation = 0x82E7B + b1;
                b = gb.ReadByte();
                SOG = b;
            }
            else
            {
                //0x9000-0x9100
                if (cDungeon >= 6 && cDungeon < 0x1A)
                    gb.BufferLocation = 0x82FBB + map;
                else if (cDungeon == 0xFF)
                    gb.BufferLocation = 0x830BB + map;
                else
                    gb.BufferLocation = 0x82EBB + map;
                byte b = gb.ReadByte();
                SOG = b;
            }
        }

        public byte[] loadSOG(byte map, bool crystals)
        {
            byte[] data = new byte[0x100];
            if (overWorld)
            {
                if (SOG == 0x0F)
                {
                    return new byte[0x200];
                }
                gb.BufferLocation = 0xBC000 + (SOG) * 0x100;
                return gb.ReadBytes(0x200);
            }
            else
            {
                if (SOG == 0xFF) //Don't load any others, keep data
                {
                    if (crystals)
                    {
                        gb.BufferLocation = 0xB2800;
                        for (int c = 0x40; c < 0xC0; c++)
                        {
                            if (c == 0x80)
                                gb.BufferLocation = 0xB2880;
                            data[c] = gb.ReadByte();
                        }
                    }
                    if (Animations == 7)
                    {
                        gb.BufferLocation = 0x307C0;
                        if (cDungeon == 5)
                            gb.BufferLocation = 0x307C2;
                        for (int i = 0xC0; i < 256; i++)
                        {
                            data[i] = gb.ReadByte();
                        }
                    }
                    return data;
                }
                if (cDungeon == 0xFF) //0xFF = Color dungeon
                {
                    gb.BufferLocation = 0x805EA + (map * 2);
                    byte location = gb.ReadByte();
                    byte bank = gb.ReadByte();
                    for (byte i = 0; i < 4; i++)
                    {
                        for (byte i2 = 0; i2 < 0x40; i2++)
                            data[(i * 0x40) + i2] = gb.ReadByte((bank * 0x4000) + (((location - 0x40) * 0x100) + (i * 0x40) + i2));
                    }
                    return data;
                }
                gb.BufferLocation = 0xB4000 + ((SOG + 0x10) * 0x100);
                if (crystals && SOG != 0xE)
                {
                    for (int i = 0; i < 256; i++)
                    {
                        data[i] = gb.ReadByte();
                        if (i == 0x40)
                        {
                            int buf = gb.BufferLocation + 0x40;
                            gb.BufferLocation = 0xB2800;
                            for (int c = 0; c < 0x80; c++)
                            {
                                if (c == 0x40)
                                    gb.BufferLocation = 0xB2880;
                                data[i + c] = gb.ReadByte();
                            }
                            gb.BufferLocation = buf;
                            i = i + 0x80;
                            if (Animations == 7 && i >= 0xC0)
                            {
                                gb.BufferLocation = 0x307C0;
                                data[i] = gb.ReadByte();
                            }
                        }
                    }
                    return data;
                }

                if (Animations == 7)
                {
                    for (int i = 0; i < 256; i++)
                    {
                        data[i] = gb.ReadByte();
                        if (i == 192)
                            gb.BufferLocation = 0x307C0;
                    }
                    return data;
                }
                return gb.ReadBytes(0x100);
            }
        }

        public byte[] loadMain()
        {
            //9200-97FF
            if (overWorld)
                return gb.ReadBytes(0xB1200, 0x600);
            else
                return gb.ReadBytes(0xB4000, 0x600);
        }

        public void getAnimations(byte map, byte dungeon, bool overworld, bool special)
        {
            if (overworld)
            {
                int i = -1;
                int secondhalf;
                if (special)
                {
                    switch (map)
                    {
                        case 0x06: i = 0x31F4; break;
                        case 0x0E: i = 0x31C4; break;
                        case 0x1B: i = 0x3204; break;
                        case 0x2B: i = 0x3214; break;
                        case 0x79: i = 0x31E4; break;
                        case 0x8C: i = 0x31D4; break;
                    }
                }
                if (i > 0)
                {
                    secondhalf = gb.Get2BytePointerAddress(i).Address;
                    if (map > 0x7F)
                        secondhalf += 0x68000;
                    else
                        secondhalf += 0x24000;
                    gb.BufferLocation = secondhalf;
                }
                else
                {
                    gb.BufferLocation = 0x24000 + (map * 2);
                    gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                    if (map > 0x7F)
                        gb.BufferLocation = 0x68000 + (gb.BufferLocation - 0x24000);
                }
                byte b = gb.ReadByte();
                Animations = b;
            }
            else
            {
                gb.BufferLocation = 0x28000;
                if (dungeon >= 0x06 && dungeon < 0x1A)
                    gb.BufferLocation += 0x4000;
                else if (dungeon == 0xFF)
                    gb.BufferLocation = 0x2BB77;
                gb.BufferLocation = gb.Get3BytePointerAddress((byte)(gb.BufferLocation / 0x4000), gb.BufferLocation + (map * 2)).Address;
                byte b = gb.ReadByte();
                Animations = b;
            }
        }

        public byte[] Animate()
        {
            //96C0-9700
            if (Animations == 7)
                return gb.ReadBytes(0xB2D00, 0x40);

            gb.BufferLocation = 0x1BD0 + (Animations * 2);
            gb.BufferLocation = gb.ReadByte() + (gb.ReadByte() * 0x100);
            while (gb.ReadByte() != 0x26) ;
            byte b1 = gb.ReadByte();
            gb.BufferLocation = 0xB0000 + ((b1 - 0x40) * 0x100);
            if (cDungeon == 0xFF && !overWorld)
            {
                gb.BufferLocation = 0x807CB + cMap;
                byte b2 = gb.ReadByte();
                if (b2 == 0)
                    gb.BufferLocation = 0xB0000 + ((b1 - 0x40) * 0x100);
                else
                    gb.BufferLocation = 0xD4000 + ((b2 + 0x20) * 0x100);
            }
            return gb.ReadBytes(0x40);
        }

        public byte[] Walls()
        {
            //9200-93FF
            if (cDungeon == 0xFF) //Colour dungeon
                gb.BufferLocation = 0x805C9;
            else
                gb.BufferLocation = 0x80000 + ((0x45A9 + cDungeon) - 0x4000);

            byte b = gb.ReadByte();
            gb.BufferLocation = 0xB4000 + (b - 0x40) * 0x100;
            return gb.ReadBytes(0x200);
        }

        public void loadPallete(byte dungeon, byte map, bool overworld, bool sideview)
        {
            if (overWorld || overworld)
            {
                gb.BufferLocation = 0x842EF + map;
                byte b = gb.ReadByte();
                palOffset = b;
                b *= 2;
                gb.BufferLocation = 0x842B1 + b;
                gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                paletteLocation = gb.BufferLocation;
                palette = gb.GetPalette(gb.BufferLocation);
                return;
            }

            if (cSideView || sideview)
            {
                if (dungeon != 0x07)
                {
                    gb.BufferLocation = 0x84401 + (dungeon * 2);
                    gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                    paletteLocation = gb.BufferLocation;
                    palette = gb.GetPalette(gb.BufferLocation);
                    return;
                }
                else if (map >= 0x64 && map <= 0x67 || map == 0x6A || map == 0x6B)
                {
                    gb.BufferLocation = 0x86750;
                    paletteLocation = gb.BufferLocation;
                    palette = gb.GetPalette(gb.BufferLocation);
                    return;
                }
                else
                {
                    gb.BufferLocation = 0x84401 + (dungeon * 2);
                    gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                    paletteLocation = gb.BufferLocation;
                    palette = gb.GetPalette(gb.BufferLocation);
                    return;
                }
            }

            if (!overWorld || !overworld)
            {
                gb.BufferLocation = 0x8523A;
                for (int i = 0; i < 0x2D; i++)
                {
                    if (gb.ReadByte() != dungeon)
                    {
                        gb.BufferLocation += 3;
                        continue;
                    }
                    if (gb.ReadByte() != map)
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
                    byte b = gb.ReadByte();
                    palOffset = b;
                    gb.BufferLocation = 0x851F6;
                    b &= 0x3F;
                    b <<= 1;
                    gb.BufferLocation += b;
                    gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                    paletteLocation = gb.BufferLocation;
                    palette = gb.GetPalette(gb.BufferLocation);
                    return;
                }

                if (dungeon == 0xFF) //0xFF = Color dungeon
                {
                    gb.BufferLocation = 0x867D0;
                    paletteLocation = gb.BufferLocation;
                    if (map != 0x1 && map != 0x13 && map != 0xF)
                        palette = gb.GetPalette(gb.BufferLocation);
                    else
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if (i == 7)
                                if (map == 0x1)
                                {
                                    paletteLocation = gb.BufferLocation;
                                    gb.BufferLocation = 0xDACF0;
                                }
                                else
                                {
                                    paletteLocation = gb.BufferLocation;
                                    gb.BufferLocation = 0xDACE0;
                                }

                            for (int k = 0; k < 4; k++)
                            {
                                palette[i, k] = GetColor(gb.BufferLocation);
                            }
                        }
                    }
                    return;
                }
                else if (dungeon > 0x09) //Indoor
                {
                    byte b = (byte)((dungeon - 0x0A) << 1);
                    gb.BufferLocation = 0x84413 + b;
                    gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address + map;
                    b = (byte)gb.ReadByte();
                    palOffset = b;
                    b *= 2;
                    gb.BufferLocation = 0x8443F + b;
                    gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                    paletteLocation = gb.BufferLocation;
                    palette = gb.GetPalette(gb.BufferLocation);
                    return;
                }
                else
                {
                    gb.BufferLocation = 0x843EF + (dungeon * 2);
                    gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                    paletteLocation = gb.BufferLocation;
                    palette = gb.GetPalette(gb.BufferLocation);
                }
            }
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

        public Tile[] loadPaletteFlipIndexes(byte map, byte dungeon)
        {
            if (overWorld)
            {
                gb.BufferLocation = 0x6A476 + cMap;
                int b = gb.ReadByte() * 0x4000;
                gb.BufferLocation = 0x69E76 + (cMap * 2);
                gb.BufferLocation = b + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
            }
            else if (dungeon == 0xFF) //0xFF = Color dungeon
            {
                gb.BufferLocation = 0x8E000;
            }
            else if (dungeon < 9)
            {
                //gb.BufferLocation = 0x8C000 + (dungeon * 0x400);
                gb.BufferLocation = 0x6A076 + dungeon * 2;
                gb.BufferLocation = 0x8C000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
            }
            else
            {
                if (dungeon == 0x0A && map == 0xFD)
                    gb.BufferLocation = 0x6A276 + 0x1E;
                else if (dungeon == 0x11 && (map == 0xC0 || map == 0xC1))
                    gb.BufferLocation = 0x6A276 + 0x1E;
                else if (dungeon == 0x0F && map == 0xA0)
                    gb.BufferLocation = 0x6A276;
                else if (dungeon == 0x1F && (map == 0xEB || map == 0xEC))
                    gb.BufferLocation = 0x6A276 + 0x28;
                else if (dungeon == 0x10 && map == 0xE9)
                    gb.BufferLocation = 0x6A276 + 0x26;
                else if (dungeon == 0x10 && map == 0xB5)
                    gb.BufferLocation = 0x6A276 + 0x01FE;
                else if (dungeon == 0x16 && (map == 0x6F || map == 0x7F || map == 0x8F))
                    gb.BufferLocation = 0x6A276;
                else
                    gb.BufferLocation = 0x6A276 + (dungeon * 2);
                gb.BufferLocation = 0x90000 + gb.ReadByte() + ((gb.ReadByte() - 0x40) * 0x100);
            }
            Tile[] tiles = new Tile[0x100];
            for (int i = 0; i < 0x100; i++)
            {
                tiles[i] = new Tile();
                tiles[i].palette = new byte[4];
                tiles[i].hFlip = new bool[4];
                tiles[i].vFlip = new bool[4];
                for (int k = 0; k < 4; k++)
                {
                    byte b = gb.ReadByte();
                    tiles[i].palette[k] = (byte)(b & 0xF);
                    if ((b & 0x40) != 0)
                        tiles[i].vFlip[k] = true;
                    if ((b & 0x20) != 0)
                        tiles[i].hFlip[k] = true;
                }
            }
            return tiles;
        }

        public Bitmap drawTileset(byte[, ,] graphicsData, Tile[] tiles)
        {
            Bitmap bmp = new Bitmap(256, 256);
            FastPixel fp = new FastPixel(bmp);
            fp.rgbValues = new byte[256 * 256 * 4];
            byte[,] formationData = loadFormation();
            fp.Lock();
            for (int tile = 0; tile < 256; tile++)
            {
                for (int corner = 0; corner < 4; corner++)
                {
                    byte i = formationData[tile, corner];
                    Tile t = tiles[tile];
                    if (i >= 0xB0 && i < 0xB4 || i == 0xC0)
                        i -= i;

                    for (int y = 0; y < 8; y++)
                    {
                        for (int x = 0; x < 8; x++)
                        {
                            Color pal = palette[t.palette[corner] % 8, graphicsData[(byte)(i + 0x10), x, y]];
                            int mainX = (tile % 16) * 16;
                            int mainY = (tile / 16) * 16;
                            int xx = mainX + ((corner % 2) * 8) + x;
                            int yy = mainY + ((corner / 2) * 8) + y;
                            if (t.hFlip[corner])
                                xx = mainX + ((corner % 2) * 8) + (7 - x);
                            if (t.vFlip[corner])
                                yy = mainY + ((corner / 2) * 8) + (7 - y);
                            fp.SetPixel(xx, yy, pal);
                        }
                    }
                }
            }
            fp.Unlock(true);
            return bmp;
        }

        public byte[,] loadFormation()
        {
            if (overWorld)
                gb.BufferLocation = 0x6AB1D;
            else if (cDungeon == 0xFF || (cDungeon == 0x10 && cMap == 0xB5))
                gb.BufferLocation = 0x20760;
            else
                gb.BufferLocation = 0x203B0;
            byte[,] data = new byte[0x100, 4];
            for (int i = 0; i < 0x100; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    data[i, k] = gb.ReadByte();
                }
            }
            return data;
        }

    }
}
