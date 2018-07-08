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



        public string StockPlusFiltersFormatted => StockPlusFilters.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();
      //  public string StockPlusKernelFormatted => StockPlusKernel.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();
       // public string KernelOnlyFormatted => KernelOnly.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();

      //  public string KernelOnlyFrequencyResponseFormatted => KernelOnlyFrequencyResponse.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();

		public string WaveOutputsFormatted => WaveOutputs.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();


        public string DFT1Formatted => DFT1.GoogleChartDataFormat(SampleRateforSummedSeries / DFT1.Count);
        public string DFT2Formatted => DFT2.GoogleChartDataFormat();
        public string FFTFormatted => FFT.GoogleChartDataFormat(SampleRateforSummedSeries / DFT1.Count);






    }
}
