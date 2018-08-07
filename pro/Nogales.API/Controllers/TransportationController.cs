using Nogales.API.Utilities;
using Nogales.BusinessModel;
using Nogales.DataProvider;
using Nogales.DataProvider.ENUM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Nogales.API.Controllers
{
    [Authorize]
    [RoutePrefix("Transportation")]
    [AuthorizeModuleAccess(Modules = "Transportation")]
    public class TransportationController : ApiController
    {
        TransportationDataProvider _transportationDataProvider;

        [HttpGet]
        [Route("GetDashboardDiverTripData")]
        public async Task<IHttpActionResult> GetDashboardDiverTripData(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();

            _transportationDataProvider = new TransportationDataProvider();
            var data = _transportationDataProvider.GetDashboardDiverTripData(targetFilter);

            return Ok(data);
        }

        [HttpPost]
        [Route("GetDashboardDriverTripDrillDownReport")]
        public async Task<IHttpActionResult> GetDashboardDriverTripDrillDownReport(TransportationFilterBO filter)
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

            _transportationDataProvider = new TransportationDataProvider();
            var result = _transportationDataProvider.GetDriverDayAndDetailedReport(startDate, endDate, filter.DriverCode);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetDriverTripConsolidatedReport")]
        public async Task<IHttpActionResult> GetDriverTripConsolidatedReport(string startDate, string endDate)
        {
            DateTime _startDate = new DateTime();
            DateTime _endDate = new DateTime();
            if (!(DateTime.TryParseExact(startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _startDate)))
            {
                throw new FormatException("Invalid start date");
            }
            if (!(DateTime.TryParseExact(endDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _endDate)))
            {
                throw new FormatException("Invalid end date");
            }

            if (_endDate < _startDate)
            {
                throw new FormatException("End Date earlier than Start Date");
            }

            _transportationDataProvider = new TransportationDataProvider();
            var data = _transportationDataProvider.GetDriverTripConsolidatedReport(_startDate, _endDate);

            return Ok(data);

        }


        [HttpGet]
        [Route("GetRouteConsolidatedReport")]
        public async Task<IHttpActionResult> GetRouteConsolidatedReport(string startDate, string endDate)
        {
            DateTime _startDate = new DateTime();
            DateTime _endDate = new DateTime();
            if (!(DateTime.TryParseExact(startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _startDate)))
            {
                throw new FormatException("Invalid start date");
            }
            if (!(DateTime.TryParseExact(endDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _endDate)))
            {
                throw new FormatException("Invalid end date");
            }

            if (_endDate < _startDate)
            {
                throw new FormatException("End Date earlier than Start Date");
            }

            _transportationDataProvider = new TransportationDataProvider();
            var data = _transportationDataProvider.GetRouteConsolidatedReport(_startDate, _endDate);

            return Ok(data);

        }


        [HttpGet]
        [Route("GetDriverDayAndDetailedReport")]
        public async Task<IHttpActionResult> GetDriverDayAndDetailedReport(string startDate, string endDate, string driverCode = "", string route = "")
        {
            DateTime _startDate = new DateTime();
            DateTime _endDate = new DateTime();
            if (!(DateTime.TryParseExact(startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _startDate)))
            {
                throw new FormatException("Invalid start date");
            }
            if (!(DateTime.TryParseExact(endDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _endDate)))
            {
                throw new FormatException("Invalid end date");
            }
            if (_endDate < _startDate)
            {
                throw new FormatException("End Date earlier than Start Date");
            }

            _transportationDataProvider = new TransportationDataProvider();
            var data = _transportationDataProvider.GetDriverDayAndDetailedReport(_startDate, _endDate, driverCode, route);

            return Ok(data);

        }


    }
}
