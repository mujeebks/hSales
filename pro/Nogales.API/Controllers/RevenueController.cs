using Nogales.API.Utilities;
using Nogales.BusinessModel;
using Nogales.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Nogales.API.Controllers
{
    [Authorize]
    [RoutePrefix("Revenue")]

    public class RevenueController : ApiController
    {
        /// <summary>
        /// Category By Month
        /// </summary>
        /// <param name="filterDate"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        [Route("CategoryByMonth")]
        [HttpGet]
        public RevenueClusteredBarChartCategoryBM CategoryByMonth(string filterDate, string category)
        {
            var revenueProvider = new RevenueDataProvider();

            var filterDateTime = DateTime.Parse(filterDate);

            var currentMonth = filterDateTime.Month;
            var previousMonth = filterDateTime.AddMonths(-1).Month;

            var currentMonthStart = filterDateTime.AddDays(1 - filterDateTime.Day);
            var currentMonthEnd = filterDateTime;

            var previousMonthStart = currentMonthStart.AddMonths(-1);
            var previousMonthEnd = currentMonthEnd.AddMonths(-1);

            var previousYearStart = currentMonthStart.AddYears(-1);
            var previousYearEnd = currentMonthEnd.AddYears(-1);

            var previousMonthYearStart = previousMonthStart.AddYears(-1);
            var previousMonthYearEnd = previousMonthEnd.AddYears(-1);

            //var model = provider.GetRevenueCategoryMonthlyData(filterDate, category);
            var model = revenueProvider
                        .GetRevenueCategoryMonthlyData(filterDateTime
                                , currentMonth.ToString(), previousMonth.ToString()
                                , currentMonthStart.ToString(), currentMonthEnd.ToString()
                                , previousMonthStart.ToString(), previousMonthEnd.ToString()
                                , previousYearStart.ToString(), previousYearEnd.ToString()
                                , previousMonthYearStart.ToString(), previousMonthYearEnd.ToString());

            model.SalesPerson = revenueProvider.GetRevenueBySalesPersonMonth(filterDateTime
                                                                                , currentMonth.ToString(), previousMonth.ToString()
                                                                                , currentMonthStart.ToString("yyyy/MM/dd"), currentMonthEnd.ToString("yyyy/MM/dd")
                                                                                , previousMonthStart.ToString("yyyy/MM/dd"), previousMonthYearEnd.ToString("yyyy/MM/dd"));
            return model;
        }

        /// <summary>
        /// Category By Year
        /// </summary>
        /// <param name="filterDate"></param>
        /// <returns></returns>
        [Route("CategoryByYear")]
        [HttpGet]
        public RevenueClusteredBarChartCategoryBM CategoryByYear(string filterDate)
        {
            var revenueProvider = new RevenueDataProvider();

            var filterDateTime = DateTime.Parse(filterDate);

            var currentYear = filterDateTime.Year;
            var previousYear = filterDateTime.AddYears(-1).Year;

            var currentStart = new DateTime(currentYear, 01, 01);
            var currentEnd = filterDateTime;

            var previousYearStart = currentStart.AddYears(-1);
            var previousYearEnd = currentEnd.AddYears(-1);

            var model = revenueProvider.GetRevenueByCategoryYear(filterDateTime
                                                                        , currentYear.ToString()
                                                                        , previousYear.ToString()
                                                                        , currentStart.ToString()
                                                                        , currentEnd.ToString()
                                                                        , previousYearStart.ToString()
                                                                        , previousYearEnd.ToString());

            model.SalesPerson = revenueProvider.GetRevenueBySalesPersonYear(filterDateTime
                                                                                        ,currentYear.ToString(),previousYear.ToString()
                                                                                        ,currentStart.ToString(),currentEnd.ToString()
                                                                                        ,previousYearStart.ToString(),previousYearEnd.ToString());
            return model;
        }

        /// <summary>
        /// Category By Year To Month
        /// </summary>
        /// <param name="filterDate"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [Route("CategoryByYearToMonth")]
        [HttpGet]
        public RevenueClusteredBarChartCategoryBM CategoryByYearToMonth(string filterDate, int month)
        {
            var date = DateTime.Parse(filterDate);
            var revenueProvider = new RevenueDataProvider();

            var filterDateTime = DateTime.Parse(filterDate);

            var currentYear = filterDateTime.Year;
            var previousYear = filterDateTime.AddYears(-1).Year;

            var currentStart = new DateTime(currentYear, 01, 01);
            var currentEnd = currentStart.AddMonths(month + 1).AddDays(-1);

            var previousYearStart = currentStart.AddYears(-1);
            var previousYearEnd = currentEnd.AddYears(-1);

            //var model = casesSoldProvider.GetRevenueByCategoryByYearToCustomData(date.Year, month);
            var model = revenueProvider.GetRevenueByCategoryYear(filterDateTime
                                                                      , currentYear.ToString()
                                                                      , previousYear.ToString()
                                                                      , currentStart.ToString()
                                                                      , currentEnd.ToString()
                                                                      , previousYearStart.ToString()
                                                                      , previousYearEnd.ToString());

            model.SalesPerson = revenueProvider.GetRevenueBySalesPersonYear(filterDateTime
                                                                                        , currentYear.ToString(), previousYear.ToString()
                                                                                        , currentStart.ToString(), currentEnd.ToString()
                                                                                        , previousYearStart.ToString(), previousYearEnd.ToString());
            return model;
        }       

        /// <summary>
        /// Total By Month
        /// </summary>
        /// <param name="filterDate"></param>
        /// <returns></returns>
        //03/03/2017
        //[Route("TotalByMonth")]
        //[HttpGet]
        //public RevenueCategroryBM TotalByMonth(string filterDate, string category)
        //{
        //    var revenueDataProvider = new RevenueDataProvider();
        //    var model = revenueDataProvider.GetTotalRevenue(filterDate, category);
        //    return model;
        //}

        [Route("ReportFilterData")]
        [HttpGet]
        public RevenueReportFilterBM GetReportFilters()
        {
            var revenueDataProvider = new RevenueDataProvider();
            var model = revenueDataProvider.GetReportFilterData();
            return model;
        }

        [Route("GetRevenueReport")]
        [HttpPost]
        public List<RevenueReportBM> GetRevenueReport(RevenueReportFilterBM searchModel)
        {
            var provider = new RevenueDataProvider();
            var result = provider.GetRevenueReport(searchModel);
            return result;
        }

        /// <summary>
        /// Get Customer Cases Sold Report
        /// </summary>
        /// <param name="filterDate"></param>
        /// <returns></returns>
        [Route("GetRevenueCustomerReport")]
        [HttpPost]
        public IHttpActionResult GetRevenueCustomerReport(RevenueReportModel reportFilterBM)
        {
            var revenueProvider = new RevenueDataProvider();

            var currentStart = DateTime.Parse(reportFilterBM.currentStartDate);
            var currentEnd = DateTime.Parse(reportFilterBM.currentEndDate);
            var previousStart = DateTime.Parse(reportFilterBM.priorStartDate);
            var previousEnd = DateTime.Parse(reportFilterBM.priorEndDate);

            var model = revenueProvider.GetCustomerRevenueReport(currentStart.Date.ToString("yyyy/MM/dd"), currentEnd.Date.ToString("yyyy/MM/dd")
                                                                    , previousStart.Date.ToString("yyyy/MM/dd"), previousEnd.Date.ToString("yyyy/MM/dd")
                                                                    , reportFilterBM.Comodity ?? string.Empty
                                                                    , reportFilterBM.Category ?? string.Empty
                                                                    , reportFilterBM.MinSalesAmt ?? string.Empty
                                                                    , !string.IsNullOrEmpty(reportFilterBM.SalesPersonCode) ? reportFilterBM.SalesPersonCode : ""
                                                                    );
            return Ok(model);
        }

        [Route("GetRevenueItemSearch")]
        [HttpGet]
        public IHttpActionResult GetReportFilterItem(string inputSearch)
        {
            var revenueDataProvider = new RevenueDataProvider();
            var model = revenueDataProvider.GetItemsForRevenueFilter(inputSearch);
            return Ok(new { filteredItems = model });

        }

    }
}
