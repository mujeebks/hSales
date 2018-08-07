using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using Nogales.BusinessModel;
using Nogales.DataProvider.Infrastructure;

namespace Nogales.DataProvider
{
    public class FinanceDataProvider
    {

        public List<FinanceCollectionDashboardBarColumnChartBO> GetDashboardCollectionData(GlobalFilter filter)
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
                    var result = adoDataAccess.ReadToDataSetViaProcedure("BI_FI_GetCollectionInfoDashboardData", parameterList.ToArray());
                    if (result != null && result.Tables.Count < 1)
                    {
                        throw new Exception("No Records found");
                    }

                    dataSetResultFromDB = result.Tables[0].AsEnumerable().ToList();
                }
                #endregion


                #region projection to DTO
                var projectedResult = dataSetResultFromDB
                                                .Select(x => new FinanceCollectionDashboardDTO
                                                {
                                                    Period = x.Field<string>("Period"),
                                                    Collector = x.Field<string>("Collector"),
                                                    InvoiceAmount = x.Field<decimal>("InvoiceAmount"),
                                                    PaidAmount = x.Field<decimal>("PaidAmount"),
                                                    PaymentOnTimePercentage = x.Field<int>("PaymentOnTime"),
                                                    PNet = x.Field<decimal>("PNet"),
                                                    PTerms = x.Field<string>("PTerms").Trim(),
                                                    TotalCollectionPercentage = x.Field<decimal>("TotalCollection"),
                                                    Ordinance = x.Field<int?>("Ordinance"),
                                                })
                                                .ToList();

                #endregion


