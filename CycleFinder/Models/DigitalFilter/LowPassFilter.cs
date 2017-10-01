using System;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Models.DigitalFilters
{
    public class LowPassFilter : DigitalFilter
    {
        public LowPassFilter()
        {
        }

        #region static factory methods

        public static LowPassFilter GetLowPassFilter(double cutoffFrequency, int filterLength)
        {
            var lowPassFilter = new LowPassFilter();
           
            //old code
            //double kernelValue;
            //for (int i = 0; i < filterLength; i++)
            //{
            //    if ((i - filterLength / 2) == 0)
            //        kernelValue = 2 * Math.PI * cutoffFrequency;
            //    else
            //        kernelValue = Math.Sin(2 * Math.PI * cutoffFrequency * (i - filterLength / 2)) / (i - filterLength / 2);
            //    kernelValue *= (0.54 - 0.46 * Math.Cos(2 * Math.PI * i / filterLength));
            //    lowPassFilter.Kernel.Add(kernelValue);                                 
            //}
            ////normalise the low-pass filter kernel for unit gain at DC
            //var sum = lowPassFilter.Kernel.Sum();
            //lowPassFilter.Kernel = lowPassFilter.Kernel.Select(x => x / sum).ToList();

            //new code
            var windowedSinc = new WindowedSinc(cutoffFrequency, filterLength, WindowType.Blackman);
            windowedSinc.NormaliseKernel();
            lowPassFilter.Kernel = windowedSinc.Kernel;


            lowPassFilter.Name = $"Low Pass: fc={cutoffFrequency}, M={filterLength}";
            return lowPassFilter;

        }

        #endregion
    }


}
