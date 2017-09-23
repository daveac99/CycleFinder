using System;
namespace CycleFinder.Models
{
    public class SineWave
    {

		//use this for textbook examples using hertz
		public SineWave(double frequency, double amplitude, double phaseShift, string label, string colour = "black")
		{
			Period = 1 / frequency; //seconds
			Frequency = frequency; //hertz
			PeakAmplitude = amplitude;
			PhaseShift = phaseShift;
            Label = label;
			Colour = colour;

		}

		public double Frequency
		{
			get;
			set;
		}

        public double PhaseShift //radians
        {
            get;
            set;
        }

		public double Period
		{
			get;
			set;
		}
		public double PeakAmplitude
		{
			get;
			set;
		}
		public string Colour
		{
			get;
			set;
		}

        public string Label { get; set; }

        public double GetAmplitudeForTime(double time)
        {
            return PeakAmplitude * (Math.Sin(2 * Math.PI * Frequency * time + PhaseShift));
        }
    }
}
