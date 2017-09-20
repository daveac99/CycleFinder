using System;
using System.Collections.Generic;
using CycleFinder.Extensions;

namespace CycleFinder.Models
{
    public class DigitalFilterViewModel
    {
        public DigitalFilterViewModel(DigitalFilterType filterType, string parameters, DigitalFilter filter)
        {
            FilterType = filterType;
            Parameters = parameters;
            DataSeries = filter.StockPricesFiltered;
            ConvertedStockInputData = filter.ConvertedStockInputData;
            NumericAnalysis = filter.Kernel;
        }

        public DigitalFilterType FilterType { get; set; }
        public string Parameters { get; set; }
        public List<double> DataSeries { get; set; }
        public string DataSeriesFormatted => DataSeries.GoogleChartDataFormat();
        public List<double> ConvertedStockInputData;
        public List<double> NumericAnalysis { get; set; }

    }
}
