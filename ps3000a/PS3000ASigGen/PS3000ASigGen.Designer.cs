/**************************************************************************
 *
 * Filename: PS3000ASigGen.Designer.cs
 * 
 * Copyright (C) 2016 - 2017 Pico Technology Ltd. See LICENSE file for terms.
 *
 **************************************************************************/

namespace PS3000ASigGen
{
    partial class AWG_SIGGEN
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
            this.controls = new System.Windows.Forms.TableLayoutPanel();
            this.awgFileInfoLabel = new System.Windows.Forms.TextBox();
            this.sigToAWG = new System.Windows.Forms.CheckBox();
            this.awgLabel = new System.Windows.Forms.Label();
            this.updateButton = new System.Windows.Forms.Button();
            this.peatkToPeakVoltageTextBox = new System.Windows.Forms.TextBox();
            this.startFrequencyTextBox = new System.Windows.Forms.TextBox();
            this.offsetVoltageTextBox = new System.Windows.Forms.TextBox();
            this.sweepCheckBox = new System.Windows.Forms.CheckBox();
            this.offsetVoltageLabel = new System.Windows.Forms.Label();
            this.fileNameTextBox = new System.Windows.Forms.TextBox();
            this.signalTypeComboBox = new System.Windows.Forms.ComboBox();
            this.SweepController = new System.Windows.Forms.TableLayoutPanel();
            this.incrementTimeIntervalLabel = new System.Windows.Forms.Label();
            this.sweepTypeLabel = new System.Windows.Forms.Label();
            this.stopFrequencyLabel = new System.Windows.Forms.Label();
            this.frequencyIncrementLabel = new System.Windows.Forms.Label();
            this.sweepTypeComboBox = new System.Windows.Forms.ComboBox();
            this.stopFreqTextBox = new System.Windows.Forms.TextBox();
            this.frequencyIncrementTextBox = new System.Windows.Forms.TextBox();
            this.timeIncrementTextBox = new System.Windows.Forms.TextBox();
            this.startFrequencyLabel = new System.Windows.Forms.Label();
            this.peakToPeakVoltageLabel = new System.Windows.Forms.Label();
            this.Start = new System.Windows.Forms.Panel();
            this.Start_button = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.controls.SuspendLayout();
            this.SweepController.SuspendLayout();
            this.Start.SuspendLayout();
            this.SuspendLayout();
            // 
            // controls
            // 
            this.controls.ColumnCount = 2;
            this.controls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.62128F));
            this.controls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.37872F));
            this.controls.Controls.Add(this.awgFileInfoLabel, 0, 6);
            this.controls.Controls.Add(this.sigToAWG, 1, 0);
            this.controls.Controls.Add(this.awgLabel, 0, 0);
            this.controls.Controls.Add(this.updateButton, 1, 6);
            this.controls.Controls.Add(this.peatkToPeakVoltageTextBox, 1, 3);
            this.controls.Controls.Add(this.startFrequencyTextBox, 1, 2);
            this.controls.Controls.Add(this.offsetVoltageTextBox, 1, 4);
            this.controls.Controls.Add(this.sweepCheckBox, 1, 5);
            this.controls.Controls.Add(this.offsetVoltageLabel, 0, 4);
            this.controls.Controls.Add(this.fileNameTextBox, 0, 1);
            this.controls.Controls.Add(this.signalTypeComboBox, 1, 1);
            this.controls.Controls.Add(this.SweepController, 0, 5);
            this.controls.Controls.Add(this.startFrequencyLabel, 0, 2);
            this.controls.Controls.Add(this.peakToPeakVoltageLabel, 0, 3);
            this.controls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controls.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.controls.Location = new System.Drawing.Point(0, 0);
            this.controls.Name = "controls";
            this.controls.RowCount = 7;
            this.controls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.controls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.89866F));
            this.controls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.08987F));
            this.controls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.13384F));
            this.controls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.500956F));
            this.controls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.43021F));
            this.controls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.60612F));
            this.controls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.controls.Size = new System.Drawing.Size(639, 523);
            this.controls.TabIndex = 0;
            this.controls.Visible = false;
            // 
            // awgFileInfoLabel
            // 
            this.awgFileInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.awgFileInfoLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.awgFileInfoLabel.Location = new System.Drawing.Point(3, 412);
            this.awgFileInfoLabel.Multiline = true;
            this.awgFileInfoLabel.Name = "awgFileInfoLabel";
            this.awgFileInfoLabel.ReadOnly = true;
            this.awgFileInfoLabel.Size = new System.Drawing.Size(489, 108);
            this.awgFileInfoLabel.TabIndex = 36;
            this.awgFileInfoLabel.Text = "AWG Files: Please ensure that the file has only one value per line, in the range " +
    "-32768 (min) to 32767 (max).";
            this.awgFileInfoLabel.Visible = false;
            // 
            // sigToAWG
            // 
            this.sigToAWG.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sigToAWG.AutoSize = true;
            this.sigToAWG.Enabled = false;
            this.sigToAWG.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.sigToAWG.Location = new System.Drawing.Point(498, 3);
            this.sigToAWG.Name = "sigToAWG";
            this.sigToAWG.Size = new System.Drawing.Size(138, 68);
            this.sigToAWG.TabIndex = 2;
            this.sigToAWG.Text = "AWG";
            this.sigToAWG.UseVisualStyleBackColor = true;
            this.sigToAWG.CheckedChanged += new System.EventHandler(this.SIGtoAWG_CheckedChanged);
            // 
            // awgLabel
            // 
            this.awgLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.awgLabel.AutoSize = true;
            this.awgLabel.Location = new System.Drawing.Point(3, 0);
            this.awgLabel.Name = "awgLabel";
            this.awgLabel.Size = new System.Drawing.Size(489, 74);
            this.awgLabel.TabIndex = 26;
            this.awgLabel.Text = "Please enter filename (include the file extension):";
            this.awgLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.awgLabel.Visible = false;
            // 
            // updateButton
            // 
            this.updateButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.updateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.updateButton.Location = new System.Drawing.Point(498, 483);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(138, 37);
            this.updateButton.TabIndex = 29;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.Update_button_Click);
            // 
            // peatkToPeakVoltageTextBox
            // 
            this.peatkToPeakVoltageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.peatkToPeakVoltageTextBox.Location = new System.Drawing.Point(498, 192);
            this.peatkToPeakVoltageTextBox.Name = "peatkToPeakVoltageTextBox";
            this.peatkToPeakVoltageTextBox.Size = new System.Drawing.Size(138, 30);
            this.peatkToPeakVoltageTextBox.TabIndex = 30;
            this.peatkToPeakVoltageTextBox.Text = "2000";
            this.peatkToPeakVoltageTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // startFrequencyTextBox
            // 
            this.startFrequencyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startFrequencyTextBox.Location = new System.Drawing.Point(498, 134);
            this.startFrequencyTextBox.Name = "startFrequencyTextBox";
            this.startFrequencyTextBox.Size = new System.Drawing.Size(138, 30);
            this.startFrequencyTextBox.TabIndex = 28;
            this.startFrequencyTextBox.Text = "1000";
            this.startFrequencyTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // offsetVoltageTextBox
            // 
            this.offsetVoltageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.offsetVoltageTextBox.Location = new System.Drawing.Point(498, 245);
            this.offsetVoltageTextBox.Name = "offsetVoltageTextBox";
            this.offsetVoltageTextBox.Size = new System.Drawing.Size(138, 30);
            this.offsetVoltageTextBox.TabIndex = 33;
            this.offsetVoltageTextBox.Text = "0";
            this.offsetVoltageTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // sweepCheckBox
            // 
            this.sweepCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sweepCheckBox.AutoSize = true;
            this.sweepCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.sweepCheckBox.Location = new System.Drawing.Point(498, 279);
            this.sweepCheckBox.Name = "sweepCheckBox";
            this.sweepCheckBox.Size = new System.Drawing.Size(138, 127);
            this.sweepCheckBox.TabIndex = 34;
            this.sweepCheckBox.Text = "Sweep Mode";
            this.sweepCheckBox.UseVisualStyleBackColor = true;
            this.sweepCheckBox.CheckedChanged += new System.EventHandler(this.Sweep_CheckedChanged);
            // 
            // offsetVoltageLabel
            // 
            this.offsetVoltageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.offsetVoltageLabel.AutoSize = true;
            this.offsetVoltageLabel.Location = new System.Drawing.Point(3, 242);
            this.offsetVoltageLabel.Name = "offsetVoltageLabel";
            this.offsetVoltageLabel.Size = new System.Drawing.Size(489, 34);
            this.offsetVoltageLabel.TabIndex = 32;
            this.offsetVoltageLabel.Text = "Offset  Voltage (mV)";
            this.offsetVoltageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileNameTextBox.Enabled = false;
            this.fileNameTextBox.Location = new System.Drawing.Point(3, 77);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Size = new System.Drawing.Size(489, 30);
            this.fileNameTextBox.TabIndex = 25;
            this.fileNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // signalTypeComboBox
            // 
            this.signalTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.signalTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.signalTypeComboBox.FormattingEnabled = true;
            this.signalTypeComboBox.Items.AddRange(new object[] {
            "Sine",
            "Square",
            "Triangle",
            "Ramp Up",
            "Ramp Down",
            "Sin(x)/x",
            "Gaussian",
            "Half Sine",
            "DC",
            "White Noise",
            "PRBS"});
            this.signalTypeComboBox.Location = new System.Drawing.Point(498, 77);
            this.signalTypeComboBox.Name = "signalTypeComboBox";
            this.signalTypeComboBox.Size = new System.Drawing.Size(138, 33);
            this.signalTypeComboBox.TabIndex = 37;
            this.signalTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.signal_type_SelectedIndexChanged);
            // 
            // SweepController
            // 
            this.SweepController.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SweepController.ColumnCount = 2;
            this.SweepController.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SweepController.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SweepController.Controls.Add(this.incrementTimeIntervalLabel, 0, 3);
            this.SweepController.Controls.Add(this.sweepTypeLabel, 0, 0);
            this.SweepController.Controls.Add(this.stopFrequencyLabel, 0, 1);
            this.SweepController.Controls.Add(this.frequencyIncrementLabel, 0, 2);
            this.SweepController.Controls.Add(this.sweepTypeComboBox, 1, 0);
            this.SweepController.Controls.Add(this.stopFreqTextBox, 1, 1);
            this.SweepController.Controls.Add(this.frequencyIncrementTextBox, 1, 2);
            this.SweepController.Controls.Add(this.timeIncrementTextBox, 1, 3);
            this.SweepController.Location = new System.Drawing.Point(3, 279);
            this.SweepController.Name = "SweepController";
            this.SweepController.RowCount = 4;
            this.SweepController.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.SweepController.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.SweepController.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.SweepController.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.SweepController.Size = new System.Drawing.Size(489, 127);
            this.SweepController.TabIndex = 38;
            this.SweepController.Visible = false;
            // 
            // incrementTimeIntervalLabel
            // 
            this.incrementTimeIntervalLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.incrementTimeIntervalLabel.AutoSize = true;
            this.incrementTimeIntervalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.incrementTimeIntervalLabel.Location = new System.Drawing.Point(3, 93);
            this.incrementTimeIntervalLabel.Name = "incrementTimeIntervalLabel";
            this.incrementTimeIntervalLabel.Size = new System.Drawing.Size(238, 34);
            this.incrementTimeIntervalLabel.TabIndex = 13;
            this.incrementTimeIntervalLabel.Text = "Increment Interval (s)";
            this.incrementTimeIntervalLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sweepTypeLabel
            // 
            this.sweepTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sweepTypeLabel.AutoSize = true;
            this.sweepTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.sweepTypeLabel.Location = new System.Drawing.Point(3, 0);
            this.sweepTypeLabel.Name = "sweepTypeLabel";
            this.sweepTypeLabel.Size = new System.Drawing.Size(238, 31);
            this.sweepTypeLabel.TabIndex = 5;
            this.sweepTypeLabel.Text = "Sweep Type";
            this.sweepTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // stopFrequencyLabel
            // 
            this.stopFrequencyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stopFrequencyLabel.AutoSize = true;
            this.stopFrequencyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.stopFrequencyLabel.Location = new System.Drawing.Point(3, 31);
            this.stopFrequencyLabel.Name = "stopFrequencyLabel";
            this.stopFrequencyLabel.Size = new System.Drawing.Size(238, 31);
            this.stopFrequencyLabel.TabIndex = 7;
            this.stopFrequencyLabel.Text = "Stop Frequency (Hz)";
            this.stopFrequencyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frequencyIncrementLabel
            // 
            this.frequencyIncrementLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frequencyIncrementLabel.AutoSize = true;
            this.frequencyIncrementLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.frequencyIncrementLabel.Location = new System.Drawing.Point(3, 62);
            this.frequencyIncrementLabel.Name = "frequencyIncrementLabel";
            this.frequencyIncrementLabel.Size = new System.Drawing.Size(238, 31);
            this.frequencyIncrementLabel.TabIndex = 8;
            this.frequencyIncrementLabel.Text = "Frequency Increment";
            this.frequencyIncrementLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sweepTypeComboBox
            // 
            this.sweepTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sweepTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sweepTypeComboBox.FormattingEnabled = true;
            this.sweepTypeComboBox.Items.AddRange(new object[] {
            "Up",
            "Down",
            "Up Down",
            "Down Up"});
            this.sweepTypeComboBox.Location = new System.Drawing.Point(247, 3);
            this.sweepTypeComboBox.Name = "sweepTypeComboBox";
            this.sweepTypeComboBox.Size = new System.Drawing.Size(239, 33);
            this.sweepTypeComboBox.TabIndex = 9;
            // 
            // stopFreqTextBox
            // 
            this.stopFreqTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stopFreqTextBox.Location = new System.Drawing.Point(247, 34);
            this.stopFreqTextBox.Name = "stopFreqTextBox";
            this.stopFreqTextBox.Size = new System.Drawing.Size(239, 30);
            this.stopFreqTextBox.TabIndex = 10;
            this.stopFreqTextBox.Text = "2000";
            this.stopFreqTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frequencyIncrementTextBox
            // 
            this.frequencyIncrementTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frequencyIncrementTextBox.Location = new System.Drawing.Point(247, 65);
            this.frequencyIncrementTextBox.Name = "frequencyIncrementTextBox";
            this.frequencyIncrementTextBox.Size = new System.Drawing.Size(239, 30);
            this.frequencyIncrementTextBox.TabIndex = 11;
            this.frequencyIncrementTextBox.Text = "10";
            this.frequencyIncrementTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // timeIncrementTextBox
            // 
            this.timeIncrementTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeIncrementTextBox.Location = new System.Drawing.Point(247, 96);
            this.timeIncrementTextBox.Name = "timeIncrementTextBox";
            this.timeIncrementTextBox.Size = new System.Drawing.Size(239, 30);
            this.timeIncrementTextBox.TabIndex = 12;
            this.timeIncrementTextBox.Text = "10";
            this.timeIncrementTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // startFrequencyLabel
            // 
            this.startFrequencyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startFrequencyLabel.AutoSize = true;
            this.startFrequencyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.startFrequencyLabel.Location = new System.Drawing.Point(3, 131);
            this.startFrequencyLabel.Name = "startFrequencyLabel";
            this.startFrequencyLabel.Size = new System.Drawing.Size(489, 58);
            this.startFrequencyLabel.TabIndex = 27;
            this.startFrequencyLabel.Text = "Start Frequency (Hz)";
            this.startFrequencyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // peakToPeakVoltageLabel
            // 
            this.peakToPeakVoltageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.peakToPeakVoltageLabel.AutoSize = true;
            this.peakToPeakVoltageLabel.Location = new System.Drawing.Point(3, 189);
            this.peakToPeakVoltageLabel.Name = "peakToPeakVoltageLabel";
            this.peakToPeakVoltageLabel.Size = new System.Drawing.Size(489, 53);
            this.peakToPeakVoltageLabel.TabIndex = 31;
            this.peakToPeakVoltageLabel.Text = "Peak to Peak Voltage (mV)";
            this.peakToPeakVoltageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Start
            // 
            this.Start.Controls.Add(this.Start_button);
            this.Start.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Start.Location = new System.Drawing.Point(0, 0);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(639, 523);
            this.Start.TabIndex = 1;
            // 
            // Start_button
            // 
            this.Start_button.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Start_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.Start_button.Location = new System.Drawing.Point(250, 226);
            this.Start_button.Name = "Start_button";
            this.Start_button.Size = new System.Drawing.Size(108, 68);
            this.Start_button.TabIndex = 0;
            this.Start_button.Text = "START";
            this.Start_button.UseVisualStyleBackColor = true;
            this.Start_button.Click += new System.EventHandler(this.Start_button_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Text files (*.txt)|*.txt| All files (*.*)|*.*";
            this.openFileDialog.Title = "Select an arbitrary waveform file";
            // 
            // AWG_SIGGEN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 523);
            this.Controls.Add(this.controls);
            this.Controls.Add(this.Start);
            this.Name = "AWG_SIGGEN";
            this.Text = "PicoScope 3000 Series (A API) Signal Generator Example";
            this.controls.ResumeLayout(false);
            this.controls.PerformLayout();
            this.SweepController.ResumeLayout(false);
            this.SweepController.PerformLayout();
            this.Start.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel controls;
        internal System.Windows.Forms.CheckBox sigToAWG;
        internal System.Windows.Forms.TextBox fileNameTextBox;
        internal System.Windows.Forms.Label awgLabel;
        internal System.Windows.Forms.TextBox startFrequencyTextBox;
        internal System.Windows.Forms.Label startFrequencyLabel;
        internal System.Windows.Forms.TextBox awgFileInfoLabel;
        internal System.Windows.Forms.Button updateButton;
        internal System.Windows.Forms.TextBox peatkToPeakVoltageTextBox;
        internal System.Windows.Forms.Label peakToPeakVoltageLabel;
        internal System.Windows.Forms.TextBox offsetVoltageTextBox;
        internal System.Windows.Forms.CheckBox sweepCheckBox;
        internal System.Windows.Forms.Label offsetVoltageLabel;
        private System.Windows.Forms.ComboBox signalTypeComboBox;
        private System.Windows.Forms.TableLayoutPanel SweepController;
        internal System.Windows.Forms.Label incrementTimeIntervalLabel;
        internal System.Windows.Forms.Label sweepTypeLabel;
        internal System.Windows.Forms.Label stopFrequencyLabel;
        internal System.Windows.Forms.Label frequencyIncrementLabel;
        internal System.Windows.Forms.ComboBox sweepTypeComboBox;
        internal System.Windows.Forms.TextBox stopFreqTextBox;
        internal System.Windows.Forms.TextBox frequencyIncrementTextBox;
        internal System.Windows.Forms.TextBox timeIncrementTextBox;
        private System.Windows.Forms.Panel Start;
        private System.Windows.Forms.Button Start_button;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}

