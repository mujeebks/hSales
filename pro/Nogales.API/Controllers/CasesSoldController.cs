using Nogales.API.Utilities;
using Nogales.BusinessModel;
using Nogales.DataProvider;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;

namespace Nogales.API.Controllers
{
    [Authorize]
    [RoutePrefix("CasesSold")]
    [AuthorizeModuleAccess(Modules = "Cases Sold,Sales")]
    public class CasesSoldController : ApiController
    {
        #region Get Category
        [Route("CategoryByMonth")]
        [HttpGet]
        public ClusteredBarChartCategoryBM CategoryByMonth(string filterDate, string category)
        {
            var casesSoldProvider = new CasesSoldProvider();
        
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


            var model = casesSoldProvider
                        .GetCasesSoldByCategoryMonth(filterDateTime
                                , currentMonth.ToString(), previousMonth.ToString()
                                , currentMonthStart.ToString("yyyy/MM/dd"), currentMonthEnd.ToString("yyyy/MM/dd")
                                , previousMonthStart.ToString("yyyy/MM/dd"), previousMonthEnd.ToString("yyyy/MM/dd")
                                , previousYearStart.ToString("yyyy/MM/dd"), previousYearEnd.ToString("yyyy/MM/dd")
                                , previousMonthYearStart.ToString("yyyy/MM/dd"), previousMonthYearEnd.ToString("yyyy/MM/dd"));

            model.SalesPerson = casesSoldProvider.GetCasesSoldBySalesPersonMonth(filterDateTime
                                                                                , currentMonth.ToString(), previousMonth.ToString()
                                                                                , currentMonthStart.ToString("yyyy/MM/dd"), currentMonthEnd.ToString("yyyy/MM/dd")
                                                                                , previousMonthStart.ToString("yyyy/MM/dd"), previousMonthEnd.ToString("yyyy/MM/dd"));
            return model;
        }

        [Route("CategoryByYear")]
        [HttpGet]
        public ClusteredBarChartCategoryBM CategoryByYear(string filterDate)
        {
            var casesSoldProvider = new CasesSoldProvider();

            var filterDateTime = DateTime.Parse(filterDate);

            var currentYear = filterDateTime.Year;
            var previousYear = filterDateTime.AddYears(-1).Year;

            var currentStart = new DateTime(currentYear, 01, 01);
            var currentEnd = filterDateTime;

            var previousYearStart = currentStart.AddYears(-1);
            var previousYearEnd = currentEnd.AddYears(-1);


            var result = GetCategoryByYTD_YTCM(filterDateTime
                                                 , currentYear.ToString()
                                                 , previousYear.ToString()
                                                 , currentStart.ToString("yyyy/MM/dd")
                                                 , currentEnd.ToString("yyyy/MM/dd")
                                                 , previousYearStart.ToString("yyyy/MM/dd")
                                                 , previousYearEnd.ToString("yyyy/MM/dd"));


            return result;
        }


        [Route("CategoryByYearToMonth")]
        [HttpGet]
        public ClusteredBarChartCategoryBM CategoryByYearToMonth(string filterDate, int month)
        {
            var date = DateTime.Parse(filterDate);
            var filterDateTime = DateTime.Parse(filterDate);

            var currentYear = filterDateTime.Year;
            var previousYear = filterDateTime.AddYears(-1).Year;

            var currentStart = new DateTime(currentYear, 01, 01);
            var currentEnd = currentStart.AddMonths(month + 1).AddDays(-1);

            var previousYearStart = currentStart.AddYears(-1);
            var previousYearEnd = currentEnd.AddYears(-1);


            var result = GetCategoryByYTD_YTCM(filterDateTime
                                                , currentYear.ToString()
                                                , previousYear.ToString()
                                                , currentStart.ToString("yyyy/MM/dd")
                                                , currentEnd.ToString("yyyy/MM/dd")
                                                , previousYearStart.ToString("yyyy/MM/dd")
                                                , previousYearEnd.ToString("yyyy/MM/dd"));


            return result;
        }

        private ClusteredBarChartCategoryBM GetCategoryByYTD_YTCM(DateTime filterDate
                                                                           , string current, string previous
                                                                           , string currentStart, string currentEnd
                                                                           , string previousYearStart, string previousYearEnd)
        {
            var casesSoldProvider = new CasesSoldProvider();

            var model = casesSoldProvider.GetCasesSoldByCategoryYear(filterDate
                                                                       , current.ToString()
                                                                       , previous.ToString()
                                                                       , currentStart.ToString()
                                                                       , currentEnd.ToString()
                                                                       , previousYearStart.ToString()
                                                                       , previousYearEnd.ToString());


            model.SalesPerson = casesSoldProvider.GetCasesSoldBySalesPersonYear(filterDate
                                                                                , current.ToString(), previous.ToString()
                                                                                , currentStart, currentEnd
                                                                                , previousYearStart, previousYearEnd);
            return model;
        }