                #region projection and mapping to BO
                var listGenericBarColumnChart = new FinanceCollectionDashboardBarColumnChartBO
                {
                    Category = "Invoice Collection",

                    Column1 = filter.Periods.Current.Label,
                    Period1 = filter.Periods.Current.End,
                    Val1 = projectedResult.Where(x => x.Period == "current").Sum(t => t.PaidAmount),
                    Color1 = TopBottom25GraphColors.Colors[5].Primary,

                    InvoiceAmount1 = projectedResult.Where(x => x.Period == "current").Sum(t => t.InvoiceAmount),
                    PaymentCollectionPercentage1 = projectedResult.Where(x => x.Period == "current").Sum(d => d.InvoiceAmount) > 0 ?
                                                     (projectedResult.Where(x => x.Period == "current").Sum(d => d.PaidAmount) /
                                                     projectedResult.Where(x => x.Period == "current").Sum(d => d.InvoiceAmount)) *
                                                     100
                                                     : 0,
                    PaymentOnTimePercentage1 = projectedResult.Any(x => x.Period == "current") ?
                                                projectedResult.Where(x => x.Period == "current").Average(t => t.PaymentOnTimePercentage) :
                                                0,


                    Column2 = filter.Periods.Historical.Label,
                    Period2 = filter.Periods.Historical.End,
                    Val2 = projectedResult.Where(x => x.Period == "historical").Sum(t => t.PaidAmount),
                    Color2 = TopBottom25GraphColors.Colors[5].Secondary,

                    InvoiceAmount2 = projectedResult.Where(x => x.Period == "historical").Sum(t => t.InvoiceAmount),
                    PaymentCollectionPercentage2 = projectedResult.Where(x => x.Period == "historical").Sum(d => d.InvoiceAmount) > 0 ?
                                                     (projectedResult.Where(x => x.Period == "historical").Sum(d => d.PaidAmount) /
                                                     projectedResult.Where(x => x.Period == "historical").Sum(d => d.InvoiceAmount)) *
                                                     100
                                                     : 0,
                    PaymentOnTimePercentage2 = projectedResult.Any(x => x.Period == "historical") ?
                                                projectedResult.Where(x => x.Period == "historical").Average(t => t.PaymentOnTimePercentage) :
                                                0,

                    Column3 = filter.Periods.Prior.Label,
                    Period3 = filter.Periods.Prior.End,
                    Val3 = projectedResult.Where(x => x.Period == "prior").Sum(t => t.PaidAmount),
                    Color3 = TopBottom25GraphColors.Colors[5].Primary,

                    InvoiceAmount3 = projectedResult.Where(x => x.Period == "prior").Sum(t => t.InvoiceAmount),
                    PaymentCollectionPercentage3 = projectedResult.Where(x => x.Period == "prior").Sum(d => d.InvoiceAmount) > 0 ?
                                                     (projectedResult.Where(x => x.Period == "prior").Sum(d => d.PaidAmount) /
                                                     projectedResult.Where(x => x.Period == "prior").Sum(d => d.InvoiceAmount)) *
                                                     100
                                                     : 0,
                    PaymentOnTimePercentage3 = projectedResult.Any(x => x.Period == "prior") ?
                                                projectedResult.Where(x => x.Period == "prior").Average(t => t.PaymentOnTimePercentage) :
                                                0,

                    SubData = projectedResult.GroupBy(x => x.PTerms)
                    //.OrderBy(x=>x.PaidAmount)
                                .Select((grp, index) => new FinanceCollectionDashboardBarColumnChartTermsSubDataBO
                                {
                                    Category = grp.Key,

                                    Column1 = filter.Periods.Current.Label,
                                    Period1 = filter.Periods.Current.End,
                                    Val1 = grp.Where(x => x.Period == "current").Sum(t => t.PaidAmount),
                                    Color1 = TopBottom25GraphColors.Colors[index].Primary,

                                    InvoiceAmount1 = grp.Where(x => x.Period == "current").Sum(t => t.InvoiceAmount),
                                    PaymentCollectionPercentage1 = grp.Where(x => x.Period == "current").Sum(r => r.InvoiceAmount) > 0 ?
                                                                    ((grp.Where(x => x.Period == "current").Sum(t => t.PaidAmount) /
                                                                    grp.Where(x => x.Period == "current").Sum(t => t.InvoiceAmount))
                                                                    * 100)
                                                                    : 0,
                                    PaymentOnTimePercentage1 = grp.Any(x => x.Period == "current") ?
                                                                grp.Where(x => x.Period == "current").Average(t => t.PaymentOnTimePercentage) :
                                                                0,

                                    Column2 = filter.Periods.Historical.Label,
                                    Period2 = filter.Periods.Historical.End,
                                    Val2 = grp.Where(x => x.Period == "historical").Sum(t => t.PaidAmount),
                                    Color2 = TopBottom25GraphColors.Colors[index].Secondary,

                                    InvoiceAmount2 = grp.Where(x => x.Period == "historical").Sum(t => t.InvoiceAmount),
                                    PaymentCollectionPercentage2 = grp.Where(x => x.Period == "historical").Sum(r => r.InvoiceAmount) > 0 ?
                                                                    ((grp.Where(x => x.Period == "historical").Sum(t => t.PaidAmount) /
                                                                    grp.Where(x => x.Period == "historical").Sum(t => t.InvoiceAmount))
                                                                    * 100)
                                                                    : 0,
                                    //PaymentCollectionPercentage2 = grp.Where(x => x.Period == "historical").Average(t => t.TotalCollectionPercentage),
                                    PaymentOnTimePercentage2 = grp.Any(x => x.Period == "historical") ?
                                                                grp.Where(x => x.Period == "historical").Average(t => t.PaymentOnTimePercentage) :
                                                                0,


                                    Column3 = filter.Periods.Prior.Label,
                                    Period3 = filter.Periods.Prior.End,
                                    Val3 = grp.Where(x => x.Period == "prior").Sum(t => t.PaidAmount),
                                    Color3 = TopBottom25GraphColors.Colors[index].Primary,

                                    InvoiceAmount3 = grp.Where(x => x.Period == "prior").Sum(t => t.InvoiceAmount),
                                    //PaymentCollectionPercentage3 = grp.Where(x => x.Period == "prior").Average(t => t.TotalCollectionPercentage),
                                    PaymentCollectionPercentage3 = grp.Where(x => x.Period == "prior").Sum(r => r.InvoiceAmount) > 0 ?
                                                                    ((grp.Where(x => x.Period == "prior").Sum(t => t.PaidAmount) /
                                                                    grp.Where(x => x.Period == "prior").Sum(t => t.InvoiceAmount))
                                                                    * 100)
                                                                    : 0,
                                    PaymentOnTimePercentage3 = grp.Any(x => x.Period == "prior") ?
                                                                grp.Where(x => x.Period == "prior").Average(t => t.PaymentOnTimePercentage) :
                                                                0,

                                    SubData = GenerateCollectorSubData(projectedResult, filter, grp.Key),
                                })
                                .OrderByDescending(r => r.Val1)
                                .ToList()

                };

                #endregion

