using Nogales.BusinessModel;
using Nogales.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nogales.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            //PayrollDataProvider _payrollDataProvider = new PayrollDataProvider();

            //_payrollDataProvider.GetData();
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();

            var targetFilter = filterLists.Where(d => d.Id == 4).FirstOrDefault();

            TransportationDataProvider obj = new TransportationDataProvider();
            var cStartDate = new DateTime(2018, 06, 01);
            var cEndDate = cStartDate.AddDays(3);
            //var sabu = obj.GetDashboardDiverTripData(targetFilter);
            var sabu1 = obj.GetRouteConsolidatedReport(cStartDate, cEndDate);
            //var sabu2 = obj.GetDriverDayAndDetailedReport(cStartDate, cEndDate, "ANGWIL");

            int a = 0;
            //var pStartDate = cStartDate.AddMonths(-1);
            //var pEndDate = cEndDate.AddMonths(-1);

            //var hStartDate = cStartDate.AddYears(-1);
            //var hEndDate = cEndDate.AddYears(-1);

            //var filter = new GlobalFilter();
            //filter.Periods = new Period
            //{
            //    Current = new DateRange { Start = cStartDate, End = cEndDate },
            //    Prior = new DateRange { Start = pStartDate, End = pEndDate },
            //    Historical = new DateRange { Start = hStartDate, End = hEndDate },
            //};
            obj.GetDashboardDiverTripData(targetFilter);

        }
    }
}
