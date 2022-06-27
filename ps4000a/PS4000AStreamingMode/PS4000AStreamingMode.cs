/******************************************************************************
 *
 *  Filename: PS4000AStreamingMode.cs
 *  
 *  Description:
 *    This is a console-mode program that demonstrates how to use the
 *    ps4000a driver API functions using .NET
 *		
 *  Examples:
 *     Collect a stream of data immediately
 *    
 *  Copyright © 2022 Pico Technology Ltd. See LICENSE file for terms.
 *
 ******************************************************************************/

using System;
using System.IO;
using System.Threading;
using System.Text;

using PS4000AImports;
using PicoPinnedArray;
using PicoStatus;

namespace PS4000AStreamingMode
{
    struct ChannelSettings
    {
        public Imports.Coupling DCcoupled;
        public Imports.Range range;
        public bool enabled;
    }
    internal class StreamingMode

    {
        public const int MAX_CHANNELS = 4;
        bool _scaleVoltages = true;
        ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };
        private ChannelSettings[] _channelSettings;
        private int channelCount = 4;
        short _handle;

        // buffers used for streaming data collection 

        short[][] buffers;

        bool _autoStop;
        bool _ready = false;
        short _trig = 0;
        uint _trigAt = 0;
        int _sampleCount;
        uint _startIndex = 0;

        /****************************************************************************
		 * StreamingCallback
		 * Used by ps4000a data streaming collection calls, on receipt of data.
		 * Used to set global flags etc checked by user routines
		 ****************************************************************************/
        void StreamingCallback(short handle,
                                int noOfSamples,
                                uint startIndex,
                                short ov,
                                uint triggerAt,
                                short triggered,
                                short autoStop,
                                IntPtr pVoid)
        {
            // used for streaming
            _sampleCount = noOfSamples;
            _startIndex = startIndex;
            _autoStop = autoStop != 0;


            // flag to say done reading data
            _ready = true;

            // flags to show if & where a trigger has occurred
            _trig = triggered;
            _trigAt = triggerAt;


        }

        /****************************************************************************
        * adc_to_mv
        *
        * If the user selects scaling to millivolts,
        * Convert an 16-bit ADC count into millivolts
        ****************************************************************************/
        int adc_to_mv(int raw, int ch)
        {
            return (_scaleVoltages) ? (raw * inputRanges[ch]) / Imports.MaxValue : raw;
        }


        static void Main(string[] args)
        {
            Console.WriteLine("PicoScope 4000 Series (ps4000a) Driver C# streaming mode example");
            Console.WriteLine("\nOpening the device...");

            short handle;


            //Open unit 
            uint status = Imports.OpenUnit(out handle, null);
            if (status == StatusCodes.PICO_POWER_SUPPLY_NOT_CONNECTED || status == StatusCodes.PICO_USB3_0_DEVICE_NON_USB3_0_PORT)
            {
                status = Imports.ps4000aChangePowerSource(handle, status);
            }
            else if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("An error has been encountered : {0}", status);
            }
            else
            {
                // Do nothing
            }
            Console.WriteLine("\nDevice successfully opened!..");

