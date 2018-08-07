using Nogales.BusinessModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.DataProvider.Utilities
{
    /// <summary>
    /// Mapper class that maps the Cases sold data into Category Cases Chart BusinessModel
    /// </summary>
    public static class CasesSoldMapperExtension
    {
        public static IEnumerable<CasesSoldCategoryChartBM>
                        GetCasesFromCategory(this List<CasesSoldMapper> listTotalCasesSold
                                                    , Expression<Func<CasesSoldMapper, bool>> predicate
                                                    , string currentName
                                                    , string previousName)
        {
            return listTotalCasesSold
                           .Where(predicate.Compile())
                           .GroupBy(x => x.Comodity)
                           .Select(y => new CasesSoldCategoryChartBM
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
                                        .Select((s, idx) => new CasesSoldCategoryChartBM
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


        public static IEnumerable<ClusteredBarChartBM>
                       GetCasesFromComodityAndCategory(this List<CasesSoldMapper> listTotalCasesSold
                                                   , Expression<Func<CasesSoldMapper, bool>> predicate
                                                   , string filterType
                                                   , DateTime filterDate
                                                  )
        {


            int currentYear = DateTime.Now.Year;
            var result = listTotalCasesSold.Where(predicate.Compile())
                        .GroupBy(x => x.Year)
                //.OrderBy(x => x.Key)
                        .OrderByDescending(x => x.Key)
                        .Select((x,idx) => new ClusteredBarChartBM
                        {
                            Category = x.Key.ToString(),
                            Column1 = filterDate.ToColumnLabelName(filterType, x.Key, false),
                            Column2 = filterDate.ToColumnLabelName(filterType, x.Key),
                            Val1 = (x.Sum(t => t.CurrentSold) ?? 0).ToRoundTwoDigits(),
                            Val2 = (x.Sum(t => t.PreviousSold) ?? 0).ToRoundTwoDigits(),
                            //Color1 = x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                            //Color2 = x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0],
                            Color1 = x.First().Comodity == "Grocery" ? BarColumnChartDistinctColors.Colors[idx].PrimaryGrocery : BarColumnChartDistinctColors.Colors[idx].PrimaryProduce,
                            Color2 = x.First().Comodity == "Grocery" ? BarColumnChartDistinctColors.Colors[idx].SecondaryGrocery : BarColumnChartDistinctColors.Colors[idx].SecondaryProduce,

                            SubData = x.GroupBy(y => y.SalesPerson)
                                    .OrderByDescending(y => y.Sum(z => z.CurrentSold.Value))
                                    .Take(5)
                                    .Select(y => new ClusteredBarChartBM
                                    {
                                        Category = y.Key,
                                        Column1 = filterDate.ToColumnLabelName(filterType, x.Key, false),
                                        Column2 = filterDate.ToColumnLabelName(filterType, x.Key),
                                        Val1 = (y.Sum(t => t.CurrentSold) ?? 0).ToRoundTwoDigits(),
                                        Val2 = (y.Sum(t => t.PreviousSold) ?? 0).ToRoundTwoDigits(),
                                        //Color1 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[2] : ChartColorBM.Colors[0],
                                        //Color2 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[3] : ChartColorBM.Colors[1],
                                        Color1 = x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                                        Color2 = x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0],    
                                    }).ToList()
                        }).ToList();
            return result;

        }



        public static IEnumerable<ClusteredBarChartBM> GetCasesSoldForTotal
                                                        (this List<CasesSoldMapper> listTotalCasesSold
                                                            , Expression<Func<CasesSoldMapper, bool>> predicate
                                                            , string filterType
                                                            , DateTime filterDate
                                                          )
        {

            int currentYear = DateTime.Now.Year;
            var result = listTotalCasesSold.Where(predicate.Compile())
                                            .GroupBy(x => x.Year)
                                            .OrderByDescending(x => x.Key)
                                        .Select(x => new ClusteredBarChartBM
                                        {
                                            Category = x.Key.ToString(),
                                            Column1 = filterDate.ToColumnLabelName(filterType, x.Key, false),
                                            Column2 = filterDate.ToColumnLabelName(filterType, x.Key),
                                            Val1 = (x.Sum(t => t.CurrentSold) ?? 0).ToRoundTwoDigits(),
                                            Val2 = (x.Sum(t => t.PreviousSold) ?? 0).ToRoundTwoDigits(),
                                            Color1 =(filterType==Constants.ByMonth? (
                                                                                        (x.Key == currentYear.ToString() ? x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2] 
                                                                                         :x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0])
                                                                                     )
                                                                                     :
                                                                                     (
                                                                                        x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3]: ChartColorBM.Colors[2]
                                                                                     )),
                                            Color2 = (filterType==Constants.ByMonth? (
                                                                                        (x.Key == currentYear.ToString() ? x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2] 
                                                                                       :x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0]) 
                                                                                      )
                                                                                      :
                                                                                      (
                                                                                        x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1]: ChartColorBM.Colors[0]
                                                                                      )),
                                            SubData = x.GroupBy(y => y.Category)
                                                        .OrderByDescending(y => y.Sum(z => z.CurrentSold.Value))
                                                        .Select(c => new ClusteredBarChartBM
                                                        {
                                                            Category = c.Key,
                                                            Column1 = filterDate.ToColumnLabelName(filterType, x.Key, false),
                                                            Column2 = filterDate.ToColumnLabelName(filterType, x.Key),
                                                            Val1 = (c.Sum(t => t.CurrentSold) ?? 0).ToRoundTwoDigits(),
                                                            Val2 = (c.Sum(t => t.PreviousSold) ?? 0).ToRoundTwoDigits(),
                                                            //Color1 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[2] : ChartColorBM.Colors[0],
                                                            //Color2 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[3] : ChartColorBM.Colors[1],
                                                            Color1 =  x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3]: ChartColorBM.Colors[2],  
                                                            Color2 =  x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0],                                                                                                      
                                                            SubData = c.GroupBy(e => e.SalesPerson)
                                                                        .OrderByDescending(e => e.Sum(z => z.CurrentSold.Value))
                                                                        .Take(10)
                                                                        .Select(s => new ClusteredBarChartBM
                                                                        {
                                                                            Category = s.Key,
                                                                            Column1 = filterDate.ToColumnLabelName(filterType, x.Key, false),
                                                                            Column2 = filterDate.ToColumnLabelName(filterType, x.Key),
                                                                            Val1 = (s.Sum(t => t.CurrentSold) ?? 0).ToRoundTwoDigits(),
                                                                            Val2 = (s.Sum(t => t.PreviousSold) ?? 0).ToRoundTwoDigits(),
                                                                            //Color1 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[2] : ChartColorBM.Colors[0],
                                                                            //Color2 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[3] : ChartColorBM.Colors[1],
                                                                            Color1 = x.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                                                                            Color2 = x.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0],
                                                                            //Code = s.Select (p => p.SalesPersonCode).First(), //sales person code                                                                     

                                                                            Code=s.Select(p=>(!string.IsNullOrEmpty(p.SalesPerson)?p.SalesPerson.TrimEnd():string.Empty)).First(),
                                                                            SalesPersonDescription =
                                                                                string.Join(",", s.Select(p =>
                                                                                                            ((!string.IsNullOrEmpty(p.SalesPersonDescription)
                                                                                                                ? p.SalesPersonDescription.Contains(",")
                                                                                                                    ? p.SalesPersonDescription.Split(',').LastOrDefault().TrimEnd() + " " + p.SalesPersonDescription.Substring(0, 1).TrimEnd()
                                                                                                                        : p.SalesPersonDescription.TrimEnd()
                                                                                                                : string.Empty)) + "" +
                                                                                                                (!string.IsNullOrEmpty(p.SalesPersonCode)
                                                                                                                    ? string.Format(" ({0})", p.SalesPersonCode.Trim())
                                                                                                                        : string.Empty))
                                                                                                .Distinct())
                                                                        }).ToList()
                                                        }).ToList(),
                                        }).ToList();

            return result;

        }

        public static IEnumerable<ClusteredBarChartBM> GetCasesSoldForDashboardTotal
                                                (this List<CasesSoldMapper> listTotalCasesSold
                                                    , Expression<Func<CasesSoldMapper, bool>> predicate

                                                  )
        {
            int currentYear = DateTime.Now.Year;
            var result = listTotalCasesSold.Where(predicate.Compile())
                                                                       .GroupBy(x => x.Year)
                                                                       .OrderByDescending(x => x.Key)
                                        .Select(x => new ClusteredBarChartBM
                                        {
                                            Category = x.Key.ToString(),
                                            Column1 = x.Select(o => o.Date.ToString("yyyy-MM-01")).First().ToString(),
                                            Column2 = x.Select(o => o.Date.AddMonths(-1).ToString("yyyy-MM-01")).First().ToString(),
                                            Val1 = (x.Sum(t => t.CurrentSold) ?? 0).ToRoundTwoDigits(),
                                            Val2 = (x.Sum(t => t.PreviousSold) ?? 0).ToRoundTwoDigits(),
                                            Color1 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[2] : ChartColorBM.Colors[0],
                                            Color2 = x.Key == currentYear.ToString() ? ChartColorBM.Colors[3] : ChartColorBM.Colors[1],
                                        }).ToList();

            return result;
        }


        public static string currentName { get; set; }

        public static string previousName { get; set; }
    }
}
