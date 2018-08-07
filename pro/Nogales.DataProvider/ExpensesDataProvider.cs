using Nogales.BusinessModel;
using Nogales.DataProvider.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nogales.DataProvider.Utilities;
using System.Data;
using System.Data.SqlClient;
using Nogales.DataProvider.ENUM;

namespace Nogales.DataProvider
{
    public class ExpensesDataProvider : DataAccessADO
    {

        public ExpensesGlobalCategoryBM GetExpensesByCategoryMonthlyData(GlobalFilter filters,string userId)
        {
            AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
            var userAccessibleCategories = _adminUserManagementDateProvider.GetUserDetails(userId);

            //string sqlString = SQLQueries.GetExpenseByCategoryQuery(current, previous, currentStart, currentEnd, previousMonthStart, previousMonthEnd);
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@currentStart", filters.Periods.Current.Start));
            parameterList.Add(new SqlParameter("@currentEnd", filters.Periods.Current.End));
            parameterList.Add(new SqlParameter("@historicalStart", filters.Periods.Historical.Start));
            parameterList.Add(new SqlParameter("@historicalEnd", filters.Periods.Historical.End));
            parameterList.Add(new SqlParameter("@priorStart", filters.Periods.Prior.Start));
            parameterList.Add(new SqlParameter("@priorEnd", filters.Periods.Prior.End));
            var dataSetResult = base.ReadToDataSetViaProcedure("BI_EP_GetExpenseByCategoryMonth", parameterList.ToArray());
            var result = GetExpensesByCategoryTemp(dataSetResult, filters, userAccessibleCategories);

            return result;
        }
        private ExpensesGlobalCategoryBM GetExpensesByCategoryTemp(DataSet dataTableResult, GlobalFilter filter, UserDetails userAccessibleCategories)
        {

            #region Map to Datatable
            var listTotalCasesSold = dataTableResult.Tables[0]
                                             .AsEnumerable()
                                             .Select(x => new ExpenseCategory
                                             {
                                                 Category = x.Field<string>("Category"),
                                                 Commodity = x.Field<string>("Commodity"),
                                                 Cost = (x.Field<decimal?>("Cost") ?? 0),
                                                 Period = x.Field<string>("Period"),
                                                 AssignedSalesPersonCode = x.Field<string>("AssignedSalesPersonCode"),
                                                 SalesPersonCode = x.Field<string>("SalesPersonCode"),
                                             })
                                             .ToList();

            #endregion

            #region Mapping categories

            var groupedCategory = listTotalCasesSold.GroupBy(x => x.Commodity).ToList();


            var CategoryCases = new ExpensesGlobalCategoryBM
            {


                Total = listTotalCasesSold
                       .GroupBy(x => x.Commodity)
                       .Select((y, secIdx) => new ExpensesCategoryChartGlobalBM
                       {
                           Category = y.Key,
                           Column1 = filter.Periods.Current.Label,
                           Column2 = filter.Periods.Historical.Label,
                           Column3 = filter.Periods.Prior.Label,
                           Val1 = (y.Where(x => x.Period == "current").Sum(t => t.Cost)),
                           Val2 = (y.Where(x => x.Period == "historical").Sum(t => t.Cost)),
                           Val3 = (y.Where(x => x.Period == "prior").Sum(t => t.Cost)),
                           Color1 = y.Key.Trim() == "Grocery" ? BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery : BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,
                           Color2 = y.Key.Trim() == "Grocery" ? BarColumnChartDistinctColors.Colors[secIdx].SecondaryGrocery : BarColumnChartDistinctColors.Colors[secIdx].SecondaryProduce,
                           Color3 = y.Key.Trim() == "Grocery" ? BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery : BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,

                           SubData = y.Where(x => !userAccessibleCategories.IsRestrictedCategoryAccess
                          || userAccessibleCategories.Categories.Select(o => o.Name).Contains(x.Category))

                           .GroupBy(c => c.Category)
                                      .Select((s, idx) => new ExpensesCategoryChartGlobalBM
                                      {
                                          Category = s.Key,
                                          Column1 = filter.Periods.Current.Label,
                                          Column2 = filter.Periods.Historical.Label,
                                          Column3 = filter.Periods.Prior.Label,
                                          Val1 = (s.Where(x => x.Period == "current").Sum(t => t.Cost)),
                                          Val2 = (s.Where(x => x.Period == "historical").Sum(t => t.Cost)),
                                          Val3 = (s.Where(x => x.Period == "prior").Sum(t => t.Cost)),
                                          Color1 = y.Key.Trim() == "Grocery" ? BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery : BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,
                                          Color2 = y.Key.Trim() == "Grocery" ? BarColumnChartDistinctColors.Colors[secIdx].SecondaryGrocery : BarColumnChartDistinctColors.Colors[secIdx].SecondaryProduce,
                                          Color3 = y.Key.Trim() == "Grocery" ? BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery : BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,

                                      })
                                      .ToList()
                       }).ToList(),

            };

            #endregion 
            return CategoryCases;
        }


