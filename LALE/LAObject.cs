using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GBHL;

namespace LALE
{
    public class LAObject
    {
        public GBFile gb;
        public bool is3Byte = false;
        public byte id;
        public byte x;
        public byte y;
        public byte h = 1;
        public byte w = 1;
        public byte direction;
        public byte length = 1;
        public bool isDoor1 = false;
        public bool isDoor2 = false;
        public bool isEntrance = false;
        public bool special = false;
        public bool hFlip = false;
        public bool vFlip = false;
        public byte[] tiles;
        public List<LAObject> objectIDs = new List<LAObject>();

        public void getOverworldObjs(LAObject objects)
        {
            LAObject O = new LAObject();
            switch (objects.id)
            {
                case 0xF5:
                    {
                        O.tiles = new byte[] { 0x25, 0x26, 0x27, 0x28 };
                        O.w = 2;
                        O.h = 2;
                        O.special = true;
                        break;
                    }
                case 0xF6:
                    {
                        O.tiles = new byte[] { 85, 90, 90, 90, 86, 87, 89, 89, 89, 88, 91, 226, 91, 226, 91 };
                        O.w = 5;
                        O.h = 3;
                        O.special = true;
                        break;
                    }
                case 0xF7: // House
                    {
                        O.tiles = new byte[] { 85, 90, 86, 87, 89, 88, 91, 226, 91 };
                        O.w = 3;
                        O.h = 3;
                        O.special = true;
                        break;
                    }
                case 0xF8: //Catfish's Maw
                    {
                        O.tiles = new byte[] { 0xB6, 0xB7, 0x66, 0x67, 0xE3, 0x68 };
                        O.w = 3;
                        O.h = 2;
                        O.special = true;
                        break;
                    }
                case 0xF9: //Palace Doors
                    {
                        O.tiles = new byte[] { 0xA4, 0xA5, 0xA6, 0xA7, 0xE3, 0xA8 };
                        O.w = 3;
                        O.h = 2;
                        O.special = true;
                        break;
                    }
                case 0xFA:
                    {
                        O.tiles = new byte[] { 187, 188, 189, 190 };
                        O.w = 2;
                        O.h = 2;
                        O.special = true;
                        break;
                    }
                case 0xFB:
                    {
                        O.tiles = new byte[] { 182, 183, 205, 206 };
                        O.w = 2;
                        O.h = 2;
                        O.special = true;
                        break;
                    }
                case 0xFC:
                    {
                        O.tiles = new byte[] { 43, 44, 45, 55, 232, 56, 51, 224, 52 };
                        O.w = 3;
                        O.h = 3;
                        O.special = true;
                        break;
                    }
                case 0xFD:
                    {
                        O.tiles = new byte[] { 82, 82, 82, 91, 226, 91 };
                        O.w = 3;
                        O.h = 2;
                        O.special = true;
                        break;
                    }
            }
            if (objects.is3Byte)
            {
                O.length = objects.length;
                O.direction = objects.direction;
                O.is3Byte = true;
            }
            if (objects.x == 0xF)
                O.hFlip = true;
            if (objects.y == 0xF)
                O.vFlip = true;
            O.id = objects.id;
            O.x = objects.x;
            O.y = objects.y;
            objectIDs.Add(O);
        }

        public LAObject getOverworldSpecial(LAObject objects)
        {
            LAObject O = new LAObject();
            switch (objects.id)
            {
                case 0xF5:
                    {
                        O.tiles = new byte[] { 0x25, 0x26, 0x27, 0x28 };
                        O.w = 2;
                        O.h = 2;
                        O.special = true;
                        break;
                    }
                case 0xF6:
                    {
                        O.tiles = new byte[] { 85, 90, 90, 90, 86, 87, 89, 89, 89, 88, 91, 226, 91, 226, 91 };
                        O.w = 5;
                        O.h = 3;
                        O.special = true;
                        break;
                    }
                case 0xF7: // House
                    {
                        O.tiles = new byte[] { 85, 90, 86, 87, 89, 88, 91, 226, 91 };
                        O.w = 3;
                        O.h = 3;
                        O.special = true;
                        break;
                    }
                case 0xF8: //Catfish's Maw
                    {
                        O.tiles = new byte[] { 0xB6, 0xB7, 0x66, 0x67, 0xE3, 0x68 };
                        O.w = 3;
                        O.h = 2;
                        O.special = true;
                        break;
                    }
                case 0xF9: //Palace Doors
                    {
                        O.tiles = new byte[] { 0xA4, 0xA5, 0xA6, 0xA7, 0xE3, 0xA8 };
                        O.w = 3;
                        O.h = 2;
                        O.special = true;
                        break;
                    }
                case 0xFA:
                    {
                        O.tiles = new byte[] { 187, 188, 189, 190 };
                        O.w = 2;
                        O.h = 2;
                        O.special = true;
                        break;
                    }
                case 0xFB:
                    {
                        O.tiles = new byte[] { 182, 183, 205, 206 };
                        O.w = 2;
                        O.h = 2;
                        O.special = true;
                        break;
                    }
                case 0xFC:
                    {
                        O.tiles = new byte[] { 43, 44, 45, 55, 232, 56, 51, 224, 52 };
                        O.w = 3;
                        O.h = 3;
                        O.special = true;
                        break;
                    }
                case 0xFD:
                    {
                        O.tiles = new byte[] { 82, 82, 82, 91, 226, 91 };
                        O.w = 3;
                        O.h = 2;
                        O.special = true;
                        break;
                    }
            }
            O.id = objects.id;
            O.x = objects.x;
            O.y = objects.y;
            if (objects.x == 0xF)
                O.hFlip = true;
            if (objects.y == 0xF)
                O.vFlip = true;
            if (objects.is3Byte)
            {
                O.length = objects.length;
                O.direction = objects.direction;
                O.is3Byte = true;
                if (!O.special)
                {
                    if (O.hFlip && O.vFlip)
                    {
                        O.y = 0;
                        O.vFlip = false;
                        O.x = 0;
                        O.hFlip = false;
                    }
                }
            }
            return O;
        }

