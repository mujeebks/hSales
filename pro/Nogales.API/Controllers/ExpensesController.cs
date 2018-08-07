using Nogales.BusinessModel;
using Nogales.DataProvider;
using Nogales.DataProvider.ENUM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Nogales.API.Utilities;

namespace Nogales.API.Controllers
{
    [Authorize]
    [RoutePrefix("Expenses")]
    [AuthorizeModuleAccess(Modules = "Expenses")]
    public class ExpensesController : ApiController
    {
        [Route("CategoryByMonth")]
        [HttpGet]
        public ExpensesCategoryBM CategoryByMonth(string filterDate, string category)
        {
            var expensesProvider = new ExpensesDataProvider();

            var filterDateTime = DateTime.Parse(filterDate);
            var dateFormattedString = filterDateTime.Date.ToString("yyyy-MM-ddT00:00:00");

            var current = filterDateTime.Month;
            var previous = filterDateTime.AddMonths(-1).Month;

            var currentMonthStart = filterDateTime.AddDays(1 - filterDateTime.Day);
            var currentMonthEnd = filterDateTime;

            var previousMonthStart = currentMonthStart.AddMonths(-1);
            var previousMonthEnd = currentMonthEnd.AddMonths(-1);

            var model = expensesProvider.GetExpensesByCategoryMonthlyDataTemp(filterDateTime
                                                                          , current.ToString(), previous.ToString()
                                                                          , currentMonthStart.ToString(), currentMonthEnd.ToString()
                                                                          , previousMonthStart.ToString(), previousMonthEnd.ToString());
            //model.SalesPerson = expensesProvider.GetExpensesBySalesPersonMonthlyData(filterDate);
            return model;
        }

        [Route("LoadExpenseChart")]
        [HttpGet]
        public IHttpActionResult LoadExpenseChart(int filterId)
        {
            var expensesProvider = new ExpensesDataProvider();

            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            //GetExpenseChardData

            var model = expensesProvider.GetExpenseChartData(targetFilter, User.Identity.GetUserId());
            //  var model = expensesProvider.GetExpensesByCategoryMonthlyData(targetFilter, User.Identity.GetUserId());

            return Ok(model);
        }

        [Route("CategoryByYear")]
        [HttpGet]
        public ExpensesCategoryBM CategoryByYear(string filterDate)
        {
            var expensesProvider = new ExpensesDataProvider();

            var filterDateTime = DateTime.Parse(filterDate);

            var currentYear = filterDateTime.Year;
            var previousYear = filterDateTime.AddYears(-1).Year;

            var currentStart = new DateTime(currentYear, 01, 01);
            var currentEnd = filterDateTime;

            var previousYearStart = currentStart.AddYears(-1);
            var previousYearEnd = currentEnd.AddYears(-1);

            var model = expensesProvider.GetExpensesCategoryByYTD_YTC(filterDateTime
                                                                          , currentYear.ToString(), previousYear.ToString()
                                                                          , currentStart.ToString(), currentEnd.ToString()
                                                                          , previousYearStart.ToString(), previousYearEnd.ToString());
            //model.SalesPerson = expensesProvider.GetExpensesBySalesPersonYearToDateData(filterDate);
            return model;
        }

        [Route("CategoryByYearToMonth")]
        [HttpGet]
        public ExpensesCategoryBM CategoryByYearToMonth(string filterDate, int month)
        {
            var expensesProvider = new ExpensesDataProvider();

            var date = DateTime.Parse(filterDate);
            var filterDateTime = DateTime.Parse(filterDate);

            var currentYear = filterDateTime.Year;
            var previousYear = filterDateTime.AddYears(-1).Year;

            var currentStart = new DateTime(currentYear, 01, 01);
            var currentEnd = currentStart.AddMonths(month + 1).AddDays(-1);

            var previousYearStart = currentStart.AddYears(-1);
            var previousYearEnd = currentEnd.AddYears(-1);

            var model = expensesProvider.GetExpensesCategoryByYTD_YTC(filterDateTime
                                                                          , currentYear.ToString(), previousYear.ToString()
                                                                          , currentStart.ToString(), currentEnd.ToString()
                                                                          , previousYearStart.ToString(), previousYearEnd.ToString());

            //model.SalesPerson = expensesProvider.GetExpensesBySalesPersonYearToCustomData(date.Year, month);
            return model;
        }

        //03/03/2017
        //[Route("TotalByMonth")]
        //[HttpGet]
        //public async Task<List<ExpensesCategoryChartBM>> TotalByMonth(string filterDate)
        //{
        //    var expensesProvider = new ExpensesDataProvider();
        //    var model = await expensesProvider.GetTotalExpenses(filterDate);
        //    return model;
        //}

