using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nogales.DataProvider;
using Nogales.BusinessModel;
using System.Threading.Tasks;
using Nogales.API.Utilities;
using Microsoft.AspNet.Identity;
namespace Nogales.API.Controllers
{
    [Authorize]
    [RoutePrefix("Profitability")]
    [AuthorizeModuleAccess(Modules = "Profitablity")]
    public class ProfitabilityController : ApiController
    {
        [HttpGet]
        [Route("GetProfitability")]
        public async Task<IHttpActionResult> ProfitabilityChartData(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var ProfiatbilityDataProvider = new ProfiatbilityDataProvider();
            var model = await ProfiatbilityDataProvider.GetProfitability(targetFilter);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetProfitabilityTemp")]
        public async Task<IHttpActionResult> ProfitabilityChartDataTemp(string beginYear, string endYear)
        {
            var ProfiatbilityDataProvider = new ProfiatbilityDataProvider();
            var model = await ProfiatbilityDataProvider.GetProfitabilityTemp(beginYear, endYear);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetMargin")]
        public async Task<IHttpActionResult> GetMargin(int filterId, double filterCustomerData, double filterItemData)
        {

            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();

            var ProfiatbilityDataProvider = new ProfiatbilityDataProvider();
            var model = await ProfiatbilityDataProvider.GetMargin(targetFilter, filterCustomerData, filterItemData);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetMarginTemp")]
        public async Task<IHttpActionResult> GetMarginTemp(double filterCustomerData, double filterItemData)
        {
            var ProfiatbilityDataProvider = new ProfiatbilityDataProvider();
            var model = await ProfiatbilityDataProvider.GetMarginTemp(DateTime.Now, filterCustomerData, filterItemData);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetMarginByDifference")]
        public async Task<IHttpActionResult> GetMarginByDifference(int filterId, bool isCustomer, double filterData)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();

            var ProfiatbilityDataProvider = new ProfiatbilityDataProvider();
            var model = await ProfiatbilityDataProvider.GetMarginByDifference(targetFilter, isCustomer, filterData);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetMarginByDifferenceTemp")]
        public async Task<IHttpActionResult> GetMarginByDifferenceTemp(bool isCustomer, double filterData)
        {
            var ProfiatbilityDataProvider = new ProfiatbilityDataProvider();
            var model = await ProfiatbilityDataProvider.GetMarginByDifferenceTemp(DateTime.Now, isCustomer, filterData);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetItemMarginReport")]
        public async Task<IHttpActionResult> GetItemMarginReport(int filterId, int period, string itemCode)
        {
            var ProfiatbilityDataProvider = new ProfiatbilityDataProvider();
            var model = await ProfiatbilityDataProvider.GetItemMarginReport(filterId, period, itemCode);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetCustomerMarginReport")]
        public async Task<IHttpActionResult> GetCustomerMarginReport(int filterId, int period, string customerCode)
        {
            var ProfiatbilityDataProvider = new ProfiatbilityDataProvider();
            var model = await ProfiatbilityDataProvider.GetCustomerMarginReport(filterId, period, customerCode);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetItemMarginReportTemp")]
        public async Task<IHttpActionResult> GetItemMarginReportTemp(DateTime date, string itemCode)
        {
            var ProfiatbilityDataProvider = new ProfiatbilityDataProvider();
            var model = await ProfiatbilityDataProvider.GetItemMarginReportTemp(date, itemCode);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetCustomerMarginReportTemp")]
        public async Task<IHttpActionResult> GetCustomerMarginReportTemp(DateTime date, string customerCode)
        {
            var ProfiatbilityDataProvider = new ProfiatbilityDataProvider();
            var model = await ProfiatbilityDataProvider.GetCustomerMarginReportTemp(date, customerCode);
            return Ok(model);
        }

        [HttpGet]
        [Route("GetProfitByItem")]
        public async Task<IHttpActionResult> GetProfitByItem(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var profitDataProvider = new ProfiatbilityDataProvider();
            var data = profitDataProvider.GetProfitByItem(targetFilter, User.Identity.GetUserId());
            return Ok(data);
        }

        [HttpGet]
        [Route("GetProfitByCustomer")]
        public async Task<IHttpActionResult> GetProfitByCustomer(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var profitDataProvider = new ProfiatbilityDataProvider();
            var data = profitDataProvider.GetProfitByCustomer(targetFilter, User.Identity.GetUserId());
            return Ok(data);
        }

        [HttpGet]
        [Route("GetCustomerWiseProfitByItem")]
        public async Task<IHttpActionResult> GetCustomerWiseProfitByItem(int filterId, string itemCode)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var profitDataProvider = new ProfiatbilityDataProvider();
            var data = profitDataProvider.GetCustomerWiseProfitByItem(targetFilter, itemCode);
            return Ok(data);
        }

        [HttpPost]
        [Route("GetProfitByCustomerDetailAndCommodity")]
        public async Task<IHttpActionResult> GetProfitByCustomerDetailAndCommodity(ProfitByCustomerRequest request)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == request.FilterId).FirstOrDefault();
            var profitDataProvider = new ProfiatbilityDataProvider();
            var data = profitDataProvider.GetProfitByCustomerDetailAndCommodity(request);
            return Ok(data);
        }

        [HttpPost]
        [Route("GetProfitByCustomerDetailForCustomerService")]
        public async Task<IHttpActionResult> GetProfitByCustomerDetailForCustomerService(ProfitByCustomerRequest request)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == request.FilterId).FirstOrDefault();
            var profitDataProvider = new ProfiatbilityDataProvider();
            var data = profitDataProvider.GetProfitByCustomerDetailForCustomerService(targetFilter, request.Commodity, request.Salesman);
            return Ok(data);
        }


        [HttpPost]
        [Route("GetProfitablityDashboardBarchartData")]
        public async Task<IHttpActionResult> GetProfitablityDashboardBarChartData(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var profitDataProvider = new ProfiatbilityDataProvider();
            var data = profitDataProvider.GetProfitData(targetFilter, User.Identity.GetUserId());
            return Ok(data);
        }

        [HttpGet]
        [Route("GetCustomerServiceDetails")]
        public async Task<IHttpActionResult> GetCustomerServiceDetails(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var profitDataProvider = new ProfiatbilityDataProvider();
            var data = profitDataProvider.GetCustomerServiceDetails(targetFilter, User.Identity.GetUserId());
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return Ok(data);
        }
    }
}
