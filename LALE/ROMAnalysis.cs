using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GBHL;
using System.IO;

namespace LALE
{
    public class ROMAnalysis
    {
        TileLoader tileLoader;
        DungeonDrawer dungeonDrawer;
        MinimapDrawer minimapDrawer;
        OverworldDrawer overworldDrawer;
        MapSaver mapSaver;
        Sprites sprites;
        Patch patches;
        GBFile gb;

        public void Analyze(string filename)
        {
            if (File.Exists(filename))
            {
                BinaryReader br = new BinaryReader(File.OpenRead(filename));
                byte[] buffer = br.ReadBytes((Int32)br.BaseStream.Length);
                gb = new GBHL.GBFile(buffer);

                tileLoader = new TileLoader(gb);
                dungeonDrawer = new DungeonDrawer(gb);
                minimapDrawer = new MinimapDrawer(gb);
                overworldDrawer = new OverworldDrawer(gb);
                patches = new Patch(gb);
                sprites = new Sprites(gb);
            }
        }
    }
}
