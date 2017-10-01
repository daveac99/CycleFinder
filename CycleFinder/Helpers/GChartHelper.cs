using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;

namespace CycleFinder.Helpers
{
    public static class GChartHelper
    {


        public static string RowsFor<TModel>(this IHtmlHelper<TModel> htmlHelper, Func<TModel, List<double>> getDataList, int sampleCutoff=0)
        {
            return RowsFor(htmlHelper, getDataList, x => 1, sampleCutoff);
        }

        public static string RowsFor<TModel>(this IHtmlHelper<TModel> htmlHelper, Func<TModel, List<double>> getDataList, Func<TModel, double> XMultiplier, int sampleCutOff=0)
        {
            //myfunc will be in the form: model => model.InputSignalSeries
            var model = htmlHelper.ViewData.Model;
            List<double> dataList = getDataList(model);
            double mult = XMultiplier(model);

            string data = "";
            var i = 0;
            foreach (var item in dataList)
            {
                if (i < (sampleCutOff==0 ? dataList.Count : sampleCutOff)-1)
                {
                    data += $"[{mult * i++},{item}],";
                }
            	else
                {
					data += $"[{mult * i++},{item}]"; //last row
					break;
                }

            }


            return data;
        }

		public static string RowsFor<TModel>(this IHtmlHelper<TModel> htmlHelper, Func<TModel, List<List<double>>> getDataLists, int sampleCutOff=0)
        {
            return RowsFor(htmlHelper, getDataLists, x => 1,  sampleCutOff);
        }

		public static string RowsFor<TModel>(this IHtmlHelper<TModel> htmlHelper, Func<TModel, List<List<double>>> getDataLists, Func<TModel, double> XMultiplier, int sampleCutOff=0)
		{
			//myfunc will be in the form: model => model.InputSignalSeries
			var model = htmlHelper.ViewData.Model;
			List<List<double>> dataLists = getDataLists(model);
			double mult = XMultiplier(model);
			
            string data = "";
            var elementCount = sampleCutOff == 0 ? dataLists[0].Count : sampleCutOff;
			for (var i = 0; i < elementCount; i++) //i is lists element - use the count of the first list
			{
				var rowitem = (i * mult).ToString(); //x-axis value
				for (var j = 0; j < dataLists.Count; j++) //j is the list number
				{
					rowitem += $",{dataLists[j][i]}";
				}
                if (i < elementCount-1)
                {
                    data += $"[{rowitem}],";
                }
                else
                {
                    data += $"[{rowitem}]"; //last row
                    break;
				}
					
			}

			return data;
		}



        public static HtmlString MyHelper<TModel,TResult>(this IHtmlHelper<TModel> htmlHelper, Func<TModel,TResult> getThing)
        {
            //getProperty will be in the form: model => model.Foo
            var model = htmlHelper.ViewData.Model;
            var thing = getThing(model);

            var thingDone = new HtmlString(String.Empty);
			//do something with the thing

			return thingDone;
        }

    }
}
