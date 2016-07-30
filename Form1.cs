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
            public static uint enemyPointCount, checkpointGroupCount, checkpointCount, areaCount, respawnCount, cameraCount, routeCount, objCount;
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
                    /* we parse these section by section. */
                    parseBOL(Globals.bolFileStr);
                    parseEnemyRoutes();
                    parseCheckpointSettings();
                    parseCheckpoints();
                    parseRouteSettings();
                    parseRoutes();
                    parseObjects();
                    /* if they say yes we have to parse 3 starting points instead of 1 */
                    DialogResult dialogResult = MessageBox.Show("Is this a battle map?", "Some Title", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        parseStartingPoints(1);
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        parseStartingPoints(0);
                    }
                    parseAreas();
                    parseCameras();
                    parseRespawns();
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

        private void parseEnemyRoutes()
        {
            /*
             * Enemy / Item Routes
            */
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
             
                    float scale = reader.ReadSingle();
                    int groupSetting = reader.ReadInt32();
                    uint set1 = reader.ReadUInt32();
                    reader.ReadUInt32();

                    enemyGrid.ColumnCount = 9;
                    enemyGrid.Columns[0].Name = "Index";
                    enemyGrid.Columns[1].Name = "X";
                    enemyGrid.Columns[2].Name = "Y";
                    enemyGrid.Columns[3].Name = "Z";
                    enemyGrid.Columns[4].Name = "Scale";
                    enemyGrid.Columns[5].Name = "Setting 0";
                    enemyGrid.Columns[6].Name = "Setting 1";
                    enemyGrid.Columns[7].Name = "Group Link";
                    enemyGrid.Columns[8].Name = "Group Setting";

                    string[] rowObject = new string[] { i.ToString(), xpos.ToString(), ypos.ToString(), zpos.ToString(), scale.ToString(), set0.ToString(), set1.ToString(), groupLink.ToString(), groupSetting.ToString() };
                    enemyGrid.Rows.Add(rowObject);
                }
                reader.Close();
            }
        }

        private void parseCheckpointSettings()
        {
            checkpointInfoGrid.ColumnCount = 11;
            checkpointInfoGrid.Columns[0].Name = "Index";
            checkpointInfoGrid.Columns[1].Name = "Point Length";
            checkpointInfoGrid.Columns[2].Name = "Group Link";
            checkpointInfoGrid.Columns[3].Name = "Previous 0";
            checkpointInfoGrid.Columns[4].Name = "Previous 1";
            checkpointInfoGrid.Columns[5].Name = "Previous 2";
            checkpointInfoGrid.Columns[6].Name = "Previous 3";
            checkpointInfoGrid.Columns[7].Name = "Next 0";
            checkpointInfoGrid.Columns[8].Name = "Next 1";
            checkpointInfoGrid.Columns[9].Name = "Next 2";
            checkpointInfoGrid.Columns[10].Name = "Next 3";

            /*
             * Checkpoint Settings (to complete dem laps man)
            */
            checkpointInfoGrid.Rows.Clear();
            checkpointInfoGrid.Refresh();

            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(Globals.bolFileStr, FileMode.Open)))
            {
                reader.BaseStream.Seek(Globals.checkpointOffset, 0);

                Globals.checkpointCount = reader.ReadUInt16();
                int groupLink = reader.ReadInt16();
                short previous0 = reader.ReadInt16();
                short previous1 = reader.ReadInt16();
                short previous2 = reader.ReadInt16();
                short previous3 = reader.ReadInt16();
                short next0 = reader.ReadInt16();
                short next1 = reader.ReadInt16();
                short next2 = reader.ReadInt16();
                short next3 = reader.ReadInt16();

                string[] row = new string[] { "0", Globals.checkpointCount.ToString(), groupLink.ToString(), previous0.ToString(), previous1.ToString(), previous2.ToString(), previous3.ToString(), next0.ToString(), next1.ToString(), next2.ToString(), next3.ToString() };
                checkpointInfoGrid.Rows.Add(row);

                reader.Close();
            }
        }

        private void parseCheckpoints()
        {
            checkpointListGrid.ColumnCount = 7;
            checkpointListGrid.Columns[0].Name = "Index";
            checkpointListGrid.Columns[1].Name = "X1";
            checkpointListGrid.Columns[2].Name = "Y1";
            checkpointListGrid.Columns[3].Name = "Z1";
            checkpointListGrid.Columns[4].Name = "X2";
            checkpointListGrid.Columns[5].Name = "Y2";
            checkpointListGrid.Columns[6].Name = "Z2";

            /*
             * Checkpoints (to complete dem laps man, for real now srs)
            */
            checkpointListGrid.Rows.Clear();
            checkpointListGrid.Refresh();

            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(Globals.bolFileStr, FileMode.Open)))
            {
                reader.BaseStream.Seek(Globals.checkpointOffset + 20, 0);

                // if Globals.checkpointCount is not defined yet...uh oh
                for (int i = 0; i <= Globals.checkpointCount - 1; i++)
                {
                    float x1 = reader.ReadSingle();
                    float y1 = reader.ReadSingle();
                    float z1 = reader.ReadSingle();
                    float x2 = reader.ReadSingle();
                    float y2 = reader.ReadSingle();
                    float z2 = reader.ReadSingle();
                    reader.ReadUInt32();

                    string[] row = new string[] { i.ToString(), x1.ToString(), y1.ToString(), z1.ToString(), x2.ToString(), y2.ToString(), z2.ToString() };
                    checkpointListGrid.Rows.Add(row);
                }

                reader.Close();
            }
        }

        private void parseRouteSettings()
        {
            routeSettingsGrid.ColumnCount = 3;
            routeSettingsGrid.Columns[0].Name = "Index";
            routeSettingsGrid.Columns[1].Name = "Point Length";
            routeSettingsGrid.Columns[2].Name = "Point Starting ID";

            /*
             * Routes Settings (path settings n shiz)
            */
            routeSettingsGrid.Rows.Clear();
            routeSettingsGrid.Refresh();

            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(Globals.bolFileStr, FileMode.Open)))
            {
                reader.BaseStream.Seek(Globals.routeListOffset, 0);

                for (int i = 0; i <= Globals.routeCount - 1; i++)
                {
                    int pointLength = reader.ReadInt16();
                    int startingIndex = reader.ReadInt16();

                    // padding.
                    for (int h = 0; h <= 3; h++)
                    {
                        reader.ReadUInt32();
                    }

                    string[] row = new string[] { i.ToString(), pointLength.ToString(), startingIndex.ToString() };
                    routeSettingsGrid.Rows.Add(row);
                }
                reader.Close();
            }
        }

        private void parseRoutes()
        {
            routeGrid.ColumnCount = 4;
            routeGrid.Columns[0].Name = "Index";
            routeGrid.Columns[1].Name = "X";
            routeGrid.Columns[2].Name = "Y";
            routeGrid.Columns[3].Name = "Z";

            /*
             * Routes (like paths n shiz)
            */
            routeGrid.Rows.Clear();
            routeGrid.Refresh();
            uint offsSub = Globals.objOffset - Globals.routePointOffset;
            uint numPoints = offsSub / 32;

            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(Globals.bolFileStr, FileMode.Open)))
            {
                reader.BaseStream.Seek(Globals.routePointOffset, 0);

                for (int i = 0; i <= numPoints - 1; i++)
                {
                    float xpos = reader.ReadSingle();
                    float ypos = reader.ReadSingle();
                    float zpos = reader.ReadSingle();

                    // padding.
                    for (int h = 0; h <= 4; h++)
                    {
                        reader.ReadUInt32();
                    }

                    string[] row = new string[] { i.ToString(), xpos.ToString(), ypos.ToString(), zpos.ToString()};
                    routeGrid.Rows.Add(row);
                }
                reader.Close();
            }
        }

        private void parseObjects()
        {
            objectsGrid.ColumnCount = 17;
            objectsGrid.Columns[0].Name = "Index";
            objectsGrid.Columns[1].Name = "Object Name";
            objectsGrid.Columns[2].Name = "X";
            objectsGrid.Columns[3].Name = "Y";
            objectsGrid.Columns[4].Name = "Z";
            objectsGrid.Columns[5].Name = "X Scale";
            objectsGrid.Columns[6].Name = "Y Scale";
            objectsGrid.Columns[7].Name = "Z Scale";
            objectsGrid.Columns[8].Name = "Rotation";
            objectsGrid.Columns[9].Name = "Object ID";
            objectsGrid.Columns[10].Name = "Route ID";
            objectsGrid.Columns[11].Name = "Setting 0";
            objectsGrid.Columns[12].Name = "Setting 1";
            objectsGrid.Columns[13].Name = "Setting 2";
            objectsGrid.Columns[14].Name = "Setting 3";
            objectsGrid.Columns[15].Name = "Setting 4";
            objectsGrid.Columns[16].Name = "Setting 5";

            /*
             * Objects
            */
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

                    int rotationX = reader.ReadInt32();
                    int rotationY = reader.ReadInt32();
                    int rotationZ = reader.ReadInt32();

                    double rotation = RotationHax.returnRotations(rotationX, rotationY, rotationZ);

                    Math.Round(rotation);

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
                    objectsGrid.Rows.Add(row);
                }
                reader.Close();
            }
        }

        private void parseStartingPoints(int isMultiplayer)
        {
            startingPointGrid.ColumnCount = 10;
            startingPointGrid.Columns[0].Name = "Index";
            startingPointGrid.Columns[1].Name = "X";
            startingPointGrid.Columns[2].Name = "Y";
            startingPointGrid.Columns[3].Name = "Z";
            startingPointGrid.Columns[4].Name = "X Scale";
            startingPointGrid.Columns[5].Name = "Y Scale";
            startingPointGrid.Columns[6].Name = "Z Scale";
            startingPointGrid.Columns[7].Name = "Rotation";
            startingPointGrid.Columns[8].Name = "Pole";
            startingPointGrid.Columns[9].Name = "Player ID";

            int numPoints;
            if (isMultiplayer == 1)
                numPoints = 4;
            else
                numPoints = 1;

            /*
             * Respawns
            */
            startingPointGrid.Rows.Clear();
            startingPointGrid.Refresh();
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

                    int rotationXZ = reader.ReadInt32();
                    int rotationYZ = reader.ReadInt32();
                    int rotationZZ = reader.ReadInt32();

                    double rotation = RotationHax.returnRotations(rotationXZ, rotationYZ, rotationZZ);

                    Math.Round(rotation);

                    int pole = reader.ReadByte();
                    int playerID = reader.ReadByte();

                    reader.ReadUInt16(); // last 2 bytes are padding

                    string[] row = new string[] { "0", xpos.ToString(), ypos.ToString(), zpos.ToString(), xscale.ToString(), yscale.ToString(), zscale.ToString(), rotation.ToString(), pole.ToString(), playerID.ToString() };
                    startingPointGrid.Rows.Add(row);
                }
                reader.Close();
            }
        }

        private void parseAreas()
        {
            areaGrid.ColumnCount = 12;
            areaGrid.Columns[0].Name = "Index";
            areaGrid.Columns[1].Name = "X";
            areaGrid.Columns[2].Name = "Y";
            areaGrid.Columns[3].Name = "Z";
            areaGrid.Columns[4].Name = "X Scale";
            areaGrid.Columns[5].Name = "Y Scale";
            areaGrid.Columns[6].Name = "Z Scale";
            areaGrid.Columns[7].Name = "Rotation";
            areaGrid.Columns[8].Name = "Unknown 0";
            areaGrid.Columns[9].Name = "Unknown 1";
            areaGrid.Columns[10].Name = "Unknown 2";
            areaGrid.Columns[11].Name = "Unknown 3";

            /*
             * Areas
            */
            areaGrid.Rows.Clear();
            areaGrid.Refresh();
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

                    int rotationX = reader.ReadInt32();
                    int rotationY = reader.ReadInt32();
                    int rotationZ = reader.ReadInt32();

                    double rotation = RotationHax.returnRotations(rotationX, rotationY, rotationZ);

                    Math.Round(rotation);

                    uint unk1 = reader.ReadUInt16();
                    uint unk2 = reader.ReadUInt16();
                    ulong unk3 = reader.ReadUInt64();
                    ulong unk4 = reader.ReadUInt64();

                    string unk3Out = unk3.ToString("X");
                    string unk4Out = unk4.ToString("X");

                    string[] row = new string[] { i.ToString(), xpos.ToString(), ypos.ToString(), zpos.ToString(), xscale.ToString(), yscale.ToString(), zscale.ToString(), rotation.ToString(), unk1.ToString(), unk2.ToString(), unk3Out, unk4Out };
                    areaGrid.Rows.Add(row);
                }
                reader.Close();
            }
        }

        private void parseCameras()
        {
            cameraGrid.ColumnCount = 23;
            cameraGrid.Columns[0].Name = "Index";
            cameraGrid.Columns[1].Name = "X1";
            cameraGrid.Columns[2].Name = "Y1";
            cameraGrid.Columns[3].Name = "Z1";
            cameraGrid.Columns[4].Name = "Rotation";
            cameraGrid.Columns[5].Name = "X2";
            cameraGrid.Columns[6].Name = "Y2";
            cameraGrid.Columns[7].Name = "Z2";
            cameraGrid.Columns[8].Name = "X3";
            cameraGrid.Columns[9].Name = "Y3";
            cameraGrid.Columns[10].Name = "Z3";
            cameraGrid.Columns[11].Name = "Unknown 0";
            cameraGrid.Columns[12].Name = "Cam Type";
            cameraGrid.Columns[13].Name = "Starting Zoom";
            cameraGrid.Columns[14].Name = "Camera Duration";
            cameraGrid.Columns[15].Name = "Unknown 1";
            cameraGrid.Columns[16].Name = "Unknown 2";
            cameraGrid.Columns[17].Name = "Unknown 3";
            cameraGrid.Columns[18].Name = "Route ID";
            cameraGrid.Columns[19].Name = "Speed";
            cameraGrid.Columns[20].Name = "End Zoom";
            cameraGrid.Columns[21].Name = "Next Camera ID";
            cameraGrid.Columns[22].Name = "Name";
            /*
             * Cameras
            */
            cameraGrid.Rows.Clear();
            cameraGrid.Refresh();
            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(Globals.bolFileStr, FileMode.Open)))
            {
                reader.BaseStream.Seek(Globals.cameraOffset, 0);

                for (int i = 0; i <= Globals.cameraCount - 1; i++)
                {
                    float x1pos = reader.ReadSingle();
                    float y1pos = reader.ReadSingle();
                    float z1pos = reader.ReadSingle();

                    int rotationX = reader.ReadInt32();
                    int rotationY = reader.ReadInt32();
                    int rotationZ = reader.ReadInt32();

                    double rotation = RotationHax.returnRotations(rotationX, rotationY, rotationZ);

                    float x2pos = reader.ReadSingle();
                    float y2pos = reader.ReadSingle();
                    float z2pos = reader.ReadSingle();

                    float x3pos = reader.ReadSingle();
                    float y3pos = reader.ReadSingle();
                    float z3pos = reader.ReadSingle();

                    int unk0 = reader.ReadByte();
                    int camType = reader.ReadByte();
                    uint startZoom = reader.ReadUInt16();
                    int cameraDur = reader.ReadInt16();
                    uint unk1 = reader.ReadUInt16();
                    uint unk2 = reader.ReadUInt16();
                    uint unk3 = reader.ReadUInt16();
                    int routeID = reader.ReadInt16();
                    uint routeSpeed = reader.ReadUInt16();
                    uint endZoom = reader.ReadUInt16();
                    int nextCam = reader.ReadInt16();

                    string camName = Encoding.ASCII.GetString(reader.ReadBytes(4));

                    string[] row = new string[] { i.ToString(), x1pos.ToString(), y1pos.ToString(), z1pos.ToString(), rotation.ToString(), x2pos.ToString(), y2pos.ToString(), z2pos.ToString(), x3pos.ToString(), y3pos.ToString(), z3pos.ToString(), unk0.ToString(), camType.ToString(), startZoom.ToString(), cameraDur.ToString(), unk1.ToString(), unk2.ToString(), unk3.ToString(), routeID.ToString(), routeSpeed.ToString(), endZoom.ToString(), nextCam.ToString(), camName.ToString()};
                    cameraGrid.Rows.Add(row);
                }
                reader.Close();
            }
        }

        private void parseRespawns()
        {
            respawnGrid.ColumnCount = 9;
            respawnGrid.Columns[0].Name = "Index";
            respawnGrid.Columns[1].Name = "X";
            respawnGrid.Columns[2].Name = "Y";
            respawnGrid.Columns[3].Name = "Z";
            respawnGrid.Columns[4].Name = "Rotation";
            respawnGrid.Columns[5].Name = "Respawn ID";
            respawnGrid.Columns[6].Name = "Unknown 0";
            respawnGrid.Columns[7].Name = "Unknown 1";
            respawnGrid.Columns[8].Name = "Unknown 2";
            /*
             * Respawn Positions
            */
            respawnGrid.Rows.Clear();
            respawnGrid.Refresh();
            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(Globals.bolFileStr, FileMode.Open)))
            {
                reader.BaseStream.Seek(Globals.respawnOffset, 0);

                for (int i = 0; i <= Globals.respawnCount - 1; i++)
                {
                    float xpos = reader.ReadSingle();
                    float ypos = reader.ReadSingle();
                    float zpos = reader.ReadSingle();

                    int rotationX = reader.ReadInt32();
                    int rotationY = reader.ReadInt32();
                    int rotationZ = reader.ReadInt32();

                    double rotation = RotationHax.returnRotations(rotationX, rotationY, rotationZ);

                    int respawnID = reader.ReadInt16();
                    int unk0 = reader.ReadInt16();
                    short unk1 = reader.ReadInt16();
                    short unk2 = reader.ReadInt16();

                    string[] row = new string[] { i.ToString(), xpos.ToString(), ypos.ToString(), zpos.ToString(), rotation.ToString(), respawnID.ToString(), unk0.ToString(), unk1.ToString(), unk2.ToString() };
                    respawnGrid.Rows.Add(row);
                }
                reader.Close();
            }
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

        private void iPKEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
           IKPEditor ikpEditor = new IKPEditor();
            ikpEditor.Show();
        }
    }
}