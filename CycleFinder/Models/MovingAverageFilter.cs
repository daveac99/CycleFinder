using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace CycleFinder.Models
{
    public class MovingAverageFilter : DigitalFilter
    {
        public MovingAverageFilter(List<double> stockInputData, int timeSpacing, int numberOfWeights) 
        {
           
            StockInputData = stockInputData;
            TimeSpacing = timeSpacing; // in weeks
            NumberOfWeights = numberOfWeights;

            GetNumericalFilter();
            ApplyNumericalFilterToStockPrices();
        }

        protected double AngularFrequencyN { get; set; }
  

        private int CentreWeightNo => (NumberOfWeights - 1) / 2;

        protected void GetNumericalFilter()
        {
            var weights = new List<double>();
            //all weights are equal
            for (var i = 0; i < NumberOfWeights; i++)
            {
                var weight = 1 / (double)NumberOfWeights;
                weights.Add(weight);
            }

            Kernel = weights;
        }




    }
}
