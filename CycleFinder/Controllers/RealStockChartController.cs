using System.Linq;
using CycleFinder.Models;
using Microsoft.AspNetCore.Mvc;
using CycleFinder.Extensions;
using CycleFinder.Helpers;
using CycleFinder.Models.DigitalFilters;

namespace CycleFinder.Controllers
{
    public class RealChartController : Controller
    {
        public RealChartController()
        {
        }

		public IActionResult RealStockChartFourier()
		{
			ViewData["Message"] = "Stock Data Chart with Fourier Analysis.";

			//just use ViewBag for viewmodel for the moment


			var inputSignalsAXJO = InputSignal.GetInputData(@"/Users/davidcampbell/Downloads/^AXJO.csv").inputSignalList;
			ViewBag.StockChart = inputSignalsAXJO.GoogleChartDataFormat();
			var frequencyConverter = 260 / (double)inputSignalsAXJO.Count;
			ViewBag.DFT = Fourier.DFT1(inputSignalsAXJO).Select(x => x.Magnitude).ToList().GoogleChartDataFormat(frequencyConverter);  //ie sample rate of 260 days per year divided by number of samples
			return View();
		}

		public IActionResult RealOilChartFourier()
		{
			ViewData["Message"] = "Crude Oil Data Chart with Fourier Analysis.";

            //just use ViewBag for viewmodel for the moment


            var inputSignalsOil = InputSignal.GetInputData(@"/Users/davidcampbell/Downloads/Crude Oil WTI Futures Historical Data.csv").inputSignalList;
            inputSignalsOil.Reverse();
			ViewBag.OilChart = inputSignalsOil.GoogleChartDataFormat();
			var frequencyConverter = 52 / (double)inputSignalsOil.Count;
			ViewBag.DFT = Fourier.DFT(inputSignalsOil).GoogleChartDataFormat(frequencyConverter);  //ie sample rate of 52 weeks per year divided by number of samples
			return View();
		
        //low cycles @ 0.26, 0.923 cycles per year
        
        }


		public IActionResult RealOilChartLowPass()
		{
			ViewData["Message"] = "Crude Oil Data Chart with Fourier Analysis.";

            //just use ViewBag for viewmodel for the moment

            var viewModel = new ChartViewModel();

			var inputSignalsOil = InputSignal.GetInputData(@"/Users/davidcampbell/Downloads/Crude Oil WTI Futures Historical Data.csv").inputSignalList;
			inputSignalsOil.Reverse();
            viewModel.InputSignalSeries = inputSignalsOil;
			var frequencyConverter = 52 / (double)inputSignalsOil.Count;
			ViewBag.DFT = Fourier.DFT(inputSignalsOil).GoogleChartDataFormat(frequencyConverter);  //ie sample rate of 52 weeks per year divided by number of samples

            viewModel.SetFilter(DigitalFilterType.LowPass, 2.9, 52, 301);
          


            return View();

			//low cycles @ 0.26, 0.923 cycles per year

		}



    }


}
