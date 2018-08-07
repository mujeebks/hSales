using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.DataProvider.Utilities
{
    public static class MDXCubeQueries
    {

        const string _cubeName = "NPI_SalesDetails01";

        #region Cases Sold

        #region Category Queries

        public static string GetCasesSoldByCategoryMonthlyQuery(string dateFilter, string commodity)
        {
            var query =
                string.Format(MDXCubeQueries.CasesSoldByCategoryMonthlyQuery
                                , (string.IsNullOrEmpty(commodity) ? "" :
                                (string.Format("-[Product].[Commodity Code].&[{0}]", (commodity == "Produce") ? "Grocery" : "Produce")))
                                , dateFilter
                                );

            return query;
        }

        public static string GetTotalCasesByCategorySoldQuery(string dateFilter)
        {
            var query =
                string.Format(MDXCubeQueries.CasesSoldByCategoryTotalQuery
                                , dateFilter);

            return query;
        }

        public static string GetCasesSoldByCategoryYearToDate(string dateFilter)
        {
            var query = string.Format(MDXCubeQueries.CasesSoldByCategoryYearToDateQuery, dateFilter);

            return query;
        }

        public static string GetCasesSoldByCategoryYearToCustomQuery(int year, int month)
        {
            var query = string.Format(MDXCubeQueries.CasesSoldByCategoryYearToCustomQuery, year, month);

            return query;
        }

        #endregion

        #region Sales Person Queries

        public static string GetCasesSoldBySalesPersonMonthlyQuery(string dateFilter)
        {
            var query = string.Format(MDXCubeQueries.CaesSoldBySalesPersonMonthlyQuery, dateFilter);

            return query;
        }

        public static string GetCasesSoldBySalesPersonYearToDateQuery(string dateFilter)
        {
            var query = string.Format(MDXCubeQueries.CasesSoldBySalesPersonYearToDateQuery, dateFilter);

            return query;
        }

        public static string GetCasesSoldBySalesPersonYearToCustomQuery(int year, int month)
        {
            var query = string.Format(MDXCubeQueries.CasesSoldBySalesPersonYearToCustomQuery, year, month);

            return query;
        }


        #endregion

        #endregion

        #region Revenue
        #region Dashboard

        #region Category Queries
        public static string GetRevenueByCategoryMonthlyQuery(string dateFilter, string commodity)
        {
            var query =
                string.Format(MDXCubeQueries.RevenueByCategoryMonthlyQuery
                                , (string.IsNullOrEmpty(commodity) ? "" :
                                (string.Format("-[Product].[Commodity Code].&[{0}]", (commodity == "Produce") ? "Grocery" : "Produce")))
                                , dateFilter
                                );

            return query;
        }

        public static string GetTotalRevenueByCategoryQuery(string dateFilter)
        {
            var query =
                string.Format(MDXCubeQueries.RevenueByCategoryTotalQuery
                                , dateFilter);

            return query;
        }

        public static string GetRevenueByCategoryYearToDateQuery(string dateFilter)
        {
            var query = string.Format(MDXCubeQueries.RevenueByCategoryYearToDateQuery, dateFilter);

            return query;
        }

        public static string GetRevenueByCategoryYearToCustomQuery(int year, int month)
        {
            var query = string.Format(MDXCubeQueries.RevenueByCategoryYearToCustomQuery, year, month);

            return query;
        }
        #endregion

        #region Sales Person Queries

        public static string GetRevenueBySalesPersonMonthlyQuery(string dateFilter)
        {
            var query = string.Format(MDXCubeQueries.RevenueBySalesPersonMonthlyQuery, dateFilter);

            return query;
        }

        public static string GetRevenueBySalesPersonYearToDateQuery(string dateFilter)
        {
            var query = string.Format(MDXCubeQueries.RevenueBySalesPersonYearToDateQuery, dateFilter);

            return query;
        }

        public static string GetRevenueBySalesPersonYearToCustomQuery(int year, int month)
        {
            var query = string.Format(MDXCubeQueries.RevenueBySalesPersonYearToCustomQuery, year, month);

            return query;
        }
        #endregion

        #endregion

        #region Report

        public static string GetAllSalesPersons()
        {
            //var asd = "WITH MEMBER A AS 1 SELECT A ON 0 , [Salesperson].[Salesperson].[Salesperson Name] ON 1 ";
            return @"WITH MEMBER A AS 1 SELECT A ON 0 ,
                        [Salesperson].[Salesperson].[Salesperson Name] ON  1
                        FROM [" + _cubeName + "]";
        }

        public static string GetAllBuyers()
        {
            return @"WITH MEMBER A AS 1 SELECT A ON 0 ,
                    [Buyer].[Buyer].[Buyer Name]
                    ON  1
                    FROM [" + _cubeName + "]";
        }

        public static string GetAllItems()
        {
            return @"WITH MEMBER A AS 1 
                    SELECT A ON 0,
                    [Product].[Product].[Item Id Description]
                    ON  1
                    FROM [" + _cubeName + "]";
        }

        public static string GetAllVendors()
        {
            return @"WITH MEMBER A AS 1 SELECT A ON 0 ,
                    [Dealer].[Dealer].[Dealer Name]
                    ON  1
                    FROM [" + _cubeName + "]";
        }

        #endregion
        #endregion

        #region Sales

        public static string GetSalesQuery(string startDate, string endDate, bool returnTop)
        {
            var query =
                string.Format(MDXCubeQueries.SalesBySalesPerson
                                , returnTop ? "TOPCOUNT" : "BOTTOMCOUNT"
                                , startDate
                                , endDate);

            return query;
        }

        public static string GetSalesMonthToDateQuery(string dateFilter, bool isTop5)
        {
            var query = string.Format(MDXCubeQueries.SalesBySalesPersonMonthTodDate, (isTop5) ? "TOP" : "BOTTOM", dateFilter);
            return query;
        }

        //public static string GetSalesTotalTop5Query(string dateFilter)
        //{
        //    var query =
        //        string.Format(MDXCubeQueries.SalesBySalesPersonTop5Monthly
        //                        , dateFilter);

        //    return query;
        //}

        //public static string GetSalesTotalBottom5Query(string dateFilter)
        //{
        //    var query =
        //        string.Format(MDXCubeQueries.SalesBySalesPersonBottom5Monthly
        //                        , dateFilter);

        //    return query;
        //}

        #endregion

        #region Expenses

        #region Category Queries
        public static string GetTotalExpensesByCategoryQuery(string dateFilter)
        {
            var query =
                string.Format(MDXCubeQueries.ExpensesByCategoryTotalQuery
                                , dateFilter);

            return query;
        }

        public static string GetExpensesByCategoryMonthlyQuery(string dateFilter, string commodity)
        {
            var query =
                string.Format(MDXCubeQueries.ExpensesByCategoryMonthlyQuery
                                , (string.IsNullOrEmpty(commodity) ? "" :
                                (string.Format("-[Product].[Commodity Code].&[{0}]", (commodity == "Produce") ? "Grocery" : "Produce")))
                                , dateFilter
                                );

            return query;
        }

        public static string GetExpensesByCategoryYearToDate(string dateFilter)
        {
            var query = string.Format(MDXCubeQueries.ExpensesByCategoryYearToDateQuery, dateFilter);

            return query;
        }

        public static string GetExpensesByCategoryYearToCustomQuery(int year, int month)
        {
            var query = string.Format(MDXCubeQueries.ExpensesByCategoryYearToCustomQuery, year, month);

            return query;
        }
        #endregion

        #region Sales Person Queries

        public static string GetExpensesBySalesPersonMonthlyQuery(string dateFilter)
        {
            var query = string.Format(MDXCubeQueries.ExpensesBySalesPersonMonthlyQuery, dateFilter);

            return query;
        }

        public static string GetExpensesBySalesPersonYearToDateQuery(string dateFilter)
        {
            var query = string.Format(MDXCubeQueries.ExpensesBySalesPersonYearToDateQuery, dateFilter);

            return query;
        }

        public static string GetExpensesBySalesPersonYearToCustomQuery(int year, int month)
        {
            var query = string.Format(MDXCubeQueries.ExpensesBySalesPersonYearToCustomQuery, year, month);

            return query;
        }


        #endregion

        #endregion

        #region Profitability

        public static string GetProfitabilityQuery(string beginYear, string endYear)
        {
            var query = string.Format(MDXCubeQueries.ProfitabilityQuery, beginYear, endYear);
            return query;
        }

        #endregion

        #region MDX Queries

        #region Cases Sold

        public static string CasesSoldByCategoryTotalQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                                    AS AGGREGATE(MTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Month]
                                                    ,1 
                                                    ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                                    ,[Measures].[Qty Sold]) 
                                    MEMBER [Measures].[CurrentValue]
                                    AS AGGREGATE(MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                                    ,[Measures].[Qty Sold]) 
                                    SELECT {{ 
                                        [Measures].[CurrentValue]
                                        ,[Measures].[PreviousValue]
                                    }} ON 0,
                                    NON EMPTY{{
                                    ([Product].[Commodity Code].[Commodity Code]-[Product].[Commodity Code].&[])
                                    *[Date All Document].[Date Hierarchy].[Date]
                                    }}ON 1
                                    FROM  
                                    (
                                    SELECT [Date All Document].[Date Hierarchy].[Date].&[{0}]
                                     ON 0 FROM 
                                    [" + _cubeName + "])";
            }
        }

        public static string CasesSoldByCategoryMonthlyQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                                    AS AGGREGATE(MTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Month]
                                                ,1
                                                ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                                ,[Measures].[Qty Sold]) 
                                    MEMBER [Measures].[CurrentValue]
                                    AS AGGREGATE(MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                                ,[Measures].[Qty Sold]) 
                                    SELECT {{ 
                                        [Measures].[CurrentValue]
                                        ,[Measures].[PreviousValue]
                                    }} ON 0,
                                    NON EMPTY{{
                                    [Specialty].[Specialty].[Specialty Name]*
                                    ([Product].[Commodity Code].[Commodity Code]-[Product].[Commodity Code].&[]{0})
                                    *[Date All Document].[Date Hierarchy].[Date]
                                    *[Date All Document].[Month].[Month]
                                    *[Date All Document].[Year].[Year]
                                    *[Salesperson].[Salesperson].[Salesperson Name] AS SalesPerson
                                    *[Salesperson].[A Salesperson Key].[A Salesperson Key]
                                    }}ON 1
                                    FROM  
                                    (
                                    SELECT [Date All Document].[Date Hierarchy].[Date].&[{1}]
                                     ON 0 FROM 
                                    [" + _cubeName + "])";
            }
        }

        public static string CasesSoldByCategoryYearToDateQuery
        {
            get
            {
                return
                        @"WITH MEMBER [Measures].[PreviousValue]
                            AS AGGREGATE(YTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Year]
                                        ,1
                                        ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                        ,[Measures].[Qty Sold]) 
                             MEMBER [Measures].[CurrentValue]
                            AS AGGREGATE(YTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                        ,[Measures].[Qty Sold]) 
                              SELECT {{ 
                                 [Measures].[CurrentValue]
                                ,[Measures].[PreviousValue]
                            }} ON 0,
                            NON EMPTY({{
                            [Specialty].[Specialty].[Specialty Name]*
                            ([Product].[Commodity Code].[Commodity Code]-[Product].[Commodity Code].&[])*
                            [Date All Document].[Date Hierarchy].[Date]*
                            [Salesperson].[Salesperson].[Salesperson Name]
                            *[Date All Document].[Year].[Year]
                            *[Salesperson].[A Salesperson Key].[A Salesperson Key]
                            }})ON 1
                            FROM  
                            (SELECT [Date All Document].[Date Hierarchy].[Date].&[{0}]  
                             ON 0 FROM 
                            [" + _cubeName + "])";
                //,[Measures].[Qty Sold]
            }
        }

        public static string CasesSoldByCategoryYearToCustomQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                        AS AGGREGATE(YTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Year]
                                        ,1
                                        ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                        ,[Measures].[Qty Sold]) 
                         MEMBER [Measures].[CurrentValue]
                        AS AGGREGATE(YTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                        ,[Measures].[Qty Sold]) 
                        SELECT {{ [Measures].[CurrentValue]
                        , [Measures].[PreviousValue]
                        }} ON 0,
                        NON EMPTY({{
                        [Specialty].[Specialty].[Specialty Name]
                        *[Product].[Commodity Code].[Commodity Code]
                        *[Date All Document].[Month].[Month]
                        *[Date All Document].[Year].[Year]
                        *[Salesperson].[A Salesperson Key].[A Salesperson Key]
                        *[Salesperson].[Salesperson].[Salesperson Name] AS SalesPerson
                        *[Date All Document].[Date Hierarchy].[Date]
                        }})ON 1
                        FROM  
                        (
                        SELECT {{[Product].[Commodity Code].[Commodity Code] -[Product].[Commodity Code].&[]}} ON 0 FROM(
                        SELECT  [Date All Document].[Date Hierarchy].[Month].&[{0}]&[{1}]
                         ON 0 FROM 
                        [" + _cubeName + "]))";
            }
        }

        public static string CaesSoldBySalesPersonMonthlyQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                                            AS AGGREGATE(MTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Month]
                                                            ,1
                                                            ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                                            ,[Measures].[Qty Sold]) 
                                            MEMBER [Measures].[CurrentValue]
                                            AS AGGREGATE(MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                                        ,[Measures].[Qty Sold]) 
                                            SELECT {{ 
                                                [Measures].[CurrentValue]
                                                ,[Measures].[PreviousValue]
                                            }} ON 0,
                                            NON EMPTY{{
                                            [Salesperson].[Salesperson].[Salesperson Name]
                                            *[Customer].[Customer].[Customer Id Name]
                                            *[Date All Document].[Date Hierarchy].[Date]
                                            *[Date All Document].[Month].[Month]
                                            }}ON 1
                                            FROM  
                                            (SELECT [Date All Document].[Date Hierarchy].[Date].&[{0}] ON 0 FROM [" + _cubeName + "])";

               
            }
        }

        public static string CasesSoldBySalesPersonYearToDateQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                            AS AGGREGATE(YTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Year]
                                        ,1
                                        ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                        ,[Measures].[Qty Sold]) 
                             MEMBER [Measures].[CurrentValue]
                            AS AGGREGATE(YTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                        ,[Measures].[Qty Sold]) 
                            SELECT {{ 
                            [Measures].[CurrentValue]
                            ,[Measures].[PreviousValue]
                            }} ON 0,
                            NON EMPTY({{
                            [Salesperson].[Salesperson].[Salesperson Name]
                            *[Customer].[Customer].[Customer Id Name]
                            *[Date All Document].[Month].[Month]
                            *[Date All Document].[Date Hierarchy].[Date]                            
                            }})ON 1
                            FROM  
                            (SELECT [Date All Document].[Date Hierarchy].[Date].&[{0}]  ON 0 FROM [" + _cubeName + "])";
            }
        }

        public static string CasesSoldBySalesPersonYearToCustomQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                        AS AGGREGATE(YTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Year]
                                        ,1
                                        ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                        ,[Measures].[Qty Sold]) 
                         MEMBER [Measures].[CurrentValue]
                        AS AGGREGATE(YTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                        ,[Measures].[Qty Sold]) 
                        SELECT {{ 
                            [Measures].[CurrentValue]
                            ,[Measures].[PreviousValue]
                        }} ON 0,
                        NON EMPTY({{
                        [Salesperson].[Salesperson].[Salesperson Name]
                        *[Customer].[Customer].[Customer Id Name]
                        *[Date All Document].[Month].[Month]                        
                        }})ON 1
                        FROM  
                        (
                        SELECT  [Date All Document].[Date Hierarchy].[Month].&[{0}]&[{1}] ON 0 FROM [" + _cubeName + "])";
            }
        }

        #endregion

        #region Revenue

        public static string RevenueByCategoryTotalQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                        AS AGGREGATE(MTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Month]
                                        ,1
                                        ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                        ,[Measures].[Amt Sales]) 
                        MEMBER [Measures].[CurrentValue]
                        AS AGGREGATE(MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                        ,[Measures].[Amt Sales]) 
                        SELECT {{ 
                        [Measures].[CurrentValue]
                        ,[Measures].[PreviousValue]
                        }} ON 0,
                        NON EMPTY{{
                        ([Product].[Commodity Code].[Commodity Code]-[Product].[Commodity Code].&[])
                        *[Date All Document].[Date Hierarchy].[Date]
                        *[Date All Document].[Month].[Month]
                        }}ON 1
                        FROM  
                        (SELECT [Date All Document].[Date Hierarchy].[Date].&[{0}] ON 0 FROM [" + _cubeName + "])";
            }
        }

        public static string RevenueByCategoryMonthlyQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                        AS AGGREGATE(MTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Month]
                                        ,1
                                        ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                        ,[Measures].[Amt Sales]) 
                        MEMBER [Measures].[CurrentValue]
                        AS AGGREGATE(MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                        ,[Measures].[Amt Sales]) 
                        SELECT {{ 
                        [Measures].[CurrentValue]
                        ,[Measures].[PreviousValue]
                        }} ON 0,
                        NON EMPTY{{
                        [Specialty].[Specialty].[Specialty Name]
                        *([Product].[Commodity Code].[Commodity Code]-[Product].[Commodity Code].&[]{0})
                        *[Date All Document].[Date Hierarchy].[Date]
                        *[Date All Document].[Month].[Month]
                        *[Salesperson].[Salesperson].[Salesperson Name]
                        *[Salesperson].[A Salesperson Key].[A Salesperson Key]
                        *[Date All Document].[Year].[Year]
                        }}ON 1
                        FROM  
                        (SELECT [Date All Document].[Date Hierarchy].[Date].&[{1}] ON 0 FROM [" + _cubeName + "])";
            }
        }

        public static string RevenueByCategoryYearToDateQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                        AS AGGREGATE(YTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Year]
                                    ,1
                                    ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                    ,[Measures].[Amt Sales]) 
                         MEMBER [Measures].[CurrentValue]
                        AS AGGREGATE(YTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                    ,[Measures].[Amt Sales]) 
                        SELECT {{ 
                            [Measures].[CurrentValue]
                            ,[Measures].[PreviousValue]
                        }} ON 0,
                        NON EMPTY({{
                        [Specialty].[Specialty].[Specialty Name]*
                        ([Product].[Commodity Code].[Commodity Code]-[Product].[Commodity Code].&[])*
                        [Date All Document].[Month].[Month]*
                        [Date All Document].[Date Hierarchy].[Date]*
                        [Salesperson].[Salesperson].[Salesperson Name]
                        *[Date All Document].[Year].[Year]
                        }})ON 1
                        FROM  
                        (SELECT [Date All Document].[Date Hierarchy].[Date].&[{0}]  ON 0 FROM [" + _cubeName + "])";
            }
        }

        public static string RevenueByCategoryYearToCustomQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                            AS AGGREGATE(YTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Year]
                                        ,1
                                        ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                        ,[Measures].[Amt Sales]) 
                            MEMBER [Measures].[CurrentValue]
                            AS AGGREGATE(YTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                        ,[Measures].[Amt Sales]) 
                            SELECT {{ 
                                     [Measures].[CurrentValue]
                                    ,[Measures].[PreviousValue] 
                            }} ON 0,
                            NON EMPTY({{
                            [Specialty].[Specialty].[Specialty Name]
                            *[Product].[Commodity Code].[Commodity Code]
                            *[Date All Document].[Month].[Month]
                            *[Salesperson].[Salesperson].[Salesperson Name]
                            *[Salesperson].[A Salesperson Key].[A Salesperson Key]
                            *[Date All Document].[Year].[Year]
                            }})ON 1
                            FROM  
                            (SELECT {{[Product].[Commodity Code].[Commodity Code] -[Product].[Commodity Code].&[]}} ON 0 
                            FROM
                            (SELECT  [Date All Document].[Date Hierarchy].[Month].&[{0}]&[{1}] ON 0 FROM [" + _cubeName + "]))";
            }
        }

        public static string RevenueBySalesPersonMonthlyQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                            AS AGGREGATE(MTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Month]
                                            ,1
                                            ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                            ,[Measures].[Amt Sales]) 
                             MEMBER [Measures].[CurrentValue]
                            AS AGGREGATE(MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                        ,[Measures].[Amt Sales]) 
                            SELECT {{ 
                                    [Measures].[CurrentValue]
                                    ,[Measures].[PreviousValue] 
                            }} ON 0,
                            NON EMPTY{{
                            [Salesperson].[Salesperson].[Salesperson Name]
                            *[Customer].[Customer].[Customer Id Name]
                            *[Date All Document].[Date Hierarchy].[Date]
                            *[Date All Document].[Month].[Month]
                            }}ON 1
                            FROM  
                            (SELECT [Date All Document].[Date Hierarchy].[Date].&[{0}]ON 0 FROM [" + _cubeName + "])";
            }
        }

        public static string RevenueBySalesPersonYearToDateQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                        AS AGGREGATE(YTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Year]
                                    ,1
                                    ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                    ,[Measures].[Amt Sales]) 
                         MEMBER [Measures].[CurrentValue]
                        AS AGGREGATE(YTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                    ,[Measures].[Amt Sales]) 
                        SELECT {{ 
                                [Measures].[CurrentValue]
                                ,[Measures].[PreviousValue] 
                        }} ON 0,
                        NON EMPTY({{
                        [Salesperson].[Salesperson].[Salesperson Name]
                        *[Customer].[Customer].[Customer Id Name]
                        *[Date All Document].[Month].[Month]
                        *[Date All Document].[Date Hierarchy].[Date]
                        }})ON 1
                        FROM  
                        (SELECT {{[Product].[Commodity Code].[Commodity Code] -[Product].[Commodity Code].&[]}} ON 0 
                        FROM 
                        (SELECT [Date All Document].[Date Hierarchy].[Date].&[{0}]  ON 0 FROM [" + _cubeName + "]))";
            }
        }

        public static string RevenueBySalesPersonYearToCustomQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                            AS AGGREGATE(YTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Year]
                                            ,1
                                            ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                            ,[Measures].[Amt Sales]) 
                             MEMBER [Measures].[CurrentValue]
                            AS AGGREGATE(YTD([Date All Document].[Date Hierarchy].CURRENTMEMBER) 
                                        ,[Measures].[Amt Sales]) 
                            SELECT {{ 
                                    [Measures].[CurrentValue]
                                    ,[Measures].[PreviousValue] 
                            }} ON 0,
                            NON EMPTY({{
                            [Salesperson].[Salesperson].[Salesperson Name]
                            *[Customer].[Customer].[Customer Id Name]
                            *[Date All Document].[Month].[Month]
                            }})ON 1
                            FROM  
                            (SELECT {{[Product].[Commodity Code].[Commodity Code] -[Product].[Commodity Code].&[]}} ON 0 
                            FROM
                            (SELECT  [Date All Document].[Date Hierarchy].[Month].&[{0}]&[{1}] ON 0 FROM [" + _cubeName + "]))";
            }
        }

        /// <summary>
        /// Query for the Revenue Report
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        internal static string SearchRevenueReportQuery(BusinessModel.RevenueReportFilterBM searchModel)
        {
            if (searchModel == null)
                return string.Empty;

            var queryString = @"
                    SELECT
                    {
                    [Measures].[Amt Sales]
                    ,[Measures].[Qty Sold]
                    ,[Measures].[Amt Cost Of Sales]
                    }

                     ON 0 ,
                    NON EMPTY { 
                    [Salesperson].[Salesperson].[Salesperson Name]*
                    [Buyer].[Buyer].[Buyer Name]*
                    [Dealer].[Dealer].[Dealer Name]*
                    [Product].[Product].[Item Id Description]*
                    [Product].[Item Class].[Class]*
                    [Customer].[Customer].[Customer Id Name]*
                    [Tran Receivables].[A Invoice Key].[A Invoice Key]*
                    [Date All Document].[Base Date].[Base Date]

                    }
                     ON 1
                    FROM 
                    (
                    SELECT {[Salesperson].[Salesperson].[Salesperson Name]-
                    [Salesperson].[Salesperson].&[]}
                    ON 0 FROM( 
                    SELECT "
                    +
                        MDXCubeQueries._getSalesPersonQuery(searchModel)
                    +
                    @"
                    ON 0 
                    FROM(
                    SELECT "
                    +
                        MDXCubeQueries._getBuyerQuery(searchModel)
                    +
                    @"
                    ON 0 FROM
                    (
                    SELECT"
                    +
                        MDXCubeQueries._getVendorQuery(searchModel)
                    +
                    @"
                    ON 0
                    FROM(
                    SELECT "
                    +
                       MDXCubeQueries._getItemQuery(searchModel)
                    +
                    @"
                    ON 0 
                    FROM(
                    SELECT
                    [Date All Document].[Date Hierarchy].[Date].&[" + searchModel.StartDate.ToString("yyyy-MM-ddTHH:mm:ss") + @"]:
                    [Date All Document].[Date Hierarchy].[Date].&[" + searchModel.StartDate.ToString("yyyy-MM-ddTHH:mm:ss") + @"]
                    ON 0
                    FROM [NPI_SalesDetails01]
                    )
                    )
                    )
                    )
                    ))";

            return queryString;

        }

        /// <summary>
        /// get dynamic part of the Revenue Report's query based on the Sales Person
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private static string _getSalesPersonQuery(BusinessModel.RevenueReportFilterBM searchModel)
        {
            string query = "{ ";
            if (searchModel.SalesPerson != null && searchModel.SalesPerson.Count > 0)
            {
                searchModel.SalesPerson.ForEach(x =>
                {
                    query += "[Salesperson].[Salesperson].[Salesperson Name].&[" + x.Value + "],";
                });
                query = query.Substring(0, query.Length - 1);
                query += " }";
            }
            else
            {
                query = "[Salesperson].[Salesperson].[All Salespersons] //Use this if no filter on SalesPerson is required";
            }
            return query;
        }

        /// <summary>
        /// get dynamic part of the Revenue Report's query based on the Buyer
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private static string _getBuyerQuery(BusinessModel.RevenueReportFilterBM searchModel)
        {
            string query = "{ ";
            if (searchModel.Buyer != null && searchModel.Buyer.Count > 0)
            {
                searchModel.Buyer.ForEach(x =>
                {
                    query += "[Buyer].[Buyer].[Buyer Name].&[" + x.Value + "],";
                });
                query = query.Substring(0, query.Length - 1);
                query += " }";
            }
            else
            {
                query = "[Buyer].[Buyer].[All Buyers] //Use this if no BUYER filter is needed";
            }
            return query;
        }

        /// <summary>
        /// get dynamic part of the Revenue Report's query based on the Vendor
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private static string _getVendorQuery(BusinessModel.RevenueReportFilterBM searchModel)
        {
            string query = "{ ";
            if (searchModel.Vendor != null && searchModel.Vendor.Count > 0)
            {
                searchModel.Vendor.ForEach(x =>
                {
                    query += "[Dealer].[Dealer].[Dealer Name].&[" + x.Value + "],";
                });
                query = query.Substring(0, query.Length - 1);
                query += " }";
            }
            else
            {
                query = "[Dealer].[Dealer].[All Dealers] //Use this if no filter required on Vendor";
            }
            return query;
        }

        /// <summary>
        /// get dynamic part of the Revenue Report's query based on the Item
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private static string _getItemQuery(BusinessModel.RevenueReportFilterBM searchModel)
        {
            string query = "{ ";
            if (searchModel.Item != null && searchModel.Item.Count > 0)
            {
                searchModel.Item.ForEach(x =>
                {
                    query += "[Product].[Product].[Item Id Description].&[" + x.Value + "],";
                });
                query = query.Substring(0, query.Length - 1);
                query += " }";
            }
            else
            {
                query = "[Product].[Product].[All Products] //Use this if no filter required on Product";
            }
            return query;
        }
        #endregion

        #region Sales

        public static string SalesBySalesPerson
        {
            get
            {
                return @"SELECT
                        {{
                        [Measures].[Amt Sales]
                        }} ON 0 ,
                        NON EMPTY{{ {0}(FILTER({{[Salesperson].[Salesperson].[Salesperson Name]
                        }}
                        ,[Measures].[Amt Sales]>0)
                        ,10,[Measures].[Amt Sales])
                        *[Date All Document].[Date Hierarchy].[Month]
                        *[Customer].[Customer].[Customer Id Name]
                        *[Customer].[Customer Type].[Customer Type]
                        }} ON 1
                        FROM 
                        (
                        SELECT {{ [Date All Document].[Date Hierarchy].[Date].&[{1}]
                        :[Date All Document].[Date Hierarchy].[Date].&[{2}]
                        }} ON 0 
                        FROM [" + _cubeName + "])";
            }
        }

        public static string SalesBySalesPersonMonthTodDate
        {
            get
            {
                return @"WITH MEMBER MTDSAles AS
                            SUM(MTD(
                            [Date All Document].[Date Hierarchy].CURRENTMEMBER
                             )
                             ,[Measures].[Amt Sales])
                            SELECT
                            {{
                            MTDSAles
                            }}
                             ON 0 ,
                            NON EMPTY {{{0}COUNT(FILTER({{[Salesperson].[Salesperson].[Salesperson Name]*
                            [Date All Document].[Date Hierarchy].[Month]
                            }}
                            ,[Measures].[Amt Sales]>0 )
                            ,5,[Measures].[Amt Sales])
                            *[Customer].[Customer].[Customer Id Name]
                            }}
                             ON 1
                            FROM 
                            (
                            SELECT {{[Salesperson].[Salesperson].[Salesperson Name]-
                            [Salesperson].[Salesperson].&[]}}
                            ON 0 FROM(
                            SELECT  [Date All Document].[Date Hierarchy].[Date].&[{1}]
                            ON 0 
                            FROM [" + _cubeName + "]))";
            }
        }

        public static string SalesBySalesPersonBottom5Monthly
        {
            get
            {
                return @"WITH MEMBER CustomerCount AS
                            DISTINCTCOUNT([Customer].[Customer].[Customer Id Name])
                            SELECT
                            {{CustomerCount
                            ,[Measures].[Amt Sales]
                            }}
                             ON 0 ,
                            BOTTOMCOUNT(FILTER({{[Salesperson].[Salesperson].[Salesperson Name]*
                            [Date All Document].[Date Hierarchy].[Month]
                            }},[Measures].[Amt Sales]>0)
                            ,5,[Measures].[Amt Sales])
                             ON 1
                            FROM 
                            (
                            SELECT [Date All Document].[Date Hierarchy].[Month].&[2016]&[{0}]
                            ON 0 
                            FROM [" + _cubeName + "])";
            }
        }

        public static string SalesBySalesPersonTop5LastMonth
        {
            get
            {
                return @"";
            }
        }

        public static string SalesBySalesPersonBottom5LastMonth
        {
            get
            {
                return @"";
            }
        }

        #endregion

        #region Dashboard

        //public static string DashboardStatistics
        //{
        //    get
        //    {
        //        return @"
        //                WITH 
        //                 MEMBER [Measures].[CurrentMonthQuantitySold]
        //                AS

        //                AGGREGATE(
        //                MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER
        //                 )
        //                 ,[Measures].[Qty Sold]) 

        //                   MEMBER [Measures].[CurrentMonthExpense]
        //                AS

        //                AGGREGATE(
        //                MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER
        //                 )
        //                 ,[Measures].[Amt Cost Of Sales]) 

        //                  MEMBER [Measures].[Gross Profit]
        //                AS

        //                AGGREGATE(
        //                MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER
        //                 )
        //                 ,[Measures].[Amt Gross Profit]
        //                 ) 
        //                  MEMBER [Measures].[CurrentMonthSales]
        //                AS

        //                AGGREGATE(
        //                MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER
        //                 )
        //                 ,[Measures].[Amt Sales]
        //                 ) 
        //                SELECT { [Measures].[CurrentMonthQuantitySold]
        //                ,[Measures].[CurrentMonthExpense]
        //                ,[Measures].[Gross Profit]
        //                ,[Measures].[CurrentMonthSales]
        //                } ON 0,
        //                NON EMPTY{ [Date All Document].[Month].[Month]


        //                }ON 1
        //                FROM  
        //                (
        //                SELECT [Date All Document].[Date Hierarchy].[Date].&[2016-07-10T00:00:00]

        //                 ON 0 FROM 
        //                [" + MDXCubeQueries._cubeName + @"])
        //        ";
        //    }
        //}

        public static string DashboardStatistics(string date)
        {

            return @"
                        WITH 
                         MEMBER [Measures].[PreviousMonthQuantitySold]
                        AS
 
                        AGGREGATE(
                        MTD(PARALLELPERIOD(
                        [Date All Document].[Date Hierarchy].[Month]
                        ,1
                        ,[Date All Document].[Date Hierarchy].CURRENTMEMBER
                        )
                         )
                         ,[Measures].[Qty Sold]) 
  
                         MEMBER [Measures].[CurrentMonthQuantitySold]
                        AS
 
                        AGGREGATE(
                        MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER
                         )
                         ,[Measures].[Qty Sold]) 
  
  
                           MEMBER [Measures].[PreviousMonthExpense]
                        AS
 
                        AGGREGATE(
                        MTD(PARALLELPERIOD(
                        [Date All Document].[Date Hierarchy].[Month]
                        ,1
                        ,[Date All Document].[Date Hierarchy].CURRENTMEMBER
                        ))
                         ,[Measures].[Amt Cost Of Sales])
  
                           MEMBER [Measures].[CurrentMonthExpense]
                        AS
 
                        AGGREGATE(
                        MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER
                         )
                         ,[Measures].[Amt Cost Of Sales]) 

                          MEMBER [Measures].[Previous Gross Profit]
                        AS
 
                        AGGREGATE(
                        MTD(PARALLELPERIOD(
                        [Date All Document].[Date Hierarchy].[Month]
                        ,1
                        ,[Date All Document].[Date Hierarchy].CURRENTMEMBER
                        ))
                         ,[Measures].[Amt Gross Profit]
                            )
                          MEMBER [Measures].[Current Gross Profit]
                        AS
 
                        AGGREGATE(
                        MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER
                         )
                         ,[Measures].[Amt Gross Profit]
                         ) 
                          MEMBER [Measures].[PreviousMonthSales]
                        AS
 
                        AGGREGATE(
                        MTD(PARALLELPERIOD(
                        [Date All Document].[Date Hierarchy].[Month]
                        ,1
                        ,[Date All Document].[Date Hierarchy].CURRENTMEMBER
                        ))
                         ,[Measures].[Amt Sales]
                         ) 
                          MEMBER [Measures].[CurrentMonthSales]
                        AS
 
                        AGGREGATE(
                        MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER
                         )
                         ,[Measures].[Amt Sales]
                         ) 
                        SELECT { [Measures].[CurrentMonthQuantitySold]
                        ,[Measures].[PreviousMonthQuantitySold]
                        ,[Measures].[CurrentMonthExpense]
                        ,[Measures].[PreviousMonthExpense]
                        ,[Measures].[Current Gross Profit]
                        ,[Measures].[Previous Gross Profit]
                        ,[Measures].[CurrentMonthSales]
                        ,[Measures].[PreviousMonthSales]
                        } ON 0,
                        NON EMPTY{ [Date All Document].[Month].[Month]


                        }ON 1
                        FROM  
                        (
                        SELECT [Date All Document].[Date Hierarchy].[Date].&[" + date + @"]

                          ON 0 FROM 
                        [" + MDXCubeQueries._cubeName + @"])
