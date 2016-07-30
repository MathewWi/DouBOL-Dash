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
namespace DouBOLDash
{
    partial class AboutBox1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox1));
            this.nameLabel = new System.Windows.Forms.Label();
            this.realmLink = new System.Windows.Forms.LinkLabel();
            this.mushyBoxImage = new System.Windows.Forms.PictureBox();
            this.programText = new System.Windows.Forms.TextBox();
            this.versionLabel = new System.Windows.Forms.Label();
            this.reanLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mushyBoxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.Location = new System.Drawing.Point(12, 8);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(139, 25);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "DouBOL Dash";
            // 
            // realmLink
            // 
            this.realmLink.AutoSize = true;
            this.realmLink.Location = new System.Drawing.Point(278, 161);
            this.realmLink.Name = "realmLink";
            this.realmLink.Size = new System.Drawing.Size(136, 13);
            this.realmLink.TabIndex = 3;
            this.realmLink.TabStop = true;
            this.realmLink.Text = "http://smsrealm.net/board/";
            // 
            // mushyBoxImage
            // 
            this.mushyBoxImage.Image = global::DouBOLDash.Properties.Resources.WEEEEEEEEEEEEEEEEE;
            this.mushyBoxImage.Location = new System.Drawing.Point(268, 8);
            this.mushyBoxImage.Name = "mushyBoxImage";
            this.mushyBoxImage.Size = new System.Drawing.Size(167, 150);
            this.mushyBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.mushyBoxImage.TabIndex = 4;
            this.mushyBoxImage.TabStop = false;
            // 
            // programText
            // 
            this.programText.Enabled = false;
            this.programText.Location = new System.Drawing.Point(12, 71);
            this.programText.Multiline = true;
            this.programText.Name = "programText";
            this.programText.ReadOnly = true;
            this.programText.Size = new System.Drawing.Size(250, 121);
            this.programText.TabIndex = 5;
            this.programText.Text = resources.GetString("programText.Text");
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.Location = new System.Drawing.Point(12, 33);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(130, 17);
            this.versionLabel.TabIndex = 6;
            this.versionLabel.Text = "v0.1 ALPHA (2016)";
            // 
            // reanLabel
            // 
            this.reanLabel.AutoSize = true;
            this.reanLabel.Location = new System.Drawing.Point(12, 55);
            this.reanLabel.Name = "reanLabel";
            this.reanLabel.Size = new System.Drawing.Size(63, 13);
            this.reanLabel.TabIndex = 7;
            this.reanLabel.Text = " By MrRean";
            // 
            // AboutBox1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 193);
            this.Controls.Add(this.reanLabel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.programText);
            this.Controls.Add(this.mushyBoxImage);
            this.Controls.Add(this.realmLink);
            this.Controls.Add(this.nameLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox1";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About DouBOL Dash";
            ((System.ComponentModel.ISupportInitialize)(this.mushyBoxImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.LinkLabel realmLink;
        private System.Windows.Forms.PictureBox mushyBoxImage;
        private System.Windows.Forms.TextBox programText;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label reanLabel;
    }
}
