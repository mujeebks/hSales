using Nogales.BusinessModel;
using Nogales.DataProvider.Infrastructure;
using Nogales.DataProvider.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using Nogales.DataProvider.ENUM;


namespace Nogales.DataProvider
{
    public class SalesDataProvider : DataAccessADO
    {
        //03/03/2017
        //public async Task<SalesChartBM> GetTotalByMonth(string dateFilter, bool isSynced = false)
        //{

        //    var filterDate = DateTime.Parse(dateFilter);

        //    //#if DEBUG
        //    //            filterDate = filterDate.AddMonths(-1);
        //    //#endif
        //    var numberOfDays = (filterDate.Date - new DateTime(filterDate.Year, filterDate.Month, 1).Date).TotalDays;
        //    var endDate = filterDate.Date.ToString("yyyy-MM-ddTHH:mm:ss");
        //    var startDate = new DateTime(filterDate.Year, filterDate.Month, 1).Date.ToString("yyyy-MM-ddTHH:mm:ss");

        //    /// Is synced then the argument "isFirstTime" should be false.
        //    var targets = this.GetSalesTargetData(!isSynced);

        //    string sqlString = MDXCubeQueries.GetSalesQuery(startDate, endDate, true);

        //    var dataTableResult = await Task.Run(() => base.GetDataTable(sqlString));

        //    var listTop5Result = dataTableResult
        //                         .AsEnumerable()
        //                         .Select(x => new SalesMapper
        //                         {
        //                             SalesPerson = x.Field<string>(0),
        //                             Customer = x.Field<string>(4),
        //                             Type = x.Field<string>(5),
        //                             SalesAmount = x.Field<double?>(6),
        //                         })
        //                         .Where(x => x.SalesPerson.Trim() != string.Empty)
        //                         .ToList();

        //    sqlString = MDXCubeQueries.GetSalesQuery(startDate, endDate, false);

        //    dataTableResult = await Task.Run(() => base.GetDataTable(sqlString));

        //    var listBottom5Result = dataTableResult
        //                        .AsEnumerable()
        //                        .Select(x => new SalesMapper
        //                        {
        //                            SalesPerson = x.Field<string>(0),
        //                            Customer = x.Field<string>(4),
        //                            Type = x.Field<string>(5),
        //                            SalesAmount = x.Field<double?>(6),
        //                        })
        //                        .Where(x => x.SalesPerson.Trim() != string.Empty)
        //                        .ToList();

        //    var result = new SalesChartBM
        //    {
        //        Top = listTop5Result.GroupBy(x => x.SalesPerson)
        //                               .Select((x, index) => new SalesSubDataBM
        //                               {
        //                                   ColumnName = x.Key,
        //                                   ColumnPoint = x.Select(y => y.Customer).Distinct().Count(),
        //                                   ColumnPointTarget = (int?)targets.Where(t => t.SalesPerson == x.Key).FirstOrDefault().CustomerCountTarget * (int)numberOfDays,
        //                                   ColumnValue = Math.Round(x.Sum(y => y.SalesAmount).Value, 2),
        //                                   ColumnValueTarget = (double?)targets.Where(t => t.SalesPerson == x.Key).FirstOrDefault().SalesTarget * numberOfDays,
        //                                   Color = ChartColorBM.Colors[index]
        //                               })
        //                               .ToList(),
        //        Bottom = listBottom5Result.GroupBy(x => x.SalesPerson)
        //                                  .Select((x, index) => new SalesSubDataBM
        //                                  {
        //                                      ColumnName = x.Key,
        //                                      ColumnPoint = x.Select(y => y.Customer).Distinct().Count(),
        //                                      ColumnPointTarget = (int?)targets.Where(t => t.SalesPerson == x.Key).FirstOrDefault().CustomerCountTarget * (int)numberOfDays,
        //                                      ColumnValue = Math.Round(x.Sum(y => y.SalesAmount).Value, 2),
        //                                      ColumnValueTarget = (double?)targets.Where(t => t.SalesPerson == x.Key).FirstOrDefault().SalesTarget * numberOfDays,
        //                                      Color = ChartColorBM.Colors[index]
        //                                  })
        //                                  .ToList(),
        //    };
        //    return result;
        //}

        public SalesCustomerBySalesPersonChartBM GetSalesBySalesPerson(string startDate, string endDate, bool isSynced = false)
        {
            //GetSalesTargetData();
            var startDateObj = DateTime.Parse(startDate);
            var endDateObj = DateTime.Parse(endDate);
            var numberOfDays = (endDateObj.Date - startDateObj.Date).TotalDays;

            var startDateFormatted = startDateObj.Date.ToString("yyyy/MM/dd");
            var endDateFormatted = endDateObj.Date.ToString("yyyy/MM/dd");

            var priorStartDateFormatted = startDateObj.AddMonths(-1).Date.ToString("yyyy/MM/dd");
            var priorEndDateFormatted = endDateObj.AddMonths(-1).Date.ToString("yyyy/MM/dd");

            /// Is synced then the argument "isFirstTime" should be false.
            //var targets = this.GetSalesTargetData(!isSynced);

            //string sqlString = MDXCubeQueries.GetSalesQuery(startDateFormatted, endDateFormatted, true);
            using (var adoDataAccess = new DataAccessADO())
            {
                var sqlQuery = SQLQueries.GetSalesReportOfSalesPersonQuery(new List<string>()
                                                                            , startDateFormatted
                                                                            , endDateFormatted
                                                                            , priorStartDateFormatted
                                                                            , priorEndDateFormatted);

                var data = adoDataAccess.ReadToDataSet(sqlQuery);

                //var dataTableResult = base.GetDataTable(sqlString);

                if (data != null && data.Tables[0].AsEnumerable().Count() > 0)
                {
                    var dataResult = data.Tables[0]
                                        .AsEnumerable()
                                        .Select(x => new SalesMapper
                                        {
                                            SalesPerson = x.Field<string>("SalesPersonCode"),
                                            SalesPersonDescription = x.Field<string>("SalesPersonDescription"),
                                            AssignedPersonCode = x.Field<string>("AssignedPersonCode"),
                                            NoOfCustomer = x.Field<int>("CustomerCount"),
                                            SalesAmountCurrent = x.Field<decimal?>("TotalSalesAmount").HasValue ? x.Field<decimal?>("TotalSalesAmount").Value : 0,
                                            SalesAmountPrior = x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value : 0,
                                            Percentage = x.Field<decimal?>("PercentageDifference").HasValue ? x.Field<decimal?>("PercentageDifference").Value : 0,
                                            //SalesPerson = x.Field<string>("SalesPerson"),
                                            ////SalesAmount = x.Field<double?>("AmtSales").Value.ToRoundTwoDigits(),
                                            //SalesAmount = Convert.ToDouble(decimal.Parse(x.Field<decimal?>("AmtSales").HasValue ? x.Field<decimal>("AmtSales").ToString() : ""))
                                        })
                                        .Where(x => x.SalesPerson.Trim() != string.Empty)
                                        .ToList();

                    var listTop5Result = dataResult.OrderByDescending(x => x.SalesAmountCurrent).Take(5).ToList();

                    var listBottom5Result = dataResult.OrderBy(x => x.SalesAmountCurrent).Take(5).ToList();
                    ////sqlString = MDXCubeQueries.GetSalesQuery(startDateFormatted, endDateFormatted, false);

                    ////dataTableResult = base.GetDataTable(sqlString);

                    //var bottomData = adoDataAccess.ReadToDataSet(SQLQueries.GetSalesQuery(startDateFormatted, endDateFormatted, false));

                    //var listBottom5Result = bottomData.Tables[0]
                    //                    .AsEnumerable()
                    //                    .Select(x => new SalesMapper
                    //                    {
                    //                        SalesPerson = x.Field<string>("SalesPerson"),
                    //                        Customer = x.Field<string>("Customer"),
                    //                        Type = x.Field<string>("type"),
                    //                        //SalesAmount = x.Field<double?>("AmtSales").Value.ToRoundTwoDigits(),
                    //                        SalesAmount = Convert.ToDouble(decimal.Parse(x.Field<decimal?>("AmtSales").HasValue ? x.Field<decimal?>("AmtSales").Value.ToString() : ""))
                    //                    })
                    //                    .Where(x => x.SalesPerson.Trim() != string.Empty)
                    //                    .ToList();

                    var result = new SalesCustomerBySalesPersonChartBM
                    {
                        Top = listTop5Result.GroupBy(x => x.SalesPerson)
                                               .Select((x, index) => new SalesCustomerBySalesPersonSubDataBM
                                               {
                                                   ColumnName = x.Key,
                                                   ColumnPoint = null,
                                                   //Sales Target
                                                   ColumnPointTarget = 0,//(double?)targets.Where(t => t.SalesPerson == x.Key.Trim()).FirstOrDefault().SalesTarget * numberOfDays,
                                                   ColumnValue = x.Sum(y => y.SalesAmount).Value.ToRoundTwoDigits(),
                                                   //Previous year
                                                   ColumnValueTarget = 0,// (double?)targets.Where(t => t.SalesPerson == x.Key.Trim()).FirstOrDefault().SalesTarget * numberOfDays,
                                                   Color = ChartColorBM.Colors[index],
                                                   SubData = new SalesCustomerBySalesPersonChartBM
                                                   {
                                                       Top = x.OrderByDescending(y => y.SalesAmount)
                                                               .Take(10)
                                                              .Select((y, indexSub) => new SalesCustomerBySalesPersonSubDataBM
                                                              {
                                                                  Color = ChartColorBM.Colors[indexSub],
                                                                  ColumnName = y.SalesPerson,
                                                                  ColumnPoint = null,
                                                                  ColumnPointTarget = null,
                                                                  ColumnValue = y.SalesAmount.Value.ToRoundTwoDigits(),
                                                                  ColumnValueTarget = null,
                                                                  SubData = new SalesCustomerBySalesPersonChartBM
                                                                  {
                                                                      Top = x.GroupBy(t => t.Type)
                                                                             .Select((z, indexSub1) => new SalesCustomerBySalesPersonSubDataBM
                                                                             {
                                                                                 Color = ChartColorBM.Colors[indexSub1],
                                                                                 ColumnName = z.Key,
                                                                                 ColumnValue = z.Sum(g => g.SalesAmount).Value.ToRoundTwoDigits(),
                                                                                 ColumnPoint = null //Assigned null
                                                                             }).ToList(),
                                                                  }
                                                              })
                                                             .ToList(),
                                                       Bottom = x.OrderBy(y => y.SalesAmount)
                                                              .Take(10)
                                                             .Select((y, indexSub) => new SalesCustomerBySalesPersonSubDataBM
                                                             {
                                                                 Color = ChartColorBM.Colors[indexSub],
                                                                 ColumnName = y.SalesPerson,
                                                                 ColumnPoint = null,
                                                                 ColumnPointTarget = null,
                                                                 ColumnValue = y.SalesAmount.Value.ToRoundTwoDigits(),
                                                                 ColumnValueTarget = null,
                                                                 SubData = new SalesCustomerBySalesPersonChartBM
                                                                 {
                                                                     Top = x.GroupBy(t => t.Type)
                                                                            .Select((z, indexSub1) => new SalesCustomerBySalesPersonSubDataBM
                                                                            {
                                                                                Color = ChartColorBM.Colors[indexSub1],
                                                                                ColumnName = z.Key,
                                                                                ColumnValue = z.Sum(g => g.SalesAmount).Value.ToRoundTwoDigits(),
                                                                                ColumnPoint = null //Assigned null
                                                                            }).ToList(),
                                                                 }
                                                             })
                                                            .ToList(),
                                                   }
                                               })
                                               .ToList(),


                        Bottom = listBottom5Result.GroupBy(x => x.SalesPerson)
                                                  .Select((x, index) => new SalesCustomerBySalesPersonSubDataBM
                                                  {
                                                      ColumnName = x.Key,
                                                      ColumnPoint = null, //Assigned null value
                                                      ColumnPointTarget = 0,//targets.Where(t => t.SalesPerson == x.Key.Trim()) != null ? (double?)targets.Where(t => t.SalesPerson == x.Key.Trim()).FirstOrDefault().SalesTarget * (int)numberOfDays : 0,
                                                      ColumnValue = x.Sum(y => y.SalesAmount).Value.ToRoundTwoDigits(),
                                                      ColumnValueTarget = 0,//targets.Where(t => t.SalesPerson == x.Key.Trim()) != null ? (double?)targets.Where(t => t.SalesPerson == x.Key.Trim()).FirstOrDefault().SalesTarget * numberOfDays : 0,
                                                      Color = ChartColorBM.Colors[index],
                                                      SubData = new SalesCustomerBySalesPersonChartBM
                                                      {
                                                          Top = x.OrderByDescending(y => y.SalesAmount)
                                                                  .Take(10)
                                                                 .Select((y, indexSub) => new SalesCustomerBySalesPersonSubDataBM
                                                                 {
                                                                     Color = ChartColorBM.Colors[indexSub],
                                                                     ColumnName = y.Customer,
                                                                     ColumnPoint = null,
                                                                     ColumnPointTarget = null,
                                                                     ColumnValue = y.SalesAmount.Value.ToRoundTwoDigits(),
                                                                     ColumnValueTarget = null,
                                                                     SubData = new SalesCustomerBySalesPersonChartBM
                                                                     {
                                                                         Top = x.GroupBy(t => t.Type)
                                                                                .Select((z, indexSub1) => new SalesCustomerBySalesPersonSubDataBM
                                                                                {
                                                                                    Color = ChartColorBM.Colors[indexSub1],
                                                                                    ColumnName = z.Key.Trim(),
                                                                                    ColumnValue = z.Sum(g => g.SalesAmount).Value.ToRoundTwoDigits(),
                                                                                    ColumnPoint = null //Assigned null
                                                                                }).ToList(),
                                                                     }
                                                                 })
                                                                .ToList(),
                                                          Bottom = x.OrderBy(y => y.SalesAmount)
                                                                 .Take(10)
                                                                .Select((y, indexSub) => new SalesCustomerBySalesPersonSubDataBM()
                                                                {
                                                                    Color = ChartColorBM.Colors[indexSub],
                                                                    ColumnName = y.Customer,
                                                                    ColumnPoint = null,
                                                                    ColumnPointTarget = null,
                                                                    ColumnValue = y.SalesAmount.Value.ToRoundTwoDigits(),
                                                                    ColumnValueTarget = null,
                                                                    SubData = new SalesCustomerBySalesPersonChartBM
                                                                    {
                                                                        Top = x.GroupBy(t => t.Type)
                                                                               .Select((z, indexSub1) => new SalesCustomerBySalesPersonSubDataBM()
                                                                               {
                                                                                   Color = ChartColorBM.Colors[indexSub1],
                                                                                   ColumnName = z.Key,
                                                                                   ColumnValue = z.Sum(g => g.SalesAmount).Value.ToRoundTwoDigits(),
                                                                                   ColumnPoint = null //Assigned null
                                                                               }).ToList(),
                                                                    }
                                                                })
                                                               .ToList(),
                                                      }
                                                  })
                                               .ToList(),
                    };
                    return result;
                }
                else
                {
                    throw new Exception("No records in the database");
                }
            }
        }

