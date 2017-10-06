using System;
using System.Collections.Generic;
using System.Text;
using GBHL;
using System.Drawing;

namespace LALE
{
    class DungeonDrawer
    {

        GBFile gb;
        public List<LAObject> objects = new List<LAObject>();
        private LAObject[,] wallTiles = new LAObject[15, 64];
        public List<Warps> warps = new List<Warps>();
        public int mapAddress;
        public byte[] data;
        public int[] pointers;
        public List<Int32> unSortedPointers;
        public byte floor;
        public byte wall;
        public byte music;
        public byte spriteBank;
        public byte eventID;
        public byte eventTrigger;
        public int eventDataLocation;

        public void getMusic(byte dungeon, byte map)
        {
            gb.BufferLocation = 0x8100 + dungeon;
            if (dungeon == 0xFF)
                gb.BufferLocation = 0x8109;
            if (map == 0xB5 && dungeon < 0x1A && dungeon >= 6)
                gb.BufferLocation = 0x810F;
            music = gb.ReadByte();
        }

        public DungeonDrawer(GBFile g)
        {
            gb = g;
        }

        public void getCollisionDataDungeon(byte map, byte dungeon, bool magGlass)
        {
            objects = new List<LAObject>();
            warps = new List<Warps>();
            if (magGlass && map == 0xF5 && dungeon >= 0x1A || magGlass && map == 0xF5 && dungeon < 6)
            {
                gb.BufferLocation = gb.Get2BytePointerAddress(0x3198).Address;
                gb.BufferLocation += 0x28001;
            }
            else
            {
                gb.BufferLocation = 0x28000;
                if (dungeon >= 6 && dungeon < 0x1A)
                    gb.BufferLocation += 0x4000;
                else if (dungeon == 0xFF)
                    gb.BufferLocation = 0x2BB77;
                gb.BufferLocation = gb.Get3BytePointerAddress((byte)(gb.BufferLocation / 0x4000), gb.BufferLocation + (map * 2)).Address + 1; //skip animation    
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
                    LAObject o = new LAObject();
                    o.is3Byte = true;
                    o.length = (byte)(b & 0xF);
                    o.direction = (byte)(b >> 4);
                    byte b2 = gb.ReadByte();
                    o.y = (byte)(b2 >> 4);
                    o.x = (byte)(b2 & 0xF);
                    o.id = gb.ReadByte();
                    objects.Add(o);
                    continue;
                }
                LAObject ob = new LAObject(); // 2-Byte tiles
                ob.y = (byte)(b >> 4);
                ob.x = (byte)(b & 0xF);
                ob.id = gb.ReadByte();
                if (ob.id >= 0xEC && ob.id <= 0xFD)// Door tiles
                {
                    int bufferloc = gb.BufferLocation;
                    ob.gb = gb;
                    ob = ob.dungeonDoors(ob);
                    gb.BufferLocation = bufferloc;
                }
                objects.Add(ob);
            }
        }

