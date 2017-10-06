using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using GBHL;

namespace LALE
{
    class Sprites
    {

        GBFile gb;
        public int objectAddress;
        public byte[] spriteData;
        public List<LAObject> spriteList = new List<LAObject>();
        public int[] pointers;
        public List<Int32> unSortedPointers;
        public byte[] spriteInfo;
        public byte[] spriteGraphics = new byte[400];

        public Sprites(GBFile g)
        {
            gb = g;
        }

        public void loadObjects(bool overworld, byte dungeon, byte map)
        {
            spriteList = new List<LAObject>();
            if (overworld)
                gb.BufferLocation = 0x58000;
            else
            {
                gb.BufferLocation = 0x58200;
                if (dungeon >= 6 && dungeon < 0x1A)
                    gb.BufferLocation = 0x58400;
                else if (dungeon == 0xFF)
                    gb.BufferLocation = 0x58600;
            }
            gb.BufferLocation = gb.Get3BytePointerAddress((byte)(gb.BufferLocation / 0x4000), gb.BufferLocation + (map * 2)).Address;
            objectAddress = gb.BufferLocation;
            byte b;
            while ((b = gb.ReadByte()) != 0xFF) //0xFE = End of room
            {
                LAObject ob = new LAObject(); // 2-Byte tiles
                ob.y = (byte)(b >> 4);
                ob.x = (byte)(b & 0xF);
                ob.id = gb.ReadByte();
                spriteList.Add(ob);
            }
            spriteData = new byte[80];
            foreach (LAObject obj in spriteList)
            {
                if (obj.y < 0 || obj.y > 7)
                    continue;
                if (obj.x < 0 || obj.x > 9)
                    continue;
                spriteData[obj.x + (obj.y * 10)] = (byte)obj.id;
            }
        }

        public Bitmap DrawSprites(Bitmap map)
        {
            FastPixel fp = new FastPixel(map);
            fp.rgbValues = new byte[160 * 128 * 4];
            fp.Lock();
            foreach (LAObject obj in spriteList)
            {
                if (obj.x < 9 && obj.y < 8)
                {
                    for (int yy = 0; yy < 16; yy++)
                    {
                        for (int xx = 0; xx < 16; xx++)
                        {
                            fp.SetPixel((obj.x * 16) + xx, (obj.y * 16) + yy, Color.Black);
                        }
                    }
                }
            }
            fp.Unlock(true);
            return map;
        }

        public Bitmap drawSelectedSprite(Image map, LAObject selected)
        {
            Bitmap b = (Bitmap)map;
            FastPixel fp = new FastPixel(b);
            fp.rgbValues = new byte[160 * 128 * 4];
            fp.Lock();
            if (selected.x > 9 || selected.y > 7)
            {
                fp.Unlock(true);
                return b;
            }
            for (int yy = 0; yy < 16; yy++)
            {
                for (int xx = 0; xx < 16; xx++)
                {
                    int x = selected.x * 16;
                    int y = selected.y * 16;
                    if (xx == 0 || xx == 15)
                    {
                        if (xx == 0)
                        {
                            //  fp.SetPixel(x + (xx - 1), y + yy, Color.White);
                            fp.SetPixel(x + xx, y + yy, Color.White);
                        }
                        else if (xx == 15)
                        {
                            //    fp.SetPixel(x + (xx + 1), y + yy, Color.White);
                            fp.SetPixel(x + xx, y + yy, Color.White);
                        }
                    }
                    else
                    {
                        if (yy == 0)
                        {
                            //      fp.SetPixel(x + xx, y + (yy - 1), Color.White);
                            fp.SetPixel(x + xx, y + yy, Color.White);
                            //      if (xx == 0)
                            //         fp.SetPixel(x + (xx - 1), y + yy, Color.White);
                            //     else if (xx == 15)
                            //         fp.SetPixel(x + (xx + 1), y + yy, Color.White);
                        }
                        else if (yy == 15)
                        {
                            //    fp.SetPixel(x + xx, y + (yy + 1), Color.White);
                            fp.SetPixel(x + xx, y + yy, Color.White);
                            //     if (xx == 0)
                            //          fp.SetPixel(x + (xx - 1), y + yy, Color.White);
                            //     else if (xx == 15)
                            //       fp.SetPixel(x + (xx + 1), y + yy, Color.White);
                        }
                    }
                }
            }
            fp.Unlock(true);
            return b;
        }