        /// <summary>
        /// Get sales analysis report
        /// Uses BI_GetCustomerReport to generate data from database
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<SalesAnalysisBM> GetSalesAnalysisReport(SalesAnalysisFilter filter)
        {
            var dataSetResult = new DataSet();

            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@startDate", filter.StartDate));
            parameterList.Add(new SqlParameter("@endDate", filter.EndDate));
            parameterList.Add(new SqlParameter("@poNumber", filter.PurchaseOrderNumber == null ?string.Empty : filter.PurchaseOrderNumber + ""));
            parameterList.Add(new SqlParameter("@itemNumber", filter.ItemNumber == null ? string.Empty : filter.ItemNumber + ""));
            parameterList.Add(new SqlParameter("@buyer", filter.Buyer?? string.Empty));

            dataSetResult = base.ReadToDataSetViaProcedure("BI_GetSalesAnalysisReport", parameterList.ToArray());

            var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new SalesAnalysisBM
                            {
                                Buyer = x.Field<string>("buyer"),
                                Description = x.Field<string>("descrip"),
                                ExtendedPrice = x.Field<decimal>("extprice"),
                                ItemNumber = x.Field<string>("itemjoin"),
                                Margin = x.Field<decimal>("margin"),
                                //MarginPercentage = x.Field<decimal?>("MarginPercentage"),
                                PurchaseOrder = x.Field<string>("purno"),
                                QuantityReceived = x.Field<decimal>("quantity"),
                                QuantityShipped = x.Field<decimal>("qtyshp"),
                            })
                            .ToList();

            return result;
        }

        public List<SalesAnalysisBM> GetSalesAnalysisReportSalesPerson(SalesAnalysisFilter filter)
        {
            var dataSetResult = new DataSet();

            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@startDate", filter.StartDate));
            parameterList.Add(new SqlParameter("@endDate", filter.EndDate));

            dataSetResult = base.ReadToDataSetViaProcedure("BI_GetSalesAnalysisReportBySalesPerson", parameterList.ToArray());

            var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new SalesAnalysisBM
                            {
                                SalesPerson = x.Field<string>("salesmn"),
                                NumberOfInvoices = x.Field<int>("invoices"),
                                QuantityShipped = x.Field<decimal>("qtyshp"),
                                Lines = x.Field<int>("lines"),
                                ExtendedPrice = x.Field<decimal>("extprice"),
                                Margin = x.Field<decimal>("margin"),
                                //MarginPercentage = x.Field<decimal?>("marginPercentage"),
                            })
                            .ToList();