        [Route("TopTenAdminExpenses")]
        [HttpGet]
        public List<GenericDrillDownBaseColumnChartBM> TopTenAdminExpenses(int filterId)
        {
            var expensesProvider = new ExpensesDataProvider();
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();
            var model = expensesProvider.GetTopTenExpenses(targetFilter.Periods.Current.Start.ToShortDateString(), targetFilter.Periods.Current.End.ToShortDateString());
            return model;
        }

        //[Route("TopTenAdminExpensesTemp")]
        //[HttpGet]
        //public List<ExpensesAmountChartBM> TopTenAdminExpensesTemp(string startDate, string endDate)
        //{
        //    var expensesProvider = new ExpensesDataProvider();
        //    var model = expensesProvider.GetTopTenExpenses(startDate, endDate);
        //    return model;
        //}

        /// <summary>
        /// Get OPEX COGS Expense Report
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="accountType"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOPEXCOGSExpenseReport")]
        public IHttpActionResult GetOPEXCOGSExpenseReport(GlobalFilterModel filter)
        {


            var journalDataProvider = new JournalDataProvider();
            var result = journalDataProvider.GetCOGSExpenseReport(filter);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return Ok(HttpStatusCode.NoContent);
            }

        }

        /// <summary>
        /// Get OPEX COGS Expense Report
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="accountType"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOPEXCOGSExpenseReportTemp")]
        public IHttpActionResult GetOPEXCOGSExpenseReportTemp(string startDate, string endDate, string accountType, string accountNumber)
        {
            var journalDataProvider = new JournalDataProvider();
            var result = journalDataProvider.GetCOGSExpenseReportTemp(startDate, endDate, accountType, accountNumber);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return Ok(HttpStatusCode.NoContent);
            }

        }


        /// <summary>
        /// Get Expenses Statitics
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [Route("GetExpensesStatistics")]
        [HttpGet]
        public IHttpActionResult GetExpensesStatistics(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();

            var expenseProvider = new ExpensesDataProvider();
            var model = expenseProvider.GetExpensesStatitics(targetFilter);
            return Ok(model);
        }

        /// <summary>
        /// Get Expenses Statitics
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [Route("GetExpensesStatisticsTemp")]
        [HttpGet]
        public IHttpActionResult GetExpensesStatisticsTemp()
        {
            var expenseProvider = new ExpensesDataProvider();
            var model = expenseProvider.GetExpensesStatiticsTemp(DateTime.Now);
            return Ok(model);
        }

        /// <summary>
        /// Get Comodity Expense Report
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="accountType"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetComodityExpenseReport")]
        public IHttpActionResult GetComodityExpenseReport(DateTime startDate, DateTime endDate, string comodity)
        {
            var expenseProvider = new ExpensesDataProvider();
            var result = expenseProvider.GetComodityExpenseReport(startDate, endDate, User.Identity.GetUserId(), !String.IsNullOrEmpty(comodity) ? comodity : "");
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return Ok(HttpStatusCode.NoContent);
            }

        }

        [Route("GetExpenseReportBySalesman")]
        [HttpPost]
        public IHttpActionResult GetExpenseReportBySalesman(GlobalFilterModel filter)
        {
            var expenseProvider = new ExpensesDataProvider();
            if (filter != null && !string.IsNullOrWhiteSpace(filter.SalesPerson))
            {

                var result = expenseProvider.GetExpenseReportBySalesman(filter.SalesPerson
                                                                , filter.FilterId, filter.Period, filter.Commodity,filter.IsCM01, filter.OrderBy);
                return Ok(result);

            }
            else
            {
                return Ok("no data found");
            }
        }


        #region Get CustomerService Expense Chart
        [HttpGet]
        [Route("GetCustomerServiceExpenseChart")]
        public async Task<IHttpActionResult> GetCustomerServiceExpenseChart(int filterId)
        {
            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var expenseProvider = new ExpensesDataProvider();
            // MOCKDATA
            var data = expenseProvider.GetCustomerServiceExpenseChart(targetFilter, User.Identity.GetUserId());
            watch.Stop();
            // new JavaScriptSerializer().Serialize()data;
            string refval = NogalesResources.CustomerServiceGraph;

            var elapsedMs = watch.ElapsedMilliseconds;
            return Ok(data);
        }

        [Route("GetExpenseInvoiceDetailsByCustomer")]
        [HttpPost]
        public IHttpActionResult GetExpenseInvoiceDetailsByCustomer(GlobalFilterModel filter)
        {
            var expenseProvider = new ExpensesDataProvider();
            if (filter != null && !string.IsNullOrWhiteSpace(filter.SalesPerson))
            {
                var result = expenseProvider.GetExpenseInvoiceDetailsByCustomer(filter.SalesPerson
                                                                , filter.FilterId, filter.Period, filter.CustomerNumber, filter.Commodity, filter.IsCM01);
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
