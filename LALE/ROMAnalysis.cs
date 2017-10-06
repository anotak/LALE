using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GBHL;
using System.IO;

// quick and dirty analysis of data
namespace LALE
{
    public class ROMAnalysis
    {
        TileLoader tileLoader;
        DungeonDrawer dungeonDrawer;
        MinimapDrawer minimapDrawer;
        OverworldDrawer overworldDrawer;
        Sprites sprites;
        Patch patches;
        GBFile gb;

        public struct Room
        {
            public int dungeonIndex;
            public byte mapIndex;
            public bool bOverworld;
            public byte bank;
        }

        SortedDictionary<int, HashSet<byte>> spritebankinfo;
        SortedDictionary<byte, HashSet<int>> reversespritebankinfo;
        SortedDictionary<byte, HashSet<Room>> spritelocationinfo;

        public void Analyze(string filename)
        {
            if (File.Exists(filename))
            {
                byte[] buffer;
                spritebankinfo = new SortedDictionary<int, HashSet<byte>>();
                reversespritebankinfo = new SortedDictionary<byte, HashSet<int>>();
                spritelocationinfo = new SortedDictionary<byte, HashSet<Room>>();

                using (BinaryReader br = new BinaryReader(File.OpenRead(filename)))
                {
                    buffer = br.ReadBytes((Int32)br.BaseStream.Length);
                }

                gb = new GBHL.GBFile(buffer);

                tileLoader = new TileLoader(gb);
                dungeonDrawer = new DungeonDrawer(gb);
                minimapDrawer = new MinimapDrawer(gb);
                overworldDrawer = new OverworldDrawer(gb);
                patches = new Patch(gb);
                sprites = new Sprites(gb);

                AELogger.Log("BEGIN ANALYSIS");
                for (int room_index = 0; room_index < 0xFF; room_index++)
                {
                    DoOverworld((byte)room_index);
                    DoDungeon(0, (byte)room_index);
                    DoDungeon(0x6, (byte)room_index);
                }
                
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("\n\n================================================\nSprite banks contain these sprites:\n");
                    foreach (KeyValuePair<int, HashSet<byte>> pair in spritebankinfo)
                    {
                        sb.Append("bank ");
                        sb.Append(pair.Key.ToString("X2"));
                        foreach (byte id in pair.Value)
                        {
                            sb.Append("\n\t");
                            sb.Append(Names.GetName(Names.sprites,id));
                            sb.Append(",");
                        }
                        sb.Length = sb.Length - 1;
                        sb.Append("\n");
                    }
                    AELogger.Log(sb);
                }



                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("\n\n================================================\nSprites are used in these rooms:\n");
                    foreach (KeyValuePair<byte, HashSet<Room>> pair in spritelocationinfo)
                    {
                        sb.Append("sprite  '");
                        sb.Append(Names.GetName(Names.sprites, pair.Key));
                        sb.Append("' in rooms:\n");
                        foreach (Room room in pair.Value)
                        {
                            if (room.bOverworld)
                            {
                                sb.Append("\tOVE ---: ");
                            }
                            else if (room.dungeonIndex < 6)
                            {
                                sb.Append("\tDUN 0-5: ");
                            }
                            else
                            {
                                sb.Append("\tDUN 6-?: ");
                            }
                            sb.Append(room.mapIndex.ToString("X2"));
                            sb.Append(" (bank ");
                            sb.Append(room.bank.ToString("X2"));
                            sb.Append("),\n");
                        }
                        sb.Length = sb.Length - 1;
                        sb.Append("\n");
                    }
                    AELogger.Log(sb);
                }

                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("\n\n================================================\nSprites are pickable in these sprite banks:\n");
                    foreach (KeyValuePair<byte, HashSet<int>> pair in reversespritebankinfo)
                    {
                        sb.Append("sprite '");
                        sb.Append(Names.GetName(Names.sprites, pair.Key));
                        sb.Append("':\n\t");
                        foreach (int id in pair.Value)
                        {
                            sb.Append(id.ToString("X2"));
                            sb.Append(", ");
                        }
                        sb.Length = sb.Length - 1;
                        sb.Append("\n");
                    }
                    AELogger.Log(sb);
                }


            } // if file.exists
        } // analyze 
        

        public void DoDungeon(byte dungeonIndex, byte mapIndex)
        {
            dungeonDrawer.getWallsandFloor(dungeonIndex, mapIndex, true);

            sprites.loadObjects(false, dungeonIndex, mapIndex);


            DoSprites(dungeonDrawer.spriteBank, dungeonIndex,mapIndex, false); //(dungeonIndex <= 5 ? 0x100 : 0x200));
        }

        public void DoOverworld(byte mapIndex)
        {
            sprites.loadObjects(true, 0, mapIndex);

            overworldDrawer.getFloor(mapIndex, false);

            DoSprites(overworldDrawer.spriteBank, 0, mapIndex, true);
        }

        public void DoSprites(byte bank, byte dungeonIndex, byte mapIndex, bool bOverworld)
        {
            int niceBank = bank; // niceBank is nice for users to read easier on the output

            if (!bOverworld)
            {
                niceBank += 0x100;
            }

            if (sprites.spriteList.Count > 0)
            {
                if (!spritebankinfo.ContainsKey(niceBank))
                {
                    spritebankinfo.Add(niceBank, new HashSet<byte>());
                }
                Room room = new Room();
                room.bOverworld = bOverworld;
                room.dungeonIndex = dungeonIndex;
                room.mapIndex = mapIndex;
                room.bank = bank;

                foreach(LAObject o in sprites.spriteList)
                {
                    spritebankinfo[niceBank].Add(o.id);

                    if (!reversespritebankinfo.ContainsKey(o.id))
                    {
                        reversespritebankinfo.Add(o.id, new HashSet<int>());
                        spritelocationinfo.Add(o.id, new HashSet<Room>());
                    }
                    reversespritebankinfo[o.id].Add(niceBank);
                    spritelocationinfo[o.id].Add(room);
                }

            } // if count > 0
        } // dosprites

    } // class
} // ns
