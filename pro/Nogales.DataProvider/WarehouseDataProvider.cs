using Nogales.BusinessModel;
using Nogales.DataProvider.Infrastructure;
using Nogales.DataProvider.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Nogales.DataProvider
{
    /// <summary>
    /// Provider for the Warehouse
    /// </summary>
    public class WarehouseDataProvider : DataAccessADO
    {
        /// <summary>
        /// Get data for the short report
        /// </summary>
        /// <param name="shipDate"> Date of shipment </param>
        /// <param name="routeNumber"> Filter data based on the routeNumber. If it is empty string then doesn't perform filtering based on rout number </param>
        /// <param name="buyerId"> Filter data based on the buyerId. If it is a empty string then doesn't perform filtering based on buyer id </param>
        /// <param name="selelectedIds"> Filter data based on the Id (primary key of the table SOTRAN). If it is null then doesn't perform filtering based on Ids </param>
        /// <returns> Short report </returns>
        public List<WarehouseShortReportBM> GetShortReport(string Date, string routeNumber, string buyerId, List<int> selelectedIds = null)
        {
            try
            {
                string query = string.Empty;
                var dataSetResult = new DataSet();
                DataTable dtIds = new DataTable();
                dtIds.Columns.Add("Id", typeof(int));

                if (selelectedIds == null || selelectedIds.Count <= 0)
                {

                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@shipDate", Date));
                    parameterList.Add(new SqlParameter("@routNo", routeNumber ?? ""));
                    parameterList.Add(new SqlParameter("@buyerName", buyerId ?? ""));
                    dataSetResult = base.ReadToDataSetViaProcedure("BI_WH_GetWarehouseShortReport", parameterList.ToArray());
                    //query = SQLQueries.GetWarehouseShortReportQuery(Date, routeNumber, buyerId);
                }
                else
                {

                    foreach (int id in selelectedIds)
                    {
                        dtIds.Rows.Add(id);
                    }
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@shipDate", Date));
                    parameterList.Add(new SqlParameter("@routNo", routeNumber));
                    parameterList.Add(new SqlParameter("@buyerName", buyerId));
                    //parameterList.Add(new SqlParameter("@selelectedIds", string.Join(",", selelectedIds)));
                    var param = new SqlParameter();
                    param.SqlDbType = SqlDbType.Structured;
                    param.Value = dtIds;
                    param.ParameterName = "@selelectedIds";
                    parameterList.Add(param);

                    dataSetResult = base.ReadToDataSetViaProcedure("BI_WH_GetWarehouseShortReportbyIds", parameterList.ToArray());
                    //query = SQLQueries.GetWarehouseShortReportFilterIdsQuery(Date, routeNumber, buyerId, selelectedIds);
                }
                
                var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new WarehouseShortReportBM
                            {
                                Id = x.Field<int>("Id"),
                                Buyer = x.Field<string>("buyer").Trim(),
                                Customer = x.Field<string>("customer").Trim(),
                                Description = x.Field<string>("description").Trim(),
                                Item = x.Field<string>("item").Trim(),
                                MarketPrice = x.Field<decimal>("Mkt.Price"),
                                //QuantityNeeded = x.Field<decimal>("QtyNeeded"),
                                Route = x.Field<string>("route").Trim(),
                                SalesOrderNumber = x.Field<string>("SO Num").Trim(),
                                TransactionCost = x.Field<decimal>("Trans.Cost"),
                                UOM = x.Field<string>("UoM").Trim(),
                                Email = x.Field<string>("email").Trim(),
                                QuantityOnHand = x.Field<decimal>("QtyOnHand"),
                                QuantityOrd = x.Field<decimal>("QtyOrdered"),
                                QtyAvailable = x.Field<decimal>("QtyAvailable"),
                                BinNo = !string.IsNullOrEmpty(x.Field<string>("binno")) ? x.Field<string>("binno").Trim() : "",
                                Picker = !string.IsNullOrEmpty(x.Field<string>("picker")) ? x.Field<string>("picker").Trim() : "",
                                PickerName = !string.IsNullOrEmpty(x.Field<string>("pickername")) ? x.Field<string>("pickername").Trim() : ""

                            })
                            .OrderBy(x => x.Customer)
                            .ToList();

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
        }

        public List<PickerProductivityReportBM> GetPickerProductivityReport(PickerProductivityReportFilterBM model)
        {
            try
            {
                DateTime sDate, eDate;
                DateTime sTime, eTime;
                sTime = eTime = new DateTime();
                #region Validation
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                if (!DateTime.TryParse(model.StartDate, out sDate))
                {
                    throw new Exception("Start Date Invalid.");
                }
                if (!DateTime.TryParse(model.EndDate, out eDate))
                {
                    throw new Exception("End Date Invalid.");
                }
                if (model.StartTime != null && !DateTime.TryParseExact(model.StartTime, "hh:mm tt", provider, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out sTime))
                {
                    throw new Exception("Start time is invalid");
                }
                if (model.EndTime != null && !DateTime.TryParseExact(model.EndTime, "hh:mm tt", provider, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out eTime))
                {
                    throw new Exception("Start time is invalid");
                }
                #endregion
                //var query = SQLQueries
                //                .GetPickerProductivityReportQuery(empId
                //                , sDate.ToString("MM/dd/yyyy 00:00:00")
                //                , eDate.ToString("MM/dd/yyyy 23:59:59"));
                //var dataSetResult = base.ReadToDataSet(query);
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@empId", model.EmployeID ?? ""));
                parameterList.Add(new SqlParameter("@startDate", DateTime.Parse(sDate.ToString("MM/dd/yyyy 00:00:00"))));
                parameterList.Add(new SqlParameter("@endDate", DateTime.Parse(eDate.ToString("MM/dd/yyyy 23:59:59"))));
                parameterList.Add(new SqlParameter("@startTime", model.StartTime == null ? "" : sTime.ToString("HH:mm:ss")));
                parameterList.Add(new SqlParameter("@endTime", model.EndTime == null ? "" : eTime.ToString("HH:mm:ss")));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetPickerProductivityReport", parameterList.ToArray());

                var result = dataSetResult.Tables[0]
                    .AsEnumerable()
                    .Select(x => new PickerProductivityReportBM
                    {
                        EmployeeId = x.Field<string>("EMPID"),
                        Name = x.Field<string>("Name"),
                        PiecesPicked = x.Field<decimal>("PiecePicked"),
                        HoursWorked = x.Field<decimal>("HoursWorked"),
                        PiecesPerHour = x.Field<decimal>("PiecesPerHour"),
                        StartTime = x.Field<DateTime>("StartTime"),
                        EndTime = x.Field<DateTime>("EndTime"),
                    })
                    .OrderBy(x => x.Name)
                    .ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database-" + e.Message);
            }

        }

        public List<PickerProductivityReportDetailsBM> GetPickerProductivityReportDetails(PickerProductivityReportFilterBM model)
        {
            try
            {
                DateTime sDate, eDate;
                DateTime sTime, eTime;
                sTime = eTime = new DateTime();
                #region Validation
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                if (!DateTime.TryParse(model.StartDate, out sDate))
                {
                    throw new Exception("Start Date Invalid.");
                }
                if (!DateTime.TryParse(model.EndDate, out eDate))
                {
                    throw new Exception("End Date Invalid.");
                }
                if (model.StartTime != null && !DateTime.TryParseExact(model.StartTime, "hh:mm tt", provider, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out sTime))
                {
                    throw new Exception("Start time is invalid");
                }
                if (model.EndTime != null && !DateTime.TryParseExact(model.EndTime, "hh:mm tt", provider, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out eTime))
                {
                    throw new Exception("Start time is invalid");
                }
                #endregion
                //var query = SQLQueries
                //                .GetPickerProductivityReportQuery(empId
                //                , sDate.ToString("MM/dd/yyyy 00:00:00")
                //                , eDate.ToString("MM/dd/yyyy 23:59:59"));
                //var dataSetResult = base.ReadToDataSet(query);
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@empId", model.EmployeID ?? ""));
                parameterList.Add(new SqlParameter("@startDate", DateTime.Parse(sDate.ToString("MM/dd/yyyy 00:00:00"))));
                parameterList.Add(new SqlParameter("@endDate", DateTime.Parse(eDate.ToString("MM/dd/yyyy 23:59:59"))));
                parameterList.Add(new SqlParameter("@startTime", model.StartTime == null ? "" : sTime.ToString("HH:mm:ss")));
                parameterList.Add(new SqlParameter("@endTime", model.EndTime == null ? "" : eTime.ToString("HH:mm:ss")));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetPickerProductivityReportDetails", parameterList.ToArray());

                var result = dataSetResult.Tables[0]
                    .AsEnumerable()
                    .Select(x => new PickerProductivityReportDetailsBM
                    {
                        EmployeeId = x.Field<string>("EMPID"),
                        Name = x.Field<string>("Name"),
                        PiecesPicked = x.Field<decimal>("PiecePicked"),
                        HoursWorked = x.Field<decimal>("HoursWorked"),
                        PiecesPerHour = x.Field<decimal>("PiecesPerHour"),
                        StartTime = x.Field<DateTime>("StartTime"),
                        EndTime = x.Field<DateTime>("EndTime"),
                        Item = x.Field<string>("item"),
                        ItemDesc = x.Field<string>("itmdesc")
                    })
                    .ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database-" + e.Message);
            }

        }

        public async Task<decimal> GetTotalPickerProductivityReport(string endDate)
        {
            DateTime eDate;
            if (!DateTime.TryParse(endDate, out eDate))
            {
                throw new Exception("Date Invalid.");
            }
            var query = SQLQueries.GetTotalProductivityMonthtodate(eDate.ToString("MM/01/yyyy 00:00:00")
                                                                    , eDate.ToString("MM/dd/yyyy 23:59:59"));

            var dataSetResult = await Task.Run(() => base.ReadToDataSet(query));
            var result = dataSetResult.Tables[0].Rows[0][0].ToString();

            return Convert.ToDecimal(string.IsNullOrEmpty(result) ? "0" : result);
        }

        public decimal GetTotalPickerProductivityReportSync(string endDate)
        {
            DateTime eDate;
            if (!DateTime.TryParse(endDate, out eDate))
            {
                throw new Exception("Date Invalid.");
            }
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@startDate", DateTime.Parse(eDate.ToString("MM/01/yyyy 00:00:00"))));
            parameterList.Add(new SqlParameter("@endDate", DateTime.Parse(eDate.ToString("MM/dd/yyyy 23:59:59"))));
            var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetTotalPickerProductivityReport", parameterList.ToArray());
            var result = dataSetResult.Tables[0].Rows[0][0].ToString();

            return Convert.ToDecimal(string.IsNullOrEmpty(result) ? "0" : result);
        }

        public async Task<DashboardStatisticsBM> GetProductivityReport(string endDate)
        {
            var currentProductivity = this.GetTotalPickerProductivityReportSync(endDate);
            decimal previousProductivity = 0;
            DateTime date = new DateTime();
            if (DateTime.TryParse(endDate, out date))
            {
                previousProductivity = this.GetTotalPickerProductivityReportSync(date.AddMonths(-1).ToString("MM-dd-yyyy"));
            }
            var commonProvider = new CommonDataProvider();
            var result = new DashboardStatisticsBM() { Name = "Productivity", Amount = (double)currentProductivity, PreviousMonthAmount = (double)previousProductivity, Change = commonProvider.DoubleToPercentageString(UtilityExtensions.CalculateChange((double)previousProductivity, (double)currentProductivity)) };
            return result;
        }

        public async Task<List<PickerProductivityChartBM>> GetPickerProductivityChart(GlobalFilter filter)
        {
            DateTime sDate = filter.Periods.Current.Start;
            DateTime eDate = filter.Periods.Current.End;
            try
            {


                var dataTable = new DataSet();
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startDate", Convert.ToDateTime(sDate.ToString("MM/dd/yyyy 00:00:00"))));
                parameterList.Add(new SqlParameter("@endDate", Convert.ToDateTime(eDate.ToString("MM/dd/yyyy 23:59:59"))));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetPickerProductivity", parameterList.ToArray());

                var result = dataSetResult.Tables[0]
                    .AsEnumerable()
                    .Select(x => new PickerProductivityChartBM
                    {
                        AvgProductivity = x.Field<decimal>(0),
                        Date = x.Field<DateTime>(1).ToString("yyyy-MM-dd"),
                    })
                    .OrderBy(x => x.Date)
                    .ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database-" + e.Message);
            }



        }

        public async Task<List<PickerProductivityChartBM>> GetPickerProductivityChartTemp(string beginDate, string endDate)
        {

            try
            {
                DateTime sDate;
                DateTime eDate;
                #region Validation
                if (!DateTime.TryParse(beginDate, out sDate))
                {
                    throw new Exception("Start Date Invalid.");
                }
                if (!DateTime.TryParse(endDate, out eDate))
                {
                    throw new Exception("End Date Invalid.");
                }
                #endregion


                var dataTable = new DataSet();
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startDate", Convert.ToDateTime(sDate.ToString("MM/dd/yyyy 00:00:00"))));
                parameterList.Add(new SqlParameter("@endDate", Convert.ToDateTime(eDate.ToString("MM/dd/yyyy 23:59:59"))));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetPickerProductivity", parameterList.ToArray());

                var result = dataSetResult.Tables[0]
                    .AsEnumerable()
                    .Select(x => new PickerProductivityChartBM
                    {
                        AvgProductivity = x.Field<decimal>(0),
                        Date = x.Field<DateTime>(1).ToString("yyyy-MM-dd"),
                    })
                    .OrderBy(x => x.Date)
                    .ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database-" + e.Message);
            }



        }

        public decimal GetPickerProductivityForecast(int predictionDay, string beginDate, string endDate)
        {

            try
            {
                DateTime sDate;
                DateTime eDate;
                #region Validation
                if (!DateTime.TryParse(beginDate, out sDate))
                {
                    throw new Exception("Start Date Invalid.");
                }
                if (!DateTime.TryParse(endDate, out eDate))
                {
                    throw new Exception("End Date Invalid.");
                }
                #endregion
                var query = SQLQueries
                                .GetPickerProductivityForecastQuery(predictionDay, sDate.ToString("MM/dd/yyyy 00:00:00")
                                , eDate.ToString("MM/dd/yyyy 23:59:59"));
                var dataSetResult = base.ReadToDataSet(query);
                var result = dataSetResult.Tables[0].Rows[0][0].ToString();

                return Convert.ToDecimal(string.IsNullOrEmpty(result) ? "0" : result);

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database-" + e.Message);
            }



        }


        public void InsertSOShortNotificationDetails(List<int> id, List<string> email, string date, string type)
        {
            List<SqlParameter> parameterList = new List<SqlParameter>();
            DataTable dtIds = new DataTable();
            dtIds.Columns.Add("Id", typeof(int));
            foreach (int ids in id)
            {
                dtIds.Rows.Add(ids);
            }
            //parameterList.Add(new SqlParameter("@id", string.Join(",", id)));
            parameterList.Add(new SqlParameter("@email", string.Join(",", email)));
            parameterList.Add(new SqlParameter("@date", DateTime.Parse(date)));
            parameterList.Add(new SqlParameter("@type", type));
            var param = new SqlParameter();
            param.SqlDbType = SqlDbType.Structured;
            param.Value = dtIds;
            param.ParameterName = "@id";
            parameterList.Add(param);
            var result = base.ExecuteNonQueryFromStoredProcedure("BI_InsertSOShortNotified", parameterList.ToArray());
            //using (var adoDataAccess = new DataAccessADO())
            //{
            //    var mail = string.Join(", ", email);
            //    var Id = string.Join("','", id);
            //    var query = SQLQueries.InsertSOShortNotified(Id, mail, date, type);
            //    var result = ExecuteQuery(query);
            //}
        }


        public void InsertSOShortIgnoredReason(List<int> id, string Reason, string date, string type)
        {
            DataTable dtIds = new DataTable();
            dtIds.Columns.Add("Id", typeof(int));
            foreach (int ids in id)
            {
                dtIds.Rows.Add(ids);
            }
            List<SqlParameter> parameterList = new List<SqlParameter>();
            //parameterList.Add(new SqlParameter("@id", string.Join(",", id)));
            var param = new SqlParameter();
            param.SqlDbType = SqlDbType.Structured;
            param.Value = dtIds;
            param.ParameterName = "@id";
            parameterList.Add(param);
            parameterList.Add(new SqlParameter("@email", Reason));
            parameterList.Add(new SqlParameter("@date", DateTime.Parse(date)));
            parameterList.Add(new SqlParameter("@type", type));

            var result = base.ExecuteNonQueryFromStoredProcedure("BI_InsertSOShortNotified", parameterList.ToArray());
            //using (var adoDataAccess = new DataAccessADO())
            //{
            //    var Id = string.Join("','", id);
            //    var query = SQLQueries.InsertSOShortNotified(Id, Reason, date, type);
            //    var result = ExecuteQuery(query);
            //}
        }

        public List<WarehouseShortReportBM> GetNotifiedIgnoredShortReports(string shipDate, string routeNumber, string buyerId)
        {
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@shipDate", shipDate));
                parameterList.Add(new SqlParameter("@routNo", routeNumber));
                parameterList.Add(new SqlParameter("@buyerName", buyerId));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetIgnoredShortReport", parameterList.ToArray());
                //var query = SQLQueries
                //                 .GetNotifiedIgnoredShortReportQuery(shipDate, routeNumber, buyerId);
                //var dataSetResult = base.ReadToDataSet(query);
                var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new WarehouseShortReportBM
                            {
                                Id = x.Field<int>("Id"),
                                Buyer = x.Field<string>("buyer").Trim(),
                                Customer = x.Field<string>("customer").Trim(),
                                Description = x.Field<string>("description").Trim(),
                                Item = x.Field<string>("item").Trim(),
                                MarketPrice = x.Field<decimal>("tPrice"),
                                QuantityOrd = x.Field<decimal>("qtyord"),
                                QuantityOnHand = x.Field<decimal>("qtyonhand"),
                                QtyAvailable = x.Field<decimal>("qtyavailable"),
                                //QuantityNeeded = x.Field<decimal>("QtyNeeded"),
                                Route = x.Field<string>("route").Trim(),
                                SalesOrderNumber = x.Field<string>("SONo").Trim(),
                                TransactionCost = x.Field<decimal>("tCost"),
                                UOM = x.Field<string>("umeasur").Trim(),
                                Email = x.Field<string>("email").Trim(),
                                Type = x.Field<string>("type").Trim(),
                                Notes = x.Field<string>("notes").Trim()
                            })
                            .ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database-" + e.Message);
            }

        }

        public decimal GetPickerForcastReport(string startDate, string endDate)
        {
            try
            {
                DateTime sDate;
                DateTime eDate;
                #region Validation
                if (!DateTime.TryParse(startDate, out sDate))
                {
                    throw new Exception("Start Date Invalid.");
                }
                if (!DateTime.TryParse(endDate, out eDate))
                {
                    throw new Exception("End Date Invalid.");
                }
                #endregion

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startDate", Convert.ToDateTime(sDate.ToString("MM/dd/yyyy 00:00:00"))));
                parameterList.Add(new SqlParameter("@endDate", Convert.ToDateTime(eDate.ToString("MM/dd/yyyy 23:59:59"))));
                var result = base.ExecuteScalarFromStoredProcedure("BI_GetPickerForCastReport", parameterList.ToArray());
                //var query = SQLQueries
                //                .GetPickerForcastReportQuery(sDate.ToString("MM/dd/yyyy 00:00:00")
                //                , eDate.ToString("MM/dd/yyyy 23:59:59"));
                //var result = base.ExecuteScalar(query);
                ////var result = dataSetResult.Tables[0].Rows[0][0].ToString();

                return Convert.ToDecimal(string.IsNullOrEmpty(Convert.ToString(result)) ? "0" : Convert.ToString(result));

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database-" + e.Message);
            }
        }

        public List<PickerProductivityReportBM> GetPickerForcastQuantityPickedReport(string startDate, string endDate)
        {

            try
            {
                DateTime sDate;
                DateTime eDate;
                #region Validation
                if (!DateTime.TryParse(startDate, out sDate))
                {
                    throw new Exception("Start Date Invalid.");
                }
                if (!DateTime.TryParse(endDate, out eDate))
                {
                    throw new Exception("End Date Invalid.");
                }
                #endregion

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startDate", Convert.ToDateTime(sDate.ToString("MM/dd/yyyy 00:00:00"))));
                parameterList.Add(new SqlParameter("@endDate", Convert.ToDateTime(eDate.ToString("MM/dd/yyyy 23:59:59"))));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetPickerForcastQuantityPickedReport", parameterList.ToArray());
                //var query = SQLQueries
                //              .GetPickerForcastQuantityPickedReportQuery(sDate.ToString("MM/dd/yyyy 00:00:00"), eDate.ToString("MM/dd/yyyy 23:59:59"));
                //var dataSetResult = base.ReadToDataSet(query);
                var result = dataSetResult.Tables[0]
                    .AsEnumerable()
                    .Select(x => new PickerProductivityReportBM
                    {

                        Name = x.Field<string>("Name"),
                        PiecesPicked = x.Field<decimal>("PiecesPicked"),
                        HoursWorked = x.Field<decimal>("HoursWorked"),
                        PiecesPerHour = x.Field<decimal>("PiecesPerHour")
                    })
                    .ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database-" + e.Message);
            }
        }

        public List<DumpAndDonaReportBM> GetDumpAndDonation(DateTime start, DateTime end)
        {

            try
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startDate", start));
                parameterList.Add(new SqlParameter("@endDate", end));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetDonationAndDump", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
      .AsEnumerable()
      .Select(x => new DumpAndDonaReportBM
      {

          Customer = x.Field<string>("custno"),
          Item = x.Field<string>("Item"),
          CustomerDesc = x.Field<string>("company"),
          ItemDesc = x.Field<string>("ItemName"),
          Revenue = x.Field<decimal>("Revenue"),
          ExtendedCost = x.Field<decimal>("Cost").ToStripDecimal(),
          QuantityShipped = x.Field<int>("Quantity"),
          Type = x.Field<string>("Type"),
          Vendor = x.Field<string>("Vendor"),
          VendorName = x.Field<string>("VendorName"),
          BuyerId = x.Field<string>("BuyerId"),
          Date = x.Field<DateTime>("Date"),
          IsConsigned = x.Field<int>("IsConsigned"),
      })
      .OrderBy(x => x.ItemDesc)
      .ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database-" + e.Message);
            }
        }

        /*New Alterations*/

        public List<ReportWithoutBINBM> GetSalesOrderWithoutBinNo(List<string> SalesPersons, DateTime date, string sono)
        {

            try
            {
                DataTable dtIds = new DataTable();
                dtIds.Columns.Add("Id", typeof(string));
                foreach (string ids in SalesPersons)
                {
                    dtIds.Rows.Add(ids);
                }
                List<SqlParameter> parameterList = new List<SqlParameter>();
                //parameterList.Add(new SqlParameter("@id", string.Join(",", id)));
                var param = new SqlParameter();
                param.SqlDbType = SqlDbType.Structured;
                param.Value = dtIds;
                param.ParameterName = "@SalesPerson";
                parameterList.Add(param);

                parameterList.Add(new SqlParameter("@date", Convert.ToDateTime(date.ToString("MM/dd/yyyy 00:00:00"))));
                parameterList.Add(new SqlParameter("@sono", sono));
                var dataSetResult = base.ReadToDataSetViaProcedure("BI_WH_GetSalesOrderNoBinReport", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                                          .AsEnumerable()
                                          .Select(x => new ReportWithoutBINBM
                                          {

                                              SalesOrderNumber = x.Field<string>("sono"),
                                              Item = x.Field<string>("item"),
                                              QuantityShipped = x.Field<int>("tqtyshp"),
                                              Date = x.Field<DateTime>("date"),
                                              Puller = x.Field<string>("name"),
                                              PullerId = x.Field<string>("puller"),
                                              SalesPerson = x.Field<string>("SalesPerson"),
                                              SalesPersonName = !string.IsNullOrEmpty(x.Field<string>("SalesPersonName")) ? x.Field<string>("SalesPersonName").Trim() : "",
                                          })
                                          .OrderBy(x => x.Item)
                                          .ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database-" + e.Message);
            }
        }

        public List<WarehousePickerProductivityDashboardBarColumnChartBO> GetPickerProductivityDashboardData(GlobalFilter filter)
        {
            var dataSetResultFromDB = new List<DataRow>();

            Func<WarehouseDashboardPickerProductivityDTO, bool> currentPerdicate = x => x.Period == "current";
            Func<WarehouseDashboardPickerProductivityDTO, bool> priorPerdicate = x => x.Period == "prior";
            Func<WarehouseDashboardPickerProductivityDTO, bool> historicalPerdicate = x => x.Period == "historical";

            #region sql parameters
            var parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@currentStart", DateTime.Parse(filter.Periods.Current.Start.ToString("MM/dd/yyyy 00:00:00"))));
            parameterList.Add(new SqlParameter("@currentEnd", DateTime.Parse(filter.Periods.Current.End.ToString("MM/dd/yyyy 23:59:59"))));
            parameterList.Add(new SqlParameter("@historicalStart", DateTime.Parse(filter.Periods.Historical.Start.ToString("MM/dd/yyyy 00:00:00"))));
            parameterList.Add(new SqlParameter("@historicalEnd", DateTime.Parse(filter.Periods.Historical.End.ToString("MM/dd/yyyy 23:59:59"))));
            parameterList.Add(new SqlParameter("@priorStart", DateTime.Parse(filter.Periods.Prior.Start.ToString("MM/dd/yyyy 00:00:00"))));
            parameterList.Add(new SqlParameter("@priorEnd", DateTime.Parse(filter.Periods.Prior.End.ToString("MM/dd/yyyy 23:59:59"))));
            #endregion

            #region fetching data from database
            using (var adoDataAccess = new DataAccessADO())
            {
                var result = adoDataAccess.ReadToDataSetViaProcedure("BI_WH_GetPickerProductivityDashboardData", parameterList.ToArray());
                if (result != null && result.Tables.Count < 1)
                {
                    throw new Exception("No Records found");
                }

                dataSetResultFromDB = result.Tables[0].AsEnumerable().ToList();
            }
            #endregion

            #region projection to DTO
            var projectedResult = dataSetResultFromDB
                                            .Select(x => new WarehouseDashboardPickerProductivityDTO
                                            {
                                                Period = x.Field<string>("Period"),
                                                UserId = x.Field<string>("UserId"),
                                                UserName = x.Field<string>("UserName"),
                                                MinutesWorked = x.Field<int>("MinutesWorked"),
                                                QtyPicked = x.Field<decimal>("QtyPicked")
                                            })
                                            .ToList();
            #endregion

            var barColumnChart = new WarehousePickerProductivityDashboardBarColumnChartBO
            {
                Category = "",

                Column1 = filter.Periods.Current.Label,
                Period1 = filter.Periods.Current.End,
                Val1 = ((decimal)(projectedResult.Where(currentPerdicate).Sum(x => x.MinutesWorked)) / 60) > 0 ?
                       projectedResult.Where(currentPerdicate).Sum(x => x.QtyPicked) /
                       (decimal)((double)projectedResult.Where(currentPerdicate).Sum(x => x.MinutesWorked) / 60)
                       : 0,
                HoursWorked1 = (projectedResult.Where(currentPerdicate).Sum(x => x.MinutesWorked) / 60),
                PiecesPicked1 = projectedResult.Where(currentPerdicate).Sum(x => x.QtyPicked),
                Color1 = TopBottom25GraphColors.Colors[0].Primary,

                Column2 = filter.Periods.Historical.Label,
                Period2 = filter.Periods.Historical.End,
                Val2 = ((decimal)(projectedResult.Where(historicalPerdicate).Sum(x => x.MinutesWorked)) / 60) > 0 ?
                       projectedResult.Where(historicalPerdicate).Sum(x => x.QtyPicked) /
                       (decimal)((double)projectedResult.Where(historicalPerdicate).Sum(x => x.MinutesWorked) / 60)
                       : 0,
                HoursWorked2 = (projectedResult.Where(historicalPerdicate).Sum(x => x.MinutesWorked) / 60),
                PiecesPicked2 = projectedResult.Where(historicalPerdicate).Sum(x => x.QtyPicked),
                Color2 = TopBottom25GraphColors.Colors[0].Secondary,

                Column3 = filter.Periods.Prior.Label,
                Period3 = filter.Periods.Prior.End,
                Val3 = ((decimal)(projectedResult.Where(priorPerdicate).Sum(x => x.MinutesWorked)) / 60) > 0 ?
                       projectedResult.Where(priorPerdicate).Sum(x => x.QtyPicked) /
                       (decimal)((double)projectedResult.Where(priorPerdicate).Sum(x => x.MinutesWorked) / 60)
                       : 0,
                HoursWorked3 = (projectedResult.Where(priorPerdicate).Sum(x => x.MinutesWorked) / 60),
                PiecesPicked3 = projectedResult.Where(priorPerdicate).Sum(x => x.QtyPicked),
                Color3 = TopBottom25GraphColors.Colors[0].Primary,

                SubData = GeneratePickerProductivityDashboardSubData(projectedResult, filter),

            };

            //adding this to a list for the UI directive.
            var retrunData = new List<WarehousePickerProductivityDashboardBarColumnChartBO>();
            retrunData.Add(barColumnChart);

            return retrunData;
        }


        private WarehousePickerProductivityDashboardBarColumnChartSubDataBO GeneratePickerProductivityDashboardSubData
            (List<WarehouseDashboardPickerProductivityDTO> listPickerData, GlobalFilter filter)
        {

            var subData = new WarehousePickerProductivityDashboardBarColumnChartSubDataBO();

            var currentPickers = listPickerData.Where(x => x.Period == "current")
                                                .GroupBy(t => t.UserId)
                                                .Select(x => new
                                                {
                                                    UserId = x.Key,
                                                    Average = (x.Sum(r => r.MinutesWorked) > 0) ? x.Sum(r => r.QtyPicked) / (decimal)((double)x.Sum(r => r.MinutesWorked) / 60) : 0
                                                })
                                                .ToList();



            var topPickers = currentPickers.OrderByDescending(x => x.Average).Take(10).Select(x => x.UserId).ToList();
            var bottomPickers = currentPickers.OrderBy(x => x.Average).Take(10).Select(x => x.UserId).ToList();

            subData.Top = MapDashboardPickerProductivityDashboardData(topPickers, listPickerData, filter);
            subData.Bottom = MapDashboardPickerProductivityDashboardData(bottomPickers, listPickerData, filter);

            return subData;
        }

        private List<WarehousePickerProductivityDashboardBarColumnChartBO> MapDashboardPickerProductivityDashboardData(List<string> listPickers
                                                                                                                        , List<WarehouseDashboardPickerProductivityDTO> listPickerData
                                                                                                                        , GlobalFilter filter)
        {
            var result = new List<WarehousePickerProductivityDashboardBarColumnChartBO>();


            listPickers.ForEach(userId =>
            {
                var index = listPickers.IndexOf(userId);

                Func<WarehouseDashboardPickerProductivityDTO, bool> currentPerdicate = x => x.Period == "current" && x.UserId == userId;
                Func<WarehouseDashboardPickerProductivityDTO, bool> priorPerdicate = x => x.Period == "prior" && x.UserId == userId;
                Func<WarehouseDashboardPickerProductivityDTO, bool> historicalPerdicate = x => x.Period == "historical" && x.UserId == userId;

                var pickerData = new WarehousePickerProductivityDashboardBarColumnChartBO
                {
                    Category = listPickerData.First(c => c.UserId == userId).UserName.Trim(),
                    PickerId = userId,

                    Column1 = filter.Periods.Current.Label,
                    Period1 = filter.Periods.Current.End,
                    Val1 = ((decimal)(listPickerData.Where(currentPerdicate).Sum(x => x.MinutesWorked)) / 60) > 0 ?
                            listPickerData.Where(currentPerdicate).Sum(x => x.QtyPicked) /
                            (decimal)((double)listPickerData.Where(currentPerdicate).Sum(x => x.MinutesWorked) / 60)
                            : 0,
                    HoursWorked1 = (listPickerData.Where(currentPerdicate).Sum(x => x.MinutesWorked) / 60),
                    PiecesPicked1 = listPickerData.Where(currentPerdicate).Sum(x => x.QtyPicked),
                    Color1 = TopBottom25GraphColors.Colors[index].Primary,

                    Column2 = filter.Periods.Historical.Label,
                    Period2 = filter.Periods.Historical.End,
                    Val2 = ((decimal)(listPickerData.Where(historicalPerdicate).Sum(x => x.MinutesWorked)) / 60) > 0 ?
                           listPickerData.Where(historicalPerdicate).Sum(x => x.QtyPicked) /
                           (decimal)((double)listPickerData.Where(historicalPerdicate).Sum(x => x.MinutesWorked) / 60)
                           : 0,
                    HoursWorked2 = (listPickerData.Where(historicalPerdicate).Sum(x => x.MinutesWorked) / 60),
                    PiecesPicked2 = listPickerData.Where(historicalPerdicate).Sum(x => x.QtyPicked),
                    Color2 = TopBottom25GraphColors.Colors[index].Secondary,

                    Column3 = filter.Periods.Prior.Label,
                    Period3 = filter.Periods.Prior.End,
                    Val3 = ((decimal)(listPickerData.Where(priorPerdicate).Sum(x => x.MinutesWorked)) / 60) > 0 ?
                           listPickerData.Where(priorPerdicate).Sum(x => x.QtyPicked) /
                           (decimal)((double)listPickerData.Where(priorPerdicate).Sum(x => x.MinutesWorked) / 60)
                           : 0,
                    HoursWorked3 = (listPickerData.Where(priorPerdicate).Sum(x => x.MinutesWorked) / 60),
                    PiecesPicked3 = listPickerData.Where(priorPerdicate).Sum(x => x.QtyPicked),
                    Color3 = TopBottom25GraphColors.Colors[index].Primary,
                };
                result.Add(pickerData);
            });

            return result;
        }


        public List<WarehousePickerProductivityDayReportBO> GetPickerProductivityDayReport(DateTime startDate
                                                                                            , DateTime endDate
                                                                                            , string userId)
        {
            try
            {
                var dataSetResultFromDB = new List<DataRow>();

                #region sql parameters
                var parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", startDate.ToString("MM/dd/yyyy 00:00:00")));
                parameterList.Add(new SqlParameter("@currentEnd", endDate.ToString("MM/dd/yyyy 23:59:59")));
                parameterList.Add(new SqlParameter("@userId", userId));
                #endregion

                #region fetching data from database
                using (var adoDataAccess = new DataAccessADO())
                {
                    var result = adoDataAccess.ReadToDataSetViaProcedure("BI_WH_GetPickerProductivityDayReport", parameterList.ToArray());
                    if (result != null && result.Tables.Count < 1)
                    {
                        throw new Exception("No Records found");
                    }

                    dataSetResultFromDB = result.Tables[0].AsEnumerable().ToList();
                }
                #endregion


                #region projection to DTO
                var projectedResult = dataSetResultFromDB
                                                .Select(x => new WarehousePickerProductivityDayReportBO
                                                {
                                                    AveragePiecesPicked = x.Field<decimal>("AvgPiecesPicked"),
                                                    UserId = x.Field<string>("UserId"),
                                                    TaskDate = x.Field<DateTime>("TaskDate"),
                                                    TaskDateString = x.Field<DateTime>("TaskDate").ToString("MMM dd, yyyy"),
                                                    StartTime = x.Field<DateTime>("StartTime"),
                                                    StartTimeString = x.Field<DateTime>("StartTime").ToString("hh:mm tt"),
                                                    EndTime = x.Field<DateTime>("EndTime"),
                                                    EndTimeString = x.Field<DateTime>("EndTime").ToString("hh:mm tt"),
                                                    PiecesPicked = x.Field<decimal>("QtyPicked"),
                                                    HoursWorked = x.Field<decimal>("HoursWorked")
                                                })
                                                .ToList();
                #endregion

                return projectedResult;

            }
            catch (Exception)
            {

                throw;
            }


        }

        public List<PickerProductivityReportDetailsBM> GetPickerProductivityDayDetailReport(PickerProductivityReportFilterBM model)
        {
            var dataSetResultFromDB = new List<DataRow>();

            #region sql parameters
            var parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@startDate", model.StartDate));
            parameterList.Add(new SqlParameter("@endDate", model.EndDate));
            parameterList.Add(new SqlParameter("@startTime", model.StartTime));
            parameterList.Add(new SqlParameter("@endTime", model.EndTime));
            parameterList.Add(new SqlParameter("@empId", model.EmployeID));
            #endregion

            #region fetching data from database
            using (var adoDataAccess = new DataAccessADO())
            {
                var result = adoDataAccess.ReadToDataSetViaProcedure("BI_WH_GetPickerProductivityReportDetails", parameterList.ToArray());
                if (result != null && result.Tables.Count < 1)
                {
                    throw new Exception("No Records found");
                }
                dataSetResultFromDB = new List<DataRow>();
                dataSetResultFromDB = result.Tables[0].AsEnumerable().ToList();
            }
            #endregion

            #region projection to dayReportBO
            var dayDetailsReportBO = dataSetResultFromDB
                                .Select(x => new PickerProductivityReportDetailsBM
                                {
                                    Name = x.Field<string>("Name").Trim(),
                                    EmployeeId = x.Field<string>("EmpId").Trim(),
                                    Item = x.Field<string>("Item").Trim(),
                                    ItemDesc = x.Field<string>("ItemDesc").Trim(),
                                    SalesOrderNo = x.Field<string>("SalesOrderNo").Trim(),
                                    PiecesPicked = x.Field<decimal>("PiecePicked"),
                                    HoursWorked = x.Field<decimal>("HoursWorked"),
                                    PiecesPerHour = x.Field<decimal>("PiecesPerHour"),
                                    StartTime = x.Field<DateTime>("StartTime"),
                                    EndTime = x.Field<DateTime>("EndTime"),
                                    StartTimeString = x.Field<DateTime>("StartTime").ToString("hh:mm tt"),
                                    EndTimeString = x.Field<DateTime>("EndTime").ToString("hh:mm tt"),
                                })
                                .ToList();
            #endregion

            return dayDetailsReportBO;
        }

    }

}
