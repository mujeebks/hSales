using Nogales.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.DataProvider.Utilities
{
    public static class CasesSoldRevenueMapperExtension
    {


        public static GenericColumnChartCategoryBM
                    GetCasesFromLocation(this List<MapCasesSoldRevenue> list
                                                , Expression<Func<MapCasesSoldRevenue, bool>> predicate, GlobalFilter filter
                                                 , string locType
                                               )
        {
            GenericColumnChartCategoryBM model = new GenericColumnChartCategoryBM();
            var predicatedList = list.Where(predicate.Compile()).ToList();

            model.All = new List<GenericColumnChartBM> { new GenericColumnChartBM
            {
                GroupName = locType,
                Label1 = filter.Periods.Current.Label,
                Label2 = filter.Periods.Current.Label,
                cValue1 = predicatedList.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesCurrentMonth),
                cValue2 = predicatedList.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesCurrentMonth),
                Period1 = filter.Periods.Current.End,
                Period2=filter.Periods.Current.End,
                Color1 = ChartColorBM.GrocerryCurrent,
                Color2 = ChartColorBM.ProduceCurrent,


                Label3 = filter.Periods.Historical.Label,
                 Label4 = filter.Periods.Historical.Label,
                cValue3 = predicatedList.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesPreviousYear),
                cValue4 = predicatedList.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesPreviousYear),
                Period3 =  filter.Periods.Historical.End,
                Period4 =  filter.Periods.Historical.End,
                Color3= ChartColorBM.GrocerryPrevious,
                Color4 = ChartColorBM.ProducePrevious,

                 Label5 = filter.Periods.Prior.Label,
                 Label6 = filter.Periods.Prior.Label,
                cValue5 = predicatedList.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesPreviousMonth),
                cValue6 = predicatedList.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesPreviousMonth),
                Period5 =  filter.Periods.Prior.End,
                 Period6 =  filter.Periods.Prior.End,
                Color5 = ChartColorBM.GrocerryCurrent,
                Color6 = ChartColorBM.ProduceCurrent,

                SalesPerson1 = "",
                SalesPerson2 = "",

                SubData = predicatedList
                .OrderByDescending(a => a.CasesCurrentMonth)
                .GroupBy(x => x.Category)
                .Select((thir,thirIdx) => new GenericColumnChartBM
                {
                    GroupName = thir.Key,
                    Label1 =  filter.Periods.Current.Label,
                    Label2 =  filter.Periods.Current.Label,
                    cValue1 = thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesCurrentMonth),
                    cValue2 = thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesCurrentMonth),
                    Period1 =  filter.Periods.Current.End,
                    Color1 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,// ChartColorBM.GrocerryCurrent,
                    Color2 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,//.ProduceCurrent,

                    Label3 =  filter.Periods.Historical.Label,
                    Label4 =  filter.Periods.Historical.Label,
                    cValue3 = thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesPreviousYear),
                    cValue4 = thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesPreviousYear),
                    Period2 = filter.Periods.Historical.End,
                    Color3 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryGrocery,//ChartColorBM.GrocerryPrevious,
                    Color4 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryProduce,//ChartColorBM.ProducePrevious,

                     Label5 =  filter.Periods.Prior.Label,
                    Label6 = filter.Periods.Prior.Label,
                    cValue5 = thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesPreviousMonth),
                    cValue6 = thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesPreviousMonth),
                    Period3 =  filter.Periods.Prior.End,
                    Color5 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                    Color6 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,

                    SalesPerson1 = "",
                    SalesPerson2 = "",
                }).ToList()

            } };
            //model.Grocery = new List<GenericColumnChartBM> { new GenericColumnChartBM
            //{
            //    GroupName = locType,
            //    Label1 =  filter.Periods.Current.Label,
            //    cValue1 = predicatedList.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesCurrentMonth),
            //    Color1 = ChartColorBM.GrocerryCurrent,

            //     Label2 =  filter.Periods.Historical.Label,
            //    cValue2 = predicatedList.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesPreviousYear),
            //    Color2 = ChartColorBM.GrocerryPrevious,


            //    Label3 =  filter.Periods.Prior.Label,
            //    cValue3 = predicatedList.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesPreviousMonth),
            //    Color3 = ChartColorBM.GrocerryCurrent,



            //    SubData = predicatedList.Where(d => d.Comodity.ToLower() == "grocery").OrderByDescending(a => a.CasesCurrentMonth).GroupBy(x => x.Category).Select(thir => new GenericColumnChartBM
            //    {
            //        GroupName = thir.Key,

            //        Label1 = filter.Periods.Current.Label,
            //        cValue1 = thir.Sum(t => t.CasesCurrentMonth),
            //        Color1 = ChartColorBM.GrocerryCurrent,

            //                          Label2 =  filter.Periods.Historical.Label,
            //        cValue2 = thir.Sum(t => t.CasesPreviousYear),
            //        Color2 = ChartColorBM.GrocerryPrevious,

            //          Label3 =  filter.Periods.Prior.Label,
            //        cValue3 = thir.Sum(t => t.CasesPreviousMonth),
            //        Color3 = ChartColorBM.GrocerryCurrent,
            //    }).ToList()

            //}};

            //model.Produce = new List<GenericColumnChartBM> {new GenericColumnChartBM
            //{
            //    GroupName = locType,
            //    Label1 = filter.Periods.Current.Label,
            //    cValue1 = predicatedList.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesCurrentMonth),
            //    Color1 = ChartColorBM.ProduceCurrent,


            //    Label2 =  filter.Periods.Historical.Label,
            //    cValue2 = predicatedList.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesPreviousYear),
            //    Color2 = ChartColorBM.ProducePrevious,

            //      Label3 =  filter.Periods.Prior.Label,
            //    cValue3 = predicatedList.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesPreviousMonth),
            //    Color3 = ChartColorBM.ProduceCurrent,


            //    SubData = predicatedList.Where(d => d.Comodity.ToLower() == "produce").OrderByDescending(a => a.CasesCurrentMonth).GroupBy(x => x.Category).Select(thir => new GenericColumnChartBM
            //    {
            //        GroupName = thir.Key,

            //        Label1 =  filter.Periods.Current.Label,
            //        cValue1 = thir.Sum(t => t.CasesCurrentMonth),
            //        Color1 = ChartColorBM.ProduceCurrent,



            //        Label2 =  filter.Periods.Historical.Label,
            //        cValue2 = thir.Sum(t => t.CasesPreviousYear),
            //        Color2 = ChartColorBM.ProducePrevious,

            //         Label3 = filter.Periods.Prior.Label,
            //        cValue3 = thir.Sum(t => t.CasesPreviousMonth),
            //        Color3 = ChartColorBM.ProduceCurrent,
            //    }).ToList()

            //}};

            return model;


        }

        public static GenericColumnChartCategoryBM
                   GetCasesFromLocationTemp(this List<MapCasesSoldRevenue> list
                                               , Expression<Func<MapCasesSoldRevenue, bool>> predicate, DateTime date
                                                , string locType
                                              )
        {
            GenericColumnChartCategoryBM model = new GenericColumnChartCategoryBM();
            var predicatedList = list.Where(predicate.Compile()).ToList();

            model.All = new List<GenericColumnChartBM> { new GenericColumnChartBM
            {
                GroupName = locType,
                Label1 = date.ToString("MMM-yy"),
                Label2 = date.ToString("MMM-yy"),
                cValue1 = predicatedList.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesCurrentMonth),
                cValue2 = predicatedList.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesCurrentMonth),
                Period1 = date,
                Period2=date,
                Color1 = ChartColorBM.GrocerryCurrent,
                Color2 = ChartColorBM.ProduceCurrent,


                Label3 = date.AddYears(-1).ToString("MMM-yy"),
                 Label4 = date.AddYears(-1).ToString("MMM-yy"),
                cValue3 = predicatedList.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesPreviousYear),
                cValue4 = predicatedList.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesPreviousYear),
                Period3 = date.AddYears(-1),
                Period4 = date.AddYears(-1),
                Color3= ChartColorBM.GrocerryPrevious,
                Color4 = ChartColorBM.ProducePrevious,

                 Label5 = date.AddMonths(-1).ToString("MMM-yy"),
                 Label6 = date.AddMonths(-1).ToString("MMM-yy"),
                cValue5 = predicatedList.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesPreviousMonth),
                cValue6 = predicatedList.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesPreviousMonth),
                Period5 = date.AddMonths(-1),
                 Period6 = date.AddMonths(-1),
                Color5 = ChartColorBM.GrocerryCurrent,
                Color6 = ChartColorBM.ProduceCurrent,

                SalesPerson1 = "",
                SalesPerson2 = "",

                SubData = predicatedList.OrderByDescending(a => a.CasesCurrentMonth).GroupBy(x => x.Category).Select(thir => new GenericColumnChartBM
                {
                    GroupName = thir.Key,
                    Label1 = date.ToString("MMM-yy"),
                    Label2 = date.ToString("MMM-yy"),
                    cValue1 = thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesCurrentMonth),
                    cValue2 = thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesCurrentMonth),
                    Period1 = date,
                    Color1 = ChartColorBM.GrocerryCurrent,
                    Color2 = ChartColorBM.ProduceCurrent,

                    Label3 = date.AddYears(-1).ToString("MMM-yy"),
                    Label4 = date.AddYears(-1).ToString("MMM-yy"),
                    cValue3 = thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesPreviousYear),
                    cValue4 = thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesPreviousYear),
                    Period2 = date.AddYears(-1),
                    Color3 = ChartColorBM.GrocerryPrevious,
                    Color4 = ChartColorBM.ProducePrevious,

                      Label5 = date.AddMonths(-1).ToString("MMM-yy"),
                    Label6 = date.AddMonths(-1).ToString("MMM-yy"),
                    cValue5 = thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesPreviousMonth),
                    cValue6 = thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesPreviousMonth),
                    Period3 = date.AddMonths(-1),
                    Color5 = ChartColorBM.GrocerryCurrent,
                    Color6 = ChartColorBM.ProduceCurrent,

                    SalesPerson1 = "",
                    SalesPerson2 = "",
                }).ToList()

            } };
            model.Grocery = new List<GenericColumnChartBM> { new GenericColumnChartBM
            {
                GroupName = locType,
                Label1 = date.ToString("MMM-yy"),
                cValue1 = predicatedList.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesCurrentMonth),
                Color1 = ChartColorBM.GrocerryCurrent,

                 Label2 = date.AddYears(-1).ToString("MMM-yy"),
                cValue2 = predicatedList.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesPreviousYear),
                Color2 = ChartColorBM.GrocerryPrevious,


                Label3 = date.AddMonths(-1).ToString("MMM-yy"),
                cValue3 = predicatedList.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesPreviousMonth),
                Color3 = ChartColorBM.GrocerryCurrent,



                SubData = predicatedList.Where(d => d.Comodity.ToLower() == "grocery").OrderByDescending(a => a.CasesCurrentMonth).GroupBy(x => x.Category).Select(thir => new GenericColumnChartBM
                {
                    GroupName = thir.Key,

                    Label1 = date.ToString("MMM-yy"),
                    cValue1 = thir.Sum(t => t.CasesCurrentMonth),
                    Color1 = ChartColorBM.GrocerryCurrent,

                                      Label2 = date.AddYears(-1).ToString("MMM-yy"),
                    cValue2 = thir.Sum(t => t.CasesPreviousYear),
                    Color2 = ChartColorBM.GrocerryPrevious,

                      Label3 = date.AddMonths(-1).ToString("MMM-yy"),
                    cValue3 = thir.Sum(t => t.CasesPreviousMonth),
                    Color3 = ChartColorBM.GrocerryCurrent,
                }).ToList()

            }};

            model.Produce = new List<GenericColumnChartBM> {new GenericColumnChartBM
            {
                GroupName = locType,
                Label1 = date.ToString("MMM-yy"),
                cValue1 = predicatedList.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesCurrentMonth),
                Color1 = ChartColorBM.ProduceCurrent,


                Label2 = date.AddYears(-1).ToString("MMM-yy"),
                cValue2 = predicatedList.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesPreviousYear),
                Color2 = ChartColorBM.ProducePrevious,

                  Label3 = date.AddMonths(-1).ToString("MMM-yy"),
                cValue3 = predicatedList.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesPreviousMonth),
                Color3 = ChartColorBM.ProduceCurrent,


                SubData = predicatedList.Where(d => d.Comodity.ToLower() == "produce").OrderByDescending(a => a.CasesCurrentMonth).GroupBy(x => x.Category).Select(thir => new GenericColumnChartBM
                {
                    GroupName = thir.Key,

                    Label1 = date.ToString("MMM-yy"),
                    cValue1 = thir.Sum(t => t.CasesCurrentMonth),
                    Color1 = ChartColorBM.ProduceCurrent,



                    Label2 = date.AddYears(-1).ToString("MMM-yy"),
                    cValue2 = thir.Sum(t => t.CasesPreviousYear),
                    Color2 = ChartColorBM.ProducePrevious,

                     Label3 = date.AddMonths(-1).ToString("MMM-yy"),
                    cValue3 = thir.Sum(t => t.CasesPreviousMonth),
                    Color3 = ChartColorBM.ProduceCurrent,
                }).ToList()

            }};

            return model;


        }

        public static IEnumerable<GenericColumnChartBM>
                     GetCasesAndRevenueFromSpecificCondition(this List<MapCasesSoldRevenue> list
                                                 , Expression<Func<MapCasesSoldRevenue, bool>> predicate, DateTime date
                                                  , int curMonth, string currGrpName, string prevGrpName, string compType, bool IsAllOthers = false
                                                )
        {



            var result = list.Where(predicate.Compile())
                        .GroupBy(x => x.MonthPart)
                        .OrderByDescending(x => x.Key)
                        .Select(pri => new GenericColumnChartBM
                        {
                            GroupName = pri.Key == curMonth ? currGrpName : prevGrpName,
                            Label1 = date.Year.ToString(),
                            cValue1 = (pri.Sum(t => t.CasesCurrentYear)),
                            rValue1 = (double)(pri.Sum(t => t.RevenueCurrentYear)).ToRoundTwoDecimalDigits(),
                            Period1 = pri.Key == curMonth ? date : date.AddMonths(-1),
                            Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                            SalesPerson1 = IsAllOthers ? string.Join(", ", list.Where(predicate.Compile())
                                                                        .Select(item => item.Category)
                                                                        .Distinct()
                                                                        .ToArray()) : "",

                            Label2 = (date.AddYears(-1).Year).ToString(),
                            cValue2 = (pri.Sum(t => t.CasesPreviousYear)),
                            rValue2 = (double)(pri.Sum(t => t.RevenuePreviousYear)).ToRoundTwoDecimalDigits(),
                            Period2 = pri.Key == curMonth ? date.AddYears(-1) : date.AddMonths(-1).AddYears(-1),
                            Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                            SalesPerson2 = IsAllOthers ? string.Join(", ", list.Where(predicate.Compile())
                                                                        .Select(item => item.Category)
                                                                        .Distinct()
                                                                        .ToArray()) : "",
                            SubData = pri.OrderByDescending(a => a.CasesCurrentYear).Take(10).GroupBy(x => x.SalesPersonCode).Select(thir => new GenericColumnChartBM
                            {
                                GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(thir.First().ActiveEmployee) ? thir.First().ActiveEmployee : thir.Key),
                                ActiveSalesPersonCode = thir.Key,
                                Label1 = string.Format("{0}-{1}", (pri.Key == curMonth ? currGrpName : prevGrpName).Substring(0, 3), date.Year.ToString().Substring(2, 2)),
                                cValue1 = (thir.Sum(t => t.CasesCurrentYear)),
                                rValue1 = (double)(thir.Sum(t => t.RevenueCurrentYear)).ToRoundTwoDecimalDigits(),
                                Period1 = pri.Key == curMonth ? date : date.AddMonths(-1),
                                Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                                SalesPerson1 = GetSalesManName(thir.First().CurrentYearEmployee),

                                Label2 = string.Format("{0}-{1}", (pri.Key == curMonth ? currGrpName : prevGrpName).Substring(0, 3), date.AddYears(-1).Year.ToString().Substring(2, 2)),
                                cValue2 = (thir.Sum(t => t.CasesPreviousYear)),
                                rValue2 = (double)(thir.Sum(t => t.RevenuePreviousYear)).ToRoundTwoDecimalDigits(),
                                Period2 = pri.Key == curMonth ? date.AddYears(-1) : date.AddMonths(-1).AddYears(-1),
                                Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                                SalesPerson2 = GetSalesManName(thir.First().PreviousYearEmployee),
                            }).ToList()

                        }).ToList();
            return result;


        }

        public static IEnumerable<GenericColumnChartBM>
                    GetCasesFromSpecificConditionTemp(this List<MapCasesSoldRevenue> list
                                                , Expression<Func<MapCasesSoldRevenue, bool>> predicate, DateTime date
                                                 , int curMonth, string currGrpName, string prevGrpName, string compType, bool IsAllOthers = false
                                               )
        {
            List<GenericColumnChartBM> result = new List<GenericColumnChartBM>();
            list = list.Where(predicate.Compile())
                        .OrderByDescending(x => x.MonthPart).ThenByDescending(d => d.CasesCurrentYear).ToList();


            var data = new GenericColumnChartBM
            {
                // SalesPerson1 = IsAllOthers ? string.Join(", ", list.Where(predicate.Compile()).Select(item => item.Category).Distinct().ToArray()) : "",
                GroupName = compType,
                Label1 = date.ToString("MMMM yy"),
                cValue1 = (list.Where(f => f.Comodity.ToLower() == compType.ToLower() && f.MonthPart == date.Month).Sum(t => t.CasesCurrentYear)),
                Period1 = date,
                Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                SalesPerson1 = "",

                Label2 = date.AddYears(-1).ToString("MMMM yy"),
                cValue2 = (list.Where(f => f.Comodity.ToLower() == compType.ToLower() && f.MonthPart == date.Month).Sum(t => t.CasesPreviousYear)),
                Period2 = date.AddYears(-1),
                Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                SalesPerson2 = "",

                Label3 = date.AddMonths(-1).ToString("MMMM yy"),
                cValue3 = (list.Where(f => f.Comodity.ToLower() == compType.ToLower() && f.MonthPart == date.AddMonths(-1).Month).Sum(t => t.CasesCurrentYear)),
                Period3 = date.AddMonths(-1),
                Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                SalesPerson3 = "",

                SubData = list.OrderByDescending(a => a.CasesCurrentYear)
                         .GroupBy(x => x.SalesPersonCode).Take(5).Select(thir => new GenericColumnChartBM
                         {
                             GroupName = GetActiveSalesManName(thir.First().ActiveEmployee != null ? thir.First().ActiveEmployee : thir.Key),
                             ActiveSalesPersonCode = thir.Key,
                             Label1 = string.Format("{0}-{1}", (currGrpName).Substring(0, 3), date.Year.ToString().Substring(2, 2)),
                             cValue1 = (thir.Where(d => d.Comodity.ToLower() == compType.ToLower() && d.MonthPart == date.Month).Sum(t => t.CasesCurrentYear)),
                             Period1 = date,
                             Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             SalesPerson1 = GetSalesManName(thir.First().CurrentYearEmployee != null ? thir.First().CurrentYearEmployee : ""),



                             Label2 = string.Format("{0}-{1}", (currGrpName).Substring(0, 3), date.Year.ToString().Substring(2, 2)),
                             cValue2 = (thir.Where(d => d.Comodity.ToLower() == compType.ToLower() && d.MonthPart == date.Month).Sum(t => t.CasesPreviousYear)),
                             Period2 = date.AddYears(-1),
                             Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                             SalesPerson2 = GetSalesManName(thir.First().PreviousYearEmployee != null ? thir.First().PreviousYearEmployee : ""),


                             Label3 = string.Format("{0}-{1}", (prevGrpName).Substring(0, 3), date.Year.ToString().Substring(2, 2)),
                             cValue3 = (thir.Where(d => d.Comodity.ToLower() == compType.ToLower() && d.MonthPart == date.AddMonths(-1).Month).Sum(t => t.CasesCurrentYear)),
                             Period3 = date.AddMonths(-1),
                             Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             SalesPerson3 = GetSalesManName(thir.First().CurrentYearEmployee != null ? thir.First().CurrentYearEmployee : ""),



                         }).ToList()
            };
            result.Add(data);

            return result;


        }

        public static IEnumerable<GenericColumnChartBM>
                   GetCasesFromSpecificCondition(this List<MapCasesSoldRevenueGlobal> list
                                               , Expression<Func<MapCasesSoldRevenueGlobal, bool>> predicate, GlobalFilter filter, string compType, bool IsAllOthers = false
                                              )
        {
            List<GenericColumnChartBM> result = new List<GenericColumnChartBM>();
            list = list.Where(predicate.Compile())
                        .OrderByDescending(d => d.CasesCurrent).ToList();


            var data = new GenericColumnChartBM
            {
                // SalesPerson1 = IsAllOthers ? string.Join(", ", list.Where(predicate.Compile()).Select(item => item.Category).Distinct().ToArray()) : "",
                GroupName = compType,
                Label1 = filter.Periods.Current.Label,
                cValue1 = (list.Where(f => f.Comodity.ToLower() == compType.ToLower()).Sum(t => t.CasesCurrent)),
                Period1 = filter.Periods.Current.End,
                Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                SalesPerson1 = "",

                Label2 = filter.Periods.Historical.Label,
                cValue2 = (list.Where(f => f.Comodity.ToLower() == compType.ToLower()).Sum(t => t.CasesHistorical)),
                Period2 = filter.Periods.Historical.End,
                Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                SalesPerson2 = "",

                Label3 = filter.Periods.Prior.Label,
                cValue3 = (list.Where(f => f.Comodity.ToLower() == compType.ToLower()).Sum(t => t.CasesPrior)),
                Period3 = filter.Periods.Prior.End,
                Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                SalesPerson3 = "",

                SubData = list.OrderByDescending(a => a.CasesCurrent)
                         .GroupBy(x => x.SalesPersonCode).Take(5).Select(thir => new GenericColumnChartBM
                         {
                             GroupName = GetActiveSalesManName(thir.First().ActiveEmployee != null ? thir.First().ActiveEmployee : thir.Key),
                             ActiveSalesPersonCode = thir.Key,
                             Label1 = filter.Periods.Current.Label,
                             cValue1 = (thir.Where(d => d.Comodity.ToLower() == compType.ToLower()).Sum(t => t.CasesCurrent)),
                             Period1 = filter.Periods.Current.End,
                             Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             SalesPerson1 = GetSalesManName(thir.First().CurrentEmployee != null ? thir.First().CurrentEmployee : ""),



                             Label2 = filter.Periods.Historical.Label,
                             cValue2 = (thir.Where(d => d.Comodity.ToLower() == compType.ToLower()).Sum(t => t.CasesHistorical)),
                             Period2 = filter.Periods.Historical.End,
                             Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                             SalesPerson2 = GetSalesManName(thir.First().HistoricalEmployee != null ? thir.First().HistoricalEmployee : ""),


                             Label3 = filter.Periods.Prior.Label,
                             cValue3 = (thir.Where(d => d.Comodity.ToLower() == compType.ToLower()).Sum(t => t.CasesPrior)),
                             Period3 = filter.Periods.Prior.End,
                             Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             SalesPerson3 = GetSalesManName(thir.First().PriorEmployee != null ? thir.First().PriorEmployee : ""),



                         }).ToList()
            };
            result.Add(data);

            return result;


        }

        public static IEnumerable<GenericColumnChartBM>
                    GetRevenueFromSpecificConditionTemp(this List<MapCasesSoldRevenue> list
                                                , Expression<Func<MapCasesSoldRevenue, bool>> predicate, DateTime date
                                                 , int curMonth, string currGrpName, string prevGrpName, string compType, bool IsAllOthers = false
                                               )
        {
            List<GenericColumnChartBM> result = new List<GenericColumnChartBM>();
            list = list.Where(predicate.Compile())
                        .OrderByDescending(x => x.MonthPart).ThenByDescending(d => d.RevenueCurrentYear).ToList();


            var data = new GenericColumnChartBM
            {
                // SalesPerson1 = IsAllOthers ? string.Join(", ", list.Where(predicate.Compile()).Select(item => item.Category).Distinct().ToArray()) : "",
                GroupName = compType,
                Label1 = date.ToString("MMMM yy"),
                rValue1 = (double)(list.Where(f => f.Comodity.ToLower() == compType && f.MonthPart == date.Month).Sum(t => t.RevenueCurrentYear)).ToRoundTwoDecimalDigits(),
                Period1 = date,
                Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                SalesPerson1 = "",

                Label2 = date.AddYears(-1).ToString("MMMM yy"),
                rValue2 = (double)(list.Where(f => f.Comodity.ToLower() == compType.ToLower() && f.MonthPart == date.Month).Sum(t => t.RevenuePreviousYear)).ToRoundTwoDecimalDigits(),
                Period2 = date.AddYears(-1),
                Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                SalesPerson2 = "",

                Label3 = date.AddMonths(-1).ToString("MMMM yy"),
                rValue3 = (double)(list.Where(f => f.Comodity.ToLower() == compType.ToLower() && f.MonthPart == date.AddMonths(-1).Month).Sum(t => t.RevenueCurrentYear)).ToRoundTwoDecimalDigits(),
                Period3 = date.AddMonths(-1),
                Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                SalesPerson3 = "",


                SubData = list.OrderByDescending(a => a.CasesCurrentYear)
                         .GroupBy(x => x.SalesPersonCode).Take(5).Select(thir => new GenericColumnChartBM
                         {
                             GroupName = GetActiveSalesManName(thir.First().ActiveEmployee != null ? thir.First().ActiveEmployee : thir.Key),
                             ActiveSalesPersonCode = thir.Key,
                             Label1 = string.Format("{0}-{1}", (currGrpName).Substring(0, 3), date.Year.ToString().Substring(2, 2)),
                             rValue1 = (double)(thir.Where(d => d.Comodity.ToLower() == compType.ToLower() && d.MonthPart == date.Month).Sum(t => t.RevenueCurrentYear)).ToRoundTwoDecimalDigits(),
                             Period1 = date,
                             Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             SalesPerson1 = GetSalesManName(thir.First().CurrentYearEmployee != null ? thir.First().CurrentYearEmployee : ""),



                             Label2 = string.Format("{0}-{1}", (currGrpName).Substring(0, 3), date.Year.ToString().Substring(2, 2)),
                             rValue2 = (double)(thir.Where(d => d.Comodity.ToLower() == compType.ToLower() && d.MonthPart == date.Month).Sum(t => t.RevenuePreviousYear)).ToRoundTwoDecimalDigits(),
                             Period2 = date.AddYears(-1),
                             Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                             SalesPerson2 = GetSalesManName(thir.First().PreviousYearEmployee != null ? thir.First().PreviousYearEmployee : ""),


                             Label3 = string.Format("{0}-{1}", (prevGrpName).Substring(0, 3), date.Year.ToString().Substring(2, 2)),
                             rValue3 = (double)(thir.Where(d => d.Comodity.ToLower() == compType.ToLower() && d.MonthPart == date.AddMonths(-1).Month).Sum(t => t.RevenueCurrentYear)).ToRoundTwoDecimalDigits(),
                             Period3 = date.AddMonths(-1),
                             Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             SalesPerson3 = GetSalesManName(thir.First().CurrentYearEmployee != null ? thir.First().CurrentYearEmployee : ""),


                         }).ToList()

            };
            result.Add(data);

            return result;


        }

        public static IEnumerable<GenericColumnChartBM>
                    GetRevenueFromSpecificCondition(this List<MapCasesSoldRevenueGlobal> list
                                                , Expression<Func<MapCasesSoldRevenueGlobal, bool>> predicate, GlobalFilter filter, string compType, bool IsAllOthers = false
                                               )
        {
            List<GenericColumnChartBM> result = new List<GenericColumnChartBM>();
            list = list.Where(predicate.Compile())
                        .OrderByDescending(d => d.RevenueCurrent).ToList();


            var data = new GenericColumnChartBM
            {
                // SalesPerson1 = IsAllOthers ? string.Join(", ", list.Where(predicate.Compile()).Select(item => item.Category).Distinct().ToArray()) : "",
                GroupName = compType,
                Label1 = filter.Periods.Current.Label,
                rValue1 = (double)(list.Where(f => f.Comodity.ToLower() == compType).Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                Period1 = filter.Periods.Current.End,
                Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                SalesPerson1 = "",

                Label2 = filter.Periods.Historical.Label,
                rValue2 = (double)(list.Where(f => f.Comodity.ToLower() == compType.ToLower()).Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                Period2 = filter.Periods.Historical.End,
                Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                SalesPerson2 = "",

                Label3 = filter.Periods.Prior.Label,
                rValue3 = (double)(list.Where(f => f.Comodity.ToLower() == compType.ToLower()).Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                Period3 = filter.Periods.Prior.End,
                Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                SalesPerson3 = "",


                SubData = list.OrderByDescending(a => a.RevenueCurrent)
                         .GroupBy(x => x.SalesPersonCode).Take(5).Select(thir => new GenericColumnChartBM
                         {
                             GroupName = GetActiveSalesManName(thir.First().ActiveEmployee != null ? thir.First().ActiveEmployee : thir.Key),
                             ActiveSalesPersonCode = thir.Key,
                             Label1 = filter.Periods.Current.Label,
                             rValue1 = (double)(thir.Where(d => d.Comodity.ToLower() == compType.ToLower()).Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                             Period1 = filter.Periods.Current.End,
                             Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             SalesPerson1 = GetSalesManName(thir.First().CurrentEmployee != null ? thir.First().CurrentEmployee : ""),



                             Label2 = filter.Periods.Historical.Label,
                             rValue2 = (double)(thir.Where(d => d.Comodity.ToLower() == compType.ToLower()).Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                             Period2 = filter.Periods.Historical.End,
                             Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                             SalesPerson2 = GetSalesManName(thir.First().HistoricalEmployee != null ? thir.First().HistoricalEmployee : ""),


                             Label3 = filter.Periods.Prior.Label,
                             rValue3 = (double)(thir.Where(d => d.Comodity.ToLower() == compType.ToLower()).Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                             Period3 = filter.Periods.Prior.End,
                             Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             SalesPerson3 = GetSalesManName(thir.First().PriorEmployee != null ? thir.First().PriorEmployee : ""),


                         }).ToList()

            };
            result.Add(data);

            return result;


        }




        public static IEnumerable<GenericColumnChartBM>
                 GetCasesAndRevenueFromSpecificConditionByYear(this List<MapCasesSoldRevenue> list
                                             , Expression<Func<MapCasesSoldRevenue, bool>> predicate, DateTime date
                                              , string compType, bool IsAllOthers = false
                                            )
        {

            List<GenericColumnChartBM> listResult = new List<GenericColumnChartBM>();

            var result = new GenericColumnChartBM
            {
                GroupName = date.Year.ToString(),
                Label1 = date.Year.ToString(),
                cValue1 = list.Where(predicate.Compile()).Sum(pri => pri.CasesCurrentYear),
                rValue1 = (double)(list.Where(predicate.Compile()).Sum(pri => pri.RevenueCurrentYear)).ToRoundTwoDecimalDigits(),
                Period1 = date,
                Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                SalesPerson1 = IsAllOthers ? string.Join(", ", list.Where(predicate.Compile())
                                                                        .Select(item => item.Category)
                                                                        .Distinct()
                                                                        .ToArray()) : "",

                Label2 = (date.AddYears(-1).Year).ToString(),
                cValue2 = list.Where(predicate.Compile()).Sum(pri => pri.CasesPreviousYear),
                rValue2 = (double)(list.Where(predicate.Compile()).Sum(pri => pri.RevenuePreviousYear)).ToRoundTwoDecimalDigits(),
                Period2 = date.AddYears(-1),
                Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                SalesPerson2 = IsAllOthers ? string.Join(", ", list.Where(predicate.Compile())
                                                                        .Select(item => item.Category)
                                                                        .Distinct()
                                                                        .ToArray()) : "",
                SubData = list.Where(predicate.Compile()).OrderByDescending(a => a.CasesCurrentYear).Take(10).GroupBy(x => x.SalesPersonCode).Select(thir => new GenericColumnChartBM
                {
                    GroupName = thir.Key,
                    Label1 = date.Year.ToString(),
                    cValue1 = (thir.Sum(t => t.CasesCurrentYear)),
                    rValue1 = (double)(thir.Sum(t => t.RevenueCurrentYear)).ToRoundTwoDecimalDigits(),
                    Period1 = date,
                    Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                    SalesPerson1 = GetSalesManName(thir.First().CurrentYearEmployee),

                    Label2 = date.AddYears(-1).Year.ToString(),
                    cValue2 = (thir.Sum(t => t.CasesPreviousYear)),
                    rValue2 = (double)(thir.Sum(t => t.RevenuePreviousYear)).ToRoundTwoDecimalDigits(),
                    Period2 = date.AddYears(-1),
                    Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                    SalesPerson2 = GetSalesManName(thir.First().PreviousYearEmployee),
                }).ToList()

            };
            listResult.Add(result);

            return listResult;


        }
        public static string GetActiveSalesManName(string nameCombination)
        {

            var names = nameCombination.Split(',');
            if (names.Length > 1)
            {
                return names[1][1] + " " + names[0];
            }
            return nameCombination;

        }
        public static string GetSalesManName(string nameCombination)
        {
            List<string> fullname = new List<string>();
            var names = nameCombination.Split('|');
            foreach (var name in names)
            {
                var singlePerson = name.Split(',');
                if (singlePerson.Length > 1)
                {
                    var personCode = singlePerson.Last().Split(new string[] { " (" }, StringSplitOptions.None);
                    //fullname.Add(personCode.First() + " " + singlePerson.First().Substring(0, 1));// + " (" + personCode.Last());
                    fullname.Add(singlePerson.First() + " " + personCode.First().Trim().Substring(0, 1));// + " (" + personCode.Last());
                }
                else
                {
                    fullname.Add(singlePerson.LastOrDefault());
                }

            }
            return string.Join(",", fullname);

        }

    }
}
