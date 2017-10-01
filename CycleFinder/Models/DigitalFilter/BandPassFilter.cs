using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using CycleFinder.Models.DigitalFilters;

namespace CycleFinder.Models.DigitalFilters
{
    public class BandPassFilter : DigitalFilter
    {
        //used by factory method
        public BandPassFilter()
        {
            
        }

        //this is the lanczos version
        public BandPassFilter(List<double> stockInputData, int timeSpacing, int numberOfWeights, double frequencyLowEndCutOff, double frequencyLowEndRollOff, double frequencyHighEndRollOff, double frequencyHighEndCutOff)
        {
            Wn = (104 * PI) / timeSpacing;
            StockInputData = stockInputData;
            TimeSpacing = timeSpacing; // in weeks
            NumberOfWeights = numberOfWeights; //must be an odd number
            W1 = frequencyLowEndCutOff * 2 * PI;
            W2 = frequencyLowEndRollOff * 2 * PI;
            W3 = frequencyHighEndRollOff * 2 * PI;
            W4 = frequencyHighEndCutOff * 2 * PI;

            GetNumericalFilter();
            ApplyNumericalFilterToStockPrices();
        }
        protected double AngularFrequencyN { get; set; }

		#region static factory methods

		public static BandPassFilter GetBandPassFilter(double cutoffFrequency, int filterLength)
		{
			var bandPassFilter = new BandPassFilter();
			double kernelValue;
			for (int i = 0; i < filterLength; i++)
			{
				if ((i - filterLength / 2) == 0)
					kernelValue = 2 * Math.PI * cutoffFrequency;
				else
					kernelValue = Math.Sin(2 * Math.PI * cutoffFrequency * (i - filterLength / 2)) / (i - filterLength / 2);
				kernelValue *= (0.54 - 0.46 * Math.Cos(2 * Math.PI * i / filterLength));
				bandPassFilter.Kernel.Add(kernelValue);
			}
			//normalise the low-pass filter kernel for unit gain at DC
			var sum = bandPassFilter.Kernel.Sum();
			bandPassFilter.Kernel = bandPassFilter.Kernel.Select(x => x / sum).ToList();
			bandPassFilter.Name = $"Low Pass: fc={cutoffFrequency}, M={filterLength}";
			return bandPassFilter;

		}

		#endregion


		protected double Wn { get; set; }
        protected double W1 { get; set; }
        protected double W2 { get; set; }
        protected double W3 { get; set; }
        protected double W4 { get; set; }

        private double L1 => W1 / Wn;
        private double L2 => W2 / Wn;
        private double L3 => W3 / Wn;
        private double L4 => W4 / Wn;
        private double L5 => L1 - L2;
        private double L6 => L4 - L3;

        private int CentreWeightNo => (NumberOfWeights - 1) / 2;

        protected void GetNumericalFilter()
        {
            var weights = new List<double>();
            //first half of weights
            for (var i = 0; i < CentreWeightNo; i++)
            {
                var mult = i + 1;
                var A = (Cos(2 * PI * L3 * mult) - Cos(2 * PI * L4 * mult)) / (2 * PI * PI * L6 * mult * mult);
                var B = (Cos(2 * PI * L2 * mult) - Cos(2 * PI * L1 * mult)) / (2 * PI * PI * L5 * mult * mult);
                var weight = A - B;
                weights.Add(weight);
            }
            //middle weight
            var centralWeight = (L3 + L4) - (L1 + L2);
            weights.Add(centralWeight);

            //last half of weights - mirror the first half
            for (var i = CentreWeightNo - 1; i >= 0; i--)
            {
                weights.Add(weights[i]);
            }
            var avge = weights.Average();
            weights = weights.Select(x => x - avge).ToList();

            //alternatively
            //var weights2 = weights;
            //double sum2 = 0;
            //foreach (var weight in weights)
            //{
            //    sum2 += weight;
            //}
            //var avge2 = sum2 / NumberOfWeights;
            //for (var i = 0; i < NumberOfWeights; i++)
            //{
            //    weights2[i] -= avge2;
            //}

            Kernel = weights;
        }




    }
}
