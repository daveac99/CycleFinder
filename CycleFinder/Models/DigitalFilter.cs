using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace CycleFinder.Models
{
    public abstract class DigitalFilter //dont want to actually instantiate this class (ie its abstract)
    {
        public DigitalFilter()
        {
        }

        protected List<double> StockInputData { get; set; }  //derived class can access
        protected List<double> NumericAnalysis { get; set; }
		protected int TimeSpacing { get; set; }
		protected int NumberOfWeights { get; set; }

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
                    sum += (ConvertedStockInputData[i + j] * NumericAnalysis[j]);
                }
                numericAnalysis[centralPriceDatum] = sum;
            }
            StockPricesFiltered = numericAnalysis.ToList();
		}
    }

    public enum DigitalFilterType
    {
        BandPass,
        HighPass,
        LowPass,
        MovingAverage
    }
}
