using Nogales.BusinessModel;
using Nogales.DataProvider.Infrastructure;
using Nogales.DataProvider.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.DataProvider
{
    public class ItemProvider : DataAccessADO
    {
        public List<ItemReportByItemMapperBO> GetItemReportByItem(string currentStart,string currentEnd,string item, int week,int month,int year)
        {
            using (var adoDataAccess = new DataAccessADO())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startdate", currentStart));
                parameterList.Add(new SqlParameter("@enddate", currentEnd));

                parameterList.Add(new SqlParameter("@item", item));
                parameterList.Add(new SqlParameter("@week", week));
                parameterList.Add(new SqlParameter("@month", month));
                parameterList.Add(new SqlParameter("@year", year));

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_GetAvgCostByWeekByItem", parameterList.ToArray());
                var result = dataTableResult.Tables[0]
                                            .AsEnumerable()
                                            .Select(x => new ItemReportByItemMapperBO
                                            {
                                                Item = x.Field<string>("Item"),
                                                InvoiceDate = x.Field<DateTime>("invdte"),
                                                InvoiceNumber = x.Field<string>("invno"),
                                                CustName = x.Field<string>("company"),
                                                CustNumber = x.Field<string>("custno"),
                                                Cost = (x.Field<decimal?>("Cost") ?? 0).ToRoundTwoDecimalDigits().ToStripDecimal(),
                                                Price = (x.Field<decimal?>("Price") ?? 0).ToRoundTwoDecimalDigits().ToStripDecimal(),
                                                BinNo=(x.Field<string>("binno")),
                                                QtyShipped= (x.Field<decimal>("qtyshp")),
                                                UnitOfMeasure= (x.Field<string>("umeasur"))
                                            }).ToList();
                return result;
            }
        }

        public GenericItemMapperBM GetItemReport(string currentStart,
            string currentEnd, List<string> item
                                                         )
        {
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("Code");
            foreach (string code in item)
            {
                dtItem.Rows.Add(code);
            }
            GenericItemMapperBM response = new GenericItemMapperBM();
            using (var adoDataAccess = new DataAccessADO())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                // parameterList.Add(new SqlParameter("@salesman", salesPerson != null ? string.Join(",", salesPerson) : ""));
                var param = new SqlParameter();
                param.SqlDbType = SqlDbType.Structured;
                param.Value = dtItem;
                param.ParameterName = "@item";
                parameterList.Add(param);

                parameterList.Add(new SqlParameter("@startdate", currentStart));
                parameterList.Add(new SqlParameter("@enddate", currentEnd));


                var dataTableResult = base.ReadToDataSetViaProcedure("BI_GetAvgCostByWeek", parameterList.ToArray());

                //string sqlString = SQLQueries.GetCasesSoldReportQuery(currentStart, currentEnd
                //                                    , previousStart, previousEnd, comodity, category, minSalesAmt);
                //var dataTableResult = adoDataAccess.ReadToDataSet(sqlString);
                var result = dataTableResult.Tables[0]
                                            .AsEnumerable()
                                            .Select(x => new ItemReportMapperBO
                                            {
                                                Item = x.Field<string>("Item"),
                                                ItemDesc = x.Field<string>("ItemDesc"),
                                                Week = x.Field<int>("Week"),
                                                Month = x.Field<int>("Month"),
                                                Year = x.Field<int>("Year"),
                                                Cost = (x.Field<decimal?>("Cost") ?? 0).ToRoundTwoDecimalDigits().ToStripDecimal(),
                                                Price = (x.Field<decimal?>("Price") ?? 0).ToRoundTwoDecimalDigits().ToStripDecimal()
                                            }).ToList();
                response.ReportData = result.OrderBy(x=>x.ItemDesc).ToList();
                response.ListItem = result.ToList().Where(x => !string.IsNullOrEmpty(x.Item)).GroupBy(x => x.Item)
                                     .Select(x => new KeyValuePair<string, string>(x.Key.Trim(), x.First().ItemDesc.Trim() + " | " + x.Key.Trim())
                                     {
                                     })
                                     .OrderBy(x => x.Key)
                                     .ToList();

                return response;
            }
        }

        public ItemMapper GetAllItems()
        {
            ItemMapper result = new ItemMapper();
            List<SqlParameter> parameterList = new List<SqlParameter>();

            var data = base.ReadToDataSetViaProcedure("BI_GetAllItems", parameterList.ToArray());
            if (data != null && data.Tables[0] != null && data.Tables[0].AsEnumerable() != null && data.Tables[0].AsEnumerable().Count() > 0)
            {
                var itemsList = data.Tables[0].AsEnumerable()

                                     .Where(x => !string.IsNullOrEmpty(x.Field<string>(0)))
                                     .Select(x => new KeyValuePair<string, string>(x.Field<string>(0), x.Field<string>(1))
                                     {
                                     })
                                     .OrderBy(x => x.Key)
                                     .ToList();
                result.ListItem = itemsList;
            }
            return result;
        }

        public ItemMapper GetItems(string item)
        {
            ItemMapper result = new ItemMapper();
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@item", item));
            var data = base.ReadToDataSetViaProcedure("BI_GetItems", parameterList.ToArray());
            if (data != null && data.Tables[0] != null && data.Tables[0].AsEnumerable() != null && data.Tables[0].AsEnumerable().Count() > 0)
            {
                var itemsList = data.Tables[0].AsEnumerable()

                                     .Where(x => !string.IsNullOrEmpty(x.Field<string>(0)))
                                     .Select(x => new KeyValuePair<string, string>(x.Field<string>(0), x.Field<string>(1))
                                     {
                                     })
                                     .OrderBy(x => x.Key)
                                     .ToList();
                result.ListItem = itemsList;
            }
            return result;
        }

        public List<ItemComparisonReportBO> GetItemComparisonReport(int filter, string comodity, List<string> item)
        {
            List<ItemComparisonReportBO> Response = new List<ItemComparisonReportBO>();
            List<ItemComparisonReport> currentWeekList = new List<ItemComparisonReport>();
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("Code");
            foreach (string code in item)
            {
                dtItem.Rows.Add(code);
            }
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var filterdata = filterLists.Where(d => d.Id == filter).FirstOrDefault();
            ItemMapper result = new ItemMapper();
            List<SqlParameter> parameterList = new List<SqlParameter>();
            var param = new SqlParameter();
            param.SqlDbType = SqlDbType.Structured;
            param.Value = dtItem;
            param.ParameterName = "@item";
            parameterList.Add(param);
            parameterList.Add(new SqlParameter("@currentStart", filterdata.Periods.Current.Start));
            parameterList.Add(new SqlParameter("@currentEnd", filterdata.Periods.Current.End));
            parameterList.Add(new SqlParameter("@historicalStart", filterdata.Periods.Historical.Start));
            parameterList.Add(new SqlParameter("@historicalEnd", filterdata.Periods.Historical.End));
            parameterList.Add(new SqlParameter("@priorStart", filterdata.Periods.Prior.Start));
            parameterList.Add(new SqlParameter("@priorEnd", filterdata.Periods.Prior.End));
            parameterList.Add(new SqlParameter("@comodity", !string.IsNullOrEmpty(comodity) ? comodity.ToLower() : ""));
            var data = base.ReadToDataSetViaProcedure("BI_GetItemComparisonReport", parameterList.ToArray());
            if (data != null && data.Tables[0] != null && data.Tables[0].AsEnumerable() != null && data.Tables[0].AsEnumerable().Count() > 0)
            {
                currentWeekList = data.Tables[0].AsEnumerable()
                                                        .Select(x => new ItemComparisonReport
                                                        {
                                                            Comodity = x.Field<string>("comodity"),
                                                            Item = x.Field<string>("item"),
                                                            ItemName = x.Field<string>("ItemName"),
                                                            Period = x.Field<string>("period"),
                                                            Quantity = (int)x.Field<int>("quantity"),
                                                            Revenue = x.Field<decimal>("revenue"),
                                                            Cost = x.Field<decimal>("cost"),
                                                            //Margin = x.Field<decimal>("margin") ?? Convert.ToDecimal(0d),
                                                        })
                                                        .OrderBy(x=>x.ItemName)
                                     .ToList();

            }



            Response = ProcessLists(currentWeekList);
            return Response;
        }

        public List<ItemVendorComparisonReportBO> GetItemVendorCostReport(int filter, string comodity, List<string> item)
        {
            List<ItemVendorComparisonReportBO> Response = new List<ItemVendorComparisonReportBO>();
            List<ItemVendorComparisonReport> currentWeekList = new List<ItemVendorComparisonReport>();
            DataTable dtItem = new DataTable();
            dtItem.Columns.Add("Code");
            foreach (string code in item)
            {
                dtItem.Rows.Add(code);
            }
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var filterdata = filterLists.Where(d => d.Id == filter).FirstOrDefault();
            ItemMapper result = new ItemMapper();
            List<SqlParameter> parameterList = new List<SqlParameter>();
            var param = new SqlParameter();
            param.SqlDbType = SqlDbType.Structured;
            param.Value = dtItem;
            param.ParameterName = "@item";
            parameterList.Add(param);
            parameterList.Add(new SqlParameter("@currentStart", filterdata.Periods.Current.Start));
            parameterList.Add(new SqlParameter("@currentEnd", filterdata.Periods.Current.End));
            parameterList.Add(new SqlParameter("@historicalStart", filterdata.Periods.Historical.Start));
            parameterList.Add(new SqlParameter("@historicalEnd", filterdata.Periods.Historical.End));
            parameterList.Add(new SqlParameter("@priorStart", filterdata.Periods.Prior.Start));
            parameterList.Add(new SqlParameter("@priorEnd", filterdata.Periods.Prior.End));
            parameterList.Add(new SqlParameter("@comodity", !string.IsNullOrEmpty(comodity) ? comodity.ToLower() : ""));
            var data = base.ReadToDataSetViaProcedure("BI_GetItemVendorComparisonReport", parameterList.ToArray());
            if (data != null && data.Tables[0] != null && data.Tables[0].AsEnumerable() != null && data.Tables[0].AsEnumerable().Count() > 0)
            {
                currentWeekList = data.Tables[0].AsEnumerable()
                                                        .Select(x => new ItemVendorComparisonReport
                                                        {
                                                            Comodity = x.Field<string>("comodity"),
                                                            Item = x.Field<string>("item"),
                                                            ItemName = x.Field<string>("ItemName"),
                                                            Period = x.Field<string>("period"),
                                                            Vendor = x.Field<string>("vendno"),
                                                            VendorName = x.Field<string>("company"),
                                                            Quantity = (int)x.Field<int>("quantity"),
                                                            Revenue = 0,
                                                            Cost = x.Field<decimal>("cost")
                                                           
                                                        })
                                                        .OrderBy(x=>x.ItemName)
                                     .ToList();

            }



            Response = ProcessVendorLists(currentWeekList);
            return Response;
        }

        private List<ItemVendorComparisonReportBO> ProcessVendorLists(List<ItemVendorComparisonReport> weekList)
        {
            List<ItemVendorComparisonReportBO> result = new List<ItemVendorComparisonReportBO>();

            try
            {
                result = weekList.GroupBy(h => new { h.Comodity, h.Item, h.Vendor }).Select(f => new ItemVendorComparisonReportBO
                {
                    Item = !string.IsNullOrEmpty(f.Key.Item) ? f.Key.Item.Trim() : "",
                    ItemName = !string.IsNullOrEmpty(f.First().ItemName) ? f.First().ItemName.Trim() : "",
                    Vendor = !string.IsNullOrEmpty(f.Key.Vendor) ? f.Key.Vendor.Trim() : "",
                    VendorName = !string.IsNullOrEmpty(f.Key.Vendor) ? f.Max(h => h.VendorName).Trim() : "",
                    Margin=Math.Round( f.Where(g => g.Period == "current").Sum(w => w.Quantity)-(f.Where(g => g.Period == "current").FirstOrDefault() != null ? f.Where(g => g.Period == "current").First().Cost.ToRoundTwoDecimalDigits().ToStripDecimal() : 0),2,MidpointRounding.AwayFromZero),
                    Comodity = f.Key.Comodity.Trim(),
                    cCurrent = f.Where(g => g.Period == "current").Sum(w => w.Quantity),
                    //cHistorical = f.Where(g => g.Period == "historical").Sum(w => w.Quantity),
                    cPrior = f.Where(g => g.Period == "prior").Sum(w => w.Quantity),

                    costCurrent = f.Where(g => g.Period == "current").FirstOrDefault() != null ? f.Where(g => g.Period == "current").First().Cost.ToRoundTwoDecimalDigits().ToStripDecimal() : 0,
                    //costHistorical = f.Where(g => g.Period == "historical").FirstOrDefault() != null ? f.Where(g => g.Period == "historical").First().Cost.ToRoundTwoDecimalDigits() : 0,
                    costPrior = f.Where(g => g.Period == "prior").FirstOrDefault() != null ? f.Where(g => g.Period == "prior").First().Cost.ToRoundTwoDecimalDigits().ToStripDecimal() : 0,
                    CostDifference = (f.Where(g => g.Period == "current").FirstOrDefault() != null ? f.Where(g => g.Period == "current").First().Cost.ToRoundTwoDecimalDigits() : 0) - (f.Where(g => g.Period == "prior").FirstOrDefault() != null ? f.Where(g => g.Period == "prior").First().Cost.ToRoundTwoDecimalDigits().ToStripDecimal() : 0)

                }).ToList();
            }
            catch (Exception ex)
            { }
            return result;
        }

        private List<ItemComparisonReportBO> ProcessLists(List<ItemComparisonReport> weekList)
        {
            List<ItemComparisonReportBO> result = new List<ItemComparisonReportBO>();

            try
            {
                result = weekList.GroupBy(h => new { h.Comodity, h.Item }).Select(f => new ItemComparisonReportBO
                {
                    Item = !string.IsNullOrEmpty(f.Key.Item) ? f.Key.Item.Trim() : "",
                    ItemName = !string.IsNullOrEmpty(f.First().ItemName) ? f.First().ItemName.Trim() : "",
                    Comodity = f.Key.Comodity.Trim(),
                    //cCurrent = f.Where(g => g.Period == "current").Sum(w => w.Quantity),
                    //cHistorical = f.Where(g => g.Period == "historical").Sum(w => w.Quantity),
                    //cPrior = f.Where(g => g.Period == "prior").Sum(w => w.Quantity),
                    //cYearCurrent = yearList.Where(g => g.Item == f.Key.Item && g.Comodity == f.Key.Comodity && g.Period == "current").Sum(r => r.Quantity),
                    //cYearHistorical = yearList.Where(g => g.Item == f.Key.Item && g.Comodity == f.Key.Comodity && g.Period == "historical").Sum(r => r.Quantity),
                    rCurrent = f.Where(g => g.Period == "current").Sum(w => w.Revenue).ToRoundTwoDecimalDigits().ToStripDecimal(),
                    rHistorical = f.Where(g => g.Period == "historical").Sum(w => w.Revenue).ToRoundTwoDecimalDigits().ToStripDecimal(),
                    rPrior = f.Where(g => g.Period == "prior").Sum(w => w.Revenue).ToRoundTwoDecimalDigits().ToStripDecimal(),
                    //rYearCurrent = yearList.Where(g => g.Item == f.Key.Item && g.Comodity == f.Key.Comodity && g.Period == "current").Sum(r => r.Revenue).ToRoundTwoDecimalDigits(),
                    //rYearHistorical = yearList.Where(g => g.Item == f.Key.Item && g.Comodity == f.Key.Comodity && g.Period == "historical").Sum(r => r.Revenue).ToRoundTwoDecimalDigits(),
                    costCurrent = f.Where(g => g.Period == "current").FirstOrDefault() != null ? f.Where(g => g.Period == "current").First().Cost.ToRoundTwoDecimalDigits().ToStripDecimal() : 0,
                    costHistorical = f.Where(g => g.Period == "historical").FirstOrDefault() != null ? f.Where(g => g.Period == "historical").First().Cost.ToRoundTwoDecimalDigits().ToStripDecimal() : 0,
                    costPrior = f.Where(g => g.Period == "prior").FirstOrDefault() != null ? f.Where(g => g.Period == "prior").First().Cost.ToRoundTwoDecimalDigits().ToStripDecimal() : 0,


                }).ToList();
            }
            catch (Exception ex)
            { }
            return result;
        }


    }
}
