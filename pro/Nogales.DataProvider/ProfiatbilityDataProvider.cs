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
using System.Globalization;
using Nogales.DataProvider.ENUM;
//using System.Web.Script.Serialization;
namespace Nogales.DataProvider
{
    public class ProfiatbilityDataProvider : DataAccessADO
    {
        public async Task<List<ProfitChartBM>> GetProfitability(GlobalFilter filter)
        {
            try
            {



                //string sqlString = MDXCubeQueries.GetProfitabilityQuery(startDateFormatted, endDateFormatted);



                var dataTable = new DataSet();
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startDate", filter.Periods.Current.Start.ToShortDateString()));
                parameterList.Add(new SqlParameter("@endDate", filter.Periods.Current.End.ToShortDateString()));
                dataTable = base.ReadToDataSetViaProcedure("BI_GetProfitability", parameterList.ToArray());

                //var dataTableResult =await Task.Run(()=> base.GetDataTable(sqlString));

                var result = dataTable.Tables[0]
                          .AsEnumerable()
                          .Select(x => new ProfitChartBM
                          {
                              //Amount = (x.Field<decimal?>("Profiability")??0).ToRoundTwoDecimalDigits().ToRoundTwoDigits(),
                              Amount = x.Field<decimal?>("Profiability").Value.ToRoundTwoDecimalDigits(),
                              Date = x.Field<DateTime>("InvoDate").ToString("yyyy-MM-dd").Substring(0, 10)
                          }).ToList();
                return result;

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database" + e.Message);
            }


        }

        public async Task<List<ProfitChartBM>> GetProfitabilityTemp(string start, string end)
        {
            try
            {


                var startDateObj = DateTime.Parse(start);
                var endDateObj = DateTime.Parse(end);
                var numberOfDays = (endDateObj.Date - startDateObj.Date).TotalDays;


                var startDateFormatted = startDateObj.Date.ToString("yyyy");
                var endDateFormatted = endDateObj.Date.ToString("yyyy");


                //string sqlString = MDXCubeQueries.GetProfitabilityQuery(startDateFormatted, endDateFormatted);



                var dataTable = new DataSet();
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startDate", start));
                parameterList.Add(new SqlParameter("@endDate", end));
                dataTable = base.ReadToDataSetViaProcedure("BI_GetProfitability", parameterList.ToArray());

                //var dataTableResult =await Task.Run(()=> base.GetDataTable(sqlString));

                var result = dataTable.Tables[0]
                          .AsEnumerable()
                          .Select(x => new ProfitChartBM
                          {
                              //Amount = (x.Field<decimal?>("Profiability")??0).ToRoundTwoDecimalDigits().ToRoundTwoDigits(),
                              Amount = x.Field<decimal?>("Profiability").Value.ToRoundTwoDecimalDigits(),
                              Date = x.Field<DateTime>("InvoDate").ToString("yyyy-MM-dd").Substring(0, 10)
                          }).ToList();
                return result;

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database" + e.Message);
            }


        }

     

        public async Task<GenericMarginChartData> GetMargin(GlobalFilter filters, double filterCustomer, double filteritem)
        {
            try
            {

                GenericMarginChartData result = new GenericMarginChartData();
                var dataTable = new DataSet();
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filters.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filters.Periods.Current.End));
                parameterList.Add(new SqlParameter("@historicalStart", filters.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filters.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@priorStart", filters.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filters.Periods.Prior.End));
                parameterList.Add(new SqlParameter("@filterCustomer", filterCustomer));
                parameterList.Add(new SqlParameter("@filteritem", filteritem));
                dataTable = base.ReadToDataSetViaProcedure("BI_GetProfitabilityData", parameterList.ToArray());

                var data = dataTable.Tables[0]
                          .AsEnumerable()
                          .Select(x => new MarginChartBM
                          {
                              InvNo = x.Field<string>("invno"),
                              BinCost = x.Field<decimal>("bincost").ToRoundTwoDecimalDigits().ToStripDecimal(),
                              Item = x.Field<string>("descrip"),
                              ItemCode = x.Field<string>("itemjoin"),
                              Period = x.Field<string>("Period"),
                              QuantityShipped = x.Field<int>("qtyshp"),
                              Revenue = x.Field<decimal>("extprice").ToRoundTwoDecimalDigits().ToStripDecimal(),
                              CustCode = x.Field<string>("custno"),
                              Company = x.Field<string>("company"),
                              Date = x.Field<DateTime>("invdte"),

                          }).ToList();

                var customerDataByFilter = dataTable.Tables[1]
                       .AsEnumerable()
                       .Select(x => new MarginChartBM
                       {
                           InvNo = x.Field<string>("invno"),
                           BinCost = x.Field<decimal>("bincost").ToRoundTwoDecimalDigits().ToStripDecimal(),
                           Item = x.Field<string>("descrip"),
                           ItemCode = x.Field<string>("itemjoin"),
                           Period = x.Field<string>("Period"),
                           QuantityShipped = x.Field<int>("qtyshp"),
                           Revenue = x.Field<decimal>("extprice").ToRoundTwoDecimalDigits().ToStripDecimal(),
                           CustCode = x.Field<string>("custno"),
                           Company = x.Field<string>("company"),
                           Date = x.Field<DateTime>("invdte"),

                       }).ToList();
                var itemDataByFilter = dataTable.Tables[2]
                     .AsEnumerable()
                     .Select(x => new MarginChartBM
                     {
                         InvNo = x.Field<string>("invno"),
                         BinCost = x.Field<decimal>("bincost").ToRoundTwoDecimalDigits().ToStripDecimal(),
                         Item = x.Field<string>("descrip"),
                         ItemCode = x.Field<string>("itemjoin"),
                         Period = x.Field<string>("Period"),
                         QuantityShipped = x.Field<int>("qtyshp"),
                         Revenue = x.Field<decimal>("extprice").ToRoundTwoDecimalDigits().ToStripDecimal(),
                         CustCode = x.Field<string>("custno"),
                         Company = x.Field<string>("company"),
                         Date = x.Field<DateTime>("invdte"),

                     }).ToList();

                data = data.Where(x => !x.Company.Contains("DONATION") && !x.Company.Contains("DUMP")).ToList();
                customerDataByFilter = customerDataByFilter.Where(x => !x.Company.Contains("DONATION") && !x.Company.Contains("DUMP")).ToList();
                itemDataByFilter = itemDataByFilter.Where(x => !x.Company.Contains("DONATION") && !x.Company.Contains("DUMP")).ToList();

                result.ProfitabiltyChart = new List<GenericProfitablityChart>() { GetCustomerMargin(customerDataByFilter, filters), GetItemMargin(itemDataByFilter, filters) };


                //customer filters
                var customerProfitabiltyPercentage = FindPercentageProfitabiltyFromDataWithDifference(customerDataByFilter, true, filters);

                //item filters
                var itemProfitabiltyPercentage = FindPercentageProfitabiltyFromDataWithDifference(itemDataByFilter, false, filters);

                result.Customer = new GenericTwoBarChartData()
                {
                    TopBottomChartByMonth = new GenericTopBottomTwoBarChartData()
                    {
                        Bottom = customerProfitabiltyPercentage.OrderBy(d => d.Tooltip).Take(10).ToList(),
                        Top = customerProfitabiltyPercentage.OrderByDescending(d => d.Tooltip).Take(10).ToList(),
                    }

                };
                result.Item = new GenericTwoBarChartData()
                {
                    TopBottomChartByMonth = new GenericTopBottomTwoBarChartData()
                    {
                        Bottom = itemProfitabiltyPercentage.OrderBy(d => d.Tooltip).Take(10).ToList(),
                        Top = itemProfitabiltyPercentage.OrderByDescending(d => d.Tooltip).Take(10).ToList(),
                    }
                };
                return result;

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database" + e.Message);
            }


        }

