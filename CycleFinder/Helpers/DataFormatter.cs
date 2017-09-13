using System;
using System.Collections.Generic;

namespace CycleFinder.Helpers
{
    public static class GoogleChartDataFormatter
    {
        public static string convertList(List<double> doubleList)
        {
            string data = "";
            var i = 0;
            foreach (var item in doubleList)
            {
                if (i < doubleList.Count)
                    data += $"[{i++},{item}],";
                else
                    data += $"[{i++},{item}]";
            }

            return data;
        }
    }
}
