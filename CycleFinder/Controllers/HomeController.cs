using System.Linq;
using CycleFinder.Models;
using Microsoft.AspNetCore.Mvc;
using CycleFinder.Extensions;

namespace CycleFinder.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            //just use ViewBag for viewmodel for the moment
            var inputSignals = InputSignal.GetInputData().inputSignalListdecimal;
            var inputSignalsDouble = InputSignal.GetInputData().inputSignalList;                            
            ViewBag.data1 = inputSignals.GoogleChartDataFormat();
    //        ViewBag.data2 = new Fourier(inputSignalsDouble).PowerSpectrum.ToList().GoogleChartDataFormat();
            var inputSignalsAXJO = InputSignal.GetInputData(@"/Users/davidcampbell/Downloads/^AXJO.csv").inputSignalListdecimal;
            ViewBag.data3 = inputSignalsAXJO.GoogleChartDataFormat();
           // ViewBag.data4 = new Fourier(inputSignalsAXJO).PowerSpectrum.ToList().GoogleChartDataFormat();
            var inputSignalsAXJOdecimal = InputSignal.GetInputData(@"/Users/davidcampbell/Downloads/^AXJO.csv").inputSignalListdecimal;
			// ViewBag.data5 = new FourierLanczos(inputSignals).CompositeAmplitudes.Where(n => n.AngularFrequency < 11).ToList().GoogleChartDataFormat(x => x.AngularFrequency, y => y.CompositeAmplitude); //, f => f. > 11);
			ViewBag.data5 = new FourierLanczos(inputSignals,"Yearly").CompositeAmplitudes.GoogleChartDataFormat(x => x.Period, y => y.CompositeAmplitude); //, f => f. > 11);
			return View();
        }

		public IActionResult WaveChart()
		{
			ViewData["Message"] = "Charts a wave.";

            var waveComposite = new WaveGenerator("Sample", 10);
            waveComposite.AddLongTermTrend(100,200);
            waveComposite.AddWave(1, 10);
            waveComposite.AddWave(3, 30);
			//just use ViewBag for viewmodel for the moment
			var inputSignalsDouble = waveComposite.CompositeTimeSeries;
			ViewBag.data1 = inputSignalsDouble.GoogleChartDataFormat();
            //var test = new FourierLanczos(inputSignalsDouble.Select(n => (decimal)n).ToList(), "Daily").CompositeAmplitudes;
            ViewBag.data2 = new FourierLanczos(inputSignalsDouble.Select(n => (decimal)n).ToList(), "Daily").CompositeAmplitudes.GoogleChartDataFormat(x => x.Period, y => y.CompositeAmplitude);

			return View();
		}







		public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
