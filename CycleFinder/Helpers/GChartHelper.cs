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



        public static HtmlString MyHelper<TModel,TResult>(this IHtmlHelper<TModel> htmlHelper, Func<TModel,TResult> getProperty)
        {
            //getProperty will be in the form: model => model.Foo
            var model = htmlHelper.ViewData.Model;
            var property = getProperty(model);

            var thingDone = new HtmlString(String.Empty);
			//do something with property 

			return thingDone;
        }

    }
}
