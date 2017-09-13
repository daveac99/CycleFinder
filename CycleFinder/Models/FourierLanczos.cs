using System;
using System.Collections.Generic;
using System.Linq;

namespace CycleFinder.Models
{
    public class FourierLanczos : IFourier
    {
        public List<decimal> prices { get; set; }

        List<decimal> dataSequence1 = new List<decimal>();
        List<decimal> dataSequence2 = new List<decimal>();
        List<double> angularFrequencies = new List<double>();

        public List<AmplitudeFrequency> CompositeAmplitudes { get; private set; }
        private int m { get { return prices.Count; } }
        private double Z { get { return Math.PI / ((m - 1) / 2); } }

        private double GetPeriod(double angle)
        {
            return 2 * Math.PI / angle;
        }

        public FourierLanczos(List<decimal> prices):this(prices, "Weekly")
        {
          
        }

        public FourierLanczos(List<decimal> prices, string digitalSpacing)
        {
            var multiplier = digitalSpacing == "Daily" ? 365 : digitalSpacing == "Weekly" ? 52 : 1;
			this.prices = prices;
			SetSequence1();
			SetSequence2();
			SetAngularFrequencies(multiplier);   
            SetCompositeAmplitudes();
        }


        public void SetSequence1()
        {
            var middlePosition= prices.Count / 2;
            dataSequence1.Add(prices[middlePosition]);
            for (int i = 1; i < prices.Count/2; i++)
            {
                dataSequence1.Add(prices[middlePosition-i] + prices[middlePosition+i]);
            }
            //divide last one by two
            dataSequence1.Add((prices.First() + prices.Last())/2);

        }

        public void SetSequence2()
        {
            var middlePosition = prices.Count / 2;
            dataSequence2.Add(0);
            for (int i = 1; i < prices.Count / 2; i++)
            {
                dataSequence2.Add(prices[middlePosition+i] - prices[middlePosition-i]);
            }
            //set last one to zero
            dataSequence2.Add(0);


        }

        public void SetAngularFrequencies(int multiplier) //yearly
        {

            for (int i = 0; i <= (m - 1) / 2; i++)
            {
                angularFrequencies.Add(i * Z * multiplier);
            }
        }


        public void SetCompositeAmplitudes()
        {
            int i = 0;
            double amplitudeCos;
            double amplitudeSin;
            CompositeAmplitudes = new List<AmplitudeFrequency>();
            foreach  (var angularFrequency in angularFrequencies)
            {
                if (i == 0)
                {
                    amplitudeCos = (double)dataSequence1.Sum() / ((m - 1) / 2);
                    amplitudeSin = 0;
                }
                else
                {
                    //double accumulator = 0;
                    //int j = 0;
                    //foreach (var dataItem in dataSequence1)
                    //{
                    //    accumulator += (double)dataItem * (j == 0 ? 1 : Math.Cos(Z) * j);
                    //    j++;
                    //}
                    int j = 0;
                    var amplitudeSumCos = dataSequence1.Sum(n => (double)n * (j == 0 ? 1 + j++ : Math.Cos(i * Z * j++)));
					j = 0;
                    var amplitudeSumSin = dataSequence2.Sum(n => (double)n * (j == 0 ? 1 + j++ : Math.Sin(i * Z * j++)));
                    amplitudeCos = amplitudeSumCos / ((m - 1) / 2);
                    amplitudeSin = amplitudeSumSin / ((m - 1) / 2);
                }

                CompositeAmplitudes.Add(new AmplitudeFrequency() 
                    {   CosineAmplitude = amplitudeCos,
                        SineAmplitude = amplitudeSin,
                        AngularFrequency = angularFrequency});
                i++;
            };
        }
    }
}

