using Nogales.BusinessModel;
using Nogales.DataProvider;
using Nogales.DataProvider.ENUM;
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
    public class CasesSoldSalesDataProvider : DataAccessADO
    {

        #region CasesSold/Sales Drill down Stacked bar chart
        public GenericDrillDownChartsChartsBO GetCasesSoldAndSalesData(GlobalFilter filters, string userId)
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
                parameterList.Add(new SqlParameter("@currentStart", filters.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filters.Periods.Current.End));
                parameterList.Add(new SqlParameter("@historicalStart", filters.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filters.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@priorStart", filters.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filters.Periods.Prior.End));
                var dataSetResultTotal = base.ReadToDataSetViaProcedure("BI_CSSL_GetCasesSoldAndSalesDasboardData", parameterList.ToArray());
                var resultTotal = dataSetResultTotal.Tables[0]
                        .AsEnumerable()
                        .Select(x => new CasesSoldRevenueChartBM
                        {
                            Commodity = !string.IsNullOrEmpty(x.Field<string>("Commodity")) ? x.Field<string>("Commodity").Trim() : "",
                            Category = !string.IsNullOrEmpty(x.Field<string>("Category")) ? x.Field<string>("Category").Trim() : "",
                            Sales = x.Field<decimal?>("Sales").Value,
                            CasesSold = x.Field<decimal?>("CasesSold").Value,
                            AssignedSalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("AssignedSalesPersonCode")) ? x.Field<string>("AssignedSalesPersonCode").Trim() : "",
                            SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? x.Field<string>("SalesPersonCode").Trim() : "",
                            SalesPersonDescription = !string.IsNullOrEmpty(x.Field<string>("SalesPersonDescription")) ? x.Field<string>("SalesPersonDescription").Trim() : "",
                            Period = !string.IsNullOrEmpty(x.Field<string>("Period")) ? x.Field<string>("Period").Trim() : "",
                        })
        .ToList();

                resultTotal = resultTotal.Where(f => f.SalesPersonCode != "DUMP" && f.SalesPersonCode != "DONA").ToList();
                CasesSoldRevenueDataProvider casesSoldRevenuDataProvide = new CasesSoldRevenueDataProvider();

                var userAccessibleCategories = _adminUserManagementDateProvider.GetUserDetails(userId);


                totalCasesSold.All = MappingToGenericListCasesold(resultTotal, filters, userAccessibleCategories);
                totalRevenue.All = MappingToGenericListRevenue(resultTotal, filters, userAccessibleCategories);
                response.TotalRevenue = totalRevenue;
                response.TotalCasesSold = totalCasesSold;
                #endregion

                var responseWithDiffPercentageInRevenue = CaluclateDifference(response);
                return responseWithDiffPercentageInRevenue;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
        }

        private List<GenericDrillDownColumnChartBM> MappingToGenericListRevenueFromCompType(List<CasesSoldRevenueChartBM> list, string compType, GlobalFilter filters)
        {
            List<GenericDrillDownColumnChartBM> result = new List<GenericDrillDownColumnChartBM>();
            list = list.OrderByDescending(s => s.Sales).ToList();
            var data = new GenericDrillDownColumnChartBM
            {
                GroupName = compType,
                Label1 = filters.Periods.Current.Label,
                rValue1 = (list.Where(f => f.Commodity.ToLower() == compType && f.Period.ToLower() == "current").Sum(t => t.Sales)),
                Period1 = filters.Periods.Current.End,
                Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                SalesPerson1 = "",

                Label2 = filters.Periods.Historical.Label,
                rValue2 = (list.Where(f => f.Commodity.ToLower() == compType && f.Period.ToLower() == "historical").Sum(t => t.Sales)),
                Period2 = filters.Periods.Historical.End,
                Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                SalesPerson2 = "",

                Label3 = filters.Periods.Prior.Label,
                rValue3 = (list.Where(f => f.Commodity.ToLower() == compType && f.Period.ToLower() == "prior").Sum(t => t.Sales)),
                Period3 = filters.Periods.Prior.End,
                Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                SalesPerson3 = "",

                SubData = list.OrderByDescending(a => a.Sales)
                .Where(x => x.Category != Constants.CategorySchools
                && x.Category != Constants.CategoryHealthcare && x.Category != Constants.CategoryInstitute)
                     .GroupBy(x => x.Category)//.Take(5)
                     .Select(sec => new GenericDrillDownColumnChartBM
                     {
                         GroupName = sec.Key,
                         Label1 = filters.Periods.Current.Label,
                         rValue1 = (sec.Where(f => f.Commodity.ToLower() == compType && f.Period.ToLower() == "current").Sum(t => t.Sales)),
                         Period1 = filters.Periods.Current.End,
                         Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                         SalesPerson1 = "",



                         Label2 = filters.Periods.Historical.Label,
                         rValue2 = (sec.Where(f => f.Commodity.ToLower() == compType && f.Period.ToLower() == "historical").Sum(t => t.Sales)),
                         Period2 = filters.Periods.Historical.End,
                         Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                         SalesPerson2 = "",



                         Label3 = filters.Periods.Prior.Label,
                         rValue3 = (sec.Where(f => f.Commodity.ToLower() == compType && f.Period.ToLower() == "prior").Sum(t => t.Sales)),
                         Period3 = filters.Periods.Prior.End,
                         Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                         SalesPerson3 = "",


                         SubData = sec.Where(w => w.Category == sec.Key).OrderByDescending(a => a.Sales)
                         .GroupBy(x => x.AssignedSalesPersonCode).Take(5).Select(thir => new GenericDrillDownColumnChartBM
                         {
                             GroupName = GetSalesManName(!string.IsNullOrEmpty(thir.First().SalesPersonDescription) ? thir.First().SalesPersonDescription : thir.Key),
                             ActiveSalesPersonCode = thir.Key,
                             Label1 = filters.Periods.Current.Label,
                             rValue1 = (thir.Where(d => d.Commodity.ToLower() == compType && d.Period.ToLower() == "current").Sum(t => t.Sales)),
                             Period1 = filters.Periods.Current.End,
                             Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             SalesPerson1 = GetSalesManName(thir.First().SalesPersonDescription),



                             Label2 = filters.Periods.Historical.Label,
                             rValue2 = (thir.Where(d => d.Commodity.ToLower() == compType && d.Period.ToLower() == "historical").Sum(t => t.Sales)),
                             Period2 = filters.Periods.Historical.End,
                             Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                             SalesPerson2 = GetSalesManName(thir.First().SalesPersonDescription),


                             Label3 = filters.Periods.Prior.Label,
                             rValue3 = (thir.Where(d => d.Commodity.ToLower() == compType && d.Period.ToLower() == "prior").Sum(t => t.Sales)),
                             Period3 = filters.Periods.Prior.End,
                             Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             SalesPerson3 = GetSalesManName(thir.First().SalesPersonDescription),


                         }).ToList().OrderByDescending(s => s.rValue1 + s.rValue2).ToList()
                     }).ToList().OrderByDescending(s => s.rValue1 + s.rValue2).ToList()
            };
            result.Add(data);
            return result;
        }

        private List<GenericDrillDownColumnChartBM> MappingToGenericListRevenue(List<CasesSoldRevenueChartBM> list, GlobalFilter filters, UserDetails userAccessibleCategories)
        {
            List<GenericDrillDownColumnChartBM> result = new List<GenericDrillDownColumnChartBM>();
            list = list.OrderByDescending(s => s.Sales).ToList();


            var data = new GenericDrillDownColumnChartBM
            {
                GroupName = "",
                Label1 = filters.Periods.Current.Label,
                rValue1 = (list.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "current").Sum(t => t.Sales)),
                Period1 = filters.Periods.Current.End,
                Color1 = ChartColorBM.GrocerryCurrent,
                SalesPerson1 = "",

                Label2 = filters.Periods.Current.Label,
                rValue2 = (list.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "current").Sum(t => t.Sales)),
                Period2 = filters.Periods.Current.End,
                Color2 = ChartColorBM.ProduceCurrent,
                SalesPerson2 = "",

                Label3 = filters.Periods.Historical.Label,
                rValue3 = (list.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "historical").Sum(t => t.Sales)),
                Period3 = filters.Periods.Historical.End,
                Color3 = ChartColorBM.GrocerryPrevious,
                SalesPerson3 = "",

                Label4 = filters.Periods.Historical.Label,
                rValue4 = (list.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "historical").Sum(t => t.Sales)),
                Period4 = filters.Periods.Historical.End,
                Color4 = ChartColorBM.ProducePrevious,
                SalesPerson4 = "",

                Label5 = filters.Periods.Prior.Label,
                rValue5 = (list.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "prior").Sum(t => t.Sales)),
                Period5 = filters.Periods.Prior.End,
                Color5 = ChartColorBM.GrocerryCurrent,
                SalesPerson5 = "",

                Label6 = filters.Periods.Prior.Label,
                rValue6 = (list.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "prior").Sum(t => t.Sales)),
                Period6 = filters.Periods.Prior.End,
                Color6 = ChartColorBM.ProduceCurrent,
                SalesPerson6 = "",



                SubData = list.OrderByDescending(a => a.Sales).Where(x => !userAccessibleCategories.IsRestrictedCategoryAccess
               || userAccessibleCategories.Categories.Select(o => o.Name).Contains(x.Category))
                //.Where(x => x.Category != Constants.CategorySchools && x.Category != Constants.CategoryHealthcare && x.Category != Constants.CategoryInstitute)
                    .GroupBy(x => x.Category)//.Take(5)
                    .Select((sec, secIdx) => new GenericDrillDownColumnChartBM
                    {
                        GroupName = sec.Key.ToSalesCategoryDisplayName(),
                        Label1 = filters.Periods.Current.Label,
                        rValue1 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "current").Sum(t => t.Sales)),
                        Period1 = filters.Periods.Current.End,
                        Color1 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                        SalesPerson1 = "",

                        Label2 = filters.Periods.Current.Label,
                        rValue2 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "current").Sum(t => t.Sales)),
                        Period2 = filters.Periods.Current.End,
                        Color2 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                        SalesPerson2 = "",

                        Label3 = filters.Periods.Historical.Label,
                        rValue3 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "historical").Sum(t => t.Sales)),
                        Period3 = filters.Periods.Historical.End,
                        Color3 = BarColumnChartDistinctColors.Colors[secIdx].SecondaryGrocery,//ChartColorBM.GrocerryPrevious,
                        SalesPerson3 = "",

                        Label4 = filters.Periods.Historical.Label,
                        rValue4 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "historical").Sum(t => t.Sales)),
                        Period4 = filters.Periods.Historical.End,
                        Color4 = BarColumnChartDistinctColors.Colors[secIdx].SecondaryProduce,//ChartColorBM.ProducePrevious,
                        SalesPerson4 = "",

                        Label5 = filters.Periods.Prior.Label,
                        rValue5 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "prior").Sum(t => t.Sales)),
                        Period5 = filters.Periods.Prior.End,
                        Color5 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                        SalesPerson5 = "",

                        Label6 = filters.Periods.Prior.Label,
                        rValue6 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "prior").Sum(t => t.Sales)),
                        Period6 = filters.Periods.Prior.End,
                        Color6 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                        SalesPerson6 = "",

                        SubData = sec.Where(w => w.Category == sec.Key).OrderByDescending(a => a.Sales)
                        .GroupBy(x => x.AssignedSalesPersonCode).Where(p => p.Any(x => x.Period == "current"))
                        //.Take(5)
                        .Select((thir, thirIdx) => new GenericDrillDownColumnChartBM
                        {
                            GroupName = GetSalemanNameFromGroup(thir),
                            //GetActiveSalesManName(!string.IsNullOrEmpty(thir.First().SalesPersonDescription) ? thir.First().SalesPersonDescription : thir.Key),
                            ActiveSalesPersonCode = thir.Key,
                            Label1 = filters.Periods.Current.Label,
                            rValue1 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "current").Sum(t => t.Sales)),
                            Period1 = filters.Periods.Current.End,
                            Color1 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                            SalesPerson1 = GetSalemanNameFromGroup(thir),

                            Label2 = filters.Periods.Current.Label,
                            rValue2 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "current").Sum(t => t.Sales)),
                            Period2 = filters.Periods.Current.End,
                            Color2 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                            SalesPerson2 = GetSalesManName(thir.First().SalesPersonDescription),

                            Label3 = filters.Periods.Historical.Label,
                            rValue3 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "historical").Sum(t => t.Sales)),
                            Period3 = filters.Periods.Historical.End,
                            Color3 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryGrocery,//ChartColorBM.GrocerryPrevious,
                            SalesPerson3 = GetSalesManName(thir.First().SalesPersonDescription),

                            Label4 = filters.Periods.Historical.Label,
                            rValue4 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "historical").Sum(t => t.Sales)),
                            Period4 = filters.Periods.Historical.End,
                            Color4 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryProduce,//ChartColorBM.ProducePrevious,
                            SalesPerson4 = GetSalesManName(thir.First().SalesPersonDescription),

                            Label5 = filters.Periods.Prior.Label,
                            rValue5 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "prior").Sum(t => t.Sales)),
                            Period5 = filters.Periods.Prior.End,
                            Color5 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                            SalesPerson5 = GetSalesManName(thir.First().SalesPersonDescription),

                            Label6 = filters.Periods.Prior.Label,
                            rValue6 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "prior").Sum(t => t.Sales)),
                            Period6 = filters.Periods.Prior.End,
                            Color6 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                            SalesPerson6 = GetSalesManName(thir.First().SalesPersonDescription),

                        }).ToList().OrderByDescending(s => s.rValue1 + s.rValue2).ToList()
                    }).ToList().OrderByDescending(s => s.rValue1 + s.rValue2).ToList()
            };
            result.Add(data);
            return result;
        }

        private GenericDrillDownChartsChartsBO CaluclateDifference(GenericDrillDownChartsChartsBO data)
        {
            var currentMonthAll = data.TotalRevenue.All.ToList()[0].rValue1 + data.TotalRevenue.All.ToList()[0].rValue2;
            var previousMonthAll = data.TotalRevenue.All.ToList()[0].rValue5 + data.TotalRevenue.All.ToList()[0].rValue6;
            var previousYearAll = data.TotalRevenue.All.ToList()[0].rValue3 + data.TotalRevenue.All.ToList()[0].rValue4;

            var caseCurrentMonthAll = data.TotalCasesSold.All.ToList()[0].cValue1 + data.TotalCasesSold.All.ToList()[0].cValue2;
            var CasePreviousMonthAll = data.TotalCasesSold.All.ToList()[0].cValue5 + data.TotalCasesSold.All.ToList()[0].cValue6;
            var CasePreviousYearAll = data.TotalCasesSold.All.ToList()[0].cValue3 + data.TotalCasesSold.All.ToList()[0].cValue4;

            data.revenueTotalMonthlyDifference = DoubleToPercentageString(CalculateChange(previousMonthAll, currentMonthAll));
            data.revenueTotalYearlyDifference = DoubleToPercentageString(CalculateChange(previousYearAll, currentMonthAll));

            data.TotalMonthlyDifference = DoubleToPercentageString(CalculateChange(CasePreviousMonthAll, caseCurrentMonthAll));
            data.TotalYearlyDifference = DoubleToPercentageString(CalculateChange(CasePreviousYearAll, caseCurrentMonthAll));
            return data;
        }

        private List<GenericDrillDownColumnChartBM> MappingToGenericListCasesold(List<CasesSoldRevenueChartBM> list, GlobalFilter filters, UserDetails userAccessibleCategories)
        {
            List<GenericDrillDownColumnChartBM> result = new List<GenericDrillDownColumnChartBM>();

            var listCurrent = list.Where(x => x.Period.ToLower() == "current").OrderByDescending(s => s.CasesSold).ToList();
            var listPrior = list.Where(x => x.Period.ToLower() == "prior").OrderByDescending(s => s.CasesSold).ToList();
            var listHistorical = list.Where(x => x.Period.ToLower() == "historical").OrderByDescending(s => s.CasesSold).ToList();


            var data = new GenericDrillDownColumnChartBM
            {
                GroupName = "",
                Label1 = filters.Periods.Current.Label,
                cValue1 = (listCurrent.Where(f => f.Commodity.ToLower() == "grocery").Sum(t => t.CasesSold)),
                rValue1 = (listCurrent.Where(f => f.Commodity.ToLower() == "grocery").Sum(t => t.Sales)),
                Period1 = filters.Periods.Current.End,
                Color1 = ChartColorBM.GrocerryCurrent,
                SalesPerson1 = "",

                Label2 = filters.Periods.Current.Label,
                cValue2 = (listCurrent.Where(f => f.Commodity.ToLower() == "produce").Sum(t => t.CasesSold)),
                rValue2 = (listCurrent.Where(f => f.Commodity.ToLower() == "produce").Sum(t => t.Sales)),
                Period2 = filters.Periods.Current.End,
                Color2 = ChartColorBM.ProduceCurrent,
                SalesPerson2 = "",

                Label3 = filters.Periods.Historical.Label,
                cValue3 = listHistorical.Where(f => f.Commodity.ToLower() == "grocery").Sum(t => t.CasesSold),
                rValue3 = listHistorical.Where(f => f.Commodity.ToLower() == "grocery").Sum(t => t.Sales),
                Period3 = filters.Periods.Historical.End,
                Color3 = ChartColorBM.GrocerryPrevious,
                SalesPerson3 = "",

                Label4 = filters.Periods.Historical.Label,
                cValue4 = (listHistorical.Where(f => f.Commodity.ToLower() == "produce").Sum(t => t.CasesSold)),
                rValue4 = (listHistorical.Where(f => f.Commodity.ToLower() == "produce").Sum(t => t.Sales)),
                Period4 = filters.Periods.Historical.End,
                Color4 = ChartColorBM.ProducePrevious,
                SalesPerson4 = "",

                Label5 = filters.Periods.Prior.Label,
                cValue5 = (listPrior.Where(f => f.Commodity.ToLower() == "grocery").Sum(t => t.CasesSold)),
                rValue5 = (listPrior.Where(f => f.Commodity.ToLower() == "grocery").Sum(t => t.Sales)),
                Period5 = filters.Periods.Prior.End,
                Color5 = ChartColorBM.GrocerryCurrent,
                SalesPerson5 = "",

                Label6 = filters.Periods.Prior.Label,
                cValue6 = (listPrior.Where(f => f.Commodity.ToLower() == "produce").Sum(t => t.CasesSold)),
                rValue6 = (listPrior.Where(f => f.Commodity.ToLower() == "produce").Sum(t => t.Sales)),
                Period6 = filters.Periods.Prior.End,
                Color6 = ChartColorBM.ProduceCurrent,
                SalesPerson6 = "",

                SubData = list.OrderByDescending(a => a.CasesSold).Where(x => !userAccessibleCategories.IsRestrictedCategoryAccess
               || userAccessibleCategories.Categories.Select(o => o.Name).Contains(x.Category))

                     .GroupBy(x => x.Category)//.Take(5)
                     .Select((sec, secIdx) => new GenericDrillDownColumnChartBM
                     {
                         GroupName = sec.Key.ToSalesCategoryDisplayName(),
                         Label1 = filters.Periods.Current.Label,
                         cValue1 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "current").Sum(t => t.CasesSold)),
                         rValue1 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "current").Sum(t => t.Sales)),
                         Period1 = filters.Periods.Current.End,
                         Color1 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                         SalesPerson1 = "",

                         Label2 = filters.Periods.Current.Label,
                         cValue2 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "current").Sum(t => t.CasesSold)),
                         rValue2 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "current").Sum(t => t.Sales)),
                         Period2 = filters.Periods.Current.End,
                         Color2 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                         SalesPerson2 = "",

                         Label3 = filters.Periods.Historical.Label,
                         cValue3 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "historical").Sum(t => t.CasesSold)),
                         rValue3 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "historical").Sum(t => t.Sales)),
                         Period3 = filters.Periods.Historical.End,
                         Color3 = BarColumnChartDistinctColors.Colors[secIdx].SecondaryGrocery,//ChartColorBM.GrocerryPrevious,
                         SalesPerson3 = "",

                         Label4 = filters.Periods.Historical.Label,
                         cValue4 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "historical").Sum(t => t.CasesSold)),
                         rValue4 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "historical").Sum(t => t.Sales)),
                         Period4 = filters.Periods.Historical.End,
                         Color4 = BarColumnChartDistinctColors.Colors[secIdx].SecondaryProduce,//ChartColorBM.ProducePrevious,
                         SalesPerson4 = "",

                         Label5 = filters.Periods.Prior.Label,
                         cValue5 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "prior").Sum(t => t.CasesSold)),
                         rValue5 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "prior").Sum(t => t.Sales)),
                         Period5 = filters.Periods.Prior.End,
                         Color5 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                         SalesPerson5 = "",

                         Label6 = filters.Periods.Prior.Label,
                         cValue6 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "prior").Sum(t => t.CasesSold)),
                         rValue6 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "prior").Sum(t => t.Sales)),
                         Period6 = filters.Periods.Prior.End,
                         Color6 = BarColumnChartDistinctColors.Colors[secIdx].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                         SalesPerson6 = "",

                         SubData = sec.Where(w => w.Category == sec.Key).OrderByDescending(a => a.CasesSold)
                             .GroupBy(x => x.AssignedSalesPersonCode).Where(p => p.Any(x => x.Period == "current"))
                             //.Take(5)
                             .Select((thir, thirIdx) => new GenericDrillDownColumnChartBM
                             {
                                 GroupName = GetSalemanNameFromGroup(thir),
                                 //  GetActiveSalesManName(!string.IsNullOrEmpty(thir.First().SalesPersonDescription) ? thir.First().SalesPersonDescription : thir.Key),
                                 ActiveSalesPersonCode = thir.Key,
                                 Label1 = filters.Periods.Current.Label,

                                 cValue1 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "current").Sum(t => t.CasesSold)),
                                 rValue1 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "current").Sum(t => t.Sales)),
                                 Period1 = filters.Periods.Current.End,
                                 Color1 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,
                                 SalesPerson1 = GetSalemanNameFromGroup(thir),
                                 //(thir.First().SalesPersonDescription),

                                 Label2 = filters.Periods.Current.Label,
                                 cValue2 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "current").Sum(t => t.CasesSold)),
                                 rValue2 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "current").Sum(t => t.Sales)),
                                 Period2 = filters.Periods.Current.End,
                                 Color2 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,
                                 SalesPerson2 = (thir.First().SalesPersonDescription),

                                 Label3 = filters.Periods.Historical.Label,
                                 cValue3 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "historical").Sum(t => t.CasesSold)),
                                 rValue3 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "historical").Sum(t => t.Sales)),
                                 Period3 = filters.Periods.Historical.End,
                                 Color3 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryGrocery,
                                 SalesPerson3 = (thir.First().SalesPersonDescription),

                                 Label4 = filters.Periods.Historical.Label,
                                 cValue4 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "historical").Sum(t => t.CasesSold)),
                                 rValue4 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "historical").Sum(t => t.Sales)),

                                 Period4 = filters.Periods.Current.End,
                                 Color4 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryProduce,
                                 SalesPerson4 = (thir.First().SalesPersonDescription),

                                 Label5 = filters.Periods.Prior.Label,
                                 cValue5 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "prior").Sum(t => t.CasesSold)),
                                 rValue5 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "prior").Sum(t => t.Sales)),
                                 Period5 = filters.Periods.Prior.End,
                                 Color5 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,
                                 SalesPerson5 = (thir.First().SalesPersonDescription),

                                 Label6 = filters.Periods.Prior.Label,
                                 cValue6 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "prior").Sum(t => t.CasesSold)),
                                 rValue6 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "prior").Sum(t => t.Sales)),
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

        private List<GenericDrillDownColumnChartBM> MappingToGenericListCasesoldFromCompType(List<CasesSoldRevenueChartBM> list, string compType, GlobalFilter filters)
        {
            List<GenericDrillDownColumnChartBM> result = new List<GenericDrillDownColumnChartBM>();
            list = list.OrderByDescending(s => s.CasesSold).ToList();





            var data = new GenericDrillDownColumnChartBM
            {
                GroupName = compType,
                Label1 = filters.Periods.Current.Label,
                cValue1 = (list.Where(f => f.Commodity.ToLower() == compType && f.Period.ToLower() == "current").Sum(t => t.CasesSold)),
                Period1 = filters.Periods.Current.End,
                //Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                Color1 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[0].PrimaryGrocery : BarColumnChartDistinctColors.Colors[0].PrimaryProduce,
                SalesPerson1 = "",

                Label2 = filters.Periods.Historical.Label,
                cValue2 = (list.Where(f => f.Commodity.ToLower() == compType && f.Period.ToLower() == "historical").Sum(t => t.CasesSold)),
                Period2 = filters.Periods.Historical.End,
                //Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                Color2 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[0].SecondaryGrocery : BarColumnChartDistinctColors.Colors[0].SecondaryProduce,
                SalesPerson2 = "",

                Label3 = filters.Periods.Prior.Label,
                cValue3 = (list.Where(f => f.Commodity.ToLower() == compType && f.Period.ToLower() == "prior").Sum(t => t.CasesSold)),
                Period3 = filters.Periods.Prior.End,
                //Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                Color3 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[0].PrimaryGrocery : BarColumnChartDistinctColors.Colors[0].PrimaryProduce,

                SalesPerson3 = "",

                SubData = list.OrderByDescending(a => a.CasesSold).Where(x => x.Category != Constants.CategorySchools && x.Category != Constants.CategoryHealthcare && x.Category != Constants.CategoryInstitute)
                     .GroupBy(x => x.Category)//.Take(5)
                     .Select((sec, idx) => new GenericDrillDownColumnChartBM
                     {
                         GroupName = sec.Key,
                         Label1 = filters.Periods.Current.Label,
                         cValue1 = (sec.Where(f => f.Commodity.ToLower() == compType && f.Period.ToLower() == "current").Sum(t => t.CasesSold)),
                         Period1 = filters.Periods.Current.End,
                         //Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                         Color1 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[idx].PrimaryGrocery : BarColumnChartDistinctColors.Colors[idx].PrimaryProduce,
                         SalesPerson1 = "",



                         Label2 = filters.Periods.Historical.Label,
                         cValue2 = (sec.Where(f => f.Commodity.ToLower() == compType && f.Period.ToLower() == "historical").Sum(t => t.CasesSold)),
                         Period2 = filters.Periods.Historical.End,
                         //Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                         Color2 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[idx].SecondaryGrocery : BarColumnChartDistinctColors.Colors[idx].SecondaryProduce,
                         SalesPerson2 = "",



                         Label3 = filters.Periods.Prior.Label,
                         cValue3 = (sec.Where(f => f.Commodity.ToLower() == compType && f.Period.ToLower() == "prior").Sum(t => t.CasesSold)),
                         Period3 = filters.Periods.Prior.End,
                         //Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                         Color3 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[idx].PrimaryGrocery : BarColumnChartDistinctColors.Colors[idx].PrimaryProduce,
                         SalesPerson3 = "",


                         SubData = sec.Where(w => w.Category == sec.Key).OrderByDescending(a => a.CasesSold)
                         .GroupBy(x => x.AssignedSalesPersonCode).Take(5)
                         .Select((thir, thirIdx) => new GenericDrillDownColumnChartBM
                         {
                             GroupName = GetSalesManName(!string.IsNullOrEmpty(thir.First().SalesPersonDescription) ? thir.First().SalesPersonDescription : thir.Key),
                             ActiveSalesPersonCode = thir.Key,
                             Label1 = filters.Periods.Current.Label,
                             cValue1 = (thir.Where(d => d.Commodity.ToLower() == compType && d.Period.ToLower() == "current").Sum(t => t.CasesSold)),
                             Period1 = filters.Periods.Current.End,
                             //Color1 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             Color1 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery : BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,
                             SalesPerson1 = GetSalesManName(thir.First().SalesPersonDescription),

                             Label2 = filters.Periods.Historical.Label,
                             cValue2 = (thir.Where(d => d.Commodity.ToLower() == compType && d.Period.ToLower() == "historical").Sum(t => t.CasesSold)),
                             Period2 = filters.Periods.Historical.End,
                             //Color2 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryPrevious : ChartColorBM.ProducePrevious,
                             Color2 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[thirIdx].SecondaryGrocery : BarColumnChartDistinctColors.Colors[thirIdx].SecondaryProduce,
                             SalesPerson2 = GetSalesManName(thir.First().SalesPersonDescription),

                             cValue3 = (thir.Where(d => d.Commodity.ToLower() == compType && d.Period.ToLower() == "prior").Sum(t => t.CasesSold)),
                             Period3 = filters.Periods.Prior.End,
                             //Color3 = compType.ToLower() == "grocery" ? ChartColorBM.GrocerryCurrent : ChartColorBM.ProduceCurrent,
                             Color3 = compType.ToLower() == "grocery" ? BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery : BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,
                             SalesPerson3 = GetSalesManName(thir.First().SalesPersonDescription),


                         }).ToList().OrderByDescending(s => s.cValue1 + s.cValue2).ToList()
                     }).ToList().OrderByDescending(s => s.cValue1 + s.cValue2).ToList()
            };
            result.Add(data);
            return result;
        }

        private string GetSalemanNameFromGroup(IGrouping<string, CasesSoldRevenueChartBM> thir)
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

        public GenericDrillDownChartsChartsBO GetCustomerServiceDetails(GlobalFilter filters, string userId)
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
                parameterList.Add(new SqlParameter("@currentStart", filters.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filters.Periods.Current.End));
                parameterList.Add(new SqlParameter("@historicalStart", filters.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filters.Periods.Historical.End));
                parameterList.Add(new SqlParameter("@priorStart", filters.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filters.Periods.Prior.End));
                var dataSetResultTotal = base.ReadToDataSetViaProcedure("BI_CSSL_CustomerServiceDetailReport", parameterList.ToArray());
                var resultTotal = dataSetResultTotal.Tables[0]
                        .AsEnumerable()
                        .Select(x => new CasesSoldRevenueChartBM
                        {
                            Commodity = !string.IsNullOrEmpty(x.Field<string>("Commodity")) ? x.Field<string>("Commodity").Trim() : "",
                            Category = !string.IsNullOrEmpty(x.Field<string>("Category")) ? x.Field<string>("Category").Trim() : "",
                            Sales = x.Field<decimal?>("Sales").Value,
                            CasesSold = x.Field<decimal?>("CasesSold").Value,
                            AssignedSalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("AssignedSalesPersonCode")) ? x.Field<string>("AssignedSalesPersonCode").Trim() : "",
                            SalesPersonCode = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? x.Field<string>("SalesPersonCode").Trim() : "",
                            SalesPersonDescription = !string.IsNullOrEmpty(x.Field<string>("SalesPersonDescription")) ? x.Field<string>("SalesPersonDescription").Trim() : "",
                            Period = !string.IsNullOrEmpty(x.Field<string>("Period")) ? x.Field<string>("Period").Trim() : "",
                            AddUser = !string.IsNullOrEmpty(x.Field<string>("AddUser")) ? x.Field<string>("AddUser").Trim() : "",
                        })
                .ToList();

                resultTotal = resultTotal.Where(f => f.SalesPersonCode != "DUMP" && f.SalesPersonCode != "DONA").ToList();
                CasesSoldRevenueDataProvider casesSoldRevenuDataProvide = new CasesSoldRevenueDataProvider();

                //  var userAccessibleCategories = _adminUserManagementDateProvider.GetUserDetails(userId);


                totalCasesSold.All = MappingToCusomerServiceGenericListCasesold(resultTotal, filters, null);
                totalRevenue.All = MappingToCusomerServiceGenericListRevenue(resultTotal, filters, null);

                //response.TotalRevenue = totalRevenue;
                response.TotalCasesSold = totalCasesSold;
                #endregion

                //   var responseWithDiffPercentageInRevenue = CaluclateDifference(response);
                return response;
            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
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
        private List<GenericDrillDownColumnChartBM> MappingToCusomerServiceGenericListRevenue(List<CasesSoldRevenueChartBM> list, GlobalFilter filters, UserDetails userAccessibleCategories)
        {
            List<GenericDrillDownColumnChartBM> result = new List<GenericDrillDownColumnChartBM>();
            list = list.OrderByDescending(s => s.Sales).ToList();


            var data = new GenericDrillDownColumnChartBM
            {
                GroupName = "",
                Label1 = filters.Periods.Current.Label,
                rValue1 = (list.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "current").Sum(t => t.Sales)),
                Period1 = filters.Periods.Current.End,
                Color1 = ChartColorBM.GrocerryCurrent,
                SalesPerson1 = "",

                Label2 = filters.Periods.Current.Label,
                rValue2 = (list.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "current").Sum(t => t.Sales)),
                Period2 = filters.Periods.Current.End,
                Color2 = ChartColorBM.ProduceCurrent,
                SalesPerson2 = "",

                Label3 = filters.Periods.Historical.Label,
                rValue3 = (list.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "historical").Sum(t => t.Sales)),
                Period3 = filters.Periods.Historical.End,
                Color3 = ChartColorBM.GrocerryPrevious,
                SalesPerson3 = "",

                Label4 = filters.Periods.Historical.Label,
                rValue4 = (list.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "historical").Sum(t => t.Sales)),
                Period4 = filters.Periods.Historical.End,
                Color4 = ChartColorBM.ProducePrevious,
                SalesPerson4 = "",

                Label5 = filters.Periods.Prior.Label,
                rValue5 = (list.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "prior").Sum(t => t.Sales)),
                Period5 = filters.Periods.Prior.End,
                Color5 = ChartColorBM.GrocerryCurrent,
                SalesPerson5 = "",

                Label6 = filters.Periods.Prior.Label,
                rValue6 = (list.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "prior").Sum(t => t.Sales)),
                Period6 = filters.Periods.Prior.End,
                Color6 = ChartColorBM.ProduceCurrent,
                SalesPerson6 = "",



                SubData = list.OrderByDescending(a => a.Sales)
                    //.Take(10)
                    .GroupBy(x => x.AddUser).Take(10)
                    .Select((sec, secIdx) => new GenericDrillDownColumnChartBM
                    {
                        GroupName = sec.Key.ToSalesCategoryDisplayName(),
                        Label1 = filters.Periods.Current.Label,
                        rValue1 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "current").Sum(t => t.Sales)),
                        Period1 = filters.Periods.Current.End,
                        Color1 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                        SalesPerson1 = "",

                        Label2 = filters.Periods.Current.Label,
                        rValue2 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "current").Sum(t => t.Sales)),
                        Period2 = filters.Periods.Current.End,
                        Color2 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                        SalesPerson2 = "",

                        Label3 = filters.Periods.Historical.Label,
                        rValue3 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "historical").Sum(t => t.Sales)),
                        Period3 = filters.Periods.Historical.End,
                        Color3 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].SecondaryGrocery,//ChartColorBM.GrocerryPrevious,
                        SalesPerson3 = "",

                        Label4 = filters.Periods.Historical.Label,
                        rValue4 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "historical").Sum(t => t.Sales)),
                        Period4 = filters.Periods.Historical.End,
                        Color4 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].SecondaryProduce,//ChartColorBM.ProducePrevious,
                        SalesPerson4 = "",

                        Label5 = filters.Periods.Prior.Label,
                        rValue5 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "prior").Sum(t => t.Sales)),
                        Period5 = filters.Periods.Prior.End,
                        Color5 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                        SalesPerson5 = "",

                        Label6 = filters.Periods.Prior.Label,
                        rValue6 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "prior").Sum(t => t.Sales)),
                        Period6 = filters.Periods.Prior.End,
                        Color6 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                        SalesPerson6 = "",

                    }).ToList().OrderByDescending(s => s.rValue1 + s.rValue2).ToList()
            };
            result.Add(data);
            return result;
        }
        private List<GenericDrillDownColumnChartBM> MappingToCusomerServiceGenericListCasesold(List<CasesSoldRevenueChartBM> list, GlobalFilter filters, UserDetails userAccessibleCategories)
        {
            List<GenericDrillDownColumnChartBM> result = new List<GenericDrillDownColumnChartBM>();

            var listCurrent = list.Where(x => x.Period.ToLower() == "current").OrderByDescending(s => s.CasesSold).ToList();
            var listPrior = list.Where(x => x.Period.ToLower() == "prior").OrderByDescending(s => s.CasesSold).ToList();
            var listHistorical = list.Where(x => x.Period.ToLower() == "historical").OrderByDescending(s => s.CasesSold).ToList();


            var data = new GenericDrillDownColumnChartBM
            {
                GroupName = "",
                Label1 = filters.Periods.Current.Label,
                cValue1 = (listCurrent.Where(f => f.Commodity.ToLower() == "grocery").Sum(t => t.CasesSold)),
                rValue1 = (listCurrent.Where(f => f.Commodity.ToLower() == "grocery").Sum(t => t.Sales)),
                Period1 = filters.Periods.Current.End,
                Color1 = ChartColorBM.GrocerryCurrent,
                SalesPerson1 = "",

                Label2 = filters.Periods.Current.Label,
                cValue2 = (listCurrent.Where(f => f.Commodity.ToLower() == "produce").Sum(t => t.CasesSold)),
                rValue2 = (listCurrent.Where(f => f.Commodity.ToLower() == "produce").Sum(t => t.Sales)),
                Period2 = filters.Periods.Current.End,
                Color2 = ChartColorBM.ProduceCurrent,
                SalesPerson2 = "",

                Label3 = filters.Periods.Historical.Label,
                cValue3 = listHistorical.Where(f => f.Commodity.ToLower() == "grocery").Sum(t => t.CasesSold),
                rValue3 = listHistorical.Where(f => f.Commodity.ToLower() == "grocery").Sum(t => t.Sales),
                Period3 = filters.Periods.Historical.End,
                Color3 = ChartColorBM.GrocerryPrevious,
                SalesPerson3 = "",

                Label4 = filters.Periods.Historical.Label,
                cValue4 = (listHistorical.Where(f => f.Commodity.ToLower() == "produce").Sum(t => t.CasesSold)),
                rValue4 = (listHistorical.Where(f => f.Commodity.ToLower() == "produce").Sum(t => t.Sales)),
                Period4 = filters.Periods.Historical.End,
                Color4 = ChartColorBM.ProducePrevious,
                SalesPerson4 = "",

                Label5 = filters.Periods.Prior.Label,
                cValue5 = (listPrior.Where(f => f.Commodity.ToLower() == "grocery").Sum(t => t.CasesSold)),
                rValue5 = (listPrior.Where(f => f.Commodity.ToLower() == "grocery").Sum(t => t.Sales)),
                Period5 = filters.Periods.Prior.End,
                Color5 = ChartColorBM.GrocerryCurrent,
                SalesPerson5 = "",

                Label6 = filters.Periods.Prior.Label,
                cValue6 = (listPrior.Where(f => f.Commodity.ToLower() == "produce").Sum(t => t.CasesSold)),
                rValue6 = (listPrior.Where(f => f.Commodity.ToLower() == "produce").Sum(t => t.Sales)),
                Period6 = filters.Periods.Prior.End,
                Color6 = ChartColorBM.ProduceCurrent,
                SalesPerson6 = "",

                SubData = list.OrderByDescending(a => a.CasesSold)
                     .GroupBy(x => x.AddUser).Take(10)
                     .Select((sec, secIdx) => new GenericDrillDownColumnChartBM
                     {
                         GroupName = sec.Key,
                         Label1 = filters.Periods.Current.Label,
                         cValue1 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "current").Sum(t => t.CasesSold)),
                         rValue1 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "current").Sum(t => t.Sales)),
                         Period1 = filters.Periods.Current.End,
                         Color1 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                         SalesPerson1 = "",

                         Label2 = filters.Periods.Current.Label,
                         cValue2 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "current").Sum(t => t.CasesSold)),
                         rValue2 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "current").Sum(t => t.Sales)),
                         Period2 = filters.Periods.Current.End,
                         Color2 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                         SalesPerson2 = "",

                         Label3 = filters.Periods.Historical.Label,
                         cValue3 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "historical").Sum(t => t.CasesSold)),
                         rValue3 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "historical").Sum(t => t.Sales)),
                         Period3 = filters.Periods.Historical.End,
                         Color3 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].SecondaryGrocery,//ChartColorBM.GrocerryPrevious,
                         SalesPerson3 = "",

                         Label4 = filters.Periods.Historical.Label,
                         cValue4 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "historical").Sum(t => t.CasesSold)),
                         rValue4 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "historical").Sum(t => t.Sales)),
                         Period4 = filters.Periods.Historical.End,
                         Color4 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].SecondaryProduce,//ChartColorBM.ProducePrevious,
                         SalesPerson4 = "",

                         Label5 = filters.Periods.Prior.Label,
                         cValue5 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "prior").Sum(t => t.CasesSold)),
                         rValue5 = (sec.Where(f => f.Commodity.ToLower() == "grocery" && f.Period.ToLower() == "prior").Sum(t => t.Sales)),
                         Period5 = filters.Periods.Prior.End,
                         Color5 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].PrimaryGrocery,//ChartColorBM.GrocerryCurrent,
                         SalesPerson5 = "",

                         Label6 = filters.Periods.Prior.Label,
                         cValue6 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "prior").Sum(t => t.CasesSold)),
                         rValue6 = (sec.Where(f => f.Commodity.ToLower() == "produce" && f.Period.ToLower() == "prior").Sum(t => t.Sales)),
                         Period6 = filters.Periods.Prior.End,
                         Color6 = BarColumnChartDistinctColors.Colors[GetColorNumber(secIdx)].PrimaryProduce,//ChartColorBM.ProduceCurrent,
                         SalesPerson6 = "",


                     }).ToList().OrderByDescending(s => s.cValue1 + s.cValue2).ToList()

            };


            /*
              SubData = sec.Where(w => w.Category == sec.Key).OrderByDescending(a => a.CasesSold)
                             .GroupBy(x => x.AssignedSalesPersonCode).Where(p => p.Any(x => x.Period == "current"))
                             //.Take(5)
                             .Select((thir, thirIdx) => new GenericDrillDownColumnChartBM
                             {
                                 GroupName = GetSalemanNameFromGroup(thir),
                                 //  GetActiveSalesManName(!string.IsNullOrEmpty(thir.First().SalesPersonDescription) ? thir.First().SalesPersonDescription : thir.Key),
                                 ActiveSalesPersonCode = thir.Key,
                                 Label1 = filters.Periods.Current.Label,

                                 cValue1 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "current").Sum(t => t.CasesSold)),
                                 rValue1 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "current").Sum(t => t.Sales)),
                                 Period1 = filters.Periods.Current.End,
                                 Color1 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,
                                 SalesPerson1 = GetSalemanNameFromGroup(thir),
                                 //(thir.First().SalesPersonDescription),

                                 Label2 = filters.Periods.Current.Label,
                                 cValue2 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "current").Sum(t => t.CasesSold)),
                                 rValue2 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "current").Sum(t => t.Sales)),
                                 Period2 = filters.Periods.Current.End,
                                 Color2 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,
                                 SalesPerson2 = (thir.First().SalesPersonDescription),

                                 Label3 = filters.Periods.Historical.Label,
                                 cValue3 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "historical").Sum(t => t.CasesSold)),
                                 rValue3 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "historical").Sum(t => t.Sales)),
                                 Period3 = filters.Periods.Historical.End,
                                 Color3 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryGrocery,
                                 SalesPerson3 = (thir.First().SalesPersonDescription),

                                 Label4 = filters.Periods.Historical.Label,
                                 cValue4 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "historical").Sum(t => t.CasesSold)),
                                 rValue4 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "historical").Sum(t => t.Sales)),

                                 Period4 = filters.Periods.Current.End,
                                 Color4 = BarColumnChartDistinctColors.Colors[thirIdx].SecondaryProduce,
                                 SalesPerson4 = (thir.First().SalesPersonDescription),

                                 Label5 = filters.Periods.Prior.Label,
                                 cValue5 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "prior").Sum(t => t.CasesSold)),
                                 rValue5 = (thir.Where(d => d.Commodity.ToLower() == "grocery" && d.Period.ToLower() == "prior").Sum(t => t.Sales)),
                                 Period5 = filters.Periods.Prior.End,
                                 Color5 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryGrocery,
                                 SalesPerson5 = (thir.First().SalesPersonDescription),

                                 Label6 = filters.Periods.Prior.Label,
                                 cValue6 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "prior").Sum(t => t.CasesSold)),
                                 rValue6 = (thir.Where(d => d.Commodity.ToLower() == "produce" && d.Period.ToLower() == "prior").Sum(t => t.Sales)),
                                 Period6 = filters.Periods.Prior.End,
                                 Color6 = BarColumnChartDistinctColors.Colors[thirIdx].PrimaryProduce,
                                 SalesPerson6 = (thir.First().SalesPersonDescription),

                             }).ToList().OrderByDescending(s => s.cValue1 + s.cValue2).ToList()
             
             */



            data.Label1 = data.Label1.Replace("Jan Jan", "Jan");
            data.Label2 = data.Label2.Replace("Jan Jan", "Jan");
            data.Label3 = data.Label3.Replace("Jan Jan", "Jan");
            data.Label4 = data.Label4.Replace("Jan Jan", "Jan");
            data.Label5 = data.Label5.Replace("Jan Jan", "Jan");
            data.Label6 = data.Label6.Replace("Jan Jan", "Jan");
            result.Add(data);

            return result;
        }
        #endregion

        #region Common  Methods

        private double DoubleToPercentageString(decimal d)
        {
            return (double)(Math.Round(d, 2) * 100);
        }
        private decimal CalculateChange(decimal previous, decimal current)
        {
            return UtilityExtensions.CalculateChangeInDecimal(previous, current);
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

        private string GetActiveSalesManName(string nameCombination)
        {
            try
            {
                var names = nameCombination.Split(',');
                if (names.Length > 1)
                {
                    return names[1][1] + " " + names[0];
                }
            }
            catch (Exception ex) { }
            return nameCombination;
        }
        #endregion

        #region Get Map Data
        public CasesSoldSalesStateMap GetSalesCasesSoldMap(GlobalFilter filter, string userId)
        {
            CasesSoldSalesStateMap casesSoldSalesStateMap = new CasesSoldSalesStateMap();

            AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
            var userAccessibleCategories = _adminUserManagementDateProvider.GetUserDetails(userId);

            try
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();

                parameterList.Add(new SqlParameter("@currentStart", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@currentEnd", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@priorStart", filter.Periods.Prior.Start));
                parameterList.Add(new SqlParameter("@priorEnd", filter.Periods.Prior.End));
                parameterList.Add(new SqlParameter("@historicalStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@historicalEnd", filter.Periods.Historical.End));

                var dataSetResult = base.ReadToDataSetViaProcedure("BI_CSSL_GetMapData", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new CasesSoldAndSalesStateData
                            {
                                CasesSoldCurrent = x.Field<decimal?>("CasesSoldCurrent").Value,
                                CasesSoldPrior = x.Field<decimal?>("CasesSoldPrior").Value,
                                CasesSoldHistorical = x.Field<decimal?>("CasesSoldHistorical").Value,
                                SalesCurrent = x.Field<decimal?>("SalesCurrent").Value,
                                SalesPrior = x.Field<decimal?>("SalesPrior").Value,
                                SalesHistorical = x.Field<decimal?>("SalesHistorical").Value,
                                State = x.Field<string>("State"),
                                Category = x.Field<string>("Category"),

                            }).ToList();

                var casessoldCategories = result
                 .Where(x => !userAccessibleCategories.IsRestrictedCategoryAccess
                 || userAccessibleCategories.Categories
                 .Select(o => o.Name).Contains(x.Category)).ToList();

                casesSoldSalesStateMap.CasesSold = casessoldCategories.GroupBy(p => p.State).Select(x => new MapData
                {
                    id = "US-" + x.Key.Trim(),
                    value = x.Sum(o => o.CasesSoldCurrent),
                    customData = "<div class=\"state-map-tooltip\">" + filter.Periods.Current.Label + ": " + (Math.Round(x.Sum(o => o.CasesSoldCurrent))).ToString("#,##0")
                       + "</br> " + filter.Periods.Historical.Label + ": " + (Math.Round(x.Sum(o => o.CasesSoldHistorical))).ToString("#,##0") + " </br > " +
                     (filter.Periods.Prior.Label != string.Empty ? (filter.Periods.Prior.Label + ": " + (Math.Round(x.Sum(o => o.CasesSoldPrior))).ToString("#,##0")) : "") + "</div>"
                }).ToList();

                var salesCategories = result.Where(x => !userAccessibleCategories.IsRestrictedCategoryAccess
                    || userAccessibleCategories.Categories.Select(o => o.Name).Contains(x.Category)).ToList();


                casesSoldSalesStateMap.Sales = salesCategories.GroupBy(p => p.State).Select(x => new MapData

                {
                    id = "US-" + x.Key.Trim(),
                    value = x.Sum(o => o.SalesCurrent),
                    customData = "<div class=\"state-map-tooltip\">" + filter.Periods.Current.Label + ": $" + (Math.Round(x.Sum(o => o.SalesCurrent))).ToString("#,##0")
                 + " </br> " + filter.Periods.Historical.Label + ": $" + (Math.Round(x.Sum(o => o.SalesHistorical))).ToString("#,##0") + " </br > " +
               (filter.Periods.Prior.Label != string.Empty ? (filter.Periods.Prior.Label + ": $" + (Math.Round(x.Sum(o => o.SalesPrior))).ToString("#,##0")) : "") + "</div>"

                }).ToList();

            }
            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
            return casesSoldSalesStateMap;
        }
        #endregion

        #region Dashboard Sales/Case Sold Statistics
        public CasesSoldSalesStatistics GetDashboardStatistics(GlobalFilter filter)
        {
            CasesSoldSalesStatistics casesSoldSalesStatistics = new CasesSoldSalesStatistics();
            try
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startDate", filter.Periods.Current.Start));
                parameterList.Add(new SqlParameter("@endDate", filter.Periods.Current.End));
                parameterList.Add(new SqlParameter("@previousStart", filter.Periods.Historical.Start));
                parameterList.Add(new SqlParameter("@previousEnd", filter.Periods.Historical.End));

                var dataSetResult = base.ReadToDataSetViaProcedure("BI_CSSL_GetDashboardStatistics", parameterList.ToArray());
                var result = dataSetResult.Tables[0]
                            .AsEnumerable()
                            .Select(x => new DashboardStatisticsData
                            {
                                CasesSold = x.Field<decimal?>("CasesSold").Value,
                                Sales = x.Field<decimal?>("SalesAmount").Value,
                                Period = x.Field<string>("Period")

                            }).ToList();

                casesSoldSalesStatistics.CasesSold = new Statistics();
                casesSoldSalesStatistics.CasesSold.CurrentValue = result[0].CasesSold;
                casesSoldSalesStatistics.CasesSold.PriorValue = result[1].CasesSold;

                casesSoldSalesStatistics.Sales = new Statistics();
                casesSoldSalesStatistics.Sales.CurrentValue = result[0].Sales;
                casesSoldSalesStatistics.Sales.PriorValue = result[1].Sales;

            }

            catch (Exception e)
            {
                throw new Exception("An error occured while fetching data from database - " + e.Message);
            }
            return casesSoldSalesStatistics;
        }
        #endregion

        #region Cases Sold Report
        public List<List<string>> GetCasesSoldReportPrerequisites()
        {
            List<SqlParameter> parameterList = new List<SqlParameter>();
            var dataSetResult = base.ReadToDataSetViaProcedure("BI_CS_GetCasesSoldReportPrerequisites", parameterList.ToArray());
            var listComodities = dataSetResult.Tables[0]
                                                .AsEnumerable()
                                                .Select(x => x.Field<string>("Comodity").Trim())
                                                .ToList();
            listComodities.Insert(0, "All");

            var listCategories = dataSetResult.Tables[1]
                                                .AsEnumerable()
                                                .Select(x => x.Field<string>("Category").Trim())
                                                .ToList();
            listCategories.Insert(0, "All");
            var result = new List<List<string>> { listComodities, listCategories };
            return result;

        }


        public CasesSoldSalesReoprtMapperWithTopBottom GetCustomerCasesSoldReport(string currentStart, string currentEnd
                                                         , string previousStart, string previousEnd, string userId
                                                         , string comodity = "", string category = ""
                                                         , string minSalesAmt = ""
                                                         , string salesPerson = "")
        {

            AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
            var userAccessibleCategories = _adminUserManagementDateProvider.GetUserAccess(userId);

            CasesSoldSalesReoprtMapperWithTopBottom response = new CasesSoldSalesReoprtMapperWithTopBottom();

            DataTable dtCategories = new DataTable();
            dtCategories.Columns.Add("Code");
            foreach (var userCategory in userAccessibleCategories.Categories)
            {
                if (userCategory.IsAccess == true) { dtCategories.Rows.Add(userCategory.Name); }
            }

            using (var adoDataAccess = new DataAccessADO())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();

                var param = new SqlParameter();
                param.SqlDbType = SqlDbType.Structured;
                param.Value = dtCategories;
                param.ParameterName = "@categories";

                parameterList.Add(param);

                parameterList.Add(new SqlParameter("@currentStart", currentStart));
                parameterList.Add(new SqlParameter("@currentEnd", currentEnd));
                parameterList.Add(new SqlParameter("@priorStart", previousStart));
                parameterList.Add(new SqlParameter("@priorEnd", previousEnd));
                parameterList.Add(new SqlParameter("@comodity", comodity));
                parameterList.Add(new SqlParameter("@category", category));
                parameterList.Add(new SqlParameter("@minSalesAmt", minSalesAmt));
                parameterList.Add(new SqlParameter("@salesPerson", salesPerson));

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_CS_GetCustomerCasesSoldReport", parameterList.ToArray());

                var result = dataTableResult.Tables[0]
                                            .AsEnumerable()
                                            .Select(x => new CasesSoldSalesReport
                                            {
                                                Customer = x.Field<string>("CompanyName"),
                                                Current = (x.Field<decimal?>("CurrentCasesSold") ?? 0),
                                                Previous = (x.Field<decimal?>("PriorCasesSold") ?? 0),
                                                Difference = (x.Field<decimal?>("Difference") ?? 0),
                                                PercentageDifference = (x.Field<decimal?>("growth") ?? 0),
                                            }).ToList();

                response.ReportData = result.OrderByDescending(x => x.Difference).ToList();
                response.ChartData = new CasesSoldSalesTopBottomTwoBarChartData
                {
                    Top = result.OrderByDescending(d => d.PercentageDifference).Take(10).Select(x => new CasesSoldSalesTwoBarChartdata
                    {
                        Category = x.Customer,
                        Color1 = ChartColorBM.GrocerryCurrent,
                        Color2 = ChartColorBM.GrocerryPrevious,
                        Label = x.Customer,
                        Value1 = Math.Round(x.Current),
                        Value2 = Math.Round(x.Previous),
                        Tooltip = Math.Round(x.PercentageDifference)
                    }
                    ).ToList(),
                    Bottom = result.OrderBy(d => d.PercentageDifference).Take(10).Select(x => new CasesSoldSalesTwoBarChartdata
                    {
                        Category = x.Customer,
                        Color1 = ChartColorBM.GrocerryCurrent,
                        Color2 = ChartColorBM.GrocerryPrevious,
                        Label = x.Customer,
                        Value1 = Math.Round(x.Current),
                        Value2 = Math.Round(x.Previous),
                        Tooltip = Math.Round(x.PercentageDifference)
                    }
                    ).ToList()
                };

                return response;
            }
        }

        #endregion

        #region Sales Reports
        public SalesMapperReportWithTopBottom GetSalesReportOfSalesPerson(List<string> salesPerson, string startDate, string endDate, string userId)
        {
            SalesMapperReportWithTopBottom response = new SalesMapperReportWithTopBottom();
            var startDateObj = DateTime.Parse(startDate);
            var endDateObj = DateTime.Parse(endDate);
            DataTable dtSalesPerson = new DataTable();
            dtSalesPerson.Columns.Add("Code");
            foreach (string code in salesPerson)
            {
                dtSalesPerson.Rows.Add(code);
            }

            using (var adoDataAccess = new DataAccessADO())
            {

                AdminManagementDataProvider _adminUserManagementDateProvider = new AdminManagementDataProvider();
                var userAccessibleCategories = _adminUserManagementDateProvider.GetUserAccess(userId);

                List<SqlParameter> parameterList = new List<SqlParameter>();
                var param = new SqlParameter();
                param.SqlDbType = SqlDbType.Structured;
                param.Value = dtSalesPerson;
                param.ParameterName = "@salesman";
                parameterList.Add(param);

                DataTable dtCategories = new DataTable();
                dtCategories.Columns.Add("Code");
                foreach (var category in userAccessibleCategories.Categories)
                {
                    if (category.IsAccess == true) { dtCategories.Rows.Add(category.Name); }
                }
                var paramCategory = new SqlParameter();
                paramCategory.SqlDbType = SqlDbType.Structured;
                paramCategory.Value = dtCategories;
                paramCategory.ParameterName = "@categories";
                parameterList.Add(paramCategory);

                parameterList.Add(new SqlParameter("@startDate", startDateObj));
                parameterList.Add(new SqlParameter("@endDate", endDateObj));
                parameterList.Add(new SqlParameter("@priorStartDate", startDateObj.AddYears(-1)));
                parameterList.Add(new SqlParameter("@priorEndDate", endDateObj.AddYears(-1)));

                var dataTableResult = base.ReadToDataSetViaProcedure("BI_SL_GetSalesPersonReport", parameterList.ToArray());
                var result = dataTableResult.Tables[0].AsEnumerable()

                       .Select(x => new SalesMapper
                       {
                           SalesPerson = !string.IsNullOrEmpty(x.Field<string>("SalesPersonCode")) ? x.Field<string>("SalesPersonCode").Trim() : "",
                           SalesPersonDescription = !string.IsNullOrEmpty(x.Field<string>("SalesPersonDescription")) ? x.Field<string>("SalesPersonDescription").Trim() : "",
                           //        SalesPersonDescription = x.Field<string>("SalesPersonDescription"),
                           AssignedPersonCode = x.Field<string>("SalesPersonCode"),
                           NoOfCustomer = x.Field<int>("CustomerCount"),
                           SalesAmountCurrent = x.Field<decimal?>("CurrentSales").HasValue ? x.Field<decimal?>("CurrentSales").Value : 0,
                           SalesAmountPrior = x.Field<decimal?>("PriorSales").HasValue ? x.Field<decimal?>("PriorSales").Value : 0,
                           Difference = (decimal)(x.Field<decimal?>("Difference").HasValue ? x.Field<decimal?>("Difference").Value : 0),
                           Percentage = (decimal)(x.Field<decimal?>("Growth").HasValue ? x.Field<decimal?>("Growth").Value : 0),


                       })
                       .ToList();
                result = result.Where(f => f.SalesPerson != "DONA" && f.SalesPerson != "DUMP").ToList();


                //result = result.GroupBy(x => x.SalesPerson)
                //                .Select(x => new SalesMapper
                //                {
                //                    SalesPerson = x.Key,
                //                    SalesPersonDescription = string.Join(",", x.Select(t => t.SalesPersonDescription.Contains(",")
                //                                                  ? string.Format("{0} {1}"
                //                                                                      , t.SalesPersonDescription.Split(',').LastOrDefault().TrimEnd()
                //                                                                      , t.SalesPersonDescription.Substring(0, 1))
                //                                                  : t.SalesPersonDescription)),
                //                    AssignedPersonCode = string.Join(",", x.Select(t => t.AssignedPersonCode)),
                //                    NoOfCustomer = x.First().NoOfCustomer,
                //                    SalesAmountCurrent = x.First().SalesAmountCurrent,
                //                    SalesAmountPrior = x.First().SalesAmountPrior,
                //                    Percentage = (decimal)x.First().Percentage,
                //                    Difference = x.First().Difference,
                //                    Binno = x.First().Binno,
                //                    Picker = x.First().Picker
                //                })
                //                .ToList();
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


        public List<SalesMapper> GetCustomerWiseReportOfSalesPerson(GlobalFilterModel filter)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();

            using (var adoDataAccess = new DataAccessADO())
            {
                var startDateObj = DateTime.Parse(filter.StartDateCurrent);
                var endDateObj = DateTime.Parse(filter.EndDateCurrent);
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@salesman", filter.SalesPerson));
                parameterList.Add(new SqlParameter("@currentStart", filter.StartDateCurrent));
                parameterList.Add(new SqlParameter("@currentEnd", filter.EndDateCurrent));
                parameterList.Add(new SqlParameter("@priorStart", startDateObj.AddYears(-1)));
                parameterList.Add(new SqlParameter("@priorEnd", endDateObj.AddYears(-1)));
                parameterList.Add(new SqlParameter("@commodity", filter.Commodity));
                var dataTableResult = new DataSet();
                if (filter.Commodity == "All")
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
                                            SalesQty = x.Field<decimal?>("CurrentCasesSold").Value,
                                            CasesSoldCurrent = x.Field<decimal?>("CurrentCasesSold").Value,
                                            SalesAmountCurrent = x.Field<decimal?>("CurrentSales").Value,
                                            SalesAmountPrior = x.Field<decimal?>("PriorSales").Value,
                                            Percentage = x.Field<decimal?>("GrowthSales").Value,
                                            Difference = x.Field<decimal?>("DifferenceSales").Value,
                                            DifferenceCasesSold = x.Field<decimal?>("DifferenceCasesSold").Value,
                                            PercentageCasesSold = x.Field<decimal?>("GrowthCasesSold").Value,

                                        }).ToList();

                    if (filter.OrderBy == "casessold")
                    {
                        return result.OrderByDescending(x => x.CasesSoldCurrent).ToList();
                    }
                    else if (filter.OrderBy == "sales")
                    {
                        return result.OrderByDescending(x => x.SalesAmountCurrent).ToList();
                    }
                    return result;
                }
                else
                    return new List<SalesMapper>();
            }
        }

        #endregion

        #region Common Reports

        public List<SalesMapper> GetCustomerServiceReportBySalesman(string salesPerspon, int filterId, int period, string comodity, string orderBy = "")
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
                parameterList.Add(new SqlParameter("@currentStart", startDate));
                parameterList.Add(new SqlParameter("@currentEnd", endDate));
                parameterList.Add(new SqlParameter("@priorStart", prevStartDate));
                parameterList.Add(new SqlParameter("@priorEnd", prevEndDate));
                parameterList.Add(new SqlParameter("@commodity", comodity));
                var dataTableResult = new DataSet();
                if (comodity == "All")
                {
                    dataTableResult = base.ReadToDataSetViaProcedure("BI_CSSL_GetCasesSoldAndSalesByCustomerServiceSalesPerson", parameterList.ToArray());
                }
                else
                {
                    dataTableResult = base.ReadToDataSetViaProcedure("BI_CSSL_GetCasesSoldAndSalesByCustomerServiceSalesPersonAndCommodity", parameterList.ToArray());
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
                                            SalesQty = x.Field<decimal?>("CurrentCasesSold").Value,
                                            CasesSoldCurrent = x.Field<decimal?>("CurrentCasesSold").Value,
                                            SalesAmountCurrent = x.Field<decimal?>("CurrentSales").Value,
                                            SalesAmountPrior = x.Field<decimal?>("PriorSales").Value,
                                            Percentage = x.Field<decimal?>("GrowthSales").Value,
                                            Difference = x.Field<decimal?>("DifferenceSales").Value,
                                            DifferenceCasesSold = x.Field<decimal?>("DifferenceCasesSold").Value,
                                            PercentageCasesSold = x.Field<decimal?>("GrowthCasesSold").Value,

                                        }).ToList();

                    if (orderBy == "casessold")
                    {
                        return result.OrderByDescending(x => x.CasesSoldCurrent).ToList();
                    }
                    else if (orderBy == "sales")
                    {
                        return result.OrderByDescending(x => x.SalesAmountCurrent).ToList();
                    }
                    return result;
                }
                else
                    return new List<SalesMapper>();
            }
        }

        #endregion

   
    }
}