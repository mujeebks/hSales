using Nogales.BusinessModel;
using Nogales.DataProvider.Infrastructure;
using Nogales.DataProvider.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Nogales.DataProvider
{
    public class CasesSoldRevenueDataProvider : DataAccessADO
    {

        /// <summary>
        /// Get data for the sold case, revenue report
        /// </summary>
        /// <param name="date"> Date of shipment </param>
        /// <returns> Short report </returns>
        public GenericDashboardChartsBO GetCasesSoldRevenueReport(GlobalFilter filters)
        {
            GenericDashboardChartsBO response = new GenericDashboardChartsBO();
            GenericColumnChartCategoryBM total = new GenericColumnChartCategoryBM();

            GenericColumnChartCategoryBM totalCasesSold = new GenericColumnChartCategoryBM();
            GenericColumnChartCategoryBM totalRevenue = new GenericColumnChartCategoryBM();
            try
            {


                ///<summary>
                ///Stored procedure which return the total dispalying in dash board
                /// </summary>
                #region
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filters.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filters.Periods.Current.End));
                parameterList.Add(new SqlParameter("@historicalStart", filters.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filters.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@priorStart", filters.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filters.Periods.Prior.End));
                var dataSetResultTotal = base.ReadToDataSetViaProcedure("BI_GetCasesSoldRevenueData", parameterList.ToArray());
                var resultTotal = dataSetResultTotal.Tables[0]
                            .AsEnumerable()
                            .Select(x => new MapCasesSoldRevenueGlobal
                            {
                                Comodity = !string.IsNullOrEmpty(x.Field<string>("Comodity")) ? x.Field<string>("Comodity").Trim() : "",
                                //SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? Constants.OutSalesPersons.ToList().Contains(x.Field<string>("SalesPersonCode").Trim()) ? "OSS" : x.Field<string>("SalesPersonCode").Trim() : "",
                                SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? x.Field<string>("SalesPersonCode").Trim() : "",
                                Category = !string.IsNullOrEmpty(x.Field<string>("category")) ? x.Field<string>("category").Trim() : "",
                                RevenueCurrent = x.Field<decimal?>("revenueCurrent").ToStripDecimal(),
                                RevenueHistorical = x.Field<decimal?>("revenueHistorical").ToStripDecimal(),
                                RevenuePrior = x.Field<decimal?>("revenueprior").ToStripDecimal(),
                                CasesCurrent = x.Field<int?>("casesCurrent") ?? 0,
                                CasesHistorical = x.Field<int?>("CasesHistorical") ?? 0,
                                CasesPrior = x.Field<int?>("casesPrior") ?? 0,
                                CurrentEmployee = !string.IsNullOrEmpty(x.Field<string>("currentEmployee")) ? x.Field<string>("currentEmployee").Trim() : "",
                                HistoricalEmployee = !string.IsNullOrEmpty(x.Field<string>("historicalEmployee")) ? x.Field<string>("historicalEmployee").Trim() : "",
                                PriorEmployee = !string.IsNullOrEmpty(x.Field<string>("priorEmployee")) ? x.Field<string>("PriorEmployee").Trim() : "",
                                ActiveEmployee = !string.IsNullOrEmpty(x.Field<string>("ActiveEmployee")) ? x.Field<string>("ActiveEmployee").Trim() : "",

                            })
            .ToList();
                resultTotal = resultTotal.Where(f => f.SalesPersonCode != "DUMP" && f.SalesPersonCode != "DONA").ToList();
                totalCasesSold.All = MappingToGenericListCasesold(resultTotal, filters);
                totalCasesSold.Grocery = MappingToGenericListCasesoldFromCompType(resultTotal, "grocery", filters);
                totalCasesSold.Produce = MappingToGenericListCasesoldFromCompType(resultTotal, "produce", filters);

                totalRevenue.All = MappingToGenericListRevenue(resultTotal, filters);
                totalRevenue.Grocery = MappingToGenericListRevenueFromCompType(resultTotal, "grocery", filters);
                totalRevenue.Produce = MappingToGenericListRevenueFromCompType(resultTotal, "produce", filters);


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


        /// <summary>
        /// Get data for categories
        /// </summary>
        /// <param name="date"> Date of shipment </param>
        /// <returns> Short report </returns>
        public GenericDashboardChartsBO GetCategories(GlobalFilter filter)
        {
            DateTime date = DateTime.Now;
            GenericDashboardChartsBO response = new GenericDashboardChartsBO();
            GenericColumnChartCategoryBM total = new GenericColumnChartCategoryBM();
            try
            {
                //string query = string.Empty;
                //query = SQLQueries.GetCasesSoldRevenueReportQuery(date);
                int curMonth = date.Month;
                int prevMonth = date.AddMonths(-1).Month;

                var currGrpName = date.ToMonthName();
                var prevGrpName = date.AddMonths(-1).ToMonthName();


                ///<summary>
                ///Stored procedure which return the categories in dashboard
                /// </summary>

                #region
                List<SqlParameter> parameterList2 = new List<SqlParameter>();
                parameterList2.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList2.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList2.Add(new SqlParameter("@historicalStart", filter.Periods.Historical.Start));
                parameterList2.Add(new SqlParameter("@historicalEnd", filter.Periods.Historical.End));
                parameterList2.Add(new SqlParameter("@priorStart", filter.Periods.Prior.Start));
                parameterList2.Add(new SqlParameter("@priorEnd", filter.Periods.Prior.End));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetCategoryQuantityRevenue", parameterList2.ToArray());
                var data = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new MapCategoryQuantityRevenueGlobal
                            {
                                Comodity = !string.IsNullOrEmpty(x.Field<string>("Comodity")) ? x.Field<string>("Comodity").Trim() : "",
                                //SalesMan = !string.IsNullOrEmpty(x.Field<string>("salesman")) ? Constants.OutSalesPersons.ToList().Contains(x.Field<string>("salesman").Trim()) ? "OSS" : x.Field<string>("salesman").Trim() : "",
                                SalesMan = !string.IsNullOrEmpty(x.Field<string>("salesman")) ? x.Field<string>("salesman").Trim() : "",
                                Category = !string.IsNullOrEmpty(x.Field<string>("Category")) ? x.Field<string>("Category").Trim() : "",
                                Company = !string.IsNullOrEmpty(x.Field<string>("Company")) ? x.Field<string>("Company").Trim() : "",
                                CustNo = !string.IsNullOrEmpty(x.Field<string>("custno")) ? x.Field<string>("custno").Trim() : "",
                                InvDate = x.Field<DateTime>("InvDate"),
                                Invoice = !string.IsNullOrEmpty(x.Field<string>("Invoice")) ? x.Field<string>("Invoice").Trim() : "",
                                Quantity = (int)x.Field<int>("Quantity"),
                                Revenue = x.Field<decimal>("Revenue"),
                                Period = x.Field<string>("period"),
                                ActiveEmployee = x.Field<string>("SalesPersonDescription"),
                                ActualSalesPersonCodes = x.Field<string>("ActualSalesPerson")
                            })
                            .ToList();
                data = data.Where(f => f.SalesMan != "DUMP" && f.SalesMan != "DONA").ToList();
                var result = Processing(data, date);

                #endregion


                response.BuyerCasesold = MappingToGenericListCaseSoldFromCategory(result, "BUYER", filter);
                response.BuyerRevenue = MappingToGenericListRevenueFromCategory(result, "BUYER", filter);

                response.NationalCasesSold = MappingToGenericListCaseSoldFromCategory(result, "NATIONAL", filter);
                response.NationalRevenue = MappingToGenericListRevenueFromCategory(result, "NATIONAL", filter);

                response.FoodServiceCasesSold = MappingToGenericListCaseSoldFromCategory(result, "FOOD SERVI", filter);
                response.FoodServiceRevenue = MappingToGenericListRevenueFromCategory(result, "FOOD SERVI", filter);

                response.CarniceriaCasesSold = MappingToGenericListCaseSoldFromCategory(result, "CARNICERIA", filter);
                response.CarniceriaRevenue = MappingToGenericListRevenueFromCategory(result, "CARNICERIA", filter);

                response.WholesalerCasesSold = MappingToGenericListCaseSoldFromCategory(result, "WHOLESALER", filter);
                response.WholesalerRevenue = MappingToGenericListRevenueFromCategory(result, "WHOLESALER", filter);

                response.WillCallCasesSold = MappingToGenericListCaseSoldFromCategory(result, "WILL CALL", filter);
                response.WillCallRevenue = MappingToGenericListRevenueFromCategory(result, "WILL CALL", filter);

                response.LossProdCasesSold = MappingToGenericListCaseSoldFromCategory(result, "LOSS PROD", filter);
                response.LossProdRevenue = MappingToGenericListRevenueFromCategory(result, "LOSS PROD", filter);
                response.RetailCasesSold = new GenericColumnChartCategoryBM
                {
                    //Grocery = result.GetCasesFromSpecificCondition(x => x.Comodity == "Grocery" && x.Category.StartsWith("RETAIL"),
                    //                                                       filter, "Grocery"),
                    //Produce = result.GetCasesFromSpecificCondition(x => x.Comodity == "Produce" && x.Category.StartsWith("RETAIL"), filter, "Produce"),
                };
                response.RetailCasesSold.All = MappingToGenericListCasesoldFromSpeciality(result.Where(x => x.Category.StartsWith("RETAIL")).ToList(), filter);

                response.RetailRevenue = new GenericColumnChartCategoryBM
                {
                    //Grocery = result.GetRevenueFromSpecificCondition(x => x.Comodity == "Grocery" && x.Category.StartsWith("RETAIL"),filter, "Grocery"),
                    //Produce = result.GetRevenueFromSpecificCondition(x => x.Comodity == "Produce" && x.Category.StartsWith("RETAIL"),filter, "Produce"),
                };
                response.RetailRevenue.All = MappingToGenericListRevenueFromSpeciality(result.Where(x => x.Category.StartsWith("RETAIL")).ToList(), filter);

                response.AllOthersCasesSold = new GenericColumnChartCategoryBM
                {
                    //Grocery = result.GetCasesFromSpecificCondition(x => x.Comodity == "Grocery" && !x.Category.StartsWith("RETAIL") &&
                    //                                                        x.Category != "BUYER" &&
                    //                                                        x.Category != "FOOD SERVI" &&
                    //                                                        x.Category != "CARNICERIA" &&
                    //                                                        x.Category != "WHOLESALER" &&
                    //                                                        x.Category != "WILL CALL" &&
                    //                                                         x.Category != "NATIONAL",
                    //                                                     filter, "Grocery", true),
                    //Produce = result.GetCasesFromSpecificCondition(x => x.Comodity == "Produce" && !x.Category.StartsWith("RETAIL") &&
                    //                                                        x.Category != "BUYER" &&
                    //                                                        x.Category != "FOOD SERVI" &&
                    //                                                        x.Category != "CARNICERIA" &&
                    //                                                        x.Category != "WHOLESALER" &&
                    //                                                        x.Category != "WILL CALL" &&
                    //                                                         x.Category != "NATIONAL", filter, "Produce", true),

                };

                response.AllOthersCasesSold.All = MappingToGenericListCasesoldFromSpeciality(result.Where(x => !x.Category.StartsWith("RETAIL") &&
                                                                            x.Category != "BUYER" &&
                                                                            x.Category != "FOOD SERVI" &&
                                                                            x.Category != "CARNICERIA" &&
                                                                            x.Category != "WHOLESALER" &&
                                                                            x.Category != "WILL CALL" &&
                                                                             x.Category != "NATIONAL").ToList(), filter);

                response.AllOthersRevenue = new GenericColumnChartCategoryBM
                {
                    //Grocery = result.GetRevenueFromSpecificCondition(x => x.Comodity == "Grocery" && !x.Category.StartsWith("RETAIL") &&
                    //                                                        x.Category != "BUYER" &&
                    //                                                        x.Category != "FOOD SERVI" &&
                    //                                                        x.Category != "CARNICERIA" &&
                    //                                                        x.Category != "WHOLESALER" &&
                    //                                                        x.Category != "WILL CALL" &&
                    //                                                         x.Category != "NATIONAL",
                    //                                                     filter, "Grocery", true),
                    //Produce = result.GetRevenueFromSpecificCondition(x => x.Comodity == "Produce" && !x.Category.StartsWith("RETAIL") &&
                    //                                                        x.Category != "BUYER" &&
                    //                                                        x.Category != "FOOD SERVI" &&
                    //                                                        x.Category != "CARNICERIA" &&
                    //                                                        x.Category != "WHOLESALER" &&
                    //                                                        x.Category != "WILL CALL" &&
                    //                                                        x.Category != "NATIONAL", filter, "Produce", true),

                };

                response.AllOthersRevenue.All = MappingToGenericListRevenueFromSpeciality(result.Where(x => !x.Category.StartsWith("RETAIL") &&
                                                                            x.Category != "BUYER" &&
                                                                            x.Category != "FOOD SERVI" &&
                                                                            x.Category != "CARNICERIA" &&
                                                                            x.Category != "WHOLESALER" &&
                                                                            x.Category != "WILL CALL" &&
                                                                            x.Category != "NATIONAL").ToList(),
                                                                            filter);

                //response.OOT = MappingToGenericListByCategory(result, "OOT", date, curMonth, currGrpName, prevGrpName);
                response.OOTCasesSold = MappingToSpecificCategoryCasesSoldFromSalesMan(result, "ot", filter);
                response.OOTRevenue = MappingToSpecificCategoryRevenueFromSalesMan(result, "ot", filter);

                //var customers = GetCustomerBySalesMan(result, filter);
                //response.SalesManReport = MappingToGenericListBySalesPerson(result, customers, filter);



                return response;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }

        }

        public List<MapCasesSoldRevenueGlobal> Processing(List<MapCategoryQuantityRevenueGlobal> data, DateTime date)
        {

            var result = data.GroupBy(c => new { c.Period, c.Category, c.SalesMan, c.Comodity })
   .Select(g => new MapCasesSoldRevenueGlobal
   {

       Category = g.Key.Category,
       Comodity = g.Key.Comodity,
       SalesPersonCode = g.Key.SalesMan,
       Period = g.Key.Period,
       CasesCurrent = g.Where(s => s.Period == "current").Sum(d => d.Quantity),
       CasesHistorical = g.Where(s => s.Period == "historical").Sum(d => d.Quantity),
       CasesPrior = g.Where(s => s.Period == "prior").Sum(d => d.Quantity),

       RevenueCurrent = g.Where(s => s.Period == "current").Sum(d => d.Revenue),
       RevenueHistorical = g.Where(s => s.Period == "historical").Sum(d => d.Revenue),
       RevenuePrior = g.Where(s => s.Period == "prior").Sum(d => d.Revenue),

       ActiveEmployee = g.First() != null ? g.First().ActiveEmployee : g.Key.SalesMan,

       CurrentEmployee = string.Join(",", g.Where(w => w.Period == "current").Select(s => s.ActualSalesPersonCodes).Distinct()),
       HistoricalEmployee = string.Join(",", g.Where(w => w.Period == "historical").Select(s => s.ActualSalesPersonCodes).Distinct()),
       PriorEmployee = string.Join(",", g.Where(w => w.Period == "prior").Select(s => s.ActualSalesPersonCodes).Distinct()),


   }).ToList();

            return result;
        }

        double CalculateChange(double previous, double current)
        {
            return UtilityExtensions.CalculateChange(previous, current);
        }

        double DoubleToPercentageString(double d)
        {
            return (Math.Round(d, 2) * 100);
        }

        public GenericDashboardChartsBO CaluclateDifference(GenericDashboardChartsBO data)
        {
            var currentMonthAll = data.TotalRevenue.All.ToList()[0].rValue1 + data.TotalRevenue.All.ToList()[0].rValue2;
            var previousMonthAll = data.TotalRevenue.All.ToList()[0].rValue5 + data.TotalRevenue.All.ToList()[0].rValue6;
            var previousYearAll = data.TotalRevenue.All.ToList()[0].rValue3 + data.TotalRevenue.All.ToList()[0].rValue4;

            var caseCurrentMonthAll = data.TotalCasesSold.All.ToList()[0].cValue1 + data.TotalCasesSold.All.ToList()[0].cValue2;
            var CasePreviousMonthAll = data.TotalCasesSold.All.ToList()[0].cValue5 + data.TotalCasesSold.All.ToList()[0].cValue6;
            var CasePreviousYearAll = data.TotalCasesSold.All.ToList()[0].cValue3 + data.TotalCasesSold.All.ToList()[0].cValue4;

            data.revenueTotalMonthlyDifference = DoubleToPercentageString(CalculateChange(previousMonthAll, currentMonthAll)).ToRoundTwoDigits();
            data.revenueTotalYearlyDifference = DoubleToPercentageString(CalculateChange(previousYearAll, currentMonthAll)).ToRoundTwoDigits();

            data.TotalMonthlyDifference = DoubleToPercentageString(CalculateChange(CasePreviousMonthAll, caseCurrentMonthAll)).ToRoundTwoDigits();
            data.TotalYearlyDifference = DoubleToPercentageString(CalculateChange(CasePreviousYearAll, caseCurrentMonthAll)).ToRoundTwoDigits();


            var currentMonthGrocery = data.TotalRevenue.Grocery.ToList()[0].rValue1;
            var previousMonthGrocery = data.TotalRevenue.Grocery.ToList()[0].rValue3;
            var previousYearGrocery = data.TotalRevenue.Grocery.ToList()[0].rValue2;

            var CaseCurrentMonthGrocery = data.TotalCasesSold.Grocery.ToList()[0].cValue1;
            var CasePreviousMonthGrocery = data.TotalCasesSold.Grocery.ToList()[0].cValue3;
            var CasePreviousYearGrocery = data.TotalCasesSold.Grocery.ToList()[0].cValue2;

            data.revenueGroceryMonthlyDifference = DoubleToPercentageString(CalculateChange(previousMonthGrocery, currentMonthGrocery)).ToRoundTwoDigits();
            data.revenueGroceryYearlyDifference = DoubleToPercentageString(CalculateChange(previousMonthGrocery, currentMonthGrocery)).ToRoundTwoDigits();

            data.GroceryMonthlyDifference = DoubleToPercentageString(CalculateChange(CasePreviousMonthGrocery, CaseCurrentMonthGrocery)).ToRoundTwoDigits();
            data.GroceryYearlyDifference = DoubleToPercentageString(CalculateChange(CasePreviousMonthGrocery, CaseCurrentMonthGrocery)).ToRoundTwoDigits();


            var currentMonthProduce = data.TotalRevenue.Produce.ToList()[0].rValue1;
            var previousMonthProduce = data.TotalRevenue.Produce.ToList()[0].rValue3;
            var previousYearProduce = data.TotalRevenue.Produce.ToList()[0].rValue2;

            var CaseCurrentMonthProduce = data.TotalCasesSold.Produce.ToList()[0].cValue1;
            var CasePreviousMonthProduce = data.TotalCasesSold.Produce.ToList()[0].cValue3;
            var CasePreviousYearProduce = data.TotalCasesSold.Produce.ToList()[0].cValue2;

            data.revenueProduceMonthlyDifference = DoubleToPercentageString(CalculateChange(previousMonthProduce, currentMonthProduce)).ToRoundTwoDigits(); ;
            data.revenueProduceYearlyDifference = DoubleToPercentageString(CalculateChange(previousMonthProduce, currentMonthProduce)).ToRoundTwoDigits(); ;

            data.ProduceMonthlyDifference = DoubleToPercentageString(CalculateChange(CasePreviousMonthProduce, CaseCurrentMonthProduce)).ToRoundTwoDigits(); ;
            data.ProduceYearlyDifference = DoubleToPercentageString(CalculateChange(CasePreviousMonthProduce, CaseCurrentMonthProduce)).ToRoundTwoDigits(); ;
            return data;
        }

        public GenericColumnChartSalesDashBoardBM GetSalesManMap(GlobalFilter filter)
        {
            GenericColumnChartSalesDashBoardBM response = new GenericColumnChartSalesDashBoardBM();
            try
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@historicalStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filter.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@priorStart", filter.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filter.Periods.Prior.End));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetSalesPersonInCasesSoldRevenueMap", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new MapCasesSoldRevenueGlobal
                            {
                                // Comodity = !string.IsNullOrEmpty(x.Field<string>("Comodity")) ? x.Field<string>("Comodity").Trim() : "",
                                //SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? Constants.OutSalesPersons.ToList().Contains(x.Field<string>("SalesPersonCode").Trim()) ? "OSS" : x.Field<string>("SalesPersonCode").Trim() : "",
                                // SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? x.Field<string>("SalesPersonCode").Trim() : "",
                                // Category = !string.IsNullOrEmpty(x.Field<string>("category")) ? x.Field<string>("category").Trim() : "",
                                RevenueCurrent = x.Field<decimal?>("revenueCurrent").ToStripDecimal(),
                                RevenueHistorical = x.Field<decimal?>("RevenueHistorical").ToStripDecimal(),
                                RevenuePrior = x.Field<decimal?>("RevenuePrior").ToStripDecimal(),
                                CasesCurrent = x.Field<int?>("CasesCurrent") ?? 0,
                                CasesHistorical = x.Field<int?>("CasesHistorical") ?? 0,
                                CasesPrior = x.Field<int?>("CasesPrior") ?? 0,
                                State = x.Field<string>("State")
                                // CurrentEmployee = !string.IsNullOrEmpty(x.Field<string>("currentEmployee")) ? x.Field<string>("currentEmployee").Trim() : "",
                                //HistoricalEmployee = !string.IsNullOrEmpty(x.Field<string>("historicalEmployee")) ? x.Field<string>("historicalEmployee").Trim() : "",
                                //PriorEmployee = !string.IsNullOrEmpty(x.Field<string>("priorEmployee")) ? x.Field<string>("priorEmployee").Trim() : "",
                                //ActiveEmployee = !string.IsNullOrEmpty(x.Field<string>("ActiveEmployee")) ? x.Field<string>("ActiveEmployee").Trim() : "",
                            })
                            .ToList();
                result = result.Where(x => x.SalesPersonCode != "DUMP" && x.SalesPersonCode != "DONA").ToList();
                // response.Local = MappingToSalesPersonDashBoard(result, filter);
                //response.All = MappingToGenericListRevenue(result, filter);/// MappingToSalesPersonDashBoard(result, filter);
                //response.OutSales = GetOutSalesSalesManDashBoardReport(filter);
                response.MapData = result.Select(pri => new GenericMapChartMB
                {
                    value = (pri.RevenueCurrent).ToStripDecimal().ToString(),
                    id = pri.State.Trim(),
                    customData = filter.Periods.Current.Label + ": $" + (pri.RevenueCurrent.ToStripDecimal()).ToString("#,##0")
                    + " </br> " + filter.Periods.Historical.Label + " : $" + (pri.RevenueHistorical.ToStripDecimal()).ToString("#,##0") + " </br > " +
                  (filter.Periods.Prior.Label != string.Empty ? (filter.Periods.Prior.Label + " : $" + (pri.RevenuePrior.ToStripDecimal()).ToString("#,##0")) : "") + ""
                }).ToList();
                response.CaseSoldMapData = result.Select(pri => new GenericMapChartMB
                {
                    value = (pri.CasesCurrent).ToString(),
                    id = pri.State.Trim(),
                    customData = filter.Periods.Current.Label + ": " + (pri.CasesCurrent).ToString()
                    + " </br> " + filter.Periods.Historical.Label + " : " + (pri.CasesHistorical).ToString() + " </br > " +
                  (filter.Periods.Prior.Label != string.Empty ? (filter.Periods.Prior.Label + " : " + (pri.CasesPrior).ToString()) : "") + ""
                }).ToList();
                response.RevenueMapData = result.Select(pri => new GenericMapChartMB
                {
                    value = (pri.CasesCurrent).ToString(),
                    id = pri.State.Trim(),
                    customData = filter.Periods.Current.Label + ": $" + (pri.RevenueCurrent.ToStripDecimal()).ToString("#,##0")
                    + " </br> " + filter.Periods.Historical.Label + " : $" + (pri.RevenueHistorical.ToStripDecimal()).ToString("#,##0") + " </br > " +
                  (filter.Periods.Prior.Label != string.Empty ? (filter.Periods.Prior.Label + " : $" + (pri.RevenuePrior.ToStripDecimal()).ToString("#,##0")) : "") + ""
                }).ToList();
                var sumvalue = result.Sum(x => x.RevenueCurrent);

                //response.Local = result.Select(pri => new GenericColumnChartBM
                //{
                //    Label1 = "State: [[title]] </br > " + filter.Periods.Current.Label + ": " + (double)(pri.RevenueCurrent.ToRoundTwoDecimalDigits())
                //    + " < br > " + filter.Periods.Historical.Label + " :" + (double)(pri.RevenueHistorical.ToRoundTwoDecimalDigits()) + " </br > " + filter.Periods.Prior.Label + " :" + (double)(pri.RevenuePrior.ToRoundTwoDecimalDigits()) + "",
                //    rValue1 = (double)(pri.RevenueCurrent.ToRoundTwoDecimalDigits()),
                //    Color1 = pri.State,
                //    Period1 = filter.Periods.Current.End,

                //    Label2 = filter.Periods.Current.Label,
                //    rValue2 = (double)(pri.RevenueCurrent).ToRoundTwoDecimalDigits(),
                //    Period2 = filter.Periods.Current.End,

                //    Label3 = filter.Periods.Historical.Label,
                //    rValue3 = (double)(pri.RevenueHistorical).ToRoundTwoDecimalDigits(),
                //    Period3 = filter.Periods.Historical.End,

                //    Label4 = filter.Periods.Historical.Label,
                //    rValue4 = (double)(pri.RevenueHistorical).ToRoundTwoDecimalDigits(),
                //    Period4 = filter.Periods.Historical.End,

                //    Label5 = filter.Periods.Prior.Label,
                //    rValue5 = (double)(pri.RevenuePrior).ToRoundTwoDecimalDigits(),
                //    Period5 = filter.Periods.Prior.End,

                //    Label6 = filter.Periods.Prior.Label,
                //    rValue6 = (double)(pri.RevenuePrior).ToRoundTwoDecimalDigits(),
                //    Period6 = filter.Periods.Prior.End

                //}).ToList();

                //response.Sales = MappingSalesToSalesPersonDashBoard(result, filter);
                // var customers = GetCustomersByAllSalesMan(result, filter);
                //response.Customer = MappingCustomersToSalesPersonDashBoard(result, customers, filter);
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
            return response;
        }
        public GenericColumnChartSalesDashBoardBM GetCasesSoldMap(GlobalFilter filter)
        {
            GenericColumnChartSalesDashBoardBM response = new GenericColumnChartSalesDashBoardBM();
            try
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@historicalStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filter.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@priorStart", filter.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filter.Periods.Prior.End));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetCasesSoldMap", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new MapCasesSoldRevenueGlobal
                            {
                                // Comodity = !string.IsNullOrEmpty(x.Field<string>("Comodity")) ? x.Field<string>("Comodity").Trim() : "",
                                //SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? Constants.OutSalesPersons.ToList().Contains(x.Field<string>("SalesPersonCode").Trim()) ? "OSS" : x.Field<string>("SalesPersonCode").Trim() : "",
                                // SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? x.Field<string>("SalesPersonCode").Trim() : "",
                                // Category = !string.IsNullOrEmpty(x.Field<string>("category")) ? x.Field<string>("category").Trim() : "",
                                RevenueCurrent = x.Field<decimal?>("revenueCurrent").ToStripDecimal(),
                                RevenueHistorical = x.Field<decimal?>("RevenueHistorical").ToStripDecimal(),
                                RevenuePrior = x.Field<decimal?>("RevenuePrior").ToStripDecimal(),
                                CasesCurrent = x.Field<int?>("CasesCurrent") ?? 0,
                                CasesHistorical = x.Field<int?>("CasesHistorical") ?? 0,
                                CasesPrior = x.Field<int?>("CasesPrior") ?? 0,
                                State = x.Field<string>("State")
                                // CurrentEmployee = !string.IsNullOrEmpty(x.Field<string>("currentEmployee")) ? x.Field<string>("currentEmployee").Trim() : "",
                                //HistoricalEmployee = !string.IsNullOrEmpty(x.Field<string>("historicalEmployee")) ? x.Field<string>("historicalEmployee").Trim() : "",
                                //PriorEmployee = !string.IsNullOrEmpty(x.Field<string>("priorEmployee")) ? x.Field<string>("priorEmployee").Trim() : "",
                                //ActiveEmployee = !string.IsNullOrEmpty(x.Field<string>("ActiveEmployee")) ? x.Field<string>("ActiveEmployee").Trim() : "",
                            })
                            .ToList();
                result = result.Where(x => x.SalesPersonCode != "DUMP" && x.SalesPersonCode != "DONA").ToList();
                // response.Local = MappingToSalesPersonDashBoard(result, filter);
                //response.All = MappingToGenericListRevenue(result, filter);/// MappingToSalesPersonDashBoard(result, filter);
                //response.OutSales = GetOutSalesSalesManDashBoardReport(filter);
                response.MapData = result.Select(pri => new GenericMapChartMB
                {
                    value = (pri.RevenueCurrent).ToStripDecimal().ToString(),
                    id = pri.State.Trim(),
                    customData = filter.Periods.Current.Label + ": $" + (pri.RevenueCurrent.ToStripDecimal()).ToString("#,##0")
                    + " </br> " + filter.Periods.Historical.Label + " : $" + (pri.RevenueHistorical.ToStripDecimal()).ToString("#,##0") + " </br > " +
                  (filter.Periods.Prior.Label != string.Empty ? (filter.Periods.Prior.Label + " : $" + (pri.RevenuePrior.ToStripDecimal()).ToString("#,##0")) : "") + ""
                }).ToList();
                response.CaseSoldMapData = result.Select(pri => new GenericMapChartMB
                {
                    value = (pri.CasesCurrent).ToString(),
                    id = pri.State.Trim(),
                    customData = filter.Periods.Current.Label + ": " + (pri.CasesCurrent).ToString()
                    + " </br> " + filter.Periods.Historical.Label + " : " + (pri.CasesHistorical).ToString() + " </br > " +
                  (filter.Periods.Prior.Label != string.Empty ? (filter.Periods.Prior.Label + " : " + (pri.CasesPrior).ToString()) : "") + ""
                }).ToList();
                response.RevenueMapData = result.Select(pri => new GenericMapChartMB
                {
                    value = (pri.CasesCurrent).ToString(),
                    id = pri.State.Trim(),
                    customData = filter.Periods.Current.Label + ": $" + (pri.RevenueCurrent.ToStripDecimal()).ToString("#,##0")
                    + " </br> " + filter.Periods.Historical.Label + " : $" + (pri.RevenueHistorical.ToStripDecimal()).ToString("#,##0") + " </br > " +
                  (filter.Periods.Prior.Label != string.Empty ? (filter.Periods.Prior.Label + " : $" + (pri.RevenuePrior.ToStripDecimal()).ToString("#,##0")) : "") + ""
                }).ToList();
                var sumvalue = result.Sum(x => x.RevenueCurrent);

                //response.Local = result.Select(pri => new GenericColumnChartBM
                //{
                //    Label1 = "State: [[title]] </br > " + filter.Periods.Current.Label + ": " + (double)(pri.RevenueCurrent.ToRoundTwoDecimalDigits())
                //    + " < br > " + filter.Periods.Historical.Label + " :" + (double)(pri.RevenueHistorical.ToRoundTwoDecimalDigits()) + " </br > " + filter.Periods.Prior.Label + " :" + (double)(pri.RevenuePrior.ToRoundTwoDecimalDigits()) + "",
                //    rValue1 = (double)(pri.RevenueCurrent.ToRoundTwoDecimalDigits()),
                //    Color1 = pri.State,
                //    Period1 = filter.Periods.Current.End,

                //    Label2 = filter.Periods.Current.Label,
                //    rValue2 = (double)(pri.RevenueCurrent).ToRoundTwoDecimalDigits(),
                //    Period2 = filter.Periods.Current.End,

                //    Label3 = filter.Periods.Historical.Label,
                //    rValue3 = (double)(pri.RevenueHistorical).ToRoundTwoDecimalDigits(),
                //    Period3 = filter.Periods.Historical.End,

                //    Label4 = filter.Periods.Historical.Label,
                //    rValue4 = (double)(pri.RevenueHistorical).ToRoundTwoDecimalDigits(),
                //    Period4 = filter.Periods.Historical.End,

                //    Label5 = filter.Periods.Prior.Label,
                //    rValue5 = (double)(pri.RevenuePrior).ToRoundTwoDecimalDigits(),
                //    Period5 = filter.Periods.Prior.End,

                //    Label6 = filter.Periods.Prior.Label,
                //    rValue6 = (double)(pri.RevenuePrior).ToRoundTwoDecimalDigits(),
                //    Period6 = filter.Periods.Prior.End

                //}).ToList();

                //response.Sales = MappingSalesToSalesPersonDashBoard(result, filter);
                // var customers = GetCustomersByAllSalesMan(result, filter);
                //response.Customer = MappingCustomersToSalesPersonDashBoard(result, customers, filter);
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
            return response;
        }
        public GenericColumnChartSalesDashBoardBM GetRevenueMap(GlobalFilter filter)
        {
            GenericColumnChartSalesDashBoardBM response = new GenericColumnChartSalesDashBoardBM();
            try
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@historicalStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filter.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@priorStart", filter.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filter.Periods.Prior.End));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetRevenueMap", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new MapCasesSoldRevenueGlobal
                            {
                                // Comodity = !string.IsNullOrEmpty(x.Field<string>("Comodity")) ? x.Field<string>("Comodity").Trim() : "",
                                //SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? Constants.OutSalesPersons.ToList().Contains(x.Field<string>("SalesPersonCode").Trim()) ? "OSS" : x.Field<string>("SalesPersonCode").Trim() : "",
                                // SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? x.Field<string>("SalesPersonCode").Trim() : "",
                                // Category = !string.IsNullOrEmpty(x.Field<string>("category")) ? x.Field<string>("category").Trim() : "",
                                RevenueCurrent = x.Field<decimal?>("revenueCurrent").ToStripDecimal(),
                                RevenueHistorical = x.Field<decimal?>("RevenueHistorical").ToStripDecimal(),
                                RevenuePrior = x.Field<decimal?>("RevenuePrior").ToStripDecimal(),
                                CasesCurrent = x.Field<int?>("CasesCurrent") ?? 0,
                                CasesHistorical = x.Field<int?>("CasesHistorical") ?? 0,
                                CasesPrior = x.Field<int?>("CasesPrior") ?? 0,
                                State = x.Field<string>("State")
                                // CurrentEmployee = !string.IsNullOrEmpty(x.Field<string>("currentEmployee")) ? x.Field<string>("currentEmployee").Trim() : "",
                                //HistoricalEmployee = !string.IsNullOrEmpty(x.Field<string>("historicalEmployee")) ? x.Field<string>("historicalEmployee").Trim() : "",
                                //PriorEmployee = !string.IsNullOrEmpty(x.Field<string>("priorEmployee")) ? x.Field<string>("priorEmployee").Trim() : "",
                                //ActiveEmployee = !string.IsNullOrEmpty(x.Field<string>("ActiveEmployee")) ? x.Field<string>("ActiveEmployee").Trim() : "",
                            })
                            .ToList();
                result = result.Where(x => x.SalesPersonCode != "DUMP" && x.SalesPersonCode != "DONA").ToList();
                // response.Local = MappingToSalesPersonDashBoard(result, filter);
                //response.All = MappingToGenericListRevenue(result, filter);/// MappingToSalesPersonDashBoard(result, filter);
                //response.OutSales = GetOutSalesSalesManDashBoardReport(filter);
                response.MapData = result.Select(pri => new GenericMapChartMB
                {
                    value = (pri.RevenueCurrent).ToStripDecimal().ToString(),
                    id = pri.State.Trim(),
                    customData = filter.Periods.Current.Label + ": $" + (pri.RevenueCurrent.ToStripDecimal()).ToString("#,##0")
                    + " </br> " + filter.Periods.Historical.Label + " : $" + (pri.RevenueHistorical.ToStripDecimal()).ToString("#,##0") + " </br > " +
                  (filter.Periods.Prior.Label != string.Empty ? (filter.Periods.Prior.Label + " : $" + (pri.RevenuePrior.ToStripDecimal()).ToString("#,##0")) : "") + ""
                }).ToList();
                response.CaseSoldMapData = result.Select(pri => new GenericMapChartMB
                {
                    value = (pri.CasesCurrent).ToString(),
                    id = pri.State.Trim(),
                    customData = filter.Periods.Current.Label + ": " + (pri.CasesCurrent).ToString()
                    + " </br> " + filter.Periods.Historical.Label + " : " + (pri.CasesHistorical).ToString() + " </br > " +
                  (filter.Periods.Prior.Label != string.Empty ? (filter.Periods.Prior.Label + " : " + (pri.CasesPrior).ToString()) : "") + ""
                }).ToList();
                response.RevenueMapData = result.Select(pri => new GenericMapChartMB
                {
                    value = (pri.CasesCurrent).ToString(),
                    id = pri.State.Trim(),
                    customData = filter.Periods.Current.Label + ": $" + (pri.RevenueCurrent.ToStripDecimal()).ToString("#,##0")
                    + " </br> " + filter.Periods.Historical.Label + " : $" + (pri.RevenueHistorical.ToStripDecimal()).ToString("#,##0") + " </br > " +
                  (filter.Periods.Prior.Label != string.Empty ? (filter.Periods.Prior.Label + " : $" + (pri.RevenuePrior.ToStripDecimal()).ToString("#,##0")) : "") + ""
                }).ToList();
                var sumvalue = result.Sum(x => x.RevenueCurrent);

                //response.Local = result.Select(pri => new GenericColumnChartBM
                //{
                //    Label1 = "State: [[title]] </br > " + filter.Periods.Current.Label + ": " + (double)(pri.RevenueCurrent.ToRoundTwoDecimalDigits())
                //    + " < br > " + filter.Periods.Historical.Label + " :" + (double)(pri.RevenueHistorical.ToRoundTwoDecimalDigits()) + " </br > " + filter.Periods.Prior.Label + " :" + (double)(pri.RevenuePrior.ToRoundTwoDecimalDigits()) + "",
                //    rValue1 = (double)(pri.RevenueCurrent.ToRoundTwoDecimalDigits()),
                //    Color1 = pri.State,
                //    Period1 = filter.Periods.Current.End,

                //    Label2 = filter.Periods.Current.Label,
                //    rValue2 = (double)(pri.RevenueCurrent).ToRoundTwoDecimalDigits(),
                //    Period2 = filter.Periods.Current.End,

                //    Label3 = filter.Periods.Historical.Label,
                //    rValue3 = (double)(pri.RevenueHistorical).ToRoundTwoDecimalDigits(),
                //    Period3 = filter.Periods.Historical.End,

                //    Label4 = filter.Periods.Historical.Label,
                //    rValue4 = (double)(pri.RevenueHistorical).ToRoundTwoDecimalDigits(),
                //    Period4 = filter.Periods.Historical.End,

                //    Label5 = filter.Periods.Prior.Label,
                //    rValue5 = (double)(pri.RevenuePrior).ToRoundTwoDecimalDigits(),
                //    Period5 = filter.Periods.Prior.End,

                //    Label6 = filter.Periods.Prior.Label,
                //    rValue6 = (double)(pri.RevenuePrior).ToRoundTwoDecimalDigits(),
                //    Period6 = filter.Periods.Prior.End

                //}).ToList();

                //response.Sales = MappingSalesToSalesPersonDashBoard(result, filter);
                // var customers = GetCustomersByAllSalesMan(result, filter);
                //response.Customer = MappingCustomersToSalesPersonDashBoard(result, customers, filter);
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
            return response;
        }
        /// <summary>
        /// Get data for the sold case, revenue report
        /// </summary>
        /// <param name="date"> Date of shipment </param>
        /// <returns> Short report </returns>
        public GenericColumnChartSalesDashBoardBM GetSalesManDashBoardReport(GlobalFilter filter)
        {

            GenericColumnChartSalesDashBoardBM response = new GenericColumnChartSalesDashBoardBM();

            try
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@historicalStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filter.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@priorStart", filter.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filter.Periods.Prior.End));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetSalesPersonInCasesSoldRevenueDashboard", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new MapCasesSoldRevenueGlobal
                            {
                                Comodity = !string.IsNullOrEmpty(x.Field<string>("Comodity")) ? x.Field<string>("Comodity").Trim() : "",
                                //SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? Constants.OutSalesPersons.ToList().Contains(x.Field<string>("SalesPersonCode").Trim()) ? "OSS" : x.Field<string>("SalesPersonCode").Trim() : "",
                                SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? x.Field<string>("SalesPersonCode").Trim() : "",
                                Category = !string.IsNullOrEmpty(x.Field<string>("category")) ? x.Field<string>("category").Trim() : "",
                                RevenueCurrent = x.Field<decimal?>("revenueCurrent").ToStripDecimal(),
                                RevenueHistorical = x.Field<decimal?>("RevenueHistorical").ToStripDecimal(),
                                RevenuePrior = x.Field<decimal?>("RevenuePrior").ToStripDecimal(),
                                CasesCurrent = x.Field<int?>("CasesCurrent") ?? 0,
                                CasesHistorical = x.Field<int?>("CasesHistorical") ?? 0,
                                CasesPrior = x.Field<int?>("CasesPrior") ?? 0,
                                CurrentEmployee = !string.IsNullOrEmpty(x.Field<string>("currentEmployee")) ? x.Field<string>("currentEmployee").Trim() : "",
                                HistoricalEmployee = !string.IsNullOrEmpty(x.Field<string>("historicalEmployee")) ? x.Field<string>("historicalEmployee").Trim() : "",
                                PriorEmployee = !string.IsNullOrEmpty(x.Field<string>("priorEmployee")) ? x.Field<string>("priorEmployee").Trim() : "",
                                ActiveEmployee = !string.IsNullOrEmpty(x.Field<string>("ActiveEmployee")) ? x.Field<string>("ActiveEmployee").Trim() : "",
                            })
                            .ToList();
                result = result.Where(x => x.SalesPersonCode != "DUMP" && x.SalesPersonCode != "DONA").ToList();
                response.Local = MappingToSalesPersonDashBoard(result, filter);
                response.All = MappingToGenericListRevenue(result, filter);/// MappingToSalesPersonDashBoard(result, filter);
                response.OutSales = GetOutSalesSalesManDashBoardReport(filter);

                response.Sales = MappingSalesToSalesPersonDashBoard(result, filter);
                response.CasesSold = MappingCasesSoldToSalesPersonDashBoard(result, filter);

                var customers = GetCustomersByAllSalesMan(result, filter);
                response.Customer = MappingCustomersToSalesPersonDashBoard(result, customers, filter);
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
            return response;
        }

        /// <summary>
        /// Get data for the out sales sold case, revenue report
        /// </summary>
        /// <param name="date"> Date of shipment </param>
        /// <returns> Short report </returns>
        public List<GenericColumnChartBM> GetOutSalesSalesManDashBoardReport(GlobalFilter filter)
        {
            List<GenericColumnChartBM> response = new List<GenericColumnChartBM>();

            try
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@historicalStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filter.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@priorStart", filter.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filter.Periods.Prior.End));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetOSSDashBoardReport", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new MapCasesSoldRevenueGlobal
                            {
                                Comodity = !string.IsNullOrEmpty(x.Field<string>("Comodity")) ? x.Field<string>("Comodity").Trim() : "",
                                SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? x.Field<string>("SalesPersonCode").Trim() : "",
                                RevenueCurrent = x.Field<decimal?>("revenue") ?? 0,
                                CasesCurrent = x.Field<int?>("Cases") ?? 0,
                                WeekNo = x.Field<int>("WeekNo"),
                                ActiveEmployee = !string.IsNullOrEmpty(x.Field<string>("ActiveEmployee")) ? x.Field<string>("ActiveEmployee").Trim() : "",
                                Period = x.Field<string>("period")
                            })
                            .ToList();
                result = result.Where(x => !x.SalesPersonCode.Contains("DUMP") && !x.SalesPersonCode.Contains("DONA")).ToList();
                response = MappingOutSalesToSalesPersonDashBoard(result, filter);
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
            return response;
        }
        public string GetSalesManName(string nameCombination)
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

        public string GetActiveSalesManName(string nameCombination)
        {
            //try
            //{
            //    var names = nameCombination.Split(',');
            //    if (names.Length > 1)
            //    {
            //        return names[1][1] + " " + names[0];
            //    }
            //}
            //catch (Exception ex){  }
            //return nameCombination;


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
        public List<GenericColumnChartBM> MappingToGenericListRevenue(List<MapCasesSoldRevenueGlobal> list, GlobalFilter filters)
        {
            List<GenericColumnChartBM> result = new List<GenericColumnChartBM>();
            list = list.OrderByDescending(s => s.RevenueCurrent).ToList();
            var data = new GenericColumnChartBM
            {
                GroupName = "",
                Label1 = filters.Periods.Current.Label,
                rValue1 = (double)(list.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits(),
                Period1 = filters.Periods.Current.End,
                Color1 = ChartColorBM.GrocerryCurrent,
                SalesPerson1 = "",

                Label2 = filters.Periods.Current.Label,
                rValue2 = (double)(list.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits(),
                Period2 = filters.Periods.Current.End,
                Color2 = ChartColorBM.ProduceCurrent,
                SalesPerson2 = "",

                Label3 = filters.Periods.Historical.Label,
                rValue3 = (double)(list.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits(),
                Period3 = filters.Periods.Historical.End,
                Color3 = ChartColorBM.GrocerryPrevious,
                SalesPerson3 = "",

                Label4 = filters.Periods.Historical.Label,
                rValue4 = (double)(list.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits(),
                Period4 = filters.Periods.Historical.End,
                Color4 = ChartColorBM.ProducePrevious,
                SalesPerson4 = "",

                Label5 = filters.Periods.Prior.Label,
                rValue5 = (double)(list.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits(),
                Period5 = filters.Periods.Prior.End,
                Color5 = ChartColorBM.GrocerryCurrent,
                SalesPerson5 = "",

                Label6 = filters.Periods.Prior.Label,
                rValue6 = (double)(list.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits(),
                Period6 = filters.Periods.Prior.End,
                Color6 = ChartColorBM.ProduceCurrent,
                SalesPerson6 = "",

                SubData = list.OrderByDescending(a => a.RevenueCurrent).Where(x => x.Category != Constants.CategorySchools && x.Category != Constants.CategoryHealthcare && x.Category != Constants.CategoryInstitute)
                    .GroupBy(x => x.Category)//.Take(5)
                    .Select((sec, secIdx) => new GenericColumnChartBM
                    {
                        GroupName = sec.Key,
                        Label1 = filters.Periods.Current.Label,
                        rValue1 = (double)(sec.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits(),
                        Period1 = filters.Periods.Current.End,
                        Color1 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                        SalesPerson1 = "",

                        Label2 = filters.Periods.Current.Label,
                        rValue2 = (double)(sec.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits(),
                        Period2 = filters.Periods.Current.End,
                        Color2 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                        SalesPerson2 = "",

                        Label3 = filters.Periods.Historical.Label,
                        rValue3 = (double)(sec.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits(),
                        Period3 = filters.Periods.Historical.End,
                        Color3 = BarColumnChartDistinctColors.Colors[secIdx].SecondaryGrocery,//ChartColorBM.GrocerryPrevious,
                        SalesPerson3 = "",

                        Label4 = filters.Periods.Historical.Label,
                        rValue4 = (double)(sec.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits(),
                        Period4 = filters.Periods.Historical.End,
                        Color4 = BarColumnChartDistinctColors.Colors[secIdx].SecondaryProduce,//ChartColorBM.ProducePrevious,
                        SalesPerson4 = "",

                        Label5 = filters.Periods.Prior.Label,
                        rValue5 = (double)(sec.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits(),
                        Period5 = filters.Periods.Prior.End,
                        Color5 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                        SalesPerson5 = "",

                        Label6 = filters.Periods.Prior.Label,
                        rValue6 = (double)(sec.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits(),
                        Period6 = filters.Periods.Prior.End,
                        Color6 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                        SalesPerson6 = "",

                        SubData = sec.Where(w => w.Category == sec.Key).OrderByDescending(a => a.RevenueCurrent)
                        .GroupBy(x => x.SalesPersonCode).Take(5)
                        .Select((thir, thirIdx) => new GenericColumnChartBM
                        {
                            GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(thir.First().ActiveEmployee) ? thir.First().ActiveEmployee : thir.Key),
                            ActiveSalesPersonCode = thir.Key,
                            Label1 = filters.Periods.Current.Label,
                            rValue1 = (double)(thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits(),
                            Period1 = filters.Periods.Current.End,
                            Color1 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                            SalesPerson1 = GetSalesManName(thir.First().CurrentEmployee),

                            Label2 = filters.Periods.Current.Label,
                            rValue2 = (double)(thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits(),
                            Period2 = filters.Periods.Current.End,
                            Color2 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                            SalesPerson2 = GetSalesManName(thir.First().CurrentEmployee),

                            Label3 = filters.Periods.Historical.Label,
                            rValue3 = (double)(thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits(),
                            Period3 = filters.Periods.Historical.End,
                            Color3 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryGrocery,//ChartColorBM.GrocerryPrevious,
                            SalesPerson3 = GetSalesManName(thir.First().HistoricalEmployee),

                            Label4 = filters.Periods.Historical.Label,
                            rValue4 = (double)(thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits(),
                            Period4 = filters.Periods.Historical.End,
                            Color4 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryProduce,//ChartColorBM.ProducePrevious,
                            SalesPerson4 = GetSalesManName(thir.First().HistoricalEmployee),

                            Label5 = filters.Periods.Prior.Label,
                            rValue5 = (double)(thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits(),
                            Period5 = filters.Periods.Prior.End,
                            Color5 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                            SalesPerson5 = GetSalesManName(thir.First().PriorEmployee),

                            Label6 = filters.Periods.Prior.Label,
                            rValue6 = (double)(thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits(),
                            Period6 = filters.Periods.Prior.End,
                            Color6 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                            SalesPerson6 = GetSalesManName(thir.First().PriorEmployee),

                        }).ToList().OrderByDescending(s => s.rValue1 + s.rValue2).ToList()
                    }).ToList().OrderByDescending(s => s.rValue1 + s.rValue2).ToList()
            };
            result.Add(data);
            return result;
        }

        public List<GenericColumnChartBM> MappingToGenericListRevenueFromSpeciality(List<MapCasesSoldRevenueGlobal> list, GlobalFilter filter)
        {
            List<GenericColumnChartBM> result = new List<GenericColumnChartBM>();
            list = list.OrderByDescending(s => s.RevenueCurrent).ToList();
            var data = new GenericColumnChartBM
            {
                GroupName = "",
                Label1 = filter.Periods.Current.Label,
                rValue1 = (double)(list.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                Period1 = filter.Periods.Current.End,
                Color1 = ChartColorBM.GrocerryCurrent,
                SalesPerson1 = "",

                Label2 = filter.Periods.Current.Label,
                rValue2 = (double)(list.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                Period2 = filter.Periods.Current.End,
                Color2 = ChartColorBM.ProduceCurrent,
                SalesPerson2 = "",

                Label3 = filter.Periods.Historical.Label,
                rValue3 = (double)(list.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                Period3 = filter.Periods.Historical.End,
                Color3 = ChartColorBM.GrocerryPrevious,
                SalesPerson3 = "",

                Label4 = filter.Periods.Historical.Label,
                rValue4 = (double)(list.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                Period4 = filter.Periods.Historical.End,
                Color4 = ChartColorBM.ProducePrevious,
                SalesPerson4 = "",

                Label5 = filter.Periods.Prior.Label,
                rValue5 = (double)(list.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                Period5 = filter.Periods.Prior.End,
                Color5 = ChartColorBM.GrocerryCurrent,
                SalesPerson5 = "",

                Label6 = filter.Periods.Prior.Label,
                rValue6 = (double)(list.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                Period6 = filter.Periods.Prior.End,
                Color6 = ChartColorBM.ProduceCurrent,
                SalesPerson6 = "",



                SubData = list.OrderByDescending(a => a.RevenueCurrent)
                        .GroupBy(x => x.SalesPersonCode).Take(5)
                        .Select((thir, thirIdx) => new GenericColumnChartBM
                        {
                            GroupName = GetActiveSalesManName(thir.First().ActiveEmployee != null ? thir.First().ActiveEmployee : thir.Key),
                            ActiveSalesPersonCode = thir.Key,
                            Label1 = filter.Periods.Current.Label,
                            rValue1 = (double)(thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                            Period1 = filter.Periods.Current.End,
                            Color1 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery, //ChartColorBM.GrocerryCurrent,
                            SalesPerson1 = thir.Where(d => d.Period == "current").FirstOrDefault() != null ? thir.Where(d => d.Period == "current").First().CurrentEmployee : "", // GetSalesManName(thir.First().CurrentYearEmployee),

                            Label2 = filter.Periods.Current.Label,
                            rValue2 = (double)(thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                            Period2 = filter.Periods.Current.End,
                            Color2 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce, //ChartColorBM.ProduceCurrent,
                            SalesPerson2 = thir.Where(d => d.Period == "current").FirstOrDefault() != null ? thir.Where(d => d.Period == "current").First().CurrentEmployee : "",  // GetSalesManName(thir.First().CurrentYearEmployee),

                            Label3 = filter.Periods.Historical.Label,
                            rValue3 = (double)(thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                            Period3 = filter.Periods.Historical.End,
                            Color3 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryGrocery, //ChartColorBM.GrocerryPrevious,
                            SalesPerson3 = thir.Where(d => d.Period == "historical").FirstOrDefault() != null ? thir.Where(d => d.Period == "historical").First().HistoricalEmployee : "",  //GetSalesManName(thir.First().PreviousYearEmployee),

                            Label4 = filter.Periods.Historical.Label,
                            rValue4 = (double)(thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                            Period4 = filter.Periods.Historical.End,
                            Color4 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryProduce, //ChartColorBM.ProducePrevious,
                            SalesPerson4 = thir.Where(d => d.Period == "historical").FirstOrDefault() != null ? thir.Where(d => d.Period == "historical").First().HistoricalEmployee : "", // GetSalesManName(thir.First().PreviousYearEmployee),

                            Label5 = filter.Periods.Prior.Label,
                            rValue5 = (double)(thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                            Period5 = filter.Periods.Prior.End,
                            Color5 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery, //ChartColorBM.GrocerryCurrent,
                            SalesPerson5 = thir.Where(d => d.Period == "prior").FirstOrDefault() != null ? thir.Where(d => d.Period == "prior").First().PriorEmployee : "",  //GetSalesManName(thir.First().CurrentYearEmployee),

                            Label6 = filter.Periods.Prior.Label,
                            rValue6 = (double)(thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits().ToStripDecimal(),
                            Period6 = filter.Periods.Prior.End,
                            Color6 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce, //ChartColorBM.ProduceCurrent,
                            SalesPerson6 = thir.Where(d => d.Period == "prior").FirstOrDefault() != null ? thir.Where(d => d.Period == "prior").First().PriorEmployee : "", // GetSalesManName(thir.First().CurrentYearEmployee),

                        }).ToList().OrderByDescending(s => s.rValue1 + s.rValue2).ToList()
            };
            result.Add(data);
            return result;
        }

        public List<GenericColumnChartBM> MappingToGenericListCasesold(List<MapCasesSoldRevenueGlobal> list, GlobalFilter filters)
        {
            List<GenericColumnChartBM> result = new List<GenericColumnChartBM>();
            list = list.OrderByDescending(s => s.CasesCurrent).ToList();

            var data = new GenericColumnChartBM
            {
                GroupName = "",
                Label1 = filters.Periods.Current.Label,
                cValue1 = (list.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.CasesCurrent)),
                rValue1 = (double)(list.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)),
                Period1 = filters.Periods.Current.End,
                Color1 = ChartColorBM.GrocerryCurrent,
                SalesPerson1 = "",

                Label2 = filters.Periods.Current.Label,
                cValue2 = (list.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.CasesCurrent)),
                rValue2 = (double)(list.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)),
                Period2 = filters.Periods.Current.End,
                Color2 = ChartColorBM.ProduceCurrent,
                SalesPerson2 = "",

                Label3 = filters.Periods.Historical.Label,
                cValue3 = (list.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.CasesHistorical)),
                rValue3 = (double)(list.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueHistorical)),
                Period3 = filters.Periods.Historical.End,
                Color3 = ChartColorBM.GrocerryPrevious,
                SalesPerson3 = "",

                Label4 = filters.Periods.Historical.Label,
                cValue4 = (list.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.CasesHistorical)),
                rValue4 = (double)(list.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenueHistorical)),
                Period4 = filters.Periods.Historical.End,
                Color4 = ChartColorBM.ProducePrevious,
                SalesPerson4 = "",

                Label5 = filters.Periods.Prior.Label,
                cValue5 = (list.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.CasesPrior)),
                rValue5 = (double)(list.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenuePrior)),
                Period5 = filters.Periods.Prior.End,
                Color5 = ChartColorBM.GrocerryCurrent,
                SalesPerson5 = "",

                Label6 = filters.Periods.Prior.Label,
                cValue6 = (list.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.CasesPrior)),
                rValue6 = (double)(list.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenuePrior)),
                Period6 = filters.Periods.Prior.End,
                Color6 = ChartColorBM.ProduceCurrent,
                SalesPerson6 = "",

                SubData = list.OrderByDescending(a => a.CasesCurrent)
                               .Where(x => x.Category != Constants.CategorySchools
                                            && x.Category != Constants.CategoryHealthcare
                                            && x.Category != Constants.CategoryInstitute)
                     .GroupBy(x => x.Category)//.Take(5)
                     .Select((sec, secIdx) => new GenericColumnChartBM
                     {
                         GroupName = sec.Key,
                         Label1 = filters.Periods.Current.Label,
                         cValue1 = (sec.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.CasesCurrent)),
                         rValue1 = (double)(sec.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)),
                         Period1 = filters.Periods.Current.End,
                         Color1 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                         SalesPerson1 = "",

                         Label2 = filters.Periods.Current.Label,
                         cValue2 = (sec.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.CasesCurrent)),
                         rValue2 = (double)(sec.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)),
                         Period2 = filters.Periods.Current.End,
                         Color2 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                         SalesPerson2 = "",

                         Label3 = filters.Periods.Historical.Label,
                         cValue3 = (sec.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.CasesHistorical)),
                         rValue3 = (double)(sec.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueHistorical)),
                         Period3 = filters.Periods.Historical.End,
                         Color3 = BarColumnChartDistinctColors.Colors[secIdx].SecondaryGrocery,//ChartColorBM.GrocerryPrevious,
                         SalesPerson3 = "",

                         Label4 = filters.Periods.Historical.Label,
                         cValue4 = (sec.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.CasesHistorical)),
                         rValue4 = (double)(sec.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenueHistorical)),
                         Period4 = filters.Periods.Historical.End,
                         Color4 = BarColumnChartDistinctColors.Colors[secIdx].SecondaryProduce,//ChartColorBM.ProducePrevious,
                         SalesPerson4 = "",

                         Label5 = filters.Periods.Prior.Label,
                         cValue5 = (sec.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.CasesPrior)),
                         rValue5 = (double)(sec.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenuePrior)),
                         Period5 = filters.Periods.Prior.End,
                         Color5 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                         SalesPerson5 = "",

                         Label6 = filters.Periods.Prior.Label,
                         cValue6 = (sec.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.CasesPrior)),
                         rValue6 = (double)(sec.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenuePrior)),
                         Period6 = filters.Periods.Prior.End,
                         Color6 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                         SalesPerson6 = "",

                         SubData = sec.Where(w => w.Category == sec.Key).OrderByDescending(a => a.CasesCurrent)
                         .GroupBy(x => x.SalesPersonCode).Take(5)
                         .Select((thir, thirIdx) => new GenericColumnChartBM
                         {
                             GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(thir.First().ActiveEmployee) ? thir.First().ActiveEmployee : thir.Key),
                             ActiveSalesPersonCode = thir.Key,
                             Label1 = filters.Periods.Current.Label,

                             cValue1 = (thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesCurrent)),
                             rValue1 = (double)(thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)),
                             Period1 = filters.Periods.Current.End,
                             Color1 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                             SalesPerson1 = GetSalesManName(thir.First().CurrentEmployee),

                             Label2 = filters.Periods.Current.Label,
                             cValue2 = (thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesCurrent)),
                             rValue2 = (double)(thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)),
                             Period2 = filters.Periods.Current.End,
                             Color2 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                             SalesPerson2 = GetSalesManName(thir.First().CurrentEmployee),

                             Label3 = filters.Periods.Historical.Label,
                             cValue3 = (thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesHistorical)),
                             rValue3 = (double)(thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.RevenueHistorical)),
                             Period3 = filters.Periods.Historical.End,
                             Color3 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryGrocery,//ChartColorBM.GrocerryPrevious,
                             SalesPerson3 = GetSalesManName(thir.First().HistoricalEmployee),

                             Label4 = filters.Periods.Historical.Label,
                             cValue4 = (thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesHistorical)),
                             rValue4 = (double)(thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.RevenueHistorical)),

                             Period4 = filters.Periods.Current.End,
                             Color4 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryProduce,//ChartColorBM.ProducePrevious,
                             SalesPerson4 = GetSalesManName(thir.First().HistoricalEmployee),

                             Label5 = filters.Periods.Prior.Label,
                             cValue5 = (thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesPrior)),
                             rValue5 = (double)(thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.RevenuePrior)),
                             Period5 = filters.Periods.Prior.End,
                             Color5 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                             SalesPerson5 = GetSalesManName(thir.First().PriorEmployee),

                             Label6 = filters.Periods.Prior.Label,
                             cValue6 = (thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesPrior)),
                             rValue6 = (double)(thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.RevenuePrior)),
                             Period6 = filters.Periods.Prior.End,
                             Color6 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                             SalesPerson6 = GetSalesManName(thir.First().PriorEmployee),

                         }).ToList().OrderByDescending(s => s.cValue1 + s.cValue2).ToList()
                     }).ToList().OrderByDescending(s => s.cValue1 + s.cValue2).ToList()
            };
            data.Label1 = data.Label1.Replace("Jan Jan", "Jan");
            data.Label2 = data.Label2.Replace("Jan Jan", "Jan");
            data.Label3 = data.Label3.Replace("Jan Jan", "Jan");
            data.Label4 = data.Label4.Replace("Jan Jan", "Jan");
            data.Label5 = data.Label5.Replace("Jan Jan", "Jan");
            data.Label6 = data.Label6.Replace("Jan Jan", "Jan");
            result.Add(data);

            return result;
        }

        public List<GenericColumnChartBM> MappingToGenericListCasesoldFromSpeciality(List<MapCasesSoldRevenueGlobal> list, GlobalFilter filter)
        {
            List<GenericColumnChartBM> result = new List<GenericColumnChartBM>();
            list = list.OrderByDescending(s => s.CasesCurrent).ToList();
            var data = new GenericColumnChartBM
            {
                GroupName = "",
                Label1 = filter.Periods.Current.Label,
                cValue1 = (list.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.CasesCurrent)),
                Period1 = filter.Periods.Current.End,
                Color1 = ChartColorBM.GrocerryCurrent,
                SalesPerson1 = "",

                Label2 = filter.Periods.Current.Label,
                cValue2 = (list.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.CasesCurrent)),
                Period2 = filter.Periods.Current.End,
                Color2 = ChartColorBM.ProduceCurrent,
                SalesPerson2 = "",

                Label3 = filter.Periods.Historical.Label,
                cValue3 = (list.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.CasesHistorical)),
                Period3 = filter.Periods.Historical.End,
                Color3 = ChartColorBM.GrocerryPrevious,
                SalesPerson3 = "",

                Label4 = filter.Periods.Historical.Label,
                cValue4 = (list.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.CasesHistorical)),
                Period4 = filter.Periods.Historical.End,
                Color4 = ChartColorBM.ProducePrevious,
                SalesPerson4 = "",

                Label5 = filter.Periods.Prior.Label,
                cValue5 = (list.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.CasesPrior)),
                Period5 = filter.Periods.Prior.End,
                Color5 = ChartColorBM.GrocerryCurrent,
                SalesPerson5 = "",

                Label6 = filter.Periods.Prior.Label,
                cValue6 = (list.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.CasesPrior)),
                Period6 = filter.Periods.Prior.End,
                Color6 = ChartColorBM.ProduceCurrent,
                SalesPerson6 = "",


                SubData = list.OrderByDescending(a => a.CasesCurrent)
                         .GroupBy(x => x.SalesPersonCode).Take(5)
                         .Select((thir, thirIdx) => new GenericColumnChartBM
                         {
                             GroupName = GetActiveSalesManName(thir.First().ActiveEmployee != null ? thir.First().ActiveEmployee : thir.Key),
                             ActiveSalesPersonCode = thir.Key,
                             Label1 = filter.Periods.Current.Label,
                             cValue1 = (thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesCurrent)),
                             Period1 = filter.Periods.Current.End,
                             Color1 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                             SalesPerson1 = thir.Where(g => g.Period == "current").FirstOrDefault() != null ? thir.Where(g => g.Period == "current").First().CurrentEmployee : "",//GetSalesManName(thir.First().CurrentYearEmployee),

                             Label2 = filter.Periods.Current.Label,
                             cValue2 = (thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesCurrent)),
                             Period2 = filter.Periods.Current.End,
                             Color2 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce, //ChartColorBM.ProduceCurrent,
                             SalesPerson2 = thir.Where(g => g.Period == "current").FirstOrDefault() != null ? thir.Where(g => g.Period == "current").First().CurrentEmployee : "",//GetSalesManName(thir.First().CurrentYearEmployee),

                             Label3 = filter.Periods.Historical.Label,
                             cValue3 = (thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesHistorical)),
                             Period3 = filter.Periods.Historical.End,
                             Color3 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryGrocery, //ChartColorBM.GrocerryPrevious,
                             SalesPerson3 = thir.Where(g => g.Period == "historical").FirstOrDefault() != null ? thir.Where(g => g.Period == "historical").First().HistoricalEmployee : "",//GetSalesManName(thir.First().CurrentYearEmployee),

                             Label4 = filter.Periods.Historical.Label,
                             cValue4 = (thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesHistorical)),
                             Period4 = filter.Periods.Historical.End,
                             Color4 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryProduce, //ChartColorBM.ProducePrevious,
                             SalesPerson4 = thir.Where(g => g.Period == "historical").FirstOrDefault() != null ? thir.Where(g => g.Period == "historical").First().HistoricalEmployee : "",//GetSalesManName(thir.First().CurrentYearEmployee),

                             Label5 = filter.Periods.Prior.Label,
                             cValue5 = (thir.Where(d => d.Comodity.ToLower() == "grocery").Sum(t => t.CasesPrior)),
                             Period5 = filter.Periods.Prior.End,
                             Color5 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery, //ChartColorBM.GrocerryCurrent,
                             SalesPerson5 = thir.Where(g => g.Period == "prior").FirstOrDefault() != null ? thir.Where(g => g.Period == "prior").First().PriorEmployee : "",//GetSalesManName(thir.First().CurrentYearEmployee),

                             Label6 = filter.Periods.Prior.Label,
                             cValue6 = (thir.Where(d => d.Comodity.ToLower() == "produce").Sum(t => t.CasesPrior)),
                             Period6 = filter.Periods.Prior.End,
                             Color6 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce, //ChartColorBM.ProduceCurrent,
                             SalesPerson6 = thir.Where(g => g.Period == "prior").FirstOrDefault() != null ? thir.Where(g => g.Period == "prior").First().PriorEmployee : ""

                         }).ToList().OrderByDescending(s => s.cValue1 + s.cValue2).ToList()

            };
            result.Add(data);
            return result;
        }

        public List<GenericColumnChartBM> MappingToGenericListCasesoldFromCompType(List<MapCasesSoldRevenueGlobal> list, string compType, GlobalFilter filters)
        {
            List<GenericColumnChartBM> result = new List<GenericColumnChartBM>();
            list = list.OrderByDescending(s => s.CasesCurrent).ToList();
            var data = new GenericColumnChartBM
            {
                GroupName = compType,
                Label1 = filters.Periods.Current.Label,
                cValue1 = (list.Where(f => f.Comodity.ToLower() == compType).Sum(t => t.CasesCurrent)),
                Period1 = filters.Periods.Current.End,
                //Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                Color1 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[0].PrimaryGrocery : BarColumnChartDistinctColors.Colors[0].PrimaryProduce,
                SalesPerson1 = "",

                Label2 = filters.Periods.Historical.Label,
                cValue2 = (list.Where(f => f.Comodity.ToLower() == compType).Sum(t => t.CasesHistorical)),
                Period2 = filters.Periods.Historical.End,
                //Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                Color2 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[0].SecondaryGrocery : BarColumnChartDistinctColors.Colors[0].SecondaryProduce,
                SalesPerson2 = "",

                Label3 = filters.Periods.Prior.Label,
                cValue3 = (list.Where(f => f.Comodity.ToLower() == compType).Sum(t => t.CasesPrior)),
                Period3 = filters.Periods.Prior.End,
                //Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                Color3 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[0].PrimaryGrocery : BarColumnChartDistinctColors.Colors[0].PrimaryProduce,

                SalesPerson3 = "",

                SubData = list.OrderByDescending(a => a.CasesCurrent).Where(x => x.Category != Constants.CategorySchools && x.Category != Constants.CategoryHealthcare && x.Category != Constants.CategoryInstitute)
                     .GroupBy(x => x.Category)//.Take(5)
                     .Select((sec, idx) => new GenericColumnChartBM
                     {
                         GroupName = sec.Key,
                         Label1 = filters.Periods.Current.Label,
                         cValue1 = (sec.Where(f => f.Comodity.ToLower() == compType).Sum(t => t.CasesCurrent)),
                         Period1 = filters.Periods.Current.End,
                         //Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                         Color1 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[idx].PrimaryGrocery : BarColumnChartDistinctColors.Colors[idx].PrimaryProduce,
                         SalesPerson1 = "",



                         Label2 = filters.Periods.Historical.Label,
                         cValue2 = (sec.Where(f => f.Comodity.ToLower() == compType).Sum(t => t.CasesHistorical)),
                         Period2 = filters.Periods.Historical.End,
                         //Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                         Color2 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[idx].SecondaryGrocery : BarColumnChartDistinctColors.Colors[idx].SecondaryProduce,
                         SalesPerson2 = "",



                         Label3 = filters.Periods.Prior.Label,
                         cValue3 = (sec.Where(f => f.Comodity.ToLower() == compType).Sum(t => t.CasesPrior)),
                         Period3 = filters.Periods.Prior.End,
                         //Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                         Color3 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[idx].PrimaryGrocery : BarColumnChartDistinctColors.Colors[idx].PrimaryProduce,
                         SalesPerson3 = "",


                         SubData = sec.Where(w => w.Category == sec.Key).OrderByDescending(a => a.CasesCurrent)
                         .GroupBy(x => x.SalesPersonCode).Take(5)
                         .Select((thir, thirIdx) => new GenericColumnChartBM
                         {
                             GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(thir.First().ActiveEmployee) ? thir.First().ActiveEmployee : thir.Key),
                             ActiveSalesPersonCode = thir.Key,
                             Label1 = filters.Periods.Current.Label,
                             cValue1 = (thir.Where(d => d.Comodity.ToLower() == compType).Sum(t => t.CasesCurrent)),
                             Period1 = filters.Periods.Current.End,
                             //Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             Color1 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery : BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,
                             SalesPerson1 = GetSalesManName(thir.First().CurrentEmployee),

                             Label2 = filters.Periods.Historical.Label,
                             cValue2 = (thir.Where(d => d.Comodity.ToLower() == compType).Sum(t => t.CasesHistorical)),
                             Period2 = filters.Periods.Historical.End,
                             //Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                             Color2 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[thirIdx].SecondaryGrocery : BarColumnChartDistinctColors.Colors[thirIdx].SecondaryProduce,
                             SalesPerson2 = GetSalesManName(thir.First().HistoricalEmployee),

                             cValue3 = (thir.Where(d => d.Comodity.ToLower() == compType).Sum(t => t.CasesPrior)),
                             Period3 = filters.Periods.Prior.End,
                             //Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             Color3 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery : BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,
                             SalesPerson3 = GetSalesManName(thir.First().PriorEmployee),


                         }).ToList().OrderByDescending(s => s.cValue1 + s.cValue2).ToList()
                     }).ToList().OrderByDescending(s => s.cValue1 + s.cValue2).ToList()
            };
            result.Add(data);
            return result;
        }

        public List<GenericColumnChartBM> MappingToGenericListRevenueFromCompType(List<MapCasesSoldRevenueGlobal> list, string compType, GlobalFilter filters)
        {
            List<GenericColumnChartBM> result = new List<GenericColumnChartBM>();
            list = list.OrderByDescending(s => s.RevenueCurrent).ToList();
            var data = new GenericColumnChartBM
            {
                GroupName = compType,
                Label1 = filters.Periods.Current.Label,
                rValue1 = (double)(list.Where(f => f.Comodity.ToLower() == compType).Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits(),
                Period1 = filters.Periods.Current.End,
                Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                SalesPerson1 = "",

                Label2 = filters.Periods.Historical.Label,
                rValue2 = (double)(list.Where(f => f.Comodity.ToLower() == compType).Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits(),
                Period2 = filters.Periods.Historical.End,
                Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                SalesPerson2 = "",

                Label3 = filters.Periods.Prior.Label,
                rValue3 = (double)(list.Where(f => f.Comodity.ToLower() == compType).Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits(),
                Period3 = filters.Periods.Prior.End,
                Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                SalesPerson3 = "",

                SubData = list.OrderByDescending(a => a.RevenueCurrent)
                .Where(x => x.Category != Constants.CategorySchools
                && x.Category != Constants.CategoryHealthcare && x.Category != Constants.CategoryInstitute)
                     .GroupBy(x => x.Category)//.Take(5)
                     .Select(sec => new GenericColumnChartBM
                     {
                         GroupName = sec.Key,
                         Label1 = filters.Periods.Current.Label,
                         rValue1 = (double)(sec.Where(f => f.Comodity.ToLower() == compType).Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits(),
                         Period1 = filters.Periods.Current.End,
                         Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                         SalesPerson1 = "",



                         Label2 = filters.Periods.Historical.Label,
                         rValue2 = (double)(sec.Where(f => f.Comodity.ToLower() == compType).Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits(),
                         Period2 = filters.Periods.Historical.End,
                         Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                         SalesPerson2 = "",



                         Label3 = filters.Periods.Prior.Label,
                         rValue3 = (double)(sec.Where(f => f.Comodity.ToLower() == compType).Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits(),
                         Period3 = filters.Periods.Prior.End,
                         Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                         SalesPerson3 = "",


                         SubData = sec.Where(w => w.Category == sec.Key).OrderByDescending(a => a.RevenueCurrent)
                         .GroupBy(x => x.SalesPersonCode).Take(5).Select(thir => new GenericColumnChartBM
                         {
                             GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(thir.First().ActiveEmployee) ? thir.First().ActiveEmployee : thir.Key),
                             ActiveSalesPersonCode = thir.Key,
                             Label1 = filters.Periods.Current.Label,
                             rValue1 = (double)(thir.Where(d => d.Comodity.ToLower() == compType).Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits(),
                             Period1 = filters.Periods.Current.End,
                             Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             SalesPerson1 = GetSalesManName(thir.First().CurrentEmployee),



                             Label2 = filters.Periods.Historical.Label,
                             rValue2 = (double)(thir.Where(d => d.Comodity.ToLower() == compType).Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits(),
                             Period2 = filters.Periods.Historical.End,
                             Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                             SalesPerson2 = GetSalesManName(thir.First().HistoricalEmployee),


                             Label3 = filters.Periods.Prior.Label,
                             rValue3 = (double)(thir.Where(d => d.Comodity.ToLower() == compType).Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits(),
                             Period3 = filters.Periods.Prior.End,
                             Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             SalesPerson3 = GetSalesManName(thir.First().PriorEmployee),


                         }).ToList().OrderByDescending(s => s.rValue1 + s.rValue2).ToList()
                     }).ToList().OrderByDescending(s => s.rValue1 + s.rValue2).ToList()
            };
            result.Add(data);
            return result;
        }
    
        public GenericColumnChartCategoryBM MappingToGenericListCaseSoldFromCategory(List<MapCasesSoldRevenueGlobal> list, string category, GlobalFilter filter)
        {

            var result = new GenericColumnChartCategoryBM
            {
                //Grocery = list.GetCasesFromSpecificCondition(x => x.Comodity == "Grocery" && x.Category.ToLower() == category.ToLower(), filter, "Grocery"),
                //Produce = list.GetCasesFromSpecificCondition(x => x.Comodity == "Produce" && x.Category.ToLower() == category.ToLower(), filter, "Produce"),
                All = MappingToGenericListCasesoldFromSpeciality(list.Where(h => h.Category.ToLower() == category.ToLower()).ToList(), filter)
            };

            return result;
        }

        public GenericColumnChartCategoryBM MappingToGenericListRevenueFromCategory(List<MapCasesSoldRevenueGlobal> list, string category, GlobalFilter filter)
        {

            var result = new GenericColumnChartCategoryBM
            {
                //Grocery = list.GetRevenueFromSpecificCondition(x => x.Comodity == "Grocery" && x.Category.ToLower() == category.ToLower(), filter, "Grocery"),
                //Produce = list.GetRevenueFromSpecificCondition(x => x.Comodity == "Produce" && x.Category.ToLower() == category.ToLower(), filter, "Produce"),
                All = MappingToGenericListRevenueFromSpeciality(list.Where(h => h.Category.ToLower() == category.ToLower()).ToList(), filter)
            };

            return result;
        }

        public GenericColumnChartCategoryBM MappingToSpecificCategoryCasesSoldFromSalesMan(List<MapCasesSoldRevenueGlobal> list, string filter, GlobalFilter filterData)
        {
            var result = new GenericColumnChartCategoryBM
            {
                //Grocery = list.GetCasesFromSpecificCondition(x => x.Comodity == "Grocery" && x.SalesPersonCode.ToLower().StartsWith(filter.ToLower()), filterData, "Grocery"),
                //Produce = list.GetCasesFromSpecificCondition(x => x.Comodity == "Produce" && x.SalesPersonCode.ToLower().StartsWith(filter.ToLower()), filterData, "Produce"),
            };
            result.All = MappingToGenericListCasesoldFromSpeciality(list.Where(x => x.SalesPersonCode.ToLower().StartsWith(filter.ToLower())).ToList(), filterData);

            return result;
        }

        public GenericColumnChartCategoryBM MappingToSpecificCategoryRevenueFromSalesMan(List<MapCasesSoldRevenueGlobal> list, string filter, GlobalFilter filterData)
        {
            var result = new GenericColumnChartCategoryBM
            {
                Grocery = list.GetRevenueFromSpecificCondition(x => x.Comodity == "Grocery" && x.SalesPersonCode.ToLower().StartsWith(filter.ToLower()), filterData, "Grocery"),
                Produce = list.GetRevenueFromSpecificCondition(x => x.Comodity == "Produce" && x.SalesPersonCode.ToLower().StartsWith(filter.ToLower()), filterData, "Produce"),
            };
            result.All = MappingToGenericListRevenueFromSpeciality(list, filterData);

            return result;
        }

        public List<MapCasesSoldRevenueGlobal> GetCustomersByAllSalesMan(List<MapCasesSoldRevenueGlobal> list, GlobalFilter filters)
        {
            //find all customers
            var salesMen = list.GroupBy(x => x.SalesPersonCode).Select(x => x.Key).ToList();
            if (salesMen.Contains("OSS"))
            {
                salesMen = salesMen.Where(x => x != "OSS").ToList();
                salesMen.AddRange(Constants.OutSalesPersons.ToList());

            }

            //string salesMenParam = string.Join(",", salesMen);

            DataTable dtSalesPerson = new DataTable();
            dtSalesPerson.Columns.Add("Code");
            foreach (string code in salesMen)
            {
                dtSalesPerson.Rows.Add(code);
            }
            List<SqlParameter> parameterList = new List<SqlParameter>();
            //parameterList.Add(new SqlParameter("@salesmen", salesMenParam));
            var param = new SqlParameter();
            param.SqlDbType = SqlDbType.Structured;
            param.Value = dtSalesPerson;
            param.ParameterName = "@salesmen";
            parameterList.Add(param);
            parameterList.Add(new SqlParameter("@currentStart", filters.Periods.Current.Start));
            parameterList.Add(new SqlParameter("@currentEnd", filters.Periods.Current.End));
            parameterList.Add(new SqlParameter("@historicalStart", filters.Periods.Historical.Start));
            parameterList.Add(new SqlParameter("@historicalEnd", filters.Periods.Historical.End));
            parameterList.Add(new SqlParameter("@priorStart", filters.Periods.Prior.Start));
            parameterList.Add(new SqlParameter("@priorEnd", filters.Periods.Prior.End));
            var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetCustomersFromSalesPersons", parameterList.ToArray());
            var result = dataSetResult.Tables[0]
               .AsEnumerable()
               .Select(x => new MapCasesSoldRevenueGlobal
               {

                   Comodity = !string.IsNullOrEmpty(x.Field<string>("comptype")) ? x.Field<string>("comptype").Trim() : "",
                   SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("salesman")) ? x.Field<string>("salesman").Trim() : "",
                   Customers = !string.IsNullOrEmpty(x.Field<string>("company")) ? x.Field<string>("company").Trim() : "",
                   CasesCurrent = x.Field<int?>("qty") ?? 0,
                   RevenueCurrent = x.Field<decimal?>("amount") ?? 0,
                   Period = x.Field<string>("period"),
               })
                                          .ToList();
            result = result.Where(f => f.SalesPersonCode != "DUMP" && f.SalesPersonCode != "DONA").ToList();
            return result;

        }

        public List<GenericColumnChartBM> MappingToSalesPersonDashBoard(List<MapCasesSoldRevenueGlobal> list
                          , GlobalFilter filter)
        {

            var result = list.GroupBy(t => t.Category).OrderByDescending(f => f.Sum(e => e.RevenueCurrent))
                 .Select(pri => new GenericColumnChartBM
                 {
                     GroupName = pri.Key,
                     Label1 = filter.Periods.Current.Label,
                     rValue1 = (double)(pri.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits(),
                     Period1 = filter.Periods.Current.End,
                     Color1 = ChartColorBM.GrocerryCurrent,
                     SalesPerson1 = GetSalesManName(pri.Where(f => f.Comodity.ToLower() == "grocery").FirstOrDefault() != null ? pri.Where(f => f.Comodity.ToLower() == "grocery").FirstOrDefault().CurrentEmployee : ""),

                     Label2 = filter.Periods.Current.Label,
                     rValue2 = (double)(pri.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits(),
                     Period2 = filter.Periods.Current.End,
                     Color2 = ChartColorBM.ProduceCurrent,
                     SalesPerson2 = GetSalesManName(pri.Where(f => f.Comodity.ToLower() == "produce").FirstOrDefault() != null ? pri.Where(f => f.Comodity.ToLower() == "produce").FirstOrDefault().CurrentEmployee : ""),

                     Label3 = filter.Periods.Historical.Label,
                     rValue3 = (double)(pri.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits(),
                     Period3 = filter.Periods.Historical.End,
                     Color3 = ChartColorBM.GrocerryPrevious,
                     SalesPerson3 = GetSalesManName(pri.Where(f => f.Comodity.ToLower() == "grocery").FirstOrDefault() != null ? pri.Where(f => f.Comodity.ToLower() == "grocery").FirstOrDefault().HistoricalEmployee : ""),

                     Label4 = filter.Periods.Historical.Label,
                     rValue4 = (double)(pri.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits(),
                     Period4 = filter.Periods.Historical.End,
                     Color4 = ChartColorBM.ProducePrevious,
                     SalesPerson4 = GetSalesManName(pri.Where(f => f.Comodity.ToLower() == "produce").FirstOrDefault() != null ? pri.Where(f => f.Comodity.ToLower() == "produce").FirstOrDefault().HistoricalEmployee : ""),

                     Label5 = filter.Periods.Prior.Label,
                     rValue5 = (double)(pri.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits(),
                     Period5 = filter.Periods.Prior.End,
                     Color5 = ChartColorBM.GrocerryCurrent,
                     SalesPerson5 = GetSalesManName(pri.Where(f => f.Comodity.ToLower() == "grocery").FirstOrDefault() != null ? pri.Where(f => f.Comodity.ToLower() == "grocery").FirstOrDefault().PriorEmployee : ""),

                     Label6 = filter.Periods.Prior.Label,
                     rValue6 = (double)(pri.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits(),
                     Period6 = filter.Periods.Prior.End,
                     Color6 = ChartColorBM.ProduceCurrent,
                     SalesPerson6 = GetSalesManName(pri.Where(f => f.Comodity.ToLower() == "produce").FirstOrDefault() != null ? pri.Where(f => f.Comodity.ToLower() == "produce").FirstOrDefault().PriorEmployee : ""),

                     SubData = pri.GroupBy(d => d.SalesPersonCode).OrderByDescending(g => g.Sum(h => h.RevenueCurrent))
                            .Select(sec => new GenericColumnChartBM
                            {
                                GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(sec.First().ActiveEmployee) ? sec.First().ActiveEmployee : sec.Key),
                                ActiveSalesPersonCode = sec.Key,
                                Label1 = filter.Periods.Current.Label,
                                rValue1 = (double)(sec.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits(),
                                Period1 = filter.Periods.Current.End,
                                Color1 = ChartColorBM.GrocerryCurrent,
                                SalesPerson1 = "",

                                Label2 = filter.Periods.Current.Label,
                                rValue2 = (double)(sec.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)).ToRoundTwoDecimalDigits(),
                                Period2 = filter.Periods.Current.End,
                                Color2 = ChartColorBM.ProduceCurrent,
                                SalesPerson2 = "",

                                Label3 = filter.Periods.Historical.Label,
                                rValue3 = (double)(sec.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits(),
                                Period3 = filter.Periods.Historical.End,
                                Color3 = ChartColorBM.GrocerryPrevious,
                                SalesPerson3 = "",

                                Label4 = filter.Periods.Historical.Label,
                                rValue4 = (double)(sec.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenueHistorical)).ToRoundTwoDecimalDigits(),
                                Period4 = filter.Periods.Current.End,
                                Color4 = ChartColorBM.ProducePrevious,
                                SalesPerson4 = "",

                                Label5 = filter.Periods.Prior.Label,
                                rValue5 = (double)(sec.Where(f => f.Comodity.ToLower() == "grocery").Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits(),
                                Period5 = filter.Periods.Prior.End,
                                Color5 = ChartColorBM.GrocerryCurrent,
                                SalesPerson5 = "",

                                Label6 = filter.Periods.Prior.Label,
                                rValue6 = (double)(sec.Where(f => f.Comodity.ToLower() == "produce").Sum(t => t.RevenuePrior)).ToRoundTwoDecimalDigits(),
                                Period6 = filter.Periods.Prior.End,
                                Color6 = ChartColorBM.ProduceCurrent,
                                SalesPerson6 = "",
                            }).ToList()

                 }).ToList();



            return result;
        }
        public List<GenericColumnChartBM> MappingOutSalesToSalesPersonDashBoard(List<MapCasesSoldRevenueGlobal> list
                       , GlobalFilter filter)
        {

            var result = list.GroupBy(t => t.SalesPersonCode).OrderByDescending(f => f.Sum(e => e.RevenueCurrent))
                 .Select(pri => new GenericColumnChartBM
                 {
                     GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(pri.First().ActiveEmployee) ? pri.First().ActiveEmployee : pri.Key),
                     ActiveSalesPersonCode = pri.Key,


                     Label1 = filter.Periods.Current.Label,
                     rValue1 = (double)(pri.Where(f => f.Period == "current"
                      && f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)),
                     Period1 = filter.Periods.Current.End,
                     Color1 = ChartColorBM.GrocerryCurrent,
                     SalesPerson1 = "",

                     Label2 = filter.Periods.Current.Label,
                     rValue2 = (double)(pri.Where(f => f.Period == "current"
                     && f.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)),
                     Period2 = filter.Periods.Current.End,
                     Color2 = ChartColorBM.ProduceCurrent,
                     SalesPerson2 = "",

                     Label3 = filter.Periods.Historical.Label,
                     rValue3 = (double)(pri.Where(f => f.Period == "historical" && f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)),
                     Period3 = filter.Periods.Historical.End,
                     Color3 = ChartColorBM.GrocerryPrevious,
                     SalesPerson3 = "",

                     Label4 = filter.Periods.Historical.Label,
                     rValue4 = (double)(pri.Where(f => f.Period == "historical"
                     && f.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)),
                     Period4 = filter.Periods.Historical.End,
                     Color4 = ChartColorBM.ProducePrevious,
                     SalesPerson4 = "",

                     Label5 = filter.Periods.Prior.Label,
                     rValue5 = (double)(pri.Where(f => f.Period == "prior" && f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)),
                     Period5 = filter.Periods.Prior.End,
                     Color5 = ChartColorBM.GrocerryCurrent,
                     SalesPerson5 = "",

                     Label6 = filter.Periods.Prior.Label,
                     rValue6 = (double)(pri.Where(f => f.Period == "prior" && f.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)),
                     Period6 = filter.Periods.Prior.End,
                     Color6 = ChartColorBM.ProduceCurrent,
                     SalesPerson6 = "",

                     SubData = pri.GroupBy(d => d.WeekNo)
                            .Select(sec => new GenericColumnChartBM
                            {
                                ActiveSalesPersonCode = pri.Key,
                                GroupName = pri.Max(d => d.WeekNo) == sec.Key ? "Last Wk" : "Prior Wk",
                                Label1 = filter.Periods.Current.Label,
                                rValue1 = (double)(pri.Where(f => f.WeekNo == sec.Key && f.Period == "current" && f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)),
                                cValue1 = pri.Where(f => f.WeekNo == sec.Key && f.Period == "current" && f.Comodity.ToLower() == "grocery").Sum(t => t.CasesCurrent),
                                Period1 = filter.Periods.Current.End,
                                Color1 = ChartColorBM.GrocerryCurrent,
                                SalesPerson1 = "",

                                Label2 = filter.Periods.Current.Label,
                                rValue2 = (double)(sec.Where(f => f.WeekNo == sec.Key && f.Period == "current" && f.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)),
                                cValue2 = sec.Where(f => f.WeekNo == sec.Key && f.Period == "current" && f.Comodity.ToLower() == "produce").Sum(t => t.CasesCurrent),
                                Period2 = filter.Periods.Current.End,
                                Color2 = ChartColorBM.ProduceCurrent,
                                SalesPerson2 = "",

                                Label3 = filter.Periods.Historical.Label,
                                rValue3 = (double)(sec.Where(f => f.WeekNo == sec.Key && f.Period == "historical" && f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)),
                                cValue3 = sec.Where(f => f.WeekNo == sec.Key && f.Period == "historical" && f.Comodity.ToLower() == "grocery").Sum(t => t.CasesCurrent),
                                Period3 = filter.Periods.Historical.End,
                                Color3 = ChartColorBM.GrocerryPrevious,
                                SalesPerson3 = "",

                                Label4 = filter.Periods.Historical.Label,
                                rValue4 = (double)(sec.Where(f => f.WeekNo == sec.Key && f.Period == "historical" && f.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)),
                                cValue4 = sec.Where(f => f.WeekNo == sec.Key && f.Period == "historical" && f.Comodity.ToLower() == "produce").Sum(t => t.CasesCurrent),
                                Period4 = filter.Periods.Historical.End,
                                Color4 = ChartColorBM.ProducePrevious,
                                SalesPerson4 = "",

                                Label5 = filter.Periods.Prior.Label,
                                rValue5 = (double)(sec.Where(f => f.WeekNo == sec.Key && f.Period == "prior" && f.Comodity.ToLower() == "grocery").Sum(t => t.RevenueCurrent)),
                                cValue5 = sec.Where(f => f.WeekNo == sec.Key && f.Period == "prior" && f.Comodity.ToLower() == "grocery").Sum(t => t.CasesCurrent),
                                Period5 = filter.Periods.Prior.End,
                                Color5 = ChartColorBM.GrocerryCurrent,
                                SalesPerson5 = "",

                                Label6 = filter.Periods.Prior.Label,
                                rValue6 = (double)(sec.Where(f => f.WeekNo == sec.Key && f.Period == "prior" && f.Comodity.ToLower() == "produce").Sum(t => t.RevenueCurrent)),
                                cValue6 = sec.Where(f => f.WeekNo == sec.Key && f.Period == "prior" && f.Comodity.ToLower() == "produce").Sum(t => t.CasesCurrent),
                                Period6 = filter.Periods.Prior.End,
                                Color6 = ChartColorBM.ProduceCurrent,
                                SalesPerson6 = "",
                            }).ToList()

                 }).ToList();



            return result;
        }

        public GenericSubDataTopBottomChartBM MappingSalesToSalesPersonDashBoard(List<MapCasesSoldRevenueGlobal> list
                         , GlobalFilter filter)
        {
            GenericSubDataTopBottomChartBM result = new GenericSubDataTopBottomChartBM();
            result.Top = list.GroupBy(t => t.SalesPersonCode).OrderByDescending(f => f.Sum(e => e.RevenueCurrent)).Take(10)
                .Select(pri => new GenericColumnChartBM
                {
                    GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(pri.First().ActiveEmployee) ? pri.First().ActiveEmployee : pri.Key),
                    ActiveSalesPersonCode = pri.Key,
                    Label1 = filter.Periods.Current.Label,
                    rValue1 = (double)(pri.Sum(t => t.RevenueCurrent)),
                    Period1 = filter.Periods.Current.End,
                    SalesPerson1 = "",


                    Label2 = filter.Periods.Historical.Label,
                    rValue2 = (double)(pri.Sum(t => t.RevenueHistorical)),
                    Period2 = filter.Periods.Historical.End,
                    SalesPerson2 = "",


                    Label3 = filter.Periods.Prior.Label,
                    rValue3 = (double)(pri.Sum(t => t.RevenuePrior)),
                    Period3 = filter.Periods.Prior.End,
                    SalesPerson3 = "",



                }).ToList();

            for (int i = 0; i < result.Top.Count; i++)
            {
                result.Top[i].Color1 = SalesPersonColors.Colors[i].Primary;
                result.Top[i].Color2 = SalesPersonColors.Colors[i].Secondary;
                // now the chart has been changed from overlapping column chart to clustered column. So the Primary and Teritary columns needs to be the same color.
                result.Top[i].Color3 = SalesPersonColors.Colors[i].Primary;
            }


            result.Bottom = list.GroupBy(t => t.SalesPersonCode).OrderBy(f => f.Sum(e => e.RevenueCurrent)).Take(10)
               .Select(pri => new GenericColumnChartBM
               {
                   GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(pri.First().ActiveEmployee) ? pri.First().ActiveEmployee : pri.Key),
                   ActiveSalesPersonCode = pri.Key,
                   Label1 = filter.Periods.Current.Label,
                   rValue1 = (double)(pri.Sum(t => t.RevenueCurrent)),
                   Period1 = filter.Periods.Current.End,
                   SalesPerson1 = "",


                   Label2 = filter.Periods.Historical.Label,
                   rValue2 = (double)(pri.Sum(t => t.RevenueHistorical)),
                   Period2 = filter.Periods.Historical.End,
                   SalesPerson2 = "",


                   Label3 = filter.Periods.Prior.Label,
                   rValue3 = (double)(pri.Sum(t => t.RevenuePrior)),
                   Period3 = filter.Periods.Prior.End,
                   SalesPerson3 = "",



               }).ToList();

            for (int i = 0; i < result.Bottom.Count; i++)
            {
                result.Bottom[i].Color1 = SalesPersonColors.Colors[i].Primary;
                result.Bottom[i].Color2 = SalesPersonColors.Colors[i].Secondary;
                // now the chart has been changed from overlapping column chart to clustered column. So the Primary and Teritary columns needs to be the same color.
                result.Bottom[i].Color3 = SalesPersonColors.Colors[i].Primary;
            }

            return result;
        }

        public GenericSubDataTopBottomChartBM MappingCasesSoldToSalesPersonDashBoard(List<MapCasesSoldRevenueGlobal> list
                        , GlobalFilter filter)
        {
            GenericSubDataTopBottomChartBM result = new GenericSubDataTopBottomChartBM();
            result.Top = list.GroupBy(t => t.SalesPersonCode).OrderByDescending(f => f.Sum(e => e.RevenueCurrent)).Take(10)
                .Select(pri => new GenericColumnChartBM
                {
                    GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(pri.First().ActiveEmployee) ? pri.First().ActiveEmployee : pri.Key),
                    ActiveSalesPersonCode = pri.Key,
                    Label1 = filter.Periods.Current.Label,
                    rValue1 = (double)(pri.Sum(t => t.CasesCurrent)),
                    Period1 = filter.Periods.Current.End,
                    SalesPerson1 = "",


                    Label2 = filter.Periods.Historical.Label,
                    rValue2 = (double)(pri.Sum(t => t.CasesHistorical)),
                    Period2 = filter.Periods.Historical.End,
                    SalesPerson2 = "",


                    Label3 = filter.Periods.Prior.Label,
                    rValue3 = (double)(pri.Sum(t => t.CasesPrior)),
                    Period3 = filter.Periods.Prior.End,
                    SalesPerson3 = "",



                }).ToList();

            for (int i = 0; i < result.Top.Count; i++)
            {
                result.Top[i].Color1 = SalesPersonColors.Colors[i].Primary;
                result.Top[i].Color2 = SalesPersonColors.Colors[i].Secondary;
                // now the chart has been changed from overlapping column chart to clustered column. So the Primary and Teritary columns needs to be the same color.
                result.Top[i].Color3 = SalesPersonColors.Colors[i].Primary;
            }


            result.Bottom = list.GroupBy(t => t.SalesPersonCode).OrderBy(f => f.Sum(e => e.RevenueCurrent)).Take(10)
               .Select(pri => new GenericColumnChartBM
               {
                   GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(pri.First().ActiveEmployee) ? pri.First().ActiveEmployee : pri.Key),
                   ActiveSalesPersonCode = pri.Key,
                   Label1 = filter.Periods.Current.Label,
                   rValue1 = (double)(pri.Sum(t => t.CasesCurrent)),
                   Period1 = filter.Periods.Current.End,
                   SalesPerson1 = "",


                   Label2 = filter.Periods.Historical.Label,
                   rValue2 = (double)(pri.Sum(t => t.CasesHistorical)),
                   Period2 = filter.Periods.Historical.End,
                   SalesPerson2 = "",


                   Label3 = filter.Periods.Prior.Label,
                   rValue3 = (double)(pri.Sum(t => t.CasesPrior)),
                   Period3 = filter.Periods.Prior.End,
                   SalesPerson3 = "",



               }).ToList();

            for (int i = 0; i < result.Bottom.Count; i++)
            {
                result.Bottom[i].Color1 = SalesPersonColors.Colors[i].Primary;
                result.Bottom[i].Color2 = SalesPersonColors.Colors[i].Secondary;
                // now the chart has been changed from overlapping column chart to clustered column. So the Primary and Teritary columns needs to be the same color.
                result.Bottom[i].Color3 = SalesPersonColors.Colors[i].Primary;
            }

            return result;
        }

        public GenericSubDataTopBottomChartBM MappingCustomersToSalesPersonDashBoard(List<MapCasesSoldRevenueGlobal> list, List<MapCasesSoldRevenueGlobal> customers
                         , GlobalFilter filters)
        {
            GenericSubDataTopBottomChartBM result = new GenericSubDataTopBottomChartBM();
            result.Top = list.GroupBy(t => t.SalesPersonCode)
                .Select(pri => new GenericColumnChartBM
                {
                    GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(pri.First().ActiveEmployee) ? pri.First().ActiveEmployee : pri.Key),
                    ActiveSalesPersonCode = pri.Key,
                    Label1 = filters.Periods.Current.Label,
                    cValue1 = customers.Where(d => d.SalesPersonCode == pri.Key && d.Period == "current").Select(d => d.Customers).Distinct().Count(),
                    Period1 = filters.Periods.Current.End,
                    SalesPerson1 = "",


                    Label2 = filters.Periods.Historical.Label,
                    cValue2 = customers.Where(d => d.SalesPersonCode == pri.Key && d.Period == "historical").Select(d => d.Customers).Distinct().Count(),
                    Period2 = filters.Periods.Historical.End,
                    SalesPerson2 = "",


                    Label3 = filters.Periods.Prior.Label,
                    cValue3 = customers.Where(d => d.SalesPersonCode == pri.Key && d.Period == "prior").Select(d => d.Customers).Distinct().Count(),
                    Period3 = filters.Periods.Prior.End,
                    SalesPerson3 = "",



                }).Take(10).OrderByDescending(d => d.cValue1).ToList();

            for (int i = 0; i < result.Top.Count; i++)
            {
                result.Top[i].Color1 = SalesPersonColors.Colors[i].Primary;
                result.Top[i].Color2 = SalesPersonColors.Colors[i].Secondary;
                // now the chart has been changed from overlapping column chart to clustered column. So the Primary and Teritary columns needs to be the same color.
                result.Top[i].Color3 = SalesPersonColors.Colors[i].Primary;
            }


            result.Bottom = list.GroupBy(t => t.SalesPersonCode)
               .Select(pri => new GenericColumnChartBM
               {
                   GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(pri.First().ActiveEmployee) ? pri.First().ActiveEmployee : pri.Key),
                   ActiveSalesPersonCode = pri.Key,
                   Label1 = filters.Periods.Current.Label,
                   cValue1 = customers.Where(d => d.SalesPersonCode == pri.Key && d.Period == "current").Select(d => d.Customers).Distinct().Count(),
                   Period1 = filters.Periods.Current.End,
                   SalesPerson1 = "",


                   Label2 = filters.Periods.Historical.Label,
                   cValue2 = customers.Where(d => d.SalesPersonCode == pri.Key && d.Period == "historical").Select(d => d.Customers).Distinct().Count(),
                   Period2 = filters.Periods.Historical.End,
                   SalesPerson2 = "",


                   Label3 = filters.Periods.Prior.Label,
                   cValue3 = customers.Where(d => d.SalesPersonCode == pri.Key && d.Period == "prior").Select(d => d.Customers).Distinct().Count(),
                   Period3 = filters.Periods.Prior.End,
                   SalesPerson3 = "",



               }).Take(10).OrderBy(f => f.cValue1).ToList();

            for (int i = 0; i < result.Bottom.Count; i++)
            {
                result.Bottom[i].Color1 = SalesPersonColors.Colors[i].Primary;
                result.Bottom[i].Color2 = SalesPersonColors.Colors[i].Secondary;
                // now the chart has been changed from overlapping column chart to clustered column. So the Primary and Teritary columns needs to be the same color.
                result.Bottom[i].Color3 = SalesPersonColors.Colors[i].Primary;
            }

            return result;
        }

        /// <summary>
        /// Get data for the sold case by location
        /// </summary>
        /// <param name="date"> Date of shipment </param>
        /// <returns> Short report </returns>
        public GenericColumnChartLocationBM GetCasesSoldByLocation(GlobalFilter filter)
        {
            GenericColumnChartLocationBM response = new GenericColumnChartLocationBM();
            List<GenericColumnChartBM> total = new List<GenericColumnChartBM>();

            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@historicalStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filter.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@priorStart", filter.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filter.Periods.Prior.End));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetCasesSoldReportByLocation", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new MapCasesSoldRevenue
                            {
                                Location = !string.IsNullOrEmpty(x.Field<string>("Location")) ? x.Field<string>("Location").Trim() : "",
                                Comodity = !string.IsNullOrEmpty(x.Field<string>("Comodity")) ? x.Field<string>("Comodity").Trim() : "",
                                Category = !string.IsNullOrEmpty(x.Field<string>("category")) ? x.Field<string>("category").Trim() : "",
                                CasesCurrentMonth = x.Field<int?>("CasesCurrentMonth") ?? 0,
                                CasesPreviousMonth = x.Field<int?>("CasesPreviousMonth") ?? 0,
                                CasesPreviousYear = x.Field<int?>("CasesPreviousYear") ?? 0,
                            })
                            .ToList();

                response.Local = result.GetCasesFromLocation(x => x.Location.ToLower() == "local", filter, "Local");

                response.OOT = result.GetCasesFromLocation(x => x.Location.ToLower() == "out of t", filter, "Out of Town");
                return response;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }

        }

        public BarChartTypes GetCasesSoldAndGrowthBySalesPerson(GlobalFilter filter, string userId)
        {

            BarChartTypes response = new BarChartTypes();

            AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
            var userAccessibleCategories = _adminUserManagementDateProvider.GetUserDetails(userId);

            try
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@priorStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filter.Periods.Historical.End));

                var dataSetResult = base.ReadToDataSetViaProcedure("BI_CS_GetCasesSoldAndGrowthBySalesPerson", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new BarChartDetails
                            {
                                Code = !string.IsNullOrEmpty(x.Field<string>("salesman")) ? x.Field<string>("salesman").Trim() : "",
                                growth = x.Field<decimal?>("growth").Value,
                                prior = x.Field<decimal?>("prior").Value,
                                value = x.Field<decimal?>("current").Value,
                                Category = !string.IsNullOrEmpty(x.Field<string>("Category")) ? x.Field<string>("Category").Trim() : "",
                                GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(x.Field<string>("salesmanname")) ? x.Field<string>("salesmanname").Trim() : ""),
                            }).Where(x => !userAccessibleCategories.IsRestrictedCategoryAccess
                         || userAccessibleCategories.Categories.Select(o => o.Name).Contains(x.Category))
                            .ToList();


                int i = 0;
                foreach (var item in result)
                {
                    if (i == 25)
                    {
                        i = 0;
                    }
                    item.Color1 = TopBottom25GraphColors.Colors[i].Primary;
                    item.Label1 = filter.Periods.Current.Label;

                    item.LabelPrior = filter.Periods.Historical.Label;

                    i++;
                }

                response.SalesPerson = new BarchartOrderBy();
                response.SalesPerson.Top = result.OrderByDescending(x => x.value).Take(25).ToList();
                response.SalesPerson.Bottom = result.OrderBy(x => x.value).Take(25).ToList();



                var chartForGrowthList = new List<BarChartDetails>();

                foreach (var item in result)
                {
                    BarChartDetails newSalesPerson = new BarChartDetails();
                    newSalesPerson.value = item.growth;
                    newSalesPerson.GroupName = item.GroupName;
                    newSalesPerson.Code = item.Code;
                    newSalesPerson.Color1 = item.Color1;
                    newSalesPerson.salesman = item.salesman;
                    newSalesPerson.Label1 = item.Label1;
                    newSalesPerson.LabelPrior = item.LabelPrior;
                    newSalesPerson.growth = item.value;
                    newSalesPerson.prior = item.prior;
                    chartForGrowthList.Add(newSalesPerson);
                }
                response.Growth = new BarchartOrderBy();
                response.Growth.Top = chartForGrowthList.OrderByDescending(x => x.value).Take(25).ToList();
                response.Growth.Bottom = chartForGrowthList.OrderBy(x => x.value).Take(25).ToList();

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
            return response;
        }

        public BarChartTypes GetCasesSoldAndGrowthByCustomer(GlobalFilter filter, string userId)
        {
            AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
            var userAccessibleCategories = _adminUserManagementDateProvider.GetUserAccess(userId);


            BarChartTypes response = new BarChartTypes();
            try
            {
                DataTable dtCategories = new DataTable();
                dtCategories.Columns.Add("Code");
                foreach (var category in userAccessibleCategories.Categories)
                {
                    if (category.IsAccess == true) { dtCategories.Rows.Add(category.Name); }
                }

                List<SqlParameter> parameterList = new List<SqlParameter>();

                var param = new SqlParameter();
                param.SqlDbType = SqlDbType.Structured;
                param.Value = dtCategories;
                param.ParameterName = "@categories";
                parameterList.Add(param);

                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@priorStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filter.Periods.Historical.End));

                var dataSetResult = base.ReadToDataSetViaProcedure("BI_CS_GetCasesSoldAndGrowthByCustomer", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new BarChartDetails
                            {
                                Code = !string.IsNullOrEmpty(x.Field<string>("CompanyNumber")) ? x.Field<string>("CompanyNumber").Trim() : "",
                                growth = x.Field<decimal?>("growth").Value,
                                prior = x.Field<decimal?>("prior").Value,
                                value = x.Field<decimal?>("current").Value,
                                GroupName = !string.IsNullOrEmpty(x.Field<string>("CompanyName")) ? x.Field<string>("CompanyName").Trim() : "",
                                Category = !string.IsNullOrEmpty(x.Field<string>("Category")) ? x.Field<string>("Category").Trim() : "",
                                GroupName1 = (!string.IsNullOrEmpty(x.Field<string>("CompanyName")) ? x.Field<string>("CompanyName").Trim() : ""),

                            })
                            //.Where(x => !userAccessibleCategories.IsRestrictedCategoryAccess || userAccessibleCategories.Categories.Select(o => o.Name).Contains(x.Category))
                            .ToList();


                int i = 0;
                foreach (var item in result)
                {
                    if (i == 25) { i = 0; }
                    item.Color1 = TopBottom25GraphColors.Colors[i].Primary;
                    item.Label1 = filter.Periods.Current.Label;
                    item.LabelPrior = filter.Periods.Historical.Label;
                    i++;
                }

                response.SalesPerson = new BarchartOrderBy();
                response.SalesPerson.Top = result.Where(x => x.Category == "topCustomers").ToList();
                response.SalesPerson.Bottom = result.Where(x => x.Category == "bottomCustomers").ToList();

                var chartForGrowthList = new List<BarChartDetails>();

                foreach (var item in result)
                {
                    BarChartDetails newSalesPerson = new BarChartDetails();
                    newSalesPerson.value = item.growth;
                    newSalesPerson.GroupName = item.GroupName;
                    newSalesPerson.Code = item.Code;
                    newSalesPerson.Color1 = item.Color1;
                    newSalesPerson.salesman = item.salesman;
                    newSalesPerson.Label1 = item.Label1;
                    newSalesPerson.growth = item.value;
                    newSalesPerson.Category = item.Category;
                    newSalesPerson.GroupName1 = item.GroupName1;
                    newSalesPerson.LabelPrior = item.LabelPrior;
                    newSalesPerson.prior = item.prior;
                    chartForGrowthList.Add(newSalesPerson);
                }
                response.Growth = new BarchartOrderBy();
                response.Growth.Top = chartForGrowthList.Where(x => x.Category == "topCustomerGrowth").ToList();
                response.Growth.Bottom = chartForGrowthList.Where(x => x.Category == "bottomCustomerGrowth").ToList();

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
            return response;
        }

        public BarChartTypes GetCasesSoldAndGrowthBySalesPersonCustomerService(GlobalFilter filter, string userId,bool isCasesSold)
        {

            BarChartTypes response = new BarChartTypes();

            AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
            var userAccessibleCategories = _adminUserManagementDateProvider.GetUserDetails(userId);

            try
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@priorStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filter.Periods.Historical.End));

                DataSet dataSetResult;

                if (isCasesSold)
                {
                    dataSetResult = base.ReadToDataSetViaProcedure("BI_CS_GetCasesSoldAndGrowthByCustomerService", parameterList.ToArray());
                }
                else
                {
                    dataSetResult = base.ReadToDataSetViaProcedure("BI_CS_GetSalesAndGrowthByCustomerService", parameterList.ToArray());
                }

                var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new BarChartDetails
                            {
                                Code = !string.IsNullOrEmpty(x.Field<string>("salesman")) ? x.Field<string>("salesman").Trim() : "",
                                growth = x.Field<decimal?>("growth").Value,
                                prior = x.Field<decimal?>("prior").Value,
                                value = x.Field<decimal?>("current").Value,
                                Category = !string.IsNullOrEmpty(x.Field<string>("Category")) ? x.Field<string>("Category").Trim() : "",
                                GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(x.Field<string>("adduser")) ? x.Field<string>("adduser").Trim() : ""),
                                AddUser= !string.IsNullOrEmpty(x.Field<string>("adduser")) ? x.Field<string>("adduser").Trim() : "",
                            }).Where(x => !userAccessibleCategories.IsRestrictedCategoryAccess
                         || userAccessibleCategories.Categories.Select(o => o.Name).Contains(x.Category))
                            .ToList();


                int i = 0;
                foreach (var item in result)
                {
                    if (i == 25)
                    {
                        i = 0;
                    }
                    item.Color1 = TopBottom25GraphColors.Colors[i].Primary;
                    item.Label1 = filter.Periods.Current.Label;

                    item.LabelPrior = filter.Periods.Historical.Label;

                    i++;
                }

                response.SalesPerson = new BarchartOrderBy();
                response.SalesPerson.Top = result.OrderByDescending(x => x.value).Take(25).ToList();
                response.SalesPerson.Bottom = result.OrderBy(x => x.value).Take(25).ToList();



                var chartForGrowthList = new List<BarChartDetails>();

                foreach (var item in result)
                {
                    BarChartDetails newSalesPerson = new BarChartDetails();
                    newSalesPerson.value = item.growth;
                    newSalesPerson.GroupName = item.AddUser;
                    newSalesPerson.Code = item.Code;
                    newSalesPerson.Color1 = item.Color1;
                    newSalesPerson.salesman = item.salesman;
                    newSalesPerson.Label1 = item.Label1;
                    newSalesPerson.LabelPrior = item.LabelPrior;
                    newSalesPerson.growth = item.value;
                    newSalesPerson.prior = item.prior;
                    chartForGrowthList.Add(newSalesPerson);
                }
                response.Growth = new BarchartOrderBy();
                response.Growth.Top = chartForGrowthList.OrderByDescending(x => x.value).Take(25).ToList();
                response.Growth.Bottom = chartForGrowthList.OrderBy(x => x.value).Take(25).ToList();

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
            return response;
        }
    }
}




