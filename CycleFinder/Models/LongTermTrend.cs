using System;
namespace CycleFinder.Models
{
    public class LongTermTrend
    {
        public LongTermTrend(double startPrice, double rate, double frequency, double amplitude, double phaseShift) //ie price increase per year
        {
            StartPrice = startPrice;
            Rate = rate;
			Period = 1 / frequency; //seconds
			Frequency = frequency; //hertz
			PeakAmplitude = amplitude;
			PhaseShift = phaseShift;
			
        }

        public double StartPrice
        {
            get;
            set;
        }
        public double EndPrice
        {
            get;
            set;
        }
        public double Rate {get;}

		public string Colour
		{
			get;
			set;
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

		public double GetAmplitudeForTime(double time)
		{
			var trendLine =  StartPrice + (Rate * time);
            var longTermCycle = PeakAmplitude * (Math.Sin(2 * Math.PI * Frequency * time + PhaseShift));
            return trendLine + longTermCycle;
		}
    }
}
