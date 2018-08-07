using Nogales.API.Utilities;
using Nogales.BusinessModel;
using Nogales.DataProvider;
using Nogales.DataProvider.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.AspNet.Identity;

namespace Nogales.API.Controllers
{
    //[Authorize]
    
    [RoutePrefix("Sales")]
    [AuthorizeModuleAccess(Modules = "Cases Sold,Sales")]
    public class SalesController : ApiController
    {
        #region Private Variables
        SalesDataProvider _salesProvider = null;
        #endregion

        public SalesController()
        {
            this._salesProvider = new SalesDataProvider();
        }

        [Route("SalesChart")]
        [HttpGet]
        public SalesChartBM SalesChartData(string startDate, string endDate, bool isSynced = false)
        {
            var model = this._salesProvider.GetSalesData(startDate, endDate, isSynced);
            return model;
        }

        [Route("SalesBySalesPerson")]
        [HttpGet]
        public SalesCustomerBySalesPersonChartBM SalesBySalesPersonChartData(string startDate, string endDate, bool isSynced = false)
        {
            var model = this._salesProvider.GetSalesBySalesPerson(startDate, endDate, isSynced);
            return model;
        }

        #region Get SalesPersons Targets
        /// <summary>
        /// Get the targets (Customer Count Targets and Sales Targets) of the sales persons
        /// </summary>
        /// <param name="isFirstTime">
        ///     It indicates whether the client requesting this API at the first time in the day.
        ///     If this request is first time then the API will sync the Sales Person Target table from Cube query
        /// </param>
        /// <returns></returns>
        [Route("GetTargets")]
        [HttpGet]
        public List<SalesTargetBM> GetSalesPersonsTargets(bool isFirstTime)
        {
            List<SalesTargetBM> result = this._salesProvider.GetSalesTargetData(isFirstTime);
            return result;
        }
        #endregion

