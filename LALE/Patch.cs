using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GBHL;

namespace LALE
{
    class Patch
    {
        public GBFile gb;

        public Patch(GBFile g)
        {
            gb = g;
        }

        public void defaultMusic(bool music)
        {
            if (music)
            {
                gb.WriteBytes(0x8156, new byte[] { 0x58, 0x41 });
                gb.WriteByte(0xBB47, 0);
                LALE.Properties.Settings.Default.DefaultMusic = true;
                LALE.Properties.Settings.Default.Save();
            }
            else
            {
                gb.WriteBytes(0x8156, new byte[] { 0xA2, 0x41 });
                gb.WriteByte(0xBB47, 0x41);
                LALE.Properties.Settings.Default.DefaultMusic = false;
                LALE.Properties.Settings.Default.Save();
            }
        }
    }
}
