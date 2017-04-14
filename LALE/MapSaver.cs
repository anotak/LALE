using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using GBHL;

namespace LALE
{
    class MapSaver
    {

        public GBFile gb;

        public MapSaver(GBFile g)
        {
            gb = g;
        }

        public void saveMinimapInfo(bool overWorld, byte dungeon, byte[] roomIndexes, byte[] minimapGraphics, byte[] overworldpal)
        {
            if (!overWorld)
            {
                if (dungeon < 9 || dungeon == 0xFF)
                {
                    if (dungeon == 0xFF)
                        gb.BufferLocation = 0x504E0;
                    else
                        gb.BufferLocation = 0x50220 + (64 * dungeon);
                    gb.WriteBytes(roomIndexes);
                    if (dungeon == 0xFF)
                        gb.BufferLocation = 0xA49A + (64 * 0x9);
                    else
                        gb.BufferLocation = 0xA49A + (64 * dungeon);
                    gb.WriteBytes(minimapGraphics);
                }
            }
            else
            {
                gb.WriteBytes(0x81797, overworldpal);
                gb.WriteBytes(0x81697, minimapGraphics);
            }
        }

        public void saveChestInfo(bool overWorld, byte map, byte dungeon, byte chest)
        {
            if (overWorld)
                gb.BufferLocation = 0x50560 + map;
            else
            {
                if (dungeon >= 6 && dungeon < 0x1A)
                    gb.BufferLocation = 0x50760 + map;
                else if (dungeon == 0xFF)
                    gb.BufferLocation = 0x50860 + map;
                else
                    gb.BufferLocation = 0x50660 + map;
            }

            gb.WriteByte(chest);
        }

        public void saveAreaInfo(bool overWorld, byte map, byte dungeon, byte animations, byte SOG, byte music, bool special, bool magGlass)
        {
            if (overWorld)
            {
                //0x9000-0x9200
                int i = -1;
                int secondhalf;
                int map2 = map;
                if (map == 0x7)
                    map++;
                byte b = (byte)((map >> 2) & 0xF8);
                byte b1 = (byte)(((map >> 1) & 0x07) | b);
                gb.BufferLocation = 0x82E7B + b1;
                gb.WriteByte(SOG);

                if (special)
                {
                    switch (map2)
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
                    if (map2 > 0x7F)
                        secondhalf += 0x68000;
                    else
                        secondhalf += 0x24000;
                    gb.BufferLocation = secondhalf;
                }
                else
                {
                    gb.BufferLocation = 0x24000 + (map2 * 2);
                    gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                    if (map2 > 0x7F)
                        gb.BufferLocation = 0x68000 + (gb.BufferLocation - 0x24000);
                }
                gb.WriteByte(animations);

                gb.BufferLocation = 0x8000 + map2;
                gb.WriteByte(music);
            }
            else
            {
                //0x9000-0x9100
                if (dungeon >= 6 && dungeon < 0x1A)
                    gb.BufferLocation = 0x82FBB + map;
                else if (dungeon == 0xFF)
                    gb.BufferLocation = 0x830BB + map;
                else
                    gb.BufferLocation = 0x82EBB + map;
                gb.WriteByte(SOG);

                if (magGlass && dungeon >= 0x1A && map == 0xF5)
                {
                    gb.BufferLocation = gb.Get2BytePointerAddress(0x3198).Address;
                    gb.BufferLocation += 0x28000;
                }
                else
                {
                    gb.BufferLocation = 0x28000;
                    if (dungeon >= 0x06 && dungeon < 0x1A)
                        gb.BufferLocation += 0x4000;
                    else if (dungeon == 0xFF)
                        gb.BufferLocation = 0x2BB77;
                    gb.BufferLocation = gb.Get3BytePointerAddress((byte)(gb.BufferLocation / 0x4000), gb.BufferLocation + (map * 2)).Address;
                }
                gb.WriteByte(animations);

                gb.BufferLocation = 0x8100 + dungeon;
                if (dungeon == 0xFF)
                    gb.BufferLocation = 0x8109;
                if (map == 0xB5 && dungeon < 0x1A && dungeon >= 6)
                    gb.BufferLocation = 0x810F;
                gb.WriteByte(music);
            }
        }