        #region UpdateSalesPersonsTargets
        /// <summary>
        /// Updates the targets of sales persons into database.
        /// </summary>
        /// <param name="model"> List of all sales persons with their new targets</param>
        [Route("updateTargets")]
        [HttpPost]
        public void UpdateSalesPersonsTargets(List<SalesTargetBM> model)
        {
            try
            {
                this._salesProvider.UpdateSalesPersonTargets(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        [Route("GetAllSalesPerson")]
        [HttpGet]
        public SalesMapper GetAllSalesPerson()
        {
            var revenueDataProvider = new SalesDataProvider();
            var model = this._salesProvider.GetAllSalesPerson();
            return model;
        }

        [Route("GetSalesReportofSalesPerson")]
        [HttpPost]
        public IHttpActionResult GetSalesReportOfSalesPerson(List<string> salesperson, string startDate, string endDate)
        {
            var data = _salesProvider.GetSalesReportOfSalesPerson(salesperson, startDate, endDate);
            return Ok(data);
        }


        [Route("GetSalesAnalysisReportofSalesPerson")]
        [HttpPost]
        public IHttpActionResult GetSalesAnalysisReportofSalesPerson(List<string> salesperson, string startDate, string endDate)
        {
            var data = _salesProvider.GetSalesAnalysisReportofSalesPerson(salesperson, startDate, endDate, User.Identity.GetUserId());
            return Ok(data);
        }

        //Category 3rd level drill down report
        [Route("GetCustomerAndSalesTemp")]
        [HttpPost]
        //[HttpGet]
        public IHttpActionResult GetCustomerAndSalesTemp(FilterModel filter)
        {
            if (filter != null && !string.IsNullOrWhiteSpace(filter.SalesPerson))
            {
                if (filter.SalesPerson == "OSS")
                    filter.SalesPerson = string.Join(",", DataProvider.Utilities.Constants.OutSalesPersons);

                var result = _salesProvider.GetCustomerAndSalesTemp(filter.SalesPerson
                                                                , filter.StartDateCurrent
                                                                , filter.EndDateCurrent
                                                                , filter.StartDatePrevious
                                                                , filter.EndDatePrevious
                                                                , filter.Category
                                                                , filter.Commodity
                                                                );
                return Ok(result);
            }
            else
            {
                return Ok("no data found");
            }
        }

        //Category 3rd level drill down report
        [Route("GetCustomerAndSales")]
        [HttpPost]
        //[HttpGet]
        public IHttpActionResult GetCustomerAndSales(GlobalFilterModel filter)
        {
            if (filter != null && !string.IsNullOrWhiteSpace(filter.SalesPerson))
            {
                if (filter.SalesPerson == "OSS")
                    filter.SalesPerson = string.Join(",", DataProvider.Utilities.Constants.OutSalesPersons);

                var result = _salesProvider.GetCustomerAndSales(filter.SalesPerson
                                                                , filter.FilterId
                                                                , filter.Period
                                                                , filter.Category
                                                                , filter.Commodity
                                                                );
                return Ok(result);
            }
            else
            {
                return Ok("no data found");
            }
        }

        //Total 4th level drilldown
        [Route("GetCustomerAndSalesReportWithoutFilter")]
        [HttpPost]
        //[HttpGet]
        public IHttpActionResult GetCustomerAndSalesReportWithoutFilter(FilterModel filter)
        {
            if (filter != null && !string.IsNullOrWhiteSpace(filter.SalesPerson))
            {
                var result = _salesProvider.GetCustomerAndSalesReportWithoutFilter(filter.SalesPerson
                                                                , filter.StartDateCurrent
                                                                , filter.EndDateCurrent
                                                                , filter.StartDatePrevious
                                                                , filter.EndDatePrevious

                                                                );
                return Ok(result);
            }
            else
            {
                return Ok("no data found");
            }
        }


        [Route("GetCustomerAndSalesReportForAnalysis")]
        [HttpPost]
        public IHttpActionResult GetCustomerAndSalesReportForAnalysis(FilterModel filter)
        {
            if (filter != null)// && !string.IsNullOrWhiteSpace(filter.SalesPerson))
            {
                var result = _salesProvider.GetCustomerAndSalesReportForAnalysis(filter.SalesPerson
                                                                , filter.StartDateCurrent
                                                                , filter.EndDateCurrent
                                                                , filter.StartDatePrevious
                                                                , filter.EndDatePrevious

                                                                );
                return Ok(result);
            }
            else
            {
                return Ok("no data found");
            }
        }

        //Total 4th level drilldown
        [Route("GetCustomerAndSalesReport")]
        [HttpPost]
        //[HttpGet]
        public IHttpActionResult GetCustomerAndSalesReport(GlobalFilterModel filter)
        {
            if (filter != null && !string.IsNullOrWhiteSpace(filter.SalesPerson))
            {
                var result = _salesProvider.GetSalesReportBySalesman(filter.SalesPerson
                                                                , filter.FilterId, filter.Period,filter.Commodity);
                return Ok(result);
            }
            else
            {
                return Ok("no data found");
            }
        }

   
        [Route("GetSalesDashboardSalesAndCustomers")]
        [HttpGet]
        public IHttpActionResult GetSalesDashboardSalesAndCustomers(string startDate, string endDate, bool isSynced = false)
        {
            var model = this._salesProvider.GetSalesDashboardSalesAndCustomers(startDate, endDate, isSynced);
            return Ok(model);
        }

        [Route("GetSalesAnalysisReport")]
        [HttpPost]
        public IHttpActionResult GetSalesAnalysisReport(SalesAnalysisFilter filter)
        {
            var result = _salesProvider.GetSalesAnalysisReport(filter);
            return Ok(result);
        }

        [Route("GetSalesAnalysisReportBySalesPerson")]
        [HttpPost]
        public IHttpActionResult GetSalesAnalysisReportBySalesPerson(SalesAnalysisFilter filter)
        {
            var result = _salesProvider.GetSalesAnalysisReportSalesPerson(filter);
            return Ok(result);
        }

        [Route("GetSalesMarginReport")]
        [HttpPost]
        public IHttpActionResult GetSalesMarginReport(SalesAnalysisFilter filter)
        {
            var result = _salesProvider.GetSalesMarginReport(filter, User.Identity.GetUserId());
            return Ok(result);
        }

        [Route("GetSalesMarginDetailedReport")]
        [HttpPost]
        public IHttpActionResult GetSalesMarginDetailedReport(SalesAnalysisFilter filter)
        {
            var result = _salesProvider.GetSalesMarginDetailedReport(filter);
            return Ok(result);
        }

        #region Top/Bottom25 chart data

        [HttpGet]
        [Route("GetSalesAndGrowthBySalesPerson")]
        public async Task<IHttpActionResult> GetSalesAndGrowthBySalesPerson(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var casesSoldRevenue = new CasesSoldRevenueDataProvider();
            var data = _salesProvider.GetSalesAndGrowthBySalesPerson(targetFilter,User.Identity.GetUserId());
            return Ok(data);
        }

        [HttpGet]
        [Route("GetSalesAndGrowthByCustomer")]
        [AuthorizeModuleAccess(Modules = "Sales,Cases Sold")]
        public async Task<IHttpActionResult> GetSalesAndGrowthByCustomer(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var casesSoldRevenue = new CasesSoldRevenueDataProvider();
            var data = _salesProvider.GetSalesAndGrowthByCustomer(targetFilter, User.Identity.GetUserId());
            return Ok(data);
        }

        [Route("GetCasesSoldDetails")]
        [HttpPost]
        public IHttpActionResult GetCasesSoldDetails(GlobalFilterModel filter)
        {
            if (filter != null && !string.IsNullOrWhiteSpace(filter.SalesPerson))
            {
                var result = _salesProvider.GetInvoiceDetailsByCustomer(filter.SalesPerson
                                                                , filter.FilterId, filter.Period, filter.CustomerNumber, filter.Commodity,filter.OrderBy);
                return Ok(result);
            }
            else
            {
                return Ok("no data found");
            }
        }
        [Route("GetInvoiceDetailsByCustomerForCustomerService")]
        [HttpPost]
        public IHttpActionResult GetInvoiceDetailsByCustomerForCustomerService(GlobalFilterModel filter)
        {
            if (filter != null && !string.IsNullOrWhiteSpace(filter.SalesPerson))
            {
                var result = _salesProvider.GetInvoiceDetailsByCustomerForCustomerService(filter.SalesPerson
                                                                , filter.FilterId, filter.Period, filter.CustomerNumber, filter.Commodity, filter.OrderBy);
                return Ok(result);
            }
            else
            {
                return Ok("no data found");
            }
        }
        #endregion
    }


}
