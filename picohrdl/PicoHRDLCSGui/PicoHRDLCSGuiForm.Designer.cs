/******************************************************************************
 *
 * Filename: PicoHRDLCSGuiForm.Designer.cs
 * 
 * Copyright (C) 2016 - 2017 Pico Technology Ltd. See LICENSE file for terms.
 *
 ******************************************************************************/

namespace PicoHRDLGui
{
    partial class PicoHRDLCSGuiForm
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
            this.runButton = new System.Windows.Forms.Button();
            this.unitInfoTextBox = new System.Windows.Forms.TextBox();
            this.channel1DataTextBox = new System.Windows.Forms.TextBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.numValuesCollectedLabel = new System.Windows.Forms.Label();
            this.numvaluesCollectedTextBox = new System.Windows.Forms.TextBox();
            this.openButton = new System.Windows.Forms.Button();
            this.numValuesToCollectedTextBox = new System.Windows.Forms.TextBox();
            this.numValuesToCollectLabel = new System.Windows.Forms.Label();
            this.channel1DataLabel = new System.Windows.Forms.Label();
            this.unitInformationLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(202, 12);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(75, 23);
            this.runButton.TabIndex = 0;
            this.runButton.Text = "Run";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // unitInfoTextBox
            // 
            this.unitInfoTextBox.Location = new System.Drawing.Point(21, 79);
            this.unitInfoTextBox.Multiline = true;
            this.unitInfoTextBox.Name = "unitInfoTextBox";
            this.unitInfoTextBox.Size = new System.Drawing.Size(164, 178);
            this.unitInfoTextBox.TabIndex = 1;
            // 
            // channel1DataTextBox
            // 
            this.channel1DataTextBox.Location = new System.Drawing.Point(202, 79);
            this.channel1DataTextBox.Multiline = true;
            this.channel1DataTextBox.Name = "channel1DataTextBox";
            this.channel1DataTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.channel1DataTextBox.Size = new System.Drawing.Size(283, 394);
            this.channel1DataTextBox.TabIndex = 2;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(410, 12);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // numValuesCollectedLabel
            // 
            this.numValuesCollectedLabel.AutoSize = true;
            this.numValuesCollectedLabel.Location = new System.Drawing.Point(18, 344);
            this.numValuesCollectedLabel.Name = "numValuesCollectedLabel";
            this.numValuesCollectedLabel.Size = new System.Drawing.Size(113, 13);
            this.numValuesCollectedLabel.TabIndex = 4;
            this.numValuesCollectedLabel.Text = "Num Values collected:";
            // 
            // numvaluesCollectedTextBox
            // 
            this.numvaluesCollectedTextBox.Location = new System.Drawing.Point(21, 360);
            this.numvaluesCollectedTextBox.Name = "numvaluesCollectedTextBox";
            this.numvaluesCollectedTextBox.ReadOnly = true;
            this.numvaluesCollectedTextBox.Size = new System.Drawing.Size(100, 20);
            this.numvaluesCollectedTextBox.TabIndex = 5;
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(21, 12);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 23);
            this.openButton.TabIndex = 6;
            this.openButton.Text = "Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // numValuesToCollectedTextBox
            // 
            this.numValuesToCollectedTextBox.Location = new System.Drawing.Point(21, 298);
            this.numValuesToCollectedTextBox.Name = "numValuesToCollectedTextBox";
            this.numValuesToCollectedTextBox.Size = new System.Drawing.Size(100, 20);
            this.numValuesToCollectedTextBox.TabIndex = 7;
            this.numValuesToCollectedTextBox.Text = "10";
            // 
            // numValuesToCollectLabel
            // 
            this.numValuesToCollectLabel.AutoSize = true;
            this.numValuesToCollectLabel.Location = new System.Drawing.Point(18, 282);
            this.numValuesToCollectLabel.Name = "numValuesToCollectLabel";
            this.numValuesToCollectLabel.Size = new System.Drawing.Size(112, 13);
            this.numValuesToCollectLabel.TabIndex = 8;
            this.numValuesToCollectLabel.Text = "Num values to collect:";
            // 
            // channel1DataLabel
            // 
            this.channel1DataLabel.AutoSize = true;
            this.channel1DataLabel.Location = new System.Drawing.Point(199, 63);
            this.channel1DataLabel.Name = "channel1DataLabel";
            this.channel1DataLabel.Size = new System.Drawing.Size(84, 13);
            this.channel1DataLabel.TabIndex = 9;
            this.channel1DataLabel.Text = "Channel 1 Data:";
            // 
            // unitInformationLabel
            // 
            this.unitInformationLabel.AutoSize = true;
            this.unitInformationLabel.Location = new System.Drawing.Point(21, 62);
            this.unitInformationLabel.Name = "unitInformationLabel";
            this.unitInformationLabel.Size = new System.Drawing.Size(84, 13);
            this.unitInformationLabel.TabIndex = 10;
            this.unitInformationLabel.Text = "Unit Information:";
            // 
            // PicoHRDLCSGuiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 496);
            this.Controls.Add(this.unitInformationLabel);
            this.Controls.Add(this.channel1DataLabel);
            this.Controls.Add(this.numValuesToCollectLabel);
            this.Controls.Add(this.numValuesToCollectedTextBox);
            this.Controls.Add(this.openButton);
            this.Controls.Add(this.numvaluesCollectedTextBox);
            this.Controls.Add(this.numValuesCollectedLabel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.channel1DataTextBox);
            this.Controls.Add(this.unitInfoTextBox);
            this.Controls.Add(this.runButton);
            this.Name = "PicoHRDLCSGuiForm";
            this.Text = "ADC-20/ADC-24 C# GUI Example";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.TextBox unitInfoTextBox;
        private System.Windows.Forms.TextBox channel1DataTextBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label numValuesCollectedLabel;
        private System.Windows.Forms.TextBox numvaluesCollectedTextBox;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.TextBox numValuesToCollectedTextBox;
        private System.Windows.Forms.Label numValuesToCollectLabel;
        private System.Windows.Forms.Label channel1DataLabel;
        private System.Windows.Forms.Label unitInformationLabel;
    }
}

