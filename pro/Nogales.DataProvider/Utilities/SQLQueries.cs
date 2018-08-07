using Nogales.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nogales.DataProvider.Utilities
{
    /// <summary>
    /// All T-Sql queries
    /// </summary>
    public static class SQLQueries
    {
        public static string GetARTransactionReport(string accountNumberStart
                                                    , string accountNumberEnd
                                                    , string startSession
                                                    , string endSession
                                                    , string startBatch
                                                    , string endBatch
                                                    , string startDate
                                                    , string endDate
                                                    , string customerNumber
                                                    , string arType)
        {
            var query = string.Format(GetARTransactionReportQuery
                                        , startDate
                                        , endDate
                                        , accountNumberStart
                                        , accountNumberEnd
                                        , startSession
                                        , endSession
                                        , startBatch
                                        , endBatch
                                        , arType
                                        , customerNumber
                                       );
            return query;
        }

        public static string GetExpenseDrillDownQuery(string accountNumberStart, string accountNumberEnd,
            string startDate, string endDate, string accountName)
        {
            return string.Format(GetExpenseDrillDownReportsQuery, accountNumberStart, accountNumberEnd,
            startDate, endDate, accountName);
        }

        public static string GetOPEXCOGSReportQuery(string startDate
                                                    , string endDate
                                                    , string previousStartDate
                                                    , string previousEndDate
                                                    , string AccountNumberStart
                                                    , string AccountNumberEnd
                                                    , string accountNumber
                                                    )
        {
            var query = string.Format(GetOPEXCOGSReportQueryString
                                        , startDate
                                        , endDate
                                        , previousStartDate
                                        , previousEndDate
                                        , AccountNumberStart
                                        , AccountNumberEnd
                                        , Constants.StartSession
                                        , Constants.EndSession
                                        , accountNumber);
            return query;
        }

        public static string GetAPJournalReport(string accountNumberStart, string accountNumberEnd
                                                , string startSession, string endSession
                                                , string startBatch, string endBatch
                                                , string startDate, string endDate
                                                , string vendorNumber
                                                , string secondStartSession
                                                , string secondEndSession
                                                , string invoiceNumber)
        {
            var query = string.Format(GetAccountPayablesReportsQuery
                                        , startDate
                                        , endDate
                                        , accountNumberStart
                                        , accountNumberEnd
                                        , startSession
                                        , endSession
                                        , startBatch
                                        , endBatch
                                        , vendorNumber
                                        , secondStartSession
                                        , secondEndSession
                                        , invoiceNumber);
            return query;
        }

        public static string GetOPEXCOGSChartQuerys(string startDate, string endDate, string previousStartDate, string previousEndDate)
        {
            var Query = string.Format(GetOPEXCOGSChartQueryString,
                                        startDate,
                                        endDate,
                                        Constants.CogsAccountStart,
                                        Constants.CogsAccountEnd,
                                        Constants.OpexAccountStart,
                                        Constants.OpexAccountEnd,
                                        previousStartDate,
                                        previousEndDate
                                       );
            return Query;
        }

        /// <summary>
        /// Generate the query to fetch data for the warehouse short report
        /// </summary>
        /// <param name="shipDate"> Date of shipment </param>
        /// <param name="routeNumber"> route number </param>
        /// <param name="buyerName"> buyer ids </param>
        /// <returns> query string for the short report </returns>
        public static string GetWarehouseShortReportQuery(string shipDate, string routeNumber, string buyerName)
        {
            var query = string.Format(WarehouseShortReportQuery
                                    , shipDate
                                    , routeNumber
                                    , buyerName);
            return query;
        }

        internal static string GetSalesQuery(string startDate, string endDate, bool syncValue)
        {
            var query = string.Format(GetSalesBySalesPersonCustomerQuery
                                    , startDate
                                    , endDate);
            return query;
        }

        public static string GetPickerProductivityReportQuery(string empId, string startDate, string endDate)
        {
            var query = string.Format(PickerProductivityReportQuery
                                    , empId
                                    , startDate
                                    , endDate);
            return query;
        }

        public static string GetTotalProductivityMonthtodate(string startDate, string endDate)
        {
            var query = string.Format(TotalProductivityMonthtodateQuery
                                    , startDate
                                    , endDate);
            return query;
        }

        public static string GetPickerProductivityChartQuery(string startDate, string endDate)
        {
            var query = string.Format(PickerProductivityChartDatetodateQuery
                                    , startDate
                                    , endDate);
            return query;
        }

        public static string GetPickerProductivityForecastQuery(int predictionDay, string startDate, string endDate)
        {
            var query = string.Format(PickerProductivityForecastQuery
                                    , predictionDay
                                    , startDate
                                    , endDate);
            return query;
        }

        /// <summary>
        /// Generate the query to fetch data for the warehouse short report
        /// </summary>
        /// <param name="shipDate">Date of shipment</param>
        /// <param name="routeNumber">route number</param>
        /// <param name="buyerName">buyer ids</param>
        /// <param name="selelectedIds"> Ids to filter </param>
        /// <returns> query string for the short report </returns>
        internal static string GetWarehouseShortReportFilterIdsQuery(string shipDate, string routeNumber, string buyerName, List<int> selelectedIds)
        {
            var query = string.Format(WarehouseShortReportWithFilterQuery
                                    , shipDate
                                    , routeNumber
                                    , buyerName
                                    , string.Join(",", selelectedIds));
            return query;
        }



        public static string GetMappedSalesPersonListQuery()
        {
            var query = string.Format(
                      GetMappedSalesPerson
                );
            return query;
        }

        public static string GetArchivedSalesPersonListQuery(string salesPersonCodes, string startDate, string endDate)
        {
            if (salesPersonCodes != "")
            {
                var query = string.Format(
                            GetArchivedSalesPersonSearchQuery
                            , salesPersonCodes
                            , startDate
                            , endDate
                            );
                return query;
            }
            else
            {
                var query = string.Format(
                            GetArchivedSalesPersonQuery
                            , salesPersonCodes
                            , startDate
                            , endDate
                            );
                return query;

            }
        }

        public static string AssignSalesPersonCodeQuery(SalesPersonMappingBM salesPersonMappingBM, int count)
        {
            var query = string.Format(
                            AssignSalesPersonCode
                            , salesPersonMappingBM.SalesPersonCode
                            , salesPersonMappingBM.SalesPersonDescription
                            , salesPersonMappingBM.AssignedPersonList[count].AssignedPersonCode
                            , salesPersonMappingBM.AssignedPersonList[count].AssignedDescription
                            , salesPersonMappingBM.AssignedPersonList[count].StartDate
                            , DBNull.Value);
            return query;
        }

        public static string UnAssignSalesPersonCodeQuery(string salesPersonCode, string assignSalesPersonCode, string endDate)
        {
            var query = string.Format(
                  UnAssignSalesPersonCode
                  , salesPersonCode
                  , assignSalesPersonCode
                  , endDate
                );
            return query;
        }


        /// <summary>
        /// Get Revenue Monthly Query
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        /// <param name="currentStart"></param>
        /// <param name="currentEnd"></param>
        /// <param name="previousMonthStart"></param>
        /// <param name="previousMonthEnd"></param>
        /// <param name="previousYearStart"></param>
        /// <param name="previousYearEnd"></param>
        /// <param name="previousMonthYearStart"></param>
        /// <param name="previousMonthYearEnd"></param>
        /// <returns></returns>
        public static string GetRevenueMonthlyQuery(string current, string previous, string currentStart, string currentEnd, string previousMonthStart, string previousMonthEnd
                                                                   , string previousYearStart, string previousYearEnd, string previousMonthYearStart, string previousMonthYearEnd)
        {
            var query =
                string.Format(SQLQueries.RevenueCurrentMonthQuery
                                , current, previous
                                , currentStart, currentEnd
                                , previousMonthStart, previousMonthEnd
                                , previousYearStart, previousYearEnd
                                , previousMonthYearStart, previousMonthYearEnd);

            return query;
        }

        public static string GetRevenueYearlyQuery(string current, string previous, string currentStart, string currentEnd, string previousYearStart, string previousYearEnd)
        {
            var query =
                string.Format(SQLQueries.RevenueCustomYearQuery
                                , current
                                , previous
                                , currentStart
                                , currentEnd
                                , previousYearStart
                                , previousYearEnd);

            return query;
        }

        /// <summary>
        /// Revenue custom year query
        /// </summary>
        internal static string RevenueCustomYearQuery
        {
            get
            {

                return @"WITH CTE_Revenue([Description], Category, InvoiceYear, SalesmanCode,Revenue)
                        AS
                        (
                        select  
                        ss.descrip
                        , c.speclty as category
                        , DATEPART(YYYY,t.invdte) as InvoiceYear
                        ,ISNULL(spm.assignedpersoncode, t.salesmn)
                        ,t.extprice as Revenue
                        from artran t
                        inner join arcust c on c.custno =t.custno AND c.custno <> 'ICADJ'
                        inner join icitem i on i.item = t.item
                        inner join NPIPROSYS.dbo.sycrlst ss on ss.chrvl = i.comcode AND ss.compid ='01' and ss.ruleid ='COMCODE'
                        left join dbo.SalesPersonMapping as spm on spm.salespersoncode COLLATE Latin1_General_CI_AS=t.salesmn 
                        AND 
                            (
                                t.invdte between spm.StartDate and ISNULL(spm.endDate,'{3}')
                                OR
                                t.invdte between spm.StartDate and ISNULL(spm.endDate,'{5}')
                            )
                        where t.arstat not in ('X','V') AND t.item <> ''   
                        AND 
                            (
                                (t.invdte between '{2}' AND '{3}')
                                OR
                                (t.invdte between '{4}' AND '{5}')
                            )
                        )
                        Select * from (
                        SELECT 
                        [Description] as Descrip, Category,InvoiceYear, SalesmanCode
                        , SUM(Revenue) as Revenue
                        FROM CTE_Revenue
                        GROUP BY InvoiceYear, Description, Category,SalesmanCode
                        ) as s 
                        PIVOT
                        (
                        SUM(Revenue) 
                        for InvoiceYear IN ([{0}],[{1}])
                        )as pvt";

            }
        }

        /// <summary>
        /// RevenueCurrent Month Query
        /// </summary>
        public static string RevenueCurrentMonthQuery
        {
            get
            {
                return @"WITH CTE_Revenue([Description], Category, InvoiceYear, InvoiceMonth,SalesmanCode,Revenue)
                        AS
                        (
                        
                        select  ss.descrip
                        ,c.speclty as category
                        ,DATEPART(YYYY,t.invdte) as InvoiceYear
                        ,DATEPART(MONTH,t.invdte) as InvoiceMonth                        
                        ,ISNULL(spm.assignedpersoncode, t.salesmn)                 
                        ,t.extprice as Revenue
                        from artran t
                        inner join arcust c on c.custno =t.custno AND c.custno <> 'ICADJ'
                        inner join icitem i on i.item = t.item
                        inner join NPIPROSYS.dbo.sycrlst ss on ss.chrvl = i.comcode AND ss.compid ='01' and ss.ruleid ='COMCODE'
						left join dbo.SalesPersonMapping as spm on spm.salespersoncode COLLATE Latin1_General_CI_AS=t.salesmn 
                        AND
							(
								 t.invdte between spm.StartDate and ISNULL(spm.endDate,'{3}')
								 OR
								 t.invdte between spm.StartDate and ISNULL(spm.endDate,'{5}')           
                                 OR
                                t.invdte between spm.StartDate and ISNULL(spm.endDate,'{7}')
								OR 
								t.invdte between spm.StartDate and ISNULL(spm.endDate,'{9}')
							)
                        where t.arstat not in ('X','V') AND t.item <> ''   
                        AND 
                        (
                            (t.invdte between '{2}' AND '{3}')
                            OR 
                            (t.invdte between '{4}' AND '{5}')
                            OR
                            (t.invdte between '{6}' AND '{7}')
                            OR 
                            (t.invdte between '{8}' AND '{9}')
                            )
                        )

                        Select * from (
                        SELECT 
                        [Description] as Descrip, Category,InvoiceYear,InvoiceMonth, SalesmanCode
                        , SUM(Revenue) as Revenue
                        FROM CTE_Revenue
                        GROUP BY InvoiceYear,InvoiceMonth, Description, Category,SalesmanCode
                        ) as s 
                        PIVOT
                        (
                        SUM(Revenue) 
                        for InvoiceMonth IN ([{0}],[{1}])
                        )as pvt";
            }
        }



        internal static string GetArchivedSalesPersonSearchQuery
        {
            get
            {
                return @" select * from [dbo].[SalesPersonMapping] where EndDate IS NOT NULL and SalesPersonCode IN ('{0}') 
												  and StartDate between '{1}' and '{2}'";
            }

        }

        internal static string GetArchivedSalesPersonQuery
        {
            get
            {
                return @" select * from [dbo].[SalesPersonMapping] where EndDate IS NOT NULL and  SalesPersonCode<>' '
												  and StartDate between '{1}' and '{2}'";
            }

        }

        internal static string UnAssignSalesPersonCode
        {
            get
            {
                return "update SalesPersonMapping set EndDate='{2}' where SalesPersonCode='{0}' and AssignedPersonCode='{1}'";
            }
        }

        internal static string AssignSalesPersonCode
        {
            get
            {
                return "insert into SalesPersonMapping values('{0}','{1}','{2}','{3}','{4}',NULL)";
            }
        }

        internal static string GetMappedSalesPerson
        {
            get
            {
                //return "select * from SalesPersonMapping ";
                return @"select sycrl.descrip as SalesPersonDescription,sycrl.chrvl as SalesPersonCode,salsprsn.Id,salsprsn.AssignedPersonCode as AssignedPersonCode,salsprsn.AssignedPersonDescription as AssignedPersonDescription,salsprsn.StartDate,salsprsn.EndDate
	                        from NPIPROSYS.[dbo].[sycrlst] AS sycrl LEFT JOIN NPICOMPANY01..[SalesPersonMapping] AS salsprsn ON sycrl.chrvl COLLATE Latin1_General_CI_AS=salsprsn.SalesPersonCode
			                    WHERE  (sycrl.ruleid = 'SLSPERS') AND (sycrl.compid = '01') --AND salsprsn.EndDate IS NULL
                                     AND  sycrl.descrip<>' '";

            }
        }

        /// <summary>
        /// "select * from SalesPersonTargets"
        /// </summary>
        internal static string GetAllSalesPersonTargets
        {
            get
            {
                return "select * from SalesPersonTargets";
            }
        }


        /// <summary>
        /// Get all sales persons and take only top some rows
        ///
        /// Query: select top {top} * from SalesPersonTargets
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        internal static string GetTopAllSalesPersonTargets(int top)
        {
            return string.Format("select top {0} * from SalesPersonTargets", top);
        }
        /// <summary>
        /// UPDATE SalesPersonTargets SET CustomerCount = {CustomerCountTarget}, Sales = {SalesTarget} WHERE SalesPerson = '{SalesPerson}'
        /// </summary>
        /// <param name="CustomerCountTarget"></param>
        /// <param name="SalesTarget"></param>
        /// <param name="SalesPerson"></param>
        /// <returns></returns>
        internal static string UpdateSalesPersonTarget(decimal CustomerCountTarget, decimal SalesTarget, string SalesPerson)
        {
            return string.Format(" UPDATE SalesPersonTargets SET CustomerCount = {0}, Sales = {1} WHERE SalesPerson = '{2}' ", CustomerCountTarget, SalesTarget, SalesPerson);
        }

        /// <summary>
        /// Insert into the table SalesPersonTargets
        ///
        /// Query: INSERT INTO SalesPersonTargets VALUES('salesPerson', {customerCountTarget}, {salesTarget}, CAST(Today AS date))
        /// </summary>
        /// <param name="salesPerson"></param>
        /// <param name="customerCountTarget"></param>
        /// <param name="salesTarget"></param>
        /// <returns></returns>
        internal static string InsertSalesPersonTarget(string salesPerson, int customerCountTarget, decimal salesTarget)
        {
            return string.Format(" INSERT INTO SalesPersonTargets VALUES('{0}', {1}, {2}, CAST('{3}' AS date)) ", salesPerson, customerCountTarget, salesTarget, DateTime.Today.ToString("yyyy-MM-dd"));
        }
        /// <summary>
        /// Remove sales persons from the table SalesPersonTargets
        ///
        /// Query : DELETE FROM SalesPersonTargets WHERE SalesPerson = '{salesPerson}'
        /// </summary>
        /// <param name="salesPerson"></param>
        /// <returns></returns>
        internal static string RemoveSalesPersonTarget(string salesPerson)
        {
            return string.Format(" DELETE FROM SalesPersonTargets WHERE SalesPerson = '{0}' ", salesPerson); ;
        }

        /// <summary>
        /// Update the column of SyncedOn of the table SalesPersonTarget
        ///
        /// Query: UPDATE SalesPersonTargets SET SyncedOn='{syncedOn}'
        /// </summary>
        /// <param name="syncedOn"></param>
        /// <returns></returns>
        internal static string UpdateAllSalesPersonTargetSyncedOn(DateTime syncedOn)
        {
            return string.Format(" UPDATE SalesPersonTargets SET SyncedOn='{0}' ", syncedOn.ToString("yyyy-MM-dd"));
        }


        private static string GetSalesBySalesPersonCustomerQuery
        {
            get
            {

                return @"SELECT  NPICOMPANY01..SalesPersonMapping.AssignedPersonDescription AS SalesPerson,
                			                DATENAME(MONTH,'{0}') AS MonthName,NPICOMPANY01..arcust.type,
                				                NPICOMPANY01..arcust.custno+' '+NPICOMPANY01..arcust.company AS Customer,
                					                SUM(NPICompany01.dbo.artran.extprice) AS AmtSales       
                                                        FROM artran 
                                                        INNER JOIN NPICOMPANY01..arcust ON NPICOMPANY01..artran.custno = NPICOMPANY01..arcust.custno
                                                        --INNER JOIN NPIPROSYS.dbo.sycrlst ON NPIPROSYS.dbo.sycrlst.chrvl=NPICOMPANY01..arcust.salesmn
                                                        INNER JOIN NPICOMPANY01..SalesPersonMapping ON  NPICOMPANY01..SalesPersonMapping.SalesPersonCode COLLATE Latin1_General_CI_AS=NPICOMPANY01..arcust.salesmn
                                                        WHERE NPICompany01.dbo.artran.custno <> 'ICADJ' 
                                                        AND NPICompany01.dbo.artran.item <> '' 
                                                        and NPICompany01.dbo.artran.arstat not in ('X','V')
                                                        AND NPICOMPANY01..artran.adddate between '{0}' AND '{1}'
                                                        AND NPICOMPANY01..SalesPersonMapping.AssignedPersonCode IS NOT NULL AND NPICOMPANY01..SalesPersonMapping.AssignedPersonDescription IS NOT NULL AND NPICOMPANY01..SalesPersonMapping.EndDate IS NULL
                                                        --AND NPICOMPANY01..arcust.company='PALETERIA LA AZTECA- OKLAHOMA'
                                                        --AND (NPIPROSYS.dbo.sycrlst.ruleid = 'SLSPERS') AND (NPIPROSYS.dbo.sycrlst.compid = '01')
                                                        --AND  NPIPROSYS.dbo.sycrlst.descrip<>' '  
                                                        --AND NPIPROSYS.dbo.sycrlst.descrip='BUYER'
                                                        GROUP BY NPICOMPANY01..arcust.custno,NPICOMPANY01..arcust.company,NPICOMPANY01..arcust.type,NPICOMPANY01..SalesPersonMapping.AssignedPersonDescription
                                                        ORDER BY SalesPerson,Customer";
            }

        }


        //internal static string InsertSOShortNotification(int Id, string route, string customer, string item, string descrip, string buyer, string umeasur, decimal qtyord, decimal tcost, decimal tprice, string sono, DateTime rqdate, string email, decimal qtyleft, decimal qtyneeded, bool notify=true,string type="Notified",string notes)
        //{
        //    return string.Format("INSERT INTO SOShortNotification VALUES('{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}=0,{15}=0,{16},{17}) ,select  route, customer, item, descrip, buyer, umeasur, qtyord, tcost, tprice, sono, rqdate, email, qtyleft, qtyneeded, notify,type,notes FROM (model)");
        //}
        //return string.Format("INSERT INTO SOShortExclution (id,route, customer, item, descrip, buyer, umeasur, qtyord, tcost, tprice, sono, rqdate, email, qtyleft, qtyneeded, notify,type,notes) select(Id, Route, Customer, Item, Description, Buyer, UOM, QuantityOrd, TransactionCost, MarketPrice, SalesOrderNumber, Date,Email,QuantityLeft,QuantityNeeded,NotifiedOrIgnored,Type,{1}) FROM ({0})");
        public static string InsertSOShortNotified(string id, string emailORIgnoredReason, string Date, string type)
        {
            var query = string.Format(InsertSOShortNotifiedQuery
                                    , id
                                    , emailORIgnoredReason
                                    , Date
                                    , type
                                     );
            return query;
        }

        private static string InsertSOShortNotifiedQuery
        {
            get
            {
                //                return @"INSERT INTO [dbo].[SOShortExclusion] 
                //                        SELECT   a1.[id_col], b1.route, c1.company,a1.item, a1.descrip, d1.buyer, a1.umeasur
                //                                , a1.qtyord, 0, 0, a1.tcost, a1.tprice, a1.sono , a1.rqdate, c1.email as Email, 'Notified', '{1}' 
                //                        FROM NPICOMPANY01..SOTRAN AS a1                       
                //                        INNER JOIN NPICOMPANY01..SOMAST AS b1 ON b1.sono = a1.sono 
                //                        INNER JOIN NPICOMPANY01..ARCUST AS c1 ON c1.custno = a1.custno 
                //                        INNER JOIN NPICOMPANY01..ICITEM AS d1 ON d1.item = a1.item 
                //                        WHERE      a1.rqdate = '{2}' AND a1.[id_col] in ({0})";


                return @" INSERT INTO [dbo].[SOShortExclusion] 
                        select 
                        sot.[id_col]
                        ,som.route
                        ,cu.company 
                        ,sot.item
                        , sot.descrip
                        ,itm.buyer
                        ,sot.umeasur
                        ,sot.origqtyord
                        , loc.lonhand
                        ,loc.lonhand - loc.lsoaloc
                        ,sot.tcost
                        ,sot.tprice
                        ,sot.sono
                        ,sot.rqdate
                        ,cu.email
                        ,'{3}'
                        ,'{1}'
                        
                        from sotran sot
                        inner join arcust cu on cu.custno = sot.custno
                        inner join somast som on som.sono = sot.sono
                        inner join icitem itm on itm.item = sot.item
                        inner join iciloc loc on loc.loctid = sot.loctid and loc.item = sot.item
                        where sot.rqdate ='{2}'
	                        AND sot.[id_col] IN ('{0}')";


            }
        }
        internal static string UpdateSOShortExclutionquery(int id, string notes)
        {

            return string.Format(" UPDATE SOShortExclution SET notes = {1} WHERE id = '{0}' ", id, notes);

        }



        public static string GetPickerForcastReportQuery(string startDate, string endDate)
        {
            var query = string.Format(PickerForcastReportQuery
                                    , startDate
                                    , endDate);
            return query;
        }

        public static string GetPickerForcastQuantityPickedReportQuery(string startDate, string endDate)
        {
            var query = string.Format(PickerForcastQuantityPickedReportQuery
                                    , startDate
                                    , endDate);
            return query;
        }

        /// <summary>
        /// Get Inventory Journal Report Query
        /// </summary>
        /// <param name="sessionRangeFrom"></param>
        /// <param name="sessionRangeTo"></param>
        /// <param name="rangeNumbers"></param>
        /// <param name="transactionDateFrom"></param>
        /// <returns></returns>
        public static string GetInventoryJournalReportQuery(string StartSession, string EndSession, string rangeNumbers, string StartDate, string EndDate)
        {
            var query = string.Format(InventoryJournalReportQuery
                                    , StartDate
                                    , EndDate
                                    , StartSession
                                    , EndSession
                                    , rangeNumbers);
            return query;
        }

        public static string GetCasesSoldBySalesPersonMonthlyQuery(DateTime Date)
        {
            DateTime currentStartDate = new DateTime(Date.Year, Date.Month, 1);
            DateTime currentEndDate = currentStartDate.AddMonths(1).AddDays(-1);

            DateTime previousStartDate = new DateTime(Date.Year, Date.AddMonths(-1).Month, 1);
            DateTime previousEndDate = previousStartDate.AddMonths(1).AddDays(-1);


            var query = string.Format(CasesSoldBySalesPersonMonthlyQuery
                                    , currentStartDate.ToString("yyyy-MM-dd 00:00:00")
                                    , currentEndDate.ToString("yyyy-MM-dd 23:59:59")
                                    , previousStartDate.ToString("yyyy-MM-dd 00:00:00")
                                    , previousEndDate.ToString("yyyy-MM-dd 23:59:59"));
            return query;
        }

        private static string CasesSoldBySalesPersonMonthlyQuery
        {
            get
            {
                //                return @"
                //                            WITH CTE_CasesSold_Current(invno,salesmn,Customer,YearStatus)
                //                            AS
                //                            (
                //	                            SELECT a1.invno,a1.salesmn,b1.company AS Customer
                //			                            ,CASE  WHEN a1.invdte BETWEEN '{0}' AND '{1}'  THEN 'Current' ELSE 'Previous' end as YearStatus			
                //			                            FROM NPICOMPANY01..armast AS a1
                //			                            INNER JOIN NPICOMPANY01..arcust AS b1 ON a1.custno=b1.custno
                //			                            WHERE a1.arstat<>'V'
                //			                            AND(a1.artype=' ' OR a1.artype='C' OR a1.artype='T')
                //			                            AND (a1.invdte BETWEEN '{0}' AND '{1}')OR(a1.invdte BETWEEN '{2}' AND '{3}')
                //                            ),
                //                            CTE_CasesSold_Main(salesmn,CurrentAmount,Customer,PreviousAmount,YearStatus)
                //                            AS
                //                            (
                //	                            SELECT casesCurrent.salesmn
                //			                            ,SUM(a1.extprice) AS CurrentAmount , casesCurrent.Customer AS Customer
                //			                            ,SUM(a1.extprice) AS PreviousAmount
                //			                            ,casesCurrent.YearStatus
                //			                             FROM NPICOMPANY01..artran AS a1
                //			                            LEFT JOIN NPICOMPANY01..icibin AS b1 ON b1.item=a1.item AND b1.binno=a1.binno
                //			                            INNER JOIN CTE_CasesSold_Current AS casesCurrent ON casesCurrent.invno=a1.invno
                //			                            WHERE a1.arstat=' '
                //			                            GROUP BY casesCurrent.salesmn
                //			                            ,casesCurrent.Customer
                //			                            ,casesCurrent.YearStatus
                //                            )
                //
                //                            SELECT salesmn AS SalesPerson,CurrentAmount,Customer,PreviousAmount,YearStatus FROM CTE_CasesSold_Main";

                return @"WITH CTE_CasesSold_Current(invno,salesmn,Customer,YearStatus)
                            AS
                            (
	                            SELECT a1.invno,a1.salesmn,b1.company AS Customer
			                            ,CASE  WHEN a1.invdte BETWEEN '2016-12-01 00:00:00' AND '2016-12-01 23:59:59'  THEN 'Current' ELSE 'Previous' end as YearStatus			
			                            FROM NPICOMPANY01..armast AS a1
			                            INNER JOIN NPICOMPANY01..arcust AS b1 ON a1.custno=b1.custno
			                            WHERE a1.arstat<>'V'
			                            AND(a1.artype=' ' OR a1.artype='C' OR a1.artype='T')
			                            AND (a1.invdte BETWEEN '2016-12-01 00:00:00' AND '2016-12-01 23:59:59')OR(a1.invdte BETWEEN '2015-12-01 00:00:00' AND '2015-12-01 23:59:59')
                            ),
                            CTE_CasesSold_Main(salesmn,CurrentAmount,Customer,PreviousAmount,YearStatus)
                            AS
                            (
	                            SELECT casesCurrent.salesmn
			                            ,SUM(a1.extprice) AS CurrentAmount , casesCurrent.Customer AS Customer
			                            ,SUM(a1.extprice) AS PreviousAmount
			                            ,casesCurrent.YearStatus
			                             FROM NPICOMPANY01..artran AS a1
			                            LEFT JOIN NPICOMPANY01..icibin AS b1 ON b1.item=a1.item AND b1.binno=a1.binno
			                            INNER JOIN CTE_CasesSold_Current AS casesCurrent ON casesCurrent.invno=a1.invno
			                            WHERE a1.arstat=' '
			                            GROUP BY casesCurrent.salesmn
			                            ,casesCurrent.Customer
			                            ,casesCurrent.YearStatus
                            )

                            SELECT salesmn AS SalesPerson,CurrentAmount,Customer,PreviousAmount,YearStatus FROM CTE_CasesSold_Main";


            }
        }

        private static string WarehouseShortReportQuery
        {
            get
            {
                //                return @"SELECT b1.route, c1.company as customer , a1.item,  a1.descrip as description, 
                //                        d1.buyer,a1.umeasur as UoM, a1.tcost as 'Trans.Cost',
                //                        a1.tprice as 'Mkt.Price', a1.sono as 'SO Num', a1.[id_col] as 'Id'
                //                        ,a1.rqdate, c1.email as Email ,Convert (Numeric(14,2),e1.ordqty) as QtyNeeded,Convert (Numeric(14,2),e1.lonhand)  as QuantityOnHand
                //                        FROM NPICOMPANY01..SOTRAN AS a1 
                //                            INNER JOIN NPICOMPANY01..SOMAST AS b1 ON b1.sono = a1.sono 
                //                            INNER JOIN NPICOMPANY01..ARCUST AS c1 ON c1.custno = a1.custno 
                //                            INNER JOIN NPICOMPANY01..ICITEM AS d1 ON d1.item = a1.item 
                //                            LEFT JOIN NPICOMPANY01..SOShortExclusion soe on soe.id = a1.id_col
                //							INNER JOIN iciloc e1 on e1.loctid=c1.loctid
                //                        WHERE 
                //                            a1.rqdate = '{0}' 
                //                            AND a1.sostat = '' 
                //                            AND (a1.shipped <> '' OR a1.caspo <> '' OR a1.issub = 1  OR a1.ishere = 1 ) 
                //                            AND ('{1}' ='' or( b1.route <>'' AND b1.route = '{1}'))
                //                            AND ('{2}' ='' OR(d1.buyer <>'' AND d1.buyer = '{2}' ))
                //                            AND soe.Id IS NULL
                //                        ORDER BY a1.descrip";
                return @"
                        select 
                        sot.sono as 'SO Num'
                        ,sot.custno , cu.company as customer, cu.email as email , itm.buyer,sot.tprice as 'Mkt.Price',sot.[id_col] as 'Id'
                        ,sot.item as item, sot.descrip as description, sot.umeasur as UoM
                        ,sot.tcost as 'Trans.Cost', sot.cost
                        ,sot.rqdate, som.route as route
                        ,sot.origqtyord as QtyOrdered--, sot.qtyord
                        , loc.lonhand QtyOnHand, loc.lonhand - loc.lsoaloc as QtyAvailable
                        from sotran sot
                        inner join arcust cu on cu.custno = sot.custno
                        inner join somast som on som.sono = sot.sono
                        inner join icitem itm on itm.item = sot.item
                        inner join iciloc loc on loc.loctid = sot.loctid and loc.item = sot.item
                        LEFT JOIN NPICOMPANY01..SOShortExclusion soe on soe.id = sot.id_col
                        where sot.rqdate ='{0}'
	                        AND sot.sostat = '' 
                            AND (sot.shipped <> '' OR sot.caspo <> '' OR sot.issub = 1  OR sot.ishere = 1 ) 
                            AND ('{1}' ='' or( som.route <>'' AND som.route = '{1}'))
                            AND ('{2}' ='' OR(itm.buyer <>'' AND itm.buyer =  '{2}' ))
                            AND soe.Id IS NULL
                        order by sot.descrip";

            }
        }

        private static string WarehouseShortReportWithFilterQuery
        {
            get
            {
                //                return @"
                //						SELECT b1.route, c1.company as customer , a1.item,  a1.descrip as description, 
                //                        d1.buyer,a1.umeasur as UoM, a1.tcost as 'Trans.Cost',
                //                        a1.tprice as 'Mkt.Price', a1.sono as 'SO Num', a1.[id_col] as 'Id'
                //                        ,a1.rqdate,c1.email as Email,Convert (Numeric(14,2),e1.ordqty) as QtyNeeded,
                //						Convert (Numeric(14,2),e1.lonhand)  as QuantityOnHand 
                //                        FROM NPICOMPANY01..SOTRAN AS a1 
                //                            INNER JOIN NPICOMPANY01..SOMAST AS b1 ON b1.sono = a1.sono 
                //                            INNER JOIN NPICOMPANY01..ARCUST AS c1 ON c1.custno = a1.custno 
                //                            INNER JOIN NPICOMPANY01..ICITEM AS d1 ON d1.item = a1.item 
                //                            LEFT JOIN NPICOMPANY01..SOShortExclusion soe on soe.id = a1.id_col
                //							INNER JOIN iciloc e1 on e1.loctid=c1.loctid
                //                        WHERE 
                //                            a1.rqdate = '{0}' 
                //                            AND a1.sostat = '' 
                //                            AND (a1.shipped <> '' OR a1.caspo <> '' OR a1.issub = 1  OR a1.ishere = 1 ) 
                //                            AND ('{1}' ='' or( b1.route <>'' AND b1.route = '{1}'))
                //                            AND ('{2}' ='' OR(d1.buyer <>'' AND d1.buyer = '{2}' ))
                //                            AND a1.[id_col] in ({3})
                //                            AND soe.Id IS NULL
                //                        ORDER BY a1.descrip";


                return @"select 
                        sot.sono as 'SO Num'
                        ,sot.custno , cu.company as customer, cu.email as email , itm.buyer,sot.tprice as 'Mkt.Price',sot.[id_col] as 'Id'
                        ,sot.item as item, sot.descrip as description, sot.umeasur as UoM
                        ,sot.tcost as 'Trans.Cost', sot.cost
                        ,sot.rqdate, som.route as route
                        ,sot.origqtyord as QtyOrdered--, sot.qtyord
                        , loc.lonhand QtyOnHand, loc.lonhand - loc.lsoaloc as QtyAvailable
                        from sotran sot
                        inner join arcust cu on cu.custno = sot.custno
                        inner join somast som on som.sono = sot.sono
                        inner join icitem itm on itm.item = sot.item
                        inner join iciloc loc on loc.loctid = sot.loctid and loc.item = sot.item
                        LEFT JOIN NPICOMPANY01..SOShortExclusion soe on soe.id = sot.id_col
                        where sot.rqdate ='{0}'
	                        AND sot.sostat = '' 
                                                    AND (sot.shipped <> '' OR sot.caspo <> '' OR sot.issub = 1  OR sot.ishere = 1 ) 
                                                    AND ('{1}' ='' or( som.route <>'' AND som.route = '{1}'))
                                                    AND ('{2}' ='' OR(itm.buyer <>'' AND itm.buyer = '{2}' ))
                                                    AND sot.[id_col] in ({3})
                                                    AND soe.Id IS NULL
                        order by sot.descrip";

            }
        }


        private static string PickerProductivityReportQuery
        {
            get
            {
                return @"

                        WITH CTE_SOTTRK(UserId, TaskNo, MinutesWorked)
                        AS
                        (
                         SELECT st.userid,st.taskno
                         ,SUM(datediff(MINUTE ,st.sdtime,st.edtime)) as MinutesWorked
                         FROM sottrk st 
                         WHERE st.sdtime between '{1}' AND '{2}'
                          AND st.edtime between '{1}' AND '{2}'
                          AND (st.userid = '{0}' or ''='{0}')
                         GROUP BY st.userid,st.taskno
                        )

                        ,CTE_Header(UserId, TaskNo, MinutesWorked, QtyPicked)
                        AS
                        (
                        SELECT st.UserId,  st.TaskNo, st.MinutesWorked, SUM(sh.tqtypicked) as QtyPicked
                        FROM CTE_SOTTRK st
                        INNER JOIN sowdtl sh on sh.taskno = st.TaskNo
                        GROUP BY st.UserId, st.TaskNo,st.MinutesWorked
                        )
                        SELECT h.UserId as EMPID
                        ,u.alphaname AS Name
                        ,cast(SUM(h.QtyPicked) as decimal(18,2)) as PiecePicked 
                        ,cast(SUM(h.MinutesWorked)/60.0 as decimal(18,2)) HoursWorked 
                        , CAST(SUM(h.QtyPicked)/(SUM(h.MinutesWorked)/60.0 ) as decimal(18,2)) as PiecesPerHour
                        FROM CTE_Header h
                        LEFT JOIN sowpkr u on u.userid = h.UserId AND u.loctid ='ONSITE'
                        GROUP BY h.UserId,u.alphaname
                        HAVING SUM(h.MinutesWorked)<>0
                        ORDER BY h.UserId";
            }
        }
        private static string TotalProductivityMonthtodateQuery
        {
            get
            {
                return @"

                           WITH CTE_SOTTRK(UserId, TaskNo, MinutesWorked)
                        AS
                        (
                         SELECT st.userid,st.taskno
                         ,SUM(datediff(MINUTE ,st.sdtime,st.edtime)) as MinutesWorked
                         FROM sottrk st 
                         WHERE st.sdtime between '{0}' AND '{1}' AND st.edtime between '{0}' AND '{1}'
                         GROUP BY st.userid,st.taskno
                        )

                        ,CTE_Header(UserId, TaskNo, MinutesWorked, QtyPicked)
                        AS
                        (
                        SELECT st.UserId,  st.TaskNo, st.MinutesWorked, SUM(sh.tqtypicked) as QtyPicked
                        FROM CTE_SOTTRK st
                        INNER JOIN sowdtl sh on sh.taskno = st.TaskNo
                        GROUP BY st.UserId, st.TaskNo,st.MinutesWorked
                        )
                        , CTE_Summary(UserId, PIecesPerHour)
                        AS
                        (
                        SELECT h.UserId
                        ,CAST(SUM(h.QtyPicked)/(SUM(h.MinutesWorked)/60.0 ) as decimal(18,2)) as PcsPerHour
                        FROM CTE_Header h
                        GROUP BY h.UserId
                        HAVING SUM(h.MinutesWorked)<>0
                        )

                        SELECT cast( SUM(s.PIecesPerHour)/COUNT(s.UserId) as decimal(18,2) )FROM CTE_Summary s";
            }
        }

        private static string PickerProductivityChartDatetodateQuery
        {
            get
            {
                return @"WITH CTE_SOTTRK(UserId, TaskNo, MinutesWorked,TaskDate)
                        AS
                        (
                         SELECT 
	                          st.userid,
	                          st.taskno
	                         ,SUM(datediff(MINUTE ,st.sdtime,st.edtime)) as MinutesWorked
	                         ,cast(st.sdtime as date) as TaskDate
                        FROM sottrk st 
                        WHERE st.sdtime between '{0}' AND '{1}' AND st.edtime between '{0}' AND '{1}'
                        GROUP BY st.userid,st.taskno,cast(st.sdtime as date)
                        )
                       
                        ,CTE_Header(MinutesWorked, QtyPicked,TaskDate)
                        AS
                        (
                        SELECT 
	                         st.MinutesWorked
	                        ,SUM(sh.tqtypicked) as QtyPicked
	                        ,st.TaskDate
                        FROM CTE_SOTTRK st
                        INNER JOIN sowdtl sh on sh.taskno = st.TaskNo
                        GROUP BY st.UserId, st.TaskNo,st.MinutesWorked,st.TaskDate
                        )
                        select cast(sum(QtyPicked) /(sum(MinutesWorked )/60.0)as decimal(18,2)) as AverageProductivity, TaskDate from  CTE_Header
                        WHERE  MinutesWorked>0
                        GROUP BY TaskDate";

            }
        }

        private static string PickerProductivityForecastQuery
        {
            get
            {
                return @"WITH CTE_SOTTRK(UserId, TaskNo, MinutesWorked,TaskDate)
                        AS
                        (
                         SELECT 
	                          st.userid,
	                          st.taskno
	                         ,SUM(datediff(MINUTE ,st.sdtime,st.edtime)) as MinutesWorked
	                         ,cast(st.sdtime as date) as TaskDate
                        FROM sottrk st 
                        WHERE st.sdtime between '{1}' AND '{2}' AND st.edtime between '{1}' AND '{2}'
						  and DATEPART(dw,st.sdtime) = {0}
                        GROUP BY st.userid,st.taskno,cast(st.sdtime as date)
                        )
                       
                        ,CTE_Header(MinutesWorked, QtyPicked,TaskDate)
                        AS
                        (
                        SELECT 
	                         st.MinutesWorked
	                        ,SUM(sh.tqtypicked) as QtyPicked
	                        ,st.TaskDate
                        FROM CTE_SOTTRK st
                        INNER JOIN sowdtl sh on sh.taskno = st.TaskNo
                        GROUP BY st.UserId, st.TaskNo,st.MinutesWorked,st.TaskDate
                        )
                        select cast(sum(QtyPicked) /(sum(MinutesWorked )/60.0)as decimal(18,2)) as AverageProductivity
						from  CTE_Header";

            }
        }
        public static string GetNotifiedIgnoredShortReportQuery(string shipDate, string routeNumber, string buyerId)
        {
            return string.Format(NotifiedIgnoredShortReportQuery, shipDate, routeNumber, buyerId);
        }


        private static string NotifiedIgnoredShortReportQuery
        {
            get
            {
                return @"SELECT *
                        FROM NPICOMPANY01..SOShortExclusion
                        WHERE rqdate = '{0}' 
                            AND ('{1}' ='' or( route <>'' AND route = '{1}'))
                            AND ('{2}' ='' OR(buyer <>'' AND buyer = '{2}' ))
                        ORDER BY description";
            }
        }


        public static string GetTopTenExpensesQuery(string startDate, string endDate)
        {
            var query = string.Format(TopTenExpensesQuery
                                              , startDate
                                              , endDate);
            return query;
        }


        internal static string TopTenExpensesQuery
        {
            get
            {
                return @"SELECT  TOP 10
                        ga.gldesc Description,SUM(a_apdist.amount) TotalExpense
                        FROM NPICOMPANY01..APDIST a_apdist 
                        INNER JOIN NPICOMPANY01..glacnt ga on ga.glacnt = a_apdist.glacnt
                        WHERE 
                        a_apdist.trandte BETWEEN '{0}' AND '{1}'
                        --AND  apsess BETWEEN  '000000' AND  '083154'
                        AND  apsess BETWEEN  '000000' AND  '999999'
                        GROUP BY ga.gldesc
                        order by TotalExpense desc";
            }
        }


        private static string PickerForcastReportQuery
        {
            get
            {
                return @"WITH CTE_SOTTRK(UserId, TaskNo, MinutesWorked,TaskDate)
                        AS
                        (
                         SELECT 
	                          st.userid,
	                          st.taskno
	                         ,SUM(datediff(MINUTE ,st.sdtime,st.edtime)) as MinutesWorked
	                         ,cast(st.sdtime as date) as TaskDate
                        FROM sottrk st 
                        WHERE st.sdtime between '{0}' AND '{1}' AND st.edtime between '{0}' AND '{1}'

                        GROUP BY st.userid,st.taskno,cast(st.sdtime as date)
                        )
                       
                        ,CTE_Header(MinutesWorked, QtyPicked,TaskDate)
                        AS
                        (
                        SELECT 
	                         st.MinutesWorked
	                        ,SUM(sh.tqtypicked) as QtyPicked
	                        ,st.TaskDate
                        FROM CTE_SOTTRK st
                        INNER JOIN sowdtl sh on sh.taskno = st.TaskNo
                        GROUP BY st.UserId, st.TaskNo,st.MinutesWorked,st.TaskDate
                        )
                        select cast(sum(QtyPicked) /(sum(MinutesWorked )/60.0)as decimal(18,2)) as AverageProductivity
						from  CTE_Header";

            }
        }

        private static string PickerForcastQuantityPickedReportQuery
        {
            get
            {
                //                return @"WITH CTE_SOTTRK(TaskNo)
                //                        AS
                //                        (
                //                         SELECT 
                //	                          st.taskno
                //                         FROM sottrk st 
                //                         WHERE st.sdtime between '{0}' AND '{1}' AND st.edtime between '{0}' AND '{1}'
                //                        GROUP BY st.taskno
                //                        )
                //                       
                //                        ,CTE_Header(itmdesc,QtyPicked)
                //                        AS
                //                        (
                //                        SELECT 
                //						    ic.itmdesc,
                //	                        SUM(sh.tqtypicked) as QtyPicked
                //                        FROM CTE_SOTTRK st
                //                        INNER JOIN sowdtl sh on sh.taskno = st.TaskNo 
                //						INNER JOIN icitem ic on sh.item=ic.item
                //                        GROUP BY st.TaskNo,ic.itmdesc
                //                        )
                //						select itmdesc, cast(sum(QtyPicked) as decimal(18,2)) as QtyPicked from  CTE_Header
                //						group by itmdesc";
                /*
                 WITH CTE_SOTTRK(TaskNumber,UserId,StartDate,SecondsTaken)
                         AS
                         (
	                        select  sottrk.taskno AS TaskNumber
									,sottrk.userid AS UserId
									,CAST(sottrk.sdtime AS DATE) AS StartDate
									--, CAST(sottrk.edtime AS DATETIME) AS EndDate
									,SUM(DATEDIFF(SECOND,sottrk.sdtime,sottrk.edtime))AS SecondsTaken
	                        FROM sottrk
			                        WHERE sottrk.sdtime>='11/01/2016 00:00:00' AND sottrk.edtime<='11/25/2016 23:59:59'
			                        GROUP BY sottrk.taskno,sottrk.userid,sottrk.sdtime--,sottrk.edtime
                         ),
                         CTE_SOWDTL(TaskNumber,UserId,StartDate,PiecesPicked,SecondsTaken,ItemId,Item)
                         AS
                         (
	                        select  CTE_SOTTRK.TaskNumber AS TaskNumber
	                        ,CTE_SOTTRK.UserId AS CTE_SOTTRK
	                        ,CTE_SOTTRK.StartDate AS StartDate
	                        ,SUM(tqtypicked) AS PiecesPicked
	                        ,SUM(CTE_SOTTRK.SecondsTaken) AS SecondsTaken
	                        ,icitem.item AS ItemId, icitem.itmdesc AS Item
	                        FROM CTE_SOTTRK
			                        INNER JOIN sowdtl ON CTE_SOTTRK.TaskNumber=sowdtl.taskno
			                        INNER JOIN icitem ON sowdtl.item=icitem.item
							GROUP BY CTE_SOTTRK.TaskNumber,CTE_SOTTRK.UserId,CTE_SOTTRK.StartDate,icitem.item,icitem.itmdesc
                         )
						--SELECT * FROM CTE_SOWDTL order by TaskNumber
                         SELECT CTE_SOWDTL.Item AS Name
                         , CAST(CAST(SUM(CTE_SOWDTL.PiecesPicked) as decimal(18,5)) as float) AS PiecesPicked,
		                        CAST(SUM(CTE_SOWDTL.SecondsTaken)/3600.00 as decimal(18,2)) AS HoursWorked
		                        ,CASE WHEN SUM(CTE_SOWDTL.PiecesPicked) <=0 THEN 0 ELSE
									CAST(CAST(SUM(PiecesPicked)/cast(SUM(SecondsTaken)/3600.0 as decimal(18,2)) AS DECIMAL(18,2)) AS FLOAT) END AS PiecesPerHour
                                FROM CTE_SOWDTL
	                            GROUP BY CTE_SOWDTL.Item
	                            --Order By (sum(SecondsTaken)/60.0)
                 */
                return @"WITH CTE_SOTTRK(TaskNumber,UserId,StartDate,SecondsTaken)
                         AS
                         (
	                        select  sottrk.taskno AS TaskNumber
                                    ,sottrk.userid AS UserId
                                    ,CAST(sottrk.sdtime AS DATE) AS StartDate
                                    ,SUM(DATEDIFF(SECOND,sottrk.sdtime,sottrk.edtime))AS SecondsTaken 
	                        FROM sottrk 		
			                        WHERE sottrk.sdtime>='{0}' AND sottrk.edtime<='{1}' 
			                        GROUP BY sottrk.taskno,sottrk.userid,sottrk.sdtime
                         ),
                         CTE_SOWDTL(TaskNumber,UserId,StartDate,PiecesPicked,SecondsTaken,ItemId,Item)
                         AS
                         (
	                        select  CTE_SOTTRK.TaskNumber AS TaskNumber,
	                        CTE_SOTTRK.UserId AS CTE_SOTTRK
                            ,CTE_SOTTRK.StartDate AS StartDate,SUM(tqtypicked) AS PiecesPicked
                            ,SUM(CTE_SOTTRK.SecondsTaken) AS SecondsTaken,
	                        icitem.item AS ItemId, icitem.itmdesc AS Item
	                        FROM CTE_SOTTRK
			                        INNER JOIN sowdtl ON CTE_SOTTRK.TaskNumber=sowdtl.taskno
			                        INNER JOIN icitem ON sowdtl.item=icitem.item			
			                        GROUP BY CTE_SOTTRK.TaskNumber,CTE_SOTTRK.UserId,CTE_SOTTRK.StartDate,icitem.item,icitem.itmdesc
                         )

                          SELECT CTE_SOWDTL.Item AS Name
                         , CAST(SUM(CTE_SOWDTL.PiecesPicked) as decimal(18,5))  AS PiecesPicked,
		                        CAST(SUM(CTE_SOWDTL.SecondsTaken)/3600.00 as decimal(18,2)) AS HoursWorked
		                        ,CASE WHEN SUM(CTE_SOWDTL.PiecesPicked) <=0 THEN 0 ELSE 
									CAST(SUM(PiecesPicked)/cast(SUM(SecondsTaken)/3600.0 as decimal(18,2)) AS DECIMAL(18,2)) END AS PiecesPerHour 
                                FROM CTE_SOWDTL
	                            GROUP BY CTE_SOWDTL.Item";
            }
        }

        private static string GetARTransactionReportQuery
        {
            get
            {
                return @"SELECT  
		                        a_ardist.glacnt as GLAccount
                                ,glacnt.gldesc as AccountName
		                        ,a_ardist.tranno as TransactionNumber
		                        ,a_ardist.trandte as TransactionDate
		                        ,a_armast.artype as ARType
		                        ,a_armast.custno as CustomerNumber
		                        ,a_armast.refno as ReferenceNumber
		                        ,a_ardist.arsess as ARSession
		                        ,a_ardist.glbatch as GLBatch, 
		                        a_ardist.amount as Amount
                        FROM NPICOMPANY01..ARDIST a_ardist  
                        INNER JOIN NPICOMPANY01..glacnt AS glacnt ON glacnt.glacnt=a_ardist.glacnt
                                left outer join NPICOMPANY01..ARMAST a_armast 
                        on a_ardist.tranno = a_armast.INVNO
						WHERE a_ardist.trandte BETWEEN '{0}' AND '{1}'
                               AND ('{2}' = '' OR (a_ardist.glacnt between '{2}' and '{3}'))
                               AND ('{4}' = '' OR ((a_ardist.arsess between '{4}' and '{5}') ))
                               AND ('{6}' = '' OR (a_ardist.glbatch between '{6}' and '{7}'))
                               AND ('{8}' = '' OR ('{8}' like '%'+ a_armast.artype +'%'))
                               AND ('{9}' = '' OR (a_armast.custno= '{9}'))";
            }
        }

        private static string GetAccountPayablesReportsQuery
        {
            get
            {
                return @"SELECT  
	                        a_apdist.glacnt as GLAccount  
                            ,glacnt.gldesc as AccountName                          
	                        ,a_apdist.trandte as TransactionDate
	                        ,a_apdist.vendno as VendorNumber
	                        ,a_apdist.invno as InvoiceNumber
	                        ,a_apdist.udref as ReferenceNumber
	                        ,a_apdist.apsess as APSession
	                        ,a_apdist.glbatch as GLBatch
	                        ,a_apdist.descrip as Description
	                        ,a_apdist.amount as Amount
                        FROM NPICOMPANY01..APDIST a_apdist 
                        INNER JOIN NPICOMPANY01..glacnt AS glacnt ON glacnt.glacnt=a_apdist.glacnt                       
                        WHERE  a_apdist.trandte BETWEEN '{0}' AND '{1}' 
                            AND ('{2}' = '' OR (a_apdist.glacnt between '{2}' and '{3}') OR (a_apdist.glacnt between '{9}' and '{10}'))
                            AND ('{4}' = '' OR (a_apdist.apsess between '{4}' and '{5}'))
                            AND ('{6}' = '' OR (a_apdist.glbatch between '{6}' and '{7}'))
                            AND ('{8}' = '' OR (a_apdist.vendno <> '' AND LTRIM(RTRIM(a_apdist.vendno)) = '{8}'))
                            AND ('{11}'='' OR (a_apdist.invno='{11}'))";
            }
        }

        private static string GetExpenseDrillDownReportsQuery
        {
            get
            {
                return @"SELECT  
	                        a_apdist.glacnt as GLAccount  
                            ,glacnt.gldesc as AccountName                          
	                        ,a_apdist.trandte as TransactionDate
	                        ,a_apdist.vendno as VendorNumber
	                        ,a_apdist.invno as InvoiceNumber
	                        ,a_apdist.udref as ReferenceNumber
	                        ,a_apdist.apsess as APSession
	                        ,a_apdist.glbatch as GLBatch
	                        ,a_apdist.descrip as Description
	                        ,a_apdist.amount as Amount
                        FROM NPICOMPANY01..APDIST a_apdist 
                        INNER JOIN NPICOMPANY01..glacnt AS glacnt ON glacnt.glacnt=a_apdist.glacnt
                WHERE  a_apdist.trandte BETWEEN '{2}' AND '{3}' 
                AND (a_apdist.glacnt between '{0}' and '{1}')
                AND ('{4}' = '' OR glacnt.gldesc = {4})";
            }
        }

        private static string GetOPEXCOGSReportQueryString
        {
            get
            {
                //                return @"SELECT  
                //	                        a_apdist.glacnt as GLAccount  
                //                            ,glacnt.gldesc as AccountName                          
                //	                        ,DATEPART(yyyy,a_apdist.trandte) as TransactionDate              
                //	                        ,a_apdist.amount as Amount
                //							,CASE WHEN a_apdist.trandte BETWEEN '{0}' AND '{1}' THEN 'Current' ELSE 'Previous' END AS CurrentPrevious
                //                        FROM NPICOMPANY01..APDIST a_apdist 
                //                        INNER JOIN NPICOMPANY01..glacnt AS glacnt ON glacnt.glacnt=a_apdist.glacnt                       
                //                        WHERE (( a_apdist.trandte BETWEEN '{0}' AND '{1}') OR (a_apdist.trandte BETWEEN '{2}' AND '{3}'))
                //                            AND ('{4}' = '' OR (a_apdist.glacnt between '{4}' and '{5}'))
                //                            AND ('{6}' = '' OR (a_apdist.apsess between '{6}' and '{7}'))
                //                            AND a_apdist.glacnt='{8}'";


                return @"
                        SELECT     
			            a_apdist.invno as InvoiceNumber                         
			            ,CAST(a_apdist.trandte AS DATE) as TransactionDate              
			            ,a_apdist.amount as Amount						
			            FROM NPICOMPANY01..APDIST a_apdist 
			            INNER JOIN NPICOMPANY01..glacnt AS glacnt ON glacnt.glacnt=a_apdist.glacnt                       
			            WHERE (( a_apdist.trandte BETWEEN '{0}' AND '{1}'))
			            AND ('{4}' = '' OR (a_apdist.glacnt between '{4}' and '{5}'))
			            AND ('{6}' = '' OR (a_apdist.apsess between '{6}' and '{7}'))
			            AND a_apdist.glacnt='{8}' order by  a_apdist.trandte";


            }
        }


        private static string GetOPEXCOGSChartQueryString
        {
            get
            {

                return @"
                        SELECT                  
                         glacnt.gldesc as AccountName
                        ,glacnt.glacnt as AccountNumber             
                        ,sum(a_apdist.amount) as Amount                        	
                        ,case WHEN a_apdist.glacnt BETWEEN '{2}' AND '{3}' THEN 'COGS' ELSE 'OPEX' end as AccountType	
                        ,CASE WHEN a_apdist.trandte BETWEEN '{0}' AND '{1}' THEN 'Current' ELSE 'Previous' end as Curprev	                        	
                        FROM NPICOMPANY01..APDIST a_apdist 
                        INNER JOIN NPICOMPANY01..glacnt AS glacnt ON glacnt.glacnt=a_apdist.glacnt                       
                        WHERE ((a_apdist.trandte BETWEEN '{0}' AND '{1}')OR(a_apdist.trandte BETWEEN '{6}' AND '{7}')) AND a_apdist.amount > 0
                        AND ((a_apdist.glacnt BETWEEN '{2}' AND '{3}') OR (a_apdist.glacnt BETWEEN '{4}' AND '{5}')) 
                        GROUP BY a_apdist.glacnt,glacnt.gldesc,a_apdist.trandte,glacnt.glacnt
                        ORDER BY Amount DESC";
            }
        }

        private static string InventoryJournalReportQuery
        {
            get
            {
                return @"SELECT 
                                a_icdist.glacnt AS Account,glacnt.gldesc AS Description,SUM( a_icdist.famount) AS Amount 
	                            FROM NPICOMPANY01..ICDIST a_icdist
	                            INNER JOIN glacnt ON a_icdist.glacnt=glacnt.glacnt
	                            WHERE trandte BETWEEN '{0}' AND '{1}' 
                                AND icsess BETWEEN  '{2}'  AND '{3}'
	                            AND NOT '{4}' LIKE '%' + icsess + '%' 
                                GROUP BY a_icdist.glacnt,glacnt.gldesc order by a_icdist.glacnt";
            }
        }


        public static string GetAllSalesPerson
        {
            get
            {


                //                //for server database use query
                //                return @"  select distinct NPICOMPANY01..arcust.salesmn,NPIPROSYS.dbo.sycrlst.descrip AS SalesPerson from NPICOMPANY01..arcust
                //                                                           INNER JOIN NPIPROSYS.dbo.sycrlst --ON NPICOMPANY01..sycrlst.chrvl=NPICOMPANY01..arcust.salesmn                             
                //                										   on NPIPROSYS.dbo.sycrlst.chrvl COLLATE Latin1_General_CI_AS  = NPICOMPANY01..arcust.salesmn 
                //                                                             where NPICOMPANY01..arcust.salesmn<>'' 
                //                                                                AND (NPIPROSYS.dbo.sycrlst.ruleid = 'SLSPERS') AND (NPIPROSYS.dbo.sycrlst.compid = '01')
                //                                                                   AND  NPIPROSYS.dbo.sycrlst.descrip<>' '";

                return @"Select SalesPersonCode as salesmn,SalesPersonDescription as SalesPerson from SalesPersonMapping";


            }
        }

        #region SalesReport

        internal static string GetSalesReportOfSalesPersonQuery(List<string> salesPerson, string startDate, string endDate, string startDatePrev, string endDatePrev)
        {
            var query = string.Format(GetSalesReportOfSalesPerson
                                    , string.Join("','", salesPerson)
                                    , startDate
                                    , endDate
                                    , startDatePrev
                                    , endDatePrev);
            return query;
        }

        private static string GetSalesReportOfSalesPerson
        {
            get
            {

                return @"
                        IF OBJECT_ID('tempdb..#temp') IS NOT NULL
                            DROP TABLE #temp
                        SELECT 
                        ISNULL(sm.assignedpersoncode, at.salesmn) SalesPersonCode
                        ,at.extprice salesAmt,am.custno Customer
                        ,case WHEN at.invdte BETWEEN '{1}' AND '{2}' THEN 'Current' ELSE 'Previous' END As Term
                        ,at.salesmn
                        into #temp
                        FROM artran at 
                        INNER JOIN armast am ON am.invno = at.invno
                        LEFT JOIN salespersonmapping sm ON sm.salespersoncode COLLATE LATIN1_GENERAL_CI_AS =at.salesmn 
                        AND (at.invdte between sm.StartDate AND ISNULL(sm.endDate,'{2}') OR at.invdte between sm.StartDate AND ISNULL(sm.endDate,'{4}'))
                        WHERE 
                        (at.invdte BETWEEN '{1}' AND '{2}' OR  at.invdte BETWEEN '{3}'  AND '{4}')
                        AND at.salesmn <>'' AND ((ISNULL(sm.assignedpersoncode, at.salesmn) IN('{0}')) OR '{0}'='')

                        Select 
                        tt.SalesPersonCode as SalesPersonCode
                        , isnull(sm.SalesPersonDescription,'') as SalesPersonDescription
                        , sm.SalesPersonCode AssignedPersonCode
                        , Max([1]) as CustomerCount
                        , SUM([Current]) as TotalSalesAmount
                        , ISNULL(SUM([Previous]),0) as TotalSalesAmountPrev
                        , CASE WHEN SUM([Current]) + SUM([Previous]) = 0 THEN 0 ELSE ((SUM([Current]) - SUM([Previous])) / ((SUM([Current]) +SUM([Previous]) /2 ))) * 100 END AS PercentageDifference
 
                        FROM
                        ( SELECT * FROM (
                        SELECT cs.SalesPersonCode, SUM(cs.salesAmt) SalesAmount, isnull((select top 1 COUNT( distinct Customer) from #temp where  Term='Current' and SalesPersonCode=cs.SalesPersonCode),0) CustomerCount, cs.Term
                        ,ROW_NUMBER() over (PARTITION BY cs.SalesPersonCode order by cs.Term) as rowNum
                        FROM #temp cs
                        GROUP BY cs.SalesPersonCode, cs.Term
                        )as pivoted
                        PIVOT  (SUM(SalesAmount) FOR Term IN ([Current],[Previous]))AS pvt1
                        PIVOT  (SUM(CustomerCount) FOR rowNum IN ([1]))AS pvt2
                        )AS tt
                        left JOIN SalesPersonMapping sm on sm.SalesPersonCode = tt.SalesPersonCode or sm.AssignedPersonCode = tt.SalesPersonCode
                        GROUP BY tt.SalesPersonCode, sm.SalesPersonDescription, sm.SalesPersonCode";

            }
        }

        #endregion

        #region casesSoldAndSalesTotalForDashboard

        public static string GetDashboardTotalTopBoxQuery(string startDate, string endDate, string previousStart, string previousEnd)
        {
            var query = string.Format(
                      GetDashboardTotalQueryString
                      , startDate
                      , endDate
                      , previousStart
                      , previousEnd
                );
            return query;
        }
        private static string GetDashboardTotalQueryString
        {
            get
            {

                return @" WITH CTE_Dashboard_Total(InvoiceYear, CasesSold,SalesAmount,GrossProfitAmt)
                        AS (
                        select 
                        DATEPART(MM,t.invdte) as InvoiceYear
                        ,SUM(CAST(ROUND(t.qtyshp, 0) AS INT)) as casesSold
                        ,SUM(t.extprice) as SalesAmount
                        ,SUM(t.extprice - t.qtyshp * t.cost) AS grossProfitAmt
                        from artran t
                        inner join icitem i on i.item = t.item
                        inner join NPIPROSYS.dbo.sycrlst ss on ss.chrvl = i.comcode AND ss.compid ='01' and ss.ruleid ='COMCODE'
						INNER JOIN arcust c on c.custno =t.custno AND c.custno <> 'ICADJ'
                        left join dbo.SalesPersonMapping as spm on spm.SalesPersonCode COLLATE Latin1_General_CI_AS=t.salesmn 
                        AND 
                        (
                        t.invdte between spm.StartDate and ISNULL(spm.endDate,'{1}')
                        OR
                        t.invdte between spm.StartDate and ISNULL(spm.endDate,'{3}')
                        )
                        where t.arstat not in ('X','V') AND t.item <> ''   AND t.salesmn<>''
                        AND 
                        (
                        (t.invdte between '{0}' AND '{1}')
                        OR
                        (t.invdte between '{2}' AND '{3}')
                        )
                        GROUP BY DATEPART(MM,t.invdte)
                        )
                        SELECT * FROM CTE_Dashboard_Total
                        ORDER BY InvoiceYear desc";
            }
        }

        #endregion

        #region ExpenseTotalForDashboard

        public static string GetExpenseTotalTopBoxQuery(string startDate, string endDate, string previousStart, string previousEnd)
        {
            var query = string.Format(
                    GetExpenseTotalQueryString
                    , startDate
                    , endDate
                    , previousStart
                    , previousEnd
                    , Constants.CogsAccountStart
                    , Constants.CogsAccountEnd
                    , Constants.OpexAccountStart
                    , Constants.OpexAccountEnd
                    );
            return query;
        }
        private static string GetExpenseTotalQueryString
        {
            get
            {

                return @"
                        SELECT     
			             DATEPART(MM,a_apdist.trandte) as InvoiceMonth 
			            ,SUM(a_apdist.amount) as Amount						
			            FROM NPICOMPANY01..APDIST a_apdist 
			            INNER JOIN NPICOMPANY01..glacnt AS glacnt ON glacnt.glacnt=a_apdist.glacnt                       
			            WHERE (( a_apdist.trandte BETWEEN '{0}' AND '{1}') OR (a_apdist.trandte BETWEEN '{2}' AND '{3}'))
			            AND ((a_apdist.glacnt BETWEEN '{4}' AND '{5}') OR (a_apdist.glacnt BETWEEN '{6}' AND '{7}')) 			            		            
                        GROUP BY   DATEPART(MM,a_apdist.trandte)";

            }
        }


        #endregion

        #region RevenueNonComodity

        public static string GetRevenueNonComodity(string date)
        {
            var query = string.Format(
                    GetRevenueNonComodityQueryString
                    , date
                                      );
            return query;
        }
        private static string GetRevenueNonComodityQueryString
        {
            get
            {
                return @"
                        declare @current_date datetime ='{0}'
                        declare @curr_Month_strt_date datetime = (SELECT DATEADD(month, DATEDIFF(month, 0, @current_date), 0))
                        declare @last_month_curr_date datetime=(SELECT DateAdd(month, -1, Convert(date, @current_date)))
                        declare @last_month_strt_date datetime=(SELECT DATEADD(month, DATEDIFF(month, 0, @last_month_curr_date), 0))
                        IF OBJECT_ID('tempdb..#temp') IS NOT NULL
                            DROP TABLE #temp
                        IF OBJECT_ID('tempdb..#revenue') IS NOT NULL
                            DROP TABLE #revenue
                        ;with CTE as(
                        select at.invno, c3.descrip typ,e5.descrip typ2,f6.descrip typ3,ss.descrip typ4
                        , spm.Speciality spec
                        ,ISNULL(spm.AssignedPersonCode,at.salesmn) salesman
                        ,at.invdte
                        ,i.item
                        ,datepart(mm,at.invdte) monthPart
                        ,case when datepart(yy,at.invdte)=datepart(yy,getdate()) then 'current' when datepart(yy,at.invdte)=(datepart(yy,getdate())-1) then 'previous' end as yearPart
                        ,at.custno
                        ,ac.company
                        ,CAST(ROUND(at.qtyshp, 0) AS INT) qty
                        ,at.extprice amt
                        ,case 
                        when at.invdte BETWEEN @curr_Month_strt_date AND @current_date then 'value1'
                        when at.invdte BETWEEN @last_month_strt_date AND @last_month_curr_date then 'value2'
                        end as condition
                        from artran at
                        INNER JOIN arcust ac on ac.custno = at.custno AND ac.custno <> 'ICADJ'
                        INNER JOIN icitem i ON i.item = at.item 
                        left JOIN npiprosys.dbo.sycrlst ss ON ss.chrvl = i.comcode AND ss.compid = '01' AND ss.ruleid='COMCODE'
                        LEFT JOIN NPIPROSYS..SYCRLST AS c3 ON c3.ruleid = 'SLSPERS ' AND c3.compid = '01        ' AND c3.chrvl = at.salesmn 
                        LEFT JOIN NPICOMPANY01..ARADDR AS d4 ON d4.invno = at.invno 
                        LEFT JOIN NPIPROSYS..SYCRLST AS e5 ON e5.ruleid = 'SOURCE  ' AND e5.compid = '01        ' AND e5.chrvl = ac.source
                        LEFT JOIN NPIPROSYS..SYCRLST AS f6 ON f6.ruleid = 'TYPE    ' AND f6.compid = '01        ' AND f6.chrvl = ac.type 
                        LEFT JOIN NPICOMPANY01..ICIBIN AS d6 ON d6.item = at.item AND d6.binno = at.binno
                        inner JOIN dbo.SalesPersonMapping as spm on (spm.AssignedPersonCode COLLATE Latin1_General_CI_AS=at.salesmn 
                        or spm.SalesPersonCode COLLATE Latin1_General_CI_AS=at.salesmn
                        )
                        AND
                        (
                        at.invdte between spm.StartDate and ISNULL(spm.endDate,@current_date)
                        OR
                        at.invdte between spm.StartDate and ISNULL(spm.endDate,@last_month_curr_date)           
                        )
                        where at.arstat not in ('X','V') AND at.item <> ''   
                        AND
                        ltrim(rtrim(ac.speclty)) not in('OOT','LOSS PROD')
                        AND 
                        (at.invdte BETWEEN @curr_Month_strt_date AND @current_date
                        OR
                        at.invdte BETWEEN @last_month_strt_date and  @last_month_curr_date
                        ) and at.salesmn!=''
                        )
                       select * into #temp from CTE where  typ4 is null
                       select typ comtype,max(spec) speciality,sum(qty) ItemShipped,sum(amt) revenue ,
                       max(salesman) Salesman,max(condition) Value,monthPart,max(yearPart) yearPart 
                       into #revenue
                       from #temp 
                       group by yearPart,monthPart,salesman,spec,typ
                       select a.monthPart,
                       SUM( ISNULL( a.revenue,0)) revenueCurrentYear
                       ,sum(isnull(a.ItemShipped,0)) casesCurrentYear
                       from #revenue a 
                       group by a.monthPart
                        ";
            }
        }


        #endregion


        #region Expense

        public static string GetExpenseByCategoryQuery(string current, string previous, string startDate, string endDate, string previousStart, string previousEnd)
        {
            var query = string.Format(
                    GetExpenseByCategoryMonthQueryString
                    , current
                    , previous
                    , startDate
                    , endDate
                    , previousStart
                    , previousEnd
                    );
            return query;
        }
        private static string GetExpenseByCategoryMonthQueryString
        {
            get
            {

                return @"
                        WITH CTE_EXPENSE_CATEGORY([description],Category, InvoiceYear, InvoiceMonth,CostOfSalesAmt)
                        AS
                        (
							SELECT 
							ss.descrip						
							, c.speclty as category
							, DATEPART(YYYY,t.invdte) as InvoiceYear
							, DATEPART(MONTH,t.invdte) as InvoiceMonth							
							, t.qtyshp * t.cost AS costOfSalesAmt                       
                          
							FROM artran t
							INNER JOIN arcust c on c.custno =t.custno AND c.custno <> 'ICADJ'
							INNER JOIN icitem i on i.item = t.item
							INNER JOIN NPIPROSYS.dbo.sycrlst ss on ss.chrvl = i.comcode AND ss.compid ='01' and ss.ruleid ='COMCODE'
														
							where t.arstat not in ('X','V') AND t.item <> ''   
							AND 
							(
								(t.invdte between '{2}' AND '{3}')
								OR 
								(t.invdte between '{4}' AND '{5}')                            
							)
                        )
						
                        Select [description] as Comodity, Category,InvoiceYear,[{1}] as previousAmt,[{0}] as currentAmt from (
                        SELECT 
                        [description], Category,InvoiceYear,InvoiceMonth
                        , SUM(CostOfSalesAmt) as CostOfSalesAmt
                        FROM CTE_EXPENSE_CATEGORY
                        GROUP BY InvoiceYear,InvoiceMonth, Description, Category
                        ) as s 

                        PIVOT
                        (
                        SUM(CostOfSalesAmt) 
                        for InvoiceMonth IN ([{0}],[{1}])
                        )as pvt						
                        ";

            }
        }

        public static string GetExpenseByCategoryYearQuery(string current, string previous, string startDate, string endDate, string previousStart, string previousEnd)
        {
            var query = string.Format(
                    GetExpenseByCategoryYearQueryString
                    , current
                    , previous
                    , startDate
                    , endDate
                    , previousStart
                    , previousEnd
                    );
            return query;
        }
        public static string GetExpenseByCategoryYearQueryString
        {
            get
            {
                return @"
                        WITH CTE_EXPENSE_CATEGORY_BY_YEAR([description],Category, InvoiceYear, CostOfSalesAmt)
                        AS
                        (
							SELECT 
							ss.descrip						
							, c.speclty as category
							, DATEPART(YYYY,t.invdte) as InvoiceYear													
							, t.qtyshp * t.cost AS costOfSalesAmt                       
                          
							FROM artran t
							INNER JOIN arcust c on c.custno =t.custno AND c.custno <> 'ICADJ'
							INNER JOIN icitem i on i.item = t.item
							INNER JOIN NPIPROSYS.dbo.sycrlst ss on ss.chrvl = i.comcode AND ss.compid ='01' and ss.ruleid ='COMCODE'
														
							where t.arstat not in ('X','V') AND t.item <> ''   
							AND 
							(
								(t.invdte between '{2}' AND '{3}')
								OR 
								(t.invdte between '{4}' AND '{5}')                                 
							)
                        )
						
                        Select [description] as Comodity, Category,[{1}] as previousAmt,[{0}] as currentAmt from (
                        SELECT 
                        [description], Category,InvoiceYear
                        , SUM(CostOfSalesAmt) as CostOfSalesAmt
                        FROM CTE_EXPENSE_CATEGORY_BY_YEAR
                        GROUP BY InvoiceYear, Description, Category
                        ) as s 

                        PIVOT
                        (
                        SUM(CostOfSalesAmt) 
                        for InvoiceYear IN ([{0}],[{1}])
                        )as pvt";
            }
        }

        #endregion

        #region Cases Sold

        #region Cases Sold Monthly

        public static string GetCasesSoldMonthlyQuery(string current, string previous, string currentStart, string currentEnd, string previousMonthStart, string previousMonthEnd, string previousYearStart, string previousYearEnd, string previousMonthYearStart, string previousMonthYearEnd)
        {
            var query =
                string.Format(SQLQueries.CasesSoldCurrentMonthQuery
                                , current, previous
                                , currentStart, currentEnd
                                , previousMonthStart, previousMonthEnd
                                , previousYearStart, previousYearEnd
                                , previousMonthYearStart, previousMonthYearEnd);

            return query;
        }

        internal static string CasesSoldCurrentMonthQuery
        {
            get
            {
                return @"WITH CTE_CASESSOLD([Description], Category, InvoiceYear, InvoiceMonth,SalesmanCode,CasesSold,SalesPersonCode,SalesPersonDescription)
                        AS
                        (
							SELECT  
							ss.descrip
							, c.speclty as category
							, DATEPART(YYYY,t.invdte) as InvoiceYear
							, DATEPART(MONTH,t.invdte) as InvoiceMonth
							, ISNULL(spm.AssignedPersonCode,t.salesmn)
							, CAST(ROUND(t.qtyshp, 0) AS INT) as casesSold                           
                            , ISNULL(spm.SalesPersonCode,t.salesmn) as SalesPersonCode
                            , spm.SalesPersonDescription as SalesPersonDescription
                            --, spm.SalesPersonDescription
							FROM artran t
							INNER JOIN arcust c on c.custno =t.custno AND c.custno <> 'ICADJ'
							INNER JOIN icitem i on i.item = t.item
							INNER JOIN NPIPROSYS.dbo.sycrlst ss on ss.chrvl = i.comcode AND ss.compid ='01' and ss.ruleid ='COMCODE'
							LEFT JOIN dbo.SalesPersonMapping as spm on spm.SalesPersonCode COLLATE Latin1_General_CI_AS=t.salesmn 
							AND
							(
								 t.invdte between spm.StartDate and ISNULL(spm.endDate,'{3}')
								 OR
								 t.invdte between spm.StartDate and ISNULL(spm.endDate,'{5}')           
                                 OR
                                t.invdte between spm.StartDate and ISNULL(spm.endDate,'{7}')
								OR 
								t.invdte between spm.StartDate and ISNULL(spm.endDate,'{9}')
							)
							where t.arstat not in ('X','V') AND t.item <> ''   
							AND 
							(
								(t.invdte between '{2}' AND '{3}')
								OR 
								(t.invdte between '{4}' AND '{5}')
                                OR
                                (t.invdte between '{6}' AND '{7}')
								OR 
								(t.invdte between '{8}' AND '{9}')
							)
                        )
                        Select pvt.*,sm.SalesPersonCode as code,sm.SalesPersonDescription as descr from (
                        SELECT 
                        [Description] as Descrip, Category,InvoiceYear,InvoiceMonth, SalesmanCode, SalesPersonCode, SalesPersonDescription
                        , SUM(CasesSold) as CasesSold
                        FROM CTE_CASESSOLD
                        GROUP BY InvoiceYear,InvoiceMonth, Description, Category,SalesmanCode, SalesPersonCode, SalesPersonDescription
                        ) as s 
                        PIVOT
                        (
                        SUM(casesSold) 
                        for InvoiceMonth IN ([{0}],[{1}])
                        )as pvt
                        LEFT JOIN SalesPersonMapping sm on sm.AssignedPersonCode = pvt.SalesmanCode";



            }
        }

        #endregion

        #region Cases Sold YTD/YTCM
        public static string GetCasesSoldYearlyQuery(string current, string previous, string currentStart, string currentEnd, string previousYearStart, string previousYearEnd)
        {
            var query =
                string.Format(SQLQueries.CasesSoldCustomYearQuery
                                , current
                                , previous
                                , currentStart
                                , currentEnd
                                , previousYearStart
                                , previousYearEnd);

            return query;
        }

        internal static string CasesSoldCustomYearQuery
        {
            get
            {
                return @"WITH CTE_CASESSOLD([Description], Category, InvoiceYear, SalesmanCode,CasesSold,SalesPersonCode,SalesPersonDescription)
                        AS
                        (
                        SELECT  
                        ss.descrip
                        , c.speclty as category
                        , DATEPART(YYYY,t.invdte) as InvoiceYear
                        ,ISNULL(spm.AssignedPersonCode,t.salesmn)
                        ,CAST(ROUND(t.qtyshp, 0) AS INT) as casesSold
                        , ISNULL(spm.SalesPersonCode,t.salesmn) as SalesPersonCode
                        , spm.SalesPersonDescription as SalesPersonDescription
                        FROM artran t
                        INNER JOIN arcust c on c.custno =t.custno AND c.custno <> 'ICADJ'
                        INNER JOIN icitem i on i.item = t.item
                        INNER JOIN NPIPROSYS.dbo.sycrlst ss on ss.chrvl = i.comcode AND ss.compid ='01' and ss.ruleid ='COMCODE'
                        LEFT JOIN dbo.SalesPersonMapping as spm on spm.SalesPersonCode COLLATE Latin1_General_CI_AS = t.salesmn 
                        AND 
                            (
                                t.invdte between spm.StartDate and ISNULL(spm.endDate,'{3}')
                                OR
                                t.invdte between spm.StartDate and ISNULL(spm.endDate,'{5}')
                            )
                        where t.arstat not in ('X','V') AND t.item <> ''   
                        AND 
                            (
                                (t.invdte between '{2}' AND '{3}')
                                OR
                                (t.invdte between '{4}' AND '{5}')
                            )
                        )
                        Select pvt.*,sm.SalesPersonCode as code,sm.SalesPersonDescription as descr from (
                        SELECT 
                        [Description] as Descrip, Category,InvoiceYear, SalesmanCode, SalesPersonCode, SalesPersonDescription
                        , SUM(CasesSold) as CasesSold
                        FROM CTE_CASESSOLD
                        GROUP BY InvoiceYear, Description, Category,SalesmanCode, SalesPersonCode, SalesPersonDescription
                        ) as s 
                        PIVOT
                        (
                        SUM(casesSold) 
                        for InvoiceYear IN ([{0}],[{1}])
                        )as pvt
                        LEFT JOIN SalesPersonMapping sm on sm.AssignedPersonCode = pvt.SalesmanCode";

            }
        }

        #endregion

        #region CasessoldSalesBySalesPersonMonthly

        public static string GetMonthlyCasesSoldBySalesPersonQuery
                            (string startDate
                            , string endDate
                            , string previousStartDate
                            , string previousEndDate
                            , string currentMonth
                            , string previousMonth)
        {

            var query = string.Format(GetCasesSoldBySalesPersonQueryString
                                       , startDate
                                       , endDate
                                       , previousStartDate
                                       , previousEndDate
                                       , currentMonth
                                       , previousMonth
                                      );
            return query;
        }

        internal static string GetCasesSoldBySalesPersonQueryString
        {
            get
            {
                return @"WITH CTE_CASES_BY_SALESPERSON(SalesPersonCode,CasesSold,Customer,InvoiceMonth)
                            AS(
                            SELECT                             
                            ISNULL(sm.AssignedPersonCode,at.salesmn) SalesPersonCode
                            ,at.qtyshp AS CasesSold
							,ac.company  as Customer
                            ,DATEPART(MM,at.invdte) as InvoiceMonth
                            FROM artran at 
                            INNER JOIN arcust as ac on at.custno=ac.custno AND ac.custno <> 'ICADJ'
                            LEFT JOIN salespersonmapping sm ON sm.SalesPersonCode COLLATE LATIN1_GENERAL_CI_AS =at.salesmn                             
							AND 
							(
							at.invdte between sm.StartDate and ISNULL(sm.endDate,'{1}')
							OR
							at.invdte between sm.StartDate and ISNULL(sm.endDate,'{3}')
							)
							where at.arstat not in ('X','V') AND at.item <> ''   
							AND 
							(
							(at.invdte between '{0}' AND '{1}')
							OR
							(at.invdte between '{2}' AND '{3}')
							)					
                            
                            )
							  Select * from (
								SELECT 
								SalesPersonCode , Customer,InvoiceMonth
								, SUM(CasesSold) as CasesSold
								FROM CTE_CASES_BY_SALESPERSON
								GROUP BY SalesPersonCode,Customer,InvoiceMonth
								) as ss
								PIVOT
								(
								SUM(CasesSold) 
								for InvoiceMonth IN ([{4}],[{5}])
								)as pvt";

            }

        }

        #endregion

        #region CasessoldSalesBySalesPersonYearly

        public static string GetYearlyCasesSoldSalesBySalesPerson
                            (string startDate
                            , string endDate
                            , string previousStartDate
                            , string previousEndDate
                            , string currentYear
                            , string previousYear
                            )
        {

            var query = string.Format(GetCasesSoldBySalesPersonYearQueryString
                                       , startDate
                                       , endDate
                                       , previousStartDate
                                       , previousEndDate
                                       , currentYear
                                       , previousYear
                                      );
            return query;
        }

        internal static string GetCasesSoldBySalesPersonYearQueryString
        {
            get
            {
                return @"WITH CTE_CASES_BY_SALESPERSON(SalesPersonCode,CasesSold,Customer,InvoiceYear)
                            AS(
                            SELECT                             
                            ISNULL(sm.AssignedPersonCode,at.salesmn)  SalesPersonCode
                            , CAST(ROUND(at.qtyshp, 0) AS INT) AS CasesSold
							, ac.company  as Customer
                            , DATEPART(YYYY,at.invdte) as InvoiceYear
                            FROM artran at 
                            INNER JOIN arcust as ac on at.custno=ac.custno AND ac.custno <> 'ICADJ'
                            LEFT JOIN salespersonmapping sm ON sm.SalesPersonCode COLLATE LATIN1_GENERAL_CI_AS =at.salesmn                             
							AND 
							(
							   at.invdte between sm.StartDate and ISNULL(sm.endDate,'{1}')
							OR at.invdte between sm.StartDate and ISNULL(sm.endDate,'{3}')
							)
							where at.arstat not in ('X','V') AND at.item <> ''   
							AND 
							(
							   (at.invdte between '{0}' AND '{1}')
							OR (at.invdte between '{2}' AND '{3}')
							)					
                            )
							  Select * from (
								SELECT 
								SalesPersonCode , Customer,InvoiceYear
								, SUM(CasesSold) as CasesSold
								FROM CTE_CASES_BY_SALESPERSON
								GROUP BY SalesPersonCode,Customer,InvoiceYear
								) as ss
								PIVOT
								(
								SUM(CasesSold) 
								for InvoiceYear IN ([{4}],[{5}])
								)as pvt";

            }

        }


        #endregion

        #region Cases Sold Report

        public static string GetCasesSoldReportPrerequisitesQuery()
        {
            var query = GetCasesSoldReportPrerequisitesQueryString;
            return query;
        }

        internal static string GetCasesSoldReportPrerequisitesQueryString
        {
            get
            {
                return @"SELECT DISTINCT ss.descrip Comodity
                        FROM npiprosys.dbo.sycrlst ss
                        WHERE ss.compid = '01' AND ss.ruleid = 'COMCODE'

                        SELECT DISTINCT c.speclty Category
                        FROM arcust c 
                        WHERE c.custno <> 'ICADJ' AND c.speclty <> ''";
            }
        }

        public static string GetCasesSoldReportQuery(string currentStart, string currentEnd,
                                                     string previousStart, string previousEnd
                                                    , string comodity, string category
                                                    , string minSalesAmt)
        {
            var query =
                string.Format(SQLQueries.GetCasesSoldReportQueryString
                                                    , currentStart, currentEnd
                                                    , previousStart, previousEnd
                                                    , comodity, category
                                                    , minSalesAmt);
            return query;
        }

        internal static string GetCasesSoldReportQueryString
        {
            get
            {

                return @"
                        WITH CTE_CasesBought(CustomerNumber,QyantityShipped
								--,AssignedSalesPersonCode
								--,SalesPersonCode
								, Term)
                        AS (
                        SELECT 
						am.custno
						,at.qtyshp
						--,ISNULL(sm.assignedpersoncode, at.salesmn) 
						--,at.salesmn
						,case WHEN at.invdte BETWEEN '{0}' AND '{1}' THEN 'Current' ELSE 'Previous' END As Term
                        FROM artran at 
						INNER JOIN icitem i ON i.item = at.item 
						INNER JOIN npiprosys.dbo.sycrlst ss ON ss.chrvl = i.comcode AND ss.compid = '01' AND ss.ruleid = 'COMCODE' AND (''='{4}' OR (ss.descrip = '{4}'))
                        INNER JOIN armast am ON am.invno = at.invno
                        LEFT JOIN salespersonmapping sm ON sm.salespersoncode COLLATE LATIN1_GENERAL_CI_AS =at.salesmn 
                        AND (at.invdte between sm.StartDate AND ISNULL(sm.endDate,'{1}') OR at.invdte between sm.StartDate AND ISNULL(sm.endDate,'{3}'))
                        WHERE 
                        (at.invdte BETWEEN '{0}' AND '{1}' OR  at.invdte BETWEEN '{2}' AND '{3}')
                        AND at.salesmn <>'' --AND ((ISNULL(sm.assignedpersoncode, at.salesmn) IN('')) OR ''='')
                        )
						,CTE_CasesBoughtCurrentPrevious
						(
						CustomerCode
						,[Current]
						,[Previous]
						)
						AS(
						SELECT * FROM (						
										SELECT  cs.CustomerNumber
											--,cs.SalesPersonCode, cs.AssignedSalesPersonCode
											, SUM(cs.QyantityShipped) as QuantityShipped
											,cs.Term 
										FROM  CTE_CasesBought cs
										GROUP BY cs.CustomerNumber
											--,cs.SalesPersonCode, cs.AssignedSalesPersonCode
											,cs.Term
										) as pivoted
										PIVOT  (SUM(QuantityShipped) FOR Term IN ([Current],[Previous]))AS pvt1
										--ORDER BY CustomerNumber
						)
						SELECT c.company as Customer
						, cb.Previous as PreviousCasesSold
						, cb.[Current] as CurrentCasesSold
						,cb.[Current] - cb.[Previous] as [Difference]
                        ,CASE WHEN cb.[Current] + cb.[Previous] = 0 THEN 0 
								ELSE ((cb.[Current] - cb.[Previous]) / ((cb.[Current] + cb.[Previous] /2 ))) * 100 
								END AS PercentageDifference
						FROM CTE_CasesBoughtCurrentPrevious cb
						INNER JOIN arcust c on c.custno =cb.CustomerCode AND c.custno <> 'ICADJ'  AND (''='{5}' OR (c.speclty = '{5}' ))
                        WHERE ( (cb.[Current] >'{6}') OR ('{6}'='')) AND ((cb.Previous >'{6}') OR ('{6}'=''))
						ORDER BY c.company";
            }
        }

        #endregion

        #endregion

        #region Revenue Dashboard

        #region RevenueBySalesPersonMonthly

        public static string GetMonthlyRevenueBySalesPersonQuery
                            (string startDate
                            , string endDate
                            , string previousStartDate
                            , string previousEndDate
                            , string currentMonth
                            , string previousMonth)
        {

            var query = string.Format(GetRevenueBySalesPersonQueryString
                                       , startDate
                                       , endDate
                                       , previousStartDate
                                       , previousEndDate
                                       , currentMonth
                                       , previousMonth
                                      );
            return query;
        }

        internal static string GetRevenueBySalesPersonQueryString
        {
            get
            {
                return @"WITH CTE_CASES_BY_SALESPERSON(SalesPersonCode,Revenue,Customer,InvoiceMonth)
                            AS(
                            SELECT                             
                            ISNULL(sm.AssignedPersonCode,at.salesmn) SalesPersonCode
                            ,at.extprice as Revenue
							,ac.company  as Customer
                            ,DATEPART(MM,at.invdte) as InvoiceMonth
                            FROM artran at 
                            INNER JOIN arcust as ac on at.custno=ac.custno AND ac.custno <> 'ICADJ'
                            LEFT JOIN salespersonmapping sm ON sm.SalesPersonCode COLLATE LATIN1_GENERAL_CI_AS =at.salesmn                             
							AND 
							(
							at.invdte between sm.StartDate and ISNULL(sm.endDate,'{1}')
							OR
							at.invdte between sm.StartDate and ISNULL(sm.endDate,'{3}')
							)
							where at.arstat not in ('X','V') AND at.item <> ''   
							AND 
							(
							(at.invdte between '{0}' AND '{1}')
							OR
							(at.invdte between '{2}' AND '{3}')
							)					
                            
                            )
							  Select * from (
								SELECT 
								SalesPersonCode , Customer,InvoiceMonth
								, SUM(Revenue) as Revenue
								FROM CTE_CASES_BY_SALESPERSON
								GROUP BY SalesPersonCode,Customer,InvoiceMonth
								) as ss
								PIVOT
								(
								SUM(Revenue) 
								for InvoiceMonth IN ([{4}],[{5}])
								)as pvt";

            }

        }

        #endregion

        #region RevenueBySalesPersonYearly
        public static string GetYearlyRevenueBySalesPerson
                            (
                                string startDate, string endDate
                                , string previousStartDate, string previousEndDate
                                , string currentYear, string previousYear
                            )
        {
            var query = string.Format(GetRevenueBySalesPersonYearQueryString
                                    , startDate, endDate
                                    , previousStartDate, previousEndDate
                                    , currentYear, previousYear);
            return query;
        }

        internal static string GetRevenueBySalesPersonYearQueryString
        {
            get
            {
                return @"WITH CTE_Revenue_By_SalesPerson(SalesPersonCode,Revenue,Customer,InvoiceYear)
                        AS
                        (
                        select  
                        ISNULL(spm.AssignedPersonCode, t.salesmn) SalesPersonCode
                        ,t.extprice as Revenue
                        ,c.company  as Customer
                        , DATEPART(YYYY,t.invdte) as InvoiceYear                  
                        from artran t
                        inner join arcust c on c.custno =t.custno AND c.custno <> 'ICADJ'                        
                        left join dbo.SalesPersonMapping as spm on spm.SalesPersonCode COLLATE Latin1_General_CI_AS=t.salesmn 
                        AND 
                            (
                                t.invdte between spm.StartDate and ISNULL(spm.endDate,'{1}')
                                OR
                                t.invdte between spm.StartDate and ISNULL(spm.endDate,'{3}')
                            )
                        where t.arstat not in ('X','V') AND t.item <> ''   
                        AND 
                            (
                                (t.invdte between '{0}' AND '{1}')
                                OR
                                (t.invdte between '{2}' AND '{3}')
                            )
                        )
                        Select * from (
                        SELECT 
                        SalesPersonCode , Customer,InvoiceYear
								, SUM(Revenue) as Revenue
                        FROM CTE_Revenue_By_SalesPerson
                        GROUP BY SalesPersonCode, Customer, InvoiceYear
                        ) as s 
                        PIVOT
                        (
                        SUM(Revenue) 
                        for InvoiceYear IN ([{4}],[{5}])
                        )as pvt";
            }
        }

        #endregion

        #region RevenueReport

        public static string GetRevenueReportQuery
                            (
                                string startDate, string endDate
                                , string salesPersons, string buyers
                                , string vendors, string items
                            )
        {
            var query = string.Format(GetRevenueReportQueryString
                                    , startDate, endDate
                                    , salesPersons, buyers
                                    , vendors, items
                                    , !string.IsNullOrEmpty(salesPersons) ? salesPersons.Substring(0, 1).ToString() : string.Empty
                                    , !string.IsNullOrEmpty(buyers) ? buyers.Substring(0, 1).ToString() : string.Empty
                                    , !string.IsNullOrEmpty(vendors) ? vendors.Substring(0, 1).ToString() : string.Empty
                                    , !string.IsNullOrEmpty(items) ? items.Substring(0, 1).ToString() : string.Empty
                                    );
            return query;
        }
        internal static string GetRevenueReportQueryString
        {
            get
            {
                return @"select
                            ISNULL(sm.SalesPersonDescription,at.salesmn) as SalesPerson
                            ,RTRIM(sl.chrvl) + '-' + sl.descrip AS buyerName
                            ,ac.dealer as Vendor
                            ,at.invno as InvoiceNumber
                            ,at.cost as costSales
                            ,at.extprice as amtSales
                            ,at.qtyshp as qtyShipped
                            ,at.package
                            ,at.custno
                            ,ac.company as Customer
                            ,ic.itmclss as class
                            ,ic.itmdesc as Item
                            ,at.invdte as InvoiceDate
                            --,* 
                            from artran at
                            inner join icitem ic on ic.item = at.item
                            inner join arcust ac on ac.custno = at.custno
                            left join NPIPROSYS.dbo.sycrlst sl ON sl.chrvl = ic.buyer AND sl.ruleid ='BUYER' AND sl.compid = '01'
                            left join SalesPersonMapping sm ON sm.SalesPersonCode COLLATE Latin1_General_CI_AS=at.salesmn 
		                            AND(at.invdte between sm.StartDate and ISNULL(sm.endDate,'{1}'))
                            WHERE at.invdte between '{0}' and '{1}' 
                            AND (SM.SalesPersonDescription in('{2}') OR '{6}'='' )
                            AND (sl.descrip in('{3}') OR '{7}'='')
                            AND (ac.dealer IN('{4}') OR '{8}'='')
                            AND (ic.itmdesc in('{5}') OR '{9}'='')";
            }
        }

        #endregion

        #endregion

        #region Sales Person

        #region Sales Person Report

        public static string GetCustomerAndSalesBySalesPerson(string salesPerson
                                                            , string startDate, string endDate
                                                            , string startDatePrev, string endDatePrev
                                                            , string category = "", string commodity = "")
        {
            var query = string.Format(GetCustomerAndSalesQuery
                                     , salesPerson.ToLower() == "oss" ? string.Join(",", Constants.OutSalesPersons.ToList()) : salesPerson
                                     , startDate
                                     , endDate
                                     , startDatePrev
                                     , endDatePrev
                                     , category
                                     , commodity);
            return query;
        }

        private static string GetCustomerAndSalesQuery
        {
            get
            {

                return @"WITH cte_sales_current(company, salesamount , qty) 
                                AS (SELECT c.company, Sum(t.extprice), Sum(CAST(ROUND(t.qtyshp, 0) AS INT)) 
                                    FROM   artran t 
                                        INNER JOIN arcust c ON c.custno = t.custno INNER JOIN icitem i ON i.item = t.item 
                                        INNER JOIN npiprosys.dbo.sycrlst ss ON ss.chrvl = i.comcode AND ss.compid = '01' AND ss.ruleid = 'COMCODE' 
                                        LEFT JOIN salespersonmapping sm ON sm.salespersoncode COLLATE latin1_general_ci_as = t.salesmn 
                                                    AND ( t.invdte BETWEEN sm.startdate AND Isnull(sm.enddate, '{2}') ) 
                                    WHERE  Isnull(sm.assignedpersoncode, t.salesmn) in (select id from CSVToTable('{0}'))
                                        AND t.invdte BETWEEN '{1}' AND '{2}' 
                                        AND (''='{6}' OR (ss.descrip = '{6}')) 
                                     AND (''=ltrim(rtrim('{5}')) OR (c.speclty like '%'+ltrim(rtrim('{5}'))+'%' ))
                                    GROUP  BY t.salesmn, c.company
                                    ), 
                                cte_sales_prev(company, salesamount) 
                                AS (SELECT c.company, Sum(t.extprice) 
                                    FROM   artran t 
                                        INNER JOIN arcust c ON c.custno = t.custno 
                                        INNER JOIN icitem i ON i.item = t.item 
                                        INNER JOIN npiprosys.dbo.sycrlst ss ON ss.chrvl = i.comcode AND ss.compid = '01' AND ss.ruleid = 'COMCODE' 
                                        LEFT JOIN salespersonmapping sm ON sm.salespersoncode COLLATE latin1_general_ci_as = t.salesmn 
                                                    AND ( t.invdte BETWEEN sm.startdate AND Isnull(sm.enddate, '{4}') ) 
                                    WHERE  Isnull(sm.assignedpersoncode, t.salesmn) in (select id from CSVToTable('{0}'))
                                        AND t.invdte BETWEEN '{3}' AND '{4}' 
                                        AND (''='{6}' OR (ss.descrip = '{6}')) 
                                AND (''=ltrim(rtrim('{5}')) OR (c.speclty like '%'+ltrim(rtrim('{5}'))+'%' ))
                                    GROUP  BY t.salesmn, 
                                            c.company) 
                        SELECT C1.company          AS Customer, 
                                Sum(C1.qty) AS TotalSalesQty,
                                Sum(C1.salesamount) AS TotalSalesAmount, 
                                Sum(C2.salesamount) AS TotalSalesAmountPrev, 
                                CASE 
                                    WHEN Sum(C1.salesamount) + Sum(C2.salesamount) = 0 THEN 0 
                                    ELSE( ( Sum(C1.salesamount) - Sum(C2.salesamount) ) / ( ( 
                                                Sum(C1.salesamount) + Sum(C2.salesamount) ) / 2 ) ) * 100 
                                END                 AS PercentageDifference 
                        FROM   cte_sales_current C1 
                                LEFT JOIN cte_sales_prev C2 
                                        ON C1.company = C2.company 
                        GROUP  BY C1.company 
                        ORDER  BY totalsalesamount DESC; 
                        ";

                #region Commented
                //return @"WITH CTE_Sales_Current(Company, SalesAmount) AS
                //        (
                //        SELECT
                //        c.company
                //        , SUM(t.extprice)
                //        FROM artran t
                //        INNER JOIN arcust c ON c.custno = t.custno

                //         LEFT JOIN salespersonmapping sm ON sm.AssignedPersonCode COLLATE LATIN1_GENERAL_CI_AS = t.salesmn

                //            AND
                //            (
                //            t.invdte between sm.StartDate and ISNULL(sm.endDate, '{2}')
                //            )
                //        WHERE ISNULL(sm.SalesPersonCode,t.salesmn) = '{0}'
                //        AND t.invdte  BETWEEN '{1}' AND '{2}'
                //        GROUP BY t.salesmn, c.company
                //        ),
                //        CTE_Sales_Prev(Company, SalesAmount) AS
                //        (
                //        SELECT
                //        c.company
                //        , SUM(t.extprice)
                //        FROM artran t
                //        INNER JOIN arcust c ON c.custno = t.custno

                //        LEFT JOIN salespersonmapping sm ON sm.AssignedPersonCode COLLATE LATIN1_GENERAL_CI_AS = t.salesmn

                //            AND
                //            (
                //            t.invdte between sm.StartDate and ISNULL(sm.endDate, '{4}')
                //            )
                //        WHERE ISNULL(sm.SalesPersonCode,t.salesmn) = '{0}'
                //        AND t.invdte  BETWEEN '{3}' AND '{4}'
                //        GROUP BY t.salesmn, c.company
                //        )


                //        SELECT
                //        C1.Company AS Customer
                //        , SUM(C1.SalesAmount) AS TotalSalesAmount
                //        , SUM(C2.SalesAmount) AS TotalSalesAmountPrev
                //        , CASE WHEN SUM(C1.SalesAmount) + SUM(C2.SalesAmount) = 0 THEN 0 ELSE((SUM(C1.SalesAmount) - SUM(C2.SalesAmount)) / ((SUM(C1.SalesAmount) + SUM(C2.SalesAmount)) / 2)) * 100 END AS PercentageDifference
                //        FROM CTE_Sales_Current C1 LEFT JOIN CTE_Sales_Prev C2 ON C1.Company = C2.Company
                //        GROUP BY C1.Company
                //        ORDER BY TotalSalesAmount DESC;";
                #endregion

            }
        }

        #endregion

        #region Salesperson Dashboard

        public static string GetSalesBySalesPersonQuery(string startDate, string endDate, string priorStartDate, string priorEndDate)
        {
            var query = string.Format(SQLQueries.GetSalesBySalesPersonQueryString
                                        , startDate
                                        , endDate
                                        , priorStartDate
                                        , priorEndDate);

            return query;

        }

        internal static string GetSalesBySalesPersonQueryString
        {
            get
            {
                return @"WITH CTE_CasesBought(AssignedSalesPersonCode
					                            ,CustomerNumber
					                            ,SalesAmount
					                            --,SalesPersonCode
					                            , Term)
                                            AS (
					                            SELECT 
					                            ISNULL(sm.assignedpersoncode, at.salesmn) 
					                            , am.custno
					                            ,at.extprice
					                            --,at.salesmn
					                            ,case WHEN at.invdte BETWEEN '{0}' AND '{1}' THEN 'Current' ELSE 'Previous' END As Term
					                            FROM artran at 
					                            INNER JOIN icitem i ON i.item = at.item 
					                            INNER JOIN armast am ON am.invno = at.invno
					                            LEFT JOIN salespersonmapping sm ON sm.salespersoncode COLLATE LATIN1_GENERAL_CI_AS =at.salesmn 
					                            AND (
							                            at.invdte between sm.StartDate AND ISNULL(sm.endDate,'{1}') 
							                            OR at.invdte between sm.StartDate AND ISNULL(sm.endDate,'{3}')
						                            )
					                            WHERE 
					                            (at.invdte BETWEEN  '{0}' AND '{1}' OR  at.invdte BETWEEN '{2}' AND '{3}' )
					                            AND at.salesmn <>'' --AND ((ISNULL(sm.assignedpersoncode, at.salesmn) IN('')) OR ''='')
                                            )
                            SELECT tt.*,sm.SalesPersonCode,sm.SalesPersonDescription FROM (
                            SELECT * FROM (		
                            SELECT cb.AssignedSalesPersonCode
	                            ,c.company as Customer
	                            ,cb.Term
	                            ,isnull(SUM(cb.SalesAmount),0) SalesAmount
                            FROM CTE_CasesBought cb
                            INNER JOIN arcust c on c.custno =cb.CustomerNumber AND c.custno <> 'ICADJ'  
                            GROUP BY cb.AssignedSalesPersonCode
	                            ,c.company 
	                            ,cb.Term) as pivoted
                            PIVOT  (SUM(SalesAmount) FOR Term IN ([Current],[Previous]))AS pvt1
                            ) tt
                            LEFT JOIN salespersonmapping sm ON sm.AssignedPersonCode COLLATE LATIN1_GENERAL_CI_AS =tt.AssignedSalesPersonCode 
					                            AND ('{1}' between sm.StartDate AND ISNULL(sm.endDate,'{1}'))
                            Order BY tt.AssignedSalesPersonCode";
            }
        }

        #endregion


        #endregion

        #region Profitability

        public static string GetProfitabilityQuery(string beginYear, string endYear)
        {
            var query = string.Format(GetProfitabilityQueryString
                                     , beginYear
                                     , endYear
                                    );
            return query;
        }

        private static string GetProfitabilityQueryString
        {
            get
            {

                return @" 
                        SELECT invdte as InvoDate,SUM(extprice- (qtyshp*cost))  AS Profiability
								FROM artran
								WHERE	custno <> 'ICADJ' AND item <> '' AND arstat not in ('X','V') AND invdte between '{0}' and '{1}'
								GROUP BY invdte
								ORDER BY invdte ASC
                        ";



            }
        }


        #endregion

        #region VendorList

        public static string GetAllVendors()
        {
            //            return @"
            //                    select distinct NPICompany01.dbo.apvend.company from NPICompany01.dbo.apvend 
            //				    cross join NPIPROSYS.dbo.syccomp where NPIPROSYS.dbo.syccomp.compid='01' ";

            return @"SELECT chrvl AS dealerId, descrip AS dealerName
                            FROM         NPIPROSYS.dbo.sycrlst
                            WHERE     (ruleid = 'CAT_TYPE') AND (compid = '01')";

        }

        #endregion

        #region BuyersList

        public static string GetAllBuyers()
        {
            return @"SELECT distinct  descrip AS buyerName
                        FROM   NPIPROSYS.dbo.sycrlst
                        WHERE  (ruleid = 'BUYER') AND (compid = '01')";
        }

        #endregion

        #region ItemList

        public static string GetItemSearchQuery(string itemString)
        {
            var query = string.Format(GetItemSearchQueryString
                                     , itemString
                                    );
            return query;
        }

        private static string GetItemSearchQueryString
        {
            get
            {

                return @" Select itmdesc from icitem where itmdesc like '{0}%' ";
            }
        }

        #endregion

        #region Dashboard Combined Cases Sold & Revenue

        /// <summary>
        /// Generate the query to fetch data for the cases sold and by month
        /// <param name="date"> Date </param>
        /// <returns> query string for the case sold, revenue report </returns>
        //internal static string GetCasesSoldRevenueReportQuery(DateTime date)
        //{
        //    var query = string.Format(CasesSoldRevenueReportByMonthly
        //                            , date
        //                          );
        //    return query;
        //}
        //        private static string CasesSoldRevenueReportByMonthly
        //        {
        //            get
        //            {
        //                return @"
        //declare @current_date datetime ='{0}'
        //declare @curr_Month_strt_date datetime = (SELECT DATEADD(month, DATEDIFF(month, 0, @current_date), 0))
        //declare @last_month_curr_date datetime=(SELECT DateAdd(month, -1, Convert(date, @current_date)))
        //declare @last_month_strt_date datetime=(SELECT DATEADD(month, DATEDIFF(month, 0, @last_month_curr_date), 0))

        //declare @last_year_curr_date datetime =(SELECT DateAdd(year, -1, Convert(date, @current_date)))
        //declare @last_year_strt_date datetime = (SELECT DATEADD(month, DATEDIFF(month, 0, @last_year_curr_date), 0))
        //declare @last_year_prev_curr_date datetime=(SELECT DateAdd(month, -1, Convert(date, @last_year_curr_date)))
        //declare @last_year_prev_strt_date datetime=(SELECT DATEADD(month, DATEDIFF(month, 0, @last_year_prev_curr_date), 0))

        //  IF OBJECT_ID('tempdb..#temp') IS NOT NULL
        //     DROP TABLE #temp
        //   IF OBJECT_ID('tempdb..#temp2') IS NOT NULL
        //     DROP TABLE #temp2
        //	   IF OBJECT_ID('tempdb..#temp3') IS NOT NULL
        //     DROP TABLE #temp3
        //   IF OBJECT_ID('tempdb..#persons') IS NOT NULL
        //     DROP TABLE #persons
        //   IF OBJECT_ID('tempdb..#revenue') IS NOT NULL
        //     DROP TABLE #revenue
        //   IF OBJECT_ID('tempdb..#shipped') IS NOT NULL
        //     DROP TABLE #shipped
        //   IF OBJECT_ID('tempdb..#currentMonthdates') IS NOT NULL
        //     DROP TABLE #currentMonthdates
        //   IF OBJECT_ID('tempdb..#lastMonthdates') IS NOT NULL
        //     DROP TABLE #lastMonthdates
        //   IF OBJECT_ID('tempdb..#lastyearcurrent') IS NOT NULL
        //     DROP TABLE #lastyearcurrent
        //   IF OBJECT_ID('tempdb..#lastyearprevious') IS NOT NULL
        //     DROP TABLE #lastyearprevious

        // ;with CTE as(
        // select at.invno, ss.descrip typ
        // , spm.Speciality spec
        // ,ISNULL(spm.AssignedPersonCode,at.salesmn) salesman
        // ,at.invdte
        // ,datepart(mm,at.invdte) monthPart
        // ,case when datepart(yy,at.invdte)=datepart(yy,getdate()) then 'current' when datepart(yy,at.invdte)=(datepart(yy,getdate())-1) then 'previous' end as yearPart
        // ,at.custno
        // ,ac.company
        // ,CAST(ROUND(at.qtyshp, 0) AS INT) qty
        // ,at.extprice amt
        // ,case 
        //  when at.invdte BETWEEN @curr_Month_strt_date AND @current_date then 'value1'
        //  when at.invdte BETWEEN @last_month_strt_date AND @last_month_curr_date then 'value2'
        //  when at.invdte BETWEEN @last_year_strt_date AND @last_year_curr_date then 'value3'
        //  when at.invdte BETWEEN @last_year_prev_strt_date AND @last_year_prev_curr_date then 'value4'
        // end as condition
        // from artran at
        // INNER JOIN arcust ac on ac.custno = at.custno AND ac.custno <> 'ICADJ'
        // INNER JOIN icitem i ON i.item = at.item 
        // INNER JOIN npiprosys.dbo.sycrlst ss ON ss.chrvl = i.comcode AND ss.compid = '01' AND ss.ruleid = 'COMCODE'
        // inner JOIN dbo.SalesPersonMapping as spm on (spm.AssignedPersonCode COLLATE Latin1_General_CI_AS=at.salesmn 
        // or spm.SalesPersonCode COLLATE Latin1_General_CI_AS=at.salesmn
        // )
        //          AND
        //       (
        //         at.invdte between spm.StartDate and ISNULL(spm.endDate,@current_date)
        //         OR
        //         at.invdte between spm.StartDate and ISNULL(spm.endDate,@last_month_curr_date)           
        //         OR
        //         at.invdte between spm.StartDate and ISNULL(spm.endDate,@last_year_curr_date)
        //         OR 
        //         at.invdte between spm.StartDate and ISNULL(spm.endDate,@last_year_prev_curr_date)
        //        )
        //       where at.arstat not in ('X','V') AND at.item <> ''   
        //       AND
        //         ltrim(rtrim(ac.speclty)) not in('OOT','LOSS PROD')
        //       AND 
        //      (at.invdte BETWEEN @curr_Month_strt_date AND @current_date
        //   OR
        //  at.invdte BETWEEN @last_month_strt_date and  @last_month_curr_date
        //   or
        //  at.invdte BETWEEN @last_year_strt_date AND @last_year_curr_date
        //   or
        //  at.invdte BETWEEN @last_year_prev_strt_date AND @last_year_prev_curr_date
        //  ) and at.salesmn!=''
        //  )

        //  select * into #temp from CTE

        //  SELECT  DATEADD(DAY, nbr - 1, @curr_Month_strt_date) [date] into #currentMonthdates
        //  FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr
        //          FROM      sys.columns c
        //        ) nbrs
        //  WHERE   nbr - 1 <= DATEDIFF(DAY, @curr_Month_strt_date, @current_date)


        //  SELECT  DATEADD(DAY, nbr - 1, @last_month_strt_date) [date] into #lastMonthdates
        //  FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr
        //          FROM      sys.columns c
        //        ) nbrs
        //  WHERE   nbr - 1 <= DATEDIFF(DAY, @last_month_strt_date, @last_month_curr_date)


        //  SELECT  DATEADD(DAY, nbr - 1, @last_year_strt_date) [date] into #lastyearcurrent
        //  FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr
        //          FROM      sys.columns c
        //        ) nbrs
        //  WHERE   nbr - 1 <= DATEDIFF(DAY, @last_year_strt_date, @last_year_curr_date)

        //  SELECT  DATEADD(DAY, nbr - 1, @last_year_prev_strt_date) [date] into #lastyearprevious
        //  FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr
        //          FROM      sys.columns c
        //        ) nbrs
        //  WHERE   nbr - 1 <= DATEDIFF(DAY, @last_year_prev_strt_date, @last_year_prev_curr_date)

        //  select  q.comtype comtype ,max(q.salesman) salesman,max(q.speciality) speciality
        //  ,sum([current]) currentYear
        //  ,sum([previous]) previousYear,max(monthPart) monthPart
        //   into #revenue

        //  from (
        //   select typ comtype,max(spec) speciality,sum(qty) ItemShipped,sum(amt) revenue ,
        //   max(salesman) Salesman,max(condition) Value,monthPart,max(yearPart) yearPart from #temp 
        //   group by yearPart,monthPart,salesman,spec,typ)c
        //    pivot
        //    (
        //     sum(revenue) for yearPart in([current],[previous])

        //       ) as q
        // group by q.monthPart,q.salesman,q.speciality,q.comtype
        //  order by q.Salesman,q.speciality

        //  select  q.comtype comtype ,max(q.salesman) salesman,max(q.speciality) speciality,sum([current]) currentYear
        //  ,sum([previous]) previousYear,max(monthPart) monthPart
        //  into #shipped

        //  from (
        //   select typ comtype,max(spec) speciality,sum(qty) ItemShipped,sum(amt) revenue ,
        //   max(salesman) Salesman,max(condition) Value,monthPart,max(yearPart) yearPart   from #temp 
        //   group by yearPart,monthPart,salesman,spec,typ)c
        //    pivot
        //    (
        //     sum(ItemShipped) for yearPart in([current],[previous])

        //       ) as q 

        //   group by q.monthPart,q.salesman,q.speciality,q.comtype
        //  order by q.Salesman,q.speciality

        //  select * into #temp2  from #temp left join SalesPersonMapping as spm on spm.AssignedPersonCode =#temp.salesman and
        //  (
        //   exists(select * from #currentMonthdates A cross join (SELECT  DATEADD(DAY, nbr - 1,  spm.StartDate) [date] FROM  
        //    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr FROM      sys.columns c
        //   ) nbrs WHERE   nbr - 1 <= DATEDIFF(DAY,  spm.StartDate, isnull(spm.EndDate,@current_date))) B
        //   where A.date=B.date)

        //   or
        //    exists(select * from #lastMonthdates A cross join (SELECT  DATEADD(DAY, nbr - 1, spm.StartDate) [date] FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr FROM      sys.columns c
        //   ) nbrs WHERE   nbr - 1 <= DATEDIFF(DAY, spm.StartDate, isnull(spm.EndDate,@current_date))) B
        //   where A.date=B.date)

        //    or
        //    exists(select * from #lastyearcurrent A cross join (SELECT  DATEADD(DAY, nbr - 1, spm.StartDate) [date] FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr FROM      sys.columns c
        //   ) nbrs WHERE   nbr - 1 <= DATEDIFF(DAY, spm.StartDate, isnull(spm.EndDate,@current_date))) B
        //   where A.date=B.date)

        //    or
        //    exists(select * from #lastyearprevious A cross join (SELECT  DATEADD(DAY, nbr - 1, spm.StartDate) [date] FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr FROM      sys.columns c
        //   ) nbrs WHERE   nbr - 1 <= DATEDIFF(DAY, spm.StartDate, isnull(spm.EndDate,@current_date))) B
        //   where A.date=B.date)
        //     ) AND (#temp.invdte between spm.StartDate and isnull(spm.EndDate,getdate()))



        //  select yearpart,monthpart,spec,typ,#temp2.AssignedPersonCode,#temp2.SalesPersonCode,
        //  max(ltrim(rtrim(spm.SalesPersonDescription)))+' ('+max(rtrim(ltrim(spm.SalesPersonCode)))+')' SalesManName,(condition) condition
        //  into #temp3
        //   from SalesPersonMapping spm
        //  left join #temp2 on spm.AssignedPersonCode=#temp2.salesman and (#temp2.invdte between spm.StartDate and isnull(spm.EndDate,getdate()))
        //   where spm.SalesPersonCode=#temp2.SalesPersonCode and spm.AssignedPersonCode=#temp2.salesman
        //  group by yearPart,monthPart, #temp2.AssignedPersonCode,#temp2.SalesPersonCode,spec,typ,condition

        //  select  q.comtype comtype ,max(q.AssignedPersonCode) salesman,max(q.speciality) speciality,max([current]) currentYear
        //  ,max([previous]) previousYear,max(monthPart) monthPart ,(select top 1 d.SalesPersonDescription from SalesPersonMapping d where d.AssignedPersonCode=q.AssignedPersonCode and EndDate is null) ActiveEmployee
        //  into #persons
        //    from (
        //    select distinct typ comtype,max(spec) speciality,AssignedPersonCode,max(yearPart) yearPart ,monthPart,
        //    employee= STUFF(
        //    (SELECT DISTINCT '|' + rtrim(ltrim(sm.SalesManName))
        //    FROM #temp3 sm
        //     WHERE rtrim(ltrim(sm.AssignedPersonCode)) = rtrim(ltrim(#temp3.AssignedPersonCode))  and sm.condition=#temp3.condition
        //    FOR XML PATH ('')) , 1, 1, '')  ,(condition) Value 
        //   from #temp3
        //   group by yearPart,monthPart,#temp3.AssignedPersonCode,spec,typ,condition)c
        //    pivot
        //    (
        //     max(employee) for yearPart in([current],[previous])

        //       ) as q 

        //   group by q.monthPart,q.AssignedPersonCode,q.speciality,q.comtype
        //  order by q.AssignedPersonCode,q.speciality

        //   select a.comtype as Comodity, a.salesman as SalesPersonCode, a.speciality as Category
        //  , a.currentYear revenueCurrentYear
        //  ,a.previousYear revenuePreviousYear
        //  ,c.currentYear casesCurrentYear
        //  ,c.previousYear casesPreviousYear
        //  ,b.currentYear,b.previousYear,b.monthPart,b.ActiveEmployee
        //    from #revenue a inner join #persons b 
        //   on
        //   a.comtype=b.comtype and a.salesman=b.salesman and a.speciality=b.speciality and a.monthPart=b.monthPart
        //   inner join #shipped c
        //   on
        //   a.comtype=c.comtype and a.salesman=c.salesman and a.speciality=c.speciality and a.monthPart=c.monthPart
        //";

        //            }
        //        }

        /// <summary>
        /// Generate the query to fetch customers for the salesperson
        /// <param name="date"> Date </param>
        /// <returns> query string for the customers by salesperson </returns>
        internal static string GetCustomersBySalesPerson(DateTime date, string salesPerson)
        {
            var query = string.Format(CustomersBySalesPerson, salesPerson
                                    , date
                                  );
            return query;
        }
        private static string CustomersBySalesPerson
        {
            get
            {
                //                return @"
                //declare @salesmen varchar(max) ='{0}'
                //declare @current_date datetime ='{1}'
                //declare @curr_Month_strt_date datetime = (SELECT DATEADD(month, DATEDIFF(month, 0, @current_date), 0))
                //declare @last_month_curr_date datetime=(SELECT DateAdd(month, -1, Convert(date, @current_date)))
                //declare @last_month_strt_date datetime=(SELECT DATEADD(month, DATEDIFF(month, 0, @last_month_curr_date), 0))


                //   IF OBJECT_ID('tempdb..#temp') IS NOT NULL
                //     DROP TABLE #temp
                //   IF OBJECT_ID('tempdb..#temp2') IS NOT NULL
                //     DROP TABLE #temp2
                //   IF OBJECT_ID('tempdb..#salesmen') IS NOT NULL
                //     DROP TABLE #salesmen


                // ;with CTE as(
                // select
                // ISNULL(spm.AssignedPersonCode,at.salesmn) salesman
                // ,at.invdte
                // ,datepart(mm,at.invdte) monthPart
                // ,at.custno
                // ,ac.company
                // ,CAST(ROUND(at.qtyshp, 0) AS INT) qty
                // ,at.extprice amt
                // ,case 
                //  when at.invdte BETWEEN @curr_Month_strt_date AND @current_date then 'value1'
                //  when at.invdte BETWEEN @last_month_strt_date AND @last_month_curr_date then 'value2'
                // end as condition
                // from artran at
                // INNER JOIN arcust ac on ac.custno = at.custno AND ac.custno <> 'ICADJ'
                // INNER JOIN icitem i ON i.item = at.item 
                // INNER JOIN npiprosys.dbo.sycrlst ss ON ss.chrvl = i.comcode AND ss.compid = '01' AND ss.ruleid = 'COMCODE'
                //  LEFT JOIN dbo.SalesPersonMapping as spm on spm.SalesPersonCode COLLATE Latin1_General_CI_AS=at.salesmn 
                //       AND
                //       (
                //         at.invdte between spm.StartDate and ISNULL(spm.endDate,@current_date)
                //         OR
                //         at.invdte between spm.StartDate and ISNULL(spm.endDate,@last_month_curr_date)           

                //        )
                //       where at.arstat not in ('X','V') AND at.item <> ''   
                //       AND 
                //            ltrim(rtrim(ac.speclty)) not in('OOT','NATIONAL','LOSS PROD')
                //       AND 
                //(at.invdte BETWEEN @curr_Month_strt_date AND @current_date
                //   OR
                //  at.invdte BETWEEN @last_month_strt_date and  @last_month_curr_date

                //  ) and at.salesmn!=''
                //  )

                //  select * into #temp from CTE

                //  select * INTO #salesmen from CSVToTable(@salesmen)

                //   select max(#salesmen.id) Salesman,monthPart, company,sum(amt) amount,sum(qty) qty 
                //    from #temp inner join #salesmen
                //    on #salesmen.id=#temp.salesman
                //    group by monthPart, company
                //    order by Salesman,qty desc
                // ";
                return @"
                declare @salesmen varchar(max) ='{0}'
                declare @current_date datetime ='{1}'
                declare @curr_Month_strt_date datetime = (SELECT DATEADD(month, DATEDIFF(month, 0, @current_date), 0))
declare @last_month_curr_date datetime=(SELECT DateAdd(month, -1, Convert(date, @current_date)))
declare @last_month_strt_date datetime=(SELECT DATEADD(month, DATEDIFF(month, 0, @last_month_curr_date), 0))
declare @last_year_curr_date datetime =(SELECT DateAdd(year, -1, Convert(date, @current_date)))
declare @last_year_strt_date datetime = (SELECT DATEADD(month, DATEDIFF(month, 0, @last_year_curr_date), 0))

   IF OBJECT_ID('tempdb..#temp') IS NOT NULL
     DROP TABLE #temp
   IF OBJECT_ID('tempdb..#temp2') IS NOT NULL
     DROP TABLE #temp2
   IF OBJECT_ID('tempdb..#salesmen') IS NOT NULL
     DROP TABLE #salesmen
     
  
 ;with CTE as(
 select ss.descrip compType,
 ISNULL(spm.AssignedPersonCode,at.salesmn) salesman
 ,at.invdte
 ,datepart(mm,at.invdte) monthPart
  ,datepart(yyyy,at.invdte) yearPart
 ,at.custno
 ,ac.company
 ,CAST(ROUND(at.qtyshp, 0) AS INT) qty
 ,at.extprice amt
 ,case 
  when at.invdte BETWEEN @curr_Month_strt_date AND @current_date then 'value1'
  when at.invdte BETWEEN @last_month_strt_date AND @last_month_curr_date then 'value2'
  when at.invdte BETWEEN @last_year_strt_date AND @last_year_curr_date then 'value3'
 end as condition
 from artran at
 INNER JOIN arcust ac on ac.custno = at.custno AND ac.custno <> 'ICADJ'
 INNER JOIN icitem i ON i.item = at.item 
 INNER JOIN npiprosys.dbo.sycrlst ss ON ss.chrvl = i.comcode AND ss.compid = '01' AND ss.ruleid = 'COMCODE'
 inner JOIN dbo.SalesPersonMapping as spm on (spm.AssignedPersonCode COLLATE Latin1_General_CI_AS=at.salesmn 
 or spm.SalesPersonCode COLLATE Latin1_General_CI_AS=at.salesmn
 )
        AND
       (
         at.invdte between spm.StartDate and ISNULL(spm.endDate,@current_date)
         OR
         at.invdte between spm.StartDate and ISNULL(spm.endDate,@last_month_curr_date)     
		   OR
         at.invdte between spm.StartDate and ISNULL(spm.endDate,@last_year_curr_date)           
      
        )
       where at.arstat not in ('X','V') AND at.item <> ''   
       AND 
            ltrim(rtrim(ac.speclty)) not in('OOT','LOSS PROD')
       AND 
(at.invdte BETWEEN @curr_Month_strt_date AND @current_date
   OR
  at.invdte BETWEEN @last_month_strt_date and  @last_month_curr_date
   OR
  at.invdte BETWEEN @last_year_strt_date and  @last_year_curr_date  
  ) and at.salesmn!=''
  )

  select * into #temp from CTE

  select * INTO #salesmen from CSVToTable(@salesmen)
  
   select comptype,max(#salesmen.id) Salesman,yearPart,monthPart, company,sum(amt) amount,sum(qty) qty 
    from #temp inner join #salesmen
    on #salesmen.id=#temp.salesman
    group by yearPart,monthPart
	, company,compType
    order by Salesman,comptype,qty desc";

            }



        }

        /// <summary>
        /// Generate the query to fetch customers for the salesperson
        /// <param name="date"> Date </param>
        /// <returns> query string for the customers by salesperson </returns>
        internal static string GetCustomersBySalesPersonByYear(DateTime date, string salesPerson)
        {
            var query = string.Format(CustomersBySalesPersonByYear, salesPerson
                                    , date
                                  );
            return query;
        }
        private static string CustomersBySalesPersonByYear
        {
            get
            {
                //                return @"
                //declare @salesmen varchar(max) ='{0}'
                //declare @current_date datetime ='{1}'
                //declare @curr_year_strt_date datetime = (SELECT DATEADD(year, DATEDIFF(year, 0, getdate()), 0))

                //declare @last_year_curr_date datetime =(SELECT DateAdd(year, -1, Convert(date, @current_date)))
                //declare @last_year_strt_date datetime = (SELECT DATEADD(year, DATEDIFF(year, 0, @last_year_curr_date), 0))

                //declare @currYear int= datepart(yyyy,@current_date)

                //declare @prevYear int= datepart(yyyy,@last_year_curr_date)

                //   IF OBJECT_ID('tempdb..#temp') IS NOT NULL
                //     DROP TABLE #temp
                //   IF OBJECT_ID('tempdb..#temp2') IS NOT NULL
                //     DROP TABLE #temp2
                //   IF OBJECT_ID('tempdb..#salesmen') IS NOT NULL
                //     DROP TABLE #salesmen


                // ;with CTE as(
                // select
                // ISNULL(spm.AssignedPersonCode,at.salesmn) salesman
                // ,at.invdte
                // ,at.custno
                // ,ac.company
                // ,CAST(ROUND(at.qtyshp, 0) AS INT) qty
                // ,at.extprice amt
                // ,case 
                //  when at.invdte BETWEEN @curr_year_strt_date AND @current_date then @currYear
                //  when at.invdte BETWEEN @last_year_strt_date AND @last_year_curr_date then @prevYear
                // end as [yearPart]
                // from artran at
                // INNER JOIN arcust ac on ac.custno = at.custno AND ac.custno <> 'ICADJ'
                // INNER JOIN icitem i ON i.item = at.item 
                // INNER JOIN npiprosys.dbo.sycrlst ss ON ss.chrvl = i.comcode AND ss.compid = '01' AND ss.ruleid = 'COMCODE'
                //  LEFT JOIN dbo.SalesPersonMapping as spm on spm.SalesPersonCode COLLATE Latin1_General_CI_AS=at.salesmn 
                //       AND
                //       (
                //         at.invdte between spm.StartDate and ISNULL(spm.endDate,@current_date)
                //         OR
                //         at.invdte between spm.StartDate and ISNULL(spm.endDate,@last_year_curr_date)           

                //        )
                //       where at.arstat not in ('X','V') AND at.item <> ''  
                //       AND 
                //    ltrim(rtrim(ac.speclty)) not in('OOT','NATIONAL','LOSS PROD')
                //       AND 
                //(at.invdte BETWEEN @curr_year_strt_date AND @current_date
                //   OR
                //  at.invdte BETWEEN @last_year_strt_date and  @last_year_curr_date

                //  ) and at.salesmn!=''
                //  )

                //  select * into #temp from CTE

                //  select * INTO #salesmen from CSVToTable(@salesmen)

                //   select max(#salesmen.id) Salesman, company,sum(amt) amount,sum(qty) qty ,yearPart
                //    from #temp inner join #salesmen
                //    on #salesmen.id=#temp.salesman
                //    group by  yearPart,company
                //    order by Salesman,company,yearPart,qty desc


                // ";
                return @"
declare @salesmen varchar(max) ='{0}'
declare @current_date datetime ='{1}'
declare @curr_year_strt_date datetime = (SELECT DATEADD(year, DATEDIFF(year, 0, getdate()), 0))

declare @last_year_curr_date datetime =(SELECT DateAdd(year, -1, Convert(date, @current_date)))
declare @last_year_strt_date datetime = (SELECT DATEADD(year, DATEDIFF(year, 0, @last_year_curr_date), 0))

declare @currYear int= datepart(yyyy,@current_date)

declare @prevYear int= datepart(yyyy,@last_year_curr_date)

   IF OBJECT_ID('tempdb..#temp') IS NOT NULL
     DROP TABLE #temp
   IF OBJECT_ID('tempdb..#temp2') IS NOT NULL
     DROP TABLE #temp2
   IF OBJECT_ID('tempdb..#salesmen') IS NOT NULL
     DROP TABLE #salesmen
     
  
 ;with CTE as(
 select
 ISNULL(spm.AssignedPersonCode,at.salesmn) salesman,
 ss.descrip comodity
 ,at.invdte
 ,at.custno
 ,ac.company
 ,CAST(ROUND(at.qtyshp, 0) AS INT) qty
 ,at.extprice amt
 ,case 
  when at.invdte BETWEEN @curr_year_strt_date AND @current_date then @currYear
  when at.invdte BETWEEN @last_year_strt_date AND @last_year_curr_date then @prevYear
 end as [yearPart]
 from artran at
 INNER JOIN arcust ac on ac.custno = at.custno AND ac.custno <> 'ICADJ'
 INNER JOIN icitem i ON i.item = at.item 
 INNER JOIN npiprosys.dbo.sycrlst ss ON ss.chrvl = i.comcode AND ss.compid = '01' AND ss.ruleid = 'COMCODE'
  LEFT JOIN dbo.SalesPersonMapping as spm on spm.SalesPersonCode COLLATE Latin1_General_CI_AS=at.salesmn 
       AND
       (
         at.invdte between spm.StartDate and ISNULL(spm.endDate,@current_date)
         OR
         at.invdte between spm.StartDate and ISNULL(spm.endDate,@last_year_curr_date)           
      
        )
       where at.arstat not in ('X','V') AND at.item <> ''  
       AND 
    ltrim(rtrim(ac.speclty)) not in('OOT','LOSS PROD')
       AND 
(at.invdte BETWEEN @curr_year_strt_date AND @current_date
   OR
  at.invdte BETWEEN @last_year_strt_date and  @last_year_curr_date
  
  ) and at.salesmn!=''
  )

  select * into #temp from CTE

  select * INTO #salesmen from CSVToTable(@salesmen)
  
   select comodity,
   max(#salesmen.id) Salesman, company,sum(amt) amount,sum(qty) qty ,yearPart
    from #temp inner join #salesmen
    on #salesmen.id=#temp.salesman
    group by  yearPart,company,comodity
    order by Salesman,company,yearPart,qty desc 
 ";

            }



        }

        /// <summary>
        /// Generate the query to fetch data for the cases sold and by year
        /// <param name="date"> Date </param>
        /// <returns> query string for the case sold, revenue report </returns>
        internal static string GetCasesSoldRevenueReportQueryByYear(DateTime date)
        {
            var query = string.Format(CasesSoldRevenueReportByYearly
                                    , date
                                  );
            return query;
        }

        private static string CasesSoldRevenueReportByYearly
        {
            get
            {
                return @"

declare @current_date datetime ='{0}'
declare @curr_year_strt_date datetime = (SELECT DATEADD(year, DATEDIFF(year, 0, getdate()), 0))

declare @last_year_curr_date datetime =(SELECT DateAdd(year, -1, Convert(date, @current_date)))
declare @last_year_strt_date datetime = (SELECT DATEADD(year, DATEDIFF(year, 0, @last_year_curr_date), 0))

   IF OBJECT_ID('tempdb..#temp') IS NOT NULL
     DROP TABLE #temp
   IF OBJECT_ID('tempdb..#temp2') IS NOT NULL
     DROP TABLE #temp2
   IF OBJECT_ID('tempdb..#persons') IS NOT NULL
     DROP TABLE #persons
   IF OBJECT_ID('tempdb..#revenue') IS NOT NULL
     DROP TABLE #revenue
   IF OBJECT_ID('tempdb..#shipped') IS NOT NULL
     DROP TABLE #shipped
   IF OBJECT_ID('tempdb..#currentYearDates') IS NOT NULL
     DROP TABLE #currentYearDates
   IF OBJECT_ID('tempdb..#lastYearDates') IS NOT NULL
     DROP TABLE #lastYearDates

 ;with CTE as(
 select at.invno, ss.descrip typ,ac.speclty spec
 ,ISNULL(spm.AssignedPersonCode,at.salesmn) salesman
 ,at.invdte
 ,case when datepart(yy,at.invdte)=datepart(yy,getdate()) then 'current' when datepart(yy,at.invdte)=(datepart(yy,getdate())-1) then 'previous' end as yearPart
 ,at.custno
 ,ac.company
 ,CAST(ROUND(at.qtyshp, 0) AS INT) qty
 ,at.extprice amt
 ,case 
  when at.invdte BETWEEN @curr_year_strt_date AND @current_date then 'value1'
  when at.invdte BETWEEN @last_year_strt_date AND @last_year_curr_date then 'value2'
 end as condition
 from artran at
 INNER JOIN arcust ac on ac.custno = at.custno AND ac.custno <> 'ICADJ'
 INNER JOIN icitem i ON i.item = at.item 
 INNER JOIN npiprosys.dbo.sycrlst ss ON ss.chrvl = i.comcode AND ss.compid = '01' AND ss.ruleid = 'COMCODE'
  LEFT JOIN dbo.SalesPersonMapping as spm on spm.SalesPersonCode COLLATE Latin1_General_CI_AS=at.salesmn 
         AND
       (
   at.invdte BETWEEN @curr_year_strt_date AND @current_date
   OR
   at.invdte BETWEEN @last_year_strt_date and  @last_year_curr_date
        )
       where at.arstat not in ('X','V') AND at.item <> ''   
       AND 
    ltrim(rtrim(ac.speclty)) not in('OOT','LOSS PROD')
       AND 
      (at.invdte BETWEEN @curr_year_strt_date AND @current_date
   OR
   at.invdte BETWEEN @last_year_strt_date and  @last_year_curr_date
  ) and at.salesmn!=''
  )

  select * into #temp from CTE

  SELECT  DATEADD(DAY, nbr - 1, @curr_year_strt_date) [date] into #currentYearDates
  FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr
          FROM      sys.columns c
        ) nbrs
  WHERE   nbr - 1 <= DATEDIFF(DAY, @curr_year_strt_date, @current_date)

  
  SELECT  DATEADD(DAY, nbr - 1, @last_year_strt_date) [date] into #lastYearDates
  FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr
          FROM      sys.columns c
        ) nbrs
  WHERE   nbr - 1 <= DATEDIFF(DAY, @last_year_strt_date, @last_year_curr_date)

  select  q.comtype comtype ,max(q.salesman) salesman,max(q.speciality) speciality
  ,sum([current]) currentYear
  ,sum([previous]) previousYear
   into #revenue

  from (
   select typ comtype,max(spec) speciality,sum(qty) ItemShipped,sum(amt) revenue ,
   max(salesman) Salesman,max(condition) Value,max(yearPart) yearPart from #temp 
   group by yearPart,salesman,spec,typ)c
    pivot
    (
     sum(revenue) for yearPart in([current],[previous])

       ) as q 
      
  group by q.salesman,q.speciality,q.comtype
  order by q.Salesman,q.speciality

  select  q.comtype comtype ,max(q.salesman) salesman,max(q.speciality) speciality,sum([current]) currentYear
  ,sum([previous]) previousYear
  into #shipped

  from (
   select typ comtype,max(spec) speciality,sum(qty) ItemShipped,sum(amt) revenue ,
   max(salesman) Salesman,max(condition) Value,max(yearPart) yearPart   from #temp 
   group by yearPart,salesman,spec,typ)c
    pivot
    (
     sum(ItemShipped) for yearPart in([current],[previous])

       ) as q 
      
   group by q.salesman,q.speciality,q.comtype
  order by q.Salesman,q.speciality

 select * into #temp2  from #temp left join SalesPersonMapping as spm on spm.AssignedPersonCode =#temp.salesman and
  (
   exists(select * from #currentYearDates A cross join (SELECT  DATEADD(DAY, nbr - 1, spm.StartDate) [date] FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr FROM      sys.columns c
   ) nbrs WHERE   nbr - 1 <= DATEDIFF(DAY, spm.StartDate, isnull(spm.EndDate,@current_date))) B
   where A.date=B.date)

   or
    exists(select * from #lastYearDates A cross join (SELECT  DATEADD(DAY, nbr - 1, spm.StartDate) [date] FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr FROM      sys.columns c
   ) nbrs WHERE   nbr - 1 <= DATEDIFF(DAY, spm.StartDate, isnull(spm.EndDate,@current_date))) B
   where A.date=B.date)

  

  )
  select  q.comtype comtype ,max(q.salesman) salesman,max(q.speciality) speciality,max([current]) currentYear
  ,max([previous]) previousYear
  into #persons

  from (
    select distinct typ comtype,max(spec) speciality,salesman,max(yearPart) yearPart,
    employee= STUFF(
    (SELECT DISTINCT '|' + rtrim(ltrim(sm.SalesPersonDescription))+' ('+rtrim(ltrim(sm.SalesPersonCode))+')'
    FROM SalesPersonMapping sm
     WHERE rtrim(ltrim(sm.AssignedPersonCode)) = rtrim(ltrim(#temp2.salesman))
    FOR XML PATH ('')) , 1, 1, '')  ,max(condition) Value
   from #temp2
   group by yearPart,salesman,spec,typ)c
    pivot
    (
     max(employee) for yearPart in([current],[previous])

       ) as q 
      
   group by q.salesman,q.speciality,q.comtype
  order by q.Salesman,q.speciality


 
   select a.comtype as Comodity, a.salesman as SalesPersonCode, a.speciality as Category
  , a.currentYear revenueCurrentYear
  ,a.previousYear revenuePreviousYear
  ,c.currentYear casesCurrentYear
  ,c.previousYear casesPreviousYear
  ,b.currentYear,b.previousYear
    from #revenue a inner join #persons b 
   on
   a.comtype=b.comtype and a.salesman=b.salesman and a.speciality=b.speciality 
   inner join #shipped c
   on
   a.comtype=c.comtype and a.salesman=c.salesman and a.speciality=c.speciality 
 


   
";

            }



        }




        /// <summary>
        /// Generate the query to fetch data for the cases sold by location
        /// <param name="date"> Date </param>
        /// <returns> query string for the case sold report by location</returns>
        internal static string GetCasesSoldByLocation(DateTime date)
        {
            var query = string.Format(CasesSoldReportByLocation
                                    , date
                                  );
            return query;
        }

        private static string CasesSoldReportByLocation
        {
            get
            {
                return @"declare @current_date datetime ='{0}'
declare @curr_Month_strt_date datetime = (SELECT DATEADD(month, DATEDIFF(month, 0, @current_date), 0))
declare @last_month_curr_date datetime=(SELECT DateAdd(month, -1, Convert(date, @current_date)))
declare @last_month_strt_date datetime=(SELECT DATEADD(month, DATEDIFF(month, 0, @last_month_curr_date), 0))

declare @last_year_curr_date datetime =(SELECT DateAdd(year, -1, Convert(date, @current_date)))
declare @last_year_strt_date datetime = (SELECT DATEADD(month, DATEDIFF(month, 0, @last_year_curr_date), 0))


;with  cte as(
select ac.Type,sc.descrip,ac.speclty
 ,CAST(ROUND(at.qtyshp, 0) AS INT) CaseSold
,case when at.invdte BETWEEN  @curr_Month_strt_date AND @current_date then 'current' when at.invdte BETWEEN @last_month_strt_date
 AND @last_month_curr_date then 'prior'
else 'lastyear' end as Period

from artran at 
inner join arcust ac on ac.custno = at.custno
inner join icitem it on it.item = at.item
inner join [NPIPROSYS].[dbo].[sycrlst] sc on sc.ruleid like 'COMCODE' and sc.compid = '01' and it.comcode = sc.chrvl
WHERE 
         ltrim(rtrim(ac.speclty)) not in('OOT','LOSS PROD') and
(
(at.invdte BETWEEN @curr_Month_strt_date AND @current_date) 
OR
(at.invdte BETWEEN @last_month_strt_date AND @last_month_curr_date) 
OR
(at.invdte BETWEEN @last_year_strt_date AND @last_year_curr_date) 
)
)



  select [Type] [Location],descrip Comodity,speclty category,sum([current]) [CasesCurrentMonth],sum([prior]) CasesPreviousMonth,sum([lastyear]) CasesPreviousYear
   from (
select [Type],descrip,speclty,Period,sum(CaseSold) CaseSold from cte
GROUP BY cte.Type,descrip,speclty,Period)c
pivot
    (
     sum(CaseSold) for Period in([current],[prior],[lastyear])

       ) as q
 group by q.type,q.descrip,q.speclty
order by q.speclty


";

            }

        }


        /// <summary>
        /// Generate the query to fetch data for the sales person dashboard
        /// <param name="date"> Date </param>
        /// <returns> query string for the sales person dashboard report</returns>
        internal static string GetCasesSoldRevenueSalesPersonDashBoard(DateTime date)
        {
            var query = string.Format(SalesPersonDashBoardReport
                                    , date
                                  );
            return query;
        }

        private static string SalesPersonDashBoardReport
        {
            get
            {
                return @"declare @current_date datetime ='{0}'
declare @curr_Month_strt_date datetime = (SELECT DATEADD(month, DATEDIFF(month, 0, @current_date),0))
declare @last_month_curr_date datetime=(SELECT DateAdd(month, -1, Convert(date, @current_date)))
declare @last_month_strt_date datetime=(SELECT DATEADD(month, DATEDIFF(month, 0, @last_month_curr_date), 0))

declare @last_year_curr_date datetime =(SELECT DateAdd(year, -1, Convert(date, @current_date)))
declare @last_year_strt_date datetime = (SELECT DATEADD(month, DATEDIFF(month, 0,@last_year_curr_date), 0))
declare @last_year_prev_curr_date datetime=(SELECT DateAdd(month, -1, Convert(date,@last_year_curr_date)))
declare @last_year_prev_strt_date datetime=(SELECT DATEADD(month, DATEDIFF(month, 0, @last_year_prev_curr_date), 0))

 IF OBJECT_ID('tempdb..#temp') IS NOT NULL
     DROP TABLE #temp
   IF OBJECT_ID('tempdb..#temp2') IS NOT NULL
     DROP TABLE #temp2
IF OBJECT_ID('tempdb..#temp3') IS NOT NULL
     DROP TABLE #temp3
   IF OBJECT_ID('tempdb..#persons') IS NOT NULL
     DROP TABLE #persons
   IF OBJECT_ID('tempdb..#revenue') IS NOT NULL
     DROP TABLE #revenue
   IF OBJECT_ID('tempdb..#shipped') IS NOT NULL
     DROP TABLE #shipped
   IF OBJECT_ID('tempdb..#currentMonthdates') IS NOT NULL
     DROP TABLE #currentMonthdates
   IF OBJECT_ID('tempdb..#lastMonthdates') IS NOT NULL
     DROP TABLE #lastMonthdates
   IF OBJECT_ID('tempdb..#lastyearcurrent') IS NOT NULL
     DROP TABLE #lastyearcurrent
   IF OBJECT_ID('tempdb..#lastyearprevious') IS NOT NULL
     DROP TABLE #lastyearprevious

 ;with CTE as(
select at.invno, ss.descrip typ,spm.Speciality spec
 ,ISNULL(spm.AssignedPersonCode,at.salesmn) salesman
 ,at.invdte
  ,datepart(mm,at.invdte) monthPart
 ,case when datepart(yy,at.invdte)=datepart(yy,getdate()) then 'current' when datepart
(yy,at.invdte)=(datepart(yy,getdate())-1) then 'previous' end as yearPart
 ,at.custno
 ,ac.company
 ,CAST(ROUND(at.qtyshp, 0) AS INT) qty
 ,at.extprice amt
 ,case 
  when at.invdte BETWEEN @curr_Month_strt_date AND @current_date then 'value1'
  when at.invdte BETWEEN @last_month_strt_date AND @last_month_curr_date then 'value2'
  when at.invdte BETWEEN @last_year_strt_date AND @last_year_curr_date then 'value3'
  when at.invdte BETWEEN @last_year_prev_strt_date AND @last_year_prev_curr_date then 'value4'
 end as condition 
from SalesPersonMapping spm 
inner join artran at on( spm.SalesPersonCode COLLATE Latin1_General_CI_AS=at.salesmn OR   spm.AssignedPersonCode COLLATE Latin1_General_CI_AS=at.salesmn 
  )   AND
       (
         at.invdte between spm.StartDate and ISNULL(spm.endDate,@current_date)
         OR
         at.invdte between spm.StartDate and ISNULL(spm.endDate,@last_month_curr_date)           
         OR
         at.invdte between spm.StartDate and ISNULL(spm.endDate,@last_year_curr_date)
         OR 
         at.invdte between spm.StartDate and ISNULL(spm.endDate,@last_year_prev_curr_date)
        )
INNER JOIN arcust ac on ac.custno = at.custno AND ac.custno <> 'ICADJ'
INNER JOIN icitem i ON i.item = at.item 
INNER JOIN npiprosys.dbo.sycrlst ss ON ss.chrvl = i.comcode AND ss.compid = '01' AND ss.ruleid = 
'COMCODE'
 
where
 
(at.invdte BETWEEN @curr_Month_strt_date AND @current_date
   OR
  at.invdte BETWEEN @last_month_strt_date and  @last_month_curr_date
   or
  at.invdte BETWEEN @last_year_strt_date AND @last_year_curr_date
   or
  at.invdte BETWEEN @last_year_prev_strt_date AND @last_year_prev_curr_date
  )
and
at.arstat not in ('X','V') AND at.item <> ''    
AND ltrim(rtrim(ac.speclty)) not in('OOT','LOSS PROD')
and ltrim(rtrim(spm.Speciality)) not in('OSS')
and at.salesmn<>''

 )
  select * into #temp from CTE

  SELECT  DATEADD(DAY, nbr - 1, @curr_Month_strt_date) [date] into #currentMonthdates
  FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr
          FROM      sys.columns c
        ) nbrs
  WHERE   nbr - 1 <= DATEDIFF(DAY, @curr_Month_strt_date, @current_date)

  
  SELECT  DATEADD(DAY, nbr - 1, @last_month_strt_date) [date] into #lastMonthdates
  FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr
          FROM      sys.columns c
        ) nbrs
  WHERE   nbr - 1 <= DATEDIFF(DAY, @last_month_strt_date, @last_month_curr_date)


  SELECT  DATEADD(DAY, nbr - 1, @last_year_strt_date) [date] into #lastyearcurrent
  FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr
          FROM      sys.columns c
        ) nbrs
  WHERE   nbr - 1 <= DATEDIFF(DAY, @last_year_strt_date, @last_year_curr_date)

  SELECT  DATEADD(DAY, nbr - 1, @last_year_prev_strt_date) [date] into #lastyearprevious
  FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr
          FROM      sys.columns c
        ) nbrs
  WHERE   nbr - 1 <= DATEDIFF(DAY, @last_year_prev_strt_date, @last_year_prev_curr_date)

    select  q.comtype comtype ,max(q.salesman) salesman,max(q.speciality) speciality
  ,sum([current]) currentYear
  ,sum([previous]) previousYear,max(monthPart) monthPart
   into #revenue

  from (
   select typ comtype,max(spec) speciality,sum(qty) ItemShipped,sum(amt) revenue ,
   max(salesman) Salesman,max(condition) Value,monthPart,max(yearPart) yearPart from #temp 
   group by yearPart,monthPart,salesman,spec,typ)c
    pivot
    (
     sum(revenue) for yearPart in([current],[previous])

       ) as q
 group by q.monthPart,q.salesman,q.speciality,q.comtype
  order by q.Salesman,q.speciality

  select  q.comtype comtype ,max(q.salesman) salesman,max(q.speciality) speciality,sum([current]) 
