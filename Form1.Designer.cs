﻿using System.IO;
using Microsoft.Win32;

namespace INFOIBV
{
    partial class INFOIBV
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.LoadImageButton = new System.Windows.Forms.Button();
            this.openImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.imageFileName = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.applyButton = new System.Windows.Forms.Button();
            this.saveImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveButton = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.FilterSize = new System.Windows.Forms.ComboBox();
            this.StructuringShape = new System.Windows.Forms.ComboBox();
            this.Binary = new System.Windows.Forms.CheckBox();
            this.LoadImage2 = new System.Windows.Forms.Button();
            this.minLength = new System.Windows.Forms.TextBox();
            this.MaxGap = new System.Windows.Forms.TextBox();
            this.EdgeThreshold = new System.Windows.Forms.TextBox();
            this.peakingtheshold = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.minAngleText = new System.Windows.Forms.TextBox();
            this.maxAngleText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadImageButton
            // 
            this.LoadImageButton.Location = new System.Drawing.Point(18, 17);
            this.LoadImageButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LoadImageButton.Name = "LoadImageButton";
            this.LoadImageButton.Size = new System.Drawing.Size(147, 35);
            this.LoadImageButton.TabIndex = 0;
            this.LoadImageButton.Text = "Load image...";
            this.LoadImageButton.UseVisualStyleBackColor = true;
            this.LoadImageButton.Click += new System.EventHandler(this.loadImageButton_Click);
            // 
            // openImageDialog
            // 
            this.openImageDialog.Filter = "Bitmap files (*.bmp;*.gif;*.jpg;*.png;*.tiff;*.jpeg)|*.bmp;*.gif;*.jpg;*.png;*.ti" + "ff;*.jpeg";
            this.openImageDialog.InitialDirectory = "..\\..\\images";
            // 
            // imageFileName
            // 
            this.imageFileName.Location = new System.Drawing.Point(174, 18);
            this.imageFileName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.imageFileName.Name = "imageFileName";
            this.imageFileName.ReadOnly = true;
            this.imageFileName.Size = new System.Drawing.Size(492, 26);
            this.imageFileName.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(20, 69);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(768, 788);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(987, 17);
            this.applyButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(154, 35);
            this.applyButton.TabIndex = 3;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // saveImageDialog
            // 
            this.saveImageDialog.Filter = "Bitmap file (*.bmp)|*.bmp";
            this.saveImageDialog.InitialDirectory = "..\\..\\images";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(1422, 17);
            this.saveButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(142, 35);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save as BMP...";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(796, 69);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(768, 788);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(1150, 20);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(262, 31);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 6;
            this.progressBar.Visible = false;
            // 
            // comboBox
            // 
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Location = new System.Drawing.Point(676, 18);
            this.comboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox.Name = "comboBox";
            this.comboBox.Size = new System.Drawing.Size(300, 28);
            this.comboBox.TabIndex = 7;
            this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_Click);
            // 
            // FilterSize
            // 
            this.FilterSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterSize.FormattingEnabled = true;
            this.FilterSize.Location = new System.Drawing.Point(676, 54);
            this.FilterSize.Name = "FilterSize";
            this.FilterSize.Size = new System.Drawing.Size(70, 28);
            this.FilterSize.TabIndex = 8;
            this.FilterSize.Visible = false;
            // 
            // StructuringShape
            // 
            this.StructuringShape.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StructuringShape.FormattingEnabled = true;
            this.StructuringShape.Location = new System.Drawing.Point(752, 54);
            this.StructuringShape.Name = "StructuringShape";
            this.StructuringShape.Size = new System.Drawing.Size(91, 28);
            this.StructuringShape.TabIndex = 9;
            this.StructuringShape.Visible = false;
            // 
            // Binary
            // 
            this.Binary.Location = new System.Drawing.Point(872, 58);
            this.Binary.Name = "Binary";
            this.Binary.Size = new System.Drawing.Size(104, 24);
            this.Binary.TabIndex = 10;
            this.Binary.Text = "Binary";
            this.Binary.UseVisualStyleBackColor = true;
            this.Binary.Visible = false;
            this.Binary.CheckedChanged += new System.EventHandler(this.Binary_CheckedChanged);
            // 
            // LoadImage2
            // 
            this.LoadImage2.Location = new System.Drawing.Point(18, 54);
            this.LoadImage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LoadImage2.Name = "LoadImage2";
            this.LoadImage2.Size = new System.Drawing.Size(147, 35);
            this.LoadImage2.TabIndex = 11;
            this.LoadImage2.Text = "Load Image 2..";
            this.LoadImage2.UseVisualStyleBackColor = true;
            this.LoadImage2.Click += new System.EventHandler(this.loadImageButton2_Click);
            // 
            // minLength
            // 
            this.minLength.AccessibleDescription = "";
            this.minLength.AccessibleName = "minL";
            this.minLength.Location = new System.Drawing.Point(987, 83);
            this.minLength.Name = "minLength";
            this.minLength.Size = new System.Drawing.Size(100, 26);
            this.minLength.TabIndex = 12;
            this.minLength.Tag = "";
            this.minLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MaxGap
            // 
            this.MaxGap.AccessibleName = "maxG";
            this.MaxGap.Location = new System.Drawing.Point(1093, 83);
            this.MaxGap.Name = "MaxGap";
            this.MaxGap.Size = new System.Drawing.Size(100, 26);
            this.MaxGap.TabIndex = 13;
            // 
            // EdgeThreshold
            // 
            this.EdgeThreshold.AccessibleName = "mit";
            this.EdgeThreshold.Location = new System.Drawing.Point(1199, 83);
            this.EdgeThreshold.Name = "EdgeThreshold";
            this.EdgeThreshold.Size = new System.Drawing.Size(100, 26);
            this.EdgeThreshold.TabIndex = 14;
            // 
            // peakingtheshold
            // 
            this.peakingtheshold.AccessibleName = "pthres";
            this.peakingtheshold.Location = new System.Drawing.Point(1305, 83);
            this.peakingtheshold.Name = "peakingtheshold";
            this.peakingtheshold.Size = new System.Drawing.Size(100, 26);
            this.peakingtheshold.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(987, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 16;
            this.label1.Text = "Min length";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(1093, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 17;
            this.label2.Text = "Max Gap";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(1199, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 18;
            this.label3.Text = "edge thresh";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(1305, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 19;
            this.label4.Text = "peaking thresh(percentage)";
            // 
            // minAngleText
            // 
            this.minAngleText.Location = new System.Drawing.Point(1422, 83);
            this.minAngleText.Name = "minAngleText";
            this.minAngleText.Size = new System.Drawing.Size(51, 26);
            this.minAngleText.TabIndex = 20;
            // 
            // maxAngleText
            // 
            this.maxAngleText.Location = new System.Drawing.Point(1479, 83);
            this.maxAngleText.Name = "maxAngleText";
            this.maxAngleText.Size = new System.Drawing.Size(51, 26);
            this.maxAngleText.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(1431, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 23);
            this.label5.TabIndex = 22;
            this.label5.Text = "min\r\n";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(1488, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 23);
            this.label6.TabIndex = 23;
            this.label6.Text = "max";
            // 
            // INFOIBV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1578, 886);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.maxAngleText);
            this.Controls.Add(this.minAngleText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.peakingtheshold);
            this.Controls.Add(this.EdgeThreshold);
            this.Controls.Add(this.MaxGap);
            this.Controls.Add(this.minLength);
            this.Controls.Add(this.LoadImage2);
            this.Controls.Add(this.Binary);
            this.Controls.Add(this.StructuringShape);
            this.Controls.Add(this.FilterSize);
            this.Controls.Add(this.comboBox);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.imageFileName);
            this.Controls.Add(this.LoadImageButton);
            this.Location = new System.Drawing.Point(10, 10);
            this.Name = "INFOIBV";
            this.ShowIcon = false;
            this.Text = "INFOIBV";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label6;

        private System.Windows.Forms.TextBox minAngleText;
        private System.Windows.Forms.TextBox maxAngleText;
        private System.Windows.Forms.Label label5;

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.TextBox peakingtheshold;

        private System.Windows.Forms.TextBox EdgeThreshold;

        private System.Windows.Forms.TextBox MaxGap;

        private System.Windows.Forms.TextBox minLength;

        private System.Windows.Forms.Button LoadImage2;
        private System.Windows.Forms.ComboBox StructuringShape;
        private System.Windows.Forms.ComboBox FilterSize;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.CheckBox Binary;

        #endregion

        private System.Windows.Forms.Button LoadImageButton;
        private System.Windows.Forms.OpenFileDialog openImageDialog;
        private System.Windows.Forms.TextBox imageFileName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.SaveFileDialog saveImageDialog;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ComboBox comboBox;
    }
}

