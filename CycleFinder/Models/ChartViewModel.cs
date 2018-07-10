using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using CycleFinder.Extensions;
using static CycleFinder.Helpers.Fourier;
using CycleFinder.Models;
using CycleFinder.Helpers;
using CycleFinder.Models.DigitalFilters;

namespace CycleFinder.Models
{
    public class ChartViewModel
    {
        //Responsibilities: Changed the way we view the data
        public ChartViewModel()
        {
        }

        internal List<double> InputSignalSeries { get; set; }
        public double SampleRateForInputSignalSeries { get; set; }

        public DigitalFilter DigitalFilter { get; set; }
        public List<Double> InputSignalConvoluted => DSP.Convolve(InputSignalSeries, DigitalFilter.Kernel);
        public string InputSignalConvolutedFormatted => InputSignalConvoluted.GoogleChartDataFormat();



        public List<DigitalFilterViewModel> Filters { get; set; } = new List<DigitalFilterViewModel>();

        internal List<double> DFT1 => DFT1(InputSignalSeries).Select(x => x.Magnitude).ToList();
        internal List<double> DFT2 => DFT2(InputSignalSeries).Select(x => x.Magnitude).ToList();
        internal List<double> FFT
        {
            get
            {
                Complex[] input = InputSignalSeries.Select(x => new Complex(x, 0)).ToArray();
                FFT(input);
                return input.Select(x => x.Magnitude).ToList();
            }
        }

        public virtual string DFT1Formatted => DFT1.GoogleChartDataFormat(SampleRateForInputSignalSeries / DFT1.Count);
        public virtual string DFT2Formatted => DFT2.GoogleChartDataFormat();
        public virtual string FFTFormatted => FFT.GoogleChartDataFormat(SampleRateForInputSignalSeries / DFT1.Count);


        public List<WaveOutput> FilterOnly
        {
            get
            {
                var wavesList = new List<WaveOutput>();

                wavesList.AddRange(Filters.Select(x => new WaveOutput(x.NumericAnalysis, $"{x.FilterType}: {x.Parameters}")));
                return wavesList;
            }
        }

        public List<WaveOutput> FilterOnlyFrequencyResponse
        {
            get
            {
                var wavesList = new List<WaveOutput>();

                wavesList.AddRange(Filters.Select(x => new WaveOutput(DFT1(x.NumericAnalysis).Select(y => y.Magnitude).ToList(), $"{x.FilterType}: {x.Parameters}")));
                return wavesList;
            }
        }


        public string FilterOnlyFormatted => FilterOnly.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();
        public string FilterOnlyFrequencyResponseFormatted => FilterOnlyFrequencyResponse.Select(x => x.OutputSeries).ToList().GoogleChartDataFormat();


        public string InputSignalSeriesFormatted => InputSignalSeries.GoogleChartDataFormat();

        //input fields
        [Display(Name = "t")]
        public int TimeSpacing { get; set; }
        [Display(Name = "# Wts")]
        public int NumberOfWeights { get; set; }
        [Display(Name = "f Lo CutO")]
        public double FrequencyLowEndCutOff { get; set; }
        [Display(Name = "f Lo RollO")]
        public double FrequencyLowEndRollOff { get; set; }
        [Display(Name = "f Hi RollO")]
        public double FrequencyHighEndRollOff { get; set; }
        [Display(Name = "f Hi CutO")]
        public double FrequencyHighEndCutOff { get; set; }

        public void AddFilter(DigitalFilterType filterType) //use already defined properties
        {
            AddFilter(filterType, TimeSpacing, NumberOfWeights, FrequencyLowEndCutOff, FrequencyLowEndRollOff, FrequencyLowEndRollOff, FrequencyHighEndCutOff);
        }

        public void AddFilter(DigitalFilterType filterType, int timeSpacing, int numberofWeights)
        {
            AddFilter(filterType, timeSpacing, numberofWeights, 0, 0, 0, 0);
        }

        public void AddFilter(DigitalFilterType filterType, int timeSpacing, int numberOfWeights, double frequencyLowEndCutOff, double frequencyLowEndRollOff, double frequencyHighEndRollOff, double frequencyHighEndCutOff)
        {

            var filter = Factory.SetDigitalFilter(filterType, InputSignalSeries, timeSpacing, numberOfWeights, frequencyLowEndCutOff, frequencyLowEndRollOff, frequencyHighEndRollOff, frequencyHighEndCutOff);

            Filters.Add(new DigitalFilterViewModel(filterType, $"Weights: {numberOfWeights}", filter));
            //set the view model properties
            TimeSpacing = timeSpacing;
            NumberOfWeights = numberOfWeights;
            FrequencyLowEndCutOff = frequencyLowEndCutOff;
            FrequencyLowEndRollOff = frequencyLowEndRollOff;
            FrequencyHighEndRollOff = frequencyHighEndRollOff;
            FrequencyHighEndCutOff = frequencyHighEndCutOff;
        }

        public void SetFilter(DigitalFilterType filterType, double cutoffFrequency, int sampleRate, int filterLength)
        {
            SetFilter(filterType, cutoffFrequency / sampleRate, filterLength);
        }

        public void SetFilter(DigitalFilterType filterType, double cutoffFrequencyLower, double cutoffFrequencyUpper, int sampleRate, int filterLength, WindowType windowType)
        {
            SetFilter(filterType, cutoffFrequencyLower / sampleRate, cutoffFrequencyUpper / sampleRate, filterLength, windowType);
        }

        //sets DigitalFilter property for lower/upper cutoff
        public void SetFilter(DigitalFilterType filterType, double cutoffFrequencyRate, int filterLength)
        {
            var filter = Factory.SetDigitalFilter(filterType, cutoffFrequencyRate, filterLength);
            DigitalFilter = filter;
        }

        //sets DigitalFilter property for band
        public void SetFilter(DigitalFilterType filterType, double cutoffFrequencyRateLower, double cutoffFrequencyRateUpper, int filterLength, WindowType windowType)
        {
            var filter = Factory.SetDigitalFilter(filterType, cutoffFrequencyRateLower, cutoffFrequencyRateUpper, filterLength, windowType);
            DigitalFilter = filter;
        }

        public void RemoveFilters(DigitalFilterType filterType)
        {
            Filters.RemoveAll(x => x.FilterType == filterType);
        }

        #region static factory methods

        public static ChartViewModel GetNewChartAndSetInputSignalSeries(string chartDataSource, double sampleRate)
        {
            var inputSignal = InputSignal.GetInputData(chartDataSource).inputSignalList;
            var chartViewModel = new ChartViewModel
            {
                InputSignalSeries = inputSignal,
                SampleRateForInputSignalSeries = sampleRate
            };
            return chartViewModel;
        }

        public static ChartViewModel GetNewChartAndSetAndReverseInputSignalSeries(string chartDataSource, double sampleRate)
        {
            var inputSignal = InputSignal.GetInputData(chartDataSource).inputSignalList;
            inputSignal.Reverse(); 
            var chartViewModel = new ChartViewModel
            {
                InputSignalSeries = inputSignal,
                SampleRateForInputSignalSeries = sampleRate
            };
            return chartViewModel;
        }


        #endregion

    }
}