currentYear
  ,sum([previous]) previousYear,max(monthPart) monthPart
  into #shipped

  from (
   select typ comtype,max(spec) speciality,sum(qty) ItemShipped,sum(amt) revenue ,
   max(salesman) Salesman,max(condition) Value,monthPart,max(yearPart) yearPart   from #temp 
   group by yearPart,monthPart,salesman,spec,typ)c
    pivot
    (
     sum(ItemShipped) for yearPart in([current],[previous])

       ) as q 
      
   group by q.monthPart,q.salesman,q.speciality,q.comtype
  order by q.Salesman,q.speciality

  select * into #temp2  from #temp left join SalesPersonMapping as spm on spm.AssignedPersonCode 
=#temp.salesman and 
  (
   exists(select * from #currentMonthdates A cross join (SELECT  DATEADD(DAY, nbr - 1,  
spm.StartDate) [date] FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr FROM    
  sys.columns c
   ) nbrs WHERE   nbr - 1 <= DATEDIFF(DAY,  spm.StartDate, isnull(spm.EndDate,@current_date))) B
   where A.date=B.date)

   or
    exists(select * from #lastMonthdates A cross join (SELECT  DATEADD(DAY, nbr - 1, spm.StartDate) 
[date] FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr FROM      sys.columns 
c
   ) nbrs WHERE   nbr - 1 <= DATEDIFF(DAY, spm.StartDate, isnull(spm.EndDate,@current_date))) B
   where A.date=B.date)

    or
    exists(select * from #lastyearcurrent A cross join (SELECT  DATEADD(DAY, nbr - 1, 
spm.StartDate) [date] FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr FROM    
  sys.columns c
   ) nbrs WHERE   nbr - 1 <= DATEDIFF(DAY, spm.StartDate, isnull(spm.EndDate,@current_date))) B
   where A.date=B.date)

    or
    exists(select * from #lastyearprevious A cross join (SELECT  DATEADD(DAY, nbr - 1, 
spm.StartDate) [date] FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY c.object_id ) AS Nbr FROM    
  sys.columns c
   ) nbrs WHERE   nbr - 1 <= DATEDIFF(DAY, spm.StartDate, isnull(spm.EndDate,@current_date))) B
   where A.date=B.date)
    )AND (#temp.invdte between spm.StartDate and isnull(spm.EndDate,getdate()))

	select yearpart,monthpart,spec,typ,#temp2.AssignedPersonCode,#temp2.SalesPersonCode,
  max(ltrim(rtrim(spm.SalesPersonDescription)))+' ('+max(rtrim(ltrim(spm.SalesPersonCode)))+')' 
SalesManName,(condition) condition
  into #temp3
   from SalesPersonMapping spm
  left join #temp2 on spm.AssignedPersonCode=#temp2.salesman and (#temp2.invdte between 
spm.StartDate and isnull(spm.EndDate,getdate()))
   where spm.SalesPersonCode=#temp2.SalesPersonCode and spm.AssignedPersonCode=#temp2.salesman
  group by yearPart,monthPart, #temp2.AssignedPersonCode,#temp2.SalesPersonCode,spec,typ,condition

  select  q.comtype comtype ,max(q.AssignedPersonCode) salesman,max(q.speciality) speciality,max
