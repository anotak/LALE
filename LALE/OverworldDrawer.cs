using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using GBHL;

namespace LALE
{
    class OverworldDrawer
    {
        GBFile gb;
        byte cMap;
        public int mapAddress;
        public byte[] mapData;
        public int[] pointers;
        public List<Int32> unSortedPointers;
        bool s;
        private Object overworldObjects = new Object();
        public List<Object> objects = new List<Object>();
        public List<Warps> warps = new List<Warps>();
        public byte floor;
        public byte music;
        bool collision;
        public byte wall;
        public byte spriteBank;

        public OverworldDrawer(GBFile g)
        {
            gb = g;
        }

        public void getMusic(byte map)
        {
            gb.BufferLocation = 0x8000 + map;
            music = gb.ReadByte();
        }

        public byte[] ReadMap(byte map, bool special)
        {
            cMap = map;
            mapData = new byte[80];
            s = special;
            collision = false;
            //c = collision;
            GetOffsetOverworld();
            mapAddress = gb.BufferLocation;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    mapData[x + (y * 10)] = gb.ReadByte();
                }

            }
            return mapData;
        }

        public int GetOffsetOverworld()
        {
            int i = -1;
            if (s)
            {
                switch (cMap)
                {
                    case 0x06: i = 0x5040; break;
                    case 0x0E: i = 0x5090; break;
                    case 0x1B: i = 0x50E0; break;
                    case 0x2B: i = 0x5130; break;
                    case 0x79: i = 0x5180; break;
                    case 0x8C: i = 0x51D0; break;
                }
            }
            if (i > 0)
            {
                return gb.BufferLocation = 0x98000 + i;
            }
            else if (cMap < 0xCC)
            {
                return gb.BufferLocation = 0x98000 + 0x50 * cMap;
            }
            else
            {
                return gb.BufferLocation = 0x9C000 + 0x50 * (cMap - 0xCC);
            }

        }

        public Bitmap drawMap(Bitmap tiles, byte[] mapData, bool borderss, Object selected)
        {
            Bitmap bmp = new Bitmap(160, 128);
            FastPixel fp = new FastPixel(bmp);
            FastPixel src = new FastPixel(tiles);
            fp.rgbValues = new byte[160 * 128 * 4];
            src.rgbValues = new byte[256 * 256 * 4];
            fp.Lock();
            src.Lock();
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    byte v = mapData[x + (y * 10)];

                    for (int yy = 0; yy < 16; yy++)
                    {
                        for (int xx = 0; xx < 16; xx++)
                        {
                            fp.SetPixel(x * 16 + xx, y * 16 + yy, src.GetPixel((v % 16) * 16 + xx, (v / 16) * 16 + yy));
                        }
                    }
                }
            }
            fp.Unlock(true);
            src.Unlock(true);

            if (borderss && collision)
                drawBorders(bmp);
            //drawSelectedObject(bmp, selected);
            return bmp;
        }

        public Bitmap drawBorders(Bitmap image)
        {
            FastPixel fp = new FastPixel(image);
            Color border;
            fp.rgbValues = new byte[160 * 128 * 4];
            fp.Lock();
            foreach (Object obj in objects)
            {
                if (obj.is3Byte && !obj.special && obj.direction == 8)
                    border = Color.DarkRed;
                else if (obj.is3Byte && !obj.special && obj.direction == 0xC)
                    border = Color.DarkBlue;
                else if (obj.is3Byte && obj.special && obj.direction == 8)
                    border = Color.DarkGoldenrod;
                else if (obj.is3Byte && obj.special && obj.direction == 0xC)
                    border = Color.Purple;
                else if (!obj.is3Byte && !obj.special)
                    border = Color.DarkGreen;
                else
                    border = Color.DeepPink;
                int x = obj.x * 16;
                int y = obj.y * 16;
                if (obj.hFlip)
                    x = -16;
                if (obj.vFlip)
                    y = -16;
                bool v = false;
                bool h = false;
                if (!obj.is3Byte && !obj.special)
                {
                    if (obj.x > 9 || obj.y > 7)
                        continue;
                }
                if (!obj.is3Byte)
                {
                    if (!obj.special)
                    {
                        for (int yy = 0; yy < 16; yy++)
                        {
                            for (int xx = 0; xx < 16; xx++)
                            {
                                if (yy > 0 && yy != 15)
                                {
                                    if (xx == 0 || xx == 15)
                                    {
                                        fp.SetPixel(x + xx, y + yy, border);
                                    }
                                }
                                else
                                {
                                    fp.SetPixel(x + xx, y + yy, border);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int yy = 0; yy < obj.h * 16; yy++)
                        {
                            for (int xx = 0; xx < obj.w * 16; xx++)
                            {
                                if (x < 0 && !h)
                                {
                                    xx = xx + 16;
                                    h = true;
                                }
                                if (y < 0 && !v)
                                {
                                    yy = yy + 16;
                                    v = true;
                                }
                                if (x + xx >= 160 || x + xx < 0)
                                    continue;
                                if (y + yy >= 128 || y + yy < 0)
                                    continue;
                                if (yy > 0 && yy != ((obj.h * 16) - 1))
                                {
                                    if (xx == 0 || xx == (obj.w * 16) - 1)
                                        fp.SetPixel(x + xx, y + yy, border);
                                }
                                else
                                    fp.SetPixel(x + xx, y + yy, border);
                            }
                        }
                    }
                }
                else
                {
                    if (!obj.special)
                    {
                        if (obj.direction == 8)
                        {
                            for (int yy = 0; yy < 16; yy++)
                            {
                                for (int xx = 0; xx < (obj.length * 16); xx++)
                                {
                                    if (x < 0 && !h)
                                    {
                                        xx = xx + 16;
                                        h = true;
                                    }
                                    if (y < 0 && !v)
                                    {
                                        yy = yy + 16;
                                        v = true;
                                    }
                                    if (x + xx >= 160 || x + xx < 0)
                                        continue;
                                    if (y + yy >= 128 || y + yy < 0)
                                        continue;
                                    if (yy > 0 && yy != 15)
                                    {
                                        if (xx == 0 || xx == (obj.length * 16) - 1)
                                            fp.SetPixel(x + xx, y + yy, border);
                                    }
                                    else
                                        fp.SetPixel(x + xx, y + yy, border);
                                }
                            }
                        }
                        else
                        {
                            for (int yy = 0; yy < obj.length * 16; yy++)
                            {
                                for (int xx = 0; xx < 16; xx++)
                                {
                                    if (x < 0 && !h)
                                    {
                                        xx = xx + 16;
                                        h = true;
                                    }
                                    if (y < 0 && !v)
                                    {
                                        yy = yy + 16;
                                        v = true;
                                    }
                                    if (x + xx >= 160 || x + xx < 0)
                                        continue;
                                    if (y + yy >= 128 || y + yy < 0)
                                        continue;
                                    if (yy > 0 && yy != (obj.length * 16) - 1)
                                    {
                                        if (xx == 0 || xx == 15)
                                            fp.SetPixel(x + xx, y + yy, border);
                                    }
                                    else
                                        fp.SetPixel(x + xx, y + yy, border);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (obj.direction == 8)
                        {
                            for (int i = 0; i < obj.length; i++)
                            {
                                for (int yy = 0; yy < obj.h * 16; yy++)
                                {
                                    for (int xx = 0 + (i * 16); xx < obj.w * (obj.length * 16); xx++)
                                    {
                                        if (x < 0 && !h)
                                        {
                                            xx = xx + 16;
                                            h = true;
                                        }
                                        if (y < 0 && !v)
                                        {
                                            yy = yy + 16;
                                            v = true;
                                        }
                                        if (x + xx >= 160 || x + xx < 0)
                                            continue;
                                        if (y + yy >= 128 || y + yy < 0)
                                            continue;
                                        if (yy > 0 && yy != ((obj.h * 16) - 1))
                                        {
                                            if (xx == 0 || xx == ((obj.length * 16) * obj.w) - 1)
                                                fp.SetPixel(x + xx, y + yy, border);
                                        }
                                        else
                                            fp.SetPixel(x + xx, y + yy, border);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < obj.length; i++)
                            {
                                for (int yy = 0 + (i * 16); yy < obj.h * (obj.length * 16); yy++)
                                {
                                    for (int xx = 0; xx < obj.w * 16; xx++)
                                    {
                                        if (x < 0 && !h)
                                        {
                                            xx = xx + 16;
                                            h = true;
                                        }
                                        if (y < 0 && !v)
                                        {
                                            yy = yy + 16;
                                            v = true;
                                        }
                                        if (x + xx >= 160 || x + xx < 0)
                                            continue;
                                        if (y + yy >= 128 || y + yy < 0)
                                            continue;
                                        if (yy > 0 && yy != ((obj.length * 16) * obj.h) - 1)
                                        {
                                            if (xx == 0 || xx == (obj.w * 16) - 1)
                                                fp.SetPixel(x + xx, y + yy, border);
                                        }
                                        else
                                            fp.SetPixel(x + xx, y + yy, border);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            fp.Unlock(true);
            return image;
        }

        public void getFloor(byte map, bool special)
        {
            int secondhalf;
            int i = -1;
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
                secondhalf = gb.Get2BytePointerAddress(i).Address + 1;
                if (map > 0x7F)
                    secondhalf += 0x68000;
                else
                    secondhalf += 0x24000;
                gb.BufferLocation = secondhalf;
            }
            else
            {
                secondhalf = 0x24000 + (map * 2);
                secondhalf = gb.Get2BytePointerAddress(secondhalf).Address + 1;
                if (map > 0x7F)
                    secondhalf = 0x68000 + (secondhalf - 0x24000);
                gb.BufferLocation = secondhalf;
            }
            byte b = gb.ReadByte();
            floor = (byte)(b & 0xF);
            wall = (byte)(b >> 4);
            spriteBank = gb.ReadByte(0x830DB + map);
        }

        public void getCollisionDataOverworld(byte map, bool s)
        {
            int i = -1;
            objects = new List<Object>();
            warps = new List<Warps>();
            overworldObjects = new Object();
            int secondhalf;
            collision = true;

            if (s)
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
                secondhalf = gb.Get2BytePointerAddress(i).Address + 1;
                if (map > 0x7F)
                    secondhalf += 0x68000;
                else
                    secondhalf += 0x24000;
                gb.BufferLocation = secondhalf;
            }
            else
            {
                secondhalf = 0x24000 + (map * 2);
                secondhalf = gb.Get2BytePointerAddress(secondhalf).Address + 1;
                if (map > 0x7F)
                    secondhalf = 0x68000 + (secondhalf - 0x24000);
                gb.BufferLocation = secondhalf;
            }
            mapAddress = gb.BufferLocation - 1;
            byte b = gb.ReadByte();
            while ((b = gb.ReadByte()) != 0xFE) //0xFE = End of room
            {
                if (b >> 4 == 0xE)
                {
                    Warps w = new Warps();
                    w.type = (byte)(b & 0x0F);
                    w.region = gb.ReadByte();
                    w.map = gb.ReadByte();
                    w.x = gb.ReadByte();
                    w.y = gb.ReadByte();
                    warps.Add(w);
                    continue;
                }
                if (b >> 4 == 8 || b >> 4 == 0xC) //3-Byte objects
                {
                    Object o = new Object();
                    o.is3Byte = true;
                    o.length = (byte)(b & 0xF);
                    o.direction = (byte)(b >> 4);
                    byte b2 = gb.ReadByte();
                    o.y = (byte)(b2 >> 4);
                    o.x = (byte)(b2 & 0xF);
                    o.id = gb.ReadByte();
                    overworldObjects.getOverworldObjs(o);
                    continue;
                }
                else
                {
                    Object ob = new Object(); // 2-Byte tiles
                    ob.y = (byte)(b >> 4);
                    ob.x = (byte)(b & 0xF);
                    ob.id = gb.ReadByte();
                    overworldObjects.getOverworldObjs(ob);
                    continue;
                }
            }
            foreach (Object obj in overworldObjects.objectIDs)
            {
                objects.Add(obj);
            }
        }

        public void loadCollisionsOverworld()
        {
            mapData = new byte[80];
            for (int i = 0; i < 80; i++)
                mapData[i] = (byte)(floor + (wall * 0x10));
            foreach (Object obj in objects)
            {
                int dx = (obj.x == 0xF ? (obj.x - 16) : obj.x);
                int dy = (obj.y == 0xF ? (obj.y - 16) : obj.y);
                byte specialObject = obj.id;
                if (!obj.is3Byte)
                {
                    if (obj.special)
                    {
                        for (int h = 0; h < obj.h; h++)
                        {
                            for (int w = 0; w < obj.w; w++)
                            {
                                if (dy < 0)
                                {
                                    obj.y = 0;
                                    dy++;
                                    h++;
                                }
                                if (dx < 0)
                                {
                                    obj.x = 0;
                                    dx++;
                                    w++;
                                }
                                if (dx > 9)
                                {
                                    dx++;
                                    if (w == obj.w - 1)
                                    {
                                        if (obj.hFlip)
                                            dx = obj.x - 1;
                                        else
                                            dx = obj.x;
                                        if (h == obj.h - 1)
                                            dy = obj.y;
                                        else
                                            dy++;
                                    }
                                    continue;
                                }
                                if (dy > 7)
                                {
                                    dx++;
                                    if (w == obj.w - 1)
                                    {
                                        if (obj.vFlip)
                                            dx = obj.x - 1;
                                        else
                                            dx = obj.x;
                                        if (h == obj.h - 1)
                                            dy = obj.y;
                                        else
                                            dy++;
                                    }
                                    continue;
                                }
                                if (obj.hFlip && obj.vFlip)
                                {
                                    obj.id = obj.tiles[(h * obj.w) + w];
                                    mapData[(obj.x + (w - 1)) + ((obj.y + (h - 1)) * 10)] = obj.id;
                                }
                                else if (obj.hFlip)
                                {
                                    obj.id = obj.tiles[(h * obj.w) + w];
                                    mapData[(obj.x + (w - 1)) + ((obj.y + h) * 10)] = obj.id;
                                }
                                else if (obj.vFlip)
                                {
                                    obj.id = obj.tiles[(h * obj.w) + w];
                                    mapData[(obj.x + w) + ((obj.y + (h - 1)) * 10)] = obj.id;
                                }
                                else
                                {
                                    obj.id = obj.tiles[(h * obj.w) + w];
                                    mapData[(obj.x + w) + ((obj.y + h) * 10)] = obj.id;
                                }
                                dx++;
                                if (w >= (obj.w - 1))
                                {
                                    if (obj.hFlip)
                                        dx = obj.x - 1;
                                    else
                                        dx = obj.x;
                                    dy++;
                                }
                            }
                        }
                        obj.id = specialObject;
                        if (obj.hFlip)
                            obj.x = 0x0F;
                        if (obj.vFlip)
                            obj.y = 0x0F;
                    }
                    else
                    {
                        if (dy < 0 || dy > 7)
                            continue;
                        if (dx < 0 || dx > 9)
                            continue;
                        mapData[obj.x + (obj.y * 10)] = (byte)obj.id;
                    }
                }
                else if (obj.is3Byte)
                {
                    if (!obj.special)
                    {
                        for (int i = 0; i < obj.length; i++)
                        {
                            if (obj.direction == 8) //Horizontal
                            {
                                if (dx < 0)
                                {
                                    dx++;
                                    i++;
                                }
                                if (dy < 0 || dy > 7)
                                    continue;
                                if (dx > 9)
                                    continue;
                                if (obj.hFlip)
                                {
                                    obj.x = 0;
                                    mapData[obj.x + (obj.y * 10) + (i - 1)] = obj.id;
                                }
                                else
                                    mapData[obj.x + (obj.y * 10) + i] = obj.id;
                                dx++;
                            }
                            else
                            {
                                if (dx < 0 || dx > 9)
                                    continue;
                                if (dy < 0)
                                {
                                    dy++;
                                    i++;
                                }
                                if (dy > 7)
                                    continue;
                                if (obj.vFlip)
                                {
                                    obj.y = 0;
                                    mapData[obj.x + (obj.y * 10) + ((i - 1) * 10)] = obj.id;
                                }
                                else
                                    mapData[obj.x + (obj.y * 10) + (i * 10)] = obj.id;
                                dy++;
                            }
                        }
                        if (obj.hFlip)
                            obj.x = 0x0F;
                        if (obj.vFlip)
                            obj.y = 0x0F;
                    }
                    else
                    {
                        for (int i = 0; i < obj.length; i++)
                        {
                            if (obj.direction == 8)
                            {
                                if (dx >= 0)
                                {
                                    dx = obj.x + (i * obj.w);
                                    if (obj.hFlip)
                                        dx = obj.x - 1 + (i * obj.w);
                                }
                                if (dy >= 0)
                                {
                                    dy = obj.y;
                                    if (obj.vFlip)
                                        dy = obj.y - 1;
                                }
                            }
                            else
                            {
                                if (dx >= 0)
                                {
                                    dx = obj.x;
                                    if (obj.hFlip)
                                        dx = obj.x - 1;
                                }
                                if (dy >= 0)
                                {
                                    dy = obj.y + (i * obj.h);
                                    if (obj.vFlip)
                                        dy = obj.y - 1 + (i * obj.h);
                                }
                            }
                            for (int h = 0; h < obj.h; h++)
                            {
                                for (int w = 0; w < obj.w; w++)
                                {
                                    if (dy < 0)
                                    {
                                        obj.y = 0;
                                        dy++;
                                        h++;
                                    }
                                    if (dx < 0)
                                    {
                                        obj.x = 0;
                                        dx++;
                                        w++;
                                    }
                                    if (dx > 9)
                                    {
                                        dx++;
                                        if (w == obj.w - 1)
                                        {
                                            if (obj.direction == 8)
                                            {
                                                if (obj.hFlip)
                                                    dx = obj.x - 1 + (i * obj.w);
                                                else
                                                    dx = obj.x + (i * obj.w);
                                            }
                                            else
                                            {
                                                if (obj.hFlip)
                                                    dx = obj.x - 1;
                                                else
                                                    dx = obj.x;
                                            }
                                            if (h == obj.h - 1)
                                                dy = obj.y;
                                            else
                                                dy++;
                                        }
                                        continue;
                                    }
                                    if (dy > 7)
                                    {
                                        dx++;
                                        if (w == obj.w - 1)
                                        {
                                            if (obj.direction == 8)
                                            {
                                                if (obj.vFlip)
                                                    dx = obj.x - 1 + (i * obj.w);
                                                else
                                                    dx = obj.x + (i * obj.w);
                                            }
                                            else
                                            {
                                                if (obj.vFlip)
                                                    dy = obj.y - 1 + (i * obj.h);
                                                else
                                                    dx = obj.x;
                                            }
                                            if (h == obj.h - 1)
                                                dy = obj.y;
                                            else
                                                dy++;
                                        }
                                        continue;
                                    }
                                    if (obj.hFlip && obj.vFlip)
                                    {
                                        obj.id = obj.tiles[(h * obj.w) + w];
                                        if (obj.direction == 8)
                                            mapData[(obj.x + (w - 1) + (i * obj.w)) + (obj.y + (h - 1) * 10)] = obj.id;
                                        else
                                            mapData[(obj.x + (w - 1)) + ((obj.y + (h - 1) + (i * obj.h)) * 10)] = obj.id;
                                    }
                                    else if (obj.hFlip)
                                    {
                                        obj.id = obj.tiles[(h * obj.w) + w];
                                        if (obj.direction == 8)
                                            mapData[(obj.x + (w - 1) + (i * obj.w)) + ((obj.y + h) * 10)] = obj.id;
                                        else
                                            mapData[(obj.x + (w - 1)) + ((obj.y + h + (i * obj.h)) * 10)] = obj.id;
                                    }
                                    else if (obj.vFlip)
                                    {
                                        obj.id = obj.tiles[(h * obj.w) + w];
                                        if (obj.direction == 8)
                                            mapData[(obj.x + w + (i * obj.w)) + ((obj.y + (h - 1)) * 10)] = obj.id;
                                        else
                                            mapData[(obj.x + w) + ((obj.y + (h - 1) + (i * obj.h)) * 10)] = obj.id;
                                    }
                                    else
                                    {
                                        obj.id = obj.tiles[(h * obj.w) + w];
                                        if (obj.direction == 8) //Horizontal
                                            mapData[(obj.x + w + (i * obj.w)) + ((obj.y + h) * 10)] = obj.id;
                                        else
                                            mapData[(obj.x + w) + (((obj.y + h) + (i * obj.h)) * 10)] = obj.id;
                                    }
                                    dx++;
                                    if (w >= (obj.w - 1))
                                    {
                                        if (obj.direction == 8)
                                        {
                                            dx = obj.x + (i * obj.w);
                                            if (obj.hFlip && h != (obj.h - 1))
                                                dx = obj.x - 1 + (i * obj.w);

                                        }
                                        else
                                        {
                                            dx = obj.x;
                                            if (obj.hFlip && h != (obj.h - 1))
                                                dx = obj.x - 1;
                                        }
                                        dy++;
                                    }
                                }
                            }
                        }
                        obj.id = specialObject;
                        if (obj.hFlip)
                            obj.x = 0x0F;
                        if (obj.vFlip)
                            obj.y = 0x0F;
                    }
                }
            }
        }

        public int getUsedSpace()
        {
            int s = 0;
            foreach (Object o in objects)
            {
                if (o.is3Byte)
                    s += 3;
                else
                    s += 2;
            }
            if (warps.Count != 0)
            {
                foreach (Warps w in warps)
                    s += 5;
            }
            return s;
        }

        public int getFreeSpace(byte Map, bool spec)
        {
            unSortedPointers = new List<Int32>();
            pointers = new int[262];
            int cMapPointer = 0;
            int map = 0;
            int index;
            int space = 0;
            while (map < 256)
            {
                int secondhalf;
                int i = 0;
                int i2 = 0;
                secondhalf = 0x24000 + (map * 2);
                secondhalf = gb.Get2BytePointerAddress(secondhalf).Address;
                if (map > 0x7F)
                    secondhalf = 0x68000 + (secondhalf - 0x24000);
                if (map == Map)
                    cMapPointer = secondhalf;
                pointers[map] = secondhalf;
                switch (map)
                {
                    case 0x06: i = 0x31F4; i2 = 1; break;
                    case 0x0E: i = 0x31C4; i2 = 2; break;
                    case 0x1B: i = 0x3204; i2 = 3; break;
                    case 0x2B: i = 0x3214; i2 = 4; break;
                    case 0x79: i = 0x31E4; i2 = 5; break;
                    case 0x8C: i = 0x31D4; i2 = 6; break;
                }
                if (i > 0)
                {
                    secondhalf = gb.Get2BytePointerAddress(i).Address;
                    if (map > 0x7F)
                        secondhalf += 0x68000;
                    else
                        secondhalf += 0x24000;
                    if (spec && map == Map)
                        cMapPointer = secondhalf;
                    pointers[i2 + 0xFF] = secondhalf;
                }
                map++;
            }
            foreach (int point in pointers)
                unSortedPointers.Add(point);
            Array.Sort(pointers);
            index = Array.IndexOf(pointers, cMapPointer);
            if (Map == 0xFF)
            {
                gb.BufferLocation = cMapPointer;
                space = 0x69E73 - cMapPointer;
            }
            else if (Map == 0x7F)
            {
                gb.BufferLocation = cMapPointer;
                space = 0x2668B - cMapPointer;
            }
            //else if (Map == 0x51 || Map == 0x63)
            // space = ((int)pointers.GetValue(index + 2) - 3) - cMapPointer;
            else
            {
                while ((int)pointers.GetValue(index + 1) == cMapPointer)
                    index++;
                space = ((int)pointers.GetValue(index + 1) - 3) - cMapPointer;
            }
            return space;
        }

        public Bitmap drawSelectedObject(Bitmap image, Object selected)
        {
            FastPixel fp = new FastPixel(image);
            Color border = Color.White;
            fp.rgbValues = new byte[160 * 128 * 4];
            fp.Lock();
            foreach (Object obj in objects)
            {
                if (obj.x != selected.x)
                    continue;
                if (obj.y != selected.y)
                    continue;
                if (obj.direction != selected.direction)
                    continue;
                if (obj.length != selected.length)
                    continue;
                if (obj.id != selected.id)
                    continue;
                int x = obj.x * 16;
                int y = obj.y * 16;
                if (obj.hFlip)
                    x = -16;
                if (obj.vFlip)
                    y = -16;
                bool v = false;
                bool h = false;
                if (!obj.is3Byte && !obj.special)
                {
                    if (obj.x > 9 || obj.y > 7)
                        continue;
                }
                if (!obj.is3Byte)
                {
                    if (!obj.special)
                    {
                        for (int yy = 0; yy < 16; yy++)
                        {
                            for (int xx = 0; xx < 16; xx++)
                            {
                                if (yy > 0 && yy != 15)
                                {
                                    if (xx == 0 || xx == 15)
                                    {
                                        fp.SetPixel(x + xx, y + yy, border);
                                    }
                                }
                                else
                                {
                                    fp.SetPixel(x + xx, y + yy, border);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int yy = 0; yy < obj.h * 16; yy++)
                        {
                            for (int xx = 0; xx < obj.w * 16; xx++)
                            {
                                if (x < 0 && !h)
                                {
                                    xx = xx + 16;
                                    h = true;
                                }
                                if (y < 0 && !v)
                                {
                                    yy = yy + 16;
                                    v = true;
                                }
                                if (x + xx >= 160 || x + xx < 0)
                                    continue;
                                if (y + yy >= 128 || y + yy < 0)
                                    continue;
                                if (yy > 0 && yy != ((obj.h * 16) - 1))
                                {
                                    if (xx == 0 || xx == (obj.w * 16) - 1)
                                        fp.SetPixel(x + xx, y + yy, border);
                                }
                                else
                                    fp.SetPixel(x + xx, y + yy, border);
                            }
                        }
                    }
                }
                else
                {
                    if (!obj.special)
                    {
                        if (obj.direction == 8)
                        {
                            for (int yy = 0; yy < 16; yy++)
                            {
                                for (int xx = 0; xx < (obj.length * 16); xx++)
                                {
                                    if (x < 0 && !h)
                                    {
                                        xx = xx + 16;
                                        h = true;
                                    }
                                    if (y < 0 && !v)
                                    {
                                        yy = yy + 16;
                                        v = true;
                                    }
                                    if (x + xx >= 160 || x + xx < 0)
                                        continue;
                                    if (y + yy >= 128 || y + yy < 0)
                                        continue;
                                    if (yy > 0 && yy != 15)
                                    {
                                        if (xx == 0 || xx == (obj.length * 16) - 1)
                                            fp.SetPixel(x + xx, y + yy, border);
                                    }
                                    else
                                        fp.SetPixel(x + xx, y + yy, border);
                                }
                            }
                        }
                        else
                        {
                            for (int yy = 0; yy < obj.length * 16; yy++)
                            {
                                for (int xx = 0; xx < 16; xx++)
                                {
                                    if (x < 0 && !h)
                                    {
                                        xx = xx + 16;
                                        h = true;
                                    }
                                    if (y < 0 && !v)
                                    {
                                        yy = yy + 16;
                                        v = true;
                                    }
                                    if (x + xx >= 160 || x + xx < 0)
                                        continue;
                                    if (y + yy >= 128 || y + yy < 0)
                                        continue;
                                    if (yy > 0 && yy != (obj.length * 16) - 1)
                                    {
                                        if (xx == 0 || xx == 15)
                                            fp.SetPixel(x + xx, y + yy, border);
                                    }
                                    else
                                        fp.SetPixel(x + xx, y + yy, border);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (obj.direction == 8)
                        {
                            for (int i = 0; i < obj.length; i++)
                            {
                                for (int yy = 0; yy < obj.h * 16; yy++)
                                {
                                    for (int xx = 0 + (i * 16); xx < obj.w * (obj.length * 16); xx++)
                                    {
                                        if (x < 0 && !h)
                                        {
                                            xx = xx + 16;
                                            h = true;
                                        }
                                        if (y < 0 && !v)
                                        {
                                            yy = yy + 16;
                                            v = true;
                                        }
                                        if (x + xx >= 160 || x + xx < 0)
                                            continue;
                                        if (y + yy >= 128 || y + yy < 0)
                                            continue;
                                        if (yy > 0 && yy != ((obj.h * 16) - 1))
                                        {
                                            if (xx == 0 || xx == ((obj.length * 16) * obj.w) - 1)
                                                fp.SetPixel(x + xx, y + yy, border);
                                        }
                                        else
                                            fp.SetPixel(x + xx, y + yy, border);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < obj.length; i++)
                            {
                                for (int yy = 0 + (i * 16); yy < obj.h * (obj.length * 16); yy++)
                                {
                                    for (int xx = 0; xx < obj.w * 16; xx++)
                                    {
                                        if (x < 0 && !h)
                                        {
                                            xx = xx + 16;
                                            h = true;
                                        }
                                        if (y < 0 && !v)
                                        {
                                            yy = yy + 16;
                                            v = true;
                                        }
                                        if (x + xx >= 160 || x + xx < 0)
                                            continue;
                                        if (y + yy >= 128 || y + yy < 0)
                                            continue;
                                        if (yy > 0 && yy != ((obj.length * 16) * obj.h) - 1)
                                        {
                                            if (xx == 0 || xx == (obj.w * 16) - 1)
                                                fp.SetPixel(x + xx, y + yy, border);
                                        }
                                        else
                                            fp.SetPixel(x + xx, y + yy, border);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            fp.Unlock(true);
            return image;
        }
    }
}
