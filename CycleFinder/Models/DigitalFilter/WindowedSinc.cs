using System;
using static System.Math;
using CycleFinder.Models.Window;

namespace CycleFinder.Models.DigitalFilters
{
    public class WindowedSinc : DigitalFilter
    {
        public WindowedSinc(double cutoffFrequency, int filterLength, WindowType windowType)
        {
            this.filterLength = filterLength;
            this.cutoffFrequency = cutoffFrequency;
            this.windowType = windowType;
            SetKernel();
        }

        private double cutoffFrequency;
        private int filterLength;
        private WindowType windowType;

        public void SetKernel()
        {
            double element;
            for (int i = 0; i < filterLength; i++)
            {
                if (i - (filterLength/2) == 0)
                {
                    element = 2 * PI * cutoffFrequency;
                }
                else
                {
                    element = Sin(2 * PI * cutoffFrequency * (i - (filterLength / 2))) / (i - (filterLength / 2));
                }

                Kernel.Add(element);
            }

            switch (windowType)
            {
                case WindowType.None:
                    break;
                    
                case WindowType.Blackman:
                    var windowBlackman = new BlackmanWindow(Kernel);
                    Kernel = windowBlackman.Result;
                    break;

                case WindowType.Hamming:
                    var windowHamming = new HammingWindow(Kernel);
                    Kernel = windowHamming.Result;
                    break;

            }
        }

    }
}
