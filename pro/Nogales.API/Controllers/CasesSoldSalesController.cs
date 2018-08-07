using Nogales.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Nogales.BusinessModel;
using System.Web.Http;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Nogales.API.Utilities;
using System.Web.Script.Serialization;

namespace Nogales.API.Controllers
{
    [Authorize]
    [RoutePrefix("CasesSoldSales")]
    [AuthorizeModuleAccess(Modules = "Cases Sold,Sales")]
    public class CasesSoldSalesController : ApiController
    {

        CasesSoldSalesDataProvider _casesSoldSalesDataProvider;

        #region Sales Cases Sold Map
        [HttpGet]
        [Route("GetSalesCasesSoldMap")]
        public async Task<IHttpActionResult> GetSalesCasesSoldMap(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            _casesSoldSalesDataProvider = new CasesSoldSalesDataProvider();
            var data = _casesSoldSalesDataProvider.GetSalesCasesSoldMap(targetFilter, User.Identity.GetUserId());
            return Ok(data);
        }

        #endregion

        #region CasesSold And Sales Dasboard Data
        [HttpGet]
        [Route("GetCasesSoldAndSalesDasboardData")]
        public async Task<IHttpActionResult> GetCasesSoldAndSalesDasboardData(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var dashboardDataProvider = new CasesSoldSalesDataProvider();
            var data = dashboardDataProvider.GetCasesSoldAndSalesData(targetFilter, User.Identity.GetUserId());
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return Ok(data);
        }
        #endregion

        #region Get Customer Service Details
        [HttpGet]
        [Route("GetCustomerServiceDetails")]
        public async Task<IHttpActionResult> GetCustomerServiceDetails(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var dashboardDataProvider = new CasesSoldSalesDataProvider();
            // MOCKDATA
            var data = dashboardDataProvider.GetCustomerServiceDetails(targetFilter, User.Identity.GetUserId());
            watch.Stop();
            // new JavaScriptSerializer().Serialize()data;
            string refval = NogalesResources.CustomerServiceGraph;
        
            var elapsedMs = watch.ElapsedMilliseconds;
            return Ok(data);
        }
        #endregion

        #region Case Sold Reports
        [Route("GetCasesSoldReportPrerequisites")]
        [HttpGet]
        public IHttpActionResult GetCasesSoldReportPrerequisites()
        {
            _casesSoldSalesDataProvider = new CasesSoldSalesDataProvider();
            var model = _casesSoldSalesDataProvider.GetCasesSoldReportPrerequisites();
            return Ok(model);
        }


        [Route("GetCasesSoldReport")]
        [HttpPost]
        public IHttpActionResult GetCasesSoldReport(CasesSoldReportFilterBM reportFilterBM)
        {
            var _casesSoldSalesDataProvider = new CasesSoldSalesDataProvider();

            var currentStart = DateTime.Parse(reportFilterBM.currentStartDate);
            var currentEnd = DateTime.Parse(reportFilterBM.currentEndDate);
            var previousStart = DateTime.Parse(reportFilterBM.priorStartDate);
            var previousEnd = DateTime.Parse(reportFilterBM.priorEndDate);

            var model = _casesSoldSalesDataProvider.GetCustomerCasesSoldReport(currentStart.Date.ToString("yyyy/MM/dd"), currentEnd.Date.ToString("yyyy/MM/dd")
                                                                    , previousStart.Date.ToString("yyyy/MM/dd"), previousEnd.Date.ToString("yyyy/MM/dd")
                                                                    , User.Identity.GetUserId()
                                                                    , reportFilterBM.Comodity ?? string.Empty
                                                                    , reportFilterBM.Category ?? string.Empty
                                                                    , reportFilterBM.MinSalesAmt ?? string.Empty
                                                                    , !string.IsNullOrEmpty(reportFilterBM.SalesPersonCode) ? reportFilterBM.SalesPersonCode : ""
                                                                    );
            return Ok(model);
        }


        #endregion

        #region Sales Reports

        [Route("GetSalesReportofSalesPerson")]
        [HttpPost]
        public IHttpActionResult GetSalesReportOfSalesPerson(List<string> salesperson, string startDate, string endDate)
        {
            _casesSoldSalesDataProvider = new CasesSoldSalesDataProvider();
            var data = _casesSoldSalesDataProvider.GetSalesReportOfSalesPerson(salesperson, startDate, endDate, User.Identity.GetUserId());
            return Ok(data);
        }
        [Route("GetCustomerWiseReportOfSalesPerson")]
        [HttpPost]
        //[HttpGet]
        public IHttpActionResult GetCustomerWiseReportOfSalesPerson(GlobalFilterModel filter)
        {
            var _casesSoldSalesDataProvider = new CasesSoldSalesDataProvider();
            if (filter != null && !string.IsNullOrWhiteSpace(filter.SalesPerson))
            {
                
                    var result = _casesSoldSalesDataProvider.GetCustomerWiseReportOfSalesPerson(filter);
                    return Ok(result);
            }
            else
            {
                return Ok("no data found");
            }
        }
        #endregion

        #region  Customer Service Report
        [Route("GetCustomerServiceReportBySalesman")]
        [HttpPost]
        public IHttpActionResult GetCustomerServiceReportBySalesman(GlobalFilterModel filter)
        {
            var casesSoldProvider = new CasesSoldSalesDataProvider();
            var result = casesSoldProvider.GetCustomerServiceReportBySalesman(filter.SalesPerson
                                                                      , filter.FilterId, filter.Period, filter.Commodity, filter.OrderBy);
            return Ok(result);
        }
        #endregion
    }
}