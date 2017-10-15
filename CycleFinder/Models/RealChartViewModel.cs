using System;
using System.Collections.Generic;

namespace CycleFinder.Models
{
    public class RealChartViewModel : ChartViewModel
    {
        public RealChartViewModel()
        {
        }

        public List<double> Commodity => InputSignalSeries;
        public List<double> FilteredCommodity => InputSignalConvoluted;
        public int SampleRate { get; set; }

       


    }
}
