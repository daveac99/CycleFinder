using System;
using System.Collections.Generic;

namespace CycleFinder.Models
{
    public class WaveOutput
    {
        public WaveOutput(List<double> outputSeries, string name)
        {
            OutputSeries = outputSeries;
            Name = name;
        }

        public string Name
        {
            get;
            private set;
        }

        public List<double> OutputSeries
        {
            get;
            private set;
        }
    }
}
