/******************************************************************************
 *
 * Filename: PicoHRDLConsole.cs
 *
 * Description:
 *     This is a Windows Forms application that demonstrates how to use the
 *     PicoLog High Resolution Data Logger (picohrdl) driver functions 
 *     using .NET to collect data on Channel 1 of an ADC-20/24 device.
 *      
 * Supported PicoLog models:
 *  
 *     ADC-20
 *     ADC-24
 *      
 * Copyright © 2015-2017 Pico Technology Ltd. See LICENSE file for terms.
 *      
 ******************************************************************************/

using System;
using System.Windows.Forms;
using System.Threading;

using PicoHRDLImports;

namespace PicoHRDLGui
{
    public partial class PicoHRDLCSGuiForm : Form
    {
        short handle;

        public PicoHRDLCSGuiForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            channel1DataTextBox.Clear();
            numvaluesCollectedTextBox.Clear();

            Thread.Sleep(1000);

            if (handle > 0)
            {
                // Set Input channel

                short analogChannelStatus = Imports.SetAnalogInChannel(handle, (short)Imports.HRDLInputs.HRDL_ANALOG_IN_CHANNEL_1, 1,
                                                (short)Imports.HRDLRange.HRDL_2500_MV, (short)1); //set channel 1, enabled, range = 2500mV, single ended

                // Set Interval
                short returnIntervalStatus = Imports.SetInterval(handle, 80, (short)Imports.HRDLConversionTime.HRDL_60MS); //sample interval time= 80ms, conversion time = 60ms

                // Specify number of values to collect and capture block of data

                int values = 0;
            
                Int32.TryParse(numValuesToCollectedTextBox.Text, out values);

                short status = Imports.HRDLRun(handle, values, (short)Imports.BlockMethod.HRDL_BM_BLOCK);

                short ready = Imports.HRDLReady(handle);

                while (ready != 1)
                {
                    ready = Imports.HRDLReady(handle);
                    System.Threading.Thread.Sleep(100);
                }

                short stopStatus = Imports.HRDLStop(handle);

                // Set up data array to retrieve values
                int[] data = new int[100];
                short overflow = 0;

                int numValues = Imports.GetValues(handle, data, out overflow, values);

                int minAdc = 0;
                int maxAdc = 0;

                // Obtain Max Min ADC Count values for Channel 1
                short returnAdcMaxMin = Imports.GetMinMaxAdcCounts(handle, out minAdc, out maxAdc, (short)Imports.HRDLInputs.HRDL_ANALOG_IN_CHANNEL_1);

                numvaluesCollectedTextBox.Text += numValues.ToString();

                float[] scaledData = new float[numValues];

                for (int n = 0; n < numValues; n++)
                {
                    scaledData[n] = adcToMv(data[n], (short)Imports.HRDLRange.HRDL_2500_MV, maxAdc);
                    channel1DataTextBox.Text += "Raw: " + data[n] + "\tScaled: " + scaledData[n] + "\r\n";
                }
            }
            else
            {
                MessageBox.Show("No connection to device.");
            }

        }

        /**
         * GetDeviceInfo 
         * 
         * Prints information about the device to the console window.
         * 
         * Inputs:
         *      handle - the handle to the device
         */
        public void GetDeviceInfo(short handle)
        {
            string[] description = {
                           "Driver Version    ",
                           "USB Version       ",
                           "Hardware Version  ",
                           "Variant Info      ",
                           "Serial            ",
                           "Cal Date          ",
                           "Kernel Ver        "
                         };

            System.Text.StringBuilder line = new System.Text.StringBuilder(80);

            if (handle >= 0)
            {
                for (short i = 0; i < 6; i++)
                {

                    Imports.GetUnitInfo(handle, line, 80, i);

                    unitInfoTextBox.Text += description[i] + ": " + line.ToString() + "\r\n";
                }
            }

        }

        /**
         * adcToMv 
         * 
         * 
         */
        public float adcToMv(int value, short range, int maxValue)
        {
            float mvValue = 0.0f;

            float vMax = (float)(Imports.MAX_VOLTAGE_RANGE / Math.Pow(2, range)); // Find voltage scaling factor

            mvValue = ((float)value / maxValue) * vMax;

            return mvValue;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            short closeStatus = Imports.HRDLCloseUnit(handle);

            System.Windows.Forms.Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Clear text boxes
            unitInfoTextBox.Clear();

            handle = Imports.HRDLOpenUnit();

            GetDeviceInfo(handle);

            // Set Mains Rejection

            short setMainsStatus = Imports.SetMains(handle, Imports.HRDLMainsRejection.HRDL_FIFTY_HERTZ);   // Set noise rejection for 50Hz  
        }
        
    }
}
