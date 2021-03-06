﻿using CycleFinder.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static CycleFinder.Helpers.Fourier;

namespace CycleFinder.Models
{
    public class StockChartSimulationGenerator : ChartViewModel
    {
        //SRP: Reason to change is if we needed to change the way generation is done
        public StockChartSimulationGenerator()
        {
			
        }

		//contains series for summed waves
		public List<SineWaveViewModel> SineWavesSeries { get; set; }
		public LongTermTrendViewModel LongTermTrendSeries { get; set; }


        public List<SineWave> SineWaves { get; set; } = new List<SineWave>();
		public LongTermTrend LongTermTrend { get; set; }


		public List<WaveOutput> WaveOutputs
		{
			get
			{
				var wavesList = new List<WaveOutput>();
				wavesList.Add(new WaveOutput(InputSignalSeries, "Composite"));
                if (SineWavesSeries != null)
				    wavesList.AddRange(SineWavesSeries.Select(x => new WaveOutput(x.InputSignalSeries, x.Label)));
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


		public void AddSineWave(double frequency, double amplitude, double phaseShift, string label="", string colour = "black")
		{
            label = label == String.Empty ? frequency.ToString() : label;
			SineWaves.Add(new SineWave(frequency, amplitude, phaseShift, label, colour));
		}

		//for summed waves
		public void SetInputSignalSeriesSummed(int numberOfSamples, double sampleFrequency)
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
		private double GetInputSignalAmplitudeForSample(int sampleNo, double sampleFequency, int numberOfSamples, SineWave wave)
		{
			var sampleTime = 1 / sampleFequency;
			var amplitude = GetInputSignalAmplitudeForTime(sampleNo * sampleTime, wave);

			return amplitude;
		}
		private double GetTrendAmplitudeForSample(int sampleNo, double sampleFequency, int numberOfSamples)
		{
			var sampleTime = 1 / sampleFequency;
			var amplitude = GetTrendAmplitudeForTime(sampleNo * sampleTime);

			return amplitude;
		}

		//per wave
		public void SetInputSignalSeriesPerWave(int numberOfSamples, double sampleFrequency)
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

				SineWavesSeries.Add(new SineWaveViewModel(sampleList, wave.Label, wave.Colour));
			}
			//long term trend
			sampleList = new List<double>();
			for (var i = 0; i < numberOfSamples; i++)
			{
				sampleList.Add(GetTrendAmplitudeForSample(i, sampleFrequency, numberOfSamples));
			}
			LongTermTrendSeries = new LongTermTrendViewModel(sampleList, LongTermTrend.Colour);


		}


        public override string DFT1Formatted => DFT1.GoogleChartDataFormat(SampleRateforSummedSeries / DFT1.Count);
        public override string DFT2Formatted => DFT2.GoogleChartDataFormat(SampleRateforSummedSeries / DFT2.Count);
        public override string FFTFormatted => FFT.GoogleChartDataFormat(SampleRateforSummedSeries / FFT.Count);

    }
}
