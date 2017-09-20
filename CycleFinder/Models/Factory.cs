using System;
using System.Collections.Generic;

namespace CycleFinder.Models
{
    public class Factory
    {
        public Factory()
        {
        }

        public static DigitalFilter GetDigitalFilter(DigitalFilterType filterType, double cutoffFrequency, int filterLength)
        {
            switch (filterType)
            {
                case DigitalFilterType.LowPass:
                    {
                        return LowPassFilter.GetLowPassFilter(cutoffFrequency, filterLength);
                    }
				default:
					{
						throw new NotSupportedException();
					}
            }
        }

        public static DigitalFilter GetDigitalFilter(DigitalFilterType filterType, List<double> stockInputData, int timeSpacing, int numberOfWeights, double frequencyLowEndCutOff =0, double frequencyLowEndRollOff=0, double frequencyHighEndRollOff=0, double frequencyHighEndCutOff =0)
        {
            switch (filterType)
            {
                case DigitalFilterType.BandPass:
                    {
                        return new BandPassFilter(stockInputData, timeSpacing, numberOfWeights, frequencyLowEndCutOff, frequencyLowEndRollOff, frequencyHighEndRollOff, frequencyHighEndCutOff);
                    }
                case DigitalFilterType.MovingAverage:
                    {
                        return new MovingAverageFilter(stockInputData, timeSpacing, numberOfWeights);
                    }
                default:
                    {
                        throw new NotSupportedException();
                    }
            }

        }
    }
}