        public GenericDrillDownChartsChartsBO GetExpenseChartData(GlobalFilter filters, string userId)
        {
            AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
            GenericDrillDownChartsChartsBO response = new GenericDrillDownChartsChartsBO();
            GenericColumnChartCategoryBM total = new GenericColumnChartCategoryBM();

            GenericDrillDownColumnChartCategoryBM totalCasesSold = new GenericDrillDownColumnChartCategoryBM();
            GenericDrillDownColumnChartCategoryBM totalRevenue = new GenericDrillDownColumnChartCategoryBM();
            try
            {

                #region
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filters.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filters.Periods.Current.End));
                parameterList.Add(new SqlParameter("@historicalStart", filters.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filters.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@priorStart", filters.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filters.Periods.Prior.End));

               

                var dataSetResultTotal = base.ReadToDataSetViaProcedure("BI_EP_GetExpenseDashboardChart", parameterList.ToArray());
                var resultTotal = dataSetResultTotal.Tables[0]
                        .AsEnumerable()
                        .Select(x => new CasesSoldRevenueChartBM
                        {
                            Commodity = !string.IsNullOrEmpty(x.Field<string>("Commodity")) ? x.Field<string>("Commodity").Trim() : "",
                            Category = !string.IsNullOrEmpty(x.Field<string>("Category")) ? x.Field<string>("Category").Trim() : "",
                            Sales = x.Field<decimal?>("Sales").Value,
                            CasesSold = x.Field<decimal?>("CasesSold").Value,
                            AssignedSalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("AssignedSalesPersonCode")) ? x.Field<string>("AssignedSalesPersonCode").Trim() : "",
                            SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? x.Field<string>("SalesPersonCode").Trim() : "",
                            SalesPersonDescription = !string.IsNullOrEmpty(x.Field<string>("SalesPersonDescription")) ? x.Field<string>("SalesPersonDescription").Trim() : "",
                            Period = !string.IsNullOrEmpty(x.Field<string>("Period")) ? x.Field<string>("Period").Trim() : "",
                            Cost = x.Field<decimal?>("Cost").Value,
                        })
        .ToList();

                resultTotal = resultTotal.Where(f => f.SalesPersonCode != "DUMP" && f.SalesPersonCode != "DONA").ToList();
                CasesSoldRevenueDataProvider casesSoldRevenuDataProvide = new CasesSoldRevenueDataProvider();

                var userAccessibleCategories = _adminUserManagementDateProvider.GetUserDetails(userId);
              
                totalRevenue.All = MappingToGenericListExpense(resultTotal, filters, userAccessibleCategories);
                response.TotalRevenue = totalRevenue;
                response.TotalCasesSold = totalCasesSold;
                #endregion

                var responseWithDiffPercentageInRevenue = CaluclateDifference(response);
                return responseWithDiffPercentageInRevenue;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
        }
        private double DoubleToPercentageString(decimal d)
        {
            return (double)(Math.Round(d, 2) * 100);
        }
        private decimal CalculateChange(decimal previous, decimal current)
        {
            return UtilityExtensions.CalculateChangeInDecimal(previous, current);
        }
        private GenericDrillDownChartsChartsBO CaluclateDifference(GenericDrillDownChartsChartsBO data)
        {
            var currentMonthAll = data.TotalRevenue.All.ToList()[0].rValue1 + data.TotalRevenue.All.ToList()[0].rValue2;
            var previousMonthAll = data.TotalRevenue.All.ToList()[0].rValue5 + data.TotalRevenue.All.ToList()[0].rValue6;
            var previousYearAll = data.TotalRevenue.All.ToList()[0].rValue3 + data.TotalRevenue.All.ToList()[0].rValue4;
            data.revenueTotalMonthlyDifference = DoubleToPercentageString(CalculateChange(previousMonthAll, currentMonthAll));
            data.revenueTotalYearlyDifference = DoubleToPercentageString(CalculateChange(previousYearAll, currentMonthAll));
            return data;
        }
        private List<GenericDrillDownColumnChartBM> MappingToGenericListExpense(List<CasesSoldRevenueChartBM> list, GlobalFilter filters, UserDetails userAccessibleCategories)
        {
            List<GenericDrillDownColumnChartBM> result = new List<GenericDrillDownColumnChartBM>();
            list = list.OrderByDescending(s => s.Cost).ToList();


            var data = new GenericDrillDownColumnChartBM
            {
                GroupName = "",
                Label1 = filters.Periods.Current.Label,
                rValue1 = (list.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "current").Sum(t => t.Cost)),
                Period1 = filters.Periods.Current.End,
                Color1 = ChartColorBM.GrocerryCurrent,
                SalesPerson1 = "",

                Label2 = filters.Periods.Current.Label,
                rValue2 = (list.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "current").Sum(t => t.Cost)),
                Period2 = filters.Periods.Current.End,
                Color2 = ChartColorBM.ProduceCurrent,
                SalesPerson2 = "",

                Label3 = filters.Periods.Historical.Label,
                rValue3 = (list.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "historical").Sum(t => t.Cost)),
                Period3 = filters.Periods.Historical.End,
                Color3 = ChartColorBM.GrocerryPrevious,
                SalesPerson3 = "",

