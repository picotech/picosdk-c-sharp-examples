/******************************************************************************
 *
 * Filename: PS4000ABlockExample.Designer.cs
 * 
 * Copyright (C) 2014 - 2017 Pico Technology Ltd. See LICENSE file for terms.
 *
 ******************************************************************************/

namespace PS4000ABlockCaptureGui
{
    partial class BlockExample
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlockExample));
            this.start = new System.Windows.Forms.Button();
            this.Voltset = new System.Windows.Forms.Panel();
            this.Timebase = new System.Windows.Forms.Panel();
            this.TriggerSettings = new System.Windows.Forms.Panel();
            this.start_capture = new System.Windows.Forms.Panel();
            this.results = new System.Windows.Forms.Panel();
            this.chart = new System.Windows.Forms.Panel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.results_chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tables = new System.Windows.Forms.Button();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.Restart = new System.Windows.Forms.Button();
            this.display_change = new System.Windows.Forms.Button();
            this.ChannelA_t = new System.Windows.Forms.TextBox();
            this.ChannelB_t = new System.Windows.Forms.TextBox();
            this.ChannelC_t = new System.Windows.Forms.TextBox();
            this.ChannelE_t = new System.Windows.Forms.TextBox();
            this.ChannelF_t = new System.Windows.Forms.TextBox();
            this.ChannelG_t = new System.Windows.Forms.TextBox();
            this.ChannelH_t = new System.Windows.Forms.TextBox();
            this.ChannelA = new System.Windows.Forms.TextBox();
            this.ChannelB = new System.Windows.Forms.TextBox();
            this.ChannelC = new System.Windows.Forms.TextBox();
            this.ChannelD = new System.Windows.Forms.TextBox();
            this.ChannelE = new System.Windows.Forms.TextBox();
            this.ChannelF = new System.Windows.Forms.TextBox();
            this.ChannelH = new System.Windows.Forms.TextBox();
            this.ChannelG = new System.Windows.Forms.TextBox();
            this.ChannelD_t = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.capture_previous = new System.Windows.Forms.Button();
            this.DeviceInfo = new System.Windows.Forms.TextBox();
            this.start_cap = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.trig_previous = new System.Windows.Forms.Button();
            this.direction_text = new System.Windows.Forms.TextBox();
            this.Channel_text = new System.Windows.Forms.TextBox();
            this.Channel = new System.Windows.Forms.ComboBox();
            this.threshold_text = new System.Windows.Forms.TextBox();
            this.threshold = new System.Windows.Forms.TextBox();
            this.direction = new System.Windows.Forms.ComboBox();
            this.Trigger_enable = new System.Windows.Forms.CheckBox();
            this.trig_next = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.timebase_next = new System.Windows.Forms.Button();
            this.timebasevar = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.timebase_previous = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.volt_next = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ChannelH_volt = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ChannelG_volt = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ChannelF_volt = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ChannelE_volt = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ChannelD_volt = new System.Windows.Forms.ComboBox();
            this.ChannelA_volt = new System.Windows.Forms.ComboBox();
            this.ChannelC_volt = new System.Windows.Forms.ComboBox();
            this.ChannelB_volt = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Voltset.SuspendLayout();
            this.Timebase.SuspendLayout();
            this.TriggerSettings.SuspendLayout();
            this.start_capture.SuspendLayout();
            this.results.SuspendLayout();
            this.chart.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.results_chart)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // start
            // 
            this.start.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.start.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
            this.start.Location = new System.Drawing.Point(231, 244);
            this.start.MaximumSize = new System.Drawing.Size(250, 113);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(250, 113);
            this.start.TabIndex = 0;
            this.start.Text = "START";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // Voltset
            // 
            this.Voltset.Controls.Add(this.Timebase);
            this.Voltset.Controls.Add(this.tableLayoutPanel1);
            this.Voltset.Controls.Add(this.textBox1);
            this.Voltset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Voltset.Location = new System.Drawing.Point(0, 0);
            this.Voltset.Name = "Voltset";
            this.Voltset.Size = new System.Drawing.Size(725, 543);
            this.Voltset.TabIndex = 1;
            this.Voltset.Visible = false;
            // 
            // Timebase
            // 
            this.Timebase.Controls.Add(this.TriggerSettings);
            this.Timebase.Controls.Add(this.tableLayoutPanel2);
            this.Timebase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Timebase.Location = new System.Drawing.Point(0, 0);
            this.Timebase.Name = "Timebase";
            this.Timebase.Size = new System.Drawing.Size(725, 543);
            this.Timebase.TabIndex = 22;
            this.Timebase.Visible = false;
            // 
            // TriggerSettings
            // 
            this.TriggerSettings.Controls.Add(this.start_capture);
            this.TriggerSettings.Controls.Add(this.tableLayoutPanel3);
            this.TriggerSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TriggerSettings.Location = new System.Drawing.Point(0, 0);
            this.TriggerSettings.Name = "TriggerSettings";
            this.TriggerSettings.Size = new System.Drawing.Size(725, 543);
            this.TriggerSettings.TabIndex = 9;
            this.TriggerSettings.Visible = false;
            // 
            // start_capture
            // 
            this.start_capture.Controls.Add(this.results);
            this.start_capture.Controls.Add(this.tableLayoutPanel4);
            this.start_capture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.start_capture.Location = new System.Drawing.Point(0, 0);
            this.start_capture.Name = "start_capture";
            this.start_capture.Size = new System.Drawing.Size(725, 543);
            this.start_capture.TabIndex = 1;
            this.start_capture.Visible = false;
            // 
            // results
            // 
            this.results.Controls.Add(this.chart);
            this.results.Controls.Add(this.tableLayoutPanel5);
            this.results.Dock = System.Windows.Forms.DockStyle.Fill;
            this.results.Location = new System.Drawing.Point(0, 0);
            this.results.Name = "results";
            this.results.Size = new System.Drawing.Size(725, 543);
            this.results.TabIndex = 1;
            this.results.Visible = false;
            // 
            // chart
            // 
            this.chart.Controls.Add(this.tableLayoutPanel6);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(725, 543);
            this.chart.TabIndex = 1;
            this.chart.Visible = false;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.86207F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.13793F));
            this.tableLayoutPanel6.Controls.Add(this.results_chart, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.tables, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(725, 543);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // results_chart
            // 
            chartArea1.AxisX.Title = "Time (ns)";
            chartArea1.AxisY.Title = "Voltage (mV)";
            chartArea1.Name = "ChartArea1";
            this.results_chart.ChartAreas.Add(chartArea1);
            this.results_chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.results_chart.Legends.Add(legend1);
            this.results_chart.Location = new System.Drawing.Point(118, 3);
            this.results_chart.Name = "results_chart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Blue;
            series1.Enabled = false;
            series1.Legend = "Legend1";
            series1.Name = "A";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(39)))), ((int)(((byte)(50)))));
            series2.Enabled = false;
            series2.Legend = "Legend1";
            series2.Name = "B";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(168)))), ((int)(((byte)(79)))));
            series3.Enabled = false;
            series3.Legend = "Legend1";
            series3.Name = "C";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.Yellow;
            series4.Enabled = false;
            series4.Legend = "Legend1";
            series4.Name = "D";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Color = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(36)))), ((int)(((byte)(130)))));
            series5.Enabled = false;
            series5.Legend = "Legend1";
            series5.Name = "E";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Color = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(149)))), ((int)(((byte)(151)))));
            series6.Enabled = false;
            series6.Legend = "Legend1";
            series6.Name = "F";
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(172)))), ((int)(((byte)(247)))));
            series7.Enabled = false;
            series7.Legend = "Legend1";
            series7.Name = "G";
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.Color = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(0)))), ((int)(((byte)(98)))));
            series8.Enabled = false;
            series8.Legend = "Legend1";
            series8.Name = "H";
            this.results_chart.Series.Add(series1);
            this.results_chart.Series.Add(series2);
            this.results_chart.Series.Add(series3);
            this.results_chart.Series.Add(series4);
            this.results_chart.Series.Add(series5);
            this.results_chart.Series.Add(series6);
            this.results_chart.Series.Add(series7);
            this.results_chart.Series.Add(series8);
            this.results_chart.Size = new System.Drawing.Size(604, 537);
            this.results_chart.TabIndex = 1;
            // 
            // tables
            // 
            this.tables.Dock = System.Windows.Forms.DockStyle.Top;
            this.tables.Location = new System.Drawing.Point(3, 3);
            this.tables.Name = "tables";
            this.tables.Size = new System.Drawing.Size(109, 33);
            this.tables.TabIndex = 2;
            this.tables.Text = "Tables";
            this.tables.UseVisualStyleBackColor = true;
            this.tables.Click += new System.EventHandler(this.tables_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 5;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.Controls.Add(this.Restart, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.display_change, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.ChannelA_t, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.ChannelB_t, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.ChannelC_t, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.ChannelE_t, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.ChannelF_t, 2, 2);
            this.tableLayoutPanel5.Controls.Add(this.ChannelG_t, 3, 2);
            this.tableLayoutPanel5.Controls.Add(this.ChannelH_t, 4, 2);
            this.tableLayoutPanel5.Controls.Add(this.ChannelA, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.ChannelB, 2, 1);
            this.tableLayoutPanel5.Controls.Add(this.ChannelC, 3, 1);
            this.tableLayoutPanel5.Controls.Add(this.ChannelD, 4, 1);
            this.tableLayoutPanel5.Controls.Add(this.ChannelE, 1, 3);
            this.tableLayoutPanel5.Controls.Add(this.ChannelF, 2, 3);
            this.tableLayoutPanel5.Controls.Add(this.ChannelH, 4, 3);
            this.tableLayoutPanel5.Controls.Add(this.ChannelG, 3, 3);
            this.tableLayoutPanel5.Controls.Add(this.ChannelD_t, 4, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 4;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.366483F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42.17311F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.550644F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 43.27808F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(725, 543);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // Restart
            // 
            this.Restart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Restart.Location = new System.Drawing.Point(3, 440);
            this.Restart.MaximumSize = new System.Drawing.Size(100, 100);
            this.Restart.MinimumSize = new System.Drawing.Size(90, 100);
            this.Restart.Name = "Restart";
            this.Restart.Size = new System.Drawing.Size(100, 100);
            this.Restart.TabIndex = 19;
            this.Restart.Text = "Restart";
            this.Restart.UseVisualStyleBackColor = true;
            this.Restart.Click += new System.EventHandler(this.Restart_Click);
            // 
            // display_change
            // 
            this.display_change.Dock = System.Windows.Forms.DockStyle.Left;
            this.display_change.Location = new System.Drawing.Point(3, 3);
            this.display_change.MaximumSize = new System.Drawing.Size(100, 100);
            this.display_change.Name = "display_change";
            this.display_change.Size = new System.Drawing.Size(100, 33);
            this.display_change.TabIndex = 17;
            this.display_change.Text = "Graph";
            this.display_change.UseVisualStyleBackColor = true;
            this.display_change.Click += new System.EventHandler(this.display_change_Click);
            // 
            // ChannelA_t
            // 
            this.ChannelA_t.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelA_t.Location = new System.Drawing.Point(148, 3);
            this.ChannelA_t.Name = "ChannelA_t";
            this.ChannelA_t.ReadOnly = true;
            this.ChannelA_t.Size = new System.Drawing.Size(139, 30);
            this.ChannelA_t.TabIndex = 20;
            this.ChannelA_t.Text = "Channel A";
            // 
            // ChannelB_t
            // 
            this.ChannelB_t.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelB_t.Location = new System.Drawing.Point(293, 3);
            this.ChannelB_t.Name = "ChannelB_t";
            this.ChannelB_t.ReadOnly = true;
            this.ChannelB_t.Size = new System.Drawing.Size(139, 30);
            this.ChannelB_t.TabIndex = 21;
            this.ChannelB_t.Text = "Channel B";
            // 
            // ChannelC_t
            // 
            this.ChannelC_t.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelC_t.Location = new System.Drawing.Point(438, 3);
            this.ChannelC_t.Name = "ChannelC_t";
            this.ChannelC_t.ReadOnly = true;
            this.ChannelC_t.Size = new System.Drawing.Size(139, 30);
            this.ChannelC_t.TabIndex = 22;
            this.ChannelC_t.Text = "Channel C";
            // 
            // ChannelE_t
            // 
            this.ChannelE_t.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelE_t.Location = new System.Drawing.Point(148, 270);
            this.ChannelE_t.Name = "ChannelE_t";
            this.ChannelE_t.ReadOnly = true;
            this.ChannelE_t.Size = new System.Drawing.Size(139, 30);
            this.ChannelE_t.TabIndex = 24;
            this.ChannelE_t.Text = "Channel E";
            // 
            // ChannelF_t
            // 
            this.ChannelF_t.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelF_t.Location = new System.Drawing.Point(293, 270);
            this.ChannelF_t.Name = "ChannelF_t";
            this.ChannelF_t.ReadOnly = true;
            this.ChannelF_t.Size = new System.Drawing.Size(139, 30);
            this.ChannelF_t.TabIndex = 25;
            this.ChannelF_t.Text = "Channel F";
            // 
            // ChannelG_t
            // 
            this.ChannelG_t.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelG_t.Location = new System.Drawing.Point(438, 270);
            this.ChannelG_t.Name = "ChannelG_t";
            this.ChannelG_t.ReadOnly = true;
            this.ChannelG_t.Size = new System.Drawing.Size(139, 30);
            this.ChannelG_t.TabIndex = 26;
            this.ChannelG_t.Text = "Channel G";
            // 
            // ChannelH_t
            // 
            this.ChannelH_t.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelH_t.Location = new System.Drawing.Point(583, 270);
            this.ChannelH_t.Name = "ChannelH_t";
            this.ChannelH_t.ReadOnly = true;
            this.ChannelH_t.Size = new System.Drawing.Size(139, 30);
            this.ChannelH_t.TabIndex = 27;
            this.ChannelH_t.Text = "Channel H";
            // 
            // ChannelA
            // 
            this.ChannelA.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelA.Location = new System.Drawing.Point(148, 42);
            this.ChannelA.Multiline = true;
            this.ChannelA.Name = "ChannelA";
            this.ChannelA.ReadOnly = true;
            this.ChannelA.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChannelA.Size = new System.Drawing.Size(139, 222);
            this.ChannelA.TabIndex = 28;
            // 
            // ChannelB
            // 
            this.ChannelB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelB.Location = new System.Drawing.Point(293, 42);
            this.ChannelB.Multiline = true;
            this.ChannelB.Name = "ChannelB";
            this.ChannelB.ReadOnly = true;
            this.ChannelB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChannelB.Size = new System.Drawing.Size(139, 222);
            this.ChannelB.TabIndex = 29;
            // 
            // ChannelC
            // 
            this.ChannelC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelC.Location = new System.Drawing.Point(438, 42);
            this.ChannelC.Multiline = true;
            this.ChannelC.Name = "ChannelC";
            this.ChannelC.ReadOnly = true;
            this.ChannelC.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChannelC.Size = new System.Drawing.Size(139, 222);
            this.ChannelC.TabIndex = 30;
            // 
            // ChannelD
            // 
            this.ChannelD.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelD.Location = new System.Drawing.Point(583, 42);
            this.ChannelD.Multiline = true;
            this.ChannelD.Name = "ChannelD";
            this.ChannelD.ReadOnly = true;
            this.ChannelD.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChannelD.Size = new System.Drawing.Size(139, 222);
            this.ChannelD.TabIndex = 31;
            // 
            // ChannelE
            // 
            this.ChannelE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelE.Location = new System.Drawing.Point(148, 310);
            this.ChannelE.Multiline = true;
            this.ChannelE.Name = "ChannelE";
            this.ChannelE.ReadOnly = true;
            this.ChannelE.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChannelE.Size = new System.Drawing.Size(139, 230);
            this.ChannelE.TabIndex = 32;
            // 
            // ChannelF
            // 
            this.ChannelF.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelF.Location = new System.Drawing.Point(293, 310);
            this.ChannelF.Multiline = true;
            this.ChannelF.Name = "ChannelF";
            this.ChannelF.ReadOnly = true;
            this.ChannelF.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChannelF.Size = new System.Drawing.Size(139, 230);
            this.ChannelF.TabIndex = 33;
            // 
            // ChannelH
            // 
            this.ChannelH.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelH.Location = new System.Drawing.Point(583, 310);
            this.ChannelH.Multiline = true;
            this.ChannelH.Name = "ChannelH";
            this.ChannelH.ReadOnly = true;
            this.ChannelH.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChannelH.Size = new System.Drawing.Size(139, 230);
            this.ChannelH.TabIndex = 34;
            // 
            // ChannelG
            // 
            this.ChannelG.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelG.Location = new System.Drawing.Point(438, 310);
            this.ChannelG.Multiline = true;
            this.ChannelG.Name = "ChannelG";
            this.ChannelG.ReadOnly = true;
            this.ChannelG.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChannelG.Size = new System.Drawing.Size(139, 230);
            this.ChannelG.TabIndex = 35;
            // 
            // ChannelD_t
            // 
            this.ChannelD_t.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelD_t.Location = new System.Drawing.Point(583, 3);
            this.ChannelD_t.Name = "ChannelD_t";
            this.ChannelD_t.ReadOnly = true;
            this.ChannelD_t.Size = new System.Drawing.Size(139, 30);
            this.ChannelD_t.TabIndex = 23;
            this.ChannelD_t.Text = "Channel D";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.capture_previous, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.DeviceInfo, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.start_cap, 1, 0);
            this.tableLayoutPanel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 34);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(725, 509);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // capture_previous
            // 
            this.capture_previous.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.capture_previous.Location = new System.Drawing.Point(3, 458);
            this.capture_previous.Name = "capture_previous";
            this.capture_previous.Size = new System.Drawing.Size(356, 48);
            this.capture_previous.TabIndex = 6;
            this.capture_previous.Text = "Previous";
            this.capture_previous.UseVisualStyleBackColor = true;
            this.capture_previous.Click += new System.EventHandler(this.capture_previous_Click);
            // 
            // DeviceInfo
            // 
            this.DeviceInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeviceInfo.Location = new System.Drawing.Point(3, 3);
            this.DeviceInfo.Multiline = true;
            this.DeviceInfo.Name = "DeviceInfo";
            this.DeviceInfo.ReadOnly = true;
            this.DeviceInfo.Size = new System.Drawing.Size(356, 248);
            this.DeviceInfo.TabIndex = 4;
            // 
            // start_cap
            // 
            this.start_cap.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.start_cap.Location = new System.Drawing.Point(365, 159);
            this.start_cap.Name = "start_cap";
            this.start_cap.Size = new System.Drawing.Size(357, 92);
            this.start_cap.TabIndex = 5;
            this.start_cap.Text = "Start";
            this.start_cap.UseVisualStyleBackColor = true;
            this.start_cap.Click += new System.EventHandler(this.start_cap_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel3.Controls.Add(this.trig_previous, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.direction_text, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.Channel_text, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.Channel, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.threshold_text, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.threshold, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.direction, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.Trigger_enable, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.trig_next, 2, 3);
            this.tableLayoutPanel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 155);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(725, 388);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // trig_previous
            // 
            this.trig_previous.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.trig_previous.Location = new System.Drawing.Point(3, 294);
            this.trig_previous.Name = "trig_previous";
            this.trig_previous.Size = new System.Drawing.Size(235, 91);
            this.trig_previous.TabIndex = 15;
            this.trig_previous.Text = "Previous";
            this.trig_previous.UseVisualStyleBackColor = true;
            this.trig_previous.Click += new System.EventHandler(this.trig_previous_Click);
            // 
            // direction_text
            // 
            this.direction_text.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.direction_text.Location = new System.Drawing.Point(3, 197);
            this.direction_text.Name = "direction_text";
            this.direction_text.ReadOnly = true;
            this.direction_text.Size = new System.Drawing.Size(235, 30);
            this.direction_text.TabIndex = 11;
            this.direction_text.Text = "Trigger Edge";
            // 
            // Channel_text
            // 
            this.Channel_text.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Channel_text.Location = new System.Drawing.Point(3, 3);
            this.Channel_text.Name = "Channel_text";
            this.Channel_text.ReadOnly = true;
            this.Channel_text.Size = new System.Drawing.Size(235, 30);
            this.Channel_text.TabIndex = 7;
            this.Channel_text.Text = "Channel";
            // 
            // Channel
            // 
            this.Channel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Channel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Channel.FormattingEnabled = true;
            this.Channel.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H"});
            this.Channel.Location = new System.Drawing.Point(244, 3);
            this.Channel.Name = "Channel";
            this.Channel.Size = new System.Drawing.Size(235, 33);
            this.Channel.TabIndex = 8;
            // 
            // threshold_text
            // 
            this.threshold_text.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.threshold_text.Location = new System.Drawing.Point(3, 100);
            this.threshold_text.Name = "threshold_text";
            this.threshold_text.ReadOnly = true;
            this.threshold_text.Size = new System.Drawing.Size(235, 30);
            this.threshold_text.TabIndex = 9;
            this.threshold_text.Text = "Threshold value (mV)";
            // 
            // threshold
            // 
            this.threshold.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.threshold.Location = new System.Drawing.Point(244, 100);
            this.threshold.Name = "threshold";
            this.threshold.Size = new System.Drawing.Size(235, 30);
            this.threshold.TabIndex = 10;
            this.threshold.Text = "1000";
            // 
            // direction
            // 
            this.direction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.direction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.direction.FormattingEnabled = true;
            this.direction.Items.AddRange(new object[] {
            "Above",
            "Below",
            "Rising",
            "Falling",
            "Rising or Falling"});
            this.direction.Location = new System.Drawing.Point(244, 197);
            this.direction.Name = "direction";
            this.direction.Size = new System.Drawing.Size(235, 33);
            this.direction.TabIndex = 12;
            // 
            // Trigger_enable
            // 
            this.Trigger_enable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Trigger_enable.AutoSize = true;
            this.Trigger_enable.Checked = true;
            this.Trigger_enable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Trigger_enable.Location = new System.Drawing.Point(485, 3);
            this.Trigger_enable.Name = "Trigger_enable";
            this.Trigger_enable.Size = new System.Drawing.Size(237, 91);
            this.Trigger_enable.TabIndex = 13;
            this.Trigger_enable.Text = "Trigger";
            this.Trigger_enable.UseVisualStyleBackColor = true;
            this.Trigger_enable.CheckedChanged += new System.EventHandler(this.Trigger_enable_CheckedChanged);
            // 
            // trig_next
            // 
            this.trig_next.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.trig_next.Location = new System.Drawing.Point(485, 294);
            this.trig_next.Name = "trig_next";
            this.trig_next.Size = new System.Drawing.Size(237, 91);
            this.trig_next.TabIndex = 14;
            this.trig_next.Text = "Next";
            this.trig_next.UseVisualStyleBackColor = true;
            this.trig_next.Click += new System.EventHandler(this.trig_next_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.textBox3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.timebase_next, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.timebasevar, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBox4, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.timebase_previous, 0, 2);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 102);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.00766F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.99234F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 97F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(725, 441);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // textBox3
            // 
            this.textBox3.Dock = System.Windows.Forms.DockStyle.Right;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.textBox3.Location = new System.Drawing.Point(98, 3);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(261, 30);
            this.textBox3.TabIndex = 5;
            this.textBox3.Text = "Please select your timebase";
            // 
            // timebase_next
            // 
            this.timebase_next.Dock = System.Windows.Forms.DockStyle.Right;
            this.timebase_next.Location = new System.Drawing.Point(587, 346);
            this.timebase_next.Name = "timebase_next";
            this.timebase_next.Size = new System.Drawing.Size(135, 92);
            this.timebase_next.TabIndex = 7;
            this.timebase_next.Text = "Next";
            this.timebase_next.UseVisualStyleBackColor = true;
            this.timebase_next.Click += new System.EventHandler(this.timebase_next_Click);
            // 
            // timebasevar
            // 
            this.timebasevar.Dock = System.Windows.Forms.DockStyle.Right;
            this.timebasevar.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.timebasevar.Location = new System.Drawing.Point(273, 64);
            this.timebasevar.Name = "timebasevar";
            this.timebasevar.Size = new System.Drawing.Size(86, 30);
            this.timebasevar.TabIndex = 4;
            this.timebasevar.Text = "0";
            // 
            // textBox4
            // 
            this.textBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.textBox4.Location = new System.Drawing.Point(365, 64);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(357, 120);
            this.textBox4.TabIndex = 6;
            this.textBox4.Text = resources.GetString("textBox4.Text");
            // 
            // timebase_previous
            // 
            this.timebase_previous.Dock = System.Windows.Forms.DockStyle.Left;
            this.timebase_previous.Location = new System.Drawing.Point(3, 346);
            this.timebase_previous.Name = "timebase_previous";
            this.timebase_previous.Size = new System.Drawing.Size(115, 92);
            this.timebase_previous.TabIndex = 8;
            this.timebase_previous.Text = "Previous";
            this.timebase_previous.UseVisualStyleBackColor = true;
            this.timebase_previous.Click += new System.EventHandler(this.timebase_previous_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.volt_next, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.ChannelH_volt, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ChannelG_volt, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.ChannelF_volt, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label7, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.ChannelE_volt, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ChannelD_volt, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.ChannelA_volt, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ChannelC_volt, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.ChannelB_volt, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 118);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(725, 425);
            this.tableLayoutPanel1.TabIndex = 20;
            // 
            // volt_next
            // 
            this.volt_next.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.volt_next.Location = new System.Drawing.Point(546, 387);
            this.volt_next.Name = "volt_next";
            this.volt_next.Size = new System.Drawing.Size(176, 35);
            this.volt_next.TabIndex = 22;
            this.volt_next.Text = "Next";
            this.volt_next.UseVisualStyleBackColor = true;
            this.volt_next.Click += new System.EventHandler(this.volt_next_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label4.Location = new System.Drawing.Point(546, 192);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(176, 40);
            this.label4.TabIndex = 12;
            this.label4.Text = "Channel H";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label3.Location = new System.Drawing.Point(365, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 40);
            this.label3.TabIndex = 11;
            this.label3.Text = "Channel G";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label5.Location = new System.Drawing.Point(546, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(176, 40);
            this.label5.TabIndex = 13;
            this.label5.Text = "Channel D";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label2.Location = new System.Drawing.Point(184, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 40);
            this.label2.TabIndex = 10;
            this.label2.Text = "Channel F";
            // 
            // ChannelH_volt
            // 
            this.ChannelH_volt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelH_volt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChannelH_volt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.ChannelH_volt.FormattingEnabled = true;
            this.ChannelH_volt.Items.AddRange(new object[] {
            "OFF",
            "10mV",
            "20mV",
            "50mV",
            "100mV",
            "200mV",
            "500mV",
            "1V",
            "2V",
            "5V",
            "10V",
            "20V",
            "50V"});
            this.ChannelH_volt.Location = new System.Drawing.Point(546, 235);
            this.ChannelH_volt.Name = "ChannelH_volt";
            this.ChannelH_volt.Size = new System.Drawing.Size(176, 33);
            this.ChannelH_volt.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label8.Location = new System.Drawing.Point(3, 192);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(175, 40);
            this.label8.TabIndex = 16;
            this.label8.Text = "Channel E";
            // 
            // ChannelG_volt
            // 
            this.ChannelG_volt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelG_volt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChannelG_volt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.ChannelG_volt.FormattingEnabled = true;
            this.ChannelG_volt.Items.AddRange(new object[] {
            "OFF",
            "10mV",
            "20mV",
            "50mV",
            "100mV",
            "200mV",
            "500mV",
            "1V",
            "2V",
            "5V",
            "10V",
            "20V",
            "50V"});
            this.ChannelG_volt.Location = new System.Drawing.Point(365, 235);
            this.ChannelG_volt.Name = "ChannelG_volt";
            this.ChannelG_volt.Size = new System.Drawing.Size(175, 33);
            this.ChannelG_volt.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label6.Location = new System.Drawing.Point(365, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(175, 40);
            this.label6.TabIndex = 14;
            this.label6.Text = "Channel C";
            // 
            // ChannelF_volt
            // 
            this.ChannelF_volt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelF_volt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChannelF_volt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.ChannelF_volt.FormattingEnabled = true;
            this.ChannelF_volt.Items.AddRange(new object[] {
            "OFF",
            "10mV",
            "20mV",
            "50mV",
            "100mV",
            "200mV",
            "500mV",
            "1V",
            "2V",
            "5V",
            "10V",
            "20V",
            "50V"});
            this.ChannelF_volt.Location = new System.Drawing.Point(184, 235);
            this.ChannelF_volt.Name = "ChannelF_volt";
            this.ChannelF_volt.Size = new System.Drawing.Size(175, 33);
            this.ChannelF_volt.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label7.Location = new System.Drawing.Point(184, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(175, 40);
            this.label7.TabIndex = 15;
            this.label7.Text = "Channel B";
            // 
            // ChannelE_volt
            // 
            this.ChannelE_volt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelE_volt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChannelE_volt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.ChannelE_volt.FormattingEnabled = true;
            this.ChannelE_volt.Items.AddRange(new object[] {
            "OFF",
            "10mV",
            "20mV",
            "50mV",
            "100mV",
            "200mV",
            "500mV",
            "1V",
            "2V",
            "5V",
            "10V",
            "20V",
            "50V"});
            this.ChannelE_volt.Location = new System.Drawing.Point(3, 235);
            this.ChannelE_volt.Name = "ChannelE_volt";
            this.ChannelE_volt.Size = new System.Drawing.Size(175, 33);
            this.ChannelE_volt.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 40);
            this.label1.TabIndex = 9;
            this.label1.Text = "Channel A";
            // 
            // ChannelD_volt
            // 
            this.ChannelD_volt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelD_volt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChannelD_volt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.ChannelD_volt.FormattingEnabled = true;
            this.ChannelD_volt.Items.AddRange(new object[] {
            "OFF",
            "10mV",
            "20mV",
            "50mV",
            "100mV",
            "200mV",
            "500mV",
            "1V",
            "2V",
            "5V",
            "10V",
            "20V",
            "50V"});
            this.ChannelD_volt.Location = new System.Drawing.Point(546, 43);
            this.ChannelD_volt.Name = "ChannelD_volt";
            this.ChannelD_volt.Size = new System.Drawing.Size(176, 33);
            this.ChannelD_volt.TabIndex = 3;
            // 
            // ChannelA_volt
            // 
            this.ChannelA_volt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelA_volt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChannelA_volt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.ChannelA_volt.FormattingEnabled = true;
            this.ChannelA_volt.Items.AddRange(new object[] {
            "OFF",
            "10mV",
            "20mV",
            "50mV",
            "100mV",
            "200mV",
            "500mV",
            "1V",
            "2V",
            "5V",
            "10V",
            "20V",
            "50V"});
            this.ChannelA_volt.Location = new System.Drawing.Point(3, 43);
            this.ChannelA_volt.Name = "ChannelA_volt";
            this.ChannelA_volt.Size = new System.Drawing.Size(175, 33);
            this.ChannelA_volt.TabIndex = 0;
            // 
            // ChannelC_volt
            // 
            this.ChannelC_volt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelC_volt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChannelC_volt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.ChannelC_volt.FormattingEnabled = true;
            this.ChannelC_volt.Items.AddRange(new object[] {
            "OFF",
            "10mV",
            "20mV",
            "50mV",
            "100mV",
            "200mV",
            "500mV",
            "1V",
            "2V",
            "5V",
            "10V",
            "20V",
            "50V"});
            this.ChannelC_volt.Location = new System.Drawing.Point(365, 43);
            this.ChannelC_volt.Name = "ChannelC_volt";
            this.ChannelC_volt.Size = new System.Drawing.Size(175, 33);
            this.ChannelC_volt.TabIndex = 2;
            // 
            // ChannelB_volt
            // 
            this.ChannelB_volt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChannelB_volt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChannelB_volt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.ChannelB_volt.FormattingEnabled = true;
            this.ChannelB_volt.Items.AddRange(new object[] {
            "OFF",
            "10mV",
            "20mV",
            "50mV",
            "100mV",
            "200mV",
            "500mV",
            "1V",
            "2V",
            "5V",
            "10V",
            "20V",
            "50V"});
            this.ChannelB_volt.Location = new System.Drawing.Point(184, 43);
            this.ChannelB_volt.Name = "ChannelB_volt";
            this.ChannelB_volt.Size = new System.Drawing.Size(175, 33);
            this.ChannelB_volt.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.textBox1.Location = new System.Drawing.Point(189, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(360, 23);
            this.textBox1.TabIndex = 24;
            this.textBox1.Text = "Please Select Your Voltage Ranges";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BlockExample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 543);
            this.Controls.Add(this.Voltset);
            this.Controls.Add(this.start);
            this.Name = "BlockExample";
            this.Text = "PicoScope 4000 Series (A API) Block Data Capture Example";
            this.Voltset.ResumeLayout(false);
            this.Voltset.PerformLayout();
            this.Timebase.ResumeLayout(false);
            this.TriggerSettings.ResumeLayout(false);
            this.start_capture.ResumeLayout(false);
            this.results.ResumeLayout(false);
            this.chart.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.results_chart)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Panel Voltset;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ChannelH_volt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox ChannelG_volt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox ChannelF_volt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ChannelE_volt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ChannelD_volt;
        private System.Windows.Forms.ComboBox ChannelA_volt;
        private System.Windows.Forms.ComboBox ChannelC_volt;
        private System.Windows.Forms.ComboBox ChannelB_volt;
        private System.Windows.Forms.Button volt_next;
        private System.Windows.Forms.Panel Timebase;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button timebase_next;
        private System.Windows.Forms.TextBox timebasevar;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button timebase_previous;
        private System.Windows.Forms.Panel TriggerSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox direction_text;
        private System.Windows.Forms.TextBox Channel_text;
        private System.Windows.Forms.ComboBox Channel;
        private System.Windows.Forms.TextBox threshold_text;
        private System.Windows.Forms.TextBox threshold;
        private System.Windows.Forms.ComboBox direction;
        private System.Windows.Forms.CheckBox Trigger_enable;
        private System.Windows.Forms.Button trig_previous;
        private System.Windows.Forms.Button trig_next;
        private System.Windows.Forms.Panel start_capture;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TextBox DeviceInfo;
        private System.Windows.Forms.Button capture_previous;
        private System.Windows.Forms.Button start_cap;
        private System.Windows.Forms.Panel results;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button Restart;
        private System.Windows.Forms.TextBox ChannelA_t;
        private System.Windows.Forms.TextBox ChannelB_t;
        private System.Windows.Forms.TextBox ChannelC_t;
        private System.Windows.Forms.TextBox ChannelD_t;
        private System.Windows.Forms.TextBox ChannelE_t;
        private System.Windows.Forms.TextBox ChannelF_t;
        private System.Windows.Forms.TextBox ChannelG_t;
        private System.Windows.Forms.TextBox ChannelH_t;
        private System.Windows.Forms.TextBox ChannelA;
        private System.Windows.Forms.TextBox ChannelB;
        private System.Windows.Forms.TextBox ChannelC;
        private System.Windows.Forms.TextBox ChannelD;
        private System.Windows.Forms.TextBox ChannelE;
        private System.Windows.Forms.TextBox ChannelF;
        private System.Windows.Forms.TextBox ChannelH;
        private System.Windows.Forms.TextBox ChannelG;
        private System.Windows.Forms.Button display_change;
        private System.Windows.Forms.Panel chart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.DataVisualization.Charting.Chart results_chart;
        private System.Windows.Forms.Button tables;
    }
}

