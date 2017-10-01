using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using CycleFinder.Extensions;
using static CycleFinder.Helpers.Fourier;
using CycleFinder.Models;
using CycleFinder.Helpers;
using CycleFinder.Models.DigitalFilters;

namespace CycleFinder.Models
{
    public class StockChartSimulationViewModel : StockChartSimulationGenerator
    {
        public StockChartSimulationViewModel()
        {

        }
        public DigitalFilter DigitalFilter { get; set; }
        public List<Double> InputSignalConvoluted => DSP.Convolve(InputSignalSeries, DigitalFilter.Kernel);
        public string InputSignalConvolutedFormatted => InputSignalConvoluted.GoogleChartDataFormat();



        public List<DigitalFilterViewModel> Filters { get; set; } = new List<DigitalFilterViewModel>();

		public List<WaveOutput> StockPlusFilters
		{
			get
			{
				var wavesList = new List<WaveOutput>();

                if (Filters.Count == 0)
                          return wavesList;
				wavesList.Add(new WaveOutput(Filters[0].ConvertedStockInputData, "Stock")); //use time space converted data
                wavesList.AddRange(Filters.Select(x => new WaveOutput(x.DataSeries, $"{x.FilterType}: {x.Parameters}")));
				return wavesList;
			}
		}



		//public List<WaveOutput> StockPlusKernel
		//{
		//	get
		//	{
		//		var wavesList = new List<WaveOutput>();

		//		wavesList.Add(new WaveOutput(InputSignalSeries, "Stock")); 
  //              wavesList.Add(new WaveOutput(Kernel.KernelValues, $"{Kernel.Name}"));
		//		return wavesList;
		//	}
		//}



		//public List<WaveOutput> KernelOnly
		//{
		//	get
		//	{
		//		var wavesList = new List<WaveOutput>();

		//		wavesList.Add(new WaveOutput(Kernel.KernelValues, $"{Kernel.Name}"));
		//		return wavesList;
		//	}
		//}

		//public List<WaveOutput> KernelOnlyFrequencyResponse
		//{
		//	get
		//	{
		//		var wavesList = new List<WaveOutput>();

  //              wavesList.Add(new WaveOutput(DFT1(Kernel.KernelValues).Select(x => x.Magnitude).ToList(), $"{Kernel.Name}"));
		//		return wavesList;
		//	}
		//}

        public List<WaveOutput> FilterOnly
		{
			get
			{
				var wavesList = new List<WaveOutput>();

				wavesList.AddRange(Filters.Select(x => new WaveOutput(x.NumericAnalysis, $"{x.FilterType}: {x.Parameters}")));
				return wavesList;
			}
		}

		public List<WaveOutput> FilterOnlyFrequencyResponse
		{
			get
			{
				var wavesList = new List<WaveOutput>();

                wavesList.AddRange(Filters.Select(x => new WaveOutput(DFT1(x.NumericAnalysis).Select(y => y.Magnitude).ToList(), $"{x.FilterType}: {x.Parameters}")));
				return wavesList;
			}
		}

        public string StockPlusFiltersFormatted => StockPlusFilters.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();
      //  public string StockPlusKernelFormatted => StockPlusKernel.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();
       // public string KernelOnlyFormatted => KernelOnly.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();
        public string FilterOnlyFormatted => FilterOnly.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();
        public string FilterOnlyFrequencyResponseFormatted => FilterOnlyFrequencyResponse.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();
      //  public string KernelOnlyFrequencyResponseFormatted => KernelOnlyFrequencyResponse.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();

		public string WaveOutputsFormatted => WaveOutputs.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();


        public string DFT1Formatted => DFT1.GoogleChartDataFormat(SampleRateforSummedSeries/DFT1.Count);  
        public string DFT2Formatted => DFT2.GoogleChartDataFormat();
        public string FFTFormatted => FFT.GoogleChartDataFormat(SampleRateforSummedSeries / DFT1.Count);
        public string InputSignalSeriesFormatted => InputSignalSeries.GoogleChartDataFormat();

        //input fields
        [Display(Name = "t")]
        public int TimeSpacing { get; set; }
        [Display(Name="# Wts")]
        public int NumberOfWeights { get; set; }
        [Display(Name="f Lo CutO")]
        public double FrequencyLowEndCutOff { get; set; }
        [Display(Name = "f Lo RollO")]
        public double FrequencyLowEndRollOff { get; set; }
        [Display(Name = "f Hi RollO")]
        public double FrequencyHighEndRollOff { get; set; }
        [Display(Name = "f Hi CutO")]
		public double FrequencyHighEndCutOff { get; set; }

        public void AddFilter(DigitalFilterType filterType) //use already defined properties
        {
            AddFilter(filterType, TimeSpacing, NumberOfWeights, FrequencyLowEndCutOff, FrequencyLowEndRollOff, FrequencyLowEndRollOff, FrequencyHighEndCutOff);
        }

        public void AddFilter(DigitalFilterType filterType, int timeSpacing, int numberofWeights)
        {
            AddFilter(filterType, timeSpacing, numberofWeights, 0,0,0,0);
        }

		public void AddFilter(DigitalFilterType filterType, int timeSpacing, int numberOfWeights, double frequencyLowEndCutOff, double frequencyLowEndRollOff, double frequencyHighEndRollOff, double frequencyHighEndCutOff)
		{
		
			var filter = Factory.GetDigitalFilter(filterType, InputSignalSeries, timeSpacing, numberOfWeights, frequencyLowEndCutOff, frequencyLowEndRollOff, frequencyHighEndRollOff, frequencyHighEndCutOff);

            Filters.Add(new DigitalFilterViewModel(filterType, $"Weights: {numberOfWeights}", filter));
            //set the view model properties
            TimeSpacing = timeSpacing;
            NumberOfWeights = numberOfWeights;
            FrequencyLowEndCutOff = frequencyLowEndCutOff;
            FrequencyLowEndRollOff = frequencyLowEndRollOff;
            FrequencyHighEndRollOff = frequencyHighEndRollOff;
            FrequencyHighEndCutOff = frequencyHighEndCutOff;
		}

        public void SetFilter(DigitalFilterType filterType, double cutoffFrequency, int sampleRate, int filterLength)
        {
            SetFilter(filterType, cutoffFrequency / sampleRate, filterLength);
        }

		public void SetFilter(DigitalFilterType filterType, double cutoffFrequencyLower, double cutoffFrequencyUpper, int sampleRate, int filterLength)
		{
			SetFilter(filterType, cutoffFrequencyLower / sampleRate, cutoffFrequencyUpper/sampleRate, filterLength);
		}

        //sets DigitalFilter property for lower/upper cutoff
        public void SetFilter(DigitalFilterType filterType, double cutoffFrequencyRate, int filterLength)
        {
            var filter = Factory.GetDigitalFilter(filterType, cutoffFrequencyRate, filterLength);
            DigitalFilter = filter;
        }

		//sets DigitalFilter property for band
		public void SetFilter(DigitalFilterType filterType, double cutoffFrequencyRateLower, double cutoffFrequencyRateUpper, int filterLength)
		{
			var filter = Factory.GetDigitalFilter(filterType, cutoffFrequencyRateLower, cutoffFrequencyRateUpper, filterLength);
			DigitalFilter = filter;
		}

        public void RemoveFilters(DigitalFilterType filterType)
        {
            Filters.RemoveAll(x => x.FilterType == filterType);
        }
	}
}