([current]) currentYear
  ,max([previous]) previousYear,max(monthPart) monthPart ,(select top 1 d.SalesPersonDescription 
from SalesPersonMapping d where d.AssignedPersonCode=q.AssignedPersonCode and EndDate is null) 
ActiveEmployee
  into #persons
    from (
    select distinct typ comtype,max(spec) speciality,AssignedPersonCode,max(yearPart) yearPart 
,monthPart,
    employee= STUFF(
    (SELECT DISTINCT '|' + rtrim(ltrim(sm.SalesManName))
    FROM #temp3 sm
     WHERE rtrim(ltrim(sm.AssignedPersonCode)) = rtrim(ltrim(#temp3.AssignedPersonCode))  and 
sm.condition=#temp3.condition
    FOR XML PATH ('')) , 1, 1, '')  ,(condition) Value 
   from #temp3
   group by yearPart,monthPart,#temp3.AssignedPersonCode,spec,typ,condition)c
    pivot
    (
     max(employee) for yearPart in([current],[previous])

       ) as q 
      
   group by q.monthPart,q.AssignedPersonCode,q.speciality,q.comtype
  order by q.AssignedPersonCode,q.speciality

select a.comtype as Comodity, a.salesman as SalesPersonCode, a.speciality as Category
  , a.currentYear revenueCurrentYear
  ,a.previousYear revenuePreviousYear
  ,c.currentYear casesCurrentYear
  ,c.previousYear casesPreviousYear
  ,b.currentYear,b.previousYear,b.monthPart,b.ActiveEmployee
    from #revenue a inner join #persons b 
   on
   a.comtype=b.comtype and a.salesman=b.salesman and a.speciality=b.speciality and 
a.monthPart=b.monthPart
   inner join #shipped c
   on
   a.comtype=c.comtype and a.salesman=c.salesman and a.speciality=c.speciality and 
a.monthPart=c.monthPart

";

            }

        }

        /// <summary>
        /// Generate the query to fetch data for the out sales sales person dashboard
        /// <param name="date"> Date </param>
        /// <returns> query string for the out sales sales person dashboard report</returns>
        internal static string GetOutSalesCasesSoldRevenueSalesPersonDashBoard(DateTime date)
        {
            var query = string.Format(OutSalesSalesPersonDashBoardReport
                                    , date
                                  );
            return query;
        }

        private static string OutSalesSalesPersonDashBoardReport
        {
            get
            {
                return @"declare @current_date datetime ='{0}'

declare @curr_Month_strt_date datetime = (SELECT DATEADD(month, DATEDIFF(month, 0, @current_date), 0))
declare @last_month_curr_date datetime=(SELECT DateAdd(month, -1, Convert(date, @current_date)))
declare @last_month_strt_date datetime=(SELECT DATEADD(month, DATEDIFF(month, 0, @last_month_curr_date), 0))

declare @last_year_curr_date datetime =(SELECT DateAdd(year, -1, Convert(date, @current_date)))
declare @last_year_strt_date datetime = (SELECT DATEADD(month, DATEDIFF(month, 0, @last_year_curr_date), 0))
declare @last_year_prev_curr_date datetime=(SELECT DateAdd(month, -1, Convert(date, @last_year_curr_date)))
declare @last_year_prev_strt_date datetime=(SELECT DATEADD(month, DATEDIFF(month, 0, @last_year_prev_curr_date), 0))

 
 ;with CTE as(
select at.invno, ss.descrip typ,spm.Speciality spec
 ,ISNULL(spm.AssignedPersonCode,at.salesmn) salesman
 ,at.invdte
  ,datepart(mm,at.invdte) monthPart
 ,datepart(yyyy,at.invdte)  yearPart
 ,at.custno
 ,ac.company
 ,CAST(ROUND(at.qtyshp, 0) AS INT) qty
 ,at.extprice amt
 ,DATEDIFF(WEEK, DATEADD(MONTH, DATEDIFF(MONTH, 0, at.invdte), 0), at.invdte) +1  WeekNo
from SalesPersonMapping spm 
inner join artran at on( spm.SalesPersonCode COLLATE Latin1_General_CI_AS=at.salesmn OR   spm.AssignedPersonCode COLLATE Latin1_General_CI_AS=at.salesmn 
  )   AND
       (
         at.invdte between spm.StartDate and ISNULL(spm.endDate,@current_date)
         OR
         at.invdte between spm.StartDate and ISNULL(spm.endDate,@last_month_curr_date)           
         OR
         at.invdte between spm.StartDate and ISNULL(spm.endDate,@last_year_curr_date)
         OR 
         at.invdte between spm.StartDate and ISNULL(spm.endDate,@last_year_prev_curr_date)
        )
INNER JOIN arcust ac on ac.custno = at.custno AND ac.custno <> 'ICADJ'
INNER JOIN icitem i ON i.item = at.item 
INNER JOIN npiprosys.dbo.sycrlst ss ON ss.chrvl = i.comcode AND ss.compid = '01' AND ss.ruleid = 
'COMCODE'
 
where
 
(at.invdte BETWEEN @curr_Month_strt_date AND @current_date
   OR
  at.invdte BETWEEN @last_month_strt_date and  @last_month_curr_date
   or
  at.invdte BETWEEN @last_year_strt_date AND @last_year_curr_date
   or
  at.invdte BETWEEN @last_year_prev_strt_date AND @last_year_prev_curr_date
  )
and
at.arstat not in ('X','V') AND at.item <> ''    
AND ltrim(rtrim(ac.speclty)) not in('OOT','LOSS PROD')
and ltrim(rtrim(spm.Speciality)) in('OSS')
and at.salesmn<>''

 )

   select typ Comodity,salesman SalesPersonCode,yearPart,monthPart,WeekNo,sum(qty) CasesCurrentYear,sum(amt) revenueCurrentYear, 
   (select top 1 d.SalesPersonDescription from SalesPersonMapping d where d.AssignedPersonCode=salesman and EndDate is null) ActiveEmployee 
   from CTE
   group by typ,salesman,yearPart,monthPart,WeekNo
  
";

            }

        }
        #endregion


        public static string GetCategories(string CatName)
        {
            var query = "";
            if (CatName!="")
            {
                 query = string.Format("select * from mtrCategory where category like '%'+ltrim(rtrim('{0}'))+'%'"
                                      , CatName

                                     );
            }
            else
            {
                 query = string.Format("select * from mtrCategory where category"
                                      , CatName

                                     );
            }
           
            return query;
        }



    }

}

