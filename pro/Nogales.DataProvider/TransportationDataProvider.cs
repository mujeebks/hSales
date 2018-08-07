using Nogales.BusinessModel;
using Nogales.DataProvider.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;

namespace Nogales.DataProvider
{
    public class TransportationDataProvider
    {
        public List<TransportationTripDashboardBarColumnChartBO> GetDashboardDiverTripData(GlobalFilter filter)
        {
            try
            {
                var dataSetResultFromDB = new List<DataRow>();

                #region sql parameters
                var parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@historicalStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filter.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@priorStart", filter.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filter.Periods.Prior.End));
                #endregion

                #region fetching data from database
                using (var adoDataAccess = new DataAccessADO())
                {
                    var result = adoDataAccess.ReadToDataSetViaProcedure("BI_TR_GetDriverTripsDashboardData", parameterList.ToArray());
                    if (result != null && result.Tables.Count < 1)
                    {
                        throw new Exception("No Records found");
                    }

                    dataSetResultFromDB = result.Tables[0].AsEnumerable().ToList();
                }
                #endregion

                #region projection to DTO
                var projectedResult = dataSetResultFromDB
                                                .Select(x => new TransportationDashboardTripDTO
                                                {
                                                    Period = x.Field<string>("Period"),
                                                    DriverCode = x.Field<string>("DriverCode"),
                                                    DriverName = x.Field<string>("DriverName"),
                                                    RouteType = x.Field<string>("RouteType"),
                                                    NumberOfTrips = x.Field<int>("NoOfTrips"),
                                                    NumberOfStops = x.Field<int>("NoOfStops"),
                                                    CasesDelivered = x.Field<decimal>("Cases")
                                                })
                                                .ToList();
                #endregion

                #region projection and mapping to BO
                var listGenericBarColumnChart = projectedResult.GroupBy(x => x.RouteType)
                                       .Select((x, index) => new TransportationTripDashboardBarColumnChartBO
                                       {
                                           Category = x.Key,

                                           Column1 = filter.Periods.Current.Label,
                                           Period1 = filter.Periods.Current.End,
                                           Val1 = x.Where(p => p.Period == "current").Sum(t => t.NumberOfTrips),
                                           NumberOfStops1 = x.Where(p => p.Period == "current").Sum(t => t.NumberOfStops),
                                           CasesDelivered1 = x.Where(p => p.Period == "current").Sum(t => t.CasesDelivered),
                                           Color1 = TopBottom25GraphColors.Colors[index].Primary,
                                           //(index == 0) ? ChartColorBM.ExpenseCurrent : ChartColorBM.MarginCurrent,

                                           Column2 = filter.Periods.Historical.Label,
                                           Period2 = filter.Periods.Historical.End,
                                           Val2 = x.Where(p => p.Period == "historical").Sum(t => t.NumberOfTrips),
                                           NumberOfStops2 = x.Where(p => p.Period == "historical").Sum(t => t.NumberOfStops),
                                           CasesDelivered2 = x.Where(p => p.Period == "historical").Sum(t => t.CasesDelivered),
                                           Color2 = TopBottom25GraphColors.Colors[index].Secondary,

                                           Column3 = filter.Periods.Prior.Label,
                                           Period3 = filter.Periods.Prior.End,
                                           Val3 = x.Where(p => p.Period == "prior").Sum(t => t.NumberOfTrips),
                                           NumberOfStops3 = x.Where(p => p.Period == "prior").Sum(t => t.NumberOfStops),
                                           CasesDelivered3 = x.Where(p => p.Period == "prior").Sum(t => t.CasesDelivered),
                                           Color3 = TopBottom25GraphColors.Colors[index].Primary,




                                           SubData = GenerateDashboardDriverTripSubData(x.ToList(), filter),
                                       })
                                       .ToList();

                #endregion

                return listGenericBarColumnChart;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public TransportaionDriverTripDayAndDetailedReportBO GetDriverDayAndDetailedReport(DateTime startDate, DateTime endDate, string driverCode = "", string route = "")
        {
            try
            {
                var dataSetDayResultFromDB = new List<DataRow>();
                var dataSetDetailResultFromDB = new List<DataRow>();

                #region sql parameters
                var parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", startDate.ToString("yyyy/MM/dd")));
                parameterList.Add(new SqlParameter("@currentEnd", endDate.ToString("yyyy/MM/dd")));
                parameterList.Add(new SqlParameter("@driverCode", driverCode));
                parameterList.Add(new SqlParameter("@route", route));
                #endregion

                #region fetching data from database
                using (var adoDataAccess = new DataAccessADO())
                {
                    var result = adoDataAccess.ReadToDataSetViaProcedure("BI_TR_GetDriverTripsDayAndDetailsReport", parameterList.ToArray());
                    if (result != null && result.Tables.Count < 1)
                    {
                        throw new Exception("No Records found");
                    }

                    dataSetDayResultFromDB = result.Tables[0].AsEnumerable().ToList();
                    dataSetDetailResultFromDB = result.Tables[1].AsEnumerable().ToList();
                }
                #endregion

                #region projection to detailedReportBO
                var detailedReportBO = dataSetDetailResultFromDB
                                                .Select(x => new TransportationDriverTripDetailedReportBO
                                                {
                                                    Address = x.Field<string>("Address").Trim(),
                                                    City = x.Field<string>("City").Trim(),
                                                    CustomerCode = x.Field<string>("CustomerCode").Trim(),
                                                    CustomerName = x.Field<string>("CustomerName").Trim(),
                                                    InvoiceDate = x.Field<DateTime>("InvoiceDate"),
                                                    InvoiceDateString = x.Field<DateTime>("InvoiceDate").ToString("MMM dd, yyyy"),
                                                    InvoiceNumber = x.Field<string>("InvoiceNumber").Trim(),
                                                    Reference = x.Field<string>("Reference").Trim(),
                                                    Route = x.Field<string>("Route").Trim(),
                                                    RouteRun = x.Field<string>("RouteRun"),
                                                    State = x.Field<string>("State").Trim(),
                                                    TruckCode = x.Field<string>("TruckCode").Trim(),
                                                    Zip = x.Field<string>("Zip").Trim(),
                                                    DriverCode = x.Field<string>("DriverCode").Trim(),
                                                    DriverName = x.Field<string>("DriverName").Trim(),
                                                    RouteType = x.Field<string>("RouteType"),
                                                    CasesDelivered = x.Field<decimal>("CasesDelivered")
                                                })
                                                .ToList();
                #endregion

                #region projection to customer report
                var customerReportBO = detailedReportBO
                                                    .GroupBy(x => new { x.CustomerCode, x.InvoiceDate, x.DriverCode })
                                                    .Select(x => new TransportationDriverTripDetailedReportBO
                                                    {
                                                        InvoiceDate = x.Key.InvoiceDate,
                                                        InvoiceDateString = x.Key.InvoiceDate.ToString("MMM dd, yyyy"),
                                                        Address = x.First().Address,
                                                        CustomerCode = x.Key.CustomerCode,
                                                        City = x.First().City,
                                                        CasesDelivered = x.Sum(c => c.CasesDelivered),
                                                        CustomerName = x.First().CustomerName,
                                                        DriverCode = x.Key.DriverCode,
                                                        DriverName = x.First().DriverName,
                                                        NumberOfInvoices = x.Select(y => y.InvoiceNumber).Distinct().Count(),
                                                        Reference = x.First().Reference,
                                                        Route = x.First().Route,
                                                        RouteRun = x.First().RouteRun,
                                                        RouteType = x.First().RouteType,
                                                        State = x.First().State,
                                                        TruckCode = x.First().TruckCode,
                                                        Zip = x.First().Zip
                                                    })
                                                    .ToList(); 
                #endregion

                #region projection to dayReportBO
                var dayReportBO = dataSetDayResultFromDB
                                    .Select(x => new TransportaionDriverTripConsolidatedReportBO
                                    {
                                        InvoiceDate = x.Field<DateTime>("InvoiceDate"),
                                        InvoiceDateString = x.Field<DateTime>("InvoiceDate").ToString("MMM dd, yyyy"),
                                        DriverCode = x.Field<string>("DriverCode").Trim(),
                                        DriverName = x.Field<string>("DriverName").Trim(),
                                        Route = x.Field<string>("Route"),
                                        RouteType = x.Field<string>("RouteType"),
                                        CasesDelivered = x.Field<decimal>("Cases"),
                                        NumberOfStops = x.Field<int>("NoOfStops"),
                                        NumberOfTrips = x.Field<int>("NoOfTrips"),
                                    })
                                    .ToList();
                #endregion

                var returnBO = new TransportaionDriverTripDayAndDetailedReportBO
                {
                    DayReport = dayReportBO,
                    DetailedReport = detailedReportBO,
                    CustomerReport = customerReportBO
                };
                return returnBO;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<TransportaionDriverTripConsolidatedReportBO> GetDriverTripConsolidatedReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                var dataSetResultFromDB = new List<DataRow>();

                #region sql parameters
                var parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", startDate.ToString("yyyy/MM/dd")));
                parameterList.Add(new SqlParameter("@currentEnd", endDate.ToString("yyyy/MM/dd")));
                #endregion

                #region fetching data from database
                using (var adoDataAccess = new DataAccessADO())
                {
                    var result = adoDataAccess.ReadToDataSetViaProcedure("BI_TR_GetAllDriversTripsConsolidatedReport", parameterList.ToArray());
                    if (result != null && result.Tables.Count < 1)
                    {
                        throw new Exception("No Records found");
                    }
                    dataSetResultFromDB = new List<DataRow>();
                    dataSetResultFromDB = result.Tables[0].AsEnumerable().ToList();
                }
                #endregion

                #region projection to dayReportBO
                var dayReportBO = dataSetResultFromDB
                                    .Select(x => new TransportaionDriverTripConsolidatedReportBO
                                    {
                                        DriverCode = x.Field<string>("DriverCode").Trim(),
                                        DriverName = x.Field<string>("DriverName").Trim(),
                                        RouteType = x.Field<string>("RouteType"),
                                        CasesDelivered = x.Field<decimal>("Cases"),
                                        NumberOfStops = x.Field<int>("NoOfStops"),
                                        NumberOfTrips = x.Field<int>("NoOfTrips"),
                                    })
                                    .ToList();
                #endregion

                return dayReportBO.OrderByDescending(x => x.NumberOfTrips).ToList();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<TransportationRouteConsolidatedReportBO> GetRouteConsolidatedReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                var dataSetResultFromDB = new List<DataRow>();

                #region sql parameters
                var parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", startDate.ToString("yyyy/MM/dd")));
                parameterList.Add(new SqlParameter("@currentEnd", endDate.ToString("yyyy/MM/dd")));
                #endregion

                #region fetching data from database
                using (var adoDataAccess = new DataAccessADO())
                {
                    var result = adoDataAccess.ReadToDataSetViaProcedure("BI_TR_GetAllRoutesConsolidatedReport", parameterList.ToArray());
                    if (result != null && result.Tables.Count < 1)
                    {
                        throw new Exception("No Records found");
                    }
                    dataSetResultFromDB = new List<DataRow>();
                    dataSetResultFromDB = result.Tables[0].AsEnumerable().ToList();
                }
                #endregion

                #region projection to dayReportBO
                var dayReportBO = dataSetResultFromDB
                                    .Select(x => new TransportationRouteConsolidatedReportBO
                                    {
                                        Route = x.Field<string>("Route").Trim(),
                                        RouteType = x.Field<string>("RouteType"),
                                        CasesDelivered = x.Field<decimal>("Cases"),
                                        NumberOfStops = x.Field<int>("NoOfStops"),
                                        NumberOfDrivers = x.Field<int>("NoOfDrivers"),
                                    })
                                    .ToList();
                #endregion

                return dayReportBO.OrderByDescending(x => x.NumberOfDrivers).ToList();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private TransportationTripDashboardBarColumnChartSubDataBO GenerateDashboardDriverTripSubData(List<TransportationDashboardTripDTO> listDriverData, GlobalFilter filter)
        {
            var subData = new TransportationTripDashboardBarColumnChartSubDataBO();

            var topDrivers = listDriverData.Where(x => x.Period == "current").OrderByDescending(x => x.NumberOfStops).Take(10).Select(x => x.DriverCode).ToList();
            var bottomDrivers = listDriverData.Where(x => x.Period == "current").OrderBy(x => x.NumberOfStops).Take(10).Select(x => x.DriverCode).ToList();

            subData.Top = MapDashboardDriverTripData(topDrivers, listDriverData, filter);
            subData.Bottom = MapDashboardDriverTripData(bottomDrivers, listDriverData, filter);

            return subData;
        }

        private List<TransportationTripDashboardBarColumnChartBO> MapDashboardDriverTripData(List<string> listDrivers
                                                                                           , List<TransportationDashboardTripDTO> listDriverData
                                                                                           , GlobalFilter filter)
        {
            var result = new List<TransportationTripDashboardBarColumnChartBO>();

            listDrivers.ForEach(x =>
            {
                var index = listDrivers.IndexOf(x);

                //var sabu = listDriverData.Where(p => p.Period == "prior" && p.DriverCode == x).ToList();

                var driverData = new TransportationTripDashboardBarColumnChartBO
                {
                    Category = x.Trim(),
                    DriverName = listDriverData.First(d => d.DriverCode == x).DriverName.Trim(),

                    Column1 = filter.Periods.Current.Label,
                    Period1 = filter.Periods.Current.End,
                    Val1 = listDriverData.Where(p => p.Period == "current" && p.DriverCode == x).Sum(t => t.NumberOfTrips),
                    NumberOfStops1 = listDriverData.Where(p => p.Period == "current" && p.DriverCode == x).Sum(t => t.NumberOfStops),
                    CasesDelivered1 = listDriverData.Where(p => p.Period == "current" && p.DriverCode == x).Sum(t => t.CasesDelivered),
                    Color1 = TopBottom25GraphColors.Colors[index].Primary,

                    Column2 = filter.Periods.Historical.Label,
                    Period2 = filter.Periods.Historical.End,
                    Val2 = listDriverData.Where(p => p.Period == "historical" && p.DriverCode == x).Sum(t => t.NumberOfTrips),
                    NumberOfStops2 = listDriverData.Where(p => p.Period == "historical" && p.DriverCode == x).Sum(t => t.NumberOfStops),
                    CasesDelivered2 = listDriverData.Where(p => p.Period == "historical" && p.DriverCode == x).Sum(t => t.CasesDelivered),
                    Color2 = TopBottom25GraphColors.Colors[index].Secondary,

                    Column3 = filter.Periods.Prior.Label,
                    Period3 = filter.Periods.Prior.End,
                    Val3 = listDriverData.Where(p => p.Period == "prior" && p.DriverCode == x).Sum(t => t.NumberOfTrips),
                    NumberOfStops3 = listDriverData.Where(p => p.Period == "prior" && p.DriverCode == x).Sum(t => t.NumberOfStops),
                    CasesDelivered3 = listDriverData.Where(p => p.Period == "prior" && p.DriverCode == x).Sum(t => t.CasesDelivered),
                    Color3 = TopBottom25GraphColors.Colors[index].Primary,


                };

                result.Add(driverData);
            });
            return result;

        }
    }


}
