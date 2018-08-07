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
    public class DashBoardDataProvider : DataAccessADO
    {
        #region OPEX
        public async Task<DashboardStatistics> GetOpexTopBoxValues(GlobalFilter filters)
        {
            DashboardStatistics opexStatistics = new DashboardStatistics();
            try
            {
                var dataTable = new DataTable();
                using (var adoDataAccess = new DataAccessADO())
                {
                    List<SqlParameter> parameterList = new List<SqlParameter>();

                    parameterList.Add(new SqlParameter("@startDate", filters.Periods.Current.Start));
                    parameterList.Add(new SqlParameter("@endDate", filters.Periods.Current.End));
                    parameterList.Add(new SqlParameter("@previousStart", filters.Periods.Historical.Start));
                    parameterList.Add(new SqlParameter("@previousEnd", filters.Periods.Historical.End));
                    parameterList.Add(new SqlParameter("@opexAccStart", Constants.OpexAccountStart));
                    parameterList.Add(new SqlParameter("@opexAccEnd", Constants.OpexAccountEnd));


                    var dataSetResult = base.ReadToDataSetViaProcedure("BI_EP_GetDashboardStatistics", parameterList.ToArray());
                    dataTable = dataSetResult.Tables[0];

                    DataRow currentRow = dataTable.NewRow();
                    DataRow priorRow = dataTable.NewRow();

                    if (dataTable.Rows.Count > 0)
                    {
                        currentRow = dataTable.Rows[0];
                        priorRow = dataTable.Rows[1];
                    }
                    double current = (currentRow != null) ? double.Parse(currentRow[1].ToString()).ToScaleDownAndRoundTwoDigits() : 0;
                    double prior = (priorRow != null) ? double.Parse(priorRow[1].ToString()).ToScaleDownAndRoundTwoDigits() : 0;

                    opexStatistics = new DashboardStatistics
                    {
                        Current = current,
                        Prior = prior,
                        Change = Math.Round(UtilityExtensions.DoubleToPercentageString(UtilityExtensions.CalculateChange(prior, current))),
                        Name = "operational expenses"
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching data from database - " + ex.Message);
            }
            return opexStatistics;
        }
        #endregion

        #region Profit, Non Commodity
        public async Task<DashboardStatistics> GetProfitTopBoxValues(GlobalFilter filters)
        {


            var dataTable = new DataTable();
            DashboardStatistics profitStatistics = new DashboardStatistics();
            try
            {
                using (var adoDataAccess = new DataAccessADO())
                {
                    List<SqlParameter> parameterList = new List<SqlParameter>();

                    parameterList.Add(new SqlParameter("@startDate", filters.Periods.Current.Start));
                    parameterList.Add(new SqlParameter("@endDate", filters.Periods.Current.End));
                    parameterList.Add(new SqlParameter("@previousStart", filters.Periods.Historical.Start));
                    parameterList.Add(new SqlParameter("@previousEnd", filters.Periods.Historical.End));

                    var dataSetResult = base.ReadToDataSetViaProcedure("BI_PF_GetDashboardStatistics", parameterList.ToArray());
                    dataTable = dataSetResult.Tables[0];

                    DataRow currentRow = dataTable.NewRow();
                    DataRow priorRow = dataTable.NewRow();
                    currentRow = dataTable.Rows[0];
                    priorRow = dataTable.Rows[1];

                    double current = (currentRow != null) ? double.Parse(currentRow[1].ToString()).ToScaleDownAndRoundTwoDigits() : 0;
                    double prior = (priorRow != null) ? double.Parse(priorRow[1].ToString()).ToScaleDownAndRoundTwoDigits() : 0;

                    profitStatistics = new DashboardStatistics
                    {
                        Current = current,
                        Prior = prior,
                        Change = Math.Round(UtilityExtensions.DoubleToPercentageString(UtilityExtensions.CalculateChange(prior, current))),
                        Name = "Gross Profit"
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching data from database - " + ex.Message);
            }
            return profitStatistics;
        }

        public async Task<List<DashboardStatistics>> GetNonCommoditySalesAndCasesSoldTopBoxValues(GlobalFilter filters)
        {


            var dataTable = new DataTable();
            List<DashboardStatistics> nonCommodityStatistics = new List<DashboardStatistics>();

            try
            {
                using (var adoDataAccess = new DataAccessADO())
                {
                    List<SqlParameter> parameterList = new List<SqlParameter>();

                    parameterList.Add(new SqlParameter("@startDate", filters.Periods.Current.Start));
                    parameterList.Add(new SqlParameter("@endDate", filters.Periods.Current.End));
                    parameterList.Add(new SqlParameter("@previousStart", filters.Periods.Historical.Start));
                    parameterList.Add(new SqlParameter("@previousEnd", filters.Periods.Historical.End));

                    var dataSetResult = base.ReadToDataSetViaProcedure("BI_NC_GetDashboardStatistics", parameterList.ToArray());
                    dataTable = dataSetResult.Tables[0];

                    DataRow currentRow = dataTable.NewRow();
                    DataRow priorRow = dataTable.NewRow();


                    currentRow = dataTable.Rows[0];
                    priorRow = dataTable.Rows[1];

                    double currentCasesSold = (currentRow != null) ? double.Parse(currentRow[1].ToString()).ToScaleDownAndRoundTwoDigits() : 0;
                    double priorCasesSold = (priorRow != null) ? double.Parse(priorRow[1].ToString()).ToScaleDownAndRoundTwoDigits() : 0;

                    double currentSales = (currentRow != null) ? double.Parse(currentRow[2].ToString()).ToScaleDownAndRoundTwoDigits() : 0;
                    double priorSales = (priorRow != null) ? double.Parse(priorRow[2].ToString()).ToScaleDownAndRoundTwoDigits() : 0;

                    DashboardStatistics salesStatistics = new DashboardStatistics();
                    DashboardStatistics casesSoldStatistics = new DashboardStatistics();

                    salesStatistics = new DashboardStatistics
                    {
                        Current = currentSales,
                        Prior = priorSales,
                        Change = Math.Round(UtilityExtensions.DoubleToPercentageString(UtilityExtensions.CalculateChange(priorSales, currentSales))),
                        Name = "Non-Commodity Revenue"
                    };


                    casesSoldStatistics = new DashboardStatistics
                    {
                        Current = currentCasesSold,
                        Prior = priorCasesSold,
                        Change = Math.Round(UtilityExtensions.DoubleToPercentageString(UtilityExtensions.CalculateChange(priorCasesSold, currentCasesSold))),
                        Name = "Non-Commodity Cases Sold"
                    };

                    nonCommodityStatistics.Add(salesStatistics);
                    nonCommodityStatistics.Add(casesSoldStatistics);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching data from database - " + ex.Message);
            }
            return nonCommodityStatistics;
        }
        #endregion
    }
}
