/**************************************************************************
 * 
 * Filename: PS5000ABlockForm.Designer.cs
 * 
 * Description:
 *  Windows Form Designer Class for PS5000ABlockCapture project.
 *  
 *  Copyright (C) 2016 - 2017 Pico Technology Ltd. See LICENSE file for terms.  
 *
 **************************************************************************/

namespace PS5000A
{
    partial class PS5000ABlockForm
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
            this.buttonOpen = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabUnit = new System.Windows.Forms.TabPage();
            this.tabChannels = new System.Windows.Forms.TabPage();
            this.comboRangeB = new System.Windows.Forms.ComboBox();
            this.comboRangeC = new System.Windows.Forms.ComboBox();
            this.comboRangeD = new System.Windows.Forms.ComboBox();
            this.comboRangeA = new System.Windows.Forms.ComboBox();
            this.tabGetData = new System.Windows.Forms.TabPage();
            this.textMessage = new System.Windows.Forms.TextBox();
            this.textData = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.textBoxUnitInfo = new System.Windows.Forms.TextBox();
            this.tabControl.SuspendLayout();
            this.tabUnit.SuspendLayout();
            this.tabChannels.SuspendLayout();
            this.tabGetData.SuspendLayout();
            this.tabAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(6, 15);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonOpen.TabIndex = 0;
            this.buttonOpen.Text = "Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabUnit);
            this.tabControl.Controls.Add(this.tabChannels);
            this.tabControl.Controls.Add(this.tabGetData);
            this.tabControl.Controls.Add(this.tabAbout);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(574, 281);
            this.tabControl.TabIndex = 3;
            // 
            // tabUnit
            // 
            this.tabUnit.Controls.Add(this.buttonOpen);
            this.tabUnit.Location = new System.Drawing.Point(4, 22);
            this.tabUnit.Name = "tabUnit";
            this.tabUnit.Padding = new System.Windows.Forms.Padding(3);
            this.tabUnit.Size = new System.Drawing.Size(566, 255);
            this.tabUnit.TabIndex = 0;
            this.tabUnit.Text = "Unit";
            this.tabUnit.UseVisualStyleBackColor = true;
            // 
            // tabChannels
            // 
            this.tabChannels.Controls.Add(this.comboRangeB);
            this.tabChannels.Controls.Add(this.comboRangeC);
            this.tabChannels.Controls.Add(this.comboRangeD);
            this.tabChannels.Controls.Add(this.comboRangeA);
            this.tabChannels.Location = new System.Drawing.Point(4, 22);
            this.tabChannels.Name = "tabChannels";
            this.tabChannels.Padding = new System.Windows.Forms.Padding(3);
            this.tabChannels.Size = new System.Drawing.Size(566, 255);
            this.tabChannels.TabIndex = 1;
            this.tabChannels.Text = "Channels";
            this.tabChannels.UseVisualStyleBackColor = true;
            // 
            // comboRangeB
            // 
            this.comboRangeB.FormattingEnabled = true;
            this.comboRangeB.Location = new System.Drawing.Point(83, 80);
            this.comboRangeB.Name = "comboRangeB";
            this.comboRangeB.Size = new System.Drawing.Size(121, 21);
            this.comboRangeB.TabIndex = 3;
            this.comboRangeB.Text = "Range B";
            // 
            // comboRangeC
            // 
            this.comboRangeC.FormattingEnabled = true;
            this.comboRangeC.Location = new System.Drawing.Point(83, 107);
            this.comboRangeC.Name = "comboRangeC";
            this.comboRangeC.Size = new System.Drawing.Size(121, 21);
            this.comboRangeC.TabIndex = 2;
            this.comboRangeC.Text = "Range C";
            // 
            // comboRangeD
            // 
            this.comboRangeD.FormattingEnabled = true;
            this.comboRangeD.Location = new System.Drawing.Point(83, 134);
            this.comboRangeD.Name = "comboRangeD";
            this.comboRangeD.Size = new System.Drawing.Size(121, 21);
            this.comboRangeD.TabIndex = 1;
            this.comboRangeD.Text = "Range D";
            // 
            // comboRangeA
            // 
            this.comboRangeA.FormattingEnabled = true;
            this.comboRangeA.Location = new System.Drawing.Point(83, 53);
            this.comboRangeA.Name = "comboRangeA";
            this.comboRangeA.Size = new System.Drawing.Size(121, 21);
            this.comboRangeA.TabIndex = 0;
            this.comboRangeA.Text = "Range A";
            // 
            // tabGetData
            // 
            this.tabGetData.Controls.Add(this.textMessage);
            this.tabGetData.Controls.Add(this.textData);
            this.tabGetData.Controls.Add(this.buttonStart);
            this.tabGetData.Location = new System.Drawing.Point(4, 22);
            this.tabGetData.Name = "tabGetData";
            this.tabGetData.Padding = new System.Windows.Forms.Padding(3);
            this.tabGetData.Size = new System.Drawing.Size(566, 255);
            this.tabGetData.TabIndex = 3;
            this.tabGetData.Text = "GetData";
            this.tabGetData.UseVisualStyleBackColor = true;
            // 
            // textMessage
            // 
            this.textMessage.Location = new System.Drawing.Point(13, 50);
            this.textMessage.Multiline = true;
            this.textMessage.Name = "textMessage";
            this.textMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textMessage.Size = new System.Drawing.Size(204, 172);
            this.textMessage.TabIndex = 2;
            // 
            // textData
            // 
            this.textData.Location = new System.Drawing.Point(234, 24);
            this.textData.Multiline = true;
            this.textData.Name = "textData";
            this.textData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textData.Size = new System.Drawing.Size(322, 198);
            this.textData.TabIndex = 1;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(13, 21);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // tabAbout
            // 
            this.tabAbout.Controls.Add(this.textBoxUnitInfo);
            this.tabAbout.Location = new System.Drawing.Point(4, 22);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabAbout.Size = new System.Drawing.Size(566, 255);
            this.tabAbout.TabIndex = 2;
            this.tabAbout.Text = "About";
            this.tabAbout.UseVisualStyleBackColor = true;
            // 
            // textBoxUnitInfo
            // 
            this.textBoxUnitInfo.Location = new System.Drawing.Point(6, 6);
            this.textBoxUnitInfo.Multiline = true;
            this.textBoxUnitInfo.Name = "textBoxUnitInfo";
            this.textBoxUnitInfo.Size = new System.Drawing.Size(179, 230);
            this.textBoxUnitInfo.TabIndex = 3;
            // 
            // PS5000ABlockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 314);
            this.Controls.Add(this.tabControl);
            this.Name = "PS5000ABlockForm";
            this.Text = "PS5000ABlockCapture";
            this.tabControl.ResumeLayout(false);
            this.tabUnit.ResumeLayout(false);
            this.tabChannels.ResumeLayout(false);
            this.tabGetData.ResumeLayout(false);
            this.tabGetData.PerformLayout();
            this.tabAbout.ResumeLayout(false);
            this.tabAbout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabUnit;
        private System.Windows.Forms.TabPage tabChannels;
        private System.Windows.Forms.ComboBox comboRangeA;
        private System.Windows.Forms.TabPage tabAbout;
        private System.Windows.Forms.TextBox textBoxUnitInfo;
        private System.Windows.Forms.ComboBox comboRangeB;
        private System.Windows.Forms.ComboBox comboRangeC;
        private System.Windows.Forms.ComboBox comboRangeD;
        private System.Windows.Forms.TabPage tabGetData;
        private System.Windows.Forms.TextBox textData;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.TextBox textMessage;
    }
}