        public async Task<GenericMarginChartData> GetMarginTemp(DateTime date, double filterCustomer, double filteritem)
        {
            try
            {

                GenericMarginChartData result = new GenericMarginChartData();
                var dataTable = new DataSet();
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@current_date", date));
                parameterList.Add(new SqlParameter("@filterCustomer", filterCustomer));
                parameterList.Add(new SqlParameter("@filteritem", filteritem));
                dataTable = base.ReadToDataSetViaProcedure("BI_GetProfitabilityData", parameterList.ToArray());

                var data = dataTable.Tables[0]
                          .AsEnumerable()
                          .Select(x => new MarginChartBM
                          {
                              InvNo = x.Field<string>("invno"),
                              BinCost = x.Field<decimal>("bincost").ToRoundTwoDecimalDigits(),
                              Item = x.Field<string>("descrip"),
                              ItemCode = x.Field<string>("itemjoin"),
                              Period = x.Field<string>("Period"),
                              QuantityShipped = x.Field<int>("qtyshp"),
                              Revenue = x.Field<decimal>("extprice").ToRoundTwoDecimalDigits(),
                              CustCode = x.Field<string>("custno"),
                              Company = x.Field<string>("company"),
                              Date = x.Field<DateTime>("invdte"),

                          }).ToList();

                var customerDataByFilter = dataTable.Tables[1]
                       .AsEnumerable()
                       .Select(x => new MarginChartBM
                       {
                           InvNo = x.Field<string>("invno"),
                           BinCost = x.Field<decimal>("bincost").ToRoundTwoDecimalDigits(),
                           Item = x.Field<string>("descrip"),
                           ItemCode = x.Field<string>("itemjoin"),
                           Period = x.Field<string>("Period"),
                           QuantityShipped = x.Field<int>("qtyshp"),
                           Revenue = x.Field<decimal>("extprice").ToRoundTwoDecimalDigits(),
                           CustCode = x.Field<string>("custno"),
                           Company = x.Field<string>("company"),
                           Date = x.Field<DateTime>("invdte"),

                       }).ToList();
                var itemDataByFilter = dataTable.Tables[2]
                     .AsEnumerable()
                     .Select(x => new MarginChartBM
                     {
                         InvNo = x.Field<string>("invno"),
                         BinCost = x.Field<decimal>("bincost").ToRoundTwoDecimalDigits(),
                         Item = x.Field<string>("descrip"),
                         ItemCode = x.Field<string>("itemjoin"),
                         Period = x.Field<string>("Period"),
                         QuantityShipped = x.Field<int>("qtyshp"),
                         Revenue = x.Field<decimal>("extprice").ToRoundTwoDecimalDigits(),
                         CustCode = x.Field<string>("custno"),
                         Company = x.Field<string>("company"),
                         Date = x.Field<DateTime>("invdte"),

                     }).ToList();

                data = data.Where(x => !x.Company.Contains("DONATION") && !x.Company.Contains("DUMP")).ToList();
                customerDataByFilter = customerDataByFilter.Where(x => !x.Company.Contains("DONATION") && !x.Company.Contains("DUMP")).ToList();
                itemDataByFilter = itemDataByFilter.Where(x => !x.Company.Contains("DONATION") && !x.Company.Contains("DUMP")).ToList();

                result.ProfitabiltyChart = new List<GenericProfitablityChart>() { GetCustomerMarginTemp(data, date), GetItemMarginTemp(data, date) };


                //customer filters
                var customerProfitabiltyPercentage = FindPercentageProfitabiltyFromDataWithDifferenceTemp(customerDataByFilter, true, date);

                //item filters
                var itemProfitabiltyPercentage = FindPercentageProfitabiltyFromDataWithDifferenceTemp(itemDataByFilter, false, date);

                result.Customer = new GenericTwoBarChartData()
                {
                    TopBottomChartByMonth = new GenericTopBottomTwoBarChartData()
                    {
                        Bottom = customerProfitabiltyPercentage.OrderBy(d => d.Tooltip).Take(10).ToList(),
                        Top = customerProfitabiltyPercentage.OrderByDescending(d => d.Tooltip).Take(10).ToList(),
                    }

                };
                result.Item = new GenericTwoBarChartData()
                {
                    TopBottomChartByMonth = new GenericTopBottomTwoBarChartData()
                    {
                        Bottom = itemProfitabiltyPercentage.OrderBy(d => d.Tooltip).Take(10).ToList(),
                        Top = itemProfitabiltyPercentage.OrderByDescending(d => d.Tooltip).Take(10).ToList(),
                    }
                };
                return result;

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database" + e.Message);
            }


        }

        public async Task<GenericMarginChartData> GetMarginByDifference(GlobalFilter filters, bool isCustomer, double filterdata)
        {
            try
            {
                GenericMarginChartData result = new GenericMarginChartData();
                var dataTable = new DataSet();
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filters.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filters.Periods.Current.End));
                parameterList.Add(new SqlParameter("@historicalStart", filters.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filters.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@priorStart", filters.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filters.Periods.Prior.End));
                parameterList.Add(new SqlParameter("@filter", filterdata));
                parameterList.Add(new SqlParameter("@isCustomer", isCustomer));
                dataTable = base.ReadToDataSetViaProcedure("BI_GetProfitabilityDataByFilter", parameterList.ToArray());

                var data = dataTable.Tables[0]
                          .AsEnumerable()
                          .Select(x => new MarginChartBM
                          {
                              InvNo = x.Field<string>("invno"),
                              BinCost = x.Field<decimal>("bincost").ToRoundTwoDecimalDigits(),
                              Item = x.Field<string>("descrip"),
                              ItemCode = x.Field<string>("itemjoin"),
                              Period = x.Field<string>("Period"),
                              QuantityShipped = x.Field<int>("qtyshp"),
                              Revenue = x.Field<decimal>("extprice").ToRoundTwoDecimalDigits(),
                              CustCode = x.Field<string>("custno"),
                              Company = x.Field<string>("company"),
                              Date = x.Field<DateTime>("invdte"),

                          }).ToList();
                data = data.Where(x => !x.Company.Contains("DONATION") && !x.Company.Contains("DUMP")).ToList();
                result.ProfitabiltyChart = new List<GenericProfitablityChart>() { GetCustomerMargin(data, filters), GetItemMargin(data, filters) };

                //customer filters
                if (isCustomer)
                {
                    var customerProfitabiltyPercentage = FindPercentageProfitabiltyFromDataWithDifference(data, true, filters);

                    result.Customer = new GenericTwoBarChartData()
                    {
                        TopBottomChartByMonth = new GenericTopBottomTwoBarChartData()
                        {
                            Bottom = customerProfitabiltyPercentage.OrderBy(d => d.Tooltip).Take(10).ToList(),
                            Top = customerProfitabiltyPercentage.OrderByDescending(d => d.Tooltip).Take(10).ToList(),
                        }

                    };
                }
                else
                {
                    //item filters
                    var itemProfitabiltyPercentage = FindPercentageProfitabiltyFromDataWithDifference(data, false, filters);

                    result.Item = new GenericTwoBarChartData()
                    {
                        TopBottomChartByMonth = new GenericTopBottomTwoBarChartData()
                        {
                            Bottom = itemProfitabiltyPercentage.OrderBy(d => d.Tooltip).Take(10).ToList(),
                            Top = itemProfitabiltyPercentage.OrderByDescending(d => d.Tooltip).Take(10).ToList(),
                        }
                    };
                }
                return result;

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database" + e.Message);
            }


        }

        public async Task<GenericMarginChartData> GetMarginByDifferenceTemp(DateTime date, bool isCustomer, double filterdata)
        {
            try
            {
                GenericMarginChartData result = new GenericMarginChartData();
                var dataTable = new DataSet();
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@current_date", date));
                parameterList.Add(new SqlParameter("@filter", filterdata));
                parameterList.Add(new SqlParameter("@isCustomer", isCustomer));
                dataTable = base.ReadToDataSetViaProcedure("BI_GetProfitabilityDataByFilter", parameterList.ToArray());

                var data = dataTable.Tables[0]
                          .AsEnumerable()
                          .Select(x => new MarginChartBM
                          {
                              InvNo = x.Field<string>("invno"),
                              BinCost = x.Field<decimal>("bincost").ToRoundTwoDecimalDigits(),
                              Item = x.Field<string>("descrip"),
                              ItemCode = x.Field<string>("itemjoin"),
                              Period = x.Field<string>("Period"),
                              QuantityShipped = x.Field<int>("qtyshp"),
                              Revenue = x.Field<decimal>("extprice").ToRoundTwoDecimalDigits(),
                              CustCode = x.Field<string>("custno"),
                              Company = x.Field<string>("company"),
                              Date = x.Field<DateTime>("invdte"),

                          }).ToList();
                data = data.Where(x => !x.Company.Contains("DONATION") && !x.Company.Contains("DUMP")).ToList();
                result.ProfitabiltyChart = new List<GenericProfitablityChart>() { GetCustomerMarginTemp(data, date), GetItemMarginTemp(data, date) };

                //customer filters
                if (isCustomer)
                {
                    var customerProfitabiltyPercentage = FindPercentageProfitabiltyFromDataWithDifferenceTemp(data, true, date);

                    result.Customer = new GenericTwoBarChartData()
                    {
                        TopBottomChartByMonth = new GenericTopBottomTwoBarChartData()
                        {
                            Bottom = customerProfitabiltyPercentage.OrderBy(d => d.Tooltip).Take(10).ToList(),
                            Top = customerProfitabiltyPercentage.OrderByDescending(d => d.Tooltip).Take(10).ToList(),
                        }

                    };
                }
                else
                {
                    //item filters
                    var itemProfitabiltyPercentage = FindPercentageProfitabiltyFromDataWithDifferenceTemp(data, false, date);

                    result.Item = new GenericTwoBarChartData()
                    {
                        TopBottomChartByMonth = new GenericTopBottomTwoBarChartData()
                        {
                            Bottom = itemProfitabiltyPercentage.OrderBy(d => d.Tooltip).Take(10).ToList(),
                            Top = itemProfitabiltyPercentage.OrderByDescending(d => d.Tooltip).Take(10).ToList(),
                        }
                    };
                }
                return result;

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database" + e.Message);
            }


        }

