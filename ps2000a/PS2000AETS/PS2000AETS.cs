using System;
using System.Collections.Generic;

using System.IO;
using System.Threading;
using System.Text;

using PS2000AImports;
using PicoPinnedArray;
using PicoStatus;



namespace PS2000AEtsBlockModeExample
{

	struct ChannelSettings
	{
		public Imports.CouplingType couplingtype;
		public Imports.Range range;
		public bool enabled;
	}
	class PS2000AETSMode
	{

		public const int MAX_CHANNELS = 4;
		uint sampleCount = 10000;
		short _handle;
		bool _ready = false;
		private Imports.ps2000aBlockReady _callbackDelegate;
		private const short _oversample = 1;
		private static uint _timebase = 2;
		bool _scaleVoltages = true;
		ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };
		private ChannelSettings[] _channelSettings;
		private int channelCount = 2;

		/****************************************************************************
		 * Callback
		 * used by ps2000a data block collection calls, on receipt of data.
		 * used to set global flags etc checked by user routines
		 ****************************************************************************/
		private void BlockCallback(short handle, uint status, IntPtr pVoid)
		{
			// flag to say done reading data

			_ready = true;
		}

		/****************************************************************************
	   *  WaitForKey
	   *  Wait for user's keypress
	   ****************************************************************************/
		private static void WaitForKey()
		{
			while (!Console.KeyAvailable) Thread.Sleep(100);

			if (Console.KeyAvailable)
			{
				Console.ReadKey(true); // clear the key
			}
		}

