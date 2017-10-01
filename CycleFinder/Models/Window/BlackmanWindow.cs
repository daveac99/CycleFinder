using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace CycleFinder.Models.Window
{
    public class BlackmanWindow : WindowBase
    {
        public BlackmanWindow(List<double> input)
        {
            // Result = Enumerable.Range(0, input.Count)
            //                    .Select(x => input.ElementAtOrDefault(x) * (0.42 - (0.5 * Cos(2 * PI * x / input.Count))) + (0.08 * Cos(4 * PI * x / input.Count)))
            //                    .ToList();
            for (var i = 0; i < input.Count; i++)
            {
                input[i] *= (0.42 - (0.5 * Cos(2 * PI * i / input.Count)) + (0.08 * Cos(4 * PI * i / input.Count)));
            }
            Result = input;
        }


    }
}
