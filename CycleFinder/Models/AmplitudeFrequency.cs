using System;
namespace CycleFinder.Models
{
    public class AmplitudeFrequency
    {
        public AmplitudeFrequency()
        {
        }

        public double CosineAmplitude { get; set; }
        public double SineAmplitude { get; set; }
        public double Frequency { get; set; }
        public double AngularFrequency { get; set; }
        public double CompositeAmplitude { get { return Math.Sqrt((CosineAmplitude * CosineAmplitude) + (SineAmplitude * SineAmplitude)); } }
        public double Period { get { return 2 * Math.PI / AngularFrequency; }   }
    }
}
