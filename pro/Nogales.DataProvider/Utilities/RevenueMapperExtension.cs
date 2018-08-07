using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nogales.BusinessModel;
using System.Linq.Expressions;
using System.Data;

namespace Nogales.DataProvider.Utilities
{
    public static class RevenueMapperExtension
    {

        public static IEnumerable<RevenueCategoryChartBM>
                            GetRevenueFromCategory(this List<RevenueMapper> listRevenueData
                                                        , Expression<Func<RevenueMapper, bool>> predicate
                                                        , string currentName
                                                        , string previousName)
        {
            var result =

            listRevenueData
                           .Where(predicate.Compile())
                           .GroupBy(x => x.Comodity)
                           .Select(y => new RevenueCategoryChartBM
                           {
                               Category = y.Key,
                               Column1 = currentName,
                               Column2 = previousName,
                               Val1 = (y.Sum(t => t.CurrentValue) ?? 0).ToRoundTwoDigits(),
                               Val2 = (y.Sum(t => t.PreviousValue) ?? 0).ToRoundTwoDigits(),
                               //Color1 = ChartColorBM.Colors[18],
                               //Color2 = ChartColorBM.Colors[19],
                               Color1 = y.Key == "Produce" ? ChartColorBM.Colors[8] : ChartColorBM.Colors[6],
                               Color2 = y.Key == "Produce" ? ChartColorBM.Colors[9] : ChartColorBM.Colors[7],
                               SubData = new RenvenueSalesPesonBM
                               {
                                   Top = y.OrderByDescending(s => s.CurrentValue)
                                        .Take(5)
                                        .Select((s, idx) => new RevenueCategoryChartBM
                                        {
                                            Category = s.SalesPerson,
                                            Column1 = currentName,
                                            Column2 = previousName,
                                            Val1 = (s.CurrentValue ?? 0).ToRoundTwoDigits(),
                                            Val2 = (s.PreviousValue ?? 0).ToRoundTwoDigits(),
                                            Color1 = ChartColorBM.Colors[10],
                                            Color2 = ChartColorBM.Colors[11],
                                            //Color1 = ChartColorBM.Colors[idx],
                                            //Color2 = ChartColorBM.Colors.Reverse().ElementAt(idx),
                                        })
                                        .ToList(),
                                   Bottom = y.OrderBy(s => s.CurrentValue)
                                               .Take(5)
                                               .Select((s, idx) => new RevenueCategoryChartBM
                                               {
                                                   Category = s.SalesPerson,
                                                   Column1 = currentName,
                                                   Column2 = previousName,
                                                   Val1 = (s.CurrentValue ?? 0).ToRoundTwoDigits(),
                                                   Val2 = (s.PreviousValue ?? 0).ToRoundTwoDigits(),
                                                   Color1 = ChartColorBM.Colors[10],
                                                   Color2 = ChartColorBM.Colors[11],
                                                   //Color1 = ChartColorBM.Colors[idx],
                                                   //Color2 = ChartColorBM.Colors.Reverse().ElementAt(idx),
                                               })
                                               .ToList(),
                               }


                           }).ToList();

            return result;
        }

