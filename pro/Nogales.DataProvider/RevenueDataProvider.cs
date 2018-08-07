using Nogales.BusinessModel;
using Nogales.DataProvider.Infrastructure;
using Nogales.DataProvider.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Nogales.BusinessModel;

namespace Nogales.DataProvider
{
    public class RevenueDataProvider : DataAccessADO
    {

        //03/03/2017
        //public RevenueCategroryBM GetTotalRevenue(string dateFilter, string category = "")
        //{
        //    var filterDate = DateTime.Parse(dateFilter);
        //    var currentMonthName = filterDate.ToShortMonthName();
        //    var previousMonthName = filterDate.AddMonths(-1).ToShortMonthName();

        //    var dateFormattedString = filterDate.Date.ToString("yyyy-MM-ddTHH:mm:ss");

        //    string sqlString = MDXCubeQueries.GetRevenueByCategoryMonthlyQuery(dateFormattedString, category);

        //    var result = GetTotalRevenueByCategory(currentMonthName, previousMonthName, sqlString);

        //    return result;

        //    //string sqlString = MDXCubeQueries.GetTotalRevenueByCategoryQuery(dateFormattedString);

        //    //var dataTableResult = base.GetDataTable(sqlString);


        //    //var listTotalRevenue = dataTableResult
        //    //                              .AsEnumerable()
        //    //                              .GroupBy(x => x.Field<string>(0))
        //    //                               .Select(y => new RevenueCategoryChartBM
        //    //                               {
        //    //                                   Category = y.Key,
        //    //                                   Column1 = currentMonthName,
        //    //                                   Column2 = previousMonthName,
        //    //                                   Val1 = y.Sum(t => t.Field<double?>("[Measures].[CurrentValue]") ?? 0).ToRoundTwoDigits(),
        //    //                                   Val2 = y.Sum(t => t.Field<double?>("[Measures].[PreviousValue]") ?? 0).ToRoundTwoDigits(),
        //    //                                   //Color1 =ChartColorBM.Colors[0],
        //    //                                   //Color2 = ChartColorBM.Colors[1],
        //    //                                   Color1 = y.Key == "Produce" ? ChartColorBM.Colors[2] : ChartColorBM.Colors[0],
        //    //                                   Color2 = y.Key == "Produce" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[1],
        //    //                               }).ToList();


        //    //return listTotalRevenue;
        //}

      
        /// <summary>
        /// revenue by monthly -Updated method
        /// </summary>
        /// <param name="filterDate"></param>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        /// <param name="currentMonthStart"></param>
        /// <param name="currentMonthEnd"></param>
        /// <param name="previousMonthStart"></param>
        /// <param name="previousMonthEnd"></param>
        /// <param name="previousYearStart"></param>
        /// <param name="previousYearEnd"></param>
        /// <param name="previousMonthYearStart"></param>
        /// <param name="previousMonthYearEnd"></param>
        /// <returns></returns>
        public RevenueClusteredBarChartCategoryBM GetRevenueCategoryMonthlyData(DateTime filterDate
                                                                          , string current, string previous
                                                                          , string currentMonthStart, string currentMonthEnd
                                                                          , string previousMonthStart, string previousMonthEnd
                                                                          , string previousYearStart, string previousYearEnd
                                                                          , string previousMonthYearStart, string previousMonthYearEnd)
        {
            string sqlQueryString = SQLQueries.GetRevenueMonthlyQuery(current, previous
                                                                         , currentMonthStart, currentMonthEnd
                                                                          , previousMonthStart, previousMonthEnd
                                                                          , previousYearStart, previousYearEnd
                                                                          , previousMonthYearStart, previousMonthYearEnd);

            var result = GetRevenueByCategory(filterDate, Constants.ByMonth, sqlQueryString, current, previous);
            return result;
          
        }

        public RevenueClusteredBarChartCategoryBM GetRevenueByCategoryYear(DateTime filterDate
                                                                           , string current, string previous
                                                                           , string currentStart, string currentEnd
                                                                           , string previousYearStart, string previousYearEnd)
        {
            string sqlQueryString = SQLQueries.GetRevenueYearlyQuery(current
                                                                    , previous
                                                                    , currentStart
                                                                    , currentEnd
                                                                    , previousYearStart
                                                                    , previousYearEnd);

            var result = GetRevenueByCategory(filterDate, Constants.ByYear, sqlQueryString, current, previous);
            return result;
        }


