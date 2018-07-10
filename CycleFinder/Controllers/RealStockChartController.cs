using System.Linq;
using CycleFinder.Extensions;
using CycleFinder.Helpers;
using CycleFinder.Models;
using CycleFinder.Models.DigitalFilters;
using Microsoft.AspNetCore.Mvc;

namespace CycleFinder.Controllers
{
    public class RealChartController : Controller  
    {
        public IActionResult RealStockChartFourier()  //TODO change name
        {
            ViewData["Message"] = "Stock Data Chart with Fourier Analysis.";

            var viewModel = ChartViewModel.GetNewChartAndSetInputSignalSeries(@"C:\Users\Dave\Downloads/AXJO.csv", 52);
            //var frequencyConverter = 260 / (double)viewModel.InputSignalSeries.Count;
            //ViewBag.DFT = Fourier.DFT1(viewModel.InputSignalSeries).Select(x => x.Magnitude).ToList()
            //    .GoogleChartDataFormat(
            //        frequencyConverter); //ie sample rate of 260 days per year divided by number of samples
            viewModel.SetFilter(DigitalFilterType.BandPass, .2, .3, (int) viewModel.SampleRateForInputSignalSeries, 301, WindowType.None);

            return View(viewModel);
        }

        public IActionResult RealOilChartFourier() 
        {
            ViewData["Message"] = "Crude Oil Data Chart with Fourier Analysis.";

            //just use ViewBag for viewmodel for the moment


            var inputSignalsOil = InputSignal
                .GetInputData(@"/Users/davidcampbell/Downloads/Crude Oil WTI Futures Historical Data.csv")
                .inputSignalList;
            inputSignalsOil.Reverse();
            ViewBag.OilChart = inputSignalsOil.GoogleChartDataFormat();
            var frequencyConverter = 52 / (double)inputSignalsOil.Count;
            ViewBag.DFT =
                Fourier.DFT(inputSignalsOil)
                    .GoogleChartDataFormat(
                        frequencyConverter); //ie sample rate of 52 weeks per year divided by number of samples

            return View();

            //low cycles @ 0.26, 0.923 cycles per year
        }


        public IActionResult RealOilChartLowPass()
        {
            ViewData["Message"] = "Crude Oil Data Chart with Fourier Analysis.";

            var viewModel = ChartViewModel.GetNewChartAndSetAndReverseInputSignalSeries(@"/Users/davidcampbell/Downloads/Crude Oil WTI Futures Historical Data.csv", 52);

            viewModel.SetFilter(DigitalFilterType.LowPass, 2.9, 52, 301);

            return View(viewModel);

            //low cycles @ 0.26, 0.923 cycles per year
        }
    }
}