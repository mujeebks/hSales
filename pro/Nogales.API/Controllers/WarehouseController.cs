using Nogales.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using Nogales.API.Utilities;
using Nogales.BusinessModel;
using Nogales.API.Utilities.Excel;
using System.Globalization;
using System.Threading.Tasks;
using Nogales.API.Models;
using Nogales.DataProvider.ENUM;

namespace Nogales.API.Controllers
{
    [Authorize]
    [RoutePrefix("Warehouse")]
    [AuthorizeModuleAccess(Modules = "Warehouse")]
    public class WarehouseController : ApiController
    {

        #region Private Variables
        WarehouseDataProvider _warehouseProvider = null;
        #endregion

        public WarehouseController()
        {
            this._warehouseProvider = new WarehouseDataProvider();
        }

        [HttpGet]
        [Route("GetSOShortReportNotificationEmails")]
        public IHttpActionResult GetEmailIds()
        {
            var result = ConfigurationManager.AppSettings["WarehouseShortReportNotifications"].ToString();
            return Ok(result.Split(',').ToList());
        }

        [HttpGet]
        [Route("GetShortReport")]
        public IHttpActionResult GenerateReport(string routeNumber, string buyerId, string shipDate)
        {
            try
            {
                DateTime tempdate = new DateTime();
                if (!(DateTime.TryParseExact(shipDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempdate)))
                {
                    throw new FormatException("Invalid date");
                }

                var warehouseDataProvider = new WarehouseDataProvider();
                var model = warehouseDataProvider.GetShortReport(shipDate, routeNumber, buyerId);
                return Ok(model);
            }
            catch (FormatException e)
            {
                return InternalServerError(e);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }


        //[HttpPost]
        //[AllowAnonymous]
        //[Route("NewCustomer")]
        //public IHttpActionResult Customer(Customer customer)
        //{
        //    try
        //    {
        //        if (customer != null)
        //        {
        //            EmailService service = new EmailService();
        //            var result = service.SendCustomerForm(customer);
        //            return Ok(result);
        //        }

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        /// <summary>
        /// Send excel report of warehouse shortage
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SendShortReportNotification")]
        public IHttpActionResult SendShortNotification(ShortReportNotificationBM model)
        {
            try
            {
                EmailService emailService = new EmailService();
                ExcelService excelService = new ExcelService();
                WarehouseDataProvider provider = new WarehouseDataProvider();
                var data = provider.GetShortReport(model.shipDate, model.routeNumber, model.buyerId, model.Ids);
                var report = excelService.GenerateWarehouseShortExcel(data, model.shipDate);
                var result = emailService.SendWarehouseShortageEmail(report, model.EmailTos, data, model.shipDate);

                WarehouseDataProvider obj = new WarehouseDataProvider();
                model.type = "Notify";
                obj.InsertSOShortNotificationDetails(model.Ids, model.EmailTos, model.shipDate, model.type);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(new Exception("Failed to send email - " + e.Message));
            }
        }

        [HttpPost]
        [Route("PickerProductivityReport")]
        public IHttpActionResult GeneratePickerProductivityReport(PickerProductivityReportFilterBM model)
        {
            var warehouseDataProvider = new WarehouseDataProvider();
            var result = warehouseDataProvider.GetPickerProductivityReport(model);
            return Ok(result);
        }
        [HttpPost]
        [Route("PickerProductivityReportDetails")]
        public IHttpActionResult GeneratePickerProductivityReportDetails(PickerProductivityReportFilterBM model)
        {
            var warehouseDataProvider = new WarehouseDataProvider();
            var result = warehouseDataProvider.GetPickerProductivityReportDetails(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("PickerProductivityChart")]
        public IHttpActionResult GeneratePickerProductivityChart(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var warehouseDataProvider = new WarehouseDataProvider();
            //var model = await warehouseDataProvider.GetPickerProductivityChart(targetFilter);
            var model = warehouseDataProvider.GetPickerProductivityDashboardData(targetFilter);
            return Ok(model);
        }

      

        [HttpGet]
        [Route("PickerProductivityChartTemp")]
        public async Task<IHttpActionResult> GeneratePickerProductivityChartTemp(string startDate, string endDate)
        {
            var warehouseDataProvider = new WarehouseDataProvider();
            var model = await warehouseDataProvider.GetPickerProductivityChartTemp(startDate, endDate);
            return Ok(model);
        }

        [HttpGet]
        [Route("PickerProductivityForecast")]
        public IHttpActionResult GenerateAvgPickerProductivityChart(int predictionDay, string startDate, string endDate)
        {
            var warehouseDataProvider = new WarehouseDataProvider();
            var model = warehouseDataProvider.GetPickerProductivityForecast(predictionDay, startDate, endDate);
            return Ok(model);
        }

        [HttpPost]
        [Route("SendShortReportIgnoredReason")]
        public IHttpActionResult SendShortIgnoredReason(ShortReportNotificationBM model)
        {
            WarehouseDataProvider provider = new WarehouseDataProvider();
            var data = provider.GetShortReport(model.shipDate, model.routeNumber, model.buyerId, model.Ids);

            WarehouseDataProvider obj = new WarehouseDataProvider();
            model.type = "Ignored";
            obj.InsertSOShortIgnoredReason(model.Ids, model.IgnoredReason, model.shipDate, model.type);

            return Ok();
        }

        [HttpGet]
        [Route("GetNotifiedIgnoredShortReports")]
        public IHttpActionResult GetNotifiedIgnoredShortReports(string routeNumber, string buyerId, string shipDate)
        {

            DateTime tempdate = new DateTime();
            if (!(DateTime.TryParseExact(shipDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempdate)))
            {
                throw new FormatException("Invalid date");
            }

            var warehouseDataProvider = new WarehouseDataProvider();
            var model = warehouseDataProvider.GetNotifiedIgnoredShortReports(shipDate, routeNumber, buyerId);
            return Ok(model);
        }

        [HttpGet]
        [Route("PickerForcastReport")]
        public IHttpActionResult PickerForcastReport(string startDate, string endDate)
        {
            var warehouseDataProvider = new WarehouseDataProvider();
            var model = warehouseDataProvider.GetPickerForcastReport(startDate, endDate);
            return Ok(model);
        }




        [HttpGet]
        [Route("PickerForcastQuantityPickedReport")]
        public IHttpActionResult PickerForcastQuantityPickedReport(string startDate, string endDate)
        {
            var warehouseDataProvider = new WarehouseDataProvider();
            var model = warehouseDataProvider.GetPickerForcastQuantityPickedReport(startDate, endDate);
            return Ok(model);
        }

        [HttpPost]
        [Route("GetSalesOrderWithNoBin")]
        public IHttpActionResult GetSalesOrderWithNoBin(ReportWithoutBinBo parameters)
        {
            var warehouseDataProvider = new WarehouseDataProvider();
            var model = warehouseDataProvider.GetSalesOrderWithoutBinNo(parameters.SalesPersons, parameters.Date, parameters.Sono);
            return Ok(model);
        }

        [HttpPost]
        [Route("GetDumpAndDonation")]
        public IHttpActionResult GetDumpAndDonation(DateTime startDate, DateTime endDate)
        {
            var warehouseDataProvider = new WarehouseDataProvider();
            var model = warehouseDataProvider.GetDumpAndDonation(startDate, endDate);
            return Ok(model);
        }

        [HttpPost]
        [Route("GetPickerProductivityDayReport")]
        public IHttpActionResult GetPickerProductivityDayReport(GlobalFilterModel filter)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filter.FilterId).FirstOrDefault();


            DateTime CurrentEndDate = targetFilter.Periods.Current.End;
            DateTime HistoricalEndDate = targetFilter.Periods.Historical.End;
            DateTime PriorEndDate = targetFilter.Periods.Prior.End;

            DateTime startDate, endDate;
            if (filter.Period == (int)PeriodEnum.Historical)
            {
                var filterListsHistorical = GlobaldataProvider.GetFilterWithPeriodsByDate(HistoricalEndDate);
                var targetFilterHistorical = filterListsHistorical.Where(d => d.Id == filter.FilterId).FirstOrDefault();
                startDate = targetFilterHistorical.Periods.Current.Start;
                endDate = targetFilterHistorical.Periods.Current.End;
            }
            else if (filter.Period == (int)PeriodEnum.Prior)
            {
                var filterListsPrior = GlobaldataProvider.GetFilterWithPeriodsByDate(PriorEndDate);
                var targetFilterPrior = filterListsPrior.Where(d => d.Id == filter.FilterId).FirstOrDefault();
                startDate = targetFilterPrior.Periods.Current.Start;
                endDate = targetFilterPrior.Periods.Current.End;
            }
            else
            {
                startDate = targetFilter.Periods.Current.Start;
                endDate = targetFilter.Periods.Current.End;
            }


            var warehouseDataProvider = new WarehouseDataProvider();
            var result = warehouseDataProvider.GetPickerProductivityDayReport(startDate, endDate, filter.CustomerNumber);
            return Ok(result);
        }

        [HttpPost]
        [Route("GetPickerProductivityDayDetailedReport")]
        public IHttpActionResult GetPickerProductivityDayDetailedReport(PickerProductivityReportFilterBM model)
        {
            var warehouseDataProvider = new WarehouseDataProvider();
            var result = warehouseDataProvider.GetPickerProductivityDayDetailReport(model);
            return Ok(result);
        }

    }
}