        /// <summary>
        /// Get Revenue By Category By Year To Year 
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public RevenueClusteredBarChartCategoryBM GetRevenueByCategoryByYearToDateData(string dateString)
        {
            var filterDate = DateTime.Parse(dateString);
            string filterType = Constants.RevenueByYear;
            var currentName = filterDate.Year.ToString();
            var previousName = filterDate.AddYears(-1).Year.ToString();
            var dateFormattedString = filterDate.Date.ToString("yyyy-MM-ddTHH:mm:ss");

            string sqlString = MDXCubeQueries.GetRevenueByCategoryYearToDateQuery(dateFormattedString);            

            //var result = GetRevenueByCategory(currentName, previousName, sqlString, filterType,null);

            //return result;
            return null;
        }

        /// <summary>
        /// Get Revenue By Category Year to custom
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public RevenueClusteredBarChartCategoryBM GetRevenueByCategoryByYearToCustomData(int year, int month)
        {
            var currentName = year.ToString();
            var previousName = (year - 1).ToString();
            string filterType = Constants.RevenueByYearToMonth;

            string sqlString = MDXCubeQueries.GetRevenueByCategoryYearToCustomQuery(year, month);            

            //var result = GetRevenueByCategory(currentName, previousName, sqlString,filterType,null);

            //return result;
            return null;
        }

       

        public List<RevenueCategoryChartBM> GetRevenueBySalesPersonMonth(DateTime filterDate
                                                                            , string current, string previous
                                                                            , string currentMonthStart, string currentMonthEnd
                                                                            , string previousMonthStart, string previousMonthEnd)
        {
            string sqlQuery = SQLQueries.GetMonthlyRevenueBySalesPersonQuery(currentMonthStart, currentMonthEnd
                                                                                , previousMonthStart, previousMonthEnd
                                                                                , current, previous);

            var result = GetRevenueBySalesPerson(filterDate, Constants.ByMonth, sqlQuery, current, previous);
            return result.ToList();
        }

      

        public List<RevenueCategoryChartBM> GetRevenueBySalesPersonYear(DateTime filterDate,
                                                                                    string current,string previous,
                                                                                    string currentYearStart,string currentYearEnd,
                                                                                    string previousYearStart,string previousYearEnd)
        {           

            string sqlQuery = SQLQueries.GetYearlyRevenueBySalesPerson(currentYearStart, currentYearEnd
                                                                    , previousYearStart, previousYearEnd
                                                                    , current, previous);

            var result = GetRevenueBySalesPerson(filterDate, Constants.ByYear, sqlQuery, current, previous);
            return result.ToList();


        }
       

        public RevenueReportFilterBM GetReportFilterData()
        {
            var salesDataProvider = new SalesDataProvider();
           
            return new RevenueReportFilterBM
            {
                Buyer = this.GetAllBuyers(SQLQueries.GetAllBuyers()),
                //Item = this.GetAllItems(MDXCubeQueries.GetAllItems()),
                PurchaseOrder = this.GetPurchaseOrders(),
                //SalesPerson = this.GetSalesPersons(MDXCubeQueries.GetAllSalesPersons()),                
                SalesPerson = salesDataProvider.GetSalesPersons(),
                Vendor = this.GetAllVendors(SQLQueries.GetAllVendors()),
            };
        }

        private List<KeyValuePair<string, string>> GetPurchaseOrders()
        {
            var _tempData = new List<KeyValuePair<string, string>>();
            return _tempData;
            //Enumerable.Range(311, 700).Select(x => new KeyValuePair<string, string>(x.ToString(), x.ToString()))
            //    .ToList();
        }

        //03/03/2017
        //private RevenueCategroryBM GetTotalRevenueByCategory(string currentName, string previousName, string sqlString)
        //{
        //    var dataTableResult = base.GetDataTable(sqlString);
        //    var listColumns = dataTableResult.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();

