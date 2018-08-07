using Nogales.BusinessModel;
using Nogales.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nogales.DataProvider.Utilities;
using Nogales.API.Utilities;

namespace Nogales.API.Controllers
{
    [Authorize]
    [RoutePrefix("Journal")]
    [AuthorizeModuleAccess(Modules = "Expenses")]
    public class JournalController : ApiController
    {

        JournalDataProvider _journalDataProvider = null;

        /// <summary>
        /// Get AP Journal Report
        /// </summary>
        /// <param name="filterData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetJournalReport")]
        public IHttpActionResult GetAPJournalReport(JournalFilterBM filterData)
        {
            if (filterData != null)
            {
                _journalDataProvider = new JournalDataProvider();
                if (string.IsNullOrEmpty(filterData.StartSession))
                    filterData.StartSession = Constants.StartSession;
                if (string.IsNullOrEmpty(filterData.EndSession))
                    filterData.EndSession = Constants.EndSession;

                //OPEX
                if (filterData.GLAccounTtype == Constants.OpexGLAccounTtype)
                {
                    filterData.AccountNumberStart = Constants.OpexAccountStart;
                    filterData.AccountNumberEnd = Constants.OpexAccountEnd;
                }
                //COGS
                else if (filterData.GLAccounTtype == Constants.COGSGLAccounTtype)
                {
                    filterData.AccountNumberStart = Constants.CogsAccountStart;
                    filterData.AccountNumberEnd = Constants.CogsAccountEnd;
                }
                //All records
                else if (filterData.GLAccounTtype == Constants.ALLGLAccounTtype)
                {
                    filterData.AccountNumberStart = Constants.OpexAccountStart;
                    filterData.AccountNumberEnd = Constants.OpexAccountEnd;
                    filterData.SecondStartSession = Constants.CogsAccountStart;
                    filterData.SecondEndSession = Constants.CogsAccountEnd;
                }
                else
                {
                    return Ok(HttpStatusCode.BadRequest);
                }

                switch (filterData.JournalType)
                {
                    //Default use AP journal type
                    case Constants.APJournalType:
                        {
                            var result = _journalDataProvider.GetAPJournalReport(filterData);
                            if (result != null)
                            {
                                if (result.Count() > 0)
                                {
                                    return Ok(result);
                                }
                                else
                                {
                                    return Ok(HttpStatusCode.NoContent);
                                }
                            }
                            else
                            {
                                return Ok(HttpStatusCode.NotFound);
                            }
                        }
                    case Constants.ARJournalType:
                        {
                            var result = _journalDataProvider.GetARJournalReport(filterData);
                            if (result != null)
                            {
                                if (result.Count() > 0)
                                {
                                    return Ok(result);
                                }
                                else
                                {
                                    return Ok(HttpStatusCode.NoContent);
                                }
                            }
                            else
                            {
                                return Ok(HttpStatusCode.NotFound);
                            }
                        }
                    case Constants.ICJournalType:
                        {
                            var result = _journalDataProvider.GetICJournalReport(filterData);
                            if (result != null)
                            {
                                if (result.Count() > 0)
                                {
                                    return Ok(result);
                                }
                                else
                                {
                                    return Ok(HttpStatusCode.NoContent);
                                }
                            }
                            else
                            {
                                return Ok(HttpStatusCode.NotFound);
                            }
                        }
                    default:
                        return Ok(HttpStatusCode.BadRequest);
                }
            }
            return Ok(HttpStatusCode.BadRequest);
        }



        /// <summary>
        /// Get AP Opex Journal Chart
        /// </summary>
        /// <param name="filterData"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetOPEXCOGSJournalChart")]
        public IHttpActionResult GetAPOPEXJournalChart(int filterId)
        {

            var filterLists = GlobaldataProvider.GetFilterWithPeriods();
            var targetFilter = filterLists.Where(d => d.Id == filterId).FirstOrDefault();

            _journalDataProvider = new JournalDataProvider();
            var result = _journalDataProvider.GetAPJournalChart(targetFilter);
            if (result != null)
            {
                if (result.OPEXCOGSExpenseJournalChart.Count > 0 || result.PayrollJournalChart.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return Ok(HttpStatusCode.NoContent);
                }
            }
            else
            {
                return Ok(HttpStatusCode.NoContent);
            }

        }

        /// <summary>
        /// Get AP Opex Journal Chart
        /// </summary>
        /// <param name="filterData"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetOPEXCOGSJournalChartTemp")]
        public IHttpActionResult GetAPOPEXJournalChartTemp([FromUri]string startDate, string endDate, string category)
        {
            if (startDate != null && endDate != null)
            {
                //filterData.StartSession = Constants.StartSession;
                //filterData.EndSession = Constants.EndSession;
                //filterData.AccountNumberStart = Constants.OpexAccountStart;
                //filterData.AccountNumberEnd = Constants.OpexAccountEnd;

                _journalDataProvider = new JournalDataProvider();
                var result = _journalDataProvider.GetAPJournalChartTemp(startDate, endDate, category);
                if (result != null)
                {
                    if (result.OPEXCOGSExpenseJournalChart.Count > 0)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return Ok(HttpStatusCode.NoContent);
                    }
                }
                else
                {
                    return Ok(HttpStatusCode.NoContent);
                }
            }
            else
            {
                return Ok(HttpStatusCode.BadRequest);
            }
        }







        /// <summary>
        ///Get AP COGS JournalChart
        /// </summary>
        /// <param name="filterData"></param>
        /// <returns></returns>
        //[HttpGet]
        //[AllowAnonymous]
        //[Route("GetAPCOGSJournalChart")]
        //public IHttpActionResult GetAPCOGSJournalChart([FromUri]JournalFilterBM filterData)
        //{
        //    if (filterData != null)
        //    {
        //        filterData.StartSession = Constants.StartSession;
        //        filterData.EndSession = Constants.EndSession;
        //        filterData.AccountNumberStart = Constants.CogsAccountStart;
        //        filterData.AccountNumberEnd = Constants.CogsAccountEnd;

        //        _journalDataProvider = new JournalDataProvider();
        //        var result = _journalDataProvider.GetAPJournalChart(filterData);
        //        if (result != null)
        //        {
        //            if ((result.SubData.Top != null && result.SubData.Top.Any()) || (result.SubData.Bottom != null && result.SubData.Bottom.Any()))
        //            {
        //                return Ok(result);
        //            }
        //            else
        //            {
        //                return Ok(HttpStatusCode.NoContent);
        //            }
        //        }
        //        else
        //        {
        //            return Ok(HttpStatusCode.NoContent);
        //        }
        //    }
        //    else
        //    {
        //        return Ok(HttpStatusCode.BadRequest);
        //    }
        //}
    }
}