                Label4 = filters.Periods.Historical.Label,
                rValue4 = (list.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "historical").Sum(t => t.Cost)),
                Period4 = filters.Periods.Historical.End,
                Color4 = ChartColorBM.ProducePrevious,
                SalesPerson4 = "",

                Label5 = filters.Periods.Prior.Label,
                rValue5 = (list.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "prior").Sum(t => t.Cost)),
                Period5 = filters.Periods.Prior.End,
                Color5 = ChartColorBM.GrocerryCurrent,
                SalesPerson5 = "",

                Label6 = filters.Periods.Prior.Label,
                rValue6 = (list.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "prior").Sum(t => t.Cost)),
                Period6 = filters.Periods.Prior.End,
                Color6 = ChartColorBM.ProduceCurrent,
                SalesPerson6 = "",



                SubData = list.OrderByDescending(a => a.Sales).Where(x => !userAccessibleCategories.IsRestrictedCategoryAccess
               || userAccessibleCategories.Categories.Select(o => o.Name).Contains(x.Category))
                //.Where(x => x.Category != Constants.CategorySchools && x.Category != Constants.CategoryHealthcare && x.Category != Constants.CategoryInstitute)
                    .GroupBy(x => x.Category)//.Take(5)
                    .Select((sec, secIdx) => new GenericDrillDownColumnChartBM
                    {
                        GroupName = sec.Key.ToSalesCategoryDisplayName(),
                        Label1 = filters.Periods.Current.Label,
                        rValue1 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "current").Sum(t => t.Cost)),
                        Period1 = filters.Periods.Current.End,
                        Color1 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                        SalesPerson1 = "",

                        Label2 = filters.Periods.Current.Label,
                        rValue2 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "current").Sum(t => t.Cost)),
                        Period2 = filters.Periods.Current.End,
                        Color2 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                        SalesPerson2 = "",

                        Label3 = filters.Periods.Historical.Label,
                        rValue3 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "historical").Sum(t => t.Cost)),
                        Period3 = filters.Periods.Historical.End,
                        Color3 = BarColumnChartDistinctColors.Colors[secIdx].SecondaryGrocery,//ChartColorBM.GrocerryPrevious,
                        SalesPerson3 = "",

                        Label4 = filters.Periods.Historical.Label,
                        rValue4 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "historical").Sum(t => t.Cost)),
                        Period4 = filters.Periods.Historical.End,
                        Color4 = BarColumnChartDistinctColors.Colors[secIdx].SecondaryProduce,//ChartColorBM.ProducePrevious,
                        SalesPerson4 = "",

                        Label5 = filters.Periods.Prior.Label,
                        rValue5 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "prior").Sum(t => t.Cost)),
                        Period5 = filters.Periods.Prior.End,
                        Color5 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                        SalesPerson5 = "",

                        Label6 = filters.Periods.Prior.Label,
                        rValue6 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "prior").Sum(t => t.Cost)),
                        Period6 = filters.Periods.Prior.End,
                        Color6 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                        SalesPerson6 = "",

                        SubData = sec.Where(w => w.Category == sec.Key).OrderByDescending(a => a.Cost)
                        .GroupBy(x => x.AssignedSalesPersonCode).Where(p => p.Any(x => x.Period == "current"))
                        //.Take(5)
                        .Select((thir, thirIdx) => new GenericDrillDownColumnChartBM
                        {
                            GroupName = GetSalemanNameFromGroup(thir),
                            //GetActiveSalesManName(!string.IsNullOrEmpty(thir.First().SalesPersonDescription) ? thir.First().SalesPersonDescription : thir.Key),
                            ActiveSalesPersonCode = thir.Key,
                            Label1 = filters.Periods.Current.Label,
                            rValue1 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "current").Sum(t => t.Cost)),
                            Period1 = filters.Periods.Current.End,
                            Color1 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                            SalesPerson1 = GetSalemanNameFromGroup(thir),

                            Label2 = filters.Periods.Current.Label,
                            rValue2 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "current").Sum(t => t.Cost)),
                            Period2 = filters.Periods.Current.End,
                            Color2 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                            SalesPerson2 = GetSalesManName(thir.First().SalesPersonDescription),

                            Label3 = filters.Periods.Historical.Label,
                            rValue3 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "historical").Sum(t => t.Cost)),
                            Period3 = filters.Periods.Historical.End,
                            Color3 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryGrocery,//ChartColorBM.GrocerryPrevious,
                            SalesPerson3 = GetSalesManName(thir.First().SalesPersonDescription),

                            Label4 = filters.Periods.Historical.Label,
                            rValue4 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "historical").Sum(t => t.Cost)),
                            Period4 = filters.Periods.Historical.End,
                            Color4 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryProduce,//ChartColorBM.ProducePrevious,
                            SalesPerson4 = GetSalesManName(thir.First().SalesPersonDescription),

                            Label5 = filters.Periods.Prior.Label,
                            rValue5 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "prior").Sum(t => t.Cost)),
                            Period5 = filters.Periods.Prior.End,
                            Color5 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                            SalesPerson5 = GetSalesManName(thir.First().SalesPersonDescription),

                            Label6 = filters.Periods.Prior.Label,
                            rValue6 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "prior").Sum(t => t.Cost)),
                            Period6 = filters.Periods.Prior.End,
                            Color6 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                            SalesPerson6 = GetSalesManName(thir.First().SalesPersonDescription),

                        }).ToList().OrderByDescending(s => s.rValue1 + s.rValue2).ToList()
                    }).ToList().OrderByDescending(s => s.rValue1 + s.rValue2).ToList()
            };
            result.Add(data);
            return result;
        }

        private string GetSalesManName(string nameCombination)
        {
            List<string> fullname = new List<string>();
            var names = nameCombination.Split('|');
            foreach (var name in names)
            {
                var singlePerson = name.Split(',');
                if (singlePerson.Length > 1)
                {
                    var personCode = singlePerson.Last().Split(new string[] { " (" }, StringSplitOptions.None);
                    fullname.Add(singlePerson.First() + " " + personCode.First().Trim().Substring(0, 1));// + " (" + personCode.Last());
                }
                else
                {
                    fullname.Add(singlePerson.LastOrDefault());
                }

            }
            return string.Join(",", fullname);

        }
        private string GetSalemanNameFromGroup(IGrouping<string, CasesSoldRevenueChartBM> thir)
        {
            string salesmanName = string.Empty;
            if (thir.Any(x => x.Period == "current"))
            {
                salesmanName = GetSalesManName(thir.Where(x => x.Period == "current").FirstOrDefault().SalesPersonDescription);
            }
            else
            {
                salesmanName = GetSalesManName(thir.First().SalesPersonDescription);
            }
            return salesmanName;

        }

        //public ExpensesGlobalCategoryBM GetExpensesByCategoryMonthlyData(GlobalFilter filters)
        //{

        //    //string sqlString = SQLQueries.GetExpenseByCategoryQuery(current, previous, currentStart, currentEnd, previousMonthStart, previousMonthEnd);
        //    List<SqlParameter> parameterList = new List<SqlParameter>();
        //    parameterList.Add(new SqlParameter("@currentStart", filters.Periods.Current.Start));
        //    parameterList.Add(new SqlParameter("@currentEnd", filters.Periods.Current.End));
        //    parameterList.Add(new SqlParameter("@historicalStart", filters.Periods.Historical.Start));
        //    parameterList.Add(new SqlParameter("@historicalEnd", filters.Periods.Historical.End));
        //    parameterList.Add(new SqlParameter("@priorStart", filters.Periods.Prior.Start));
        //    parameterList.Add(new SqlParameter("@priorEnd", filters.Periods.Prior.End));
        //    var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetExpenseByCategoryMonth", parameterList.ToArray());
        //    var result = GetExpensesByCategory(dataSetResult, filters);

        //    return result;
        //}

        public ExpensesCategoryBM GetExpensesByCategoryMonthlyDataTemp(DateTime filterDate
                                                                          , string current, string previous
                                                                          , string currentStart, string currentEnd
                                                                          , string previousMonthStart, string previousMonthEnd)
        {


            //string sqlString = SQLQueries.GetExpenseByCategoryQuery(current, previous, currentStart, currentEnd, previousMonthStart, previousMonthEnd);
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@current", current));
            parameterList.Add(new SqlParameter("@previous", previous));
            parameterList.Add(new SqlParameter("@current_date_start", currentStart));
            parameterList.Add(new SqlParameter("@current_date", currentEnd));
            parameterList.Add(new SqlParameter("@previous_date_start", previousMonthStart));
            parameterList.Add(new SqlParameter("@previous_date", previousMonthEnd));
            var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetExpenseByCategoryMonth", parameterList.ToArray());
            var result = GetExpensesByCategoryTemp(current.ToString(), previous.ToString(), dataSetResult, filterDate, Constants.ByMonth);

            return result;
        }

        public ExpensesCategoryBM GetExpensesCategoryByYTD_YTC(DateTime filterDate
                                                                           , string current, string previous
                                                                           , string currentStart, string currentEnd
                                                                           , string previousYearStart, string previousYearEnd)
        {

            //string sqlString = MDXCubeQueries.GetExpensesByCategoryYearToDate(dateFormattedString);GetExpenseByCategoryYearQuery
            string sqlString = SQLQueries.GetExpenseByCategoryYearQuery(current, previous, currentStart, currentEnd, previousYearStart, previousYearEnd);
            var dataTableResult = new DataSet();
            using (var adoDataAccess = new DataAccessADO())
            {
                dataTableResult = adoDataAccess.ReadToDataSet(sqlString);
            }

            var result = GetExpensesByCategoryQuery(current, previous, sqlString, filterDate, Constants.ByYear);

            return result;
        }


        private ExpensesGlobalCategoryBM GetExpensesByCategory(DataSet dataTableResult, GlobalFilter filter)
        {
           
            #region Map to Datatable
            var listTotalCasesSold = dataTableResult.Tables[0]
                                             .AsEnumerable()
                                             .Select(x => new ExpensesDataMapperGlobal
                                             {
                                                 Category = x.Field<string>("Category"),
                                                 Comodity = x.Field<string>("Comodity"),                           
                                                 CurrentSold = (x.Field<decimal?>("currentAmt") ?? 0).ToRoundTwoDecimalDigits(),
                                                 HistoricalSold = (x.Field<decimal?>("historicalAmt") ?? 0).ToRoundTwoDecimalDigits(),
                                                 PriorSold = (x.Field<decimal?>("priorAmt") ?? 0).ToRoundTwoDecimalDigits(),
                                             })
                                             .ToList();
            #endregion

            #region Mapping categories
            var CategoryCases = new ExpensesGlobalCategoryBM
            {

                Total = listTotalCasesSold
                       .GroupBy(x => x.Comodity)
                       .Select(y => new ExpensesCategoryChartGlobalBM
                       {
                           Category = y.Key,
                           Column1 = filter.Periods.Current.Label,
                           Column2 = filter.Periods.Historical.Label,
                           Column3 = filter.Periods.Prior.Label,
                           Val1 = Convert.ToDecimal(y.Sum(t => t.CurrentSold) ),
                           Val2 = Convert.ToDecimal(y.Sum(t => t.HistoricalSold)),
                           Val3 = Convert.ToDecimal(y.Sum(t => t.PriorSold) ),
                           Color1 = y.Key.Trim() == "Grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                           Color2 = y.Key.Trim() == "Grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                           Color3 = y.Key.Trim() == "Grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                           //Color1 = y.Key=="Grocery"? ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                           //Color2 = y.Key == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0],
                           //Color1 = y.First().Category =="Grocery"?ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                           //Color2 = y.First().Category == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0],
                           SubData = y.GroupBy(c => c.Category)
                                      .Select((s, idx) => new ExpensesCategoryChartGlobalBM
                                      {
                                          Category = s.Key,
                                          Column1 = filter.Periods.Current.Label,
                                          Column2 = filter.Periods.Historical.Label,
                                          Column3 = filter.Periods.Prior.Label,
                                          Val1 = Convert.ToDecimal((s.Sum(t => t.CurrentSold))),
                                          Val2 = Convert.ToDecimal(s.Sum(t => t.HistoricalSold)),
                                          Val3 = Convert.ToDecimal(s.Sum(t => t.PriorSold)),
                                          Color1 = y.Key.Trim() == "Grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                                          Color2 = y.Key.Trim() == "Grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                                          Color3 = y.Key.Trim() == "Grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                                          //Color1 = y.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                                          //Color2 = y.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0],
                                      })
                                      .ToList()
                       }).ToList(),

            };

            #endregion
            return CategoryCases;
        }

        private ExpensesCategoryBM GetExpensesByCategoryTemp(string currentName, string previousName, DataSet dataTableResult, DateTime filterDate, string filterType)
        {
           
            #region Map to Datatable
            var listTotalCasesSold = dataTableResult.Tables[0]
                                             .AsEnumerable()
                                             .Select(x => new ExpensesDataMapper
                                             {
                                                 Category = x.Field<string>("Category"),
                                                 Comodity = x.Field<string>("Comodity"),
                                                 //DateSold = DateTime.Parse(x.Field<string>(4).ToString()),
                                                 //SalesPerson =
                                                 CurrentSold = (x.Field<decimal?>("currentAmt") ?? 0).ToRoundTwoDecimalDigits(),
                                                 PreviousSold = (x.Field<decimal?>("previousAmt") ?? 0).ToRoundTwoDecimalDigits(),
                                                 Year = filterType == Constants.ByMonth ? (x.Field<int>("InvoiceYear")) : filterDate.Year,
                                             })
                                             .ToList();
            #endregion

            #region Mapping categories
            var CategoryCases = new ExpensesCategoryBM
            {

                Total = listTotalCasesSold
                       .GroupBy(x => x.Comodity)
                       .Select(y => new ExpensesCategoryChartBM
                       {
                           Category = y.Key,
                           Column1 = filterDate.ToColumnLabelName(filterType, y.First().Year.ToString(), false),
                           Column2 = filterDate.ToColumnLabelName(filterType, y.First().Year.ToString()),
                           Val1 = (y.Sum(t => t.CurrentSold) ?? 0).ToRoundTwoDigits(),
                           Val2 = (y.Sum(t => t.PreviousSold) ?? 0).ToRoundTwoDigits(),
                           Color1 = y.Key.Trim() == "Grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                           Color2 = y.Key.Trim() == "Grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                           //Color1 = y.Key=="Grocery"? ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                           //Color2 = y.Key == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0],
                           //Color1 = y.First().Category =="Grocery"?ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                           //Color2 = y.First().Category == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0],
                           SubData = y.GroupBy(c => c.Category)
                                      .Select((s, idx) => new ExpensesCategoryChartBM
                                      {
                                          Category = s.Key,
                                          Column1 = filterDate.ToColumnLabelName(filterType, y.First().Year.ToString(), false),
                                          Column2 = filterDate.ToColumnLabelName(filterType, y.First().Year.ToString()),
                                          Val1 = (s.Sum(t => t.CurrentSold) ?? 0).ToRoundTwoDigits(),
                                          Val2 = (s.Sum(t => t.PreviousSold) ?? 0).ToRoundTwoDigits(),
                                          Color1 = y.Key.Trim() == "Grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                                          Color2 = y.Key.Trim() == "Grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                                          //Color1 = y.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                                          //Color2 = y.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0],
                                      })
                                      .ToList()
                       }).ToList(),


                //Buyer = listTotalCasesSold.GetExpensesFromCategory((x => x.Category == "BUYER"), currentName, previousName),
                //FoodService = listTotalCasesSold.GetExpensesFromCategory((x => x.Category == "FOOD SERVI"), currentName, previousName),
                //Carniceria = listTotalCasesSold.GetExpensesFromCategory((x => x.Category == "CARNICERIA"), currentName, previousName),
                //LossProd = listTotalCasesSold.GetExpensesFromCategory((x => x.Category == "LOSS PROD"), currentName, previousName),
                //National = listTotalCasesSold.GetExpensesFromCategory((x => x.Category == "NATIONAL"), currentName, previousName),
                //Wholesaler = listTotalCasesSold.GetExpensesFromCategory((x => x.Category == "WHOLESALER"), currentName, previousName),
                //WillCall = listTotalCasesSold.GetExpensesFromCategory((x => x.Category == "WILL CALL"), currentName, previousName),
                //Retail = listTotalCasesSold.GetExpensesFromCategory((x => x.Category.StartsWith("RETAIL")), currentName, previousName),
                //AllOthers = listTotalCasesSold.GetExpensesFromCategory(((x => !x.Category.StartsWith("RETAIL") &&
                //                                                            x.Category != "BUYER" &&
                //                                                            x.Category != "FOOD SERVI" &&
                //                                                            x.Category != "CARNICERIA" &&
                //                                                            x.Category != "LOSS PROD" &&
                //                                                            x.Category != "NATIONAL" &&
                //                                                            x.Category != "WHOLESALER" &&
                //                                                            x.Category != "WILL CALL")), currentName, previousName),


            };

            #endregion
            return CategoryCases;
        }

        private ExpensesCategoryBM GetExpensesByCategoryQuery(string currentName, string previousName, string sqlString, DateTime filterDate, string filterType)
        {
            //var dataTableResult = base.GetDataTable(sqlString);
            //var listColumns = dataTableResult.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            var dataTableResult = new DataSet();
            using (var adoDataAccess = new DataAccessADO())
            {
                dataTableResult = adoDataAccess.ReadToDataSet(sqlString);
            }
            #region Map to Datatable
            var listTotalCasesSold = dataTableResult.Tables[0]
                                             .AsEnumerable()
                                             .Select(x => new ExpensesDataMapper
                                             {
                                                 Category = x.Field<string>("Category"),
                                                 Comodity = x.Field<string>("Comodity"),
                                                 //DateSold = DateTime.Parse(x.Field<string>(4).ToString()),
                                                 //SalesPerson =
                                                 CurrentSold = (x.Field<decimal?>("currentAmt") ?? 0).ToRoundTwoDecimalDigits(),
                                                 PreviousSold = (x.Field<decimal?>("previousAmt") ?? 0).ToRoundTwoDecimalDigits(),
                                                 Year = filterType == Constants.ByMonth ? (x.Field<int>("InvoiceYear")) : filterDate.Year,
                                             })
                                             .ToList();
            #endregion

            #region Mapping categories
            var CategoryCases = new ExpensesCategoryBM
            {

                Total = listTotalCasesSold
                       .GroupBy(x => x.Comodity)
                       .Select(y => new ExpensesCategoryChartBM
                       {
                           Category = y.Key,
                           Column1 = filterDate.ToColumnLabelName(filterType, y.First().Year.ToString(), false),
                           Column2 = filterDate.ToColumnLabelName(filterType, y.First().Year.ToString()),
                           Val1 = (y.Sum(t => t.CurrentSold) ?? 0).ToRoundTwoDigits(),
                           Val2 = (y.Sum(t => t.PreviousSold) ?? 0).ToRoundTwoDigits(),
                           Color1 = y.Key.Trim() == "Grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                           Color2 = y.Key.Trim() == "Grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                           //Color1 = y.Key=="Grocery"? ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                           //Color2 = y.Key == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0],
                           //Color1 = y.First().Category =="Grocery"?ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                           //Color2 = y.First().Category == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0],
                           SubData = y.GroupBy(c => c.Category)
                                      .Select((s, idx) => new ExpensesCategoryChartBM
                                      {
                                          Category = s.Key,
                                          Column1 = filterDate.ToColumnLabelName(filterType, y.First().Year.ToString(), false),
                                          Column2 = filterDate.ToColumnLabelName(filterType, y.First().Year.ToString()),
                                          Val1 = (s.Sum(t => t.CurrentSold) ?? 0).ToRoundTwoDigits(),
                                          Val2 = (s.Sum(t => t.PreviousSold) ?? 0).ToRoundTwoDigits(),
                                          Color1 = y.Key.Trim() == "Grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                                          Color2 = y.Key.Trim() == "Grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                                          //Color1 = y.First().Comodity == "Grocery" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[2],
                                          //Color2 = y.First().Comodity == "Grocery" ? ChartColorBM.Colors[1] : ChartColorBM.Colors[0],
                                      })
                                      .ToList()
                       }).ToList(),


                //Buyer = listTotalCasesSold.GetExpensesFromCategory((x => x.Category == "BUYER"), currentName, previousName),
                //FoodService = listTotalCasesSold.GetExpensesFromCategory((x => x.Category == "FOOD SERVI"), currentName, previousName),
                //Carniceria = listTotalCasesSold.GetExpensesFromCategory((x => x.Category == "CARNICERIA"), currentName, previousName),
                //LossProd = listTotalCasesSold.GetExpensesFromCategory((x => x.Category == "LOSS PROD"), currentName, previousName),
                //National = listTotalCasesSold.GetExpensesFromCategory((x => x.Category == "NATIONAL"), currentName, previousName),
                //Wholesaler = listTotalCasesSold.GetExpensesFromCategory((x => x.Category == "WHOLESALER"), currentName, previousName),
                //WillCall = listTotalCasesSold.GetExpensesFromCategory((x => x.Category == "WILL CALL"), currentName, previousName),
                //Retail = listTotalCasesSold.GetExpensesFromCategory((x => x.Category.StartsWith("RETAIL")), currentName, previousName),
                //AllOthers = listTotalCasesSold.GetExpensesFromCategory(((x => !x.Category.StartsWith("RETAIL") &&
                //                                                            x.Category != "BUYER" &&
                //                                                            x.Category != "FOOD SERVI" &&
                //                                                            x.Category != "CARNICERIA" &&
                //                                                            x.Category != "LOSS PROD" &&
                //                                                            x.Category != "NATIONAL" &&
                //                                                            x.Category != "WHOLESALER" &&
                //                                                            x.Category != "WILL CALL")), currentName, previousName),


            };

            #endregion
            return CategoryCases;
        }

      
        public List<GenericDrillDownBaseColumnChartBM> GetTopTenExpenses(string startDate, string endDate)
        {


            var startDateObj = new DateTime(); ;
            var endDateObj = new DateTime(); DateTime.Parse(endDate);

            if (!(DateTime.TryParse(startDate, out startDateObj) && DateTime.TryParse(endDate, out endDateObj)))
            {
                throw new Exception("Date format Incorrect.");
            }

            var startDateFormatted = startDateObj.ToString("yyyy/MM/dd");
            var endDateFormatted = endDateObj.ToString("yyyy/MM/dd");
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@start", startDateFormatted));
            parameterList.Add(new SqlParameter("@end", endDateFormatted));
            var result = base.ReadToDataSetViaProcedure("BI_GetTopTenExpenses", parameterList.ToArray());
            //var query = SQLQueries.GetTopTenExpensesQuery(startDateFormatted, endDateFormatted);
            //var result = adoDataAccess.ReadToDataSet(query);

            var topTenExpensesResult = result.Tables[0]
                                            .AsEnumerable()
                                            .Select((x, thirIdx) => new GenericDrillDownBaseColumnChartBM
                                            {
                                                cValue1 = x.Field<decimal>("TotalExpense"),
                                                Label1  = x.Field<string>("Description"),
                                                GroupName= x.Field<string>("Description"),
                                                cValue2=0,
                                                Color1  = TopBottom25GraphColors.Colors[thirIdx].Primary,
                                                //  Description = x.Field<string>("Description"),
                                                //  TotalExpense = Convert.ToInt32(x.Field<decimal>("TotalExpense")),
                                            })
                                            .ToList();
         //   topTenExpensesResult.Add(new ExpensesAmountChartBM {Description="Test test",TotalExpense=-50 });
          //  topTenExpensesResult.Add(new ExpensesAmountChartBM { Description = "Test test 2", TotalExpense = -60 });
            return topTenExpensesResult;


        }

        /// <summary>
        /// Get Expenses Statitics
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ExpenseStatistics GetExpensesStatitics(GlobalFilter filters)
        {
            ExpenseStatistics model = new ExpenseStatistics();
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@currentStart", filters.Periods.Current.Start));
            parameterList.Add(new SqlParameter("@currentEnd", filters.Periods.Current.End));
            parameterList.Add(new SqlParameter("@historicalStart", filters.Periods.Historical.Start));
            parameterList.Add(new SqlParameter("@historicalEnd", filters.Periods.Historical.End));
            parameterList.Add(new SqlParameter("@priorStart", filters.Periods.Prior.Start));
            parameterList.Add(new SqlParameter("@priorEnd", filters.Periods.Prior.End));
            var resultData = base.ReadToDataSetViaProcedure("BI_GetPayrollDetailsWithHistory", parameterList.ToArray());
            var datatableToList = GetEmployeePayrollDetails(resultData.Tables[0]);

            List<SqlParameter> parameterList2 = new List<SqlParameter>();
            parameterList2.Add(new SqlParameter("@currentStart", filters.Periods.Current.Start));
            parameterList2.Add(new SqlParameter("@currentEnd", filters.Periods.Current.End));
            parameterList2.Add(new SqlParameter("@historicalStart", filters.Periods.Historical.Start));
            parameterList2.Add(new SqlParameter("@historicalEnd", filters.Periods.Historical.End));
            parameterList2.Add(new SqlParameter("@priorStart", filters.Periods.Prior.Start));
            parameterList2.Add(new SqlParameter("@priorEnd", filters.Periods.Prior.End));
            var AdminExpenses = base.ReadToDataSetViaProcedure("BI_GetAdminExpensesWithHistory", parameterList2.ToArray());
            var AdminExpensesList = AdminExpenses.Tables[0].AsEnumerable()
                    .Select(x => new MapAdminExpensesData
                    {
                        Date = x.Field<DateTime>("date"),
                        Description = x.Field<string>("Description"),
                        Expense = x.Field<decimal?>("Expense") ?? 0,
                        Period = x.Field<string>("Period"),
                    }).ToList();



            model = CalculateStatitics(datatableToList, AdminExpensesList, filters);
            return model;
        }


        /// <summary>
        /// Get Expenses Statitics
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ExpenseStatistics GetExpensesStatiticsTemp(DateTime date)
        {
            ExpenseStatistics model = new ExpenseStatistics();

            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@endDate", date));
            var resultData = base.ReadToDataSetViaProcedure("BI_GetPayrollDetailsWithHistory", parameterList.ToArray());
            var datatableToList = GetEmployeePayrollDetails(resultData.Tables[0]);

            List<SqlParameter> parameterList2 = new List<SqlParameter>();
            parameterList2.Add(new SqlParameter("@endDate", date));
            var AdminExpenses = base.ReadToDataSetViaProcedure("BI_GetAdminExpensesWithHistory", parameterList2.ToArray());
            var AdminExpensesList = AdminExpenses.Tables[0].AsEnumerable()
                    .Select(x => new MapAdminExpensesData
                    {
                        Date = x.Field<DateTime>("date"),
                        Description = x.Field<string>("Description"),
                        Expense = x.Field<decimal?>("Expense") ?? 0
                    }).ToList();



            model = CalculateStatiticsTemp(datatableToList, AdminExpensesList, date);
            return model;
        }
        public List<MapEmployeePayrollData> GetEmployeePayrollDetails(DataTable dt)
        {


            var data = dt.AsEnumerable()
                    .Select(x => new MapEmployeePayrollData
                    {
                        Date = x.Field<DateTime>("DateAdded"),
                        Year = x.Field<int>("Year"),
                        Month = x.Field<int>("Month"),
                        CompanyId = x.Field<string>("CompanyId"),
                        CompanyName = x.Field<string>("CompanyName"),
                        LName = x.Field<string>("LName"),
                        FName = x.Field<string>("FName"),
                        EmployeeId = x.Field<string>("EmpId"),
                        Level = x.Field<string>("Level"),
                        Position = x.Field<string>("Position"),
                        Amount = x.Field<decimal?>("Amount") ?? 0,
                        Regular = x.Field<decimal?>("Regular") ?? 0,
                        OverTime = x.Field<decimal?>("Overtime") ?? 0,
                        EarningsTotal = x.Field<decimal?>("EarningsTotal") ?? 0,
                        DeductionTotal = x.Field<decimal?>("DeductionTotal") ?? 0,
                        LiabilityTotal = x.Field<decimal?>("LiabilityTotal") ?? 0,
                        Period = x.Field<string>("Period"),
                    }).ToList();
            return data;

        }


        public ExpenseStatistics CalculateStatitics(List<MapEmployeePayrollData> data, List<MapAdminExpensesData> adminExpenses, GlobalFilter filters)
        {
            ExpenseStatistics model = new ExpenseStatistics();

            //Wages
            var wagesCurrent = data.Where(s => s.Period == "current").Sum(d => d.Amount);
            var wagesPrevious = data.Where(s => s.Period == "prior").Sum(d => d.Amount);
            model.Wages = GetStatitics(wagesCurrent, wagesPrevious);

            //Ot Wages
            var otWagesCurrent = data.Where(s => s.Period == "current").Sum(d => d.OverTime);
            var otWagesPrevious = data.Where(s => s.Period == "prior").Sum(d => d.OverTime);
            model.OtWages = GetStatitics(otWagesCurrent, otWagesPrevious);

            //AdminExpeses
            var adminExpCurrent = adminExpenses.Where(s => s.Period == "current").Sum(d => d.Expense);
            var adminExpPrevious = adminExpenses.Where(s => s.Period == "prior").Sum(d => d.Expense);
            model.AdminExp = GetStatitics(adminExpCurrent, adminExpPrevious);

            return model;
        }

        public ExpenseStatistics CalculateStatiticsTemp(List<MapEmployeePayrollData> data, List<MapAdminExpensesData> adminExpenses, DateTime date)
        {
            ExpenseStatistics model = new ExpenseStatistics();

            //Wages
            var wagesCurrent = data.Where(s => s.Date.Year == date.Year && s.Date.Month == date.Month).Sum(d => d.Amount);
            var wagesPrevious = data.Where(s => s.Date.Month == date.AddMonths(-1).Month).Sum(d => d.Amount);
            model.Wages = GetStatitics(wagesCurrent, wagesPrevious);

            //Ot Wages
            var otWagesCurrent = data.Where(s => s.Date.Year == date.Year && s.Date.Month == date.Month).Sum(d => d.OverTime);
            var otWagesPrevious = data.Where(s => s.Date.Month == date.AddMonths(-1).Month).Sum(d => d.OverTime);
            model.OtWages = GetStatitics(otWagesCurrent, otWagesPrevious);

            //AdminExpeses
            var adminExpCurrent = adminExpenses.Where(s => s.Date.Year == date.Year && s.Date.Month == date.Month).Sum(d => d.Expense);
            var adminExpPrevious = adminExpenses.Where(s => s.Date.Month == date.AddMonths(-1).Month).Sum(d => d.Expense);
            model.AdminExp = GetStatitics(adminExpCurrent, adminExpPrevious);

            return model;
        }

        public double GetStatitics(decimal current, decimal previous)
        {
            if (current == 0 && previous == 0)
                return 0;
            if (previous == 0)
                return 100;
            return (((current - previous) / previous) * 100).ToRoundTwoDecimalDigits();
        }

        /// <summary>
        /// Get Comodity Expense
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<ExpensesData> GetComodityExpenseReport(DateTime startDate, DateTime endDate, string userId, string comodity = "")
        {
            List<ExpensesData> result = new List<ExpensesData>();
            try
            {

                AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", startDate));
                parameterList.Add(new SqlParameter("@currentEnd", endDate));
                parameterList.Add(new SqlParameter("@priorStart", startDate.AddMonths(-1)));
                parameterList.Add(new SqlParameter("@priorEnd", endDate.AddMonths(-1)));
                parameterList.Add(new SqlParameter("@commodity", comodity));
                var dataSet = base.ReadToDataSetViaProcedure("BI_EP_ComodityExpenseReport", parameterList.ToArray());
                result = dataSet.Tables[0].AsEnumerable()
                     .Select(x => new ExpensesData
                     {
                         CurrentLabel = startDate.ToString("MMM yy"),
                         PriorLabel = startDate.AddMonths(-1).ToString("MMM yy"),
                         Comodity = !string.IsNullOrEmpty(x.Field<string>("Commodity")) ? x.Field<string>("Commodity").Trim() : "",
                         CurrentExpense = x.Field<decimal>("current"),
                         PriorExpense = x.Field<decimal>("prior"),
                         CurrentRevenue = x.Field<decimal>("rCurrent"),
                         PriorRevenue = x.Field<decimal>("rPrior"),
                         Category = !string.IsNullOrEmpty(x.Field<string>("Category")) ? x.Field<string>("Category").Trim() : "",
                         Percentage = (decimal)(x.Field<decimal?>("current").HasValue ? x.Field<decimal?>("current").Value : 0).ToPercentageDifference(x.Field<decimal?>("prior").HasValue ? x.Field<decimal?>("prior").Value : 0),
                     })
                     .OrderByDescending(p=>p.CurrentExpense)
                     .ToList();

                var userAccessibleCategories = _adminUserManagementDateProvider.GetUserDetails(userId);
                result = result.Where(x => !userAccessibleCategories.IsRestrictedCategoryAccess
                 || userAccessibleCategories.Categories.Select(o => o.Name).Contains(x.Category)).ToList();
            }
            catch (Exception ex)
            { }
            return result;

        }

        public List<SalesMapper> GetExpenseReportBySalesman(string salesPerspon, int filterId, int period, string comodity, bool isCM01=false, string orderBy = "")
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
                    if (isCM01)
                    {
                        dataTableResult = base.ReadToDataSetViaProcedure("BI_EP_GetExpenseBySalesPersonForCustomerService", parameterList.ToArray());
                    }
                    else
                    {
                        dataTableResult = base.ReadToDataSetViaProcedure("BI_EP_GetExpenseBySalesPerson", parameterList.ToArray());
                    }
                }
                else
                {
                    if (isCM01)
                    {
                        dataTableResult = base.ReadToDataSetViaProcedure("BI_EP_GetExpenseBySalesPersonCommodityForCustomerService", parameterList.ToArray());
                    }
                    else
                    {
                        dataTableResult = base.ReadToDataSetViaProcedure("BI_EP_GetExpenseBySalesPersonWithCommodity", parameterList.ToArray());
                    }
                }
                if (dataTableResult != null && dataTableResult.Tables.Count > 0)
                {
                    var result = dataTableResult.Tables[0]
                                        .AsEnumerable()
                                        .Select(x => new SalesMapper
                                        {
                                            CustomerNumber    = x.Field<string>("CompanyNumber"),
                                            Customer          = x.Field<string>("Company"),                                         
                                            DifferenceExpense = x.Field<decimal?>("DifferenceExpense").Value,
                                            PercentageExpense = x.Field<decimal?>("GrowthExpense").Value,
                                            ExpenseCurrent    = x.Field<decimal?>("CurrentCost").Value,
                                            ExpensePrior      = x.Field<decimal?>("PriorCost").Value
                                        }).ToList();

                   
                        return result.OrderByDescending(x => x.ExpenseCurrent).ToList();
                 
                   
                }
                else
                    return new List<SalesMapper>();
            }
        } 

        public GenericDrillDownChartsChartsBO GetCustomerServiceExpenseChart(GlobalFilter filters, string userId)
        {
            AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
            GenericDrillDownChartsChartsBO response = new GenericDrillDownChartsChartsBO();
            GenericColumnChartCategoryBM total = new GenericColumnChartCategoryBM();

            GenericDrillDownColumnChartCategoryBM totalCasesSold = new GenericDrillDownColumnChartCategoryBM();
            GenericDrillDownColumnChartCategoryBM totalRevenue = new GenericDrillDownColumnChartCategoryBM();
            try
            {

                #region
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filters.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filters.Periods.Current.End));
                parameterList.Add(new SqlParameter("@historicalStart", filters.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filters.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@priorStart", filters.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filters.Periods.Prior.End));
                var dataSetResultTotal = base.ReadToDataSetViaProcedure("BI_EP_CustomerServiceExpenseChart", parameterList.ToArray());
                var resultTotal = dataSetResultTotal.Tables[0]
                        .AsEnumerable()
                        .Select(x => new CasesSoldRevenueChartBM
                        {
                            Commodity = !string.IsNullOrEmpty(x.Field<string>("Commodity")) ? x.Field<string>("Commodity").Trim() : "",
                            Category = !string.IsNullOrEmpty(x.Field<string>("Category")) ? x.Field<string>("Category").Trim() : "",
                            Cost = x.Field<decimal?>("Expense").Value,   
                            AssignedSalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("AssignedSalesPersonCode")) ? x.Field<string>("AssignedSalesPersonCode").Trim() : "",
                            SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? x.Field<string>("SalesPersonCode").Trim() : "",
                            SalesPersonDescription = !string.IsNullOrEmpty(x.Field<string>("SalesPersonDescription")) ? x.Field<string>("SalesPersonDescription").Trim() : "",
                            Period = !string.IsNullOrEmpty(x.Field<string>("Period")) ? x.Field<string>("Period").Trim() : "",
                            AddUser = !string.IsNullOrEmpty(x.Field<string>("AddUser")) ? x.Field<string>("AddUser").Trim() : "",
                        })
                .ToList();

                resultTotal = resultTotal.Where(f => f.SalesPersonCode != "DUMP" && f.SalesPersonCode != "DONA").ToList();
                CasesSoldRevenueDataProvider casesSoldRevenuDataProvide = new CasesSoldRevenueDataProvider();

                //  var userAccessibleCategories = _adminUserManagementDateProvider.GetUserDetails(userId);


             
                totalRevenue.All = MappingToCusomerServiceGenericListExpense(resultTotal, filters, null);

                response.TotalRevenue = totalRevenue;
               // response.TotalCasesSold = totalCasesSold;
                #endregion

                //   var responseWithDiffPercentageInRevenue = CaluclateDifference(response);
                return response;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
        }

        private List<GenericDrillDownColumnChartBM> MappingToCusomerServiceGenericListExpense(List<CasesSoldRevenueChartBM> list, GlobalFilter filters, UserDetails userAccessibleCategories)
        {
            List<GenericDrillDownColumnChartBM> result = new List<GenericDrillDownColumnChartBM>();
            list = list.OrderByDescending(s => s.Sales).ToList();


            var data = new GenericDrillDownColumnChartBM
            {
                GroupName = "",
                Label1 = filters.Periods.Current.Label,
                rValue1 = (list.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "current").Sum(t => t.Cost)),
                Period1 = filters.Periods.Current.End,
                Color1 = ChartColorBM.GrocerryCurrent,
                SalesPerson1 = "",

                Label2 = filters.Periods.Current.Label,
                rValue2 = (list.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "current").Sum(t => t.Cost)),
                Period2 = filters.Periods.Current.End,
                Color2 = ChartColorBM.ProduceCurrent,
                SalesPerson2 = "",

                Label3 = filters.Periods.Historical.Label,
                rValue3 = (list.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "historical").Sum(t => t.Cost)),
                Period3 = filters.Periods.Historical.End,
                Color3 = ChartColorBM.GrocerryPrevious,
                SalesPerson3 = "",

                Label4 = filters.Periods.Historical.Label,
                rValue4 = (list.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "historical").Sum(t => t.Cost)),
                Period4 = filters.Periods.Historical.End,
                Color4 = ChartColorBM.ProducePrevious,
                SalesPerson4 = "",

                Label5 = filters.Periods.Prior.Label,
                rValue5 = (list.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "prior").Sum(t => t.Cost)),
                Period5 = filters.Periods.Prior.End,
                Color5 = ChartColorBM.GrocerryCurrent,
                SalesPerson5 = "",

                Label6 = filters.Periods.Prior.Label,
                rValue6 = (list.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "prior").Sum(t => t.Cost)),
                Period6 = filters.Periods.Prior.End,
                Color6 = ChartColorBM.ProduceCurrent,
                SalesPerson6 = "",



                SubData = list.OrderByDescending(a => a.Sales)
                    //.Take(10)
                    .GroupBy(x => x.AddUser).Take(10)
                    .Select((sec, secIdx) => new GenericDrillDownColumnChartBM
                    {
                        GroupName = sec.Key.ToSalesCategoryDisplayName(),
                        Label1 = filters.Periods.Current.Label,
                        rValue1 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "current").Sum(t => t.Cost)),
                        Period1 = filters.Periods.Current.End,
                        Color1 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                        SalesPerson1 = "",

                        Label2 = filters.Periods.Current.Label,
                        rValue2 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "current").Sum(t => t.Cost)),
                        Period2 = filters.Periods.Current.End,
                        Color2 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                        SalesPerson2 = "",

                        Label3 = filters.Periods.Historical.Label,
                        rValue3 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "historical").Sum(t => t.Cost)),
                        Period3 = filters.Periods.Historical.End,
                        Color3 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].SecondaryGrocery,//ChartColorBM.GrocerryPrevious,
                        SalesPerson3 = "",

                        Label4 = filters.Periods.Historical.Label,
                        rValue4 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "historical").Sum(t => t.Cost)),
                        Period4 = filters.Periods.Historical.End,
                        Color4 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].SecondaryProduce,//ChartColorBM.ProducePrevious,
                        SalesPerson4 = "",

                        Label5 = filters.Periods.Prior.Label,
                        rValue5 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "prior").Sum(t => t.Cost)),
                        Period5 = filters.Periods.Prior.End,
                        Color5 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                        SalesPerson5 = "",

                        Label6 = filters.Periods.Prior.Label,
                        rValue6 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "prior").Sum(t => t.Cost)),
                        Period6 = filters.Periods.Prior.End,
                        Color6 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                        SalesPerson6 = "",

                    }).ToList().OrderByDescending(s => s.rValue1 + s.rValue2).ToList()
            };
            result.Add(data);
            return result;
        }

        private int GetColorNumber(int idx)
        {
            string color = string.Empty;
            if (idx >= 10)
            {
                idx = idx % 10;
            }
            return idx;
        }
        public List<CasesSoldDetailsMapper> GetExpenseInvoiceDetailsByCustomer(string salesPerspon, int filterId, int period
                                                                , string customerNumber, string commodity, bool isCM01)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            //Fetching the current and historical dates.
            var startDate = targetFilter.Periods.Current.Start;
            var endDate = targetFilter.Periods.Current.End;
            var prevStartDate = targetFilter.Periods.Historical.Start;
            var prevEndDate = targetFilter.Periods.Historical.End;

            DateTime CurrentEndDate = targetFilter.Periods.Current.End;
            DateTime HistoricalEndDate = targetFilter.Periods.Historical.End;
            DateTime PriorEndDate = targetFilter.Periods.Prior.End;

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
                parameterList.Add(new SqlParameter("@salesPerson", salesPerspon));
                parameterList.Add(new SqlParameter("@currentStart", startDate));
                parameterList.Add(new SqlParameter("@currentEnd", endDate));
                parameterList.Add(new SqlParameter("@customerCode", customerNumber));
                parameterList.Add(new SqlParameter("commodity", commodity));
                var dataTableResult = new DataSet();
                if (isCM01)
                {
                     dataTableResult = base.ReadToDataSetViaProcedure("BI_EP_GetInvoiceDetailsByCustomerForCustomerService", parameterList.ToArray());
                }
                else
                {
                     dataTableResult = base.ReadToDataSetViaProcedure("BI_EP_GetInvoiceDetailsByCustomer", parameterList.ToArray());
                }
                if (dataTableResult != null && dataTableResult.Tables.Count > 0)
                {
                    var result = dataTableResult.Tables[0]
                                        .AsEnumerable()
                                        .Select(x => new CasesSoldDetailsMapper
                                        {
                                            Comodity      = x.Field<string>("Commodity"),
                                            InvoiceDate   = x.Field<DateTime>("InvoiceDate"),
                                            InvoiceNumber = x.Field<string>("InvoiceNumber"),
                                            Item          = x.Field<string>("ItemCode"),
                                            ItemDesc      = x.Field<string>("ItemDescription"),
                                            Expense       = x.Field<decimal?>("Expense").Value,
                                            SalesMan      = x.Field<string>("SalesManDescription"),
                                            Sono          = x.Field<string>("SalesOrderNumber"),
                                            Transeq       = x.Field<string>("SalesManCode")
                                        }).ToList();


                    return result.OrderByDescending(x => x.Expense).ToList();


                }
                else
                    return new List<CasesSoldDetailsMapper>();
            }
        }

    }
}