        //    #region Map to Datatable
        //    var listRevenueData = dataTableResult
        //                                     .AsEnumerable()
        //                                     .Select(x => new RevenueMapper
        //                                     {
        //                                         Category = x.Field<string>(0),
        //                                         Comodity = x.Field<string>(1),
        //                                         // DateSold = DateTime.Parse(x.Field<string>(4).ToString()),
        //                                         SalesPerson = listColumns.Contains("[Salesperson].[Salesperson].[Salesperson Name].[MEMBER_CAPTION]")
        //                                                         ? x.Field<string>("[Salesperson].[Salesperson].[Salesperson Name].[MEMBER_CAPTION]")
        //                                                         : string.Empty,
        //                                         CurrentValue = (x.Field<double?>("[Measures].[CurrentValue]") ?? 0).ToRoundTwoDigits(),
        //                                         PreviousValue = (x.Field<double?>("[Measures].[PreviousValue]") ?? 0).ToRoundTwoDigits(),
        //                                     })
        //                                     .ToList();
        //    #endregion

        //    #region Mapping categories
        //    var revenueCategory = new RevenueCategroryBM
        //    {
        //        //Buyer = listRevenueData.GetRevenueFromCategory((x => x.Category == "BUYER"), currentName, previousName),
        //        //FoodService = listRevenueData.GetRevenueFromCategory((x => x.Category == "FOOD SERVI"), currentName, previousName),
        //        //Carniceria = listRevenueData.GetRevenueFromCategory((x => x.Category == "CARNICERIA"), currentName, previousName),
        //        //LossProd = listRevenueData.GetRevenueFromCategory((x => x.Category == "LOSS PROD"), currentName, previousName),
        //        //National = listRevenueData.GetRevenueFromCategory((x => x.Category == "NATIONAL"), currentName, previousName),
        //        //Wholesaler = listRevenueData.GetRevenueFromCategory((x => x.Category == "WHOLESALER"), currentName, previousName),
        //        //WillCall = listRevenueData.GetRevenueFromCategory((x => x.Category == "WILL CALL"), currentName, previousName),
        //        //Retail = listRevenueData.GetRevenueFromCategory((x => x.Category.StartsWith("RETAIL")), currentName, previousName),
        //        //AllOthers = listRevenueData.GetRevenueFromCategory(((x => !x.Category.StartsWith("RETAIL") &&
        //        //                                                            x.Category != "BUYER" &&
        //        //                                                            x.Category != "FOOD SERVI" &&
        //        //                                                            x.Category != "CARNICERIA" &&
        //        //                                                            x.Category != "LOSS PROD" &&
        //        //                                                            x.Category != "NATIONAL" &&
        //        //                                                            x.Category != "WHOLESALER" &&
        //        //                                                            x.Category != "WILL CALL")), currentName, previousName),

        //        Total = listRevenueData
        //                 .GroupBy(x => x.Comodity)
        //                 .Select(y => new RevenueCategoryChartBM
        //                 {
        //                     Category = y.Key,
        //                     Column1 = currentName,
        //                     Column2 = previousName,
        //                     Val1 = (y.Sum(t => t.CurrentValue) ?? 0).ToRoundTwoDigits(),
        //                     Val2 = (y.Sum(t => t.PreviousValue) ?? 0).ToRoundTwoDigits(),
        //                     //Color1 = ChartColorBM.Colors[0],
        //                     //Color2 = ChartColorBM.Colors[1],
        //                     Color1 = y.Key == "Produce" ? ChartColorBM.Colors[2] : ChartColorBM.Colors[0],
        //                     Color2 = y.Key == "Produce" ? ChartColorBM.Colors[3] : ChartColorBM.Colors[1],
        //                     //SubData = new RenvenueSalesPesonBM
        //                     //{
        //                     //    Top = y.OrderByDescending(s => s.CurrentValue)
        //                     //         .Take(5)
        //                     //         .Select((s, idx) => new RevenueCategoryChartBM
        //                     //         {
        //                     //             Category = s.SalesPerson,
        //                     //             Column1 = currentName,
        //                     //             Column2 = previousName,
        //                     //             Val1 = s.CurrentValue,
        //                     //             Val2 = s.PreviousValue,
        //                     //             Color1 = ChartColorBM.Colors[4],
        //                     //             Color2 = ChartColorBM.Colors[5],
        //                     //             //Color1 = ChartColorBM.Colors[idx],
        //                     //             //Color2 = ChartColorBM.Colors.Reverse().ElementAt(idx),
        //                     //         })
        //                     //         .ToList(),
        //                     //    Bottom = y.OrderBy(s => s.CurrentValue)
        //                     //                .Take(5)
        //                     //                .Select((s, idx) => new RevenueCategoryChartBM
        //                     //                {
        //                     //                    Category = s.SalesPerson,
        //                     //                    Column1 = currentName,
        //                     //                    Column2 = previousName,
        //                     //                    Val1 = s.CurrentValue,
        //                     //                    Val2 = s.PreviousValue,
        //                     //                    Color1 = ChartColorBM.Colors[4],
        //                     //                    Color2 = ChartColorBM.Colors[5],
        //                     //                    //Color1 = ChartColorBM.Colors[idx],
        //                     //                    //Color2 = ChartColorBM.Colors.Reverse().ElementAt(idx),
        //                     //                })
        //                     //                .ToList(),
        //                     //}
        //                 }).ToList(),
        //    };

