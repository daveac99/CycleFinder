using System;
namespace CycleFinder.Models
{
    public class PowerSpectrum
    {
        public PowerSpectrum(double[,] cosData, double[,] sinData)
        {
            for (var i = 0; i < cosData.GetUpperBound(1) ; i++)
            {
               // data[cosData[]]
            }
        }



       // [1,8898],[2,2727],[3,8383]

        //could use a dictionary here ie key value pairs
        public double[,] data
		        {
		            get;
		            set;
		        }
    }
}
