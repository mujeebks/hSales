using Nogales.BusinessModel;
using Nogales.DataProvider.ENUM;
using Nogales.DataProvider.Infrastructure;
using Nogales.DataProvider.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
namespace Nogales.DataProvider
{
    public class JournalDataProvider : DataAccessADO
    {
        public List<APJournalReportBM> GetExpenseReport(string accountNumberStart, string accountNumberEnd,
            string startDate, string endDate, string accountName)
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

            var query = SQLQueries
                        .GetExpenseDrillDownQuery(accountNumberStart, accountNumberEnd, startDate, endDate, accountName);

            var dataSetResult = base.ReadToDataSet(query);
            var result = dataSetResult.Tables[0]
            .AsEnumerable()
                .Select(x => new APJournalReportBM
                {
                    Amount = x.Field<decimal>("Amount"),
                    Description = x.Field<string>("Description").Trim(),
                    GLAccount = x.Field<string>("GLAccount").Trim(),
                    GLBatch = x.Field<string>("GLBatch").Trim(),
                    InvoiceNumber = x.Field<string>("InvoiceNumber").Trim(),
                    ReferenceNumber = x.Field<string>("ReferenceNumber").Trim(),
                    Session = x.Field<string>("APSession").Trim(),
                    TransactionDate = x.Field<DateTime>("TransactionDate"),
                    Vendor = x.Field<string>("VendorNumber").Trim(),
                    AccountName = x.Field<string>("AccountName").Trim()
                })
                .ToList();
            return result;
        }

        public List<APJournalReportBM> GetAPJournalReport(JournalFilterBM filterData)
        {
            DateTime sDate;
            DateTime eDate;
            #region Validation
            if (!DateTime.TryParse(filterData.StartDate, out sDate))
            {
                throw new Exception("Start Date Invalid.");
            }
            if (!DateTime.TryParse(filterData.EndDate, out eDate))
            {
                throw new Exception("End Date Invalid.");
            }
            #endregion
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@start", sDate));
            parameterList.Add(new SqlParameter("@end", DateTime.Parse(eDate.ToString("yyyy/MM/dd 23:59:59"))));
            parameterList.Add(new SqlParameter("@acc_start", filterData.AccountNumberStart));
            parameterList.Add(new SqlParameter("@acc_end", filterData.AccountNumberEnd));
            parameterList.Add(new SqlParameter("@session_start", filterData.StartSession));
            parameterList.Add(new SqlParameter("@session_end", filterData.EndSession));
            parameterList.Add(new SqlParameter("@batch_start", filterData.StartBatch));
            parameterList.Add(new SqlParameter("@batch_end", filterData.EndBatch));
            parameterList.Add(new SqlParameter("@vendor_number", filterData.VendorNumber));
            parameterList.Add(new SqlParameter("@second_session_start", !string.IsNullOrEmpty(filterData.SecondStartSession) ? filterData.SecondStartSession : ""));
            parameterList.Add(new SqlParameter("@second_session_end", !string.IsNullOrEmpty(filterData.SecondEndSession) ? filterData.SecondEndSession : ""));
            parameterList.Add(new SqlParameter("@invoice", filterData.InvoiceNumber));

            var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetAPJournalReport", parameterList.ToArray());

            //var query = SQLQueries
            //            .GetAPJournalReport(filterData.AccountNumberStart
            //                                , filterData.AccountNumberEnd
            //                                , filterData.StartSession
            //                                , filterData.EndSession
            //                                , filterData.StartBatch
            //                                , filterData.EndBatch
            //                                , sDate.ToString("yyyy/MM/dd 00:00:00")
            //                                , eDate.ToString("yyyy/MM/dd 23:59:59")
            //                                , filterData.VendorNumber
            //                                , filterData.SecondStartSession
            //                                , filterData.SecondEndSession
            //                                , filterData.InvoiceNumber);


            //var dataSetResult = base.ReadToDataSet(query);
            var result = dataSetResult.Tables[0]
            .AsEnumerable()
                .Select(x => new APJournalReportBM
                {
                    Amount = x.Field<decimal>("Amount"),
                    Description = x.Field<string>("Description").Trim(),
                    GLAccount = x.Field<string>("GLAccount").Trim(),
                    GLBatch = x.Field<string>("GLBatch").Trim(),
                    InvoiceNumber = x.Field<string>("InvoiceNumber").Trim(),
                    ReferenceNumber = x.Field<string>("ReferenceNumber").Trim(),
                    Session = x.Field<string>("APSession").Trim(),
                    TransactionDate = x.Field<DateTime>("TransactionDate"),
                    Vendor = x.Field<string>("VendorNumber").Trim(),
                    AccountName = x.Field<string>("AccountName").Trim()
                })
                .OrderByDescending(p=>p.Amount)
                .ToList();
            return result;
        }

