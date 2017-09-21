using System;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Helpers
{
    public static class DSP
    {
       public static List<double> Convolve(List<double> inputSignal, List<double> kernel)
        {
            var output = new double[inputSignal.Count];
            for (int i = kernel.Count-1; i < inputSignal.Count-1; i++)
            {
                for (int j = 0; j < kernel.Count-1; j++)
                {
                    output[i] += inputSignal[i - j] * kernel[j];
                }
            }
            return output.ToList();

        }
    }
}