            StreamingMode streamingcapture = new StreamingMode(handle);
            streamingcapture.getdeviceinfo();
        }
        public StreamingMode(short handle)
        {
            _handle = handle;
        }
        private void getdeviceinfo()
        {
            string[] description = {
                           "Driver Version    ",
                           "USB Version       ",
                           "Hardware Version  ",
                           "Variant Info      ",
                           "Serial            ",
                           "Cal Date          ",
                           "Kernel Ver.       ",
                           "Digital Hardware  ",
                           "Analogue Hardware ",
                           "Firmware 1        ",
                           "Firmware 2        "
                         };

            StringBuilder line = new StringBuilder(80);


            for (uint i = 0; i < description.Length; i++)
            {
                short requiredSize;
                Imports.GetUnitInfo(_handle, line, 80, out requiredSize, i);



                Console.WriteLine("{0}: {1}", description[i], line);


            }
            setchannel();
        }

        private void setchannel()
        {
            {
                _channelSettings = new ChannelSettings[MAX_CHANNELS];

                for (int i = 0; i < MAX_CHANNELS; i++)
                {
                    _channelSettings[i].enabled = true;
                    _channelSettings[i].DCcoupled = Imports.Coupling.DC;
                    _channelSettings[i].range = Imports.Range.Range_5V;
                }

                for (int i = 0; i < channelCount; i++) // reset channels to most recent settings
                {
                    Imports.SetChannel(_handle, Imports.Channel.CHANNEL_A + i,
                                       (short)(_channelSettings[(int)(Imports.Channel.CHANNEL_A + i)].enabled ? 1 : 0),
                                       _channelSettings[(int)(Imports.Channel.CHANNEL_A + i)].DCcoupled,
                                       _channelSettings[(int)(Imports.Channel.CHANNEL_A + i)].range,
                                       (float)0.0);
                }
                streamingdatahandler(0);

            }

        }
        /****************************************************************************
		 * StreamingDataHandler
		 * - acquires data 
		 * * Input :
		 * - unit : the unit to use.
		 * - text : the text to display before the display of data slice
		 * - offset : the offset into the data buffer to start the display's slice.
		 ****************************************************************************/
        private void streamingdatahandler(uint preTrigger)
        {


            Console.WriteLine("Press a key to start");
            while (!Console.KeyAvailable) Thread.Sleep(100);

            if (Console.KeyAvailable)
            {
                Console.ReadKey(true); // clear the key
            }
            //set trigger

            Imports.SetSimpleTrigger(_handle, 0, Imports.Channel.CHANNEL_A, 0, Imports.ThresholdDirection.None, 0, 0);/* Trigger disabled	*/


            uint tempBufferSize = 50000; /*  Ensure buffer is large enough */

            uint totalSamples = 0;
            uint triggeredAt = 0;
            uint status;
            short setAutoStop = 1;
            uint downsampleRatio;
            Imports.ReportedTimeUnits timeUnits;
            uint sampleInterval;
            Imports.DownSamplingMode ratioMode;
            uint postTrigger;
            bool autoStop;

            downsampleRatio = 1;
            timeUnits = Imports.ReportedTimeUnits.MilliSeconds;
            sampleInterval = 10;
            ratioMode = Imports.DownSamplingMode.None;
            postTrigger = 10;
            autoStop = false;


            // pinned buffer creation
            PinnedArray<short>[] appBuffersPinned = new PinnedArray<short>[channelCount];



            buffers = new short[channelCount][];

            for (int channel = 0; channel < channelCount; channel++) // create data buffers
            {

                buffers[channel] = new short[tempBufferSize];
                appBuffersPinned[channel] = new PinnedArray<short>(buffers[channel]);

                status = Imports.SetDataBuffers(_handle, (Imports.Channel)(channel), buffers[channel], null, (int)tempBufferSize, 0, ratioMode);

                if (status != StatusCodes.PICO_OK)
                {
                    Console.WriteLine("StreamDataHandler:Imports.SetDataBuffers Channel {0} Status = 0x{1:X6}\n", (char)('A' + channel), status);
                }
            }
            Console.WriteLine("Waiting for trigger...Press a key to abort");

            _autoStop = false;
            // Start the device collecting data
            status = Imports.RunStreaming(_handle, ref sampleInterval, timeUnits, preTrigger, postTrigger, setAutoStop, downsampleRatio, ratioMode, tempBufferSize);

            if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("StreamDataHandler:ps2000aRunStreaming Status = 0x{0:X6}", status);
                _autoStop = true;           // if there's a problem, set _autoStop = true to drop out, clean up memory, and close the text writer.
            }
            Console.WriteLine("Run Streaming : {0} ", status);

            Console.WriteLine("Streaming data...Press a key to abort");


            while (!_autoStop && !Console.KeyAvailable)
            {

                Thread.Sleep(10);
                _ready = false;

                status = Imports.GetStreamingLatestValues(_handle, StreamingCallback, IntPtr.Zero);
                if (status != StatusCodes.PICO_OK)
                {
                    Console.WriteLine("An error has been encountered : {0}", status);
                }
                else
                {
                    // Do nothing

                }

                if (_ready && _sampleCount > 0) /* can be ready and have no data, if autoStop has fired */
                {
                    if (_trig > 0)
                    {
                        triggeredAt = totalSamples + _trigAt;
                    }

                    totalSamples += (uint)_sampleCount;

                    Console.Write("\nCollected {0} samples, index = {1}, Total = {2}", _sampleCount, _startIndex, totalSamples);

                    if (_trig > 0)
                    {
                        Console.Write("\tTrig at Index {0}", triggeredAt);
                    }

                    for (uint i = _startIndex; i < (_startIndex + _sampleCount); i++)
                    {
                        // Build File Body
                        for (int ch = 0; ch < channelCount; ch++)
                        {
                            if (_channelSettings[ch].enabled)
                            {
                                Console.Write("\n{0} {1} {2} {3} {4},",
                                                (char)('A' + ch),
                                                appBuffersPinned[ch].Target[i],
                                                adc_to_mv(appBuffersPinned[ch].Target[i], (int)_channelSettings[(int)(Imports.Channel.CHANNEL_A + ch)].range),
                                                appBuffersPinned[ch].Target[i],
                                                adc_to_mv(appBuffersPinned[ch].Target[i], (int)_channelSettings[(int)(Imports.Channel.CHANNEL_A + ch)].range));

                            }
                        }

                        Console.WriteLine();
                    }

                }


            }
            if (Console.KeyAvailable)
            {
                Console.ReadKey(true); // clear the key
            }

            Imports.Stop(_handle);

            if (!_autoStop)
            {
                Console.WriteLine("\nData collection aborted - press any key to continue.");
                while (!Console.KeyAvailable) Thread.Sleep(100);

                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true); // clear the key
                }
            }
            Console.WriteLine();
        }
    }
}
