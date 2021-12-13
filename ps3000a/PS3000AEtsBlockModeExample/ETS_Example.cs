/**************************************************************************
 *
 * Filename: PS3000ACSConsole.cs
 *
 * Description:
 *   This is a console-mode program that demonstrates how to use the
 *   PicoScope 3000 Series (ps3000a) driver functions. using .NET
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
 *    Collect a block of Analogue samples immediately
 *    Collect a block of Analogue samples when a trigger event occurs
 *    Collect Analogue samples in rapid block mode when a trigger event occurs
 *    Collect a stream of Analogue data 
 *    Collect a stream of Analogue data and show when a trigger event occurs
 *   
 *    Collect a block of Digital Samples immediately
 *    Collect a block of Digital Samples when a Digital trigger event occurs
 *    Collect a block of Digital and Analogue Samples when an Analogue AND a Digital trigger event occurs
 *    Collect a block of Digital and Analogue Samples when an Analogue OR a Digital trigger event occurs
 *    Collect a stream of Digital Samples 
 *    Collect a stream of Digital Samples and show Aggregated results
 *
 * Copyright (C) 2011-2018 Pico Technology Ltd. See LICENSE file for terms.
 *  
 **************************************************************************/

using System;
using System.IO;
using System.Threading;
using System.Text;

using PS3000AImports;
using PicoPinnedArray;
using PicoStatus;

namespace PS3000ACSConsole
{
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

	class PS3000ACSConsole
	{

		private readonly short _handle;
		public const int BUFFER_SIZE = 1024; // Number of samples per waveform capture for block captures
		public const int RAPID_BLOCK_BUFFER_SIZE = 100; // Number of samples per waveform capture for rapid block captures
		public const int MAX_CHANNELS = 4;
		public const int QUAD_SCOPE = 4;
		public const int DUAL_SCOPE = 2;


		uint _timebase = 8;
		short _oversample = 1;
		bool _scaleVoltages = true;

		ushort[] inputRanges = { 10, 20, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000 };

		bool _ready = false;
		short _trig = 0;
		uint _trigAt = 0;
		int _sampleCount = 0;
		uint _startIndex = 0;
		bool _autoStop;

		// Buffers used for streaming data collection

		short[][] appBuffers;
		short[][] buffers;
		short[][] appDigiBuffers;
		short[][] digiBuffers;

		private ChannelSettings[] _channelSettings;
		private int _channelCount;
		private Imports.Range _firstRange;
		private Imports.Range _lastRange;
		private int _digitalPorts;
		private Imports.ps3000aBlockReady _callbackDelegate;
		private string StreamFile = "stream.txt";
		private string BlockFile = "block.txt";

		/****************************************************************************
		 * BlockCallback
		 * used by ps3000a data block collection calls, on receipt of data.
		 * used to set global flags etc checked by user routines
		 ****************************************************************************/
		void BlockCallback(short handle, short status, IntPtr pVoid)
		{
			// flag to say done reading data
			if (status != (short)StatusCodes.PICO_CANCELLED)
			{
				_ready = true;
			}
		}

		/****************************************************************************
		 * SetDefaults - restore default settings
		 ****************************************************************************/
		void SetDefaults()
		{
			for (int i = 0; i < _channelCount; i++) // reset channels to most recent settings
			{
				Imports.SetChannel(_handle, Imports.Channel.ChannelA + i,
								   (short)(_channelSettings[(int)(Imports.Channel.ChannelA + i)].enabled ? 1 : 0),
								   _channelSettings[(int)(Imports.Channel.ChannelA + i)].DCcoupled,
								   _channelSettings[(int)(Imports.Channel.ChannelA + i)].range,
								   (float)0.0);
			}
		}

