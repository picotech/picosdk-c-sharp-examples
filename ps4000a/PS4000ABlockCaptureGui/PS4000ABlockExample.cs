/******************************************************************************
 *
 *  Filename: PS4000ABlockExamples.cs
 * 
 *  Description:
 *    This is a Block Capture program that shows how to trigger on
 *    one channel and display results for a PicoScope 4000 Series device using
 *    the ps4000a driver API functions.   
 *
 *  Examples:
 *     Collect a block of samples immediately
 *     Collect a block of samples when a trigger event occurs
 *
 *  Copyright (C) 2014 - 2017 Pico Technology Ltd. See LICENSE file for terms.
 * 
 ******************************************************************************/

using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using PS4000AImports;
using PicoStatus;
using PicoPinnedArray;


namespace PS4000ABlockCaptureGui
{
    public partial class BlockExample : Form
    {
        public BlockExample()
        {
            InitializeComponent();
        }

        bool _ready = false;
        public const int BUFFER_SIZE = 1024;

        uint status = 0;
        int timeInterval;
        ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };
        short handle = 0;
        uint _timebase = 8;
        Imports.Range[] combobox_values = new Imports.Range[8];
        private int numberOfChannels = 0;

        void BlockCallback(short handle, short status, IntPtr pVoid)
        {
            // flag to say done reading data
            _ready = true;
        }

        int adc_to_mv(int raw, Imports.Range range)
        {
            return (raw * inputRanges[(int)(range)]) / Imports.MaxValue;
        }

        short mv_to_adc(int mv, Imports.Range range)
        {
            return (short)((mv * Imports.MaxValue) / inputRanges[(int)(range)]);
        }

        private void BlockExample_Close(object sender, FormClosedEventArgs e)
        {
            Imports.CloseUnit(handle);
        }

