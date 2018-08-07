using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nogales.BusinessModel;
using Nogales.DataProvider;
using System.Threading.Tasks;
using Nogales.DataProvider.ENUM;

namespace Nogales.API.Controllers
{

    [RoutePrefix("Finance")]
    public class FinanceController : ApiController
    {
        FinanceDataProvider _financeDataProvider;

        [HttpGet]
        [Route("GetDashboardCollectionInfoData")]
        public async Task<IHttpActionResult> GetDashboardCollectionInfoData(int filterId)
        {
            try
            {
                var filterLists = GlobaldataProvider.GetFilterWithPeriods();
                var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();

                _financeDataProvider = new FinanceDataProvider();

                var data = _financeDataProvider.GetDashboardCollectionData(targetFilter);
                return Ok(data);
            }
            catch (Exception e)
            {

                return Ok(e);
            }
         

        }


        [HttpPost]
        [Route("GetCollectorsReport")]
        public async Task<IHttpActionResult> GetCollectorsReport(FinanceFilterBO filter)
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

            _financeDataProvider = new FinanceDataProvider();

            var result = _financeDataProvider.GetCollectorDetailsReport(startDate, endDate, filter.PTerms, filter.Collector);

            return Ok(result);

        }
    }
}
