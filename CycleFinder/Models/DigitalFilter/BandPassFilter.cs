using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

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

        public void someMethod()
        {
            var l = new BandPassFilter();
            l.InvertKernel();
        }


		#region static factory methods

        //this is the version from the DSP book
		public static BandPassFilter GetBandPassFilter(double cutoffFrequencyLower, double cutoffFrequencyUpper, int filterLength, WindowType windowType)
		{
			var lowPassFilter = new LowPassFilter();

            //first low pass filter for lower cutoff
			var windowedSinc = new WindowedSinc(cutoffFrequencyLower, filterLength, windowType);
			windowedSinc.NormaliseKernel();
			lowPassFilter.Kernel = windowedSinc.Kernel;

            var highPassFilter = new LowPassFilter();
            windowedSinc = new WindowedSinc(cutoffFrequencyUpper, filterLength, windowType);
            windowedSinc.NormaliseKernel();
            highPassFilter.Kernel = windowedSinc.Kernel;
            highPassFilter.InvertKernel();

            //add low and high pass to make a band reject filter
            var bandPassFilter = new BandPassFilter();
            
            bandPassFilter.Kernel = bandPassFilter.Compound(lowPassFilter.Kernel, highPassFilter.Kernel);

            //change band reject into band pass using spectral inversion
            bandPassFilter.InvertKernel();

            bandPassFilter.Name = $"Band Pass: fcl={cutoffFrequencyLower} fcu={cutoffFrequencyUpper}, M={filterLength}";
            //bandPassFilter.Kernel = highPassFilter.Kernel; //TODO remove
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
