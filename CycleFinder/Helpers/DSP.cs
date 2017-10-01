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
            var offset = (kernel.Count + 1) / 2;
            for (int i = kernel.Count-1; i < inputSignal.Count-1; i++)
            {
                for (int j = 0; j < kernel.Count-1; j++)
                {
                    output[i - offset] += inputSignal[i - j] * kernel[j];
                }
            }
            return output.ToList();

        }

        public static List<double> Compound(List<double> firstList, List<double> secondList)
        {
            //var result =
              // from i in
            		//Enumerable.Range(0, Math.Max(firstList.Count, secondList.Count))
               //select firstList.ElementAtOrDefault(i) + secondList.ElementAtOrDefault(i);


            var result = Enumerable.Range(0, Math.Max(firstList.Count, secondList.Count)).Select(x => firstList.ElementAtOrDefault(x) + secondList.ElementAtOrDefault(x));
            return result.ToList();
        }

        public static List<double> Compound(List<List<double>> lists)
        {
            var result = lists[0];
            for (int i = 1; i < lists.Count; i++)
            {
                result = Compound(result, lists[i]);
            }
            return result;


        }
    }
}
