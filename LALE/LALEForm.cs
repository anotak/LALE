using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace LALE
{
    public partial class LALEForm : Form
    {

        GBHL.GBFile gb;
        string filename = "";
        string exportToFilename = "";
        Bitmap tileset;
        Bitmap[] bigMaps;
        TileLoader tileLoader;
        DungeonDrawer dungeonDrawer;
        MinimapDrawer minimapDrawer;
        OverworldDrawer overworldDrawer;
        MapSaver mapSaver;
        Sprites sprites;
        Patch patches;
        ExportMap ExportMaps;
        public List<Warps> WL = new List<Warps>();
        Object selectedObject = new Object();
        Point lastMapHoverPoint = new Point(-1, -1);
        Color[,] palette = new Color[8, 4];
        Color[,] paletteCopy;
        int objectX;
        int objectY;
        byte[] mapData;
        byte[] mapDataCopy;
        byte selectedTile = 0;
        byte mapIndex = 0;
        byte dungeonIndex = 0;
        byte animations;
        byte SOG;
        byte floorTile;
        byte wallTiles;
        byte spriteBank;
        byte music;
        byte chestData;
        int freespace;
        int usedspace;
        bool overWorld = true;
        bool overLay = true;
        bool sideView = false;
        int exportGroupIndex;

        byte palOffset;
        byte startXPos;
        byte startYPos;
        byte SEDung;
        byte SEMap;
        bool SEoverworld;

        public LALEForm()
        {
            InitializeComponent();
        }

        private void openROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select a ROM";
            ofd.Filter = "Gameboy ROM|*.gbc;*.gb";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filename = ofd.FileName;
                BinaryReader br = new BinaryReader(File.OpenRead(filename));
                byte[] buffer = br.ReadBytes((Int32)br.BaseStream.Length);

                gb = new GBHL.GBFile(buffer);
                //toolStripStatusLabel1.Text = ofd.SafeFileName;
                //MessageBox.Show("ROM sucessfully loaded.");

                tileLoader = new TileLoader(gb);
                dungeonDrawer = new DungeonDrawer(gb);
                minimapDrawer = new MinimapDrawer(gb);
                overworldDrawer = new OverworldDrawer(gb);
                mapSaver = new MapSaver(gb);
                patches = new Patch(gb);
                sprites = new Sprites(gb);

                //if (comboBox1.SelectedIndex != 0)
                // comboBox1.SelectedIndex = 0;
                if (tabControl1.SelectedIndex != 0)
                    tabControl1.SelectedIndex = 0;
                nMap.Enabled = true;
                comboBox1.Enabled = true;
                cSpecialTiles.Enabled = true;
                rOverlay.Enabled = true;
                rCollision.Enabled = true;
                cSideview.Enabled = true;
                cSideview2.Enabled = true;
                nRegion.Enabled = true;
                nAnimations.Enabled = true;
                nSOG.Enabled = true;
                nFloor.Enabled = true;
                nWall.Enabled = true;
                nMusic.Enabled = true;
                nSpriteBank.Enabled = true;
                nSelected.Enabled = true;
                comDirection.Enabled = true;
                nLength.Enabled = true;
                nObjectID.Enabled = true;
                cObjectList.Enabled = true;
                cSprite.Enabled = true;
                defaultMusicToolStripMenuItem.Enabled = true;
                defaultMusicToolStripMenuItem.Checked = LALE.Properties.Settings.Default.DefaultMusic;
                pMinimap.SelectedIndex = 0;
                LoadSandA();
                setSpriteData();
                loadTileset();
                //overworldDrawer.getCollisionDataOverworld(0, false);                
                WL = new List<Warps>();
                WL = overworldDrawer.warps;
                drawMinimap();
                bigMaps = new Bitmap[tabControl1.TabCount];

                gb.WriteByte(0x2FFFF, 0xFE); //Ends collision data for indoor map FF

                br.Close();
            }
        }

        public void LoadSandA()
        {
            tileLoader.getAnimations(mapIndex, dungeonIndex, overWorld, cSpecialTiles.Checked);
            animations = tileLoader.Animations;
            tileLoader.getSOG(mapIndex, overWorld);
            SOG = tileLoader.SOG;
            Chest chest = new Chest(gb, overWorld, dungeonIndex, mapIndex);
            chestData = chest.chestData;
            if (overWorld)
            {
                overworldDrawer.getFloor(mapIndex, cSpecialTiles.Checked);
                floorTile = overworldDrawer.floor;
                wallTiles = overworldDrawer.wall;
                spriteBank = overworldDrawer.spriteBank;
                overworldDrawer.getMusic(mapIndex);
                music = overworldDrawer.music;
                nMusic.Value = music;
            }
            else
            {
                cSpecialTiles.Checked = false;
                dungeonDrawer.getMusic(dungeonIndex, mapIndex);
                music = dungeonDrawer.music;
                nMusic.Value = music;
                dungeonDrawer.getWallsandFloor(dungeonIndex, mapIndex, cMagGlass.Checked);
                wallTiles = dungeonDrawer.wall;
                floorTile = dungeonDrawer.floor;
                spriteBank = dungeonDrawer.spriteBank;
                nWall.Value = wallTiles;
                nSpriteBank.Value = spriteBank;
                nFloor.Value = floorTile;
            }
        }

        public void loadTileset()
        {
            if (animations != tileLoader.Animations)
                tileLoader.Animations = animations;
            if (SOG != tileLoader.SOG)
                tileLoader.SOG = SOG;
            if (overWorld)
            {
                if (floorTile != overworldDrawer.floor)
                    overworldDrawer.floor = floorTile;
                if (wallTiles != overworldDrawer.wall)
                    overworldDrawer.wall = wallTiles;
                if (spriteBank != overworldDrawer.spriteBank)
                    overworldDrawer.spriteBank = spriteBank;

                nFloor.Value = (byte)overworldDrawer.floor;
                nWall.Value = (byte)overworldDrawer.wall;
                nSpriteBank.Value = (byte)overworldDrawer.spriteBank;
            }
            else
                cSpecialTiles.Checked = false;
            byte[, ,] data = tileLoader.loadTileset(dungeonIndex, mapIndex, overWorld, rCrystals.Checked, sideView);
            tileLoader.loadPallete(dungeonIndex, mapIndex, overWorld, sideView);
            palette = tileLoader.palette;
            palOffset = tileLoader.palOffset;
            TileLoader.Tile[] tilez = tileLoader.loadPaletteFlipIndexes(mapIndex, dungeonIndex);
            pTiles.Image = tileLoader.drawTileset(data, tilez);
            tileset = tileLoader.drawTileset(data, tilez);

            nAnimations.Value = tileLoader.Animations;
            animations = tileLoader.Animations;
            nSOG.Value = tileLoader.SOG;
        }

        private void setCollisionData()
        {
            comDirection.Enabled = false;
            nLength.Enabled = false;
            nSelected.Value = -1;
            comDirection.SelectedIndex = 0;
            nObjectID.Value = 0;
            nLength.Value = 1;
        }

        private void setSpriteData()
        {
            if (!cSprite.Checked)
            {
                nSpriteSelected.Enabled = false;
                nSpriteID.Enabled = false;
            }
            sprites.loadObjects(overWorld, dungeonIndex, mapIndex);
            //sprites.loadSpriteBanks(overWorld, dungeonIndex, mapIndex);
            nSpriteSelected.Maximum = sprites.spriteList.Count - 1;
            nSpriteSelected.Value = -1;
            nSpriteID.Value = 0;
        }

        private void drawSprites()
        {
            Bitmap b = sprites.DrawSprites((Bitmap)pMap.Image);
            pMap.Image = (Image)b;
        }

        private void drawDungeon()
        {
            cSpecialTiles.Checked = false;
            if (cSprite.Checked)
                gBoxCollisions.Enabled = false;
            else
                gBoxCollisions.Enabled = true;
            if (wallTiles != dungeonDrawer.wall)
                dungeonDrawer.wall = wallTiles;
            if (floorTile != dungeonDrawer.floor)
                dungeonDrawer.floor = floorTile;
            if (spriteBank != dungeonDrawer.spriteBank)
                dungeonDrawer.spriteBank = spriteBank;
            dungeonDrawer.loadWalls();
            dungeonDrawer.loadCollisionsDungeon();
            mapData = dungeonDrawer.data;
            pMap.Image = dungeonDrawer.DrawCollisions(tileset, mapData, collisionBordersToolStripMenuItem.Checked);
            if (cSprite.Checked)
                drawSprites();
            if (!cSprite.Checked)
                toolStripStatusLabel1.Text = "Map: 0x" + dungeonDrawer.mapAddress.ToString("X");
            else
                toolStripStatusLabel1.Text = "Objects: 0x" + sprites.objectAddress.ToString("X");
            toolStripStatusLabel2.Text = "Used/Free Space: " + usedspace.ToString() + "/" + freespace.ToString();
            nFloor.Value = dungeonDrawer.floor;
            nWall.Value = wallTiles;
            nSpriteBank.Value = spriteBank;
            nSelected.Maximum = dungeonDrawer.objects.Count - 1;
        }

        public void drawOverworld()
        {
            if (overLay)
            {
                gBoxCollisions.Enabled = false;
                pObject.Invalidate();
                mapData = overworldDrawer.ReadMap(mapIndex, cSpecialTiles.Checked);
                pMap.Image = overworldDrawer.drawMap(tileset, mapData, collisionBordersToolStripMenuItem.Checked, selectedObject);
                if (cSprite.Checked)
                    drawSprites();
                toolStripStatusLabel2.Text = "";
                if (!cSprite.Checked)
                    toolStripStatusLabel1.Text = "Map: 0x" + overworldDrawer.mapAddress.ToString("X");
                else
                {
                    toolStripStatusLabel1.Text = "Objects: 0x" + sprites.objectAddress.ToString("X");
                    toolStripStatusLabel2.Text = "Used/Free Space: " + usedspace.ToString() + "/" + freespace.ToString();
                }
                CollisionList.Items.Clear();
            }
            else if (!overLay)
            {
                if (cSprite.Checked)
                    gBoxCollisions.Enabled = false;
                else
                    gBoxCollisions.Enabled = true;
                pObject.Invalidate();
                overworldDrawer.loadCollisionsOverworld();
                mapData = overworldDrawer.mapData;
                pMap.Image = overworldDrawer.drawMap(tileset, mapData, collisionBordersToolStripMenuItem.Checked, selectedObject);
                if (cSprite.Checked)
                    drawSprites();
                if (!cSprite.Checked)
                    toolStripStatusLabel1.Text = "Collision: 0x" + overworldDrawer.mapAddress.ToString("X");
                else
                    toolStripStatusLabel1.Text = "Objects: 0x" + sprites.objectAddress.ToString("X");
                toolStripStatusLabel2.Text = "Used/Free Space: " + usedspace.ToString() + "/" + freespace.ToString();
                nSelected.Maximum = overworldDrawer.objects.Count - 1;
            }
        }

        public void drawMinimap()
        {
            if (!overWorld)
            {
                minimapDrawer.loadMinimapDData(dungeonIndex);
                byte[, ,] data = minimapDrawer.loadMinimapDungeon();
                pMinimapD.Image = minimapDrawer.drawDungeonTiles(data);
            }
            else if (overWorld)
            {
                byte[, ,] data = minimapDrawer.loadMinimapOverworld();
                pMinimap.Image = minimapDrawer.drawOverworldTiles(data);
            }
        }

        private void nMap_ValueChanged(object sender, EventArgs e)
        {
            mapIndex = (byte)nMap.Value;
            LoadSandA();
            loadTileset();
            if ((tabControl1.SelectedIndex != 0 && mapIndex == 0xF5 && dungeonIndex >= 0x1A) || tabControl1.SelectedIndex != 0 && mapIndex == 0xF5 && dungeonIndex < 6)
            {
                cMagGlass.Visible = true;
                cMagGlass1.Visible = true;
            }
            else
            {
                cMagGlass.Visible = false;
                cMagGlass1.Visible = false;
            }
            if (overWorld)
            {
                overworldDrawer.getCollisionDataOverworld(mapIndex, cSpecialTiles.Checked);
                if (!overLay)
                {
                    setCollisionData();
                    collisionListView();
                }
                pMinimap.SelectedIndex = (int)nMap.Value;
                if (!cSprite.Checked)
                {
                    freespace = overworldDrawer.getFreeSpace(mapIndex, cSpecialTiles.Checked);
                    usedspace = overworldDrawer.getUsedSpace();
                }
                else
                {
                    setSpriteData();
                    freespace = sprites.getFreeSpace(overWorld, mapIndex, dungeonIndex);
                    usedspace = sprites.getUsedSpace();
                }
                setSpriteData();
                drawOverworld();
                WL = new List<Warps>();
                WL = overworldDrawer.warps;
            }
            else
            {
                cSpecialTiles.Checked = false;
                dungeonDrawer.getCollisionDataDungeon(mapIndex, dungeonIndex, cMagGlass.Checked);
                dungeonDrawer.getEventData(mapIndex, dungeonIndex);
                nEventID.Value = dungeonDrawer.eventID;
                nEventTrigger.Value = dungeonDrawer.eventTrigger;
                label18.Text = "Location: 0x" + dungeonDrawer.eventDataLocation.ToString("X");
                setCollisionData();
                setSpriteData();
                WL = new List<Warps>();
                WL = dungeonDrawer.warps;
                if (!cSprite.Checked)
                {
                    freespace = dungeonDrawer.getFreeSpace(mapIndex, dungeonIndex, cMagGlass.Checked);
                    usedspace = dungeonDrawer.getUsedSpace();
                }
                else
                {
                    freespace = sprites.getFreeSpace(overWorld, mapIndex, dungeonIndex);
                    usedspace = sprites.getUsedSpace();
                }
                drawDungeon();
            }
        }

        private void pTiles_MouseClick(object sender, MouseEventArgs e)
        {
            selectTile(pTiles.SelectedIndex);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 8)
            {
                dungeonIndex = 0xFF;
                nMap.Maximum = 0x15;
            }
            else
            {
                dungeonIndex = (byte)(comboBox1.SelectedIndex);
                nMap.Maximum = 0xFF;
            }
            nMap.Enabled = true;
            pMinimapD.SelectedIndex = 0;
            minimapDrawer.loadMinimapDData(dungeonIndex);
            mapIndex = minimapDrawer.roomIndexes[0];
            nMap.Value = mapIndex;
            drawMinimap();
            LoadSandA();
            loadTileset();
            dungeonDrawer.getCollisionDataDungeon(mapIndex, dungeonIndex, cMagGlass.Checked);
            dungeonDrawer.getEventData(mapIndex, dungeonIndex);
            nEventID.Value = dungeonDrawer.eventID;
            nEventTrigger.Value = dungeonDrawer.eventTrigger;
            label18.Text = "Location: 0x" + dungeonDrawer.eventDataLocation.ToString("X");
            WL = new List<Warps>();
            WL = dungeonDrawer.warps;
            setCollisionData();
            if (!cSprite.Checked)
            {
                freespace = dungeonDrawer.getFreeSpace(mapIndex, dungeonIndex, cMagGlass.Checked);
                usedspace = dungeonDrawer.getUsedSpace();
            }
            else
            {
                freespace = sprites.getFreeSpace(overWorld, mapIndex, dungeonIndex);
                usedspace = sprites.getUsedSpace();
            }
            collisionListView();
            drawDungeon();
            setSpriteData();
        }

        private void selectTile(int tile)
        {
            selectedTile = (byte)tile;
            if (pTiles.SelectedIndex != tile || (pTiles.SelectionRectangle.Width != 1 || pTiles.SelectionRectangle.Height != 1))
                pTiles.SelectedIndex = selectedTile;
            label4.Text = "Selected tile: " + pTiles.SelectedIndex.ToString("X");
            pTiles.Invalidate();
        }

        private void pMinimap_MouseClick(object sender, MouseEventArgs e)
        {
            if (pTiles.Image != null)
            {
                if (overWorld)
                {
                    mapIndex = (byte)pMinimap.SelectedIndex;
                    nMap.Value = mapIndex;
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pTiles.Image != null)
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    exportGroupIndex = tabControl1.SelectedIndex;
                    nMap.Minimum = 0x0;
                    nMap.Maximum = 0xFF;
                    mapIndex = 0;
                    label7.Text = "Floor row:";
                    nWall.Maximum = 0xF;
                    gEventData.Visible = false;
                    cSideview.Checked = false;
                    cSideview2.Checked = false;
                    cSprite.Checked = false;
                    overLay = true;
                    rOverlay.Enabled = true;
                    rCollision.Enabled = true;
                    nMap.Enabled = true;
                    rCrystals.Enabled = false;
                    rCrystals.Checked = false;
                    nMap.Value = mapIndex;
                    pMinimap.SelectedIndex = mapIndex;
                    overWorld = true;
                    LoadSandA();
                    setCollisionData();
                    WL = new List<Warps>();
                    WL = overworldDrawer.warps;
                    setSpriteData();
                    loadTileset();
                    drawMinimap();
                    rOverlay.Checked = true;
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    exportGroupIndex = tabControl1.SelectedIndex;
                    cSpecialTiles.Checked = false;
                    nMap.Minimum = 0x0;
                    nMap.Maximum = 0xFF;
                    dungeonIndex = 0;
                    comboBox1.SelectedIndex = 0;
                    label7.Text = "Wall tiles:";
                    nWall.Maximum = 0x9;
                    gEventData.Visible = true;
                    cSpecialTiles.Checked = false;
                    cSprite.Checked = false;
                    cSideview2.Checked = false;
                    rCrystals.Enabled = true;
                    rOverlay.Enabled = false;
                    rCollision.Enabled = false;
                    rCollision.Checked = false;
                    rOverlay.Checked = false;
                    overLay = false;
                    nMap.Enabled = true;
                    overWorld = false;
                    minimapDrawer.loadMinimapDData(dungeonIndex);
                    mapIndex = minimapDrawer.roomIndexes[0];
                    nMap.Value = mapIndex;
                    pMinimapD.SelectedIndex = mapIndex;
                    overWorld = false;
                    LoadSandA();
                    loadTileset();
                    dungeonDrawer.getCollisionDataDungeon(mapIndex, dungeonIndex, cMagGlass.Checked);
                    dungeonDrawer.getEventData(mapIndex, dungeonIndex);
                    nEventID.Value = dungeonDrawer.eventID;
                    nEventTrigger.Value = dungeonDrawer.eventTrigger;
                    label18.Text = "Location: 0x" + dungeonDrawer.eventDataLocation.ToString("X");
                    WL = new List<Warps>();
                    WL = dungeonDrawer.warps;
                    setCollisionData();
                    if (!cSprite.Checked)
                    {
                        freespace = dungeonDrawer.getFreeSpace(mapIndex, dungeonIndex, cMagGlass.Checked);
                        usedspace = dungeonDrawer.getUsedSpace();
                    }
                    else
                    {
                        freespace = sprites.getFreeSpace(overWorld, mapIndex, dungeonIndex);
                        usedspace = sprites.getUsedSpace();
                    }
                    collisionListView();
                    setSpriteData();
                    drawDungeon();
                    drawMinimap();
                }
                else
                {
                    exportGroupIndex = tabControl1.SelectedIndex;
                    cSpecialTiles.Checked = false;
                    nMap.Maximum = 0xFF;
                    dungeonIndex = (byte)nRegion.Value;
                    cSpecialTiles.Checked = false;
                    label7.Text = "Wall tiles:";
                    nWall.Maximum = 0x9;
                    gEventData.Visible = true;
                    cSideview.Checked = false;
                    rCrystals.Enabled = true;
                    overLay = false;
                    overWorld = false;
                    cSprite.Checked = false;
                    rOverlay.Enabled = false;
                    rCollision.Enabled = false;
                    rCollision.Checked = false;
                    rOverlay.Checked = false;
                    nMap.Enabled = true;
                    mapIndex = 0x6C;
                    nMap.Value = mapIndex;
                    LoadSandA();
                    dungeonDrawer.getCollisionDataDungeon(mapIndex, dungeonIndex, cMagGlass.Checked);
                    dungeonDrawer.getEventData(mapIndex, dungeonIndex);
                    nEventID.Value = dungeonDrawer.eventID;
                    nEventTrigger.Value = dungeonDrawer.eventTrigger;
                    label18.Text = "Location: 0x" + dungeonDrawer.eventDataLocation.ToString("X");
                    WL = new List<Warps>();
                    WL = dungeonDrawer.warps;
                    setCollisionData();
                    if (!cSprite.Checked)
                    {
                        freespace = dungeonDrawer.getFreeSpace(mapIndex, dungeonIndex, cMagGlass.Checked);
                        usedspace = dungeonDrawer.getUsedSpace();
                    }
                    else
                    {
                        freespace = sprites.getFreeSpace(overWorld, mapIndex, dungeonIndex);
                        usedspace = sprites.getUsedSpace();
                    }
                    collisionListView();
                    setSpriteData();
                    loadTileset();
                    drawDungeon();
                }
            }
        }

        public int getDandOObjectID(int x, int y)
        {
            x /= 16;
            y /= 16;
            if (!overWorld && !cSprite.Checked)
            {
                for (int i = dungeonDrawer.objects.Count - 1; i > -1; i--)
                {
                    if (dungeonDrawer.objects[i].x == x && dungeonDrawer.objects[i].y == y)
                        return i;
                    else
                    {
                        Object o = dungeonDrawer.objects[i];
                        if (dungeonDrawer.objects[i].is3Byte)
                        {
                            if (o.direction == 8)
                            {
                                if (x >= o.x && x < o.length + o.x && y == o.y)
                                    return i;
                            }
                            else
                            {
                                if (x == o.x && y >= o.y && y < o.y + o.length)
                                    return i;
                            }
                        }
                        if (dungeonDrawer.objects[i].isDoor1)
                        {
                            if (x >= o.x && x < o.w + o.x && y == o.y)
                                return i;
                        }
                        if (dungeonDrawer.objects[i].isDoor2)
                        {
                            if (x == o.x && y >= o.y && y < o.y + o.h)
                                return i;
                        }
                        if (dungeonDrawer.objects[i].isEntrance)
                        {
                            if (x >= o.x && y >= o.y && y < o.y + o.h && x < o.x + o.w)
                                return i;
                        }
                    }
                }
            }
            else if (overWorld && !cSprite.Checked)
            {
                for (int i = overworldDrawer.objects.Count - 1; i > -1; i--)
                {
                    if (overworldDrawer.objects[i].x == x && overworldDrawer.objects[i].y == y)
                        return i;
                    else
                    {
                        Object o = overworldDrawer.objects[i];
                        int dx = (o.x == 0xF ? (o.x - 16) : o.x);
                        int dy = (o.y == 0xF ? (o.y - 16) : o.y);
                        if (o.is3Byte)
                        {
                            if (o.direction == 8)
                            {
                                if (o.special)
                                {
                                    if (x >= dx && y >= dy && y < dy + o.h && x < dx + (o.w * o.length))
                                        return i;
                                }
                                if (x >= dx && x < o.length + dx && y == dy)
                                    return i;
                            }
                            else
                            {
                                if (o.special)
                                {
                                    if (x >= dx && y >= dy && y < dy + (o.h * o.length) && x < dx + o.w)
                                        return i;
                                }
                                if (x == dx && y >= dy && y < dy + o.length)
                                    return i;
                            }
                        }
                        else if (o.special)
                        {
                            if (x >= dx && y >= dy && y < dy + o.h && x < dx + o.w)
                                return i;
                        }
                    }
                }
            }
            else if (cSprite.Checked)
            {
                for (int i = sprites.spriteList.Count - 1; i > -1; i--)
                {
                    if (sprites.spriteList[i].x == x && sprites.spriteList[i].y == y)
                        return i;
                }
            }
            return -1;
        }

        private void pMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (pTiles.Image != null)
            {
                int ind = getDandOObjectID(e.X, e.Y);
                if (overWorld && overLay && !cSprite.Checked)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        //if (lastMapHoverPoint.X == e.X / 16 && lastMapHoverPoint.Y == e.Y / 16)
                        //    return;
                        Graphics g = Graphics.FromImage(pMap.Image);
                        if (pTiles.SelectionRectangle.Width == 1 && pTiles.SelectionRectangle.Height == 1)
                        {
                            g.DrawImage(pTiles.Image, new Rectangle(e.X / 16 * 16, e.Y / 16 * 16, 16, 16), (selectedTile % 16) * 16, (selectedTile / 16) * 16, 16, 16, GraphicsUnit.Pixel);
                            mapData[e.X / 16 + (e.Y / 16) * 10] = selectedTile;
                        }
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        selectTile(mapData[e.X / 16 + (e.Y / 16) * 10]);
                    }
                }
                else if (overWorld && !overLay && !cSprite.Checked)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (ind > -1)
                        {
                            selectCollision(overworldDrawer.objects[ind], ind);
                            objectX = overworldDrawer.objects[ind].x;
                            objectY = overworldDrawer.objects[ind].y;
                            selectedObject = overworldDrawer.objects[ind];
                        }
                        else
                            selectCollision(null, ind);
                        if (ind != -1)
                            overworldDrawer.drawSelectedObject((Bitmap)pMap.Image, selectedObject);
                    }
                }
                else if (!overWorld && !cSprite.Checked)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (ind > -1)
                        {
                            selectCollision(dungeonDrawer.objects[ind], ind);
                            objectX = dungeonDrawer.objects[ind].x;
                            objectY = dungeonDrawer.objects[ind].y;
                            selectedObject = dungeonDrawer.objects[ind];
                        }
                        else
                            selectCollision(null, ind);
                        if (ind != -1)
                            dungeonDrawer.drawSelectedObject((Bitmap)pMap.Image, selectedObject);
                    }
                }
                else if (cSprite.Checked)
                {
                    if (ind > -1)
                    {
                        selectCollision(sprites.spriteList[ind], ind);
                        objectX = sprites.spriteList[ind].x;
                        objectY = sprites.spriteList[ind].y;
                        selectedObject = sprites.spriteList[ind];
                    }
                    else
                        selectCollision(null, ind);
                    if (ind != -1)
                        sprites.drawSelectedSprite(pMap.Image, selectedObject);
                }
                lastMapHoverPoint = new Point(e.X / 16, e.Y / 16);
                pMap.Invalidate();

            }
        }

        private void pMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (pMap.Image == null)
                return;
            lblHoverPos.Text = "X: " + (e.X / 16).ToString("X") + " Y: " + (e.Y / 16).ToString("X");
            if (overWorld && overLay && !cSprite.Checked)
            {
                if (e.Button == MouseButtons.Left)
                {
                    //if (lastMapHoverPoint.X == e.X / 16 && lastMapHoverPoint.Y == e.Y / 16)
                    //    return;
                    Graphics g = Graphics.FromImage(pMap.Image);
                    if (pTiles.SelectionRectangle.Width == 1 && pTiles.SelectionRectangle.Height == 1)
                    {
                        if (e.X / 16 > 9 || e.Y / 16 > 7 || e.X / 16 < 0 || e.Y / 16 < 0)
                            return;
                        g.DrawImage(pTiles.Image, new Rectangle(e.X / 16 * 16, e.Y / 16 * 16, 16, 16), (selectedTile % 16) * 16, (selectedTile / 16) * 16, 16, 16, GraphicsUnit.Pixel);
                        mapData[e.X / 16 + (e.Y / 16) * 10] = selectedTile;
                    }
                }
            }
            else if (overWorld && !overLay && !cSprite.Checked)
            {
                int ind = (int)nSelected.Value;
                int x = e.X / 16;
                int y = e.Y / 16;
                if (e.Button == MouseButtons.Left)
                {
                    if (ind > -1)
                    {
                        if (objectX == 0xF)
                            objectX = -1;
                        if (objectY == 0xF)
                            objectY = -1;
                        overworldDrawer.objects[ind].x = (byte)(objectX + (x - lastMapHoverPoint.X));
                        overworldDrawer.objects[ind].y = (byte)(objectY + (y - lastMapHoverPoint.Y));
                        if (objectX + (x - lastMapHoverPoint.X) < 0 || objectX + (x - lastMapHoverPoint.X) > 9)
                        {
                            if (!overworldDrawer.objects[ind].is3Byte && !overworldDrawer.objects[ind].special || !overworldDrawer.objects[ind].special)
                            {
                                overworldDrawer.objects[ind].x = 0;
                                overworldDrawer.objects[ind].hFlip = false;
                            }
                            else
                            {
                                overworldDrawer.objects[ind].x = 0xF;
                                overworldDrawer.objects[ind].hFlip = true;
                            }
                        }
                        else
                            overworldDrawer.objects[ind].hFlip = false;
                        if (objectY + (y - lastMapHoverPoint.Y) < 0 || objectY + (y - lastMapHoverPoint.Y) > 7)
                        {
                            if (!overworldDrawer.objects[ind].is3Byte && !overworldDrawer.objects[ind].special || !overworldDrawer.objects[ind].special)
                            {
                                overworldDrawer.objects[ind].y = 0;
                                overworldDrawer.objects[ind].vFlip = false;
                            }
                            else
                            {
                                overworldDrawer.objects[ind].y = 0x0F;
                                overworldDrawer.objects[ind].vFlip = true;
                            }
                        }
                        else
                            overworldDrawer.objects[ind].vFlip = false;
                        drawOverworld();
                    }
                    if (ind != -1)
                        overworldDrawer.drawSelectedObject((Bitmap)pMap.Image, selectedObject);
                }
            }
            else if (!overWorld && !cSprite.Checked)
            {
                int ind = (int)nSelected.Value;
                int x = e.X / 16;
                int y = e.Y / 16;
                if (e.Button == MouseButtons.Left)
                {
                    if (ind > -1)
                    {
                        if (objectX == 0xF)
                            objectX = -1;
                        if (objectY == 0xF)
                            objectY = -1;
                        dungeonDrawer.objects[ind].x = (byte)(objectX + (x - lastMapHoverPoint.X));
                        dungeonDrawer.objects[ind].y = (byte)(objectY + (y - lastMapHoverPoint.Y));
                        if (objectX + (x - lastMapHoverPoint.X) < 0 || objectX + (x - lastMapHoverPoint.X) > 9)
                            dungeonDrawer.objects[ind].x = 0;
                        if (objectY + (y - lastMapHoverPoint.Y) < 0 || objectY + (y - lastMapHoverPoint.Y) > 7)
                            dungeonDrawer.objects[ind].y = 0;
                        drawDungeon();
                    }
                    if (ind != -1)
                        dungeonDrawer.drawSelectedObject((Bitmap)pMap.Image, selectedObject);
                }
            }
            else if (cSprite.Checked)
            {
                int ind = (int)nSpriteSelected.Value;
                int x = e.X / 16;
                int y = e.Y / 16;
                if (e.Button == MouseButtons.Left)
                {
                    if (ind > -1)
                    {
                        sprites.spriteList[ind].x = (byte)(objectX + (x - lastMapHoverPoint.X));
                        sprites.spriteList[ind].y = (byte)(objectY + (y - lastMapHoverPoint.Y));
                        if (objectX + (x - lastMapHoverPoint.X) < 0 || objectX + (x - lastMapHoverPoint.X) > 9)
                            sprites.spriteList[ind].x = 0;
                        if (objectY + (y - lastMapHoverPoint.Y) < 0 || objectY + (y - lastMapHoverPoint.Y) > 7)
                            sprites.spriteList[ind].y = 0;
                        if (!overWorld)
                            drawDungeon();
                        else
                            drawOverworld();
                        if (ind != -1)
                            sprites.drawSelectedSprite(pMap.Image, selectedObject);
                    }
                }
            }
            //lastMapHoverPoint = new Point(e.X / 16, e.Y / 16);
            pMap.Invalidate();
        }

        private void saveROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        private void save()
        {
            if (filename == "")
                return;
            bool save;
            List<byte> bl = new List<byte>();
            foreach (byte b in gb.Buffer)
                bl.Add(b);
            byte[] ba = bl.ToArray();
            GBHL.GBFile gbb = new GBHL.GBFile(ba);
            mapSaver.gb = gbb;
            patches.gb = gbb;
            if (overWorld)
            {
                if (usedspace > freespace)
                {
                    DialogResult dr = MessageBox.Show("There is more space being used than allocated.\nSaving may corrupt the next rooms data. This is not\na permanent change and can be fixed.\n\nWould you like to save this data anyway?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (dr == DialogResult.Yes)
                    {
                        if (!cSprite.Checked)
                        {
                            save = mapSaver.saveOverworldCollision(
                                overworldDrawer.objects,
                                overworldDrawer.warps,
                                mapIndex,
                                floorTile,
                                wallTiles,
                                spriteBank,
                                cSpecialTiles.Checked,
                                usedspace,
                                freespace,
                                cSpecialTiles.Checked,
                                overworldDrawer.pointers,
                                overworldDrawer.unSortedPointers);
                        }
                        else
                        {
                            save = mapSaver.saveSprites(sprites.spriteList, overWorld, mapIndex, dungeonIndex, usedspace, freespace, sprites.pointers, sprites.unSortedPointers);
                        }

                        if (save)
                        {
                            ba = bl.ToArray();
                            gbb = new GBHL.GBFile(ba);
                            mapSaver.gb = gbb;
                            patches.gb = gbb;
                        }
                    }
                    else
                        return;
                }
                else
                {
                    if (!cSprite.Checked)
                    {
                        save = mapSaver.saveOverworldCollision(
                            overworldDrawer.objects,
                            overworldDrawer.warps,
                            mapIndex,
                            floorTile,
                            wallTiles,
                            spriteBank,
                            cSpecialTiles.Checked,
                            usedspace,
                            freespace,
                            cSpecialTiles.Checked,
                            overworldDrawer.pointers,
                            overworldDrawer.unSortedPointers);
                    }
                    else
                    {
                        save = mapSaver.saveSprites(
                            sprites.spriteList,
                            overWorld,
                            mapIndex,
                            dungeonIndex,
                            usedspace,
                            freespace,
                            sprites.pointers,
                            sprites.unSortedPointers);
                    }
                    if (save)
                    {
                        ba = bl.ToArray();
                        gbb = new GBHL.GBFile(ba);
                        mapSaver.gb = gbb;
                        patches.gb = gbb;
                    }
                }
                if (rOverlay.Checked)
                    mapSaver.saveOverlay(mapData, mapIndex, cSpecialTiles.Checked);
            }
            else
            {
                if (usedspace > freespace)
                {
                    DialogResult dr = MessageBox.Show("There is more space being used than allocated.\nSaving may corrupt the next rooms data. This is not\na permanent change and can be fixed.\n\nWould you like to save this data anyway?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (dr == DialogResult.Yes)
                    {
                        if (!cSprite.Checked)
                        {
                            save = mapSaver.saveDungeonCollision(dungeonDrawer.objects,
                                    dungeonDrawer.warps,
                                    dungeonIndex,
                                    mapIndex,
                                    floorTile,
                                    wallTiles,
                                    spriteBank,
                                    usedspace,
                                    freespace,
                                    dungeonDrawer.pointers,
                                    dungeonDrawer.unSortedPointers,
                                    cMagGlass.Checked);
                        }
                        else
                        {
                            save = mapSaver.saveSprites(sprites.spriteList,
                                    overWorld,
                                    mapIndex,
                                    dungeonIndex,
                                    usedspace,
                                    freespace,
                                    sprites.pointers,
                                    sprites.unSortedPointers);
                        }
                        if (save)
                        {
                            ba = bl.ToArray();
                            gbb = new GBHL.GBFile(ba);
                            mapSaver.gb = gbb;
                            patches.gb = gbb;
                        }
                    }
                    else
                        return;
                }
                else
                {
                    if (!cSprite.Checked)
                    {
                        save = mapSaver.saveDungeonCollision(dungeonDrawer.objects,
                                dungeonDrawer.warps,
                                dungeonIndex,
                                mapIndex,
                                floorTile,
                                wallTiles,
                                spriteBank,
                                usedspace,
                                freespace,
                                dungeonDrawer.pointers,
                                dungeonDrawer.unSortedPointers,
                                cMagGlass.Checked);
                    }
                    else
                    {
                        save = mapSaver.saveSprites(sprites.spriteList,
                                overWorld,
                                mapIndex,
                                dungeonIndex,
                                usedspace,
                                freespace,
                                sprites.pointers,
                                sprites.unSortedPointers);
                    }
                    if (save)
                    {
                        ba = bl.ToArray();
                        gbb = new GBHL.GBFile(ba);
                        mapSaver.gb = gbb;
                        patches.gb = gbb;
                    }
                }
            }
            patches.defaultMusic(defaultMusicToolStripMenuItem.Checked);
            //mapSaver.saveSprites(sprites.spriteList, overWorld, mapIndex, dungeonIndex, usedspace, freespace);
            mapSaver.saveMinimapInfo(overWorld, dungeonIndex, minimapDrawer.roomIndexes, minimapDrawer.minimapGraphics, minimapDrawer.overworldPal);
            mapSaver.saveAreaInfo(overWorld, mapIndex, dungeonIndex, animations, SOG, music, cSpecialTiles.Checked, cMagGlass.Checked);
            mapSaver.saveChestInfo(overWorld, mapIndex, dungeonIndex, chestData);
            mapSaver.savePaletteInfo(palette, overWorld, cSideview.Checked, dungeonIndex, mapIndex, palOffset);
            if (!overWorld)
                mapSaver.saveDungeonEventInfo(dungeonIndex, mapIndex, (byte)nEventID.Value, (byte)nEventTrigger.Value);
            gb.Buffer = gbb.Buffer;
            writeFile();
        }

        private void writeFile()
        {
            BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.Open));
            bw.Write(gb.Buffer);
            bw.Close();
            if (overWorld)
            {
                if (cSprite.Checked)
                {
                    freespace = sprites.getFreeSpace(true, mapIndex, dungeonIndex);
                    usedspace = sprites.getUsedSpace();
                }
                else
                {
                    freespace = overworldDrawer.getFreeSpace(mapIndex, cSpecialTiles.Checked);
                    usedspace = overworldDrawer.getUsedSpace();
                }
                drawOverworld();
            }
            else
            {
                if (cSprite.Checked)
                {
                    freespace = sprites.getFreeSpace(false, mapIndex, dungeonIndex);
                    usedspace = sprites.getUsedSpace();
                }
                else
                {
                    freespace = dungeonDrawer.getFreeSpace(mapIndex, dungeonIndex, cMagGlass.Checked);
                    usedspace = dungeonDrawer.getUsedSpace();
                }
                drawDungeon();
            }
        }

        private void pMinimapD_MouseClick(object sender, MouseEventArgs e)
        {
            if (pTiles.Image != null)
            {
                int s = (e.X / 8) + ((e.Y / 8) * 8);
                mapIndex = minimapDrawer.roomIndexes[s];
                nMap.Value = mapIndex;
                LoadSandA();
                loadTileset();
                collisionListView();
                setCollisionData();
                setSpriteData();
                drawDungeon();
            }
        }

        private void rCrystals_CheckedChanged_1(object sender, EventArgs e)
        {
            loadTileset();
            drawDungeon();
        }

        private void cSideview_CheckedChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2)
                sideView = cSideview2.Checked;
            else
                sideView = cSideview.Checked;
            LoadSandA();
            loadTileset();
            drawDungeon();
        }

        private void cSpecialTiles_CheckedChanged(object sender, EventArgs e)
        {
            overworldDrawer.getCollisionDataOverworld(mapIndex, cSpecialTiles.Checked);
            overworldDrawer.loadCollisionsOverworld();
            if (!cSprite.Checked)
            {
                usedspace = overworldDrawer.getUsedSpace();
                freespace = overworldDrawer.getFreeSpace(mapIndex, cSpecialTiles.Checked);
            }
            else
            {
                usedspace = sprites.getUsedSpace();
                freespace = sprites.getFreeSpace(overWorld, mapIndex, dungeonIndex);
            }
            LoadSandA();
            loadTileset();
            drawOverworld();
        }

        private void nRegion_ValueChanged(object sender, EventArgs e)
        {
            dungeonIndex = (byte)nRegion.Value;
            LoadSandA();
            setCollisionData();
            setSpriteData();
            loadTileset();
            collisionListView();
            dungeonDrawer.getCollisionDataDungeon(mapIndex, dungeonIndex, cMagGlass.Checked);
            dungeonDrawer.getEventData(mapIndex, dungeonIndex);
            nEventID.Value = dungeonDrawer.eventID;
            nEventTrigger.Value = dungeonDrawer.eventTrigger;
            label18.Text = "Location: 0x" + dungeonDrawer.eventDataLocation.ToString("X");
            if (tabControl1.SelectedIndex == 2 && mapIndex == 0xF5 && (dungeonIndex >= 0x1A || dungeonIndex < 6))
                cMagGlass.Visible = true;
            else
                cMagGlass.Visible = false;
            if (!cSprite.Checked)
            {
                freespace = dungeonDrawer.getFreeSpace(mapIndex, dungeonIndex, cMagGlass.Checked);
                usedspace = dungeonDrawer.getUsedSpace();
            }
            else
            {
                freespace = sprites.getFreeSpace(overWorld, mapIndex, dungeonIndex);
                usedspace = sprites.getUsedSpace();
            }
            WL = new List<Warps>();
            WL = dungeonDrawer.warps;
            drawDungeon();
        }

        private void rOverlay_CheckedChanged(object sender, EventArgs e)
        {
            if (rOverlay.Checked)
            {
                overLay = true;
                overworldDrawer.getCollisionDataOverworld(mapIndex, cSpecialTiles.Checked);
                WL = new List<Warps>();
                WL = overworldDrawer.warps;
                drawOverworld();
            }
        }

        private void rCollision_CheckedChanged(object sender, EventArgs e)
        {
            if (rCollision.Checked)
            {
                overLay = false;
                overworldDrawer.getCollisionDataOverworld(mapIndex, cSpecialTiles.Checked);
                WL = new List<Warps>();
                WL = overworldDrawer.warps;
                setCollisionData();
                if (!cSprite.Checked)
                {
                    freespace = overworldDrawer.getFreeSpace(mapIndex, cSpecialTiles.Checked);
                    usedspace = overworldDrawer.getUsedSpace();
                }
                else
                {
                    freespace = sprites.getFreeSpace(overWorld, mapIndex, dungeonIndex);
                    usedspace = sprites.getUsedSpace();
                }
                collisionListView();
                drawOverworld();
            }
        }

        private void collisionBordersToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (pTiles.Image != null)
            {
                if (overWorld)
                    drawOverworld();
                else
                    drawDungeon();
            }
        }

        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Credits credits = new Credits();
            credits.ShowDialog();
        }

        private void nAnimations_ValueChanged(object sender, EventArgs e)
        {
            animations = (byte)nAnimations.Value;
            loadTileset();
            if (overWorld)
                drawOverworld();
            else
                drawDungeon();
        }

        private void nSOG_ValueChanged(object sender, EventArgs e)
        {
            SOG = (byte)nSOG.Value;
            loadTileset();
            if (overWorld)
                drawOverworld();
            else
                drawDungeon();
        }

        private void nFloor_ValueChanged(object sender, EventArgs e)
        {
            floorTile = (byte)nFloor.Value;
            loadTileset();
            if (overWorld)
                drawOverworld();
            else
                drawDungeon();
        }

        private void nMusic_ValueChanged(object sender, EventArgs e)
        {
            music = (byte)nMusic.Value;
        }

        private void nWall_ValueChanged(object sender, EventArgs e)
        {
            wallTiles = (byte)nWall.Value;
            loadTileset();
            if (!overWorld)
                drawDungeon();
            else
                drawOverworld();
        }
        
        private void nSpriteBank_ValueChanged(object sender, EventArgs e)
        {
            spriteBank = (byte)nSpriteBank.Value;
            loadTileset();

            if (overWorld)
                drawOverworld();
            else
                drawDungeon();
        }

        private void nSelected_ValueChanged(object sender, EventArgs e)
        {
            if (nSelected.Value == -1)
            {
                if (cObjectList.Checked)
                {
                    if (CollisionList.SelectedIndex != -1)
                        CollisionList.SetSelected(CollisionList.SelectedIndex, false);
                }
                pObject.Invalidate();
                if (overWorld)
                    drawOverworld();
                else
                    drawDungeon();
                return;
            }
            Object o = new Object();
            if (overWorld)
                o = overworldDrawer.objects[(byte)nSelected.Value];
            else
                o = dungeonDrawer.objects[(byte)nSelected.Value];
            if (o.is3Byte)
            {
                comDirection.Enabled = true;
                nLength.Enabled = true;
                nLength.Value = o.length;
                if (o.direction == 8)
                    comDirection.SelectedIndex = 0;
                else
                    comDirection.SelectedIndex = 1;
            }
            else
            {
                comDirection.Enabled = false;
                nLength.Enabled = false;
            }
            nObjectID.Value = o.id;
            if (cObjectList.Checked)
                CollisionList.SelectedIndex = (byte)nSelected.Value;
            selectedObject = o;
            lblHoverPos.Text = "X: " + o.x.ToString("X") + " Y: " + o.y.ToString("X");
            if (overWorld)
            {
                drawOverworld();
                overworldDrawer.drawSelectedObject((Bitmap)pMap.Image, selectedObject);
            }
            else
            {
                drawDungeon();
                dungeonDrawer.drawSelectedObject((Bitmap)pMap.Image, selectedObject);
            }
            pObject.Invalidate();
        }

        private void selectCollision(Object o, int index)
        {
            if (index == -1)
            {
                if (cSprite.Checked)
                {
                    nSpriteSelected.Value = -1;
                    nSpriteID.Value = 0;
                }
                else
                {
                    nSelected.Value = -1;
                    comDirection.Enabled = false;
                    nLength.Enabled = false;
                    nObjectID.Value = 0;
                    pObject.Invalidate();
                }
                return;
            }
            if (cSprite.Checked)
            {
                nSpriteSelected.Value = index;
                nSpriteID.Value = o.id;
            }
            else
            {
                nSelected.Value = index;
                if (o.is3Byte)
                {
                    comDirection.Enabled = true;
                    nLength.Enabled = true;
                }
                comDirection.SelectedIndex = (o.direction == 8 ? 0 : 1);
                nLength.Value = o.length;
                nObjectID.Value = o.id;
            }
        }

        private void nObjectID_ValueChanged(object sender, EventArgs e)
        {
            if (nSelected.Value == -1)
                return;
            string s;
            Object o = new Object();
            if (overWorld)
            {
                o = overworldDrawer.objects[(byte)nSelected.Value];
                o.id = (byte)nObjectID.Value;
                o = o.getOverworldSpecial(o);
                overworldDrawer.objects[(byte)nSelected.Value] = o;

            }
            else
            {
                o = dungeonDrawer.objects[(byte)nSelected.Value];
                o.id = (byte)nObjectID.Value;
                if (o.id >= 0xEC && o.id <= 0xFD)// Door tiles
                {
                    o.gb = gb;
                    o = o.dungeonDoors(o);
                    dungeonDrawer.objects[(byte)nSelected.Value] = o;
                }
                else
                {
                    o.isDoor2 = false;
                    o.isEntrance = false;
                    o.isDoor1 = false;
                }
            }

            if (cObjectList.Checked == true)
            {
                if (o.is3Byte)
                    s = "3-Byte";
                else
                    s = "2-Byte";
                s += "      0x" + o.id.ToString("X");
                CollisionList.Items[(int)nSelected.Value] = s;
            }

            selectedObject = o;
            if (overWorld)
                drawOverworld();
            else
                drawDungeon();
            pObject.Invalidate();
        }

        private void nLength_ValueChanged(object sender, EventArgs e)
        {
            if (nSelected.Value == -1)
                return;
            Object o = new Object();
            if (overWorld)
                o = overworldDrawer.objects[(byte)nSelected.Value];
            else
                o = dungeonDrawer.objects[(byte)nSelected.Value];
            o.length = (byte)nLength.Value;
            if (overWorld)
                drawOverworld();
            else
                drawDungeon();
        }

        private void comDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (nSelected.Value == -1)
                return;
            Object o = new Object();
            if (overWorld)
                o = overworldDrawer.objects[(byte)nSelected.Value];
            else
                o = dungeonDrawer.objects[(byte)nSelected.Value];
            if (comDirection.SelectedIndex == 0)
                o.direction = 8;
            else
                o.direction = 0xC;
            if (overWorld)
                drawOverworld();
            else
                drawDungeon();
        }

        private void collisionListView()
        {
            if (cObjectList.Checked)
            {
                CollisionList.Items.Clear();
                List<Object> obj = new List<Object>();
                string s;
                if (overWorld)
                    obj = overworldDrawer.objects;
                else
                    obj = dungeonDrawer.objects;
                foreach (Object ob in obj)
                {
                    if (ob.is3Byte)
                        s = "3-Byte";
                    else
                        s = "2-Byte";
                    s += "      0x" + ob.id.ToString("X");
                    CollisionList.Items.Add(s);
                }
                CollisionList.SelectedIndex = (int)nSelected.Value;
            }
        }

        private void CollisionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            nSelected.Value = CollisionList.SelectedIndex;
            if (nSelected.Value != -1)
            {
                if (overWorld)
                {
                    drawOverworld();
                    overworldDrawer.drawSelectedObject((Bitmap)pMap.Image, selectedObject);
                }
                else
                {
                    drawDungeon();
                    dungeonDrawer.drawSelectedObject((Bitmap)pMap.Image, selectedObject);
                }
            }
        }

        private void pObject_Paint(object sender, PaintEventArgs e)
        {
            if (pTiles.Image != null && !overLay)
            {
                if (nSelected.Value == -1)
                {
                    pObject.Width = 20;
                    pObject.Height = 20;
                    return;
                }
                if (!overWorld)
                {
                    if (selectedObject != null)
                    {
                        if (selectedObject.id < 0xEC || selectedObject.id > 0xFD || selectedObject.is3Byte)
                        {
                            pObject.Width = 20;
                            pObject.Height = 20;
                            e.Graphics.DrawImage(pTiles.Image, new Rectangle(0, 0, 16, 16), (selectedObject.id % 16) * 16, (selectedObject.id / 16) * 16, 16, 16, GraphicsUnit.Pixel);
                        }
                        else
                        {
                            pObject.Width = (selectedObject.w * 16) + 4;
                            pObject.Height = (selectedObject.h * 16) + 4;
                            for (int y = 0; y < selectedObject.h; y++)
                            {
                                for (int x = 0; x < selectedObject.w; x++)
                                {
                                    int id = selectedObject.tiles[(y * selectedObject.w) + x];
                                    e.Graphics.DrawImage(pTiles.Image, new Rectangle(0 + (x * 16), 0 + (y * 16), 16, 16), (id % 16) * 16, (id / 16) * 16, 16, 16, GraphicsUnit.Pixel);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (selectedObject != null)
                    {
                        if (selectedObject.id < 0xF5 || selectedObject.id >= 0xFE)
                        {
                            pObject.Width = (selectedObject.w * 16) + 4;
                            pObject.Height = (selectedObject.h * 16) + 4;
                            e.Graphics.DrawImage(pTiles.Image, new Rectangle(0, 0, 16, 16), (selectedObject.id % 16) * 16, (selectedObject.id / 16) * 16, 16, 16, GraphicsUnit.Pixel);
                        }
                        else
                        {
                            pObject.Width = (selectedObject.w * 16) + 4;
                            pObject.Height = (selectedObject.h * 16) + 4;
                            for (int y = 0; y < selectedObject.h; y++)
                            {
                                for (int x = 0; x < selectedObject.w; x++)
                                {
                                    int id = selectedObject.tiles[(y * selectedObject.w) + x];
                                    e.Graphics.DrawImage(pTiles.Image, new Rectangle(0 + (x * 16), 0 + (y * 16), 16, 16), (id % 16) * 16, (id / 16) * 16, 16, 16, GraphicsUnit.Pixel);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void bCollisionUp_Click(object sender, EventArgs e)
        {
            if (CollisionList.Items.Count == 0 || nSelected.Value == -1)
                return;
            object list = CollisionList.SelectedItem;
            byte index = (byte)CollisionList.SelectedIndex;
            List<Object> objects = new List<Object>();
            if (overWorld)
                objects = overworldDrawer.objects;
            else
                objects = dungeonDrawer.objects;
            Object O = objects[index];
            if (index == 0)
                return;
            objects.Remove(O);
            objects.Insert(index - 1, O);
            CollisionList.Items.Remove(list);
            CollisionList.Items.Insert(index - 1, list);

            string s;
            CollisionList.Items.Clear();
            foreach (Object obj in objects)
            {
                if (obj.is3Byte)
                    s = "3-Byte";
                else
                    s = "2-Byte";
                s += "      0x" + obj.id.ToString("X");
                CollisionList.Items.Add(s);
            }
            CollisionList.SelectedIndex = index - 1;

            if (overWorld)
                drawOverworld();
            else
                drawDungeon();

        }

        private void bCollisionDown_Click(object sender, EventArgs e)
        {
            if (CollisionList.Items.Count == 0 || nSelected.Value == -1)
                return;
            object list = CollisionList.SelectedItem;
            byte index = (byte)CollisionList.SelectedIndex;
            List<Object> objects = new List<Object>();
            if (overWorld)
                objects = overworldDrawer.objects;
            else
                objects = dungeonDrawer.objects;
            Object O = objects[index];
            if (index == nSelected.Maximum)
                return;
            objects.Remove(O);
            objects.Insert(index + 1, O);
            CollisionList.Items.Remove(list);
            CollisionList.Items.Insert(index + 1, list);

            string s;
            CollisionList.Items.Clear();
            foreach (Object obj in objects)
            {
                if (obj.is3Byte)
                    s = "3-Byte";
                else
                    s = "2-Byte";
                s += "      0x" + obj.id.ToString("X");
                CollisionList.Items.Add(s);
            }
            CollisionList.SelectedIndex = index + 1;

            if (overWorld)
                drawOverworld();
            else
                drawDungeon();

        }

        private void pTiles_Paint(object sender, PaintEventArgs e)
        {
            pObject.Invalidate();
        }

        private void cObjectList_CheckedChanged(object sender, EventArgs e)
        {
            if (!cObjectList.Checked)
            {
                CollisionList.Enabled = false;
                bCollisionDown.Enabled = false;
                bCollisionUp.Enabled = false;
                bAdd.Enabled = false;
                bDelete.Enabled = false;
                CollisionList.Items.Clear();
            }
            else
            {
                bAdd.Enabled = true;
                bDelete.Enabled = true;
                CollisionList.Enabled = true;
                bCollisionDown.Enabled = true;
                bCollisionUp.Enabled = true;
                collisionListView();
            }
        }

        private void toolStripWarpEditor_Click(object sender, EventArgs e)
        {
            if (pTiles.Image != null)
            {
                if (cSprite.Checked)
                {
                    MessageBox.Show("You must be editing the collisions/overlay to edit warps.");
                }
                else
                {
                    List<Warps> backup = new List<Warps>();
                    foreach (Warps w in WL)
                        backup.Add(w);
                    WarpEditor WEdit = new WarpEditor(backup);
                    WEdit.ShowDialog();
                    if (WEdit.DialogResult == DialogResult.OK)
                    {
                        WL = WEdit.warpList;
                        if (overWorld)
                        {
                            overworldDrawer.warps = backup;
                            usedspace = overworldDrawer.getUsedSpace();
                            drawOverworld();
                        }
                        else
                        {
                            dungeonDrawer.warps = backup;
                            usedspace = dungeonDrawer.getUsedSpace();
                            drawDungeon();
                        }
                    }
                }
            }
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            NewObject newOb = new NewObject(gb, overWorld);
            if (newOb.ShowDialog() == DialogResult.OK)
            {
                Object O = new Object();
                O = newOb.O;
                selectedObject = O;
                if (overWorld)
                {
                    if (nSelected.Value == -1)
                        overworldDrawer.objects.Add(O);
                    else
                        overworldDrawer.objects.Insert((byte)nSelected.Value, O);
                    if (O.is3Byte)
                    {
                        //freespace -= 3;
                        usedspace += 3;
                    }
                    else
                    {
                        //freespace -= 2;
                        usedspace += 2;
                    }
                    collisionListView();
                    drawOverworld();
                    if (nSelected.Value == -1)
                        nSelected.Value = overworldDrawer.objects.Count - 1;
                }
                else
                {
                    if (nSelected.Value == -1)
                        dungeonDrawer.objects.Add(O);
                    else
                        dungeonDrawer.objects.Insert((byte)nSelected.Value, O);
                    if (O.is3Byte)
                    {
                        //freespace -= 3;
                        usedspace += 3;
                    }
                    else
                    {
                        //freespace -= 2;
                        usedspace += 2;
                    }
                    collisionListView();
                    drawDungeon();
                    if (nSelected.Value == -1)
                        nSelected.Value = dungeonDrawer.objects.Count - 1;
                }
                pObject.Invalidate();
            }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            if (nSelected.Value == -1)
                return;
            Object O = new Object();
            if (overWorld)
            {
                O = overworldDrawer.objects[(byte)nSelected.Value];
                if (O.is3Byte)
                {
                    //freespace += 3;
                    usedspace -= 3;
                }
                else
                {
                    //freespace += 2;
                    usedspace -= 2;
                }
                overworldDrawer.objects.RemoveAt((byte)nSelected.Value);
                if ((byte)nSelected.Value == overworldDrawer.objects.Count)
                    nSelected.Value -= 1;
                if (nSelected.Value != -1)
                    selectedObject = overworldDrawer.objects[(byte)nSelected.Value];
            }
            else
            {
                if (O.is3Byte)
                {
                    //freespace += 3;
                    usedspace -= 3;
                }
                else
                {
                    //freespace += 2;
                    usedspace -= 2;
                }
                dungeonDrawer.objects.RemoveAt((byte)nSelected.Value);
                if ((byte)nSelected.Value == dungeonDrawer.objects.Count)
                    nSelected.Value -= 1;
                if (nSelected.Value != -1)
                    selectedObject = dungeonDrawer.objects[(byte)nSelected.Value];
            }
            collisionListView();
            //pObject.Invalidate();
            if (overWorld)
                drawOverworld();
            else
                drawDungeon();
        }

        private void cm_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            if (overWorld)
                overworldDrawer.objects.Clear();
            else
                dungeonDrawer.objects.Clear();
            nSelected.Value = -1;
            collisionListView();
            if (overWorld)
            {
                usedspace = overworldDrawer.getUsedSpace();
                freespace = overworldDrawer.getFreeSpace(mapIndex, cSpecialTiles.Checked);
                drawOverworld();
            }
            else
            {
                usedspace = dungeonDrawer.getUsedSpace();
                freespace = dungeonDrawer.getFreeSpace(mapIndex, dungeonIndex, cMagGlass.Checked);
                drawDungeon();
            }
        }

        private void CollisionList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                CollisionList.ContextMenuStrip = new ContextMenuStrip();
                CollisionList.ContextMenuStrip.Items.Add("Clear All");
                CollisionList.ContextMenuStrip.Show(Cursor.Position);
                CollisionList.ContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(cm_ItemClicked);
            }
        }

        private void pMap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int ind = getDandOObjectID(e.X, e.Y);
            if (overWorld && !overLay && !cSprite.Checked)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (ind > -1)
                    {
                        overworldDrawer.objects.RemoveAt(ind);
                        if (nSelected.Value == overworldDrawer.objects.Count)
                            nSelected.Value--;
                        collisionListView();
                        usedspace = overworldDrawer.getUsedSpace();
                        drawOverworld();
                        if (nSelected.Value != -1)
                            selectedObject = overworldDrawer.objects[(byte)nSelected.Value];
                        pObject.Invalidate();
                    }
                }
            }
            else if (!overLay)
            {
                if (!cSprite.Checked)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (ind > -1)
                        {
                            dungeonDrawer.objects.RemoveAt(ind);
                            if (nSelected.Value == dungeonDrawer.objects.Count)
                                nSelected.Value--;
                            usedspace = dungeonDrawer.getUsedSpace();
                            collisionListView();
                            drawDungeon();
                            if (nSelected.Value != -1)
                                selectedObject = dungeonDrawer.objects[(byte)nSelected.Value];
                            pObject.Invalidate();
                        }
                    }
                }
            }
            lastMapHoverPoint = new Point(e.X / 16, e.Y / 16);
            pMap.Invalidate();
        }

        private void toolChestEditor_Click(object sender, EventArgs e)
        {
            if (pTiles.Image != null)
            {
                byte chest = 0;
                chest += chestData;
                ChestEditor CE = new ChestEditor(gb, chestData);
                CE.ShowDialog();
                if (CE.DialogResult == DialogResult.OK)
                    chestData = CE.chestData;
            }
        }

        private void toolMiniMapEditor_Click(object sender, EventArgs e)
        {
            if (pTiles.Image != null && tabControl1.SelectedIndex != 2)
            {
                Bitmap bmp = new Bitmap(128, 128);
                FastPixel fp = new FastPixel(bmp);
                FastPixel src;
                if (!overWorld)
                    src = new FastPixel((Bitmap)pMinimapD.Image);
                else
                    src = new FastPixel((Bitmap)pMinimap.Image);
                src.rgbValues = new byte[128 * 128 * 4];
                fp.rgbValues = new byte[128 * 128 * 4];
                fp.Lock();
                src.Lock();
                if (!overWorld)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        for (int x = 0; x < 8; x++)
                        {
                            for (int y1 = 0; y1 < 8; y1++)
                            {
                                for (int x1 = 0; x1 < 8; x1++)
                                {
                                    fp.SetPixel((x * 8) + x1, (y * 8) + y1, src.GetPixel((x * 8) + x1, (y * 8) + y1));
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < 16; y++)
                    {
                        for (int x = 0; x < 16; x++)
                        {
                            for (int y1 = 0; y1 < 8; y1++)
                            {
                                for (int x1 = 0; x1 < 8; x1++)
                                {
                                    fp.SetPixel((x * 8) + x1, (y * 8) + y1, src.GetPixel((x * 8) + x1, (y * 8) + y1));
                                }
                            }
                        }
                    }
                }
                fp.Unlock(true);
                src.Unlock(true);
                byte[, ,] loadmini;
                List<byte> ri = new List<byte>();
                List<byte> mg = new List<byte>();
                List<byte> op = new List<byte>();
                if (!overWorld)
                {
                    foreach (byte b in minimapDrawer.roomIndexes)
                        ri.Add(b);
                }
                else
                {
                    foreach (byte b in minimapDrawer.overworldPal)
                        op.Add(b);
                }
                foreach (byte b1 in minimapDrawer.minimapGraphics)
                    mg.Add(b1);
                if (!overWorld)
                    loadmini = minimapDrawer.loadMinimapDungeon();
                else
                    loadmini = minimapDrawer.loadMinimapOverworld();
                byte[] mga = mg.ToArray();
                byte[] ria = ri.ToArray();
                byte[] opa = op.ToArray();
                List<byte> gbb = new List<byte>();
                foreach (byte bb in gb.Buffer)
                    gbb.Add(bb);
                GBHL.GBFile g = new GBHL.GBFile(gbb.ToArray());
                MinimapEditor ME = new MinimapEditor(g, bmp, ria, loadmini, mga, overWorld, opa, dungeonIndex);
                ME.ShowDialog();
                if (ME.DialogResult == DialogResult.OK)
                {
                    minimapDrawer.minimapGraphics = ME.minimapData;
                    if (overWorld)
                    {
                        minimapDrawer.overworldPal = ME.overworldpal;
                        pMinimap.Image = ME.pMinimapO.Image;
                    }
                    else
                    {
                        minimapDrawer.roomIndexes = ME.roomIndexes;
                        pMinimapD.Image = ME.pMinimap.Image;
                        gb.Buffer = ME.gb.Buffer;
                    }
                }
            }
        }

        private void nSpriteSelected_ValueChanged(object sender, EventArgs e)
        {
            if (nSpriteSelected.Value == -1)
            {
                nSpriteID.Value = 0;
                if (overWorld)
                    drawOverworld();
                else
                    drawDungeon();
                return;
            }
            Object o = new Object();
            o = sprites.spriteList[(byte)nSpriteSelected.Value];
            nSpriteID.Value = (byte)o.id;
            selectedObject = o;
            lblHoverPos.Text = "X: " + o.x.ToString("X") + " Y: " + o.y.ToString("X");
            if (overWorld)
                drawOverworld();
            else
                drawDungeon();
            sprites.drawSelectedSprite(pMap.Image, selectedObject);
        }

        private void cSprite_CheckedChanged(object sender, EventArgs e)
        {
            if (cSprite.Checked)
            {
                bAddSprite.Enabled = true;
                bDeleteSprite.Enabled = true;
                nSpriteSelected.Enabled = true;
                nSpriteID.Enabled = true;
                nSelected.Value = -1;
                if (cObjectList.Checked)
                    cObjectList.Checked = false;
                usedspace = sprites.getUsedSpace();
                freespace = sprites.getFreeSpace(overWorld, mapIndex, dungeonIndex);
            }
            else
            {
                nSpriteID.Value = 0;
                nSpriteSelected.Enabled = false;
                nSpriteID.Enabled = false;
                bAddSprite.Enabled = false;
                bDeleteSprite.Enabled = false;
            }
            if (overWorld)
            {
                if (!cSprite.Checked)
                {
                    usedspace = overworldDrawer.getUsedSpace();
                    freespace = overworldDrawer.getFreeSpace(mapIndex, cSpecialTiles.Checked);
                }
                drawOverworld();
            }
            else
            {
                if (!cSprite.Checked)
                {
                    usedspace = dungeonDrawer.getUsedSpace();
                    freespace = dungeonDrawer.getFreeSpace(mapIndex, dungeonIndex, cMagGlass.Checked);
                }
                drawDungeon();
            }
            setSpriteData();
        }

        private void nSpriteID_ValueChanged(object sender, EventArgs e)
        {
            if (nSpriteSelected.Value == -1)
            {
                spriteLabel.Text = "Sprite: null";
                return;
            }
            sprites.spriteList[(byte)nSpriteSelected.Value].id = (byte)nSpriteID.Value;

            spriteLabel.Text = "Sprite: " + Names.GetName(Names.sprites, (int)nSpriteID.Value);
        }

        private void bAddSprite_Click(object sender, EventArgs e)
        {
            Object o = new Object();
            o.id = 0;
            o.x = 0;
            o.y = 0;
            sprites.spriteList.Add(o);
            nSpriteSelected.Maximum += 1;
            usedspace += 2;
            if (overWorld)
                drawOverworld();
            else
                drawDungeon();
        }

        private void bDeleteSprite_Click(object sender, EventArgs e)
        {
            if (nSpriteSelected.Value != -1)
            {
                int ind = sprites.spriteList.IndexOf(selectedObject);
                sprites.spriteList.RemoveAt(ind);
                nSpriteSelected.Value -= 1;
                nSpriteSelected.Maximum -= 1;
                usedspace -= 2;
                if (overWorld)
                    drawOverworld();
                else
                    drawDungeon();
            }
        }

        private void exportMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pTiles.Image != null && overLay && tabControl1.SelectedIndex == 0)
            {
                SaveFileDialog s = new SaveFileDialog();
                s.Title = "Export Map Group";
                s.Filter = "PNG Files (*.png)|*.png";
                if (s.ShowDialog() != DialogResult.OK)
                    return;
                exportToFilename = s.FileName;
                ExportMaps = new ExportMap();
                ExportMaps.pBar.Maximum = 256;
                ExportMaps.Text = "Generating Map";
                new Thread(exportTheMaps).Start();
                ExportMaps.ShowDialog();
            }
        }

        private void exportTheMaps()
        {
            int i = 0;
            int xp = 0;
            int yp = 0;
            byte[] bigmap;
            Bitmap b = new Bitmap(2560, 2048);
            Bitmap srcB = new Bitmap(256, 256);
            FastPixel fp = new FastPixel(b);
            fp.rgbValues = new byte[2560 * 2048 * 4];
            fp.Lock();
            while (i != 256)
            {
                ExportMaps.setValue(i, 255);

                tileLoader.getAnimations((byte)i, 0, true, cSpecialTiles.Checked);
                tileLoader.getSOG((byte)i, true);
                byte[, ,] data = tileLoader.loadTileset(0, (byte)i, true, false, false);
                tileLoader.loadPallete(0, (byte)i, true, false);
                TileLoader.Tile[] tilez = tileLoader.loadPaletteFlipIndexes((byte)i, 0);
                srcB = tileLoader.drawTileset(data, tilez);
                FastPixel src = new FastPixel(srcB);
                src.rgbValues = new byte[256 * 256 * 4];
                src.Lock();

                bigmap = overworldDrawer.ReadMap((byte)i, cSpecialTiles.Checked);
                if (xp == 16)
                {
                    yp++;
                    xp = 0;
                }
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        byte v = bigmap[x + (y * 10)];

                        for (int yy = 0; yy < 16; yy++)
                        {
                            for (int xx = 0; xx < 16; xx++)
                            {
                                fp.SetPixel((xp * 160) + (x * 16) + xx, (yp * 128) + (y * 16) + yy, src.GetPixel((v % 16) * 16 + xx, (v / 16) * 16 + yy));
                            }
                        }
                    }
                }
                i++;
                xp++;
                src.Unlock(true);
            }
            fp.Unlock(true);
            if (exportToFilename != "")
                b.Save(exportToFilename);
        }

        private void cMagGlass_CheckedChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                if (cMagGlass1.Checked)
                    cMagGlass.Checked = true;
                else
                    cMagGlass.Checked = false;
            }
            dungeonDrawer.getCollisionDataDungeon(mapIndex, dungeonIndex, cMagGlass.Checked);
            dungeonDrawer.loadCollisionsDungeon();
            if (!cSprite.Checked)
            {
                usedspace = dungeonDrawer.getUsedSpace();
                freespace = dungeonDrawer.getFreeSpace(mapIndex, dungeonIndex, cMagGlass.Checked);
            }
            else
            {
                usedspace = sprites.getUsedSpace();
                freespace = sprites.getFreeSpace(overWorld, mapIndex, dungeonIndex);
            }
            cObjectList.Checked = false;
            nSelected.Value = -1;
            LoadSandA();
            loadTileset();
            drawDungeon();
        }

        private void toolStartPosEditor_Click(object sender, EventArgs e)
        {
            if (pTiles.Image != null)
            {
                StartEditor SE = new StartEditor(gb);
                SE.ShowDialog();
                if (SE.DialogResult == DialogResult.OK)
                {
                    startXPos = SE.xPos;
                    startYPos = SE.yPos;
                    SEMap = SE.map;
                    SEDung = SE.dungeon;
                    SEoverworld = SE.overworld;
                    mapSaver.saveStartPos(SEoverworld, SEDung, SEMap, startXPos, startYPos);
                }
            }
        }

        private void toolTextEditor_Click(object sender, EventArgs e)
        {
            if (pTiles.Image != null)
            {
                byte[] backup = new byte[gb.Buffer.Length];
                int i = 0;
                foreach (byte b in gb.Buffer)
                {
                    backup[i] = b;
                    i++;
                }
                TextEditor TE = new TextEditor(backup);
                TE.ShowDialog();

                if (TE.DialogResult == DialogResult.OK)
                {
                    gb.Buffer = TE.gb.Buffer;
                }
            }
        }

        private void tSpriteEditor_Click(object sender, EventArgs e)
        {
            if (pTiles.Image != null)
            {
                SpriteEditor SE = new SpriteEditor(gb, overWorld, mapIndex, dungeonIndex);
                SE.ShowDialog();

                if (SE.DialogResult == DialogResult.OK)
                {
                    gb.Buffer = SE.gb.Buffer;
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (pTiles.Image != null)
            {
                byte[] backup = new byte[gb.Buffer.Length];
                int i = 0;
                foreach (byte b in gb.Buffer)
                {
                    backup[i] = b;
                    i++;
                }
                SignEditor SE = new SignEditor(backup, mapIndex);
                SE.ShowDialog();

                if (SE.DialogResult == DialogResult.OK)
                {
                    gb.Buffer = SE.gb.Buffer;
                }
            }
        }

        private void toolOwlStatueEditor_Click(object sender, EventArgs e)
        {
            if (pTiles.Image != null)
            {
                byte[] backup = new byte[gb.Buffer.Length];
                int i = 0;
                foreach (byte b in gb.Buffer)
                {
                    backup[i] = b;
                    i++;
                }
                OwlStatueEditor OE = new OwlStatueEditor(backup);
                OE.ShowDialog();

                if (OE.DialogResult == DialogResult.OK)
                {
                    gb.Buffer = OE.gb.Buffer;
                }
            }
        }

        private void clearOverlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (overLay && pTiles.Image != null)
            {
                int i = 0;
                foreach (byte b in mapData)
                {
                    mapData[i] = 0;
                    i++;
                }
                pMap.Image = overworldDrawer.drawMap(tileset, mapData, collisionBordersToolStripMenuItem.Checked, selectedObject);
            }
        }

        private void toolCopy_Click(object sender, EventArgs e)
        {
            if (overLay && pTiles.Image != null)
            {
                int i = 0;
                mapDataCopy = new byte[mapData.Length];
                foreach (byte b in mapData)
                {
                    mapDataCopy[i] = b;
                    i++;
                }
            }
        }

        private void toolPaste_Click(object sender, EventArgs e)
        {
            if (overLay && mapDataCopy != null && pTiles.Image != null)
            {
                int i = 0;
                foreach (byte b in mapDataCopy)
                {
                    mapData[i] = b;
                    i++;
                }
                pMap.Image = overworldDrawer.drawMap(tileset, mapData, collisionBordersToolStripMenuItem.Checked, selectedObject);
            }
        }

        private void exportDungeonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pTiles.Image != null && !overLay && tabControl1.SelectedIndex == 1)
            {
                SaveFileDialog s = new SaveFileDialog();
                s.Title = "Export Dungeon Map Group";
                s.Filter = "PNG Files (*.png)|*.png";
                if (s.ShowDialog() != DialogResult.OK)
                    return;
                exportToFilename = s.FileName;
                ExportMaps = new ExportMap();
                ExportMaps.pBar.Maximum = 64;
                ExportMaps.Text = "Generating Map";
                new Thread(exportTheDungeonMaps).Start();
                ExportMaps.ShowDialog();
            }
        }

        private void exportTheDungeonMaps()
        {
            int i = 0;
            bool sideview;
            int xp = 0;
            int yp = 0;
            byte[] bigmap;
            Bitmap b = new Bitmap(1280, 1024);
            Bitmap srcB = new Bitmap(256, 256);
            FastPixel fp = new FastPixel(b);
            fp.rgbValues = new byte[1280 * 1024 * 4];
            fp.Lock();

            byte[] sideviewIndexes = new byte[64];
            byte[] roomIndexes = gb.ReadBytes(0x50220 + (64 * dungeonIndex), 64);
            if (dungeonIndex == 0xFF)
                roomIndexes = gb.ReadBytes(0x504E0, 64);
            for (int qq = 0; qq < 64; qq++)
            {
                dungeonDrawer.getCollisionDataDungeon(roomIndexes[qq], dungeonIndex, false);
                if (dungeonDrawer.warps != null)
                {
                    for (int ind = 0; ind < dungeonDrawer.warps.Count(); ind++)
                    {
                        if (dungeonDrawer.warps[ind].type == 2)
                        {
                            int q = Array.IndexOf(roomIndexes, dungeonDrawer.warps[ind].map);
                            if (q == -1)
                                sideviewIndexes[qq] = 0;
                            else
                                sideviewIndexes[q] = 1;
                        }
                    }
                }
                else
                    sideviewIndexes[qq] = 0;
            }

            while (i != 64)
            {
                if (sideviewIndexes[i] != 0)
                    sideview = true;
                else
                    sideview = false;
                ExportMaps.setValue(i, 63);

                tileLoader.getAnimations(roomIndexes[i], dungeonIndex, false, false);
                tileLoader.getSOG(roomIndexes[i], false);
                byte[, ,] data = tileLoader.loadTileset(dungeonIndex, roomIndexes[i], false, rCrystals.Checked, sideview);
                tileLoader.loadPallete(dungeonIndex, roomIndexes[i], false, sideView);
                TileLoader.Tile[] tilez = tileLoader.loadPaletteFlipIndexes(roomIndexes[i], dungeonIndex);
                srcB = tileLoader.drawTileset(data, tilez);
                FastPixel src = new FastPixel(srcB);
                src.rgbValues = new byte[256 * 256 * 4];
                src.Lock();

                dungeonDrawer.getCollisionDataDungeon(roomIndexes[i], dungeonIndex, false);
                dungeonDrawer.getWallsandFloor(dungeonIndex, roomIndexes[i], false);
                dungeonDrawer.loadCollisionsDungeon();
                bigmap = dungeonDrawer.data;

                if (xp == 8)
                {
                    yp++;
                    xp = 0;
                }
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        byte v = bigmap[x + (y * 10)];

                        for (int yy = 0; yy < 16; yy++)
                        {
                            for (int xx = 0; xx < 16; xx++)
                            {
                                fp.SetPixel((xp * 160) + (x * 16) + xx, (yp * 128) + (y * 16) + yy, src.GetPixel((v % 16) * 16 + xx, (v / 16) * 16 + yy));
                            }
                        }
                    }
                }
                i++;
                xp++;
                src.Unlock(true);
            }
            fp.Unlock(true);
            if (exportToFilename != "")
                b.Save(exportToFilename);
        }

        private void repointCollisionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pTiles.Image != null)
            {
                if (overLay && !cSprite.Checked)
                    return;
                byte[] backup = new byte[gb.Buffer.Length];
                int i = 0;
                foreach (byte b in gb.Buffer)
                {
                    backup[i] = b;
                    i++;
                }

                int address;
                List<Warps> warps;
                List<Object> objects;

                if (overWorld && !cSprite.Checked)
                {
                    address = overworldDrawer.mapAddress;
                    objects = overworldDrawer.objects;
                    warps = overworldDrawer.warps;

                }
                else if (!overWorld && !cSprite.Checked)
                {
                    address = dungeonDrawer.mapAddress;
                    objects = dungeonDrawer.objects;
                    warps = dungeonDrawer.warps;
                }
                else
                {
                    address = sprites.objectAddress;
                    objects = sprites.spriteList;
                    warps = null;
                }

                RepointCollision RC = new RepointCollision(backup,
                    overWorld, dungeonIndex, mapIndex, address,
                    cSpecialTiles.Checked, cMagGlass.Checked, cSprite.Checked,
                    warps, objects, wallTiles, floorTile);
                RC.ShowDialog();

                if (RC.DialogResult == DialogResult.OK)
                {
                    gb.Buffer = RC.gb.Buffer;
                    if (overWorld)
                    {
                        if (!cSprite.Checked)
                        {
                            overworldDrawer.loadCollisionsOverworld();

                            overworldDrawer.getCollisionDataOverworld(mapIndex, cSpecialTiles.Checked);
                            overworldDrawer.getFloor(mapIndex, cSpecialTiles.Checked);
                            usedspace = overworldDrawer.getUsedSpace();
                            freespace = overworldDrawer.getFreeSpace(mapIndex, cSpecialTiles.Checked);
                            drawOverworld();
                        }
                        else
                        {
                            sprites.loadObjects(true, dungeonIndex, mapIndex);
                            usedspace = sprites.getUsedSpace();
                            freespace = sprites.getFreeSpace(true, mapIndex, dungeonIndex);
                            drawOverworld();
                            setSpriteData();
                            drawSprites();
                        }
                    }
                    else
                    {
                        if (!cSprite.Checked)
                        {
                            dungeonDrawer.loadCollisionsDungeon();
                            dungeonDrawer.getWallsandFloor(dungeonIndex, mapIndex, cMagGlass.Checked);
                            dungeonDrawer.getCollisionDataDungeon(mapIndex, dungeonIndex, cMagGlass.Checked);
                            usedspace = dungeonDrawer.getUsedSpace();
                            freespace = dungeonDrawer.getFreeSpace(mapIndex, dungeonIndex, cMagGlass.Checked);
                            drawDungeon();
                        }
                        else
                        {
                            sprites.loadObjects(false, dungeonIndex, mapIndex);
                            usedspace = sprites.getUsedSpace();
                            freespace = sprites.getFreeSpace(true, mapIndex, dungeonIndex);
                            drawDungeon();
                            setSpriteData();
                            drawSprites();
                        }
                    }
                }
            }
        }

        private void pMap_MouseLeave(object sender, EventArgs e)
        {
            lastMapHoverPoint = new Point(-1, -1);
        }

        private void toolMinibossEditor_Click(object sender, EventArgs e)
        {
            if (pTiles.Image != null)
            {
                byte[] backup = new byte[gb.Buffer.Length];
                int i = 0;
                foreach (byte b in gb.Buffer)
                {
                    backup[i] = b;
                    i++;
                }

                MinibossEditor ME = new MinibossEditor(backup, overWorld, mapIndex);
                ME.ShowDialog();

                if (ME.DialogResult == DialogResult.OK)
                {
                    gb.Buffer = ME.gb.Buffer;
                }
            }
        }

        private void toolPaletteEditor_Click(object sender, EventArgs e)
        {
            if (pTiles.Image != null)
            {
                byte[] backup = new byte[gb.Buffer.Length];
                int i = 0;
                foreach (byte b in gb.Buffer)
                {
                    backup[i] = b;
                    i++;
                }

                PaletteEditor PE = new PaletteEditor(tileLoader, this, palette, backup, dungeonIndex, mapIndex, overWorld, cSideview.Checked, cSpecialTiles.Checked, rCrystals.Checked, palOffset);
                PE.ShowDialog();

                if (PE.DialogResult == DialogResult.OK)
                {
                    
                    palette = PE.palette;
                    tileLoader.palette = palette;
                    palOffset = PE.offset;
                    tileLoader.getAnimations((byte)mapIndex, (byte)dungeonIndex, overWorld, cSpecialTiles.Checked);
                    tileLoader.getSOG((byte)mapIndex, overWorld);
                    byte[, ,] data = tileLoader.loadTileset((byte)dungeonIndex, (byte)mapIndex, overWorld, rCrystals.Checked,  cSideview.Checked);
                    TileLoader.Tile[] tilez = tileLoader.loadPaletteFlipIndexes((byte)mapIndex, (byte)dungeonIndex);
                    pTiles.Image = tileLoader.drawTileset(data, tilez);
                    tileset = (Bitmap)pTiles.Image;

                    if (overWorld)
                        drawOverworld();
                    else
                        drawDungeon();
                }
            }
        }

        private void copyMapPaletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pTiles.Image != null)
            {
                Color[] colors = new Color[32];
                paletteCopy = new Color[8, 4];
                int q = 0;
                for (int k = 0; k < 8; k++)
                    for (int i = 0; i < 4; i++)
                    {
                        colors[q] = palette[k, i];
                        q++;
                    }
                int f = 0;
                q = 0;
                foreach (Color c in colors)
                {
                    if (q > 3)
                    {
                        f++;
                        q = 0;
                    }
                    paletteCopy[f, q] = c;
                    q++;
                }
            }
        }

        private void pasteMapPaletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pTiles.Image != null && paletteCopy != null)
            {
                palette = paletteCopy;
                tileLoader.palette = paletteCopy;
                tileLoader.getAnimations((byte)mapIndex, (byte)dungeonIndex, overWorld, cSpecialTiles.Checked);
                tileLoader.getSOG((byte)mapIndex, overWorld);
                byte[, ,] data = tileLoader.loadTileset((byte)dungeonIndex, (byte)mapIndex, overWorld, rCrystals.Checked, sideView);
                TileLoader.Tile[] tilez = tileLoader.loadPaletteFlipIndexes((byte)mapIndex, (byte)dungeonIndex);
                pTiles.Image = tileLoader.drawTileset(data, tilez);
                tileset = (Bitmap)pTiles.Image;

                if (overWorld)
                    drawOverworld();
                else
                    drawDungeon();
            }
        }

        private void forumsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = new System.Diagnostics.ProcessStartInfo("http://zeldahacking.net/");
            p.Start();
        }
    }
}