        /// <summary>
        /// Get AP Journal Chart
        /// </summary>
        /// <param name="filterData"></param>
        /// <returns></returns>
        public OPEXCOGSExpenseJournalBM GetAPJournalChart(GlobalFilter filters)
        {
            var result = new OPEXCOGSExpenseJournalBM();
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@currentStart", filters.Periods.Current.Start));
            parameterList.Add(new SqlParameter("@currentEnd", filters.Periods.Current.End));
            parameterList.Add(new SqlParameter("@historicalStart", filters.Periods.Historical.Start));
            parameterList.Add(new SqlParameter("@historicalEnd", filters.Periods.Historical.End));
            parameterList.Add(new SqlParameter("@priorStart", filters.Periods.Prior.Start));
            parameterList.Add(new SqlParameter("@priorEnd", filters.Periods.Prior.End));
            parameterList.Add(new SqlParameter("@CogsAccountStart", Constants.CogsAccountStart));
            parameterList.Add(new SqlParameter("@CogsAccountEnd", Constants.CogsAccountEnd));
            parameterList.Add(new SqlParameter("@OpexAccountStart", Constants.OpexAccountStart));
            parameterList.Add(new SqlParameter("@OpexAccountEnd", Constants.OpexAccountEnd));

            var resultData = base.ReadToDataSetViaProcedure("BI_GetEmployeeOPEXCOGSPayRollDetails", parameterList.ToArray());


            var listDataResult = resultData.Tables[0]
                                        .AsEnumerable()
                                        .Select(x =>
                                            new APJournalBM
                                            {
                                                AccountName = x.Field<string>("AccountName"),
                                                AccountNumber = x.Field<string>("AccountNumber"),
                                                Amount = x.Field<decimal>("Amount"),
                                                AccountType = x.Field<string>("AccountType"),
                                                CurrPrev = x.Field<string>("Curprev"),
                                                DepartmentCode = x.Field<int?>("DepartmentCode") ?? 0,
                                                DepartmentName = x.Field<string>("DepartmentName")
                                            })

                                        .ToList();


            result.OPEXCOGSExpenseJournalChart = new List<OPEXCOGSExpenseJournalChartBM>()
            {
                listDataResult.GetOpexChartData(x => x.AccountType == "OPEX", filters),
                listDataResult.GetCogsChartData(x => x.AccountType == "COGS", filters)
        };


            parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@currentStart", filters.Periods.Current.Start));
            parameterList.Add(new SqlParameter("@currentEnd", filters.Periods.Current.End));
            parameterList.Add(new SqlParameter("@historicalStart", filters.Periods.Historical.Start));
            parameterList.Add(new SqlParameter("@historicalEnd", filters.Periods.Historical.End));
            parameterList.Add(new SqlParameter("@priorStart", filters.Periods.Prior.Start));
            parameterList.Add(new SqlParameter("@priorEnd", filters.Periods.Prior.End));
            parameterList.Add(new SqlParameter("@CogsAccountStart", Constants.CogsAccountStart));
            parameterList.Add(new SqlParameter("@CogsAccountEnd", Constants.CogsAccountEnd));
            parameterList.Add(new SqlParameter("@OpexAccountStart", Constants.OpexAccountStart));
            parameterList.Add(new SqlParameter("@OpexAccountEnd", Constants.OpexAccountEnd));

            resultData = base.ReadToDataSetViaProcedure("BI_EP_GetEmployeeOPEXCOGSPayRollDetailsHistorical", parameterList.ToArray());

            //    var resultDataCurrentPrior = base.ReadToDataSetViaProcedure("BI_EP_GetEmployeeOPEXCOGSPayRollDetails", parameterList.ToArray());

            var payrollDatatableToList0 = GetEmployeePayrollDetails(resultData.Tables[0]);
            //    var payrollDatatableToList1 = GetEmployeePayrollDetailsCurrentPrior(resultDataCurrentPrior.Tables[0]);

            result.PayrollJournalChart = MappingPayroll(payrollDatatableToList0, filters);

            var query = payrollDatatableToList0
                            .GroupBy(c => c.EmployeeId)
                            .Select(g => new MapEmployeePayrollData
                            {
                  
                            });
            return result;


        }

        /// <summary>
        /// Get AP Journal Chart
        /// </summary>
        /// <param name="filterData"></param>
        /// <returns></returns>
        public OPEXCOGSExpenseJournalBM GetAPJournalChartTemp(string startDate, string endDate, string category)
        {
            var result = new OPEXCOGSExpenseJournalBM();
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

            string currenStarttDate = sDate.ToString("yyyy/MM/dd 00:00:00");
            string currentEndDate = eDate.ToString("yyyy/MM/dd 23:59:59");
            string previousStartDate = string.Empty;
            string previousEndDate = string.Empty;
            string columnStartDate = string.Empty;
            string columnPrevDate = string.Empty;
            var date = DateTime.Parse(endDate);

            if (category == Constants.COGSOPEXJournalByMonth)
            {
                previousStartDate = sDate.AddMonths(-1).ToString("yyyy/MM/dd 00:00:00");

                previousEndDate = eDate.AddMonths(-1).ToString("yyyy/MM/dd 23:59:59");
                columnStartDate = sDate.ToString("MMM") + " " + sDate.Year;
                if (sDate.Month == 1)
                    columnPrevDate = sDate.AddMonths(-1).ToString("MMM") + " " + sDate.AddYears(-1).Year;
                else
                    columnPrevDate = sDate.AddMonths(-1).ToString("MMM") + " " + sDate.Year;

            }
            else if (category == Constants.COGSOPEXJournalByYear || category == Constants.COGSOPEXJournalByYearToMonth)
            {
                previousStartDate = sDate.AddYears(-1).ToString("yyyy/MM/dd 00:00:00");
                previousEndDate = eDate.AddYears(-1).ToString("yyyy/MM/dd 23:59:59");
                columnStartDate = sDate.Year.ToString();
                columnPrevDate = sDate.AddYears(-1).Year.ToString();
            }
            else
            {
                return null;
            }
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@startDate", currenStarttDate));
            parameterList.Add(new SqlParameter("@endDate", currentEndDate));
            parameterList.Add(new SqlParameter("@CogsAccountStart", Constants.CogsAccountStart));
            parameterList.Add(new SqlParameter("@CogsAccountEnd", Constants.CogsAccountEnd));
            parameterList.Add(new SqlParameter("@OpexAccountStart", Constants.OpexAccountStart));
            parameterList.Add(new SqlParameter("@OpexAccountEnd", Constants.OpexAccountEnd));
            parameterList.Add(new SqlParameter("@prevStartDate", previousStartDate));
            parameterList.Add(new SqlParameter("@prevEndDate", previousEndDate));

            var resultData = base.ReadToDataSetViaProcedure("BI_GetEmployeeOPEXCOGSPayRollDetails", parameterList.ToArray());
            //string Query = SQLQueries.GetOPEXCOGSChartQuerys(
            //                                      currenStarttDate
            //                                    , currentEndDate
            //                                    ,previousStartDate
            //                                    , previousEndDate);


            //var resultData = base.ReadToDataSet(Query);

            var listDataResult = resultData.Tables[0]
                                        .AsEnumerable()
                                        .Select(x =>
                                            new APJournalBM
                                            {
                                                AccountName = x.Field<string>("AccountName"),
                                                AccountNumber = x.Field<string>("AccountNumber"),
                                                Amount = x.Field<decimal>("Amount"),
                                                AccountType = x.Field<string>("AccountType"),
                                                CurrPrev = x.Field<string>("Curprev"),
                                                DepartmentCode = x.Field<int?>("DepartmentCode") ?? 0,
                                                DepartmentName = x.Field<string>("DepartmentName")
                                            })

                                        .ToList();


            result.OPEXCOGSExpenseJournalChart = new List<OPEXCOGSExpenseJournalChartBM>()
            {
                listDataResult.GetOpexChartData(x => x.AccountType == "OPEX", date),
                listDataResult.GetCogsChartData(x => x.AccountType == "COGS", date)
        };

            var payrollDatatableToList = GetEmployeePayrollDetailsTemp(resultData.Tables[1], DateTime.Parse(endDate));
            result.OPEXCOGSExpenseJournalChart.AddRange(MappingPayrollTemp(payrollDatatableToList, DateTime.Parse(endDate)));
            return result;

        }

