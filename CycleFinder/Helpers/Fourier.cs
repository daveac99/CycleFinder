using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CycleFinder.Helpers
{
    public static class Fourier
    {
		//double[] cosSums = new double[1000];
		//double[] sinSums = new double[1000];
  //      double[] powerSpectrum = new double[1000];

		//public Fourier(List<double> signalList)
   //     {

			//var n = 0;
			//for (var k = 0; k <= 888; k++)
			//{
   //             n = 0;
			//	cosSums[k] = signalList.Sum(arg => arg * Math.Cos(2 * Math.PI * k * n++ / signalList.Count()));
			//	sinSums[k] = signalList.Sum(arg => arg * Math.Sin(2 * Math.PI * k * n++ / signalList.Count()));
			//}


        //    for (var k = 0; k <= 888; k++)
        //    {
        //        powerSpectrum[k] = cosSums[k] * cosSums[k] + sinSums[k] * sinSums[k];
        //    }
        //}

        public static List<double> DFT(List<double> inputList)
        {
            return DFT2(inputList).Select(x => x.Magnitude).ToList();
        }

        public static Complex[] DFT1(List<double> inputList)
        {
            var outputArray = new Complex[inputList.Count];
            var inputArray = inputList.ToArray();

            for (var m = 0; m <= inputList.Count/2; m++)
            {
                double realSum = 0;
                double imaginarySum = 0;
                for (var n = 0; n < inputList.Count; n++)
                {
                    var input = inputArray[n];
                    var real = Math.Cos(2 * Math.PI * n * m / inputList.Count);
                    var imaginary = Math.Sin(2 * Math.PI * n * m / inputList.Count);

                    realSum += (input * real); 
                    imaginarySum += (input * imaginary);
                }
                outputArray[m] = new Complex(realSum, imaginarySum);

            }
            return outputArray;
        }

		public static Complex[] DFT2(List<double> inputList)
		{

            var outputArray = new Complex[inputList.Count];

			for (var m = 0; m <= inputList.Count/2; m++)
			{
			    var n = 0;
			    var realSum = inputList.Sum(arg => arg * Math.Cos(2 * Math.PI * m * n++ / inputList.Count()));
			    var imaginarySum = inputList.Sum(arg => arg * Math.Sin(2 * Math.PI * m * n++ / inputList.Count()));
                outputArray[m] = new Complex(realSum, imaginarySum);
			}
            return outputArray;

		}

		/* Performs a Bit Reversal Algorithm on a postive integer 
		 * for given number of bits
		 * e.g. 011 with 3 bits is reversed to 110 */
		public static int BitReverse(int n, int bits)
		{
			int reversedN = n;
			int count = bits - 1;

			n >>= 1;
			while (n > 0)
			{
				reversedN = (reversedN << 1) | (n & 1);
				count--;
				n >>= 1;
			}

			return ((reversedN << count) & ((1 << bits) - 1));
		}

		/* Uses Cooley-Tukey iterative in-place algorithm with radix-2 DIT case
		 * assumes no of points provided are a power of 2 */
		public static void FFT(Complex[] buffer)
		{

			int bits = (int)Math.Log(buffer.Length, 2);
			for (int j = 1; j < buffer.Length / 2; j++)
			{

				int swapPos = BitReverse(j, bits);
				var temp = buffer[j];
				buffer[j] = buffer[swapPos];
				buffer[swapPos] = temp;
			}

			for (int N = 2; N <= buffer.Length; N <<= 1)
			{
				for (int i = 0; i < buffer.Length; i += N)
				{
					for (int k = 0; k < N / 2; k++)
					{

						int evenIndex = i + k;
						int oddIndex = i + k + (N / 2);
						var even = buffer[evenIndex];
                        try {
                            var odd = buffer[oddIndex];


							double term = -2 * Math.PI * k / (double)N;
							Complex exp = new Complex(Math.Cos(term), Math.Sin(term)) * odd;

							buffer[evenIndex] = even + exp;
							buffer[oddIndex] = even - exp;
                        }
                        catch (Exception ex){
                            var x = ex.Message;
                        }


					}
				}
			}
		}



		//public double[] PowerSpectrum 
		        //{
		        //    get
          //          {
          //              return powerSpectrum;
          //          }
		        //    //set;
		        //}


    }
}
