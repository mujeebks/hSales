using Nogales.BusinessModel;
using Nogales.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    [RoutePrefix("Dashboard")]
    public class DashboardController : ApiController
    {

        [Route("Totals")]
        public async Task<List<DashboardStatisticsBM>> GetDashboardTotals(int filterId, bool isSynced = false)
        {
            var commonProvider = new CommonDataProvider();
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var statistics = await commonProvider.GetDashboardStatics(targetFilter);

            //Previous month stastics
            //var previousStatics = commonProvider.GetPreviousMonthDashboardStatics(Convert.ToDateTime(dateFilter));

            //var warehouseDataProvider = new WarehouseDataProvider();
            // statistics.Add(await warehouseDataProvider.GetProductivityReport(dateFilter));
            return statistics;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetVersion")]
        public string GetVersion()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
            if (attributes.Any())
                return ((AssemblyFileVersionAttribute)attributes[0]).Version;
            else
                return string.Empty;
        }

        [HttpGet]
        [Route("GetCasesSoldRevenueByMonth")]
        public async Task<IHttpActionResult> GetCasesSoldRevenueByMonth(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var casesSoldRevenue = new CasesSoldRevenueDataProvider();
            var data = casesSoldRevenue.GetCasesSoldRevenueReport(targetFilter);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return Ok(data);
        }
      
        [HttpGet]
        [Route("GetCategories")]
        public async Task<IHttpActionResult> GetCategories(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var casesSoldRevenue = new CasesSoldRevenueDataProvider();
            var data = casesSoldRevenue.GetCategories(targetFilter);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetCasesSoldByLocation")]
        public async Task<IHttpActionResult> GetCasesSoldByLoc(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var casesSoldRevenue = new CasesSoldRevenueDataProvider();
            var data = casesSoldRevenue.GetCasesSoldByLocation(targetFilter);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetSalesManDashBoardReport")]
        public async Task<IHttpActionResult> GetSalesManDashBoardReport(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var casesSoldRevenue = new CasesSoldRevenueDataProvider();           
            var data = casesSoldRevenue.GetSalesManDashBoardReport(targetFilter);

            return Ok(data);
        }

        [HttpGet]
        [Route("GetSalesReportByMap")]
        public async Task<IHttpActionResult> GetSalesManMap(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var casesSoldRevenue = new CasesSoldRevenueDataProvider();
            var data = casesSoldRevenue.GetSalesManMap(targetFilter);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetCasesSoldByMap")]
        public async Task<IHttpActionResult> GetCasesSoldByMap(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var casesSoldRevenue = new CasesSoldRevenueDataProvider();
            var data = casesSoldRevenue.GetCasesSoldMap(targetFilter);
            return Ok(data);
        }
        
        [HttpGet]
        [Route("GetRevenueByMap")]
        public async Task<IHttpActionResult> GetRevenueByMap(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var casesSoldRevenue = new CasesSoldRevenueDataProvider();
            var data = casesSoldRevenue.GetRevenueMap(targetFilter);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetCasesSoldAndRevenueData")]
        public async Task<IHttpActionResult> GetCasesSoldAndRevenueData(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var casesSoldRevenue = new CasesSoldSalesDataProvider();
            var data = casesSoldRevenue.GetCasesSoldAndSalesData(targetFilter, User.Identity.GetUserId());
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return Ok(data);
        }

        #region DashBoard TopBox Values

        [HttpGet]
        [Route("GetOpexTopBoxValues")]
        public async Task<IHttpActionResult> GetOpexTopBoxValues(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var dashboardDataProvider = new DashBoardDataProvider();
            var data = dashboardDataProvider.GetOpexTopBoxValues(targetFilter);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return Ok(data);
        }


        [HttpGet]
        [Route("GetProfitTopBoxValues")]
        public async Task<IHttpActionResult> GetProfitTopBoxValues(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var dashboardDataProvider = new DashBoardDataProvider();
            var data = dashboardDataProvider.GetProfitTopBoxValues(targetFilter);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return Ok(data);
        }

        [HttpGet]
        [Route("GetNonCommoditySalesAndCasesSoldTopBoxValues")]
        public async Task<IHttpActionResult> GetNonCommoditySalesAndCasesSoldTopBoxValues(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var dashboardDataProvider = new DashBoardDataProvider();
            var data = dashboardDataProvider.GetNonCommoditySalesAndCasesSoldTopBoxValues(targetFilter);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return Ok(data);
        }

 

        [HttpGet]
        [Route("GetDashboardStatistics")]
        public async Task<IHttpActionResult> GetDashboardStatistics(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var casesSoldSalesDataProvider = new CasesSoldSalesDataProvider();
            var data = casesSoldSalesDataProvider.GetDashboardStatistics(targetFilter);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return Ok(data);
        }

        #endregion

        #region Unused Codes


        //[Route("Totals")]
        //public async Task<List<DashboardStatisticsBM>> GetDashboardTotals(string dateFilter, bool isSynced = false)
        //{
        //    var commonProvider = new CommonDataProvider();
        //    var statistics = await commonProvider.GetDashboardStatics(Convert.ToDateTime(dateFilter));

        //    //Previous month stastics
        //    //var previousStatics = commonProvider.GetPreviousMonthDashboardStatics(Convert.ToDateTime(dateFilter));

        //    //var warehouseDataProvider = new WarehouseDataProvider();
        //    // statistics.Add(await warehouseDataProvider.GetProductivityReport(dateFilter));
        //    return statistics;
        //}

        //[HttpGet]
        //[Route("GetCasesSoldRevenueByMonthTemp")]
        //public async Task<IHttpActionResult> GetCasesSoldRevenueByMonthTemp()
        //{

        //    var watch = System.Diagnostics.Stopwatch.StartNew();
        //    var casesSoldRevenue = new CasesSoldRevenueDataProvider();
        //    var data = casesSoldRevenue.GetCasesSoldRevenueReportTemp(DateTime.Now);
        //    watch.Stop();
        //    var elapsedMs = watch.ElapsedMilliseconds;
        //    return Ok(data);
        //}

        //[HttpGet]
        //[Route("GetCategoriesTemp")]
        //public async Task<IHttpActionResult> GetCategoriesTemp()
        //{
        //    var casesSoldRevenue = new CasesSoldRevenueDataProvider();
        //    var data = casesSoldRevenue.GetCategoriesTemp(DateTime.Now);
        //    return Ok(data);
        //}
        //[HttpGet]
        //[Route("GetCasesSoldByLocationTemp")]
        //public async Task<IHttpActionResult> GetCasesSoldByLocTemp()
        //{
        //    var casesSoldRevenue = new CasesSoldRevenueDataProvider();
        //    var data = casesSoldRevenue.GetCasesSoldByLocationTemp(DateTime.Now);
        //    return Ok(data);
        //}
        #endregion

    }
}