        public List<GenericTwoBarChartdata> FindPercentageProfitabiltyFromData(List<MarginChartBM> data, bool isByMonth, bool isCustomer, DateTime date)
        {
            List<GenericTwoBarChartdata> summary = new List<GenericTwoBarChartdata>();
            if (isCustomer)
            {
                summary = data.GroupBy(d => new { d.Period, d.CustCode })
                                    .Select(w => new GenericTwoBarChartdata
                                    {
                                        Category = w.First().Company,
                                        Period = w.Key.Period,
                                        Value1 = (decimal)w.Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                                        Label = w.Key.CustCode
                                    }).ToList();
            }
            else
            {
                summary = data.GroupBy(d => new { d.Period, d.ItemCode })
                                   .Select(w => new GenericTwoBarChartdata
                                   {
                                       Category = w.First().Item,
                                       Period = w.Key.Period,
                                       Value1 = (decimal)w.Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                                       Label = w.Key.ItemCode
                                   }).ToList();
            }


            var result = summary.GroupBy(c => new { c.Label })
        .Select(g => new GenericTwoBarChartdata
        {
            Category = !string.IsNullOrEmpty(g.First().Category) ? g.First().Category.Trim() : "",
            Label = !string.IsNullOrEmpty(g.Key.Label) ? g.Key.Label.Trim() : "",
            Value1 = (decimal)(g.Where(d => d.Period == "current").Sum(s => s.Value1)),
            Value2 = isByMonth ?
                    (decimal)(g.Where(d => d.Period == "prior").Sum(s => s.Value1)) :
                    (decimal)(g.Where(d => d.Period == "previous").Sum(s => s.Value1)),

            Tooltip2 = (double)(g.Where(d => d.Period == "current").Sum(s => s.Value1) -
            (isByMonth ? g.Where(d => d.Period == "prior").Sum(s => s.Value1)
            : g.Where(d => d.Period == "previous").Sum(s => s.Value1))),

            Tooltip = (g.Where(d => d.Period == "current").Sum(s => s.Value1)
            .ToPercentageDifference((isByMonth ? g.Where(d => d.Period == "prior").Sum(s => s.Value1)
            : g.Where(d => d.Period == "previous").Sum(s => s.Value1)))),

            Color1 = ChartColorBM.MarginCurrent,
            Color2 = ChartColorBM.MarginPrevious,
            Period = date.ToString("MMM yyyy"),
            Period2 = isByMonth ? date.AddMonths(-1).ToString("MMM yyyy") : date.AddYears(-1).ToString("MMM yyyy")
        }).ToList();

            return result;
        }

        public List<GenericTwoBarChartdata> FindPercentageProfitabiltyFromDataWithDifference(List<MarginChartBM> data, bool isCustomer, GlobalFilter filter)
        {
            //check the filter based on the year or not because year filter dont have the prior period
            bool basedOnYear = (filter.Id == (int)GlobalFilterEnum.ThisYear) || (filter.Id == (int)GlobalFilterEnum.ThisYearToDate) || (filter.Id == (int)GlobalFilterEnum.ThisYearToLastMonth);

            List<GenericTwoBarChartdata> summary = new List<GenericTwoBarChartdata>();
            if (isCustomer)
            {
                summary = data.GroupBy(d => new { d.Period, d.CustCode })
                                    .Select(w => new GenericTwoBarChartdata
                                    {
                                        Category = w.First().Company,
                                        Period = w.Key.Period,
                                        Value1 = (decimal)w.Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),
                                        Value2 = (decimal)w.Sum(s => s.Revenue).ToStripDecimal(),
                                        Label = w.Key.CustCode
                                    }).ToList();
            }
            else
            {
                summary = data.GroupBy(d => new { d.Period, d.ItemCode })
                                   .Select(w => new GenericTwoBarChartdata
                                   {
                                       Category = w.First().Item,
                                       Period = w.Key.Period,
                                       Value1 = (decimal)w.Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),
                                       Value2 = (decimal)w.Sum(s => s.QuantityShipped),
                                       Label = w.Key.ItemCode
                                   }).ToList();
            }


            var result = summary.GroupBy(c => new { c.Label })
        .Select(g => new GenericTwoBarChartdata
        {
            Category = !string.IsNullOrEmpty(g.First().Category) ?
                                    string.Format(isCustomer ? "{0} (${1})" : "{0} ({1})"
                                                    , g.First().Category.Trim()
                                                    , (((decimal)(g.Where(d => d.Period == "current").Sum(s => s.Value2))) > 999)
                                                        ? string.Format(isCustomer ? "{0}K" : "{0}", Math.Round(((decimal)(g.Where(d => d.Period == "current").Sum(s => s.Value2))) / 1000, 1))
                                                        : ((decimal)(g.Where(d => d.Period == "current").Sum(s => s.Value2))).ToString()
                                                  )
                                                        : "",
            Label = !string.IsNullOrEmpty(g.Key.Label) ? g.Key.Label.Trim() : "",
            Value1 = (decimal)(g.Where(d => d.Period == "current").Sum(s => s.Value1)).ToStripDecimal(),
            Value2 = (decimal)(g.Where(d => d.Period == (basedOnYear ? "previous" : "prior")).Sum(s => s.Value1)).ToStripDecimal(),

            Tooltip2 = (double)(g.Where(d => d.Period == "current").Sum(s => s.Value1) -
            g.Where(d => d.Period == (basedOnYear ? "previous" : "prior")).Sum(s => s.Value1)),

            Tooltip = (g.Where(d => d.Period == "current").Sum(s => s.Value1)
            .ToPercentageDifference(g.Where(d => d.Period == (basedOnYear ? "previous" : "prior")).Sum(s => s.Value1)
           )),

            Color1 = ChartColorBM.MarginCurrent,
            Color2 = ChartColorBM.MarginPrevious,
            Period = filter.Periods.Current.Label,
            Period2 = filter.Periods.Historical.Label
        }).ToList();