		/****************************************************************************
		* mv_to_adc
		*
		* Convert a millivolt value into a 16-bit ADC count
		*
		*  (useful for setting trigger thresholds)
		****************************************************************************/
		short mv_to_adc(short mv, short ch)
		{
			return (short)((mv * Imports.MaxValue) / inputRanges[ch]);
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


		public PS2000AETSMode(short handle)
		{
			_handle = handle;
		}

		// MAIN
		static void Main(string[] args)
		{
			Console.WriteLine("PicoScope 2000 Series (ps2000a) Driver C# Block mode example");
			Console.WriteLine("\nOpening the device...");

			short handle;


			//OPEN UNIT
			uint status = Imports.OpenUnit(out handle, null);
			if (status != StatusCodes.PICO_OK)
			{
				Console.WriteLine("Unable to open device");
				Console.WriteLine("Error code : {0}", status);
				WaitForKey();
			}
			else
				Console.WriteLine("\nDevice successfully opened!..");
			Console.WriteLine("Press any key to begin");
			WaitForKey();

			PS2000AETSMode consoleExample = new PS2000AETSMode(handle);
			consoleExample.Run("First 10 readings", 0);
		}

		private void Run(string text, int offset)
		{

			uint status;

			// SETUP CHANNELS
			_channelSettings = new ChannelSettings[MAX_CHANNELS];
			int _channelCount = 2;
			for (int i = 0; i < _channelCount; i++) // reset channels to most recent settings
			{
				_channelSettings[i].enabled = true;
				_channelSettings[i].couplingtype = Imports.CouplingType.PS2000A_DC;
				_channelSettings[i].range = Imports.Range.Range_1V;

				Imports.SetChannel(_handle, Imports.Channel.ChannelA + i,
								   (short)(_channelSettings[(int)(Imports.Channel.ChannelA + i)].enabled ? 1 : 0),
								   _channelSettings[(int)(Imports.Channel.ChannelA + i)].couplingtype,
								   _channelSettings[(int)(Imports.Channel.ChannelA + i)].range,
								   (float)0.0);
				
			}

			// SET TIMEBASE
			
			int timeInterval;
			int maxSamples = 0;
			status = 1;
			do
			{
				status = Imports.GetTimebase(_handle, _timebase, (int)sampleCount, out timeInterval, _oversample, out maxSamples, 0);
				if (StatusCodes.PICO_OK != status)
				{
					Console.WriteLine("Selected timebase {0} could not be used. Error : 0x{1:X6}\n", _timebase, status);
					_timebase++;
				}
				Console.WriteLine("Timebase: {0}\t Sampling Interval (ns): {1}\n", _timebase, timeInterval);
			} while (StatusCodes.PICO_OK != status);

			

			// SETUP ETS
			int sampleTimePicoseconds;
			short etsCycles = 20;
			short etsInterleave = 4;
			Imports.SetEts(_handle, Imports.EtsMode.PS2000A_FAST, etsCycles, etsInterleave, out sampleTimePicoseconds);
			Console.WriteLine("ETS effective sampling time : {0}", sampleTimePicoseconds);
			if (StatusCodes.PICO_OK != status)
			{
				Console.WriteLine("Set ETS - Error : 0x{0:X6}", status);
				return;
			}

			// SETUP SIGGEN
			status = Imports.SetSigGenBuiltIn(_handle,0, 2000000,(int)Imports.WaveType.PS2000A_SINE,1000,1000,0,0,Imports.SweepType.PS2000A_DOWN,Imports.ExtraOperations.PS2000A_ES_OFF,0,0,Imports.SigGenTrigType.PS2000A_SIGGEN_RISING,Imports.SigGenTrigSource.PS2000A_SIGGEN_NONE,0);
			if (StatusCodes.PICO_OK != status)
			{
				Console.WriteLine("Sig gen - Error : 0x{0:X6}", status);
				
			}

			// SETUP A SIMPLE TRIGGER

			short triggerVoltage = mv_to_adc(0, (short)_channelSettings[(int)Imports.Channel.ChannelA].range); // Threshold value to be used for "SetSimpleTrigger"

			status = Imports.SetSimpleTrigger(_handle, 1, Imports.Channel.ChannelA, triggerVoltage, Imports.ThresholdDirection.Rising, 0, 0);
			if (status != StatusCodes.PICO_OK)
            {
                Console.WriteLine("set trigger error:0x{0:X6}", status);
            }

			// SETUP DATA BUFFERS TO TELL DRIVER WHERE TO STORE SAMPLED DATA
			
			PinnedArray<short>[] minPinned = new PinnedArray<short>[channelCount];
			PinnedArray<short>[] maxPinned = new PinnedArray<short>[channelCount];


			for (int i = 0; i < channelCount; i++)
			{
				short[] minBuffers = new short[sampleCount];
				short[] maxBuffers = new short[sampleCount];
				minPinned[i] = new PinnedArray<short>(minBuffers);
				maxPinned[i] = new PinnedArray<short>(maxBuffers);


				status = Imports.SetDataBuffers(_handle, (Imports.Channel)i, maxBuffers, minBuffers, (int)sampleCount, 0, Imports.RatioMode.None);
				if (status != StatusCodes.PICO_OK)
				{
					Console.WriteLine("BlockDataHandler:ps2000aSetDataBuffer Channel {0} Status = 0x{1:X6}", (char)('A' + i), status);
				}
			}

			//SETUP TIME BUFFERS TO TELL DRIVER WHERE TO STORE SAMPLE TIMES

			long[] etsTime = new long[sampleCount];
			PinnedArray<long> etsPinned = new PinnedArray<long>(etsTime);
			status = Imports.SetEtsTimeBuffer(_handle, etsTime, sampleCount);
			if (StatusCodes.PICO_OK != status)
			{
				Console.WriteLine("ETS Time Buffer - Error : 0x{0:X6}", status);
				return;
			}

			// RUN BLOCK
			int timeIndisposed;
			Console.WriteLine("Collecting Device Data...\n");
			Console.WriteLine("Starting Block Capture");

			/* Start it collecting, then wait for completion*/
			_ready = false;
			_callbackDelegate = BlockCallback;
			status = Imports.RunBlock(_handle, 0, (int)sampleCount, _timebase, _oversample, out timeIndisposed, 0, _callbackDelegate, IntPtr.Zero);
			if (status != StatusCodes.PICO_OK)
			{
				Console.WriteLine("set runblock error:0x{0:X6}", status);
			}
			while (!_ready)
			{
				Thread.Sleep(100);
			}
            Console.WriteLine("i have passed runblock");
			Imports.Stop(_handle);

			// EXTRACT VALUES FROM THE DEVICE
			if (_ready)
			{
				short overflow;
				Imports.GetValues(_handle, 0, ref sampleCount, 1, Imports.DownSamplingMode.None, 0, out overflow);

				/* Print out the first 10 readings, converting the readings to mV if required */
				Console.WriteLine(text);
				Console.WriteLine("readings will be in {0}", (_scaleVoltages) ? ("mV") : ("ADC Counts"));

				for (int ch = 0; ch < channelCount; ch++)
				{
					if (_channelSettings[ch].enabled)
					{
						Console.Write("   Ch{0}    ", (char)('A' + ch));
					}
				}
				Console.WriteLine();

				for (int i = offset; i < offset + 10; i++) //show data starting from  offset to 10, to print all values use samplecount
				{
					for (int ch = 0; ch < channelCount; ch++)
					{
						if (_channelSettings[ch].enabled)
						{
							Console.Write("{0,6}    ", _scaleVoltages ?
											  adc_to_mv(maxPinned[ch].Target[i], (int)_channelSettings[(int)(Imports.Channel.ChannelA + ch)].range)  // If _scaleVoltages, show mV values
											  : maxPinned[ch].Target[i]);                                                                           // else show ADC counts
						}
					}

					Console.WriteLine();

				}

			}

			foreach (PinnedArray<short> p in minPinned)
			{
				if (p != null)
				{
					p.Dispose();
				}
			}

			foreach (PinnedArray<short> p in maxPinned)
			{
				if (p != null)
				{
					p.Dispose();
				}
			}

		}

		
	}

	
	}