                return new List<FinanceCollectionDashboardBarColumnChartBO> { listGenericBarColumnChart };

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private FinanceCollectionDashboardBarColumnChartCollectorSubDataBO GenerateCollectorSubData(List<FinanceCollectionDashboardDTO> listFinanceCollectionData, GlobalFilter filter, string pTerms = "")
        {
            var subData = new FinanceCollectionDashboardBarColumnChartCollectorSubDataBO();
            var listFinanceCollectionDataFilteredByPaymentTerms = listFinanceCollectionData.Where(x => x.PTerms == pTerms).ToList();
            //List<String> preferences = new List<String> { "Humberto Hernandez", "Enrique Pineda", "Claudia Campos", "Open Position" };
            var topCollectors = listFinanceCollectionDataFilteredByPaymentTerms
                                            .Where(x => x.PTerms == pTerms && x.Period == "current")
                                            //.OrderBy(x => preferences.IndexOf(x.Collector))
                                            .OrderBy(x=>x.Ordinance)
                                            .Take(10)
                                            .Select(x => x.Collector)
                                            .ToList();

            var bottomCollectors = listFinanceCollectionDataFilteredByPaymentTerms
                                            .Where(x => x.PTerms == pTerms && x.Period == "current")
                                            .OrderByDescending(x => x.Ordinance)
                                            .Take(10)
                                            .Select(x => x.Collector)
                                            .ToList();

            subData.Top = MapDashboardDriverTripData(topCollectors, listFinanceCollectionDataFilteredByPaymentTerms, filter);
            subData.Bottom = MapDashboardDriverTripData(bottomCollectors, listFinanceCollectionDataFilteredByPaymentTerms, filter);

            return subData;
        }

        private List<FinanceCollectionDashboardBarColumnChartBO> MapDashboardDriverTripData(List<string> listCollectors
                                                                                            , List<FinanceCollectionDashboardDTO> listCollectorData
                                                                                            , GlobalFilter filter)
        {
            var result = new List<FinanceCollectionDashboardBarColumnChartBO>();

            listCollectors.ForEach(c =>
            {
                var index = listCollectors.IndexOf(c);

                //var sabu = listDriverData.Where(p => p.Period == "prior" && p.DriverCode == x).ToList();

                var currentCollectorData = listCollectorData.Where(x => x.Collector == c).ToList();

                var collectorData = new FinanceCollectionDashboardBarColumnChartBO
                {
                    Category = c.Trim(),

                    Column1 = filter.Periods.Current.Label,
                    Period1 = filter.Periods.Current.End,
                    Val1 = listCollectorData.Where(x => x.Period == "current" && x.Collector == c).Sum(t => t.PaidAmount),
                    Color1 = TopBottom25GraphColors.Colors[index].Primary,

                    InvoiceAmount1 = listCollectorData.Where(x => x.Period == "current" && x.Collector == c).Sum(t => t.InvoiceAmount),
                    PaymentCollectionPercentage1 = currentCollectorData.Where(x => x.Period == "current").Sum(f => f.InvoiceAmount) > 0 ?
                                                    (currentCollectorData.Where(x => x.Period == "current").Sum(f => f.PaidAmount) /
                                                    currentCollectorData.Where(x => x.Period == "current").Sum(f => f.InvoiceAmount)) *
                                                    100
                                                    : 0,
                    PaymentOnTimePercentage1 = listCollectorData.Any(x => x.Period == "current" && x.Collector == c) ? 
                                                listCollectorData.Where(x => x.Period == "current" && x.Collector == c).Average(t => t.PaymentOnTimePercentage) :
                                                0d,

                    Column2 = filter.Periods.Historical.Label,
                    Period2 = filter.Periods.Historical.End,
                    Val2 = listCollectorData.Where(x => x.Period == "historical" && x.Collector == c).Sum(t => t.PaidAmount),
                    Color2 = TopBottom25GraphColors.Colors[index].Secondary,

                    InvoiceAmount2 = listCollectorData.Where(x => x.Period == "historical" && x.Collector == c).Sum(t => t.InvoiceAmount),
                    //PaymentCollectionPercentage2 = listCollectorData.Any(x => x.Period == "historical" && x.Collector == c) ? listCollectorData.Where(x => x.Period == "historical" && x.Collector == c).Average(t => t.TotalCollectionPercentage) : 0,
                    PaymentOnTimePercentage2 = listCollectorData.Any(x => x.Period == "historical" && x.Collector == c) ? 
                                                listCollectorData.Where(x => x.Period == "historical" && x.Collector == c).Average(t => t.PaymentOnTimePercentage) : 
                                                0d,
                    PaymentCollectionPercentage2 = currentCollectorData.Where(x => x.Period == "historical").Sum(f => f.InvoiceAmount) > 0 ?
                                                    (currentCollectorData.Where(x => x.Period == "historical").Sum(f => f.PaidAmount) /
                                                    currentCollectorData.Where(x => x.Period == "historical").Sum(f => f.InvoiceAmount)) *
                                                    100
                                                    : 0,

                    Column3 = filter.Periods.Prior.Label,
                    Period3 = filter.Periods.Prior.End,
                    Val3 = listCollectorData.Where(x => x.Period == "prior" && x.Collector == c).Sum(t => t.PaidAmount),
                    Color3 = TopBottom25GraphColors.Colors[index].Primary,

                    InvoiceAmount3 = listCollectorData.Where(x => x.Period == "prior" && x.Collector == c).Sum(t => t.InvoiceAmount),
                    //PaymentCollectionPercentage3 = listCollectorData.Any(x => x.Period == "prior" && x.Collector == c) ? listCollectorData.Where(x => x.Period == "prior" && x.Collector == c).Average(t => t.TotalCollectionPercentage) : 0,
                    PaymentOnTimePercentage3 = listCollectorData.Any(x => x.Period == "prior" && x.Collector == c) ? 
                                                listCollectorData.Where(x => x.Period == "prior" && x.Collector == c).Average(t => t.PaymentOnTimePercentage) : 
                                                0d,
                    PaymentCollectionPercentage3 = currentCollectorData.Where(x => x.Period == "prior").Sum(f => f.InvoiceAmount) > 0 ?
                                                    (currentCollectorData.Where(x => x.Period == "prior").Sum(f => f.PaidAmount) /
                                                    currentCollectorData.Where(x => x.Period == "prior").Sum(f => f.InvoiceAmount)) *
                                                    100
                                                    : 0,

                };

                result.Add(collectorData);
            });
            return result;

        }

