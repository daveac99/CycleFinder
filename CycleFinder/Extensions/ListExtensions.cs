using System;
using System.Collections.Generic;
using CycleFinder.Models;
namespace CycleFinder.Extensions
{
    public static class ListExtensions
    {
        //handles multiple list inputs
        public static string GoogleChartDataFormat(this List<List<double>>  dataLists, double XMultiplier=1) //use the (sample rate/ N) for example to convert values on x axis
        {
            string data = "";
            var elementCount = dataLists[0].Count;
            for (var i = 0; i < elementCount; i++) //i is lists element - use the count of the first list
            {
                var rowitem = (i * XMultiplier).ToString(); //x-axis value
				for (var j = 0; j < dataLists.Count; j++) //j is the list number
                {
                    rowitem += $",{dataLists[j][i]}";
                }
                if (i < elementCount-1)
                    data += $"[{rowitem}],";
                else
                    data += $"[{rowitem}]"; //last row
            }

			return data;            
        }

        //handles single list
		public static string GoogleChartDataFormat(this List<double> dataList,double XMultiplier=1)
		{
			string data = "";
			var i = 0;
			foreach (var item in dataList)
			{
				if (i < dataList.Count-1)
					data += $"[{XMultiplier * i++},{item}],";
				else
					data += $"[{XMultiplier * i++},{item}]"; //last row
			}

			return data;
		}


		//TODO: Use a generic type
		public static string GoogleChartDataFormat(this List<decimal> dataList)
		{
			string data = "";
			var i = 0;
			foreach (var item in dataList)
			{
				if (i < dataList.Count-1)
					data += $"[{i++},{item}],";
				else
					data += $"[{i++},{item}]";
			}

			return data;
		}

		public static string GoogleChartDataFormat(this List<AmplitudeFrequency> dataList)
		{
			string data = "";
			var i = 0;
			foreach (var item in dataList)
			{
				if (i++ < dataList.Count-1)
					data += $"[{item.AngularFrequency},{item.CompositeAmplitude}],";
				else
					data += $"[{item.AngularFrequency},{item.CompositeAmplitude}]";
                if (item.AngularFrequency > 11)
                    break;
			}

			return data;
		}


		public static string GoogleChartDataFormat<T, TResult>(this List<T> dataList, Func<T, TResult> X, Func<T, TResult> Y)
        {
            return GoogleChartDataFormat<T, TResult>(dataList, X, Y, n => false);
        }
		

        public static string GoogleChartDataFormat<T,TResult>(this List<T> dataList, Func<T, TResult> X, Func<T, TResult> Y, Func<T, bool> completionCondition)
		{
			string data = "";
			var i = 0;
			foreach (var item in dataList)
			{
				if (i++ < dataList.Count-1)
                    data += $"[{X(item)},{Y(item)}],";
				else
                    data += $"[{X(item)},{Y(item)}]";
                if (completionCondition(item))
					break;
			}

			return data;
		}

    }
}
