using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DouBOLDash
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            // We add all of the possible music IDs here. There's a few battle ones that I haven't documented
            InitializeComponent();
            listMusicIDS.Items.Add("Baby Park");
            listMusicIDS.Items.Add("Peach Beach");
            listMusicIDS.Items.Add("Daisy Cruiser");
            listMusicIDS.Items.Add("Luigi Circuit");
            listMusicIDS.Items.Add("Mario Circuit");
            listMusicIDS.Items.Add("Yoshi Circuit");
            listMusicIDS.Items.Add("Mushroom Bridge");
            listMusicIDS.Items.Add("Mushroom City");
            listMusicIDS.Items.Add("Waluigi Stadium");
            listMusicIDS.Items.Add("Wario Colosseum");
            listMusicIDS.Items.Add("Dino Dino Jungle");
            listMusicIDS.Items.Add("DK Mountain");
            listMusicIDS.Items.Add("Bowser's Castle");
            listMusicIDS.Items.Add("Rainbow Road");
            listMusicIDS.Items.Add("Dry Dry Desert");
            listMusicIDS.Items.Add("Sherbet Land");
            listMusicIDS.Items.Add("Unknown");
        }

        /*
         * Class for defining globals that will be accessable from anywhere
         */
        public static class Globals
        {
            public static String bolFileStr;
            public static uint enemyPointCount, checkpointGroupCount, areaCount, respawnCount, cameraCount, routeCount, objCount;
            public static uint enemyOffset, checkpointOffset, routeListOffset, routePointOffset, objOffset, startingPointOffset, areaOffset, cameraOffset, respawnOffset, unkOffset, readFileSize;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // set up our file dialog and set the file to be opened to BOL
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "BOL Files (*.bol)|*.bol|All files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Globals.bolFileStr = openFileDialog1.FileName;
                try
                {
                    parseBOL(Globals.bolFileStr);
                    this.Text = "DouBOL Dash " + Globals.bolFileStr;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured while opening the file " + Globals.bolFileStr + "\n Exception Thrown: \n" + ex, "An Error Occured");
                }
            }
        }

        /* 
         * Where the BOL is actually parsed. The bytes are fed into an EndianBinaryReader. 
         * This isn't the usual BinaryReader because this one is in Big Endian, while the BinaryReader is little.
         * We give the reader the values, starting at 0x00 and go through the entire file.
         */
        public void parseBOL(String bolFile)
        {
            uint magic, unk1, unk2, unk3, unk4, unk5, unk6, unk7, unk8, unk9, pad1, pad2, pad3, pad4, pad5, lapCount, musicID;
            float unkFloat1, unkFloat2, unkFloat3, unkFloat4, unkFloat5;
            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(bolFile, FileMode.Open)))
            {
                magic = reader.ReadUInt32(); // Magic (4 bytes)
                unk1 = reader.ReadUInt32(); // Unknown Value (4 bytes)
                unk2 = reader.ReadUInt32(); // Unknown value (4 bytes)
                unkFloat1 = reader.ReadSingle(); // Unknown Float (4 bytes)
                unkFloat2 = reader.ReadSingle(); // Unknown Float (4 bytes)
                unkFloat3 = reader.ReadSingle(); // Unknown Float (4 bytes)
                lapCount = reader.ReadByte(); // Lap Count (1 byte)
                musicID = reader.ReadByte(); // Music ID (1 byte)
                Globals.enemyPointCount = reader.ReadUInt16();
                Globals.checkpointGroupCount = reader.ReadUInt16();
                Globals.objCount = reader.ReadUInt16();
                Globals.areaCount = reader.ReadUInt16();
                Globals.cameraCount = reader.ReadUInt16();
                Globals.routeCount = reader.ReadUInt16();
                Globals.respawnCount = reader.ReadUInt16();

                unk3 = reader.ReadUInt16();
                unk4 = reader.ReadUInt16();
                unkFloat4 = reader.ReadSingle();
                unkFloat5 = reader.ReadSingle();
                // these are guesses...
                unk5 = reader.ReadByte();
                unk6 = reader.ReadUInt16();
                unk7 = reader.ReadByte();
                unk8 = reader.ReadUInt32();
                unk9 = reader.ReadUInt16();
                // always 0. padding?
                pad1 = reader.ReadUInt32();
                pad2 = reader.ReadUInt16();

                // these are VERY important
                Globals.enemyOffset = reader.ReadUInt32();
                Globals.checkpointOffset = reader.ReadUInt32();
                Globals.routeListOffset = reader.ReadUInt32();
                Globals.routePointOffset = reader.ReadUInt32();
                Globals.objOffset = reader.ReadUInt32();
                Globals.startingPointOffset = reader.ReadUInt32();
                Globals.areaOffset = reader.ReadUInt32();
                Globals.cameraOffset = reader.ReadUInt32();
                Globals.respawnOffset = reader.ReadUInt32();
                Globals.unkOffset = reader.ReadUInt32();
                Globals.readFileSize = reader.ReadUInt32();
                pad3 = reader.ReadUInt32();
                pad4 = reader.ReadUInt32();
                pad5 = reader.ReadUInt32();

                /*
                 * SECTION 1 -- ENEMY / ITEM POINTS (EIPT)
                */
                // TODO -- DO THIS

                // begin very ugly check
                // TODO -- do this better
                if (musicID == 0x21)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Baby Park");
                else if (musicID == 0x22)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Peach Beach");
                else if (musicID == 0x23)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Daisy Cruiser");
                else if (musicID == 0x24)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Luigi Circuit");
                else if (musicID == 0x25)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Mario Circuit");
                else if (musicID == 0x26)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Yoshi Circuit");
                else if (musicID == 0x27)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Unknown");
                else if (musicID == 0x28)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Mushroom Bridge");
                else if (musicID == 0x29)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Mushroom City");
                else if (musicID == 0x2A)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Waluigi Stadium");
                else if (musicID == 0x2B)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Wario Colosseum");
                else if (musicID == 0x2C)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Dino Dino Jungle");
                else if (musicID == 0x2D)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("DK Mountain");
                else if (musicID == 0x2E)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Unknown");
                else if (musicID == 0x2F)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Bowser's Castle");
                else if (musicID == 0x30)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Unknown");
                else if (musicID == 0x31)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Rainbow Road");
                else if (musicID == 0x32)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Dry Dry Desert");
                else if (musicID == 0x33)
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Sherbet Land");
                else
                    listMusicIDS.SelectedIndex = listMusicIDS.FindStringExact("Unknown");

                // Unsigned int (or float) -> string conversion for text boxes
                unk4Input.Text = unk1.ToString();
                unk8Input.Text = unk2.ToString();
                unkFltC.Text = unkFloat1.ToString();
                unkFlt10.Text = unkFloat2.ToString();
                unkFlt14.Text = unkFloat3.ToString();
                lapCountInput.Text = lapCount.ToString();

                /* 
                * All controls except for open were disabled upon program start / or when a file is not loaded.
                * This is because we could see some odd results if somebody inputs something into the boxes before 
                * the program inserts the course data.
                */
                unk4Input.Enabled = true;
                unk8Input.Enabled = true;
                unkFltC.Enabled = true;
                unkFlt10.Enabled = true;
                unkFlt14.Enabled = true;
                lapCountInput.Enabled = true;
                listMusicIDS.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                treeView1.Enabled = true;

                // We're done with this reader.
                reader.Close();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveBOL(Globals.bolFileStr);
            }
            catch (IOException)
            {
                MessageBox.Show("An error occured while saving the file.", "An Error Occured");
            }
        }

        public void saveBOL(String bolFile)
        {
            uint magic, unk4, unk8;
            float unkFloat1, unkFloat2, unkFloat3;
            byte lapCount, musicID;

            using (EndianBinaryWriter writer = new EndianBinaryWriter(File.Open(bolFile, FileMode.Create)))
            {
                unkFloat1 = float.Parse(unkFltC.Text);
                unkFloat2 = float.Parse(unkFlt10.Text);
                unkFloat3 = float.Parse(unkFlt14.Text);

                unk4 = UInt32.Parse(unk4Input.Text);
                unk8 = UInt32.Parse(unk8Input.Text);
                lapCount = Convert.ToByte(lapCountInput.Text);

                if (listMusicIDS.SelectedIndex == 0)
                    musicID = 0x21;
                else if (listMusicIDS.SelectedIndex == 1)
                    musicID = 0x22;
                else if (listMusicIDS.SelectedIndex == 2)
                    musicID = 0x23;
                else if (listMusicIDS.SelectedIndex == 3)
                    musicID = 0x24;
                else if (listMusicIDS.SelectedIndex == 4)
                    musicID = 0x25;
                else if (listMusicIDS.SelectedIndex == 5)
                    musicID = 0x26;
                else if (listMusicIDS.SelectedIndex == 6)
                    musicID = 0x27;
                else if (listMusicIDS.SelectedIndex == 7)
                    musicID = 0x28;
                else if (listMusicIDS.SelectedIndex == 8)
                    musicID = 0x29;
                else if (listMusicIDS.SelectedIndex == 9)
                    musicID = 0x2A;
                else if (listMusicIDS.SelectedIndex == 10)
                    musicID = 0x2B;
                else if (listMusicIDS.SelectedIndex == 11)
                    musicID = 0x2C;
                else if (listMusicIDS.SelectedIndex == 12)
                    musicID = 0x2D;
                else if (listMusicIDS.SelectedIndex == 13)
                    musicID = 0x2E;
                else if (listMusicIDS.SelectedIndex == 14)
                    musicID = 0x2F;
                else if (listMusicIDS.SelectedIndex == 15)
                    musicID = 0x30;
                else if (listMusicIDS.SelectedIndex == 16)
                    musicID = 0x31;
                else if (listMusicIDS.SelectedIndex == 17)
                    musicID = 0x32;
                else if (listMusicIDS.SelectedIndex == 18)
                    musicID = 0x33;
                else
                    musicID = 0x21;

                // check for invalid lap count
                if (lapCount < 1 || lapCount > 9)
                {
                    lapCountInput.BackColor = Color.Teal;
                    MessageBox.Show("Invalid Lap Count. Needs to be a value between 1 and 9.", "Invalid Value");
                }
                else
                {
                    // we need to change it back to white
                    lapCountInput.BackColor = Color.White;
                    magic = 0x30303135;
                    writer.Write(magic); // Magic (4 bytes)
                    writer.Write(unk4); // Unknown (4 bytes)
                    writer.Write(unk8); // Unknown (4 bytes)
                    writer.Write(unkFloat1); // Unknown Float (4 bytes)
                    writer.Write(unkFloat2); // Unknown Float (4 bytes)
                    writer.Write(unkFloat3); // Unknown Float (4 bytes)
                    writer.Write(lapCount); // Lap Count (1 byte)
                    writer.Write(musicID); // Music ID (1 byte)

                    // close the file
                    writer.Close();
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "BOL Files (*.bol)|*.bol";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string bolStr = saveFileDialog1.FileName;
                try
                {
                    saveBOL(bolStr);
                }
                catch (IOException)
                {
                    MessageBox.Show("An error occured while saving the file.", "An Error Occured");
                }
            }
        }

        private void parseObjects()
        {
            float xpos, ypos, zpos, xscale, yscale, zscale, rotation;
            uint objUnk1, objUnk2, objID, set0, set1, set2, set3, set4, set5;
            int routeID;

            dataGridView1.ColumnCount = 16;
            dataGridView1.Columns[0].Name = "Index";
            dataGridView1.Columns[1].Name = "X";
            dataGridView1.Columns[2].Name = "Y";
            dataGridView1.Columns[3].Name = "Z";
            dataGridView1.Columns[4].Name = "X Scale";
            dataGridView1.Columns[5].Name = "Y Scale";
            dataGridView1.Columns[6].Name = "Z Scale";
            dataGridView1.Columns[7].Name = "Rotation";
            dataGridView1.Columns[8].Name = "Object ID";
            dataGridView1.Columns[9].Name = "Route ID";
            dataGridView1.Columns[10].Name = "Setting 0";
            dataGridView1.Columns[11].Name = "Setting 1";
            dataGridView1.Columns[12].Name = "Setting 2";
            dataGridView1.Columns[13].Name = "Setting 3";
            dataGridView1.Columns[14].Name = "Setting 4";
            dataGridView1.Columns[15].Name = "Setting 5";

            /*
             * SECTION WHOKNOWS -- OBJECTS
            */
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(Globals.bolFileStr, FileMode.Open)))
            {
                reader.BaseStream.Seek(Globals.objOffset, 0);

                for (int i = 0; i <= Globals.objCount; i++)
                {
                    xpos = reader.ReadSingle();
                    ypos = reader.ReadSingle();
                    zpos = reader.ReadSingle();

                    xscale = reader.ReadSingle();
                    yscale = reader.ReadSingle();
                    zscale = reader.ReadSingle();

                    rotation = reader.ReadSingle();

                    objUnk1 = reader.ReadUInt32();
                    objUnk2 = reader.ReadUInt32();

                    objID = reader.ReadUInt16();
                    routeID = reader.ReadInt16();

                    set0 = reader.ReadUInt32();
                    set1 = reader.ReadUInt32();
                    set2 = reader.ReadUInt32();
                    set3 = reader.ReadUInt32();
                    set4 = reader.ReadUInt32();
                    set5 = reader.ReadUInt32();

                    string set0Out = set0.ToString("X");
                    string set1Out = set1.ToString("X");
                    string set2Out = set2.ToString("X");
                    string set3Out = set3.ToString("X");
                    string set4Out = set4.ToString("X");
                    string set5Out = set5.ToString("X");

                    string[] row = new string[] { i.ToString(), xpos.ToString(), ypos.ToString(), zpos.ToString(), xscale.ToString(), yscale.ToString(), zscale.ToString(), rotation.ToString(), objID.ToString(), routeID.ToString(), set0Out, set1Out, set2Out, set3Out, set4Out, set5Out };
                    dataGridView1.Rows.Add(row);
                }
                reader.Close();
            }
        }

        private void parseStartingPoints(int isMultiplayer)
        {
            float xpos, ypos, zpos, xscale, yscale, zscale;
            int playerID, pole;
            uint rotationX, rotationY, rotationZ;

            dataGridView1.ColumnCount = 12;
            dataGridView1.Columns[0].Name = "Index";
            dataGridView1.Columns[1].Name = "X";
            dataGridView1.Columns[2].Name = "Y";
            dataGridView1.Columns[3].Name = "Z";
            dataGridView1.Columns[4].Name = "X Scale";
            dataGridView1.Columns[5].Name = "Y Scale";
            dataGridView1.Columns[6].Name = "Z Scale";
            dataGridView1.Columns[7].Name = "Rotation X";
            dataGridView1.Columns[8].Name = "Rotation Y";
            dataGridView1.Columns[9].Name = "Rotation Z";
            dataGridView1.Columns[10].Name = "Pole";
            dataGridView1.Columns[11].Name = "Player ID";

            int numPoints;
            if (isMultiplayer == 1)
                numPoints = 4;
            else
                numPoints = 1;

            /*
             * Respawns
            */
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(Globals.bolFileStr, FileMode.Open)))
            {
                for (int i = 1; i <= numPoints; i++)
                {
                    reader.BaseStream.Seek(Globals.startingPointOffset, 0);
                    // there's only usually 1 entrance.
                    // plus there's no value for it in the hex file :v
                    xpos = reader.ReadSingle();
                    ypos = reader.ReadSingle();
                    zpos = reader.ReadSingle();

                    xscale = reader.ReadSingle();
                    yscale = reader.ReadSingle();
                    zscale = reader.ReadSingle();

                    uint rotationXZ, rotationYZ, rotationZZ;

                    rotationXZ = reader.ReadUInt32();
                    rotationYZ = reader.ReadUInt32();
                    rotationZZ = reader.ReadUInt32();

                    rotationX = ~rotationXZ;
                    rotationY = ~rotationYZ;
                    rotationZ = ~rotationZZ;

                    /* 
                     * the 2nd set seems to be padding.
                     * this is also in other parts...
                    */

                    pole = reader.ReadByte();
                    playerID = reader.ReadByte();

                    reader.ReadUInt16(); // last 2 bytes are padding

                    string[] row = new string[] { "0", xpos.ToString(), ypos.ToString(), zpos.ToString(), xscale.ToString(), yscale.ToString(), zscale.ToString(), rotationX.ToString(), rotationY.ToString(), rotationZ.ToString(), pole.ToString(), playerID.ToString() };
                    dataGridView1.Rows.Add(row);
                }
            reader.Close();
            }
        }

        private void parseAreas()
        {
            float xpos, ypos, zpos, xscale, yscale, zscale;
            uint unk1, unk2;
            ulong unk3, unk4;
            uint rotationX, rotationY, rotationZ;

            dataGridView1.ColumnCount = 14;
            dataGridView1.Columns[0].Name = "Index";
            dataGridView1.Columns[1].Name = "X";
            dataGridView1.Columns[2].Name = "Y";
            dataGridView1.Columns[3].Name = "Z";
            dataGridView1.Columns[4].Name = "X Scale";
            dataGridView1.Columns[5].Name = "Y Scale";
            dataGridView1.Columns[6].Name = "Z Scale";
            dataGridView1.Columns[7].Name = "Rotation X";
            dataGridView1.Columns[8].Name = "Rotation Y";
            dataGridView1.Columns[9].Name = "Rotation Z";
            dataGridView1.Columns[10].Name = "Unknown 0";
            dataGridView1.Columns[11].Name = "Unknown 1";
            dataGridView1.Columns[12].Name = "Unknown 2";
            dataGridView1.Columns[13].Name = "Unknown 3";

            /*
             * Respawns
            */
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(Globals.bolFileStr, FileMode.Open)))
            {
                reader.BaseStream.Seek(Globals.areaOffset, 0);

                for (int i = 0; i <= Globals.areaCount - 1; i++)
                {
                    xpos = reader.ReadSingle();
                    ypos = reader.ReadSingle();
                    zpos = reader.ReadSingle();

                    xscale = reader.ReadSingle();
                    yscale = reader.ReadSingle();
                    zscale = reader.ReadSingle();

                    rotationX = reader.ReadUInt32();
                    rotationY = reader.ReadUInt32();
                    rotationZ = reader.ReadUInt32();

                    unk1 = reader.ReadUInt16();
                    unk2 = reader.ReadUInt16();
                    unk3 = reader.ReadUInt64();
                    unk4 = reader.ReadUInt64();

                    string unk3Out = unk3.ToString("X");
                    string unk4Out = unk4.ToString("X");

                    string[] row = new string[] { i.ToString(), xpos.ToString(), ypos.ToString(), zpos.ToString(), xscale.ToString(), yscale.ToString(), zscale.ToString(), rotationX.ToString(), rotationY.ToString(), rotationZ.ToString(), unk1.ToString(), unk2.ToString(), unk3Out, unk4Out };
                    dataGridView1.Rows.Add(row); 
                }
            reader.Close();
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeView1.SelectedNode.Index == 3)
                parseObjects();
            // here's the thing about starting points.
            // They don't have a set count for how many there are. 
            // So we have to ask the user if this track is a battle stage.
            // Battle stages have 4 starts. Regular has 1.
            else if (treeView1.SelectedNode.Index == 4)
            {
                DialogResult dialogResult = MessageBox.Show("Is this a battle map?", "Some Title", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    parseStartingPoints(1);
                }
                else if (dialogResult == DialogResult.No)
                {
                    parseStartingPoints(0);
                }
            }
            else if (treeView1.SelectedNode.Index == 5)
                parseAreas();
            else
                MessageBox.Show("Not supported yet.");
        }
    }
}