        // Opens device, brings up the voltage setting panel and populates device information
        private void start_Click(object sender, EventArgs e)
        {
            status = Imports.OpenUnit(out handle, null);

            if (handle == 0)
            {
                MessageBox.Show("The Device code not open\n error code : " + status.ToString(), "Device Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            // If not connected on a USB 3.0 port, the device will need to be powered from USB 2.0 ports
            if (status == StatusCodes.PICO_USB3_0_DEVICE_NON_USB3_0_PORT || status == StatusCodes.PICO_POWER_SUPPLY_NOT_CONNECTED)
            {
                MessageBox.Show("Using USB power", "Under Powered", MessageBoxButtons.OK, MessageBoxIcon.Information);
                status = Imports.ps4000aChangePowerSource(handle, status);
            }

            Voltset.Visible = true;
            ChannelA_volt.SelectedIndex = 0;
            ChannelB_volt.SelectedIndex = 0;
            ChannelC_volt.SelectedIndex = 0;
            ChannelD_volt.SelectedIndex = 0;
            ChannelE_volt.SelectedIndex = 0;
            ChannelF_volt.SelectedIndex = 0;
            ChannelG_volt.SelectedIndex = 0;
            ChannelH_volt.SelectedIndex = 0;

            // Don't need to use channels E to H if the device only has 4 channels
            if(numberOfChannels == 4)
            {
                ChannelE_volt.Enabled = false;
                ChannelF_volt.Enabled = false;
                ChannelG_volt.Enabled = false;
                ChannelH_volt.Enabled = false;
            }

            string[] description = {
                           "Driver Version    ",
                           "USB Version       ",
                           "Hardware Version  ",
                           "Variant Info      ",
                           "Serial            ",
                           "Cal Date          ",
                           "Kernel Ver        ",
                           "Digital Hardware  ",
                           "Analogue Hardware "
                         };

            System.Text.StringBuilder line = new System.Text.StringBuilder(80);
            for (int i = 0; i < 9; i++)
            {
                short requiredSize;
                Imports.GetUnitInfo(handle, line, 80, out requiredSize, (uint) i);

                DeviceInfo.Text += description[i].ToString() + " : " + line.ToString() + Environment.NewLine;

                if (i == PicoInfo.PICO_VARIANT_INFO)
                {
                    try
                    {
                        numberOfChannels = Int32.Parse(line[1].ToString());
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }

        // Sets up the voltage levels for the channels
        private void volt_next_Click(object sender, EventArgs e)
        {
            // range is shifted by -1 bcause of index list number
            combobox_values[0] = (Imports.Range)(ChannelA_volt.SelectedIndex - 1);
            combobox_values[1] = (Imports.Range)(ChannelB_volt.SelectedIndex - 1);
            combobox_values[2] = (Imports.Range)(ChannelC_volt.SelectedIndex - 1);
            combobox_values[3] = (Imports.Range)(ChannelD_volt.SelectedIndex - 1);
            combobox_values[4] = (Imports.Range)(ChannelE_volt.SelectedIndex - 1);
            combobox_values[5] = (Imports.Range)(ChannelF_volt.SelectedIndex - 1);
            combobox_values[6] = (Imports.Range)(ChannelG_volt.SelectedIndex - 1);
            combobox_values[7] = (Imports.Range)(ChannelH_volt.SelectedIndex - 1);

            // Check to make sure that not all channels are off
            if (combobox_values.All(item => item.Equals((Imports.Range)(-1))))
            {
                MessageBox.Show("One channel needs to be enabled", "Error Channel Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int ch = 0; ch < numberOfChannels; ch++)
            {
                 status = Imports.SetChannel(handle,                                                                                // the device handle
                                            Imports.Channel.CHANNEL_A + ch,                                                         // the channel you wish to set
                                            (short)(combobox_values[ch] != (Imports.Range)(-1) ? 1 : 0),                            // is enabled if off is not selected
                                            Imports.Coupling.DC,
                                            combobox_values[ch] != (Imports.Range)(-1) ? combobox_values[ch] : (Imports.Range)(0),  // -1 is not a correct range will cause an error
                                             0); // no analogue offset 

                 if (combobox_values[ch] != (Imports.Range)(-1))
                 {
                     results_chart.Series[((char)(ch + 65)).ToString()].Enabled = true;
                 }
                 else
                 {
                     results_chart.Series[((char)(ch + 65)).ToString()].Enabled = false;
                 }


                if (status != 0)
                {
                    MessageBox.Show("Error setting channel Error Code : " + status.ToString(),
                                    "Error Setting Channel",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
            }

            Timebase.Visible = true;
        }

        // Finds the most suitable time base then saves it as _timebase
        private void timebase_next_Click(object sender, EventArgs e)
        {
            int maxsamples;

            try
            {
                _timebase = uint.Parse(timebasevar.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter numeric value only", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            while (Imports.GetTimebase(handle, _timebase, BUFFER_SIZE, out timeInterval, out maxsamples, 0) != 0)
            {
                _timebase++;
            }

            var result = MessageBox.Show("Timebase index chosen is: " + _timebase.ToString() + " which corresponds to a " + timeInterval.ToString() + " ns sample interval.\nClick OK to continue or Cancel to select another timebase index.",
                                         "Timebase Settings",
                                         MessageBoxButtons.OKCancel,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                TriggerSettings.Visible = true;
                Channel.SelectedIndex = 0;
                direction.SelectedIndex = 0;
            }
        }

        // Enables the trigger control if user wishes to use trigger or not
        private void Trigger_enable_CheckedChanged(object sender, EventArgs e)
        {
            Channel_text.Enabled= Trigger_enable.Checked;
            Channel.Enabled = Trigger_enable.Checked;
            threshold_text.Enabled = Trigger_enable.Checked;
            threshold.Enabled = Trigger_enable.Checked;
            direction_text.Enabled = Trigger_enable.Checked;
            direction.Enabled = Trigger_enable.Checked;
        }

        // Enables or disables the trigger depending on the check box
        private void trig_next_Click(object sender, EventArgs e)
        {
            short _threshold = 0;

            if (!Trigger_enable.Checked)
            {
                if ((status = Imports.SetSimpleTrigger(handle, 0, Imports.Channel.CHANNEL_A, 0, Imports.ThresholdDirection.Rising, 0, 0)) != 0)// disabling trigger
                {
                    MessageBox.Show("Cannot set trigger, Error code :" + status.ToString(),
                                    "Error Setting Trigger",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            else
            {
                if (combobox_values[Channel.SelectedIndex] == (Imports.Range)(-1))
                {
                    MessageBox.Show("Please Select a Channel which is enabled");
                    return;
                }

                try
                {
                    _threshold = Int16.Parse(threshold.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter numeric value only for threshold", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                status = Imports.SetSimpleTrigger(handle,                                                           // handle selects the device
                                            1,                                                                      // if this value is non zero will enable trigger
                                            Imports.Channel.CHANNEL_A + Channel.SelectedIndex,                       // select channel
                                            mv_to_adc(_threshold, combobox_values[Channel.SelectedIndex]),          // function use adc values, so must be converted
                                            Imports.ThresholdDirection.Above + direction.SelectedIndex,             // Direction that trigger will see e.g rising or above
                                            0,                                                                      //  Delay time ins sample period between trigger and first sample being taken
                                            0);                                                                     //  time is ms that system will wait if no trigger occurs, set to zero to wait indefinately
            }
            start_capture.Visible = true;
        }

        // Will start the actual run block function
        private void start_cap_Click(object sender, EventArgs e)
        {
            Imports.ps4000aBlockReady _callbackDelegate;
            uint sampleCount = BUFFER_SIZE;


            PinnedArray<short>[] minPinned = new PinnedArray<short>[Imports.OCTO_SCOPE];
            PinnedArray<short>[] maxPinned = new PinnedArray<short>[Imports.OCTO_SCOPE];
            int timeIndisposed;

            for (int i = 0; i < 8; i++)
            {
                short[] minBuffers = new short[sampleCount];
                short[] maxBuffers = new short[sampleCount];
                minPinned[i] = new PinnedArray<short>(minBuffers);
                maxPinned[i] = new PinnedArray<short>(maxBuffers);
                status = Imports.SetDataBuffers(handle, (Imports.Channel)i, maxBuffers, minBuffers, (int)sampleCount, 0, Imports.DownSamplingMode.None);
            }

            _ready = false;
            _callbackDelegate = BlockCallback;
            status = Imports.RunBlock(handle, 0, (int)sampleCount, _timebase, out timeIndisposed, 0, _callbackDelegate, IntPtr.Zero);

            if (status != 0)
            {
                MessageBox.Show("Error running block :" + status.ToString(),
                                    "Error runblock",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                return;
            }
            results.Visible = true;


            while (!_ready)
            {
                Thread.Sleep(0);
            }

            if (_ready)
            {

                short overflow;
                Imports.GetValues(handle, 0, ref sampleCount, 1, Imports.DownSamplingMode.None, 0, out overflow);

                for (int i = 0; i < sampleCount; i++)
                {
                    for (int ch = 0; ch < numberOfChannels; ch++)
                    {
                        if (combobox_values[ch] != (Imports.Range)(-1))
                        {

                            double time = timeInterval * i;
                            int value = adc_to_mv(maxPinned[ch].Target[i], combobox_values[ch]);
                            results_chart.Series[((char)(ch + 65)).ToString()].Points.AddY(value);
                            results_chart.Series[((char)(ch + 65)).ToString()].Points[i].AxisLabel = time.ToString();

                            switch (ch)
                            {
                                case 0:
                                    ChannelA.Text += (value.ToString() + Environment.NewLine);
                                    break;
                                case 1:
                                    ChannelB.Text += (value.ToString() + Environment.NewLine);
                                    break;
                                case 2:
                                    ChannelC.Text += (value.ToString() + Environment.NewLine);
                                    break;
                                case 3:
                                    ChannelD.Text += (value.ToString() + Environment.NewLine);
                                    break;
                                case 4:
                                    ChannelE.Text += (value.ToString() + Environment.NewLine);
                                    break;
                                case 5:
                                    ChannelF.Text += (value.ToString() + Environment.NewLine);
                                    break;
                                case 6:
                                    ChannelG.Text += (value.ToString() + Environment.NewLine);
                                    break;
                                case 7:
                                    ChannelH.Text += (value.ToString() + Environment.NewLine);
                                    break;
                            }
                        }
                        else
                        {

                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("data collection aborted", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        //returns to the previous pannel
        private void timebase_previous_Click(object sender, EventArgs e)
        {
            Timebase.Visible = true;
        }
        private void trig_previous_Click(object sender, EventArgs e)
        {
            TriggerSettings.Visible = false;
        }
        private void capture_previous_Click(object sender, EventArgs e)
        {
            start_capture.Visible = false;
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            results.Visible = false;
            Timebase.Visible = false;
            start_capture.Visible = false;
            TriggerSettings.Visible = false;

            ChannelA.Text = string.Empty;
            ChannelB.Text = string.Empty;
            ChannelC.Text = string.Empty;
            ChannelD.Text = string.Empty;
            ChannelE.Text = string.Empty;
            ChannelF.Text = string.Empty;
            ChannelG.Text = string.Empty;
            ChannelH.Text = string.Empty;

            foreach (var series in results_chart.Series)
            {
                series.Points.Clear();
            }
        }

        // Changes the view so can see it as a graph or just lists of variables
        private void display_change_Click(object sender, EventArgs e)
        {
                chart.Visible = true;
        }

        private void tables_Click(object sender, EventArgs e)
        {
            chart.Visible = false;
        }
    }
}
