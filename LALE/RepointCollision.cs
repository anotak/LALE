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
    public partial class RepointCollision : Form
    {
        public GBHL.GBFile gb;
        bool overWorld;
        int Dungeon;
        int Map;
        public int mapAddress;
        bool special;
        bool magGlass;
        bool sprites;
        bool copyData = false;
        List<Object> mapObjects;
        List<Warps> mapWarps;
        int wall;
        int floor;

        public RepointCollision(byte[] g, bool overworld, int dungeon, int map, int address, bool spec, bool mag, bool sprite, List<Warps> warps, List<Object> objects, int wallvalue, int floorvalue)
        {
            InitializeComponent();
            gb = new GBHL.GBFile(g);
            Dungeon = dungeon;
            overWorld = overworld;
            Map = map;
            mapAddress = address;
            special = spec;
            magGlass = mag;
            sprites = sprite;
            mapWarps = warps;
            mapObjects = objects;
            wall = wallvalue;
            floor = floorvalue;

            nAddress.Value = address;
            if (!sprite)
            {
                if (overWorld)
                {
                    if (map < 0x80)
                    {
                        nAddress.Minimum = 0x24200;
                        nAddress.Maximum = //0x2668B;
                            0x27FFD;
                    }
                    else
                    {
                        nAddress.Minimum = 0x68000;
                        nAddress.Maximum = //0x69E73;
                            0x6BFFD;
                    }
                }
                else
                {
                    if (dungeon < 6 || dungeon >= 0x1A)
                    {
                        nAddress.Maximum = 0x2BB74;
                        nAddress.Minimum = 0x28200;
                    }
                    else if (dungeon >= 6 || dungeon < 0x1A)
                    {
                        nAddress.Maximum = 0x2FFFD;
                        nAddress.Minimum = 0x2C200;
                    }
                    if (dungeon == 0xFF)
                    {
                        nAddress.Maximum = 0x2BFFD;
                        nAddress.Minimum = 0x28200;
                    }
                }
            }
            else
            {
                nAddress.Minimum = 0x58640;
                nAddress.Maximum = 0x5BFFD;
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void bAccept_Click(object sender, EventArgs e)
        {
            byte[] pointer = gb.Get2BytePointer(mapAddress);
            if (!sprites)
            {
                if (overWorld)
                {
                    if (special)
                    {
                        switch (Map)
                        {
                            case 0x06: gb.BufferLocation = 0x31F4; break;
                            case 0x0E: gb.BufferLocation = 0x31C4; break;
                            case 0x1B: gb.BufferLocation = 0x3204; break;
                            case 0x2B: gb.BufferLocation = 0x3214; break;
                            case 0x79: gb.BufferLocation = 0x31E4; break;
                            case 0x8C: gb.BufferLocation = 0x31D4; break;
                        }
                    }
                    else
                        gb.BufferLocation = 0x24000 + (Map * 2);
                }
                else
                {
                    if (magGlass && Map == 0xF5 && Dungeon >= 0x1A || magGlass && Map == 0xF5 && Dungeon < 6)
                        gb.BufferLocation = 0x3198;
                    else
                    {
                        gb.BufferLocation = (0x28000 + (Map * 2));
                        if (Dungeon >= 6 && Dungeon < 0x1A)
                            gb.BufferLocation += 0x4000;
                        else if (Dungeon == 0xFF)
                            gb.BufferLocation = (0x2BB77 + (Map * 2));
                    }
                }
            }
            else
            {
                if (overWorld)
                    gb.BufferLocation = 0x58000;
                else
                {
                    gb.BufferLocation = 0x58200;
                    if (Dungeon >= 6 && Dungeon < 0x1A)
                        gb.BufferLocation = 0x58400;
                    else if (Dungeon == 0xFF)
                        gb.BufferLocation = 0x58600;
                }
                gb.BufferLocation += (Map * 2);
            }

            gb.WriteBytes(pointer);
            if (sprites)
                gb.WriteByte(mapAddress - 1, 0xFF);
            else
                gb.WriteByte(mapAddress - 1, 0xFE);

            if (copyData)
            {
                if (overWorld && !sprites)
                {
                    gb.BufferLocation = mapAddress + 1; //Skip animation
                    byte b = (byte)((wall * 0x10) + floor);
                    gb.WriteByte(b);
                    foreach (Object obj in mapObjects)
                    {
                        if (obj.is3Byte)
                        {
                            if (obj.direction == 8)
                            {
                                b = (byte)(0x80 + obj.length);
                                gb.WriteByte(b);
                            }
                            else
                            {
                                b = (byte)(0xC0 + obj.length);
                                gb.WriteByte(b);
                            }
                            b = (byte)((obj.y * 0x10) + obj.x);
                            gb.WriteByte(b);
                            gb.WriteByte(obj.id);
                        }
                        else
                        {
                            b = (byte)((obj.y * 0x10) + obj.x);
                            gb.WriteByte(b);
                            gb.WriteByte(obj.id);
                        }
                    }
                    if (mapWarps.Count != 0)
                    {
                        foreach (Warps warp in mapWarps)
                        {
                            b = (byte)(0xE0 + warp.type);
                            gb.WriteByte(b);
                            gb.WriteByte(warp.region);
                            gb.WriteByte(warp.map);
                            gb.WriteByte(warp.x);
                            gb.WriteByte(warp.y);
                        }
                    }
                    gb.WriteByte(0xFE);
                }
                else if (!overWorld && !sprites)
                {
                    gb.BufferLocation = mapAddress + 1; //Skip animation
                    byte b = (byte)((wall * 0x10) + floor);
                    gb.WriteByte(b);
                    foreach (Object obj in mapObjects)
                    {
                        if (obj.is3Byte)
                        {
                            if (obj.direction == 8)
                            {
                                b = (byte)(0x80 + obj.length);
                                gb.WriteByte(b);
                            }
                            else
                            {
                                b = (byte)(0xC0 + obj.length);
                                gb.WriteByte(b);
                            }
                            b = (byte)((obj.y * 0x10) + obj.x);
                            gb.WriteByte(b);
                            gb.WriteByte(obj.id);
                        }
                        else
                        {
                            b = (byte)((obj.y * 0x10) + obj.x);
                            gb.WriteByte(b);
                            gb.WriteByte(obj.id);
                        }
                    }
                    if (mapWarps.Count != 0)
                    {
                        foreach (Warps warp in mapWarps)
                        {
                            b = (byte)(0xE0 + warp.type);
                            gb.WriteByte(b);
                            gb.WriteByte(warp.region);
                            gb.WriteByte(warp.map);
                            gb.WriteByte(warp.x);
                            gb.WriteByte(warp.y);
                        }
                    }
                    gb.WriteByte(0xFE);
                }
                else if (sprites)
                {
                    foreach (Object obj in mapObjects)
                    {
                        byte b = (byte)((obj.y * 0x10) + obj.x);
                        gb.WriteByte(b);
                        gb.WriteByte(obj.id);
                    }
                    gb.WriteByte(0xFF);
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void nAddress_ValueChanged(object sender, EventArgs e)
        {
            mapAddress = (int)nAddress.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            copyData = checkBox1.Checked;
        }
    }
}
