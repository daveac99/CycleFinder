using System;
using System.Collections.Generic;

namespace CycleFinder.Models
{
    public class SineWaveViewModel
    {
        public SineWaveViewModel(List<double> inputSignalSeries, string colour = "black")
        {
            InputSignalSeries = inputSignalSeries;
            Colour = colour;
        }

        public string Colour {get; set;}

		//contains series for wave
		public List<double> InputSignalSeries { get; private set; }
    }
}