        public void saveOverlay(byte[] data, byte map, bool s)
        {
            int i = 0;
            if (s)
            {
                switch (map)
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
                gb.BufferLocation = 0x98000 + i;
            else if (map < 0xCC)
                gb.BufferLocation = 0x98000 + 0x50 * map;
            else
                gb.BufferLocation = 0x9C000 + 0x50 * (map - 0xCC);

            foreach (byte b in data)
                gb.WriteByte(b);
        }

        public bool saveOverworldCollision(List<Object> objects, List<Warps> warps, byte map, byte floor, byte unknown, bool special, int usedspace, int freespace, bool s, int[] pointers, List<Int32> unSortedPointers)
        {
            int cMapPointer;
            int secondhalf;
            int index;
            int i = 0;
            bool check;
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
            }
            else
            {
                secondhalf = 0x24000 + (map * 2);
                secondhalf = gb.Get2BytePointerAddress(secondhalf).Address + 1;
                if (map > 0x7F)
                    secondhalf = 0x68000 + (secondhalf - 0x24000);
            }
            cMapPointer = secondhalf - 1;
            gb.BufferLocation = secondhalf;
            byte b = (byte)((unknown * 0x10) + floor);
            gb.WriteByte(b);
            foreach (Object obj in objects)
            {
                check = checkWrite(true, false);
                if (check != false)
                    return true;
                if (obj.is3Byte)
                {
                    if (obj.direction == 8)
                    {
                        b = (byte)(0x80 + obj.length);
                        gb.WriteByte(b);
                        check = checkWrite(true, false);
                        if (check != false)
                            return true;
                    }
                    else
                    {
                        b = (byte)(0xC0 + obj.length);
                        gb.WriteByte(b);
                        check = checkWrite(true, false);
                        if (check != false)
                            return true;
                    }
                    b = (byte)((obj.y * 0x10) + obj.x);
                    gb.WriteByte(b);
                    check = checkWrite(true, false);
                    if (check != false)
                        return true;
                    gb.WriteByte(obj.id);
                    check = checkWrite(true, false);
                    if (check != false)
                        return true;
                }
                else
                {
                    b = (byte)((obj.y * 0x10) + obj.x);
                    gb.WriteByte(b);
                    check = checkWrite(true, false);
                    if (check != false)
                        return true;
                    gb.WriteByte(obj.id);
                    check = checkWrite(true, false);
                    if (check != false)
                        return true;
                }
            }
            if (warps.Count != 0)
            {
                foreach (Warps warp in warps)
                {
                    check = checkWrite(true, false);
                    if (check != false)
                        return true;
                    b = (byte)(0xE0 + warp.type);
                    gb.WriteByte(b);
                    check = checkWrite(true, false);
                    if (check != false)
                        return true;
                    gb.WriteByte(warp.region);
                    check = checkWrite(true, false);
                    if (check != false)
                        return true;
                    gb.WriteByte(warp.map);
                    check = checkWrite(true, false);
                    if (check != false)
                        return true;
                    gb.WriteByte(warp.x);
                    check = checkWrite(true, false);
                    if (check != false)
                        return true;
                    gb.WriteByte(warp.y);
                }
            }
            check = checkWrite(true, false);
            if (check != false)
                return true;
            if (usedspace < freespace)
            {
                b = 0;
                gb.WriteByte(0xFE);
                usedspace += 1;
                while (usedspace != freespace)
                {
                    gb.WriteByte(b);
                    usedspace++;
                }
            }
            else if (usedspace > freespace && map != 0xFF)
            {
                gb.WriteByte(0xFE);
                byte[] bytes = gb.Get2BytePointer(gb.BufferLocation);
                gb.WriteByte(0);
                gb.WriteByte(0);
                index = Array.IndexOf(pointers, cMapPointer);
                int imaps = 0;
                int index2 = 0;
                while (pointers[index + 1] == cMapPointer)
                    index++;
                index2 = index2 + index;
                cMapPointer = pointers[index2 + 1];
                while (pointers[index + 1] == cMapPointer)
                {
                    imaps++;
                    index++;
                }
                if (imaps != 1)
                {
                    int i2 = 0;
                    for (int imap = imaps; imap != 0; imap--)
                    {
                        index = unSortedPointers.IndexOf(cMapPointer);
                        gb.WriteBytes((0x24000 + ((index + i2) * 2)), bytes);
                        unSortedPointers.RemoveAt(index);
                        i2++;
                    }
                }
                else
                {
                    index = unSortedPointers.IndexOf(cMapPointer);
                    if (index > 255)
                    {
                        if (index == 256)
                            gb.BufferLocation = 0x31F4;
                        else if (index == 257)
                            gb.BufferLocation = 0x31C4;
                        else if (index == 258)
                            gb.BufferLocation = 0x3204;
                        else if (index == 259)
                            gb.BufferLocation = 0x3214;
                        else if (index == 260)
                            gb.BufferLocation = 0x31E4;
                        else if (index == 261)
                            gb.BufferLocation = 0x31D4;
                        gb.WriteBytes(bytes);
                    }
                    else
                        gb.WriteBytes((0x24000 + (index * 2)), bytes);
                }
            }
            return false;
        }

