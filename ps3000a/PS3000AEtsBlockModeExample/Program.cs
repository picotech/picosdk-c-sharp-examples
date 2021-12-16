using System;
using System.Collections.Generic;

using System.IO;
using System.Threading;
using System.Text;

using PS3000AImports;
using PicoPinnedArray;
using PicoStatus;

/*
	Steps :
	1) Open unit
	2) Setup channels
	3) Set Timebase
	4) Set up ETS using
	5) Setup a simple Trigger.
	6) Run Block
	7) wait until unit is ready using the callback
	8) Setup data buffers (Use ps3000aSetDataBuffer to tell the driver where your memory buffer is and use ps3000aSetEtsTimeBuffer or ps3000aSetEtsTimeBuffers to tell the driver where to store the sample times.)
	9) Collect data
	10) Display the data.
	11) While you want to collect updated captures, repeat steps 7-10.
	12) Repeat steps 6-11.
	13) Stop unit
	14) Close unit
*/

namespace PS3000AEtsBlockModeExample
{

	class Program
	{

		SortedDictionary<long, short> EtsSortedDictionaryChannelA;
		SortedDictionary<long, short> EtsSortedDictionaryChannelB;
		SortedDictionary<long, short> EtsSortedDictionaryChannelC;
		SortedDictionary<long, short> EtsSortedDictionaryChannelD;

		public bool _ready = false;
		public void BlockCallback(short handle, short status, IntPtr pVoid)
		{
			// flag to say done reading data
			if (status != (short)StatusCodes.PICO_CANCELLED)
			{
				_ready = true;
			}
		}

		private Imports.ps3000aBlockReady _callbackDelegate;

		/****************************************************************************
		*  SetTrigger  (Analogue Channels)
		*  This function sets all the required trigger parameters, and calls the 
		*  triggering functions
		****************************************************************************/
		uint SetTrigger(short _handle , Imports.TriggerChannelProperties[] channelProperties,
							short nChannelProperties,
							Imports.TriggerConditions[] triggerConditions,
							short nTriggerConditions,
							Imports.ThresholdDirection[] directions,
							Pwq pwq,
							uint delay,
							short auxOutputEnabled,
							int autoTriggerMs)
		{
			uint status;

			if ((status = Imports.SetTriggerChannelProperties(_handle, channelProperties, nChannelProperties, auxOutputEnabled,
																autoTriggerMs)) != 0)
			{
				return status;
			}

			if ((status = Imports.SetTriggerChannelConditions(_handle, triggerConditions, nTriggerConditions)) != 0)
			{
				return status;
			}

			if (directions == null) directions = new Imports.ThresholdDirection[] { Imports.ThresholdDirection.None,
				Imports.ThresholdDirection.None, Imports.ThresholdDirection.None, Imports.ThresholdDirection.None,
				Imports.ThresholdDirection.None, Imports.ThresholdDirection.None};

			if ((status = Imports.SetTriggerChannelDirections(_handle,
															  directions[(int)Imports.Channel.ChannelA],
															  directions[(int)Imports.Channel.ChannelB],
															  directions[(int)Imports.Channel.ChannelC],
															  directions[(int)Imports.Channel.ChannelD],
															  directions[(int)Imports.Channel.External],
															  directions[(int)Imports.Channel.Aux])) != 0)
			{
				return status;
			}

			if ((status = Imports.SetTriggerDelay(_handle, delay)) != 0)
			{
				return status;
			}
			return status;
		}
		/****************************************************************************
		* PowerSourceSwitch - Handle status errors connected with the power source
		****************************************************************************/