        public List<FinanceCollectorReportDTO> GetCollectorDetailsReport(DateTime startDate, DateTime endDate, string pTerms, string collector)
        {
            var dataSetResultFromDB = new List<DataRow>();

            #region sql parameters
            var parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@currentStart", startDate.ToString("yyyy/MM/dd")));
            parameterList.Add(new SqlParameter("@currentEnd", endDate.ToString("yyyy/MM/dd")));
            parameterList.Add(new SqlParameter("@pterms", pTerms));
            parameterList.Add(new SqlParameter("@collector", collector));
            #endregion

            #region fetching data from database
            using (var adoDataAccess = new DataAccessADO())
            {
                var result = adoDataAccess.ReadToDataSetViaProcedure("BI_FI_GetCollectorDetailsReport", parameterList.ToArray());
                if (result != null && result.Tables.Count < 1)
                {
                    throw new Exception("No Records found");
                }
                dataSetResultFromDB = new List<DataRow>();
                dataSetResultFromDB = result.Tables[0].AsEnumerable().ToList();

                #region projection to dayReportBO
                var collectorReportBO = dataSetResultFromDB
                                    .Select(x => new FinanceCollectorReportDTO
                                    {
                                        BalanceAmount = x.Field<decimal>("balance"),
                                        CollectionOnTimePercentage = x.Field<decimal>("totalCollection"),
                                        //Collector = x.Field<int>("collector"),
                                        CollectorName = x.Field<string>("CollectorName"),
                                        CustomerCode = x.Field<string>("custno"),
                                        CustomerName = x.Field<string>("company"),
                                        DatePaid = x.Field<DateTime?>("dtepaid"),
                                        DueDate = x.Field<DateTime>("dueDate"),
                                        InvoiceAmount = x.Field<decimal>("invamt"),
                                        InvoiceDate = x.Field<DateTime>("invdte"),
                                        InvoiceNumber = x.Field<string>("invno"),
                                        DaysOverDue = x.Field<int>("overDueDate"),
                                        AmountCollected = x.Field<decimal>("paidamt"),
                                        PaymentOnTimePercentage = x.Field<int>("payOnTime"),
                                        PNet = x.Field<decimal>("pnet"),
                                        PTerms = x.Field<string>("pterms"),
                                        SalesManCode = x.Field<string>("salesmn"),
                                        SalesManName = x.Field<string>("SalesPersonDescription"),
                                    })
                                    .ToList();
                #endregion

                return collectorReportBO.ToList();
            }
            #endregion
        }
    }
}
