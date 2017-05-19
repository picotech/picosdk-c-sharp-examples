/******************************************************************************
 *
 * Filename: PS4000ASigGen.Designer.cs
 * 
 * Copyright (C) 2014 - 2017 Pico Technology Ltd. See LICENSE file for terms.
 *
 ******************************************************************************/

namespace PS4000ASigGen
{
    partial class PS4000ASigGen
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
            this.awg_label2 = new System.Windows.Forms.TextBox();
            this.SIGtoAWG = new System.Windows.Forms.CheckBox();
            this.awg_label = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Update_button = new System.Windows.Forms.Button();
            this.pk_pk = new System.Windows.Forms.TextBox();
            this.start_freq = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.offsetvoltage = new System.Windows.Forms.TextBox();
            this.Sweep = new System.Windows.Forms.CheckBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.file_name = new System.Windows.Forms.TextBox();
            this.signal_type = new System.Windows.Forms.ComboBox();
            this.SweepController = new System.Windows.Forms.TableLayoutPanel();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.sweep_type = new System.Windows.Forms.ComboBox();
            this.stop_freq = new System.Windows.Forms.TextBox();
            this.freq_incre = new System.Windows.Forms.TextBox();
            this.time_incre = new System.Windows.Forms.TextBox();
            this.Start = new System.Windows.Forms.Panel();
            this.Start_button = new System.Windows.Forms.Button();
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
            this.controls.Controls.Add(this.awg_label2, 0, 6);
            this.controls.Controls.Add(this.SIGtoAWG, 1, 0);
            this.controls.Controls.Add(this.awg_label, 0, 0);
            this.controls.Controls.Add(this.Label2, 0, 2);
            this.controls.Controls.Add(this.Update_button, 1, 6);
            this.controls.Controls.Add(this.pk_pk, 1, 3);
            this.controls.Controls.Add(this.start_freq, 1, 2);
            this.controls.Controls.Add(this.Label4, 0, 3);
            this.controls.Controls.Add(this.offsetvoltage, 1, 4);
            this.controls.Controls.Add(this.Sweep, 1, 5);
            this.controls.Controls.Add(this.Label1, 0, 4);
            this.controls.Controls.Add(this.file_name, 0, 1);
            this.controls.Controls.Add(this.signal_type, 1, 1);
            this.controls.Controls.Add(this.SweepController, 0, 5);
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
            // awg_label2
            // 
            this.awg_label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.awg_label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.awg_label2.Location = new System.Drawing.Point(3, 412);
            this.awg_label2.Multiline = true;
            this.awg_label2.Name = "awg_label2";
            this.awg_label2.ReadOnly = true;
            this.awg_label2.Size = new System.Drawing.Size(489, 108);
            this.awg_label2.TabIndex = 36;
            this.awg_label2.Text = "Please ensure that the file has only one value per line, in the range -32768 (min" +
    ") to 32767 (max).";
            this.awg_label2.Visible = false;
            // 
            // SIGtoAWG
            // 
            this.SIGtoAWG.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SIGtoAWG.AutoSize = true;
            this.SIGtoAWG.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.SIGtoAWG.Location = new System.Drawing.Point(498, 3);
            this.SIGtoAWG.Name = "SIGtoAWG";
            this.SIGtoAWG.Size = new System.Drawing.Size(138, 68);
            this.SIGtoAWG.TabIndex = 2;
            this.SIGtoAWG.Text = "AWG";
            this.SIGtoAWG.UseVisualStyleBackColor = true;
            this.SIGtoAWG.CheckedChanged += new System.EventHandler(this.SIGtoAWG_CheckedChanged);
            // 
            // awg_label
            // 
            this.awg_label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.awg_label.AutoSize = true;
            this.awg_label.Location = new System.Drawing.Point(3, 0);
            this.awg_label.Name = "awg_label";
            this.awg_label.Size = new System.Drawing.Size(489, 74);
            this.awg_label.TabIndex = 26;
            this.awg_label.Text = "Please enter filename (don\'t forget the .txt extension):";
            this.awg_label.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.awg_label.Visible = false;
            // 
            // Label2
            // 
            this.Label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.Label2.Location = new System.Drawing.Point(3, 131);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(489, 58);
            this.Label2.TabIndex = 27;
            this.Label2.Text = "Start Frequency (Hz)";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Update_button
            // 
            this.Update_button.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Update_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.Update_button.Location = new System.Drawing.Point(498, 483);
            this.Update_button.Name = "Update_button";
            this.Update_button.Size = new System.Drawing.Size(138, 37);
            this.Update_button.TabIndex = 29;
            this.Update_button.Text = "Update";
            this.Update_button.UseVisualStyleBackColor = true;
            this.Update_button.Click += new System.EventHandler(this.Update_button_Click);
            // 
            // pk_pk
            // 
            this.pk_pk.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pk_pk.Location = new System.Drawing.Point(498, 192);
            this.pk_pk.Name = "pk_pk";
            this.pk_pk.Size = new System.Drawing.Size(138, 30);
            this.pk_pk.TabIndex = 30;
            this.pk_pk.Text = "2000";
            this.pk_pk.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // start_freq
            // 
            this.start_freq.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.start_freq.Location = new System.Drawing.Point(498, 134);
            this.start_freq.Name = "start_freq";
            this.start_freq.Size = new System.Drawing.Size(138, 30);
            this.start_freq.TabIndex = 28;
            this.start_freq.Text = "1000";
            this.start_freq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label4
            // 
            this.Label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(3, 189);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(489, 53);
            this.Label4.TabIndex = 31;
            this.Label4.Text = "PktoPk (mV)";
            this.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // offsetvoltage
            // 
            this.offsetvoltage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.offsetvoltage.Location = new System.Drawing.Point(498, 245);
            this.offsetvoltage.Name = "offsetvoltage";
            this.offsetvoltage.Size = new System.Drawing.Size(138, 30);
            this.offsetvoltage.TabIndex = 33;
            this.offsetvoltage.Text = "0";
            this.offsetvoltage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Sweep
            // 
            this.Sweep.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Sweep.AutoSize = true;
            this.Sweep.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.Sweep.Location = new System.Drawing.Point(498, 279);
            this.Sweep.Name = "Sweep";
            this.Sweep.Size = new System.Drawing.Size(138, 127);
            this.Sweep.TabIndex = 34;
            this.Sweep.Text = "Sweep Mode";
            this.Sweep.UseVisualStyleBackColor = true;
            this.Sweep.CheckedChanged += new System.EventHandler(this.Sweep_CheckedChanged);
            // 
            // Label1
            // 
            this.Label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(3, 242);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(489, 34);
            this.Label1.TabIndex = 32;
            this.Label1.Text = "Offset (mV)";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // file_name
            // 
            this.file_name.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.file_name.Location = new System.Drawing.Point(3, 77);
            this.file_name.Name = "file_name";
            this.file_name.Size = new System.Drawing.Size(489, 30);
            this.file_name.TabIndex = 25;
            this.file_name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // signal_type
            // 
            this.signal_type.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.signal_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.signal_type.FormattingEnabled = true;
            this.signal_type.Items.AddRange(new object[] {
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
            this.signal_type.Location = new System.Drawing.Point(498, 77);
            this.signal_type.Name = "signal_type";
            this.signal_type.Size = new System.Drawing.Size(138, 33);
            this.signal_type.TabIndex = 37;
            this.signal_type.SelectedIndexChanged += new System.EventHandler(this.signal_type_SelectedIndexChanged);
            // 
            // SweepController
            // 
            this.SweepController.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SweepController.ColumnCount = 2;
            this.SweepController.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SweepController.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SweepController.Controls.Add(this.Label8, 0, 3);
            this.SweepController.Controls.Add(this.Label5, 0, 0);
            this.SweepController.Controls.Add(this.Label7, 0, 1);
            this.SweepController.Controls.Add(this.Label6, 0, 2);
            this.SweepController.Controls.Add(this.sweep_type, 1, 0);
            this.SweepController.Controls.Add(this.stop_freq, 1, 1);
            this.SweepController.Controls.Add(this.freq_incre, 1, 2);
            this.SweepController.Controls.Add(this.time_incre, 1, 3);
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
            // Label8
            // 
            this.Label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.Label8.Location = new System.Drawing.Point(3, 93);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(238, 34);
            this.Label8.TabIndex = 13;
            this.Label8.Text = "Increment Time Interval (s)";
            this.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label5
            // 
            this.Label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.Label5.Location = new System.Drawing.Point(3, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(238, 31);
            this.Label5.TabIndex = 5;
            this.Label5.Text = "Sweep Type";
            this.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label7
            // 
            this.Label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.Label7.Location = new System.Drawing.Point(3, 31);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(238, 31);
            this.Label7.TabIndex = 7;
            this.Label7.Text = "Stop Frequency (Hz)";
            this.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label6
            // 
            this.Label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.Label6.Location = new System.Drawing.Point(3, 62);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(238, 31);
            this.Label6.TabIndex = 8;
            this.Label6.Text = "Frequency Increment";
            this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sweep_type
            // 
            this.sweep_type.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sweep_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sweep_type.FormattingEnabled = true;
            this.sweep_type.Items.AddRange(new object[] {
            "Up",
            "Down",
            "Up Down",
            "Down Up"});
            this.sweep_type.Location = new System.Drawing.Point(247, 3);
            this.sweep_type.Name = "sweep_type";
            this.sweep_type.Size = new System.Drawing.Size(239, 33);
            this.sweep_type.TabIndex = 9;
            // 
            // stop_freq
            // 
            this.stop_freq.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stop_freq.Location = new System.Drawing.Point(247, 34);
            this.stop_freq.Name = "stop_freq";
            this.stop_freq.Size = new System.Drawing.Size(239, 30);
            this.stop_freq.TabIndex = 10;
            this.stop_freq.Text = "2000";
            this.stop_freq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // freq_incre
            // 
            this.freq_incre.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.freq_incre.Location = new System.Drawing.Point(247, 65);
            this.freq_incre.Name = "freq_incre";
            this.freq_incre.Size = new System.Drawing.Size(239, 30);
            this.freq_incre.TabIndex = 11;
            this.freq_incre.Text = "10";
            this.freq_incre.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // time_incre
            // 
            this.time_incre.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.time_incre.Location = new System.Drawing.Point(247, 96);
            this.time_incre.Name = "time_incre";
            this.time_incre.Size = new System.Drawing.Size(239, 30);
            this.time_incre.TabIndex = 12;
            this.time_incre.Text = "10";
            this.time_incre.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // PS4000ASigGen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 523);
            this.Controls.Add(this.controls);
            this.Controls.Add(this.Start);
            this.Name = "PS4000ASigGen";
            this.Text = "PicoScope 4000 Series (A API) Signal Generator Example";
            this.controls.ResumeLayout(false);
            this.controls.PerformLayout();
            this.SweepController.ResumeLayout(false);
            this.SweepController.PerformLayout();
            this.Start.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel controls;
        internal System.Windows.Forms.CheckBox SIGtoAWG;
        internal System.Windows.Forms.TextBox file_name;
        internal System.Windows.Forms.Label awg_label;
        internal System.Windows.Forms.TextBox start_freq;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox awg_label2;
        internal System.Windows.Forms.Button Update_button;
        internal System.Windows.Forms.TextBox pk_pk;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TextBox offsetvoltage;
        internal System.Windows.Forms.CheckBox Sweep;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.ComboBox signal_type;
        private System.Windows.Forms.TableLayoutPanel SweepController;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.ComboBox sweep_type;
        internal System.Windows.Forms.TextBox stop_freq;
        internal System.Windows.Forms.TextBox freq_incre;
        internal System.Windows.Forms.TextBox time_incre;
        private System.Windows.Forms.Panel Start;
        private System.Windows.Forms.Button Start_button;
    }
}

