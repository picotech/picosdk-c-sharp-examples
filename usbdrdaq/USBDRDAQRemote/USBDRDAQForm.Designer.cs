namespace DrDAQRemote
{
    partial class USBDRDAQForm
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
            this.components = new System.ComponentModel.Container();
            this.refreshrate = new System.Windows.Forms.Timer(this.components);
            this.tabDevice1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.device3Serial = new System.Windows.Forms.TextBox();
            this.nameDevice3 = new System.Windows.Forms.TextBox();
            this.device2Serial = new System.Windows.Forms.TextBox();
            this.nameDevice2 = new System.Windows.Forms.TextBox();
            this.device1Serial = new System.Windows.Forms.TextBox();
            this.nameDevice1 = new System.Windows.Forms.TextBox();
            this.runDevice3 = new System.Windows.Forms.CheckBox();
            this.settingsDevice3 = new System.Windows.Forms.CheckedListBox();
            this.runDevice2 = new System.Windows.Forms.CheckBox();
            this.settingsDevice2 = new System.Windows.Forms.CheckedListBox();
            this.runDevice1 = new System.Windows.Forms.CheckBox();
            this.settingsDevice1 = new System.Windows.Forms.CheckedListBox();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.serverActive = new System.Windows.Forms.CheckBox();
            this.labelRate = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.serverAddress = new System.Windows.Forms.Label();
            this.serverAddr = new System.Windows.Forms.TextBox();
            this.tabOverview = new System.Windows.Forms.TabPage();
            this.checkBoxRunning = new System.Windows.Forms.CheckBox();
            this.logBox = new System.Windows.Forms.TextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabDevice1.SuspendLayout();
            this.tabSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabOverview.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // refreshrate
            // 
            this.refreshrate.Enabled = true;
            this.refreshrate.Interval = 20000;
            this.refreshrate.Tick += new System.EventHandler(this.refreshrate_Tick);
            // 
            // tabDevice1
            // 
            this.tabDevice1.Controls.Add(this.label2);
            this.tabDevice1.Controls.Add(this.label1);
            this.tabDevice1.Controls.Add(this.device3Serial);
            this.tabDevice1.Controls.Add(this.nameDevice3);
            this.tabDevice1.Controls.Add(this.device2Serial);
            this.tabDevice1.Controls.Add(this.nameDevice2);
            this.tabDevice1.Controls.Add(this.device1Serial);
            this.tabDevice1.Controls.Add(this.nameDevice1);
            this.tabDevice1.Controls.Add(this.runDevice3);
            this.tabDevice1.Controls.Add(this.settingsDevice3);
            this.tabDevice1.Controls.Add(this.runDevice2);
            this.tabDevice1.Controls.Add(this.settingsDevice2);
            this.tabDevice1.Controls.Add(this.runDevice1);
            this.tabDevice1.Controls.Add(this.settingsDevice1);
            this.tabDevice1.Location = new System.Drawing.Point(4, 22);
            this.tabDevice1.Name = "tabDevice1";
            this.tabDevice1.Padding = new System.Windows.Forms.Padding(3);
            this.tabDevice1.Size = new System.Drawing.Size(422, 215);
            this.tabDevice1.TabIndex = 2;
            this.tabDevice1.Text = "Device Settings";
            this.tabDevice1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(343, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Serial Number";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(343, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "XML Tag";
            // 
            // device3Serial
            // 
            this.device3Serial.Enabled = false;
            this.device3Serial.Location = new System.Drawing.Point(239, 30);
            this.device3Serial.Name = "device3Serial";
            this.device3Serial.Size = new System.Drawing.Size(100, 20);
            this.device3Serial.TabIndex = 17;
            // 
            // nameDevice3
            // 
            this.nameDevice3.Enabled = false;
            this.nameDevice3.Location = new System.Drawing.Point(239, 4);
            this.nameDevice3.Name = "nameDevice3";
            this.nameDevice3.Size = new System.Drawing.Size(100, 20);
            this.nameDevice3.TabIndex = 15;
            this.nameDevice3.Text = "DrDaq3";
            // 
            // device2Serial
            // 
            this.device2Serial.Enabled = false;
            this.device2Serial.Location = new System.Drawing.Point(124, 30);
            this.device2Serial.Name = "device2Serial";
            this.device2Serial.Size = new System.Drawing.Size(97, 20);
            this.device2Serial.TabIndex = 12;
            // 
            // nameDevice2
            // 
            this.nameDevice2.Enabled = false;
            this.nameDevice2.Location = new System.Drawing.Point(124, 4);
            this.nameDevice2.Name = "nameDevice2";
            this.nameDevice2.Size = new System.Drawing.Size(97, 20);
            this.nameDevice2.TabIndex = 10;
            this.nameDevice2.Text = "DrDaq2";
            // 
            // device1Serial
            // 
            this.device1Serial.Enabled = false;
            this.device1Serial.Location = new System.Drawing.Point(10, 30);
            this.device1Serial.Name = "device1Serial";
            this.device1Serial.Size = new System.Drawing.Size(97, 20);
            this.device1Serial.TabIndex = 8;
            // 
            // nameDevice1
            // 
            this.nameDevice1.Enabled = false;
            this.nameDevice1.Location = new System.Drawing.Point(10, 4);
            this.nameDevice1.Name = "nameDevice1";
            this.nameDevice1.Size = new System.Drawing.Size(97, 20);
            this.nameDevice1.TabIndex = 5;
            this.nameDevice1.Text = "DrDaq1";
            // 
            // runDevice3
            // 
            this.runDevice3.AutoSize = true;
            this.runDevice3.Enabled = false;
            this.runDevice3.Location = new System.Drawing.Point(239, 189);
            this.runDevice3.Name = "runDevice3";
            this.runDevice3.Size = new System.Drawing.Size(65, 17);
            this.runDevice3.TabIndex = 16;
            this.runDevice3.Text = "Enabled";
            this.runDevice3.UseVisualStyleBackColor = true;
            // 
            // settingsDevice3
            // 
            this.settingsDevice3.CheckOnClick = true;
            this.settingsDevice3.Enabled = false;
            this.settingsDevice3.FormattingEnabled = true;
            this.settingsDevice3.Items.AddRange(new object[] {
            "Light Level",
            "Inside Temp",
            "Sound Level",
            "pH Sensor",
            "Scope",
            "External 1",
            "External 2",
            "External 3"});
            this.settingsDevice3.Location = new System.Drawing.Point(239, 59);
            this.settingsDevice3.Name = "settingsDevice3";
            this.settingsDevice3.Size = new System.Drawing.Size(96, 124);
            this.settingsDevice3.TabIndex = 14;
            // 
            // runDevice2
            // 
            this.runDevice2.AutoSize = true;
            this.runDevice2.Enabled = false;
            this.runDevice2.Location = new System.Drawing.Point(124, 189);
            this.runDevice2.Name = "runDevice2";
            this.runDevice2.Size = new System.Drawing.Size(65, 17);
            this.runDevice2.TabIndex = 13;
            this.runDevice2.Text = "Enabled";
            this.runDevice2.UseVisualStyleBackColor = true;
            // 
            // settingsDevice2
            // 
            this.settingsDevice2.CheckOnClick = true;
            this.settingsDevice2.Enabled = false;
            this.settingsDevice2.FormattingEnabled = true;
            this.settingsDevice2.Items.AddRange(new object[] {
            "Light Level",
            "Inside Temp",
            "Sound Level",
            "pH Sensor",
            "Scope",
            "External 1",
            "External 2",
            "External 3"});
            this.settingsDevice2.Location = new System.Drawing.Point(124, 59);
            this.settingsDevice2.Name = "settingsDevice2";
            this.settingsDevice2.Size = new System.Drawing.Size(97, 124);
            this.settingsDevice2.TabIndex = 9;
            // 
            // runDevice1
            // 
            this.runDevice1.AutoSize = true;
            this.runDevice1.Enabled = false;
            this.runDevice1.Location = new System.Drawing.Point(10, 189);
            this.runDevice1.Name = "runDevice1";
            this.runDevice1.Size = new System.Drawing.Size(65, 17);
            this.runDevice1.TabIndex = 6;
            this.runDevice1.Text = "Enabled";
            this.runDevice1.UseVisualStyleBackColor = true;
            // 
            // settingsDevice1
            // 
            this.settingsDevice1.CheckOnClick = true;
            this.settingsDevice1.Enabled = false;
            this.settingsDevice1.FormattingEnabled = true;
            this.settingsDevice1.Items.AddRange(new object[] {
            "Light Level",
            "Inside Temp",
            "Sound Level",
            "pH Sensor",
            "Scope",
            "External 1",
            "External 2",
            "External 3"});
            this.settingsDevice1.Location = new System.Drawing.Point(10, 59);
            this.settingsDevice1.Name = "settingsDevice1";
            this.settingsDevice1.Size = new System.Drawing.Size(97, 124);
            this.settingsDevice1.TabIndex = 3;
            this.settingsDevice1.SelectedIndexChanged += new System.EventHandler(this.settingsDevice1_SelectedIndexChanged);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.serverActive);
            this.tabSettings.Controls.Add(this.labelRate);
            this.tabSettings.Controls.Add(this.numericUpDown1);
            this.tabSettings.Controls.Add(this.serverAddress);
            this.tabSettings.Controls.Add(this.serverAddr);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(422, 215);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Server Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // serverActive
            // 
            this.serverActive.AutoSize = true;
            this.serverActive.Location = new System.Drawing.Point(10, 33);
            this.serverActive.Name = "serverActive";
            this.serverActive.Size = new System.Drawing.Size(90, 17);
            this.serverActive.TabIndex = 5;
            this.serverActive.Text = "Server Active";
            this.serverActive.UseVisualStyleBackColor = true;
            // 
            // labelRate
            // 
            this.labelRate.AutoSize = true;
            this.labelRate.Location = new System.Drawing.Point(3, 74);
            this.labelRate.Name = "labelRate";
            this.labelRate.Size = new System.Drawing.Size(70, 13);
            this.labelRate.TabIndex = 4;
            this.labelRate.Text = "Refresh Rate";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown1.Location = new System.Drawing.Point(80, 72);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(47, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // serverAddress
            // 
            this.serverAddress.AutoSize = true;
            this.serverAddress.Location = new System.Drawing.Point(3, 10);
            this.serverAddress.Name = "serverAddress";
            this.serverAddress.Size = new System.Drawing.Size(38, 13);
            this.serverAddress.TabIndex = 1;
            this.serverAddress.Text = "Server";
            // 
            // serverAddr
            // 
            this.serverAddr.Location = new System.Drawing.Point(80, 7);
            this.serverAddr.MaxLength = 100;
            this.serverAddr.Name = "serverAddr";
            this.serverAddr.Size = new System.Drawing.Size(336, 20);
            this.serverAddr.TabIndex = 0;
            // 
            // tabOverview
            // 
            this.tabOverview.Controls.Add(this.checkBoxRunning);
            this.tabOverview.Controls.Add(this.logBox);
            this.tabOverview.Location = new System.Drawing.Point(4, 22);
            this.tabOverview.Name = "tabOverview";
            this.tabOverview.Padding = new System.Windows.Forms.Padding(3);
            this.tabOverview.Size = new System.Drawing.Size(422, 215);
            this.tabOverview.TabIndex = 0;
            this.tabOverview.Text = "Overview";
            this.tabOverview.UseVisualStyleBackColor = true;
            // 
            // checkBoxRunning
            // 
            this.checkBoxRunning.AutoSize = true;
            this.checkBoxRunning.Location = new System.Drawing.Point(10, 6);
            this.checkBoxRunning.Name = "checkBoxRunning";
            this.checkBoxRunning.Size = new System.Drawing.Size(94, 17);
            this.checkBoxRunning.TabIndex = 12;
            this.checkBoxRunning.Text = "Devices Open";
            this.checkBoxRunning.UseVisualStyleBackColor = true;
            this.checkBoxRunning.CheckedChanged += new System.EventHandler(this.checkBoxRunning_CheckedChanged);
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(10, 38);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(403, 170);
            this.logBox.TabIndex = 11;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabOverview);
            this.tabControl.Controls.Add(this.tabSettings);
            this.tabControl.Controls.Add(this.tabDevice1);
            this.tabControl.Location = new System.Drawing.Point(-2, 1);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(430, 241);
            this.tabControl.TabIndex = 1;
            // 
            // USBDRDAQForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 243);
            this.Controls.Add(this.tabControl);
            this.Name = "USBDRDAQForm";
            this.Text = "USB DrDAQ Remote";
            this.Load += new System.EventHandler(this.USBDRDAQForm_Load);
            this.tabDevice1.ResumeLayout(false);
            this.tabDevice1.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabOverview.ResumeLayout(false);
            this.tabOverview.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer refreshrate;
        private System.Windows.Forms.TabPage tabDevice1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox device3Serial;
        private System.Windows.Forms.TextBox nameDevice3;
        private System.Windows.Forms.TextBox device2Serial;
        private System.Windows.Forms.TextBox nameDevice2;
        private System.Windows.Forms.TextBox device1Serial;
        private System.Windows.Forms.TextBox nameDevice1;
        private System.Windows.Forms.CheckBox runDevice3;
        private System.Windows.Forms.CheckedListBox settingsDevice3;
        private System.Windows.Forms.CheckBox runDevice2;
        private System.Windows.Forms.CheckedListBox settingsDevice2;
        private System.Windows.Forms.CheckBox runDevice1;
        private System.Windows.Forms.CheckedListBox settingsDevice1;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.CheckBox serverActive;
        private System.Windows.Forms.Label labelRate;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label serverAddress;
        private System.Windows.Forms.TextBox serverAddr;
        private System.Windows.Forms.TabPage tabOverview;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.CheckBox checkBoxRunning;
    }
}

