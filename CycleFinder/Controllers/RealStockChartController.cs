using System.Linq;
using CycleFinder.Models;
using Microsoft.AspNetCore.Mvc;
using CycleFinder.Extensions;

namespace CycleFinder.Controllers
{
    public class RealStockChartController : Controller
    {
        public RealStockChartController()
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

    }


}