        public List<MapEmployeePayrollData> GetEmployeePayrollDetails(DataTable dt)
        {


            var data = dt.AsEnumerable()
                    .Select(x => new MapEmployeePayrollData
                    {
                        Date = x.Field<DateTime>("DateAdded"),
                        Period = x.Field<string>("Period"),
                        CompanyId = x.Field<string>("CompanyId"),
                        CompanyName = x.Field<string>("CompanyName"),
                        LName = x.Field<string>("LName"),
                        FName = x.Field<string>("FName"),
                        EmployeeId = x.Field<string>("EmpId"),
                        Level = x.Field<string>("Level"),
                        Position = x.Field<string>("Position"),
                        Regular = x.Field<decimal?>("Regular") ?? 0,
                        Amount = x.Field<decimal?>("Amount") ?? 0,
                        OverTime = x.Field<decimal?>("Overtime") ?? 0,
                        EarningsTotal = x.Field<decimal?>("EarningsTotal") ?? 0,
                        DeductionTotal = x.Field<decimal?>("DeductionTotal") ?? 0,
                        LiabilityTotal = x.Field<decimal?>("LiabilityTotal") ?? 0,
                        DepartmentCode = x.Field<int>("DepartmentCode"),
                        DepartmentName = x.Field<string>("DepartmentName")
                    }).ToList();
            return data;

        }


