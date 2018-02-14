/**************************************************************************
 *
 * Filename: PS2000CSSigGen.cs
 *
 * Description:
 *   This is a program that lets you control the AWG/Signal Generator for 
 *   PicoScope 2000 Series oscilloscope using the ps2000 driver API 
 *   functions.
 *   
 * Supported PicoScope models:
 *
 *		PicoScope 2203
 *		PicoScope 2204 & 2204A
 *		PicoScope 2205 & 2205A
 *
 * Examples:
 *    Output signal from signal generator
 *    Load in file and create signal using the Arbitrary Waveform Generator
 *    
 * Copyright © 2017 Pico Technology Ltd. See LICENSE file for terms.
 * 
 **************************************************************************/

using System;
using System.Windows.Forms;

using PS2000Imports;

namespace PS2000CSSigGen
{
    public partial class AWG_SIGGEN : Form
    {
        short handle = 0;
        short status = 0;

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

        // Attempt to open device
        private void Start_button_Click(object sender, EventArgs e)
        {
            // Open unit and show splash screen
            MessageBox.Show("Opening the device...", "PicoScope 2000 Series Driver C# Signal Generator Example Program");

            handle = Imports.OpenUnit();

            if (handle > 0)
            {
                // >0 (oscilloscope handle): if the oscilloscope opened. Use this as the
                // handle argument for all subsequent API calls for this oscilloscope.
                MessageBox.Show("Device opened successfully", "Device handle : " + handle.ToString(), MessageBoxButtons.OK);

                sigToAWG.Enabled = true;
                fileNameTextBox.Enabled = true;
                controls.Visible = true;
            }
            else
            {
                if (handle == 0)
                {
                    // 0: if no oscilloscope is found
                    MessageBox.Show("Unable to open device", "Oscilloscope not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (handle == -1)
                {
                    // -1: if the oscilloscope fails to open
                    MessageBox.Show("Unable to open device", "Oscilloscope failed to open", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Unable to open device", "Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Changes from signal generator to abitary waveform generator
        private void SIGtoAWG_CheckedChanged(object sender, EventArgs e)
        {
            awgLabel.Visible = sigToAWG.Checked;
            awgFileInfoLabel.Visible = sigToAWG.Checked;
            signalTypeComboBox.Visible = !sigToAWG.Checked;

            if (sigToAWG.Checked)
            {
                fileNameTextBox.Clear();
                fileNameTextBox.ReadOnly = false;
                SweepController.Visible = false;
            }
            else
            {
                fileNameTextBox.Text = "Please select signal type";
                fileNameTextBox.ReadOnly = true;
            }
        }

        // Enables sweep controls
        private void Sweep_CheckedChanged(object sender, EventArgs e)
        {
            SweepController.Visible = sweepCheckBox.Checked;
        }

        // Disable sweep for DC waveType
        private void signal_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            Imports.WaveType waveTypeSelected = (Imports.WaveType)signalTypeComboBox.SelectedIndex;

            if (waveTypeSelected == Imports.WaveType.DC_VOLTAGE)
            {
                sweepCheckBox.Checked = false;
                sweepCheckBox.Enabled = false;
            }
            else
            {
                sweepCheckBox.Enabled = true;
            }
        }

        /// <summary>
        /// Handle Update button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_button_Click(object sender, EventArgs e)
        {
            int offset = 0;
            uint pkToPk = 0;
            uint sweeps = 0;
            Double stopFreq = 0;
            Double startFreq = 0;
            Double increment = 0;
            Double dwellTime = 0;
            Imports.SweepType sweeptype = Imports.SweepType.UP;

            try
            {
                startFreq = Convert.ToDouble(startFrequencyTextBox.Text);
                pkToPk = Convert.ToUInt32(peakToPeakVoltageTextBox.Text) * 1000;
                offset = Convert.ToInt32(offsetVoltageTextBox.Text) * 1000;
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
                sweeptype = Imports.SweepType.UP;
            }

            if (sigToAWG.Checked)
            {
                uint waveformSize = 0;
                string line;
                System.IO.StreamReader file;

                byte[] waveform = new byte[Imports.PS2000_AWG_MAX_BUFFER_SIZE];

                try
                {
                    file = new System.IO.StreamReader(fileNameTextBox.Text);
                }
                catch
                {
                    MessageBox.Show("Cannot open file", "Error file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                while ((line = file.ReadLine()) != null && waveformSize < Imports.PS2000_AWG_MAX_BUFFER_SIZE)
                {
                    try
                    {
                        waveform[waveformSize] = Convert.ToByte(line);
                    }
                    catch
                    {
                        if (Convert.ToInt32(line) > byte.MaxValue)
                        {
                            waveform[waveformSize] = byte.MaxValue;
                        }
                        else
                        {
                            waveform[waveformSize] = byte.MinValue;
                        }
                    }
                    waveformSize++;
                }

                file.Close();

                Array.Resize(ref waveform, (int)waveformSize);

                // As frequency depends on the number or points need to use delta phase                             
                double startDeltaPhase = GetDeltaPhase(startFreq, waveformSize);
                double stopDeltaPhase = GetDeltaPhase(stopFreq, waveformSize);
                double deltaPhaseIncrement = GetDeltaPhase(increment, waveformSize);
                double dwellCount = dwellTime / (1 / Imports.PS2000_AWG_DDS_FREQUENCY);

                status = Imports.SetSigGenArbitrary(handle, 
                                                    offset, 
                                                    pkToPk,
                                                    (uint)startDeltaPhase,
                                                    (uint)stopDeltaPhase,
                                                    (uint)deltaPhaseIncrement,
                                                    (uint)dwellCount, 
                                                    waveform, 
                                                    waveformSize, 
                                                    sweeptype,
                                                    sweeps);

                if (status == 0)
                {
                    MessageBox.Show("Parameter is out of range", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Imports.WaveType wavetype = Imports.WaveType.SINE;

                if (signalTypeComboBox.SelectedIndex < (int)Imports.WaveType.MAX_WAVE_TYPES)
                {
                    wavetype = (Imports.WaveType)(signalTypeComboBox.SelectedIndex);

                    if (wavetype == Imports.WaveType.DC_VOLTAGE)
                    {
                        pkToPk = 0;
                    }
                }

                status = Imports.SetSigGenBuiltIn(handle,
                                                  offset,
                                                  pkToPk,
                                                  wavetype,
                                                  (float)startFreq,
                                                  (float)stopFreq,
                                                  (float)increment,
                                                  (float)dwellTime,
                                                  sweeptype,
                                                  sweeps);

                if (status == 0)
                {
                    MessageBox.Show("Error SetSigGen parameters is out of range", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Calc and return delta phase
        /// </summary>
        /// <param name="startFreq"></param>
        /// <param name="waveformSize"></param>
        /// <returns></returns>
        private double GetDeltaPhase(double Freq, uint waveformSize)
        {
            double deltaPhase = 0;

            deltaPhase = ((Freq * waveformSize) / 
                         Imports.PS2000_AWG_MAX_BUFFER_SIZE) *
                         Imports.PS2000_AWG_PHASE_ACCUMULATOR * 
                         (1 / Imports.PS2000_AWG_DDS_FREQUENCY);

            return deltaPhase;
        }

        /// <summary>
        /// When the form is closed, disconnect device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AWG_SIGGEN_FormClosing(object sender, FormClosingEventArgs e)
        {
            Imports.CloseUnit(handle);
        }
    }
}