        public bool saveDungeonCollision(List<Object> objects, List<Warps> warps, byte dungeon, byte map, byte floor, byte wall, int usedspace, int freespace, int[] pointers, List<Int32> unSortedPointers, bool magGlass)
        {
            int cMapPointer;
            int index;
            bool check;
            if (magGlass && dungeon >= 0x1A && map == 0xF5)
            {
                gb.BufferLocation = gb.Get2BytePointerAddress(0x3198).Address;
                gb.BufferLocation += 0x28001; //Skip animation
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
            cMapPointer = gb.BufferLocation - 1;
            byte b = (byte)((wall * 0x10) + floor);
            gb.WriteByte(b);
            foreach (Object obj in objects)
            {
                check = checkWrite(false, false);
                if (check != false)
                    return true;
                if (obj.is3Byte)
                {
                    if (obj.direction == 8)
                    {
                        b = (byte)(0x80 + obj.length);
                        gb.WriteByte(b);
                        check = checkWrite(false, false);
                        if (check != false)
                            return true;
                    }
                    else
                    {
                        b = (byte)(0xC0 + obj.length);
                        gb.WriteByte(b);
                        check = checkWrite(false, false);
                        if (check != false)
                            return true;
                    }
                    b = (byte)((obj.y * 0x10) + obj.x);
                    gb.WriteByte(b);
                    check = checkWrite(false, false);
                    if (check != false)
                        return true;
                    gb.WriteByte(obj.id);
                    check = checkWrite(false, false);
                    if (check != false)
                        return true;
                }
                else
                {
                    b = (byte)((obj.y * 0x10) + obj.x);
                    gb.WriteByte(b);
                    check = checkWrite(false, false);
                    if (check != false)
                        return true;
                    gb.WriteByte(obj.id);
                    check = checkWrite(false, false);
                    if (check != false)
                        return true;
                }
            }
            if (warps.Count != 0)
            {
                foreach (Warps warp in warps)
                {
                    check = checkWrite(false, false);
                    if (check != false)
                        return true;
                    b = (byte)(0xE0 + warp.type);
                    gb.WriteByte(b);
                    check = checkWrite(false, false);
                    if (check != false)
                        return true;
                    gb.WriteByte(warp.region);
                    check = checkWrite(false, false);
                    if (check != false)
                        return true;
                    gb.WriteByte(warp.map);
                    check = checkWrite(false, false);
                    if (check != false)
                        return true;
                    gb.WriteByte(warp.x);
                    check = checkWrite(false, false);
                    if (check != false)
                        return true;
                    gb.WriteByte(warp.y);
                }
            }
            check = checkWrite(false, false);
            if (check != false)
                return true;
            if (usedspace < freespace)
            {
                b = 0;
                gb.WriteByte(0xFE);
                usedspace += 1;
                while (usedspace != freespace)
                {
                    gb.WriteByte(b);
                    usedspace++;
                }
            }
            else if (usedspace > freespace)
            {
                int c;
                int x = 1;
                gb.WriteByte(0xFE);
                c = gb.BufferLocation;
                byte[] bytes = gb.Get2BytePointer(gb.BufferLocation);
                gb.WriteByte(0);
                gb.WriteByte(0);
                index = Array.IndexOf(pointers, cMapPointer);
                int imaps = 0;
                int index2 = 0;
                while (pointers[index + 1] == cMapPointer)
                    index++;
                index2 = index2 + index;
                cMapPointer = pointers[index2 + 1];
                while (pointers[index + 1] == cMapPointer)
                {
                    imaps++;
                    index++;
                }
                if (imaps == 1)
                {
                    index = unSortedPointers.IndexOf(cMapPointer);
                    if (index > 0xFF)
                    {
                        gb.WriteBytes((0x3198), bytes);
                        index = Array.IndexOf(pointers, cMapPointer);
                    }
                    else
                    {
                        if (dungeon >= 0x6 && dungeon < 0x1A)
                            gb.WriteBytes((0x2C000 + (index * 2)), bytes);
                        else if (dungeon == 0xFF)
                            gb.WriteBytes((0x2BB77 + (index * 2)), bytes);
                        else
                            gb.WriteBytes((0x28000 + (index * 2)), bytes);
                    }
                }
                else
                {
                    int i = 0;
                    for (int imap = imaps; imap != 0; imap--)
                    {
                        index = unSortedPointers.IndexOf(cMapPointer);
                        if (dungeon >= 0x6 && dungeon < 0x1A)
                            gb.WriteBytes((0x2C000 + ((index + i) * 2)), bytes);
                        else if (dungeon == 0xFF)
                            gb.WriteBytes((0x2BB77 + ((index + i) * 2)), bytes);
                        else
                            gb.WriteBytes((0x28000 + ((index + i) * 2)), bytes);
                        unSortedPointers.RemoveAt(index);
                        i++;
                    }
                }
                while (unSortedPointers[index + x] < c)
                {
                    c = c - unSortedPointers[index + x];
                    cMapPointer = unSortedPointers[index + x] + c;
                    gb.BufferLocation = cMapPointer;
                    gb.WriteByte(0xFE);
                    c = gb.BufferLocation;
                    bytes = gb.Get2BytePointer(gb.BufferLocation);
                    if (dungeon >= 0x6 && dungeon < 0x1A)
                        gb.WriteBytes((0x2C000 + ((index + x) * 2)), bytes);
                    else if (dungeon == 0xFF)
                        gb.WriteBytes((0x2BB77 + ((index + x) * 2)), bytes);
                    else
                        gb.WriteBytes((0x28000 + ((index + x) * 2)), bytes);
                    x++;
                }
            }
            return false;
        }

        public bool checkWrite(bool overworld, bool sprites)
        {
            if (!sprites)
            {
                if (!overworld)
                {

                    if (gb.BufferLocation > 0x2BB76 && gb.BufferLocation < 0x2BBB7 || gb.BufferLocation > 0x2FFFF)
                    {
                        System.Windows.Forms.MessageBox.Show("Not all collisions/warps could be saved because it would overwrite other data.");
                        gb.WriteByte(gb.BufferLocation - 1, 0xFE);
                        return true;
                    }
                }
                else
                {
                    if (gb.BufferLocation > 0x2668D && gb.BufferLocation < 0x68000 || gb.BufferLocation > 0x69E75)
                    {
                        System.Windows.Forms.MessageBox.Show("Not all collisions/warps could be saved because it would overwrite other data.");
                        gb.WriteByte(gb.BufferLocation - 1, 0xFE);
                        return true;
                    }
                }
            }
            else
            {
                if (overworld && gb.BufferLocation > 0x59663)
                {
                    System.Windows.Forms.MessageBox.Show("Not all sprites could be saved because it would overwrite other data.");
                    gb.WriteByte(gb.BufferLocation - 1, 0xFF);
                    return true;
                }
                else if (gb.BufferLocation == 0x58CA4)
                {
                    System.Windows.Forms.MessageBox.Show("Not all sprites could be saved because it would overwrite other data.");
                    gb.WriteByte(gb.BufferLocation - 1, 0xFF);
                    return true;
                }
                else if (gb.BufferLocation == 0x59185)
                {
                    System.Windows.Forms.MessageBox.Show("To alter sprites within this room please use a hex editor.");
                    gb.WriteByte(gb.BufferLocation, 0xFF);
                    return true;
                }
                else if (gb.BufferLocation > 0x596FF)
                {
                    System.Windows.Forms.MessageBox.Show("Not all sprites could be saved because it would overwrite other data.");
                    gb.WriteByte(gb.BufferLocation - 1, 0xFF);
                    return true;
                }
            }
            return false;
        }

        public bool saveSprites(List<Object> objects, bool overworld, byte map, byte dungeon, int usedspace, int freespace, int[] pointers, List<Int32> unSortedPointers)
        {
            bool check;
            int cMapPointer;
            int index;
            if (overworld)
                gb.BufferLocation = 0x58000;
            else
            {
                gb.BufferLocation = 0x58200;
                if (dungeon >= 6 && dungeon < 0x1A)
                    gb.BufferLocation = 0x58400;
                else if (dungeon == 0xFF && !overworld)
                    gb.BufferLocation = 0x58600;
            }
            gb.BufferLocation = gb.Get3BytePointerAddress((byte)(gb.BufferLocation / 0x4000), gb.BufferLocation + (map * 2)).Address;
            cMapPointer = gb.BufferLocation;
            foreach (Object obj in objects)
            {
                byte b = (byte)((obj.y * 0x10) + obj.x);
                check = checkWrite(overworld, true);
                if (check != false)
                    return true;
                gb.WriteByte(b);
                check = checkWrite(overworld, true);
                if (check != false)
                    return true;
                gb.WriteByte(obj.id);
            }
            check = checkWrite(overworld, true);
            if (check != false)
                return true;
            if (usedspace < freespace)
            {
                byte b = 0;
                gb.WriteByte(0xFF);
                usedspace += 1;
                while (usedspace != freespace)
                {
                    gb.WriteByte(b);
                    usedspace++;
                }
            }
            else if (usedspace > freespace)
            {
                int i = 1;
                int imaps = 0;
                int c;
                gb.WriteByte(0xFF);
                c = gb.BufferLocation;
                byte[] bytes = gb.Get2BytePointer(gb.BufferLocation);
                index = Array.IndexOf(pointers, cMapPointer);
                int index2 = 0;
                while (pointers[index + 1] == cMapPointer)
                    index++;
                index2 = index2 + index;
                cMapPointer = pointers[index2 + 1];
                while (pointers[index + 1] == cMapPointer)
                {
                    imaps++;
                    index++;
                }
                index = unSortedPointers.IndexOf(cMapPointer);
                if (overworld)
                    gb.WriteBytes((0x58000 + ((byte)index * 2)), bytes);
                else
                {
                    if (dungeon >= 0x6 && dungeon < 0x1A)
                        gb.WriteBytes((0x58400 + (index * 2)), bytes);
                    else if (dungeon == 0xFF)
                        gb.WriteBytes((0x58600 + (index * 2)), bytes);
                    else
                        gb.WriteBytes((0x58200 + (index * 2)), bytes);
                }
                while (unSortedPointers[index + i] < c)
                {
                    c = c - unSortedPointers[index + i];
                    cMapPointer = unSortedPointers[index + i] + c;
                    gb.BufferLocation = cMapPointer;
                    gb.WriteByte(0xFF);
                    c = gb.BufferLocation;
                    bytes = gb.Get2BytePointer(gb.BufferLocation);
                    if (overworld)
                        gb.WriteBytes((0x58000 + ((index + i) * 2)), bytes);
                    else
                    {
                        if (dungeon >= 0x6 && dungeon < 0x1A)
                            gb.WriteBytes((0x58400 + ((index + i) * 2)), bytes);
                        else if (dungeon == 0xFF)
                            gb.WriteBytes((0x58600 + ((index + i) * 2)), bytes);
                        else
                            gb.WriteBytes((0x58200 + ((index + i) * 2)), bytes);
                    }
                    i++;
                }
            }
            return false;
        }

        public void saveDungeonEventInfo(byte dungeon, byte map, byte ID, byte trigger)
        {
            if (dungeon == 0xFF)
                gb.BufferLocation = 0x50200 + map;
            else if (dungeon >= 6 && dungeon < 0x1A)
                gb.BufferLocation = 0x50100 + map;
            else
                gb.BufferLocation = 0x50000 + map;
            gb.WriteByte((byte)((ID << 4) + trigger));
        }

        public void saveStartPos(bool overworld, byte dungeon, byte map, byte xPos, byte yPos)
        {
            gb.BufferLocation = 0x53DE;
            gb.WriteByte(xPos);
            gb.BufferLocation = 0x53E3;
            gb.WriteByte(yPos);
            gb.BufferLocation = 0x53CB;
            gb.WriteByte(map);
            gb.BufferLocation = 0x53D5;
            if (overworld)
            {
                gb.WriteByte(0);
                gb.BufferLocation = 0x53DA;
                gb.WriteByte(0);
            }
            else
            {
                gb.WriteByte(1);
                gb.BufferLocation = 0x53DA;
                gb.WriteByte(dungeon);
            }
        }

        public void savePaletteInfo(Color[,] palette, bool overworld, bool sideview, int dungeon, int map, byte offset)
        {
            if (overworld)
            {
                gb.BufferLocation = 0x842EF + map;
                gb.WriteByte(offset);
                int b = offset * 2;
                gb.BufferLocation = 0x842B1 + b;
                gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
            }
            else
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
                    off = offset;
                    gb.WriteByte(off);
                    gb.BufferLocation = 0x851F6;
                    off &= 0x3F;
                    off <<= 1;
                    gb.BufferLocation += off;
                    gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                    goto l;
                }
                if (dungeon == 0xFF) //0xFF = Color dungeon
                {
                    gb.BufferLocation = 0x867D0;
                    goto l;
                }
                else if (dungeon > 0x09) //Indoor
                {
                    byte b = (byte)((dungeon - 0x0A) << 1);
                    gb.BufferLocation = 0x84413 + b;
                    gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address + map;
                    b = (byte)(gb.ReadByte() << 1);
                    gb.BufferLocation = 0x8443F + b;
                    gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                }
                else
                {
                    gb.BufferLocation = 0x843EF + (dungeon * 2);
                    gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                }

                if (sideview)
                {
                    if (dungeon != 0x07)
                    {
                        gb.BufferLocation = 0x84401 + (dungeon * 2);
                        gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                    }
                    else if (map >= 0x64 && map <= 0x67 || map == 0x6A || map == 0x6B)
                    {
                        gb.BufferLocation = 0x86750;
                    }
                    else
                    {
                        gb.BufferLocation = 0x84401 + (dungeon * 2);
                        gb.BufferLocation = gb.Get2BytePointerAddress(gb.BufferLocation).Address;
                    }
                }
            }

        l:
            {
                int r = 0;
                int g = 0;
                int blu = 0;

                for (int k = 0; k < 8; k++)
                {
                    if (dungeon == 0xFF)
                    {
                        if (k == 7)
                            if (map == 0x1)
                                gb.BufferLocation = 0xDACF0;
                            else if (map == 0x13 || map == 0xF)
                                gb.BufferLocation = 0xDACE0;
                    }
                    for (int i = 0; i < 4; i++)
                    {

                        r = palette[k, i].R / 8;
                        g = palette[k, i].G / 8;
                        blu = palette[k, i].B / 8;
                        blu *= 4;
                        g *= 2;

                        blu *= 256;
                        g *= 16;
                        int value = r + g + blu;
                        gb.WriteByte((byte)(value & 0xFF));
                        gb.WriteByte((byte)(value >> 8));
                    }
                }
            }
        }
    }
}