        public List<MapEmployeePayrollData> GetEmployeePayrollDetailsCurrentPrior(DataTable dt)
        {


            var data = dt.AsEnumerable()
                    .Select(x => new MapEmployeePayrollData
                    {
                        Date = x.Field<DateTime>("DateAdded"),
                        Period = x.Field<string>("Period"),
                       // CompanyId = x.Field<string>("CompanyId"),
                        //CompanyName = x.Field<string>("CompanyName"),
                       // LName = x.Field<string>("LName"),
                        //FName = x.Field<string>("FName"),
                        EmployeeId = x.Field<string>("EmpId"),
                        //Level = x.Field<string>("Level"),
                       // Position = x.Field<string>("Position"),
                        //Regular = x.Field<decimal?>("Regular") ?? 0,
                        Amount = x.Field<decimal?>("Amount") ?? 0,
                        //OverTime = x.Field<decimal?>("Overtime") ?? 0,
                        //EarningsTotal = x.Field<decimal?>("EarningsTotal") ?? 0,
                       // //DeductionTotal = x.Field<decimal?>("DeductionTotal") ?? 0,
                       // LiabilityTotal = x.Field<decimal?>("LiabilityTotal") ?? 0,
                       // DepartmentCode = x.Field<int>("DepartmentCode"),
                        DepartmentName = x.Field<string>("DepartmentName")
                    }).ToList();
            return data;

        }
        public List<MapEmployeePayrollData> GetEmployeePayrollDetailsTemp(DataTable dt, DateTime date)
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
                        Regular = x.Field<decimal?>("Regular") ?? 0,
                        Amount = x.Field<decimal?>("Amount") ?? 0,
                        OverTime = x.Field<decimal?>("Overtime") ?? 0,
                        EarningsTotal = x.Field<decimal?>("EarningsTotal") ?? 0,
                        DeductionTotal = x.Field<decimal?>("DeductionTotal") ?? 0,
                        LiabilityTotal = x.Field<decimal?>("LiabilityTotal") ?? 0,
                        DepartmentCode = x.Field<int>("DepartmentCode"),
                        DepartmentName = x.Field<string>("DepartmentName")
                    }).ToList();
            return data;

        }

        public List<OPEXCOGSExpenseJournalChartBM> MappingPayroll(List<MapEmployeePayrollData> data, GlobalFilter filters)
        {
            var xx = data.Where(c => c.Period == "historical").Sum(s => s.Amount).ToStripDecimal();
            List<OPEXCOGSExpenseJournalChartBM> result = new List<OPEXCOGSExpenseJournalChartBM>();
            var regular = new OPEXCOGSExpenseJournalChartBM
            {
                Category = "Regular",

                Color1 = ChartColorBM.ExpenseCurrent,
                Color2 = ChartColorBM.ExpensePrevious,
                Color3 = ChartColorBM.ExpenseCurrent,

                Column1 = filters.Periods.Current.Label,
                Column2 = filters.Periods.Historical.Label,
                Column3 = filters.Periods.Prior.Label,

                Period1 = filters.Periods.Current.End,
                Period2 = filters.Periods.Historical.End,
                Period3 = filters.Periods.Prior.End,

                Val1 = data.Where(c => c.Period == "current").Sum(s => s.Amount).ToStripDecimal(),
                Val2 = data.Where(c => c.Period == "historical").Sum(s => s.Amount).ToStripDecimal(),
                Val3 = data.Where(c => c.Period == "prior").Sum(s => s.Amount).ToStripDecimal(),

                SubData = data.Select(j => new OPEXCOGSExpenseJournalTopBottomBM
                {
                    Top = data.GroupBy(y => y.DepartmentCode)
                               .OrderByDescending(z => z.Where(p => p.Period == "current").Sum(p => p.Amount))
                                                                         .Take(10)
                                                                         .Select(y => new OPEXCOGSExpenseJournalChartBM
                                                                         {
                                                                             Category = y.First().DepartmentName.ToString().TrimEnd(),

                                                                             Column1 = filters.Periods.Current.Label,
                                                                             Column2 = filters.Periods.Historical.Label,
                                                                             Column3 = filters.Periods.Prior.Label,

                                                                             Period1 = filters.Periods.Current.End,
                                                                             Period2 = filters.Periods.Historical.End,
                                                                             Period3 = filters.Periods.Prior.End,

                                                                             Val1 = y.Where(c => c.Period == "current").Sum(s => s.Amount).ToStripDecimal(),
                                                                             Val2 = y.Where(c => c.Period == "historical").Sum(s => s.Amount).ToStripDecimal(),
                                                                             Val3 = y.Where(c => c.Period == "prior").Sum(s => s.Amount).ToStripDecimal(),

                                                                             Color1 = ChartColorBM.ExpenseCurrent,
                                                                             Color2 = ChartColorBM.ExpensePrevious,
                                                                             Color3 = ChartColorBM.ExpenseCurrent,


                                                                             SubData = null

                                                                         }).ToList(),
                    Bottom = data.GroupBy(y => y.DepartmentCode)
                                                 .OrderBy(z => z.Where(p => p.Period == "current").Sum(p => p.Amount))
                                                                     .Take(10)
                                                                     .Select(y => new OPEXCOGSExpenseJournalChartBM
                                                                     {
                                                                         Category = y.First().DepartmentName.ToString().TrimEnd(),
                                                                         Column1 = filters.Periods.Current.Label,
                                                                         Column2 = filters.Periods.Historical.Label,
                                                                         Column3 = filters.Periods.Prior.Label,

                                                                         Period1 = filters.Periods.Current.End,
                                                                         Period2 = filters.Periods.Historical.End,
                                                                         Period3 = filters.Periods.Prior.End,

                                                                         Val1 = y.Where(c => c.Period == "current").Sum(s => s.Amount).ToStripDecimal(),
                                                                         Val2 = y.Where(c => c.Period == "historical").Sum(s => s.Amount).ToStripDecimal(),
                                                                         Val3 = y.Where(c => c.Period == "prior").Sum(s => s.Amount).ToStripDecimal(),

                                                                         Color1 = ChartColorBM.ExpenseCurrent,
                                                                         Color2 = ChartColorBM.ExpensePrevious,
                                                                         Color3 = ChartColorBM.ExpenseCurrent,

                                                                         SubData = null

                                                                     }).ToList(),
                }).FirstOrDefault()
            };

            var overTime = new OPEXCOGSExpenseJournalChartBM
            {
                Category = "OverTime",

                Color1 = ChartColorBM.ExpenseCurrent,
                Color2 = ChartColorBM.ExpensePrevious,
                Color3 = ChartColorBM.ExpenseCurrent,

                Column1 = filters.Periods.Current.Label,
                Column2 = filters.Periods.Historical.Label,
                Column3 = filters.Periods.Prior.Label,

                Period1 = filters.Periods.Current.End,
                Period2 = filters.Periods.Historical.End,
                Period3 = filters.Periods.Prior.End,

                Val1 = data.Where(c => c.Period == "current").Sum(s => s.OverTime).ToStripDecimal(),
                Val2 = data.Where(c => c.Period == "historical").Sum(s => s.OverTime).ToStripDecimal(),
                Val3 = data.Where(c => c.Period == "prior").Sum(s => s.OverTime).ToStripDecimal(),

                SubData = data.Select(j => new OPEXCOGSExpenseJournalTopBottomBM
                {
                    Top = data.GroupBy(y => y.DepartmentCode)
                                .OrderByDescending(z => z.Where(p => p.Period == "current").Sum(p => p.OverTime))
                                                                          .Take(10)
                                                                          .Select(y => new OPEXCOGSExpenseJournalChartBM
                                                                          {
                                                                              Category = y.First().DepartmentName.ToString().TrimEnd(),

                                                                              Column1 = filters.Periods.Current.Label,
                                                                              Column2 = filters.Periods.Historical.Label,
                                                                              Column3 = filters.Periods.Prior.Label,

                                                                              Period1 = filters.Periods.Current.End,
                                                                              Period2 = filters.Periods.Historical.End,
                                                                              Period3 = filters.Periods.Prior.End,

                                                                              Val1 = y.Where(c => c.Period == "current").Sum(s => s.OverTime).ToStripDecimal(),
                                                                              Val2 = y.Where(c => c.Period == "historical").Sum(s => s.OverTime).ToStripDecimal(),
                                                                              Val3 = y.Where(c => c.Period == "prior").Sum(s => s.OverTime).ToStripDecimal(),

                                                                              Color1 = ChartColorBM.ExpenseCurrent,
                                                                              Color2 = ChartColorBM.ExpensePrevious,
                                                                              Color3 = ChartColorBM.ExpenseCurrent,


                                                                              SubData = null

                                                                          }).ToList(),
                    Bottom = data.GroupBy(y => y.DepartmentCode)
                                                  .OrderBy(z => z.Where(p => p.Period == "current").Sum(p => p.OverTime))
                                                                      .Take(10)
                                                                      .Select(y => new OPEXCOGSExpenseJournalChartBM
                                                                      {
                                                                          Category = y.First().DepartmentName.ToString().TrimEnd(),
                                                                          Column1 = filters.Periods.Current.Label,
                                                                          Column2 = filters.Periods.Historical.Label,
                                                                          Column3 = filters.Periods.Prior.Label,

                                                                          Period1 = filters.Periods.Current.End,
                                                                          Period2 = filters.Periods.Historical.End,
                                                                          Period3 = filters.Periods.Prior.End,

                                                                          Val1 = y.Where(c => c.Period == "current").Sum(s => s.OverTime).ToStripDecimal(),
                                                                          Val2 = y.Where(c => c.Period == "historical").Sum(s => s.OverTime).ToStripDecimal(),
                                                                          Val3 = y.Where(c => c.Period == "prior").Sum(s => s.OverTime).ToStripDecimal(),

                                                                          Color1 = ChartColorBM.ExpenseCurrent,
                                                                          Color2 = ChartColorBM.ExpensePrevious,
                                                                          Color3 = ChartColorBM.ExpenseCurrent,

                                                                          SubData = null

                                                                      }).ToList(),
                }).FirstOrDefault()
            };
            result.Add(regular);
            result.Add(overTime);
            return result;
        }

        public List<OPEXCOGSExpenseJournalChartBM> MappingPayrollTemp(List<MapEmployeePayrollData> data, DateTime date)
        {
            List<OPEXCOGSExpenseJournalChartBM> result = new List<OPEXCOGSExpenseJournalChartBM>();
            var regular = new OPEXCOGSExpenseJournalChartBM
            {
                Category = "Regular",

                Color1 = ChartColorBM.ExpenseCurrent,
                Color2 = ChartColorBM.ExpensePrevious,
                Color3 = ChartColorBM.ExpenseCurrent,

                Column1 = date.ToString("MMM yy"),
                Column2 = date.AddYears(-1).ToString("MMM yy"),
                Column3 = date.AddMonths(-1).ToString("MMM yy"),

                Period1 = date,
                Period2 = date.AddYears(-1),
                Period3 = date.AddMonths(-1),

                Val1 = data.Where(c => c.Date.Year == date.Year && c.Date.Month == date.Month).Sum(s => s.Amount),
                Val2 = data.Where(c => c.Date.Year == date.AddYears(-1).Year && c.Date.Month == date.Month).Sum(s => s.Amount),
                Val3 = data.Where(c => c.Date.Month == date.AddMonths(-1).Month).Sum(s => s.Amount),

                SubData = data.Select(j => new OPEXCOGSExpenseJournalTopBottomBM
                {
                    Top = data.GroupBy(y => y.DepartmentCode)
                               .OrderByDescending(z => z.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month).Sum(p => p.Amount))
                                                                         .Take(10)
                                                                         .Select(y => new OPEXCOGSExpenseJournalChartBM
                                                                         {
                                                                             Category = y.First().DepartmentName.ToString().TrimEnd(),

                                                                             Column1 = date.ToString("MMM yy"),
                                                                             Column2 = date.AddYears(-1).ToString("MMM yy"),
                                                                             Column3 = date.AddMonths(-1).ToString("MMM yy"),

                                                                             Period1 = date,
                                                                             Period2 = date.AddYears(-1),
                                                                             Period3 = date.AddMonths(-1),

                                                                             Val1 = y.Where(c => c.Date.Year == date.Year && c.Date.Month == date.Month).Sum(s => s.Amount),
                                                                             Val2 = y.Where(c => c.Date.Year == date.AddYears(-1).Year && c.Date.Month == date.Month).Sum(s => s.Amount),
                                                                             Val3 = y.Where(c => c.Date.Month == date.AddMonths(-1).Month).Sum(s => s.Amount),

                                                                             Color1 = ChartColorBM.ExpenseCurrent,
                                                                             Color2 = ChartColorBM.ExpensePrevious,
                                                                             Color3 = ChartColorBM.ExpenseCurrent,


                                                                             SubData = null

                                                                         }).ToList(),
                    Bottom = data.GroupBy(y => y.DepartmentCode)
                                                 .OrderBy(z => z.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month).Sum(p => p.Amount))
                                                                     .Take(10)
                                                                     .Select(y => new OPEXCOGSExpenseJournalChartBM
                                                                     {
                                                                         Category = y.First().DepartmentName.ToString().TrimEnd(),
                                                                         Column1 = date.ToString("MMM yy"),
                                                                         Column2 = date.AddYears(-1).ToString("MMM yy"),
                                                                         Column3 = date.AddMonths(-1).ToString("MMM yy"),

                                                                         Period1 = date,
                                                                         Period2 = date.AddYears(-1),
                                                                         Period3 = date.AddMonths(-1),

                                                                         Val1 = y.Where(c => c.Date.Year == date.Year && c.Date.Month == date.Month).Sum(s => s.Amount),
                                                                         Val2 = y.Where(c => c.Date.Year == date.AddYears(-1).Year && c.Date.Month == date.Month).Sum(s => s.Amount),
                                                                         Val3 = y.Where(c => c.Date.Month == date.AddMonths(-1).Month).Sum(s => s.Amount),

                                                                         Color1 = ChartColorBM.ExpenseCurrent,
                                                                         Color2 = ChartColorBM.ExpensePrevious,
                                                                         Color3 = ChartColorBM.ExpenseCurrent,

                                                                         SubData = null

                                                                     }).ToList(),
                }).FirstOrDefault()
            };

            var overTime = new OPEXCOGSExpenseJournalChartBM
            {
                Category = "OverTime",

                Color1 = ChartColorBM.ExpenseCurrent,
                Color2 = ChartColorBM.ExpensePrevious,
                Color3 = ChartColorBM.ExpenseCurrent,

                Column1 = date.ToString("MMM yy"),
                Column2 = date.AddYears(-1).ToString("MMM yy"),
                Column3 = date.AddMonths(-1).ToString("MMM yy"),

                Period1 = date,
                Period2 = date.AddYears(-1),
                Period3 = date.AddMonths(-1),

                Val1 = data.Where(c => c.Date.Year == date.Year && c.Date.Month == date.Month).Sum(s => s.OverTime),
                Val2 = data.Where(c => c.Date.Year == date.AddYears(-1).Year && c.Date.Month == date.Month).Sum(s => s.OverTime),
                Val3 = data.Where(c => c.Date.Month == date.AddMonths(-1).Month).Sum(s => s.OverTime),

                SubData = data.Select(j => new OPEXCOGSExpenseJournalTopBottomBM
                {
                    Top = data.GroupBy(y => y.DepartmentCode)
                                .OrderByDescending(z => z.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month).Sum(p => p.OverTime))
                                                                          .Take(10)
                                                                          .Select(y => new OPEXCOGSExpenseJournalChartBM
                                                                          {
                                                                              Category = y.First().DepartmentName.ToString().TrimEnd(),

                                                                              Column1 = date.ToString("MMM yy"),
                                                                              Column2 = date.AddYears(-1).ToString("MMM yy"),
                                                                              Column3 = date.AddMonths(-1).ToString("MMM yy"),

                                                                              Period1 = date,
                                                                              Period2 = date.AddYears(-1),
                                                                              Period3 = date.AddMonths(-1),

                                                                              Val1 = y.Where(c => c.Date.Year == date.Year && c.Date.Month == date.Month).Sum(s => s.OverTime),
                                                                              Val2 = y.Where(c => c.Date.Year == date.AddYears(-1).Year && c.Date.Month == date.Month).Sum(s => s.OverTime),
                                                                              Val3 = y.Where(c => c.Date.Month == date.AddMonths(-1).Month).Sum(s => s.OverTime),

                                                                              Color1 = ChartColorBM.ExpenseCurrent,
                                                                              Color2 = ChartColorBM.ExpensePrevious,
                                                                              Color3 = ChartColorBM.ExpenseCurrent,


                                                                              SubData = null

                                                                          }).ToList(),
                    Bottom = data.GroupBy(y => y.DepartmentCode)
                                                  .OrderBy(z => z.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month).Sum(p => p.OverTime))
                                                                      .Take(10)
                                                                      .Select(y => new OPEXCOGSExpenseJournalChartBM
                                                                      {
                                                                          Category = y.First().DepartmentName.ToString().TrimEnd(),
                                                                          Column1 = date.ToString("MMM yy"),
                                                                          Column2 = date.AddYears(-1).ToString("MMM yy"),
                                                                          Column3 = date.AddMonths(-1).ToString("MMM yy"),

                                                                          Period1 = date,
                                                                          Period2 = date.AddYears(-1),
                                                                          Period3 = date.AddMonths(-1),

                                                                          Val1 = y.Where(c => c.Date.Year == date.Year && c.Date.Month == date.Month).Sum(s => s.OverTime),
                                                                          Val2 = y.Where(c => c.Date.Year == date.AddYears(-1).Year && c.Date.Month == date.Month).Sum(s => s.OverTime),
                                                                          Val3 = y.Where(c => c.Date.Month == date.AddMonths(-1).Month).Sum(s => s.OverTime),

                                                                          Color1 = ChartColorBM.ExpenseCurrent,
                                                                          Color2 = ChartColorBM.ExpensePrevious,
                                                                          Color3 = ChartColorBM.ExpenseCurrent,

                                                                          SubData = null

                                                                      }).ToList(),
                }).FirstOrDefault()
            };
            result.Add(regular);
            result.Add(overTime);
            return result;
        }

        public List<ARJournalReportBM> GetARJournalReport(JournalFilterBM filterData)
        {
            DateTime sDate;
            DateTime eDate;
            #region Validation
            if (!DateTime.TryParse(filterData.StartDate, out sDate))
            {
                throw new Exception("Start Date Invalid.");
            }
            if (!DateTime.TryParse(filterData.EndDate, out eDate))
            {
                throw new Exception("End Date Invalid.");
            }
            #endregion
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@start", sDate));
            parameterList.Add(new SqlParameter("@end", DateTime.Parse(eDate.ToString("yyyy/MM/dd 23:59:59"))));
            parameterList.Add(new SqlParameter("@acc_start", filterData.AccountNumberStart));
            parameterList.Add(new SqlParameter("@acc_end", filterData.AccountNumberEnd));
            parameterList.Add(new SqlParameter("@session_start", filterData.StartSession));
            parameterList.Add(new SqlParameter("@session_end", filterData.EndSession));
            parameterList.Add(new SqlParameter("@batch_start", filterData.StartBatch));
            parameterList.Add(new SqlParameter("@batch_end", filterData.EndBatch));
            parameterList.Add(new SqlParameter("@customer_number", filterData.CustomerNumber));
            parameterList.Add(new SqlParameter("@arType", filterData.ARType));

            var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetARJournalReport", parameterList.ToArray());

            //var query = SQLQueries
            //                .GetARTransactionReport(
            //                    filterData.AccountNumberStart
            //                    , filterData.AccountNumberEnd
            //                    , filterData.StartSession
            //                    , filterData.EndSession
            //                    , filterData.StartBatch
            //                    , filterData.EndBatch
            //                    , sDate.ToString("yyyy/MM/dd 00:00:00")
            //                    , eDate.ToString("yyyy/MM/dd 23:59:59")
            //                    , filterData.CustomerNumber
            //                    , filterData.ARType);

            //var dataSetResult = base.ReadToDataSet(query);
            var result = dataSetResult.Tables[0]
          .AsEnumerable()
                .Select(x => new ARJournalReportBM
                {
                    Amount = x.Field<decimal>("Amount").ToStripDecimal(),
                    Customer = string.IsNullOrEmpty(x.Field<string>("CustomerNumber")) ? string.Empty : x.Field<string>("CustomerNumber").Trim(),
                    GLAccount = x.Field<string>("GLAccount").Trim(),
                    AccountName = x.Field<string>("AccountName").Trim(),
                    GLBatch = x.Field<string>("GLBatch").Trim(),
                    TransactionNumber = x.Field<string>("TransactionNumber"),
                    ReferenceNumber = string.IsNullOrEmpty(x.Field<string>("ReferenceNumber")) ? string.Empty : x.Field<string>("ReferenceNumber").Trim(),
                    Session = x.Field<string>("ARSession").Trim(),
                    TransactionDate = x.Field<DateTime>("TransactionDate"),
                    //Date = x.Field<DateTime>(1).ToString("yyyy-MM-dd"),
                    ARType = string.IsNullOrEmpty(x.Field<string>("ARType")) ? string.Empty : x.Field<string>("ARType").Trim(),
                })
                .OrderByDescending(x=>x.Amount)
                .ToList();
            return result;
        }

        /// <summary>
        /// Get ICJournal Report
        /// </summary>
        /// <param name="filterData"></param>
        /// <returns></returns>
        public List<ICJournalReportBM> GetICJournalReport(JournalFilterBM filterData)
        {

            try
            {
                DateTime startDate;
                var rangeNumbersData = string.Join(",", Enumerable.Range(2, 1244).Select(x => x.ToString().PadLeft(6, '0')).ToList());

                if (!DateTime.TryParse(filterData.StartDate, out startDate))
                {
                    throw new Exception("Start date is invalid");
                }

                var query = SQLQueries.GetInventoryJournalReportQuery(filterData.StartSession, filterData.EndSession, rangeNumbersData, filterData.StartDate, filterData.EndDate);
                var dataSetResult = base.ReadToDataSet(query);

                var resultData = dataSetResult.Tables[0].AsEnumerable().Select(x => new ICJournalReportBM
                {
                    GLAccount = x.Field<string>("Account"),
                    AccountName = x.Field<string>("Description"),
                    TotalAmount = x.Field<decimal>("Amount")
                })
                .OrderByDescending(p=>p.TotalAmount)
                .ToList();
                return resultData;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching data from database-" + ex.Message);
            }

        }

        public List<OPEXCOGSExpenseJournalReport> GetCOGSExpenseReport(GlobalFilterModel filter)
        {
            string AccountNumberStart;
            string AccountNumberEnd;

            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filter.FilterId).FirstOrDefault();


            DateTime CurrentEndDate = targetFilter.Periods.Current.End;
            DateTime HistoricalEndDate = targetFilter.Periods.Historical.End;
            DateTime PriorEndDate = targetFilter.Periods.Prior.End;

            DateTime startDate, endDate, prevStartDate, prevEndDate;
            if (filter.Period == (int)PeriodEnum.Historical)
            {
                var filterListsHistorical = GlobaldataProvider.GetFilterWithPeriodsByDate(HistoricalEndDate);
                var targetFilterHistorical = filterListsHistorical.Where(d => d.Id == filter.FilterId).FirstOrDefault();
                startDate = targetFilterHistorical.Periods.Current.Start;
                endDate = targetFilterHistorical.Periods.Current.End;
                prevStartDate = targetFilterHistorical.Periods.Historical.Start;
                prevEndDate = targetFilterHistorical.Periods.Historical.End;
            }
            else if (filter.Period == (int)PeriodEnum.Prior)
            {
                var filterListsPrior = GlobaldataProvider.GetFilterWithPeriodsByDate(PriorEndDate);
                var targetFilterPrior = filterListsPrior.Where(d => d.Id == filter.FilterId).FirstOrDefault();
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


            //OPEX
            if (filter.AccountType == Constants.OpexGLAccounTtype)
            {
                AccountNumberStart = Constants.OpexAccountStart;
                AccountNumberEnd = Constants.OpexAccountEnd;
            }
            //COGS
            else if (filter.AccountType == Constants.COGSGLAccounTtype)
            {
                AccountNumberStart = Constants.CogsAccountStart;
                AccountNumberEnd = Constants.CogsAccountEnd;
            }
            else
            {
                throw new Exception("Invalid parameter");
            }

            string currenStartDate = startDate.ToString("yyyy/MM/dd 00:00:00");
            string currentEndDate = endDate.ToString("yyyy/MM/dd 23:59:59");
            string previousStartDate = prevStartDate.ToString("yyyy/MM/dd 00:00:00");
            string previousEndDate = prevEndDate.ToString("yyyy/MM/dd 23:59:59");
            //string columnStartDate = string.Empty;
            //string columnPrevDate = string.Empty;

            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@current_date_start", currenStartDate));
            parameterList.Add(new SqlParameter("@current_date", currentEndDate));
            parameterList.Add(new SqlParameter("@previous_date_start", previousStartDate));
            parameterList.Add(new SqlParameter("@previous_date", previousEndDate));
            parameterList.Add(new SqlParameter("@acc_start", AccountNumberStart));
            parameterList.Add(new SqlParameter("@acc_end", AccountNumberEnd));
            parameterList.Add(new SqlParameter("@session_start", Constants.StartSession));
            parameterList.Add(new SqlParameter("@session_end", Constants.EndSession));
            parameterList.Add(new SqlParameter("@accno", filter.AccountNumber));

            var resultData = base.ReadToDataSetViaProcedure("BI_GetGetOPEXCOGSReport", parameterList.ToArray());

            //string Query = SQLQueries.GetOPEXCOGSReportQuery(
            //                                    currenStartDate
            //                                  , currentEndDate
            //                                  , previousStartDate
            //                                  , previousEndDate
            //                                  , AccountNumberStart
            //                                  , AccountNumberEnd
            //                                  , accountNumber);

            //var resultData = base.ReadToDataSet(Query);


            var listDataResult = resultData.Tables[0]
                                            .AsEnumerable()
                                            .Select(x =>
                                            new
                                            {
                                                InvoiceNumber = x.Field<string>("InvoiceNumber"),
                                                Date = x.Field<DateTime>("TransactionDate"),
                                                Amount = x.Field<decimal>("Amount")
                                            }).ToList();

            var result = listDataResult.Select(x => new OPEXCOGSExpenseJournalReport
            {
                InvoiceNumber = x.InvoiceNumber.TrimEnd(),
                Date = x.Date.ToString("MM-dd-yyyy"),
                Amount = Convert.ToDecimal(x.Amount)

            })
            .OrderByDescending(p=>p.Date)
            .ToList();

            //var listDataResult = resultData.Tables[0]
            //                            .AsEnumerable()
            //                            .Select(x =>
            //                                new
            //                                {
            //                                    AccountName = x.Field<string>("AccountName"),
            //                                    AccountNumber=x.Field<string>("GLAccount"),
            //                                    Amount = x.Field<decimal>("Amount"),
            //                                    //TransactionDate = x.Field<string>("TransactionDate"),
            //                                    CurrentPrevious = x.Field<string>("CurrentPrevious")
            //                                })

            //                            .ToList();

            //var result = listDataResult
            //            .GroupBy(x=>x.AccountNumber)
            //            .Select(x=>new OPEXCOGSExpenseJournalReport
            //            {
            //                 //AccountNumber=x.AccountNumber,
            //                 //AmountCurrent=x. Where(y=>y.CurrentPrevious=="Current").Sum(z=>z.Amount),
            //                 //AmountPrior = x.Where(y => y.CurrentPrevious == "Previous").Sum(z=>z.Amount),
            //                 //Percentage = Math.Round(x.Where(y => y.CurrentPrevious == "Current").Sum(z => z.Amount) - x.Where(y => y.CurrentPrevious == "Previous").Sum(z => z.Amount)
            //                 //           / (x.Where(y => y.CurrentPrevious == "Current").Sum(z => z.Amount) + x.Where(y => y.CurrentPrevious == "Previous").Sum(z => z.Amount))
            //                 //                   / 2 * 100, 2),

            //                 ////Percentage = x.Where(y => y.CurrentPrevious == "Current").Sum(z => z.Amount) - x.Where(y => y.CurrentPrevious == "Previous").Sum(z => z.Amount)
            //                 ////           / x.Where(y =>{ if(((y.CurrentPrevious == "Previous").Sum(z => z.Amount))>0) return Convert.ToDecimal((y.CurrentPrevious == "Previous").Sum(z => z.Amount)); else return 0;})

            //                AccountNumber = x.Key.ToString(),
            //                AmountCurrent =x.Where(y=>y.CurrentPrevious=="Current").Select(z=>z.Amount).ToString(),
            //                AmountPrior = x.Where(y => y.CurrentPrevious == "Previous").Select(z => z.Amount).ToString(),
            //                Percentage = Math.Round(listDataResult.Where(y => y.CurrentPrevious == "Current").Sum(z => z.Amount) - listDataResult.Where(y => y.CurrentPrevious == "Previous").Sum(z => z.Amount)
            //                            / (listDataResult.Where(y => y.CurrentPrevious == "Current").Sum(z => z.Amount) + listDataResult.Where(y => y.CurrentPrevious == "Previous").Sum(z => z.Amount))
            //                                    / 2 * 100, 2),


            //            }).ToList();

            return result;
        }

        public List<OPEXCOGSExpenseJournalReport> GetCOGSExpenseReportTemp(string startDate, string endDate, string AccountType, string accountNumber)
        {
            DateTime SDate;

            string AccountNumberStart;
            string AccountNumberEnd;

            #region Validation
            if (!DateTime.TryParse(startDate, out SDate))
            {
                throw new Exception("Start Date Invalid.");
            }
            //if (!DateTime.TryParse(endDate, out EDate))
            //{
            //    throw new Exception("End Date Invalid.");
            //}
            DateTime EDate = SDate.AddDays(DateTime.Now.Day - 1);
            if (EDate.Month != SDate.Month)
            {
                EDate = EDate.AddMonths(-1);
            }

            #endregion


            //OPEX
            if (AccountType == Constants.OpexGLAccounTtype)
            {
                AccountNumberStart = Constants.OpexAccountStart;
                AccountNumberEnd = Constants.OpexAccountEnd;
            }
            //COGS
            else if (AccountType == Constants.COGSGLAccounTtype)
            {
                AccountNumberStart = Constants.CogsAccountStart;
                AccountNumberEnd = Constants.CogsAccountEnd;
            }
            else
            {
                throw new Exception("Invalid parameter");
            }

            string currenStartDate = SDate.ToString("yyyy/MM/dd 00:00:00");
            string currentEndDate = EDate.ToString("yyyy/MM/dd 23:59:59");
            string previousStartDate = SDate.AddYears(-1).ToString("yyyy/MM/dd 00:00:00");
            string previousEndDate = EDate.AddYears(-1).ToString("yyyy/MM/dd 23:59:59");
            //string columnStartDate = string.Empty;
            //string columnPrevDate = string.Empty;

            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@current_date_start", currenStartDate));
            parameterList.Add(new SqlParameter("@current_date", currentEndDate));
            parameterList.Add(new SqlParameter("@previous_date_start", previousStartDate));
            parameterList.Add(new SqlParameter("@previous_date", previousEndDate));
            parameterList.Add(new SqlParameter("@acc_start", AccountNumberStart));
            parameterList.Add(new SqlParameter("@acc_end", AccountNumberEnd));
            parameterList.Add(new SqlParameter("@session_start", Constants.StartSession));
            parameterList.Add(new SqlParameter("@session_end", Constants.EndSession));
            parameterList.Add(new SqlParameter("@accno", accountNumber));

            var resultData = base.ReadToDataSetViaProcedure("BI_GetGetOPEXCOGSReport", parameterList.ToArray());

            //string Query = SQLQueries.GetOPEXCOGSReportQuery(
            //                                    currenStartDate
            //                                  , currentEndDate
            //                                  , previousStartDate
            //                                  , previousEndDate
            //                                  , AccountNumberStart
            //                                  , AccountNumberEnd
            //                                  , accountNumber);

            //var resultData = base.ReadToDataSet(Query);


            var listDataResult = resultData.Tables[0]
                                            .AsEnumerable()
                                            .Select(x =>
                                            new
                                            {
                                                InvoiceNumber = x.Field<string>("InvoiceNumber"),
                                                Date = x.Field<DateTime>("TransactionDate"),
                                                Amount = x.Field<decimal>("Amount")
                                            }).ToList();

            var result = listDataResult.Select(x => new OPEXCOGSExpenseJournalReport
            {
                InvoiceNumber = x.InvoiceNumber.TrimEnd(),
                Date = x.Date.ToString("MM-dd-yyyy"),
                Amount = Convert.ToDecimal(x.Amount)

            }).ToList();

            //var listDataResult = resultData.Tables[0]
            //                            .AsEnumerable()
            //                            .Select(x =>
            //                                new
            //                                {
            //                                    AccountName = x.Field<string>("AccountName"),
            //                                    AccountNumber=x.Field<string>("GLAccount"),
            //                                    Amount = x.Field<decimal>("Amount"),
            //                                    //TransactionDate = x.Field<string>("TransactionDate"),
            //                                    CurrentPrevious = x.Field<string>("CurrentPrevious")
            //                                })

            //                            .ToList();

            //var result = listDataResult
            //            .GroupBy(x=>x.AccountNumber)
            //            .Select(x=>new OPEXCOGSExpenseJournalReport
            //            {
            //                 //AccountNumber=x.AccountNumber,
            //                 //AmountCurrent=x. Where(y=>y.CurrentPrevious=="Current").Sum(z=>z.Amount),
            //                 //AmountPrior = x.Where(y => y.CurrentPrevious == "Previous").Sum(z=>z.Amount),
            //                 //Percentage = Math.Round(x.Where(y => y.CurrentPrevious == "Current").Sum(z => z.Amount) - x.Where(y => y.CurrentPrevious == "Previous").Sum(z => z.Amount)
            //                 //           / (x.Where(y => y.CurrentPrevious == "Current").Sum(z => z.Amount) + x.Where(y => y.CurrentPrevious == "Previous").Sum(z => z.Amount))
            //                 //                   / 2 * 100, 2),

            //                 ////Percentage = x.Where(y => y.CurrentPrevious == "Current").Sum(z => z.Amount) - x.Where(y => y.CurrentPrevious == "Previous").Sum(z => z.Amount)
            //                 ////           / x.Where(y =>{ if(((y.CurrentPrevious == "Previous").Sum(z => z.Amount))>0) return Convert.ToDecimal((y.CurrentPrevious == "Previous").Sum(z => z.Amount)); else return 0;})

            //                AccountNumber = x.Key.ToString(),
            //                AmountCurrent =x.Where(y=>y.CurrentPrevious=="Current").Select(z=>z.Amount).ToString(),
            //                AmountPrior = x.Where(y => y.CurrentPrevious == "Previous").Select(z => z.Amount).ToString(),
            //                Percentage = Math.Round(listDataResult.Where(y => y.CurrentPrevious == "Current").Sum(z => z.Amount) - listDataResult.Where(y => y.CurrentPrevious == "Previous").Sum(z => z.Amount)
            //                            / (listDataResult.Where(y => y.CurrentPrevious == "Current").Sum(z => z.Amount) + listDataResult.Where(y => y.CurrentPrevious == "Previous").Sum(z => z.Amount))
            //                                    / 2 * 100, 2),


            //            }).ToList();

            return result;
        }


    }
}