            return result;
        }


        public List<GenericTwoBarChartdata> FindPercentageProfitabiltyFromDataWithDifferenceTemp(List<MarginChartBM> data, bool isCustomer, DateTime date)
        {
            List<GenericTwoBarChartdata> summary = new List<GenericTwoBarChartdata>();
            if (isCustomer)
            {
                summary = data.GroupBy(d => new { d.Period, d.CustCode })
                                    .Select(w => new GenericTwoBarChartdata
                                    {
                                        Category = w.First().Company,
                                        Period = w.Key.Period,
                                        Value1 = (decimal)w.Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                                        Value2 = (decimal)w.Sum(s => (s.Revenue)),
                                        Label = w.Key.CustCode
                                    }).ToList();
            }
            else
            {
                summary = data.GroupBy(d => new { d.Period, d.ItemCode })
                                   .Select(w => new GenericTwoBarChartdata
                                   {
                                       Category = w.First().Item,
                                       Period = w.Key.Period,
                                       Value1 = (decimal)w.Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                                       Value2 = (decimal)w.Sum(s => (s.Revenue)),
                                       Label = w.Key.ItemCode
                                   }).ToList();
            }


            var result = summary.GroupBy(c => new { c.Label })
        .Select(g => new GenericTwoBarChartdata
        {
            Category = !string.IsNullOrEmpty(g.First().Category) ?
                                    string.Format(isCustomer ? "{0} (${1})" : "{0} ({1})"
                                                    , g.First().Category.Trim()
                                                    , (((decimal)(g.Where(d => d.Period == "current").Sum(s => s.Value1))) > 999)
                                                        ? string.Format("{0}K", Math.Round(((decimal)(g.Where(d => d.Period == "current").Sum(s => s.Value1))) / 1000, 1))
                                                        : ((decimal)(g.Where(d => d.Period == "current").Sum(s => s.Value2))).ToString()
                                                  )
                                                        : "",
            Label = !string.IsNullOrEmpty(g.Key.Label) ? g.Key.Label.Trim() : "",
            Value1 = (decimal)(g.Where(d => d.Period == "current").Sum(s => s.Value1)),
            Value2 = (decimal)(g.Where(d => d.Period == "prior").Sum(s => s.Value1)),

            Tooltip2 = (double)(g.Where(d => d.Period == "current").Sum(s => s.Value1) -
            g.Where(d => d.Period == "prior").Sum(s => s.Value1)),

            Tooltip = (g.Where(d => d.Period == "current").Sum(s => s.Value1)
            .ToPercentageDifference(g.Where(d => d.Period == "prior").Sum(s => s.Value1)
           )),

            Color1 = ChartColorBM.MarginCurrent,
            Color2 = ChartColorBM.MarginPrevious,
            Period = date.ToString("MMM yyyy"),
            Period2 = date.AddMonths(-1).ToString("MMM yyyy")
        }).ToList();

            return result;
        }


        private GenericProfitablityChart GetCustomerMargin(List<MarginChartBM> data, GlobalFilter filter)
        {
            var result = new GenericProfitablityChart
            {
                Category = "Customer Margin",

                Column1 = filter.Periods.Current.Label,
                Column2 = filter.Periods.Historical.Label,
                Column3 = filter.Periods.Prior.Label,

                Val1 = (decimal)data.Where(c => c.Period == "current").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),
                Val2 = (decimal)data.Where(c => c.Period == "previous").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),
                Val3 = (decimal)data.Where(c => c.Period == "prior").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),

                Color1 = ChartColorBM.MarginCurrent,
                Color2 = ChartColorBM.MarginPrevious,
                Color3 = ChartColorBM.MarginCurrent,

                SubData = data.Select(y => new GenericProfitablityTopBottomBM
                {
                    Top = data.GroupBy(f => f.CustCode)
                    .OrderByDescending(g => g.Where(k => k.Period == "current").Sum(h => (h.Revenue) - (h.BinCost * h.QuantityShipped)))
                    .Take(10).Select(j => new GenericProfitablityChart
                    {
                        Id = j.Key,
                        Key = "Customer",
                        Category = j.First().Company,

                        Column1 = filter.Periods.Current.Label,
                        Column2 = filter.Periods.Historical.Label,
                        Column3 = filter.Periods.Prior.Label,

                        Val1 = (decimal)j.Where(c => c.Period == "current").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),
                        Val2 = (decimal)j.Where(c => c.Period == "previous").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),
                        Val3 = (decimal)j.Where(c => c.Period == "prior").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),

                        Color1 = ChartColorBM.MarginCurrent,
                        Color2 = ChartColorBM.MarginPrevious,
                        Color3 = ChartColorBM.MarginCurrent,
                    }).ToList(),

                    Bottom = data.GroupBy(f => f.CustCode)
                    .OrderBy(g => g.Where(k => k.Period == "current").Sum(h => (h.Revenue) - (h.BinCost * h.QuantityShipped)))
                    .Take(10).Select(j => new GenericProfitablityChart
                    {
                        Id = j.Key,
                        Key = "Customer",
                        Category = j.First().Company,

                        Column1 = filter.Periods.Current.Label,
                        Column2 = filter.Periods.Historical.Label,
                        Column3 = filter.Periods.Prior.Label,

                        Val1 = (decimal)j.Where(c => c.Period == "current").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),
                        Val2 = (decimal)j.Where(c => c.Period == "previous").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),
                        Val3 = (decimal)j.Where(c => c.Period == "prior").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),

                        Color1 = ChartColorBM.MarginCurrent,
                        Color2 = ChartColorBM.MarginPrevious,
                        Color3 = ChartColorBM.MarginCurrent,
                    }).ToList(),
                }).FirstOrDefault()
            };
            return result;


        }

        private GenericProfitablityChart GetCustomerMarginTemp(List<MarginChartBM> data, DateTime date)
        {
            var result = new GenericProfitablityChart
            {
                Category = "Customer Margin",

                Column1 = date.ToString("MMM yyyy"),
                Column2 = date.AddYears(-1).ToString("MMM yyyy"),
                Column3 = date.AddMonths(-1).ToString("MMM yyyy"),

                Val1 = (decimal)data.Where(c => c.Period == "current").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                Val2 = (decimal)data.Where(c => c.Period == "previous").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                Val3 = (decimal)data.Where(c => c.Period == "prior").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),

                Color1 = ChartColorBM.MarginCurrent,
                Color2 = ChartColorBM.MarginPrevious,
                Color3 = ChartColorBM.MarginCurrent,

                SubData = data.Select(y => new GenericProfitablityTopBottomBM
                {
                    Top = data.GroupBy(f => f.CustCode)
                    .OrderByDescending(g => g.Where(k => k.Period == "current").Sum(h => (h.Revenue) - (h.BinCost * h.QuantityShipped)))
                    .Take(10).Select(j => new GenericProfitablityChart
                    {
                        Id = j.Key,
                        Key = "Customer",
                        Category = j.First().Company,

                        Column1 = date.ToString("MMM yyyy"),
                        Column2 = date.AddYears(-1).ToString("MMM yyyy"),
                        Column3 = date.AddMonths(-1).ToString("MMM yyyy"),

                        Val1 = (decimal)j.Where(c => c.Period == "current").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                        Val2 = (decimal)j.Where(c => c.Period == "previous").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                        Val3 = (decimal)j.Where(c => c.Period == "prior").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),

                        Color1 = ChartColorBM.MarginCurrent,
                        Color2 = ChartColorBM.MarginPrevious,
                        Color3 = ChartColorBM.MarginCurrent,
                    }).ToList(),

                    Bottom = data.GroupBy(f => f.CustCode)
                    .OrderBy(g => g.Where(k => k.Period == "current").Sum(h => (h.Revenue) - (h.BinCost * h.QuantityShipped)))
                    .Take(10).Select(j => new GenericProfitablityChart
                    {
                        Id = j.Key,
                        Key = "Customer",
                        Category = j.First().Company,

                        Column1 = date.ToString("MMM yyyy"),
                        Column2 = date.AddYears(-1).ToString("MMM yyyy"),
                        Column3 = date.AddMonths(-1).ToString("MMM yyyy"),

                        Val1 = (decimal)j.Where(c => c.Period == "current").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                        Val2 = (decimal)j.Where(c => c.Period == "previous").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                        Val3 = (decimal)j.Where(c => c.Period == "prior").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),

                        Color1 = ChartColorBM.MarginCurrent,
                        Color2 = ChartColorBM.MarginPrevious,
                        Color3 = ChartColorBM.MarginCurrent,
                    }).ToList(),
                }).FirstOrDefault()
            };
            return result;


        }

        private GenericProfitablityChart GetItemMargin(List<MarginChartBM> data, GlobalFilter filter)
        {
            var result = new GenericProfitablityChart
            {
                Category = "Item Margin",

                Column1 = filter.Periods.Current.Label,
                Column2 = filter.Periods.Historical.Label,
                Column3 = filter.Periods.Prior.Label,

                Val1 = (decimal)data.Where(c => c.Period == "current").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),
                Val2 = (decimal)data.Where(c => c.Period == "previous").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),
                Val3 = (decimal)data.Where(c => c.Period == "prior").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),

                Color1 = ChartColorBM.MarginCurrent,
                Color2 = ChartColorBM.MarginPrevious,
                Color3 = ChartColorBM.MarginCurrent,

                SubData = data.Select(y => new GenericProfitablityTopBottomBM
                {
                    Top = data.GroupBy(f => f.ItemCode)
                    .OrderByDescending(g => g.Where(k => k.Period == "current").Sum(h => (h.Revenue) - (h.BinCost * h.QuantityShipped)))
                    .Take(10).Select(j => new GenericProfitablityChart
                    {
                        Id = j.Key,
                        Key = "Item",
                        Category = j.First().Item,

                        Column1 = filter.Periods.Current.Label,
                        Column2 = filter.Periods.Historical.Label,
                        Column3 = filter.Periods.Prior.Label,

                        Val1 = (decimal)j.Where(c => c.Period == "current").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),
                        Val2 = (decimal)j.Where(c => c.Period == "previous").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),
                        Val3 = (decimal)j.Where(c => c.Period == "prior").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),

                        Color1 = ChartColorBM.MarginCurrent,
                        Color2 = ChartColorBM.MarginPrevious,
                        Color3 = ChartColorBM.MarginCurrent,
                    }).ToList(),

                    Bottom = data.GroupBy(f => f.ItemCode)
                    .OrderBy(g => g.Where(k => k.Period == "current").Sum(h => (h.Revenue) - (h.BinCost * h.QuantityShipped)))
                    .Take(10).Select(j => new GenericProfitablityChart
                    {
                        Id = j.Key,
                        Key = "Item",
                        Category = j.First().Item,

                        Column1 = filter.Periods.Current.Label,
                        Column2 = filter.Periods.Historical.Label,
                        Column3 = filter.Periods.Prior.Label,

                        Val1 = (decimal)j.Where(c => c.Period == "current").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),
                        Val2 = (decimal)j.Where(c => c.Period == "previous").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),
                        Val3 = (decimal)j.Where(c => c.Period == "prior").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits().ToStripDecimal(),

                        Color1 = ChartColorBM.MarginCurrent,
                        Color2 = ChartColorBM.MarginPrevious,
                        Color3 = ChartColorBM.MarginCurrent,
                    }).ToList(),
                }).FirstOrDefault()
            };
            return result;


        }

        private GenericProfitablityChart GetItemMarginTemp(List<MarginChartBM> data, DateTime date)
        {
            var result = new GenericProfitablityChart
            {
                Category = "Item Margin",

                Column1 = date.ToString("MMM yyyy"),
                Column2 = date.AddYears(-1).ToString("MMM yyyy"),
                Column3 = date.AddMonths(-1).ToString("MMM yyyy"),

                Val1 = (decimal)data.Where(c => c.Period == "current").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                Val2 = (decimal)data.Where(c => c.Period == "previous").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                Val3 = (decimal)data.Where(c => c.Period == "prior").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),

                Color1 = ChartColorBM.MarginCurrent,
                Color2 = ChartColorBM.MarginPrevious,
                Color3 = ChartColorBM.MarginCurrent,

                SubData = data.Select(y => new GenericProfitablityTopBottomBM
                {
                    Top = data.GroupBy(f => f.ItemCode)
                    .OrderByDescending(g => g.Where(k => k.Period == "current").Sum(h => (h.Revenue) - (h.BinCost * h.QuantityShipped)))
                    .Take(10).Select(j => new GenericProfitablityChart
                    {
                        Id = j.Key,
                        Key = "Item",
                        Category = j.First().Item,

                        Column1 = date.ToString("MMM yyyy"),
                        Column2 = date.AddYears(-1).ToString("MMM yyyy"),
                        Column3 = date.AddMonths(-1).ToString("MMM yyyy"),

                        Val1 = (decimal)j.Where(c => c.Period == "current").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                        Val2 = (decimal)j.Where(c => c.Period == "previous").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                        Val3 = (decimal)j.Where(c => c.Period == "prior").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),

                        Color1 = ChartColorBM.MarginCurrent,
                        Color2 = ChartColorBM.MarginPrevious,
                        Color3 = ChartColorBM.MarginCurrent,
                    }).ToList(),

                    Bottom = data.GroupBy(f => f.ItemCode)
                    .OrderBy(g => g.Where(k => k.Period == "current").Sum(h => (h.Revenue) - (h.BinCost * h.QuantityShipped)))
                    .Take(10).Select(j => new GenericProfitablityChart
                    {
                        Id = j.Key,
                        Key = "Item",
                        Category = j.First().Item,

                        Column1 = date.ToString("MMM yyyy"),
                        Column2 = date.AddYears(-1).ToString("MMM yyyy"),
                        Column3 = date.AddMonths(-1).ToString("MMM yyyy"),

                        Val1 = (decimal)j.Where(c => c.Period == "current").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                        Val2 = (decimal)j.Where(c => c.Period == "previous").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),
                        Val3 = (decimal)j.Where(c => c.Period == "prior").Sum(s => (s.Revenue) - (s.BinCost * s.QuantityShipped)).ToScaleDownAndRoundTwoDigits(),

                        Color1 = ChartColorBM.MarginCurrent,
                        Color2 = ChartColorBM.MarginPrevious,
                        Color3 = ChartColorBM.MarginCurrent,
                    }).ToList(),
                }).FirstOrDefault()
            };
            return result;


        }

        public async Task<List<MarginChartBM>> GetCustomerMarginReport(int filterId, int period, string Custno)
        {
            try
            {
                var filterLists = GlobaldataProvider.GetFilterWithPeriods();
                var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();


                DateTime CurrentEndDate = targetFilter.Periods.Current.End;
                DateTime HistoricalEndDate = targetFilter.Periods.Historical.End;
                DateTime PriorEndDate = targetFilter.Periods.Prior.End;

                DateTime startDate, endDate;
                if (period == (int)PeriodEnum.Historical)
                {
                    var filterListsHistorical = GlobaldataProvider.GetFilterWithPeriodsByDate(HistoricalEndDate);
                    var targetFilterHistorical = filterListsHistorical.Where(d => d.Id == filterId).FirstOrDefault();
                    startDate = targetFilterHistorical.Periods.Current.Start;
                    endDate = targetFilterHistorical.Periods.Current.End;
                }
                else if (period == (int)PeriodEnum.Prior)
                {
                    var filterListsPrior = GlobaldataProvider.GetFilterWithPeriodsByDate(PriorEndDate);
                    var targetFilterPrior = filterListsPrior.Where(d => d.Id == filterId).FirstOrDefault();
                    startDate = targetFilterPrior.Periods.Current.Start;
                    endDate = targetFilterPrior.Periods.Current.End;
                }
                else
                {
                    startDate = targetFilter.Periods.Current.Start;
                    endDate = targetFilter.Periods.Current.End;
                }

                List<MarginChartBM> result = new List<MarginChartBM>();
                var dataTable = new DataSet();
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startDate", startDate));
                parameterList.Add(new SqlParameter("@endDate", endDate));
                parameterList.Add(new SqlParameter("@customer", Custno));
                dataTable = base.ReadToDataSetViaProcedure("BI_GetCustomerMarginReport", parameterList.ToArray());

                result = dataTable.Tables[0]
                         .AsEnumerable()
                         .Select(x => new MarginChartBM
                         {
                             InvNo = x.Field<string>("invno"),
                             Date = x.Field<DateTime>("invdte"),
                             CreditCode = x.Field<string>("CreditCode"),
                             ItemCode = x.Field<string>("item"),
                             Item = x.Field<string>("descrip"),
                             CustCode = x.Field<string>("Customer"),
                             Company = x.Field<string>("CustomerName"),
                             SalesPerson = x.Field<string>("SalesPerson"),
                             QuantityShipped = x.Field<int>("qtyshp"),
                             Revenue = x.Field<decimal>("extprice").ToRoundTwoDecimalDigits(),
                             Margin = x.Field<decimal>("Margin"),
                             MarginPercentage = x.Field<decimal>("Margin%").ToRoundTwoDecimalDigits(),

                             //BinCost = x.Field<decimal>("bincost").ToRoundTwoDecimalDigits(),
                             //


                         })
                         .OrderByDescending(p=>p.Revenue)
                         .ToList();

                return result;

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database" + e.Message);
            }


        }

        public async Task<List<MarginChartBM>> GetCustomerMarginReportTemp(DateTime date, string Custno)
        {
            try
            {
                List<MarginChartBM> result = new List<MarginChartBM>();
                var dataTable = new DataSet();
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@date", date));
                parameterList.Add(new SqlParameter("@customer", Custno));
                dataTable = base.ReadToDataSetViaProcedure("BI_GetCustomerMarginReport", parameterList.ToArray());

                result = dataTable.Tables[0]
                         .AsEnumerable()
                         .Select(x => new MarginChartBM
                         {
                             InvNo = x.Field<string>("invno"),
                             Date = x.Field<DateTime>("invdte"),
                             CreditCode = x.Field<string>("CreditCode"),
                             ItemCode = x.Field<string>("item"),
                             Item = x.Field<string>("descrip"),
                             CustCode = x.Field<string>("Customer"),
                             Company = x.Field<string>("CustomerName"),
                             SalesPerson = x.Field<string>("SalesPerson"),
                             QuantityShipped = x.Field<int>("qtyshp"),
                             Revenue = x.Field<decimal>("extprice").ToRoundTwoDecimalDigits(),
                             Margin = x.Field<decimal>("Margin"),
                             MarginPercentage = x.Field<decimal>("Margin%").ToRoundTwoDecimalDigits(),

                             //BinCost = x.Field<decimal>("bincost").ToRoundTwoDecimalDigits(),
                             //


                         }).ToList();

                return result;

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database" + e.Message);
            }


        }

        public async Task<List<MarginChartBM>> GetItemMarginReport(int filterId, int period, string item)
        {
            try
            {
                var filterLists = GlobaldataProvider.GetFilterWithPeriods();
                var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();


                DateTime CurrentEndDate = targetFilter.Periods.Current.End;
                DateTime HistoricalEndDate = targetFilter.Periods.Historical.End;
                DateTime PriorEndDate = targetFilter.Periods.Prior.End;

                DateTime startDate, endDate;
                if (period == (int)PeriodEnum.Historical)
                {
                    var filterListsHistorical = GlobaldataProvider.GetFilterWithPeriodsByDate(HistoricalEndDate);
                    var targetFilterHistorical = filterListsHistorical.Where(d => d.Id == filterId).FirstOrDefault();
                    startDate = targetFilterHistorical.Periods.Current.Start;
                    endDate = targetFilterHistorical.Periods.Current.End;
                }
                else if (period == (int)PeriodEnum.Prior)
                {
                    var filterListsPrior = GlobaldataProvider.GetFilterWithPeriodsByDate(PriorEndDate);
                    var targetFilterPrior = filterListsPrior.Where(d => d.Id == filterId).FirstOrDefault();
                    startDate = targetFilterPrior.Periods.Current.Start;
                    endDate = targetFilterPrior.Periods.Current.End;
                }
                else
                {
                    startDate = targetFilter.Periods.Current.Start;
                    endDate = targetFilter.Periods.Current.End;
                }

                List<MarginChartBM> result = new List<MarginChartBM>();
                var dataTable = new DataSet();
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startDate", startDate));
                parameterList.Add(new SqlParameter("@endDate", endDate));
                parameterList.Add(new SqlParameter("@item", item));
                dataTable = base.ReadToDataSetViaProcedure("BI_GetItemMarginReport", parameterList.ToArray());

                result = dataTable.Tables[0]
                         .AsEnumerable()
                         .Select(x => new MarginChartBM
                         {
                             CustCode = x.Field<string>("Customer"),
                             Company = x.Field<string>("company"),
                             ItemCode = x.Field<string>("item"),
                             Item = x.Field<string>("descrip"),
                             CreditCode = x.Field<string>("CreditCode"),
                             InvNo = x.Field<string>("invno"),
                             Date = x.Field<DateTime>("invdte"),
                             SalesPerson = x.Field<string>("SalesPerson"),
                             QuantityShipped = x.Field<int>("qtyshp"),
                             Revenue = x.Field<decimal>("extprice").ToRoundTwoDecimalDigits(),
                             ExtCost = x.Field<decimal>("extCost").ToRoundTwoDecimalDigits(),
                             Margin = x.Field<decimal>("Margin"),
                             MarginPercentage = x.Field<decimal>("Margin%").ToRoundTwoDecimalDigits(),
                         }).ToList();

                return result;

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database" + e.Message);
            }


        }

        public async Task<List<MarginChartBM>> GetItemMarginReportTemp(DateTime date, string item)
        {
            try
            {
                List<MarginChartBM> result = new List<MarginChartBM>();
                var dataTable = new DataSet();
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@date", date));
                parameterList.Add(new SqlParameter("@item", item));
                dataTable = base.ReadToDataSetViaProcedure("BI_GetItemMarginReport", parameterList.ToArray());

                result = dataTable.Tables[0]
                         .AsEnumerable()
                         .Select(x => new MarginChartBM
                         {
                             CustCode = x.Field<string>("Customer"),
                             Company = x.Field<string>("company"),
                             ItemCode = x.Field<string>("item"),
                             Item = x.Field<string>("descrip"),
                             CreditCode = x.Field<string>("CreditCode"),
                             InvNo = x.Field<string>("invno"),
                             Date = x.Field<DateTime>("invdte"),
                             SalesPerson = x.Field<string>("SalesPerson"),
                             QuantityShipped = x.Field<int>("qtyshp"),
                             Revenue = x.Field<decimal>("extprice").ToRoundTwoDecimalDigits(),
                             ExtCost = x.Field<decimal>("extCost").ToRoundTwoDecimalDigits(),
                             Margin = x.Field<decimal>("Margin"),
                             MarginPercentage = x.Field<decimal>("Margin%").ToRoundTwoDecimalDigits(),
                         }).ToList();

                return result;

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database" + e.Message);
            }
        }

        public BarchartOrderBy GetProfitByItem(GlobalFilter filter,string userId)
        {
            try
            {

                AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
                var userAccessibleCategories = _adminUserManagementDateProvider.GetUserAccess(userId);

                var parameterList = new List<SqlParameter>();

                DataTable dtCategories = new DataTable();
                dtCategories.Columns.Add("Code");
                foreach (var category in userAccessibleCategories.Categories)
                {
                    if (category.IsAccess == true) { dtCategories.Rows.Add(category.Name); }
                }
                var param = new SqlParameter();
                param.SqlDbType = SqlDbType.Structured;
                param.Value = dtCategories;
                param.ParameterName = "@categories";


                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@priorStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filter.Periods.Historical.End));
                parameterList.Add(param);

                var dataSetResult = base.ReadToDataSetViaProcedure("BI_PF_GetProfitByItem", parameterList.ToArray());
                var casesSoldProvider = new CasesSoldRevenueDataProvider();
                var result = dataSetResult.Tables[0]
                                .AsEnumerable()
                                .Select(x => new BarChartDetails
                                {
                                    Code = !string.IsNullOrEmpty(x.Field<string>("ItemCode")) ? x.Field<string>("ItemCode").Trim() : "",
                                    GroupName = !string.IsNullOrEmpty(x.Field<string>("ItemDescription")) ? x.Field<string>("ItemDescription").Trim() : "",
                                    growth = (x.Field<decimal?>("growth").Value),
                                    prior = (x.Field<decimal?>("prior").Value),
                                    value = (x.Field<decimal?>("current").Value),
                                    Category = !string.IsNullOrEmpty(x.Field<string>("Category")) ? x.Field<string>("Category").Trim() : "",
                                    GroupName1 = casesSoldProvider.GetActiveSalesManName(!string.IsNullOrEmpty(x.Field<string>("ItemDescription")) ? x.Field<string>("ItemDescription").Trim() : ""),
                                })
                                .ToList();
                var profitByItem = new BarchartOrderBy
                {
                    Top = result.Where(x => x.Category == "Top")
                                .Select((x, idx) => new BarChartDetails
                                {
                                    Code = x.Code,
                                    GroupName = x.GroupName,
                                    GroupName1 = x.GroupName1,
                                    growth = x.growth,
                                    prior = x.prior,
                                    value = x.value,
                                    Category = x.Category,
                                    Color1 = TopBottom25GraphColors.Colors[idx].Primary,
                                    Label1 = filter.Periods.Current.Label,
                                    LabelPrior = filter.Periods.Historical.Label
                                })
                                .ToList(),
                    Bottom = result.Where(x => x.Category == "Bottom")
                                .Select((x, idx) => new BarChartDetails
                                {
                                    Code = x.Code,
                                    GroupName = x.GroupName,
                                    GroupName1 = x.GroupName1,
                                    growth = x.growth,
                                    prior = x.prior,
                                    value = x.value,
                                    Category = x.Category,
                                    Color1 = TopBottom25GraphColors.Colors[idx].Primary,
                                    Label1 = filter.Periods.Current.Label,
                                    LabelPrior = filter.Periods.Historical.Label
                                })
                                .ToList(),

                };

                return profitByItem;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);

            }


        }

        public BarchartOrderBy GetProfitByCustomer(GlobalFilter filter, string userId)
        {
            AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
            var userAccessibleCategories = _adminUserManagementDateProvider.GetUserAccess(userId);

        
            try
            {
                DataTable dtCategories = new DataTable();
                dtCategories.Columns.Add("Code");
                foreach (var category in userAccessibleCategories.Categories)
                {
                    if (category.IsAccess == true) { dtCategories.Rows.Add(category.Name); }
                }
                var param = new SqlParameter();
                param.SqlDbType = SqlDbType.Structured;
                param.Value = dtCategories;
                param.ParameterName = "@categories";

                var parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@priorStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filter.Periods.Historical.End));
                parameterList.Add(param);

                var dataSetResult = base.ReadToDataSetViaProcedure("BI_PF_GetProfitByCustomer", parameterList.ToArray());
                var casesSoldProvider = new CasesSoldRevenueDataProvider();
                var result = dataSetResult.Tables[0]
                                .AsEnumerable()
                                .Select(x => new BarChartDetails
                                {
                                    Code = !string.IsNullOrEmpty(x.Field<string>("CustomerNumber")) ? x.Field<string>("CustomerNumber").Trim() : "",
                                    GroupName = !string.IsNullOrEmpty(x.Field<string>("CustomerName")) ? x.Field<string>("CustomerName").Trim() : "",
                                    growth = (x.Field<decimal?>("growth").Value),
                                    prior = (x.Field<decimal?>("prior").Value),
                                    value = (x.Field<decimal?>("current").Value),
                                    Category = !string.IsNullOrEmpty(x.Field<string>("Category")) ? x.Field<string>("Category").Trim() : "",
                                    GroupName1 = !string.IsNullOrEmpty(x.Field<string>("CustomerName")) ? x.Field<string>("CustomerName").Trim() : "",
                                })
                                .ToList();
                var profitByCustomer = new BarchartOrderBy
                {
                    Top = result.Where(x => x.Category == "Top")
                                .Select((x, idx) => new BarChartDetails
                                {
                                    Code = x.Code,
                                    GroupName = x.GroupName,
                                    GroupName1=x.GroupName1,
                                    growth = x.growth,
                                    prior = x.prior,
                                    value = x.value,
                                    Category = x.Category,
                                    Color1 = TopBottom25GraphColors.Colors[idx].Primary,
                                    Label1 = filter.Periods.Current.Label,
                                    LabelPrior = filter.Periods.Historical.Label
                                })
                                .ToList(),
                    Bottom = result.Where(x => x.Category == "Bottom")
                                .Select((x, idx) => new BarChartDetails
                                {
                                    Code = x.Code,
                                    GroupName = x.GroupName,
                                    GroupName1 = x.GroupName1,
                                    growth = x.growth,
                                    prior = x.prior,
                                    value = x.value,
                                    Category = x.Category,
                                    Color1 = TopBottom25GraphColors.Colors[idx].Primary,
                                    Label1 = filter.Periods.Current.Label,
                                    LabelPrior = filter.Periods.Historical.Label
                                })
                                .ToList(),

                };

                return profitByCustomer;
            }
            catch (Exception e)
            {
                ErrorLog.ErrorLogging(e);
                throw new Exception("An error occured while fetching data from database - " + e.Message);

            }


        }

        public List<ProfitabilityChartBM> GetCustomerWiseProfitByItem(GlobalFilter filter,string itemCode)
        {
            try
            {
                var parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@priorStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filter.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@itemCode", itemCode));


                var dataSetResult = base.ReadToDataSetViaProcedure("BI_PF_GetProfitByItemDetails", parameterList.ToArray());
                var casesSoldProvider = new CasesSoldRevenueDataProvider();
                var result = dataSetResult.Tables[0]
                                .AsEnumerable()
                                .Select(x => new ProfitabilityChartBM
                                {
                                    CompanyCode= !string.IsNullOrEmpty(x.Field<string>("CompanyNumber")) ? x.Field<string>("CompanyNumber").Trim() : "",
                                    CompanyName= !string.IsNullOrEmpty(x.Field<string>("CompanyName")) ? x.Field<string>("CompanyName").Trim() : "",
                                    CurrentProfit= x.Field<decimal?>("ProfitCurrent").Value,
                                    GrowthProfit =  x.Field<decimal?>("growth").Value,
                                    DifferenceProfit = x.Field<decimal?>("Difference").Value,
                                    PriorProfit = x.Field<decimal?>("ProfitPrior").Value,
                                }).OrderByDescending(x=>x.CurrentProfit)
                                .ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);

            }
        }

        public List<ProfitByCustomer> GetProfitByCustomerDetailAndCommodity(ProfitByCustomerRequest request)
        {
            try
            {

                var filterLists = GlobaldataProvider.GetFilterWithPeriods();

                var targetFilter = filterLists.Where(d => d.Id == request.FilterId).FirstOrDefault();


                DateTime CurrentEndDate = targetFilter.Periods.Current.End;
                DateTime HistoricalEndDate = targetFilter.Periods.Historical.End;
                DateTime PriorEndDate = targetFilter.Periods.Prior.End;

                DateTime startDate, endDate, prevStartDate, prevEndDate;
                if (request.Period == (int)PeriodEnum.Historical)
                {
                    var filterListsHistorical = GlobaldataProvider.GetFilterWithPeriodsByDate(HistoricalEndDate);
                    var targetFilterHistorical = filterListsHistorical.Where(d => d.Id == request.FilterId).FirstOrDefault();
                    startDate = targetFilterHistorical.Periods.Current.Start;
                    endDate = targetFilterHistorical.Periods.Current.End;
                    prevStartDate = targetFilterHistorical.Periods.Historical.Start;
                    prevEndDate = targetFilterHistorical.Periods.Historical.End;
                }
                else if (request.Period == (int)PeriodEnum.Prior)
                {
                    var filterListsPrior = GlobaldataProvider.GetFilterWithPeriodsByDate(PriorEndDate);
                    var targetFilterPrior = filterListsPrior.Where(d => d.Id == request.FilterId).FirstOrDefault();
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

                var parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", startDate));
                parameterList.Add(new SqlParameter("@currentEnd", endDate));
             
                parameterList.Add(new SqlParameter("@commodity", request.Commodity));
               
                if (request.Salesman == null) {request.Salesman = string.Empty; }
                parameterList.Add(new SqlParameter("@salesman", request.Salesman));
                DataSet dataSetResult = new DataSet();
                if (request.IsCM01 ==true)
                {
                  
                    dataSetResult = base.ReadToDataSetViaProcedure("BI_PF_GetProfitByCustomerDetailsForCustomerService", parameterList.ToArray());
                   
                }
                else
                {
                    parameterList.Add(new SqlParameter("@itemCode", request.ItemCode));
                    parameterList.Add(new SqlParameter("@customerCode", request.CustomerCode));
                    dataSetResult = base.ReadToDataSetViaProcedure("BI_PF_GetProfitByCustomerDetails", parameterList.ToArray());
                }
                var result = dataSetResult.Tables[0]
                                .AsEnumerable()
                                .Select(x => new ProfitByCustomer
                                {
                                    Commodity = !string.IsNullOrEmpty(x.Field<string>("Commodity")) ? x.Field<string>("Commodity").Trim() : "",
                                    InvoiceDate = (x.Field<DateTime>("InvoiceDate")),
                                    InvoiceNumber= !string.IsNullOrEmpty(x.Field<string>("InvoiceNumber")) ? x.Field<string>("InvoiceNumber").Trim() : "",
                                    ItemCode = !string.IsNullOrEmpty(x.Field<string>("ItemCode")) ? x.Field<string>("ItemCode").Trim() : "",
                                    ItemDescription = !string.IsNullOrEmpty(x.Field<string>("ItemDescription")) ? x.Field<string>("ItemDescription").Trim() : "",
                                    SalesManCode = !string.IsNullOrEmpty(x.Field<string>("SalesManCode")) ? x.Field<string>("SalesManCode").Trim() : "",
                                    SalesManDescription = !string.IsNullOrEmpty(x.Field<string>("SalesManDescription")) ? x.Field<string>("SalesManDescription").Trim() : "",
                                    SalesOrder = !string.IsNullOrEmpty(x.Field<string>("SalesOrder")) ? x.Field<string>("SalesOrder").Trim() : "",
                                    Cost = x.Field<decimal?>("Cost").Value,
                                    QtyShip = x.Field<decimal?>("qtyshp").Value,
                                    ExtPrice = x.Field<decimal?>("extprice").Value,
                                   Profit = x.Field<decimal?>("Profit").Value,
                                }).OrderByDescending(x=>x.Profit).ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);

            }


        }

        public List<ProfitByCustomer> GetProfitByCustomerDetailForCustomerService(GlobalFilter filter, string commodity, string salesman)
        {
            try
            {
                var parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@commodity", commodity));
                if (salesman == null) { salesman = string.Empty; }
                parameterList.Add(new SqlParameter("@salesman", salesman));
                DataSet dataSetResult = new DataSet();

                dataSetResult = base.ReadToDataSetViaProcedure("BI_PF_GetProfitByCustomerDetailsForCustomerService", parameterList.ToArray());

                var result = dataSetResult.Tables[0]
                                .AsEnumerable()
                                .Select(x => new ProfitByCustomer
                                {
                                    Commodity = !string.IsNullOrEmpty(x.Field<string>("Commodity")) ? x.Field<string>("Commodity").Trim() : "",
                                    InvoiceDate = (x.Field<DateTime>("InvoiceDate")),
                                    InvoiceNumber = !string.IsNullOrEmpty(x.Field<string>("InvoiceNumber")) ? x.Field<string>("InvoiceNumber").Trim() : "",
                                    ItemCode = !string.IsNullOrEmpty(x.Field<string>("ItemCode")) ? x.Field<string>("ItemCode").Trim() : "",
                                    ItemDescription = !string.IsNullOrEmpty(x.Field<string>("ItemDescription")) ? x.Field<string>("ItemDescription").Trim() : "",
                                    SalesManCode = !string.IsNullOrEmpty(x.Field<string>("SalesManCode")) ? x.Field<string>("SalesManCode").Trim() : "",
                                    SalesManDescription = !string.IsNullOrEmpty(x.Field<string>("SalesManDescription")) ? x.Field<string>("SalesManDescription").Trim() : "",
                                    SalesOrder = !string.IsNullOrEmpty(x.Field<string>("SalesOrder")) ? x.Field<string>("SalesOrder").Trim() : "",
                                    Cost = x.Field<decimal?>("Cost").Value,
                                    QtyShip = x.Field<decimal?>("qtyshp").Value,
                                    ExtPrice = x.Field<decimal?>("extprice").Value,
                                    Profit = x.Field<decimal?>("Profit").Value,
                                }).OrderByDescending(x => x.Profit).ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);

            }


        }

        #region Get Profit Data

        private string GetSalemanNameFromGroup(IGrouping<string, ProfitablityBarChartData> thir)
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
        public GenericDrillDownChartsChartsBO GetProfitData(GlobalFilter filters, string userId)
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
                parameterList.Add(new SqlParameter("@startDate", filters.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@endDate", filters.Periods.Current.End));
                parameterList.Add(new SqlParameter("@historicalStart", filters.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filters.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@previousStart", filters.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@previousEnd", filters.Periods.Prior.End));

                var dataSetResultTotal = base.ReadToDataSetViaProcedure("BI_PF_GetDashboardData", parameterList.ToArray());
                var resultTotal = dataSetResultTotal.Tables[0]
                        .AsEnumerable()
                        .Select(x => new ProfitablityBarChartData
                        {
                            Commodity = !string.IsNullOrEmpty(x.Field<string>("Commodity")) ? x.Field<string>("Commodity").Trim() : "",
                            Category = !string.IsNullOrEmpty(x.Field<string>("Category")) ? x.Field<string>("Category").Trim() : "",
                            Profit = x.Field<decimal?>("Profit").Value,
                            AssignedSalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("AssignedPersonCode")) ? x.Field<string>("AssignedPersonCode").Trim() : "",
                          //  SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? x.Field<string>("SalesPersonCode").Trim() : "",
                            SalesPersonDescription = !string.IsNullOrEmpty(x.Field<string>("AssignedPersonDescription")) ? x.Field<string>("AssignedPersonDescription").Trim() : "",
                            Period = !string.IsNullOrEmpty(x.Field<string>("Period")) ? x.Field<string>("Period").Trim() : "",
                        })
        .ToList();

                resultTotal = resultTotal.Where(f => f.SalesPersonCode != "DUMP" && f.SalesPersonCode != "DONA").ToList();
                CasesSoldRevenueDataProvider casesSoldRevenuDataProvide = new CasesSoldRevenueDataProvider();

                var userAccessibleCategories = _adminUserManagementDateProvider.GetUserDetails(userId);


                //totalCasesSold.All = MappingToGenericListProfit(resultTotal, filters, userAccessibleCategories);
                totalRevenue.All = MappingToGenericListProfit(resultTotal, filters, userAccessibleCategories);
                response.TotalRevenue = totalRevenue;
                response.TotalCasesSold = totalCasesSold;


                var responseWithDiffPercentageInRevenue = CaluclateDifference(response);
                return responseWithDiffPercentageInRevenue;
                #endregion
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }


        }
        public GenericDrillDownChartsChartsBO GetCustomerServiceDetails(GlobalFilter filters, string userId)
        {
            AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
            GenericDrillDownChartsChartsBO response = new GenericDrillDownChartsChartsBO();
            GenericColumnChartCategoryBM total = new GenericColumnChartCategoryBM();

            GenericDrillDownColumnChartCategoryBM totalCasesSold = new GenericDrillDownColumnChartCategoryBM();
            GenericDrillDownColumnChartCategoryBM totalProfit = new GenericDrillDownColumnChartCategoryBM();
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
                var dataSetResultTotal = base.ReadToDataSetViaProcedure("BI_PF_CustomerServiceDetailReport", parameterList.ToArray());
                var resultTotal = dataSetResultTotal.Tables[0]
                        .AsEnumerable()
                             .Select(x => new ProfitablityBarChartData
                             {
                                  Commodity = !string.IsNullOrEmpty(x.Field<string>("Commodity")) ? x.Field<string>("Commodity").Trim() : "",
                                 Category = !string.IsNullOrEmpty(x.Field<string>("Category")) ? x.Field<string>("Category").Trim() : "",
                                 Profit = x.Field<decimal?>("Profit").Value,
                                 AssignedSalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("AssignedPersonCode")) ? x.Field<string>("AssignedPersonCode").Trim() : "",
                                 SalesPersonDescription = !string.IsNullOrEmpty(x.Field<string>("AssignedPersonDescription")) ? x.Field<string>("AssignedPersonDescription").Trim() : "",
                                 Period = !string.IsNullOrEmpty(x.Field<string>("Period")) ? x.Field<string>("Period").Trim() : "",
                                 AddUser= !string.IsNullOrEmpty(x.Field<string>("adduser")) ? x.Field<string>("adduser").Trim() : "",
                             })
                .ToList();

                resultTotal = resultTotal.Where(f => f.SalesPersonCode != "DUMP" && f.SalesPersonCode != "DONA").ToList();
                CasesSoldRevenueDataProvider casesSoldRevenuDataProvide = new CasesSoldRevenueDataProvider();

                //  var userAccessibleCategories = _adminUserManagementDateProvider.GetUserDetails(userId);


                totalProfit.All = MappingToCusomerServiceGenericListProfit(resultTotal, filters, null);

                response.TotalRevenue = totalProfit;
                    //response.TotalCasesSold = totalCasesSold;
                #endregion

                //   var responseWithDiffPercentageInRevenue = CaluclateDifference(response);
                return response;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
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
        private decimal CalculateChange(decimal previous, decimal current)
        {
            return UtilityExtensions.CalculateChangeInDecimal(previous, current);
        }
        private double DoubleToPercentageString(decimal d)
        {
            return (double)(Math.Round(d, 2) * 100);
        }
        private List<GenericDrillDownColumnChartBM> MappingToGenericListProfit(List<ProfitablityBarChartData> list, GlobalFilter filters, UserDetails userAccessibleCategories)
        {
            List<GenericDrillDownColumnChartBM> result = new List<GenericDrillDownColumnChartBM>();

            var listCurrent = list.Where(x => x.Period.ToLower() == "current").OrderByDescending(s => s.Profit).ToList();
            var listPrior = list.Where(x => x.Period.ToLower() == "prior").OrderByDescending(s => s.Profit).ToList();
            var listHistorical = list.Where(x => x.Period.ToLower() == "historical").OrderByDescending(s => s.Profit).ToList();


            var data = new GenericDrillDownColumnChartBM
            {
                GroupName = "",
                Label1 = filters.Periods.Current.Label,

                rValue1 = (listCurrent.Where(f => f.Commodity.ToLower() == "grocery").Sum(t => t.Profit)),
                Period1 = filters.Periods.Current.End,
                Color1 = ChartColorBM.GrocerryCurrent,
                SalesPerson1 = "",

                Label2 = filters.Periods.Current.Label,

                rValue2 = (listCurrent.Where(f => f.Commodity.ToLower() == "produce").Sum(t => t.Profit)),
                Period2 = filters.Periods.Current.End,
                Color2 = ChartColorBM.ProduceCurrent,
                SalesPerson2 = "",

                Label3 = filters.Periods.Historical.Label,

                rValue3 = listHistorical.Where(f => f.Commodity.ToLower() == "grocery").Sum(t => t.Profit),
                Period3 = filters.Periods.Historical.End,
                Color3 = ChartColorBM.GrocerryPrevious,
                SalesPerson3 = "",

                Label4 = filters.Periods.Historical.Label,

                rValue4 = (listHistorical.Where(f => f.Commodity.ToLower() == "produce").Sum(t => t.Profit)),
                Period4 = filters.Periods.Historical.End,
                Color4 = ChartColorBM.ProducePrevious,
                SalesPerson4 = "",

                Label5 = filters.Periods.Prior.Label,

                rValue5 = (listPrior.Where(f => f.Commodity.ToLower() == "grocery").Sum(t => t.Profit)),
                Period5 = filters.Periods.Prior.End,
                Color5 = ChartColorBM.GrocerryCurrent,
                SalesPerson5 = "",

                Label6 = filters.Periods.Prior.Label,

                rValue6 = (listPrior.Where(f => f.Commodity.ToLower() == "produce").Sum(t => t.Profit)),
                Period6 = filters.Periods.Prior.End,
                Color6 = ChartColorBM.ProduceCurrent,
                SalesPerson6 = "",

                SubData = list.OrderByDescending(a => a.Profit).Where(x => !userAccessibleCategories.IsRestrictedCategoryAccess
               || userAccessibleCategories.Categories.Select(o => o.Name).Contains(x.Category))

                     .GroupBy(x => x.Category)//.Take(5)
                     .Select((sec, secIdx) => new GenericDrillDownColumnChartBM
                     {
                         GroupName = sec.Key.ToSalesCategoryDisplayName(),
                         Label1 = filters.Periods.Current.Label,
                         rValue1 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "current").Sum(t => t.Profit)),
                         Period1 = filters.Periods.Current.End,
                         Color1 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                         SalesPerson1 = "",

                         Label2 = filters.Periods.Current.Label,
                         rValue2 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "current").Sum(t => t.Profit)),
                         Period2 = filters.Periods.Current.End,
                         Color2 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                         SalesPerson2 = "",

                         Label3 = filters.Periods.Historical.Label,

                         rValue3 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "historical").Sum(t => t.Profit)),
                         Period3 = filters.Periods.Historical.End,
                         Color3 = BarColumnChartDistinctColors.Colors[secIdx].SecondaryGrocery,//ChartColorBM.GrocerryPrevious,
                         SalesPerson3 = "",

                         Label4 = filters.Periods.Historical.Label,

                         rValue4 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "historical").Sum(t => t.Profit)),
                         Period4 = filters.Periods.Historical.End,
                         Color4 = BarColumnChartDistinctColors.Colors[secIdx].SecondaryProduce,//ChartColorBM.ProducePrevious,
                         SalesPerson4 = "",

                         Label5 = filters.Periods.Prior.Label,

                         rValue5 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "prior").Sum(t => t.Profit)),
                         Period5 = filters.Periods.Prior.End,
                         Color5 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                         SalesPerson5 = "",

                         Label6 = filters.Periods.Prior.Label,

                         rValue6 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "prior").Sum(t => t.Profit)),
                         Period6 = filters.Periods.Prior.End,
                         Color6 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                         SalesPerson6 = "",

                         SubData = sec.Where(w => w.Category == sec.Key).OrderByDescending(a => a.Profit)
                             .GroupBy(x => x.AssignedSalesPersonCode).Where(p => p.Any(x => x.Period == "current"))
                             //.Take(5)
                             .Select((thir, thirIdx) => new GenericDrillDownColumnChartBM
                             {
                                 GroupName = GetSalemanNameFromGroup(thir),
                                 //  GetActiveSalesManName(!string.IsNullOrEmpty(thir.First().SalesPersonDescription) ? thir.First().SalesPersonDescription : thir.Key),
                                 ActiveSalesPersonCode = thir.Key,
                                 Label1 = filters.Periods.Current.Label,


                                 rValue1 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "current").Sum(t => t.Profit)),
                                 Period1 = filters.Periods.Current.End,
                                 Color1 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,
                                 SalesPerson1 = GetSalemanNameFromGroup(thir),
                                 //(thir.First().SalesPersonDescription),

                                 Label2 = filters.Periods.Current.Label,

                                 rValue2 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "current").Sum(t => t.Profit)),
                                 Period2 = filters.Periods.Current.End,
                                 Color2 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,
                                 SalesPerson2 = (thir.First().SalesPersonDescription),

                                 Label3 = filters.Periods.Historical.Label,

                                 rValue3 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "historical").Sum(t => t.Profit)),
                                 Period3 = filters.Periods.Historical.End,
                                 Color3 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryGrocery,
                                 SalesPerson3 = (thir.First().SalesPersonDescription),

                                 Label4 = filters.Periods.Historical.Label,

                                 rValue4 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "historical").Sum(t => t.Profit)),

                                 Period4 = filters.Periods.Current.End,
                                 Color4 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryProduce,
                                 SalesPerson4 = (thir.First().SalesPersonDescription),

                                 Label5 = filters.Periods.Prior.Label,

                                 rValue5 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "prior").Sum(t => t.Profit)),
                                 Period5 = filters.Periods.Prior.End,
                                 Color5 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,
                                 SalesPerson5 = (thir.First().SalesPersonDescription),

                                 Label6 = filters.Periods.Prior.Label,

                                 rValue6 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "prior").Sum(t => t.Profit)),
                                 Period6 = filters.Periods.Prior.End,
                                 Color6 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,
                                 SalesPerson6 = (thir.First().SalesPersonDescription),

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

        private List<GenericDrillDownColumnChartBM> MappingToCusomerServiceGenericListProfit(List<ProfitablityBarChartData> list, GlobalFilter filters, UserDetails userAccessibleCategories)
        {
            List<GenericDrillDownColumnChartBM> result = new List<GenericDrillDownColumnChartBM>();
            list = list.OrderByDescending(s => s.Profit).ToList();


            var data = new GenericDrillDownColumnChartBM
            {
                GroupName = "",
                Label1 = filters.Periods.Current.Label,
                rValue1 = (list.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "current").Sum(t => t.Profit)),
                Period1 = filters.Periods.Current.End,
                Color1 = ChartColorBM.GrocerryCurrent,
                SalesPerson1 = "",

                Label2 = filters.Periods.Current.Label,
                rValue2 = (list.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "current").Sum(t => t.Profit)),
                Period2 = filters.Periods.Current.End,
                Color2 = ChartColorBM.ProduceCurrent,
                SalesPerson2 = "",

                Label3 = filters.Periods.Historical.Label,
                rValue3 = (list.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "historical").Sum(t => t.Profit)),
                Period3 = filters.Periods.Historical.End,
                Color3 = ChartColorBM.GrocerryPrevious,
                SalesPerson3 = "",

                Label4 = filters.Periods.Historical.Label,
                rValue4 = (list.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "historical").Sum(t => t.Profit)),
                Period4 = filters.Periods.Historical.End,
                Color4 = ChartColorBM.ProducePrevious,
                SalesPerson4 = "",

                Label5 = filters.Periods.Prior.Label,
                rValue5 = (list.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "prior").Sum(t => t.Profit)),
                Period5 = filters.Periods.Prior.End,
                Color5 = ChartColorBM.GrocerryCurrent,
                SalesPerson5 = "",

                Label6 = filters.Periods.Prior.Label,
                rValue6 = (list.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "prior").Sum(t => t.Profit)),
                Period6 = filters.Periods.Prior.End,
                Color6 = ChartColorBM.ProduceCurrent,
                SalesPerson6 = "",



                SubData = list.OrderByDescending(a => a.Profit)
                   
                    .GroupBy(x => x.AddUser)//.Take(5)
                    .Take(10)
                    .Select((sec, secIdx) => new GenericDrillDownColumnChartBM
                    {
                        GroupName = sec.Key.ToSalesCategoryDisplayName(),
                        Label1 = filters.Periods.Current.Label,
                        rValue1 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "current").Sum(t => t.Profit)),
                        Period1 = filters.Periods.Current.End,
                        Color1 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                        SalesPerson1 = "",

                        Label2 = filters.Periods.Current.Label,
                        rValue2 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "current").Sum(t => t.Profit)),
                        Period2 = filters.Periods.Current.End,
                        Color2 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                        SalesPerson2 = "",

                        Label3 = filters.Periods.Historical.Label,
                        rValue3 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "historical").Sum(t => t.Profit)),
                        Period3 = filters.Periods.Historical.End,
                        Color3 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].SecondaryGrocery,//ChartColorBM.GrocerryPrevious,
                        SalesPerson3 = "",

                        Label4 = filters.Periods.Historical.Label,
                        rValue4 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "historical").Sum(t => t.Profit)),
                        Period4 = filters.Periods.Historical.End,
                        Color4 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].SecondaryProduce,//ChartColorBM.ProducePrevious,
                        SalesPerson4 = "",

                        Label5 = filters.Periods.Prior.Label,
                        rValue5 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "prior").Sum(t => t.Profit)),
                        Period5 = filters.Periods.Prior.End,
                        Color5 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                        SalesPerson5 = "",

                        Label6 = filters.Periods.Prior.Label,
                        rValue6 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "prior").Sum(t => t.Profit)),
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

        #endregion
    }
}
