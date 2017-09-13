using System;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Models
{
    public class WaveGenerator
    {
        public WaveGenerator(string stockName, double timePeriod)
        {
			StockName = stockName;
			TimePeriod = timePeriod;

		}

        //case here for DI
        //public WaveGenerator(string stockName = "Apple", double timePeriod = 9) //years
        //{
        //    StockName = stockName;
        //    TimePeriod = timePeriod;
        //    AddLongTermTrend(15, 130, timePeriod);  //years
        //    AddWave(3, 40);
        //    AddWave(.5,25);
        //    AddWave(.2,16);

        //}

        public string StockName { get; }
        public double TimePeriod { get; }

        public LongTermTrend LongTermTrend { get; private set; }

        public List<Wave> Waves { get; private set; } = new List<Wave>();

		//generate price series from wave properties
		public List<double> CompositeTimeSeries 
        {
            get{
                
				var dataPoints = 365 * TimePeriod;
                var timeSeries = new double[(int)dataPoints];
                if (LongTermTrend != null)
                {
                    for (var i = 0; i < dataPoints; i++)
                    {
                        timeSeries[i] = LongTermTrend.StartPrice + ((i / dataPoints) * (LongTermTrend.EndPrice - LongTermTrend.StartPrice));
                    }
                }
                foreach (var wave in Waves)
                {
                    //generate daily points
                    var cyclePointsLength = 365 * wave.Period;
                    for (var i = 0; i < dataPoints; i++)
                    {
                        var cycle = (int)(i / cyclePointsLength);
                        timeSeries[i] += wave.Amplitude/2 * (Math.Sin(2 * Math.PI * (i - (cycle * cyclePointsLength))/ cyclePointsLength));
                    }
                }
                return timeSeries.ToList();
            }
        }

        public void AddLongTermTrend(double startPrice, double endPrice)
        {
            LongTermTrend = new LongTermTrend(startPrice, endPrice,0,0,0);
        }

        public void AddWave(double period, double amplitude, string colour = "black")
        {
            var wave = new Wave(period, amplitude, colour);

            Waves.Add(wave);

        }

    }
}
