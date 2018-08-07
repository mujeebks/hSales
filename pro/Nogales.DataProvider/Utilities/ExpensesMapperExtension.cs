using Nogales.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.DataProvider.Utilities
{
    public static class ExpensesMapperExtension
    {
        public static IEnumerable<ExpensesCategoryChartBM>
                        GetExpensesFromCategory(this List<ExpensesDataMapper> listTotalCasesSold
                                                    , Expression<Func<ExpensesDataMapper, bool>> predicate
                                                    , string currentName
                                                    , string previousName)
        {
            return listTotalCasesSold
                           .Where(predicate.Compile())
                           .GroupBy(x => x.Comodity)
                           .Select(y => new ExpensesCategoryChartBM
                           {
                               Category = y.Key,
                               Column1 = currentName,
                               Column2 = previousName,
                               Val1 = (y.Sum(t => t.CurrentSold) ?? 0).ToRoundTwoDigits(),
                               Val2 = (y.Sum(t => t.PreviousSold) ?? 0).ToRoundTwoDigits(),
                               //Color1 =ChartColorBM.Colors[6],
                               //Color2 =ChartColorBM.Colors[7],
                               Color1 = y.Key == "Produce" ? ChartColorBM.Colors[8] : ChartColorBM.Colors[6],
                               Color2 = y.Key == "Produce" ? ChartColorBM.Colors[9] : ChartColorBM.Colors[7],
                               SubData = y.OrderByDescending(s => s.CurrentSold)
                                        .Take(5)
                                        .Select((s, idx) => new ExpensesCategoryChartBM
                                        {
                                            Category = s.SalesPerson,
                                            Column1 = currentName.Substring(0, 3),
                                            Column2 = previousName.Substring(0, 3),
                                            Val1 = (s.CurrentSold ?? 0).ToRoundTwoDigits(),
                                            Val2 = (s.PreviousSold ?? 0).ToRoundTwoDigits(),
                                            Color1 = ChartColorBM.Colors[10],
                                            Color2 = ChartColorBM.Colors[11],
                                            //Color1 = ChartColorBM.Colors[idx],
                                            //Color2 = ChartColorBM.Colors.Reverse().ElementAt(idx),
                                        })
                                        .ToList()
                           }).ToList();
        }

        public static string currentName { get; set; }

        public static string previousName { get; set; }
    }
}
