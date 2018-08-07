using Nogales.BusinessModel;
using Nogales.DataProvider.Infrastructure;
using Nogales.DataProvider.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;

namespace Nogales.DataProvider
{
    public class CommonDataProvider : DataAccessADO
    {
   
        public async Task<List<DashboardStatisticsBM>> GetDashboardStatics(GlobalFilter filters)  // Data to populate the statistic boxes in the dashboard
        {

            var dataTable = new DataTable();
            var expenseDataTable = new DataTable();
            var nonComodityDataTable = new DataTable();

            using (var adoDataAccess = new DataAccessADO())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startDate", filters.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@endDate", filters.Periods.Current.End));
                if (filters.Periods.Prior.Start.Year == 1900)
                {
                    parameterList.Add(new SqlParameter("@previousStart", filters.Periods.Historical.Start));
                    parameterList.Add(new SqlParameter("@previousEnd", filters.Periods.Historical.End));
                }
                else
                {
                    parameterList.Add(new SqlParameter("@previousStart", filters.Periods.Historical.Start));
                    parameterList.Add(new SqlParameter("@previousEnd", filters.Periods.Historical.End));
                }
                //parameterList.Add(new SqlParameter("@previousStart", filters.Periods.Prior.Start));
                //parameterList.Add(new SqlParameter("@previousEnd", filters.Periods.Prior.End));
                parameterList.Add(new SqlParameter("@cogAccStart", Constants.CogsAccountStart));
                parameterList.Add(new SqlParameter("@cogAccEnd", Constants.CogsAccountEnd));
                parameterList.Add(new SqlParameter("@opexAccStart", Constants.OpexAccountStart));
                parameterList.Add(new SqlParameter("@opexAccEnd", Constants.OpexAccountEnd));

                var dataSetResult = base.ReadToDataSetViaProcedure("BI_GetDashboardStatics", parameterList.ToArray());