		static uint PowerSourceSwitch(short handle, uint status)
		{
			char ch;

			switch (status)
			{
				case StatusCodes.PICO_POWER_SUPPLY_NOT_CONNECTED:
					do
					{
						Console.WriteLine("5V Power Supply not connected\nDo you want to run using power from the USB lead? Y\\N");
						ch = char.ToUpper(Console.ReadKey(true).KeyChar);
						if (ch == 'Y')
						{
							Console.WriteLine("Powering the unit via USB");
							status = Imports.ChangePowerSource(handle, status);
						}
					}
					while (ch != 'Y' && ch != 'N');
					Console.WriteLine(ch == 'N' ? "Please use the 5V power supply to power this unit" : "");
					break;

				case StatusCodes.PICO_POWER_SUPPLY_CONNECTED:
					Console.WriteLine("Using 5V power supply voltage");
					status = Imports.ChangePowerSource(handle, status);
					break;

				case StatusCodes.PICO_POWER_SUPPLY_UNDERVOLTAGE:
					do
					{
						Console.WriteLine("");
						Console.WriteLine("\nUSB not supplying required voltage");
						Console.WriteLine("\nPlease plug in the +5V power supply,");
						Console.WriteLine("then hit a key to continue, or Esc to exit...");
						ch = Console.ReadKey().KeyChar;

						if (ch == 0x1B)
						{
							Environment.Exit(0);
						}
						status = PowerSourceSwitch(handle, StatusCodes.PICO_POWER_SUPPLY_CONNECTED);
					}
					while (status == (short)StatusCodes.PICO_POWER_SUPPLY_REQUEST_INVALID);
					break;
			}
			return status;
		}

