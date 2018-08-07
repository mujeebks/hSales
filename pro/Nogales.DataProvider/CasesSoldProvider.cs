using Nogales.BusinessModel;
using Nogales.DataProvider.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Nogales.DataProvider.Utilities;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.SqlClient;
using Nogales.DataProvider.ENUM;

namespace Nogales.DataProvider
{
    public class CasesSoldProvider : DataAccessADO
    {

       
        public ClusteredBarChartCategoryBM GetCasesSoldByCategoryMonth(DateTime filterDate
                                                                         , string current, string previous
                                                                         , string currentMonthStart, string currentMonthEnd
                                                                         , string previousMonthStart, string previousMonthEnd
                                                                         , string previousYearStart, string previousYearEnd
                                                                         , string previousMonthYearStart, string previousMonthYearEnd)
        {

            var dataSetResult = new DataSet();
            using (var adoDataAccess = new DataAccessADO())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@current", current));
                parameterList.Add(new SqlParameter("@previous", previous));
                parameterList.Add(new SqlParameter("@current_start_date", currentMonthStart));
                parameterList.Add(new SqlParameter("@current_date", currentMonthEnd));
                parameterList.Add(new SqlParameter("@previous_month_start_date", previousMonthStart));
                parameterList.Add(new SqlParameter("@previous_month_date", previousMonthEnd));
                parameterList.Add(new SqlParameter("@previous_year_start_date", previousYearStart));
                parameterList.Add(new SqlParameter("@previous_year_date", previousYearEnd));
                parameterList.Add(new SqlParameter("@previous_year_prior_month_start_date", previousMonthYearStart));
                parameterList.Add(new SqlParameter("@previous_year_prior_month_date", previousMonthYearEnd));

                dataSetResult = base.ReadToDataSetViaProcedure("BI_GetCasesSoldCategoryByMonth", parameterList.ToArray());
            }

            var result = GetCasesSoldByCategory(filterDate, Constants.ByMonth, dataSetResult, current, previous);
            return result;
        }

        public List<CasesSoldSalesPersonChartBM> GetCasesSoldBySalesPersonMonth(DateTime filterDate
                                                                            , string current, string previous
                                                                            , string currentMonthStart, string currentMonthEnd
                                                                            , string previousMonthStart, string previousMonthEnd)
        {
            string sqlQuery = SQLQueries.GetMonthlyCasesSoldBySalesPersonQuery(currentMonthStart, currentMonthEnd
                                                                               , previousMonthStart, previousMonthEnd
                                                                               , current, previous);

            var result = GetCasesSoldBySalesPerson(filterDate, Constants.ByMonth, sqlQuery, current, previous);
            return result.ToList();
        }

        public ClusteredBarChartCategoryBM GetCasesSoldByCategoryYear(DateTime filterDate
                                                                           , string current, string previous
                                                                           , string currentStart, string currentEnd
                                                                           , string previousYearStart, string previousYearEnd)
        {
            string sqlQueryString = SQLQueries.GetCasesSoldYearlyQuery(current
                                                                    , previous
                                                                    , currentStart
                                                                    , currentEnd
                                                                    , previousYearStart
                                                                    , previousYearEnd);

            var result = GetCasesSoldByCategoryYear(filterDate, Constants.ByYear, sqlQueryString, current, previous);
            return result;
        }

        public List<CasesSoldSalesPersonChartBM> GetCasesSoldBySalesPersonYear(DateTime filterDate
                                                                            , string current, string previous
                                                                            , string currentYearStart, string currentYearEnd
                                                                            , string previousYearStart, string previousYearEnd)
        {
            string sqlQuery = SQLQueries.GetYearlyCasesSoldSalesBySalesPerson(currentYearStart, currentYearEnd
                                                                               , previousYearStart, previousYearEnd
                                                                               , current, previous);


            var result = GetCasesSoldBySalesPerson(filterDate, Constants.ByYear, sqlQuery, current, previous);
            return result.ToList();
        }