        public void loadCollisionsDungeon()
        {
            data = new byte[80];
            for (int i = 0; i < 80; i++)
                data[i] = floor;
            for (int i = 0; i < 64; i++)
            {
                LAObject o = wallTiles[wall, i];
                if (o != null)
                    data[o.x + (o.y * 10)] = (byte)o.id;
            }
            foreach (LAObject obj in objects)
            {
                byte door = obj.id;
                int dx = (obj.x == 0xF ? (obj.x - 16) : obj.x);
                int dy = (obj.y == 0xF ? (obj.y - 16) : obj.y);

                if (!obj.is3Byte && !obj.isDoor1 && !obj.isDoor2 && !obj.isEntrance)
                {
                    if (dy < 0 || dy > 7)
                        continue;
                    if (dx < 0 || dx > 9)
                        continue;
                    data[obj.x + (obj.y * 10)] = (byte)obj.id;
                }
                else if (obj.is3Byte)
                {
                    for (int i = 0; i < obj.length; i++)
                    {
                        if (obj.direction == 8) //horizontal
                        {
                            if (dy > 7 || dy < 0)
                                continue;
                            if (dx < 0)
                            {
                                dx++;
                                obj.x = 0;
                                i++;
                            }
                            if (dx > 10)
                                continue;
                            if (obj.x + (obj.y * 10) + i >= 80)
                            {
                                continue;
                            }
                            data[obj.x + (obj.y * 10) + i] = obj.id;
                            dx++;

                        }
                        else
                        {
                            if (dx > 9)
                                continue;
                            if (dy < 0)
                            {
                                dy++;
                                obj.y = 0;
                                i++;
                            }
                            if (dy > 7)
                                continue;
                            if (obj.x + (obj.y * 10) + (i * 10) >= 80)
                            {
                                continue;
                            }
                            data[obj.x + (obj.y * 10) + (i * 10)] = obj.id;
                            dy++;
                        }
                    }
                }
                else if (obj.isDoor1)
                {
                    obj.w = 2;
                    obj.h = 1;
                    for (int i = 0; i < 2; i++)
                    {
                        if (dx < 0)
                        {
                            dx++;
                            obj.x = 0;
                            i++;
                        }
                        if (dx > 9)
                            continue;
                        if (dy > 7)
                            continue;

                        if ((obj.x + i) + (obj.y * 10) >= 80)
                        {
                            continue;
                        }
                        obj.id = obj.tiles[i];
                        data[(obj.x + i) + (obj.y * 10)] = (byte)obj.id;
                        dx++;
                    }
                    obj.id = door;
                }
                else if (obj.isDoor2)
                {
                    obj.h = 2;
                    obj.w = 1;
                    for (int i = 0; i < 2; i++)
                    {
                        if (dy < 0)
                        {
                            dy++;
                            obj.y = 0;
                            i++;
                        }
                        if (dy > 7)
                            continue;
                        if (dx > 9)
                            continue;
                        if (obj.x + ((obj.y + i) * 10) >= 80)
                        {
                            continue;
                        }
                        obj.id = obj.tiles[i];
                        
                        data[obj.x + ((obj.y + i) * 10)] = obj.id;
                        dy++;
                    }
                    obj.id = door;
                }
                else if (obj.isEntrance)
                {
                    int i = 0;
                    for (int y = 0; y < 3; y++)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            if (dy < 0)
                            {
                                dy++;
                                obj.y = 0;
                                i++;
                            }
                            if (dy > 7)
                                continue;
                            if (dx < 0)
                            {
                                dx++;
                                obj.x = 0;
                                i++;
                            }
                            if (dx > 9)
                                continue;
                            if ((obj.x + x) + ((obj.y + y) * 10) >= 80)
                            {
                                continue;
                            }
                            obj.id = obj.tiles[i];
                            data[(obj.x + x) + ((obj.y + y) * 10)] = (byte)obj.id;
                            i++;
                            dx++;
                            if (x == 3)
                            {
                                dy++;
                                dx = obj.x;
                            }
                        }
                    }
                    obj.id = door;
                }
            }
        }

        public int getUsedSpace()
        {
            int s = 0;
            foreach (LAObject o in objects)
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

        public Bitmap DrawCollisions(Bitmap tiles, byte[] mapData, bool borders)
        {
            Bitmap b = new Bitmap(160, 128);
            FastPixel fp = new FastPixel(b);
            FastPixel src = new FastPixel(tiles);
            src.rgbValues = new byte[256 * 256 * 4];
            fp.rgbValues = new byte[160 * 128 * 4];
            fp.Lock();
            src.Lock();
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    byte id = mapData[x + (y * 10)];
                    for (int yy = 0; yy < 16; yy++)
                    {
                        for (int xx = 0; xx < 16; xx++)
                        {
                            fp.SetPixel(x * 16 + xx, y * 16 + yy, src.GetPixel((id % 16) * 16 + xx, (id / 16) * 16 + yy));
                        }
                    }
                }
            }
            fp.Unlock(true);
            src.Unlock(true);
            if (borders)
                drawBorders(b);
            return b;
        }

        public Bitmap drawBorders(Bitmap image)
        {
            FastPixel fp = new FastPixel(image);
            fp.rgbValues = new byte[160 * 128 * 4];
            fp.Lock();
            foreach (LAObject obj in objects)
            {
                int x = obj.x * 16;
                int y = obj.y * 16;
                if (obj.hFlip)
                    x = -16;
                if (obj.vFlip)
                    y = -16;
                bool v = false;
                bool h = false;
                if (!obj.is3Byte)
                {
                    if (obj.x > 9 || obj.y > 7)
                        continue;
                }
                if (!obj.is3Byte)
                {
                    if (obj.isEntrance || obj.isDoor1 || obj.isDoor2)
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
                                        fp.SetPixel(x + xx, y + yy, Color.DeepPink);
                                }
                                else
                                    fp.SetPixel(x + xx, y + yy, Color.DeepPink);
                            }
                        }
                    }
                    else
                    {
                        for (int yy = 0; yy < 16; yy++)
                        {
                            for (int xx = 0; xx < 16; xx++)
                            {
                                if (yy > 0 && yy != 15)
                                {
                                    if (xx == 0 || xx == 15)
                                        fp.SetPixel(x + xx, y + yy, Color.DarkGreen);
                                }
                                else
                                    fp.SetPixel(x + xx, y + yy, Color.DarkGreen);
                            }
                        }
                    }
                }
                else
                {
                    if (!obj.isDoor1 || !obj.isDoor2 || !obj.isEntrance)
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
                                            fp.SetPixel(x + xx, y + yy, Color.DarkRed);
                                    }
                                    else
                                        fp.SetPixel(x + xx, y + yy, Color.DarkRed);
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
                                            fp.SetPixel(x + xx, y + yy, Color.DarkBlue);
                                    }
                                    else
                                        fp.SetPixel(x + xx, y + yy, Color.DarkBlue);
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
                                                fp.SetPixel(x + xx, y + yy, Color.DarkGoldenrod);
                                        }
                                        else
                                            fp.SetPixel(x + xx, y + yy, Color.DarkGoldenrod);
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
                                            if (xx == 0 || xx == obj.w * 16)
                                                fp.SetPixel(x + xx, y + yy, Color.Purple);
                                        }
                                        else
                                            fp.SetPixel(x + xx, y + yy, Color.Purple);
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

        public void getWallsandFloor(byte dungeon, byte map, bool magGlass)
        {
            if (magGlass && map == 0xF5 && dungeon >= 0x1A)
            {
                gb.BufferLocation = gb.Get2BytePointerAddress(0x3198).Address;
                gb.BufferLocation += 0x28001;
            }
            else
            {
                gb.BufferLocation = 0x28000;
                if (dungeon >= 6 && dungeon < 0x1A)
                    gb.BufferLocation += 0x4000;
                else if (dungeon == 0xFF)
                    gb.BufferLocation = 0x2BB77;
                gb.BufferLocation = gb.Get3BytePointerAddress((byte)(gb.BufferLocation / 0x4000), gb.BufferLocation + (map * 2)).Address + 1; //skip animation     
            }
            byte b = gb.ReadByte();
            wall = (byte)(b >> 4); //Upper nybble = wall type
            floor = (byte)(b & 0xF); //Lower nybble = floor type

            spriteBank = gb.ReadByte(0x830DB + ((dungeon <= 5 ? 1 : 2) << 8) + map);
        }

        public void loadWalls()
        {
            gb.BufferLocation = 0x50917;
            for (int i = 0; i < 9; i++)
            {
                byte b = 0;
                int count = 0;
                List<LAObject> listt = new List<LAObject>();
                while ((b = gb.ReadByte()) != 0xFF)
                {
                    LAObject t = new LAObject();
                    t.y = (byte)(b / 16);
                    t.x = (byte)(b - (t.y * 16));
                    listt.Add(t);
                    count++;
                }
                byte[] buffer = gb.ReadBytes(count);
                for (int k = 0; k < count; k++)
                {
                    LAObject t = listt[k];
                    t.id = buffer[k];
                    listt[k] = t;
                    wallTiles[i, k] = listt[k];
                }
            }
        }

        public int getFreeSpace(byte Map, byte dungeon, bool magGlass)
        {
            pointers = new int[257];
            unSortedPointers = new List<Int32>();
            int cMapPointer = 0;
            int map = 0;
            int index;
            int space = 0;
            while (map < 256)
            {
                gb.BufferLocation = 0x28000 + (map * 2);
                if (dungeon >= 6 && dungeon < 0x1A)
                    gb.BufferLocation += 0x4000;
                else if (dungeon == 0xFF)
                    gb.BufferLocation = 0x2BB77 + (map * 2);
                gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                //gb.Get3BytePointerAddress((byte)(gb.BufferLocation / 0x4000), gb.BufferLocation + (map * 2))
                if (map == Map)
                    cMapPointer = gb.BufferLocation;
                pointers[map] = gb.BufferLocation;
                if (dungeon >= 0x1A || dungeon < 0x6)
                {
                    switch (map)
                    {
                        case 0xF5:
                            {
                                gb.BufferLocation = gb.Get2BytePointerAddress(0x3198).Address;
                                gb.BufferLocation += 0x28000;
                                if (magGlass && map == Map)
                                    cMapPointer = gb.BufferLocation;
                                pointers[1 + 0xFF] = gb.BufferLocation;
                                break;
                            }

                    }
                }
                map++;
            }
            foreach (int point in pointers)
                unSortedPointers.Add(point);
            Array.Sort(pointers);
            index = Array.IndexOf(pointers, cMapPointer);

            if (dungeon == 0xFF && Map == 0x15)
                space = 0x2BF43 - (cMapPointer + 3);
            else if (Map != 0xFF)
            {
                while ((int)pointers.GetValue(index + 1) == cMapPointer)
                    index++;
                space = ((int)pointers.GetValue(index + 1) - 3) - cMapPointer;
            }
            else
            {
                gb.BufferLocation = cMapPointer;
                if (dungeon < 6 || dungeon >= 0x1A)
                    space = 0x2BB77 - (cMapPointer + 3);
                else
                    space = 0x2FFFF - (cMapPointer + 2);
            }
            return space;
        }

        public void getEventData(byte map, byte dungeon)
        {
            if (dungeon == 0xFF)
                gb.BufferLocation = 0x50200 + map;
            else if (dungeon >= 6 && dungeon < 0x1A)
                gb.BufferLocation = 0x50100 + map;
            else
                gb.BufferLocation = 0x50000 + map;
            eventDataLocation = gb.BufferLocation;
            byte b = gb.ReadByte();
            eventID = (byte)(b >> 4);
            eventTrigger = (byte)(b & 0xF);
        }

        public Bitmap drawSelectedObject(Bitmap image, LAObject selected)
        {
            Color border = Color.White;
            FastPixel fp = new FastPixel(image);
            fp.rgbValues = new byte[160 * 128 * 4];
            fp.Lock();
            foreach (LAObject obj in objects)
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

                if (!obj.is3Byte)
                {
                    if (obj.x > 9 || obj.y > 7)
                        continue;
                }
                if (!obj.is3Byte)
                {
                    if (obj.isEntrance || obj.isDoor1 || obj.isDoor2)
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
                    else
                    {
                        for (int yy = 0; yy < 16; yy++)
                        {
                            for (int xx = 0; xx < 16; xx++)
                            {
                                if (yy > 0 && yy != 15)
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
                    if (!obj.isDoor1 || !obj.isDoor2 || !obj.isEntrance)
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
                                            if (xx == 0 || xx == obj.w * 16)
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