        #endregion

        #region Reports
        [Route("GetCasesSoldReport")]
        [HttpPost]
        public IHttpActionResult GetCasesSoldReport(CasesSoldReportFilterBM reportFilterBM)
        {
            var casesSoldProvider = new CasesSoldProvider();

            var currentStart = DateTime.Parse(reportFilterBM.currentStartDate);
            var currentEnd = DateTime.Parse(reportFilterBM.currentEndDate);
            var previousStart = DateTime.Parse(reportFilterBM.priorStartDate);
            var previousEnd = DateTime.Parse(reportFilterBM.priorEndDate);

            var model = casesSoldProvider.GetCustomerCasesSoldReport(currentStart.Date.ToString("yyyy/MM/dd"), currentEnd.Date.ToString("yyyy/MM/dd")
                                                                    , previousStart.Date.ToString("yyyy/MM/dd"), previousEnd.Date.ToString("yyyy/MM/dd")
                                                                    , reportFilterBM.Comodity ?? string.Empty
                                                                    , reportFilterBM.Category ?? string.Empty
                                                                    , reportFilterBM.MinSalesAmt ?? string.Empty
                                                                    , !string.IsNullOrEmpty(reportFilterBM.SalesPersonCode) ? reportFilterBM.SalesPersonCode : ""
                                                                    );
            return Ok(model);
        }


        [Route("GetCasesSoldReportPrerequisites")]
        [HttpGet]
        public IHttpActionResult GetCasesSoldReportPrerequisites()
        {
            var casesSoldProvider = new CasesSoldProvider();
            var model = casesSoldProvider.GetCasesSoldReportPrerequisites();
            return Ok(model);
        }
        #endregion

        #region Casesold Top/Bottom 25 Report
        [HttpGet]
        [Route("GetCasesSoldAndGrowthBySalesPerson")]
        public async Task<IHttpActionResult> GetCasesSoldAndGrowthBySalesPerson(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var casesSoldRevenue = new CasesSoldRevenueDataProvider();
            var data = casesSoldRevenue.GetCasesSoldAndGrowthBySalesPerson(targetFilter, User.Identity.GetUserId());
            return Ok(data);
        }

        [HttpGet]
        [Route("GetCasesSoldAndGrowthByCustomer")]
        public async Task<IHttpActionResult> GetCasesSoldAndGrowthByCustomer(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var casesSoldRevenue = new CasesSoldRevenueDataProvider();
            var data = casesSoldRevenue.GetCasesSoldAndGrowthByCustomer(targetFilter, User.Identity.GetUserId());
            return Ok(data);
        }

        //Total 4th level drilldown
        [Route("GetCustomerAndCasessoldReport")]
        [HttpPost]
        public IHttpActionResult GetCustomerAndCasessoldReport(GlobalFilterModel filter)
        {
            var casesSoldProvider = new CasesSoldProvider();
            if (filter != null && !string.IsNullOrWhiteSpace(filter.SalesPerson))
            {
                if (filter.Category == "Customer")
                {
                    var result = casesSoldProvider.GetSalesReportByCustomer(filter.SalesPerson
                                                                , filter.FilterId, filter.Period, filter.Commodity,filter.OrderBy);
                    return Ok(result);
                }

                else
                {
                    var result = casesSoldProvider.GetSalesReportBySalesman(filter.SalesPerson
                                                                    , filter.FilterId, filter.Period, filter.Commodity,filter.OrderBy);
                    return Ok(result);
                }
            }
            else
            {
                return Ok("no data found");
            }
        }


      
        [HttpGet]
        [Route("GetCasesSoldAndGrowthBySalesPersonCustomerService")]
        public async Task<IHttpActionResult> GetCasesSoldAndGrowthBySalesPersonCustomerService(int filterId,bool isCasesSold)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var casesSoldRevenue = new CasesSoldRevenueDataProvider();
            var data = casesSoldRevenue.GetCasesSoldAndGrowthBySalesPersonCustomerService(targetFilter, User.Identity.GetUserId(),isCasesSold);
            return Ok(data);
        }
        #endregion









    }
}
