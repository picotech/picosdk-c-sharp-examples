/******************************************************************************
 *
 * Filename: PS4000ASigGen.cs
 *
 * Description:
 *   This is a program that lets you control the AWG/Signal Generator for 
 *   PicoScope 4000 Series oscilloscope using the ps4000a driver API 
 *   functions.
 *   
 * Supported PicoScope models:
 *
 *		PicoScope 4824
 *
 * Examples:
 *    Outputs signal from signal generator
 *    Loads in file and creates signal using the Arbitrary Waveform Generator
 *    
 * Copyright (C) 2014 - 2017 Pico Technology Ltd. See LICENSE file for terms.
 * 
 ******************************************************************************/

using System;
using System.Windows.Forms;

using PS4000AImports;
using PicoStatus;

namespace PS4000ASigGen
{
    public partial class PS4000ASigGen : Form
    {

        short handle = 0;
        UInt32 status;

        // Intialise view 
        public PS4000ASigGen()
        {
            InitializeComponent();
            file_name.Text = "Please select signal type";
            file_name.ReadOnly = true;
            SIGtoAWG.Checked = false;
            Sweep.Checked = false;
            signal_type.SelectedIndex = 0;
            sweep_type.SelectedIndex = 0;
        }

        // Opens device
        private void Start_button_Click(object sender, EventArgs e)
        {
            // Opens device 
            status = Imports.OpenUnit(out handle, null);

            // If handle is zero there is an issue, will also need to change power source if not using a USB 3.0 port
            if (status != StatusCodes.PICO_OK)
            {
                if (handle > 0 && status == StatusCodes.PICO_USB3_0_DEVICE_NON_USB3_0_PORT)
                {
                    status = Imports.ps4000aChangePowerSource(handle, status);
                }
                else
                {
                    MessageBox.Show("Cannot open device error code: " + status.ToString(), "Error Opening Device", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }

            controls.Visible = true;
        }

        // When the form is closed, disconnect device
        private void AWG_SIGGEN_Close(object sender, FormClosedEventArgs e)
        {
            Imports.CloseUnit(handle);
        }

        // Changes from signal generator to arbitary waveform generator
        private void SIGtoAWG_CheckedChanged(object sender, EventArgs e)
        {
            awg_label.Visible = SIGtoAWG.Checked;
            awg_label2.Visible = SIGtoAWG.Checked;
            signal_type.Visible = !SIGtoAWG.Checked;

            if (SIGtoAWG.Checked)
            {
                file_name.Clear();
                file_name.ReadOnly = false;
            }
            else
            {
                file_name.Text = "Please select signal type";
                file_name.ReadOnly = true;
            }
        }

        // Enables sweep controls
        private void Sweep_CheckedChanged(object sender, EventArgs e)
        {
            SweepController.Visible = Sweep.Checked;
        }

        // If DC or white noise sweep is not enabled so hides button
        private void signal_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (signal_type.SelectedIndex == 8 || signal_type.SelectedIndex == 9)
            {
                Sweep.Checked = false;
                Sweep.Enabled = false;
            }
            else
            {
                Sweep.Enabled = true;
            }

        }


        private void Update_button_Click(object sender, EventArgs e)
        {
            Imports.SweepType sweeptype = Imports.SweepType.PS4000A_UP;
            Imports.ExtraOperations operations = Imports.ExtraOperations.PS4000A_ES_OFF;
            uint shots = 0;
            uint sweeps = 0;
            Imports.SigGenTrigType triggertype = Imports.SigGenTrigType.PS4000A_SIGGEN_RISING;
            Imports.SigGenTrigSource triggersource = Imports.SigGenTrigSource.PS4000A_SIGGEN_NONE;
            short extinthreshold = 0;
            double stopfreq;
            double startfreq;
            double increment;
            double dwelltime;
            int offset;
            uint pktopk;


            try
            {
                startfreq = Convert.ToDouble(start_freq.Text);
                pktopk = Convert.ToUInt32(pk_pk.Text) * 1000;
                offset = Convert.ToInt32(offsetvoltage.Text) * 1000;
            }
            catch
            {
                MessageBox.Show("Error with start frequency, offset and/or pktopk", "INVALID VALUES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Sweep.Checked)
            {
                try
                {
                    stopfreq = Convert.ToDouble(stop_freq.Text);
                    increment = Convert.ToDouble(freq_incre.Text);
                    dwelltime = Convert.ToDouble(time_incre.Text);
                    sweeptype = (Imports.SweepType)(sweep_type.SelectedIndex);
                }
                catch
                {
                    MessageBox.Show("Sweep values are incorrect", "INCORRECT VALUES", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            else
            {
                stopfreq = startfreq;
                increment = 0;
                dwelltime = 0;
                sweeptype = Imports.SweepType.PS4000A_UP;
            }

            if (SIGtoAWG.Checked)
            {
                Imports.IndexMode index = Imports.IndexMode.PS4000A_SINGLE;
                int waveformsize = 0;
                string line;
                System.IO.StreamReader file;
                short[] waveform = new short[Imports.SIG_GEN_BUFFER_SIZE];

                try
                {
                    file = new System.IO.StreamReader(file_name.Text);
                }
                catch
                {
                    MessageBox.Show("Cannot open file", "Error file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                while ((line = file.ReadLine()) != null && waveformsize < (Imports.SIG_GEN_BUFFER_SIZE))
                {
                    waveform[waveformsize] = Convert.ToInt16(line);
                    waveformsize++;
                }

                file.Close();


                Array.Resize(ref waveform, waveformsize);

                // As frequency depends on the number or points need to use delta phase 
                uint startdeltaphase = (uint)(((1.0 * startfreq * waveformsize) / Imports.SIG_GEN_BUFFER_SIZE) * (1.0 * Imports.AWG_PHASE_ACCUMULATOR / Imports.AWG_DAC_FREQUENCY));
                uint stopdeltaphase = (uint)(((1.0 * stopfreq * waveformsize) / Imports.SIG_GEN_BUFFER_SIZE) * (1.0 * Imports.AWG_PHASE_ACCUMULATOR / Imports.AWG_DAC_FREQUENCY));
                uint _increment = (uint)(((1.0 * increment * waveformsize) / Imports.SIG_GEN_BUFFER_SIZE) * (1.0 * Imports.AWG_PHASE_ACCUMULATOR / Imports.AWG_DAC_FREQUENCY));
                uint dwell = (uint)(dwelltime);



                if ((status = Imports.SetSigGenArbitrary(handle,
                    offset,
                   pktopk,
                  startdeltaphase,
                  stopdeltaphase,
                  _increment,
                  dwell,
                  ref waveform[0],
                  waveformsize,
                  sweeptype,
                  operations,
                  index,
                  shots,
                  sweeps,
                  triggertype,
                  triggersource,
                  extinthreshold)) != 0)
                {
                    MessageBox.Show("Error SetSigGenArbitray error code :" + status.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                Imports.WaveType wavetype = Imports.WaveType.PS4000A_SINE;

                if (signal_type.SelectedIndex < 9)
                {
                    if ((wavetype = (Imports.WaveType)(signal_type.SelectedIndex)) == Imports.WaveType.PS4000A_DC_VOLTAGE)
                    {
                        pktopk = 0;
                    }

                }
                else
                {
                    operations = (Imports.ExtraOperations)(signal_type.SelectedIndex - 8);
                }


                if ((status = Imports.SetSigGenBuiltIn(handle,
                    offset,
                    pktopk,
                    wavetype,
                    startfreq,
                    stopfreq,
                    increment,
                    dwelltime,
                    sweeptype,
                    operations,
                    shots,
                    sweeps,
                    triggertype,
                    triggersource,
                    extinthreshold
                    )) != 0)
                {
                    MessageBox.Show("Error SetSigGenBuiltIn error code :" + status.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

    }
}
