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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DouBOLDash
{
    public partial class IKPEditor : Form
    {
        public IKPEditor()
        {
            InitializeComponent();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openIPKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "IKP Files (*.ikp)|*.ikp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string ipkFile = openFileDialog1.FileName;
                try
                {
                    parseIPK(ipkFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured while opening the file " + ipkFile + "\n Exception Thrown: \n" + ex, "An Error Occured");
                }
            }
        }

        private void parseIPK(string ipkFile)
        {
            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(ipkFile, FileMode.Open)))
            {
                float unk1 = reader.ReadSingle();
                textBox1.Text = unk1.ToString();
                float unk2 = reader.ReadSingle();
                textBox2.Text = unk2.ToString();
                float unk3 = reader.ReadSingle();
                textBox3.Text = unk3.ToString();
                float unk4 = reader.ReadSingle();
                textBox4.Text = unk4.ToString();
                float unk5 = reader.ReadSingle();
                textBox5.Text = unk5.ToString();
                float unk6 = reader.ReadSingle();
                textBox6.Text = unk6.ToString();
                float unk7 = reader.ReadSingle();
                textBox7.Text = unk7.ToString();
                float unk8 = reader.ReadSingle();
                textBox8.Text = unk8.ToString();
                float unk9 = reader.ReadSingle();
                textBox9.Text = unk9.ToString();
                float unk10 = reader.ReadSingle();
                textBox10.Text = unk10.ToString();
                float unk11 = reader.ReadSingle();
                textBox11.Text = unk11.ToString();
                byte unk12 = reader.ReadByte();
                textBox12.Text = unk12.ToString();
                byte shit1 = reader.ReadByte();
                textBox28.Text = shit1.ToString();
                byte shit2 = reader.ReadByte();
                byte shit3 = reader.ReadByte();
                float unk13 = reader.ReadSingle();
                textBox13.Text = unk13.ToString();
                float unk14 = reader.ReadSingle();
                textBox14.Text = unk14.ToString();
                float unk15 = reader.ReadSingle();
                textBox15.Text = unk15.ToString();
                float unk16 = reader.ReadSingle();
                textBox16.Text = unk16.ToString();
                float unk17 = reader.ReadSingle();
                textBox17.Text = unk17.ToString();
                float unk18 = reader.ReadSingle();
                textBox18.Text = unk18.ToString();
                float unk19 = reader.ReadSingle();
                textBox19.Text = unk19.ToString();
                float unk20 = reader.ReadSingle();
                textBox20.Text = unk20.ToString();
                float unk21 = reader.ReadSingle();
                textBox21.Text = unk21.ToString();
                float unk22 = reader.ReadSingle();
                textBox22.Text = unk22.ToString();
                byte unk23 = reader.ReadByte();
                textBox23.Text = unk23.ToString();
                byte unk24 = reader.ReadByte();
                textBox24.Text = unk24.ToString();
                reader.ReadByte();
                reader.ReadByte();
                float unk25 = reader.ReadSingle();
                textBox25.Text = unk25.ToString();
                float unk26 = reader.ReadSingle();
                textBox26.Text = unk26.ToString();
                // we're done with this reader
                reader.Close();
            }
        }

        private void saveIPK(string ipkFile)
        {
            using (EndianBinaryWriter writer = new EndianBinaryWriter(File.Open(ipkFile, FileMode.Create)))
            {
                float unk1 = Convert.ToSingle(textBox1.Text);
                writer.Write(unk1);
                float unk2 = Convert.ToSingle(textBox2.Text);
                writer.Write(unk2);
                float unk3 = Convert.ToSingle(textBox3.Text);
                writer.Write(unk3);
                float unk4 = Convert.ToSingle(textBox4.Text);
                writer.Write(unk4);
                float unk5 = Convert.ToSingle(textBox5.Text);
                writer.Write(unk5);
                float unk6 = Convert.ToSingle(textBox6.Text);
                writer.Write(unk6);
                float unk7 = Convert.ToSingle(textBox7.Text);
                writer.Write(unk7);
                float unk8 = Convert.ToSingle(textBox8.Text);
                writer.Write(unk8);
                float unk9 = Convert.ToSingle(textBox9.Text);
                writer.Write(unk9);
                float unk10 = Convert.ToSingle(textBox10.Text);
                writer.Write(unk10);
                float unk11 = Convert.ToSingle(textBox11.Text);
                writer.Write(unk11);
                byte unk12 = Convert.ToByte(textBox12.Text);
                writer.Write(unk12);
                byte unk28 = Convert.ToByte(textBox28.Text);
                writer.Write(unk28);
                byte derp2 = 0;
                writer.Write(derp2);
                writer.Write(derp2);
                float unk13 = Convert.ToSingle(textBox13.Text);
                writer.Write(unk13);
                float unk14 = Convert.ToSingle(textBox14.Text);
                writer.Write(unk14);
                float unk15 = Convert.ToSingle(textBox15.Text);
                writer.Write(unk15);
                float unk16 = Convert.ToSingle(textBox16.Text);
                writer.Write(unk16);
                float unk17 = Convert.ToSingle(textBox17.Text);
                writer.Write(unk17);
                float unk18 = Convert.ToSingle(textBox18.Text);
                writer.Write(unk18);
                float unk19 = Convert.ToSingle(textBox19.Text);
                writer.Write(unk19);
                float unk20 = Convert.ToSingle(textBox20.Text);
                writer.Write(unk20);
                float unk21 = Convert.ToSingle(textBox21.Text);
                writer.Write(unk21);
                float unk22 = Convert.ToSingle(textBox22.Text);
                writer.Write(unk22);
                byte unk23 = Convert.ToByte(textBox23.Text);
                Console.WriteLine("unk23: " + unk23.ToString());
                writer.Write(unk23);
                byte unk24 = Convert.ToByte(textBox24.Text);
                Console.WriteLine("unk24: " + unk24.ToString());
                writer.Write(unk24);
                byte derp1 = 0;
                writer.Write(derp1);
                writer.Write(derp1);
                float unk25 = Convert.ToSingle(textBox25.Text);
                writer.Write(unk25);
                float unk26 = Convert.ToSingle(textBox26.Text);
                writer.Write(unk26);
                writer.Close();
            }
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void saveIPKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "IKP Files (*.ikp)|*.ikp";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string ipkFile = saveFileDialog1.FileName;
                try
                {
                    saveIPK(ipkFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured while saving the file " + ipkFile + "\n Exception Thrown: \n" + ex, "An Error Occured");
                }
            }
        }
    }
}