        public LAObject dungeonDoors(LAObject ob)
        {
            int door = ob.id;
            int y1 = ob.y;
            int x1 = ob.x;
            byte[] tiles;
            switch (door)
            {
                case 0xEC: gb.BufferLocation = 0x35EF; break; // Key Doors
                case 0xED: gb.BufferLocation = 0x360A; break;
                case 0xEE: gb.BufferLocation = 0x3625; break;
                case 0xEF: gb.BufferLocation = 0x3640; break;

                case 0xF4: gb.BufferLocation = 0x36A7; break;  // Open Doors      
                case 0xF5: gb.BufferLocation = 0x36DF; break;
                case 0xF6: gb.BufferLocation = 0x36F3; break;
                case 0xF7: gb.BufferLocation = 0x3707; break;

                case 0xF0: gb.BufferLocation = 0x36A7; break;  // Closed Doors      
                case 0xF1: gb.BufferLocation = 0x36DF; break;
                case 0xF2: gb.BufferLocation = 0x36F3; break;
                case 0xF3: gb.BufferLocation = 0x3707; break;

                case 0xF8: gb.BufferLocation = 0x371B; break; // Boss Door

                case 0xF9: gb.BufferLocation = 0x3753; break; // ?? Stairs maybe
                case 0xFA: gb.BufferLocation = 0x3762; break; // FLip Wall
                case 0xFB: gb.BufferLocation = 0x3771; break; // One-way Arrow

                case 0xFC: gb.BufferLocation = 0x378D; break; // Dungeon Entrances

                case 0xFD: gb.BufferLocation = 0x37AB; break; //Indoor Entrances
            }
            if (door == 0xEC || door == 0xED || door == 0xF0 || door == 0xF1 || door == 0xF4 || door == 0xF5 || door == 0xF8 || door == 0xFA || door == 0xFB || door == 0xFD)
            {
                tiles = new byte[2];
                ob.isDoor1 = true;
                ob.isDoor2 = false;
                ob.isEntrance = false;
                ob.w = 2;
                ob.h = 1;
                ob.tiles = tiles;
                ob.y = (byte)y1;
                ob.x = (byte)x1;
                for (int i = 0; i < 2; i++)
                    tiles[i] = gb.ReadByte();
                ob.id = (byte)door;
                //objects.Add(ob);
            }
            else if (door == 0xEE || door == 0xEF || door == 0xF2 || door == 0xF3 || door == 0xF6 || door == 0xF7)
            {
                tiles = new byte[2];
                ob.isDoor2 = true;
                ob.isEntrance = false;
                ob.isDoor1 = false;
                ob.w = 1;
                ob.h = 2;
                ob.tiles = tiles;
                ob.x = (byte)x1;
                ob.y = (byte)y1;
                for (int i = 0; i < 2; i++)
                    tiles[i] = gb.ReadByte();
                ob.id = (byte)door;
                //objects.Add(ob);
            }
            else if (door == 0xFC)//Dungeon entrance
            {
                tiles = new byte[13];
                ob.tiles = tiles;
                ob.isEntrance = true;
                ob.isDoor1 = false;
                ob.isDoor2 = false;
                ob.w = 4;
                ob.h = 3;
                for (int i = 0; i < 13; i++)
                    tiles[i] = gb.ReadByte();
                ob.id = (byte)door;
                //objects.Add(ob);
            }
            return ob;
        }
    }
}
