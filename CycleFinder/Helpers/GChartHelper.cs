using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Collections.Generic;

namespace CycleFinder.Helpers
{
    public static class GChartHelper
    {
        public static string RowsFor<TModel>(this IHtmlHelper<TModel> htmlHelper, Func<TModel,List<double>> getDataList)
        {
            //myfunc will be in the form: model => model.InputSignalSeries
            var model = htmlHelper.ViewData.Model;
                     List<double> dataList = getDataList(model);

            string data = "";
            var i = 0;
            foreach (var item in dataList)
            {
            	if (i < dataList.Count)
            		data += $"[{i++},{item}],";
            	else
            		data += $"[{i++},{item}]"; //last row
            }


            return data;
        }

		public static string RowsFor<TModel>(this IHtmlHelper<TModel> htmlHelper, Func<TModel, List<List<double>>> getDataList)
		{
			//myfunc will be in the form: model => model.InputSignalSeries
			var model = htmlHelper.ViewData.Model;
			List<List<double>> dataLists = getDataList(model);

			string data = "";
			var elementCount = dataLists[0].Count;
			for (var i = 0; i < elementCount; i++) //i is lists element - use the count of the first list
			{
				var rowitem = i.ToString(); //x-axis value
				for (var j = 0; j < dataLists.Count; j++) //j is the list number
				{
					rowitem += $",{dataLists[j][i]}";
				}
				if (i < elementCount)
					data += $"[{rowitem}],";
				else
					data += $"[{rowitem}]"; //last row
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