                dataTable = dataSetResult.Tables[0];
                expenseDataTable = dataSetResult.Tables[1];
                nonComodityDataTable = dataSetResult.Tables[2];
            }

            DataTable dt = new DataTable();
            DataRow drCur = dt.NewRow();
            DataRow drPrev = dt.NewRow();

            DataRow exCur = dt.NewRow();
            DataRow exPrev = dt.NewRow();

            DataRow nonComCur = dt.NewRow();
            DataRow nonComPrev = dt.NewRow();

            if (dataTable.Rows.Count > 0)
            {
                drCur = dataTable.Rows[0];
                drPrev = dataTable.Rows[1];
                //drCur = dataTable.Rows[0][0].ToString() == DateTime.Now.Month.ToString() ? dataTable.Rows[0] : (dataTable.Rows.Count > 1) ? dataTable.Rows[1] : null;
                //drPrev = dataTable.Rows[0][0].ToString() == DateTime.Now.AddMonths(-1).Month.ToString() ? dataTable.Rows[0] : (dataTable.Rows.Count > 1) ? dataTable.Rows[1] : null;
            }
            else
            {
                drCur = null;
                drPrev = null;
            }
            if (expenseDataTable.Rows.Count > 0)
            {
                exCur = expenseDataTable.Rows[0];
                exPrev = expenseDataTable.Rows[1];

                //exCur = expenseDataTable.Rows[0][0].ToString() == DateTime.Now.Month.ToString() ? expenseDataTable.Rows[0] : (expenseDataTable.Rows.Count > 1) ? expenseDataTable.Rows[1] : null;
                //exPrev = expenseDataTable.Rows[0][0].ToString() == DateTime.Now.AddMonths(-1).Month.ToString() ? expenseDataTable.Rows[0] : (expenseDataTable.Rows.Count > 1) ? expenseDataTable.Rows[1] : null;
            }
            else
            {
                exCur = null;
                exPrev = null;
            }
            if (nonComodityDataTable.Rows.Count > 0)
            {
                nonComCur = nonComodityDataTable.Rows[0][0].ToString() == DateTime.Now.Month.ToString() ? nonComodityDataTable.Rows[0] : (nonComodityDataTable.Rows.Count > 1) ? nonComodityDataTable.Rows[1] : null;
                nonComPrev = nonComodityDataTable.Rows[0][0].ToString() == DateTime.Now.AddMonths(-1).Month.ToString() ? nonComodityDataTable.Rows[0] : (nonComodityDataTable.Rows.Count > 1) ? nonComodityDataTable.Rows[1] : null;
            }
            else
            {
                nonComCur = null;
                nonComPrev = null;
            }


            var data = new
            {

                //CasesSold = (drCur != null) ? double.Parse(drCur[1].ToString()).ToScaleDownAndRoundTwoDigits() : 0,
                //PreviousCasesSold = (drPrev != null) ? double.Parse(drPrev[1].ToString()).ToScaleDownAndRoundTwoDigits() : 0,

                Expense = (exCur != null) ? double.Parse(exCur[1].ToString()).ToScaleDownAndRoundTwoDigits() : 0,
                PreviousExpense = (exPrev != null) ? double.Parse(exPrev[1].ToString()).ToScaleDownAndRoundTwoDigits() : 0,

                GrossProfit = (drCur != null) ? double.Parse(drCur[1].ToString()).ToScaleDownAndRoundTwoDigits() : 0,
                PreviousGrossProfit = (drPrev != null) ? double.Parse(drPrev[1].ToString()).ToScaleDownAndRoundTwoDigits() : 0,

                //Sales = (drCur != null) ? double.Parse(drCur[2].ToString()).ToScaleDownAndRoundTwoDigits() : 0,
                //PreviousSales = (drPrev != null) ? double.Parse(drPrev[2].ToString()).ToScaleDownAndRoundTwoDigits() : 0,


                NonComodityRevenue = (nonComCur != null) ? double.Parse(nonComCur[1].ToString()).ToScaleDownAndRoundTwoDigits() : 0,
                NonComodityPrevious = (nonComPrev != null) ? double.Parse(nonComPrev[1].ToString()).ToScaleDownAndRoundTwoDigits() : 0,

            };

            var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(filters.Periods.Current.Start.Month).Substring(0, 3);
            var PreviousMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(filters.Periods.Prior.Start.Month).Substring(0, 3);

            var result = new List<DashboardStatisticsBM>();
            //  result.Add(new DashboardStatisticsBM() { Month = monthName, PreviousMonth = PreviousMonth, Name = "Cases Sold", Amount = data.CasesSold, PreviousMonthAmount = data.PreviousCasesSold, Change = DoubleToPercentageString(CalculateChange(data.PreviousCasesSold, data.CasesSold)) });
            result.Add(new DashboardStatisticsBM() { Month = monthName, PreviousMonth = PreviousMonth, Name = "operational expenses", Amount = data.Expense, PreviousMonthAmount = data.PreviousExpense, Change = DoubleToPercentageString(UtilityExtensions.CalculateChange(data.PreviousExpense, data.Expense)) });
            result.Add(new DashboardStatisticsBM() { Month = monthName, PreviousMonth = PreviousMonth, Name = "Gross Profit", Amount = data.GrossProfit, PreviousMonthAmount = data.PreviousGrossProfit, Change = DoubleToPercentageString(UtilityExtensions.CalculateChange(data.PreviousGrossProfit, data.GrossProfit)) });
            // result.Add(new DashboardStatisticsBM() { Month = monthName, PreviousMonth = PreviousMonth, Name = "Sales", Amount = data.Sales, PreviousMonthAmount = data.PreviousSales, Change = DoubleToPercentageString(CalculateChange(data.PreviousSales, data.Sales)) });
            result.Add(new DashboardStatisticsBM() { Month = monthName, PreviousMonth = PreviousMonth, Name = "Non Commodity Revenue", Amount = data.NonComodityRevenue, PreviousMonthAmount = data.NonComodityPrevious, Change = DoubleToPercentageString(UtilityExtensions.CalculateChange(data.NonComodityPrevious, data.NonComodityRevenue)) });
            return result;
        }

        internal double CalculateChange(double previous, double current)
        {
            if (previous == 0)
                return 0;

            var change = current - previous;
            return (previous < 0) ? 0 : (double)change / Math.Abs(previous);
        }

        internal double DoubleToPercentageString(double d)
        {
            return (Math.Round(d, 4) * 100);
        }
    }
}
