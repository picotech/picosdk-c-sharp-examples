/**************************************************************************
 *
 * Filename: PS3000ASigGen.cs
 *
 * Description:
 *   This is a program that lets you control the AWG/Signal Generator for 
 *   PicoScope 3000 Series oscilloscope using the ps3000a driver API 
 *   functions.
 *   
 * Supported PicoScope models:
 *
 *		PicoScope 3204A/B/D
 *		PicoScope 3205A/B/D
 *		PicoScope 3206A/B/D
 *		PicoScope 3207A/B
 *		PicoScope 3204 MSO & D MSO
 *		PicoScope 3205 MSO & D MSO
 *		PicoScope 3206 MSO & D MSO
 *		PicoScope 3404A/B/D/D MSO
 *		PicoScope 3405A/B/D/D MSO
 *		PicoScope 3406A/B/D/D MSO
 *
 * Examples:
 *    Outputs signal from signal generator
 *    Loads in file and creates signal using the Arbitrary Waveform Generator
 *    
 * Copyright (C) 2011 - 2017 Pico Technology Ltd. See LICENSE file for terms.
 * 
 **************************************************************************/

using System;
using System.Windows.Forms;

using PS3000AImports;
using PicoStatus;

namespace PS3000ASigGen
{
    public partial class AWG_SIGGEN : Form
    {

        short handle = 0;
        UInt32 status;

        short minArbitraryWaveformValue;
        short maxArbitraryWaveformValue;
        uint minArbitraryWaveformSize;
        uint maxArbitraryWaveformSize;

        // Initialise view 
        public AWG_SIGGEN()
        {
            InitializeComponent();
            fileNameTextBox.Text = "Please select signal type";
            fileNameTextBox.ReadOnly = true;
            sigToAWG.Checked = false;
            sweepCheckBox.Checked = false;
            signalTypeComboBox.SelectedIndex = 0;
            sweepTypeComboBox.SelectedIndex = 0;
        }