            return result;
        }

        public List<SalesMarginReportBM> GetSalesMarginReport(SalesAnalysisFilter filter,string userId)
        {

            AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
            var userAccessibleCategories = _adminUserManagementDateProvider.GetUserAccess(userId);

            var dataSetResult = new DataSet();

            List<SqlParameter> parameterList = new List<SqlParameter>();
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("Code");
            if (filter.item != null)
            {
                foreach (string code in filter.item)
                {
                    dtItem.Rows.Add(code);
                }
            }

            DataTable dtCategories = new DataTable();
            dtCategories.Columns.Add("Code");
            foreach (var category in userAccessibleCategories.Categories)
            {
                if (category.IsAccess == true) { dtCategories.Rows.Add(category.Name); }
            }
        

            var param = new SqlParameter();
            param.SqlDbType = SqlDbType.Structured;
            param.Value = dtItem;
            param.ParameterName = "@item";
            parameterList.Add(param);

            param = new SqlParameter();
            param.SqlDbType = SqlDbType.Structured;
            param.Value = dtCategories;
            param.ParameterName = "@categories";
            parameterList.Add(param);

            parameterList.Add(new SqlParameter("@startDate", filter.StartDate));
            parameterList.Add(new SqlParameter("@endDate", filter.EndDate));
            parameterList.Add(new SqlParameter("@reportName", filter.ReportName));
            parameterList.Add(new SqlParameter("@marginLowerLimit", filter.LowerMarginLimit));
            parameterList.Add(new SqlParameter("@marginUpperLimit", filter.UpperMarginLimit));

            dataSetResult = base.ReadToDataSetViaProcedure("BI_SL_GetSalesAnalysisReport", parameterList.ToArray());

            var result = new List<SalesMarginReportBM>();
            if(filter.ReportName == "EmployeeReport")
            {
                result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new SalesMarginReportBM
                            {
                                Employee = x.Field<string>("salesmn"),
                                //Customer = x.Field<string>("customer"),
                                InvoicesNumber = x.Field<int>("invoices"),
                                QuantityShipped = x.Field<decimal>("qtyship"),
                                //Lines = x.Field<int>("lines"),
                                ExtendedPrice = x.Field<decimal>("extprices"),
                                Margin = x.Field<decimal>("margin"),
                                MarginPercentage = x.Field<decimal?>("marginPercentage"),
                            })
                            .OrderByDescending(p => p.Margin)
                            .ToList();
            }
            else if(filter.ReportName == "CustomerReport")
            {
                result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new SalesMarginReportBM
                            {
                                Customer = x.Field<string>("custno").Trim(),
                                Description = x.Field<string>("company").Trim(),
                                Employee = x.Field<string>("salesmn").Trim(),
                                ExtendedPrice = x.Field<decimal>("extprices"),
                                QuantityShipped = x.Field<decimal>("qtyship"),
                                Margin = x.Field<decimal>("margin"),
                                MarginPercentage = x.Field<decimal?>("marginPercentage"),
                            })
                            .OrderByDescending(p=>p.Margin)
                            .ToList();
            }
            else if(filter.ReportName == "SalesOrderReport")
            {
                result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new SalesMarginReportBM
                            {
                                SalesOrder = x.Field<string>("sono").Trim(),
                                ExtendedPrice = x.Field<decimal>("extprices"),
                                Employee = x.Field<string>("salesmn").Trim(),
                                Customer = x.Field<string>("customer").Trim(),
                                QuantityShipped = x.Field<decimal>("qtyship"),
                                Margin = x.Field<decimal>("margin"),
                                MarginPercentage = x.Field<decimal?>("marginPercentage"),
                            })
                            .OrderByDescending(p => p.Margin)
                            .ToList();
            }
            else if(filter.ReportName == "PurchaseOrderReport")
            {
                result = dataSetResult.Tables[0]
                        .AsEnumerable()
                        .Select(x => new SalesMarginReportBM
                        {
                            PurchaseOrder = x.Field<string>("purno"),
                            Buyer = x.Field<string>("buyer"),
                            Item = x.Field<string>("item"),
                            Description = x.Field<string>("descrip"),
                            ExtendedPrice = x.Field<decimal>("extprices"),
                            //Employee = x.Field<string>("salesmn"),
                            //Customer = x.Field<string>("customer"),
                            QuantityShipped = x.Field<decimal>("qtyship"),
                            Margin = x.Field<decimal>("margin"),
                            MarginPercentage = x.Field<decimal?>("marginPercentage"),
                        })
                        .OrderByDescending(p => p.Margin)
                        .ToList();
            }
            else
            {
                throw new Exception("");
            }
            return result;
        }

        public List<SalesMarginReportBM> GetSalesMarginDetailedReport(SalesAnalysisFilter filter)
        {
            var dataSetResult = new DataSet();

            List<SqlParameter> parameterList = new List<SqlParameter>();
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("Code");
            foreach (string code in filter.item)
            {
                dtItem.Rows.Add(code);
            }
            var param = new SqlParameter();
            param.SqlDbType = SqlDbType.Structured;
            param.Value = dtItem;
            param.ParameterName = "@item";
            parameterList.Add(param);
            parameterList.Add(new SqlParameter("@startDate", filter.StartDate));
            parameterList.Add(new SqlParameter("@endDate", filter.EndDate));
            parameterList.Add(new SqlParameter("@marginLowerLimit", filter.LowerMarginLimit));
            parameterList.Add(new SqlParameter("@marginUpperLimit", filter.UpperMarginLimit));
            parameterList.Add(new SqlParameter("@employee", string.Empty));
            parameterList.Add(new SqlParameter("@customerNumber", string.Empty));
            parameterList.Add(new SqlParameter("@soNumber", string.Empty));
            parameterList.Add(new SqlParameter("@poNumber", string.Empty));

            switch (filter.ReportName)
            {
                case "EmployeeReport":
                    parameterList.First(x => x.ParameterName == "@employee").Value = filter.FilterValue;
                    break;
                case "CustomerReport":
                    parameterList.First(x => x.ParameterName == "@customerNumber").Value = filter.FilterValue;
                    break;
                case "SalesOrderReport":
                    parameterList.First(x => x.ParameterName == "@soNumber").Value = filter.FilterValue;
                    break;
                case "PurchaseOrderReport":
                    parameterList.First(x => x.ParameterName == "@poNumber").Value = filter.FilterValue;
                    break;
                default:
                    break;
            }


            dataSetResult = base.ReadToDataSetViaProcedure("BI_SL_GetSalesMarginDetailedReport", parameterList.ToArray());

            var result = dataSetResult.Tables[0]
                        .AsEnumerable()
                        .Select(x => new SalesMarginReportBM
                        {
                            Employee = x.Field<string>("salesmn"),
                            Customer = x.Field<string>("company"),
                            CustomerNumber = x.Field<string>("custno"),
                            InvoiceNumber = x.Field<string>("invno"),
                            InvoiceDate = x.Field<DateTime>("invdte"),
                            Item = x.Field<string>("item"),
                            Description = x.Field<string>("descrip"),
                            ExtendedPrice = x.Field<decimal>("extprice"),
                            QuantityShipped = x.Field<decimal>("qtyshp"),
                            Margin = x.Field<decimal>("margin"),
                            MarginPercentage = x.Field<decimal?>("marginPercentage"),
                        })
                        .OrderByDescending(p=>p.Margin)
                        .ToList();
                        
            return result;
        }

        /// <summary>
        /// Get Customer By Sales Person
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isSynced"></param>
        /// <returns></returns>

        //03/03/2017
        //public SalesCustomerBySalesPersonChartBM GetCustomerBySalesPerson(string startDate, string endDate, bool isSynced = false)
        //{
        //    //GetSalesTargetData();
        //    var startDateObj = DateTime.Parse(startDate);
        //    var endDateObj = DateTime.Parse(endDate);
        //    var numberOfDays = (endDateObj.Date - startDateObj.Date).TotalDays;

        //    var startDateFormatted = startDateObj.Date.ToString("yyyy-MM-ddTHH:mm:ss");
        //    var endDateFormatted = endDateObj.Date.ToString("yyyy-MM-ddTHH:mm:ss");

        //    /// Is synced then the argument "isFirstTime" should be false.
        //    var targets = this.GetSalesTargetData(!isSynced);

        //    string sqlString = MDXCubeQueries.GetSalesQuery(startDateFormatted, endDateFormatted, true);

        //    var dataTableResult = base.GetDataTable(sqlString);

        //    var listTop5Result = dataTableResult
        //        .AsEnumerable()
        //        .Select(x => new SalesMapper
        //        {
        //            SalesPerson = x.Field<string>(0),
        //            Customer = x.Field<string>(4),
        //            Type = x.Field<string>(5)
        //        }).Where(x => x.SalesPerson.Trim() != string.Empty).ToList();

        //    sqlString = MDXCubeQueries.GetSalesQuery(startDateFormatted, endDateFormatted, false);

        //    dataTableResult = base.GetDataTable(sqlString);

        //    var listBottom5Result = dataTableResult
        //                        .AsEnumerable()
        //                        .Select(x => new SalesMapper
        //                        {
        //                            SalesPerson = x.Field<string>(0),
        //                            Customer = x.Field<string>(4),
        //                            Type = x.Field<string>(5)
        //                        })
        //                        .Where(x => x.SalesPerson.Trim() != string.Empty)
        //                        .ToList();
        //    var result = new SalesCustomerBySalesPersonChartBM
        //    {
        //        Top = listTop5Result.GroupBy(x => x.SalesPerson)
        //                          .Select((x, index) => new SalesCustomerBySalesPersonSubDataBM
        //                          {
        //                              ColumnName = x.Key,
        //                              ColumnPoint = null,
        //                              ColumnPointTarget = x.Select(y => y.Customer).Distinct().Count(),
        //                              ColumnValue = x.Select(y => y.Customer).Distinct().Count(),
        //                              ColumnValueTarget = (int?)targets.Where(t => t.SalesPerson == x.Key).FirstOrDefault().CustomerCountTarget * numberOfDays,
        //                              Color = ChartColorBM.Colors[index],
        //                              SubData = new SalesCustomerBySalesPersonChartBM
        //                              {
        //                                  Top = x.OrderByDescending(y => y.Customer.Distinct().Count())
        //                                        .Take(10)
        //                                        .Select((y, indexSub) => new SalesCustomerBySalesPersonSubDataBM
        //                                        {
        //                                            Color = ChartColorBM.Colors[indexSub],
        //                                            ColumnName = y.Customer,
        //                                            ColumnPoint = null,
        //                                            ColumnPointTarget = null,
        //                                            ColumnValue = y.Customer.Distinct().Count(),
        //                                            ColumnValueTarget = null,
        //                                            SubData = new SalesCustomerBySalesPersonChartBM
        //                                            {
        //                                                Top = x.GroupBy(t => t.Type)
        //                                                        .Select((z, indexSub1) => new SalesCustomerBySalesPersonSubDataBM
        //                                                        {
        //                                                            Color = ChartColorBM.Colors[indexSub1],
        //                                                            ColumnName = z.Key,
        //                                                            ColumnValue = z.Select(h => h.Customer).Distinct().Count(),
        //                                                            ColumnPoint = null
        //                                                        }).ToList(),
        //                                            }
        //                                        }).ToList(),
        //                                  Bottom = x.OrderBy(y => y.Customer.Distinct().Count())
        //                                              .Take(10)
        //                                             .Select((y, indexSub) => new SalesCustomerBySalesPersonSubDataBM
        //                                             {
        //                                                 Color = ChartColorBM.Colors[indexSub],
        //                                                 ColumnName = y.Customer,
        //                                                 ColumnPoint = null,
        //                                                 ColumnPointTarget = null,
        //                                                 ColumnValue = y.Customer.Distinct().Count(),
        //                                                 ColumnValueTarget = null,
        //                                                 SubData = new SalesCustomerBySalesPersonChartBM
        //                                                 {
        //                                                     Top = x.GroupBy(t => t.Type)
        //                                                            .Select((z, indexSub1) => new SalesCustomerBySalesPersonSubDataBM
        //                                                            {
        //                                                                Color = ChartColorBM.Colors[indexSub1],
        //                                                                ColumnName = z.Key,
        //                                                                ColumnValue = z.Select(h => h.Customer).Distinct().Count(),
        //                                                                ColumnPoint = null
        //                                                            }).ToList(),
        //                                                 }
        //                                             })
        //                                            .ToList()
        //                              }
        //                          }).ToList(),



        //        Bottom = listBottom5Result.GroupBy(x => x.SalesPerson)
        //                                  .Select((x, index) => new SalesCustomerBySalesPersonSubDataBM
        //                                  {
        //                                      ColumnName = x.Key,
        //                                      ColumnPoint = null,
        //                                      ColumnPointTarget = x.Select(y => y.Customer).Distinct().Count(),
        //                                      ColumnValue = x.Select(y => y.Customer).Distinct().Count(),
        //                                      ColumnValueTarget = (int?)targets.Where(t => t.SalesPerson == x.Key).FirstOrDefault().CustomerCountTarget * numberOfDays,
        //                                      Color = ChartColorBM.Colors[index],
        //                                      SubData = new SalesCustomerBySalesPersonChartBM
        //                                      {
        //                                          Top = x.OrderByDescending(y => y.Customer.Distinct().Count())
        //                                                  .Take(10)
        //                                                 .Select((y, indexSub) => new SalesCustomerBySalesPersonSubDataBM
        //                                                 {
        //                                                     Color = ChartColorBM.Colors[indexSub],
        //                                                     ColumnName = y.Customer,
        //                                                     ColumnPoint = null,
        //                                                     ColumnPointTarget = null,
        //                                                     ColumnValue = y.Customer.Distinct().Count(),
        //                                                     ColumnValueTarget = null,
        //                                                     SubData = new SalesCustomerBySalesPersonChartBM
        //                                                     {
        //                                                         Top = x.GroupBy(t => t.Type)
        //                                                                .Select((z, indexSub1) => new SalesCustomerBySalesPersonSubDataBM
        //                                                                {
        //                                                                    Color = ChartColorBM.Colors[indexSub1],
        //                                                                    ColumnName = z.Key,
        //                                                                    ColumnValue = z.Select(h => h.Customer).Distinct().Count(),
        //                                                                    ColumnPoint = null
        //                                                                }).ToList(),
        //                                                     }
        //                                                 })
        //                                                .ToList(),
        //                                          Bottom = x.OrderBy(y => y.Customer.Distinct().Count())
        //                                                 .Take(10)
        //                                                .Select((y, indexSub) => new SalesCustomerBySalesPersonSubDataBM
        //                                                {
        //                                                    Color = ChartColorBM.Colors[indexSub],
        //                                                    ColumnName = y.Customer,
        //                                                    ColumnPoint = null,
        //                                                    ColumnPointTarget = null,
        //                                                    ColumnValue = y.Customer.Distinct().Count(),
        //                                                    ColumnValueTarget = null,
        //                                                    SubData = new SalesCustomerBySalesPersonChartBM
        //                                                    {
        //                                                        Top = x.GroupBy(t => t.Type)
        //                                                               .Select((z, indexSub1) => new SalesCustomerBySalesPersonSubDataBM
        //                                                               {
        //                                                                   Color = ChartColorBM.Colors[indexSub1],
        //                                                                   ColumnName = z.Key,
        //                                                                   ColumnValue = z.Select(h => h.Customer).Distinct().Count(),
        //                                                                   ColumnPoint = null
        //                                                               }).ToList(),
        //                                                    }
        //                                                })
        //                                               .ToList(),
        //                                      }
        //                                  })
        //                               .ToList(),

        //    };
        //    return result;
        //}

        /// <summary>
        /// Get the sales data for the sales dashboard. The top 10, bottom 10 and their drill down data.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isSynced"></param>
        /// <returns></returns>
        public SalesChartBM GetSalesData(string startDate, string endDate, bool isSynced = false)
        {

            //GetSalesTargetData();
            var startDateObj = DateTime.Parse(startDate);
            var endDateObj = DateTime.Parse(endDate);
            var numberOfDays = (endDateObj.Date - startDateObj.Date).TotalDays;
            //#if DEBUG
            //            var startDateFormatted = startDateObj.Date.AddMonths(-2).ToString("yyyy-MM-ddTHH:mm:ss");
            //            var endDateFormatted = endDateObj.Date.AddMonths(-2).ToString("yyyy-MM-ddTHH:mm:ss");
            //#endif

            var startDateFormatted = startDateObj.Date.ToString("yyyy/MM/dd");
            var endDateFormatted = endDateObj.Date.ToString("yyyy/MM/dd");

            var priorStartDateFormatted = startDateObj.AddMonths(-1).Date.ToString("yyyy/MM/dd");
            var priorEndDateFormatted = endDateObj.AddMonths(-1).Date.ToString("yyyy/MM/dd");

            /// Is synced then the argument "isFirstTime" should be false.
            //var targets = this.GetSalesTargetData(!isSynced);

            string sqlString = SQLQueries.GetSalesBySalesPersonQuery(startDateFormatted
                                                                    , endDateFormatted
                                                                    , priorStartDateFormatted
                                                                    , priorEndDateFormatted);

            //string sqlString = MDXCubeQueries.GetSalesQuery(startDateFormatted, endDateFormatted, true);
            using (var adoDataAccess = new DataAccessADO())
            {
                var dataTableResult = adoDataAccess.ReadToDataSet(sqlString);

                var dataResult = dataTableResult.Tables[0]
                                     .AsEnumerable()
                                     .Select(x => new SalesMapper
                                     {
                                         AssignedPersonCode = x.Field<string>(0),
                                         Customer = x.Field<string>(1),
                                         SalesAmountCurrent = (x.Field<decimal?>(2)).ToRoundTwoDecimalDigits(),
                                         SalesAmountPrior = (x.Field<decimal?>(3)).ToRoundTwoDecimalDigits(),
                                         SalesPerson = x.Field<string>(4),
                                         SalesPersonDescription = x.Field<string>(5)
                                     })
                                     .Where(x => x.AssignedPersonCode.Trim() != string.Empty)
                                     .ToList();

                var dataGroupedResult = dataResult.GroupBy(x => new { x.AssignedPersonCode, x.Customer, x.SalesAmountCurrent, x.SalesAmountPrior })
                                                    .Select(x => new SalesMapper
                                                    {
                                                        AssignedPersonCode = x.First().AssignedPersonCode,
                                                        Customer = x.First().Customer,
                                                        SalesAmountCurrent = x.First().SalesAmountCurrent,
                                                        SalesAmountPrior = x.First().SalesAmountPrior,
                                                        SalesPerson = string.Join(",", x.Select(y =>
                                                                                    (
                                                                                    (!string.IsNullOrEmpty(y.SalesPersonDescription) &&
                                                                                        y.SalesPersonDescription.Contains(','))
                                                                                        ? y.SalesPersonDescription.Split(',').Last().Trim() + " " + y.SalesPersonDescription.Substring(0, 1)
                                                                                        : string.Empty)
                                                                                        + "" +
                                                                                        (!string.IsNullOrEmpty(y.SalesPersonDescription)
                                                                                            ? string.Format(" ({0})", y.SalesPerson.Trim())
                                                                                            : string.Empty)
                                                                                        )
                                                                                    .Distinct())
                                                    })
                                                    .ToList();

                var listTop5Sales = dataGroupedResult.GroupBy(x => x.AssignedPersonCode)
                                                    .Select(x => new { salesperson = x.Key, TotalSales = x.Sum(y => y.SalesAmountCurrent) })
                                                    .OrderByDescending(y => y.TotalSales)
                                                    .Take(5)
                                                    .ToList();

                var listBottom5Sales = dataGroupedResult.GroupBy(x => x.AssignedPersonCode)
                                                    .Select(x => new { salesperson = x.Key, TotalSales = x.Sum(y => y.SalesAmountCurrent) })
                                                    .OrderBy(y => y.TotalSales)
                                                    .Take(5)
                                                    .ToList();


                var result = new SalesChartBM
                {
                    Top = dataGroupedResult.Where(x => listTop5Sales.Any(s => s.salesperson == x.AssignedPersonCode))
                                            .GroupBy(grp => grp.AssignedPersonCode)
                                            .Select((x, index) => new SalesSubDataBM
                                            {
                                                ColumnName = x.Key,
                                                ColumnValue = x.Sum(y => y.SalesAmountCurrent).ToRoundTwoDecimalDigits(),
                                                ColumnValueTarget = x.Sum(y => y.SalesAmountPrior).ToRoundTwoDecimalDigits(),
                                                Color = ChartColorBM.Colors[index],
                                                ColumnValueToolTip = x.First().SalesPerson,
                                                ColumnValueTargetToolTip = x.First().SalesPerson,
                                                SubData = new SalesChartBM
                                                {
                                                    Top = x.OrderByDescending(y => y.SalesAmount)
                                                           .Take(10)
                                                          .Select((y, indexSub) => new SalesSubDataBM
                                                          {
                                                              Color = ChartColorBM.Colors[indexSub],
                                                              ColumnName = y.Customer,
                                                              ColumnPoint = null,
                                                              ColumnPointTarget = null,
                                                              ColumnValue = y.SalesAmountCurrent.ToRoundTwoDecimalDigits(),
                                                              ColumnValueTarget = y.SalesAmountPrior.ToRoundTwoDecimalDigits(),
                                                          })
                                                          .OrderByDescending(c => c.ColumnValue)
                                                         .ToList(),
                                                    Bottom = x.OrderByDescending(y => y.SalesAmount)
                                                           .Take(10)
                                                          .Select((y, indexSub) => new SalesSubDataBM
                                                          {
                                                              Color = ChartColorBM.Colors[indexSub],
                                                              ColumnName = y.Customer,
                                                              ColumnPoint = null,
                                                              ColumnPointTarget = null,
                                                              ColumnValue = y.SalesAmountCurrent.ToRoundTwoDecimalDigits(),
                                                              ColumnValueTarget = y.SalesAmountPrior.ToRoundTwoDecimalDigits(),
                                                          })
                                                          .OrderBy(c => c.ColumnValue)
                                                         .ToList(),
                                                }
                                            })
                                            .OrderByDescending(c => c.ColumnValue)
                                            .ToList(),
                    Bottom = dataGroupedResult.Where(x => listBottom5Sales.Any(s => s.salesperson == x.AssignedPersonCode))
                                            .GroupBy(grp => grp.AssignedPersonCode)
                                            .Select((x, index) => new SalesSubDataBM
                                            {
                                                ColumnName = x.Key,
                                                ColumnValue = x.Sum(y => y.SalesAmountCurrent).ToRoundTwoDecimalDigits(),
                                                ColumnValueTarget = x.Sum(y => y.SalesAmountPrior).ToRoundTwoDecimalDigits(),
                                                Color = ChartColorBM.Colors[index],
                                                ColumnValueToolTip = x.First().SalesPerson,
                                                ColumnValueTargetToolTip = x.First().SalesPerson,
                                                SubData = new SalesChartBM
                                                {
                                                    Top = x.OrderByDescending(y => y.SalesAmount)
                                                           .Take(10)
                                                          .Select((y, indexSub) => new SalesSubDataBM
                                                          {
                                                              Color = ChartColorBM.Colors[indexSub],
                                                              ColumnName = y.Customer,
                                                              ColumnPoint = null,
                                                              ColumnPointTarget = null,
                                                              ColumnValue = y.SalesAmountCurrent.ToRoundTwoDecimalDigits(),
                                                              ColumnValueTarget = y.SalesAmountPrior.ToRoundTwoDecimalDigits(),
                                                          })
                                                          .OrderByDescending(c => c.ColumnValue)
                                                         .ToList(),
                                                    Bottom = x.OrderByDescending(y => y.SalesAmount)
                                                           .Take(10)
                                                          .Select((y, indexSub) => new SalesSubDataBM
                                                          {
                                                              Color = ChartColorBM.Colors[indexSub],
                                                              ColumnName = y.Customer,
                                                              ColumnPoint = null,
                                                              ColumnPointTarget = null,
                                                              ColumnValue = y.SalesAmountCurrent.ToRoundTwoDecimalDigits(),
                                                              ColumnValueTarget = y.SalesAmountPrior.ToRoundTwoDecimalDigits(),
                                                          })
                                                          .OrderBy(c => c.ColumnValue)
                                                         .ToList(),
                                                }
                                            })
                                            .OrderByDescending(c => c.ColumnValue)
                                            .ToList()
                };



                #region Commented
                //var result = new SalesChartBM
                //{
                //    Top = listTop5Result.GroupBy(x => x.SalesPerson)
                //                           .Select((x, index) => new SalesSubDataBM
                //                           {
                //                               ColumnName = x.Key,
                //                               ColumnPoint = x.Select(y => y.Customer).Distinct().Count(),
                //                               ColumnPointTarget = (int?)targets.Where(t => t.SalesPerson == x.Key).FirstOrDefault().CustomerCountTarget * (int)numberOfDays,
                //                               ColumnValue = x.Sum(y => y.SalesAmount).Value.ToRoundTwoDigits(),
                //                               ColumnValueTarget = (double?)targets.Where(t => t.SalesPerson == x.Key).FirstOrDefault().SalesTarget * numberOfDays,
                //                               Color = ChartColorBM.Colors[index],
                //                               SubData = new SalesChartBM
                //                               {
                //                                   Top = x.OrderByDescending(y => y.SalesAmount)
                //                                           .Take(10)
                //                                          .Select((y, indexSub) => new SalesSubDataBM
                //                                          {
                //                                              Color = ChartColorBM.Colors[indexSub],
                //                                              ColumnName = y.Customer,
                //                                              ColumnPoint = null,
                //                                              ColumnPointTarget = null,
                //                                              ColumnValue = y.SalesAmount.Value.ToRoundTwoDigits(),
                //                                              ColumnValueTarget = null,
                //                                              SubData = new SalesChartBM
                //                                              {
                //                                                  Top = x.GroupBy(t => t.Type)
                //                                                         .Select((z, indexSub1) => new SalesSubDataBM
                //                                                         {
                //                                                             Color = ChartColorBM.Colors[indexSub1],
                //                                                             ColumnName = z.Key,
                //                                                             ColumnValue = z.Sum(g => g.SalesAmount).Value.ToRoundTwoDigits(),
                //                                                             ColumnPoint = z.Select(h => h.Customer).Distinct().Count()
                //                                                         }).ToList(),
                //                                              }
                //                                          })
                //                                         .ToList(),
                //                                   Bottom = x.OrderBy(y => y.SalesAmount)
                //                                          .Take(10)
                //                                         .Select((y, indexSub) => new SalesSubDataBM
                //                                         {
                //                                             Color = ChartColorBM.Colors[indexSub],
                //                                             ColumnName = y.Customer,
                //                                             ColumnPoint = null,
                //                                             ColumnPointTarget = null,
                //                                             ColumnValue = y.SalesAmount.Value.ToRoundTwoDigits(),
                //                                             ColumnValueTarget = null,
                //                                             SubData = new SalesChartBM
                //                                             {
                //                                                 Top = x.GroupBy(t => t.Type)
                //                                                        .Select((z, indexSub1) => new SalesSubDataBM
                //                                                        {
                //                                                            Color = ChartColorBM.Colors[indexSub1],
                //                                                            ColumnName = z.Key,
                //                                                            ColumnValue = z.Sum(g => g.SalesAmount).Value.ToRoundTwoDigits(),
                //                                                            ColumnPoint = z.Select(h => h.Customer).Distinct().Count()
                //                                                        }).ToList(),
                //                                             }
                //                                         })
                //                                        .ToList(),
                //                               }
                //                           })
                //                           .ToList(),
                //    Bottom = listBottom5Result.GroupBy(x => x.SalesPerson)
                //                              .Select((x, index) => new SalesSubDataBM
                //                              {
                //                                  ColumnName = x.Key,
                //                                  ColumnPoint = x.Select(y => y.Customer).Distinct().Count(),
                //                                  ColumnPointTarget = (int?)targets.Where(t => t.SalesPerson == x.Key).FirstOrDefault().CustomerCountTarget * (int)numberOfDays,
                //                                  ColumnValue = x.Sum(y => y.SalesAmount).Value.ToRoundTwoDigits(),
                //                                  ColumnValueTarget = (double?)targets.Where(t => t.SalesPerson == x.Key).FirstOrDefault().SalesTarget * numberOfDays,
                //                                  Color = ChartColorBM.Colors[index],
                //                                  SubData = new SalesChartBM
                //                                  {
                //                                      Top = x.OrderByDescending(y => y.SalesAmount)
                //                                              .Take(10)
                //                                             .Select((y, indexSub) => new SalesSubDataBM
                //                                             {
                //                                                 Color = ChartColorBM.Colors[indexSub],
                //                                                 ColumnName = y.Customer,
                //                                                 ColumnPoint = null,
                //                                                 ColumnPointTarget = null,
                //                                                 ColumnValue = y.SalesAmount.Value.ToRoundTwoDigits(),
                //                                                 ColumnValueTarget = null,
                //                                                 SubData = new SalesChartBM
                //                                                 {
                //                                                     Top = x.GroupBy(t => t.Type)
                //                                                            .Select((z, indexSub1) => new SalesSubDataBM
                //                                                            {
                //                                                                Color = ChartColorBM.Colors[indexSub1],
                //                                                                ColumnName = z.Key,
                //                                                                ColumnValue = z.Sum(g => g.SalesAmount).Value.ToRoundTwoDigits(),
                //                                                                ColumnPoint = z.Select(h => h.Customer).Distinct().Count()
                //                                                            }).ToList(),
                //                                                 }
                //                                             })
                //                                            .ToList(),
                //                                      Bottom = x.OrderBy(y => y.SalesAmount)
                //                                             .Take(10)
                //                                            .Select((y, indexSub) => new SalesSubDataBM
                //                                            {
                //                                                Color = ChartColorBM.Colors[indexSub],
                //                                                ColumnName = y.Customer,
                //                                                ColumnPoint = null,
                //                                                ColumnPointTarget = null,
                //                                                ColumnValue = y.SalesAmount.Value.ToRoundTwoDigits(),
                //                                                ColumnValueTarget = null,
                //                                                SubData = new SalesChartBM
                //                                                {
                //                                                    Top = x.GroupBy(t => t.Type)
                //                                                           .Select((z, indexSub1) => new SalesSubDataBM
                //                                                           {
                //                                                               Color = ChartColorBM.Colors[indexSub1],
                //                                                               ColumnName = z.Key,
                //                                                               ColumnValue = z.Sum(g => g.SalesAmount).Value.ToRoundTwoDigits(),
                //                                                               ColumnPoint = z.Select(h => h.Customer).Distinct().Count()
                //                                                           }).ToList(),
                //                                                }
                //                                            })
                //                                           .ToList(),
                //                                  }
                //                              })
                //                           .ToList(),
                //};
                #endregion
                return result;
            }
        }

        /// <summary>
        /// Get Targets (Customer count and sales) of Sales Persons.
        /// This function will sync the table SalesPersonTargets if it is not synced today
        /// </summary>
        /// <param name="isFirstTime"> If this value is false, then this function will not sync the table SalesPersonTargets by assuming that the table already synced today </param>
        /// <returns> List of sales persons with their customer count and sales targets </returns>
        public List<SalesTargetBM> GetSalesTargetData(bool isFirstTime)
        {
            #region Sync the table SalesPersonTargets
            /// If isFirstTime is false then we assumes that the table SalesPersonTargets already synced
            if (isFirstTime && !this.IsSalesPersonTargetsSynced())
            {
                /// Sync the table SalesPersonTargets
                this.SyncSalesPersonTargets();
            }
            #endregion

            #region Get sales persons with their targets
            using (var adoDataAccess = new DataAccessADO())
            {
                var data = adoDataAccess.ReadToDataSet(SQLQueries.GetAllSalesPersonTargets);
                if (data != null && data.Tables[0] != null && data.Tables[0].AsEnumerable() != null && data.Tables[0].AsEnumerable().Count() > 0)
                {
                    var salesPersons = data.Tables[0].AsEnumerable()
                                        .Select(x => new SalesTargetBM()
                                        {
                                            SalesPerson = x.Field<string>("SalesPerson"),
                                            CustomerCountTarget = x.Field<int>("CustomerCount"),
                                            SalesTarget = x.Field<decimal>("Sales")
                                        })
                                        .ToList();
                    return salesPersons;
                }
                throw new Exception("No sales persons in the database");
            }
            #endregion
        }

        /// <summary>
        /// Update targets of sales persons based on the parameter "model"
        /// </summary>
        /// <param name="model"> List of all sales persons with their new targets </param>
        public void UpdateSalesPersonTargets(List<SalesTargetBM> model)
        {
            using (var adoDataAccess = new DataAccessADO())
            {
                var query = string.Empty;
                for (int i = 0; i < model.Count; i++)
                {
                    query += SQLQueries.UpdateSalesPersonTarget(model[i].CustomerCountTarget, model[i].SalesTarget, model[i].SalesPerson);
                }
                var data = adoDataAccess.ReadToDataSet(query);
            }
        }

        #region Private Functions

        /// <summary>
        /// Sync sales persons in the ADO.Net table, SalesPersonTargets from the sales person from Cube query
        /// </summary>
        private void SyncSalesPersonTargets()
        {
            string syncQuery = string.Empty;

            var salesDataProvider = new SalesDataProvider();
            var cubeResult = new DataTable();
            using (var adoDataAccess = new DataAccessADO())
            {
                cubeResult = adoDataAccess.ReadToDataSet(SQLQueries.GetAllSalesPerson).Tables[0];
            }
            var cubeSalesPersons = cubeResult
                                             .AsEnumerable()
                                             .Where(x => !string.IsNullOrEmpty(x.Field<string>(1)))
                                             .Select(x => x.Field<string>(1))
                                             .ToList();

            using (var adoDataAccess = new DataAccessADO())
            {
                var data = adoDataAccess.ReadToDataSet(SQLQueries.GetAllSalesPersonTargets);
                if (data != null && data.Tables[0] != null && data.Tables[0].AsEnumerable() != null && data.Tables[0].AsEnumerable().Count() > 0)
                {
                    var adoSalesPersons = data.Tables[0].AsEnumerable()
                                             .Select(x => x.Field<string>("SalesPerson"))
                                             .ToList();
                    #region Add sales persons they are not in the table SalesPersonTargets
                    for (int i = 0; i < cubeSalesPersons.Count; i++)
                    {
                        if (adoSalesPersons.IndexOf(cubeSalesPersons[i]) == -1)
                            syncQuery += SQLQueries.InsertSalesPersonTarget(cubeSalesPersons[i], 0, 0);
                    }
                    #endregion

                    #region Remove sales persons from the table SalesPersonTargets they are not in the cube result
                    for (int i = 0; i < adoSalesPersons.Count; i++)
                    {
                        if (cubeSalesPersons.IndexOf(adoSalesPersons[i]) == -1)
                            syncQuery += SQLQueries.RemoveSalesPersonTarget(adoSalesPersons[i]);
                    }
                    #endregion

                    syncQuery += SQLQueries.UpdateAllSalesPersonTargetSyncedOn(DateTime.Today);
                }
                else
                {
                    #region Add all sales persons from cube to table
                    for (int i = 0; i < cubeSalesPersons.Count; i++)
                    {
                        syncQuery += SQLQueries.InsertSalesPersonTarget(cubeSalesPersons[i], 0, 0);
                    }
                    #endregion

                    syncQuery += SQLQueries.UpdateAllSalesPersonTargetSyncedOn(DateTime.Today);
                }

                if (syncQuery != string.Empty)
                {

                    /// Execute sql query
                    adoDataAccess.ReadToDataSet(syncQuery);
                }
            }
        }

        /// <summary>
        /// Check whether the table SalesPersonTargets is synced today
        /// </summary>
        /// <returns></returns>
        private bool IsSalesPersonTargetsSynced()
        {
            var isSynced = false;
            using (var adoDataAccess = new DataAccessADO())
            {
                var data = adoDataAccess.ReadToDataSet(SQLQueries.GetTopAllSalesPersonTargets(1));
                if (data != null && data.Tables[0] != null && data.Tables[0].AsEnumerable() != null)
                {
                    var salesPersons = data.Tables[0].AsEnumerable().FirstOrDefault();
                    if (salesPersons == null)
                        return false;
                    var lastSyncedDate = salesPersons.Field<DateTime>("SyncedOn");
                    if (lastSyncedDate.Date == DateTime.Today.Date)
                    {
                        isSynced = true;
                    }
                }
            }
            return isSynced;
        }
        #endregion

        public SalesMapper GetAllSalesPerson()
        {
            SalesMapper result = new SalesMapper();
            List<SqlParameter> parameterList = new List<SqlParameter>();

            var data = base.ReadToDataSetViaProcedure("BI_GetAllSalesPersons", parameterList.ToArray());
            if (data != null && data.Tables[0] != null && data.Tables[0].AsEnumerable() != null && data.Tables[0].AsEnumerable().Count() > 0)
            {
                var salesPersons = data.Tables[0].AsEnumerable()

                                     .Where(x => !string.IsNullOrEmpty(x.Field<string>(0)))
                                     .Select(x => new KeyValuePair<string, string>(x.Field<string>(0), x.Field<string>(1))
                                     {
                                     })
                                     .OrderBy(x => x.Key)
                                     .ToList();
                result.ListSalesPerson = salesPersons;
            }
            return result;
        }

        public List<KeyValuePair<string, string>> GetSalesPersons(string sqlString = "")
        {


            try
            {
                using (var adoDataAccess = new DataAccessADO())
                {
                    var data = adoDataAccess.ReadToDataSet(SQLQueries.GetAllSalesPerson);
                    if (data != null && data.Tables[0] != null && data.Tables[0].AsEnumerable() != null && data.Tables[0].AsEnumerable().Count() > 0)
                    {
                        var salesPersons = data.Tables[0].AsEnumerable()

                                             .Where(x => !string.IsNullOrEmpty(x.Field<string>(0)))
                                             .Select(x => new KeyValuePair<string, string>(x.Field<string>(0), x.Field<string>(1))
                                             {
                                             })
                                             .OrderBy(x => x.Key)
                                             .ToList();
                        return salesPersons;
                    }
                    throw new Exception("No sales persons in the database");
                }

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database-" + e.Message);
            }

        }

        public SalesMapperReportWithTopBottom GetSalesReportOfSalesPerson(List<string> salesPerson, string startDate, string endDate)
        {
            SalesMapperReportWithTopBottom response = new SalesMapperReportWithTopBottom();
            var startDateObj = DateTime.Parse(startDate);
            //var startDateFormatted = startDateObj.Date.ToString("yyyy-MM-dd HH:mm:ss");
            var endDateObj = DateTime.Parse(endDate);
            DataTable dtSalesPerson = new DataTable();
            dtSalesPerson.Columns.Add("Code");
            foreach (string code in salesPerson)
            {
                dtSalesPerson.Rows.Add(code);
            }
            //var endDateFormatted = endDateObj.Date.ToString("yyyy-MM-dd HH:mm:ss");
            using (var adoDataAccess = new DataAccessADO())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                // parameterList.Add(new SqlParameter("@salesman", salesPerson != null ? string.Join(",", salesPerson) : ""));
                var param = new SqlParameter();
                param.SqlDbType = SqlDbType.Structured;
                param.Value = dtSalesPerson;
                param.ParameterName = "@salesman";
                parameterList.Add(param);
                parameterList.Add(new SqlParameter("@startDate", startDateObj));
                parameterList.Add(new SqlParameter("@endDate", endDateObj));
                parameterList.Add(new SqlParameter("@priorStartDate", startDateObj.AddYears(-1)));
                parameterList.Add(new SqlParameter("@priorEndDate", endDateObj.AddYears(-1)));

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_GetSalesReportofSalesPerson", parameterList.ToArray());
                var result = dataTableResult.Tables[0].AsEnumerable()

                       .Select(x => new SalesMapper
                       {
                           SalesPerson = x.Field<string>("SalesPersonCode"),
                           SalesPersonDescription = x.Field<string>("SalesPersonDescription"),
                           AssignedPersonCode = x.Field<string>("AssignedPersonCode"),
                           NoOfCustomer = x.Field<int>("CustomerCount"),
                           SalesAmountCurrent = x.Field<decimal?>("TotalSalesAmount").HasValue ? x.Field<decimal?>("TotalSalesAmount").Value.ToStripDecimal() : 0,
                           SalesAmountPrior = x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value.ToStripDecimal() : 0,
                           Percentage = (decimal)(x.Field<decimal?>("TotalSalesAmount").HasValue ? x.Field<decimal?>("TotalSalesAmount").Value : 0).ToPercentageDifference(x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value : 0),
                           Difference = (x.Field<decimal?>("TotalSalesAmount").HasValue ? x.Field<decimal?>("TotalSalesAmount").Value : 0) - (x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value.ToStripDecimal() : 0),
                           Binno = x.Field<string>("Binno"),
                           Picker = x.Field<string>("Picker")

                       })
                       .ToList();
                result = result.Where(f => f.SalesPerson != "DONA" && f.SalesPerson != "DUMP").ToList();


                result = result.GroupBy(x => x.SalesPerson)
                                .Select(x => new SalesMapper
                                {
                                    SalesPerson = x.Key,
                                    SalesPersonDescription = string.Join(",", x.Select(t => t.SalesPersonDescription.Contains(",")
                                                                  ? string.Format("{0} {1}"
                                                                                      , t.SalesPersonDescription.Split(',').LastOrDefault().TrimEnd()
                                                                                      , t.SalesPersonDescription.Substring(0, 1))
                                                                  : t.SalesPersonDescription)),
                                    AssignedPersonCode = string.Join(",", x.Select(t => t.AssignedPersonCode)),
                                    NoOfCustomer = x.First().NoOfCustomer,
                                    SalesAmountCurrent = x.First().SalesAmountCurrent,
                                    SalesAmountPrior = x.First().SalesAmountPrior,
                                    Percentage = (decimal)x.First().Percentage.ToRoundDigits(),
                                    Difference = x.First().Difference,
                                    Binno=x.First().Binno,
                                    Picker=x.First().Picker
                                })
                                .ToList();
                response.ReportData = result;
                response.ChartData = new GenericTopBottomTwoBarChartData()
                {
                    Top = result.OrderByDescending(d => d.Percentage).Take(10).Select(d => new GenericTwoBarChartdata
                    {
                        Category = d.SalesPerson,
                        Color1 = ChartColorBM.SalesManCurrent,
                        Value1 = d.SalesAmountCurrent,
                        Color2 = ChartColorBM.SalesManPrevious,
                        Value2 = d.SalesAmountPrior,
                        Label = d.SalesPersonDescription,
                        Tooltip = d.Percentage.ToRoundDigits()

                    }).ToList(),
                    Bottom = result.OrderBy(d => d.Percentage).Take(10).Select(d => new GenericTwoBarChartdata
                    {
                        Category = d.SalesPerson,
                        Color1 = ChartColorBM.SalesManCurrent,
                        Value1 = d.SalesAmountCurrent,
                        Color2 = ChartColorBM.SalesManPrevious,
                        Value2 = d.SalesAmountPrior,
                        Label = d.SalesPersonDescription,
                        Tooltip = d.Percentage.ToRoundDigits()
                    }).ToList()
                };
                return response;
            }
        }

        public List<SalesAnalysisMapper> GetSalesAnalysisReportofSalesPerson(List<string> salesPerson, string startDate, string endDate,string userId)
        {

            AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
            var userAccessibleCategories = _adminUserManagementDateProvider.GetUserAccess(userId);

            List<SalesAnalysisMapper> result = new List<SalesAnalysisMapper>();
            var startDateObj = DateTime.Parse(startDate);
            var endDateObj = DateTime.Parse(endDate);
            DataTable dtSalesPerson = new DataTable();
            dtSalesPerson.Columns.Add("Code");
            foreach (string code in salesPerson)
            {
                dtSalesPerson.Rows.Add(code);
            }



            DataTable dtCategories = new DataTable();
            dtCategories.Columns.Add("Code");
            foreach (var category in userAccessibleCategories.Categories)
            {
                if (category.IsAccess == true) { dtCategories.Rows.Add(category.Name); }
            }
         

            using (var adoDataAccess = new DataAccessADO())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                //parameterList.Add(new SqlParameter("@salesman", salesPerson != null ? string.Join(",", salesPerson) : ""));
                //parameterList.Add(new SqlParameter("@salesman",dtSalesPerson));
                var param = new SqlParameter();
                param.SqlDbType = SqlDbType.Structured;
                param.Value = dtSalesPerson;
                param.ParameterName = "@salesman";
                parameterList.Add(param);

                var paramCategory = new SqlParameter();
                paramCategory.SqlDbType = SqlDbType.Structured;
                paramCategory.Value = dtCategories;
                paramCategory.ParameterName = "@categories";
                parameterList.Add(paramCategory);

                parameterList.Add(new SqlParameter("@startDate", startDateObj));
                parameterList.Add(new SqlParameter("@endDate", endDateObj));

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_SL_GetSalesPersonAnalysisReport", parameterList.ToArray());

                try
                {
                    result = dataTableResult.Tables[0].AsEnumerable()

                                        .Select(x => new SalesAnalysisMapper
                                        {
                                            SalesPerson = x.Field<string>("SalesPerson"),
                                            GroceryCasesSold = x.Field<decimal>("GroceryQty"),
                                            GroceryRevenue = x.Field<decimal>("GroceryRev"),
                                            NoOfCustomer = x.Field<int>("CustomerCount"),
                                            ProduceCasesSold = x.Field<decimal>("ProduceQty"),
                                            ProduceRevenue = x.Field<decimal>("ProduceRev"),
                                            TotalCasesSold = x.Field<decimal>("TotalQty"),
                                            TotalRevenue = x.Field<decimal>("TotalRev"),
                                            ManualCasesSold = x.Field<decimal>("ManualQty"),
                                            ManualRevenue = x.Field<decimal>("ManualRev")
                                        })
                                        .ToList();
                    result = result
                        .Where(d => d.SalesPerson != "DUMP" && d.SalesPerson != "DONA" && d.SalesPerson.Trim() != "")
                        .OrderByDescending(x=>x.TotalRevenue)
                        .ToList();
                }
                catch (Exception ex)
                { }

                return result;
            }
        }

        public List<SalesMapper> GetCustomerAndSales(string salesPerspon, int filterId, int period, string category = "",
                                                     string commodity = "")
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
                parameterList.Add(new SqlParameter("@startdate", startDate));
                parameterList.Add(new SqlParameter("@enddate", endDate));
                parameterList.Add(new SqlParameter("@prevstartdate", prevStartDate));
                parameterList.Add(new SqlParameter("@prevenddate", prevEndDate));
                parameterList.Add(new SqlParameter("@category", category));
                parameterList.Add(new SqlParameter("@commodity", commodity));

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_GetSalesPersonReport", parameterList.ToArray());

                if (dataTableResult != null && dataTableResult.Tables.Count > 0)
                {
                    var result = dataTableResult.Tables[0]
                                            .AsEnumerable()
                                            .Select(x => new SalesMapper
                                            {
                                                Customer = x.Field<string>("Customer"),
                                                CustomerNumber=x.Field<string>("custno"),
                                                SalesQty = x.Field<int?>("TotalSalesQty").Value,
                                                SalesAmountCurrent = x.Field<decimal?>("TotalSalesAmount").Value,
                                                SalesAmountPrior = x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value : 0,
                                                Percentage = (decimal)x.Field<decimal?>("TotalSalesAmount").Value.ToPercentageDifference(x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value : 0),
                                                Difference = (x.Field<decimal?>("TotalSalesAmount").HasValue ? x.Field<decimal?>("TotalSalesAmount").Value : 0) - (x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value : 0)
                                            }).ToList();
                    return result;
                }
                else
                    return new List<SalesMapper>();
            }
        }

        public List<SalesMapper> GetCustomerAndSalesTemp(string salesPerspon, string startDateCurrent,
                                                     string endDateCurrent, string startDatePrevious,
                                                     string endDatePrevious, string category = "",
                                                     string commodity = "")
        {
            var startDateObj = DateTime.Parse(startDateCurrent);
            var endDateObj = DateTime.Parse(endDateCurrent);

            using (var adoDataAccess = new DataAccessADO())
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@salesman", salesPerspon));
                parameterList.Add(new SqlParameter("@startdate", startDateObj));
                parameterList.Add(new SqlParameter("@enddate", endDateObj));
                parameterList.Add(new SqlParameter("@prevstartdate", startDateObj.AddYears(-1)));
                parameterList.Add(new SqlParameter("@prevenddate", endDateObj.AddYears(-1)));
                parameterList.Add(new SqlParameter("@category", category));
                parameterList.Add(new SqlParameter("@commodity", commodity));

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_GetSalesPersonReport", parameterList.ToArray());

                if (dataTableResult != null && dataTableResult.Tables.Count > 0)
                {
                    var result = dataTableResult.Tables[0]
                                            .AsEnumerable()
                                            .Select(x => new SalesMapper
                                            {
                                                Customer = x.Field<string>("Customer"),
                                                SalesQty = x.Field<int?>("TotalSalesQty").Value,
                                                SalesAmountCurrent = x.Field<decimal?>("TotalSalesAmount").Value,
                                                SalesAmountPrior = x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value : 0,
                                                Percentage = (decimal)x.Field<decimal?>("TotalSalesAmount").Value.ToPercentageDifference(x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value : 0),
                                                Difference = (x.Field<decimal?>("TotalSalesAmount").HasValue ? x.Field<decimal?>("TotalSalesAmount").Value : 0) - (x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value : 0)
                                            }).ToList();
                    return result;
                }
                else
                    return new List<SalesMapper>();
            }
        }

        public List<SalesMapper> GetCustomerAndSalesReportWithoutFilter(string salesPerspon, string startDateCurrent,
                                                    string endDateCurrent, string startDatePrevious,
                                                    string endDatePrevious)
        {
            var startDateObj = DateTime.Parse(startDateCurrent);
            var endDateObj = DateTime.Parse(endDateCurrent);

            using (var adoDataAccess = new DataAccessADO())
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@salesman", salesPerspon));
                parameterList.Add(new SqlParameter("@startdate", startDateObj));
                parameterList.Add(new SqlParameter("@enddate", endDateObj));
                parameterList.Add(new SqlParameter("@prevstartdate", startDateObj.AddYears(-1)));
                parameterList.Add(new SqlParameter("@prevenddate", endDateObj.AddYears(-1)));
                parameterList.Add(new SqlParameter("@comodity", "All"));

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_GetSalesPersonReportWithoutComodity", parameterList.ToArray());
                if (dataTableResult != null && dataTableResult.Tables.Count > 0)
                {
                    var result = dataTableResult.Tables[0]
                                        .AsEnumerable()
                                        .Select(x => new SalesMapper
                                        {
                                            Customer = x.Field<string>("Customer"),
                                            SalesQty = x.Field<int?>("TotalSalesQty").Value,
                                            SalesAmountCurrent = x.Field<decimal?>("TotalSalesAmount").Value.ToStripDecimal(),
                                            SalesAmountPrior = x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value.ToStripDecimal() : 0,
                                            Percentage = (decimal)x.Field<decimal?>("TotalSalesAmount").Value.ToPercentageDifference(x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value : 0),
                                            Difference = (x.Field<decimal?>("TotalSalesAmount").HasValue ? x.Field<decimal?>("TotalSalesAmount").Value.ToStripDecimal() : 0) - (x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value.ToStripDecimal() : 0)
                                        }).ToList();
                    return result;
                }
                else
                    return new List<SalesMapper>();
            }
        }

        public List<SalesMapper> GetCustomerAndSalesReportForAnalysis(string salesPerspon, string startDateCurrent,
                                                  string endDateCurrent, string startDatePrevious,
                                                  string endDatePrevious)
        {
            var startDateObj = DateTime.Parse(startDateCurrent);
            var endDateObj = DateTime.Parse(endDateCurrent);

            using (var adoDataAccess = new DataAccessADO())
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@salesman", salesPerspon));
                parameterList.Add(new SqlParameter("@startdate", startDateObj));
                parameterList.Add(new SqlParameter("@enddate", endDateObj));
                parameterList.Add(new SqlParameter("@prevstartdate", startDateObj.AddYears(-1)));
                parameterList.Add(new SqlParameter("@prevenddate", endDateObj.AddYears(-1)));

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_GetSalesPersonReportForAnalysis", parameterList.ToArray());
                if (dataTableResult != null && dataTableResult.Tables.Count > 0)
                {
                    var result = dataTableResult.Tables[0]
                                        .AsEnumerable()
                                        .Select(x => new SalesMapper
                                        {
                                            Customer = x.Field<string>("Customer"),
                                            SalesQty = x.Field<int?>("TotalSalesQty").Value,
                                            SalesAmountCurrent = x.Field<decimal?>("TotalSalesAmount").Value.ToStripDecimal(),
                                            SalesAmountPrior = x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value.ToStripDecimal() : 0,
                                            Percentage = (decimal)x.Field<decimal?>("TotalSalesAmount").Value.ToPercentageDifference(x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value : 0),
                                            Difference = (x.Field<decimal?>("TotalSalesAmount").HasValue ? x.Field<decimal?>("TotalSalesAmount").Value : 0).ToStripDecimal() - (x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value.ToStripDecimal() : 0)
                                        }).ToList();
                    return result;
                }
                else
                    return new List<SalesMapper>();
            }
        }

        public List<SalesMapper> GetCustomerAndSalesReport(string salesPerspon, int filterId, int period,string comodity)
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
                parameterList.Add(new SqlParameter("@startdate", startDate));
                parameterList.Add(new SqlParameter("@enddate", endDate));
                parameterList.Add(new SqlParameter("@prevstartdate", prevStartDate));
                parameterList.Add(new SqlParameter("@prevenddate", prevEndDate));
                parameterList.Add(new SqlParameter("@comodity", comodity));

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_GetSalesPersonReportWithoutComodity", parameterList.ToArray());
                if (dataTableResult != null && dataTableResult.Tables.Count > 0)
                {
                    var result = dataTableResult.Tables[0]
                                        .AsEnumerable()
                                        .Select(x => new SalesMapper
                                        {
                                            CustomerNumber=x.Field<string>("custno"),
                                            Customer = x.Field<string>("Customer"),
                                            SalesQty = x.Field<int?>("TotalSalesQty").Value,
                                            SalesAmountCurrent = x.Field<decimal?>("TotalSalesAmount").Value.ToStripDecimal(),
                                            SalesAmountPrior = x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value : 0,
                                            Percentage = (decimal)x.Field<decimal?>("TotalSalesAmount").Value.ToPercentageDifference(x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value : 0),
                                            Difference = (x.Field<decimal?>("TotalSalesAmount").HasValue ? x.Field<decimal?>("TotalSalesAmount").Value.ToStripDecimal() : 0) - (x.Field<decimal?>("TotalSalesAmountPrev").HasValue ? x.Field<decimal?>("TotalSalesAmountPrev").Value.ToStripDecimal() : 0)
                                        }).ToList();
                    return result;
                }
                else
                    return new List<SalesMapper>();
            }
        }
        public List<CasesSoldDetailsMapper> GetCustomerAndSalesReportDetails(string salesPerspon, int filterId, int period,string customerNumber)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();


           // DateTime CurrentEndDate = targetFilter.Periods.Current.End;
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
                parameterList.Add(new SqlParameter("@startdate", startDate));
                parameterList.Add(new SqlParameter("@enddate", endDate));
                parameterList.Add(new SqlParameter("@custno", customerNumber));
                //parameterList.Add(new SqlParameter("@prevstartdate", prevStartDate));
                //parameterList.Add(new SqlParameter("@prevenddate", prevEndDate));

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_GetSalesPersonReportWithoutComodityDetailedReport", parameterList.ToArray());
                if (dataTableResult != null && dataTableResult.Tables.Count > 0)
                {
                    var result = dataTableResult.Tables[0]
                                        .AsEnumerable()
                                        .Select(x => new CasesSoldDetailsMapper
                                        {
                                            Comodity =x.Field<string>("comodity"),
                                            InvoiceDate = x.Field<DateTime>("invdte"),
                                            InvoiceNumber = x.Field<string>("invno"),
                                            Item = x.Field<string>("item"),
                                            ItemDesc = x.Field<string>("descrip"),
                                            Quantity = x.Field<int>("qty"),
                                            ExtPrice = x.Field<decimal>("extprice"),
                                            SalesMan = x.Field<string>("salesmn"),
                                            Sono = x.Field<string>("sono"),
                                            Transeq = x.Field<string>("transeq")
                                        }).ToList();
                    return result;
                }
                else
                    return new List<CasesSoldDetailsMapper>();
            }
        }

        public List<SalesChartBM> GetSalesDashboardSalesAndCustomers(string startDate, string endDate, bool isSynced = false)
        {
            var startDateObj = DateTime.Parse(startDate);
            var endDateObj = DateTime.Parse(endDate);

            var startDateFormatted = startDateObj.Date.ToString("yyyy/MM/dd");
            var endDateFormatted = endDateObj.Date.ToString("yyyy/MM/dd");

            var priorStartDateFormatted = startDateObj.AddMonths(-1).Date.ToString("yyyy/MM/dd");
            var priorEndDateFormatted = endDateObj.AddMonths(-1).Date.ToString("yyyy/MM/dd");

            string sqlString = SQLQueries.GetSalesBySalesPersonQuery(startDateFormatted
                                                                    , endDateFormatted
                                                                    , priorStartDateFormatted
                                                                    , priorEndDateFormatted);

            //string sqlString = MDXCubeQueries.GetSalesQuery(startDateFormatted, endDateFormatted, true);
            using (var adoDataAccess = new DataAccessADO())
            {
                var dataTableResult = adoDataAccess.ReadToDataSet(sqlString);

                #region Map data to sales mapper
                var dataResult = dataTableResult.Tables[0]
                                             .AsEnumerable()
                                             .Select(x => new SalesMapper
                                             {
                                                 AssignedPersonCode = x.Field<string>(0),
                                                 Customer = x.Field<string>(1),
                                                 SalesAmountCurrent = x.Field<decimal?>(2) ?? 0,
                                                 SalesAmountPrior = x.Field<decimal?>(3) ?? 0,
                                                 SalesPerson = x.Field<string>(4),
                                                 SalesPersonDescription = x.Field<string>(5)
                                             })
                                             .Where(x => x.AssignedPersonCode.Trim() != string.Empty)
                                             .ToList();
                #endregion

                #region Group assigned sales person and pivot the names
                var dataGroupedResult = dataResult.GroupBy(x => new { x.AssignedPersonCode, x.Customer, x.SalesAmountCurrent, x.SalesAmountPrior })
                                                            .Select(x => new SalesMapper
                                                            {
                                                                AssignedPersonCode = x.First().AssignedPersonCode,
                                                                Customer = x.First().Customer,
                                                                SalesAmountCurrent = x.First().SalesAmountCurrent,
                                                                SalesAmountPrior = x.First().SalesAmountPrior,
                                                                SalesPerson = string.Join(",", x.Select(y =>
                                                                                            (!string.IsNullOrEmpty(y.SalesPersonDescription)
                                                                                                ?
                                                                                                    y.SalesPersonDescription.Contains(',')
                                                                                                    ? y.SalesPersonDescription.Split(',').Last().Trim() + " " + y.SalesPersonDescription.Substring(0, 1)
                                                                                                    : y.SalesPersonDescription.Trim()
                                                                                                : string.Empty)
                                                                                                + "" +
                                                                                                (!string.IsNullOrEmpty(y.SalesPersonDescription)
                                                                                                    ? string.Format(" ({0})", y.SalesPerson.Trim())
                                                                                                    : string.Empty)
                                                                                                )
                                                                                            .Distinct())
                                                            })
                                                            .ToList();
                #endregion

                #region Top 5 and Bottom 5 sales by sales persons
                var listTop5SalesBySalesPerson = dataGroupedResult.GroupBy(x => x.AssignedPersonCode)
                                                            .Select(x => new { salesperson = x.Key, TotalSales = x.Sum(y => y.SalesAmountCurrent) })
                                                            .OrderByDescending(y => y.TotalSales)
                                                            .Take(5)
                                                            .ToList();

                var listBottom5SalesBySalesPerson = dataGroupedResult.GroupBy(x => x.AssignedPersonCode)
                                                    .Select(x => new { salesperson = x.Key, TotalSales = x.Sum(y => y.SalesAmountCurrent) })
                                                    .OrderBy(y => y.TotalSales)
                                                    .Take(5)
                                                    .ToList();
                #endregion

                #region Top 5 and Bottom 5 customers by sales persons
                var listTop5CustomersBySalesPerson = dataGroupedResult.GroupBy(x => x.AssignedPersonCode)
                                                            .Select(x => new
                                                            {
                                                                salesperson = x.Key,
                                                                TotalCustomers = x.Where(f => f.SalesAmountCurrent > 0)
                                                                                                                      .Select(r => r.Customer)
                                                                                                                      .Distinct()
                                                                                                                      .Count()
                                                            })
                                                            .OrderByDescending(y => y.TotalCustomers)
                                                            .Take(5)
                                                            .ToList();

                var listBottom5CustomersBySalesPerson = dataGroupedResult.GroupBy(x => x.AssignedPersonCode)
                                                    .Select(x => new
                                                    {
                                                        salesperson = x.Key,
                                                        TotalCustomers = x.Where(f => f.SalesAmountCurrent > 0)
                                                                                                              .Select(r => r.Customer)
                                                                                                              .Distinct()
                                                                                                              .Count()
                                                    })
                                                    .OrderBy(y => y.TotalCustomers)
                                                    .Take(5)
                                                    .ToList();
                #endregion

                #region Sales By SalesPerson Data
                var resultSalesBySalesPerson = new SalesChartBM
                {
                    Top = dataGroupedResult.Where(x => listTop5SalesBySalesPerson.Any(s => s.salesperson == x.AssignedPersonCode))
                                                    .GroupBy(grp => grp.AssignedPersonCode)
                                                    .Select((x, index) => new SalesSubDataBM
                                                    {
                                                        ColumnName = x.Key,
                                                        ColumnValue = x.Sum(y => y.SalesAmountCurrent).ToRoundTwoDecimalDigits(),
                                                        ColumnValueTarget = x.Sum(y => y.SalesAmountPrior).ToRoundTwoDecimalDigits(),
                                                        Color = ChartColorBM.Colors[index],
                                                        ColumnValueToolTip = x.First().SalesPerson,
                                                        ColumnValueTargetToolTip = x.First().SalesPerson,
                                                        SubData = new SalesChartBM
                                                        {
                                                            Top = x.OrderByDescending(y => y.SalesAmountCurrent)
                                                                   .Take(10)
                                                                  .Select((y, indexSub) => new SalesSubDataBM
                                                                  {
                                                                      Color = ChartColorBM.Colors[indexSub],
                                                                      ColumnName = y.Customer,
                                                                      ColumnPoint = null,
                                                                      ColumnPointTarget = null,
                                                                      ColumnValue = y.SalesAmountCurrent.ToRoundTwoDecimalDigits(),
                                                                      ColumnValueTarget = y.SalesAmountPrior.ToRoundTwoDecimalDigits(),
                                                                  })
                                                                  .OrderByDescending(c => c.ColumnValue)
                                                                 .ToList(),
                                                            Bottom = x.OrderByDescending(y => y.SalesAmountCurrent)
                                                                   .Take(10)
                                                                      .Select((y, indexSub) => new SalesSubDataBM
                                                                      {
                                                                          Color = ChartColorBM.Colors[indexSub],
                                                                          ColumnName = y.Customer,
                                                                          ColumnPoint = null,
                                                                          ColumnPointTarget = null,
                                                                          ColumnValue = y.SalesAmountCurrent.ToRoundTwoDecimalDigits(),
                                                                          ColumnValueTarget = y.SalesAmountPrior.ToRoundTwoDecimalDigits(),
                                                                      })
                                                                      .OrderBy(c => c.ColumnValue)
                                                                     .ToList(),
                                                        }
                                                    })
                                                    .OrderByDescending(c => c.ColumnValue)
                                                    .ToList(),
                    Bottom = dataGroupedResult.Where(x => listBottom5SalesBySalesPerson.Any(s => s.salesperson == x.AssignedPersonCode))
                                                    .GroupBy(grp => grp.AssignedPersonCode)
                                                    .Select((x, index) => new SalesSubDataBM
                                                    {
                                                        ColumnName = x.Key,
                                                        ColumnValue = x.Sum(y => y.SalesAmountCurrent).ToRoundTwoDecimalDigits(),
                                                        ColumnValueTarget = x.Sum(y => y.SalesAmountPrior).ToRoundTwoDecimalDigits(),
                                                        Color = ChartColorBM.Colors[index],
                                                        ColumnValueToolTip = x.First().SalesPerson,
                                                        ColumnValueTargetToolTip = x.First().SalesPerson,
                                                        SubData = new SalesChartBM
                                                        {
                                                            Top = x.OrderByDescending(y => y.SalesAmountCurrent)
                                                                   .Take(10)
                                                                  .Select((y, indexSub) => new SalesSubDataBM
                                                                  {
                                                                      Color = ChartColorBM.Colors[indexSub],
                                                                      ColumnName = y.Customer,
                                                                      ColumnPoint = null,
                                                                      ColumnPointTarget = null,
                                                                      ColumnValue = y.SalesAmountCurrent.ToRoundTwoDecimalDigits(),
                                                                      ColumnValueTarget = y.SalesAmountPrior.ToRoundTwoDecimalDigits(),
                                                                  })
                                                                  .OrderByDescending(c => c.ColumnValue)
                                                                 .ToList(),
                                                            Bottom = x.OrderByDescending(y => y.SalesAmountCurrent)
                                                                   .Take(10)
                                                                  .Select((y, indexSub) => new SalesSubDataBM
                                                                  {
                                                                      Color = ChartColorBM.Colors[indexSub],
                                                                      ColumnName = y.Customer,
                                                                      ColumnPoint = null,
                                                                      ColumnPointTarget = null,
                                                                      ColumnValue = y.SalesAmountCurrent.ToRoundTwoDecimalDigits(),
                                                                      ColumnValueTarget = y.SalesAmountPrior.ToRoundTwoDecimalDigits(),
                                                                  })
                                                                  .OrderBy(c => c.ColumnValue)
                                                                 .ToList(),
                                                        }
                                                    })
                                                    .OrderByDescending(c => c.ColumnValue)
                                                    .ToList()
                };
                #endregion

                #region Customers By SalesPerson Data
                var resultCustomersBySalesPerson = new SalesChartBM
                {
                    Top = dataGroupedResult.Where(x => listTop5CustomersBySalesPerson.Any(s => s.salesperson == x.AssignedPersonCode))
                                                    .GroupBy(grp => grp.AssignedPersonCode)
                                                    .Select((x, index) => new SalesSubDataBM
                                                    {
                                                        ColumnName = x.Key,
                                                        ColumnValue = x.Where(f => f.SalesAmountCurrent > 0).Select(r => r.Customer).Distinct().Count(),
                                                        ColumnValueTarget = x.Where(f => f.SalesAmountPrior > 0).Select(r => r.Customer).Distinct().Count(),
                                                        Color = ChartColorBM.Colors[index],
                                                        ColumnValueToolTip = x.First().SalesPerson,
                                                        ColumnValueTargetToolTip = x.First().SalesPerson,
                                                        SubData = new SalesChartBM
                                                        {
                                                            Top = x.OrderByDescending(y => y.SalesAmountCurrent)
                                                                   .Take(10)
                                                                  .Select((y, indexSub) => new SalesSubDataBM
                                                                  {
                                                                      Color = ChartColorBM.Colors[indexSub],
                                                                      ColumnName = y.Customer,
                                                                      ColumnPoint = null,
                                                                      ColumnPointTarget = null,
                                                                      ColumnValue = y.SalesAmountCurrent.ToRoundTwoDecimalDigits(),
                                                                      ColumnValueTarget = y.SalesAmountPrior.ToRoundTwoDecimalDigits(),
                                                                  })
                                                                  .OrderByDescending(c => c.ColumnValue)
                                                                 .ToList(),
                                                            Bottom = x.OrderByDescending(y => y.SalesAmountCurrent)
                                                                   .Take(10)
                                                                      .Select((y, indexSub) => new SalesSubDataBM
                                                                      {
                                                                          Color = ChartColorBM.Colors[indexSub],
                                                                          ColumnName = y.Customer,
                                                                          ColumnPoint = null,
                                                                          ColumnPointTarget = null,
                                                                          ColumnValue = y.SalesAmountCurrent.ToRoundTwoDecimalDigits(),
                                                                          ColumnValueTarget = y.SalesAmountPrior.ToRoundTwoDecimalDigits(),
                                                                      })
                                                                      .OrderBy(c => c.ColumnValue)
                                                                     .ToList(),
                                                        }
                                                    })
                                                    .OrderByDescending(c => c.ColumnValue)
                                                    .ToList(),
                    Bottom = dataGroupedResult.Where(x => listBottom5CustomersBySalesPerson.Any(s => s.salesperson == x.AssignedPersonCode))
                                                    .GroupBy(grp => grp.AssignedPersonCode)
                                                    .Select((x, index) => new SalesSubDataBM
                                                    {
                                                        ColumnName = x.Key,
                                                        ColumnValue = x.Where(f => f.SalesAmountCurrent > 0).Select(r => r.Customer).Distinct().Count(),
                                                        ColumnValueTarget = x.Where(f => f.SalesAmountPrior > 0).Select(r => r.Customer).Distinct().Count(),
                                                        Color = ChartColorBM.Colors[index],
                                                        ColumnValueToolTip = x.First().SalesPerson,
                                                        ColumnValueTargetToolTip = x.First().SalesPerson,
                                                        SubData = new SalesChartBM
                                                        {
                                                            Top = x.OrderByDescending(y => y.SalesAmountCurrent)
                                                                   .Take(10)
                                                                  .Select((y, indexSub) => new SalesSubDataBM
                                                                  {
                                                                      Color = ChartColorBM.Colors[indexSub],
                                                                      ColumnName = y.Customer,
                                                                      ColumnPoint = null,
                                                                      ColumnPointTarget = null,
                                                                      ColumnValue = y.SalesAmountCurrent.ToRoundTwoDecimalDigits(),
                                                                      ColumnValueTarget = y.SalesAmountPrior.ToRoundTwoDecimalDigits(),
                                                                  })
                                                                  .OrderByDescending(c => c.ColumnValue)
                                                                 .ToList(),
                                                            Bottom = x.OrderByDescending(y => y.SalesAmountCurrent)
                                                                   .Take(10)
                                                                  .Select((y, indexSub) => new SalesSubDataBM
                                                                  {
                                                                      Color = ChartColorBM.Colors[indexSub],
                                                                      ColumnName = y.Customer,
                                                                      ColumnPoint = null,
                                                                      ColumnPointTarget = null,
                                                                      ColumnValue = y.SalesAmountCurrent.ToRoundTwoDecimalDigits(),
                                                                      ColumnValueTarget = y.SalesAmountPrior.ToRoundTwoDecimalDigits(),
                                                                  })
                                                                  .OrderBy(c => c.ColumnValue)
                                                                 .ToList(),
                                                        }
                                                    })
                                                    .OrderByDescending(c => c.ColumnValue)
                                                    .ToList()
                };
                #endregion

                return new List<SalesChartBM> { resultSalesBySalesPerson, resultCustomersBySalesPerson };
            }
        }

        public string GetActiveSalesManName(string nameCombination)
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
        public BarChartTypes GetSalesAndGrowthBySalesPerson(GlobalFilter filter,string userId)
        {
            AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
            var userAccessibleCategories = _adminUserManagementDateProvider.GetUserDetails(userId);

            BarChartTypes response = new BarChartTypes();
            //CaseSoldByGrowOrder();

            try
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));


                parameterList.Add(new SqlParameter("@priorStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filter.Periods.Historical.End));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_SL_GetSalesAndGrowthBySalesPerson", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new BarChartDetails
                            {
                                 Code = !string.IsNullOrEmpty(x.Field<string>("Salesman")) ? x.Field<string>("Salesman").Trim() : "",
                                growth = x.Field<decimal?>("growth").Value,
                                prior = x.Field<decimal?>("prior").Value,
                                value = x.Field<decimal?>("current").Value,
                                Category = !string.IsNullOrEmpty(x.Field<string>("Category")) ? x.Field<string>("Category").Trim() : "",
                                GroupName = GetActiveSalesManName(!string.IsNullOrEmpty(x.Field<string>("SalesManName")) ? x.Field<string>("salesmanname").Trim() : ""),
                            })
                             .Where(x => !userAccessibleCategories.IsRestrictedCategoryAccess || userAccessibleCategories.Categories.Select(o => o.Name).Contains(x.Category))
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
                    newSalesPerson.growth = item.value;
                    newSalesPerson.prior = item.prior;
                    newSalesPerson.LabelPrior = item.LabelPrior;
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

        public BarChartTypes GetSalesAndGrowthByCustomer(GlobalFilter filter, string userId)
        {
            AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
            var userAccessibleCategories = _adminUserManagementDateProvider.GetUserAccess(userId);

            BarChartTypes response = new BarChartTypes();
       
            try
            {
                DateTime minDate = DateTime.MinValue;

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
  

                var dataSetResult = base.ReadToDataSetViaProcedure("BI_SL_GetSalesAndGrowthByCustomer", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new BarChartDetails
                            {
                                Code = !string.IsNullOrEmpty(x.Field<string>("CompanyNumber")) ? x.Field<string>("CompanyNumber").Trim() : "",
                                growth = Convert.ToDecimal(x.Field<Int32?>("growth")),
                                prior = Convert.ToDecimal(x.Field<Int32?>("prior")),
                                value = Convert.ToDecimal(x.Field<Int32?>("current")),
                                GroupName = !string.IsNullOrEmpty(x.Field<string>("CompanyName")) ? x.Field<string>("CompanyName").Trim() : "",
                                Category = !string.IsNullOrEmpty(x.Field<string>("Category")) ? x.Field<string>("Category").Trim() : "",
                                GroupName1 = !string.IsNullOrEmpty(x.Field<string>("CompanyName")) ? x.Field<string>("CompanyName").Trim() : ""

                            })
                            .ToList();


                int i = 0;
                foreach (var item in result)
                {
                    try
                    {
                        if (i == 25) { i = 0; }
                        item.Color1 = TopBottom25GraphColors.Colors[i].Primary;
                        item.Label1 = filter.Periods.Current.Label;
                        item.LabelPrior = filter.Periods.Historical.Label;
                        item.GroupName = GetActiveSalesManName(item.GroupName);
                        item.GroupName1 = GetActiveSalesManName(item.GroupName1);
                        i++;
                    }
                    catch (Exception ex)
                    {

                    }
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

        public List<SalesMapper> GetSalesReportBySalesman(string salesPerspon, int filterId, int period, string comodity)
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
                parameterList.Add(new SqlParameter("@salesman", salesPerspon));
                parameterList.Add(new SqlParameter("@currentStart", startDate));
                parameterList.Add(new SqlParameter("@currentEnd", endDate));
                parameterList.Add(new SqlParameter("@priorStart", prevStartDate));
                parameterList.Add(new SqlParameter("@priorEnd", prevEndDate));
                parameterList.Add(new SqlParameter("@commodity", comodity));

                DataSet dataTableResult = new DataSet();
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
                                            SalesQty  = x.Field<decimal?>("CurrentCasesSold").Value,
                                            CasesSoldCurrent = x.Field<decimal?>("CurrentCasesSold").Value,
                                            SalesAmountCurrent = x.Field<decimal?>("CurrentSales").Value,
                                            SalesAmountPrior = x.Field<decimal?>("PriorSales").Value,
                                            Percentage = x.Field<decimal?>("GrowthSales").Value,
                                            Difference = x.Field<decimal?>("DifferenceSales").Value,
                                            DifferenceCasesSold = x.Field<decimal?>("DifferenceCasesSold").Value,
                                            PercentageCasesSold = x.Field<decimal?>("GrowthCasesSold").Value,

                                        }).ToList();
                    return result;
                }
                else
                    return new List<SalesMapper>();
            }
        }
        public List<CasesSoldDetailsMapper> GetInvoiceDetailsByCustomer(string salesPerspon, int filterId, int period
                                                                        , string customerNumber,string commodity,string orderBy)
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
              
                var dataTableResult = base.ReadToDataSetViaProcedure("BI_CU_GetInvoiceDetailsByCustomer", parameterList.ToArray());
                if (dataTableResult != null && dataTableResult.Tables.Count > 0)
                {
                    var result = dataTableResult.Tables[0]
                                        .AsEnumerable()
                                        .Select(x => new CasesSoldDetailsMapper
                                        {
                                            Comodity = x.Field<string>("Commodity"),
                                            InvoiceDate = x.Field<DateTime>("InvoiceDate"),
                                            InvoiceNumber = x.Field<string>("InvoiceNumber"),
                                            Item = x.Field<string>("ItemCode"),
                                            ItemDesc = x.Field<string>("ItemDescription"),
                                            Quantity = x.Field<decimal?>("CasesSold").Value,
                                            ExtPrice = x.Field<decimal?>("Sales").Value,
                                            SalesMan = x.Field<string>("SalesManDescription"),
                                            Sono = x.Field<string>("SalesOrderNumber"),
                                            Transeq = x.Field<string>("SalesManCode")
                                        }).ToList();

                    if (orderBy == "casessold")
                    {
                        return result.OrderByDescending(x => x.Quantity).ToList();
                    }
                    else if (orderBy == "sales")
                    {
                        return result.OrderByDescending(x => x.ExtPrice).ToList();
                    }
                    return result;
                }
                else
                    return new List<CasesSoldDetailsMapper>();
            }
        }

        public List<CasesSoldDetailsMapper> GetInvoiceDetailsByCustomerForCustomerService(string salesPerspon, int filterId, int period
                                                                      , string customerNumber, string commodity, string orderBy)
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

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_CU_GetInvoiceDetailsByCustomerForCustomerService", parameterList.ToArray());
                if (dataTableResult != null && dataTableResult.Tables.Count > 0)
                {
                    var result = dataTableResult.Tables[0]
                                        .AsEnumerable()
                                        .Select(x => new CasesSoldDetailsMapper
                                        {
                                            Comodity = x.Field<string>("Commodity"),
                                            InvoiceDate = x.Field<DateTime>("InvoiceDate"),
                                            InvoiceNumber = x.Field<string>("InvoiceNumber"),
                                            Item = x.Field<string>("ItemCode"),
                                            ItemDesc = x.Field<string>("ItemDescription"),
                                            Quantity = x.Field<decimal?>("CasesSold").Value,
                                            ExtPrice = x.Field<decimal?>("Sales").Value,
                                            SalesMan = x.Field<string>("SalesManDescription"),
                                            Sono = x.Field<string>("SalesOrderNumber"),
                                            Transeq = x.Field<string>("SalesManCode")
                                        }).ToList();

                    if (orderBy == "casessold")
                    {
                        return result.OrderByDescending(x => x.Quantity).ToList();
                    }
                    else if (orderBy == "sales")
                    {
                        return result.OrderByDescending(x => x.ExtPrice).ToList();
                    }
                    return result;
                }
                else
                    return new List<CasesSoldDetailsMapper>();
            }
        }
    }
}
