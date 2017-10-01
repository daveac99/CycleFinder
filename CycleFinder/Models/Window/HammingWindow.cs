using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace CycleFinder.Models.Window
{
    public class HammingWindow : WindowBase
    {
        public HammingWindow(List<double> input)
        {
			{
				//Result = Enumerable.Range(0, input.Count)
           //                        .Select(x => input.ElementAtOrDefault(x) * (0.54 - (0.46 * Cos(2 * PI * x / input.Count))))
								   //.ToList();

				for (var i = 0; i < input.Count; i++)
				{
                    input[i] *= (0.54 - (0.46 * Cos(2 * PI * i / input.Count)));
				}
				Result = input;
			}
        }
    }
}
