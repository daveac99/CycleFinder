using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static CycleFinder.Models.Fourier;

namespace CycleFinder.Models
{
    public class StockChartSimulationGenerator
    {
        public StockChartSimulationGenerator()
        {
			
        }

		//contains series for summed waves
		internal List<double> InputSignalSeries { get; set; }
		private List<SineWaveViewModel> SineWavesSeries { get; set; }
		private LongTermTrendViewModel LongTermTrendSeries { get; set; }


        public List<SineWave> SineWaves { get; set; } = new List<SineWave>();
		public LongTermTrend LongTermTrend { get; set; }

		internal List<double> DFT1 => DFT1(InputSignalSeries).Select(x => x.Magnitude).ToList();
		internal List<double> DFT2 => DFT2(InputSignalSeries).Select(x => x.Magnitude).ToList();
		internal List<double> FFT
		{
			get
			{
				Complex[] input = InputSignalSeries.Select(x => new Complex(x, 0)).ToArray();
				FFT(input);
				return input.Select(x => x.Magnitude).ToList();
			}
		}






		public List<WaveOutput> WaveOutputs
		{
			get
			{
				var wavesList = new List<WaveOutput>();
				wavesList.Add(new WaveOutput(InputSignalSeries, "Composite"));
                if (SineWavesSeries != null)
				    wavesList.AddRange(SineWavesSeries.Select(x => new WaveOutput(x.InputSignalSeries, "Sine Wave")));
                if (LongTermTrendSeries?.InputSignalSeries != null)
				    wavesList.Add(new WaveOutput(LongTermTrendSeries.InputSignalSeries, "Long Term Trend"));
				return wavesList;
			}
		}
		public double SampleRateforSummedSeries { get; set; }

		//summed waves
		//public double GetInputSignalAmplitudeForTime(double time)
		//{
		//    double amplitudeSum = 0;
		//    foreach (var sinewave in SineWaves)
		//    {
		//        amplitudeSum += sinewave.GetAmplitudeForTime(time);
		//    }

		//    return amplitudeSum;
		//}

		//per wave
		public double GetInputSignalAmplitudeForTime(double time, SineWave wave)
		{
			double amplitude = 0;
			amplitude = wave.GetAmplitudeForTime(time);

			return amplitude;
		}

		public double GetTrendAmplitudeForTime(double time)
		{
			double amplitude = 0;
			amplitude = LongTermTrend.GetAmplitudeForTime(time);

			return amplitude;
		}

		//summed waves
		//public double GetInputSignalAmplitudeForSample(int sampleNo, double sampleFequency, int numberOfSamples)
		//{
		//          var sampleTime = 1 / sampleFequency;
		//          var amplitude = GetInputSignalAmplitudeForTime(sampleNo * sampleTime);

		//  return amplitude;
		//}

		//per wave
		public double GetInputSignalAmplitudeForSample(int sampleNo, double sampleFequency, int numberOfSamples, SineWave wave)
		{
			var sampleTime = 1 / sampleFequency;
			var amplitude = GetInputSignalAmplitudeForTime(sampleNo * sampleTime, wave);

			return amplitude;
		}

		public double GetTrendAmplitudeForSample(int sampleNo, double sampleFequency, int numberOfSamples)
		{
			var sampleTime = 1 / sampleFequency;
			var amplitude = GetTrendAmplitudeForTime(sampleNo * sampleTime);

			return amplitude;
		}

		public void AddSineWave(double frequency, double amplitude, double phaseShift, string colour = "black")
		{
			SineWaves.Add(new SineWave(frequency, amplitude, phaseShift, colour));
		}

		//for summed waves
		public void GetInputSignalSeriesSummed(int numberOfSamples, double sampleFrequency)
		{
			SampleRateforSummedSeries = sampleFrequency;
			var sampleList = new List<double>();
			double amplitude = 0;
			for (var i = 0; i < numberOfSamples; i++)
			{
				amplitude = 0;
				//indivdual sine waves
				foreach (var wave in SineWaves)
				{
					amplitude += GetInputSignalAmplitudeForSample(i, sampleFrequency, numberOfSamples, wave);
				}
				//long term trend
				amplitude += GetTrendAmplitudeForSample(i, sampleFrequency, numberOfSamples);

				sampleList.Add(amplitude);
			}


			InputSignalSeries = sampleList;
		}

		//per wave
		public void GetInputSignalSeriesPerWave(int numberOfSamples, double sampleFrequency)
		{
			SineWavesSeries = new List<SineWaveViewModel>();
			var sampleList = new List<double>();
			foreach (var wave in SineWaves)
			{
				sampleList = new List<double>();
				for (var i = 0; i < numberOfSamples; i++)
				{
					sampleList.Add(GetInputSignalAmplitudeForSample(i, sampleFrequency, numberOfSamples, wave));
				}

				SineWavesSeries.Add(new SineWaveViewModel(sampleList, wave.Colour));
			}
			//long term trend
			sampleList = new List<double>();
			for (var i = 0; i < numberOfSamples; i++)
			{
				sampleList.Add(GetTrendAmplitudeForSample(i, sampleFrequency, numberOfSamples));
			}
			LongTermTrendSeries = new LongTermTrendViewModel(sampleList, LongTermTrend.Colour);


		}




	}
}
