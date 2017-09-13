﻿using System;
using Microsoft.AspNetCore.Mvc;
using CycleFinder.Models;

namespace CycleFinder.Controllers
{
    public class StockChartSimulationController : Controller
    {
        public StockChartSimulationController()
        {
        }

		public IActionResult SineWavesChart()
		{
			ViewData["Message"] = "Charts sine waves and fourier transform.";
			var viewmodel = new StockChartSimulationViewModel();
			viewmodel.AddSineWave(1000, 1, 0, "blue");
			viewmodel.AddSineWave(2000, .5, .75 * Math.PI, "green");

			viewmodel.GetInputSignalSeriesSummed(8, 8000);
			//var DFT = Fourier.DFT1(sampleList);

			return View(viewmodel);
		}


		public IActionResult StockChartFourier()
		{
			ViewData["Message"] = "Charts sine waves and fourier transform.";
			var viewmodel = new StockChartSimulationViewModel(); //TODO create a StockChartViewModel which inherits from SineWaveViewModel
																 //viewmodel.AddSineWave(1000,1,0,"blue");
																 //viewmodel.AddSineWave(2000,.5, .75 * Math.PI,"green");
			viewmodel.AddSineWave(2.5, 30, 0, "blue");
			viewmodel.AddSineWave(7, 10, 0, "green");
			viewmodel.AddSineWave(21, 5, 0, "green");
			viewmodel.AddSineWave(35, 2, 0, "green");
			viewmodel.LongTermTrend = new LongTermTrend(100, 200, 1, 100, 0);
			viewmodel.GetInputSignalSeriesPerWave(520, 260);
			viewmodel.GetInputSignalSeriesSummed(520, 260);
			//var DFT = Fourier.DFT1(sampleList);

			return View(viewmodel);
		}

		public IActionResult BandPassFilteredChart()
		{
			ViewData["Message"] = "Band Pass filter Chart.";

			//just use ViewBag for viewmodel for the moment

			var stockChartSimulationViewModel = new StockChartSimulationViewModel(); //TODO create a StockChartViewModel which inherits from SineWaveViewModel
															 //viewmodel.AddSineWave(1000,1,0,"blue");
															 //viewmodel.AddSineWave(2000,.5, .75 * Math.PI,"green");
			stockChartSimulationViewModel.AddSineWave(0.25, 90, 0); //2
			stockChartSimulationViewModel.AddSineWave(0.67, 45, 0); //3
			stockChartSimulationViewModel.AddSineWave(1, 15, 0);    //4
            stockChartSimulationViewModel.AddSineWave(3, 10, 0);    //5
            stockChartSimulationViewModel.AddSineWave(5, 7, 0);    //6
			stockChartSimulationViewModel.LongTermTrend = new LongTermTrend(100, 30, 0.1, 100, 0); //1
			stockChartSimulationViewModel.GetInputSignalSeriesPerWave(2000, 52);
            stockChartSimulationViewModel.GetInputSignalSeriesSummed(2000, 52);
            //viewmodel.AddFilter(DigitalFilterType.BandPass, 7,99,1,1.5,3,4);
            stockChartSimulationViewModel.AddFilter(DigitalFilterType.BandPass, 7, 199, 0.14, 0.2, 0.30, 0.36); //sets property values as well
            //viewmodel.AddFilter(DigitalFilterType.MovingAverage,1,30);

			return View(stockChartSimulationViewModel);
		}

        [HttpPost]
        public IActionResult BandPassFilteredChart(StockChartSimulationViewModel stockChartSimulationViewModel)
        {
            stockChartSimulationViewModel.RemoveFilters(DigitalFilterType.BandPass);
            stockChartSimulationViewModel.AddFilter(DigitalFilterType.BandPass); //use the properties set in the view

            return View(stockChartSimulationViewModel);
        }
	}
}

//vary only t:
//  1, 99, 0.18, 0.24, 0.26, 0.32)  OK - A=2.5 to 5 - uneven
//  2, 99, 0.18, 0.24, 0.26, 0.32)  OK - A=25 - fairly even
//  3, 99, 0.18, 0.24, 0.26, 0.32)  A =37 and 5 - uneven
//  4, 99, 0.18, 0.24, 0.26, 0.32); very smooth A = 50 - 2.5 cycles
//  5, 99, 0.18, 0.24, 0.26, 0.32)  A=30  and only 1 cycle - but uneven


// Remove other waves (ie leave only cycle #2):
//  4, 99, 0.18, 0.24, 0.26, 0.32); very smooth A = 50 (still even though original is 90) - 2.5 cycles

//put filter outside the 0.25 range - still only cycle#2
// 4, 99, 0.38, 0.44, 0.46, 0.52); - gives smooth A=10, 2.5 cycles (expected it to be flat!)

//even more out of range:
// 4, 99, 1.0, 1.06, 2.0, 2.06); - gives smooth A=5, 2.5 cycles - conclusion 

//now try putting outside as above but use t=1 - perhaps t=1 is better!
// 1, 99, 0.38, 0.44, 0.46, 0.52) - now A=5
// 1, 99, 1.0, 1.06, 2.0, 2.06); - now A=1 

//now vary the taps (still with only cycle#2)
// 1, 99, 0.18, 0.24, 0.26, 0.32) - A=2 - even
// 1, 31, 0.18, 0.24, 0.26, 0.32) - A=0.007
// 1, 3, 0.18, 0.24, 0.26, 0.32);  - A=practically zero

//best t of 4 with 99 taps and still ony cycle#2
//4, 99, 0.18, 0.24, 0.26, 0.32) - A=50

//** by filter efficiciency, he means how close you can get the bands
//now vary delta
// 4, 99, 0, 0.25, 0.30, 0.6);     - A=83   $ * 99 * (.25 * 2pi) = 621
// 4, 99, 0.10, 0.20, 0.30, 0.40); - A=84   4 * 99 * (.1 * 2pi) = 250
// 4, 99, 0.10, 0.16, 0.30, 0.36); - A=81   4 * 99 * (.06*2pi) = 149    (*a-1)
// 4, 99, 0.10, 0.12, 0.30, 0.32); - A=69   4 * 99 * (.02*2pi) = 49
// 4, 99, 0.10, 0.12, 0.30, 0.32); - A=69   4 * 99 * (.02*2pi) = 49

//more data points
//2000 data points
//4, 99, 0.10, 0.16, 0.30, 0.36  - A=80 (no difference from same one above (a-1)
//4, 199, 0.10, 0.16, 0.30, 0.36); - A=67
//7, 99, 0.10, 0.16, 0.30, 0.36) - A=57
//7, 199, 0.10, 0.16, 0.30, 0.36); - A=31

//2000 data points - put all the waves back
// 7, 199, 0.10, 0.16, 0.30, 0.36); - A=38 to %$ - uneven
//4, 99, 0.10, 0.16, 0.30, 0.36); - A=80,110,40,120
//4, 99, 0.14, 0.2, 0.30, 0.36) - A=80,95,60,100 - more even
//7, 199, 0.14, 0.2, 0.30, 0.36 - A=50 - fairly even
//4, 199, 0.14, 0.2, 0.30, 0.36) -A=70,100,65