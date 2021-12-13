using System;
using System.Collections.Generic;

using System.IO;
using System.Threading;
using System.Text;

using PS3000AImports;
using PicoPinnedArray;
using PicoStatus;

/*
Open unit

Setup channels

Set Timebase

Set up ETS using

Setup a simple Trigger.

Run Block

wait until unit is ready using the callback

Setup data buffers (Use ps3000aSetDataBuffer to tell the driver where your memory buffer is and use ps3000aSetEtsTimeBuffer or ps3000aSetEtsTimeBuffers to tell the driver where to store the sample times.)

Collect data

Display the data.

While you want to collect updated captures, repeat steps 7-10.

Repeat steps 6-11.

Stop unit

Close unit
*/

namespace PS3000AEtsBlockModeExample
{

	class Program
	{

		SortedDictionary<long, short> myDr1;
		SortedDictionary<long, short> myDr2;
		SortedDictionary<long, short> myDr3;
		SortedDictionary<long, short> myDr4;

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

			myDr1 = new SortedDictionary<long, short>();
			myDr2 = new SortedDictionary<long, short>();
			myDr3 = new SortedDictionary<long, short>();
			myDr4 = new SortedDictionary<long, short>();

			Int16 handle;
			uint status = StatusCodes.PICO_OK;

			// 1) Open unit
			status = Imports.OpenUnit(out handle, null);

			if (status != StatusCodes.PICO_OK)
			{
				status = PowerSourceSwitch(handle, status);

				if (status == StatusCodes.PICO_POWER_SUPPLY_UNDERVOLTAGE)
				{
					status = PowerSourceSwitch(handle, status);
				}
				else if (status == StatusCodes.PICO_USB3_0_DEVICE_NON_USB3_0_PORT)
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
				_channelSettings[i].DCcoupled = Imports.Coupling.DC;
				_channelSettings[i].range = Imports.Range.Range_20V;

				Imports.SetChannel(handle, Imports.Channel.ChannelA + i,
								   (short)(_channelSettings[(int)(Imports.Channel.ChannelA + i)].enabled ? 1 : 0),
								   _channelSettings[(int)(Imports.Channel.ChannelA + i)].DCcoupled,
								   _channelSettings[(int)(Imports.Channel.ChannelA + i)].range,
								   (float)0.0);
				if (0 != status)
					return;
			}

			// 3) Set Timebase
			uint _timebase = 100;
			const uint sampleCount = 50;
			int timeInterval;
			short _oversample = 0;
			int maxSamples = 0;
			status = 1;
			while (status != StatusCodes.PICO_OK)
			{
				status = Imports.GetTimebase(handle, _timebase, (int)sampleCount, out timeInterval, _oversample, out maxSamples, 0);
				Console.WriteLine("Selected timebase {0} could not be used. {1}\n", _timebase , status);
				_timebase++;
			}

			// 4) Set up ETS
			int sampleTimePicoseconds;
			Imports.SetEts(handle, Imports.EtsMode.PS3000A_MAX_SWEEP_TYPES, 20, 4, out sampleTimePicoseconds);
			if (0 != status)
				return;

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
			SetTrigger(handle, sourceDetails, 1, conditions, 1, directions, null, 0, 0, 100);

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
						Console.WriteLine("IN");
						minBuffers[i] = new short[sampleCount];
						maxBuffers[i] = new short[sampleCount];
						minPinned[i] = new PinnedArray<short>(minBuffers[i]);
						maxPinned[i] = new PinnedArray<short>(maxBuffers[i]);
						uint segmentIndex = 0;

						status = Imports.SetDataBuffer(handle, (Imports.Channel)i, minBuffers[i], (int)sampleCount, segmentIndex, Imports.RatioMode.None);
						if (0 != status)
							return;

						if (status != StatusCodes.PICO_OK)
						{
							Console.WriteLine("BlockDataHandler:Imports.SetDataBuffers Channel {0} Status = 0x{1:X6}", (char)('A' + i), status);
						}
					}
				}
			}

			long[] etsTime = new long[sampleCount];
			PinnedArray<long> etsPinned = new PinnedArray<long>(etsTime);
			status = Imports.SetEtsTimeBuffer(handle, etsTime, sampleCount);
			if (0 != status)
			{
				Console.WriteLine("ETS Time Buffer - Status : {0}", status);
			}

			// 6) Run Block
			int timeIndisposed;
			_callbackDelegate = BlockCallback;

			int loopCount = 0;
			int maxLoop = 10;
			while (loopCount < maxLoop) {
				Console.WriteLine("loopCount: {0} and maxLoop: {1}",
										  loopCount, maxLoop);
				loopCount++;
				
				status = Imports.RunBlock(handle, 0, (int)sampleCount, _timebase, _oversample, out timeIndisposed, 0, _callbackDelegate, IntPtr.Zero);
				if (0 != status)
				{
					Console.WriteLine("Run Block - Status : {0}", status);
				}

				short status2=0;

				Thread.Sleep(1000);

				// 7) wait until unit is ready using the callback
				// Performed in the Callback Function

				// 8) Extract the values from the device.
				short overflow;
				uint finalSampleCount = sampleCount;
				status = Imports.GetValues(handle, 0, ref finalSampleCount, 0, Imports.RatioMode.None, 0, out overflow);
				if (0 != status)
				{
					Console.WriteLine("Get Values - Status : {0}", status);
				}

				// 9) Collect data
				Console.WriteLine("Count : {0}", myDr1.Count);
                for (int i = 0; i < sampleCount; i++)
                {
                    Console.WriteLine("Values : {0} {1}", etsTime[i], minBuffers[0][i]);
                    myDr1[etsTime[i]] = minBuffers[0][i];
                    myDr2[etsTime[i]] = minBuffers[1][i];
                    myDr3[etsTime[i]] = minBuffers[2][i];
                    myDr4[etsTime[i]] = minBuffers[3][i];
                }

                // 10) Display the data.
                // Display the key/value pairs
                Console.WriteLine("Count : {0}", myDr1.Count);
				foreach (KeyValuePair<long, short> pair in myDr1)
				{
					Console.WriteLine("Key: {0} and Value: {1}",
										  pair.Key, pair.Value);
				}
				foreach (KeyValuePair<long, short> pair in myDr2)
				{
					Console.WriteLine("Key: {0} and Value: {1}",
										  pair.Key, pair.Value);
				}
				foreach (KeyValuePair<long, short> pair in myDr3)
				{
					Console.WriteLine("Key: {0} and Value: {1}",
										  pair.Key, pair.Value);
				}
				foreach (KeyValuePair<long, short> pair in myDr4)
				{
					Console.WriteLine("Key: {0} and Value: {1}",
										  pair.Key, pair.Value);
				}

				// 11) While you want to collect updated captures, repeat steps 7 - 10.

				Thread.Sleep(1000);

				// 12) Repeat steps 6 - 11.
			}
			// 13) Stop unit
			status = Imports.Stop(handle);
			Thread.Sleep(10000);

			// 14) Close unit
			status = Imports.CloseUnit(handle);

			Console.WriteLine("Hello World!");
		}

		static void Main(string[] args)
		{
			Program consoleExample = new Program();
			consoleExample.Run();

        }
    }

	struct ChannelSettings
	{
		public Imports.Coupling DCcoupled;
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
