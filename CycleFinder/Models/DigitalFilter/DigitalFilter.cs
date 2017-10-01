using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace CycleFinder.Models.DigitalFilters
{
    public abstract class DigitalFilter //dont want to actually instantiate this class (ie its abstract)
    {
        protected DigitalFilter()
        {
        }

        protected List<double> StockInputData { get; set; }  //derived class can access
        public List<double> Kernel { get; set; } = new List<double>();
		protected int TimeSpacing { get; set; }
		protected int NumberOfWeights { get; set; }
        public string Name { get; set; }

        public List<double> StockPricesFiltered { get; private set; }

        public List<double> ConvertedStockInputData //spaces the data according to TimeSpacing parameter
        {
            get
            {
				var convertedStockInputData = new List<double>();
				//do conversion for time spacing....
				for (var i = 0; i < StockInputData.Count; i++)
				{
					if (i % TimeSpacing == 0)
						convertedStockInputData.Add(StockInputData[i]);
				}
				return convertedStockInputData;               
            }
        
        }


		protected void ApplyNumericalFilterToStockPrices()
		{
            var numericAnalysis = new double[ConvertedStockInputData.Count];
            //create StockPricesFiltered list which lines up with ConvertedStockInputData
            for (var i = 0; i < (ConvertedStockInputData.Count - NumberOfWeights); i++)  //cant go right to the end
            {
                var centralPriceDatum = i + ((NumberOfWeights + 1) / 2);
                double sum = 0;
                for (var j = 0; j < NumberOfWeights; j++)
                {
                    sum += (ConvertedStockInputData[i + j] * Kernel[j]);
                }
                numericAnalysis[centralPriceDatum] = sum;
            }
            StockPricesFiltered = numericAnalysis.ToList();
		}


        //filter kernel for unit gain at DC
        public void NormaliseKernel()
        {
            var sum = Kernel.Aggregate((a, b) => a + b);
            Kernel = Kernel.Select(x => x / sum).ToList();

        }

        public void InvertKernel()
        {
            Kernel = Kernel.Select(x => -x).ToList();
            int index = Kernel.Count / 2;
            Kernel[index] += 1;
                              
        }

		public  List<double> CompoundWithAnotherKernel(List<double> otherList)
		{
            var firstList = Kernel;
            var secondList = otherList;

			var result = Enumerable.Range(0, Math.Max(firstList.Count, secondList.Count)).Select(x => firstList.ElementAtOrDefault(x) + secondList.ElementAtOrDefault(x));
			return result.ToList();
		}


    }

    public enum DigitalFilterType
    {
        BandPass,
        HighPass,
        LowPass,
        MovingAverage,
        WindowedSinc
    }

    public enum WindowType
    {
        Hamming,
        Blackman,
        None
    }
}
