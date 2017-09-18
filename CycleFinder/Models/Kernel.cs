using System;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Models
{
    public class Kernel
    {
        public Kernel()
        {
        }

        public List<double> KernelValues { get; set; }
        public string Name { get; set; }

        #region static factory methods

        public static Kernel GetLowPassKernel(double cutoffFrequency, int filterLength)
        {
            var kernel = new Kernel();
            double kernelValue;
            var kernelValues = new List<double>();
            for (int i = 0; i < filterLength; i++)
            {
                if ((i - filterLength / 2) == 0)
                    kernelValue = 2 * Math.PI * cutoffFrequency;
                else
                    kernelValue = Math.Sin(2 * Math.PI * cutoffFrequency * (i - filterLength / 2)) / (i - filterLength / 2);
                kernelValue *= (0.54 - 0.46 * Math.Cos(2 * Math.PI * i / filterLength));
                kernel.KernelValues.Add(kernelValue);                                 
            }
            //normalise the low-pass filter kernel for unit gain at DC
            var sum = kernelValues.Sum();
            kernel.KernelValues = kernel.KernelValues.Select(x => x / sum).ToList();
            kernel.Name = $"Low Pass: fc={cutoffFrequency}, M={filterLength}";
            return kernel;

        }

        #endregion
    }
}
