using Nogales.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.DataProvider.Utilities
{
    public static class JournalMapperExtension
    {
        public static OPEXCOGSExpenseJournalChartBM
            GetOpexChartData(this List<APJournalBM> list
                                        , Expression<Func<APJournalBM, bool>> predicate, GlobalFilter date
                                       )
        {
            OPEXCOGSExpenseJournalChartBM model = new OPEXCOGSExpenseJournalChartBM();
            var predicatedList = list.Where(predicate.Compile()).ToList();
            var data = predicatedList.Select(x => new OPEXCOGSExpenseJournalChartBM
            {
                Category = "OPEX",
                Column1 = date.Periods.Current.Label,
                Column2 = date.Periods.Historical.Label,
                Column3 = date.Periods.Prior.Label,

                Period1 = date.Periods.Current.End,
                Period2 = date.Periods.Historical.End,
                Period3 = date.Periods.Prior.End,

                Val1 = Math.Round(predicatedList.Where(y => y.CurrPrev == "current").Sum(t => t.Amount)),
                Val2 = Math.Round(predicatedList.Where(y => y.CurrPrev == "historical").Sum(t => t.Amount)),
                Val3 = Math.Round(predicatedList.Where(y => y.CurrPrev == "prior").Sum(t => t.Amount)),

                Color1 = ChartColorBM.ExpenseCurrent,
                Color2 = ChartColorBM.ExpensePrevious,
                Color3 = ChartColorBM.ExpenseCurrent,

                SubData = new OPEXCOGSExpenseJournalTopBottomBM
                {
                    Top = predicatedList.GroupBy(y => y.DepartmentCode)
                    .OrderByDescending(z => z.Where(p => p.CurrPrev == "current").Sum(p => p.Amount))
                                            .Take(10)
                                            .Select(y => new OPEXCOGSExpenseJournalChartBM
                                            {
                                                Category = y.First().DepartmentName.ToString().TrimEnd(),

                                                Column1 = date.Periods.Current.Label,
                                                Column2 = date.Periods.Historical.Label,
                                                Column3 = date.Periods.Prior.Label,

                                                Period1 = date.Periods.Current.End,
                                                Period2 = date.Periods.Historical.End,
                                                Period3 = date.Periods.Prior.End,

                                                Val1 = Math.Round(y.Where(z => z.CurrPrev == "current").Sum(z => z.Amount)),
                                                Val2 = Math.Round(y.Where(z => z.CurrPrev == "historical").Sum(z => z.Amount)),
                                                Val3 = Math.Round(y.Where(z => z.CurrPrev == "prior").Sum(z => z.Amount)),

                                                Color1 = ChartColorBM.ExpenseCurrent,
                                                Color2 = ChartColorBM.ExpensePrevious,
                                                Color3 = ChartColorBM.ExpenseCurrent,


                                                SubData = new OPEXCOGSExpenseJournalTopBottomBM
                                                {
                                                    Top = y.GroupBy(y2 => y2.AccountName).OrderByDescending(z => z.Where(p => p.CurrPrev == "Current").Sum(p => p.Amount)).Take(10).Select(y2 => new OPEXCOGSExpenseJournalChartBM
                                                    {
                                                        Category = y2.Key.ToString().TrimEnd(),

                                                        Column1 = date.Periods.Current.Label,
                                                        Column2 = date.Periods.Historical.Label,
                                                        Column3 = date.Periods.Prior.Label,

                                                        Period1 = date.Periods.Current.End,
                                                        Period2 = date.Periods.Historical.End,
                                                        Period3 = date.Periods.Prior.End,

                                                        Val1 = Math.Round(y2.Where(z => z.CurrPrev == "current").Sum(z => z.Amount)),
                                                        Val2 = Math.Round(y2.Where(z => z.CurrPrev == "historical").Sum(z => z.Amount)),
                                                        Val3 = Math.Round(y2.Where(z => z.CurrPrev == "prior").Sum(z => z.Amount)),

                                                        Color1 = ChartColorBM.ExpenseCurrent,
                                                        Color2 = ChartColorBM.ExpensePrevious,
                                                        Color3 = ChartColorBM.ExpenseCurrent,


                                                        AccountNumber = y2.Select(z => z.AccountNumber).FirstOrDefault().TrimEnd(),
                                                        SubData = null

                                                    }).ToList(),
                                                    Bottom = y.GroupBy(y2 => y2.AccountName)
                                                    .OrderBy(z => z.Where(p => p.CurrPrev == "current").Sum(p => p.Amount))
                                                    .Take(10)
                                                    .Select(y2 => new OPEXCOGSExpenseJournalChartBM
                                                    {
                                                        Category = y2.Key.ToString().TrimEnd(),
                                                        Column1 = date.Periods.Current.Label,
                                                        Column2 = date.Periods.Historical.Label,
                                                        Column3 = date.Periods.Prior.Label,


                                                        Period1 = date.Periods.Current.End,
                                                        Period2 = date.Periods.Historical.End,
                                                        Period3 = date.Periods.Prior.End,

                                                        Val1 = Math.Round(y2.Where(z => z.CurrPrev == "current").Sum(z => z.Amount)),
                                                        Val2 =Math.Round(y2.Where(z => z.CurrPrev == "historical").Sum(z => z.Amount)),
                                                        Val3 = Math.Round(y2.Where(z => z.CurrPrev == "prior").Sum(z => z.Amount)),

                                                        Color1 = ChartColorBM.ExpenseCurrent,
                                                        Color2 = ChartColorBM.ExpensePrevious,
                                                        Color3 = ChartColorBM.ExpenseCurrent,
                                                        AccountNumber = y2.Select(z => z.AccountNumber).FirstOrDefault().TrimEnd(),
                                                        SubData = null

                                                    }).ToList(),
                                                }

                                            }).ToList(),
                    Bottom = predicatedList.GroupBy(y => y.DepartmentCode)
                    .OrderBy(z => z.Where(p => p.CurrPrev == "current").Sum(p => p.Amount))
                                        .Take(10)
                                        .Select(y => new OPEXCOGSExpenseJournalChartBM
                                        {
                                            Category = y.First().DepartmentName.ToString().TrimEnd(),
                                            Column1 = date.Periods.Current.Label,
                                            Column2 = date.Periods.Historical.Label,
                                            Column3 = date.Periods.Prior.Label,

                                            Period1 = date.Periods.Current.End,
                                            Period2 = date.Periods.Historical.End,
                                            Period3 = date.Periods.Prior.End,

                                            Val1 = y.Where(z => z.CurrPrev == "current").Sum(z => z.Amount),
                                            Val2 = y.Where(z => z.CurrPrev == "historical").Sum(z => z.Amount),
                                            Val3 = y.Where(z => z.CurrPrev == "prior").Sum(z => z.Amount),

                                            Color1 = ChartColorBM.ExpenseCurrent,
                                            Color2 = ChartColorBM.ExpensePrevious,
                                            Color3 = ChartColorBM.ExpenseCurrent,
                                            AccountNumber = y.Select(z => z.AccountNumber).FirstOrDefault().TrimEnd(),
                                            SubData = y.Select(sa => new OPEXCOGSExpenseJournalTopBottomBM
                                            {
                                                Top = y.GroupBy(y2 => y2.AccountName).OrderByDescending(z => z.Where(p => p.CurrPrev == "Current").Sum(p => p.Amount)).Take(10).Select(y2 => new OPEXCOGSExpenseJournalChartBM
                                                {
                                                    Category = y2.Key.ToString().TrimEnd(),

                                                    Column1 = date.Periods.Current.Label,
                                                    Column2 = date.Periods.Historical.Label,
                                                    Column3 = date.Periods.Prior.Label,

                                                    Period1 = date.Periods.Current.End,
                                                    Period2 = date.Periods.Historical.End,
                                                    Period3 = date.Periods.Prior.End,

                                                    Val1 = y2.Where(z => z.CurrPrev == "current").Sum(z => z.Amount),
                                                    Val2 = y2.Where(z => z.CurrPrev == "historical").Sum(z => z.Amount),
                                                    Val3 = y2.Where(z => z.CurrPrev == "prior").Sum(z => z.Amount),

                                                    Color1 = ChartColorBM.ExpenseCurrent,
                                                    Color2 = ChartColorBM.ExpensePrevious,
                                                    Color3 = ChartColorBM.ExpenseCurrent,


                                                    AccountNumber = y2.Select(z => z.AccountNumber).FirstOrDefault().TrimEnd(),
                                                    SubData = null

                                                }).ToList(),
                                                Bottom = y.GroupBy(y2 => y2.AccountName)
                                                    .OrderBy(z => z.Where(p => p.CurrPrev == "current").Sum(p => p.Amount))
                                                    .Take(10)
                                                    .Select(y2 => new OPEXCOGSExpenseJournalChartBM
                                                    {
                                                        Category = y2.Key.ToString().TrimEnd(),
                                                        Column1 = date.Periods.Current.Label,
                                                        Column2 = date.Periods.Historical.Label,
                                                        Column3 = date.Periods.Prior.Label,

                                                        Period1 = date.Periods.Current.End,
                                                        Period2 = date.Periods.Historical.End,
                                                        Period3 = date.Periods.Prior.End,

                                                        Val1 = y2.Where(z => z.CurrPrev == "current").Sum(z => z.Amount),
                                                        Val2 = y2.Where(z => z.CurrPrev == "historical").Sum(z => z.Amount),
                                                        Val3 = y2.Where(z => z.CurrPrev == "prior").Sum(z => z.Amount),

                                                        Color1 = ChartColorBM.ExpenseCurrent,
                                                        Color2 = ChartColorBM.ExpensePrevious,
                                                        Color3 = ChartColorBM.ExpenseCurrent,
                                                        AccountNumber = y2.Select(z => z.AccountNumber).FirstOrDefault().TrimEnd(),
                                                        SubData = null

                                                    }).ToList(),
                                            }).FirstOrDefault()

                                        }).ToList(),
                }
            }).ToList().FirstOrDefault();
            return data;


        }

        public static OPEXCOGSExpenseJournalChartBM
           GetCogsChartData(this List<APJournalBM> list
                                       , Expression<Func<APJournalBM, bool>> predicate, GlobalFilter filters
                                      )
        {
            OPEXCOGSExpenseJournalChartBM model = new OPEXCOGSExpenseJournalChartBM();
            var predicatedList = list.Where(predicate.Compile()).ToList();
            var data = predicatedList.Select(x => new OPEXCOGSExpenseJournalChartBM
            {
                Category = "COGS",
                Column1 = filters.Periods.Current.Label,
                Column2 = filters.Periods.Historical.Label,
                Column3 = filters.Periods.Prior.Label,

                Period1 = filters.Periods.Current.End,
                Period2 = filters.Periods.Historical.End,
                Period3 = filters.Periods.Prior.End,

                Val1 = predicatedList.Where(y => y.CurrPrev == "current").Sum(t => t.Amount).ToStripDecimal(),
                Val2 = predicatedList.Where(y => y.CurrPrev == "historical").Sum(t => t.Amount).ToStripDecimal(),
                Val3 = predicatedList.Where(y => y.CurrPrev == "prior").Sum(t => t.Amount).ToStripDecimal(),

                Color1 = ChartColorBM.ExpenseCurrent,
                Color2 = ChartColorBM.ExpensePrevious,
                Color3 = ChartColorBM.ExpenseCurrent,


                SubData = predicatedList.Select(sa => new OPEXCOGSExpenseJournalTopBottomBM
                {
                    Top = predicatedList.GroupBy(y2 => y2.AccountName).OrderByDescending(z => z.Where(p => p.CurrPrev == "Current").Sum(p => p.Amount)).Take(10).Select(y2 => new OPEXCOGSExpenseJournalChartBM
                    {
                        Category = y2.Key.ToString().TrimEnd(),

                        Column1 = filters.Periods.Current.Label,
                        Column2 = filters.Periods.Historical.Label,
                        Column3 = filters.Periods.Prior.Label,

                        Period1 = filters.Periods.Current.End,
                        Period2 = filters.Periods.Historical.End,
                        Period3 = filters.Periods.Prior.End,

                        Val1 = y2.Where(z => z.CurrPrev == "current").Sum(z => z.Amount).ToStripDecimal(),
                        Val2 = y2.Where(z => z.CurrPrev == "historical").Sum(z => z.Amount).ToStripDecimal(),
                        Val3 = y2.Where(z => z.CurrPrev == "prior").Sum(z => z.Amount).ToStripDecimal(),

                        Color1 = ChartColorBM.ExpenseCurrent,
                        Color2 = ChartColorBM.ExpensePrevious,
                        Color3 = ChartColorBM.ExpenseCurrent,


                        AccountNumber = y2.Select(z => z.AccountNumber).FirstOrDefault().TrimEnd(),
                        SubData = null

                    }).ToList(),
                    Bottom = predicatedList.GroupBy(y2 => y2.AccountName)
                    .OrderBy(z => z.Where(p => p.CurrPrev == "current").Sum(p => p.Amount))
                    .Take(10)
                    .Select(y2 => new OPEXCOGSExpenseJournalChartBM
                    {
                        Category = y2.Key.ToString().TrimEnd(),
                        Column1 = filters.Periods.Current.Label,
                        Column2 = filters.Periods.Historical.Label,
                        Column3 = filters.Periods.Prior.Label,

                        Period1 = filters.Periods.Current.End,
                        Period2 = filters.Periods.Historical.End,
                        Period3 = filters.Periods.Prior.End,

                        Val1 = y2.Where(z => z.CurrPrev == "current").Sum(z => z.Amount).ToStripDecimal(),
                        Val2 = y2.Where(z => z.CurrPrev == "historical").Sum(z => z.Amount).ToStripDecimal(),
                        Val3 = y2.Where(z => z.CurrPrev == "prior").Sum(z => z.Amount).ToStripDecimal(),

                        Color1 = ChartColorBM.ExpenseCurrent,
                        Color2 = ChartColorBM.ExpensePrevious,
                        Color3 = ChartColorBM.ExpenseCurrent,
                        AccountNumber = y2.Select(z => z.AccountNumber).FirstOrDefault().TrimEnd(),
                        SubData = null

                    }).ToList(),
                }).FirstOrDefault()
            }).ToList().FirstOrDefault();
            return data;


        }

        public static OPEXCOGSExpenseJournalChartBM
         GetOpexChartData(this List<APJournalBM> list
                                     , Expression<Func<APJournalBM, bool>> predicate, DateTime date
                                    )
        {
            OPEXCOGSExpenseJournalChartBM model = new OPEXCOGSExpenseJournalChartBM();
            var predicatedList = list.Where(predicate.Compile()).ToList();
            var data = predicatedList.Select(x => new OPEXCOGSExpenseJournalChartBM
            {
                Category = "OPEX",
                Column1 = date.ToString("MMM yy"),
                Column2 = date.AddYears(-1).ToString("MMM yy"),
                Column3 = date.AddMonths(-1).ToString("MMM yy"),

                Period1 = date,
                Period2 = date.AddYears(-1),
                Period3 = date.AddMonths(-1),

                Val1 = predicatedList.Where(y => y.CurrPrev == "Current").Sum(t => t.Amount),
                Val2 = predicatedList.Where(y => y.CurrPrev == "LastYear").Sum(t => t.Amount),
                Val3 = predicatedList.Where(y => y.CurrPrev == "Previous").Sum(t => t.Amount),

                Color1 = ChartColorBM.ExpenseCurrent,
                Color2 = ChartColorBM.ExpensePrevious,
                Color3 = ChartColorBM.ExpenseCurrent,

                SubData = new OPEXCOGSExpenseJournalTopBottomBM
                {
                    Top = predicatedList.GroupBy(y => y.DepartmentCode)
                    .OrderByDescending(z => z.Where(p => p.CurrPrev == "Current").Sum(p => p.Amount))
                                            .Take(10)
                                            .Select(y => new OPEXCOGSExpenseJournalChartBM
                                            {
                                                Category = y.First().DepartmentName.ToString().TrimEnd(),

                                                Column1 = date.ToString("MMM yy"),
                                                Column2 = date.AddYears(-1).ToString("MMM yy"),
                                                Column3 = date.AddMonths(-1).ToString("MMM yy"),

                                                Period1 = date,
                                                Period2 = date.AddYears(-1),
                                                Period3 = date.AddMonths(-1),

                                                Val1 = y.Where(z => z.CurrPrev == "Current").Sum(z => z.Amount),
                                                Val2 = y.Where(z => z.CurrPrev == "LastYear").Sum(z => z.Amount),
                                                Val3 = y.Where(z => z.CurrPrev == "Previous").Sum(z => z.Amount),

                                                Color1 = ChartColorBM.ExpenseCurrent,
                                                Color2 = ChartColorBM.ExpensePrevious,
                                                Color3 = ChartColorBM.ExpenseCurrent,


                                                SubData = new OPEXCOGSExpenseJournalTopBottomBM
                                                {
                                                    Top = y.GroupBy(y2 => y2.AccountName).OrderByDescending(z => z.Where(p => p.CurrPrev == "Current").Sum(p => p.Amount)).Take(10).Select(y2 => new OPEXCOGSExpenseJournalChartBM
                                                    {
                                                        Category = y2.Key.ToString().TrimEnd(),

                                                        Column1 = date.ToString("MMM yy"),
                                                        Column2 = date.AddYears(-1).ToString("MMM yy"),
                                                        Column3 = date.AddMonths(-1).ToString("MMM yy"),

                                                        Period1 = date,
                                                        Period2 = date.AddYears(-1),
                                                        Period3 = date.AddMonths(-1),

                                                        Val1 = y2.Where(z => z.CurrPrev == "Current").Sum(z => z.Amount),
                                                        Val2 = y2.Where(z => z.CurrPrev == "LastYear").Sum(z => z.Amount),
                                                        Val3 = y2.Where(z => z.CurrPrev == "Previous").Sum(z => z.Amount),

                                                        Color1 = ChartColorBM.ExpenseCurrent,
                                                        Color2 = ChartColorBM.ExpensePrevious,
                                                        Color3 = ChartColorBM.ExpenseCurrent,


                                                        AccountNumber = y2.Select(z => z.AccountNumber).FirstOrDefault().TrimEnd(),
                                                        SubData = null

                                                    }).ToList(),
                                                    Bottom = y.GroupBy(y2 => y2.AccountName)
                                                    .OrderBy(z => z.Where(p => p.CurrPrev == "Current").Sum(p => p.Amount))
                                                    .Take(10)
                                                    .Select(y2 => new OPEXCOGSExpenseJournalChartBM
                                                    {
                                                        Category = y.Key.ToString().TrimEnd(),
                                                        Column1 = date.ToString("MMM yy"),
                                                        Column2 = date.AddYears(-1).ToString("MMM yy"),
                                                        Column3 = date.AddMonths(-1).ToString("MMM yy"),


                                                        Period1 = date,
                                                        Period2 = date.AddYears(-1),
                                                        Period3 = date.AddMonths(-1),

                                                        Val1 = y2.Where(z => z.CurrPrev == "Current").Sum(z => z.Amount),
                                                        Val2 = y2.Where(z => z.CurrPrev == "LastYear").Sum(z => z.Amount),
                                                        Val3 = y2.Where(z => z.CurrPrev == "Previous").Sum(z => z.Amount),

                                                        Color1 = ChartColorBM.ExpenseCurrent,
                                                        Color2 = ChartColorBM.ExpensePrevious,
                                                        Color3 = ChartColorBM.ExpenseCurrent,
                                                        AccountNumber = y2.Select(z => z.AccountNumber).FirstOrDefault().TrimEnd(),
                                                        SubData = null

                                                    }).ToList(),
                                                }

                                            }).ToList(),
                    Bottom = predicatedList.GroupBy(y => y.DepartmentCode)
                    .OrderBy(z => z.Where(p => p.CurrPrev == "Current").Sum(p => p.Amount))
                                        .Take(10)
                                        .Select(y => new OPEXCOGSExpenseJournalChartBM
                                        {
                                            Category = y.First().DepartmentName.ToString().TrimEnd(),
                                            Column1 = date.ToString("MMM yy"),
                                            Column2 = date.AddYears(-1).ToString("MMM yy"),
                                            Column3 = date.AddMonths(-1).ToString("MMM yy"),

                                            Period1 = date,
                                            Period2 = date.AddYears(-1),
                                            Period3 = date.AddMonths(-1),

                                            Val1 = y.Where(z => z.CurrPrev == "Current").Sum(z => z.Amount),
                                            Val2 = y.Where(z => z.CurrPrev == "LastYear").Sum(z => z.Amount),
                                            Val3 = y.Where(z => z.CurrPrev == "Previous").Sum(z => z.Amount),

                                            Color1 = ChartColorBM.ExpenseCurrent,
                                            Color2 = ChartColorBM.ExpensePrevious,
                                            Color3 = ChartColorBM.ExpenseCurrent,
                                            AccountNumber = y.Select(z => z.AccountNumber).FirstOrDefault().TrimEnd(),
                                            SubData = y.Select(sa => new OPEXCOGSExpenseJournalTopBottomBM
                                            {
                                                Top = y.GroupBy(y2 => y2.AccountName).OrderByDescending(z => z.Where(p => p.CurrPrev == "Current").Sum(p => p.Amount)).Take(10).Select(y2 => new OPEXCOGSExpenseJournalChartBM
                                                {
                                                    Category = y2.Key.ToString().TrimEnd(),

                                                    Column1 = date.ToString("MMM yy"),
                                                    Column2 = date.AddYears(-1).ToString("MMM yy"),
                                                    Column3 = date.AddMonths(-1).ToString("MMM yy"),

                                                    Period1 = date,
                                                    Period2 = date.AddYears(-1),
                                                    Period3 = date.AddMonths(-1),

                                                    Val1 = y2.Where(z => z.CurrPrev == "Current").Sum(z => z.Amount),
                                                    Val2 = y2.Where(z => z.CurrPrev == "LastYear").Sum(z => z.Amount),
                                                    Val3 = y2.Where(z => z.CurrPrev == "Previous").Sum(z => z.Amount),

                                                    Color1 = ChartColorBM.ExpenseCurrent,
                                                    Color2 = ChartColorBM.ExpensePrevious,
                                                    Color3 = ChartColorBM.ExpenseCurrent,


                                                    AccountNumber = y2.Select(z => z.AccountNumber).FirstOrDefault().TrimEnd(),
                                                    SubData = null

                                                }).ToList(),
                                                Bottom = y.GroupBy(y2 => y2.AccountName)
                                                    .OrderBy(z => z.Where(p => p.CurrPrev == "Current").Sum(p => p.Amount))
                                                    .Take(10)
                                                    .Select(y2 => new OPEXCOGSExpenseJournalChartBM
                                                    {
                                                        Category = y.Key.ToString().TrimEnd(),
                                                        Column1 = date.ToString("MMM yy"),
                                                        Column2 = date.AddYears(-1).ToString("MMM yy"),
                                                        Column3 = date.AddMonths(-1).ToString("MMM yy"),

                                                        Period1 = date,
                                                        Period2 = date.AddYears(-1),
                                                        Period3 = date.AddMonths(-1),

                                                        Val1 = y2.Where(z => z.CurrPrev == "Current").Sum(z => z.Amount),
                                                        Val2 = y2.Where(z => z.CurrPrev == "LastYear").Sum(z => z.Amount),
                                                        Val3 = y2.Where(z => z.CurrPrev == "Previous").Sum(z => z.Amount),

                                                        Color1 = ChartColorBM.ExpenseCurrent,
                                                        Color2 = ChartColorBM.ExpensePrevious,
                                                        Color3 = ChartColorBM.ExpenseCurrent,
                                                        AccountNumber = y2.Select(z => z.AccountNumber).FirstOrDefault().TrimEnd(),
                                                        SubData = null

                                                    }).ToList(),
                                            }).FirstOrDefault()

                                        }).ToList(),
                }
            }).ToList().FirstOrDefault();
            return data;


        }


        public static OPEXCOGSExpenseJournalChartBM
                  GetCogsChartData(this List<APJournalBM> list
                                              , Expression<Func<APJournalBM, bool>> predicate, DateTime date
                                             )
        {
            OPEXCOGSExpenseJournalChartBM model = new OPEXCOGSExpenseJournalChartBM();
            var predicatedList = list.Where(predicate.Compile()).ToList();
            var data = predicatedList.Select(x => new OPEXCOGSExpenseJournalChartBM
            {
                Category = "COGS",
                Column1 = date.ToString("MMM yy"),
                Column2 = date.AddYears(-1).ToString("MMM yy"),
                Column3 = date.AddMonths(-1).ToString("MMM yy"),

                Period1 = date,
                Period2 = date.AddYears(-1),
                Period3 = date.AddMonths(-1),

                Val1 = predicatedList.Where(y => y.CurrPrev == "Current").Sum(t => t.Amount),
                Val2 = predicatedList.Where(y => y.CurrPrev == "LastYear").Sum(t => t.Amount),
                Val3 = predicatedList.Where(y => y.CurrPrev == "Previous").Sum(t => t.Amount),

                Color1 = ChartColorBM.ExpenseCurrent,
                Color2 = ChartColorBM.ExpensePrevious,
                Color3 = ChartColorBM.ExpenseCurrent,


                SubData = predicatedList.Select(sa => new OPEXCOGSExpenseJournalTopBottomBM
                {
                    Top = predicatedList.GroupBy(y2 => y2.AccountName).OrderByDescending(z => z.Where(p => p.CurrPrev == "Current").Sum(p => p.Amount)).Take(10).Select(y2 => new OPEXCOGSExpenseJournalChartBM
                    {
                        Category = y2.Key.ToString().TrimEnd(),

                        Column1 = date.ToString("MMM yy"),
                        Column2 = date.AddYears(-1).ToString("MMM yy"),
                        Column3 = date.AddMonths(-1).ToString("MMM yy"),

                        Period1 = date,
                        Period2 = date.AddYears(-1),
                        Period3 = date.AddMonths(-1),

                        Val1 = y2.Where(z => z.CurrPrev == "Current").Sum(z => z.Amount),
                        Val2 = y2.Where(z => z.CurrPrev == "LastYear").Sum(z => z.Amount),
                        Val3 = y2.Where(z => z.CurrPrev == "Previous").Sum(z => z.Amount),

                        Color1 = ChartColorBM.ExpenseCurrent,
                        Color2 = ChartColorBM.ExpensePrevious,
                        Color3 = ChartColorBM.ExpenseCurrent,


                        AccountNumber = y2.Select(z => z.AccountNumber).FirstOrDefault().TrimEnd(),
                        SubData = null

                    }).ToList(),
                    Bottom = predicatedList.GroupBy(y2 => y2.AccountName)
                    .OrderBy(z => z.Where(p => p.CurrPrev == "Current").Sum(p => p.Amount))
                    .Take(10)
                    .Select(y2 => new OPEXCOGSExpenseJournalChartBM
                    {
                        Category = y2.Key.ToString().TrimEnd(),
                        Column1 = date.ToString("MMM yy"),
                        Column2 = date.AddYears(-1).ToString("MMM yy"),
                        Column3 = date.AddMonths(-1).ToString("MMM yy"),

                        Period1 = date,
                        Period2 = date.AddYears(-1),
                        Period3 = date.AddMonths(-1),

                        Val1 = y2.Where(z => z.CurrPrev == "Current").Sum(z => z.Amount),
                        Val2 = y2.Where(z => z.CurrPrev == "LastYear").Sum(z => z.Amount),
                        Val3 = y2.Where(z => z.CurrPrev == "Previous").Sum(z => z.Amount),

                        Color1 = ChartColorBM.ExpenseCurrent,
                        Color2 = ChartColorBM.ExpensePrevious,
                        Color3 = ChartColorBM.ExpenseCurrent,
                        AccountNumber = y2.Select(z => z.AccountNumber).FirstOrDefault().TrimEnd(),
                        SubData = null

                    }).ToList(),
                }).FirstOrDefault()
            }).ToList().FirstOrDefault();
            return data;


        }
    }
}