        //    #endregion

        //    return revenueCategory;
        //}

        /// <summary>
        /// Get Revenue By Category - Updated
        /// </summary>
        /// <param name="filterDate"></param>
        /// <param name="filterType"></param>
        /// <param name="sqlQuery"></param>
        /// <param name="currentLabel"></param>
        /// <param name="previousLabel"></param>
        /// <returns></returns>
        public RevenueClusteredBarChartCategoryBM GetRevenueByCategory(DateTime filterDate, string filterType, string sqlQuery, string currentLabel, string previousLabel)
        {
            try
            {
                var dataTableResult = new DataSet();
                using (var adoDataAccess = new DataAccessADO())
                {
                    dataTableResult = adoDataAccess.ReadToDataSet(sqlQuery);
                }
                var listofRevenue = dataTableResult.Tables[0]
                                                 .AsEnumerable()
                                                 .Select(x => new RevenueMapper
                                                 {
                                                     Category = x.Field<string>("Category").TrimEnd(),
                                                     Comodity = x.Field<string>("Descrip").TrimEnd(),
                                                     SalesPerson = x.Field<string>("SalesmanCode").TrimEnd(),
                                                     SalesPersonCode = x.Field<string>("SalesmanCode").TrimEnd(),
                                                     CurrentValue = (x.Field<decimal?>(currentLabel) ?? 0).ToRoundTwoDecimalDigits(),
                                                     PreviousValue = (x.Field<decimal?>(previousLabel) ?? 0).ToRoundTwoDecimalDigits(),
                                                     Year = filterType == Constants.ByMonth ? (x.Field<int>("InvoiceYear")).ToString() : filterDate.Year.ToString(),
                                                 })
                                                 .ToList();

                var totalGrocery = listofRevenue.GetRevenueForTotal(x => x.Comodity == "Grocery", filterType, filterDate).ToList();
                var totalProduce = listofRevenue.GetRevenueForTotal(x => x.Comodity == "Produce", filterType, filterDate).ToList();
                var allTotals = new List<ClusteredStackChartBM>();
                allTotals = GetClusteredStackChartData(totalGrocery, totalProduce, Constants.ByMonth);

                var revenueCategories = new RevenueClusteredBarChartCategoryBM
                {
                    Total = new RevenueComodityBM
                    {
                        Grocery = totalGrocery,
                        Produce = totalProduce,
                        All = allTotals
                    },
                    Buyer = GetRevenueComodityBM(listofRevenue, "BUYER", filterType, filterDate),
                    FoodService = GetRevenueComodityBM(listofRevenue, "FOOD SERVI", filterType, filterDate),
                    Carniceria = GetRevenueComodityBM(listofRevenue, "CARNICERIA", filterType, filterDate),
                    LossProd = GetRevenueComodityBM(listofRevenue, "LOSS PROD", filterType, filterDate),
                    National = GetRevenueComodityBM(listofRevenue, "NATIONAL", filterType, filterDate),
                    Wholesaler = GetRevenueComodityBM(listofRevenue, "WHOLESALER", filterType, filterDate),
                    WillCall = GetRevenueComodityBM(listofRevenue, "WILL CALL", filterType, filterDate),
                    Retail = new RevenueComodityBM
                    {
                        Grocery = listofRevenue.GetRevenueFromComodityAndCategory(x => x.Comodity == "Grocery" && x.Category.StartsWith("RETAIL"), filterType, filterDate),
                        Produce = listofRevenue.GetRevenueFromComodityAndCategory(x => x.Comodity == "Produce" && x.Category.StartsWith("RETAIL"), filterType, filterDate),
                    },
                    AllOthers = new RevenueComodityBM
                    {
                        Grocery = listofRevenue.GetRevenueFromComodityAndCategory(x => x.Comodity == "Grocery" &&
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
                        Produce = listofRevenue.GetRevenueFromComodityAndCategory(x => x.Comodity == "Produce" && !x.Category.StartsWith("RETAIL") &&
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

                return revenueCategories;
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
        private List<ClusteredStackChartBM> GetClusteredStackChartData(List<RevenueClusteredBarChartBM> totalGrocery, List<RevenueClusteredBarChartBM> totalProduce, string filterType)
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

        private RevenueComodityBM GetRevenueComodityBM(List<RevenueMapper> TotalListRevenueData, string category, string filterType, DateTime filterDate)
        {
            return new RevenueComodityBM
            {
                Grocery = TotalListRevenueData.GetRevenueFromComodityAndCategory(x => x.Comodity == "Grocery" && x.Category == category, filterType, filterDate),
                Produce = TotalListRevenueData.GetRevenueFromComodityAndCategory(x => x.Comodity == "Produce" && x.Category == category, filterType, filterDate),
            };
        }       

        private List<RevenueCategoryChartBM> GetRevenueBySalesPerson(DateTime filterDate, string filterType, string sqlQuery, string currentLabel, string previousLabel)
        {
            try
            {
                var dataTableResult = new DataSet();
                using (var adoDataAccess = new DataAccessADO())
                {
                    dataTableResult = adoDataAccess.ReadToDataSet(sqlQuery);
                }
                

                #region Map to Datatable
                var listRevenueData = dataTableResult.Tables[0]
                                                 .AsEnumerable()
                                                 .Select(x => new RevenueMapper
                                                 {
                                                     SalesPerson = x.Field<string>("SalesPersonCode").TrimEnd(),
                                                     Customer = x.Field<string>("Customer").TrimEnd(),
                                                     CurrentValue = (x.Field<decimal?>(currentLabel) ?? 0).ToRoundTwoDecimalDigits(),
                                                     PreviousValue = (x.Field<decimal?>(previousLabel) ?? 0).ToRoundTwoDecimalDigits(),
                                                 })
                                                 .ToList();
                #endregion

                              


                #region Mapping Chart data
                var listRevenueChartData = listRevenueData
                                                   .Where(x => !string.IsNullOrEmpty(x.SalesPerson))
                                                   .GroupBy(x => x.SalesPerson)
                                                   .Select((y, index) => new RevenueCategoryChartBM
                                                   {
                                                       Category = y.Key,
                                                       Column1 = filterDate.ToColumnLabelName(filterType, filterDate.Year.ToString(), false),
                                                       Column2 = filterDate.ToColumnLabelName(filterType, filterDate.Year.ToString()),
                                                       Val1 = y.Sum(t => t.CurrentValue ?? 0),
                                                       Val2 = y.Sum(t => t.PreviousValue ?? 0),
                                                       Color1 = ChartColorBM.Colors[2],
                                                       Color2 = ChartColorBM.Colors[0],
                                                       //Color1 = ChartColorBM.Colors[index],
                                                       //Color2 = ChartColorBM.Colors.Reverse().ElementAt(index),
                                                       SubData = new RenvenueSalesPesonBM
                                                       {
                                                           Top = y.OrderByDescending(s => s.CurrentValue)
                                                                .Take(5)
                                                                .Select((s, idx) => new RevenueCategoryChartBM
                                                                {
                                                                    Category = s.Customer,
                                                                    Column1 = filterDate.ToColumnLabelName(filterType, filterDate.Year.ToString(), false),
                                                                    Column2 = filterDate.ToColumnLabelName(filterType, filterDate.Year.ToString()),
                                                                    Val1 = s.CurrentValue,
                                                                    Val2 = s.PreviousValue,
                                                                    Color1 = ChartColorBM.Colors[2],
                                                                    Color2 = ChartColorBM.Colors[3],
                                                                    //Color1 = ChartColorBM.Colors[idx],
                                                                    //Color2 = ChartColorBM.Colors.Reverse().ElementAt(idx),
                                                                })
                                                                .ToList(),
                                                           Bottom = y.OrderBy(s => s.CurrentValue)
                                                                        .Take(5)
                                                                        .Select((s, idx) => new RevenueCategoryChartBM
                                                                        {
                                                                            Category = s.Customer,
                                                                            Column1 = filterDate.ToColumnLabelName(filterType, filterDate.Year.ToString(), false),
                                                                            Column2 = filterDate.ToColumnLabelName(filterType, filterDate.Year.ToString()),
                                                                            Val1 = s.CurrentValue,
                                                                            Val2 = s.PreviousValue,
                                                                            Color1 = ChartColorBM.Colors[2],
                                                                            Color2 = ChartColorBM.Colors[3],
                                                                            //Color1 = ChartColorBM.Colors[idx],
                                                                            //Color2 = ChartColorBM.Colors.Reverse().ElementAt(idx),
                                                                        })
                                                                        .ToList(),
                                                       }
                                                   }).ToList();
                #endregion

                return listRevenueChartData.OrderByDescending(x => x.Val1).Take(10).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get sales persons
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        
        //03/03/2017
        //private List<KeyValuePair<string, string>> GetSalesPersons(string sqlString)
        //{
        //    var dataTableResult = base.GetDataTable(sqlString);

        //    #region Map to Datatable
        //    var salesPersons = dataTableResult
        //                                     .AsEnumerable()
        //                                     .Where(x => !string.IsNullOrEmpty(x.Field<string>(0)))
        //                                     .Select(x => new KeyValuePair<string, string>(x.Field<string>(0), x.Field<string>(0))
        //                                     {
        //                                     })
        //                                     .OrderBy(x => x.Key)
        //                                     .ToList();
        //    #endregion

        //    return salesPersons;
        //}

        /// <summary>
        /// Get all buyers
        /// </summary>
        /// <param name="sqlString">query string</param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> GetAllBuyers(string sqlString)
        {
            //var dataTableResult = base.GetDataTable(sqlString);
            var dataTableResult = new DataTable();
            using (var adoDataAccess = new DataAccessADO())
            {
                dataTableResult = adoDataAccess.ReadToDataSet(sqlString).Tables[0];
            }


            #region Map to Datatable
            var buyers = dataTableResult
                                             .AsEnumerable()
                                             .Where(x => !string.IsNullOrEmpty(x.Field<string>(0)))
                                             .Select(x => new KeyValuePair<string, string>(x.Field<string>(0), x.Field<string>(0))
                                             {
                                             })
                                             .OrderBy(x => x.Key)
                                             .ToList();
            #endregion

            return buyers;
        }

        /// <summary>
        /// Get all Items
        /// </summary>
        /// <param name="sqlString">query string</param>
        /// <returns></returns>
        
        //03/03/2017
        //private List<KeyValuePair<string, string>> GetAllItems(string sqlString)
        //{
        //    var dataTableResult = base.GetDataTable(sqlString);

        //    #region Map to Datatable
        //    //var items = dataTableResult
        //    //                                 .AsEnumerable()
        //    //                                 .Where(x => !string.IsNullOrEmpty(x.Field<string>(1)))
        //    //                                 // Performed grouped by as result from the sql give duplicate data
        //    //                                 .GroupBy(x => x.Field<string>(1))
        //    //                                 .Select(x => new KeyValuePair<string, string>(x.Key, x.Key)
        //    //                                 {
        //    //                                 })
        //    //                                 .OrderBy(x => x.Key)
        //    //                                 .ToList();

        //    var items = dataTableResult
        //                                     .AsEnumerable()
        //                                     .Where(x => !string.IsNullOrEmpty(x.Field<string>(0)))

        //                                     .Select(x => x.Field<string>(0)).Distinct()
        //                                     .Select(x => new KeyValuePair<string, string>(x, x))
        //                                     .OrderBy(x => x.Key)
        //                                     .ToList();
        //    #endregion

        //    return items;
        //}

        /// <summary>
        /// Get all vendors
        /// </summary>
        /// <param name="sqlString">query string</param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> GetAllVendors(string sqlString)
        {
            //var dataTableResult = base.GetDataTable(sqlString);
            var dataTableResult = new DataTable();
            using (var adoDataAccess = new DataAccessADO())
            {
                dataTableResult = adoDataAccess.ReadToDataSet(sqlString).Tables[0];
            }

            #region Map to Datatable
            var vendors = dataTableResult
                                             .AsEnumerable()
                                             .Where(x => !string.IsNullOrEmpty(x.Field<string>(0)))
                                             .Select(x => new KeyValuePair<string, string>(x.Field<string>(0), x.Field<string>(0))
                                             {
                                             })
                                             .OrderBy(x => x.Key)
                                             .ToList();
            #endregion

            return vendors;
        }

        public List<RevenueReportBM> GetRevenueReport(RevenueReportFilterBM searchModel)
        {
            if (searchModel.StartDate == null || searchModel.EndDate == null)
                return new List<RevenueReportBM>();

            //var salesPersonstring=searchModel.SalesPerson.Count>0?string.Concat(",",searchModel.SalesPerson):string.Empty;
            var salesPersons = searchModel.SalesPerson.Select(kvp => kvp.Value);
            string salesPersonstring = string.Join("','", salesPersons);

            //var buyerString = searchModel.Buyer.Count>0?string.Concat(",", searchModel.Buyer):string.Empty;
            var buyers = searchModel.Buyer.Select(kvp => kvp.Value);
            string buyersString = string.Join("','", buyers);

            //var vendorString = searchModel.Vendor.Count > 0 ? string.Concat(",", searchModel.Vendor.ToList().ForEach(x =>x.Value)).ToString() : string.Empty;
            var vendors = searchModel.Vendor.Select(kvp => kvp.Value);
            string vendorString = string.Join("','", vendors);

            //var itemString=searchModel.Item.Count>0? string.Concat(",",searchModel.Item):string.Empty;
            var items = searchModel.Item.Select(kvp => kvp.Value);
            string itemString = string.Join("','", items);

            

            var sqlQueryString = SQLQueries.GetRevenueReportQuery(searchModel.StartDate.ToString(), searchModel.EndDate.ToString(),
                                                                    salesPersonstring.ToString(), buyersString.ToString(),
                                                                    vendorString.ToString(), itemString.ToString());

            var dataTableResult = new DataTable();
            using (var dataAccessAdo = new DataAccessADO())
            {
                dataTableResult = dataAccessAdo.ReadToDataSet(sqlQueryString).Tables[0];
            }
            //var dataTableResult = base.GetDataTable(MDXCubeQueries.SearchRevenueReportQuery(searchModel));

            //var vendors = new List<RevenueReportBM>();
            //vendors.Add(new RevenueReportBM() { Buyer = "Jason 1", Class = "Class 1", Customer = "Abhi test 1", InvoiceDate = DateTime.Today.AddDays(-3) });
            //vendors.Add(new RevenueReportBM() { Buyer = "Jason 2", Class = "Class 1", Customer = "Abhi test 2", InvoiceDate = DateTime.Today.AddDays(-3) });
            //vendors.Add(new RevenueReportBM() { Buyer = "Jason 3", Class = "Class 1", Customer = "Abhi test 3", InvoiceDate = DateTime.Today.AddDays(-3) });
            //vendors.Add(new RevenueReportBM() { Buyer = "Jason 4", Class = "Class 1", Customer = "Abhi test 4", InvoiceDate = DateTime.Today.AddDays(-3) });

            var result = dataTableResult.AsEnumerable()
                            .Select(x => new RevenueReportBM()
                            {
                                SalesPerson = x.Field<string>("SalesPerson"),
                                Buyer = x.Field<string>("buyerName"),
                                Vendor = x.Field<string>("Vendor"),
                                Item = x.Field<string>("Item"),
                                Class = x.Field<string>("class"),
                                Customer = x.Field<string>("Customer"),
                                InvoiceNumber = x.Field<string>("InvoiceNumber"),
                                //InvoiceDate = x.Field<DateTime?>(7)
                                Cost = x.Field<decimal?>("costSales").HasValue?x.Field<decimal>("costSales"):0,
                                SalesPrice=x.Field<decimal?>("amtSales").HasValue?x.Field<decimal>("amtSales"):0,
                                InvoiceDate = x.Field<DateTime>("InvoiceDate"),
                                Quantity = x.Field<decimal?>("qtyShipped").HasValue ? x.Field<decimal>("qtyShipped") : 0,
                            })
                            .ToList();

            return result;
            #region Map to Datatable
            #endregion

            //return data;
        }

        public GenericReoprtMapperWithTopBottom GetCustomerRevenueReport(string currentStart, string currentEnd
                                                          , string previousStart, string previousEnd
                                                          , string comodity = "", string category = ""
                                                          , string minSalesAmt = "",  string salesPerson = "")
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

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_GetCustomerRevenueReport", parameterList.ToArray());

                //string sqlString = SQLQueries.GetCasesSoldReportQuery(currentStart, currentEnd
                //                                    , previousStart, previousEnd, comodity, category, minSalesAmt);
                //var dataTableResult = adoDataAccess.ReadToDataSet(sqlString);
                var result = dataTableResult.Tables[0]
                                            .AsEnumerable()
                                            .Select(x => new GenericReportMapperBM
                                            {
                                                Customer = x.Field<string>("Customer"),
                                                Current = (x.Field<decimal?>("CurrentRevenue") ?? 0).ToRoundTwoDecimalDigits().ToStripDecimal(),
                                                Previous = (x.Field<decimal?>("PreviousRevenue") ?? 0).ToRoundTwoDecimalDigits().ToStripDecimal(),
                                                Difference = (x.Field<decimal?>("Difference") ?? 0).ToRoundTwoDecimalDigits().ToStripDecimal(),
                                                PercentageDifference = (x.Field<decimal?>("CurrentRevenue") ?? 0).ToPercentageDifference((x.Field<decimal?>("PreviousRevenue") ?? 0)),
                                            }).ToList();
                
                response.ReportData = result.OrderByDescending(x => x.PercentageDifference).ToList();
                response.ChartData = new GenericTopBottomTwoBarChartData
                {
                    Top = result.OrderByDescending(d => d.PercentageDifference).Take(10).Select(x => new GenericTwoBarChartdata
                    {
                        Category = x.Customer,
                        Color1 = ChartColorBM.ProduceCurrent,
                        Color2 = ChartColorBM.ProducePrevious,
                        Label = x.Customer,
                        Value1 = (decimal)x.Current.ToStripDecimal(),
                        Value2 = (decimal)x.Previous.ToStripDecimal(),
                        Tooltip = x.PercentageDifference
                    }
                    ).ToList(),
                    Bottom = result.OrderBy(d => d.PercentageDifference).Take(10).Select(x => new GenericTwoBarChartdata
                    {
                        Category = x.Customer,
                        Color1 = ChartColorBM.ProduceCurrent,
                        Color2 = ChartColorBM.ProducePrevious,
                        Label = x.Customer,
                        Value1 = (decimal)x.Current.ToStripDecimal(),
                        Value2 = (decimal)x.Previous.ToStripDecimal(),
                        Tooltip = x.PercentageDifference
                    }
                    ).ToList()
                };

                return response;
            }
        }


        /// <summary>
        /// Get the list of Items for the Revenue Report filter w.r.t to the serached key
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetItemsForRevenueFilter(string inputString)
        {
            //var result = GetAllItems(MDXCubeQueries.GetAllItems());

            string sqlQueryString=SQLQueries.GetItemSearchQuery(inputString);

            var dataTableResult = new DataTable();
            using (var dataAccessAdo = new DataAccessADO())
            {
                dataTableResult = dataAccessAdo.ReadToDataSet(sqlQueryString).Tables[0];
            }

            var searchKey = inputString.ToLower();

            //if (!string.IsNullOrEmpty(inputString))
            //    result = result.Where(x => x.Value.ToLower().Contains(searchKey)).ToList();

            var result = dataTableResult.AsEnumerable()
                                        .Select(x => x.Field<string>(0).TrimEnd()).Distinct()
                                        .Select(x => new KeyValuePair<string, string>(x, x))
                                        .ToList();

            return result;
        }
    }
}