        public static IEnumerable<RevenueClusteredBarChartBM> GetRevenueForTotal
                                                        (this List<RevenueMapper> TotalListRevenueData
                                                        , Expression<Func<RevenueMapper, bool>> predicate
                                                        , string filterType
                                                        , DateTime filterDate
                                                        )
        {

            int currentYear = DateTime.Now.Year;
            var result = TotalListRevenueData.Where(predicate.Compile())
                                        .GroupBy(x => x.Year)
                                        .OrderByDescending(x => x.Key)
                                        .Select(x => new RevenueClusteredBarChartBM
                                        {
                                            Category = x.Key.ToString(),
                                            Column1 = filterDate.ToColumnLabelName(filterType, x.Key, false),
                                            Column2 = filterDate.ToColumnLabelName(filterType, x.Key),
                                            Val1 = (x.Sum(t => t.CurrentValue) ?? 0).ToRoundTwoDigits(),
                                            Val2 = (x.Sum(t => t.PreviousValue) ?? 0).ToRoundTwoDigits(),                                           
                                            //Color1 = (x.Key == currentYear.ToString() ? x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2] : x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0]),
                                            //Color2 = (x.Key == currentYear.ToString() ? x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2] : x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0]),
                                            Color1 = (filterType == Constants.ByMonth ? (
                                                                                        (x.Key == currentYear.ToString() ? x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2]
                                                                                         : x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0])
                                                                                     )
                                                                                     :
                                                                                     (
                                                                                        x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2]
                                                                                     )),
                                            Color2 = (filterType == Constants.ByMonth ? (
                                                                                        (x.Key == currentYear.ToString() ? x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2]
                                                                                       : x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0])
                                                                                      )
                                                                                      :
                                                                                      (
                                                                                        x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0]
                                                                                      )),
                                            SubData = x.GroupBy(y => y.Category)
                                                        .OrderByDescending(y => y.Sum(z => z.CurrentValue.Value))
                                                        .Select(c => new RevenueClusteredBarChartBM
                                                        {
                                                            Category = c.Key,
                                                            Column1 = filterDate.ToColumnLabelName(filterType, x.Key, false),
                                                            Column2 = filterDate.ToColumnLabelName(filterType, x.Key),
                                                            Val1 = (c.Sum(t => t.CurrentValue) ?? 0).ToRoundTwoDigits(),
                                                            Val2 = (c.Sum(t => t.PreviousValue) ?? 0).ToRoundTwoDigits(),
                                                            //Color1 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[2] : ChartColorBM.Colors[0],
                                                            //Color2 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[3] : ChartColorBM.Colors[1],
                                                            Color1 = x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                                                            Color2 = x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0], 
                                                            SubData = c.GroupBy(z => z.SalesPerson)
                                                                        .OrderByDescending(y => y.Sum(z => z.CurrentValue))
                                                                        .Take(10)
                                                                        .Select(z => new RevenueClusteredBarChartBM
                                                                        {
                                                                            Category = z.Key,
                                                                            Column1 = filterDate.ToColumnLabelName(filterType, x.Key, false),
                                                                            Column2 = filterDate.ToColumnLabelName(filterType, x.Key),
                                                                            Val1 = (z.Sum(t => t.CurrentValue) ?? 0).ToRoundTwoDigits(),
                                                                            Val2 = (z.Sum(t => t.PreviousValue) ?? 0).ToRoundTwoDigits(),
                                                                            //Color1 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[2] : ChartColorBM.Colors[0],
                                                                            //Color2 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[3] : ChartColorBM.Colors[1],
                                                                            Color1 = x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                                                                            Color2 = x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0], 
                                                                            Code = z.Select(p => p.SalesPersonCode).First()

                                                                        }).ToList()

                                                        }).ToList()

                                        }).ToList();
            return result;
        }

        public static IEnumerable<RevenueClusteredBarChartBM> GetRevenueFromComodityAndCategory
                                                        (
                                                        this List<RevenueMapper> TotalListRevenueData
                                                        , Expression<Func<RevenueMapper,bool>> predicate
                                                        , string filterType
                                                        , DateTime filterDate
                                                        )
                                                            
        {
            int currentYear=DateTime.Now.Year;
            var result=TotalListRevenueData.Where(predicate.Compile())
                                            .GroupBy(x=>x.Year)
                                            .OrderByDescending(x=>x.Key)
                                            .Select(x => new RevenueClusteredBarChartBM
                                            {
                                                Category=x.Key.ToString(),
                                                Column1 = filterDate.ToColumnLabelName(filterType, x.Key, false),
                                                Column2 = filterDate.ToColumnLabelName(filterType, x.Key),
                                                Val1 = (x.Sum(t=>t.CurrentValue)??0).ToRoundTwoDigits(),
                                                Val2=(x.Sum(t=>t.PreviousValue)??0).ToRoundTwoDigits(),
                                                //Color1 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[2] : ChartColorBM.Colors[0],
                                                //Color2 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[3] : ChartColorBM.Colors[1],
                                                //Color1 = (x.Key == currentYear.ToString() ? x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2] : x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0]),
                                                //Color2 = (x.Key == currentYear.ToString() ? x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2] : x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0]),
                                                Color1 = x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                                                Color2 = x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0],
                                                SubData=x.GroupBy(y=>y.SalesPerson)
                                                            .OrderByDescending(y=>y.Sum(z=>z.CurrentValue))
                                                            .Take(5)
                                                            .Select(y => new RevenueClusteredBarChartBM
                                                            {
                                                                Category=y.Key.ToString(),
                                                                Column1 = filterDate.ToColumnLabelName(filterType, x.Key, false),
                                                                Column2 = filterDate.ToColumnLabelName(filterType, x.Key),
                                                                Val1 = (x.Sum(t=>t.CurrentValue)??0).ToRoundTwoDigits(),
                                                                Val2=(x.Sum(t=>t.PreviousValue)??0).ToRoundTwoDigits(),
                                                                //Color1 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[2] : ChartColorBM.Colors[0],
                                                                //Color2 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[3] : ChartColorBM.Colors[1],
                                                                Color1 = x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                                                                Color2 = x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0], 
                                                            }).ToList()

                                            }).ToList();
            return result;
        }
        

    }
}