        private ClusteredBarChartCategoryBM GetCasesSoldByCategoryYear(DateTime filterDate, string filterType, string sqlQuery, string currentLabel, string previousLabel)
        {
            try
            {
                var dataTableResult = new DataSet();
                using (var adoDataAccess = new DataAccessADO())
                {
                    dataTableResult = adoDataAccess.ReadToDataSet(sqlQuery);
                }
                var listofCasesSold = dataTableResult.Tables[0]
                                                 .AsEnumerable()
                                                 .Select(x => new CasesSoldMapper
                                                 {
                                                     Category = x.Field<string>("Category").TrimEnd(),
                                                     Comodity = x.Field<string>("Descrip").TrimEnd(),
                                                     SalesPerson = x.Field<string>("SalesmanCode").TrimEnd(), //Assigned person code
                                                     SalesPersonCode = (!string.IsNullOrEmpty(x.Field<string>("code")) ? x.Field<string>("code").TrimEnd() : string.Empty), // Sales person code
                                                     SalesPersonDescription = x.Field<string>("descr"), //Sales person description
                                                     CurrentSold = x.Field<int?>(currentLabel) ?? 0,
                                                     PreviousSold = x.Field<int?>(previousLabel) ?? 0,
                                                     Year = filterType == Constants.ByMonth ? (x.Field<int>("InvoiceYear")).ToString() : filterDate.Year.ToString(),
                                                 })
                                                 .ToList();

                var totalGrocery = listofCasesSold.GetCasesSoldForTotal(x => x.Comodity == "Grocery", filterType, filterDate).ToList();
                var totalProduce = listofCasesSold.GetCasesSoldForTotal(x => x.Comodity == "Produce", filterType, filterDate).ToList();
                var allTotals = new List<ClusteredStackChartBM>();
                allTotals = GetClusteredStackChartData(totalGrocery, totalProduce, Constants.ByMonth);

                #region mapping w.r.t to Categories
                var casesSoldCategories = new ClusteredBarChartCategoryBM
                {
                    Total = new CasesSoldComodityBM
                    {
                        Grocery = totalGrocery,
                        Produce = totalProduce,
                        All = allTotals
                    },
                    Buyer = GetCasesSoldComodityBM(listofCasesSold, "BUYER", filterType, filterDate),
                    FoodService = GetCasesSoldComodityBM(listofCasesSold, "FOOD SERVI", filterType, filterDate),
                    Carniceria = GetCasesSoldComodityBM(listofCasesSold, "CARNICERIA", filterType, filterDate),
                    LossProd = GetCasesSoldComodityBM(listofCasesSold, "LOSS PROD", filterType, filterDate),
                    National = GetCasesSoldComodityBM(listofCasesSold, "NATIONAL", filterType, filterDate),
                    Wholesaler = GetCasesSoldComodityBM(listofCasesSold, "WHOLESALER", filterType, filterDate),
                    WillCall = GetCasesSoldComodityBM(listofCasesSold, "WILL CALL", filterType, filterDate),
                    Retail = new CasesSoldComodityBM
                    {
                        Grocery = listofCasesSold.GetCasesFromComodityAndCategory(x => x.Comodity == "Grocery" && x.Category.StartsWith("RETAIL"), filterType, filterDate),
                        Produce = listofCasesSold.GetCasesFromComodityAndCategory(x => x.Comodity == "Produce" && x.Category.StartsWith("RETAIL"), filterType, filterDate),
                    },
                    AllOthers = new CasesSoldComodityBM
                    {
                        Grocery = listofCasesSold.GetCasesFromComodityAndCategory(x => x.Comodity == "Grocery" &&
                                                                                !x.Category.StartsWith("RETAIL") &&
                                                                                x.Category != "BUYER" &&
                                                                                x.Category != "FOOD SERVI" &&
                                                                                x.Category != "CARNICERIA" &&
                                                                                x.Category != "LOSS PROD" &&
                                                                                x.Category != "NATIONAL" &&
                                                                                x.Category != "WHOLESALER" &&
                                                                                x.Category != "WILL CALL"
                                                                                        , filterType
                                                                                        , filterDate),
                        Produce = listofCasesSold.GetCasesFromComodityAndCategory(x => x.Comodity == "Produce" && !x.Category.StartsWith("RETAIL") &&
                                                                                x.Category != "BUYER" &&
                                                                                x.Category != "FOOD SERVI" &&
                                                                                x.Category != "CARNICERIA" &&
                                                                                x.Category != "LOSS PROD" &&
                                                                                x.Category != "NATIONAL" &&
                                                                                x.Category != "WHOLESALER" &&
                                                                                x.Category != "WILL CALL"
                                                                                        , filterType
                                                                                        , filterDate),

                    },
                };
                #endregion

                return casesSoldCategories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        private ClusteredBarChartCategoryBM GetCasesSoldByCategory(DateTime filterDate, string filterType, DataSet dataTableResult, string currentLabel, string previousLabel)
        {
            try
            {

                var listofCasesSold = dataTableResult.Tables[0]
                                                 .AsEnumerable()
                                                 .Select(x => new CasesSoldMapper
                                                 {
                                                     Category = x.Field<string>("Category").TrimEnd(),
                                                     Comodity = x.Field<string>("Descrip").TrimEnd(),
                                                     SalesPerson = x.Field<string>("SalesmanCode").TrimEnd(), //Assigned person code
                                                     SalesPersonCode = (!string.IsNullOrEmpty(x.Field<string>("code")) ? x.Field<string>("code").TrimEnd() : string.Empty), // Sales person code
                                                     SalesPersonDescription = x.Field<string>("descr"), //Sales person description
                                                     CurrentSold = x.Field<int?>(currentLabel) ?? 0,
                                                     PreviousSold = x.Field<int?>(previousLabel) ?? 0,
                                                     Year = filterType == Constants.ByMonth ? (x.Field<int>("InvoiceYear")).ToString() : filterDate.Year.ToString(),
                                                 })
                                                 .ToList();

                var totalGrocery = listofCasesSold.GetCasesSoldForTotal(x => x.Comodity == "Grocery", filterType, filterDate).ToList();
                var totalProduce = listofCasesSold.GetCasesSoldForTotal(x => x.Comodity == "Produce", filterType, filterDate).ToList();
                var allTotals = new List<ClusteredStackChartBM>();
                allTotals = GetClusteredStackChartData(totalGrocery, totalProduce, Constants.ByMonth);

                #region mapping w.r.t to Categories
                var casesSoldCategories = new ClusteredBarChartCategoryBM
                {
                    Total = new CasesSoldComodityBM
                    {
                        Grocery = totalGrocery,
                        Produce = totalProduce,
                        All = allTotals
                    },
                    Buyer = GetCasesSoldComodityBM(listofCasesSold, "BUYER", filterType, filterDate),
                    FoodService = GetCasesSoldComodityBM(listofCasesSold, "FOOD SERVI", filterType, filterDate),
                    Carniceria = GetCasesSoldComodityBM(listofCasesSold, "CARNICERIA", filterType, filterDate),
                    LossProd = GetCasesSoldComodityBM(listofCasesSold, "LOSS PROD", filterType, filterDate),
                    National = GetCasesSoldComodityBM(listofCasesSold, "NATIONAL", filterType, filterDate),
                    Wholesaler = GetCasesSoldComodityBM(listofCasesSold, "WHOLESALER", filterType, filterDate),
                    WillCall = GetCasesSoldComodityBM(listofCasesSold, "WILL CALL", filterType, filterDate),
                    Retail = new CasesSoldComodityBM
                    {
                        Grocery = listofCasesSold.GetCasesFromComodityAndCategory(x => x.Comodity == "Grocery" && x.Category.StartsWith("RETAIL"), filterType, filterDate),
                        Produce = listofCasesSold.GetCasesFromComodityAndCategory(x => x.Comodity == "Produce" && x.Category.StartsWith("RETAIL"), filterType, filterDate),
                    },
                    AllOthers = new CasesSoldComodityBM
                    {
                        Grocery = listofCasesSold.GetCasesFromComodityAndCategory(x => x.Comodity == "Grocery" &&
                                                                                !x.Category.StartsWith("RETAIL") &&
                                                                                x.Category != "BUYER" &&
                                                                                x.Category != "FOOD SERVI" &&
                                                                                x.Category != "CARNICERIA" &&
                                                                                x.Category != "LOSS PROD" &&
                                                                                x.Category != "NATIONAL" &&
                                                                                x.Category != "WHOLESALER" &&
                                                                                x.Category != "WILL CALL"
                                                                                        , filterType
                                                                                        , filterDate),
                        Produce = listofCasesSold.GetCasesFromComodityAndCategory(x => x.Comodity == "Produce" && !x.Category.StartsWith("RETAIL") &&
                                                                                x.Category != "BUYER" &&
                                                                                x.Category != "FOOD SERVI" &&
                                                                                x.Category != "CARNICERIA" &&
                                                                                x.Category != "LOSS PROD" &&
                                                                                x.Category != "NATIONAL" &&
                                                                                x.Category != "WHOLESALER" &&
                                                                                x.Category != "WILL CALL"
                                                                                        , filterType
                                                                                        , filterDate),

                    },
                };
                #endregion

                return casesSoldCategories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private CasesSoldComodityBM GetCasesSoldComodityBM(List<CasesSoldMapper> listTotalCasesSold, string category, string filterType, DateTime filterDate)
        {
            return new CasesSoldComodityBM
            {
                Grocery = listTotalCasesSold.GetCasesFromComodityAndCategory(x => x.Comodity == "Grocery" && x.Category == category, filterType, filterDate),
                Produce = listTotalCasesSold.GetCasesFromComodityAndCategory(x => x.Comodity == "Produce" && x.Category == category, filterType, filterDate),
            };
        }


        private IEnumerable<CasesSoldSalesPersonChartBM> GetCasesSoldBySalesPerson(DateTime filterDate, string filterType, string sqlQuery, string currentLabel, string previousLabel)
        {
            try
            {
                var dataTableResult = new DataSet();
                using (var adoDataAccess = new DataAccessADO())
                {
                    dataTableResult = adoDataAccess.ReadToDataSet(sqlQuery);
                }

                var listofCasesSold = new List<CasesSoldMapper>();

                if (filterType == Constants.ByMonth)
                {
                    listofCasesSold = dataTableResult.Tables[0]
                                                   .AsEnumerable()
                                                   .Select(x => new CasesSoldMapper
                                                   {
                                                       Customer = (x.Field<string>("Customer") ?? string.Empty).TrimEnd(),
                                                       SalesPerson = (x.Field<string>("SalesPersonCode") ?? string.Empty).TrimEnd(),
                                                       CurrentSold = (x.Field<decimal?>(currentLabel) ?? 0).ToRoundTwoDecimalDigits(),
                                                       PreviousSold = (x.Field<decimal?>(previousLabel) ?? 0).ToRoundTwoDecimalDigits(),

                                                   })
                                                   .ToList();
                }
                else
                {

                    listofCasesSold = dataTableResult.Tables[0]
                                                    .AsEnumerable()
                                                    .Select(x => new CasesSoldMapper
                                                    {
                                                        Customer = (x.Field<string>("Customer") ?? string.Empty).TrimEnd(),
                                                        SalesPerson = (x.Field<string>("SalesPersonCode") ?? string.Empty).TrimEnd(),
                                                        CurrentSold = Convert.ToDouble((x.Field<int?>(currentLabel) ?? 0)),
                                                        PreviousSold = Convert.ToDouble((x.Field<int?>(previousLabel) ?? 0)),

                                                    })
                                                    .ToList();
                }

                var result = listofCasesSold
                                            .GroupBy(x => x.SalesPerson)
                                            .OrderByDescending(x => x.Sum(t => t.CurrentSold))
                                            .Take(10)
                                            .Select(grp => new CasesSoldSalesPersonChartBM
                                            {
                                                Category = grp.Key,
                                                Column1 = filterDate.ToColumnLabelName(filterType, filterDate.Year.ToString(), false),
                                                Column2 = filterDate.ToColumnLabelName(filterType, filterDate.Year.ToString()),
                                                Color1 = ChartColorBM.Colors[2],
                                                Color2 = ChartColorBM.Colors[0],
                                                Val1 = (grp.Sum(t => t.CurrentSold) ?? 0).ToRoundTwoDigits(),
                                                Val2 = (grp.Sum(t => t.PreviousSold) ?? 0).ToRoundTwoDigits(),
                                                SubData = new CasesSoldSalesPersonBM
                                                {
                                                    Top = grp.OrderByDescending(s => s.CurrentSold)
                                                            .Take(5)
                                                            .Select((s, idx) => new CasesSoldSalesPersonChartBM
                                                            {
                                                                Category = s.Customer,
                                                                Column1 = filterDate.ToColumnLabelName(filterType, filterDate.Year.ToString(), false),
                                                                Column2 = filterDate.ToColumnLabelName(filterType, filterDate.Year.ToString()),
                                                                Val1 = s.CurrentSold,
                                                                Val2 = s.PreviousSold,
                                                                Color1 = ChartColorBM.Colors[2],
                                                                Color2 = ChartColorBM.Colors[3],
                                                            }).ToList(),
                                                    Bottom = grp.OrderBy(s => s.CurrentSold)
                                                                .Take(5)
                                                                .Select((s, idx) => new CasesSoldSalesPersonChartBM
                                                                {
                                                                    Category = s.Customer,
                                                                    Column1 = filterDate.ToColumnLabelName(filterType, filterDate.Year.ToString(), false),
                                                                    Column2 = filterDate.ToColumnLabelName(filterType, filterDate.Year.ToString()),
                                                                    Val1 = s.CurrentSold,
                                                                    Val2 = s.PreviousSold,
                                                                    Color1 = ChartColorBM.Colors[2],
                                                                    Color2 = ChartColorBM.Colors[3],
                                                                }).ToList()
                                                }



                                            })
                                            .ToList();

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Clustered Stack Chart Data
        /// </summary>
        /// <param name="totalGrocery"></param>
        /// <param name="totalProduce"></param>
        /// <param name="filterType"></param>
        /// <returns></returns>
        private List<ClusteredStackChartBM> GetClusteredStackChartData(List<ClusteredBarChartBM> totalGrocery, List<ClusteredBarChartBM> totalProduce, string filterType)
        {
            var allTotals = new List<ClusteredStackChartBM>();
            string currentYear = DateTime.Now.Year.ToString();

            for (int i = 0; i < totalGrocery.Count(); i++)
            {
                //Current month
                var CurrentMonthclusteredStackChart = new ClusteredStackChartBM
                {
                    Year = filterType == Constants.ByMonth ? Convert.ToDateTime(totalGrocery[i].Column1).ToString("MMM yyyy") : totalGrocery[i].Category,
                    Grocery = totalGrocery[i].Val1.ToString(),
                    Produce = totalProduce[i].Val1.ToString(),
                    Color1 = totalGrocery[i].Category == currentYear ? ChartColorBM.Colors[2] : ChartColorBM.Colors[0],
                    Color2 = totalGrocery[i].Category == currentYear ? ChartColorBM.Colors[3] : ChartColorBM.Colors[1],
                };
                allTotals.Add(CurrentMonthclusteredStackChart);

                //Previous month
                var PreviousMonthclusterdStackChart = new ClusteredStackChartBM
                {
                    Year = filterType == Constants.ByMonth ? Convert.ToDateTime(totalGrocery[i].Column2).ToString("MMM yyyy") : (Convert.ToInt32(totalGrocery[i].Category) - 1).ToString(),
                    Grocery = totalGrocery[i].Val2.ToString(),
                    Produce = totalProduce[i].Val2.ToString(),
                    Color1 = totalGrocery[i].Category == currentYear ? ChartColorBM.Colors[2] : ChartColorBM.Colors[0],
                    Color2 = totalGrocery[i].Category == currentYear ? ChartColorBM.Colors[3] : ChartColorBM.Colors[1],
                };
                allTotals.Add(PreviousMonthclusterdStackChart);
            }
            return allTotals;
        }

        public GenericReoprtMapperWithTopBottom GetCustomerCasesSoldReport(string currentStart, string currentEnd
                                                           , string previousStart, string previousEnd
                                                           , string comodity = "", string category = ""
                                                           , string minSalesAmt = ""
                                                           , string salesPerson = "")
        {
            GenericReoprtMapperWithTopBottom response = new GenericReoprtMapperWithTopBottom();
            using (var adoDataAccess = new DataAccessADO())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();

                parameterList.Add(new SqlParameter("@current_start_date", currentStart));
                parameterList.Add(new SqlParameter("@current_date", currentEnd));
                parameterList.Add(new SqlParameter("@previous_start_date", previousStart));
                parameterList.Add(new SqlParameter("@previous_date", previousEnd));
                parameterList.Add(new SqlParameter("@comodity", comodity));
                parameterList.Add(new SqlParameter("@category", category));
                parameterList.Add(new SqlParameter("@minSalesAmt", minSalesAmt));
                parameterList.Add(new SqlParameter("@salesPerson", salesPerson));

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_GetCustomerCasesSoldReport", parameterList.ToArray());
                var result = dataTableResult.Tables[0]
                                            .AsEnumerable()
                                            .Select(x => new GenericReportMapperBM
                                            {
                                                Customer = x.Field<string>("Customer"),
                                                Current = (double)(x.Field<int?>("CurrentCasesSold") ?? 0),
                                                Previous = (double)(x.Field<int?>("PreviousCasesSold") ?? 0),
                                                Difference = (double)(x.Field<int?>("Difference") ?? 0),
                                                PercentageDifference = ((decimal)(x.Field<int?>("CurrentCasesSold") ?? 0)).ToPercentageDifference((decimal)(x.Field<int?>("PreviousCasesSold") ?? 0))// (x.Field<decimal?>("PercentageDifference") ?? 0).ToRoundDigits(),
                                            }).ToList();
                response.ReportData = result.OrderByDescending(x => x.Difference).ToList();
                response.ChartData = new GenericTopBottomTwoBarChartData
                {
                    Top = result.OrderByDescending(d => d.PercentageDifference).Take(10).Select(x => new GenericTwoBarChartdata
                    {
                        Category = x.Customer,
                        Color1 = ChartColorBM.GrocerryCurrent,
                        Color2 = ChartColorBM.GrocerryPrevious,
                        Label = x.Customer,
                        Value1 = (decimal)x.Current,
                        Value2 = (decimal)x.Previous,
                        Tooltip = x.PercentageDifference
                    }
                    ).ToList(),
                    Bottom = result.OrderBy(d => d.PercentageDifference).Take(10).Select(x => new GenericTwoBarChartdata
                    {
                        Category = x.Customer,
                        Color1 = ChartColorBM.GrocerryCurrent,
                        Color2 = ChartColorBM.GrocerryPrevious,
                        Label = x.Customer,
                        Value1 = (decimal)x.Current,
                        Value2 = (decimal)x.Previous,
                        Tooltip = x.PercentageDifference
                    }
                    ).ToList()
                };

                return response;
            }
        }


        public List<List<string>> GetCasesSoldReportPrerequisites()
        {
            // string sqlString = SQLQueries.GetCasesSoldReportPrerequisitesQuery();
            //using (var adoDataAccess = new DataAccessADO())
            //{
            List<SqlParameter> parameterList = new List<SqlParameter>();
            var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetCasesSoldReportPrerequisites", parameterList.ToArray());

            //  var dataSetResult = adoDataAccess.ReadToDataSet(sqlString);

            var listComodities = dataSetResult.Tables[0]
                                                .AsEnumerable()
                                                .Select(x => x.Field<string>("Comodity").Trim())
                                                .ToList();
            listComodities.Insert(0, "All");

            var listCategories = dataSetResult.Tables[1]
                                                .AsEnumerable()
                                                .Select(x => x.Field<string>("Category").Trim())
                                                .ToList();
            listCategories.Insert(0, "All");
            var result = new List<List<string>> { listComodities, listCategories };

            return result;
            //}

        }

        public List<SalesMapper> GetSalesReportBySalesman(string salesPerspon, int filterId, int period, string comodity,string orderBy="")
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
      
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();


            DateTime CurrentEndDate = targetFilter.Periods.Current.End;
            DateTime HistoricalEndDate = targetFilter.Periods.Historical.End;
            DateTime PriorEndDate = targetFilter.Periods.Prior.End;

            DateTime startDate, endDate, prevStartDate, prevEndDate;
            if (period == (int)PeriodEnum.Historical)
            {
                var filterListsHistorical = GlobaldataProvider.GetFilterWithPeriodsByDate(HistoricalEndDate);
                var targetFilterHistorical = filterListsHistorical.Where(d => d.Id == filterId).FirstOrDefault();
                startDate = targetFilterHistorical.Periods.Current.Start;
                endDate = targetFilterHistorical.Periods.Current.End;
                prevStartDate = targetFilterHistorical.Periods.Historical.Start;
                prevEndDate = targetFilterHistorical.Periods.Historical.End;
            }
            else if (period == (int)PeriodEnum.Prior)
            {
                var filterListsPrior = GlobaldataProvider.GetFilterWithPeriodsByDate(PriorEndDate);
                var targetFilterPrior = filterListsPrior.Where(d => d.Id == filterId).FirstOrDefault();
                startDate = targetFilterPrior.Periods.Current.Start;
                endDate = targetFilterPrior.Periods.Current.End;
                prevStartDate = targetFilterPrior.Periods.Historical.Start;
                prevEndDate = targetFilterPrior.Periods.Historical.End;
            }
            else
            {
                startDate = targetFilter.Periods.Current.Start;
                endDate = targetFilter.Periods.Current.End;
                prevStartDate = targetFilter.Periods.Historical.Start;
                prevEndDate = targetFilter.Periods.Historical.End;
            }

            using (var adoDataAccess = new DataAccessADO())
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@salesman", salesPerspon));
                parameterList.Add(new SqlParameter("@currentStart", startDate));
                parameterList.Add(new SqlParameter("@currentEnd", endDate));
                parameterList.Add(new SqlParameter("@priorStart", prevStartDate));
                parameterList.Add(new SqlParameter("@priorEnd", prevEndDate));
                parameterList.Add(new SqlParameter("@commodity", comodity));
                var dataTableResult = new DataSet();
                if (comodity == "All")
                {
                     dataTableResult = base.ReadToDataSetViaProcedure("BI_CSSL_GetCasesSoldAndSalesBySalesPerson", parameterList.ToArray());
                }
                else
                {
                    dataTableResult = base.ReadToDataSetViaProcedure("BI_CSSL_GetCasesSoldAndSalesBySalesPersonAndCommodity", parameterList.ToArray());
                }
                if (dataTableResult != null && dataTableResult.Tables.Count > 0)
                {
                    var result = dataTableResult.Tables[0]
                                        .AsEnumerable()
                                        .Select(x => new SalesMapper
                                        {
                                            CustomerNumber = x.Field<string>("CompanyNumber"),
                                            Customer = x.Field<string>("Company"),
                                            CasesSoldPrior = x.Field<decimal?>("PriorCasesSold").Value,
                                            SalesQty= x.Field<decimal?>("CurrentCasesSold").Value,
                                            CasesSoldCurrent = x.Field<decimal?>("CurrentCasesSold").Value,
                                            SalesAmountCurrent = x.Field<decimal?>("CurrentSales").Value,
                                            SalesAmountPrior = x.Field<decimal?>("PriorSales").Value,
                                            Percentage = x.Field<decimal?>("GrowthSales").Value,
                                            Difference = x.Field<decimal?>("DifferenceSales").Value,
                                            DifferenceCasesSold = x.Field<decimal?>("DifferenceCasesSold").Value,
                                            PercentageCasesSold = x.Field<decimal?>("GrowthCasesSold").Value,

                                        }).ToList();

                    if (orderBy == "casessold")
                    {
                        return result.OrderByDescending(x => x.CasesSoldCurrent).ToList();
                    }
                    else if (orderBy == "sales")
                    {
                        return result.OrderByDescending(x => x.SalesAmountCurrent).ToList();
                    }
                    return result;
                }
                else
                    return new List<SalesMapper>();
            }
        }

        public List<CustomerMapper> GetSalesReportByCustomer(string customer, int filterId, int period, string comodity,string orderBy)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();


            DateTime CurrentEndDate = targetFilter.Periods.Current.End;
            DateTime HistoricalEndDate = targetFilter.Periods.Historical.End;
            DateTime PriorEndDate = targetFilter.Periods.Prior.End;

            DateTime startDate, endDate, prevStartDate, prevEndDate;
            if (period == (int)PeriodEnum.Historical)
            {
                var filterListsHistorical = GlobaldataProvider.GetFilterWithPeriodsByDate(HistoricalEndDate);
                var targetFilterHistorical = filterListsHistorical.Where(d => d.Id == filterId).FirstOrDefault();
                startDate = targetFilterHistorical.Periods.Current.Start;
                endDate = targetFilterHistorical.Periods.Current.End;
               
            }
            else if (period == (int)PeriodEnum.Prior)
            {
                var filterListsPrior = GlobaldataProvider.GetFilterWithPeriodsByDate(PriorEndDate);
                var targetFilterPrior = filterListsPrior.Where(d => d.Id == filterId).FirstOrDefault();
                startDate = targetFilterPrior.Periods.Current.Start;
                endDate = targetFilterPrior.Periods.Current.End;
               
            }
            else
            {
                startDate = targetFilter.Periods.Current.Start;
                endDate = targetFilter.Periods.Current.End;
             
            }

            using (var adoDataAccess = new DataAccessADO())
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@customerCode", customer));
                parameterList.Add(new SqlParameter("@currentStart", startDate));
                parameterList.Add(new SqlParameter("@currentEnd", endDate));
                parameterList.Add(new SqlParameter("@commodity", comodity));

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_CU_GetInvoiceDetailsByCustomer", parameterList.ToArray());
                if (dataTableResult != null && dataTableResult.Tables.Count > 0)
                {
                    var result = dataTableResult.Tables[0]
                                        .AsEnumerable()
                                        .Select(x => new CustomerMapper
                                        {
                                            Commodity = (!string.IsNullOrEmpty(x.Field<string>("Commodity")) ? x.Field<string>("Commodity").TrimEnd() : string.Empty),
                                            InvoiceNumber = (!string.IsNullOrEmpty(x.Field<string>("InvoiceNumber")) ? x.Field<string>("InvoiceNumber").TrimEnd() : string.Empty),
                                            ItemCode = (!string.IsNullOrEmpty(x.Field<string>("ItemCode")) ? x.Field<string>("ItemCode").TrimEnd() : string.Empty),
                                            ItemDescription = (!string.IsNullOrEmpty(x.Field<string>("ItemDescription")) ? x.Field<string>("ItemDescription").TrimEnd() : string.Empty),
                                            SalesmanCode = (!string.IsNullOrEmpty(x.Field<string>("SalesmanCode")) ? x.Field<string>("SalesmanCode").TrimEnd() : string.Empty),
                                            SalesmanDescription = (!string.IsNullOrEmpty(x.Field<string>("SalesmanDescription")) ? x.Field<string>("SalesmanDescription").TrimEnd() : string.Empty),
                                            SalesOrderNumber = (!string.IsNullOrEmpty(x.Field<string>("SalesOrderNumber")) ? x.Field<string>("SalesOrderNumber").TrimEnd() : string.Empty),
                                            Sales = x.Field<decimal?>("Sales").Value,
                                            CasesSold = x.Field<decimal?>("CasesSold").Value,
                                            InvoiceDate = x.Field<DateTime>("InvoiceDate"),
                                        }).ToList();

                    if (orderBy == "casessold")
                    {
                        return result.OrderByDescending(x => x.CasesSold).ToList();
                    }
                    else if (orderBy == "sales")
                    {
                        return result.OrderByDescending(x => x.Sales).ToList();
                    }
                    return result;
                }
                else
                    return new List<CustomerMapper>();
            }
        }

      
    }
}

