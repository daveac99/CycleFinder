using System;
using System.Collections.Generic;

namespace CycleFinder.Models
{
    public class LongTermTrendViewModel
    {
        public LongTermTrendViewModel(List<double> inputSignalSeries, string colour="black")
        {
            InputSignalSeries = inputSignalSeries;
            Colour = colour;
        }

        //contains series for trend
        public List<double> InputSignalSeries { get; private set; }

        public string Colour {get; set;}
    }
}