";

        }

        #endregion

        #region Expenses

        public static string ExpensesByCategoryTotalQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                                    AS AGGREGATE(MTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Month]
                                                    ,1 
                                                    ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                                    ,[Measures].[Amt Cost Of Sales]) 
                                    MEMBER [Measures].[CurrentValue]
                                    AS AGGREGATE(MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                                    ,[Measures].[Amt Cost Of Sales]) 
                                    SELECT {{ 
                                        [Measures].[CurrentValue]
                                        ,[Measures].[PreviousValue]
                                    }} ON 0,
                                    NON EMPTY{{
                                    ([Product].[Commodity Code].[Commodity Code]-[Product].[Commodity Code].&[])
                                    *[Date All Document].[Date Hierarchy].[Date]
                                    }}ON 1
                                    FROM  
                                    (
                                    SELECT [Date All Document].[Date Hierarchy].[Date].&[{0}]
                                     ON 0 FROM 
                                    [" + _cubeName + "])";
            }
        }

        public static string ExpensesByCategoryMonthlyQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                                    AS AGGREGATE(MTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Month]
                                                ,1
                                                ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                                ,[Measures].[Amt Cost Of Sales]) 
                                    MEMBER [Measures].[CurrentValue]
                                    AS AGGREGATE(MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                                ,[Measures].[Amt Cost Of Sales]) 
                                    SELECT {{ 
                                        [Measures].[CurrentValue]
                                        ,[Measures].[PreviousValue]
                                    }} ON 0,
                                    NON EMPTY{{
                                    [Specialty].[Specialty].[Specialty Name]*
                                    ([Product].[Commodity Code].[Commodity Code]-[Product].[Commodity Code].&[]{0})
                                    *[Date All Document].[Date Hierarchy].[Date]
                                    *[Date All Document].[Month].[Month]
                                    *[Salesperson].[Salesperson].[Salesperson Name] AS SalesPerson
                                    }}ON 1
                                    FROM  
                                    (
                                    SELECT [Date All Document].[Date Hierarchy].[Date].&[{1}]
                                     ON 0 FROM 
                                    [" + _cubeName + "])";
            }
        }

        public static string ExpensesByCategoryYearToDateQuery
        {
            get
            {
                return
                        @"WITH MEMBER [Measures].[PreviousValue]
                            AS AGGREGATE(YTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Year]
                                        ,1
                                        ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                        ,[Measures].[Amt Cost Of Sales]) 
                             MEMBER [Measures].[CurrentValue]
                            AS AGGREGATE(YTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                        ,[Measures].[Amt Cost Of Sales]) 
                              SELECT {{ 
                                 [Measures].[CurrentValue]
                                ,[Measures].[PreviousValue]
                            }} ON 0,
                            NON EMPTY({{
                            [Specialty].[Specialty].[Specialty Name]*
                            ([Product].[Commodity Code].[Commodity Code]-[Product].[Commodity Code].&[])*
                            [Date All Document].[Date Hierarchy].[Date]*
                            [Salesperson].[Salesperson].[Salesperson Name]
                            }})ON 1
                            FROM  
                            (SELECT [Date All Document].[Date Hierarchy].[Date].&[{0}]  
                             ON 0 FROM 
                            [" + _cubeName + "])";
            }
        }

        public static string ExpensesByCategoryYearToCustomQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                        AS AGGREGATE(YTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Year]
                                        ,1
                                        ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                        ,[Measures].[Amt Cost Of Sales]) 
                         MEMBER [Measures].[CurrentValue]
                        AS AGGREGATE(YTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                        ,[Measures].[Amt Cost Of Sales]) 
                        SELECT {{ [Measures].[CurrentValue]
                        , [Measures].[PreviousValue]
                        }} ON 0,
                        NON EMPTY({{
                        [Specialty].[Specialty].[Specialty Name]
                        *[Product].[Commodity Code].[Commodity Code]
                        *[Date All Document].[Month].[Month]
                        }})ON 1
                        FROM  
                        (
                        SELECT {{[Product].[Commodity Code].[Commodity Code] -[Product].[Commodity Code].&[]}} ON 0 FROM(
                        SELECT  [Date All Document].[Date Hierarchy].[Month].&[{0}]&[{1}]
                         ON 0 FROM 
                        [" + _cubeName + "]))";
            }
        }

        public static string ExpensesBySalesPersonMonthlyQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                            AS AGGREGATE(MTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Month]
                                            ,1
                                            ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                            ,[Measures].[Amt Cost Of Sales]) 
                            MEMBER [Measures].[CurrentValue]
                            AS AGGREGATE(MTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                        ,[Measures].[Amt Cost Of Sales]) 
                            SELECT {{ 
                                [Measures].[CurrentValue]
                                ,[Measures].[PreviousValue]
                            }} ON 0,
                            NON EMPTY{{
                            [Salesperson].[Salesperson].[Salesperson Name]
                            *[Customer].[Customer].[Customer Id Name]
                            *[Date All Document].[Date Hierarchy].[Date]
                            *[Date All Document].[Month].[Month]
                            }}ON 1
                            FROM  
                            (SELECT [Date All Document].[Date Hierarchy].[Date].&[{0}] ON 0 FROM [" + _cubeName + "])";
            }
        }

        public static string ExpensesBySalesPersonYearToDateQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                            AS AGGREGATE(YTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Year]
                                        ,1
                                        ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                        ,[Measures].[Amt Cost Of Sales]) 
                             MEMBER [Measures].[CurrentValue]
                            AS AGGREGATE(YTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                        ,[Measures].[Amt Cost Of Sales]) 
                            SELECT {{ 
                            [Measures].[CurrentValue]
                            ,[Measures].[PreviousValue]
                            }} ON 0,
                            NON EMPTY({{
                            [Salesperson].[Salesperson].[Salesperson Name]
                            *[Customer].[Customer].[Customer Id Name]
                            *[Date All Document].[Month].[Month]
                            *[Date All Document].[Date Hierarchy].[Date]
                            }})ON 1
                            FROM  
                            (SELECT [Date All Document].[Date Hierarchy].[Date].&[{0}]  ON 0 FROM [" + _cubeName + "])";
            }
        }

        public static string ExpensesBySalesPersonYearToCustomQuery
        {
            get
            {
                return @"WITH MEMBER [Measures].[PreviousValue]
                        AS AGGREGATE(YTD(PARALLELPERIOD([Date All Document].[Date Hierarchy].[Year]
                                        ,1
                                        ,[Date All Document].[Date Hierarchy].CURRENTMEMBER))
                                        ,[Measures].[Amt Cost Of Sales]) 
                         MEMBER [Measures].[CurrentValue]
                        AS AGGREGATE(YTD([Date All Document].[Date Hierarchy].CURRENTMEMBER)
                                        ,[Measures].[Amt Cost Of Sales]) 
                        SELECT {{ 
                            [Measures].[CurrentValue]
                            ,[Measures].[PreviousValue]
                        }} ON 0,
                        NON EMPTY({{
                        [Salesperson].[Salesperson].[Salesperson Name]
                        *[Customer].[Customer].[Customer Id Name]
                        *[Date All Document].[Month].[Month]
                        }})ON 1
                        FROM  
                        (
                        SELECT  [Date All Document].[Date Hierarchy].[Month].&[{0}]&[{1}] ON 0 FROM [" + _cubeName + "])";
            }
        }



       

        #endregion

        #region Profitability

        public static string ProfitabilityQuery
        {

            get
            {
                return @" WITH 
                            MEMBER [Profiability]
                            AS
                            [Measures].[Amt Sales]  - [Measures].[Amt Cost Of Sales]
                            SELECT {{  [Measures].[Profiability]
                            }} ON 0,
                            NON EMPTY{{
                            [Date All Document].[Date Hierarchy].[Date]
                            }}ON 1

                            FROM
                            (
                            SELECT 
                            {{[Date All Document].[Date Hierarchy].[Year].&[{0}]:
                            [Date All Document].[Date Hierarchy].[Year].&[{1}]}}
                            ON 0 FROM
                            [" + _cubeName + "])";
            }
        }

        #endregion

        #endregion


    }
}