		public void Run()
		{
			// [ 1 , 6 , 11 , 16 ] => [ 1 , 6 , 11 , 16 ]
			// [ 2 , 7 , 12 , 17 ] => [ 1 , 2 , 6 , 7 , 11 , 12 , 16 , 17 ]
			EtsSortedDictionaryChannelA = new SortedDictionary<long, short>();
			EtsSortedDictionaryChannelB = new SortedDictionary<long, short>();
			EtsSortedDictionaryChannelC = new SortedDictionary<long, short>();
			EtsSortedDictionaryChannelD = new SortedDictionary<long, short>();

			Int16 handle;
			uint status = StatusCodes.PICO_OK;

			// 1) Open unit
			status = Imports.OpenUnit(out handle, null);
			if (StatusCodes.PICO_OK != status)
			{
				status = PowerSourceSwitch(handle, status);

				if (StatusCodes.PICO_POWER_SUPPLY_UNDERVOLTAGE == status)
				{
					status = PowerSourceSwitch(handle, status);
				}
				else if (StatusCodes.PICO_USB3_0_DEVICE_NON_USB3_0_PORT == status)
				{
					status = PowerSourceSwitch(handle, StatusCodes.PICO_POWER_SUPPLY_NOT_CONNECTED);
				}
				else
				{
					// Do nothing
				}
			}


			// 2) Setup channels
			ChannelSettings[] _channelSettings = new ChannelSettings[4];
			int _channelCount = 4;
			for (int i = 0; i < _channelCount; i++) // reset channels to most recent settings
			{
				_channelSettings[i].enabled = true;
				_channelSettings[i].coupled = Imports.Coupling.AC;
				_channelSettings[i].range = Imports.Range.Range_1V;

				Imports.SetChannel(handle, Imports.Channel.ChannelA + i,
								   (short)(_channelSettings[(int)(Imports.Channel.ChannelA + i)].enabled ? 1 : 0),
								   _channelSettings[(int)(Imports.Channel.ChannelA + i)].coupled,
								   _channelSettings[(int)(Imports.Channel.ChannelA + i)].range,
								   (float)0.0);
				if (StatusCodes.PICO_OK != status)
				{
					Console.WriteLine("Set Channel {0} - Error = 0x{0:X6}", (char)('A' + i), status);
					return;
				}
			}

			// 3) Set Timebase
			uint _timebase = 1;
			const uint sampleCount = 200000;
			int timeInterval;
			short _oversample = 0;
			int maxSamples = 0;
			status = 1;
			do
			{
				status = Imports.GetTimebase(handle, _timebase, (int)sampleCount, out timeInterval, _oversample, out maxSamples, 0);
				if (StatusCodes.PICO_OK != status)
				{
					Console.WriteLine("Selected timebase {0} could not be used. Error : 0x{1:X6}\n", _timebase, status);
					_timebase++;
				}
			} while (StatusCodes.PICO_OK != status);

			uint pkToPk = 2000000;
			double frequency = 100000;
			Imports.SetSigGenBuiltInV2(
				handle, 
				0, pkToPk, 
				Imports.WaveType.PS3000A_SINE, 
				frequency, frequency, 0, 0, 
				Imports.SweepType.PS3000A_DOWN, 
				Imports.ExtraOperations.PS3000A_ES_OFF, 
				0, 0, 
				Imports.SigGenTrigType.PS3000A_SIGGEN_RISING, 
				Imports.SigGenTrigSource.PS3000A_SIGGEN_NONE, 0);

			// 4) Set up ETS
			int sampleTimePicoseconds;
			short etsCycles = 20;
			short etsInterleave = 4;
			Imports.SetEts(handle, Imports.EtsMode.PS3000A_SLOW, etsCycles, etsInterleave, out sampleTimePicoseconds);
			Console.WriteLine(" SampleTime SetTime : {0}", sampleTimePicoseconds);
			if (StatusCodes.PICO_OK != status)
			{
				Console.WriteLine("Set ETS - Error : 0x{0:X6}", status);
				return;
			}

			// 5) Setup a simple Trigger.
			/* Trigger enabled
			 * Rising edge
			 * Threshold = 1000mV */
			short triggerVoltage = 100;
			Imports.TriggerChannelProperties[] sourceDetails = new Imports.TriggerChannelProperties[] {
				new Imports.TriggerChannelProperties(triggerVoltage,
														 0,
														 triggerVoltage,
														 0,
														 Imports.Channel.ChannelA,
														 Imports.ThresholdMode.Level)};

			Imports.TriggerConditions[] conditions = new Imports.TriggerConditions[] {
			  new Imports.TriggerConditions(Imports.TriggerState.True,
											Imports.TriggerState.DontCare,
											Imports.TriggerState.DontCare,
											Imports.TriggerState.DontCare,
											Imports.TriggerState.DontCare,
											Imports.TriggerState.DontCare,
											Imports.TriggerState.DontCare)};

			Imports.ThresholdDirection[] directions = new Imports.ThresholdDirection[]
											{ Imports.ThresholdDirection.Rising,
											Imports.ThresholdDirection.None,
											Imports.ThresholdDirection.None,
											Imports.ThresholdDirection.None,
											Imports.ThresholdDirection.None,
											Imports.ThresholdDirection.None };

			int autoTrigger = 100;
			status = SetTrigger(handle, sourceDetails, 1, conditions, 1, directions, null, 0, 0, autoTrigger);
			if (StatusCodes.PICO_OK != status)
			{
				Console.WriteLine("Set Trigger - Error : 0x{0:X6}", status);
				return;
			}

			// 8) Setup data buffers(Use ps3000aSetDataBuffer to tell the driver where your memory buffer is and use ps3000aSetEtsTimeBuffer or ps3000aSetEtsTimeBuffers to tell the driver where to store the sample times.)
			PinnedArray<short>[] minPinned = new PinnedArray<short>[_channelCount];
			PinnedArray<short>[] maxPinned = new PinnedArray<short>[_channelCount];

			short[][] minBuffers = new short[_channelCount][];
			short[][] maxBuffers = new short[_channelCount][];
			Imports.Mode mode = Imports.Mode.ANALOGUE;
			if (mode == Imports.Mode.ANALOGUE || mode == Imports.Mode.MIXED)
			{

				// Set up data buffers for average data collection on analogue channels

				for (int i = 0; i < _channelCount; i++)
				{
					if (_channelSettings[i].enabled)
					{
						minBuffers[i] = new short[sampleCount];
						maxBuffers[i] = new short[sampleCount];
						minPinned[i] = new PinnedArray<short>(minBuffers[i]);
						maxPinned[i] = new PinnedArray<short>(maxBuffers[i]);

						uint segmentIndex = 0;
						status = Imports.SetDataBuffers(handle, (Imports.Channel)i, minBuffers[i], maxBuffers[i], (int)sampleCount, segmentIndex, Imports.RatioMode.Aggregate);

						if (StatusCodes.PICO_OK != status)
						{
							Console.WriteLine("SetDataBuffers Channel {0} - Error = 0x{1:X6}", (char)('A' + i), status);
							return;
						}
					}
				}
			}

			long[] etsTime = new long[sampleCount];
			PinnedArray<long> etsPinned = new PinnedArray<long>(etsTime);
			status = Imports.SetEtsTimeBuffer(handle, etsTime, sampleCount);
			if (StatusCodes.PICO_OK != status)
			{
				Console.WriteLine("ETS Time Buffer - Error : 0x{0:X6}", status);
				return;
			}

			// 6) Run Block
			int timeIndisposed;
			_callbackDelegate = BlockCallback;

			int loopCount = 0;
			int maxLoop = 3;
			while (loopCount < maxLoop) {
				_ready = false;
				Console.WriteLine("loopCount: {0} and maxLoop: {1}",
										  loopCount, 
										  maxLoop);
				loopCount++;
				
				status = Imports.RunBlock(handle, 0, (int)sampleCount, _timebase, _oversample, out timeIndisposed, 0, _callbackDelegate, IntPtr.Zero);
				if (StatusCodes.PICO_OK != status)
				{
					Console.WriteLine("Run Block - Error : 0x{0:X6}", status);
					return;
				}

				short status2=0;

				// 7) wait until unit is ready using the callback
				while ( !_ready ) ;

				// 8) Extract the values from the device.
				short overflow;
				uint finalSampleCount = sampleCount;
				status = Imports.GetValues(handle, 0, ref finalSampleCount, 0, Imports.RatioMode.Aggregate, 0, out overflow);
				if (StatusCodes.PICO_OK != status)
				{
					Console.WriteLine("Get Values - Error : 0x{0:X6}", status);
					return;
				}

				// 9) Collect data
				Console.WriteLine("Count : {0}", EtsSortedDictionaryChannelA.Count);
                for (int i = 0; i < sampleCount; i++)
                {
                    EtsSortedDictionaryChannelA[etsTime[i]] = minBuffers[0][i];
                    EtsSortedDictionaryChannelB[etsTime[i]] = minBuffers[1][i];
                    EtsSortedDictionaryChannelC[etsTime[i]] = minBuffers[2][i];
                    EtsSortedDictionaryChannelD[etsTime[i]] = minBuffers[3][i];
                }

                // 10) Display the data.
                // Display the results at the end

				// 11) While you want to collect updated captures, repeat steps 7 - 10.
				Thread.Sleep(1000);

				// 12) Repeat steps 6 - 11.
			}
			// 13) Stop unit
			status = Imports.Stop(handle);
			if (StatusCodes.PICO_OK != status)
			{
				Console.WriteLine("Unit Stop - Error : 0x{0:X6}", status);
				return;
			}
			Thread.Sleep(1000);

			// Display the key/value pairs
			foreach (KeyValuePair<long, short> pair in EtsSortedDictionaryChannelA)
			{
				Console.WriteLine("Key: {0} and Value: {1} {2} {3} {4}",
									  pair.Key, 
									  EtsSortedDictionaryChannelA[pair.Key], 
									  EtsSortedDictionaryChannelB[pair.Key], 
									  EtsSortedDictionaryChannelC[pair.Key], 
									  EtsSortedDictionaryChannelD[pair.Key]);
			}
			Console.WriteLine("Count : {0}", EtsSortedDictionaryChannelA.Count);
			Console.WriteLine("Count : {0}", EtsSortedDictionaryChannelB.Count);
			Console.WriteLine("Count : {0}", EtsSortedDictionaryChannelC.Count);
			Console.WriteLine("Count : {0}", EtsSortedDictionaryChannelD.Count);

			// 14) Close unit
			status = Imports.CloseUnit(handle);
			{
				Console.WriteLine("Close Unit - Error : 0x{0:X6}", status);
				return;
			}

		}

		static void Main(string[] args)
		{
			Program consoleExample = new Program();
			consoleExample.Run();
        }
    }

	struct ChannelSettings
	{
		public Imports.Coupling coupled;
		public Imports.Range range;
		public bool enabled;
	}

	class Pwq
	{
		public Imports.PwqConditions[] conditions;
		public short nConditions;
		public Imports.ThresholdDirection direction;
		public uint lower;
		public uint upper;
		public Imports.PulseWidthType type;

		public Pwq(Imports.PwqConditions[] conditions,
			short nConditions,
			Imports.ThresholdDirection direction,
			uint lower, uint upper,
			Imports.PulseWidthType type)
		{
			this.conditions = conditions;
			this.nConditions = nConditions;
			this.direction = direction;
			this.lower = lower;
			this.upper = upper;
			this.type = type;
		}
	}

}
