using System;
using System.Collections.Generic;

namespace CycleFinder.Models
{
    public class Wave
    {
        public Wave(double period, double amplitude, string colour = "black")
        {
            Period = period; //years
            Amplitude = amplitude;
            Colour = colour;
        }



        public double Frequency 
        {
            get;
            set;
        }

        public double Period 
        {
            get;
            set;
        }
        public double Amplitude
        {
            get;
            set;
        }
        public string Colour
        {
            get;
            set;
        }
        public double TimeSpan
        {
            get;
            set;
        }
    }
}
