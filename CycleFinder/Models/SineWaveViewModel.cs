using System;
using System.Collections.Generic;

namespace CycleFinder.Models
{
    public class SineWaveViewModel
    {
        public SineWaveViewModel(List<double> inputSignalSeries, string label, string colour = "black")
        {
            InputSignalSeries = inputSignalSeries;
            Colour = colour;
            Label = label;
        }

        public string Colour {get; set;}
        public string Label { get; set; }

		//contains series for wave
		public List<double> InputSignalSeries { get; private set; }
    }
}
