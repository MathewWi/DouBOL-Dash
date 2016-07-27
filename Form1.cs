/*
    Copyright 2016 MrRean

    This file is part of DouBOLDash.
    DouBOLDash is free software: you can redistribute it and/or modify it under
    the terms of the GNU General Public License as published by the Free
    Software Foundation, either version 3 of the License, or (at your option)
    any later version.

    DouBOLDash is distributed in the hope that it will be useful, but WITHOUT ANY 
    WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
    FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    You should have received a copy of the GNU General Public License along 
    with DouBOLDash. If not, see http://www.gnu.org/licenses/.
*/
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
            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(bolFile, FileMode.Open)))
            {
                uint magic = reader.ReadUInt32(); // Magic (4 bytes) (THIS IS REQUIRED!!)

                // 0x30303135
                if (magic != 808464693)
                    MessageBox.Show("Invalid file.");
                else
                {
                    // this is here so when we get rejected, it doesn't change the name to a failed file
                    this.Text = "DouBOL Dash v0.1 ALPHA (" + Globals.bolFileStr + ")";
                    uint unk1 = reader.ReadUInt32(); // Unknown Value (4 bytes)
                    uint unk2 = reader.ReadUInt32(); // Unknown value (4 bytes)
                    float unkFloat1 = reader.ReadSingle(); // Unknown Float (4 bytes)
                    float unkFloat2 = reader.ReadSingle(); // Unknown Float (4 bytes)
                    float unkFloat3 = reader.ReadSingle(); // Unknown Float (4 bytes)
                    byte lapCount = reader.ReadByte(); // Lap Count (1 byte)
                    byte musicID = reader.ReadByte(); // Music ID (1 byte)
                    // make all of these globals, we'll use them later
                    Globals.enemyPointCount = reader.ReadUInt16(); // Enemy/Item Point Count (2 bytes)
                    Globals.checkpointGroupCount = reader.ReadUInt16(); // Checkpoint Group Count (2 bytes)
                    Globals.objCount = reader.ReadUInt16(); // Object Count (2 bytes)
                    Globals.areaCount = reader.ReadUInt16(); // Area Count (2 bytes)
                    Globals.cameraCount = reader.ReadUInt16(); // Camera Count (2 bytes)
                    Globals.routeCount = reader.ReadUInt16(); // Route Count (2 bytes)
                    Globals.respawnCount = reader.ReadUInt16(); // Respawn Count (2 Bytes)

                    uint unk3 = reader.ReadUInt16();
                    uint unk4 = reader.ReadUInt16();
                    float unkFloat4 = reader.ReadSingle();
                    float unkFloat5 = reader.ReadSingle();
                    // these are guesses...
                    uint unk5 = reader.ReadByte();
                    uint unk6 = reader.ReadUInt16();
                    uint unk7 = reader.ReadByte();
                    uint unk8 = reader.ReadUInt32();
                    uint unk9 = reader.ReadUInt16();
                    // always 0. padding?
                    reader.ReadUInt32();
                    reader.ReadUInt16();

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
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    reader.ReadUInt32();

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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /* Close the program */
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

        private void parseEnemyRoutes(int isNode)
        {
            int groupVar = 0;
            int groupNum = 0;

            dataGridView1.ColumnCount = 9;
            dataGridView1.Columns[0].Name = "Index";
            dataGridView1.Columns[1].Name = "X";
            dataGridView1.Columns[2].Name = "Y";
            dataGridView1.Columns[3].Name = "Z";
            dataGridView1.Columns[4].Name = "Scale";
            dataGridView1.Columns[5].Name = "Setting 0";
            dataGridView1.Columns[6].Name = "Setting 1";
            dataGridView1.Columns[7].Name = "Group Link";
            dataGridView1.Columns[8].Name = "Group Setting";

            /*
             * Enemy / Item Routes
            */
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();

            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(Globals.bolFileStr, FileMode.Open)))
            {
                reader.BaseStream.Seek(Globals.enemyOffset, 0);

                for (int i = 0; i <= Globals.enemyPointCount - 1; i++)
                {
                    float xpos = reader.ReadSingle();
                    float ypos = reader.ReadSingle();
                    float zpos = reader.ReadSingle();

                    uint set0 = reader.ReadUInt16();
                    int groupLink = reader.ReadInt16();

                    Console.WriteLine(groupLink.ToString() + "\n");

                    float scale = reader.ReadSingle();
                    int groupSetting = reader.ReadInt32();
                    uint set1 = reader.ReadUInt32();
                    reader.ReadUInt32();

                    // this makes it so the subnode doesn't create a node it shouldn't
                    if (isNode == 0)
                    {
                        // now we check if groupLink is not -1.
                        // The reason is because when it's not -1, it creates a new group
                        // so we create a group when this is entered
                        if (groupLink != -1)
                        {
                            // groupVar+1 each run. This makes it so we can cut the groups in half
                            // we'd have double the amount of groups we need, so only enter per 2 non -1 entries
                            ++groupVar;
                            if (groupVar % 2 == 0)
                            {
                                // add our node
                                string childNode;
                                childNode = "Group " + groupNum.ToString();
                                treeView1.SelectedNode.Nodes.Add(childNode);
                                ++groupNum;
                            }
                        }
                    }

                    string[] row = new string[] { "0", xpos.ToString(), ypos.ToString(), zpos.ToString(), scale.ToString(), set0.ToString(), set1.ToString(), groupLink.ToString(), groupSetting.ToString() };
                    dataGridView1.Rows.Add(row);
                }
                reader.Close();
            }
        }

        private void parseObjects()
        {
            dataGridView1.ColumnCount = 17;
            dataGridView1.Columns[0].Name = "Index";
            dataGridView1.Columns[1].Name = "Object Name";
            dataGridView1.Columns[2].Name = "X";
            dataGridView1.Columns[3].Name = "Y";
            dataGridView1.Columns[4].Name = "Z";
            dataGridView1.Columns[5].Name = "X Scale";
            dataGridView1.Columns[6].Name = "Y Scale";
            dataGridView1.Columns[7].Name = "Z Scale";
            dataGridView1.Columns[8].Name = "Rotation";
            dataGridView1.Columns[9].Name = "Object ID";
            dataGridView1.Columns[10].Name = "Route ID";
            dataGridView1.Columns[11].Name = "Setting 0";
            dataGridView1.Columns[12].Name = "Setting 1";
            dataGridView1.Columns[13].Name = "Setting 2";
            dataGridView1.Columns[14].Name = "Setting 3";
            dataGridView1.Columns[15].Name = "Setting 4";
            dataGridView1.Columns[16].Name = "Setting 5";

            /*
             * Objects
            */
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(Globals.bolFileStr, FileMode.Open)))
            {
                reader.BaseStream.Seek(Globals.objOffset, 0);

                for (int i = 0; i <= Globals.objCount - 1; i++)
                {

                    float xpos = reader.ReadSingle();
                    float ypos = reader.ReadSingle();
                    float zpos = reader.ReadSingle();

                    float xscale = reader.ReadSingle();
                    float yscale = reader.ReadSingle();
                    float zscale = reader.ReadSingle();

                    uint rotationX = reader.ReadUInt32();
                    uint rotationY = reader.ReadUInt32();
                    uint rotationZ = reader.ReadUInt32();

                    int rotation = RotationHax.returnRotations(rotationX, rotationY, rotationZ);

                    // objID is never negative so let's make it an int
                    int objID = reader.ReadInt16();
                    string objectName = ObjectNames.objectIDToString(objID);
                    int routeID = reader.ReadInt16();

                    uint set0 = reader.ReadUInt32();
                    uint set1 = reader.ReadUInt32();
                    uint set2 = reader.ReadUInt32();
                    uint set3 = reader.ReadUInt32();
                    uint set4 = reader.ReadUInt32();
                    uint set5 = reader.ReadUInt32();

                    string set0Out = set0.ToString("X");
                    string set1Out = set1.ToString("X");
                    string set2Out = set2.ToString("X");
                    string set3Out = set3.ToString("X");
                    string set4Out = set4.ToString("X");
                    string set5Out = set5.ToString("X");

                    string[] row = new string[] { i.ToString(), objectName, xpos.ToString(), ypos.ToString(), zpos.ToString(), xscale.ToString(), yscale.ToString(), zscale.ToString(), rotation.ToString(), objID.ToString(), routeID.ToString(), set0Out, set1Out, set2Out, set3Out, set4Out, set5Out };
                    dataGridView1.Rows.Add(row);
                }
                reader.Close();
            }
        }

        private void parseStartingPoints(int isMultiplayer)
        {
            dataGridView1.ColumnCount = 10;
            dataGridView1.Columns[0].Name = "Index";
            dataGridView1.Columns[1].Name = "X";
            dataGridView1.Columns[2].Name = "Y";
            dataGridView1.Columns[3].Name = "Z";
            dataGridView1.Columns[4].Name = "X Scale";
            dataGridView1.Columns[5].Name = "Y Scale";
            dataGridView1.Columns[6].Name = "Z Scale";
            dataGridView1.Columns[7].Name = "Rotation";
            dataGridView1.Columns[8].Name = "Pole";
            dataGridView1.Columns[9].Name = "Player ID";

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
                    float xpos = reader.ReadSingle();
                    float ypos = reader.ReadSingle();
                    float zpos = reader.ReadSingle();

                    float xscale = reader.ReadSingle();
                    float yscale = reader.ReadSingle();
                    float zscale = reader.ReadSingle();

                    uint rotationXZ = reader.ReadUInt32();
                    uint rotationYZ = reader.ReadUInt32();
                    uint rotationZZ = reader.ReadUInt32();

                    int rotation = RotationHax.returnRotations(rotationXZ, rotationYZ, rotationZZ);

                    int pole = reader.ReadByte();
                    int playerID = reader.ReadByte();

                    reader.ReadUInt16(); // last 2 bytes are padding

                    string[] row = new string[] { "0", xpos.ToString(), ypos.ToString(), zpos.ToString(), xscale.ToString(), yscale.ToString(), zscale.ToString(), rotation.ToString(), pole.ToString(), playerID.ToString() };
                    dataGridView1.Rows.Add(row);
                }
            reader.Close();
            }
        }

        private void parseAreas()
        {
            dataGridView1.ColumnCount = 12;
            dataGridView1.Columns[0].Name = "Index";
            dataGridView1.Columns[1].Name = "X";
            dataGridView1.Columns[2].Name = "Y";
            dataGridView1.Columns[3].Name = "Z";
            dataGridView1.Columns[4].Name = "X Scale";
            dataGridView1.Columns[5].Name = "Y Scale";
            dataGridView1.Columns[6].Name = "Z Scale";
            dataGridView1.Columns[7].Name = "Rotation";
            dataGridView1.Columns[8].Name = "Unknown 0";
            dataGridView1.Columns[9].Name = "Unknown 1";
            dataGridView1.Columns[10].Name = "Unknown 2";
            dataGridView1.Columns[11].Name = "Unknown 3";

            /*
             * Areas
            */
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(Globals.bolFileStr, FileMode.Open)))
            {
                reader.BaseStream.Seek(Globals.areaOffset, 0);

                for (int i = 0; i <= Globals.areaCount - 1; i++)
                {
                    float xpos = reader.ReadSingle();
                    float ypos = reader.ReadSingle();
                    float zpos = reader.ReadSingle();

                    float xscale = reader.ReadSingle();
                    float yscale = reader.ReadSingle();
                    float zscale = reader.ReadSingle();

                    uint rotationX = reader.ReadUInt32();
                    uint rotationY = reader.ReadUInt32();
                    uint rotationZ = reader.ReadUInt32();

                    int rotation = RotationHax.returnRotations(rotationX, rotationY, rotationZ);

                    uint unk1 = reader.ReadUInt16();
                    uint unk2 = reader.ReadUInt16();
                    ulong unk3 = reader.ReadUInt64();
                    ulong unk4 = reader.ReadUInt64();

                    string unk3Out = unk3.ToString("X");
                    string unk4Out = unk4.ToString("X");

                    string[] row = new string[] { i.ToString(), xpos.ToString(), ypos.ToString(), zpos.ToString(), xscale.ToString(), yscale.ToString(), zscale.ToString(), rotation.ToString(), unk1.ToString(), unk2.ToString(), unk3Out, unk4Out };
                    dataGridView1.Rows.Add(row); 
                }
            reader.Close();
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Enemy / Item Route Root Nodes
            if (treeView1.SelectedNode.Index == 0 && treeView1.SelectedNode.Parent == null)
                parseEnemyRoutes(0);
            // Enemy / Item Route subnodes
            else if (treeView1.SelectedNode.Index == 0 && treeView1.SelectedNode.Parent != null)
                parseEnemyRoutes(1);
            // Objects
            else if (treeView1.SelectedNode.Index == 3)
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
            // Areas
            else if (treeView1.SelectedNode.Index == 5)
                parseAreas();
            // Other we haven't parsed yet (or something odd landed in here)
            else
                MessageBox.Show("Not supported yet.");
        }

        public void saveBOL(String bolFile)
        {
            byte musicID;

            using (EndianBinaryWriter writer = new EndianBinaryWriter(File.Open(bolFile, FileMode.Create)))
            {
                /* We begin building the file with the header, of course. */
                float unkFloat1 = float.Parse(unkFltC.Text);
                float unkFloat2 = float.Parse(unkFlt10.Text);
                float unkFloat3 = float.Parse(unkFlt14.Text);

                uint unk4 = UInt32.Parse(unk4Input.Text);
                uint unk8 = UInt32.Parse(unk8Input.Text);
                byte lapCount = Convert.ToByte(lapCountInput.Text);

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
                    uint magic = 0x30303135;
                    writer.Write(magic); // Magic (4 bytes)
                    writer.Write(unk4); // Unknown (4 bytes)
                    writer.Write(unk8); // Unknown (4 bytes)
                    writer.Write(unkFloat1); // Unknown Float (4 bytes)
                    writer.Write(unkFloat2); // Unknown Float (4 bytes)
                    writer.Write(unkFloat3); // Unknown Float (4 bytes)
                    writer.Write(lapCount); // Lap Count (1 byte)
                    writer.Write(musicID); // Music ID (1 byte)

                    /* Objects */
                    // object count
                    writer.BaseStream.Seek(0x1E, 0);
                    writer.Write(Globals.objCount);
                    // object offset
                    writer.BaseStream.Seek(0x54, 0);
                    writer.Write(Globals.objOffset);

                    // we jump to the offset with the objects
                    writer.BaseStream.Seek(Globals.objOffset, 0);

                    // we forcefully select the Objects node to dump the data
                    TreeNode treeNode = treeView1.Nodes[3];
                    treeView1.SelectedNode = treeNode;
                    Console.Write(treeView1.SelectedNode.ToString() + " \n");
                    treeView1.Focus();

                    Console.WriteLine(dataGridView1.Rows.Count.ToString() + " \n");

                    // go through each DataGrid row and dump the data.
                    // we don't dump 0 or 1 because they're just index & name
                    // both don't get inserted back into the game
                    for (int row = 0; row < dataGridView1.Rows.Count; row++)
                    {
                        string xpos = dataGridView1.Rows[row].Cells[2].Value.ToString();
                        string ypos = dataGridView1.Rows[row].Cells[3].Value.ToString();
                        string zpos = dataGridView1.Rows[row].Cells[4].Value.ToString();
                        string xscale = dataGridView1.Rows[row].Cells[5].Value.ToString();
                        string yscale = dataGridView1.Rows[row].Cells[6].Value.ToString();
                        string zscale = dataGridView1.Rows[row].Cells[7].Value.ToString();
                        string rotation = dataGridView1.Rows[row].Cells[8].Value.ToString();
                        string obj = dataGridView1.Rows[row].Cells[9].Value.ToString();
                        string route = dataGridView1.Rows[row].Cells[10].Value.ToString();
                        string set0 = dataGridView1.Rows[row].Cells[11].Value.ToString();
                        string set1 = dataGridView1.Rows[row].Cells[12].Value.ToString();
                        string set2 = dataGridView1.Rows[row].Cells[13].Value.ToString();
                        string set3 = dataGridView1.Rows[row].Cells[14].Value.ToString();
                        string set4 = dataGridView1.Rows[row].Cells[15].Value.ToString();
                        string set5 = dataGridView1.Rows[row].Cells[16].Value.ToString();

                        float xposFloat = Convert.ToSingle(xpos); // 4
                        float yposFloat = Convert.ToSingle(ypos); // 4
                        float zposFloat = Convert.ToSingle(zpos); // 4
                        float xscaleFloat = Convert.ToSingle(xscale); // 4
                        float yscaleFloat = Convert.ToSingle(yscale); // 4
                        float zscaleFloat = Convert.ToSingle(zscale); // 4
                        uint rotations = Convert.ToUInt32(rotation); // 4
                        uint unk1 = Convert.ToUInt32(655360000); // 4
                        uint unk2 = Convert.ToUInt32(655360000); // 4
                        short objectID = Convert.ToInt16(obj, 16); // 2
                        short routeID = Convert.ToInt16(route); // 2
                        int set0l = Convert.ToInt32(set0, 16); // 4
                        int set1l = Convert.ToInt32(set1, 16); // 4
                        int set2l = Convert.ToInt32(set2, 16); // 4
                        int set3l = Convert.ToInt32(set3, 16); // 4
                        int set4l = Convert.ToInt32(set4, 16); // 4
                        int set5l = Convert.ToInt32(set5, 16); // 4

                        writer.Write(xposFloat); // 4
                        writer.Write(yposFloat); // 4
                        writer.Write(zposFloat); // 4
                        writer.Write(xscaleFloat); // 4
                        writer.Write(yscaleFloat); // 4
                        writer.Write(zscaleFloat); // 4
                        writer.Write(rotations); // 4
                        writer.Write(unk1); // 4
                        writer.Write(unk2); // 4
                        writer.Write(objectID); // 2
                        writer.Write(routeID); // 2
                        writer.Write(set0l); // 4
                        writer.Write(set1l); // 4
                        writer.Write(set2l); // 4
                        writer.Write(set3l); // 4
                        writer.Write(set4l); // 4
                        writer.Write(set5l); // 4
                    }
                    // close the file
                    writer.Close();
                }
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.Show();
        }
    }
}