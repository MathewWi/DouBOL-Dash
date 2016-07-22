using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DouBOLDash
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void oPenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "BOL Files (*.bol)|*.bol|All files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string bolStr = openFileDialog1.FileName;
                try
                {
                    parseBOL(bolStr);
                }
                catch (IOException)
                {
                    MessageBox.Show("An error occured while opening the file.", "An Error Occured");
                }
            }
        }

        public void parseBOL(String bolFile)
        {
            uint magic, unk1, unk2, lapCount, musicID;
            float unkFloat1, unkFloat2, unkFloat3;

            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(bolFile, FileMode.Open)))
            {
                magic = reader.ReadUInt32();
                unk1 = reader.ReadUInt32();
                unk2 = reader.ReadUInt32();
                unkFloat1 = reader.ReadSingle();
                unkFloat2 = reader.ReadSingle();
                unkFloat3 = reader.ReadSingle();
                lapCount = reader.ReadByte();
                musicID = reader.ReadByte();

                unk4Input.Text = unk1.ToString();
                unk8Input.Text = unk2.ToString();
                unkFltC.Text = unkFloat1.ToString();
                unkFlt10.Text = unkFloat2.ToString();
                unkFlt14.Text = unkFloat3.ToString();
                lapCountInput.Text = lapCount.ToString();
                musicIDInput.Text = musicID.ToString();
            }


            }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
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
                musicID = Convert.ToByte(musicIDInput.Text);

                magic = 0x30303135;
                writer.Write(magic);
                writer.Write(unk4);
                writer.Write(unk8);
                writer.Write(unkFloat1);
                writer.Write(unkFloat2);
                writer.Write(unkFloat3);
                writer.Write(lapCount);
                writer.Write(musicID);
            }
        }
    }
}