        //opens device
        private void Start_button_Click(object sender, EventArgs e)
        {
            //opens device 
            status = Imports.OpenUnit(out handle, null);

            //if handle is zero there is a issue, will also need to change power source if the power supply is not connected (for PicoScope 3400/ 3400D models) or if not using a USB 3.0 port
            if (handle == 0)
            {
                MessageBox.Show("Cannot open device error code: " + status.ToString(), "Error Opening Device", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            else if (status != Status.PICO_OK)
            {
                status = Imports.ChangePowerSource(handle, status);
            }

            // Find max AWG Buffer Size

            status = Imports.SigGenArbitraryMinMaxValues(handle, out minArbitraryWaveformValue, out maxArbitraryWaveformValue, out minArbitraryWaveformSize, out maxArbitraryWaveformSize);

            if (status == Status.PICO_OK)
            {
                if (maxArbitraryWaveformSize > 0)
                {
                    sigToAWG.Enabled = true;
                    fileNameTextBox.Enabled = true;
                }
            }

            controls.Visible = true;
        }

        // When the form is closed, disconnect device
        private void AWG_SIGGEN_Close(object sender, FormClosedEventArgs e)
        {
            Imports.CloseUnit(handle);
        }

        //changes from signal generator to abitary waveform geerator
        private void SIGtoAWG_CheckedChanged(object sender, EventArgs e)
        {
            awgLabel.Visible = sigToAWG.Checked;
            awgFileInfoLabel.Visible = sigToAWG.Checked;
            signalTypeComboBox.Visible = !sigToAWG.Checked;

            if (sigToAWG.Checked)
            {
                fileNameTextBox.Clear();
                fileNameTextBox.ReadOnly = false;
            }
            else
            {
                fileNameTextBox.Text = "Please select signal type";
                fileNameTextBox.ReadOnly = true;
            }
        }

        //enables sweep controls
        private void Sweep_CheckedChanged(object sender, EventArgs e)
        {
            SweepController.Visible = sweepCheckBox.Checked;
        }

        //If dc or white noise sweep is not enable so hides button
        private void signal_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (signalTypeComboBox.SelectedIndex == 8 || signalTypeComboBox.SelectedIndex == 9)
            {
                sweepCheckBox.Checked = false;
                sweepCheckBox.Enabled = false;
            }
            else
            {
                sweepCheckBox.Enabled = true;
            }

        }


        private void Update_button_Click(object sender, EventArgs e)
        {
            Imports.SweepType sweeptype = Imports.SweepType.PS3000A_UP;
            Imports.ExtraOperations operations = Imports.ExtraOperations.PS3000A_ES_OFF;
            uint shots = 0;
            uint sweeps = 0;
            Imports.SigGenTrigType triggertype = Imports.SigGenTrigType.PS3000A_SIGGEN_RISING;
            Imports.SigGenTrigSource triggersource = Imports.SigGenTrigSource.PS3000A_SIGGEN_NONE;
            short extinthreshold = 0;
            double stopFreq;
            double startFreq;
            double increment;
            double dwellTime;
            int offset;
            uint pkToPk;


            try
            {
                startFreq = Convert.ToDouble(startFrequencyTextBox.Text);
                pkToPk = Convert.ToUInt32(peatkToPeakVoltageTextBox.Text) * 1000;
                offset = Convert.ToInt32(offsetVoltageTextBox.Text);

            }
            catch
            {
                MessageBox.Show("Error with start frequency, offset and/or pktopk", "INVALID VALUES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (sweepCheckBox.Checked)
            {
                try
                {
                    stopFreq = Convert.ToDouble(stopFreqTextBox.Text);
                    increment = Convert.ToDouble(frequencyIncrementTextBox.Text);
                    dwellTime = Convert.ToDouble(timeIncrementTextBox.Text);
                    sweeptype = (Imports.SweepType)(sweepTypeComboBox.SelectedIndex);
                }
                catch
                {
                    MessageBox.Show("Sweep values are incorrect", "INCORRECT VALUES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            else
            {
                stopFreq = startFreq;
                increment = 0;
                dwellTime = 0;
                sweeptype = Imports.SweepType.PS3000A_UP;
            }

            if (sigToAWG.Checked)
            {
                Imports.IndexMode indexMode = Imports.IndexMode.PS3000A_SINGLE;
                int waveformsize = 0;
                string line;
                System.IO.StreamReader file;

                short[] waveform = new short[maxArbitraryWaveformSize];

                try
                {
                    file = new System.IO.StreamReader(fileNameTextBox.Text);
                }
                catch
                {
                    MessageBox.Show("Cannot open file", "Error file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                while ((line = file.ReadLine()) != null && waveformsize < maxArbitraryWaveformSize)
                {
                    try
                    {
                        waveform[waveformsize] = Convert.ToInt16(line);
                    }
                    catch
                    {
                        if (Convert.ToInt32(line) > Int16.MaxValue)
                        {
                            waveform[waveformsize] = Int16.MaxValue;
                        }
                        else
                        {
                            waveform[waveformsize] = Int16.MinValue;
                        }

                    }
                    waveformsize++;
                }

                file.Close();


                Array.Resize(ref waveform, waveformsize);

                // As frequency depends on the number or points need to use delta phase.
                // Use the SigGenFrequencyToPhase method to calculate this.

                uint startDeltaPhase;
                uint stopDeltaPhase;
                uint deltaPhaseIncrement;
                uint dwellCount;

                
                status = Imports.SigGenFrequencyToPhase(handle, startFreq, indexMode, (uint) waveformsize, out startDeltaPhase);
                status = Imports.SigGenFrequencyToPhase(handle, stopFreq, indexMode, (uint) waveformsize, out stopDeltaPhase);
                status = Imports.SigGenFrequencyToPhase(handle, increment, indexMode, (uint) waveformsize, out deltaPhaseIncrement);
                status = Imports.SigGenFrequencyToPhase(handle, (double) (1.0 / dwellTime), indexMode, (uint) waveformsize, out dwellCount);
                
                status = Imports.SetSigGenArbitrary(handle, offset, pkToPk, startDeltaPhase, stopDeltaPhase, deltaPhaseIncrement, dwellCount, waveform, waveformsize, sweeptype,
                                                        operations, indexMode, shots, sweeps, triggertype, triggersource, extinthreshold);

                if (status != Status.PICO_OK)
                {
                    MessageBox.Show("Error SetSigGenArbitrary error code :" + status.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                Imports.WaveType wavetype = Imports.WaveType.PS3000A_SINE;

                if (signalTypeComboBox.SelectedIndex < (int) Imports.WaveType.PS3000A_MAX_WAVE_TYPES)
                {
                    if ((wavetype = (Imports.WaveType)(signalTypeComboBox.SelectedIndex)) == Imports.WaveType.PS3000A_DC_VOLTAGE)
                    {
                        pkToPk = 0;
                    }

                }
                else
                {
                    operations = (Imports.ExtraOperations)(signalTypeComboBox.SelectedIndex - 8);
                }


                status = Imports.SetSigGenBuiltInV2(handle, offset, pkToPk, wavetype, startFreq, stopFreq, increment, dwellTime, sweeptype,
                                                        operations, shots, sweeps, triggertype, triggersource, extinthreshold);

                if (status != Status.PICO_OK)
                {
                    MessageBox.Show("Error SetSigGenBuiltInV2 error code :" + status.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

    }
}
