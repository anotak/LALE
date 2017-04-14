using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GBHL;
namespace LALE
{
    class Chest
    {

        GBFile gb;
        byte dungeon;
        byte map;
        bool overworld;
        int location;
        public byte chestData;

        public Chest(GBHL.GBFile g, bool overWorld, byte dung, byte Map)
        {
            gb = g;
            dungeon = dung;
            map = Map;
            overworld = overWorld;
            loadChestData();
        }

        private void loadChestData()
        {
            if (overworld)
            {
                gb.BufferLocation = 0x50560 + map;
                location = gb.BufferLocation;
                chestData = gb.ReadByte();
            }
            else if (dungeon < 6 || dungeon >= 0x1A && dungeon != 0xFF)
            {
                gb.BufferLocation = 0x50660 + map;
                location = gb.BufferLocation;
                chestData = gb.ReadByte();
            }
            else if (dungeon >= 6 && dungeon < 0x1A)
            {
                gb.BufferLocation = 0x50760 + map;
                location = gb.BufferLocation;
                chestData = gb.ReadByte();
            }
            else if (dungeon == 0xFF)
            {
                gb.BufferLocation = 0x50860 + map;
                location = gb.BufferLocation;
                chestData = gb.ReadByte();
            }
        }
    }
}
