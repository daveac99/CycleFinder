using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using CycleFinder.Extensions;
using static CycleFinder.Models.Fourier;

namespace CycleFinder.Models
{
    public class StockChartSimulationViewModel : StockChartSimulationGenerator
    {
        public StockChartSimulationViewModel()
        {
           
        }

        public List<DigitalFilterViewModel> Filters { get; set; } = new List<DigitalFilterViewModel>();

		public List<WaveOutput> StockPlusFilters
		{
			get
			{
				var wavesList = new List<WaveOutput>();
				wavesList.Add(new WaveOutput(Filters[0].ConvertedStockInputData, "Stock")); //use time space converted data
                wavesList.AddRange(Filters.Select(x => new WaveOutput(x.DataSeries, $"{x.FilterType}: {x.Parameters}")));
				return wavesList;
			}
		}

        public string StockPlusFiltersFormatted => StockPlusFilters.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();

		public string WaveOutputsFormatted => WaveOutputs.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();


        public string DFT1Formatted => DFT1.GoogleChartDataFormat(SampleRateforSummedSeries/DFT1.Count);
        public string DFT2Formatted => DFT2.GoogleChartDataFormat();
        public string FFTFormatted => FFT.GoogleChartDataFormat(SampleRateforSummedSeries / DFT1.Count);
        public string InputSignalSeriesFormatted => InputSignalSeries.GoogleChartDataFormat();

        public void AddFilter(DigitalFilterType filterType, int timeSpacing, int numberofWeights)
        {
            AddFilter(filterType, timeSpacing, numberofWeights, 0,0,0,0);
        }

		public void AddFilter(DigitalFilterType filterType, int timeSpacing, int numberOfWeights, double frequencyLowEndCutOff, double frequencyLowEndRollOff, double frequencyHighEndRollOff, double frequencyHighEndCutOff)
		{
		
			var filter = Factory.GetDigitalFilter(filterType, InputSignalSeries, timeSpacing, numberOfWeights, frequencyLowEndCutOff, frequencyLowEndRollOff, frequencyHighEndRollOff, frequencyHighEndCutOff);

            Filters.Add(new DigitalFilterViewModel(filterType, $"Weights: {numberOfWeights}", filter));
		}
	}
}