		/****************************************************************************
		 * adc_to_mv
		 *
		 * Convert an 16-bit ADC count into millivolts
		 ****************************************************************************/
		int adc_to_mv(int raw, int ch)
		{
			return (raw * inputRanges[ch]) / Imports.MaxValue;
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
		 * BlockDataHandler
		 * - Used by all block data routines
		 * - acquires data (user sets trigger mode before calling), displays 10 items
		 *   and saves all to block.txt
		 * Input :
		 * - text : the text to display before the display of data slice
		 * - offset : the offset into the data buffer to start the display's slice.
		 ****************************************************************************/
		void EtsBlockDataHandler(string text, int offset, Imports.Mode mode)
		{
			uint status;
			bool retry;
			uint sampleCount = BUFFER_SIZE;
			uint segmentIndex = 0;

			PinnedArray<short>[] minPinned = new PinnedArray<short>[_channelCount];
			PinnedArray<short>[] maxPinned = new PinnedArray<short>[_channelCount];
			PinnedArray<short>[] digiPinned = new PinnedArray<short>[_digitalPorts];
			PinnedArray<long> etsPinned;

			int timeIndisposed;

			if (mode == Imports.Mode.ANALOGUE || mode == Imports.Mode.MIXED)
			{

				// Set up data buffers for average data collection on analogue channels

				for (int i = 0; i < _channelCount; i++)
				{
					if (_channelSettings[i].enabled)
					{
						short[] minBuffers = new short[sampleCount];
						short[] maxBuffers = new short[sampleCount];
						minPinned[i] = new PinnedArray<short>(minBuffers);
						maxPinned[i] = new PinnedArray<short>(maxBuffers);

						status = Imports.SetDataBuffers(_handle, (Imports.Channel)i, maxBuffers, minBuffers, (int)sampleCount, segmentIndex, Imports.RatioMode.Average);

						if (status != StatusCodes.PICO_OK)
						{
							Console.WriteLine("BlockDataHandler:Imports.SetDataBuffers Channel {0} Status = 0x{1:X6}", (char)('A' + i), status);
						}
					}
				}
			}

			if (mode == Imports.Mode.DIGITAL || mode == Imports.Mode.MIXED)
			{
				for (int i = 0; i < _digitalPorts; i++)
				{
					short[] digiBuffer = new short[sampleCount];
					digiPinned[i] = new PinnedArray<short>(digiBuffer);

					status = Imports.SetDataBuffer(_handle, i + Imports.Channel.PS3000A_DIGITAL_PORT0, digiBuffer, (int)sampleCount, segmentIndex, Imports.RatioMode.None);

					if (status != StatusCodes.PICO_OK)
					{
						Console.WriteLine("BlockDataHandler:Imports.SetDataBuffer {0} Status = 0x{1,0:X6}", i + Imports.Channel.PS3000A_DIGITAL_PORT0, status);
					}
				}
			}
			long[] etsTime = new long[sampleCount];
			etsPinned = new PinnedArray<long>(etsTime);
			status = Imports.SetEtsTimeBuffer(_handle, etsTime, sampleCount);

			/* Find the maximum number of samples and the time interval (in nanoseconds) at the current _timebase. */
			/* Use downsampling as opposed to oversampling if required. */
			int timeInterval;
			int maxSamples;

			while (Imports.GetTimebase(_handle, _timebase, (int)sampleCount, out timeInterval, _oversample, out maxSamples, 0) != 0)
			{
				Console.WriteLine("Selected timebase {0} could not be used.\n", _timebase);
				_timebase++;

			}
			Console.WriteLine("Timebase: {0}\t Sampling Interval (ns): {1}\n", _timebase, timeInterval);

			/* Start it collecting, then wait for completion*/
			_ready = false;
			_callbackDelegate = BlockCallback;

			do
			{
				retry = false;

				status = Imports.RunBlock(_handle, 0, (int)sampleCount, _timebase, _oversample, out timeIndisposed, 0, _callbackDelegate, IntPtr.Zero);

				if (status == StatusCodes.PICO_POWER_SUPPLY_CONNECTED || status == StatusCodes.PICO_POWER_SUPPLY_NOT_CONNECTED || status == StatusCodes.PICO_POWER_SUPPLY_UNDERVOLTAGE)
				{
					status = PowerSourceSwitch(_handle, status);
					retry = true;
				}
				else
				{
					Console.WriteLine(status != (short)StatusCodes.PICO_OK ? "BlockDataHandler:Imports.RunBlock Status = 0x{0:X6}" : "", status);
				}
			}
			while (retry);

			Console.WriteLine("Waiting for data...Press a key to abort");
			Console.WriteLine();


			while (!_ready && !Console.KeyAvailable)
			{
				Thread.Sleep(10);
			}

			if (Console.KeyAvailable)
			{
				Console.ReadKey(true); // clear the key
			}

			Imports.Stop(_handle);

			if (_ready)
			{
				short overflow;
				uint startIndex = 0;
				uint downSampleRatio = 2;

				// Request data from the driver for analogue channels
				if (mode == Imports.Mode.ANALOGUE || mode == Imports.Mode.MIXED)
				{
					status = Imports.GetValues(_handle, startIndex, ref sampleCount, downSampleRatio, Imports.RatioMode.Average, segmentIndex, out overflow); // Average data collection
				}

				if (mode == Imports.Mode.DIGITAL || mode == Imports.Mode.MIXED)
				{
					status = Imports.GetValues(_handle, startIndex, ref sampleCount, downSampleRatio, Imports.RatioMode.None, segmentIndex, out overflow);
				}

				if (status == StatusCodes.PICO_OK)
				{

					/* Print out the first 10 readings, converting the readings to mV if required */
					Console.WriteLine(text);
					Console.WriteLine();

					if (mode == Imports.Mode.ANALOGUE || mode == Imports.Mode.MIXED)
					{
						Console.WriteLine("Value {0}", (_scaleVoltages) ? ("mV") : ("ADC Counts"));

						for (int ch = 0; ch < _channelCount; ch++)
						{
							if (_channelSettings[ch].enabled)
							{
								Console.Write("Channel {0}          ", (char)('A' + ch));
							}
						}
					}
					if (mode == Imports.Mode.DIGITAL || mode == Imports.Mode.MIXED)
					{
						Console.Write("DIGITAL VALUE");
					}

					Console.WriteLine();


					for (int i = offset; i < offset + 10; i++)
					{
						if (mode == Imports.Mode.ANALOGUE || mode == Imports.Mode.MIXED)
						{
							for (int ch = 0; ch < _channelCount; ch++)
							{
								if (_channelSettings[ch].enabled)
								{
									Console.Write("{0,8}          ", _scaleVoltages ?
										adc_to_mv(maxPinned[ch].Target[i], (int)_channelSettings[(int)(Imports.Channel.ChannelA + ch)].range)  // If _scaleVoltages, show mV values
										: maxPinned[ch].Target[i]);                                                                           // else show ADC counts
								}
							}
						}
						if (mode == Imports.Mode.DIGITAL || mode == Imports.Mode.MIXED)
						{

							short digiValue = (short)(0x00ff & digiPinned[1].Target[i]); // Mask lower 8-bits to get Port 1 data values
							digiValue <<= 8;    // Shift by 8 bits
							digiValue |= (short)(0x00ff & digiPinned[0].Target[i]); // Mask Port 0 values to get lower 8 bits and apply bitwise inclusive OR to combine with Port 1 values  
							Console.Write("0x{0,4:X}", digiValue.ToString("X4"));
						}
						Console.WriteLine();
					}

					if (mode == Imports.Mode.ANALOGUE || mode == Imports.Mode.MIXED)
					{
						PrintBlockFile(Math.Min(sampleCount, BUFFER_SIZE), timeInterval, minPinned, maxPinned);
					}
				}
				else
				{
					if (status == (short)StatusCodes.PICO_POWER_SUPPLY_CONNECTED || status == (short)StatusCodes.PICO_POWER_SUPPLY_NOT_CONNECTED || status == (short)StatusCodes.PICO_POWER_SUPPLY_UNDERVOLTAGE)
					{
						if (status == (short)StatusCodes.PICO_POWER_SUPPLY_UNDERVOLTAGE)
						{
							status = PowerSourceSwitch(_handle, status);
						}
						else
						{
							Console.WriteLine("Power source changed. Data collection aborted");
						}
					}
					else
					{
						Console.WriteLine("BlockDataHandler:Imports.GetValues Status = 0x{0:X6}", status);
					}
				}
			}
			else
			{
				Console.WriteLine("Data collection aborted.");
			}

			Imports.Stop(_handle);

			if (mode == Imports.Mode.ANALOGUE || mode == Imports.Mode.MIXED)
			{
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

			if (mode == Imports.Mode.DIGITAL || mode == Imports.Mode.MIXED)
			{
				foreach (PinnedArray<short> p in digiPinned)
				{
					if (p != null)
					{
						p.Dispose();
					}
				}
			}
		}

		/// <summary>
		/// Print the block data capture to file 
		/// </summary>
		private void PrintBlockFile(uint sampleCount, int timeInterval, PinnedArray<short>[] minPinned, PinnedArray<short>[] maxPinned)
		{
			var sb = new StringBuilder();

			sb.AppendFormat("For each of the {0} Channels, results shown are....", _channelCount);
			sb.AppendLine();
			sb.AppendLine("Time interval Maximum Aggregated value ADC Count & mV, Minimum Aggregated value ADC Count & mV");
			sb.AppendLine();

			// Build Header
			string[] heading = { "Time", "Channel", "Max ADC", "Max mV", "Min ADC", "Min mV" };

			for (int i = 0; i < _channelCount; i++)
			{
				if (_channelSettings[i].enabled)
				{
					sb.AppendFormat("{0,10} {1,10} {2,10} {3,10} {4,10} {5,10}",
									heading[0],
									heading[1],
									heading[2],
									heading[3],
									heading[4],
									heading[5]);
				}
			}

			sb.AppendLine();

			// Build Body
			for (int i = 0; i < sampleCount; i++)
			{
				for (int ch = 0; ch < _channelCount; ch++)
				{
					if (_channelSettings[ch].enabled)
					{
						sb.AppendFormat("{0,10} {1,10} {2,10} {3,10} {4,10} {5,10}",
										i * timeInterval,
										(char)('A' + ch),
										maxPinned[ch].Target[i],
										adc_to_mv(maxPinned[ch].Target[i], (int)_channelSettings[(int)(Imports.Channel.ChannelA + ch)].range),
										minPinned[ch].Target[i],
										adc_to_mv(minPinned[ch].Target[i], (int)_channelSettings[(int)(Imports.Channel.ChannelA + ch)].range));
					}
				}

				sb.AppendLine();
			}

			// Print contents to file
			using (TextWriter writer = new StreamWriter(BlockFile, false))
			{
				writer.Write(sb.ToString());
				writer.Close();
			}
		}



		/****************************************************************************
		*  WaitForKey
		*  Wait for user's keypress
		****************************************************************************/
		private static void WaitForKey()
		{
			while (!Console.KeyAvailable)
			{
				Thread.Sleep(100);
			}

			if (Console.KeyAvailable)
			{
				Console.ReadKey(true); // clear the key
			}
		}

		/****************************************************************************
		*  SetTrigger  (Analogue Channels)
		*  This function sets all the required trigger parameters, and calls the 
		*  triggering functions
		****************************************************************************/
		uint SetTrigger(Imports.TriggerChannelProperties[] channelProperties,
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

			if (pwq == null) pwq = new Pwq(null, 0, Imports.ThresholdDirection.None, 0, 0, Imports.PulseWidthType.None);

			status = Imports.SetPulseWidthQualifier(_handle, pwq.conditions,
													pwq.nConditions, pwq.direction,
													pwq.lower, pwq.upper, pwq.type);

			return status;
		}

		/****************************************************************************
	   * CollectBlockTriggered
	   *  this function demonstrates how to collect a single block of data from the
	   *  unit, when a trigger event occurs.
	   ****************************************************************************/
		void CollectETSBlock()
		{
			short triggerVoltage = mv_to_adc(1000, (short)_channelSettings[(int)Imports.Channel.ChannelA].range); // ChannelInfo stores ADC counts

			Imports.TriggerChannelProperties[] sourceDetails = new Imports.TriggerChannelProperties[] {
				new Imports.TriggerChannelProperties(triggerVoltage,
														 256*10,
														 triggerVoltage,
														 256*10,
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

			Console.WriteLine("Collect block triggered...");


			Console.Write("Collects when value rises past {0}", (_scaleVoltages) ?
						  adc_to_mv(sourceDetails[0].ThresholdMajor,
									(int)_channelSettings[(int)Imports.Channel.ChannelA].range)
									: sourceDetails[0].ThresholdMajor);
			Console.WriteLine("{0}", (_scaleVoltages) ? ("mV") : (" ADC Counts"));

			Console.WriteLine("Press a key to start...");
			WaitForKey();

			SetDefaults();

			/* Trigger enabled
			 * Rising edge
			 * Threshold = 1000mV */
			SetTrigger(sourceDetails, 1, conditions, 1, directions, null, 0, 0, 0);

			EtsBlockDataHandler("Ten readings after trigger:", 0, Imports.Mode.ANALOGUE);
		}

		/****************************************************************************
		* Initialise unit' structure with Variant specific defaults
		****************************************************************************/
		void GetDeviceInfo()
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

			System.Text.StringBuilder line = new System.Text.StringBuilder(80);

			if (_handle >= 0)
			{
				for (int i = 0; i < description.Length; i++)
				{
					short requiredSize;
					Imports.GetUnitInfo(_handle, line, 80, out requiredSize, i);

					if (i == 3)
					{
						if (line[1] == '4')  // PicoScope 3400 A/B/D/D MSO device
						{
							_channelCount = QUAD_SCOPE;
						}
						else
						{
							_channelCount = DUAL_SCOPE;
						}

						// Voltage range will depend on the variant - PicoScope 3000 A/B and 3204/5/6 MSO devices have a different lower range.

						if (line[4] == 'A' || line[4] == 'B' || (line.Length == 7 && line[4] == 'M'))
						{
							_firstRange = Imports.Range.Range_50MV;
						}
						else
						{
							_firstRange = Imports.Range.Range_20MV;
						}

						// Check if device has digital channels
						if (line.ToString().EndsWith("MSO"))
						{
							_digitalPorts = 2;
						}
						else
						{
							_digitalPorts = 0;
						}

					}

					Console.WriteLine("{0}: {1}", description[i], line);
				}

				_lastRange = Imports.Range.Range_20V;
			}
		}

		/****************************************************************************
		 * Select input voltage ranges for analog channels
		 ****************************************************************************/
		void SetVoltages()
		{
			bool valid = false;
			short count = 0;

			/* See what ranges are available... */
			for (int i = (int)_firstRange; i <= (int)_lastRange; i++)
			{
				Console.WriteLine("{0} . {1} mV", i, inputRanges[i]);
			}

			do
			{
				/* Ask the user to select a range */
				Console.WriteLine("\nSpecify voltage range ({0}..{1})", _firstRange, _lastRange);
				Console.WriteLine("99 - switches channel off");
				for (int ch = 0; ch < _channelCount; ch++)
				{
					Console.WriteLine("");
					uint range = 8;

					do
					{
						try
						{
							Console.WriteLine("Channel: {0}", (char)('A' + ch));
							range = uint.Parse(Console.ReadLine());
							valid = true;
						}
						catch
						{
							valid = false;
							Console.WriteLine("\nEnter numeric values only");
						}

					} while ((range != 99 && (range < (uint)_firstRange || range > (uint)_lastRange) || !valid));


					if (range != 99)
					{
						_channelSettings[ch].range = (Imports.Range)range;
						Console.WriteLine(" = {0} mV", inputRanges[range]);
						_channelSettings[ch].enabled = true;
						count++;
					}
					else
					{
						Console.WriteLine("Channel Switched off");
						_channelSettings[ch].enabled = false;
						_channelSettings[ch].range = Imports.Range.Range_MAX_RANGE - 1;
					}
				}
				Console.Write(count == 0 ? "\n*** At least 1 channel must be enabled *** \n" : "");
			}
			while (count == 0); // must have at least one channel enabled

			SetDefaults();  // Set defaults now, so that if all but 1 channels get switched off, timebase updates to timebase 0 will work
		}

		/****************************************************************************
		 * SetTimebase
		 * Select timebase and set _oversample to 1
		 *
		 ****************************************************************************/
		void SetTimebase()
		{
			int timeInterval;
			int maxSamples;
			bool valid = false;

			Console.WriteLine("Specify timebase index:");

			do
			{
				try
				{
					_timebase = uint.Parse(Console.ReadLine());
					valid = true;
				}
				catch
				{
					valid = false;
					Console.WriteLine("\nEnter numeric values only");
				}

			} while (!valid);

			while (Imports.GetTimebase(_handle, _timebase, BUFFER_SIZE, out timeInterval, 1, out maxSamples, 0) != 0)
			{
				Console.WriteLine("Selected timebase index {0} could not be used", _timebase);
				_timebase++;
			}

			Console.WriteLine("Timebase index {0} - {1} ns", _timebase, timeInterval);
			_oversample = 1;
		}

		/****************************************************************************
		* DisplaySettings 
		* Displays information about the user configurable settings in this example
		***************************************************************************/
		void DisplaySettings()
		{
			int ch;
			int voltage;

			Console.WriteLine("\n\nReadings will be scaled in {0}", (_scaleVoltages) ? ("mV") : ("ADC counts"));

			for (ch = 0; ch < _channelCount; ch++)
			{
				if (!_channelSettings[ch].enabled)
				{
					Console.WriteLine("Channel {0} Voltage Range = Off", (char)('A' + ch));
				}
				else
				{
					voltage = inputRanges[(int)_channelSettings[ch].range];
					Console.Write("Channel {0} Voltage Range = ", (char)('A' + ch));

					if (voltage < 1000)
					{
						Console.WriteLine("{0}mV", voltage);
					}
					else
					{
						Console.WriteLine("{0}V", voltage / 1000);
					}
				}
			}
			Console.WriteLine();
		}

		/****************************************************************************
		* Run - show menu and call user selected options
		****************************************************************************/
		public void Run()
		{
			// setup devices
			GetDeviceInfo();
			_timebase = 1;

			_channelSettings = new ChannelSettings[MAX_CHANNELS];

			for (int i = 0; i < MAX_CHANNELS; i++)
			{
				_channelSettings[i].enabled = true;
				_channelSettings[i].DCcoupled = Imports.Coupling.DC;
				_channelSettings[i].range = Imports.Range.Range_5V;
			}

			// main loop - read key and call routine
			char ch = ' ';
			while (ch != 'X')
			{
				DisplaySettings();

				Console.WriteLine("");
				Console.WriteLine("V - Set voltages");
				Console.WriteLine("T - Run ETS Block              I - Set timebase");
				Console.WriteLine("A - ADC counts/mV");
				Console.WriteLine("                                 X - Exit");
				Console.WriteLine("Operation:");

				ch = char.ToUpper(Console.ReadKey(true).KeyChar);

				Console.WriteLine("\n");
				switch (ch)
				{

					case 'T':
						CollectETSBlock();
						break;

					case 'V':
						SetVoltages();
						break;

					case 'I':
						SetTimebase();
						break;

					case 'A':
						_scaleVoltages = !_scaleVoltages;
						break;

					case 'X':
						/* Handled by outer loop */
						break;

					default:
						Console.WriteLine("Invalid operation");
						break;
				}
			}
		}


		private PS3000ACSConsole(short handle)
		{
			_handle = handle;
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



		static uint deviceOpen(out short handle)
		{
			uint status = Imports.OpenUnit(out handle, null);

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


			if (status != StatusCodes.PICO_OK)
			{
				Console.WriteLine("Unable to open device");
				Console.WriteLine("Error code : 0x{0}", Convert.ToString(status, 16));
				WaitForKey();
			}
			else
			{
				Console.WriteLine("Handle: {0}", handle);
			}

			return status;
		}


		static void Main2()
		{
			Console.WriteLine("PicoScope 3000 Series (ps3000a) Driver C# Example Program");
			Console.WriteLine("Version 1.1\n\n");
			Console.WriteLine("Enumerating devices...\n");

			short count = 0;
			short serialsLength = 40;
			StringBuilder serials = new StringBuilder(serialsLength);

			uint status = Imports.EnumerateUnits(out count, serials, ref serialsLength);

			if (status != StatusCodes.PICO_OK)
			{
				Console.WriteLine("No devices found.\n");
				Console.WriteLine("Error code : {0}", status);
				Console.WriteLine("Press any key to exit.\n");
				WaitForKey();
				Environment.Exit(0);
			}
			else
			{
				if (count == 1)
				{
					Console.WriteLine("Found {0} device:", count);
				}
				else
				{
					Console.WriteLine("Found {0} devices", count);
				}

				Console.WriteLine("Serial(s) {0}", serials);

			}

			Console.WriteLine("\nOpening the device...");

			short handle = 0;

			if (deviceOpen(out handle) == 0)
			{
				Console.WriteLine("Device opened successfully\n");

				PS3000ACSConsole consoleExample = new PS3000ACSConsole(handle);
				consoleExample.Run();

				Imports.CloseUnit(handle);
			}
		}
	}
}