        public int getUsedSpace()
        {
            int s = 0;
            foreach (LAObject o in spriteList)
                s += 2;
            return s;
        }

        public int getFreeSpace(bool overworld, byte Map, byte dungeon)
        {
            unSortedPointers = new List<Int32>();
            pointers = new int[256];
            int cMapPointer = 0;
            int map = 0;
            int index;
            int space = 0;
            while (map < 256)
            {
                if (overworld)
                    gb.BufferLocation = 0x58000;
                else
                {
                    gb.BufferLocation = 0x58200;
                    if (dungeon >= 6 && dungeon < 0x1A)
                        gb.BufferLocation = 0x58400;
                    else if (dungeon == 0xFF)
                        gb.BufferLocation = 0x58600;
                }
                gb.BufferLocation = gb.Get3BytePointerAddress((byte)(gb.BufferLocation / 0x4000), gb.BufferLocation + (map * 2)).Address;
                if (map == Map)
                    cMapPointer = gb.BufferLocation;
                pointers[map] = gb.BufferLocation;
                map++;
            }
            foreach (int point in pointers)
                unSortedPointers.Add(point);
            Array.Sort(pointers);
            index = Array.IndexOf(pointers, cMapPointer);
            if (Map == 0xFF)
            {
                gb.BufferLocation = cMapPointer;
                if (overworld)
                    space = 0x59663 - cMapPointer;
                else if (dungeon >= 0x1A || dungeon < 6)
                    space = 0x58CA3 - cMapPointer;
                else
                    space = 0x59185 - cMapPointer;

            }
            else
            {
                while ((int)pointers.GetValue(index + 1) == cMapPointer)
                    index++;
                space = ((int)pointers.GetValue(index + 1) - 1) - cMapPointer;
            }
            return space;
        }

        public void loadSpriteBanks(bool overWorld, byte dungeon, byte map)
        {

            gb.BufferLocation = 0x830DB + map;
            if (dungeon >= 6 && dungeon < 0x1A)
                gb.BufferLocation = 0x831DB + map;
            byte b = gb.ReadByte();

            if (!overWorld)
            {
                if (dungeon == 0x10 && map == 0xB5)
                    b = 0x3D;
            }
            //else
            //{
            //0x0DBD Look into??
            //}

            int a = (b << 2);
            if (dungeon != 0xFF)
            {
                gb.BufferLocation = 0x833FB + a;
                if (!overWorld)
                    gb.BufferLocation = 0x836FB + a;
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

        public void getSpriteLocation(bool overworld, bool sidescrolling, byte dungeon, byte map)
        {
            spriteGraphics = new byte[400];
            int skip = 0;

            for (int i = 0; i < 4; i++)
            {
                byte B1 = spriteInfo[i];

                if (B1 != 0)
                {
                    int b = (byte)(B1 & 0x3F);
                    int bank = ((((B1 & 0xF) * 0x10) + (B1 >> 4)) >> 2) & 3;

                    gb.BufferLocation = 0x8262E + bank;
                    byte B2 = gb.ReadByte();
                    if (B2 != 0)
                    {
                        bank = bank | 20;
                    }
                    gb.BufferLocation = (bank * 0x4000) + (b * 0x100);

                    for (int h = 0; h < 100; h++)
                    {
                        spriteGraphics[h + skip] = gb.ReadByte();
                    }
                }
                else
                    skip += 100;
            }
        }
    }
}
